using System;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.DataModels
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
        public async override Task<DbOperationResult> Save()
        {
            var result = await new WatchFolderRepo().Insert(this);
            if (!result.IsError)
                Id = result.Id;

            return result;
        }

        /// <summary>
        /// Delete from the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async override Task<DbOperationResult> Delete()
        {
            var result = await new WatchFolderRepo().Delete(this);
            if (!result.IsError)
                Id = -1;

            return result;
        }
    }
}
