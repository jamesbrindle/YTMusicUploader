using JBToolkit;
using JBToolkit.WinForms;
using System;
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
        private ArtistCache ArtistCache { get; set; }

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

            new Thread((ThreadStart)delegate { GetArtists(); }).Start();
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
            ShowPreloader(true);
            AppendUpdatesText("Fetching artists...", ColourHelper.HexStringToColor("#0f0466"));
            ArtistCache = Requests.GetArtists(MainForm.Settings.AuthenticationCookie);
            BindArtists();
        }

        private void GetAlbums(string artistNodeName, string artist)
        {
            ShowPreloader(true);
            AppendUpdatesText($"Fetching songs for arists: {artist}...", ColourHelper.HexStringToColor("#0f0466"));
            var albumSongCollection = Requests.GetArtistSongs(MainForm.Settings.AuthenticationCookie, artistNodeName);
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
    }
}
