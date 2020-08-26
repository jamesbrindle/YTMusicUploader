
namespace YTMusicUploader.MusicBrainz.API.Cache
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// A simple cache interface.
    /// </summary>
    public interface IRequestCache
    {
        /// <summary>
        /// Add a request and its response to the cache.
        /// </summary>
        Task Add(string request, Stream response);

        /// <summary>
        /// Add a request and its response to the cache.
        /// </summary>
        /// <returns>True, if a cache entry matching the request was found.</returns>
        Task<bool> TryGetCachedItem(string request, out Stream stream);
    }
}
