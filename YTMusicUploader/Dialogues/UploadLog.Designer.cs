namespace YTMusicUploader.Dialogues
{
    partial class UploadLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadLog));
            this.pnlUploads = new System.Windows.Forms.Panel();
            this.dgvUploads = new JBToolkit.WinForms.FastScrollDataGridView();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbIssueLogIcon = new System.Windows.Forms.PictureBox();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.pnlUploads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUploads
            // 
            this.pnlUploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUploads.Controls.Add(this.dgvUploads);
            this.pnlUploads.Location = new System.Drawing.Point(26, 76);
            this.pnlUploads.MinimumSize = new System.Drawing.Size(600, 400);
            this.pnlUploads.Name = "pnlUploads";
            this.pnlUploads.Size = new System.Drawing.Size(988, 633);
            this.pnlUploads.TabIndex = 1;
            // 
            // dgvUploads
            // 
            this.dgvUploads.AllowUserToAddRows = false;
            this.dgvUploads.AllowUserToDeleteRows = false;
            this.dgvUploads.AllowUserToOrderColumns = true;
            this.dgvUploads.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvUploads.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUploads.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUploads.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUploads.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUploads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUploads.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvUploads.DisableScroll = false;
            this.dgvUploads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUploads.Location = new System.Drawing.Point(0, 0);
            this.dgvUploads.MinimumSize = new System.Drawing.Size(600, 400);
            this.dgvUploads.Name = "dgvUploads";
            this.dgvUploads.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUploads.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvUploads.RowHeadersVisible = false;
            this.dgvUploads.Size = new System.Drawing.Size(988, 633);
            this.dgvUploads.TabIndex = 0;
            this.dgvUploads.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvUploads_CellContentClick);
            this.dgvUploads.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvUploads_CellMouseEnter);
            this.dgvUploads.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvUploads_ColumnHeaderMouseClick);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(73, 34);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(124, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Upload Log. Loading...";
            // 
            // pbIssueLogIcon
            // 
            this.pbIssueLogIcon.Image = global::YTMusicUploader.Properties.Resources.happy;
            this.pbIssueLogIcon.Location = new System.Drawing.Point(23, 20);
            this.pbIssueLogIcon.Name = "pbIssueLogIcon";
            this.pbIssueLogIcon.Size = new System.Drawing.Size(40, 40);
            this.pbIssueLogIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIssueLogIcon.TabIndex = 2;
            this.pbIssueLogIcon.TabStop = false;
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
            this.pbRefresh.TabIndex = 5;
            this.pbRefresh.TabStop = false;
            this.pbRefresh.Click += new System.EventHandler(this.PbRefresh_Click);
            this.pbRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseDown);
            this.pbRefresh.MouseEnter += new System.EventHandler(this.PbRefresh_MouseEnter);
            this.pbRefresh.MouseLeave += new System.EventHandler(this.PbRefresh_MouseLeave);
            this.pbRefresh.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseUp);
            // 
            // UploadLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 732);
            this.Controls.Add(this.pbRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIssueLogIcon);
            this.Controls.Add(this.pnlUploads);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "UploadLog";
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Load += new System.EventHandler(this.IssueLog_Load);
            this.pnlUploads.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlUploads;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbIssueLogIcon;
        private JBToolkit.WinForms.FastScrollDataGridView dgvUploads;
        private System.Windows.Forms.PictureBox pbRefresh;
    }
}