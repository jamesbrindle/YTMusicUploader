
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// In MusicBrainz, a track is the way a recording is represented on a particular
    /// release (or, more exactly, on a particular medium).
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Track"/>
    [DataContract(Name = "track")]
    public class Track
    {
        /// <summary>
        /// Gets or sets the MusicBrainz id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        [DataMember(Name = "number")]
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        [DataMember(Name = "position")]
        public int Position { get; set; }
        
        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [DataMember(Name = "length")]
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the recording.
        /// </summary>
        [DataMember(Name = "recording")]
        public Recording Recording { get; set; }

    }
}
