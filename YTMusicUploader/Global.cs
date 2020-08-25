using JBToolkit.Assemblies;
using JBToolkit.Windows;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace YTMusicUploader
{
    /// <summary>
    /// Globally accessible properties. Typically returns calculated values based on
    /// user preference or system configuration or from the application's App.config settings file
    /// </summary>
    public static class Global
    {
        private static string _applicationVersion = null;
        private static string _appDataLocation = null;
        private static string _dbLocation = null;
        private static string _edgeFolder = null;
        private static string _edgeVersion = null;
        private static string _googleVisitorId = null;
        private static string _youTubeBaseUrl = null;
        private static string _youTubeMusicUploadUrl = null;
        private static string _youTubeMusicParams = null;
        private static int? _recheckForUploadedSongsInDays = null;
        private static float? _youTubeUploadedSimilarityPercentageForMatch = null;
        private static string _workingDirectory = null;
        private static string[] _supportedFiles = null;

        /// <summary>
        /// Returns application's version from Assembly
        /// </summary>
        public static string ApplicationVersion
        {
            get
            {
                if (_applicationVersion != null)
                    return _applicationVersion;

                _applicationVersion = VersionHelper.GetVersionShort();
                return _applicationVersion;
            }
        }

        /// <summary>
        /// Returns user's specific AppData directory's YTMusicUploader folder
        /// </summary>
        public static string AppDataLocation
        {
            get
            {
                if (_appDataLocation != null)
                    return _appDataLocation;

                _appDataLocation = Path.Combine(DirectoryHelper.GetPath(KnownFolder.LocalAppData), @"YTMusicUploader");
                return _appDataLocation;
            }
        }

        /// <summary>
        /// Return the database file location in the users AppData path
        /// </summary>
        public static string DbLocation
        {
            get
            {
                if (_dbLocation != null)
                    return _dbLocation;

                _dbLocation = Path.Combine(AppDataLocation, @"ytuploader.db");
                return _dbLocation;
            }
        }

        /// <summary>
        /// Returns user's specific AppData directory's YTMusicUploader folder
        /// </summary>
        public static string EdgeFolder
        {
            get
            {
                if (_edgeFolder != null)
                    return _edgeFolder;

                _edgeFolder = Path.Combine(AppDataLocation, EdgeVersion);
                return _edgeFolder;
            }
        }

        /// <summary>
        /// Returns the version of Edge we're currently using for the WebView2 control in this application
        /// </summary>
        public static string EdgeVersion
        {
            get
            {
                if (_edgeVersion != null)
                    return _edgeVersion;

                try
                {
                    if (ConfigurationManager.AppSettings["EdgeVersion"] != null)
                        _edgeVersion = ConfigurationManager.AppSettings["EdgeVersion"];
                }
                catch { }

                _edgeVersion = "84.0.522.63";
                return _edgeVersion;
            }
        }

        /// <summary>
        /// Returns the Google visitor ID for YouTube Music request headers
        /// </summary>
        public static string GoogleVisitorId
        {
            get
            {
                if (_googleVisitorId != null)
                    return _googleVisitorId;

                try
                {
                    if (ConfigurationManager.AppSettings["GoogleVisitorId"] != null)
                        _googleVisitorId = ConfigurationManager.AppSettings["GoogleVisitorId"];
                }
                catch { }

                _googleVisitorId = "CgtvVTcxa1EtbV9hayiMu-P0BQ%3D%3D";
                return _googleVisitorId;
            }
        }

        /// <summary>
        /// Main API URL for YouTube Music
        /// </summary>
        public static string YouTubeBaseUrl
        {
            get
            {
                if (_youTubeBaseUrl != null)
                    return _youTubeBaseUrl;

                try
                {
                    if (ConfigurationManager.AppSettings["YouTubeMusicBaseUrl"] != null)
                        _youTubeBaseUrl = ConfigurationManager.AppSettings["YouTubeMusicBaseUrl"];
                }
                catch { }

                _youTubeBaseUrl = "https://music.youtube.com/youtubei/v1/";
                return _youTubeBaseUrl;
            }
        }

        /// <summary>
        /// Upload specific API URL for YouTube Music
        /// </summary>
        public static string YouTubeMusicUploadUrl
        {
            get
            {
                if (_youTubeMusicUploadUrl != null)
                    return _youTubeMusicUploadUrl;

                try
                {
                    if (ConfigurationManager.AppSettings["YouTubeMusicUploadUrl"] != null)
                        _youTubeMusicUploadUrl = ConfigurationManager.AppSettings["YouTubeMusicUploadUrl"];
                }
                catch { }


                _youTubeMusicUploadUrl = "https://upload.youtube.com/upload/usermusic/http?authuser=0";
                return _youTubeMusicUploadUrl;
            }
        }

        /// <summary>
        /// Main URL parameters for typical YouTube music API calls
        /// </summary>
        public static string YouTubeMusicParams
        {
            get
            {
                if (_youTubeMusicParams != null)
                    return _youTubeMusicParams;

                try
                {
                    if (ConfigurationManager.AppSettings["YouTubeMusicParams"] != null)
                        _youTubeMusicParams = ConfigurationManager.AppSettings["YouTubeMusicParams"];
                }
                catch { }

                _youTubeMusicParams = "?alt=json&key=AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30";
                return _youTubeMusicParams;
            }
        }

        /// <summary>
        /// How many days delay before performing a recheck of the local music library against
        /// what's already uploaded to YouTube Music
        /// </summary>
        public static int RecheckForUploadedSongsInDays
        {
            get
            {
                if (_recheckForUploadedSongsInDays != null)
                    return (int)_recheckForUploadedSongsInDays;

                try
                {
                    if (ConfigurationManager.AppSettings["RecheckForUploadedSongsInDays"] != null)
                        _recheckForUploadedSongsInDays = ConfigurationManager.AppSettings["RecheckForUploadedSongsInDays"].ToInt();
                }
                catch { }

                _recheckForUploadedSongsInDays = 30;
                return (int)_recheckForUploadedSongsInDays;
            }
        }

        /// <summary>
        /// Levenstein similarity match success value (as float type) to match against already uploaded YouTube Music files
        /// </summary>
        public static float YouTubeUploadedSimilarityPercentageForMatch
        {
            get
            {
                if (_youTubeUploadedSimilarityPercentageForMatch != null)
                    return (float)_youTubeUploadedSimilarityPercentageForMatch;

                try
                {
                    if (ConfigurationManager.AppSettings["YouTubeUploadedSimilarityPercentageForMatch"] != null)
                        _youTubeUploadedSimilarityPercentageForMatch = float.Parse(ConfigurationManager.AppSettings["YouTubeUploadedSimilarityPercentageForMatch"]);
                }
                catch { }

                _youTubeUploadedSimilarityPercentageForMatch = 0.75f;
                return (float)_youTubeUploadedSimilarityPercentageForMatch;
            }
        }

        /// <summary>
        /// MusicBrainz Request cache Location
        /// </summary>
        public static string CacheLocation
        {
            get
            {
                string cacheDir = Path.Combine(AppDataLocation, @"Cache");
                if (Directory.Exists(cacheDir))
                    Directory.CreateDirectory(cacheDir);

                return cacheDir;
            }
        }

        /// <summary>
        /// Returns the folder location where application's .exe resides 
        /// (Tyically in the Program Files or Program Files x86 folder)
        /// </summary>
        public static string WorkingDirectory
        {
            get
            {
                if (_workingDirectory != null)
                    return _workingDirectory;

                _workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return _workingDirectory;
            }
        }

        /// <summary>
        /// Returns an array of YouTube Music upload support music file type extensions
        /// </summary>
        public static string[] SupportedFiles
        {
            get
            {
                if (_supportedFiles != null)
                    return _supportedFiles;
                try
                {
                    if (ConfigurationManager.AppSettings["GoogleVisitorId"] != null)
                        _supportedFiles = ConfigurationManager.AppSettings["SupportedFileTypes"].Split(';');
                }
                catch { }

                _supportedFiles = new string[] { ".flac", ".m4a", ".mp3", ".oga", ".wma" };
                return _supportedFiles;
            }
        }
    }
}
