namespace RepoTool.Enums.Parser
{
    public enum EnTypeModifierFlag
    {   
        /// <summary>
        /// Indicates that a class is partial (can be split across multiple files)
        /// </summary>
        Partial,
        
        /// <summary>
        /// Indicates that type allows unsafe code
        /// </summary>
        Unsafe
    }
}