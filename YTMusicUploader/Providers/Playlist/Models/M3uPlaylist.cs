using System.Collections.Generic;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class M3uPlaylist : BasePlaylist<M3uPlaylistEntry>
    {
        public M3uPlaylist()
        {
            Comments = new List<string>();
        }

        public bool IsExtended { get; set; }
        public List<string> Comments { get; set; }
    }
}
