using System.Reflection;

namespace NCron
{
    internal static class ApplicationInfo
    {
        public static readonly string ApplicationName = Assembly.GetEntryAssembly().GetName().Name;
    }
}