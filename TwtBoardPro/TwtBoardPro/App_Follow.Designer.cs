namespace twtboardpro
{
    partial class App_Follow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App_Follow));
            this.grpScrapProfileImage = new System.Windows.Forms.GroupBox();
            this.chk_UseGroup = new System.Windows.Forms.CheckBox();
            this.txt_InsertGroup = new System.Windows.Forms.TextBox();
            this.chkAppFollowAdd_PublicIP = new System.Windows.Forms.CheckBox();
            this.btn_App_FollowAccountCreation = new System.Windows.Forms.Button();
            this.btn_App_FollowAccountCreatorStop = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstProcessLoggerForAccounts = new System.Windows.Forms.ListBox();
            this.timerAppsWebManager = new System.Windows.Forms.Timer(this.components);
            this.grpScrapProfileImage.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpScrapProfileImage
            // 
            this.grpScrapProfileImage.BackColor = System.Drawing.Color.Transparent;
            this.grpScrapProfileImage.Controls.Add(this.chk_UseGroup);
            this.grpScrapProfileImage.Controls.Add(this.txt_InsertGroup);
            this.grpScrapProfileImage.Controls.Add(this.chkAppFollowAdd_PublicIP);
            this.grpScrapProfileImage.Controls.Add(this.btn_App_FollowAccountCreation);
            this.grpScrapProfileImage.Controls.Add(this.btn_App_FollowAccountCreatorStop);
            this.grpScrapProfileImage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpScrapProfileImage.Location = new System.Drawing.Point(2, 12);
            this.grpScrapProfileImage.Name = "grpScrapProfileImage";
            this.grpScrapProfileImage.Size = new System.Drawing.Size(828, 91);
            this.grpScrapProfileImage.TabIndex = 65;
            this.grpScrapProfileImage.TabStop = false;
            this.grpScrapProfileImage.Text = "Input";
            // 
            // chk_UseGroup
            // 
            this.chk_UseGroup.AutoSize = true;
            this.chk_UseGroup.Location = new System.Drawing.Point(521, 47);
            this.chk_UseGroup.Name = "chk_UseGroup";
            this.chk_UseGroup.Size = new System.Drawing.Size(86, 17);
            this.chk_UseGroup.TabIndex = 84;
            this.chk_UseGroup.Text = "Use Group";
            this.chk_UseGroup.UseVisualStyleBackColor = true;
            this.chk_UseGroup.Visible = false;
            // 
            // txt_InsertGroup
            // 
            this.txt_InsertGroup.Location = new System.Drawing.Point(625, 45);
            this.txt_InsertGroup.Name = "txt_InsertGroup";
            this.txt_InsertGroup.Size = new System.Drawing.Size(156, 21);
            this.txt_InsertGroup.TabIndex = 83;
            this.txt_InsertGroup.Visible = false;
            // 
            // chkAppFollowAdd_PublicIP
            // 
            this.chkAppFollowAdd_PublicIP.AutoSize = true;
            this.chkAppFollowAdd_PublicIP.Location = new System.Drawing.Point(78, 44);
            this.chkAppFollowAdd_PublicIP.Name = "chkAppFollowAdd_PublicIP";
            this.chkAppFollowAdd_PublicIP.Size = new System.Drawing.Size(126, 17);
            this.chkAppFollowAdd_PublicIP.TabIndex = 81;
            this.chkAppFollowAdd_PublicIP.Text = "ADD Public IP";
            this.chkAppFollowAdd_PublicIP.UseVisualStyleBackColor = true;
            this.chkAppFollowAdd_PublicIP.Visible = false;
            // 
            // btn_App_FollowAccountCreation
            // 
            this.btn_App_FollowAccountCreation.BackColor = System.Drawing.Color.Transparent;
            this.btn_App_FollowAccountCreation.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btn_App_FollowAccountCreation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_App_FollowAccountCreation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_App_FollowAccountCreation.ForeColor = System.Drawing.Color.Black;
            this.btn_App_FollowAccountCreation.Location = new System.Drawing.Point(270, 39);
            this.btn_App_FollowAccountCreation.Name = "btn_App_FollowAccountCreation";
            this.btn_App_FollowAccountCreation.Size = new System.Drawing.Size(88, 27);
            this.btn_App_FollowAccountCreation.TabIndex = 80;
            this.btn_App_FollowAccountCreation.UseVisualStyleBackColor = false;
            this.btn_App_FollowAccountCreation.Click += new System.EventHandler(this.btn_App_FollowAccountCreation_Click);
            // 
            // btn_App_FollowAccountCreatorStop
            // 
            this.btn_App_FollowAccountCreatorStop.BackColor = System.Drawing.Color.Transparent;
            this.btn_App_FollowAccountCreatorStop.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btn_App_FollowAccountCreatorStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_App_FollowAccountCreatorStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_App_FollowAccountCreatorStop.ForeColor = System.Drawing.Color.Black;
            this.btn_App_FollowAccountCreatorStop.Location = new System.Drawing.Point(395, 38);
            this.btn_App_FollowAccountCreatorStop.Name = "btn_App_FollowAccountCreatorStop";
            this.btn_App_FollowAccountCreatorStop.Size = new System.Drawing.Size(82, 27);
            this.btn_App_FollowAccountCreatorStop.TabIndex = 78;
            this.btn_App_FollowAccountCreatorStop.UseVisualStyleBackColor = false;
            this.btn_App_FollowAccountCreatorStop.Click += new System.EventHandler(this.btn_App_FollowAccountCreatorStop_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lstProcessLoggerForAccounts);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(2, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(822, 145);
            this.groupBox2.TabIndex = 67;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process Logger";
            // 
            // lstProcessLoggerForAccounts
            // 
            this.lstProcessLoggerForAccounts.FormattingEnabled = true;
            this.lstProcessLoggerForAccounts.Location = new System.Drawing.Point(6, 19);
            this.lstProcessLoggerForAccounts.Name = "lstProcessLoggerForAccounts";
            this.lstProcessLoggerForAccounts.Size = new System.Drawing.Size(800, 108);
            this.lstProcessLoggerForAccounts.TabIndex = 0;
            // 
            // timerAppsWebManager
            // 
            this.timerAppsWebManager.Interval = 3000;
            // 
            // App_Follow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 302);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpScrapProfileImage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "App_Follow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arab Follow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.App_Follow_FormClosed);
            this.Load += new System.EventHandler(this.App_Follow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.App_Follow_Paint);
            this.grpScrapProfileImage.ResumeLayout(false);
            this.grpScrapProfileImage.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpScrapProfileImage;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstProcessLoggerForAccounts;
        private System.Windows.Forms.Button btn_App_FollowAccountCreatorStop;
        private System.Windows.Forms.Button btn_App_FollowAccountCreation;
        private System.Windows.Forms.CheckBox chkAppFollowAdd_PublicIP;
        private System.Windows.Forms.Timer timerAppsWebManager;
        private System.Windows.Forms.TextBox txt_InsertGroup;
        private System.Windows.Forms.CheckBox chk_UseGroup;

    }
}