using System.Reflection;

namespace RepoTool.Constants
{
    public static class TypeConstants
    {
        // TODO: Possibly add BindingFlags.FlattenHierarchy
        public static readonly BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
    }
}