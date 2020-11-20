using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Application Log (info and error) Dialogue
    /// </summary>
    public partial class ApplicationLog : OptimisedMetroForm
    {
        private int _previousIndex;
        private bool _sortDirection;

        private LogsRepo LogsRepo { get; set; }

        /// <summary>
        /// Form to display upload issues. Data fetched from the local database.
        /// </summary>
        public ApplicationLog() : base(formResizable: true)
        {
            LogsRepo = new LogsRepo();
            InitializeComponent();
            SuspendDrawing(this);
        }

        private void ApplicationLoad_Load(object sender, EventArgs e)
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
            if (dgvLog.InvokeRequired)
            {
                PopulateDelegate d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                dgvLog.DataSource = LogsRepo.LoadAll().Result;
                dgvLog.Columns["Machine"].Visible = false;
                dgvLog.Columns["LogTypeId"].Width = 75;
                dgvLog.Columns["Id"].Width = 55;

                SetTitle("Application Log");
                SetLogsClearedMessage($"Logs are cleared after {Global.ClearLogsAfterDays} days.");
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

        delegate void SetLogsClearedMessageDelegate(string title);
        private void SetLogsClearedMessage(string text)
        {
            if (lblLogClearMessage.InvokeRequired)
            {
                SetLogsClearedMessageDelegate d = new SetLogsClearedMessageDelegate(SetLogsClearedMessage);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblLogClearMessage.Text = text;
            }
        }

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            dgvLog.DataSource = LogsRepo.LoadAll().Result;
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

        private void DgvLogs_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            dgvLog.DataSource = SortData(
                (List<MusicFile>)dgvLog.DataSource, dgvLog.Columns[e.ColumnIndex].Name, _sortDirection);

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
