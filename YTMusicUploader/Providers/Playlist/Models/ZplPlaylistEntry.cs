using System;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class ZplPlaylistEntry : BasePlaylistEntry
    {
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public TimeSpan Duration { get; set; }
        public string TrackTitle { get; set; }
        public string TrackArtist { get; set; }
    }
}
