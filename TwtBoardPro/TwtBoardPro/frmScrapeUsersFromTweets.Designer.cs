namespace twtboardpro
{
    partial class frmScrapeUsersFromTweets
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
            this.grpScrapeMemberUsingUrl = new System.Windows.Forms.GroupBox();
            this.label43 = new System.Windows.Forms.Label();
            this.txtUploadTweetsUrl = new System.Windows.Forms.TextBox();
            this.btnUploadTweetsUrl = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label115 = new System.Windows.Forms.Label();
            this.btnStopScrap = new System.Windows.Forms.Button();
            this.txtlimitScrapeUsers = new System.Windows.Forms.TextBox();
            this.btnStartScrap = new System.Windows.Forms.Button();
            this.grpSendDirectMessage = new System.Windows.Forms.GroupBox();
            this.lstScrapMember = new System.Windows.Forms.ListBox();
            this.grpScrapeMemberUsingUrl.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpSendDirectMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpScrapeMemberUsingUrl
            // 
            this.grpScrapeMemberUsingUrl.BackColor = System.Drawing.Color.Transparent;
            this.grpScrapeMemberUsingUrl.Controls.Add(this.label43);
            this.grpScrapeMemberUsingUrl.Controls.Add(this.txtUploadTweetsUrl);
            this.grpScrapeMemberUsingUrl.Controls.Add(this.btnUploadTweetsUrl);
            this.grpScrapeMemberUsingUrl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpScrapeMemberUsingUrl.Location = new System.Drawing.Point(12, 12);
            this.grpScrapeMemberUsingUrl.Name = "grpScrapeMemberUsingUrl";
            this.grpScrapeMemberUsingUrl.Size = new System.Drawing.Size(704, 86);
            this.grpScrapeMemberUsingUrl.TabIndex = 9;
            this.grpScrapeMemberUsingUrl.TabStop = false;
            this.grpScrapeMemberUsingUrl.Text = "Scrape  Member";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.ForeColor = System.Drawing.Color.Black;
            this.label43.Location = new System.Drawing.Point(63, 34);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(71, 13);
            this.label43.TabIndex = 14;
            this.label43.Text = "Upload Url:";
            // 
            // txtUploadTweetsUrl
            // 
            this.txtUploadTweetsUrl.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUploadTweetsUrl.ForeColor = System.Drawing.Color.Black;
            this.txtUploadTweetsUrl.Location = new System.Drawing.Point(163, 31);
            this.txtUploadTweetsUrl.Name = "txtUploadTweetsUrl";
            this.txtUploadTweetsUrl.ReadOnly = true;
            this.txtUploadTweetsUrl.Size = new System.Drawing.Size(378, 21);
            this.txtUploadTweetsUrl.TabIndex = 12;
            // 
            // btnUploadTweetsUrl
            // 
            this.btnUploadTweetsUrl.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadTweetsUrl.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnUploadTweetsUrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadTweetsUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadTweetsUrl.ForeColor = System.Drawing.Color.Black;
            this.btnUploadTweetsUrl.Location = new System.Drawing.Point(571, 31);
            this.btnUploadTweetsUrl.Name = "btnUploadTweetsUrl";
            this.btnUploadTweetsUrl.Size = new System.Drawing.Size(80, 27);
            this.btnUploadTweetsUrl.TabIndex = 13;
            this.btnUploadTweetsUrl.UseVisualStyleBackColor = false;
            this.btnUploadTweetsUrl.Click += new System.EventHandler(this.btnUploadTweetsUrl_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label115);
            this.groupBox1.Controls.Add(this.btnStopScrap);
            this.groupBox1.Controls.Add(this.txtlimitScrapeUsers);
            this.groupBox1.Controls.Add(this.btnStartScrap);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 67);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label115
            // 
            this.label115.AutoSize = true;
            this.label115.BackColor = System.Drawing.Color.Transparent;
            this.label115.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label115.Location = new System.Drawing.Point(63, 27);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(168, 13);
            this.label115.TabIndex = 16;
            this.label115.Text = "No Of Users To Be Scraped:";
            // 
            // btnStopScrap
            // 
            this.btnStopScrap.BackColor = System.Drawing.Color.Transparent;
            this.btnStopScrap.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btnStopScrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStopScrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopScrap.ForeColor = System.Drawing.Color.Black;
            this.btnStopScrap.Location = new System.Drawing.Point(438, 20);
            this.btnStopScrap.Name = "btnStopScrap";
            this.btnStopScrap.Size = new System.Drawing.Size(83, 27);
            this.btnStopScrap.TabIndex = 15;
            this.btnStopScrap.UseVisualStyleBackColor = false;
            // 
            // txtlimitScrapeUsers
            // 
            this.txtlimitScrapeUsers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlimitScrapeUsers.Location = new System.Drawing.Point(237, 24);
            this.txtlimitScrapeUsers.Name = "txtlimitScrapeUsers";
            this.txtlimitScrapeUsers.Size = new System.Drawing.Size(68, 21);
            this.txtlimitScrapeUsers.TabIndex = 17;
            this.txtlimitScrapeUsers.Text = "50";
            this.txtlimitScrapeUsers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStartScrap
            // 
            this.btnStartScrap.BackColor = System.Drawing.Color.Transparent;
            this.btnStartScrap.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStartScrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartScrap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartScrap.ForeColor = System.Drawing.Color.Black;
            this.btnStartScrap.Location = new System.Drawing.Point(339, 19);
            this.btnStartScrap.Name = "btnStartScrap";
            this.btnStartScrap.Size = new System.Drawing.Size(77, 28);
            this.btnStartScrap.TabIndex = 14;
            this.btnStartScrap.UseVisualStyleBackColor = false;
            this.btnStartScrap.Click += new System.EventHandler(this.btnStartScrap_Click);
            // 
            // grpSendDirectMessage
            // 
            this.grpSendDirectMessage.BackColor = System.Drawing.Color.Transparent;
            this.grpSendDirectMessage.Controls.Add(this.lstScrapMember);
            this.grpSendDirectMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSendDirectMessage.Location = new System.Drawing.Point(10, 171);
            this.grpSendDirectMessage.Name = "grpSendDirectMessage";
            this.grpSendDirectMessage.Size = new System.Drawing.Size(707, 130);
            this.grpSendDirectMessage.TabIndex = 81;
            this.grpSendDirectMessage.TabStop = false;
            this.grpSendDirectMessage.Text = "Logger";
            // 
            // lstScrapMember
            // 
            this.lstScrapMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstScrapMember.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstScrapMember.FormattingEnabled = true;
            this.lstScrapMember.HorizontalScrollbar = true;
            this.lstScrapMember.Location = new System.Drawing.Point(3, 17);
            this.lstScrapMember.Name = "lstScrapMember";
            this.lstScrapMember.ScrollAlwaysVisible = true;
            this.lstScrapMember.Size = new System.Drawing.Size(701, 110);
            this.lstScrapMember.TabIndex = 18;
            // 
            // frmScrapeUsersFromTweets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 312);
            this.Controls.Add(this.grpSendDirectMessage);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpScrapeMemberUsingUrl);
            this.Name = "frmScrapeUsersFromTweets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scrape Users from Tweets";
            this.Load += new System.EventHandler(this.frmScrapeUsersFromTweets_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmScrapeUsersFromTweets_Paint);
            this.grpScrapeMemberUsingUrl.ResumeLayout(false);
            this.grpScrapeMemberUsingUrl.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpSendDirectMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpScrapeMemberUsingUrl;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtUploadTweetsUrl;
        private System.Windows.Forms.Button btnUploadTweetsUrl;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label115;
        private System.Windows.Forms.Button btnStopScrap;
        private System.Windows.Forms.TextBox txtlimitScrapeUsers;
        private System.Windows.Forms.Button btnStartScrap;
        private System.Windows.Forms.GroupBox grpSendDirectMessage;
        private System.Windows.Forms.ListBox lstScrapMember;
    }
}