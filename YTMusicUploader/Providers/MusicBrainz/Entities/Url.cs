
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// A URL in MusicBrainz is a specific entity representing a regular internet Uniform Resource Locator.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/URL"/>
    [DataContract(Name = "url")]
    public class Url
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        [DataMember(Name = "resource")]
        public string Resource { get; set; }
    }
}
