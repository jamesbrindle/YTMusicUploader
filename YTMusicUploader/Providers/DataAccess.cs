using Dapper;
using JBToolkit.Network;
using JBToolkit.Threads;
using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// Abastract database repository access class.
    /// </summary>
    public abstract class DataAccess
    {
        /// <summary>
        /// Checks if the database file is present in the users AppData path. If it's not 
        /// present it will copy over the template database file from the Program Files (or working directory) AppData folder
        /// </summary>
        public static void CheckAndCopyDatabaseFile()
        {
            if (!File.Exists(Global.DbLocation))
            {
                if (!Directory.Exists(Global.AppDataLocation))
                    Directory.CreateDirectory(Global.AppDataLocation);

                File.Copy(Path.Combine(Global.WorkingDirectory, @"AppData\ytuploader.db"), Global.DbLocation);
            }
            else
            {
                if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm")))
                {
                    try
                    {
                        File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm"));
                    }
                    catch { }
                }

                if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal")))
                {
                    try
                    {
                        File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal"));
                    }
                    catch { }
                }

                PerformAnyDbUpgrades();
            }

            CheckDatabaseIntegrity();
        }

        /// <summary>
        /// Delete the app data database (user's database)
        /// </summary>
        public static void ResetDatabase()
        {
            if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm")))
            {
                try
                {
                    File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-shm"));
                }
                catch { }
            }

            if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal")))
            {
                try
                {
                    File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db-wal"));
                }
                catch { }
            }

            if (File.Exists(Path.Combine(Global.AppDataLocation, "ytuploader.db")))
            {
                try
                {
                    File.Delete(Path.Combine(Global.AppDataLocation, "ytuploader.db"));
                }
                catch { }
            }

            if (!File.Exists(Global.DbLocation))
            {
                if (!Directory.Exists(Global.AppDataLocation))
                    Directory.CreateDirectory(Global.AppDataLocation);

                File.Copy(Path.Combine(Global.WorkingDirectory, @"AppData\ytuploader.db"), Global.DbLocation);
            }

            PerformAnyDbUpgrades();
        }

        /// <summary>
        /// Runs on form load to ensure the database schema is at the latest version following an application upgrade
        /// </summary>
        public static void PerformAnyDbUpgrades()
        {
            using (var conn = DbConnection())
            {
                conn.Open();

                try
                {
                    //
                    // Added Mbid Column to MusicFiles Table in v1.2
                    //

                    var columns = conn.Query<string>(
                            @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                    if (!columns.Contains("MbId"))
                    {
                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN MbId TEXT");
                    }

                    //
                    // Added ReleaseMbId Column to MusicFiles Table in v1.3.6
                    //

                    columns = conn.Query<string>(
                            @"SELECT name 
                             FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                    if (!columns.Contains("ReleaseMbId"))
                    {
                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN ReleaseMbId TEXT");
                    }

                    //
                    // Added EntityId Column to MusicFiles Table in v1.3.6
                    //

                    columns = conn.Query<string>(
                            @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                    if (!columns.Contains("EntityId"))
                    {
                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN EntityId TEXT");
                    }

                    //
                    // Added Logs Table in 1.4.9
                    // 

                    string result = conn.Query<string>(
                                    @"SELECT name FROM sqlite_master WHERE type='table' AND name='Logs';")
                                        .ToList()
                                        .FirstOrDefault();

                    if (string.IsNullOrEmpty(result))
                    {
                        conn.Execute(
                            @"CREATE TABLE ""LogType"" (
	                        ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                        ""Description""	TEXT NOT NULL
                        );");

                        conn.Execute(
                            @"INSERT INTO LogType (Id, Description) VALUES
                              (1, 'Info'),
                              (2, 'Error')");

                        conn.Execute(
                            @"CREATE TABLE ""Logs"" (
	                            ""Id""	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	                            ""Event""	TEXT NOT NULL,
	                            ""LogTypeId""	INTEGER NOT NULL,
	                            ""Machine""	TEXT NOT NULL,
	                            ""Source""	TEXT NOT NULL,
	                            ""Message""	TEXT NOT NULL,
	                            ""StackTrace""	TEXT,
	                            FOREIGN KEY(""LogTypeId"") REFERENCES ""LogType""(""Id"")
                        );");
                    }

                    columns = conn.Query<string>(
                            @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('Settings')").ToList();

                    if (!columns.Contains("SendLogsToSource"))
                    {
                        conn.Execute(
                            @"ALTER TABLE Settings
                              ADD SendLogsToSource INTEGER DEFAULT 1");

                    }

                    //
                    // Added additional logs types in 1.5.1
                    // 

                    result = conn.Query<string>(
                                @"SELECT Description
                                  FROM LogType
                                  WHERE Description = 'Warning'")
                                .ToList()
                                .FirstOrDefault();

                    if (result == null)
                    {
                        conn.Execute(
                           @"INSERT INTO LogType (Description)
                             VALUES ('Warning'),
                                    ('Critical')");
                    }

                    //
                    // Added version to log in 1.5.2
                    // 

                    columns = conn.Query<string>(
                            @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('Logs')").ToList();

                    if (!columns.Contains("Version"))
                    {
                        conn.Execute(
                            @"ALTER TABLE Logs
                              ADD COLUMN Version TEXT");
                    }

                    //
                    // Added failure attempts columns to MusicFile in 1.5.2
                    // 

                    columns = conn.Query<string>(
                           @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                    if (!columns.Contains("UploadAttempts"))
                    {
                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN UploadAttempts INTEGER");

                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN LastUploadError TEXT ");

                        conn.Execute(
                           @"UPDATE MusicFiles
                             SET LastUploadError = '0001-01-01 00:00:00'");
                    }

                    //
                    // Corrected column 'Version' data type in 1.5.4 - Should be text
                    //

                    string versionDataType = conn.Query<string>(
                            "SELECT TYPEOF(Version) FROM Logs LIMIT 1").ToList()
                                                                       .FirstOrDefault();

                    if (versionDataType != null && versionDataType.ToLower() != "text")
                    {
                        conn.Execute(
                          @"DROP TABLE ""Logs"";
                            CREATE TABLE ""Logs"" (
                                ""Id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                                ""Event"" TEXT NOT NULL,
                                ""LogTypeId"" INTEGER NOT NULL,
                                ""Machine""   TEXT NOT NULL,
                                ""Source""    TEXT NOT NULL,
                                ""Message""   TEXT NOT NULL,
                                ""Version""   TEXT,
                                ""StackTrace""    TEXT     
                            ); ");
                    }

                    //
                    // Version 1.6.0 - Added song 'VideoId' column
                    //               - Added 'PlaylistFiles' table
                    //

                    columns = conn.Query<string>(
                            @"SELECT name 
                              FROM PRAGMA_TABLE_INFO('MusicFiles')").ToList();

                    if (!columns.Contains("VideoId"))
                    {
                        conn.Execute(
                            @"ALTER TABLE MusicFiles
                              ADD COLUMN VideoId TEXT");
                    }

                    bool playlistTableExists = conn.Query<string>(
                        @"SELECT name 
                          FROM sqlite_master WHERE type='table' AND name='PlaylistFiles';").ToList().Count > 0;

                    if (!playlistTableExists)
                    {
                        conn.Execute(
                            @"CREATE TABLE ""PlaylistFiles"" (
                                ""Id""    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                ""Title"" TEXT,
                                ""Description""   TEXT,
                                ""PlaylistId""    TEXT,
                                ""Path""  TEXT NOT NULL,
                                ""LastModifiedDate""  TEXT NOT NULL,
                                ""LastUpload""    TEXT
                            ); ");
                    }
                }
                catch { }

                conn.Close();
            }
        }

        public static void CheckDatabaseIntegrity()
        {
            try
            {
                Logger.LogInfo("CheckDatabaseIntegrity", "Checking database integrity");

                var _s = new SettingsRepo().Load();
                var _w = new WatchFolderRepo().Load();
                var _m = new MusicFileRepo().LoadAll(true);
                var _l = new LogsRepo().LoadSpecific("3");

                Logger.LogInfo("CheckDatabaseIntegrity", "Database integrity check complete - No issues");
            }
            catch (Exception e)
            {
                if (
                    MessageBox.Show(
                        $"Unfortunately the database integrity check has failed ({e.Message}). YT Music Uploader cannot continue in this state. " +
                        $"If you click 'OK', YT Music Uploader will reset the database to its original state. You'll lose any uploaded file states but the program" +
                        $" should then work. Otherwise click cancel to attempt to rectify the database yourself located in: %localappdata%\\YTMusicUploader",
                        "Database Integrity Check Fail",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Error) == MessageBoxResult.OK)
                {
                    ResetDatabase();
                    Logger.LogInfo("CheckDatabaseIntegrity", "Database has been reset due to integrity check failure. Comfirmed by user.");
                }
            }
        }

        /// <summary>
        /// Create an SQLite connection to the database file in the users AppData path
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection DbConnection(bool readOnly = false)
        {
            return new SQLiteConnection("Data Source=" + Global.DbLocation + ";cache=shared" + (readOnly ? ";Read Only=True" : ""));
        }

        internal static MySqlConnection RemoteDbConnection()
        {
            return new MySqlConnection(NetworkHelper.ConnectionString("cLx81TqecjVCRGCPV0fmTi99LTsUqYorQR7JiG9RYvqiqic2bT0S3RBh3CyzDt17MVKPaeMs/HOEZvZObCVjy5NN9krbKP2X/SHHuHTMEOBFxunNaGpTvaZl1Fv54uo6PfkaYeQKPUSnorKNxPVQsZAyfr5imIHyj1qP2pW86V6bueaviRkxMRW72G6t3ZNMIgkOZb+0drCq4xphSrz0H8ctMDq304suE/IFFHQ1M1mBPsfi38Zwjwwy3aA+40mf"));
        }
    }

    /// <summary>
    /// Database process execution and query result object. Contains if there's any errors and the time taken 
    /// to perform the database operation.
    /// </summary>
    public class DbOperationResult
    {
        /// <summary>
        /// Database process execution and query result object for 'success'. Contains the time taken 
        /// to perform the database operation along with the scoped database entry ID
        /// </summary>
        public static DbOperationResult Success(int id, TimeSpan executionTime)
        {
            return new DbOperationResult
            {
                Id = id,
                IsError = false,
                ErrorReason = string.Empty,
                ExecutionTime = GetElapsedTime(executionTime)
            };
        }

        /// <summary>
        /// Database process execution and query result object for 'failure'. Contains the error reason and 
        /// the time taken to perform the database operation
        /// </summary>
        public static DbOperationResult Fail(string errorReason, TimeSpan executionTime)
        {
            return new DbOperationResult
            {
                Id = -1,
                IsError = false,
                ErrorReason = errorReason,
                ExecutionTime = GetElapsedTime(executionTime)
            };
        }

        /// <summary>
        /// Scoped datbase entry ID of the record inserted or updated
        /// </summary>
        public int Id { get; set; } = -1;
        public bool IsError { get; set; } = false;
        public string ErrorReason { get; set; }
        public string ExecutionTime { get; set; } = "0";

        private static string GetElapsedTime(TimeSpan timespan)
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                 timespan.Hours,
                                 timespan.Minutes,
                                 timespan.Seconds,
                                 timespan.Milliseconds / 10);
        }
    }
}
