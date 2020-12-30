using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Providers.RequestModels
{
    public class PlaylistCollection : List<Playlist> { }

    public class Playlist
    {
        public enum PrivacyStatusEmum
        {
            Private,
            Public,
            Unlisted
        }

        public string BrowseId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string CoverArtUrl { get; set; }
        public string Duration { get; set; }
        public PrivacyStatusEmum? PrivacyStatus { get; set; } = null;
        public PlaylistSongCollection Songs { get; set; }
    }
}
