
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract(Name = "text-representation")]
    public class TextRepresentation
    {
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        [DataMember(Name = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        [DataMember(Name = "script")]
        public string Script { get; set; }
    }
}
