using JBToolkit.WinForms;
using MetroFramework;
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
        public ToolTip ResetTooltip { get; set; }
        public ToolTip RefreshTooltip { get; set; }

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
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();
            InitialiseTooltips();
            SuspendDrawing(this);
        }

        private void IssueLog_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);
            ThreadPool.QueueUserWorkItem(delegate
            {
                Populate();
            });
        }

        private void InitialiseTooltips()
        {
            RefreshTooltip = new ToolTip
            {
                ToolTipTitle = "Refresh Data Source",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            RefreshTooltip.SetToolTip(pbRefresh,
                "\nRefresh the list from the database");

            ResetTooltip = new ToolTip
            {
                ToolTipTitle = "Reset All Issues",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            ResetTooltip.SetToolTip(pbReset,
                "\nResets all music file issues forcing a retry");
        }

        delegate void PopulateDelegate();

        /// <summary>
        /// Populates the DataGridView control with data.
        /// </summary>
        public void Populate()
        {
            if (dvgLog.InvokeRequired)
            {
                var d = new PopulateDelegate(Populate);
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
                        dvgLog.Columns["VideoId"].Visible = false;
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

                    SetResetEnabled(((List<MusicFile>)dvgLog.DataSource).Count > 0);
                }
                catch (Exception e)
                {
                    Logger.Log(e, "Unable to fetch issues from the database and bind gridview", Log.LogTypeEnum.Critical);
                }

                SetTitle("Issues Log");
            }
        }

        delegate void SetTitleDelegate(string title);
        private void SetTitle(string text)
        {
            if (lblTitle.InvokeRequired)
            {
                var d = new SetTitleDelegate(SetTitle);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblTitle.Text = text;
            }
        }

        delegate void SetResetEnabledDelegate(bool enabled);
        private void SetResetEnabled(bool enabled)
        {
            if (pbReset.InvokeRequired)
            {
                var d = new SetResetEnabledDelegate(SetResetEnabled);
                Invoke(d, new object[] { enabled });
            }
            else
            {
                pbReset.Enabled = enabled;

                if (enabled)
                    pbReset.Image = Properties.Resources.reset_large_up;
                else
                    pbReset.Image = Properties.Resources.reset_large_disabled;
            }
        }

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
            SetResetEnabled(((List<MusicFile>)dvgLog.DataSource).Count > 0);
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

        private void PbReset_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,
                                "\r\nThis will reset all music file issues and have them retry the upload straight away." +
                                "\r\n\r\nAre you sure you want to contine?", "Confirm Action",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error,
                                150) == DialogResult.Yes)
            {
                var _ = MainForm.MusicFileRepo.ResetIssueStatusAll().Result;
                dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                MainForm.SetIssuesLabel((dvgLog.Rows.Count).ToString());
                SetResetEnabled(false);
                ChangesMade = true;
            }
        }

        private void PbReset_MouseDown(object sender, MouseEventArgs e)
        {
            if (pbReset.Enabled)
                pbReset.Image = Properties.Resources.reset_large_down;
        }

        private void PbReset_MouseEnter(object sender, EventArgs e)
        {
            if (pbReset.Enabled)
                pbReset.Image = Properties.Resources.reset_large_hover;
        }

        private void PbReset_MouseLeave(object sender, EventArgs e)
        {
            if (pbReset.Enabled)
                pbReset.Image = Properties.Resources.reset_large_up;
        }

        private void PbReset_MouseUp(object sender, MouseEventArgs e)
        {
            if (pbReset.Enabled)
                pbReset.Image = Properties.Resources.reset_large_hover;
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

        private void DvgLog_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DvgLog_CellContentClick(sender, e);
        }

        private void DvgLog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dvgLog.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Name == "Bitmap")
            {
                int id = (int)dvgLog.Rows[e.RowIndex].Cells["Id"].Value;
                var _ = MainForm.MusicFileRepo.ResetIssueStatus(id).Result;
                dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                MainForm.SetIssuesLabel((dvgLog.Rows.Count).ToString());                
                ChangesMade = true;
            }

            SetResetEnabled(((List<MusicFile>)dvgLog.DataSource).Count > 0);
        }

        private void DvgLog_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dvgLog.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.GetType().Name == "Bitmap")
            {
                int id = (int)dvgLog.Rows[e.RowIndex].Cells["Id"].Value;
                var _ = MainForm.MusicFileRepo.ResetIssueStatus(id).Result;
                dvgLog.DataSource = MainForm.MusicFileRepo.LoadIssues().Result;
                MainForm.SetIssuesLabel(dvgLog.Rows.Count.ToString());                
                ChangesMade = true;
            }

            SetResetEnabled(((List<MusicFile>)dvgLog.DataSource).Count > 0);
        }

        private void IssueLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _shown = true;
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
