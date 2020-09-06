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
        public ArtistCollection Artists { get; set; } = new ArtistCollection();

        public class ArtistCollection : List<Artist>
        { }

        public class Artist
        {
            public string ArtistName { get; set; }
            public string BrowseId { get; set; }
            public AlbumSongCollection AlbumSongCollection { get; set; } = new AlbumSongCollection();
        }

        public class AlbumSongCollection
        {
            public AlbumCollection Albums { get; set; } = new AlbumCollection();
            public SongCollection Songs { get; set; } = new SongCollection();
        }

        public class AlbumCollection : List<Alumb>
        {
            public HashSet<string> AlbumHashSet { get; set; } = new HashSet<string>();
        }

        public class Alumb
        {
            public string Title { get; set; }
            public string CoverArtUrl { get; set; }
            public SongCollection Songs { get; set; }
        }

        public class SongCollection : List<Song>
        { }

        public class Song
        {
            public string EntityId { get; set; }
            public string Title { get; set; }
            public string CoverArtUrl { get; set; }
            public string Duration { get; set; }
        }
    }
}
