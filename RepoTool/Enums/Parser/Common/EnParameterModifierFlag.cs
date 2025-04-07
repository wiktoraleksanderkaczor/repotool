namespace RepoTool.Enums.Parser
{
    public enum EnParameterModifierFlag
    {
        /// <summary>
        /// Indicates that a parameter is optional
        /// </summary>
        Optional,
        
        /// <summary>
        /// Indicates that a parameter is a rest/varargs parameter
        /// </summary>
        Rest,
        
        /// <summary>
        /// Indicates that a parameter is passed by reference
        /// </summary>
        ByReference,

        /// <summary>
        /// Indicates that a parameter is passed by value
        /// </summary>
        ByValue,
        
        /// <summary>
        /// Indicates that a parameter is an output parameter
        /// </summary>
        Output,
        
        /// <summary>
        /// Indicates that a parameter has a default value
        /// </summary>
        HasDefaultValue,
        
        /// <summary>
        /// Indicates that a parameter is a keyword parameter (named parameter)
        /// </summary>
        Keyword,

        /// <summary>
        /// Indicates that a parameter is 'this' meaning acting on the current variable
        /// e.g. the instance of the class
        /// </summary>
        This
    }
}