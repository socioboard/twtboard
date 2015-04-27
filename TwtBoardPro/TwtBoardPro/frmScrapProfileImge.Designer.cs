namespace twtboardpro
{
    partial class frmScrapProfileImge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScrapProfileImge));
            this.txtScrapeUserName = new System.Windows.Forms.TextBox();
            this.lblScrapeImageHeading = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lstProcessLoggerForImage = new System.Windows.Forms.ListBox();
            this.grpScrapProfileImage = new System.Windows.Forms.GroupBox();
            this.rad_UserID = new System.Windows.Forms.RadioButton();
            this.rad_UserName = new System.Windows.Forms.RadioButton();
            this.btnBrowseUploadUserInfo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStopScraing = new System.Windows.Forms.Button();
            this.btnStartScraping = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpScrapProfileImage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtScrapeUserName
            // 
            this.txtScrapeUserName.Location = new System.Drawing.Point(101, 28);
            this.txtScrapeUserName.Name = "txtScrapeUserName";
            this.txtScrapeUserName.ReadOnly = true;
            this.txtScrapeUserName.Size = new System.Drawing.Size(400, 21);
            this.txtScrapeUserName.TabIndex = 1;
            // 
            // lblScrapeImageHeading
            // 
            this.lblScrapeImageHeading.AutoSize = true;
            this.lblScrapeImageHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblScrapeImageHeading.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrapeImageHeading.Location = new System.Drawing.Point(141, 9);
            this.lblScrapeImageHeading.Name = "lblScrapeImageHeading";
            this.lblScrapeImageHeading.Size = new System.Drawing.Size(330, 16);
            this.lblScrapeImageHeading.TabIndex = 3;
            this.lblScrapeImageHeading.Text = "Using User Name / User ID Scrape Profile Images";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(4, 31);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(87, 13);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "User Name/Id";
            // 
            // lstProcessLoggerForImage
            // 
            this.lstProcessLoggerForImage.FormattingEnabled = true;
            this.lstProcessLoggerForImage.Location = new System.Drawing.Point(6, 17);
            this.lstProcessLoggerForImage.Name = "lstProcessLoggerForImage";
            this.lstProcessLoggerForImage.Size = new System.Drawing.Size(611, 121);
            this.lstProcessLoggerForImage.TabIndex = 13;
            // 
            // grpScrapProfileImage
            // 
            this.grpScrapProfileImage.BackColor = System.Drawing.Color.Transparent;
            this.grpScrapProfileImage.Controls.Add(this.rad_UserID);
            this.grpScrapProfileImage.Controls.Add(this.rad_UserName);
            this.grpScrapProfileImage.Controls.Add(this.btnBrowseUploadUserInfo);
            this.grpScrapProfileImage.Controls.Add(this.txtScrapeUserName);
            this.grpScrapProfileImage.Controls.Add(this.lblUserName);
            this.grpScrapProfileImage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpScrapProfileImage.Location = new System.Drawing.Point(12, 38);
            this.grpScrapProfileImage.Name = "grpScrapProfileImage";
            this.grpScrapProfileImage.Size = new System.Drawing.Size(625, 92);
            this.grpScrapProfileImage.TabIndex = 64;
            this.grpScrapProfileImage.TabStop = false;
            this.grpScrapProfileImage.Text = "Input";
            // 
            // rad_UserID
            // 
            this.rad_UserID.AutoSize = true;
            this.rad_UserID.Location = new System.Drawing.Point(254, 57);
            this.rad_UserID.Name = "rad_UserID";
            this.rad_UserID.Size = new System.Drawing.Size(69, 17);
            this.rad_UserID.TabIndex = 78;
            this.rad_UserID.TabStop = true;
            this.rad_UserID.Text = "User ID";
            this.rad_UserID.UseVisualStyleBackColor = true;
            this.rad_UserID.CheckedChanged += new System.EventHandler(this.rad_UserID_CheckedChanged);
            // 
            // rad_UserName
            // 
            this.rad_UserName.AutoSize = true;
            this.rad_UserName.Location = new System.Drawing.Point(101, 57);
            this.rad_UserName.Name = "rad_UserName";
            this.rad_UserName.Size = new System.Drawing.Size(88, 17);
            this.rad_UserName.TabIndex = 77;
            this.rad_UserName.TabStop = true;
            this.rad_UserName.Text = "User Name";
            this.rad_UserName.UseVisualStyleBackColor = true;
            this.rad_UserName.CheckedChanged += new System.EventHandler(this.rad_UserName_CheckedChanged);
            // 
            // btnBrowseUploadUserInfo
            // 
            this.btnBrowseUploadUserInfo.BackColor = System.Drawing.Color.Transparent;
            this.btnBrowseUploadUserInfo.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnBrowseUploadUserInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrowseUploadUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseUploadUserInfo.ForeColor = System.Drawing.Color.Transparent;
            this.btnBrowseUploadUserInfo.Location = new System.Drawing.Point(516, 24);
            this.btnBrowseUploadUserInfo.Name = "btnBrowseUploadUserInfo";
            this.btnBrowseUploadUserInfo.Size = new System.Drawing.Size(91, 32);
            this.btnBrowseUploadUserInfo.TabIndex = 76;
            this.btnBrowseUploadUserInfo.UseVisualStyleBackColor = false;
            this.btnBrowseUploadUserInfo.Click += new System.EventHandler(this.btnBrowseUploadUserInfo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnStopScraing);
            this.groupBox1.Controls.Add(this.btnStartScraping);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(618, 73);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Action Submit";
            // 
            // btnStopScraing
            // 
            this.btnStopScraing.BackColor = System.Drawing.Color.Transparent;
            this.btnStopScraing.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btnStopScraing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopScraing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopScraing.ForeColor = System.Drawing.Color.Transparent;
            this.btnStopScraing.Location = new System.Drawing.Point(302, 25);
            this.btnStopScraing.Name = "btnStopScraing";
            this.btnStopScraing.Size = new System.Drawing.Size(91, 32);
            this.btnStopScraing.TabIndex = 75;
            this.btnStopScraing.UseVisualStyleBackColor = false;
            this.btnStopScraing.Click += new System.EventHandler(this.btnStopScraing_Click);
            // 
            // btnStartScraping
            // 
            this.btnStartScraping.BackColor = System.Drawing.Color.Transparent;
            this.btnStartScraping.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStartScraping.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartScraping.Enabled = false;
            this.btnStartScraping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartScraping.ForeColor = System.Drawing.Color.Black;
            this.btnStartScraping.Location = new System.Drawing.Point(129, 25);
            this.btnStartScraping.Name = "btnStartScraping";
            this.btnStartScraping.Size = new System.Drawing.Size(88, 32);
            this.btnStartScraping.TabIndex = 74;
            this.btnStartScraping.UseVisualStyleBackColor = false;
            this.btnStartScraping.Click += new System.EventHandler(this.btnStartScraping_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lstProcessLoggerForImage);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 242);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 150);
            this.groupBox2.TabIndex = 66;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process Logger";
            // 
            // frmScrapProfileImge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 404);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpScrapProfileImage);
            this.Controls.Add(this.lblScrapeImageHeading);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScrapProfileImge";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScrapeProfileImage";
            this.Load += new System.EventHandler(this.frmScrapProfileImge_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmScrapProfileImge_Paint);
            this.grpScrapProfileImage.ResumeLayout(false);
            this.grpScrapProfileImage.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtScrapeUserName;
        private System.Windows.Forms.Label lblScrapeImageHeading;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.ListBox lstProcessLoggerForImage;
        private System.Windows.Forms.GroupBox grpScrapProfileImage;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStartScraping;
        private System.Windows.Forms.Button btnStopScraing;
        private System.Windows.Forms.Button btnBrowseUploadUserInfo;
        private System.Windows.Forms.RadioButton rad_UserID;
        private System.Windows.Forms.RadioButton rad_UserName;
    }
}