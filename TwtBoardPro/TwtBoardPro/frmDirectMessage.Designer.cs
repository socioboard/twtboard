namespace twtboardpro
{
    partial class frmDirectMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDirectMessage));
            this.label6 = new System.Windows.Forms.Label();
            this.txtDirectMessage = new System.Windows.Forms.TextBox();
            this.txtFollowerUserDirectMessaging = new System.Windows.Forms.TextBox();
            this.grpSendDirectMessage = new System.Windows.Forms.GroupBox();
            this.lstDirectMessageSend = new System.Windows.Forms.ListBox();
            this.grpfollowerfollowWithDelay = new System.Windows.Forms.GroupBox();
            this.chkUniqueMessage = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtDirectMessageThreads = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtNoOfUsers = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.txtDirectMessageMinDelay = new System.Windows.Forms.TextBox();
            this.txtDirectMessageMaxDelay = new System.Windows.Forms.TextBox();
            this.grpUserInfo = new System.Windows.Forms.GroupBox();
            this.chkFollowers = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSubmitAction = new System.Windows.Forms.GroupBox();
            this.btnStartMessaging = new System.Windows.Forms.Button();
            this.btnStop_DirectMessageThreads = new System.Windows.Forms.Button();
            this.btnUploadFollowerUserDirectMessage = new System.Windows.Forms.Button();
            this.btnUploadDirectMessage = new System.Windows.Forms.Button();
            this.grpSendDirectMessage.SuspendLayout();
            this.grpfollowerfollowWithDelay.SuspendLayout();
            this.grpUserInfo.SuspendLayout();
            this.grpSubmitAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(-174, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 53;
            this.label6.Text = "Tweet Message File:";
            // 
            // txtDirectMessage
            // 
            this.txtDirectMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDirectMessage.ForeColor = System.Drawing.Color.Black;
            this.txtDirectMessage.Location = new System.Drawing.Point(191, 38);
            this.txtDirectMessage.Name = "txtDirectMessage";
            this.txtDirectMessage.ReadOnly = true;
            this.txtDirectMessage.Size = new System.Drawing.Size(466, 21);
            this.txtDirectMessage.TabIndex = 54;
            // 
            // txtFollowerUserDirectMessaging
            // 
            this.txtFollowerUserDirectMessaging.Enabled = false;
            this.txtFollowerUserDirectMessaging.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFollowerUserDirectMessaging.ForeColor = System.Drawing.Color.Black;
            this.txtFollowerUserDirectMessaging.Location = new System.Drawing.Point(191, 82);
            this.txtFollowerUserDirectMessaging.Name = "txtFollowerUserDirectMessaging";
            this.txtFollowerUserDirectMessaging.ReadOnly = true;
            this.txtFollowerUserDirectMessaging.Size = new System.Drawing.Size(466, 21);
            this.txtFollowerUserDirectMessaging.TabIndex = 56;
            // 
            // grpSendDirectMessage
            // 
            this.grpSendDirectMessage.BackColor = System.Drawing.Color.Transparent;
            this.grpSendDirectMessage.Controls.Add(this.lstDirectMessageSend);
            this.grpSendDirectMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSendDirectMessage.Location = new System.Drawing.Point(36, 291);
            this.grpSendDirectMessage.Name = "grpSendDirectMessage";
            this.grpSendDirectMessage.Size = new System.Drawing.Size(812, 131);
            this.grpSendDirectMessage.TabIndex = 79;
            this.grpSendDirectMessage.TabStop = false;
            this.grpSendDirectMessage.Text = "Logger";
            // 
            // lstDirectMessageSend
            // 
            this.lstDirectMessageSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDirectMessageSend.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstDirectMessageSend.FormattingEnabled = true;
            this.lstDirectMessageSend.HorizontalScrollbar = true;
            this.lstDirectMessageSend.Location = new System.Drawing.Point(3, 17);
            this.lstDirectMessageSend.Name = "lstDirectMessageSend";
            this.lstDirectMessageSend.ScrollAlwaysVisible = true;
            this.lstDirectMessageSend.Size = new System.Drawing.Size(806, 111);
            this.lstDirectMessageSend.TabIndex = 18;
            // 
            // grpfollowerfollowWithDelay
            // 
            this.grpfollowerfollowWithDelay.BackColor = System.Drawing.Color.Transparent;
            this.grpfollowerfollowWithDelay.Controls.Add(this.chkUniqueMessage);
            this.grpfollowerfollowWithDelay.Controls.Add(this.label20);
            this.grpfollowerfollowWithDelay.Controls.Add(this.txtDirectMessageThreads);
            this.grpfollowerfollowWithDelay.Controls.Add(this.label32);
            this.grpfollowerfollowWithDelay.Controls.Add(this.txtNoOfUsers);
            this.grpfollowerfollowWithDelay.Controls.Add(this.label34);
            this.grpfollowerfollowWithDelay.Controls.Add(this.label18);
            this.grpfollowerfollowWithDelay.Controls.Add(this.label33);
            this.grpfollowerfollowWithDelay.Controls.Add(this.txtDirectMessageMinDelay);
            this.grpfollowerfollowWithDelay.Controls.Add(this.txtDirectMessageMaxDelay);
            this.grpfollowerfollowWithDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpfollowerfollowWithDelay.ForeColor = System.Drawing.Color.Black;
            this.grpfollowerfollowWithDelay.Location = new System.Drawing.Point(36, 122);
            this.grpfollowerfollowWithDelay.Name = "grpfollowerfollowWithDelay";
            this.grpfollowerfollowWithDelay.Size = new System.Drawing.Size(812, 84);
            this.grpfollowerfollowWithDelay.TabIndex = 89;
            this.grpfollowerfollowWithDelay.TabStop = false;
            this.grpfollowerfollowWithDelay.Text = "Settings";
            // 
            // chkUniqueMessage
            // 
            this.chkUniqueMessage.AutoSize = true;
            this.chkUniqueMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUniqueMessage.Location = new System.Drawing.Point(572, 54);
            this.chkUniqueMessage.Name = "chkUniqueMessage";
            this.chkUniqueMessage.Size = new System.Drawing.Size(118, 17);
            this.chkUniqueMessage.TabIndex = 59;
            this.chkUniqueMessage.Text = "Unique Message";
            this.chkUniqueMessage.UseVisualStyleBackColor = true;
            this.chkUniqueMessage.CheckedChanged += new System.EventHandler(this.chkUniqueMessage_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.Black;
            this.label20.Location = new System.Drawing.Point(511, 20);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(93, 13);
            this.label20.TabIndex = 58;
            this.label20.Text = "No. Of Threads";
            // 
            // txtDirectMessageThreads
            // 
            this.txtDirectMessageThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDirectMessageThreads.Location = new System.Drawing.Point(619, 17);
            this.txtDirectMessageThreads.Name = "txtDirectMessageThreads";
            this.txtDirectMessageThreads.Size = new System.Drawing.Size(49, 21);
            this.txtDirectMessageThreads.TabIndex = 57;
            this.txtDirectMessageThreads.Text = "7";
            this.txtDirectMessageThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(106, 53);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(44, 13);
            this.label32.TabIndex = 52;
            this.label32.Text = "Delay ";
            // 
            // txtNoOfUsers
            // 
            this.txtNoOfUsers.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfUsers.ForeColor = System.Drawing.Color.Black;
            this.txtNoOfUsers.Location = new System.Drawing.Point(309, 18);
            this.txtNoOfUsers.Name = "txtNoOfUsers";
            this.txtNoOfUsers.Size = new System.Drawing.Size(49, 21);
            this.txtNoOfUsers.TabIndex = 49;
            this.txtNoOfUsers.Text = "5";
            this.txtNoOfUsers.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.Location = new System.Drawing.Point(461, 57);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(81, 13);
            this.label34.TabIndex = 56;
            this.label34.Text = "(In Seconds)";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(106, 21);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(98, 13);
            this.label18.TabIndex = 50;
            this.label18.Text = "No. Of followers";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(369, 57);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(18, 13);
            this.label33.TabIndex = 55;
            this.label33.Text = "to";
            // 
            // txtDirectMessageMinDelay
            // 
            this.txtDirectMessageMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDirectMessageMinDelay.ForeColor = System.Drawing.Color.Black;
            this.txtDirectMessageMinDelay.Location = new System.Drawing.Point(309, 53);
            this.txtDirectMessageMinDelay.Name = "txtDirectMessageMinDelay";
            this.txtDirectMessageMinDelay.Size = new System.Drawing.Size(49, 21);
            this.txtDirectMessageMinDelay.TabIndex = 53;
            this.txtDirectMessageMinDelay.Text = "5";
            this.txtDirectMessageMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDirectMessageMaxDelay
            // 
            this.txtDirectMessageMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDirectMessageMaxDelay.ForeColor = System.Drawing.Color.Black;
            this.txtDirectMessageMaxDelay.Location = new System.Drawing.Point(397, 54);
            this.txtDirectMessageMaxDelay.Name = "txtDirectMessageMaxDelay";
            this.txtDirectMessageMaxDelay.Size = new System.Drawing.Size(49, 21);
            this.txtDirectMessageMaxDelay.TabIndex = 54;
            this.txtDirectMessageMaxDelay.Text = "8";
            this.txtDirectMessageMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grpUserInfo
            // 
            this.grpUserInfo.BackColor = System.Drawing.Color.Transparent;
            this.grpUserInfo.Controls.Add(this.chkFollowers);
            this.grpUserInfo.Controls.Add(this.label1);
            this.grpUserInfo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpUserInfo.Location = new System.Drawing.Point(36, 12);
            this.grpUserInfo.Name = "grpUserInfo";
            this.grpUserInfo.Size = new System.Drawing.Size(812, 100);
            this.grpUserInfo.TabIndex = 90;
            this.grpUserInfo.TabStop = false;
            this.grpUserInfo.Text = "User or Msg Info";
            // 
            // chkFollowers
            // 
            this.chkFollowers.AutoSize = true;
            this.chkFollowers.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.chkFollowers.Location = new System.Drawing.Point(19, 73);
            this.chkFollowers.Name = "chkFollowers";
            this.chkFollowers.Size = new System.Drawing.Size(79, 17);
            this.chkFollowers.TabIndex = 53;
            this.chkFollowers.Text = "Followers";
            this.chkFollowers.UseVisualStyleBackColor = true;
            this.chkFollowers.CheckedChanged += new System.EventHandler(this.chkFollowers_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(31, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 51;
            this.label1.Text = "Message";
            // 
            // grpSubmitAction
            // 
            this.grpSubmitAction.BackColor = System.Drawing.Color.Transparent;
            this.grpSubmitAction.Controls.Add(this.btnStartMessaging);
            this.grpSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpSubmitAction.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.grpSubmitAction.Location = new System.Drawing.Point(38, 222);
            this.grpSubmitAction.Name = "grpSubmitAction";
            this.grpSubmitAction.Size = new System.Drawing.Size(810, 65);
            this.grpSubmitAction.TabIndex = 93;
            this.grpSubmitAction.TabStop = false;
            this.grpSubmitAction.Text = "Submit Action";
            // 
            // btnStartMessaging
            // 
            this.btnStartMessaging.BackColor = System.Drawing.Color.Transparent;
            this.btnStartMessaging.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStartMessaging.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartMessaging.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartMessaging.ForeColor = System.Drawing.Color.Black;
            this.btnStartMessaging.Location = new System.Drawing.Point(222, 23);
            this.btnStartMessaging.Name = "btnStartMessaging";
            this.btnStartMessaging.Size = new System.Drawing.Size(92, 32);
            this.btnStartMessaging.TabIndex = 52;
            this.btnStartMessaging.UseVisualStyleBackColor = false;
            this.btnStartMessaging.Click += new System.EventHandler(this.btnStartMessaging_Click);
            // 
            // btnStop_DirectMessageThreads
            // 
            this.btnStop_DirectMessageThreads.BackColor = System.Drawing.Color.Transparent;
            this.btnStop_DirectMessageThreads.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btnStop_DirectMessageThreads.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStop_DirectMessageThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop_DirectMessageThreads.ForeColor = System.Drawing.Color.Black;
            this.btnStop_DirectMessageThreads.Location = new System.Drawing.Point(368, 245);
            this.btnStop_DirectMessageThreads.Name = "btnStop_DirectMessageThreads";
            this.btnStop_DirectMessageThreads.Size = new System.Drawing.Size(84, 32);
            this.btnStop_DirectMessageThreads.TabIndex = 91;
            this.btnStop_DirectMessageThreads.UseVisualStyleBackColor = false;
            this.btnStop_DirectMessageThreads.Click += new System.EventHandler(this.btnStop_DirectMessageThreads_Click);
            // 
            // btnUploadFollowerUserDirectMessage
            // 
            this.btnUploadFollowerUserDirectMessage.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadFollowerUserDirectMessage.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnUploadFollowerUserDirectMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadFollowerUserDirectMessage.Enabled = false;
            this.btnUploadFollowerUserDirectMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadFollowerUserDirectMessage.ForeColor = System.Drawing.Color.Black;
            this.btnUploadFollowerUserDirectMessage.Location = new System.Drawing.Point(690, 79);
            this.btnUploadFollowerUserDirectMessage.Name = "btnUploadFollowerUserDirectMessage";
            this.btnUploadFollowerUserDirectMessage.Size = new System.Drawing.Size(80, 27);
            this.btnUploadFollowerUserDirectMessage.TabIndex = 57;
            this.btnUploadFollowerUserDirectMessage.UseVisualStyleBackColor = false;
            this.btnUploadFollowerUserDirectMessage.Click += new System.EventHandler(this.btnUploadFollowerUserDirectMessage_Click);
            // 
            // btnUploadDirectMessage
            // 
            this.btnUploadDirectMessage.BackColor = System.Drawing.Color.Transparent;
            this.btnUploadDirectMessage.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnUploadDirectMessage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadDirectMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadDirectMessage.ForeColor = System.Drawing.Color.Black;
            this.btnUploadDirectMessage.Location = new System.Drawing.Point(689, 34);
            this.btnUploadDirectMessage.Name = "btnUploadDirectMessage";
            this.btnUploadDirectMessage.Size = new System.Drawing.Size(80, 27);
            this.btnUploadDirectMessage.TabIndex = 55;
            this.btnUploadDirectMessage.UseVisualStyleBackColor = false;
            this.btnUploadDirectMessage.Click += new System.EventHandler(this.btnUploadDirectMessage_Click);
            // 
            // frmDirectMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 431);
            this.Controls.Add(this.btnStop_DirectMessageThreads);
            this.Controls.Add(this.grpSubmitAction);
            this.Controls.Add(this.grpfollowerfollowWithDelay);
            this.Controls.Add(this.grpSendDirectMessage);
            this.Controls.Add(this.btnUploadFollowerUserDirectMessage);
            this.Controls.Add(this.txtFollowerUserDirectMessaging);
            this.Controls.Add(this.btnUploadDirectMessage);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDirectMessage);
            this.Controls.Add(this.grpUserInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmDirectMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Direct Message";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDirectMessage_FormClosed);
            this.Load += new System.EventHandler(this.frmDirectMessage_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmDirectMessage_Paint);
            this.grpSendDirectMessage.ResumeLayout(false);
            this.grpfollowerfollowWithDelay.ResumeLayout(false);
            this.grpfollowerfollowWithDelay.PerformLayout();
            this.grpUserInfo.ResumeLayout(false);
            this.grpUserInfo.PerformLayout();
            this.grpSubmitAction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartMessaging;
        private System.Windows.Forms.Button btnUploadDirectMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDirectMessage;
        private System.Windows.Forms.TextBox txtFollowerUserDirectMessaging;
        private System.Windows.Forms.Button btnUploadFollowerUserDirectMessage;
        private System.Windows.Forms.GroupBox grpSendDirectMessage;
        private System.Windows.Forms.ListBox lstDirectMessageSend;
        private System.Windows.Forms.GroupBox grpfollowerfollowWithDelay;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtDirectMessageThreads;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtNoOfUsers;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtDirectMessageMinDelay;
        private System.Windows.Forms.TextBox txtDirectMessageMaxDelay;
        private System.Windows.Forms.GroupBox grpUserInfo;
        private System.Windows.Forms.Button btnStop_DirectMessageThreads;
        private System.Windows.Forms.GroupBox grpSubmitAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkFollowers;
        private System.Windows.Forms.CheckBox chkUniqueMessage;
    }
}