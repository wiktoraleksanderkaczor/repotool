namespace RepoTool.Models.Parser.Tools.Builders.Object
{
    public record NewProperty
    {
        /// <summary>
        /// Indicates the name of the property.
        /// </summary>
        public required string PropertyName { get; init; }
    }
}