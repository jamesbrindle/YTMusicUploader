using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YTMusicUploader.Providers
{
    public abstract class DataAccess
    {
        public static string DBLocation
        {
            get
            {
                return Path.Combine(Global.AppDataLocation, @"ytuploader.db");
            }
        }

        public static void CheckAndCopyDatabaseFile()
        {
            if (!File.Exists(DBLocation))
            {
                if (!Directory.Exists(Global.AppDataLocation))
                    Directory.CreateDirectory(Global.AppDataLocation);

                File.Copy(Path.Combine(Global.WorkingDirectory, @"AppData\ytuploader.db"), DBLocation);
            }
            else
            {
                if (File.Exists("ytuploader.db-shm"))
                {
                    try
                    {
                        File.Delete("ytuploader.db-shm");
                    }
                    catch { }
                }

                if (File.Exists("ytuploader.db-wal"))
                {
                    try
                    {
                        File.Delete("ytuploader.db-wal");
                    }
                    catch { }
                }
            }
        }

        public static SQLiteConnection DbConnection()
        {
            return new SQLiteConnection("Data Source=" + DBLocation);
        }
    }

    public class DbOperationResult
    {
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

        public int Id { get; set; } = -1;
        public bool IsError { get; set; } = false;        
        public string ErrorReason { get; set; }
        public string ExecutionTime { get; set; } = "0";

        private static string GetElapsedTime(TimeSpan timespan)
        {
           return string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                timespan.Hours, timespan.Minutes, timespan.Seconds,
                timespan.Milliseconds / 10);
        }
    }
}
