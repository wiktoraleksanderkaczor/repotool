namespace RepoTool.Enums.Parser.Items.Statements
{
    /// <summary>
    /// Defines the possible types of control flow statements.
    /// </summary>
    public enum EnControlFlowStatementType
    {
        /// <summary>
        /// Represents a break statement.
        /// </summary>
        Break,

        /// <summary>
        /// Represents a continue statement.
        /// </summary>
        Continue,

        /// <summary>
        /// Represents a goto statement.
        /// </summary>
        Goto,

        /// <summary>
        /// Represents a return statement.
        /// </summary>
        Return,

        /// <summary>
        /// Represents a yield statement.
        /// </summary>
        Yield,

        /// <summary>
        /// Represents a throw statement.
        /// </summary>
        Throw
    }
}