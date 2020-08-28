
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A MusicBrainz release represents the unique release (i.e. issuing) of a product on a specific
    /// date with specific release information such as the country, label, barcode and packaging.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Release"/>
    [DataContract(Name = "release")]
    public partial class Release
    {
        #region Properties

        /// <summary>
        /// Gets or sets the score (only available in search results).
        /// </summary>
        [DataMember(Name = "score")]
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the MusicBrainz id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the quality.
        /// </summary>
        [DataMember(Name = "quality")]
        public string Quality { get; set; }

        /// <summary>
        /// Gets or sets the text-representation.
        /// </summary>
        [DataMember(Name = "text-representation")]
        public TextRepresentation TextRepresentation { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        [DataMember(Name = "country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the barcode.
        /// </summary>
        [DataMember(Name = "barcode")]
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the release-group.
        /// </summary>
        [DataMember(Name = "release-group")]
        public ReleaseGroup ReleaseGroup { get; set; }

        /// <summary>
        /// Gets or sets the cover-art-archive.
        /// </summary>
        [DataMember(Name = "cover-art-archive")]
        public CoverArtArchive CoverArtArchive { get; set; }

        #endregion

        #region Subqueries

        /// <summary>
        /// Gets or sets a list of artists associated to this release.
        /// </summary>
        /// <example>
        /// var e = await Release.GetAsync(mbid, "artists");
        /// </example>
        [DataMember(Name = "artist-credit")]
        public List<NameCredit> Credits { get; set; }

        /// <summary>
        /// Gets or sets a list of labels associated to this release.
        /// </summary>
        /// <example>
        /// var e = await Release.GetAsync(mbid, "labels");
        /// </example>
        [DataMember(Name = "label-info")]
        public List<LabelInfo> Labels { get; set; }

        /// <summary>
        /// Gets or sets a list of media/tracks associated to this release.
        /// </summary>
        /// <example>
        /// var e = await Release.GetAsync(mbid, "recordings");
        /// </example>
        [DataMember(Name = "media")]
        public List<Medium> Media { get; set; }

        /// <summary>
        /// Gets or sets a list of relations associated to this release.
        /// </summary>
        /// <example>
        /// var e = await Release.GetAsync(mbid, "url-rels");
        /// </example>
        [DataMember(Name = "relations")]
        public List<Relation> Relations { get; set; }

        #endregion
    }
}
