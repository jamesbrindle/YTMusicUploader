using JBToolkit.WinForms;
using System;
using System.Threading;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Form to display upload successes. Data fetched from the local database.
    /// </summary>
    public partial class UploadLog : OptimisedMetroForm
    {
        private MainForm MainForm { get; set; }

        /// <summary>
        /// Form to display upload successes. Data fetched from the local database.
        /// </summary>
        public UploadLog(MainForm mainForm) : base(formResizable: true)
        {
            MainForm = mainForm;
            InitializeComponent();
            SuspendDrawing(this);
        }

        private void IssueLog_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);

            new Thread((ThreadStart)delegate { Populate(); }).Start();
        }

        delegate void PopulateDelegate();

        /// <summary>
        /// Populates the DataGridView control with data.
        /// </summary>
        public void Populate()
        {
            if (dgvUploads.InvokeRequired)
            {
                PopulateDelegate d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                dgvUploads.DataSource = MainForm.MusicFileRepo.LoadUploaded().Result;
                dgvUploads.Columns["Removed"].Visible = false;
                dgvUploads.Columns["Error"].Visible = false;
                dgvUploads.Columns["ErrorReason"].Visible = false;
                dgvUploads.Columns["Id"].Width = 55;
                dgvUploads.Columns["Path"].FillWeight = 300;
                dgvUploads.Columns["LastUpload"].Width = 100;

                SetTitle("Upload Log");
            }
        }

        delegate void SetTitleDelegate(string title);
        private void SetTitle(string text)
        {
            if (lblTitle.InvokeRequired)
            {
                SetTitleDelegate d = new SetTitleDelegate(SetTitle);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblTitle.Text = text;
            }
        }

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            dgvUploads.DataSource = MainForm.MusicFileRepo.LoadUploaded().Result;
        }

        private void PbRefresh_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_down;
        }

        private void PbRefresh_MouseEnter(object sender, EventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_hover;
        }

        private void PbRefresh_MouseLeave(object sender, EventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh;
        }
    }
}
