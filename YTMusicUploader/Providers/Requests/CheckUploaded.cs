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
using System.Windows;
using YTMusicUploader.Business;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Providers
{
    /// <summary>
    /// YouTube Music API Request Methods
    /// 
    /// Thanks to: sigma67: 
    ///     https://ytmusicapi.readthedocs.io/en/latest/ 
    ///     https://github.com/sigma67/ytmusicapi
    /// </summary>
    public partial class Requests
    {
        /// <summary>
        /// Not present or present with an indication of where the result came from - a new request or from the cache
        /// </summary>
        public enum UploadCheckResult
        {
            NotPresent,
            Present_FromCache,
            Present_NewRequest
        }
        public static ArtistCache ArtistCache { get; set; } = null;
        public static Thread UploadCheckPreloaderThread { get; set; } = null;
        public static Thread UploadCheckPreloaderSleepThread { get; set; } = null;
        public static bool IssueWithGatheringUploadedMusic { get; set; } = false;

        /// <summary>
        /// Object to store pre fetched already uploaded check results
        /// </summary>
        public static class UploadCheckCache
        {
            public static List<MusicFileCacheObject> CachedObjects { get; set; } = new List<MusicFileCacheObject>();
            public static bool Sleep { get; set; } = false;
            public static bool Pause { get; set; } = false;
            public static bool CleanUp { get; set; } = false;
            public static HashSet<string> CachedObjectHash { get; set; } = new HashSet<string>();

            public class MusicFileCacheObject
            {
                public string MusicFilePath { get; set; }
                public string MbId { get; set; }
                public string ReleaseMbId { get; set; }
                public string EntityId { get; set; }
                public string BrowseId { get; set; }
                public string VideoId { get; set; }
                public bool Result { get; set; }
            }
        }

        /// <summary>
        /// Set the uploaded artist cached gathered from YouTube Music
        /// </summary>
        public static void LoadArtistCache(string authenticationCookie)
        {
            try
            {
                if (!IssueWithGatheringUploadedMusic)
                {
                    var artistGetThread = new Thread((ThreadStart)delegate
                    {
                        Logger.LogInfo("LoadArtistCache", "Loading artists for cache");

                        UploadCheckCache.Pause = true;
                        ArtistCache = null;
                        ArtistCache = GetArtists(authenticationCookie);
                        ArtistCache.Playlists = Playlists.GetPlaylists(authenticationCookie);
                        UploadCheckCache.Pause = false;

                        Logger.LogInfo("LoadArtistCache", "Load artists complete");
                    });

                    artistGetThread.Start();

                    bool cancel = false;
                    int threadTimer = 0;
                    while (!cancel)
                    {
                        if (!artistGetThread.IsAlive)
                        {
                            cancel = true;
                            break;
                        }

                        if (threadTimer > 360) // 6 minutes
                        {
                            MessageBox.Show(
                                "There was an issue trying to fetch your 'Uploads' collection from YouTube Music. You should still be able to upload " +
                                "songs but checking for already uploaded songs will be performed at a reduced rate due to no caching.\n\n" +
                                "Please forward this message to:\n\nhttps://github.com/jamesbrindle/YTMusicUploader/issues",
                                "Issue Fetching YT Music Collection",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                            IssueWithGatheringUploadedMusic = true;
                            cancel = true;
                            try
                            {
                                artistGetThread.Abort();
                            }
                            catch { }
                            throw new ApplicationException("LoadArtistCache - Load artist cache timeout expired. Probably bug");
                        }

                        Thread.Sleep(1000);
                        threadTimer++;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("thread was being aborted") ||
                    (e.InnerException != null && e.InnerException.Message.ToLower().Contains("thread was being aborted")))
                {
                    Logger.Log(e, "LoadArtistCache - Load artist cache timeout expired due to thread aborting after system change.", Log.LogTypeEnum.Warning);
                }
                else
                {
                    Logger.Log(e, "LoadArtistCache - Load artist cache timeout expired. Probably bug", Log.LogTypeEnum.Critical);
                }
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
            try
            {
                Logger.LogInfo("LoadArtistCache", "Uploads details pre-fetch started (with no complete info)");

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
                        // The prefecher runs in parallel and doesn't leave any time
                        // for the normal requests... So every second grant a full 100ms

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

                            while (UploadCheckCache.Pause)
                                ThreadHelper.SafeSleep(200);

                            UploadCheckCache.CachedObjects.Add(new UploadCheckCache.MusicFileCacheObject
                            {
                                MusicFilePath = cacheObject.Path,
                                Result = IsSongUploaded(cacheObject.Path, cookieValue, out string entityId, out string videoId, musicDataFetcher, false) != UploadCheckResult.NotPresent,
                                EntityId = entityId,
                                VideoId = videoId,

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
            catch (Exception e)
            {
                Logger.Log(e, "StartPrefetchingUploadedFilesCheck");
            }
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
            out string videoId,
            MusicDataFetcher musicDataFetcher = null,
            bool checkCheck = true,
            bool parallel = true)
        {
            entityId = string.Empty;
            videoId = string.Empty;

            try
            {
                if (checkCheck && UploadCheckCache.CachedObjectHash.Contains(musicFilePath))
                {
                    var cache = UploadCheckCache.CachedObjects
                                                .Where(m => m.MusicFilePath == musicFilePath)
                                                .FirstOrDefault();

                    entityId = cache.EntityId;
                    videoId = cache.VideoId;
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
                        return IsSongUploadedMulitpleNameVariation(artist, album, track, cookieValue, parallel, out entityId, out videoId)
                                        ? UploadCheckResult.Present_NewRequest
                                        : UploadCheckResult.NotPresent;
                    }
                    catch
                    {
                        return UploadCheckResult.NotPresent;
                    }
                }
            }
            catch
            {
                return UploadCheckResult.NotPresent;
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
        private static bool IsSongUploadedMulitpleNameVariation(
            string artist,
            string album,
            string track,
            string cookieValue,
            bool parallel,
            out string entityId,
            out string videoId)
        {
            // Make sure they're not null
            artist = artist ?? "";
            album = album ?? "";
            track = track ?? "";

            string originalAlbum = album;
            string originalTrack = track;

            bool result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
            if (result)
                return result;

            if (!string.IsNullOrEmpty(album))
            {
                try
                {
                    album = album.Substring(album.IndexOf("-") + 1, album.Length - 1 - album.IndexOf("-")).Trim();
                    if (album != originalAlbum)
                    {
                        result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                        if (result)
                            return result;

                        originalAlbum = album;
                    }
                }
                catch { }

                try
                {
                    album = Regex.Replace(album, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                    if (album != originalAlbum)
                    {
                        result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                        if (result)
                            return result;

                        originalAlbum = album;
                    }
                }
                catch { }

                // Difference versions of the album are usual in brackets, so removing this block not report false duplicates
                //try
                //{
                //    album = Regex.Replace(album, @"(?<=\()(.*?)(?=\))", "").Replace("()", "").Replace("  ", " ").Trim();
                //    if (album != originalAlbum)
                //    {
                //        result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId);
                //        if (result)
                //            return result;
                //    }
                //}
                //catch { }
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

            if (track != originalTrack)
            {
                result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                if (result)
                    return result;

                originalTrack = track;
            }

            try
            {
                track = Regex.Replace(track, @"(\d)+-(\d)+", "").Trim();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;

                    originalTrack = track;
                }
            }
            catch { }

            try
            {
                track = Regex.Replace(track, @"(?<=\()(.*?)(?=\))", "").Replace("()", "").Replace("  ", " ").Trim();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;

                    originalTrack = track;
                }
            }
            catch { }

            try
            {
                track = Regex.Replace(track, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;

                    originalTrack = track;
                }
            }
            catch
            { }

            try
            {
                track = track.UnQuote();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;

                    originalTrack = track;
                }
            }
            catch
            { }

            try
            {
                track = Regex.Replace(track, artist, "").Trim();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;

                    originalTrack = track;
                }
            }
            catch { }

            try
            {
                track = Regex.Replace(track, album, "").Trim();
                if (track != originalTrack)
                {
                    result = IsSongUploaded(artist, album, track, cookieValue, parallel, out entityId, out videoId);
                    if (result)
                        return result;
                }
            }
            catch { }

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
        private static bool IsSongUploaded(
            string artist,
            string album,
            string track,
            string cookieValue,
            bool parallel,
            out string entityId,
            out string videoId)
        {
            entityId = string.Empty;
            videoId = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Global.YTMusicBaseUrl + "search" + Global.YTMusicParams);
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

                    var runObject = JObject.Parse(result);
                    var runs = runObject.Descendants()
                                        .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "runs")
                                        .Select(p => ((JProperty)p).Value).ToList();

                    float matchSuccessMinimum = Global.YTMusicUploadedSimilarityPercentageForMatch;
                    float artistSimilarity = 0.0f;
                    float albumSimilarity = 0.0f;
                    float trackSimilarity = 0.0f;

                    bool foundTrack = false;
                    foreach (JToken run in runs)
                    {
                        if (!parallel)
                            ThreadHelper.SafeSleep(5);

                        if (run.ToString().Contains("text"))
                        {
                            var runArray = run.ToObject<SearchResultContext.Run[]>();
                            if (runArray.Length > 0)
                            {
                                if (runArray[0].text.ToLower().Contains("no results found"))
                                {
                                    return IsPresentInArtistsCache(cookieValue,
                                                                   artist,
                                                                   album,
                                                                   track,
                                                                   matchSuccessMinimum,
                                                                   parallel,
                                                                   out entityId,
                                                                   out videoId);
                                }
                                else
                                {
                                    float _artistSimilarity = 0.0f;
                                    float _albumSimilartity = 0.0f;
                                    float _trackSimilarity = 0.0f;

                                    if (parallel)
                                    {
                                        DetermineSimilarity_Parallel(runArray,
                                                                     artist,
                                                                     album,
                                                                     track,
                                                                     ref _artistSimilarity,
                                                                     ref _albumSimilartity,
                                                                     ref _trackSimilarity,
                                                                     matchSuccessMinimum);
                                    }
                                    else
                                    {
                                        DetermineSimilarity_Standard(runArray,
                                                                     artist,
                                                                     album,
                                                                     track,
                                                                     ref _artistSimilarity,
                                                                     ref _albumSimilartity,
                                                                     ref _trackSimilarity,
                                                                     matchSuccessMinimum);
                                    }

                                    if (artistSimilarity < _artistSimilarity)
                                        artistSimilarity = _artistSimilarity;

                                    if (albumSimilarity < _albumSimilartity)
                                        albumSimilarity = _albumSimilartity;

                                    if (trackSimilarity < _trackSimilarity)
                                        trackSimilarity = _trackSimilarity;

                                    if (artistSimilarity >= matchSuccessMinimum &&
                                        trackSimilarity >= matchSuccessMinimum)
                                    {
                                        foundTrack = true;
                                    }

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
                                                    albumSimilarity >= matchSuccessMinimum &&
                                                    trackSimilarity >= matchSuccessMinimum)
                                                {

                                                    var entityIdRuns = run.Parent.Parent.Parent.Parent.Descendants()
                                                                        .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "entityId")
                                                                        .Select(p => ((JProperty)p).Value).ToList();

                                                    if (entityIdRuns.Count > 0)
                                                    {
                                                        entityId = entityIdRuns[0].ToString();
                                                    }

                                                    var videoIdRuns = run.Parent.Parent.Parent.Parent.Parent.Parent.Parent
                                                                         .Descendants()
                                                                         .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "videoId")
                                                                         .Select(p => ((JProperty)p).Value).ToList();

                                                    if (videoIdRuns.Count > 0)
                                                    {
                                                        videoId = videoIdRuns[0].ToString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (artistSimilarity >= matchSuccessMinimum &&
                                                    trackSimilarity >= matchSuccessMinimum)
                                                {
                                                    var entityIdRuns = run.Parent.Parent.Parent.Parent
                                                                          .Descendants()
                                                                          .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "entityId")
                                                                          .Select(p => ((JProperty)p).Value).ToList();

                                                    if (entityIdRuns.Count > 0)
                                                    {
                                                        entityId = entityIdRuns[0].ToString();
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(entityId))
                                                            Console.Out.WriteLine("here");
                                                    }

                                                    var videoIdRuns = run.Parent.Parent.Parent.Parent.Parent.Parent.Parent
                                                                          .Descendants()
                                                                          .Where(t => t.Type == JTokenType.Property && ((JProperty)t).Name == "videoId")
                                                                          .Select(p => ((JProperty)p).Value).ToList();

                                                    if (videoIdRuns.Count > 0)
                                                    {
                                                        videoId = videoIdRuns[0].ToString();
                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(entityId))
                                                            Console.Out.WriteLine("here");
                                                    }
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
                            albumSimilarity >= matchSuccessMinimum &&
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

                    return IsPresentInArtistsCache(cookieValue,
                                                   artist,
                                                   album,
                                                   track,
                                                   matchSuccessMinimum,
                                                   parallel,
                                                   out entityId,
                                                   out videoId);
                }
            }
            catch (Exception e)
            {
                var _ = e;
#if DEBUG
                Console.Out.WriteLine("IsSongUploaded: " + e.Message);
#endif
                return false;
            }
        }

        private static void DetermineSimilarity_Parallel(
            SearchResultContext.Run[] runArray,
            string artist,
            string album,
            string track,
            ref float artistSimilarity,
            ref float albumSimilarity,
            ref float trackSimilarity,
            float matchSuccessMinimum)
        {
            float _artistSimilarity = 0.0f;
            float _albumSimilarity = 0.0f;
            float _trackSimilarity = 0.0f;

            Parallel.ForEach(runArray, (runElement) =>
            {
                if (runElement.text == null)
                    runElement.text = string.Empty;

                float __artistSimilarity = 0.0f;
                float __albumSimilartity = 0.0f;
                float __trackSimilarity = 0.0f;

                Parallel.For(0, 3, (i, state) =>
                {
                    switch (i)
                    {
                        case 0:
                            if (__artistSimilarity < matchSuccessMinimum)
                                __artistSimilarity = Levenshtein.Similarity(runElement.text.UnQuote(), artist.UnQuote());
                            break;
                        case 1:
                            if (__albumSimilartity < matchSuccessMinimum)
                                __albumSimilartity = Levenshtein.Similarity(runElement.text.UnQuote(), album.UnQuote());
                            break;
                        case 2:
                            if (__trackSimilarity < matchSuccessMinimum)
                                __trackSimilarity = Levenshtein.Similarity(runElement.text.UnQuote(), track.UnQuote());
                            break;
                        default:
                            break;
                    }
                });

                if (_artistSimilarity < __artistSimilarity)
                    _artistSimilarity = __artistSimilarity;

                if (_albumSimilarity < __albumSimilartity)
                    _albumSimilarity = __albumSimilartity;

                if (_trackSimilarity < __trackSimilarity)
                    _trackSimilarity = __trackSimilarity;
            });

            artistSimilarity = _artistSimilarity;
            albumSimilarity = _albumSimilarity;
            trackSimilarity = _trackSimilarity;
        }

        private static void DetermineSimilarity_Standard(
            SearchResultContext.Run[] runArray,
            string artist,
            string album,
            string track,
            ref float artistSimilarity,
            ref float albumSimilarity,
            ref float trackSimilarity,
            float matchSuccessMinimum)
        {
            foreach (var runElement in runArray)
            {
                ThreadHelper.SafeSleep(5);

                if (runElement.text == null)
                    runElement.text = string.Empty;

                if (artistSimilarity < matchSuccessMinimum)
                    artistSimilarity = Levenshtein.Similarity(runElement.text.UnQuote(), artist.UnQuote());

                if (albumSimilarity < matchSuccessMinimum)
                    albumSimilarity = Levenshtein.Similarity(runElement.text.UnQuote(), album.UnQuote());

                if (trackSimilarity < matchSuccessMinimum)
                    trackSimilarity = Levenshtein.Similarity(runElement.text.UnQuote(), track.UnQuote());
            }
        }

        private static bool IsPresentInArtistsCache(
            string cookieValue,
            string artist,
            string album,
            string track,
            float matchSuccessMinimum,
            bool parallel,
            out string entityId,
            out string videoId)
        {
            entityId = string.Empty;
            videoId = string.Empty;

            if (ArtistCache != null)
            {
                if (parallel)
                {
                    IsPresentInArtistsCache_Parallel(cookieValue,
                                                     artist,
                                                     album,
                                                     track,
                                                     matchSuccessMinimum,
                                                     out entityId,
                                                     out videoId);
                }
                else
                {
                    IsPresentInArtistsCache_Standard(cookieValue,
                                                     artist,
                                                     album,
                                                     track,
                                                     matchSuccessMinimum,
                                                     out entityId,
                                                     out videoId);
                }

                if (!string.IsNullOrEmpty(entityId))
                    return true;
            }

            return false;
        }

        private static void IsPresentInArtistsCache_Parallel(
            string cookieValue,
            string artist,
            string album,
            string track,
            float matchSuccessMinimum,
            out string entityId,
            out string videoId)
        {
            string tempEntityId = string.Empty;
            string tempVideoId = string.Empty;

            Parallel.ForEach(ArtistCache.Artists.AsParallel(), (artistCacheItem, stateA, indexA) =>
            {
                float artistSimilarity = Levenshtein.Similarity(artist.UnQuote(), artistCacheItem.ArtistName.UnQuote());
                if (artistSimilarity >= matchSuccessMinimum)
                {
                    if (artistCacheItem.AlbumSongCollection.Songs.Count == 0)
                        artistCacheItem.AlbumSongCollection = GetArtistSongs(cookieValue, artistCacheItem.BrowseId);

                    bool trackFound = false;
                    Parallel.ForEach(artistCacheItem.AlbumSongCollection.Songs.AsParallel(), (songCacheItem, stateB, indexB) =>
                    {
                        if (!trackFound)
                        {
                            float trackSimilarity = Levenshtein.Similarity(track.UnQuote(), songCacheItem.Title.UnQuote());
                            if (trackSimilarity > matchSuccessMinimum)
                            {
                                tempEntityId = songCacheItem.EntityId;
                                tempVideoId = songCacheItem.VideoId;
                                trackFound = true;
                                stateB.Break();
                            }
                            else
                            {
                                string modTrack = GetCleanedTrackName(track, artist, album);
                                string ytTitle = GetCleanedTrackName(songCacheItem.Title, artist, album);

                                trackSimilarity = Levenshtein.Similarity(modTrack.UnQuote(), ytTitle.UnQuote());
                                if (trackSimilarity > matchSuccessMinimum)
                                {
                                    tempEntityId = songCacheItem.EntityId;
                                    tempVideoId = songCacheItem.VideoId;
                                    trackFound = true;
                                    stateB.Break();
                                }
                            }
                        }
                    });

                    if (trackFound)
                        stateA.Break();
                }
            });

            entityId = tempEntityId;
            videoId = tempVideoId;
        }

        private static void IsPresentInArtistsCache_Standard(
            string cookieValue,
            string artist,
            string album,
            string track,
            float matchSuccessMinimum,
            out string entityId,
            out string videoId)
        {
            entityId = string.Empty;
            videoId = string.Empty;

            foreach (var artistCacheItem in ArtistCache.Artists)
            {
                ThreadHelper.SafeSleep(1);
                float artistSimilarity = Levenshtein.Similarity(artist.UnQuote(), artistCacheItem.ArtistName.UnQuote());
                if (artistSimilarity >= matchSuccessMinimum)
                {
                    if (artistCacheItem.AlbumSongCollection.Songs.Count == 0)
                        artistCacheItem.AlbumSongCollection = GetArtistSongs(cookieValue, artistCacheItem.BrowseId);

                    bool trackFound = false;

                    foreach (var songCacheItem in artistCacheItem.AlbumSongCollection.Songs)
                    {
                        ThreadHelper.SafeSleep(1);
                        if (!trackFound)
                        {
                            float trackSimilarity = Levenshtein.Similarity(track.UnQuote(), songCacheItem.Title.UnQuote());
                            if (trackSimilarity > matchSuccessMinimum)
                            {
                                entityId = songCacheItem.EntityId;
                                videoId = songCacheItem.VideoId;
                                trackFound = true;
                                break;
                            }
                            else
                            {
                                string modTrack = GetCleanedTrackName(track, artist, album);
                                string ytTitle = GetCleanedTrackName(songCacheItem.Title, artist, album);

                                trackSimilarity = Levenshtein.Similarity(modTrack.UnQuote(), ytTitle.UnQuote());
                                if (trackSimilarity > matchSuccessMinimum)
                                {
                                    entityId = songCacheItem.EntityId;
                                    videoId = songCacheItem.VideoId;
                                    trackFound = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (trackFound)
                        break;
                }
            }
        }

        private static string GetCleanedTrackName(string trackName, string artist, string album)
        {
            trackName = trackName ?? "";
            artist = artist ?? "";
            album = album ?? "";

            if (!string.IsNullOrEmpty(trackName) && trackName.Length > 2 && trackName.Substring(0, 2).IsNumeric())
            {
                try
                {
                    trackName = trackName.Substring(2).Trim();
                    if (trackName.StartsWith("_") || trackName.StartsWith("-") || trackName.StartsWith("."))
                        trackName = trackName.Substring(1).Trim();
                }
                catch { }
            }

            trackName = Regex.Replace(trackName, @"(\d)+-(\d)+", "").Trim();
            trackName = Regex.Replace(trackName, @"(?<=\[)(.*?)(?=\])", "").Replace("[]", "").Replace("  ", " ").Trim();
            trackName = Regex.Replace(trackName, @"(?<=\()(.*?)(?=\))", "").Replace("()", "").Replace("  ", " ").Trim();

            if (trackName.ToLower().Contains(artist.ToLower()))
                trackName = Regex.Replace(trackName, artist, "").Trim();

            if (trackName.ToLower().Contains(album.ToLower()))
                trackName = Regex.Replace(trackName, album, "").Trim();

            return trackName;
        }
    }
}
