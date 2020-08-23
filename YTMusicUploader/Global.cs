using JBToolkit.Assemblies;
using JBToolkit.Windows;
using System.IO;
using System.Reflection;

namespace YTMusicUploader
{
    /// <summary>
    /// Globally accessible properties
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Returns application's version from Assembly
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                return VersionHelper.GetVersionShort();
            }
        }

        /// <summary>
        /// Returns user's specific AppData directory's YTMusicUploader folder
        /// </summary>
        public static string AppDataLocation
        {
            get
            {
                return Path.Combine(DirectoryHelper.GetPath(KnownFolder.LocalAppData), @"YTMusicUploader");
            }
        }

        /// <summary>
        /// Returns user's specific AppData directory's YTMusicUploader folder
        /// </summary>
        public static string EdgeFolder
        {
            get
            {
                return Path.Combine(AppDataLocation, @"84.0.522.63");
            }
        }

        /// <summary>
        /// Returns folder where application .exe resides
        /// </summary>
        public static string WorkingDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        /// Returns array of YouTube Music upload support music file type extensions
        /// </summary>
        public static string[] SupportedFiles
        {
            get
            {
                return new string[] { ".flac", ".m4a", ".mp3", ".oga", ".wma" };
            }
        }
    }
}
