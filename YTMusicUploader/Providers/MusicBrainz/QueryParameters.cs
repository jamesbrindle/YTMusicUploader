namespace YTMusicUploader.MusicBrainz.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using YTMusicUploader.MusicBrainz.API.Entities;

    /// <summary>
    /// Helper for building MusicBrainz query strings.
    /// </summary>
    /// <typeparam name="T">The entity type to search for.</typeparam>
    /// <remarks>
    /// See https://musicbrainz.org/doc/Development/XML_Web_Service/Version_2/Search
    /// </remarks>
    public class QueryParameters<T> : IEnumerable<QueryParameters<T>.Node>
    {
        private readonly List<Node> nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameters{T}"/> class.
        /// </summary>
        public QueryParameters()
        {
            nodes = new List<Node>();
        }

        /// <summary>
        /// Add a field to the query paramaters.
        /// </summary>
        /// <param name="key">The field key.</param>
        /// <param name="value">The field value.</param>
        /// <param name="negate">Negate the field (will result in 'AND NOT key:value')</param>
        public void Add(string key, string value, bool negate = false)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(string.Format(Resources.Messages.MissingParameter, "key"));
            }

            if (!Validate(key))
            {
                throw new Exception(string.Format(Resources.Messages.InvalidQueryParameter, key));
            }

            nodes.Add(new Node(key, value, negate));
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        public override string ToString()
        {
            return BuildQueryString();
        }

        private string BuildQueryString()
        {
            var sb = new StringBuilder();

            foreach (var item in nodes)
            {
                // Append operator.
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }

                // Negate operator.
                if (item.Negate)
                {
                    sb.Append("NOT ");
                }

                // Append key.
                sb.Append(item.Key);
                sb.Append(':');

                // Append value.
                string value = item.Value;

                if (value.Contains("AND") || value.Contains("OR"))
                {
                    if (!value.StartsWith("("))
                    {
                        // The search value appears to be an expression, so enclose it in brackets.
                        sb.Append("(" + value + ")");
                    }
                    else
                    {
                        sb.Append(value);
                    }
                }
                else if (value.Contains(" ") && !value.StartsWith("\""))
                {
                    // The search value contains whitespace but isn't quoted.
                    sb.Append("\"" + value + "\"");
                }
                else
                {
                    // The search value is already quoted or doesn't need quoting, so just append it.
                    sb.AppendFormat(value);
                }
            }

            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        private bool Validate(string key)
        {
            key = "-" + key + "-";

            if (typeof(T) == typeof(Artist))
            {
                return Resources.Constants.ArtistQueryParams.IndexOf(key) >= 0;
            }

            if (typeof(T) == typeof(Recording))
            {
                return Resources.Constants.RecordingQueryParams.IndexOf(key) >= 0;
            }

            if (typeof(T) == typeof(Release))
            {
                return Resources.Constants.ReleaseQueryParams.IndexOf(key) >= 0;
            }

            if (typeof(T) == typeof(ReleaseGroup))
            {
                return Resources.Constants.ReleaseGroupQueryParams.IndexOf(key) >= 0;
            }

            return false;
        }

        public class Node
        {
            /// <summary>
            /// Gets the key of the node.
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Gets the value of the node.
            /// </summary>
            public string Value { get; private set; }

            /// <summary>
            /// Gets a value indicating whether the node should be negated.
            /// </summary>
            public bool Negate { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            public Node(string key, string value, bool negate)
            {
                this.Key = key;
                this.Value = value;
                this.Negate = negate;
            }
        }
    }
}
