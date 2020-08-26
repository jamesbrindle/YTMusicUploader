
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract(Name = "label-info")]
    public class LabelInfo
    {
        /// <summary>
        /// Gets or sets the catalog-number.
        /// </summary>
        [DataMember(Name = "catalog-number")]
        public string CatalogNumber { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        [DataMember(Name = "label")]
        public Label Label { get; set; }
    }
}
