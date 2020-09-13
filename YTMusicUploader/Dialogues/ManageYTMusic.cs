using JBToolkit;
using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Dialogue
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
            SuspendDrawing(this);
        }

        private void ManageYTMusic_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ClearFields();
            ResumeDrawing(this);

            if (Requests.ArtistCache == null ||
                Requests.ArtistCache.Artists == null ||
                Requests.ArtistCache.Artists.Count == 0)
            {
                new Thread((ThreadStart)delegate { GetArtists(); }).Start();
            }
            else
                BindArtists(false);
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
        }

        private void GetArtists()
        {
            DisableAllActionButtons(true);
            SetTreeViewEnabled(false);
            ShowPreloader(true);
            AppendUpdatesText("Fetching artists...", ColourHelper.HexStringToColor("#0f0466"));
            Requests.ArtistCache = Requests.GetArtists(MainForm.Settings.AuthenticationCookie);
            BindArtists(true);
        }

        private void GetAlbums(string artistNodeName, string artist, bool isDeleting = false)
        {
            DisableAllActionButtons(true);
            SetTreeViewEnabled(false);
            ShowPreloader(true);

            if (!isDeleting)
                AppendUpdatesText($"Fetching songs for arists: {artist}...", ColourHelper.HexStringToColor("#0f0466"));

            var albumSongCollection = Requests.GetArtistSongs(MainForm.Settings.AuthenticationCookie, artistNodeName);
            Requests.ArtistCache.Artists.Where(a => a.BrowseId == artistNodeName).FirstOrDefault().AlbumSongCollection = albumSongCollection;

            BindAlbumNodes(artistNodeName, albumSongCollection, !isDeleting, !isDeleting, isDeleting);
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
                foreach (TreeNode artistNode in tvUploads.Nodes[0].Nodes)
                {
                    // Retrieve album and tracks if not already received
                    if (artistNode.Checked && (artistNode.Nodes == null || artistNode.Nodes.Count == 0))
                        if (artistNode != null)
                            if (((MusicManageTreeNodeModel)artistNode.Tag).NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Artist)
                                if (artistNode.Nodes == null || artistNode.Nodes.Count == 0)
                                    GetAlbums(artistNode.Name, ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle, true);

                    foreach (TreeNode albumNode in artistNode.Nodes)
                    {
                        List<TreeNode> tracksToDelete = new List<TreeNode>();
                        foreach (TreeNode trackNode in albumNode.Nodes)
                            if (trackNode.Checked)
                                tracksToDelete.Add(trackNode);

                        if (albumNode.Checked)
                        {
                            var musicManagetreeNodeModel = (MusicManageTreeNodeModel)albumNode.Tag;
                            string entityId = musicManagetreeNodeModel.EntityOrBrowseId;
                            if (Requests.DeleteAlbumOrTrackFromYTMusic(MainForm.Settings.AuthenticationCookie, entityId, out string errorMessage))
                            {
                                MainForm.MusicFileRepo.DeleteByBrowseId(entityId).Wait();
                                AppendUpdatesText($"Deleted Album: {musicManagetreeNodeModel.ArtistTitle} - " +
                                                  $"{musicManagetreeNodeModel.ArtistTitle}",
                                                  ColourHelper.HexStringToColor("#0d5601"));

                                foreach (TreeNode trackNode in tracksToDelete)
                                    albumNode.Nodes.Remove(trackNode);
                            }
                            else
                            {
                                AppendUpdatesText($"Error Deleting Album: {musicManagetreeNodeModel.ArtistTitle} - " +
                                                  $"{musicManagetreeNodeModel.ArtistTitle}:: " +
                                                  $"{errorMessage}",
                                                  ColourHelper.HexStringToColor("#0d5601"));
                            }
                        }

                        else
                        {
                            tracksToDelete.AsParallel().ForAllInApproximateOrder(nodeToDelete =>
                            {
                                var musicManagetreeNodeModel = (MusicManageTreeNodeModel)nodeToDelete.Tag;
                                string entityid = musicManagetreeNodeModel.EntityOrBrowseId;
                                if (Requests.DeleteAlbumOrTrackFromYTMusic(MainForm.Settings.AuthenticationCookie, entityid, out string errorMessage))
                                {
                                    MainForm.MusicFileRepo.DeleteByEntityId(entityid).Wait();
                                    AppendUpdatesText($"Deleted Track: {musicManagetreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManagetreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManagetreeNodeModel.SongTitle}",
                                                      ColourHelper.HexStringToColor("#0d5601"));

                                    albumNode.Nodes.Remove(nodeToDelete);
                                }
                                else
                                {
                                    AppendUpdatesText($"Error Deleting Track: {musicManagetreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManagetreeNodeModel.ArtistTitle} - " +
                                                      $"{musicManagetreeNodeModel.SongTitle}:: " +
                                                      $"{errorMessage}",
                                                      ColourHelper.HexStringToColor("#e20000"));
                                }
                            });
                        }

                        ChangeChildCount(albumNode);
                    }

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

                tvUploads.SelectedNode = tvUploads.Nodes[0];

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
