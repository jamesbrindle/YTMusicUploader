namespace YTMusicUploader.Dialogues
{
    partial class IssueLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueLog));
            this.pnlIssueLog = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbIssueLogIcon = new System.Windows.Forms.PictureBox();
            this.dgvIssues = new JBToolkit.WinForms.FastScrollDataGridView();
            this.pnlIssueLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssues)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlIssueLog
            // 
            this.pnlIssueLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlIssueLog.Controls.Add(this.dgvIssues);
            this.pnlIssueLog.Location = new System.Drawing.Point(26, 76);
            this.pnlIssueLog.Name = "pnlIssueLog";
            this.pnlIssueLog.Size = new System.Drawing.Size(988, 701);
            this.pnlIssueLog.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(73, 34);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(106, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Issue Log. Loading...";
            // 
            // pbIssueLogIcon
            // 
            this.pbIssueLogIcon.Image = global::YTMusicUploader.Properties.Resources.unhappy;
            this.pbIssueLogIcon.Location = new System.Drawing.Point(23, 20);
            this.pbIssueLogIcon.Name = "pbIssueLogIcon";
            this.pbIssueLogIcon.Size = new System.Drawing.Size(40, 40);
            this.pbIssueLogIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIssueLogIcon.TabIndex = 2;
            this.pbIssueLogIcon.TabStop = false;
            // 
            // dgvIssues
            // 
            this.dgvIssues.AllowUserToAddRows = false;
            this.dgvIssues.AllowUserToDeleteRows = false;
            this.dgvIssues.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvIssues.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIssues.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvIssues.BackgroundColor = System.Drawing.Color.White;
            this.dgvIssues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIssues.DisableScroll = false;
            this.dgvIssues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIssues.Location = new System.Drawing.Point(0, 0);
            this.dgvIssues.Name = "dgvIssues";
            this.dgvIssues.ReadOnly = true;
            this.dgvIssues.RowHeadersVisible = false;
            this.dgvIssues.Size = new System.Drawing.Size(988, 701);
            this.dgvIssues.TabIndex = 0;
            // 
            // IssueLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 800);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIssueLogIcon);
            this.Controls.Add(this.pnlIssueLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(920, 800);
            this.Name = "IssueLog";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Load += new System.EventHandler(this.IssueLog_Load);
            this.pnlIssueLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIssues)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlIssueLog;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbIssueLogIcon;
        private JBToolkit.WinForms.FastScrollDataGridView dgvIssues;
    }
}