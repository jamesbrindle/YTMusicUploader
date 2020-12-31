using System;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class PlsPlaylistEntry : BasePlaylistEntry
    {
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
        public int Nr { get; set; }
    }
}
