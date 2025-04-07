namespace RepoTool.Enums.Parser
{
    public enum EnClassModifierFlag
    {   
        /// <summary>
        /// Indicates that a class is partial (can be split across multiple files)
        /// </summary>
        Partial,
        
        /// <summary>
        /// Indicates that a class is generic
        /// </summary>
        Generic,
        
        /// <summary>
        /// Indicates that a class is an interface
        /// </summary>
        Interface,

        /// <summary>
        /// Indicates that a class is a attribute
        /// </summary>
        Attribute,
    }
}