using JBToolkit.Threads;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YTMusicUploader.MusicBrainz.API;
using YTMusicUploader.MusicBrainz.API.Entities;
using YTMusicUploader.MusicBrainz.API.Entities.Collections;

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
    /// NOTE: Only 1 request per second allowed via MusicBrainz API, hence plenty of 'Thread.Sleep(1100)' commands.
    ///    
    /// </summary>
    public class MusicDataFetcher
    {
        public class TrackAndReleaseMbId
        {
            public string MbId { get; set; } = string.Empty;
            public string ReleaseMbId { get; set; } = string.Empty;
        }

        /// <summary>
        /// .Net implementation client of MusicBrainz API
        /// </summary>
        public MusicBrainzClient MusicBrainzClient { get; set; }

        private DateTime MusicBrainzLastRequest { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets meta data / tag information from the music file itself and looks up any missing data
        /// using a MusicBrainz API implementation (for instance album cover art).
        /// </summary>
        public MusicDataFetcher()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            MusicBrainzClient = new MusicBrainzClient();
        }

        private bool IsLastRequestTooSoon()
        {
            return MusicBrainzLastRequest > DateTime.Now.AddMilliseconds(-1100);
        }

        /// <summary>
        /// First looks at the file meta data for the track MBID then makes a request to MusicBrainz if it's not found
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <param name="tryMusicBrainz">Use MusicBrainz as a fallback</param>
        /// <returns>MusicBrainz ID</returns>
        public async Task<string> GetTrackMbId(string path, bool tryMusicBrainz)
        {
            try
            {
                // Get tags from file
                var tags = GetMusicTagLibFile(path).Tag;

                if (tags != null &&
                    tags.MusicBrainzTrackId != null &&
                    tags.MusicBrainzReleaseId != null &&
                   !tags.MusicBrainzReleaseId.ToLower().Contains("usicbrainz"))
                {
                    return tags.MusicBrainzTrackId;
                }

                if (tryMusicBrainz)
                {
                    if ((tags.FirstPerformer ?? tags.FirstAlbumArtist) != null &&
                         tags.Title != null)
                    {
                        var result = GetRecordingFromMusicBrainzWithAlbumNameVariation(
                                                tags.FirstPerformer ?? tags.FirstAlbumArtist,
                                                tags.Album,
                                                tags.Title);

                        if (result != null)
                            return await Task.FromResult(result.Id);
                    }
                }
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("GetTrackMbId: " + e.Message);
#endif
            }

            return null;
        }

        /// <summary>
        /// First looks at the file meta data for the release MBID, then makes a request to MusicBrainz if it's not found
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <param name="tryMusicBrainz">Use MusicBrainz as a fallback</param>
        /// <returns>MusicBrainz ID</returns>
        public async Task<string> GetReleaseMbId(string path, bool tryMusicBrainz)
        {
            try
            {
                // Get tags from file
                var tags = GetMusicTagLibFile(path).Tag;

                if (!string.IsNullOrEmpty(tags.MusicBrainzReleaseId) &&
                    !tags.MusicBrainzReleaseId.ToLower().Contains("usicbrainz"))
                {
                    return await Task.FromResult(tags.MusicBrainzReleaseId);
                }

                if (tryMusicBrainz)
                {
                    if ((tags.FirstPerformer ?? tags.FirstAlbumArtist) != null &&
                         tags.Album != null &&
                         tags.Title != null)
                    {
                        var result = GetReleaseFromMusicBrainzWithAlbumNameVariation(
                                                tags.FirstPerformer ?? tags.FirstAlbumArtist ?? "",
                                                tags.Album);
                        if (result != null)
                            return await Task.FromResult(result.Id);
                    }
                }
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("GetReleaseMbId: " + e.Message);
#endif
            }

            return null;
        }

        /// <summary>
        /// Get just the track MBID and ReleaseMBID from the music file or optionally from MusicBrainz if not present
        /// </summary>
        /// <param name="path">Path to music file</param>
        /// <param name="tryWithMusicBrainz">Try to get the data from MusicBrainz if not present</param>
        /// <returns>TrackAndReleaseMbId object</returns>
        public TrackAndReleaseMbId GetTrackAndReleaseMbId(string path, bool tryWithMusicBrainz)
        {
            var metaData = GetMusicFileMetaData(path);

            if (metaData != null && !string.IsNullOrEmpty(metaData.MbId) && !string.IsNullOrEmpty(metaData.ReleaseMbId))
                return new TrackAndReleaseMbId
                {
                    MbId = metaData.MbId.Contains("usicbrainz")
                                            ? null
                                            : metaData.MbId,

                    ReleaseMbId = metaData.ReleaseMbId.Contains("usicbrainz")
                                                            ? null
                                                            : metaData.MbId,
                };

            if (tryWithMusicBrainz)
            {
                var recording = GetRecordingFromMusicBrainzWithAlbumNameVariation(path);
                if (recording != null)
                {
                    return new TrackAndReleaseMbId
                    {
                        MbId = recording.Id.ToLower().Contains("usicbrainz")
                                                        ? null
                                                        : recording.Id,

                        ReleaseMbId = recording.Releases[0].Id.ToLower().Contains("usicbrainz")
                                                        ? null
                                                        : recording.Releases[0].Id
                    };
                };
            }

            return new TrackAndReleaseMbId();
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via query using the artist, album an track names.
        /// This method also tries different variations of the album name
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainzWithAlbumNameVariation(string artist, string album, string track)
        {
            if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(track))
            {
                if (!string.IsNullOrEmpty(album))
                {
                    try
                    {
                        album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
                        album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                    }
                    catch { }
                }

                if (track.Length > 2 && track.Substring(0, 2).IsNumeric())
                {
                    try
                    {
                        track = track.Substring(2).Trim();
                        if (track.StartsWith("_") || track.StartsWith("-") || track.StartsWith("."))
                            track = track.Substring(1).Trim();
                    }
                    catch { }
                }

                track = Regex.Replace(track, @"(\d)+-(\d)+", "").Trim();
                return GetRecordingFromMusicBrainz(artist, album, track);
            }

            return null;
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via query using the artist, album an track names from the music file meta data
        /// This method also tries different variations of the album name
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainzWithAlbumNameVariation(string path)
        {
            var data = GetMusicFileMetaData(path);
            if (data != null)
            {
                string artist = data.Artist;
                string album = data.Album;
                string track = data.Track;

                if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(track))
                {
                    if (!string.IsNullOrEmpty(album))
                    {
                        try
                        {
                            album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
                            album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                        }
                        catch { }
                    }

                    if (track.Length > 2 && track.Substring(0, 2).IsNumeric())
                    {
                        try
                        {
                            track = track.Substring(2).Trim();
                            if (track.StartsWith("_") || track.StartsWith("-") || track.StartsWith("."))
                                track = track.Substring(1).Trim();
                        }
                        catch { }
                    }

                    track = Regex.Replace(track, @"(\d)+-(\d)+", "").Trim();
                    return GetRecordingFromMusicBrainz(artist, album, track);
                }
            }

            return null;
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via query using the artist, album an track names
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainz(string artist, string album, string track)
        {
            while (IsLastRequestTooSoon())
                ThreadHelper.SafeSleep(1100);

            QueryParameters<Recording> query = null;
            if (!string.IsNullOrEmpty(album))
            {
                query = new QueryParameters<Recording>()
                    {
                        { "artist", artist },
                        { "release", album },
                        { "recording", track }
                    };
            }
            else
            {
                query = new QueryParameters<Recording>()
                    {
                        { "artist", artist },
                        { "recording", track }
                    };
            }

            RecordingList recordings = null;
            try
            {
                MusicBrainzLastRequest = DateTime.Now;
                recordings = MusicBrainzClient.Recordings.SearchAsync(query).GetAwaiter().GetResult();
            }
            catch
            {
                ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler
                try
                {
                    MusicBrainzLastRequest = DateTime.Now;
                    recordings = MusicBrainzClient.Recordings.SearchAsync(query).GetAwaiter().GetResult();
                }
                catch { }
            }

            if (recordings != null && recordings.Count > 0)
            {
                IEnumerable<Recording> matches = null;
                if (!string.IsNullOrEmpty(album))
                    matches = recordings.Items.Where(r => r.Title == track && r.Releases.Any(s => s.Title == album));
                else
                    matches = recordings.Items.Where(r => r.Title == track);

                if (matches != null && matches.Count() > 0)
                    return matches.OrderByDescending(r => r.Releases.Count).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Get the recording (track data) from MusicBrainz via the MBID
        /// </summary>
        /// <returns>MusicBrainz Recording object</returns>
        public Recording GetRecordingFromMusicBrainz(string mbId)
        {
            while (IsLastRequestTooSoon())
                ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler

            try
            {
                MusicBrainzLastRequest = DateTime.Now;
                return MusicBrainzClient.Recordings.GetAsync(mbId).GetAwaiter().GetResult();
            }
            catch
            {
                ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler
                try
                {
                    MusicBrainzLastRequest = DateTime.Now;
                    return MusicBrainzClient.Recordings.GetAsync(mbId).GetAwaiter().GetResult();
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Get the release (typically, the album data) from MusicBrainz via query using the artist, album an track names
        /// This method also tries different variations of the album name
        /// </summary>
        /// <returns>MusicBrainz Release object</returns>
        public Release GetReleaseFromMusicBrainzWithAlbumNameVariation(string artist, string album)
        {
            if (!string.IsNullOrEmpty(artist) && !string.IsNullOrEmpty(album))
            {
                try
                {
                    album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
                    album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                }
                catch { }

                return GetReleaseFromMusicBrainz(artist, album);
            }

            return null;
        }

        /// <summary>
        /// Get the release (typically, the album data) from MusicBrainz via query using the artist, album an track names
        /// </summary>
        /// <returns>MusicBrainz Release object</returns>
        public Release GetReleaseFromMusicBrainz(string artist, string album)
        {
            while (IsLastRequestTooSoon())
                ThreadHelper.SafeSleep(1100);

            try
            {
                MusicBrainzLastRequest = DateTime.Now;
                var artists = MusicBrainzClient.Artists.SearchAsync(artist.Quote()).GetAwaiter().GetResult();
                if (artists != null && artists.Items.Count > 0)
                {
                    var query = new QueryParameters<Release>()
                        {
                            { "arid", artists.Items.FirstOrDefault().Id },
                            { "release", album },
                            { "type", "album" },
                            { "status", "official" }
                        };

                    ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler

                    var releases = MusicBrainzClient.Releases.SearchAsync(query).GetAwaiter().GetResult();
                    if (releases != null && releases.Items.Count > 0)
                        return releases.Items.Where(r => r.Date != null && IsCompactDisc(r)).OrderBy(r => r.Date).First();
                }
            }
            catch
            {
                ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler
                try
                {
                    MusicBrainzLastRequest = DateTime.Now;
                    var artists = MusicBrainzClient.Artists.SearchAsync(artist.Quote()).GetAwaiter().GetResult();
                    if (artists != null && artists.Items.Count > 0)
                    {
                        var query = new QueryParameters<Release>()
                        {
                            { "arid", artists.Items.FirstOrDefault().Id },
                            { "release", album },
                            { "type", "album" },
                            { "status", "official" }
                        };

                        ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler

                        var releases = MusicBrainzClient.Releases.SearchAsync(query).GetAwaiter().GetResult();
                        if (releases != null && releases.Items.Count > 0)
                            return releases.Items.Where(r => r.Date != null && IsCompactDisc(r)).OrderBy(r => r.Date).First();
                    }
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Get the release (typically, the album data) from MusicBrainz via the MBID
        /// </summary>
        /// <returns>MusicBrainz Release object</returns>
        public Release GetReleaseFromMusicBrainz(string mbid)
        {
            while (IsLastRequestTooSoon())
                ThreadHelper.SafeSleep(1100);

            try
            {
                MusicBrainzLastRequest = DateTime.Now;
                return MusicBrainzClient.Releases.GetAsync(mbid).GetAwaiter().GetResult();
            }
            catch
            {
                ThreadHelper.SafeSleep(1100); // MusicBrains throttle handler
                try
                {
                    MusicBrainzLastRequest = DateTime.Now;
                    return MusicBrainzClient.Releases.GetAsync(mbid).GetAwaiter().GetResult();
                }
                catch { }
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
        /// <param name="tryMusicBrainz">Use MusicBrainz as a fallback</param>
        /// <returns>Thumbnail image</returns>
        public async Task<Image> GetAlbumArtwork(string path, bool tryMusicBrainz = true)
        {
            try
            {
                var file = TagLib.File.Create(path);
                using (var stream = new MemoryStream())
                {
                    var artIpicture = file.Tag.Pictures.FirstOrDefault();
                    if (artIpicture != null)
                    {
                        byte[] pData = artIpicture.Data.Data;
                        stream.Write(pData, 0, Convert.ToInt32(pData.Length));
                        var bmp = new Bitmap(stream, false);

                        return await Task.FromResult(ResizeBitmap(bmp));
                    }
                    else
                    {
                        // Use online CoverArtArchive to get album artwork
                        if (tryMusicBrainz)
                        {
                            if (!string.IsNullOrEmpty(file.Tag.MusicBrainzReleaseId) &&
                                !file.Tag.MusicBrainzReleaseId.ToLower().Contains("usicbrainz"))
                            {
                                var coverImage1 = GetAlbumArtworkFromCoverArtArchive(file.Tag.MusicBrainzReleaseId, false);
                                if (coverImage1 != null)
                                    return await Task.FromResult(coverImage1);
                            }

                            var coverImage2 = GetAlbumArtworkFromCoverArtArchive(path, true);
                            if (coverImage2 != null)
                                return await Task.FromResult(coverImage2);
                        }

                        return await Task.FromResult(Properties.Resources.default_artwork);
                    }
                }
            }
            catch
            {
                return await Task.FromResult(Properties.Resources.default_artwork);
            }
        }

        /// <summary>
        /// Makes a requst to ConvertArchive to try and retrive the album art cover image
        /// </summary>
        /// <param name="path">Full path to the music file</param>
        /// <returns>Thumbnail image</returns>
        public Image GetAlbumArtworkFromCoverArtArchive(string pathOrMbid, bool isPath)
        {
            try
            {
                string mbid = isPath ? GetReleaseMbId(pathOrMbid, true).Result : pathOrMbid;
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

                string mbId = string.Empty;
                string releaseMbId = string.Empty;
                string artist = string.Empty;
                string album = string.Empty;
                string track = string.Empty;
                TimeSpan? duration = null;
                int bitsPerSecond = -1;

                if (tags != null && tags.Tag != null)
                {
                    mbId = tags.Tag.MusicBrainzTrackId;
                    releaseMbId = tags.Tag.MusicBrainzReleaseId;
                    artist = tags.Tag.FirstPerformer ?? tags.Tag.FirstAlbumArtist;
                    album = tags.Tag.Album;
                    track = tags.Tag.Title;
                    bitsPerSecond = tags.Properties.AudioBitrate;
                    duration = tags.Properties.Duration;
                }

                return new MusicFileMetaData
                {
                    MbId = mbId,
                    ReleaseMbId = releaseMbId,
                    Artist = artist,
                    Album = album,
                    Track = track,
                    Duration = (TimeSpan)duration,
                    Bitrate = bitsPerSecond
                };
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("GetMusicFileMetaData: " + e.Message);
#endif
            }

            return null;
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
        public async Task<string> GetMusicFileMetaDataString(string path)
        {
            try
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

                sb.AppendLine();
                sb.AppendLine("Artist: " + (string.IsNullOrEmpty(artist) ? "-" : artist));
                sb.AppendLine("Album: " + (string.IsNullOrEmpty(album) ? "-" : album));
                sb.AppendLine("Track: " + (string.IsNullOrEmpty(track) ? "-" : track));
                sb.AppendLine("Duration: " + (string.IsNullOrEmpty(duration) ? "-" : duration));
                sb.AppendLine("Bitrate: " + (string.IsNullOrEmpty(bitsPerSecond) ? "-" : bitsPerSecond));

                return await Task.FromResult(sb.ToString());
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("GetMusicFileMetaDataString: " + e.Message);
#endif
            }

            return null;
        }

        /// <summary>
        /// Gets a TabLib (TabLib library object of music file meta data) file object of a given music music file
        /// </summary>
        /// <param name="path">Full path to music file</param>
        /// <returns>TagLib File</returns>
        public static TagLib.File GetMusicTagLibFile(string path)
        {
            var file = TagLib.File.Create(path);
            return file;
        }

        /// <summary>
        /// Peforms a HTTP web request and downloads an image
        /// </summary>
        public static byte[] GetImageBytesFromUrl(string url)
        {
            byte[] buf = null;
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)req.GetResponse();

                if (response != null)
                {
                    var stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var br = new BinaryReader(stream))
                        {
                            int len = (int)response.ContentLength;
                            buf = br.ReadBytes(len);
                            br.Close();
                        }

                        stream.Close();
                    }

                    response.Close();
                }
            }
            catch
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
            var result = new Bitmap(50, 50);
            using (var g = Graphics.FromImage(result))
                g.DrawImage(bmp, 0, 0, 50, 50);

            return result;
        }
    }

    /// <summary>
    /// Custom music file meta tag object
    /// </summary>
    public class MusicFileMetaData
    {
        public string MbId { get; set; }
        public string ReleaseMbId { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Track { get; set; }
        public TimeSpan Duration { get; set; }
        public int Bitrate { get; set; }
    }
}
