namespace RepoTool.Enums.Parser
{
    public enum EnVariableModifierFlag
    {
        /// <summary>
        /// Indicates that a variable is constant/final and cannot be changed after initialization
        /// </summary>
        Constant,
        
        /// <summary>
        /// Indicates that a variable is volatile (can be modified by multiple threads)
        /// </summary>
        Volatile
    }
}