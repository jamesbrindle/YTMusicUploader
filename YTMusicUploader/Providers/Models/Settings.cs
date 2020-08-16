using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace YTMusicUploader.Providers.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public bool StartWithWindows { get; set; }
        public int Trottle { get; set; }
        public string AuthenticationCookie { get; set; }
    }
}
