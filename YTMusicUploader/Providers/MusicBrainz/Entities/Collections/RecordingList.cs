
namespace YTMusicUploader.MusicBrainz.API.Entities.Collections
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// List of recordings returned by MusicBrainz search requests.
    /// </summary>
    [DataContract]
    public class RecordingList : QueryResult
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "recordings")]
        public List<Recording> Items { get; set; }
    }

    // NOTE: for MusicBrainz ws/3 this additional class might no longer be necessary.
    //       See https://tickets.metabrainz.org/browse/MBS-9731

    /// <summary>
    /// List of recordings returned by MusicBrainz browse requests.
    /// </summary>
    [DataContract]
    internal class RecordingListBrowse
    {
        /// <summary>
        /// Gets or sets the list of artists.
        /// </summary>
        [DataMember(Name = "recordings")]
        public List<Recording> Items { get; set; }

        // NOTE: hide members of the base class to make serialization work

        /// <summary>
        /// Gets or sets the total list items count.
        /// </summary>
        [DataMember(Name = "recording-count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the list offset.
        /// </summary>
        [DataMember(Name = "recording-offset")]
        public int Offset { get; set; }
    }
}
