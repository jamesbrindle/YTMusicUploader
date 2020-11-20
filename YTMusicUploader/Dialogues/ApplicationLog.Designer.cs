namespace YTMusicUploader.Dialogues
{
    partial class ApplicationLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ApplicationLog));
            this.pnlIssueLog = new System.Windows.Forms.Panel();
            this.dgvLog = new JBToolkit.WinForms.FastScrollDataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.pbIssueLogIcon = new System.Windows.Forms.PictureBox();
            this.lblLogClearMessage = new System.Windows.Forms.Label();
            this.pnlIssueLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlIssueLog
            // 
            this.pnlIssueLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIssueLog.Controls.Add(this.dgvLog);
            this.pnlIssueLog.Location = new System.Drawing.Point(26, 76);
            this.pnlIssueLog.MinimumSize = new System.Drawing.Size(600, 400);
            this.pnlIssueLog.Name = "pnlIssueLog";
            this.pnlIssueLog.Size = new System.Drawing.Size(988, 633);
            this.pnlIssueLog.TabIndex = 1;
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToOrderColumns = true;
            this.dgvLog.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLog.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLog.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLog.DisableScroll = false;
            this.dgvLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLog.Location = new System.Drawing.Point(0, 0);
            this.dgvLog.MinimumSize = new System.Drawing.Size(600, 400);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.Size = new System.Drawing.Size(988, 633);
            this.dgvLog.TabIndex = 0;
            this.dgvLog.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvLogs_ColumnHeaderMouseClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(76, 34);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(145, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Application Log. Loading...";
            // 
            // pbRefresh
            // 
            this.pbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbRefresh.Image = global::YTMusicUploader.Properties.Resources.refresh;
            this.pbRefresh.Location = new System.Drawing.Point(989, 38);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Size = new System.Drawing.Size(25, 25);
            this.pbRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRefresh.TabIndex = 4;
            this.pbRefresh.TabStop = false;
            this.pbRefresh.Click += new System.EventHandler(this.PbRefresh_Click);
            this.pbRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseDown);
            this.pbRefresh.MouseEnter += new System.EventHandler(this.PbRefresh_MouseEnter);
            this.pbRefresh.MouseLeave += new System.EventHandler(this.PbRefresh_MouseLeave);
            this.pbRefresh.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseUp);
            // 
            // pbIssueLogIcon
            // 
            this.pbIssueLogIcon.Image = global::YTMusicUploader.Properties.Resources.log_icon;
            this.pbIssueLogIcon.Location = new System.Drawing.Point(26, 20);
            this.pbIssueLogIcon.Name = "pbIssueLogIcon";
            this.pbIssueLogIcon.Size = new System.Drawing.Size(37, 39);
            this.pbIssueLogIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIssueLogIcon.TabIndex = 2;
            this.pbIssueLogIcon.TabStop = false;
            // 
            // lblLogClearMessage
            // 
            this.lblLogClearMessage.AutoSize = true;
            this.lblLogClearMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogClearMessage.Location = new System.Drawing.Point(729, 34);
            this.lblLogClearMessage.Name = "lblLogClearMessage";
            this.lblLogClearMessage.Size = new System.Drawing.Size(161, 13);
            this.lblLogClearMessage.TabIndex = 5;
            this.lblLogClearMessage.Text = "Logs are cleared after 30 days.";
            // 
            // ApplicationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 732);
            this.Controls.Add(this.lblLogClearMessage);
            this.Controls.Add(this.pbRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIssueLogIcon);
            this.Controls.Add(this.pnlIssueLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ApplicationLog";
            this.Load += new System.EventHandler(this.ApplicationLoad_Load);
            this.pnlIssueLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlIssueLog;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbIssueLogIcon;
        private JBToolkit.WinForms.FastScrollDataGridView dgvLog;
        private System.Windows.Forms.PictureBox pbRefresh;
        private System.Windows.Forms.Label lblLogClearMessage;
    }
}