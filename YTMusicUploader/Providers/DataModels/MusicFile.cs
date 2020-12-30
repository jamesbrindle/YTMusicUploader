using System;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.DataModels
{
    /// <summary>
    /// Music library file and status
    /// </summary>
    [Serializable]
    public class MusicFile : DbModels
    {
        /// <summary>
        /// Music library file and status
        /// </summary>
        public MusicFile()
        { }

        /// <summary>
        /// Music library file and status
        /// </summary>
        public MusicFile(string path, string hash)
        {
            Path = path;
            Hash = hash;
        }

        /// <summary>
        /// Music library file and status
        /// </summary>
        public MusicFile(string path)
        {
            Path = path;
        }

        public string Path { get; set; }
        public string MbId { get; set; }
        public string ReleaseMbId { get; set; }
        public string EntityId { get; set; }
        public string VideoId { get; set; }
        public string Hash { get; set; }
        public DateTime LastUpload { get; set; }
        public bool? Error { get; set; }
        public string ErrorReason { get; set; }
        public int? UploadAttempts { get; set; }
        public DateTime? LastUploadError { get; set; } = null;
        public bool? Removed { get; set; } = null;

        /// <summary>
        /// Insert or update the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async override Task<DbOperationResult> Save()
        {
            if (Id > 0)
            {
                return await new MusicFileRepo().Update(this);
            }
            else
            {
                var result = await new MusicFileRepo().Insert(this);
                if (!result.IsError)
                    Id = result.Id;

                return result;
            }
        }

        /// <summary>
        /// Delete from the database (for MusicFiles, it typically set the flag of 'Removed' but doesn't actually
        /// delete from the database. This is so the file (using a checksum hash) can be compared with others to
        /// identify an already uploaded file if the files music to a different path
        /// </summary>
        /// the entry from the database</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async override Task<DbOperationResult> Delete()
        {
            var result = await new MusicFileRepo().Delete(this, false);
            if (!result.IsError)
                Id = -1;

            return result;
        }

        /// <summary>
        /// Delete from the database.
        /// </summary>
        /// <param name="destroy">False (default) just sets the flag to 'remove'. True (destroy) actually removes
        /// the entry from the database</param>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async Task<DbOperationResult> Delete(bool destroy)
        {
            var result = await new MusicFileRepo().Delete(this, destroy);
            if (!result.IsError)
                Id = -1;

            return result;
        }
    }
}
