using System;
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
        public override DbOperationResult Save()
        {
            return new SettingsRepo().Update(this);
        }

        /// <summary>
        /// Not implemented for the 'Settings' model
        /// </summary>
        public override DbOperationResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
