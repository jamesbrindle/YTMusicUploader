using Dapper;
using System;
using System.Data.SQLite;
using System.IO;
using System.Linq;

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
            }
        }

        /// <summary>
        /// Create an SQLite connection to the database file in the users AppData path
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection DbConnection()
        {
            return new SQLiteConnection("Data Source=" + Global.DbLocation);
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
