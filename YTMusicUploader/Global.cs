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
        private static bool? _multiThreadedRequests = null;
        private static int? _maxDegreesOfParallelism = null;
        private static int? _clearLogsAfterDays = null;
        private static int? _recheckForUploadedSongsInDays = null;
        private static string _dbLocation = null;
        private static string _edgeFolder = null;
        private static string _edgeVersion = null;
        private static string _googleVisitorId = null;
        private static string _yTBaseUrl = null;
        private static string _yTMusicUploadUrl = null;
        private static string _yTMusicParams = null;
        private static int? _yTMusic500ErrorRetryAttempts = null;
        private static int? _yTMusicIssuesMainProcessRetry = null;
        private static float? _yTUploadedSimilarityPercentageForMatch = null;
        private static string _musicBrainzBaseUrl = null;
        private static string _musicBrainzUserAgent = null;
       
     
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

                _applicationVersion = VersionHelper.GetVersionFull();
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
                catch
                {
                    _edgeVersion = "84.0.522.63";
                }

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
                catch
                {
                    _googleVisitorId = "CgtvVTcxa1EtbV9hayiMu-P0BQ%3D%3D";
                }

                return _googleVisitorId;
            }
        }

        /// <summary>
        /// Check already uploaded tracks with YouTube Music in parallel
        /// </summary>
        public static bool MultiThreadedRequests
        {
            get
            {
                if (_multiThreadedRequests != null)
                    return (bool)_multiThreadedRequests;

                try
                {
                    if (ConfigurationManager.AppSettings["MultiThreadedRequests"] != null)
                        _multiThreadedRequests = ConfigurationManager.AppSettings["MultiThreadedRequests"].ToBool();
                }
                catch
                {
                    _multiThreadedRequests = true;
                }

                return (bool)_multiThreadedRequests;
            }
        }

        /// <summary>
        /// Check already uploaded tracks with YouTube Music in parallel
        /// </summary>
        public static int MaxDegreesOfParallelism
        {
            get
            {
                if (_maxDegreesOfParallelism != null)
                    return (int)_maxDegreesOfParallelism;

                try
                {
                    if (ConfigurationManager.AppSettings["MaxDegreesOfParallelism"] != null)
                        _maxDegreesOfParallelism = ConfigurationManager.AppSettings["MaxDegreesOfParallelism"].ToInt();
                }
                catch
                {
                    _maxDegreesOfParallelism = 4;
                }

                return (int)_maxDegreesOfParallelism;
            }
        }

        /// <summary>
        /// Check already uploaded tracks with YouTube Music in parallel
        /// </summary>
        public static int ClearLogsAfterDays
        {
            get
            {
                if (_clearLogsAfterDays != null)
                    return (int)_clearLogsAfterDays;

                try
                {
                    if (ConfigurationManager.AppSettings["ClearLogsAfterNoOfDays"] != null)
                        _clearLogsAfterDays = ConfigurationManager.AppSettings["ClearLogsAfterNoOfDays"].ToInt();
                }
                catch
                {
                    _clearLogsAfterDays = 30;
                }

                return (int)_clearLogsAfterDays;
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
                catch
                {
                    _recheckForUploadedSongsInDays = 30;
                }

                return (int)_recheckForUploadedSongsInDays;
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
                catch
                {
                    _supportedFiles = new string[] { ".flac", ".m4a", ".mp3", ".oga", ".wma" };
                }

                return _supportedFiles;
            }
        }

        /// <summary>
        /// Main API URL for YouTube Music
        /// </summary>
        public static string YTMusicBaseUrl
        {
            get
            {
                if (_yTBaseUrl != null)
                    return _yTBaseUrl;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusicMusicBaseUrl"] != null)
                        _yTBaseUrl = ConfigurationManager.AppSettings["YTMusicMusicBaseUrl"];
                }
                catch
                {
                    _yTBaseUrl = "https://music.youtube.com/youtubei/v1/";
                }

                return _yTBaseUrl;
            }
        }

        /// <summary>
        /// Upload specific API URL for YouTube Music
        /// </summary>
        public static string YTMusicUploadUrl
        {
            get
            {
                if (_yTMusicUploadUrl != null)
                    return _yTMusicUploadUrl;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusicUploadUrl"] != null)
                        _yTMusicUploadUrl = ConfigurationManager.AppSettings["YTMusicUploadUrl"];
                }
                catch
                {
                    _yTMusicUploadUrl = "https://upload.youtube.com/upload/usermusic/http?authuser=0";
                }

                return _yTMusicUploadUrl;
            }
        }

        /// <summary>
        /// Main URL parameters for typical YouTube music API calls
        /// </summary>
        public static string YTMusicParams
        {
            get
            {
                if (_yTMusicParams != null)
                    return _yTMusicParams;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusicParams"] != null)
                        _yTMusicParams = ConfigurationManager.AppSettings["YTMusicParams"];
                }
                catch
                {
                    _yTMusicParams = "?alt=json&key=AIzaSyC9XL3ZjWddXya6X74dJoCTL-WEYFDNX30";
                }

                return _yTMusicParams;
            }
        }

        /// <summary>
        /// How many retry attempts to perform on a YT Music 500 error during upload
        /// </summary>
        public static int YTMusic500ErrorRetryAttempts
        {
            get
            {
                if (_yTMusic500ErrorRetryAttempts != null)
                    return (int)_yTMusic500ErrorRetryAttempts;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusic500ErrorRetryAttempts"] != null)
                        _yTMusic500ErrorRetryAttempts = ConfigurationManager.AppSettings["YTMusic500ErrorRetryAttempts"].ToInt();
                }
                catch
                {
                    _yTMusic500ErrorRetryAttempts = 4;
                }

                return (int)_yTMusic500ErrorRetryAttempts;
            }
        }

        /// <summary>
        /// How many times to repeat the main process when issues are present
        /// </summary>
        public static int YTMusicIssuesMainProcessRetry
        {
            get
            {
                if (_yTMusicIssuesMainProcessRetry != null)
                    return (int)_yTMusicIssuesMainProcessRetry;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusicIssuesMainProcessRetry"] != null)
                        _yTMusicIssuesMainProcessRetry = ConfigurationManager.AppSettings["YTMusicIssuesMainProcessRetry"].ToInt();
                }
                catch
                {
                    _yTMusicIssuesMainProcessRetry = 1;
                }

                return (int)_yTMusicIssuesMainProcessRetry;
            }
        }

        /// <summary>
        /// Levenstein similarity match success value (as float type) to match against already uploaded YouTube Music files
        /// </summary>
        public static float YTMusicUploadedSimilarityPercentageForMatch
        {
            get
            {
                if (_yTUploadedSimilarityPercentageForMatch != null)
                    return (float)_yTUploadedSimilarityPercentageForMatch;

                try
                {
                    if (ConfigurationManager.AppSettings["YTMusicUploadedSimilarityPercentageForMatch"] != null)
                        _yTUploadedSimilarityPercentageForMatch = float.Parse(ConfigurationManager.AppSettings["YTMusicUploadedSimilarityPercentageForMatch"]);
                }
                catch
                {
                    _yTUploadedSimilarityPercentageForMatch = 0.75f;
                }

                return (float)_yTUploadedSimilarityPercentageForMatch;
            }
        }      

        /// <summary>
        /// MusicBrainz API Base URL
        /// </summary>
        public static string MusicBrainzBaseUrl
        {
            get
            {
                if (_musicBrainzBaseUrl != null)
                    return _musicBrainzBaseUrl;

                try
                {
                    if (ConfigurationManager.AppSettings["MusicBrainsBaseUrl"] != null)
                        _musicBrainzBaseUrl = ConfigurationManager.AppSettings["MusicBrainsBaseUrl"];
                }
                catch
                {
                    _musicBrainzBaseUrl = "https://musicbrainz.org/ws/2/";
                }

                return _musicBrainzBaseUrl;
            }
        }


        /// <summary>
        /// Custom MusicBrainz User-Agent header
        /// </summary>
        public static string MusicBrainzUserAgent
        {
            get
            {
                if (_musicBrainzUserAgent != null)
                    return _musicBrainzUserAgent;

                try
                {
                    if (ConfigurationManager.AppSettings["MusicBrainsUserAgent"] != null)
                        _musicBrainzUserAgent =
                            ConfigurationManager.AppSettings["MusicBrainsUserAgent"]
                            + @"/" + ApplicationVersion + " ( " + "james.brindle@jb-net.co.uk" + " )";
                }
                catch
                {
                    _musicBrainzUserAgent = "YTMusicUploader"
                                + @"/" + ApplicationVersion + " ( " + "james.brindle@jb-net.co.uk" + " )";
                }

                return _musicBrainzUserAgent;
            }
        }    
    }
}
