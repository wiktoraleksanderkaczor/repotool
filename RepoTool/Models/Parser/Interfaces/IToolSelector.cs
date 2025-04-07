public interface IToolSelector<TEnum> where TEnum : struct
{
    TEnum ToolSelection { get; init; }
}