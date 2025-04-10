using RepoTool.Models.Parser.Items.Expressions;

namespace RepoTool.Models.Parser.Items.Common
{
    /// <summary>
    /// Defines the possible targets for an attribute.
    /// </summary>
    public enum EnAttributeTarget
    {
        /// <summary>
        /// The attribute target is an assembly.
        /// </summary>
        Assembly,
        
        /// <summary>
        /// The attribute target is a module.
        /// </summary>
        Module,
        
        /// <summary>
        /// The attribute target is a class.
        /// </summary>
        Class,
        
        /// <summary>
        /// The attribute target is a struct.
        /// </summary>
        Struct,
        
        /// <summary>
        /// The attribute target is an enum.
        /// </summary>
        Enum,
        
        /// <summary>
        /// The attribute target is a constructor.
        /// </summary>
        Constructor,
        
        /// <summary>
        /// The attribute target is a callable (method, function, etc.).
        /// </summary>
        Callable,
        
        /// <summary>
        /// The attribute target is a property.
        /// </summary>
        Property,
        
        /// <summary>
        /// The attribute target is a field.
        /// </summary>
        Field,
        
        /// <summary>
        /// The attribute target is an event.
        /// </summary>
        Event,
        
        /// <summary>
        /// The attribute target is an interface.
        /// </summary>
        Interface,
        
        /// <summary>
        /// The attribute target is a parameter.
        /// </summary>
        Parameter,
        
        /// <summary>
        /// The attribute target is a delegate.
        /// </summary>
        Delegate,
        
        /// <summary>
        /// The attribute target is a return value.
        /// </summary>
        ReturnValue,
        
        /// <summary>
        /// The attribute target is a generic parameter.
        /// </summary>
        GenericParameter,
        
    }

    /// <summary>
    /// Represents the definition of an attribute.
    /// </summary>
    public record AttributeDefinition : Construct
    {
        /// <summary>
        /// The target of the attribute.
        /// </summary>
        public required EnAttributeTarget AttributeTarget { get; init; }
        
        /// <summary>
        /// The attribute usage expression.
        /// </summary>
        public required CallableUseExpression Attribute { get; init; }
    }
}
