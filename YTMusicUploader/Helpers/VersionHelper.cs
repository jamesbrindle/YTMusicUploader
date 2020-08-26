using System.Reflection;

namespace JBToolkit.Assemblies
{
    /// <summary>
    /// Methods for retreiving version information from the current assembly
    /// </summary>
    public static class VersionHelper
    {
        /// <summary>
        /// Major and minor
        /// </summary>
        /// <returns>I.e. 1.1</returns>
        public static string GetVersionShort()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[0] + "." + parts[1];
        }

        /// <summary>
        /// Major, minor, build int, build date
        /// </summary>
        /// <returns>i.e. 1.1.14.234234</returns>
        public static string GetVersionFull()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[0] + "." + parts[1] + "." + parts[2];
        }

        /// <summary>
        /// Custom - If only wanting build info
        /// </summary>
        /// <returns>i.e 14.234234</returns>
        public static string GetVerionLast()
        {
            string[] parts = Assembly.GetCallingAssembly().GetName().Version.ToString().Split('.');
            return parts[3] + "." + parts[4];
        }
    }
}
