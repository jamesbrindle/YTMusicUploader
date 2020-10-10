using JBToolkit;
using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Dialogue Form to delete song / albums from YT Music
    /// </summary>
    public partial class ManageYTMusic : OptimisedMetroForm
    {
        /// <summary>
        /// Form to see and delete music currently uploaded to YouTube music
        /// </summary>
        private MainForm MainForm { get; set; }
        private bool ChangesMade { get; set; } = false;

        /// <summary>
        /// Form to manage (see and delete) music currently uploaded to YouTube music
        /// </summary>
        public ManageYTMusic(MainForm mainForm) : base(formResizable: true)
        {
            MainForm = mainForm;
            InitializeComponent();
        }

        private void ManageYTMusic_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ClearFields();
            SetTextBoxCueBanner(tbSearchArtists, "Press 'Enter' for next match.");

            DisableAllActionButtons(true);
            SetTreeViewEnabled(false);
            ShowPreloader(true);

            if (Requests.ArtistCache == null ||
                Requests.ArtistCache.Artists == null ||
                Requests.ArtistCache.Artists.Count == 0)
            {
                new Thread((ThreadStart)delegate { GetArtists(); }).Start();
            }
            else
            {
                new Thread((ThreadStart)delegate
                {
                    AppendUpdatesText("Loading artists from cache...", ColourHelper.HexStringToColor("#0f0466"));
                    BindArtists(false);
                }).Start();
            }
        }

        private void ClearFields()
        {
            lblArtistTitle.Text = "Nothing selected";
            lblAlbumTitle.Text = "-";
            lblSongTitle.Text = "-";
            lblDuration.Text = "-";
            lblDatabaseExistence.Text = "-";
            lblMbId.Text = "-";
            lblUploaded.Text = "-";

            tvUploads.AfterSelect -= TvUploads_AfterSelect;
            tbSearchArtists.TextChanged -= TbSearchArtists_TextChanged;
            tbSearchArtists.KeyDown -= TbSearchArtists_KeyDown;

            tbSearchArtists.Text = "";

            tvUploads.AfterSelect += TvUploads_AfterSelect;
            tbSearchArtists.TextChanged += TbSearchArtists_TextChanged;
            tbSearchArtists.KeyDown += TbSearchArtists_KeyDown;           
        }

        private void GetArtists()
        {
            DisableAllActionButtons(true);
            SetTreeViewEnabled(false);
            ShowPreloader(true);
            AppendUpdatesText("Fetching artists...", ColourHelper.HexStringToColor("#0f0466"));
            Requests.ArtistCache = Requests.GetArtists(MainForm.Settings.AuthenticationCookie);
            BindArtists(true);
            ClearFields();
        }

        private void BindArtists(bool showFetchedMessage = true)
        {
            var artistNodes = new List<TreeNode>();
            foreach (var artist in Requests.ArtistCache.Artists)
            {
                var artistNode = new TreeNode
                {
                    Name = artist.BrowseId,
                    Text = artist.ArtistName,
                    Tag = Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Artist,
                        ArtistTitle = artist.ArtistName,
                        EntityOrBrowseId = artist.BrowseId
                    }
                };

                artistNodes.Add(artistNode);

                // v1.4.3: This block can take too. Don't bother...

                //if (artist.AlbumSongCollection != null &&
                //    artist.AlbumSongCollection.Albums != null &&
                //    artist.AlbumSongCollection.Albums.Count > 0)
                //{
                //    BindAlbumNodesFromArtistBind(artistNode, artist.AlbumSongCollection, false, showFetchedMessage);
                //}
            }

            AddArtistNodesToTree(artistNodes);

            string artistText = "artists";
            int artistCount = Requests.ArtistCache.Artists.Count;

            if (artistCount == 1)
                artistText = "artist";

            if (showFetchedMessage)
                AppendUpdatesText($"Fetched {artistCount} {artistText}.",
                                  ColourHelper.HexStringToColor("#0d5601"));

            ShowPreloader(false);
            SetTreeViewEnabled(true);
            DisableAllActionButtons(false);
            ResumeDrawing(tvUploads);
        }

        private void GetAlbums(TreeNode artistNode, string artist, bool isDeleting = false)
        {
            DisableAllActionButtons(true);
            SetTreeViewEnabled(false);
            ShowPreloader(true);

            if (!isDeleting)
            {
                AppendUpdatesText($"Fetching songs for arists: {artist}...", ColourHelper.HexStringToColor("#0f0466"));
                new Thread((ThreadStart)delegate
                {
                    var albumSongCollection = Requests.GetArtistSongs(MainForm.Settings.AuthenticationCookie, artistNode.Name);
                    Requests.ArtistCache.Artists.Where(a => a.BrowseId == artistNode.Name).FirstOrDefault().AlbumSongCollection = albumSongCollection;
                    BindAlbumNodesFromSelect(artistNode, albumSongCollection, !isDeleting, !isDeleting, isDeleting);
                }).Start();
            }
            else
            {
                var albumSongCollection = Requests.GetArtistSongs(MainForm.Settings.AuthenticationCookie, artistNode.Name);
                Requests.ArtistCache.Artists.Where(a => a.BrowseId == artistNode.Name).FirstOrDefault().AlbumSongCollection = albumSongCollection;
                BindAlbumNodesFromSelect(artistNode, albumSongCollection, !isDeleting, !isDeleting, isDeleting);
            }
        }

        private void BindAlbumNodesFromSelect(
            TreeNode artistNode,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true,
            bool isDeleting = false)
        {
            SetTreeViewEnabled(false);
            if (artistNode != null)
            {
                var albumNodes = new List<TreeNode>();
                foreach (var album in albumSongCollection.Albums)
                {
                    var songNodes = new List<TreeNode>();
                    string releaseMbId = string.Empty;

                    foreach (var song in album.Songs)
                    {
                        var musicFile = MainForm.MusicFileRepo.LoadFromEntityId(song.EntityId).Result;
                        string databaseExistenceText = "Not found or not mapped";

                        if (musicFile != null && musicFile.Id != 0 && musicFile.Id != -1)
                        {
                            databaseExistenceText = $"Exists ({musicFile.Id})";
                            releaseMbId = string.IsNullOrEmpty(musicFile.ReleaseMbId) ? releaseMbId : musicFile.ReleaseMbId;
                        }

                        songNodes.Add(new TreeNode
                        {
                            Name = song.EntityId,
                            Text = song.Title,
                            Tag = Tag = new MusicManageTreeNodeModel
                            {
                                NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Song,
                                ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                                AlbumTitle = album.Title,
                                SongTitle = song.Title,
                                Duration = song.Duration,
                                CovertArtUrl = song.CoverArtUrl,
                                DatabaseExistence = databaseExistenceText,
                                MbId = musicFile == null || string.IsNullOrEmpty(musicFile.MbId) ? "-" : musicFile.MbId,
                                EntityOrBrowseId = song.EntityId,
                                Uploaded = musicFile == null ? "-" : musicFile.LastUpload.ToString("dd/MM/yyyy HH:mm")
                            }
                        });
                    }

                    var albumNode = new TreeNode
                    {
                        Name = Guid.NewGuid().ToString(),
                        Text = album.Title,
                        Tag = Tag = new MusicManageTreeNodeModel
                        {
                            NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Album,
                            ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                            AlbumTitle = album.Title,
                            CovertArtUrl = album.CoverArtUrl,
                            DatabaseExistence = string.IsNullOrEmpty(releaseMbId) ? "Not found or not mapped" : "Tracks exists for this album",
                            MbId = string.IsNullOrEmpty(releaseMbId) ? "-" : releaseMbId,
                            EntityOrBrowseId = album.EntityId,
                            Uploaded = "-"
                        }
                    };

                    albumNode.Nodes.AddRange(songNodes.ToArray());
                    albumNode.Text = albumNode.Text + " (" + songNodes.Count + ")";
                    albumNodes.Add(albumNode);
                };

                AddChildNodes(artistNode, albumNodes);

                int albumCount = albumSongCollection.Albums.Count;
                int songCount = albumSongCollection.Songs.Count;

                string albumText = "albums";
                string songText = "tracks";

                if (albumCount == 1)
                    albumText = "album";

                if (songCount == 1)
                    songText = "track";

                if (showFetchedMessage)
                    AppendUpdatesText($"Fetched {albumCount} {albumText}, {songCount} {songText}.",
                                      ColourHelper.HexStringToColor("#0d5601"));

                artistNode.Text = artistNode.Text + " (" + artistNode.Nodes.Count + ")";
                if (expand)
                    artistNode.Expand();

                if (artistNode.Checked)
                    CheckAllChildNodes(artistNode, true);
            }

            if (!isDeleting)
            {
                ShowPreloader(false);
                SetTreeViewEnabled(true);
                DisableAllActionButtons(false);
            }
        }

        [Obsolete(message: "Faster without it")]
        private void BindAlbumNodesFromArtistBind(
            TreeNode artistNode,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true)
        {
            var albumNodes = new List<TreeNode>();
            foreach (var album in albumSongCollection.Albums)
            {
                var songNodes = new List<TreeNode>();
                string releaseMbId = string.Empty;

                album.Songs.AsParallel().ForAllInApproximateOrder(song =>
                {
                    var musicFile = MainForm.MusicFileRepo.LoadFromEntityId(song.EntityId).Result;
                    string databaseExistenceText = "Not found or not mapped";

                    if (musicFile != null && musicFile.Id != 0 && musicFile.Id != -1)
                    {
                        databaseExistenceText = $"Exists ({musicFile.Id})";
                        releaseMbId = string.IsNullOrEmpty(musicFile.ReleaseMbId) ? releaseMbId : musicFile.ReleaseMbId;
                    }

                    songNodes.Add(new TreeNode
                    {
                        Name = song.EntityId,
                        Text = song.Title,
                        Tag = Tag = new MusicManageTreeNodeModel
                        {
                            NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Song,
                            ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                            AlbumTitle = album.Title,
                            SongTitle = song.Title,
                            Duration = song.Duration,
                            CovertArtUrl = song.CoverArtUrl,
                            DatabaseExistence = databaseExistenceText,
                            MbId = musicFile == null || string.IsNullOrEmpty(musicFile.MbId) ? "-" : musicFile.MbId,
                            EntityOrBrowseId = song.EntityId,
                            Uploaded = musicFile == null ? "-" : musicFile.LastUpload.ToString("dd/MM/yyyy HH:mm")
                        }
                    });
                });

                var albumNode = new TreeNode
                {
                    Name = Guid.NewGuid().ToString(),
                    Text = album.Title,
                    Tag = Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Album,
                        ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                        AlbumTitle = album.Title,
                        CovertArtUrl = album.CoverArtUrl,
                        DatabaseExistence = string.IsNullOrEmpty(releaseMbId) ? "Not found or not mapped" : "Tracks exists for this album",
                        MbId = string.IsNullOrEmpty(releaseMbId) ? "-" : releaseMbId,
                        EntityOrBrowseId = album.EntityId,
                        Uploaded = "-"
                    }
                };

                albumNode.Nodes.AddRange(songNodes.ToArray());
                albumNode.Text = albumNode.Text + " (" + songNodes.Count + ")";
                albumNodes.Add(albumNode);
            }

            AddChildNodesFromArtistBind(artistNode, albumNodes);

            int albumCount = albumSongCollection.Albums.Count;
            int songCount = albumSongCollection.Songs.Count;

            string albumText = "albums";
            string songText = "tracks";

            if (albumCount == 1)
                albumText = "album";

            if (songCount == 1)
                songText = "track";

            if (showFetchedMessage)
                AppendUpdatesText($"Fetched {albumCount} {albumText}, {songCount} {songText}.",
                                  ColourHelper.HexStringToColor("#0d5601"));

            artistNode.Text = artistNode.Text + " (" + artistNode.Nodes.Count + ")";
            if (expand)
                artistNode.Expand();

            if (artistNode.Checked)
                CheckAllChildNodes(artistNode, true);
        }

        private void CheckAllChildNodes(TreeNode parentNode, bool checking)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                node.Checked = checking;
                if (node.Nodes != null && node.Nodes.Count > 0)
                    CheckAllChildNodes(node, checking);
            }
        }

        private void UncheckParentNodes(TreeNode node)
        {
            var parentNode = node.Parent;
            if (parentNode != null)
            {
                parentNode.Checked = false;
                UncheckParentNodes(parentNode);
            }
        }

        private void CheckParentNodes(TreeNode node)
        {
            var parentNode = node.Parent;
            if (parentNode != null)
            {
                bool allSelected = true;
                foreach (TreeNode childNode in parentNode.Nodes)
                {
                    if (!childNode.Checked)
                        allSelected = false;
                }

                if (allSelected)
                    parentNode.Checked = true;

                CheckParentNodes(parentNode);
            }
        }

        private int CountChecked(ref bool nonFetchedArtistAlbumsSelected, TreeNode node = null)
        {
            int count = 0;
            if (node == null)
            {
                node = tvUploads.Nodes[0];
                nonFetchedArtistAlbumsSelected = false;
            }

            foreach (TreeNode childNode in node.Nodes)
            {
                if (((MusicManageTreeNodeModel)childNode.Tag).NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Song &&
                    childNode.Checked)
                {
                    count++;
                }
                else
                {
                    if (((MusicManageTreeNodeModel)childNode.Tag).NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Artist &&
                        childNode.Checked &&
                        (childNode.Nodes == null ||
                        childNode.Nodes.Count == 0))
                    {
                        nonFetchedArtistAlbumsSelected = true;
                    }
                }

                if (childNode.Nodes != null && childNode.Nodes.Count > 0)
                    count += CountChecked(ref nonFetchedArtistAlbumsSelected, childNode);
            }

            return count;
        }

        private void ResetMusicFileEntryStates()
        {
            new Thread((ThreadStart)delegate
            {
                DisableAllActionButtons(true);
                MainForm.MusicFileRepo.ResetAllMusicFileUploadedStates().Wait();
                AppendUpdatesText("Music file entry state reset complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));
                DisableAllActionButtons(false);
                ShowPreloader(false);
            }).Start();
        }

        private void ResetUserDatabase()
        {
            new Thread((ThreadStart)delegate
            {
                DisableAllActionButtons(true);
                DataAccess.ResetDatabase();
                AppendUpdatesText("Database reset complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));
                DisableAllActionButtons(false);
                ShowPreloader(false);
            }).Start();
        }

        private void DeleteTracksFromYouTubeMusic()
        {
            new Thread((ThreadStart)delegate
            {
                DisableAllActionButtons(true);
                SetTreeViewEnabled(false);

                foreach (TreeNode artistNode in tvUploads.Nodes[0].Nodes)
                {
                    // Retrieve album and tracks if not already received
                    if (artistNode.Checked && (artistNode.Nodes == null || artistNode.Nodes.Count == 0))
                        if (artistNode != null)
                            if (((MusicManageTreeNodeModel)artistNode.Tag).NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Artist)
                                if (artistNode.Nodes == null || artistNode.Nodes.Count == 0)
                                    GetAlbums(artistNode, ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle, true);

                    DisableAllActionButtons(true);
                    SetTreeViewEnabled(false);

                    var albumNodes = new List<TreeNode>();
                    foreach (TreeNode albumNode in artistNode.Nodes)
                        albumNodes.Add(albumNode);

                    albumNodes.AsParallel().ForAllInApproximateOrder(albumNode =>
                    {
                        List<TreeNode> tracksToDelete = new List<TreeNode>();
                        foreach (TreeNode trackNode in albumNode.Nodes)
                            if (trackNode.Checked)
                                tracksToDelete.Add(trackNode);

                        var musicManageAbumTreeNodeModel = (MusicManageTreeNodeModel)albumNode.Tag;
                        string albumEntityId = musicManageAbumTreeNodeModel.EntityOrBrowseId;

                        if (albumNode.Checked && albumEntityId != "[Single]")
                        {
                            if (Requests.DeleteAlbumOrTrackFromYTMusic(MainForm.Settings.AuthenticationCookie, albumEntityId, out string errorMessage))
                            {
                                foreach (TreeNode songNode in tracksToDelete)
                                {
                                    string songEntityId = ((MusicManageTreeNodeModel)songNode.Tag).EntityOrBrowseId;
                                    MainForm.MusicFileRepo.DeleteByEntityId(songEntityId).Wait();
                                    albumNode.Nodes.Remove(songNode);
                                }

                                Requests.ArtistCache.Artists.RemoveAlbum(
                                                                   ((MusicManageTreeNodeModel)artistNode.Tag).EntityOrBrowseId,
                                                                   musicManageAbumTreeNodeModel.EntityOrBrowseId);

                                AppendUpdatesText($"Deleted Album: {musicManageAbumTreeNodeModel.ArtistTitle} - " +
                                                  $"{musicManageAbumTreeNodeModel.AlbumTitle}",
                                                  ColourHelper.HexStringToColor("#0d5601"));

                            }
                            else
                            {
                                AppendUpdatesText($"Error Deleting Album: {musicManageAbumTreeNodeModel.ArtistTitle} - " +
                                                  $"{musicManageAbumTreeNodeModel.AlbumTitle}:: " +
                                                  $"{errorMessage}",
                                                  ColourHelper.HexStringToColor("#0d5601"));
                            }
                        }
                        else
                        {
                            tracksToDelete.AsParallel().ForAllInApproximateOrder(nodeToDelete =>
                            {
                                var musicManageTrackTreeNodeModel = (MusicManageTreeNodeModel)nodeToDelete.Tag;
                                string trackEntityId = musicManageTrackTreeNodeModel.EntityOrBrowseId;
                                if (Requests.DeleteAlbumOrTrackFromYTMusic(MainForm.Settings.AuthenticationCookie, trackEntityId, out string errorMessage))
                                {
                                    MainForm.MusicFileRepo.DeleteByEntityId(trackEntityId).Wait();
                                    AppendUpdatesText($"Deleted Track: {musicManageTrackTreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManageTrackTreeNodeModel.AlbumTitle} - " +
                                                      $"{musicManageTrackTreeNodeModel.SongTitle}",
                                                      ColourHelper.HexStringToColor("#0d5601"));

                                    albumNode.Nodes.Remove(nodeToDelete);
                                    Requests.ArtistCache.Artists.RemoveSong(
                                                                    ((MusicManageTreeNodeModel)artistNode.Tag).EntityOrBrowseId,
                                                                    musicManageTrackTreeNodeModel.EntityOrBrowseId);
                                }
                                else
                                {
                                    AppendUpdatesText($"Error Deleting Track: {musicManageTrackTreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManageTrackTreeNodeModel.AlbumTitle} - " +
                                                      $"{musicManageTrackTreeNodeModel.SongTitle}:: " +
                                                      $"{errorMessage}",
                                                      ColourHelper.HexStringToColor("#e20000"));
                                }
                            });
                        }

                        ChangeChildCount(albumNode);
                    });

                    /// Remove album node if no track nodes left
                    for (int i = artistNode.Nodes.Count - 1; i >= 0; i--)
                    {
                        if (artistNode.Nodes[i].Nodes == null || artistNode.Nodes[i].Nodes.Count == 0)
                        {
                            artistNode.Nodes.RemoveAt(i);
                            ChangeChildCount(artistNode);
                        }
                    }
                }

                // Need to set this to null so a) TreeView doesn't scroll and b) A GET request isn't triggered
                tvUploads.SelectedNode = null;

                // Remove artist node if no alumb nodes left
                for (int i = tvUploads.Nodes[0].Nodes.Count - 1; i >= 0; i--)
                {
                    if (tvUploads.Nodes[0].Nodes[i].Nodes == null || tvUploads.Nodes[0].Nodes[i].Nodes.Count == 0)
                    {
                        if (tvUploads.Nodes[0].Nodes[i].Checked)
                            tvUploads.Nodes[0].Nodes.RemoveAt(i);
                    }
                }

                ChangeChildCount(tvUploads.Nodes[0]);
                SetCheckedLabel("0 tracks checked");

                AppendUpdatesText("Uploaded songs deletion complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));

                DisableAllActionButtons(false);
                DisableDeleteFromYTMusicButton();
                SetTreeViewEnabled(true);
                ShowPreloader(false);

            }).Start();
        }

        public void ChangeCount(TreeNode node)
        {
            int end = node.Text.LastIndexOf("(");
            node.Text = node.Text.Substring(0, end).Trim() + " (" + node.Nodes.Count + ")";
        }

        private void ManageYTMusic_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangesMade)
                DialogResult = DialogResult.Yes;
            else
                DialogResult = DialogResult.Cancel;
        }       
    }
}
