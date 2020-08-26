
namespace YTMusicUploader.MusicBrainz.API.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// A release group is used to group several different releases into a single logical entity.
    /// </summary>
    /// <see href="https://musicbrainz.org/doc/Release_Group"/>
    [DataContract(Name = "release-group")]
    public partial class ReleaseGroup
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
        /// Gets or sets the first release date.
        /// </summary>
        [DataMember(Name = "first-release-date")]
        public string FirstReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the primary type.
        /// </summary>
        [DataMember(Name = "primary-type")]
        public string PrimaryType { get; set; }

        /// <summary>
        /// Gets or sets the secondary types.
        /// </summary>
        [DataMember(Name = "secondary-types")]
        public List<string> SecondaryTypes { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        [DataMember(Name = "rating")]
        public Rating Rating { get; set; }

        #endregion

        #region Subqueries

        /// <summary>
        /// Gets or sets a list of artists associated to this release-group.
        /// </summary>
        /// <example>
        /// var e = await ReleaseGroup.GetAsync(mbid, "artists");
        /// </example>
        [DataMember(Name = "artist-credit")]
        public List<NameCredit> Credits { get; set; }

        /// <summary>
        /// Gets or sets a list of releases associated to this release-group.
        /// </summary>
        /// <example>
        /// var e = await ReleaseGroup.GetAsync(mbid, "releases");
        /// </example>
        [DataMember(Name = "releases")]
        public List<Release> Releases { get; set; }

        /// <summary>
        /// Gets or sets a list of tags associated to this release-group.
        /// </summary>
        /// <example>
        /// var e = await ReleaseGroup.GetAsync(mbid, "tags");
        /// </example>
        [DataMember(Name = "tags")]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Gets or sets a list of relations associated to this release-group.
        /// </summary>
        /// <example>
        /// var e = await ReleaseGroup.GetAsync(mbid, "url-rels");
        /// </example>
        [DataMember(Name = "relations")]
        public List<Relation> Relations { get; set; }

        #endregion
    }
}
