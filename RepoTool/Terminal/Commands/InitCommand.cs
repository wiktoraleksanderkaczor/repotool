// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.Schema;
using Microsoft.EntityFrameworkCore;
using RepoTool.Constants;
using RepoTool.Extensions;
using RepoTool.Helpers;
using RepoTool.Options.Common;
using RepoTool.Persistence;
using RepoTool.Terminal.Commands.Common;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RepoTool.Terminal.Commands
{
    public class InitCommand : AsyncCommand<CommonSettings>
    {
        private readonly RepoToolDbContext _dbContext;
        public InitCommand(RepoToolDbContext dbContext) => _dbContext = dbContext;

        public override async Task<int> ExecuteAsync(CommandContext context, CommonSettings settings)
        {
            // Ensure containing directory exists
            if ( !Directory.Exists(PathConstants.RepoToolFolder) )
            {
                Directory.CreateDirectory(PathConstants.RepoToolFolder);
            }

            // Ensure database file exists
            if ( !File.Exists(PathConstants.DatabasePath) )
            {
                File.Create(PathConstants.DatabasePath).Dispose();
            }

            // Apply migrations
            // Ensure the database is created and migrated
            // dbContext.Database.EnsureCreated() does not work with SQLite    
            _dbContext.Database.Migrate();

            // Find types implementing IOptionModel in the calling assembly
            Assembly callingAssembly = Assembly.GetExecutingAssembly();
            IEnumerable<Type> optionModelTypes = callingAssembly
                .GetTypes()
                .Where(t =>
                    t is { IsClass: true, IsAbstract: false }
                    && t.IsAssignableTo(typeof(IOptionModel)))
                .Distinct();

            JsonSchema jsonSchema = JsonSchema.Empty;
            foreach ( Type modelType in optionModelTypes )
            {
                try
                {
                    // Look for a public static property named "Section".
                    PropertyInfo sectionProperty = modelType.GetProperty("Section", BindingFlags.Public | BindingFlags.Static)
                        ?? throw new InvalidOperationException($"Static property 'Section' not found on type {modelType.FullName}.");

                    // Get the value of the static property.
                    string sectionName = sectionProperty
                        // Pass null for static property access.
                        .GetValue(null) as string
                            ?? throw new InvalidOperationException($"Static property 'Section' not found on type {modelType.FullName}.");


                    JsonSchema jsonSchemaSection = await JsonHelper.GetOrCreateJsonSchemaAsync(modelType);
                    jsonSchema = jsonSchema.Merge(jsonSchemaSection, sectionName);

                }
                catch ( Exception ex ) when ( ex is not InvalidOperationException ) // Catch reflection/invocation issues, but let InvalidOperationExceptions propagate
                {
                    // Wrap the original exception for clarity if needed, or simply rethrow.
                    // Since logging is removed, rethrowing is crucial for visibility.
                    throw new InvalidOperationException($"Failed to register options for type {modelType.FullName}.", ex);
                }
            }

            await File.WriteAllTextAsync(PathConstants.SettingsSchemaPath, jsonSchema.ToJson());

            // Create settings.json file in repo tool folder pointing to settings-schema.json as $schema
            string schemaValue = $"{Path.GetFileName(PathConstants.SettingsSchemaPath)}";
            string minimalSettingsContent = $$"""
            {
              "$schema": "{{schemaValue}}"
            }
            """;

            if ( File.Exists(PathConstants.SettingsPath) )
            {
                try
                {
                    // Read existing content
                    string existingContent = await File.ReadAllTextAsync(PathConstants.SettingsPath);
                    JsonNode? jsonNode = JsonNode.Parse(existingContent, new JsonNodeOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }, new JsonDocumentOptions()
                    {
                        AllowTrailingCommas = true,
                        CommentHandling = JsonCommentHandling.Skip
                    });

                    // Check if it's a JSON object
                    if ( jsonNode is JsonObject jsonObject )
                    {
                        // Add or update the $schema property
                        jsonObject["$schema"] = schemaValue;

                        // Write the modified JSON back, preserving formatting if possible
                        string updatedContent = jsonObject.ToJson();
                        await File.WriteAllTextAsync(PathConstants.SettingsPath, updatedContent);
                    }
                    else
                    {
                        AnsiConsole.WriteLine("The settings file is not a valid JSON object.");
                        Environment.Exit(1);
                    }
                }
                catch ( JsonException )
                {
                    // If parsing fails (invalid JSON)
                    AnsiConsole.WriteLine("Invalid JSON format in settings file.");
                    throw;
                }
                catch ( Exception )
                {
                    throw; // Rethrow if the error should halt execution
                }
            }
            else
            {
                // File doesn't exist, create it with minimal content
                await File.WriteAllTextAsync(PathConstants.SettingsPath, minimalSettingsContent);
            }

            return 0;
        }
    }
}
