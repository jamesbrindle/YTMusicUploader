using System;
using System.Threading.Tasks;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.DataModels
{
    /// <summary>
    /// Application settings data
    /// </summary>
    [Serializable]
    public class Settings : DbModels
    {
        public bool StartWithWindows { get; set; } = true;
        public int ThrottleSpeed { get; set; } = -1;
        public string AuthenticationCookie { get; set; } = null;

        /// <summary>
        /// Updates the database
        /// </summary>
        /// <returns>DbOperationResult - Showing success or fail, with messages and stats</returns>
        public async override Task<DbOperationResult> Save()
        {
            return await new SettingsRepo().Update(this);
        }

        /// <summary>
        /// Not implemented for the 'Settings' model
        /// </summary>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async override Task<DbOperationResult> Delete()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }
    }
}
