using JBToolkit.Assemblies;
using JBToolkit.Windows;
using System.IO;
using System.Reflection;

namespace YTMusicUploader
{
    public static class Global
    {
        public static string ApplicationVersion
        {
            get
            {
                return VersionHelper.GetVersionShort();
            }
        }

        public static string AppDataLocation
        {
            get
            {
                return Path.Combine(DirectoryHelper.GetPath(KnownFolder.LocalAppData), @"YTMusicUploader");
            }
        }

        public static string WorkingDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string[] SupportedFiles
        {
            get
            {
                return new string[] { ".flac", ".m4a", ".mp3", ".oga", ".wma" };
            }
        }

        public static string SupportedFilesWilcard
        {
            get
            {
                return "*.flac;*.m4a;*.mp3;*.oga;*.wma";
            }
        }
    }
}
