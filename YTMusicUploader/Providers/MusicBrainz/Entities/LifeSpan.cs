
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract(Name = "life-span")]
    public class LifeSpan
    {
        /// <summary>
        /// Gets or sets the begin date.
        /// </summary>
        [DataMember(Name = "begin")]
        public string Begin { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        [DataMember(Name = "end")]
        public string End { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the life-span ended or not.
        /// </summary>
        [DataMember(Name = "ended")]
        public bool? Ended { get; set; }
    }
}
