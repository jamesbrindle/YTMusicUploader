using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Dialogue
    /// </summary>
    public partial class IssueLog : OptimisedMetroForm
    {
        private int _previousIndex;
        private bool _sortDirection;

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
                dgvIssues.Columns["MbId"].Visible = false;
                dgvIssues.Columns["ReleaseMbId"].Visible = false;
                dgvIssues.Columns["EntityId"].Visible = false;
                dgvIssues.Columns["BrowseId"].Visible = false;
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

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            dgvIssues.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
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

        private void PbRefresh_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_hover;
        }

        private void DgvUploads_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            dgvIssues.DataSource = SortData(
                (List<MusicFile>)dgvIssues.DataSource, dgvIssues.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        public List<MusicFile> SortData(List<MusicFile> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }
    }
}
