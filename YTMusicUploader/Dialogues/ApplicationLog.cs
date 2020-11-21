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

        private Dictionary<string, string> _logTypeDic = new Dictionary<string, string>
        {
            {"0", "All" },
            {"1", "Info" },
            {"2", "Error" },
            {"3", "Warning" },
            {"4", "Critical" },
            {"2,3,4", "Errors and Warnings" },
        };

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
            ddLogType.SelectedIndexChanged -= DdLogType_SelectedIndexChanged;
            OnLoad(e);            
            ResumeDrawing(this);
            BindLogTypes();
            new Thread((ThreadStart)delegate { Populate(); }).Start();
        }

        private void BindLogTypes()
        {
            ddLogType.DataSource = new BindingSource(_logTypeDic, null);
            ddLogType.DisplayMember = "Value";
            ddLogType.ValueMember = "Key";
            ddLogType.SelectedValue = "2,3,4";

            ddLogType.SelectedIndexChanged += DdLogType_SelectedIndexChanged;
        }

        private void DdLogType_SelectedIndexChanged(object sender, EventArgs  e)
        {
            Populate();
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
                try
                {
                    if (((string)ddLogType.SelectedValue) == "0")
                        dgvLog.DataSource = LogsRepo.LoadAll().Result;
                    else
                        dgvLog.DataSource = LogsRepo.LoadSpecific(((string)ddLogType.SelectedValue)).Result;

                    dgvLog.Columns["Machine"].Visible = false;
                    dgvLog.Columns["LogTypeId"].Width = 75;
                    dgvLog.Columns["Id"].Width = 55;
                }
                catch (Exception e)
                {
                    Logger.Log(e, "Unable to fetch logs from the database and bind gridview", Log.LogTypeEnum.Critcal);
                }

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
            if (((string)ddLogType.SelectedValue) == "0")
                dgvLog.DataSource = LogsRepo.LoadAll().Result;
            else
                dgvLog.DataSource = LogsRepo.LoadSpecific(((string)ddLogType.SelectedValue)).Result;
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
                (List<Log>)dgvLog.DataSource, dgvLog.Columns[e.ColumnIndex].Name, _sortDirection);

            _previousIndex = e.ColumnIndex;
        }

        public List<Log> SortData(List<Log> list, string column, bool ascending)
        {
            return ascending ?
                list.OrderBy(_ => _.GetType().GetProperty(column).GetValue(_)).ToList() :
                list.OrderByDescending(_ => _.GetType().GetProperty(column).GetValue(_)).ToList();
        }
    }
}
