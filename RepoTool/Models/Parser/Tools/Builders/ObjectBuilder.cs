using RepoTool.Attributes;
using RepoTool.Models.Parser.Interfaces;
using RepoTool.Models.Parser.Tools.Builders.Common;
using RepoTool.Models.Parser.Tools.Navigation;

namespace RepoTool.Models.Parser.Tools.Builders
{
    public enum EnObjectBuilderTool
    {    
        /// <summary>
        /// Represents a scroll down action.
        /// </summary>
        [ToolChoice(typeof(ScrollDown))]
        ScrollDown,

        /// <summary>
        /// Represents a page down action.
        /// </summary>
        [ItemChoice(typeof(PageDown))]
        PageDown,
        
        /// <summary>
        /// Represents a new property.
        /// </summary>
        [ItemChoice(typeof(NewProperty))]
        NewProperty,

        /// <summary>
        /// Represents the end of an object.
        /// </summary>
        [ItemChoice(typeof(EndItem))]
        EndItem
    }

    public record NewProperty
    {
        /// <summary>
        /// Indicates the name of the property.
        /// </summary>
        public required string PropertyName { get; init; }
    }

    // public enum EnObjectTerminationReason
    // {
    //     /// <summary>
    //     /// Indicates that the object has been terminated due to a parsing error.
    //     /// </summary>
    //     Mistake,

    //     /// <summary>
    //     /// Indicates that the object has been terminated due to reaching the end of the object.
    //     /// </summary>
    //     EndObject
    // }

    // public record EndObject
    // {
    //     /// <summary>
    //     /// Indicates why the end of the object has been reached.
    //     /// </summary>
    //     public required EnObjectTerminationReason TerminationReason { get; init; }
    // }


    public record ObjectBuilderSelector : IToolSelector<EnObjectBuilderTool>
    {
        public required EnObjectBuilderTool ToolSelection { get; init; }
    }
}