using System.Reflection;

namespace JBToolkit.Assemblies
{
    public static class VersionHelper
    {
        public static string GetVersionShort()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[0] + "." + parts[1];
        }

        public static string GetVersionFull()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[4];
        }

        public static string GetVerionLast()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[3] + "." + parts[4];
        }
    }
}
