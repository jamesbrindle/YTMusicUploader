
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Artist credits indicate who is the main credited artist (or artists) for releases, release groups, tracks and recordings.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Artist_Credits"/>
    [DataContract(Name = "name-credit")]
    public class NameCredit
    {
        /// <summary>
        /// Gets or sets the joinphrase.
        /// </summary>
        [DataMember(Name = "joinphrase")]
        public string JoinPhrase { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        [DataMember(Name = "artist")]
        public Artist Artist { get; set; }
    }
}