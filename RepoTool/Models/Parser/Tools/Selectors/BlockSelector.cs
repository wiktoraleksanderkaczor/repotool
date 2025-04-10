using RepoTool.Attributes;
using RepoTool.Models.Parser.Interfaces;
using RepoTool.Models.Parser.Items.Blocks;
using RepoTool.Models.Parser.Tools.Navigation;


/// <summary>
/// Defines the possible types of code blocks.
/// </summary>
public enum EnBlockType
{
    /// <summary>
    /// Represents a scroll down action.
    /// </summary>
    [ToolChoice(typeof(ScrollDown))]
    ScrollDown,

    /// <summary>
    /// Represents a namespace block.
    /// </summary>
    [ItemChoice(typeof(NamespaceBlock))]
    Namespace,

    /// <summary>
    /// Represents a class block.
    /// </summary>
    [ItemChoice(typeof(ClassBlock))]
    Class,

    /// <summary>
    /// Represents a struct block.
    /// </summary>
    [ItemChoice(typeof(StructBlock))]
    Struct,

    /// <summary>
    /// Represents an interface block.
    /// </summary>
    [ItemChoice(typeof(InterfaceBlock))]
    Interface,

    /// <summary>
    /// Represents an enum block.
    /// </summary>
    [ItemChoice(typeof(EnumBlock))]
    Enum,

    /// <summary>
    /// Represents a record block.
    /// </summary>
    [ItemChoice(typeof(RecordBlock))]
    Record,

    /// <summary>
    /// Represents a callable block (e.g., method, function).
    /// </summary>
    [ItemChoice(typeof(CallableBlock))]
    Callable
}

public record BlockSelector : IToolSelector<EnBlockType>
{
    /// <summary>
    /// The type of the block.
    /// </summary>
    /// <example>
    /// <code>
    /// public class MyClass { }
    /// </code>
    /// Would be parsed as:
    /// <code>
    /// {
    ///     "ToolSelection": "Class"
    /// }
    /// </code>
    /// </example>
    public required EnBlockType ToolSelection { get; init; }
}
