using System;
using System.Collections.Generic;

namespace YTMusicUploader.Providers.RequestModels
{
    /// <summary>
    /// Used to store temporary artist details from YouTube Music to be used for lookups
    /// </summary>
    public class ArtistCache
    {
        public DateTime LastSetTime { get; set; } = DateTime.Now;
        public List<Artist> Artists { get; set; } = new List<Artist>();

        public class Artist
        {
            public string ArtistName { get; set; }
            public string BrowseId { get; set; }
            public List<Song> Songs { get; set; } = new List<Song>();
        }

        public class Song
        {
            public string EntityId { get; set; }
            public string Title { get; set; }
        }
    }
}
