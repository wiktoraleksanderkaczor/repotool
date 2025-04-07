namespace RepoTool.Enums.Parser
{
    public enum EnPropertyModifierFlag
    {        
        /// <summary>
        /// Indicates that a property is read-only
        /// </summary>
        ReadOnly,
        
        /// <summary>
        /// Indicates that a property has a custom getter
        /// </summary>
        HasCustomGetter,
        
        /// <summary>
        /// Indicates that a property has a custom setter
        /// </summary>
        HasCustomSetter,
        
        /// <summary>
        /// Indicates that a property is an auto-property
        /// i.e. compiler automatically provides the backing field
        /// </summary>
        AutoProperty,
    }
}