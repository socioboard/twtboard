namespace twtboardpro
{
    partial class WebsitesLiker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebsitesLiker));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChkUseMessage = new System.Windows.Forms.CheckBox();
            this.txtLikeNoOfwebsite = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_BrowseMessages = new System.Windows.Forms.Button();
            this.txt_Message = new System.Windows.Forms.TextBox();
            this.e = new System.Windows.Forms.Label();
            this.btn_BrowseWebsites = new System.Windows.Forms.Button();
            this.txt_WebsitesLink = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_StartWebsitelike = new System.Windows.Forms.Button();
            this.btn_StopWebsiteLike = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LsWebsiteLikerLogger = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox1.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.groupBox1.Controls.Add(this.ChkUseMessage);
            this.groupBox1.Controls.Add(this.txtLikeNoOfwebsite);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btn_BrowseMessages);
            this.groupBox1.Controls.Add(this.txt_Message);
            this.groupBox1.Controls.Add(this.e);
            this.groupBox1.Controls.Add(this.btn_BrowseWebsites);
            this.groupBox1.Controls.Add(this.txt_WebsitesLink);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(775, 233);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Details";
            // 
            // ChkUseMessage
            // 
            this.ChkUseMessage.AutoSize = true;
            this.ChkUseMessage.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ChkUseMessage.Location = new System.Drawing.Point(314, 139);
            this.ChkUseMessage.Name = "ChkUseMessage";
            this.ChkUseMessage.Size = new System.Drawing.Size(91, 17);
            this.ChkUseMessage.TabIndex = 8;
            this.ChkUseMessage.Text = "Use Message";
            this.ChkUseMessage.UseVisualStyleBackColor = true;
            this.ChkUseMessage.CheckedChanged += new System.EventHandler(this.ChkUseMessage_CheckedChanged);
            // 
            // txtLikeNoOfwebsite
            // 
            this.txtLikeNoOfwebsite.Location = new System.Drawing.Point(168, 136);
            this.txtLikeNoOfwebsite.Name = "txtLikeNoOfwebsite";
            this.txtLikeNoOfwebsite.Size = new System.Drawing.Size(85, 20);
            this.txtLikeNoOfwebsite.TabIndex = 7;
            this.txtLikeNoOfwebsite.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(37, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Like No Of Website";
            this.label2.Visible = false;
            // 
            // btn_BrowseMessages
            // 
            this.btn_BrowseMessages.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btn_BrowseMessages.Enabled = false;
            this.btn_BrowseMessages.Location = new System.Drawing.Point(618, 82);
            this.btn_BrowseMessages.Name = "btn_BrowseMessages";
            this.btn_BrowseMessages.Size = new System.Drawing.Size(84, 28);
            this.btn_BrowseMessages.TabIndex = 5;
            this.btn_BrowseMessages.UseVisualStyleBackColor = true;
            this.btn_BrowseMessages.Click += new System.EventHandler(this.btn_BrowseMessages_Click);
            // 
            // txt_Message
            // 
            this.txt_Message.Location = new System.Drawing.Point(105, 87);
            this.txt_Message.Name = "txt_Message";
            this.txt_Message.Size = new System.Drawing.Size(448, 20);
            this.txt_Message.TabIndex = 4;
            // 
            // e
            // 
            this.e.AutoSize = true;
            this.e.BackColor = System.Drawing.Color.Transparent;
            this.e.Location = new System.Drawing.Point(37, 90);
            this.e.Name = "e";
            this.e.Size = new System.Drawing.Size(50, 13);
            this.e.TabIndex = 3;
            this.e.Text = "Message";
            // 
            // btn_BrowseWebsites
            // 
            this.btn_BrowseWebsites.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btn_BrowseWebsites.Location = new System.Drawing.Point(618, 46);
            this.btn_BrowseWebsites.Name = "btn_BrowseWebsites";
            this.btn_BrowseWebsites.Size = new System.Drawing.Size(84, 28);
            this.btn_BrowseWebsites.TabIndex = 2;
            this.btn_BrowseWebsites.UseVisualStyleBackColor = true;
            this.btn_BrowseWebsites.Click += new System.EventHandler(this.btn_BrowseWebsites_Click);
            // 
            // txt_WebsitesLink
            // 
            this.txt_WebsitesLink.Enabled = false;
            this.txt_WebsitesLink.Location = new System.Drawing.Point(105, 46);
            this.txt_WebsitesLink.Name = "txt_WebsitesLink";
            this.txt_WebsitesLink.Size = new System.Drawing.Size(448, 20);
            this.txt_WebsitesLink.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(37, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Websites";
            // 
            // groupBox2
            // 
            this.groupBox2.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.groupBox2.Controls.Add(this.btn_StartWebsitelike);
            this.groupBox2.Controls.Add(this.btn_StopWebsiteLike);
            this.groupBox2.Location = new System.Drawing.Point(12, 265);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(775, 56);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Submit Action";
            // 
            // btn_StartWebsitelike
            // 
            this.btn_StartWebsitelike.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btn_StartWebsitelike.Location = new System.Drawing.Point(284, 19);
            this.btn_StartWebsitelike.Name = "btn_StartWebsitelike";
            this.btn_StartWebsitelike.Size = new System.Drawing.Size(84, 31);
            this.btn_StartWebsitelike.TabIndex = 1;
            this.btn_StartWebsitelike.UseVisualStyleBackColor = true;
            this.btn_StartWebsitelike.Click += new System.EventHandler(this.btn_StartWebsitelike_Click);
            // 
            // btn_StopWebsiteLike
            // 
            this.btn_StopWebsiteLike.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btn_StopWebsiteLike.Location = new System.Drawing.Point(403, 19);
            this.btn_StopWebsiteLike.Name = "btn_StopWebsiteLike";
            this.btn_StopWebsiteLike.Size = new System.Drawing.Size(84, 31);
            this.btn_StopWebsiteLike.TabIndex = 0;
            this.btn_StopWebsiteLike.UseVisualStyleBackColor = true;
            this.btn_StopWebsiteLike.Click += new System.EventHandler(this.btn_StopWebsiteLike_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.LsWebsiteLikerLogger);
            this.groupBox3.Location = new System.Drawing.Point(12, 340);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(781, 109);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "WebsiteLike Logger";
            // 
            // LsWebsiteLikerLogger
            // 
            this.LsWebsiteLikerLogger.FormattingEnabled = true;
            this.LsWebsiteLikerLogger.Location = new System.Drawing.Point(6, 18);
            this.LsWebsiteLikerLogger.Name = "LsWebsiteLikerLogger";
            this.LsWebsiteLikerLogger.Size = new System.Drawing.Size(769, 82);
            this.LsWebsiteLikerLogger.TabIndex = 0;
            // 
            // WebsitesLiker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(805, 461);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebsitesLiker";
            this.Text = "Website Liker";
            this.Load += new System.EventHandler(this.WebsitesLiker_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_WebsitesLink;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_BrowseMessages;
        private System.Windows.Forms.TextBox txt_Message;
        private System.Windows.Forms.Label e;
        private System.Windows.Forms.Button btn_BrowseWebsites;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_StartWebsitelike;
        private System.Windows.Forms.Button btn_StopWebsiteLike;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLikeNoOfwebsite;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox ChkUseMessage;
        private System.Windows.Forms.ListBox LsWebsiteLikerLogger;
    }
}