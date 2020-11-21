using System;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.DataModels
{
    /// <summary>
    /// Information or error log object
    /// </summary>
    [Serializable]
    public class Log : DbModels
    {
        /// <summary>
        ///  Defined the type of log - Error or info
        /// </summary>
        public enum LogTypeEnum
        {
            Info = 1,
            Error = 2,
            Warning = 3,
            Critcal = 4
        }

        public DateTime Event { get; set; }
        public LogTypeEnum LogTypeId { get; set; }
        public string Machine { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <returns></returns>
        public override Task<DbOperationResult> Delete()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds to the log to the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async override Task<DbOperationResult> Save()
        {
            var result = await new LogsRepo().Add(this);
            if (!result.IsError)
                Id = result.Id;

            return result;
        }
    }
}
