using JBToolkit;
using JBToolkit.WinForms;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Dialogues
{
    // TODO: Check why 'database existence' isn't working


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
            SetTreeViewEnabled(false);
            ShowPreloader(true);
            AppendUpdatesText("Fetching artists...", ColourHelper.HexStringToColor("#0f0466"));
            Requests.ArtistCache = Requests.GetArtists(MainForm.Settings.AuthenticationCookie);
            BindArtists(true);
        }

        private void GetAlbums(string artistNodeName, string artist)
        {
            SetTreeViewEnabled(false);
            ShowPreloader(true);
            AppendUpdatesText($"Fetching songs for arists: {artist}...", ColourHelper.HexStringToColor("#0f0466"));
            var albumSongCollection = Requests.GetArtistSongs(MainForm.Settings.AuthenticationCookie, artistNodeName);
            Requests.ArtistCache.Artists.Where(a => a.BrowseId == artistNodeName).FirstOrDefault().AlbumSongCollection = albumSongCollection;

            BindAlbumNodes(artistNodeName, albumSongCollection);
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
            new Thread((ThreadStart)delegate {
                MainForm.MusicFileRepo.ResetAllMusicFileUploadedStates().Wait();
                AppendUpdatesText("Music file entry state reset complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));

            }).Start();          
        }

        private void ResetUserDatabase()
        {
            new Thread((ThreadStart)delegate {
                DataAccess.ResetDatabase();
                AppendUpdatesText("Database reset complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));

            }).Start();
        }

        private void DeleteTracksFromYouTubeMusic()
        {
            new Thread((ThreadStart)delegate {

                // TODO: Delete YouTube Music uploaded tracks implementation

                AppendUpdatesText("Uploaded track deletion complete.",
                                   ColourHelper.HexStringToColor("#0d5601"));

            }).Start();
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
