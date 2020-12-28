namespace YTMusicUploader.Updater
{
    partial class UpdaterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterForm));
            this.pbPreloader = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreloader
            // 
            this.pbPreloader.Image = ((System.Drawing.Image)(resources.GetObject("pbPreloader.Image")));
            this.pbPreloader.Location = new System.Drawing.Point(-117, -62);
            this.pbPreloader.Name = "pbPreloader";
            this.pbPreloader.Size = new System.Drawing.Size(400, 300);
            this.pbPreloader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPreloader.TabIndex = 0;
            this.pbPreloader.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 7.5F);
            this.lblStatus.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblStatus.Location = new System.Drawing.Point(2, 150);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(164, 16);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Downloading New Version";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // UpdaterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(168, 168);
            this.ControlBox = false;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pbPreloader);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(170, 170);
            this.MinimumSize = new System.Drawing.Size(170, 170);
            this.Name = "UpdaterForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbPreloader;
        private System.Windows.Forms.Label lblStatus;
    }
}