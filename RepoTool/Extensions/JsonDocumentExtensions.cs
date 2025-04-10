using System.Text.Json;
using System.Text.Json.Nodes;
using Json.More;
using RepoTool.Enums.Parser.Tools.Builders.Iterable;
using RepoTool.Helpers;

namespace RepoTool.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="JsonDocument"/>.
    /// </summary>
    public static class JsonDocumentExtensions
    {
        // /// <summary>
        // /// Updates a JsonDocument with the provided key-value pairs. This version optimizes performance
        // /// by directly serializing values to JsonNode instead of intermediate strings.
        // /// </summary>
        // /// <param name="document">The JsonDocument to update.</param>
        // /// <param name="updates">Dictionary of key-value pairs to update in the document.</param>
        // /// <returns>A new JsonDocument with the applied updates.</returns>
        // /// <exception cref="InvalidOperationException">Thrown when the root element is not a JSON object or parsing fails.</exception>
        // public static JsonDocument UpdateJsonDocument(this JsonDocument document, Dictionary<string, object> updates)
        // {
        //     // Ensure the root element is an object
        //     if (document.RootElement.ValueKind != JsonValueKind.Object)
        //     {
        //         throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
        //     }

        //     // Parse the root element into a JsonObject for modification
        //     JsonObject? jsonObject = JsonNode.Parse(document.RootElement.GetRawText())?.AsObject();

        //     if (jsonObject is null)
        //     {
        //         // This should theoretically not happen if the ValueKind check passed, but defensive check.
        //         throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
        //     }

        //     foreach (KeyValuePair<string, object> update in updates)
        //     {
        //         // Directly serialize the object value to a JsonNode.
        //         // This avoids the intermediate step of serializing to a string and then parsing back.
        //         JsonNode? valueNode = JsonSerializer.SerializeToNode(update.Value);

        //         // Assign the new JsonNode to the JsonObject.
        //         // If valueNode is null (e.g., if update.Value was null and options handle it that way),
        //         // it will effectively set the property to JSON null.
        //         jsonObject[update.Key] = valueNode;
        //     }

        //     // Serialize the modified JsonObject back to a string and parse it into a new JsonDocument.
        //     // This final serialization/parsing is necessary to return an immutable JsonDocument.
        //     // Consider if returning JsonObject or modifying in place (if mutable structure is acceptable)
        //     // could offer further performance gains in specific scenarios, but JsonDocument is immutable.
        //     return JsonDocument.Parse(jsonObject.ToJsonString());
        // }



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
            JsonArray? jsonArray = JsonNode.Parse(document.RootElement.GetRawText())?.AsArray();

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
            return JsonDocument.Parse(jsonArray.ToJsonString());
        }

        public static object? GetPropertyValue(this JsonDocument document, string propertyName, Type propertyType)
        {
            // Ensure the root element is an object
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("The root element of the JsonDocument must be an object.");
            }

            // Parse the root element into a JsonObject for modification
            JsonObject? jsonObject = JsonNode.Parse(document.RootElement.GetRawText())?.AsObject();

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


            JsonObject? jsonObject = JsonNode.Parse(document.RootElement.GetRawText())?.AsObject();

            if (jsonObject is null)
            {

                throw new InvalidOperationException("Failed to parse the JsonDocument root element into a JsonObject.");
            }
            
            return jsonObject.TryGetPropertyValue(propertyName, out JsonNode? value)
                ? value ?? JsonNode.Parse("null") : throw new InvalidOperationException($"Property '{propertyName}' not found in the JsonDocument.");
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
            JsonArray? jsonArray = JsonNode.Parse(document.RootElement.GetRawText())?.AsArray();

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
            JsonNode? jsonObject = JsonNode.Parse(document.RootElement.GetRawText());

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
            // Using JsonNode.Parse on GetRawText is necessary here to get a mutable representation.
            JsonObject? jsonObject = JsonNode.Parse(document.RootElement.GetRawText())?.AsObject();

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

            // Serialize the modified JsonObject back to a string and parse it into a new JsonDocument.
            // This is required to return an immutable JsonDocument.
            return JsonDocument.Parse(jsonObject.ToJsonString());
        }
    }
}