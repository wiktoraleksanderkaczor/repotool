// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using RepoTool.Attributes.Parser;
using RepoTool.Enums.Json;
using RepoTool.Extensions;
using RepoTool.Flags.Parser;
using RepoTool.Models.Inference;
using RepoTool.Models.Inference.Contexts;
using RepoTool.Models.Inference.Contexts.Parser;
using RepoTool.Models.Parser.Interfaces;
using RepoTool.Models.Parser.Tools;
using RepoTool.Models.Parser.Tools.Builders;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Builders.Item;
using RepoTool.Models.Parser.Tools.Builders.Iterable;
using RepoTool.Models.Parser.Tools.Navigation;
using RepoTool.Persistence;
using RepoTool.Persistence.Entities;
using Action = RepoTool.Models.Inference.Contexts.Parser.Action;

namespace RepoTool.Helpers
{
    internal sealed class ParserHelper
    {
        private readonly RepositoryHelper _repositoryHelper;
        private readonly InferenceHelper _inferenceHelper;
        private readonly RepoToolDbContext _dbContext;

        public ParserHelper(
            RepositoryHelper repositoryHelper,
            InferenceHelper inferenceHelper,
            RepoToolDbContext dbContext)
        {
            _repositoryHelper = repositoryHelper;
            _inferenceHelper = inferenceHelper;
            _dbContext = dbContext;
        }

        public async Task<ParsedFileEntity> ParseFileAsync(string filePath, string? reference = null)
        {
            string? content = await _repositoryHelper.GetFileContentAsync(filePath, reference).ConfigureAwait(false) ?? throw new FileNotFoundException($"File '{filePath}' not found.");

            // Compute SHA256 hash of the string representation
            string contentHash = content.ToSha256Hash();

            List<LanguageEntity> languageEntities = await _dbContext.Languages.ToListAsync().ConfigureAwait(false);

            LanguageEntity? languageEntity = null;
            foreach ( LanguageEntity language in languageEntities )
            {
                // Check if the file matches the language patterns
                Matcher matcher = new();
                matcher.AddIncludePatterns(language.Patterns);
                if ( matcher.Match(filePath).HasMatches )
                {
                    languageEntity = language;
                    break;
                }
            }

            if ( languageEntity == null )
            {
                throw new InvalidOperationException($"Language configuration not found for file '{filePath}'. Ensure a language entry with matching patterns exists.");
            }

#if !DEBUG
            // Get existing parsed file if exists.
            ParsedFileEntity? parsedFile = await _dbContext.ParsedFiles
                .FirstOrDefaultAsync(p =>
                    p.FileHash == contentHash
                    && p.FilePath == filePath);

            if (parsedFile is not null)
                return parsedFile;
#endif

            ParserContext parserContext = new()
            {
                FilePath = filePath,
                ItemPath = new ItemPath
                {
                    Components = [
                    new ItemPathRootComponent
                    {
                        RecordType = typeof(ParsedData),
                        // TODO: Might need JsonDocument.Parse("{}") here
                        CurrentObject = null,
                    }
                ]
                },
                CodeWindow = new CodeWindow
                {
                    StashedWindows = [],
                    FileContent = content
                },
                ActionWindow = new ActionWindow { Actions = [] }
            };

            JsonDocument parsedDataDocument = await ParseRecursively(parserContext).ConfigureAwait(false);
            string parsedDataDocumentJson = parsedDataDocument.ToString()
                ?? throw new InvalidOperationException("Parsed data parsing failed");
            ParsedData parsedData = JsonHelper.DeserializeJsonToType<ParsedData>(parsedDataDocumentJson)
                ?? throw new InvalidOperationException("Parsed data deserialization failed");

            ParsedFileEntity parsedFileEntity = new()
            {
                FilePath = filePath,
                FileHash = contentHash,
                LanguageId = languageEntity.Id,
                Language = languageEntity,
                ParsedFile = parsedData
            };

            try
            {
                _dbContext.ParsedFiles.Add(parsedFileEntity);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch ( Exception ex )
            {
                throw new InvalidOperationException("Error saving parsed file entity.", ex);
            }

            return parsedFileEntity;
        }

        /// <summary>
        /// Recursively parses the given type and returns a JsonDocument.
        /// </summary>
        /// <param name="parserContext">The parser context.</param>
        /// <returns>A JsonDocument representing the parsed data.</returns>
        /// <exception cref="NotSupportedException">Thrown if the type is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the parsing fails.</exception>
        /// IMPORTANT: First call to this method must be with the root RECORD type (e.g., ParsedData).
        private async Task<JsonDocument> ParseRecursively(
            ParserContext parserContext)
        {
            // Get Handling Type (using the refined helper method)
            Type objectType = parserContext.ItemPath.LastObjectType;
            EnSchemaOutput handlingType = await JsonHelper.GetItemSchemaHandlingTypeAsync(objectType).ConfigureAwait(false);

            // Branch Based on Handling Type
            switch ( handlingType )
            {
                case EnSchemaOutput.ObjectType:
                    // --- Object Handling ---
                    switch ( objectType.IsCollectionType() )
                    {
                        case true:
                            // Handle collection types separately
                            return await ParseMapping(parserContext).ConfigureAwait(false);
                        case false:
                            // Handle standard object types
                            return await ParseRecord(parserContext).ConfigureAwait(false);
                    }
                case EnSchemaOutput.IterableType:
                    // Assume ParseIterable handles its own relative path additions correctly.
                    // No need to reset path here, as ParseIterable receives the current context.
                    if ( !objectType.IsCollectionType() )
                    {
                        // TODO: May still be okay, possible change to warning instead?
                        throw new NotSupportedException($"Type {objectType.Name} is not a recognized iterable type.");
                    }
                    return await ParseIterable(parserContext).ConfigureAwait(false);

                case EnSchemaOutput.ValueType:
                    // Directly infer the value
                    InferenceRequest<ParserContext> inferenceRequest = new() { Context = parserContext };
                    return await _inferenceHelper.GetInferenceAsync<JsonDocument, ParserContext>(inferenceRequest).ConfigureAwait(false)
                        ?? throw new InvalidOperationException($"Value inference failed for type {objectType.FullName} at path {parserContext.ItemPath}.");

                default:
                    throw new NotSupportedException($"Unsupported handling type encountered: {handlingType}");
            }
        }

        /// <summary>
        /// Parses the tool selector for the given tool type.
        /// IMPORTANT: Must add tool item path component before calling this method.
        /// </summary>
        /// <param name="parserContext"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        private async Task<Type> ParseToolSelector(
            ParserContext parserContext)
        {
            ItemPathPropertyComponent propertyComponent = parserContext.ItemPath.GetLastComponent<ItemPathPropertyComponent>();

            ItemPathToolComponent toolComponent = parserContext.ItemPath.Components
                .LastOrDefault() as ItemPathToolComponent
                ?? throw new InvalidOperationException("Last component is not a tool component.");

            // Check for inheritance from IToolSelector<T> on toolType
            bool isToolSelector = toolComponent.ToolType.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IToolSelector<>));

            // Use creation tool
            if ( isToolSelector )
            {
                // Infer the tool choice using the context
                InferenceRequest<ParserContext> inferenceRequest = new() { Context = parserContext };
                JsonDocument toolChoice = await ParseRecursively(parserContext).ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"Tool choice inference failed for type {toolComponent.ToolType.FullName} at path {parserContext.ItemPath}.");

                // IMPORTANT: Remove the property component to restore the path
                parserContext.ItemPath.RemoveLastComponent();

                // Parse tool choice as IToolSelector<T>
                Type? toolSelectorInterface = toolComponent.ToolType.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IToolSelector<>))
                    ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not implement IToolSelector<T>.");

                // Get the generic type argument (enum type) from the interface
                Type enumType = toolSelectorInterface.GetGenericArguments().FirstOrDefault()
                    ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a generic argument for IToolSelector<T>.");

                // Verify that the tool choice type has a ToolSelection property
                PropertyInfo? toolSelectionProperty = toolComponent.ToolType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.PropertyType == enumType)
                    ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a tool selection property.");

                // Continue with tool use by getting enumeration value from the tool choice
                object toolChoiceEnum = toolChoice.GetPropertyValue(toolSelectionProperty.Name, toolSelectionProperty.PropertyType)
                    ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a valid tool selection value.");

                // Check for ToolChoice on the tool choice enum
                ToolChoiceAttribute? toolChoiceEnumAttribute = toolChoiceEnum
                    .GetType()
                    .GetField(
                        toolChoiceEnum.ToString()
                            ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a valid tool selection value."))
                    ?.GetCustomAttributes(typeof(ToolChoiceAttribute), false)
                    .FirstOrDefault() as ToolChoiceAttribute;

                // Check for ItemChoice on the tool choice enum
                ItemChoiceAttribute? itemChoiceEnumAttribute = toolChoiceEnum
                    .GetType()
                    .GetField(
                        toolChoiceEnum.ToString()
                            ?? throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a valid tool selection value."))
                    ?.GetCustomAttributes(typeof(ItemChoiceAttribute), false)
                    .FirstOrDefault() as ItemChoiceAttribute;

                switch (toolChoiceEnumAttribute, itemChoiceEnumAttribute)
                {
                    case (null, null):
                        throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} does not have a valid tool selection value.");
                    case (null, not null):
                        // ItemChoice attribute found
                        return itemChoiceEnumAttribute.ItemChoice;
                    case (not null, null):
                        // ToolChoice attribute found; add new tool selector component and log action
                        parserContext.ItemPath.AddComponent(
                            new ItemPathToolComponent
                            {
                                ToolType = toolChoiceEnumAttribute.ToolChoice,
                                CurrentObject = null,
                            });
                        parserContext.ActionWindow.AddAction(new Action
                        {
                            IsSuccess = true,
                            Message = $"Selected {toolChoiceEnumAttribute.ToolChoice} tool for {propertyComponent.PropertyInfo.PropertyType.Name} item.",
                            ItemPath = parserContext.ItemPath.DeepClone()
                        });
                        return await ParseToolSelector(parserContext).ConfigureAwait(false);
                    default:
                        // Both attributes found, invalid
                        throw new InvalidOperationException($"Tool choice type {toolComponent.ToolType.Name} has both ToolChoice and ItemChoice attributes, which is not allowed.");
                }
            }

            throw new InvalidOperationException($"Tool type {toolComponent.ToolType.Name} does not implement IToolSelector<T>.");
        }

        private async Task<JsonDocument> ParseRecord(
            ParserContext parserContext)
        {
            Type objectType = parserContext.ItemPath.LastObjectType;

            // Check if type has ToolChoice attribute... for abstract items

            if ( objectType
                .GetCustomAttributes(typeof(ToolChoiceAttribute), false)
                .FirstOrDefault() is ToolChoiceAttribute toolChoiceAttribute )
            {
                parserContext.ItemPath.AddComponent(
                    new ItemPathToolComponent
                    {
                        ToolType = toolChoiceAttribute.ToolChoice,
                        CurrentObject = null,
                    });
                objectType = await ParseToolSelector(parserContext).ConfigureAwait(false);
            }

            if ( objectType.IsAbstract || objectType.IsInterface )
            {
                throw new InvalidOperationException($"Cannot parse abstract or interface type {objectType.FullName}.");
            }

            // --- Process objects with defined properties using one-shot inference + sequential ignored field processing ---

            // Identify Fields
            PropertyInfo[] properties = objectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // If no initial data is provided, create an empty JSON object
            // IMPORTANT: Must always add path before recursive parse call
            if ( parserContext.ItemPath.Components.LastOrDefault()?.CurrentObject is null )
            {
                parserContext.ItemPath.UpdateCurrentObject(JsonDocument.Parse("{}"));
            }

            // Process Properties Sequentially
            // IMPORTANT: Calling with more than one field leads to messes, can always batch/parallelize
            foreach ( PropertyInfo property in properties )
            {
                // Add component to item path for the recursive call
                parserContext.ItemPath.AddComponent(
                    new ItemPathPropertyComponent
                    {
                        PropertyName = property.Name,
                        PropertyInfo = property,
                        CurrentObject = null,
                        JsonSpecialModifier = property.GetJsonSpecialModifier()
                    });

                // Recursively parse the ignored field
                using JsonDocument parsedFieldData = await ParseRecursively(parserContext).ConfigureAwait(false);

                // Add the parsed field data to the parent object
                JsonDocument? parentObject = parserContext.ItemPath.ParentComponent?.CurrentObject;
                JsonDocument? newParentObject = parentObject?.UpdateJsonDocument(property.Name, parsedFieldData);

                // IMPORTANT: Remove the property component to restore the path
                parserContext.ItemPath.RemoveLastComponent();

                // Merge the result back into 'parent' object
                parserContext.ItemPath.UpdateCurrentObject(newParentObject);
            }

            if ( parserContext.ItemPath.GetLastComponent() is not ItemPathToolComponent )
            {
                parserContext.ItemPath.AddComponent(
                    new ItemPathToolComponent
                    {
                        ToolType = typeof(SummarizeAction),
                        CurrentObject = null,
                    });

                // Summarize the parsed data
                InferenceRequest<SummarizationContext> summarizeInferenceRequest = new()
                {
                    Context = new SummarizationContext
                    {
                        ItemPath = parserContext.ItemPath,
                        Content = parserContext.ItemPath.ParentComponent?.CurrentObjectJson
                            ?? throw new InvalidOperationException($"Current object is null at path {parserContext.ItemPath}.")
                    }
                };
                SummarizeAction summarizeAction = await _inferenceHelper.GetInferenceAsync<SummarizeAction, SummarizationContext>(summarizeInferenceRequest).ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"Summarization failed for type {objectType.FullName} at path {parserContext.ItemPath}.");

                // IMPORTANT: Remove the property component to restore the path
                parserContext.ItemPath.RemoveLastComponent();

                // Update action log with summarization result
                parserContext.ActionWindow.AddAction(
                    new Action
                    {
                        IsSuccess = true,
                        Message = summarizeAction.Message,
                        ItemPath = parserContext.ItemPath.DeepClone()
                    }
                );
            }

            // Return Result for Standard Object
            return parserContext.ItemPath.GetLastComponent()?.CurrentObject
                ?? throw new InvalidOperationException($"Parsing resulted in null data for type {objectType.FullName} at path {parserContext.ItemPath}.");
        }

        private async Task<JsonDocument> ParseMapping(
            ParserContext parserContext)
        {
            Type objectType = parserContext.ItemPath.LastObjectType;

            JsonSpecialModifier jsonSpecialFlag = ( parserContext.ItemPath.GetLastComponent() as ItemPathPropertyComponent )?.JsonSpecialModifier
                ?? JsonSpecialModifier.None;

            if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
            {
                parserContext.CodeWindow.StashWindow();
                parserContext.CodeWindow.ResetWindowPosition();
            }

            Type propertyType = objectType.GetPropertyElementType()
                ?? throw new NotSupportedException($"Unable to determine element type for mapping type {objectType.Name} at path {parserContext.ItemPath}");

            // Set the current object in the item path to a new mapping
            parserContext.ItemPath.UpdateCurrentObject(JsonDocument.Parse("{}"));

            bool isCompleted = false;
            while ( !isCompleted )
            {
                // Use the current context (pointing to the object being built) for inference
                parserContext.ItemPath.AddComponent(
                    new ItemPathToolComponent
                    {
                        ToolType = typeof(ItemBuilderSelector),
                        CurrentObject = null,
                    });
                InferenceRequest<ParserContext> selectorRequest = new() { Context = parserContext };
                // Type toolType = await ParseToolSelector(parserContext);
                // TODO: Might be ParseRecursively?
                object toolItem = await _inferenceHelper
                    .GetInferenceAsync(selectorRequest).ConfigureAwait(false)
                        ?? throw new InvalidOperationException($"Object builder selection failed at path {parserContext.ItemPath}.");

                switch ( toolItem )
                {
                    case NewProperty newProperty:
                        // Update item path FOR THE RECURSIVE CALL
                        parserContext.ItemPath.AddComponent(
                            new ItemPathKeyComponent
                            {
                                Key = newProperty.PropertyName,
                                ItemType = propertyType,
                                // No initial data for the new property
                                CurrentObject = null,
                            });

                        JsonDocument? parsedValue = null;
                        try
                        {
                            // Recursively parse the property value using the updated context
                            parsedValue = await ParseRecursively(parserContext).ConfigureAwait(false);
                        }
                        finally
                        {
                            // IMPORTANT: Remove the property component to restore the path
                            parserContext.ItemPath.RemoveLastComponent();
                        }

                        if ( parsedValue is null )
                        {
                            throw new InvalidOperationException($"Parsed value is null for property {newProperty.PropertyName} at path {parserContext.ItemPath}.");
                        }

                        // Add the parsed field data to the parent object
                        JsonDocument? currentObject = parserContext.ItemPath.GetLastComponent()?.CurrentObject;
                        JsonDocument? newParentObject = currentObject?.UpdateJsonDocument(newProperty.PropertyName, parsedValue);

                        // Merge the result back into 'parent' object
                        parserContext.ItemPath.UpdateCurrentObject(newParentObject);
                        // Log the addition of a new property
                        parserContext.ActionWindow.AddAction(new Action
                        {
                            IsSuccess = true,
                            Message = $"Added new property '{newProperty.PropertyName}' with value of '{parsedValue.GetAsJsonNode().ToJsonString()}'.",
                            ItemPath = parserContext.ItemPath.DeepClone()
                        });
                        break;

                    case EndItem endItem:
                        // Code to execute if FullContentScan is present
                        if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
                        {
                            if ( !parserContext.CodeWindow.IsFinished )
                            {
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = false,
                                        Message = "Full content scan not finished, you must check entire file",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                continue;
                            }
                        }

                        switch ( endItem.TerminationReason )
                        {
                            case EnItemTerminationReason.Finished:
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = true,
                                        Message = $"Finished parsing object data for {objectType.Name} at path {parserContext.ItemPath.FullPath}.",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                // Mark the object parsing as complete
                                isCompleted = true;
                                break;
                            case EnItemTerminationReason.Mistake:
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = false,
                                        Message = $"Tried to end {objectType.Name} at path {parserContext.ItemPath.FullPath} by mistake.",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                break;
                            default:
                                throw new NotSupportedException($"Unhandled termination reason: {endItem.TerminationReason}");
                        }

                        break;

                    default:
                        // Handle common tools like ScrollDown
                        if ( HandleCommonTools(parserContext, toolItem) )
                        {
                            break;
                        }

                        // Throw on unhandled tool selection
                        throw new NotSupportedException($"Unhandled object tool selection: {toolItem} at path {parserContext.ItemPath}");
                }
            }

            if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
            {
                parserContext.CodeWindow.PopWindow();
            }

            return parserContext.ItemPath.GetLastComponent()?.CurrentObject
                ?? throw new InvalidOperationException($"Parsing resulted in null data for type {objectType.FullName} at path {parserContext.ItemPath}.");
        }

        private async Task<JsonDocument> ParseIterable(
            ParserContext parserContext)
        {
            JsonSpecialModifier jsonSpecialFlag = ( parserContext.ItemPath.GetLastComponent() as ItemPathPropertyComponent )?.JsonSpecialModifier
                ?? JsonSpecialModifier.None;

            if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
            {
                parserContext.CodeWindow.StashWindow();
                parserContext.CodeWindow.ResetWindowPosition();
            }

            // Determine the element type (element type)
            Type objectType = parserContext.ItemPath.LastObjectType;
            Type? elementType = objectType.GetIterableElementType()
                ?? throw new NotSupportedException($"Unable to determine element type for iterable type {objectType.Name} at path {parserContext.ItemPath}");

            JsonDocument? iterableObject = parserContext.ItemPath.GetLastComponent()?.CurrentObject;
            if ( iterableObject is null )
            {
                JsonDocument emptyArray = JsonDocument.Parse("[]");
                parserContext.ItemPath.UpdateCurrentObject(emptyArray);
            }

            bool isCompleted = false;
            while ( !isCompleted )
            {
                // Use the current context (pointing to the object being built) for inference
                parserContext.ItemPath.AddComponent(
                    new ItemPathToolComponent
                    {
                        ToolType = typeof(IterableBuilderSelector),
                        CurrentObject = null,
                    });
                InferenceRequest<ParserContext> selectorRequest = new() { Context = parserContext };
                Type toolType = await ParseToolSelector(parserContext).ConfigureAwait(false);
                parserContext.ItemPath.AddComponent(
                    new ItemPathToolComponent
                    {
                        ToolType = toolType,
                        CurrentObject = null,
                    });
                JsonDocument toolItemDocument = await ParseRecursively(parserContext).ConfigureAwait(false);
                object toolItem = JsonHelper.DeserializeJsonDocumentToType(toolItemDocument, toolType);
                parserContext.ItemPath.RemoveLastComponent();

                switch ( toolItem )
                {
                    case NewItem newItem:
                        int itemIndex = parserContext.ItemPath.GetLastComponent()?.CurrentObject?.GetAsJsonArray().Count ??
                            throw new InvalidOperationException($"Parent object is null at path {parserContext.ItemPath}.");
                        // Update path for the recursive call
                        parserContext.ItemPath.AddComponent(
                            new ItemPathIndexComponent
                            {
                                Index = itemIndex,
                                ItemType = elementType,
                                // No initial data for the new item
                                CurrentObject = null
                            }
                        );

                        JsonDocument? parsedItem = null;
                        try
                        {
                            // Parse the new item using updated context
                            // Pass null for initialData
                            parsedItem = await ParseRecursively(parserContext).ConfigureAwait(false);
                        }
                        finally
                        {
                            // IMPORTANT: Remove the property component to restore the path
                            parserContext.ItemPath.RemoveLastComponent();
                        }

                        if ( parsedItem is null )
                        {
                            throw new InvalidOperationException($"Parsed value is null for index {itemIndex} at path {parserContext.ItemPath}.");
                        }

                        // Get current object
                        JsonDocument? currentObject = parserContext.ItemPath.GetLastComponent()?.CurrentObject;

                        // Check for uniqueness if applicable
                        JsonArray items = currentObject?.GetAsJsonArray()
                            ?? throw new InvalidOperationException($"Current object is null at path {parserContext.ItemPath}");

                        if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.UniqueItems)
                            && items.Any(item => item?.ToJsonString() == parsedItem.GetAsJsonNode().ToJsonString()) )
                        {
                            parserContext.ActionWindow.AddAction(new Action
                            {
                                IsSuccess = false,
                                Message = $"Duplicate item '{parsedItem.GetAsJsonNode().ToJsonString()}' cannot be added to unique collection",
                                ItemPath = parserContext.ItemPath.DeepClone()
                            });
                            continue;
                        }

                        // Add the parsed field data to the parent object
                        JsonDocument? newParentObject = currentObject?.UpdateJsonDocument(parsedItem, newItem.InsertAt);

                        // Merge the result back into 'parent' object
                        parserContext.ItemPath.UpdateCurrentObject(newParentObject);
                        // Log the addition of a new item
                        parserContext.ActionWindow.AddAction(new Action
                        {
                            IsSuccess = true,
                            Message = $"Added new item at index {itemIndex}. Item added is '{parsedItem.GetAsJsonNode().ToJsonString()}'",
                            ItemPath = parserContext.ItemPath.DeepClone()
                        });
                        break;

                    case EndItem endItem:
                        // Code to execute if FullContentScan is present
                        if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
                        {
                            if ( !parserContext.CodeWindow.IsFinished )
                            {
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = false,
                                        Message = "Full content scan not finished, must check entire file",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                continue;
                            }
                        }

                        switch ( endItem.TerminationReason )
                        {
                            case EnItemTerminationReason.Finished:
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = true,
                                        Message = $"Finished parsing iterable data for {objectType.Name} at path {parserContext.ItemPath.FullPath}.",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                // Mark the object parsing as complete
                                isCompleted = true;
                                break;
                            case EnItemTerminationReason.Mistake:
                                parserContext.ActionWindow.AddAction(
                                    new Action
                                    {
                                        IsSuccess = false,
                                        Message = $"Tried to end {objectType.Name} at path {parserContext.ItemPath.FullPath} by mistake.",
                                        ItemPath = parserContext.ItemPath.DeepClone()
                                    });
                                break;
                            default:
                                throw new NotSupportedException($"Unhandled termination reason: {endItem.TerminationReason}");
                        }

                        break;

                    default:
                        // Handle common tools
                        if ( HandleCommonTools(parserContext, toolItem) )
                        {
                            break;
                        }

                        throw new NotSupportedException($"Unhandled iterable tool selection: {toolItem} at path {parserContext.ItemPath}");
                }
            }

            if ( jsonSpecialFlag.HasFlag(JsonSpecialModifier.FullContentScan) )
            {
                parserContext.CodeWindow.PopWindow();
            }

            return parserContext.ItemPath.GetLastComponent()?.CurrentObject
                ?? throw new InvalidOperationException($"Parsing resulted in null data for type {objectType.FullName} at path {parserContext.ItemPath}.");
        }

        /// <summary>
        /// Handles common tool calls like scrolling within the parser context.
        /// </summary>
        /// <typeparam name="T">The type of the tool call object.</typeparam>
        /// <param name="parserContext">The current parser context.</param>
        /// <param name="toolItem">The tool call object.</param>
        /// <returns>True if the tool call was handled; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if toolCall is null.</exception>
        private static bool HandleCommonTools<T>(
            ParserContext parserContext,
            T toolItem) where T : class
        {
            switch ( toolItem )
            {
                case ScrollDown scrollDownTool:
                    // Handle scroll down action
                    if ( parserContext.CodeWindow?.LinesLeft == 0 )
                    {
                        parserContext.ActionWindow.AddAction(new Action
                        {
                            IsSuccess = false,
                            Message = $"Cannot scroll down {scrollDownTool.NumberOfLines} lines at {parserContext.ItemPath.FullPath}, no lines left.",
                            ItemPath = null
                        });
                    }
                    else
                    {
                        if ( parserContext.CodeWindow?.ScrollWindow(scrollDownTool.NumberOfLines) ?? false )
                        {
                            parserContext.ActionWindow.AddAction(new Action
                            {
                                IsSuccess = true,
                                Message = $"Scrolled down {scrollDownTool.NumberOfLines} lines at {parserContext.ItemPath.FullPath}",
                                ItemPath = null
                            });
                        }
                        else
                        {
                            parserContext.ActionWindow.AddAction(new Action
                            {
                                IsSuccess = true,
                                Message = $"Scrolling down {scrollDownTool.NumberOfLines} lines did not change window position",
                                ItemPath = null
                            });
                        }
                    }
                    return true;

                case PageDown pageDownTool:
                    // Handle page down action
                    parserContext.CodeWindow?.PageWindow(pageDownTool.OverlappingLines);
                    parserContext.ActionWindow.AddAction(new Action
                    {
                        IsSuccess = true,
                        Message = $"Page down with {pageDownTool.OverlappingLines} overlapping lines at {parserContext.ItemPath.FullPath}",
                        ItemPath = null
                    });
                    return true;

                case null:
                    throw new ArgumentNullException(nameof(toolItem), "Tool call cannot be null.");

                default:
                    return false;
            }
        }
    }
}
