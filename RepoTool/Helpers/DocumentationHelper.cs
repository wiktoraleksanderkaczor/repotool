// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using System.Xml.XPath;
using Json.Schema.Generation;
using LoxSmoke.DocXml;
using Microsoft.Extensions.DependencyInjection;
using RepoTool.Attributes.Helpers;
using RepoTool.Constants;
using RepoTool.Extensions;
using RepoTool.Flags.Parser;
using RepoTool.Models.Documentation;
using Spectre.Console;

namespace RepoTool.Helpers
{
    /// <summary>
    /// Provides helper methods for accessing XML documentation comments using the DocXml library.
    /// </summary>
    [ServiceLifetime(ServiceLifetime.Singleton)]
    public class DocumentationHelper
    {
        private readonly DocXmlReader _docXmlReader;

        /// <summary>
        /// Initializes the DocXmlReader with the embedded documentation XML.
        /// </summary>
        /// <remarks>
        /// This method should be called once during application startup or before the first use
        /// of GetTypeDocumentation. It's designed to load the XML only once for efficiency.
        /// Consider making this thread-safe if used in a multi-threaded context without guaranteed
        /// single initialization.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if the embedded documentation resource cannot be loaded or parsed.</exception>
        public DocumentationHelper()
        {
            try
            {
                string xmlContent = ResourceHelper.GetModelDocumentation();
                using StringReader stringReader = new(xmlContent);
                XPathDocument xpathDoc = new(stringReader);
                _docXmlReader = new DocXmlReader(xpathDoc);
            }
            catch ( Exception ex )
            {
                // Wrap the original exception for better context
                throw new InvalidOperationException("Failed to initialize DocXmlReader from embedded resource.", ex);
            }
        }

        /// <summary>
        /// Gets the structured XML documentation for a specific type using the DocXml library.
        /// </summary>
        /// <param name="type">The type to get documentation for.</param>
        /// <param name="derivedTypeDepth">Controls how many levels of derived types to collect. -1 for all levels, 0 for none, positive number for specific depth.</param>
        /// <returns>A <see cref="TypeDocumentation"/> object containing the summary, remarks, examples, fields, and properties from the type's XML documentation, or null if documentation is not found or an error occurs.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the input type is null.</exception>
        public TypeDocumentation GetTypeDocumentation(Type type, int derivedTypeDepth = -1)
        {
            try
            {
                Type typeToDocument = type;
                Type[]? elementTypes = null;
                if ( type.IsGenericType )
                {
                    // Handle generic types
                    elementTypes = type.GetGenericArguments();
                }

                TypeComments? typeComments = _docXmlReader.GetTypeComments(typeToDocument);

                List<MemberDocumentation>? fields = GetMembersDocumentationModel(type, MemberTypes.Field);
                List<MemberDocumentation>? properties = GetMembersDocumentationModel(type, MemberTypes.Property);
                List<StructDocumentation>? structs = GetStructMembersDocumentationModel(type);

                // Get derived types only if the type is abstract or an interface
                List<TypeDocumentation>? derivedTypes = ( type.IsAbstract || type.IsInterface )
                    ? GetAllDerivedTypesDocumentation(type, derivedTypeDepth) : null;

                TypeDocumentation typeDocumentation = new()
                {
                    TypeName = GetTypeName(typeToDocument),
                    DerivesFrom = GetTypeInheritance(type),
                    IsAbstract = type.IsAbstract,
                    IsInterface = type.IsInterface,
                    Summary = string.IsNullOrWhiteSpace(typeComments?.Summary) ? null : typeComments?.Summary,
                    Remarks = string.IsNullOrWhiteSpace(typeComments?.Remarks) ? null : typeComments?.Remarks,
                    Example = string.IsNullOrWhiteSpace(typeComments?.Example) ? null : typeComments?.Example,
                    Fields = fields,
                    Properties = properties,
                    // For generics, we don't want to collect derived types to avoid recursion
                    Generics = elementTypes?.Select(t => GetTypeDocumentation(t, 0)).ToList(),
                    // Only collect derived types if the type is abstract or an interface
                    DerivedTypes = derivedTypes,
                    Structs = structs
                };

                return typeDocumentation;
            }
            catch ( Exception ex )
            {
                // Log the exception appropriately in a real application
                string typeIdentifier = GetTypeName(type);
                AnsiConsole.WriteLine($"Error reading documentation for type '{typeIdentifier}': {ex.Message}");
                throw;
            }
        }

        public PropertyDocumentation GetPropertyDocumentation(PropertyInfo propertyInfo)
        {
            CommonComments? propertyComments = _docXmlReader.GetMemberComments(propertyInfo);
            JsonSpecialFlag jsonSpecialFlag = propertyInfo.GetJsonSpecialFlag();
            EnumComments? enumComments = jsonSpecialFlag != JsonSpecialFlag.None
                ? _docXmlReader.GetEnumComments(typeof(JsonSpecialFlag)) : null;

            return new PropertyDocumentation()
            {
                PropertyName = propertyInfo.Name,
                Handling = enumComments?.ValueComments
                    .Where(x => jsonSpecialFlag.HasFlag((JsonSpecialFlag)x.Value) && x.Value != default)
                    .Select(x =>
                        new HandlingDocumentation()
                        {
                            Name = x.Name,
                            Summary = string.IsNullOrWhiteSpace(x.Summary) ? null : x.Summary,
                            Remarks = string.IsNullOrWhiteSpace(x.Remarks) ? null : x.Remarks,
                            Example = string.IsNullOrWhiteSpace(x.Example) ? null : x.Example
                        }
                    ).ToList(),
                TypeName = GetTypeName(propertyInfo.PropertyType),
                Summary = string.IsNullOrWhiteSpace(propertyComments.Summary) ? null : propertyComments.Summary,
                Remarks = string.IsNullOrWhiteSpace(propertyComments.Remarks) ? null : propertyComments.Remarks,
                Example = string.IsNullOrWhiteSpace(propertyComments.Example) ? null : propertyComments.Example
            };
        }

        /// <summary>
        /// Gets documentation for all types that derive from the specified type with depth control,
        /// providing both a hierarchical and a flattened list.
        /// </summary>
        /// <param name="baseType">The base type to find derived types for.</param>
        /// <param name="derivedTypeDepth">Controls how many levels of derived types to collect. -1 for all levels, 0 for none, positive number for specific depth.</param>
        /// <returns>A list containing the flattened list of all derived types.</returns>
        private List<TypeDocumentation>? GetAllDerivedTypesDocumentation(Type baseType, int derivedTypeDepth)
        {
            // Base case: if depth is 0, don't collect any derived types
            if ( derivedTypeDepth == 0 )
            {
                return null;
            }

            // Get all types that derive directly from baseType
            HashSet<Type> directDerivedTypes = GetDirectlyDerivedTypes(baseType);
            List<TypeDocumentation> derivedTypeDocs = [];

            // Calculate the next depth level (if not unlimited)
            int nextDepthLevel = derivedTypeDepth == -1 ? -1 : derivedTypeDepth - 1;

            foreach ( Type derivedType in directDerivedTypes )
            {
                // Skip the type itself if it somehow appears in the derived types list
                if ( derivedType == baseType )
                {
                    continue;
                }

                // Get documentation for this derived type without its own derived types initially
                // This prevents infinite loops if GetTypeDocumentation were to call back immediately
                TypeDocumentation? derivedTypeDoc = GetTypeDocumentation(derivedType, 0);
                if ( derivedTypeDoc != null )
                {
                    List<TypeDocumentation>? recursiveDerived = null;
                    if ( nextDepthLevel != 0 )
                    {
                        // Recursively get derived types for this type with the decremented depth,
                        // but only if we are not at the final depth level requested (nextDepthLevel != 0).
                        recursiveDerived = GetAllDerivedTypesDocumentation(derivedType, nextDepthLevel);
                        derivedTypeDoc.DerivedTypes = recursiveDerived;
                    }

                    // Add the current derived type doc to the flat list
                    derivedTypeDocs.Add(derivedTypeDoc);
                    // Add all recursively found flat derived types to the current flat list
                    if ( recursiveDerived != null )
                    {
                        derivedTypeDocs.AddRange(recursiveDerived);
                    }
                }
            }

            return derivedTypeDocs.Count > 0 ? derivedTypeDocs : null;
        }


        /// <summary>
        /// Gets types that directly derive from the specified base type.
        /// </summary>
        /// <param name="baseType">The base type to find direct derived types for.</param>
        /// <returns>A HashSet of types that directly derive from the specified type.</returns>
        private HashSet<Type> GetDirectlyDerivedTypes(Type baseType)
        {
            HashSet<Type> derivedTypes = [];

            // Get all loaded assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach ( Assembly assembly in assemblies )
            {
                try
                {
                    // Get all types from the assembly
                    Type[] assemblyTypes = assembly.GetTypes();

                    foreach ( Type type in assemblyTypes )
                    {
                        // Skip abstract types, interfaces, and the base type itself
                        if ( type.IsAbstract || type.IsInterface || type == baseType )
                        {
                            continue;
                        }

                        // Check for class inheritance
                        if ( baseType.IsClass && type.BaseType == baseType )
                        {
                            derivedTypes.Add(type);
                            continue;
                        }

                        // Check for direct interface implementation
                        if ( baseType.IsInterface )
                        {
                            Type[] interfaces = type.GetInterfaces();

                            // Check if the type directly implements the interface
                            // and not through another interface that extends it
                            if ( interfaces.Contains(baseType) &&
                                !interfaces.Any(i => i != baseType &&
                                                      i.GetInterfaces().Contains(baseType)) )
                            {
                                derivedTypes.Add(type);
                            }
                        }
                    }
                }
                catch ( ReflectionTypeLoadException ex )
                {
                    // Handle or log the exception when types cannot be loaded
                    foreach ( Type? loadableType in ex.Types.Where(t => t != null) )
                    {
                        if ( loadableType == null ||
                            loadableType.IsAbstract ||
                            loadableType.IsInterface ||
                            loadableType == baseType )
                        {
                            continue;
                        }

                        // Check for class inheritance
                        if ( baseType.IsClass && loadableType.BaseType == baseType )
                        {
                            derivedTypes.Add(loadableType);
                            continue;
                        }

                        // Check for direct interface implementation
                        if ( baseType.IsInterface )
                        {
                            try
                            {
                                Type[] interfaces = loadableType.GetInterfaces();

                                // Check if the type directly implements the interface
                                if ( interfaces.Contains(baseType) &&
                                    !interfaces.Any(i => i != baseType &&
                                                         i.GetInterfaces().Contains(baseType)) )
                                {
                                    derivedTypes.Add(loadableType);
                                }
                            }
                            catch
                            {
                                // Skip if we can't get the interfaces
                                continue;
                            }
                        }
                    }
                }
                catch ( Exception )
                {
                    // Silently continue if an assembly cannot be processed
                    continue;
                }
            }

            return derivedTypes;
        }

        /// <summary>
        /// Gets documentation for members (Fields or Properties) of a type as a list of MemberDocumentation objects.
        /// </summary>
        private List<MemberDocumentation>? GetMembersDocumentationModel(Type type, MemberTypes memberType)
        {
            List<MemberDocumentation> memberDocs = [];
            MemberInfo[] members = type.GetMembers(TypeConstants.DefaultBindingFlags)
                                       .Where(m => m.MemberType == memberType)
                                       .Where(m => m.GetCustomAttribute<JsonExcludeAttribute>() == null)
                                       .Where(m => m.Name != "value__") // Exclude backing field for enums
                                       .OrderBy(m => m.Name) // Ensure consistent order
                                       .ToArray();

            foreach ( MemberInfo member in members )
            {
                MemberDocumentation? memberDoc = GetMemberDocumentationModel(member);
                if ( memberDoc != null )
                {
                    memberDocs.Add(memberDoc);
                }
            }
            return memberDocs.Count > 0 ? memberDocs : null;
        }

        /// <summary>
        /// Gets documentation for struct type property members as a list of StructDocumentation objects. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<StructDocumentation>? GetStructMembersDocumentationModel(Type type)
        {
            MemberInfo[] members = type.GetMembers(TypeConstants.DefaultBindingFlags)
                                       .Where(m => m.MemberType == MemberTypes.Property)
                                       .Where(m => m.GetCustomAttribute<JsonExcludeAttribute>() == null)
                                       .Where(m => m.Name != "value__") // Exclude backing field for enums
                                       .OrderBy(m => m.Name) // Ensure consistent order
                                       .ToArray();

            List<StructDocumentation> structDocs = [];
            foreach ( MemberInfo member in members )
            {
                CommonComments? comments = _docXmlReader.GetMemberComments(member);

                // Handle property itself being an enum
                if ( member is PropertyInfo propertyInfo )
                {
                    if ( propertyInfo.PropertyType.IsEnum )
                    {
                        StructDocumentation structDoc = new()
                        {
                            TypeName = GetTypeName(propertyInfo.PropertyType),
                            Fields = GetMembersDocumentationModel(propertyInfo.PropertyType, MemberTypes.Field),
                            Summary = string.IsNullOrWhiteSpace(comments?.Summary) ? null : comments?.Summary,
                            Remarks = string.IsNullOrWhiteSpace(comments?.Remarks) ? null : comments?.Remarks,
                            Example = string.IsNullOrWhiteSpace(comments?.Example) ? null : comments?.Example,
                        };
                        structDocs.Add(structDoc);
                    }

                    // Handle generic arguments for something like List<Enum>
                    Type[] genericArguments = propertyInfo.PropertyType.GetGenericArguments();
                    if ( genericArguments.Length > 0 )
                    {
                        foreach ( Type genericArgument in genericArguments )
                        {
                            if ( genericArgument.IsEnum )
                            {
                                StructDocumentation structDoc = new()
                                {
                                    TypeName = GetTypeName(genericArgument),
                                    Fields = GetMembersDocumentationModel(genericArgument, MemberTypes.Field),
                                    Summary = string.IsNullOrWhiteSpace(comments?.Summary) ? null : comments?.Summary,
                                    Remarks = string.IsNullOrWhiteSpace(comments?.Remarks) ? null : comments?.Remarks,
                                    Example = string.IsNullOrWhiteSpace(comments?.Example) ? null : comments?.Example,
                                };
                                structDocs.Add(structDoc);
                            }
                        }
                    }
                }
            }

            return structDocs.Count > 0 ? structDocs : null;
        }

        /// <summary>
        /// Gets the structured XML documentation for a specific member (Field or Property).
        /// Returns null if the member is not a field or property.
        /// </summary>
        private MemberDocumentation? GetMemberDocumentationModel(MemberInfo member)
        {
            string? memberTypeName = GetMemberTypeName(member);

            // Ensure it's a field or property
            if ( memberTypeName == null )
            {
                return null;
            }

            CommonComments? comments = _docXmlReader.GetMemberComments(member);
            string? memberValue = null;

            // Check if the member is a literal field (constant) and belongs to an enum type
            if ( member is FieldInfo fieldInfo
                && fieldInfo.IsLiteral
                && !fieldInfo.IsInitOnly
                && fieldInfo.DeclaringType != null
                && fieldInfo.DeclaringType.IsEnum )
            {
                if ( fieldInfo.Name == "value__" ) // Filter out backing field
                {
                    return null;
                }
                // Get the underlying value of the enum constant
                // object? rawValue = fieldInfo.GetRawConstantValue();
                // memberValue = rawValue?.ToString(); // Convert the value to string, handle potential null
                memberValue = fieldInfo.Name;
            }

            MemberDocumentation memberDoc = new()
            {
                Name = member.Name,
                TypeName = memberTypeName,
                Summary = string.IsNullOrWhiteSpace(comments?.Summary) ? null : comments?.Summary,
                Remarks = string.IsNullOrWhiteSpace(comments?.Remarks) ? null : comments?.Remarks,
                Example = string.IsNullOrWhiteSpace(comments?.Example) ? null : comments?.Example,
                Value = memberValue // Assign the retrieved value or null
            };

            return memberDoc;
        }

        /// <summary>
        /// Gets the full name of a type, handling potential nested types.
        /// </summary>
        private static string GetTypeName(Type type)
        {
            // Handle generic types gracefully
            if ( type.IsGenericType )
            {
                string genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetTypeName));
                // Use FullName for nested type handling, then remove backtick info
                string baseName = type.GetGenericTypeDefinition().FullName ?? type.GetGenericTypeDefinition().Name;
                return $"{baseName.Split('`')[0]}<{genericArgs}>".Replace('+', '.');
            }
            return type.FullName?.Replace('+', '.') ?? type.Name;
        }


        /// <summary>
        /// Gets the type name for a member (Field or Property).
        /// </summary>
        private static string? GetMemberTypeName(MemberInfo member)
        {
            return member switch
            {
                FieldInfo field => GetTypeName(field.FieldType),
                PropertyInfo property => GetTypeName(property.PropertyType),
                _ => null, // Return null for members that aren't fields or properties
            };
        }

        /// <summary>
        /// Gets a list of type names that the specified type directly derives from.
        /// This includes the base class (if not Object) and directly implemented interfaces.
        /// </summary>
        /// <param name="type">The type to analyze</param>
        /// <returns>A list of full type names that the specified type derives from</returns>
        private static List<string>? GetTypeInheritance(Type type)
        {
            if ( type == null )
            {
                return null;
            }

            List<string> derivesFrom = [];

            // Add base class if it's not Object (which is the implicit base class for all classes)
            if ( type.BaseType != null && type.BaseType != typeof(object) )
            {
                derivesFrom.Add(GetTypeName(type.BaseType));
            }

            Type[] interfaces = type
                .GetInterfaces()
                .Where(i => !i.Namespace?.StartsWith("System") ?? false) // Exclude System interfaces
                .ToArray();

            // Add directly implemented interfaces
            foreach ( Type interfaceType in interfaces )
            {
                // Check if this is a directly implemented interface
                // (not inherited through the base class or other interfaces)
                bool isDirectlyImplemented = true;

                // For classes, check if the interface is implemented by the base class
                if ( type.BaseType != null )
                {
                    foreach ( Type baseInterfaceType in type.BaseType.GetInterfaces() )
                    {
                        if ( baseInterfaceType == interfaceType )
                        {
                            isDirectlyImplemented = false;
                            break;
                        }
                    }
                }

                // For interfaces, check if the interface is extended by another implemented interface
                if ( type.IsInterface && isDirectlyImplemented )
                {
                    foreach ( Type otherInterface in type.GetInterfaces() )
                    {
                        if ( otherInterface != interfaceType )
                        {
                            foreach ( Type extendedInterface in otherInterface.GetInterfaces() )
                            {
                                if ( extendedInterface == interfaceType )
                                {
                                    isDirectlyImplemented = false;
                                    break;
                                }
                            }

                            if ( !isDirectlyImplemented )
                            {
                                break;
                            }
                        }
                    }
                }

                if ( isDirectlyImplemented )
                {
                    derivesFrom.Add(GetTypeName(interfaceType));
                }
            }

            return derivesFrom.Count > 0 ? derivesFrom : null;
        }
    }
}
