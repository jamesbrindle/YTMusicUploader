using JBToolkit.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader.Dialogues
{
    /// <summary>
    /// Success Upload Log Dialogue
    /// </summary>
    public partial class UploadLog : OptimisedMetroForm
    {
        private int _previousIndex;
        private bool _sortDirection;

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
            if (dvgLog.InvokeRequired)
            {
                var d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                try
                {
                    Logger.LogInfo("Populate - Uploads", "Loading issues from the database");

                    dvgLog.DataSource = MainForm.MusicFileRepo.LoadUploaded().Result;
                    dvgLog.Columns["ReleaseMbId"].Visible = false;
                    dvgLog.Columns["EntityId"].Visible = false;
                    dvgLog.Columns["Hash"].Visible = false;
                    dvgLog.Columns["Removed"].Visible = false;
                    dvgLog.Columns["Error"].Visible = false;
                    dvgLog.Columns["ErrorReason"].Visible = false;
                    dvgLog.Columns["UploadAttempts"].Visible = false;
                    dvgLog.Columns["LastUploadError"].Visible = false;
                    dvgLog.Columns["Id"].Width = 55;
                    dvgLog.Columns["Path"].FillWeight = 300;
                    dvgLog.Columns["LastUpload"].Width = 100;
                    dvgLog.Columns["MbId"].DefaultCellStyle = GetHyperLinkStyleForGridCell();

                }
                catch (Exception e)
                {
                    Logger.Log(e, "Unable to fetch uploads from the database and bind gridview", Log.LogTypeEnum.Critical);
                }

                SetTitle("Upload Log");
            }
        }

        private DataGridViewCellStyle GetHyperLinkStyleForGridCell()
        {
            // Set the Font and Uderline into the Content of the grid cell .  
            {
                var hyperlinkStyle = new DataGridViewCellStyle();
                var l_objFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
                hyperlinkStyle.Font = l_objFont;
                hyperlinkStyle.ForeColor = Color.Blue;
                return hyperlinkStyle;
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

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            dvgLog.DataSource = MainForm.MusicFileRepo.LoadUploaded().Result;
        }

        private void PbRefresh_MouseDown(object sender, MouseEventArgs e)
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

        private void PbRefresh_MouseUp(object sender, MouseEventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_hover;
        }

        private void DgvUploads_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == _previousIndex)
                _sortDirection ^= true; // toggle direction

            dvgLog.DataSource = SortData(
                (List<MusicFile>)dvgLog.DataSource, dvgLog.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        private void DgvUploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dvgLog.Columns[dvgLog.CurrentCell.ColumnIndex].HeaderText.Contains("MbId"))
            {
                if (!string.IsNullOrWhiteSpace(dvgLog.CurrentCell.EditedFormattedValue.ToString()))
                    System.Diagnostics.Process.Start("https://musicbrainz.org/recording/" + dvgLog.CurrentCell.EditedFormattedValue);
            }
        }

        private void DgvUploads_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dvgLog.Columns[e.ColumnIndex].Name != "MbId")
            {
                dvgLog.Cursor = Cursors.Default;
            }
            else
            {
                if (e.RowIndex == -1 || e.ColumnIndex == -1)
                    dvgLog.Cursor = Cursors.Default;
                else
                {
                    if (dvgLog.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ||
                        string.IsNullOrEmpty(dvgLog.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        dvgLog.Cursor = Cursors.Default;
                    else
                        dvgLog.Cursor = Cursors.Hand;
                }
            }
        }

        public List<MusicFile> SortData(List<MusicFile> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }
    }
}
