using System.Text.Json;
using System.Text.Json.Nodes;
using Json.More;
using Json.Pointer;
using RepoTool.Enums.Parser.Tools.Builders.Iterable;
using RepoTool.Helpers;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="JsonDocument"/>.
    /// </summary>
    public static class JsonDocumentExtensions
    {
        /// <summary>
        /// Removes the JSON element specified by the JSON Pointer.
        /// Traverses the JSON structure according to the pointer segments and removes the target element
        /// from its parent object or array.
        /// </summary>
        /// <param name="document">The JsonDocument to modify.</param>
        /// <param name="pointer">The JSON Pointer indicating the element to remove.</param>
        /// <returns>A new, independent JsonDocument instance with the specified element removed.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the pointer path is invalid (e.g., segment not found, incorrect type for segment, index out of bounds)
        /// or if the target element indicated by the last segment does not exist in the parent.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if parsing the input JsonDocument fails or if an unexpected state occurs during traversal
        /// (e.g., the calculated parent node is not an object or array).
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the pointer is empty or null.
        /// </exception>
        public static JsonDocument RemoveAtPointer(this JsonDocument document, JsonPointer pointer)
        {
            if (pointer.Count == 0)
            {
                throw new ArgumentNullException(nameof(pointer), "Pointer cannot be empty.");
            }

            // Parse the document into a mutable JsonNode for modification.
            JsonNode? rootNode = document.RootElement.AsNode();
            if (rootNode is null)
            {
                // This should generally not happen if the input JsonDocument is valid.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonNode.");
            }

            JsonNode? parentNode = rootNode;
            // Traverse the path using all segments *except* the last one to find the parent node.
            foreach (string segmentValue in pointer[..^1])
            {
                if (parentNode is JsonObject jsonObject)
                {
                    // Try to get the next node using the segment value as a key.
                    if (jsonObject.TryGetPropertyValue(segmentValue, out JsonNode? nextNode) && nextNode is not null)
                    {
                        parentNode = nextNode;
                    }
                    else
                    {
                        // The key specified by the segment does not exist in the current object.
                        throw new ArgumentException(
                            $"Invalid pointer path: Segment '{segmentValue}' not found in object.", nameof(pointer));
                    }
                }
                else if (parentNode is JsonArray jsonArray)
                {
                    // Try to parse the segment value as an integer index.
                    // Use System.Linq.Enumerable.All for the check
                    if (Enumerable.All(segmentValue, char.IsDigit) && int.TryParse(segmentValue, out int index) && index >= 0 && index < jsonArray.Count)
                    {
                        // Access the element at the specified index.
                        JsonNode? nextNode = jsonArray[index];
                        // Even if the index is valid, the element itself might be JSON null.
                        // We need a non-null node to continue traversal.
                        if (nextNode is not null)
                        {
                            parentNode = nextNode;
                        }
                        else
                        {
                            // Found a JSON null at a valid index. Cannot traverse further through it.
                            throw new ArgumentException(
                            $"Invalid pointer path: Cannot traverse through null element found at index {index}.", nameof(pointer));
                        }
                    }
                    else
                    {
                        // The segment value is not a valid integer index or is out of the array bounds.
                        throw new ArgumentException(
                            $"Invalid pointer path: Segment '{segmentValue}' is not a valid index or is out of bounds for the array.", nameof(pointer));
                    }
                }
                else
                {
                    // The current node is a value (string, number, boolean, null), not a container. Cannot traverse further.
                    throw new ArgumentException(
                    $"Invalid pointer path: Cannot traverse through non-container element at segment '{segmentValue}'.", nameof(pointer));
                }
            }

            // After the loop, 'parentNode' holds the direct parent of the element to be removed.
            // 'lastSegment' holds the key or index of the element to remove within that parent.
            string lastSegmentValue = pointer[pointer.Count - 1];

            if (parentNode is JsonObject finalParentObject)
            {
                // Check if the key exists before attempting removal.
                if (finalParentObject.ContainsKey(lastSegmentValue))
                {
                    finalParentObject.Remove(lastSegmentValue);
                }
                else
                {
                    // The current node is a value (string, number, boolean, null), not a container. Cannot traverse further.
                    throw new ArgumentException(
                    $"Invalid pointer path: Target key '{lastSegmentValue}' not found in the final object.", nameof(pointer));
                }
            }
            else if (parentNode is JsonArray finalParentArray)
            {
                // Check if the last segment represents a valid integer index.
                // Use System.Linq.Enumerable.All for the check
                if (Enumerable.All(lastSegmentValue, char.IsDigit) && int.TryParse(lastSegmentValue, out int indexToRemove))
                {
                    // Check if the index is within the bounds of the array.
                    if (indexToRemove >= 0 && indexToRemove < finalParentArray.Count)
                    {
                        finalParentArray.RemoveAt(indexToRemove);
                    }
                    else
                    {
                        // The index is out of bounds for the parent array.
                        throw new ArgumentException(
                            $"Invalid pointer path: Target index {indexToRemove} is out of bounds for the final array (size: {finalParentArray.Count}).", nameof(pointer));
                    }
                }
                else
                {
                    // The last segment is not a valid non-negative integer index for the parent array.
                    throw new ArgumentException(
                    $"Invalid pointer path: Target segment '{lastSegmentValue}' is not a valid index for the final array.", nameof(pointer));
                }
            }
            else
            {
                // This state should theoretically not be reachable if the pointer had at least one segment
                // and the traversal logic is correct, as the parent must be an object or array.
                throw new InvalidOperationException("Internal error: The calculated parent node is not a JsonObject or JsonArray.");
            }

            // Serialize the modified root node back into a new, immutable JsonDocument.
            return rootNode.ToJsonDocument();
        }

        /// <summary>
        /// Updates a JsonDocument containing an iterable with addition of the provided object. 
        /// This version optimizes performance by directly serializing values to JsonNode instead of intermediate strings.
        /// </summary>
        /// <param name="document">The JsonDocument to update.</param>
        /// <param name="item">The object to add to the iterable in the document.</param>
        /// <param name="insertAt">Specifies where to insert the new item in the iterable.</param>
        /// <returns>A new JsonDocument with the applied updates.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the root element is not a JSON object or parsing fails.</exception>
        public static JsonDocument UpdateJsonDocument(this JsonDocument document, object item, EnIterableInsertAt insertAt)
        {
            // Ensure the root element is an object
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an array.");
            }

            // Parse the root element into a JsonArray for modification
            JsonArray? jsonArray = document.RootElement.AsNode()?.AsArray();

            if (jsonArray is null)
            {
                // This should theoretically not happen if the ValueKind check passed, but defensive check.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonArray.");
            }

            // Directly serialize the object value to a JsonNode.
            JsonNode? valueNode = JsonSerializer.SerializeToNode(item);

            // Add the new JsonNode to the JsonArray.
            switch (insertAt)
            {
                case EnIterableInsertAt.Start:
                    jsonArray = jsonArray.Prepend(valueNode).ToJsonArray();
                    break;
                case EnIterableInsertAt.End:
                    jsonArray = jsonArray.Append(valueNode).ToJsonArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(insertAt), insertAt, null);
            }

            // Serialize the modified JsonArray back to a string and parse it into a new JsonDocument.
            return jsonArray.ToJsonDocument();
        }

        public static object? GetPropertyValue(this JsonDocument document, string propertyName, Type propertyType)
        {
            // Ensure the root element is an object
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
            }

            // Parse the root element into a JsonObject for modification
            JsonObject? jsonObject = document.RootElement.AsNode()?.AsObject();

            if (jsonObject is null)
            {
                // This should theoretically not happen if the ValueKind check passed, but defensive check.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
            }

            jsonObject.TryGetPropertyValue(propertyName, out JsonNode? value);

            return value != null
                ? JsonHelper.DeserializeJsonToType(value.ToJsonString(), propertyType)
                : null;
        }

        public static JsonNode? GetPropertyValue(this JsonDocument document, string propertyName)
        {

            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
            }


            JsonObject? jsonObject = document.RootElement.AsNode()?.AsObject();

            if (jsonObject is null)
            {

                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
            }

            return jsonObject.TryGetPropertyValue(propertyName, out JsonNode? value)
                ? value ?? JsonNode.Parse("null")
                : throw new InvalidOperationException($"Property '{propertyName}' not found in the JsonDocument.");
        }

        /// <summary>
        /// Gets the root element of the JsonDocument as a JsonArray.
        /// This method is useful for scenarios where the root element is expected to be an array.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static JsonArray GetAsJsonArray(this JsonDocument document)
        {
            // Ensure the root element is an array
            if (document.RootElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an array.");
            }

            // Parse the root element into a JsonArray for modification
            JsonArray? jsonArray = document.RootElement.AsNode()?.AsArray();

            if (jsonArray is null)
            {
                // This should theoretically not happen if the ValueKind check passed, but defensive check.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonArray.");
            }

            return jsonArray;
        }

        /// <summary>
        /// Gets the root element of the JsonDocument as a JsonNode.
        /// This method is useful for scenarios where the root element is expected to be a JSON node.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static JsonNode GetAsJsonNode(this JsonDocument document)
        {
            // Ensure the root element is an object
            // if (document.RootElement.ValueKind != JsonValueKind.Object)
            // {
            //     throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
            // }

            // Parse the root element into a JsonObject for modification
            JsonNode? jsonObject = document.RootElement.AsNode();

            if (jsonObject is null)
            {
                // This should theoretically not happen if the ValueKind check passed, but defensive check.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
            }

            return jsonObject;
        }

        /// <summary>
        /// Updates a JsonDocument with a single key-value pair. This version optimizes performance
        /// by directly serializing the value to JsonNode instead of intermediate strings.
        /// </summary>
        /// <param name="document">The JsonDocument to update.</param>
        /// <param name="fieldName">The key of the field to update.</param>
        /// <param name="value">The value to set for the specified field.</param>
        /// <returns>A new JsonDocument with the applied update.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the root element is not a JSON object or parsing fails.</exception>
        public static JsonDocument UpdateJsonDocument(this JsonDocument document, string fieldName, JsonDocument value)
        {
            return document.UpdateJsonDocument(new Dictionary<string, JsonDocument> { { fieldName, value } });
        }

        /// <summary>
        /// Updates a JsonDocument with the provided key-JsonDocument pairs. This version optimizes performance
        /// by cloning JsonElements instead of parsing raw text repeatedly.
        /// </summary>
        /// <param name="document">The JsonDocument to update.</param>
        /// <param name="updates">Dictionary of key-JsonDocument pairs to update in the document.</param>
        /// <returns>A new JsonDocument with the applied updates.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the root element is not a JSON object or parsing fails.</exception>
        public static JsonDocument UpdateJsonDocument(this JsonDocument document, Dictionary<string, JsonDocument> updates)
        {
            // Ensure the root element is an object
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
            }

            // Parse the root element into a JsonObject for modification
            JsonObject? jsonObject = document.RootElement.AsNode()?.AsObject();

            if (jsonObject is null)
            {
                // This should theoretically not happen if the ValueKind check passed, but defensive check.
                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
            }

            foreach (KeyValuePair<string, JsonDocument> update in updates)
            {
                // Clone the root element of the source JsonDocument.
                // Cloning is necessary because JsonDocument owns its memory, and we need a copy.
                JsonElement clonedElement = update.Value.RootElement.Clone();

                // Convert the cloned JsonElement to a JsonNode.
                // This avoids parsing the raw text of the update document.
                JsonNode? valueNode = JsonSerializer.SerializeToNode(clonedElement);

                // Assign the new JsonNode to the JsonObject.
                jsonObject[update.Key] = valueNode;

                // Dispose the JsonDocument provided in the dictionary as its content is now copied/cloned.
                update.Value.Dispose();
            }

            return jsonObject.ToJsonDocument();
        }
    }
}