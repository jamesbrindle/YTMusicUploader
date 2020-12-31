using System;
using System.Collections.Generic;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class M3uPlaylistEntry : BasePlaylistEntry
    {
        public M3uPlaylistEntry()
        {
            CustomProperties = new Dictionary<string, string>();
            Comments = new List<string>();
        }

        public TimeSpan Duration { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string AlbumArtist { get; set; }
        public Dictionary<string, string> CustomProperties { get; set; }
        public List<string> Comments { get; set; }
    }
}