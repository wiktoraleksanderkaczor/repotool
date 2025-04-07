namespace RepoTool.Models.Documentation
{
    public record HandlingDocumentation : BaseDocumentation
    {
        public required string Name { get; init; }
    }

    public record PropertyDocumentation : BaseTypedDocumentation
    {
        public required string PropertyName { get; init; }

        public required List<HandlingDocumentation>? Handling { get; init; }
    }
}