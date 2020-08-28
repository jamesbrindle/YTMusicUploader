using JBToolkit.FuzzyLogic;
using JBToolkit.StreamHelpers;
using JBToolkit.Threads;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using YTMusicUploader.Business;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
    /// It may request a little attention depending on the results, seen as though we're trying to match a song based
    /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
    /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
    /// a match if it's above 0.75.
    /// 
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        public enum UploadCheckResult
        {
            NotPresent,
            Present_FromCache,
            Present_NewRequest
        }

        public static Thread UploadCheckPreloaderThread { get; set; } = null;
        public static Thread UploadCheckPreloaderSleepThread { get; set; } = null;

        /// <summary>
        /// Object to store pre fetched already uploaded check results
        /// </summary>
        public static class UploadCheckCache
        {
            public static List<MusicFileCacheObject> CachedObjects { get; set; } = new List<MusicFileCacheObject>();
            public static bool Sleep { get; set; } = false;
            public static bool CleanUp { get; set; } = false;
            public static HashSet<string> CachedObjectHash { get; set; } = new HashSet<string>();

            public class MusicFileCacheObject
            {
                public string MusicFilePath { get; set; }
                public string MbId { get; set; }
                public string ReleaseMbId { get; set; }
                public string EntityId { get; set; }
                public bool Result { get; set; }
            }
        }

        /// <summary>
        /// Starts a new thread which loops through MusicFiles to check if they've already been uploaded to YouTube Music ahead of
        /// time and stores them in a cache to speed up the upload checker.
        /// </summary>
        /// <param name="musicFilesList">Path to music file to be uploaded</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="musicDataFetcher">You can pass an existing MusicDataFetcher object, or one will be created if left blank</param>
        public static void StartPrefetchingUploadedFilesCheck(
            List<MusicFile> musicFilesList,
            string cookieValue,
            MusicDataFetcher musicDataFetcher = null)
        {
            UploadCheckCache.CleanUp = false;
            UploadCheckCache.CachedObjects.Clear();
            UploadCheckCache.CachedObjectHash.Clear();

            try
            {
                if (UploadCheckPreloaderThread != null)
                    UploadCheckPreloaderThread.Abort();
            }
            catch { }

            UploadCheckPreloaderThread = new Thread((ThreadStart)delegate
            {
                while (!UploadCheckCache.CleanUp)
                {
                    UploadCheckCache.Sleep = true;
                    ThreadHelper.SafeSleep(100);
                    UploadCheckCache.Sleep = false;
                    ThreadHelper.SafeSleep(1000);
                }

                UploadCheckCache.CachedObjects.Clear();
                UploadCheckCache.CachedObjectHash.Clear();
            })
            {
                IsBackground = true
            };
            UploadCheckPreloaderThread.Start();

            UploadCheckPreloaderThread = new Thread((ThreadStart)delegate
            {
                musicFilesList.AsParallel().ForAllInApproximateOrder(cacheObject =>
                {
                    if (!UploadCheckCache.CleanUp)
                    {
                        while (UploadCheckCache.Sleep)
                            ThreadHelper.SafeSleep(2);

                        UploadCheckCache.CachedObjects.Add(new UploadCheckCache.MusicFileCacheObject
                        {
                            MusicFilePath = cacheObject.Path,
                            Result = IsSongUploaded(cacheObject.Path, cookieValue, out string entityId, musicDataFetcher, false) != UploadCheckResult.NotPresent,
                            EntityId = entityId,

                            MbId = !string.IsNullOrEmpty(cacheObject.MbId)
                                                ? cacheObject.MbId
                                                : musicDataFetcher.GetTrackMbId(cacheObject.Path, false).Result,

                            ReleaseMbId = !string.IsNullOrEmpty(cacheObject.MbId)
                                                       ? cacheObject.ReleaseMbId
                                                       : musicDataFetcher.GetReleaseMbId(cacheObject.Path, false).Result

                        });

                        UploadCheckCache.CachedObjectHash.Add(cacheObject.Path);
                    }
                    else
                    {
                        try
                        {
                            return;
                        }
                        catch { }
                    }
                }, Global.MaxDegreesOfParallelism);
            })
            {
                IsBackground = true
            };
            UploadCheckPreloaderThread.Start();
        }

        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// </summary>
        /// <param name="musicFilePath">Path to music file to be uploaded</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="entityId">Output YouTube Music song entity ID if found</param>
        /// <param name="musicDataFetcher">You can pass an existing MusicDataFetcher object, or one will be created if left blank</param>
        /// <param name="checkCheck">Whether or not to refer to cache for lookup (only useful while scanning)/param>
        /// <returns>True if song is found, false otherwise</returns>
        public static UploadCheckResult IsSongUploaded(
            string musicFilePath,
            string cookieValue,
            out string entityId,
            MusicDataFetcher musicDataFetcher = null,
            bool checkCheck = true)
        {
            entityId = string.Empty;
            if (checkCheck && UploadCheckCache.CachedObjectHash.Contains(musicFilePath))
            {
                var cache = UploadCheckCache.CachedObjects
                                            .Where(m => m.MusicFilePath == musicFilePath)
                                            .FirstOrDefault();

                entityId = cache.EntityId;
                return cache.Result
                                ? UploadCheckResult.Present_FromCache
                                : UploadCheckResult.NotPresent;
            }
            else
            {
                if (musicDataFetcher == null)
                    musicDataFetcher = new MusicDataFetcher();

                var musicFileMetaData = musicDataFetcher.GetMusicFileMetaData(musicFilePath);
                if (musicFileMetaData == null ||
                    musicFileMetaData.Artist == null ||
                    musicFileMetaData.Track == null)
                {
                    return UploadCheckResult.NotPresent;
                }

                string artist = musicFileMetaData.Artist;
                string album = musicFileMetaData.Album;
                string track = musicFileMetaData.Track;

                try
                {
                    return IsSongUploadedMulitpleAlbumNameVariation(artist, album, track, cookieValue, out entityId)
                                    ? UploadCheckResult.Present_NewRequest
                                    : UploadCheckResult.NotPresent;
                }
                catch
                {
                    return UploadCheckResult.NotPresent;
                }
            }
        }

        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// </summary>
        /// <param name="artist">Artist name from music file meta tag</param>
        /// <param name="album">Album name from music file meta tag</param>
        /// <param name="track">Track or song name from music file meta tag</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="entityId">Output YouTube Music song entity ID if found</param>
        /// <returns>True if song is found, false otherwise</returns>
        public static bool IsSongUploadedMulitpleAlbumNameVariation(
            string artist,
            string album,
            string track,
            string cookieValue,
            out string entityId)
        {
            // Make sure they're not null
            artist = artist ?? "";
            album = album ?? "";
            track = track ?? "";

            string originalAlbum = album;
            string originalTrack = track;

            bool result = IsSongUploaded(artist, album, track, cookieValue, out entityId);
            if (result)
                return result;

            if (!string.IsNullOrEmpty(album))
            {
                try
                {
                    album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();

                    // Don't bother trying the exact same search
                    if (album != originalAlbum)
                    {
                        result = IsSongUploaded(artist, album, track, cookieValue, out entityId);
                        if (result)
                            return result;
                    }

                    originalAlbum = album;
                }
                catch { }

                try
                {
                    album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();

                    // Don't bother trying the exact same search
                    if (album != originalAlbum)
                    {
                        result = IsSongUploaded(artist, album, track, cookieValue, out entityId);
                        if (result)
                            return result;
                    }
                }
                catch { }
            }

            try
            {
                if (!string.IsNullOrEmpty(track) && track.Length > 2 && track.Substring(0, 2).IsNumeric())
                {
                    try
                    {
                        track = track.Substring(2).Trim();
                        if (track.StartsWith("_") || track.StartsWith("-") || track.StartsWith("."))
                            track = track.Substring(1).Trim();
                    }
                    catch { }
                }
            }
            catch { }

            // Don't bother trying the exact same search
            if (track != originalTrack)
            {
                result = IsSongUploaded(artist, album, track, cookieValue, out entityId);
                if (result)
                    return result;
            }

            originalTrack = track;
            track = Regex.Replace(track, @"(\d)+-(\d)+", "").Trim();

            // Don't bother trying the exact same search
            if (track != originalTrack)
            {
                result = IsSongUploaded(artist, album, track, cookieValue, out entityId);
                if (result)
                    return result;
            }

            return false;
        }

        /// <summary>
        /// HttpWebRequest POST request to send to YouTube to determine if the song about to be uploaded already exists
        /// It may request a little attention depending on the results, seen as though we're trying to match a song based
        /// on artist, album and track name, which may be slightly different in the uploading music file's meta tags. Currently
        /// it uses a Levenshtein distance fuzzy logic implementation to check similarity between strings and is considered
        /// a match if it's above 0.75.
        /// </summary>
        /// <param name="artist">Artist name from music file meta tag</param>
        /// <param name="album">Album name from music file meta tag</param>
        /// <param name="track">Track or song name from music file meta tag</param>
        /// <param name="cookieValue">Cookie from a previous YouTube Music sign in via this application (stored in the database)</param>
        /// <param name="entityId">Output YouTube Music song entity ID if found</param>
        /// <returns>True if song is found, false otherwise</returns>
        public static bool IsSongUploaded(
            string artist,
            string album,
            string track,
            string cookieValue,
            out string entityId)
        {
            entityId = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Global.YouTubeBaseUrl + "search" + Global.YouTubeMusicParams);
                request = AddStandardHeaders(request, cookieValue);

                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Goog-AuthUser"] = "0";
                request.Headers["x-origin"] = "https://music.youtube.com";
                request.Headers["X-Goog-Visitor-Id"] = Global.GoogleVisitorId;
                request.Headers["Authorization"] = GetAuthorisation(GetSAPISIDFromCookie(cookieValue));

                var context = JsonConvert.DeserializeObject<SearchContext>(
                                                SafeFileStream.ReadAllText(
                                                                    Path.Combine(
                                                                            Global.WorkingDirectory,
                                                                            @"AppData\search_uploads_context.json")));

                if (!string.IsNullOrEmpty(album))
                    context.query = string.Format("{0} {1} {2}", artist, album, track);
                else
                    context.query = string.Format("{0} {1}", artist, track);

                byte[] postBytes = GetPostBytes(JsonConvert.SerializeObject(context));
                request.ContentLength = postBytes.Length;

                using (var requestStream = request.GetRequestStream())
                {
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                }

                postBytes = null;
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    string result;
                    using (var brotli = new Brotli.BrotliStream(
                                                        response.GetResponseStream(),
                                                        System.IO.Compression.CompressionMode.Decompress,
                                                        true))
                    {
                        var streamReader = new StreamReader(brotli);
                        result = streamReader.ReadToEnd();
                    }

                    JObject runObject = JObject.Parse(result);
                    List<JToken> runs = runObject.Descendants()
                                          .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "runs")
                                          .Select(p => ((JProperty)p).Value).ToList();

                    float matchSuccessMinimum = Global.YouTubeUploadedSimilarityPercentageForMatch;
                    float artistSimilarity = 0.0f;
                    float albumSimilartity = 0.0f;
                    float trackSimilarity = 0.0f;

                    bool foundTrack = false;

                    foreach (JToken run in runs)
                    {
                        if (run.ToString().Contains("text"))
                        {
                            var runArray = run.ToObject<SearchResult.Run[]>();
                            if (runArray.Length > 0)
                            {
                                if (runArray[0].text.ToLower().Contains("no results found"))
                                    return true;
                                else
                                {
                                    Parallel.ForEach(runArray, (runElement) =>
                                    {
                                        if (runElement.text == null)
                                            runElement.text = string.Empty;

                                        float _artistSimilarity = 0.0f;
                                        float _albumSimilartity = 0.0f;
                                        float _trackSimilarity = 0.0f;

                                        Parallel.For(0, 3, (i, state) =>
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    if (artistSimilarity < matchSuccessMinimum)
                                                        _artistSimilarity = Levenshtein.Similarity(runElement.text, artist);
                                                    break;
                                                case 1:
                                                    if (_albumSimilartity < matchSuccessMinimum)
                                                        _albumSimilartity = Levenshtein.Similarity(runElement.text, album);
                                                    break;
                                                case 2:
                                                    if (_trackSimilarity < matchSuccessMinimum)
                                                        _trackSimilarity = Levenshtein.Similarity(runElement.text, track);
                                                    break;
                                                default:
                                                    break;
                                            }
                                        });

                                        if (artistSimilarity < _artistSimilarity)
                                            artistSimilarity = _artistSimilarity;

                                        if (albumSimilartity < _albumSimilartity)
                                            albumSimilartity = _albumSimilartity;

                                        if (trackSimilarity < _trackSimilarity)
                                        {
                                            foundTrack = true;
                                            trackSimilarity = _trackSimilarity;
                                        }
                                    });

                                    if (foundTrack && string.IsNullOrEmpty(entityId))
                                    {
                                        // We're getting the YouTube Music file 'entityId' for no particular reason at this stage,
                                        // but could come in very handy if we find a use for it in the future, like more accurate
                                        // search results, or if we want to 'delete' an uploaded song, which you do via the entityId

                                        if (run.ToString().ToLower().Contains("delete song"))
                                        {
                                            if (!string.IsNullOrEmpty(album))
                                            {
                                                if (artistSimilarity >= matchSuccessMinimum &&
                                                    albumSimilartity >= matchSuccessMinimum &&
                                                    trackSimilarity >= matchSuccessMinimum)
                                                {

                                                    var deleteRuns = run.Parent.Parent.Parent.Parent.Descendants()
                                                                        .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "entityId")
                                                                        .Select(p => ((JProperty)p).Value).ToList();

                                                    if (deleteRuns.Count > 0)
                                                        entityId = deleteRuns[0].ToString();
                                                }
                                            }
                                            else
                                            {
                                                if (artistSimilarity >= matchSuccessMinimum &&
                                                    trackSimilarity >= matchSuccessMinimum)
                                                {
                                                    var deleteRuns = run.Parent.Parent.Parent.Parent.Descendants()
                                                                        .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "entityId")
                                                                        .Select(p => ((JProperty)p).Value).ToList();

                                                    if (deleteRuns.Count > 0)
                                                        entityId = deleteRuns[0].ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(album))
                    {
                        if (artistSimilarity >= matchSuccessMinimum &&
                            albumSimilartity >= matchSuccessMinimum &&
                            trackSimilarity >= matchSuccessMinimum)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (artistSimilarity >= matchSuccessMinimum &&
                            trackSimilarity >= matchSuccessMinimum)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
