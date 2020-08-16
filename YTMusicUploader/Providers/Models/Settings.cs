using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Providers.Models
{
    [Serializable]
    public class Settings
    {
        public int Id { get; set; } = 1;
        public bool StartWithWindows { get; set; } = true;
        public int ThrottleSpeed { get; set; } = -1;
        public string AuthenticationCookie { get; set; } = null;

        public DbOperationResult Save()
        {
            return new SettingsRepo().Update(this);
        }
    }
}
