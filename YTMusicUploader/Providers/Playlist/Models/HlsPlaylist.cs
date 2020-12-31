using System.Collections.Generic;

namespace YTMusicUploader.Providers.Playlist.Models
{
    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public abstract class HlsPlaylist<T> : BasePlaylist<T> where T : HlsPlaylistEntry
    {
        public string Name { get; set; }
        public List<string> Comments { get; set; }
        public int Version { get; set; }
        //[Obsolete("The EXT-X-ALLOW-CACHE tag was removed in protocol version 7.")]
        public string AllowCache { get; set; }

        public HlsPlaylist()
        {
            Comments = new List<string>();
        }
    }

    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class HlsMediaPlaylist : HlsPlaylist<HlsMediaPlaylistEntry>
    {
        public int MediaSequence { get; set; }
        public int? TargetDuration { get; set; }
        public int? DiscontinuitySequence { get; set; }
        public string PlaylistType { get; set; }
        public bool EndList { get; set; }
        public bool IFramesOnly { get; set; }

        public HlsMediaPlaylist() : base() { }
    }

    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class HlsMasterPlaylist : HlsPlaylist<HlsMasterPlaylistEntry>
    {
        public List<string> Media { get; set; }
        public List<string> IFrameStreamInf { get; set; }
        public List<string> SessionData { get; set; }
        public List<string> SessionKey { get; set; }

        public HlsMasterPlaylist() : base()
        {
            Media = new List<string>();
            IFrameStreamInf = new List<string>();
            SessionData = new List<string>();
            SessionData = new List<string>();
        }
    }
}
