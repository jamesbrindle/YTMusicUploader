using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Upload Issues Log Dialogue
    /// </summary>
    public partial class IssueLog : OptimisedMetroForm
    {
        private int _previousIndex;
        private bool _sortDirection;
        private bool _shown = false;

        private bool ChangesMade { get; set; } = false;

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
            if (dvgLog.InvokeRequired)
            {
                PopulateDelegate d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                try
                {
                    if (!_shown)
                    {

                        Logger.LogInfo("Populate - Issues", "Loading issues from the database");
                        dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;

                        dvgLog.Columns["Hash"].Visible = false;
                        dvgLog.Columns["Removed"].Visible = false;
                        dvgLog.Columns["MbId"].Visible = false;
                        dvgLog.Columns["ReleaseMbId"].Visible = false;
                        dvgLog.Columns["EntityId"].Visible = false;
                        dvgLog.Columns["LastUpload"].Visible = false;
                        dvgLog.Columns["Error"].Visible = false;
                        dvgLog.Columns["Id"].Width = 55;
                        dvgLog.Columns["Path"].FillWeight = 300;
                        dvgLog.Columns["LastUpload"].Width = 100;

                        var buttonColumn = new DataGridViewImageColumn
                        {
                            Name = "Reset",
                            Image = Properties.Resources.reset
                        };

                        dvgLog.Columns.Add(buttonColumn);
                        dvgLog.Columns["Reset"].Width = 50;

                        _shown = true;
                    }
                    else
                    {
                        dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                    }
                }
                catch (Exception e)
                {
                    Logger.Log(e, "Unable to fetch issues from the database and bind gridview", Log.LogTypeEnum.Critcal);
                }

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
            dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
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

        private void DgvIssues_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            dvgLog.DataSource = SortData(
                (List<MusicFile>)dvgLog.DataSource, dvgLog.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        public List<MusicFile> SortData(List<MusicFile> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }

        private void DvgLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 12)
            {
                int id = (int)dvgLog.Rows[e.RowIndex].Cells["Id"].Value;
                var _ = MainForm.MusicFileRepo.ResetIssueStatus(id).Result;
                dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                MainForm.SetIssuesLabel((dvgLog.Rows.Count).ToString());
                ChangesMade = true;
            }
        }

        private void IssueLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ChangesMade)
            {
                ChangesMade = false;
                DialogResult = DialogResult.Yes;
            }
            else
                DialogResult = DialogResult.Cancel;
        }
    }
}
