namespace MixedCampaignManager.CustomUserControls
{
    partial class tweetusercontrols
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
            this.grp_tweetsettings = new System.Windows.Forms.GroupBox();
            this.btnGetHashTagsCampaign = new System.Windows.Forms.Button();
            this.grp_TweetImageFile = new System.Windows.Forms.GroupBox();
            this.btn_BrowseTweetImage = new System.Windows.Forms.Button();
            this.txt_TweetImageFilePath = new System.Windows.Forms.TextBox();
            this.chk_TweetWithImage = new System.Windows.Forms.CheckBox();
            this.chkboxKeepSingleMessage = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkTweetMentionUserScrapedList = new System.Windows.Forms.CheckBox();
            this.chkCampTweetMentionUser = new System.Windows.Forms.CheckBox();
            this.btnUploadTweetMentionUserList = new System.Windows.Forms.Button();
            this.txt_CmpTweetMentionUserList = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnUploadTweetMessage = new System.Windows.Forms.Button();
            this.txt_CmpTweetMessageFile = new System.Windows.Forms.TextBox();
            this.chkAllTweetsPerAccount = new System.Windows.Forms.CheckBox();
            this.chkbosUseHashTags = new System.Windows.Forms.CheckBox();
            this.Grp_TweetParDay = new System.Windows.Forms.GroupBox();
            this.ChkboxTweetPerday = new System.Windows.Forms.CheckBox();
            this.label85 = new System.Windows.Forms.Label();
            this.txtMaximumTweet = new System.Windows.Forms.TextBox();
            this.grp_NoOfTweetParAc = new System.Windows.Forms.GroupBox();
            this.txtNoOfTweets = new System.Windows.Forms.TextBox();
            this.lblNoOfTweets = new System.Windows.Forms.Label();
            this.grp_tweetsettings.SuspendLayout();
            this.grp_TweetImageFile.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Grp_TweetParDay.SuspendLayout();
            this.grp_NoOfTweetParAc.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_tweetsettings
            // 
            this.grp_tweetsettings.Controls.Add(this.btnGetHashTagsCampaign);
            this.grp_tweetsettings.Controls.Add(this.grp_TweetImageFile);
            this.grp_tweetsettings.Controls.Add(this.chk_TweetWithImage);
            this.grp_tweetsettings.Controls.Add(this.chkboxKeepSingleMessage);
            this.grp_tweetsettings.Controls.Add(this.groupBox1);
            this.grp_tweetsettings.Controls.Add(this.chkAllTweetsPerAccount);
            this.grp_tweetsettings.Controls.Add(this.chkbosUseHashTags);
            this.grp_tweetsettings.Controls.Add(this.Grp_TweetParDay);
            this.grp_tweetsettings.Controls.Add(this.grp_NoOfTweetParAc);
            this.grp_tweetsettings.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_tweetsettings.Location = new System.Drawing.Point(3, 2);
            this.grp_tweetsettings.Name = "grp_tweetsettings";
            this.grp_tweetsettings.Size = new System.Drawing.Size(773, 152);
            this.grp_tweetsettings.TabIndex = 0;
            this.grp_tweetsettings.TabStop = false;
            this.grp_tweetsettings.Text = "Tweeter Settings";
            this.grp_tweetsettings.Enter += new System.EventHandler(this.grp_tweetsettings_Enter);
            // 
            // btnGetHashTagsCampaign
            // 
            this.btnGetHashTagsCampaign.BackColor = System.Drawing.Color.Transparent;
            this.btnGetHashTagsCampaign.BackgroundImage = global::MixedCampaignManager.Properties.Resources.get_hash_tags_campaign;
            this.btnGetHashTagsCampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGetHashTagsCampaign.Location = new System.Drawing.Point(91, 123);
            this.btnGetHashTagsCampaign.Name = "btnGetHashTagsCampaign";
            this.btnGetHashTagsCampaign.Size = new System.Drawing.Size(119, 26);
            this.btnGetHashTagsCampaign.TabIndex = 98;
            this.btnGetHashTagsCampaign.UseVisualStyleBackColor = false;
            this.btnGetHashTagsCampaign.Visible = false;
            this.btnGetHashTagsCampaign.Click += new System.EventHandler(this.btnGetHashTagsCampaign_Click);
            // 
            // grp_TweetImageFile
            // 
            this.grp_TweetImageFile.Controls.Add(this.btn_BrowseTweetImage);
            this.grp_TweetImageFile.Controls.Add(this.txt_TweetImageFilePath);
            this.grp_TweetImageFile.Location = new System.Drawing.Point(223, 64);
            this.grp_TweetImageFile.Name = "grp_TweetImageFile";
            this.grp_TweetImageFile.Size = new System.Drawing.Size(544, 36);
            this.grp_TweetImageFile.TabIndex = 97;
            this.grp_TweetImageFile.TabStop = false;
            this.grp_TweetImageFile.Visible = false;
            // 
            // btn_BrowseTweetImage
            // 
            this.btn_BrowseTweetImage.BackColor = System.Drawing.Color.Transparent;
            this.btn_BrowseTweetImage.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btn_BrowseTweetImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_BrowseTweetImage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_BrowseTweetImage.ForeColor = System.Drawing.Color.Black;
            this.btn_BrowseTweetImage.Location = new System.Drawing.Point(421, 7);
            this.btn_BrowseTweetImage.Name = "btn_BrowseTweetImage";
            this.btn_BrowseTweetImage.Size = new System.Drawing.Size(78, 27);
            this.btn_BrowseTweetImage.TabIndex = 94;
            this.btn_BrowseTweetImage.UseVisualStyleBackColor = false;
            this.btn_BrowseTweetImage.Click += new System.EventHandler(this.btn_BrowseTweetImage_Click);
            // 
            // txt_TweetImageFilePath
            // 
            this.txt_TweetImageFilePath.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_TweetImageFilePath.Location = new System.Drawing.Point(16, 11);
            this.txt_TweetImageFilePath.Name = "txt_TweetImageFilePath";
            this.txt_TweetImageFilePath.ReadOnly = true;
            this.txt_TweetImageFilePath.Size = new System.Drawing.Size(399, 18);
            this.txt_TweetImageFilePath.TabIndex = 0;
            this.txt_TweetImageFilePath.TextChanged += new System.EventHandler(this.txt_TweetImageFilePath_TextChanged);
            // 
            // chk_TweetWithImage
            // 
            this.chk_TweetWithImage.AutoSize = true;
            this.chk_TweetWithImage.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_TweetWithImage.Location = new System.Drawing.Point(7, 71);
            this.chk_TweetWithImage.Name = "chk_TweetWithImage";
            this.chk_TweetWithImage.Size = new System.Drawing.Size(114, 16);
            this.chk_TweetWithImage.TabIndex = 96;
            this.chk_TweetWithImage.Text = "Tweet With Image";
            this.chk_TweetWithImage.UseVisualStyleBackColor = true;
            this.chk_TweetWithImage.CheckedChanged += new System.EventHandler(this.chk_TweetWithImage_CheckedChanged);
            // 
            // chkboxKeepSingleMessage
            // 
            this.chkboxKeepSingleMessage.AutoSize = true;
            this.chkboxKeepSingleMessage.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxKeepSingleMessage.ForeColor = System.Drawing.Color.Black;
            this.chkboxKeepSingleMessage.Location = new System.Drawing.Point(7, 90);
            this.chkboxKeepSingleMessage.Name = "chkboxKeepSingleMessage";
            this.chkboxKeepSingleMessage.Size = new System.Drawing.Size(174, 16);
            this.chkboxKeepSingleMessage.TabIndex = 95;
            this.chkboxKeepSingleMessage.Text = "Use Message With Duplicates";
            this.chkboxKeepSingleMessage.UseVisualStyleBackColor = true;
            this.chkboxKeepSingleMessage.CheckedChanged += new System.EventHandler(this.chkboxKeepSingleMessage_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkTweetMentionUserScrapedList);
            this.groupBox1.Controls.Add(this.chkCampTweetMentionUser);
            this.groupBox1.Controls.Add(this.btnUploadTweetMentionUserList);
            this.groupBox1.Controls.Add(this.txt_CmpTweetMentionUserList);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnUploadTweetMessage);
            this.groupBox1.Controls.Add(this.txt_CmpTweetMessageFile);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(7, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 48);
            this.groupBox1.TabIndex = 94;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MSG File";
            // 
            // chkTweetMentionUserScrapedList
            // 
            this.chkTweetMentionUserScrapedList.AutoSize = true;
            this.chkTweetMentionUserScrapedList.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTweetMentionUserScrapedList.Location = new System.Drawing.Point(475, 19);
            this.chkTweetMentionUserScrapedList.Name = "chkTweetMentionUserScrapedList";
            this.chkTweetMentionUserScrapedList.Size = new System.Drawing.Size(186, 16);
            this.chkTweetMentionUserScrapedList.TabIndex = 98;
            this.chkTweetMentionUserScrapedList.Text = "Mention User Using ScrapedList";
            this.chkTweetMentionUserScrapedList.UseVisualStyleBackColor = true;
            this.chkTweetMentionUserScrapedList.CheckedChanged += new System.EventHandler(this.chkTweetMentionUserScrapedList_CheckedChanged);
            // 
            // chkCampTweetMentionUser
            // 
            this.chkCampTweetMentionUser.AutoSize = true;
            this.chkCampTweetMentionUser.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCampTweetMentionUser.Location = new System.Drawing.Point(379, 19);
            this.chkCampTweetMentionUser.Name = "chkCampTweetMentionUser";
            this.chkCampTweetMentionUser.Size = new System.Drawing.Size(91, 16);
            this.chkCampTweetMentionUser.TabIndex = 97;
            this.chkCampTweetMentionUser.Text = "Mention User";
            this.chkCampTweetMentionUser.UseVisualStyleBackColor = true;
            this.chkCampTweetMentionUser.CheckedChanged += new System.EventHandler(this.chkCampTweetMentionUser_CheckedChanged);
            // 
            // btnUploadTweetMentionUserList
            // 
            this.btnUploadTweetMentionUserList.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadTweetMentionUserList.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btnUploadTweetMentionUserList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadTweetMentionUserList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadTweetMentionUserList.ForeColor = System.Drawing.Color.Black;
            this.btnUploadTweetMentionUserList.Location = new System.Drawing.Point(683, 14);
            this.btnUploadTweetMentionUserList.Name = "btnUploadTweetMentionUserList";
            this.btnUploadTweetMentionUserList.Size = new System.Drawing.Size(71, 28);
            this.btnUploadTweetMentionUserList.TabIndex = 95;
            this.btnUploadTweetMentionUserList.UseVisualStyleBackColor = false;
            this.btnUploadTweetMentionUserList.Visible = false;
            this.btnUploadTweetMentionUserList.Click += new System.EventHandler(this.btnUploadTweetMentionUserList_Click);
            // 
            // txt_CmpTweetMentionUserList
            // 
            this.txt_CmpTweetMentionUserList.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CmpTweetMentionUserList.ForeColor = System.Drawing.Color.Black;
            this.txt_CmpTweetMentionUserList.Location = new System.Drawing.Point(470, 19);
            this.txt_CmpTweetMentionUserList.Name = "txt_CmpTweetMentionUserList";
            this.txt_CmpTweetMentionUserList.ReadOnly = true;
            this.txt_CmpTweetMentionUserList.Size = new System.Drawing.Size(212, 18);
            this.txt_CmpTweetMentionUserList.TabIndex = 94;
            this.txt_CmpTweetMentionUserList.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 91;
            this.label6.Text = "Tweet Message File";
            // 
            // btnUploadTweetMessage
            // 
            this.btnUploadTweetMessage.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadTweetMessage.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btnUploadTweetMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadTweetMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadTweetMessage.ForeColor = System.Drawing.Color.Black;
            this.btnUploadTweetMessage.Location = new System.Drawing.Point(302, 11);
            this.btnUploadTweetMessage.Name = "btnUploadTweetMessage";
            this.btnUploadTweetMessage.Size = new System.Drawing.Size(71, 28);
            this.btnUploadTweetMessage.TabIndex = 93;
            this.btnUploadTweetMessage.UseVisualStyleBackColor = false;
            this.btnUploadTweetMessage.Click += new System.EventHandler(this.btnUploadTweetMessage_Click);
            // 
            // txt_CmpTweetMessageFile
            // 
            this.txt_CmpTweetMessageFile.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CmpTweetMessageFile.ForeColor = System.Drawing.Color.Black;
            this.txt_CmpTweetMessageFile.Location = new System.Drawing.Point(111, 17);
            this.txt_CmpTweetMessageFile.Name = "txt_CmpTweetMessageFile";
            this.txt_CmpTweetMessageFile.ReadOnly = true;
            this.txt_CmpTweetMessageFile.Size = new System.Drawing.Size(185, 18);
            this.txt_CmpTweetMessageFile.TabIndex = 92;
            this.txt_CmpTweetMessageFile.TextChanged += new System.EventHandler(this.txt_CmpTweetMessageFile_TextChanged);
            // 
            // chkAllTweetsPerAccount
            // 
            this.chkAllTweetsPerAccount.AutoSize = true;
            this.chkAllTweetsPerAccount.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllTweetsPerAccount.ForeColor = System.Drawing.Color.Black;
            this.chkAllTweetsPerAccount.Location = new System.Drawing.Point(7, 109);
            this.chkAllTweetsPerAccount.Name = "chkAllTweetsPerAccount";
            this.chkAllTweetsPerAccount.Size = new System.Drawing.Size(142, 16);
            this.chkAllTweetsPerAccount.TabIndex = 90;
            this.chkAllTweetsPerAccount.Text = "All Tweets Per Account";
            this.chkAllTweetsPerAccount.UseVisualStyleBackColor = true;
            this.chkAllTweetsPerAccount.CheckedChanged += new System.EventHandler(this.chkAllTweetsPerAccount_CheckedChanged);
            // 
            // chkbosUseHashTags
            // 
            this.chkbosUseHashTags.AutoSize = true;
            this.chkbosUseHashTags.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbosUseHashTags.ForeColor = System.Drawing.Color.Black;
            this.chkbosUseHashTags.Location = new System.Drawing.Point(7, 128);
            this.chkbosUseHashTags.Name = "chkbosUseHashTags";
            this.chkbosUseHashTags.Size = new System.Drawing.Size(78, 16);
            this.chkbosUseHashTags.TabIndex = 89;
            this.chkbosUseHashTags.Text = "Hash Tags";
            this.chkbosUseHashTags.UseVisualStyleBackColor = true;
            this.chkbosUseHashTags.CheckedChanged += new System.EventHandler(this.chkbosUseHashTags_CheckedChanged);
            // 
            // Grp_TweetParDay
            // 
            this.Grp_TweetParDay.Controls.Add(this.ChkboxTweetPerday);
            this.Grp_TweetParDay.Controls.Add(this.label85);
            this.Grp_TweetParDay.Controls.Add(this.txtMaximumTweet);
            this.Grp_TweetParDay.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Grp_TweetParDay.ForeColor = System.Drawing.Color.Black;
            this.Grp_TweetParDay.Location = new System.Drawing.Point(224, 103);
            this.Grp_TweetParDay.Name = "Grp_TweetParDay";
            this.Grp_TweetParDay.Size = new System.Drawing.Size(397, 44);
            this.Grp_TweetParDay.TabIndex = 87;
            this.Grp_TweetParDay.TabStop = false;
            this.Grp_TweetParDay.Text = "Tweet Per Day";
            // 
            // ChkboxTweetPerday
            // 
            this.ChkboxTweetPerday.AutoSize = true;
            this.ChkboxTweetPerday.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ChkboxTweetPerday.ForeColor = System.Drawing.Color.Black;
            this.ChkboxTweetPerday.Location = new System.Drawing.Point(16, 19);
            this.ChkboxTweetPerday.Name = "ChkboxTweetPerday";
            this.ChkboxTweetPerday.Size = new System.Drawing.Size(119, 17);
            this.ChkboxTweetPerday.TabIndex = 80;
            this.ChkboxTweetPerday.Text = "Use Tweet Per Day";
            this.ChkboxTweetPerday.UseVisualStyleBackColor = true;
            this.ChkboxTweetPerday.CheckedChanged += new System.EventHandler(this.ChkboxTweetPerday_CheckedChanged);
            // 
            // label85
            // 
            this.label85.AutoSize = true;
            this.label85.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label85.ForeColor = System.Drawing.Color.Black;
            this.label85.Location = new System.Drawing.Point(159, 21);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(156, 13);
            this.label85.TabIndex = 77;
            this.label85.Text = "Maximum No Of Tweet Per Day";
            this.label85.Visible = false;
            // 
            // txtMaximumTweet
            // 
            this.txtMaximumTweet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaximumTweet.Location = new System.Drawing.Point(324, 17);
            this.txtMaximumTweet.Name = "txtMaximumTweet";
            this.txtMaximumTweet.Size = new System.Drawing.Size(31, 22);
            this.txtMaximumTweet.TabIndex = 79;
            this.txtMaximumTweet.Text = "10";
            this.txtMaximumTweet.Visible = false;
            this.txtMaximumTweet.TextChanged += new System.EventHandler(this.txtMaximumTweet_TextChanged);
            // 
            // grp_NoOfTweetParAc
            // 
            this.grp_NoOfTweetParAc.Controls.Add(this.txtNoOfTweets);
            this.grp_NoOfTweetParAc.Controls.Add(this.lblNoOfTweets);
            this.grp_NoOfTweetParAc.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_NoOfTweetParAc.ForeColor = System.Drawing.Color.Black;
            this.grp_NoOfTweetParAc.Location = new System.Drawing.Point(627, 103);
            this.grp_NoOfTweetParAc.Name = "grp_NoOfTweetParAc";
            this.grp_NoOfTweetParAc.Size = new System.Drawing.Size(140, 44);
            this.grp_NoOfTweetParAc.TabIndex = 88;
            this.grp_NoOfTweetParAc.TabStop = false;
            this.grp_NoOfTweetParAc.Text = "No Of Tweets";
            // 
            // txtNoOfTweets
            // 
            this.txtNoOfTweets.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfTweets.ForeColor = System.Drawing.Color.Black;
            this.txtNoOfTweets.Location = new System.Drawing.Point(90, 15);
            this.txtNoOfTweets.Name = "txtNoOfTweets";
            this.txtNoOfTweets.Size = new System.Drawing.Size(31, 22);
            this.txtNoOfTweets.TabIndex = 75;
            this.txtNoOfTweets.Text = "10";
            this.txtNoOfTweets.TextChanged += new System.EventHandler(this.txtNoOfTweets_TextChanged);
            // 
            // lblNoOfTweets
            // 
            this.lblNoOfTweets.AutoSize = true;
            this.lblNoOfTweets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblNoOfTweets.ForeColor = System.Drawing.Color.Black;
            this.lblNoOfTweets.Location = new System.Drawing.Point(6, 21);
            this.lblNoOfTweets.Name = "lblNoOfTweets";
            this.lblNoOfTweets.Size = new System.Drawing.Size(76, 13);
            this.lblNoOfTweets.TabIndex = 76;
            this.lblNoOfTweets.Text = "No. Of Tweets";
            // 
            // tweetusercontrols
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grp_tweetsettings);
            this.Name = "tweetusercontrols";
            this.Size = new System.Drawing.Size(776, 159);
            this.Load += new System.EventHandler(this.tweeusercontrols_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.tweetusercontrols_Paint);
            this.grp_tweetsettings.ResumeLayout(false);
            this.grp_tweetsettings.PerformLayout();
            this.grp_TweetImageFile.ResumeLayout(false);
            this.grp_TweetImageFile.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Grp_TweetParDay.ResumeLayout(false);
            this.Grp_TweetParDay.PerformLayout();
            this.grp_NoOfTweetParAc.ResumeLayout(false);
            this.grp_NoOfTweetParAc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_tweetsettings;
        private System.Windows.Forms.CheckBox chkAllTweetsPerAccount;
        private System.Windows.Forms.CheckBox chkbosUseHashTags;
        private System.Windows.Forms.GroupBox Grp_TweetParDay;
        private System.Windows.Forms.CheckBox ChkboxTweetPerday;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.TextBox txtMaximumTweet;
        private System.Windows.Forms.GroupBox grp_NoOfTweetParAc;
        private System.Windows.Forms.TextBox txtNoOfTweets;
        private System.Windows.Forms.Label lblNoOfTweets;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnUploadTweetMessage;
        private System.Windows.Forms.TextBox txt_CmpTweetMessageFile;
        private System.Windows.Forms.CheckBox chkboxKeepSingleMessage;
        private System.Windows.Forms.CheckBox chk_TweetWithImage;
        private System.Windows.Forms.GroupBox grp_TweetImageFile;
        private System.Windows.Forms.TextBox txt_TweetImageFilePath;
        private System.Windows.Forms.Button btn_BrowseTweetImage;
        private System.Windows.Forms.Button btnGetHashTagsCampaign;
        private System.Windows.Forms.TextBox txt_CmpTweetMentionUserList;
        private System.Windows.Forms.Button btnUploadTweetMentionUserList;
        private System.Windows.Forms.CheckBox chkCampTweetMentionUser;
        private System.Windows.Forms.CheckBox chkTweetMentionUserScrapedList;
    }
}
