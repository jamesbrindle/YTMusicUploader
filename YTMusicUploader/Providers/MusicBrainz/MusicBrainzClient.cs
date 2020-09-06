namespace YTMusicUploader.MusicBrainz.API
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using YTMusicUploader.MusicBrainz.API.Cache;
    using YTMusicUploader.MusicBrainz.API.Services;

    /// <summary>
    /// MusicBrainz client.
    /// </summary>
    public sealed class MusicBrainzClient : IDisposable
    {
        /// <summary>
        /// The default address of the MusicBrainz webservice.
        /// </summary>
        public static string ServiceBaseAddress = Global.MusicBrainzBaseUrl;

        private static string UserAgent = Global.MusicBrainzUserAgent;

        #region Public services

        /// <summary>
        /// Gets the artists entity service.
        /// </summary>
        public IArtistService Artists { get; }

        /// <summary>
        /// Gets the recordings entity service.
        /// </summary>
        public IRecordingService Recordings { get; }

        /// <summary>
        /// Gets the releases entity service.
        /// </summary>
        public IReleaseService Releases { get; }

        /// <summary>
        /// Gets the release-groups entity service.
        /// </summary>
        public IReleaseGroupService ReleaseGroups { get; }

        /// <summary>
        /// Gets the work entity service.
        /// </summary>
        public IWorkService Work { get; }

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="IRequestCache"/> implementation.
        /// </summary>
        public IRequestCache Cache { get; set; }

        private HttpClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicBrainzClient"/> class.
        /// </summary>
        public MusicBrainzClient()
            : this(ServiceBaseAddress, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicBrainzClient"/> class.
        /// </summary>
        /// <param name="proxy">The <see cref="IWebProxy"/> used to connect to the webservice.</param>
        public MusicBrainzClient(IWebProxy proxy)
            : this(ServiceBaseAddress, proxy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicBrainzClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address of the webservice (default = <see cref="ServiceBaseAddress"/>).</param>
        public MusicBrainzClient(string baseAddress)
            : this(baseAddress, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MusicBrainzClient"/> class.
        /// </summary>
        /// <param name="baseAddress">The base address of the webservice (default = <see cref="ServiceBaseAddress"/>).</param>
        /// <param name="proxy">The <see cref="IWebProxy"/> used to connect to the webservice.</param>
        public MusicBrainzClient(string baseAddress, IWebProxy proxy)
        {
            var urlBuilder = new UrlBuilder(true);

            Artists = new ArtistService(this, urlBuilder);
            Recordings = new RecordingService(this, urlBuilder);
            Releases = new ReleaseService(this, urlBuilder);
            ReleaseGroups = new ReleaseGroupService(this, urlBuilder);
            Work = new WorkService(this, urlBuilder);

            client = CreateHttpClient(new Uri(baseAddress), true, proxy);
        }

        /// <summary>
        /// Disposes the HTTP client.
        /// </summary>
        public void Dispose()
        {
            client.Dispose();
        }

        [DataContract]
        public class ResponseError
        {
            [DataMember(Name = "error")]
            public string Message { get; set; }
        }

        internal async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var cache = Cache ?? NullCache.Default;

                var serializer = new DataContractJsonSerializer(typeof(T));

                if (await cache.TryGetCachedItem(url, out Stream stream).ConfigureAwait(false))
                {
                    var result = (T)serializer.ReadObject(stream);
                    stream.Close();
                    return result;
                }

                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw CreateWebserviceException(response.StatusCode, url, stream);
                    }

                    await cache.Add(url, stream).ConfigureAwait(false);

                    return (T)serializer.ReadObject(stream);
                }
            }
            catch
            {
                throw;
            }
        }

        private WebServiceException CreateWebserviceException(HttpStatusCode status, string url, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(ResponseError));

            var error = (ResponseError)serializer.ReadObject(stream);

            return new WebServiceException(error.Message, status, url);
        }

        private HttpClient CreateHttpClient(Uri baseAddress, bool automaticDecompression, IWebProxy proxy)
        {
            var handler = new HttpClientHandler();

            if (proxy != null)
            {
                handler.Proxy = proxy;
                handler.UseProxy = true;
            }

            if (automaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler);

            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            client.BaseAddress = baseAddress;

            return client;
        }
    }
}
