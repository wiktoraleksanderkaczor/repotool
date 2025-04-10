namespace RepoTool.Enums.Parser.Items.Directives
{
    /// <summary>
    /// Represents symbol definition directives (#define, #undef)
    /// </summary>
    public enum EnDefinitionType
    {
        /// <summary>
        /// Represents a #define directive.
        /// </summary>
        Define,
        
        /// <summary>
        /// Represents an #undef directive.
        /// </summary>
        Undefine
    }
}