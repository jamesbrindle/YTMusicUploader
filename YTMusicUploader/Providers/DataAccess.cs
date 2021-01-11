using JBToolkit.Network;
using MySql.Data.MySqlClient;
using System;
using System.Data.SQLite;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// Abastract database repository access class.
    /// </summary>
    public abstract class DataAccess
    {
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
