// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Text;
using RepoTool.Enums.Parser.Common;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Represents information about a type definition.
    /// - Handles generics and generic constraints
    /// - Works with wrapped types like lists, dictionaries, etc.
    /// </summary>
    public record TypeInfoDefinition
    {
        /// <summary>
        /// The type itself.
        /// </summary>
        /// <example>
        /// <code>
        /// List&lt;string&gt;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Type": { "TypeName": "List" }
        /// }
        /// </code>
        /// </example>
        public required TypeInfoComponent Type { get; init; }

        /// <summary>
        /// The types that this type contains.
        /// E.g. for a List&lt;int&gt;, the component is int.
        /// Do not include current level of type itself in the components list.
        /// </summary>
        /// <example>
        /// <code>
        /// List&lt;string&gt;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "InnerComponents": [ { "TypeName": "string" } ]
        /// }
        /// </code>
        /// </example>
        public required List<TypeInfoComponent>? InnerComponents { get; init; }
    }

    /// <summary>
    /// Represents a component of a type definition.
    /// </summary>
    public record TypeInfoComponent
    {
        /// <summary>
        /// The type name, e.g. int, string, List, Dictionary etc.
        /// It could be a user-defined type like MyType.
        /// More complex ones like Dict&lt;string, int&gt; when built from collections.
        /// It could also be TEntity or TKey, TValue etc. for generics.
        /// It could also be null for duck-typed languages.
        /// </summary>
        /// <example>
        /// <code>
        /// List&lt;string&gt;
        /// </code>
        /// Would have "TypeName": "List" for the outer component.
        /// </example>
        public required string? TypeName { get; init; }

        /// <summary>
        /// Fully qualified name of the type in its namespace or where it was imported
        /// e.g. MyNamespace.MyClass.MyStruct.MyType would be ["MyNamespace", "MyClass", "MyStruct"]
        /// e.g. System.Collections.Generic.List&lt;int&gt; would be ["System", "Collections", "Generic"]
        /// Primitive types do not have a fully qualified name so those would be null.
        /// </summary>
        /// <example>
        /// <code>
        /// System.Collections.Generic.List&lt;string&gt;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///    "FullyQualifiedPath": ["System", "Collections", "Generic"]
        /// }
        /// </code>
        /// </example>
        public required List<string>? FullyQualifiedPath { get; init; }

        /// <summary>
        /// List of all applicable modifier flags for this type.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// int?
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ModifierFlags": ["Nullable"]
        /// }
        /// </code>
        /// </example>
        public required List<EnTypeInfoModifier> ModifierFlags { get; init; }

        /// <summary>
        /// Generic constraint information.
        /// </summary>
        /// <example>
        /// <code>
        /// where T : class
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "Constraints": [{ "ConstraintFlags": "IsReferenceTypeKind" }]
        /// }
        /// </code>
        /// </example>
        public required List<GenericConstraintInfo>? Constraints { get; init; }

        /// <summary>
        /// Generic type arguments.
        /// </summary>
        /// <example>
        /// <code>
        /// List&lt;string&gt;
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "GenericArguments": [ { "Type": { "TypeName": "string" } } ]
        /// }
        /// </code>
        /// </example>
        public required List<TypeInfoDefinition>? GenericArguments { get; init; }

        public string GetUniqueName()
        {
            StringBuilder sb = new();
            if ( FullyQualifiedPath != null )
            {
                foreach ( string scope in FullyQualifiedPath )
                {
                    sb = sb.Append(scope);
                    sb = sb.Append('.');
                }
            }

            if ( TypeName != null )
            {
                sb = sb.Append(TypeName);
            }
            sb = sb.Append(GetGenericArgumentString());
            return sb.ToString();
        }

        private string GetGenericArgumentString()
        {
            StringBuilder sb = new();
            if ( GenericArguments != null && GenericArguments.Count > 0 )
            {
                sb = sb.Append('<');
                if ( GenericArguments.Count > 0 )
                {
                    bool first = true;
                    foreach ( TypeInfoDefinition genericArgument in GenericArguments )
                    {
                        if ( !first )
                        {
                            sb = sb.Append(", ");
                        }
                        sb = sb.Append(genericArgument.Type.GetUniqueName());
                        first = false;
                    }
                }
                sb = sb.Append('>');
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// Represents information about generic constraints.
    /// </summary>
    public record GenericConstraintInfo
    {
        /// <summary>
        /// Name accompanying constraint flag
        /// e.g. which class to inherit from for Inherits constraint
        /// or which value/reference type to use for IsValueTypeKind or IsReferenceTypeKind
        /// It could also be null for constraints like NotNull
        /// </summary>
        /// <example>
        /// <code>
        /// where T : MyClass
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///     "Name": "MyClass"
        /// }
        /// </code>
        /// </example>
        public required string? Name { get; init; }

        /// <summary>
        /// List of all applicable constraint flags for this type.
        /// Apply all applicable flags for the current item.
        /// </summary>
        /// <example>
        /// <code>
        /// where T : class
        /// </code>
        /// Would be parsed as:
        /// <code>
        /// {
        ///   "ConstraintFlags": ["IsReferenceTypeKind"]
        /// }
        /// </code>
        /// </example>
        public required List<EnGenericConstraintType> ConstraintFlags { get; init; }
    }
}
