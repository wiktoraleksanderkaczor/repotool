namespace RepoTool.Models.Parser.Tools.Builders.Common
{
    public enum EnItemTerminationReason
    {
        /// <summary>
        /// Indicates that the iterable has been terminated due to a parsing error.
        /// </summary>
        Mistake,

        /// <summary>
        /// Indicates that the iterable has been terminated due to reaching the end of the iterable.
        /// </summary>
        Finished
    }

    public record EndItem
    {
        /// <summary>
        /// Indicates why the end of the item has been reached.
        /// </summary>
        public required EnItemTerminationReason TerminationReason { get; init; }
    }
}