
namespace YTMusicUploader.MusicBrainz.API.Entities.Collections
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// List of releases returned by MusicBrainz search requests.
    /// </summary>
    [DataContract]
    public class ReleaseList : QueryResult
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "releases")]
        public List<Release> Items { get; set; }
    }

    // NOTE: for MusicBrainz ws/3 this additional class might no longer be necessary.
    //       See https://tickets.metabrainz.org/browse/MBS-9731

    /// <summary>
    /// List of releases returned by MusicBrainz browse requests.
    /// </summary>
    [DataContract]
    internal class ReleaseListBrowse
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "releases")]
        public List<Release> Items { get; set; }

        /// <summary>
        /// Gets or sets the total list items count.
        /// </summary>
        [DataMember(Name = "release-count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the list offset.
        /// </summary>
        [DataMember(Name = "release-offset")]
        public int Offset { get; set; }
    }
}
