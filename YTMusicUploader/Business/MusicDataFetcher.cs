using Hqub.MusicBrainz.API;
using Hqub.MusicBrainz.API.Entities;
using JBToolkit.Synchronicity;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using YTMusicUploader.Business.MusicBrainz;

namespace YTMusicUploader.Business
{
    /// <summary>
    /// Gets meta data / tag information from the music file itself and looks up any missing data
    /// using a MusicBrainz API implementation (for instance album cover art, in which it will lookup
    /// CoverArtArchive: https://coverartarchive.org/
    /// 
    /// Thanks to: avatar29A
    ///     https://github.com/avatar29A/MusicBrainz
    ///    
    /// </summary>
    public class MusicDataFetcher
    {
        /// <summary>
        /// .Net implementation client of MusicBrainz API
        /// </summary>
        public MusicBrainzClient MusicBrainzClient { get; set; }

        /// <summary>
        /// Gets meta data / tag information from the music file itself and looks up any missing data
        /// using a MusicBrainz API implementation (for instance album cover art).
        /// </summary>
        public MusicDataFetcher()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            MusicBrainzClient = new MusicBrainzClient()
            {
                Cache = new FileRequestCache(Global.CacheLocation)
            };
        }

        /// <summary>
        /// First looks at the file meta data for the track MBID then makes a request to MusicBrainz if it's not found
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>MusicBrainz ID</returns>
        public string GetTrackMbId(string path)
        {
            try
            {
                // Get tags from file
                var tags = GetMusicTagLibFile(path).Tag;

                if (!string.IsNullOrEmpty(tags.MusicBrainzTrackId) &&
                    !tags.MusicBrainzReleaseId.ToLower().Contains("usicbrainz"))
                {
                    return tags.MusicBrainzTrackId;
                }

                var result = GetRecordingFromMusicBrainzWithAlbumNameVariations(
                                        tags.FirstAlbumArtist,
                                        tags.Album,
                                        tags.Title);
                if (result != null)
                    return result.Id;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// First looks at the file meta data for the release MBID, then makes a request to MusicBrainz if it's not found
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>MusicBrainz ID</returns>
        public string GetReleaseMbId(string path)
        {
            try
            {
                // Get tags from file
                var tags = GetMusicTagLibFile(path).Tag;

                if (!string.IsNullOrEmpty(tags.MusicBrainzReleaseId) &&
                    !tags.MusicBrainzReleaseId.ToLower().Contains("usicbrainz"))
                {
                    return tags.MusicBrainzReleaseId;
                }

                var result = GetReleaseFromMusicBrainzWithAlbumNameVariations(
                                        tags.FirstPerformer ?? tags.FirstAlbumArtist ?? "",
                                        tags.Album);
                if (result != null)
                    return result.Id;
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via query using the artist, album an track names.
        /// This method also tries different variations of the album name
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainzWithAlbumNameVariations(string artist, string album, string track)
        {
            var recording = GetRecordingFromMusicBrainz(artist, album, track);

            if (recording != null)
                return recording;

            album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
            recording = GetRecordingFromMusicBrainz(artist, album, track);

            if (recording != null)
                return recording;

            album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
            recording = GetRecordingFromMusicBrainz(artist, album, track);

            if (recording != null)
                return recording;

            return null;
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via query using the artist, album an track names
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainz(string artist, string album, string track)
        {
            try
            {
                var query = new QueryParameters<Recording>()
                {
                    { "artist", artist },
                    { "release", album },
                    { "recording", track }
                };

                var recordings = AsyncHelper.RunSync(() => MusicBrainzClient.Recordings.SearchAsync(query));
                var matches = recordings.Items.Where(r => r.Title == track && r.Releases.Any(s => s.Title == album));

                // Get the best match (in this case, we use the recording that has the most releases associated).
                return matches.OrderByDescending(r => r.Releases.Count).FirstOrDefault();
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Get the release (typically, the album data) from MusicBrainz via query using the artist, album an track names
        /// This method also tries different variations of the album name
        /// </summary>
        /// <returns>MusicBrainz Release object</returns>
        public Release GetReleaseFromMusicBrainzWithAlbumNameVariations(string artist, string album)
        {
            var release = GetReleaseFromMusicBrainz(artist, album);

            if (release != null)
                return release;

            album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
            release = GetReleaseFromMusicBrainz(artist, album);

            if (release != null)
                return release;

            album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
            release = GetReleaseFromMusicBrainz(artist, album);

            if (release != null)
                return release;

            return null;
        }

        /// <summary>
        /// Get the release (typically, the album data) from MusicBrainz via query using the artist, album an track names
        /// </summary>
        /// <returns>MusicBrainz Release object</returns>
        public Release GetReleaseFromMusicBrainz(string artist, string album)
        {
            try
            {
                var artists = AsyncHelper.RunSync(() => MusicBrainzClient.Artists.SearchAsync(artist.Quote()));
                if (artists != null && artists.Items.Count > 0)
                {
                    var query = new QueryParameters<Release>()
                        {
                            { "arid", artists.FirstOrDefault().Id },
                            { "release", album },
                            { "type", "album" },
                            { "status", "official" }
                        };

                    // Search for a release by title.
                    var releases = AsyncHelper.RunSync(() => MusicBrainzClient.Releases.SearchAsync(query));

                    // Get the oldest release (remember to sort out items with no date set).
                    if (releases != null && releases.Items.Count > 0)
                        return releases.Items.Where(r => r.Date != null && IsCompactDisc(r)).OrderBy(r => r.Date).First();
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e);
            }

            return null;
        }

        private static bool IsCompactDisc(Release r)
        {
            if (r.Media == null || r.Media.Count == 0)
                return false;

            return r.Media[0].Format == "CD";
        }

        /// <summary>
        /// Returns album artwork. First it will look at actual file to see if one is embedded, if not it
        /// will make a request to get the MBID, then use that to make a requst to ConvertArchive to try
        /// and retrive the album art cover image
        /// </summary>
        /// <param name="path">Full path to the music file</param>
        /// <returns>Thumbnail image</returns>
        public Image GetAlbumArtwork(string path)
        {
            try
            {
                TagLib.File file = TagLib.File.Create(path);
                using (var stream = new MemoryStream())
                {
                    var artIpicture = file.Tag.Pictures.FirstOrDefault();
                    if (artIpicture != null)
                    {
                        byte[] pData = artIpicture.Data.Data;
                        stream.Write(pData, 0, Convert.ToInt32(pData.Length));
                        var bmp = new Bitmap(stream, false);

                        return ResizeBitmap(bmp);
                    }
                    else
                    {
                        // Use online CoverArtArchive to get album artwork

                        var coverImage = GetAlbumArtworkFromCoverArtArchive(path);
                        if (coverImage != null)
                            return coverImage;

                        return Properties.Resources.default_artwork;
                    }
                }
            }
            catch
            {
                return Properties.Resources.default_artwork;
            }
        }

        /// <summary>
        /// Makes a requst to ConvertArchive to try and retrive the album art cover image
        /// </summary>
        /// <param name="path">Full path to the music file</param>
        /// <returns>Thumbnail image</returns>
        public Image GetAlbumArtworkFromCoverArtArchive(string path)
        {
            try
            {
                string mbid = GetReleaseMbId(path);
                if (!string.IsNullOrEmpty(mbid))
                {
                    var uri = CoverArtArchive.GetCoverArtUri(mbid);
                    if (uri != null)
                    {
                        try
                        {
                            byte[] imageBytes = GetImageBytesFromUrl(uri.ToString());
                            if (imageBytes != null)
                                using (var ms = new MemoryStream(imageBytes))
                                    return ResizeBitmap(new Bitmap(ms));
                        }
                        catch
                        {
                            // There's no front cover
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// Returns MusicFileMetaData object of music file meta data such as:
        ///   - Artist
        ///   - Album name
        ///   - Track name
        ///   - Duration
        ///   - Bitrate
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>Multi-line string</returns>
        public MusicFileMetaData GetMusicFileMetaData(string path)
        {
            try
            {
                var tags = GetMusicTagLibFile(path);

                string artist = string.Empty;
                string album = string.Empty;
                string track = string.Empty;
                TimeSpan? duration = null;
                int bitsPerSecond = -1;

                if (tags != null && tags.Tag != null)
                {
                    artist = tags.Tag.FirstPerformer ?? tags.Tag.FirstAlbumArtist;
                    album = tags.Tag.Album;
                    track = tags.Tag.Title;
                    bitsPerSecond = tags.Properties.AudioBitrate;
                    duration = tags.Properties.Duration;
                }

                if (string.IsNullOrEmpty(track) && !string.IsNullOrEmpty(album) && !string.IsNullOrEmpty(artist))
                {
                    var recording = GetReleaseFromMusicBrainzWithAlbumNameVariations(artist, album);
                    track = recording.Title;
                }

                return new MusicFileMetaData
                {
                    Artist = artist,
                    Album = album,
                    Track = track,
                    Duration = (TimeSpan)duration,
                    Bitrate = bitsPerSecond
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a single multi-line string of music file meta data such as:
        ///   - Artist
        ///   - Album name
        ///   - Track name
        ///   - Duration
        ///   - Bitrate
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>Multi-line string</returns>
        public string GetMusicFileMetaDataString(string path)
        {
            var sb = new StringBuilder();
            var tags = GetMusicTagLibFile(path);

            string artist = string.Empty;
            string album = string.Empty;
            string track = string.Empty;
            string duration = string.Empty;
            string bitsPerSecond = string.Empty;

            if (tags != null && tags.Tag != null)
            {
                artist = tags.Tag.FirstPerformer ?? tags.Tag.FirstAlbumArtist;
                album = tags.Tag.Album;
                track = tags.Tag.Title;
                bitsPerSecond = tags.Properties.AudioBitrate + " Kbps";
                duration = string.Format("{0:00}:{1:00}:{2:00}",
                                         tags.Properties.Duration.Hours,
                                         tags.Properties.Duration.Minutes,
                                         tags.Properties.Duration.Seconds);

            }

            if (string.IsNullOrEmpty(track) && !string.IsNullOrEmpty(album) && !string.IsNullOrEmpty(artist))
            {
                var recording = GetReleaseFromMusicBrainzWithAlbumNameVariations(artist, album);
                track = recording.Title;
            }

            sb.AppendLine();
            sb.AppendLine("Artist: " + (string.IsNullOrEmpty(artist) ? "-" : artist));
            sb.AppendLine("Album: " + (string.IsNullOrEmpty(album) ? "-" : album));
            sb.AppendLine("Track: " + (string.IsNullOrEmpty(track) ? "-" : track));
            sb.AppendLine("Duration: " + (string.IsNullOrEmpty(duration) ? "-" : duration));
            sb.AppendLine("Bitrate: " + (string.IsNullOrEmpty(bitsPerSecond) ? "-" : bitsPerSecond));

            return sb.ToString();
        }

        /// <summary>
        /// Gets a TabLib (TabLib library object of music file meta data) file object of a given music music file
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>TagLib File</returns>
        public static TagLib.File GetMusicTagLibFile(string path)
        {
            TagLib.File file = TagLib.File.Create(path);
            return file;
        }

        /// <summary>
        /// Peforms a HTTP web request and downloads an image
        /// </summary>
        public static byte[] GetImageBytesFromUrl(string url)
        {
            byte[] buf;
            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                Stream stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception)
            {
                buf = null;
            }

            return buf;
        }

        /// <summary>
        ///  Resizes the image to 50 x 50 for displaying in the main form
        /// </summary>
        private static Bitmap ResizeBitmap(Bitmap bmp)
        {
            Bitmap result = new Bitmap(50, 50);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(bmp, 0, 0, 50, 50);

            return result;
        }
    }

    /// <summary>
    /// Custom music file meta tag object
    /// </summary>
    public class MusicFileMetaData
    {
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Track { get; set; }
        public TimeSpan Duration { get; set; }
        public int Bitrate { get; set; }
    }
}
