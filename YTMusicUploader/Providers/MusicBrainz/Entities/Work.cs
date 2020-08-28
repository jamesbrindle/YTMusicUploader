
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// In MusicBrainz terminology, a work is a distinct intellectual or artistic creation,
    /// which can be expressed in the form of one or more audio recordings.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Work"/>
    [DataContract(Name = "work")]
    public partial class Work
    {
        #region Properties

        /// <summary>
        /// Gets or sets the MusicBrainz id.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        [DataMember(Name = "language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the ISW code.
        /// </summary>
        [DataMember(Name = "iswc")]
        public string ISWC { get; set; }

        /// <summary>
        /// Gets or sets the disambiguation.
        /// </summary>
        [DataMember(Name = "disambiguation")]
        public string Disambiguation { get; set; }

        #endregion

        #region Subqueries

        /// <summary>
        /// Gets or sets a list of relations associated to this work.
        /// </summary>
        /// <example>
        /// var e = await Work.GetAsync(mbid, "url-rels");
        /// </example>
        [DataMember(Name = "relations")]
        public List<Relation> Relations { get; set; }

        #endregion
    }
}
