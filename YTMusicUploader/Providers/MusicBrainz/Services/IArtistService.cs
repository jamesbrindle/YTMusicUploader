namespace YTMusicUploader.MusicBrainz.API.Services
{
    using System.Threading.Tasks;
    using YTMusicUploader.MusicBrainz.API.Entities;
    using YTMusicUploader.MusicBrainz.API.Entities.Collections;

    /// <summary>
    /// Interface defining the artist service.
    /// </summary>
    public interface IArtistService
    {
        /// <summary>
        /// Lookup an artist in the MusicBrainz database.
        /// </summary>
        /// <param name="id">The artist MusicBrainz id.</param>
        /// <param name="inc">A list of entities to include (subqueries).</param>
        /// <returns></returns>
        Task<Artist> GetAsync(string id, params string[] inc);

        /// <summary>
        /// Search for an artist in the MusicBrainz database, matching the given query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <param name="limit">The maximum number of artists to return (default = 25).</param>
        /// <param name="offset">The offset to the artists list (enables paging, default = 0).</param>
        /// <returns></returns>
        Task<ArtistList> SearchAsync(string query, int limit = 25, int offset = 0);

        /// <summary>
        /// Search for an artist in the MusicBrainz database, matching the given query.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <param name="limit">The maximum number of artists to return (default = 25).</param>
        /// <param name="offset">The offset to the artists list (enables paging, default = 0).</param>
        /// <returns></returns>
        Task<ArtistList> SearchAsync(QueryParameters<Artist> query, int limit = 25, int offset = 0);

        /// <summary>
        /// Browse all the artists in the MusicBrainz database, which are directly linked to the entity with given id.
        /// </summary>
        /// <param name="entity">The name of the related entity.</param>
        /// <param name="id">The id of the related entity.</param>
        /// <param name="limit">The maximum number of artists to return (default = 25).</param>
        /// <param name="offset">The offset to the artists list (enables paging, default = 0).</param>
        /// <param name="inc">A list of entities to include (subqueries).</param>
        /// <returns></returns>
        Task<ArtistList> BrowseAsync(string entity, string id, int limit = 25, int offset = 0, params string[] inc);
    }
}
