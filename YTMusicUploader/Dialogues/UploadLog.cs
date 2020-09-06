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
    /// Form to display upload successes. Data fetched from the local database.
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
            if (dgvUploads.InvokeRequired)
            {
                PopulateDelegate d = new PopulateDelegate(Populate);
                Invoke(d, new object[] { });
            }
            else
            {
                dgvUploads.DataSource = MainForm.MusicFileRepo.LoadUploaded().Result;
                dgvUploads.Columns["ReleaseMbId"].Visible = false;
                dgvUploads.Columns["EntityId"].Visible = false;
                dgvUploads.Columns["Hash"].Visible = false;
                dgvUploads.Columns["Removed"].Visible = false;
                dgvUploads.Columns["Error"].Visible = false;
                dgvUploads.Columns["ErrorReason"].Visible = false;
                dgvUploads.Columns["Id"].Width = 55;
                dgvUploads.Columns["Path"].FillWeight = 300;
                dgvUploads.Columns["LastUpload"].Width = 100;
                dgvUploads.Columns["MbId"].DefaultCellStyle = GetHyperLinkStyleForGridCell();
                SetTitle("Upload Log");
            }
        }

        private DataGridViewCellStyle GetHyperLinkStyleForGridCell()
        {
            // Set the Font and Uderline into the Content of the grid cell .  
            {
                DataGridViewCellStyle hyperlinkStyle = new DataGridViewCellStyle();                
                Font l_objFont = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
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

            dgvUploads.DataSource = SortData(
                (List<MusicFile>)dgvUploads.DataSource, dgvUploads.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        private void DgvUploads_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUploads.Columns[dgvUploads.CurrentCell.ColumnIndex].HeaderText.Contains("MbId"))
            {
                if (!string.IsNullOrWhiteSpace(dgvUploads.CurrentCell.EditedFormattedValue.ToString()))
                    System.Diagnostics.Process.Start("https://musicbrainz.org/recording/" + dgvUploads.CurrentCell.EditedFormattedValue);
            }
        }

        private void DgvUploads_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUploads.Columns[e.ColumnIndex].Name != "MbId")
            {
                dgvUploads.Cursor = Cursors.Default;
            }
            else
            {
                if (e.RowIndex == -1 || e.ColumnIndex == -1)
                    dgvUploads.Cursor = Cursors.Default;
                else
                {
                    if (dgvUploads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ||
                        string.IsNullOrEmpty(dgvUploads.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                        dgvUploads.Cursor = Cursors.Default;
                    else
                        dgvUploads.Cursor = Cursors.Hand;
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
