using JBToolkit.WinForms;
using System;
using System.Threading;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Dialogue
    /// </summary>
    public partial class IssueLog : OptimisedMetroForm
    {
        /// <summary>
        /// Form to display upload issues. Data fetched from the local database.
        /// </summary>
        private MainForm MainForm { get; set; }

        /// <summary>
        /// Form to display upload issues. Data fetched from the local database.
        /// </summary>
        public IssueLog(MainForm mainForm) : base(formResizable: true)
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
            if (dgvIssues.InvokeRequired)
            {
                PopulateDelegate d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                dgvIssues.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                dgvIssues.Columns["Hash"].Visible = false;
                dgvIssues.Columns["Removed"].Visible = false;
                dgvIssues.Columns["Id"].Width = 55;
                dgvIssues.Columns["Error"].Width = 45;
                dgvIssues.Columns["Path"].FillWeight = 300;
                dgvIssues.Columns["LastUpload"].Width = 100;

                SetTitle("Issues Log");
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
    }
}
