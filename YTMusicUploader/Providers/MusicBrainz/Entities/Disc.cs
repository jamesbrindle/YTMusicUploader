
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Name = "disc")]
    public class Disc
    {
        /// <summary>
        /// Gets or sets the MusicBrainz id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the number of sectors.
        /// </summary>
        [DataMember(Name = "sectors")]
        public int Sectors { get; set; }

        /// <summary>
        /// Gets or sets the track offsets.
        /// </summary>
        [DataMember(Name = "offsets")]
        public List<int> Offsets { get; set; }
    }
}
