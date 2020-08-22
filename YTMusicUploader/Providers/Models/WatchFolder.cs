using System;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.Models
{
    /// <summary>
    /// Selected libraries to scan and watch data
    /// </summary>
    [Serializable]
    public class WatchFolder : DbModels
    {
        public string Path { get; set; }

        /// <summary>
        /// Insert into the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public override DbOperationResult Save()
        {
            var result = new WatchFolderRepo().Insert(this);
            if (!result.IsError)
                Id = result.Id;

            return result;
        }

        /// <summary>
        /// Delete from the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public override DbOperationResult Delete()
        {
            var result = new WatchFolderRepo().Delete(this);
            if (!result.IsError)
                Id = -1;

            return result;
        }
    }
}
