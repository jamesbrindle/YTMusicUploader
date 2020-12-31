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
    public abstract class HlsPlaylistEntry : BasePlaylistEntry
    {
        public Dictionary<string, string> CustomProperties { get; set; }
        public List<string> Comments { get; set; }

        public HlsPlaylistEntry()
        {
            CustomProperties = new Dictionary<string, string>();
            Comments = new List<string>();
        }
    }

    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class HlsMediaPlaylistEntry : HlsPlaylistEntry
    {
        public int Duration { get; set; }
        public string Title { get; set; }
        public int MediaSequence { get; set; }
        public bool Discontinuity { get; set; }
        public string ByteRange { get; set; }
        public string Key { get; set; }
        public string Map { get; set; }
        public DateTime? ProgramDateTime { get; set; }

        public HlsMediaPlaylistEntry() : base() { }
    }

    /// <summary>
    /// Music playlist library implementation
    /// </summary>
    /// <remarks>
    /// See https://github.com/tmk907/PlaylistsNET
    /// </remarks>
    public class HlsMasterPlaylistEntry : HlsPlaylistEntry
    {
        //[Obsolete("The PROGRAM-ID attribute of the EXT-X-STREAM-INF tag was removed in protocol version 6.")]
        public int? ProgramId { get; set; }
        public int Bandwidth { get; set; }
        public int? AverageBandwidth { get; set; }
        public List<string> Codecs { get; set; }
        public string Resolution { get; set; }
        public double? FrameRate { get; set; }
        public string HdcpLevel { get; set; }
        public string Audio { get; set; }
        public string Video { get; set; }
        public string Subtitles { get; set; }
        public string ClosedCaptions { get; set; }

        public HlsMasterPlaylistEntry() : base()
        {
            Codecs = new List<string>();
        }
    }
}
