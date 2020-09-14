using System;
using System.Collections.Generic;
using System.Linq;

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
        {
            /// <summary>
            /// Remove a single song from the cache following a YT Music deletion
            /// </summary>
            public void RemoveSong(string artistBrowseId, string songEntityId)
            {
                var artist = this.Where(a => a.BrowseId == artistBrowseId).FirstOrDefault();

                foreach (var album in artist.AlbumSongCollection.Albums)
                {
                    if (album.Songs.Where(s => s.EntityId == songEntityId).Any())
                        album.Songs.Remove(
                                        album.Songs.Where(s => s.EntityId == songEntityId)
                                                   .FirstOrDefault());
                }

                for (int i = artist.AlbumSongCollection.Albums.Count - 1; i >= 0; i--)
                {
                    if (artist.AlbumSongCollection.Albums[i].Songs.Count == 0)
                        artist.AlbumSongCollection.Albums.Remove(artist.AlbumSongCollection.Albums[i]);
                }

                if (artist.AlbumSongCollection.Songs.Where(s => s.EntityId == songEntityId).Any())
                    artist.AlbumSongCollection.Songs.Remove(
                                                        artist.AlbumSongCollection.Songs.Where(s => s.EntityId == songEntityId)
                                                                                        .FirstOrDefault());

                if (artist.AlbumSongCollection.Songs.Count == 0)
                    this.Remove(artist);
            }

            /// <summary>
            /// Remove an album from the cache following a YT Music deletion
            /// </summary>
            public void RemoveAlbum(string artistBrowseId, string albumEntityId)
            {
                var songsToRemove = new List<Song>();

                var artist = this.Where(a => a.BrowseId == artistBrowseId).FirstOrDefault();
                if (artist != null && artist.AlbumSongCollection.Albums != null && artist.AlbumSongCollection.Albums.Count > 0)
                {
                    var album = artist.AlbumSongCollection.Albums.Where(a => a.EntityId == albumEntityId).FirstOrDefault();
                    if (album != null)
                    {
                        for (int i = album.Songs.Count - 1; i >= 0; i--)
                        {
                            songsToRemove.Add(album.Songs[i]);
                            album.Songs.Remove(album.Songs[i]);
                        }
                    }
                }

                foreach (var song in songsToRemove)
                {
                    if (artist.AlbumSongCollection.Songs.Where(s => s.EntityId == song.EntityId).Any())
                    {
                        artist.AlbumSongCollection.Songs.Remove(
                                                            artist.AlbumSongCollection.Songs.Where(s => s.EntityId == song.EntityId)
                                                                                            .FirstOrDefault());
                    }
                }

                if (artist.AlbumSongCollection.Albums.Where(a => a.EntityId == albumEntityId).Any())
                {
                    artist.AlbumSongCollection.Albums.Remove(
                                                        artist.AlbumSongCollection.Albums.Where(a => a.EntityId == albumEntityId)
                                                                                         .FirstOrDefault());
                }

                if (artist.AlbumSongCollection.Albums.Count == 0)
                    this.Remove(artist);
            }
        }

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
            public string EntityId { get; set; }
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
