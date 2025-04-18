// Copyright (c) 2025 RepoTool. All rights reserved.
// Licensed under the Business Source License

using System.Reflection;
using System.Text.Json;
using Json.More;
using RepoTool.Flags.Parser;

namespace RepoTool.Models.Inference.Contexts.Parser
{
    /// <summary>
    /// Represents a path to an item in the parsed content.
    /// </summary>
    public record ItemPath
    {
        /// <summary>
        /// The components that make up the item path.
        /// </summary>
        public required List<ItemPathComponent> Components { get; set; }

        /// <summary>
        /// Convenience to show the full path as a string.
        /// </summary>
        public string FullPath => string.Join(".", Components.Select(c => c switch
        {
            ItemPathRootComponent rootComponent => rootComponent.RecordType.Name,
            ItemPathIndexComponent indexComponent => $"[{indexComponent.Index}]",
            ItemPathKeyComponent keyComponent => $"[{keyComponent.Key}]",
            ItemPathPropertyComponent propertyComponent => propertyComponent.PropertyName,
            // Tool components are not included in the path string
            ItemPathToolComponent toolComponent => string.Empty,
            _ => throw new NotSupportedException("Unknown component type.")
        }));

        /// <summary>
        /// Convenience to get the last component object type.
        /// </summary>
        /// <returns>Type</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public Type GetLastObjectType()
        {
            // Get the last component type
            return Components.Count > 0
                ? Components[^1] switch
                {
                    ItemPathRootComponent rootComponent => rootComponent.RecordType,
                    ItemPathIndexComponent indexComponent => indexComponent.ItemType,
                    ItemPathKeyComponent keyComponent => keyComponent.ItemType,
                    ItemPathPropertyComponent propertyComponent => propertyComponent.PropertyInfo.PropertyType,
                    ItemPathToolComponent toolComponent => toolComponent.ToolType,
                    _ => throw new NotSupportedException("Unknown component type.")
                }
                : throw new InvalidOperationException("No components in path.");
        }

        /// <summary>
        /// Adds a tool component to the end of the current path in-place.
        /// </summary>
        /// <param name="component"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void AddComponent(ItemPathComponent component)
        {
            if ( component == null )
            {
                throw new ArgumentNullException(nameof(component));
            }

            // Modify the list in-place
            Components.Add(component);
        }


        /// <summary>
        /// Removes the last component from the current path in-place.
        /// Does nothing if the path is empty.
        /// </summary>
        public void RemoveLastComponent()
        {
            // Remove the last component
            if ( Components.Count > 0 )
            {
                // Modify the list in-place
                Components.RemoveAt(Components.Count - 1);
            }
        }

        /// <summary>
        /// Get the last component object.
        /// </summary>
        /// <returns>ItemPathComponent?</returns>
        public ItemPathComponent? GetLastComponent() =>
            // Get the last component
            Components.Count > 0 ? Components[^1] : null;

        /// <summary>
        /// Convenience to get the last component for a specific component type.
        /// </summary>
        /// <returns>Type</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public TComponent GetLastComponent<TComponent>()
            where TComponent : ItemPathComponent
        {
            // Get the last component matching TComponent type
            return Components.Count > 0
                ? Components.LastOrDefault(c => c is TComponent) as TComponent
                    ?? throw new InvalidOperationException("No component of the specified type found.")
                : throw new InvalidOperationException("No components in path.");
        }

        /// <summary>
        /// Get the parent component object.
        /// </summary>
        /// <returns>ItemPathComponent?</returns>
        public ItemPathComponent? GetParentComponent() =>
            // Get the parent component for the last component
            Components.Count > 0 ? Components[^2] : null;

        public JsonSpecialFlag GetJsonSpecialFlag()
        {
            // Get the special flag for the last component
            return Components.Count > 0
                ? Components[^1] switch
                {
                    ItemPathPropertyComponent propertyComponent => propertyComponent.JsonSpecialFlag,
                    _ => JsonSpecialFlag.None
                }
                : JsonSpecialFlag.None;
        }

        /// <summary>
        /// Update current component object.
        /// </summary>
        /// <param name="currentObject">The current object to set.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="currentObject"/> is null.</exception>
        public void UpdateCurrentObject(JsonDocument? currentObject)
        {
            if ( currentObject == null )
            {
                throw new ArgumentNullException(nameof(currentObject));
            }

            // Update the current object for the last component
            if ( Components.Count > 0 )
            {
                Components[^1].CurrentObject = currentObject;
            }
        }
    }

    public abstract record ItemPathComponent
    {
        /// <summary>
        /// Represents the current object in the item path.
        /// Will show partial object when editing.
        /// </summary>
        public required JsonDocument? CurrentObject { get; set; }

        /// <summary>
        /// Represents the current object in the item path as JSON
        /// Will show partial object when editing.
        /// </summary>
        public string? CurrentObjectJson => CurrentObject?.RootElement.ToJsonString();
    }

    /// <summary>
    /// Represents an index in the item path for an iterable
    /// </summary>
    public record ItemPathIndexComponent : ItemPathComponent
    {
        private int _index;

        /// <summary>
        /// The index in the item path.
        /// </summary>
        public required int Index
        {
            get => _index;
            init
            {
                if ( value < 0 )
                {
                    throw new ArgumentException("Index cannot be negative.", nameof(value));
                }
                _index = value;
            }
        }

        public required Type ItemType { get; init; }
    }

    /// <summary>
    /// Represents a key in the item path for a mapping
    /// </summary>
    public record ItemPathKeyComponent : ItemPathComponent
    {
        private string _key = null!;

        /// <summary>
        /// The key in the item path.
        /// </summary>
        public required string Key
        {
            get => _key;
            init
            {
                if ( string.IsNullOrWhiteSpace(value) )
                {
                    throw new ArgumentException("Key cannot be empty or whitespace.", nameof(value));
                }
                _key = value;
            }
        }

        public required Type ItemType { get; init; }
    }

    public record ItemPathRootComponent : ItemPathComponent
    {
        public required Type RecordType { get; init; }
    }

    /// <summary>
    /// Represents a property in the item path.
    /// </summary>
    public record ItemPathPropertyComponent : ItemPathComponent
    {
        /// <summary>
        /// Represents the name of the property in the item path.
        /// </summary>
        private string _propertyName = null!;

        /// <summary>
        /// Represents the name of the property in the item path.
        /// </summary>
        public required string PropertyName
        {
            get => _propertyName;
            init
            {
                if ( string.IsNullOrWhiteSpace(value) )
                {
                    throw new ArgumentException("Property name cannot be empty or whitespace.", nameof(value));
                }
                _propertyName = value;
            }
        }

        /// <summary>
        /// Property currently being processed
        /// </summary>
        public required PropertyInfo PropertyInfo { get; init; }

        /// <summary>
        /// Specifies the special flags associated with the property.
        /// </summary>
        public required JsonSpecialFlag JsonSpecialFlag { get; init; }
    }

    /// <summary>
    /// Represents a tool used in the item path.
    /// </summary>
    public record ItemPathToolComponent : ItemPathComponent
    {
        private Type _toolType = null!;

        /// <summary>
        /// Tool output type.
        /// </summary>
        public required Type ToolType
        {
            get => _toolType; init => _toolType = value ?? throw new ArgumentNullException(nameof(value), "Tool type cannot be null.");
        }
    }
}
