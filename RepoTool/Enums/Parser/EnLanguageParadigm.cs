namespace RepoTool.Enums.Parser
{
    /// <summary>
    /// Represents the programming paradigm of a language.
    /// </summary>
    [Flags]
    public enum EnLanguageParadigm
    {
        /// <summary>
        /// Object-oriented programming paradigm.
        /// </summary>
        ObjectOriented,

        /// <summary>
        /// Functional programming paradigm.
        /// </summary>
        Functional,

        /// <summary>
        /// Procedural programming paradigm.
        /// </summary>
        Procedural,
        
        /// <summary>
        /// Declarative programming paradigm.
        /// </summary>
        Declarative,

        /// <summary>
        /// Logic programming paradigm.
        /// </summary>
        Logic,

        /// <summary>
        /// Concurrent programming paradigm.
        /// </summary>
        Concurrent,
        
        /// <summary>
        /// Symbolic programming paradigm
        /// </summary>
        Symbolic,

        /// <summary>
        /// Represents a data paradigm.
        /// e.g. JSON, YAML etc.
        /// </summary>
        Data,

        /// <summary>
        /// Other less common paradigms.
        /// </summary>
        Other
    }
}