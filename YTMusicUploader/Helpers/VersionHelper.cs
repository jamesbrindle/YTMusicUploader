using Octokit;
using System;
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

        /// <summary>
        /// Get's the latest version from GitHub
        /// </summary>
        /// <param name="htmlUrl">URL to the latest version on GitHub</param>
        /// <returns>Version tag</returns>
        public static string GetVersionLatest(out string htmlUrl)
        {
            htmlUrl = null;

            try
            {
                var client = new GitHubClient(new ProductHeaderValue("YTMusicUploader"));
                var releases = client.Repository.Release.GetAll("jamesbrindle", "YTMusicUploader").Result;
                var latest = releases[0];
                htmlUrl = latest.HtmlUrl;

                return latest.TagName.Replace("v", "");
            }
            catch
            {
                return GetVersionFull();
            }
        }

        /// <summary>
        /// Determines if version on GitHub is later than the current version
        /// </summary>
        /// <param name="htmlUrl">URL to the latest version on GitHub<</param>
        /// <returns>True if the version on  GitHub is later than the current version, false otherwise</returns>
        public static bool LatestVersionGreaterThanCurrentVersion(out string htmlUrl, out string latestVersionTag)
        {
            htmlUrl = null;
            latestVersionTag = null;

            try
            {
                latestVersionTag = GetVersionLatest(out htmlUrl);
                var currentVersion = new Version(GetVersionFull());
                var latestVersion = new Version(latestVersionTag);

                if (latestVersion.CompareTo(currentVersion) > 0)
                    return true;
            }
            catch { }

            return false;
        }
    }
}
