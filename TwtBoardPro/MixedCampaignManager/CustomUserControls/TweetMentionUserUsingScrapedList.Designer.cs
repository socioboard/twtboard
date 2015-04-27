namespace MixedCampaignManager.CustomUserControls
{
    partial class TweetMentionUserUsingScrapedList
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label6 = new System.Windows.Forms.Label();
            this.chkTweetFollowing = new System.Windows.Forms.CheckBox();
            this.chkTweetFollowers = new System.Windows.Forms.CheckBox();
            this.txtTweetCountMentinUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTweetMentionNoofUserCount = new System.Windows.Forms.TextBox();
            this.grpMentionUserScrapedList = new System.Windows.Forms.GroupBox();
            this.txtRetweetMentionUserName = new System.Windows.Forms.TextBox();
            this.grpMentionUserScrapedList.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(24, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 93;
            this.label6.Text = "Mention User Name";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // chkTweetFollowing
            // 
            this.chkTweetFollowing.AutoSize = true;
            this.chkTweetFollowing.BackColor = System.Drawing.Color.Transparent;
            this.chkTweetFollowing.Location = new System.Drawing.Point(133, 46);
            this.chkTweetFollowing.Name = "chkTweetFollowing";
            this.chkTweetFollowing.Size = new System.Drawing.Size(70, 17);
            this.chkTweetFollowing.TabIndex = 97;
            this.chkTweetFollowing.Text = "Following";
            this.chkTweetFollowing.UseVisualStyleBackColor = false;
            this.chkTweetFollowing.CheckedChanged += new System.EventHandler(this.chkTweetFollowing_CheckedChanged);
            // 
            // chkTweetFollowers
            // 
            this.chkTweetFollowers.AutoSize = true;
            this.chkTweetFollowers.BackColor = System.Drawing.Color.Transparent;
            this.chkTweetFollowers.Location = new System.Drawing.Point(29, 47);
            this.chkTweetFollowers.Name = "chkTweetFollowers";
            this.chkTweetFollowers.Size = new System.Drawing.Size(70, 17);
            this.chkTweetFollowers.TabIndex = 96;
            this.chkTweetFollowers.Text = "Followers";
            this.chkTweetFollowers.UseVisualStyleBackColor = false;
            this.chkTweetFollowers.CheckedChanged += new System.EventHandler(this.chkTweetFollowers_CheckedChanged);
            // 
            // txtTweetCountMentinUser
            // 
            this.txtTweetCountMentinUser.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTweetCountMentinUser.ForeColor = System.Drawing.Color.Black;
            this.txtTweetCountMentinUser.Location = new System.Drawing.Point(173, 71);
            this.txtTweetCountMentinUser.Name = "txtTweetCountMentinUser";
            this.txtTweetCountMentinUser.Size = new System.Drawing.Size(47, 18);
            this.txtTweetCountMentinUser.TabIndex = 98;
            this.txtTweetCountMentinUser.Text = "50";
            this.txtTweetCountMentinUser.TextChanged += new System.EventHandler(this.txtTweetCountMentinUser_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(26, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 99;
            this.label1.Text = "No of Users To Be scraped";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(213, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 100;
            this.label2.Text = "Count Mention User :";
            // 
            // txtTweetMentionNoofUserCount
            // 
            this.txtTweetMentionNoofUserCount.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTweetMentionNoofUserCount.ForeColor = System.Drawing.Color.Black;
            this.txtTweetMentionNoofUserCount.Location = new System.Drawing.Point(324, 45);
            this.txtTweetMentionNoofUserCount.Name = "txtTweetMentionNoofUserCount";
            this.txtTweetMentionNoofUserCount.Size = new System.Drawing.Size(47, 18);
            this.txtTweetMentionNoofUserCount.TabIndex = 101;
            this.txtTweetMentionNoofUserCount.Text = "1";
            this.txtTweetMentionNoofUserCount.TextChanged += new System.EventHandler(this.txtTweetMentionNoofUserCount_TextChanged);
            // 
            // grpMentionUserScrapedList
            // 
            this.grpMentionUserScrapedList.BackColor = System.Drawing.Color.Transparent;
            this.grpMentionUserScrapedList.Controls.Add(this.txtRetweetMentionUserName);
            this.grpMentionUserScrapedList.Location = new System.Drawing.Point(3, 1);
            this.grpMentionUserScrapedList.Name = "grpMentionUserScrapedList";
            this.grpMentionUserScrapedList.Size = new System.Drawing.Size(398, 100);
            this.grpMentionUserScrapedList.TabIndex = 102;
            this.grpMentionUserScrapedList.TabStop = false;
            this.grpMentionUserScrapedList.Text = "Mention User Details";
            this.grpMentionUserScrapedList.Enter += new System.EventHandler(this.grpMentionUserScrapedList_Enter);
            // 
            // txtRetweetMentionUserName
            // 
            this.txtRetweetMentionUserName.Location = new System.Drawing.Point(130, 18);
            this.txtRetweetMentionUserName.Name = "txtRetweetMentionUserName";
            this.txtRetweetMentionUserName.Size = new System.Drawing.Size(187, 20);
            this.txtRetweetMentionUserName.TabIndex = 0;
            this.txtRetweetMentionUserName.Text = "Enter UserName";
            this.txtRetweetMentionUserName.TextChanged += new System.EventHandler(this.txtRetweetMentionUserName_TextChanged);
            // 
            // TweetMentionUserUsingScrapedList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cyan;
            this.Controls.Add(this.txtTweetMentionNoofUserCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTweetCountMentinUser);
            this.Controls.Add(this.chkTweetFollowing);
            this.Controls.Add(this.chkTweetFollowers);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.grpMentionUserScrapedList);
            this.Name = "TweetMentionUserUsingScrapedList";
            this.Size = new System.Drawing.Size(404, 109);
            this.Load += new System.EventHandler(this.TweetMentionUserUsingScrapedList_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TweetMentionUserUsingScrapedList_Paint);
            this.grpMentionUserScrapedList.ResumeLayout(false);
            this.grpMentionUserScrapedList.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkTweetFollowing;
        private System.Windows.Forms.CheckBox chkTweetFollowers;
        private System.Windows.Forms.TextBox txtTweetCountMentinUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTweetMentionNoofUserCount;
        private System.Windows.Forms.GroupBox grpMentionUserScrapedList;
        private System.Windows.Forms.TextBox txtRetweetMentionUserName;
    }
}
