
namespace YTMusicUploader.MusicBrainz.API
{
    using System;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Helper class to build MusicBrainz webservice urls.
    /// </summary>
    class UrlBuilder
    {
        private const string LookupTemplate = "{0}/{1}/?inc={2}&fmt=json";
        private const string BrowseTemplate = "{0}?{1}={2}&limit={3}&offset={4}&inc={5}&fmt=json";
        private const string SearchTemplate = "{0}?query={1}&limit={2}&offset={3}&fmt=json";

        bool validate;

        public UrlBuilder(bool clientSideValidation)
        {
            validate = clientSideValidation;
        }

        /// <summary>
        /// Creates a webservice lookup template.
        /// </summary>
        public string CreateLookupUrl(string entity, string mbid, params string[] inc)
        {
            return CreateLookupUrl(entity, mbid, string.Join("+", inc));
        }

        /// <summary>
        /// Creates a webservice lookup template.
        /// </summary>
        private string CreateLookupUrl(string entity, string mbid, string inc)
        {
            return string.Format(LookupTemplate, entity, mbid, inc);
        }

        /// <summary>
        /// Creates a webservice browse template.
        /// </summary>
        public string CreateBrowseUrl(string entity, string relatedEntity, string mbid, int limit, int offset, params string[] inc)
        {
            return CreateBrowseUrl(entity, relatedEntity, mbid, limit, offset, string.Join("+", inc));
        }

        /// <summary>
        /// Creates a webservice browse template.
        /// </summary>
        public string CreateBrowseUrl(string entity, string relatedEntity, string mbid, string type, string status,
            int limit, int offset, params string[] inc)
        {
            var url = CreateBrowseUrl(entity, relatedEntity, mbid, limit, offset, string.Join("+", inc));

            if (validate && !ValidateBrowseParam(Resources.Constants.BrowseStatus, status))
            {
                throw new ArgumentException(string.Format(Resources.Messages.InvalidQueryValue, status, "status"));
            }

            if (validate && !ValidateBrowseParam(Resources.Constants.BrowseType, type))
            {
                throw new ArgumentException(string.Format(Resources.Messages.InvalidQueryValue, type, "type"));
            }

            if (!string.IsNullOrEmpty(type))
            {
                url += "&type=" + type;
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            return url;
        }

        /// <summary>
        /// Creates a webservice browse template.
        /// </summary>
        private string CreateBrowseUrl(string entity, string relatedEntity, string mbid, int limit, int offset, string inc)
        {
            return string.Format(BrowseTemplate, entity, relatedEntity, mbid, limit, offset, inc);
        }

        /// <summary>
        /// Creates a webservice search template.
        /// </summary>
        public string CreateSearchUrl(string entity, string query, int limit, int offset)
        {
            query = WebUtility.UrlEncode(query);

            return string.Format(SearchTemplate, entity, query, limit, offset);
        }

        private bool ValidateBrowseParam(string availableParams, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true; // Irgnore, if no value specified.
            }

            if (value.IndexOf('|') > 0)
            {
                return value.Split('|').All(s => availableParams.IndexOf("+" + s + "+") >= 0);
            }

            return availableParams.IndexOf("+" + value + "+") >= 0;
        }
    }
}
