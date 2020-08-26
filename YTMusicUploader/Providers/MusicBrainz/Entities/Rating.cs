
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Rating information.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Rating_System"/>
    [DataContract(Name = "rating")]
    public class Rating
    {
        /// <summary>
        /// Gets or sets the votes-count.
        /// </summary>
        [DataMember(Name = "votes-count")]
        public int VotesCount { get; set; }

        /// <summary>
        /// Gets or sets the rating value.
        /// </summary>
        [DataMember(Name = "value")]
        public double? Value { get; set; }
    }
}
