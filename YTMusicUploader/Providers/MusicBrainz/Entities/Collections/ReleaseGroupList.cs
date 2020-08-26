
namespace YTMusicUploader.MusicBrainz.API.Entities.Collections
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// List of release-groups returned by MusicBrainz search requests.
    /// </summary>
    [DataContract]
    public class ReleaseGroupList : QueryResult
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "release-groups")]
        public List<ReleaseGroup> Items { get; set; }
    }

    // NOTE: for MusicBrainz ws/3 this additional class might no longer be necessary.
    //       See https://tickets.metabrainz.org/browse/MBS-9731

    /// <summary>
    /// List of release-groups returned by MusicBrainz browse requests.
    /// </summary>
    [DataContract]
    internal class ReleaseGroupListBrowse
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "release-groups")]
        public List<ReleaseGroup> Items { get; set; }

        // NOTE: hide members of the base class to make serialization work

        /// <summary>
        /// Gets or sets the total list items count.
        /// </summary>
        [DataMember(Name = "release-group-count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the list offset.
        /// </summary>
        [DataMember(Name = "release-group-offset")]
        public int Offset { get; set; }
    }
}
