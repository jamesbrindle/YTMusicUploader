using System;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class ZplPlaylist : BasePlaylist<ZplPlaylistEntry>
    {
        public string Author { get; set; }
        public string Generator { get; set; }
        public string Guid { get; set; }
        public int ItemCount { get; set; }
        public string Title { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}
