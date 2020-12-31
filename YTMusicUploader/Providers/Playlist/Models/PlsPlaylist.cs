namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class PlsPlaylist : BasePlaylist<PlsPlaylistEntry>
    {
        public PlsPlaylist()
        {
            Version = 2;
        }
        public int Version { get; set; }
        public int NumberOfEntries { get { return PlaylistEntries.Count; } }
    }
}
