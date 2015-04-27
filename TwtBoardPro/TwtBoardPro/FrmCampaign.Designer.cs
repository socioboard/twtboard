namespace twtboardpro
{
    partial class FrmCampaign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCampaign));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCampaignSetData = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.txtNewCampaignName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmboxCampiagnName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnViewCampaign = new System.Windows.Forms.Button();
            this.grpBoxCampaignDetaills = new System.Windows.Forms.GroupBox();
            this.btnKeywordSearch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtKeywordList = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btn_FollowKeyWordsFileUpload = new System.Windows.Forms.Button();
            this.txtUsernameList = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpBoxCampaignDetaills.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.splitContainer1.Location = new System.Drawing.Point(0, 1);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel1.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.Controls.Add(this.btnViewCampaign);
            this.splitContainer1.Panel2.Controls.Add(this.grpBoxCampaignDetaills);
            this.splitContainer1.Size = new System.Drawing.Size(726, 463);
            this.splitContainer1.SplitterDistance = 198;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCampaignSetData);
            this.groupBox1.Controls.Add(this.btnEdit);
            this.groupBox1.Controls.Add(this.txtNewCampaignName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmboxCampiagnName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 14);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 130);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create & Select Campaign";
            // 
            // btnCampaignSetData
            // 
            this.btnCampaignSetData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCampaignSetData.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCampaignSetData.Location = new System.Drawing.Point(480, 74);
            this.btnCampaignSetData.Name = "btnCampaignSetData";
            this.btnCampaignSetData.Size = new System.Drawing.Size(172, 42);
            this.btnCampaignSetData.TabIndex = 5;
            this.btnCampaignSetData.UseVisualStyleBackColor = true;
            this.btnCampaignSetData.Click += new System.EventHandler(this.btnCampaignSetData_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(480, 15);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(172, 43);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // txtNewCampaignName
            // 
            this.txtNewCampaignName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNewCampaignName.Location = new System.Drawing.Point(158, 85);
            this.txtNewCampaignName.Name = "txtNewCampaignName";
            this.txtNewCampaignName.Size = new System.Drawing.Size(290, 21);
            this.txtNewCampaignName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(22, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Create Campaign";
            // 
            // cmboxCampiagnName
            // 
            this.cmboxCampiagnName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboxCampiagnName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboxCampiagnName.FormattingEnabled = true;
            this.cmboxCampiagnName.Items.AddRange(new object[] {
            "Select Campaign Name"});
            this.cmboxCampiagnName.Location = new System.Drawing.Point(158, 28);
            this.cmboxCampiagnName.Name = "cmboxCampiagnName";
            this.cmboxCampiagnName.Size = new System.Drawing.Size(290, 21);
            this.cmboxCampiagnName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Campaign";
            // 
            // btnViewCampaign
            // 
            this.btnViewCampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnViewCampaign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewCampaign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewCampaign.Location = new System.Drawing.Point(299, 205);
            this.btnViewCampaign.Name = "btnViewCampaign";
            this.btnViewCampaign.Size = new System.Drawing.Size(136, 45);
            this.btnViewCampaign.TabIndex = 6;
            this.btnViewCampaign.UseVisualStyleBackColor = true;
            this.btnViewCampaign.Click += new System.EventHandler(this.btnViewCampaign_Click);
            // 
            // grpBoxCampaignDetaills
            // 
            this.grpBoxCampaignDetaills.Controls.Add(this.btnKeywordSearch);
            this.grpBoxCampaignDetaills.Controls.Add(this.label5);
            this.grpBoxCampaignDetaills.Controls.Add(this.txtKeywordList);
            this.grpBoxCampaignDetaills.Controls.Add(this.label4);
            this.grpBoxCampaignDetaills.Controls.Add(this.btnSubmit);
            this.grpBoxCampaignDetaills.Controls.Add(this.btn_FollowKeyWordsFileUpload);
            this.grpBoxCampaignDetaills.Controls.Add(this.txtUsernameList);
            this.grpBoxCampaignDetaills.Controls.Add(this.label3);
            this.grpBoxCampaignDetaills.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.grpBoxCampaignDetaills.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxCampaignDetaills.Location = new System.Drawing.Point(12, 3);
            this.grpBoxCampaignDetaills.Name = "grpBoxCampaignDetaills";
            this.grpBoxCampaignDetaills.Size = new System.Drawing.Size(706, 192);
            this.grpBoxCampaignDetaills.TabIndex = 0;
            this.grpBoxCampaignDetaills.TabStop = false;
            this.grpBoxCampaignDetaills.Text = "Campaign Details";
            // 
            // btnKeywordSearch
            // 
            this.btnKeywordSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnKeywordSearch.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnKeywordSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnKeywordSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeywordSearch.Location = new System.Drawing.Point(563, 99);
            this.btnKeywordSearch.Name = "btnKeywordSearch";
            this.btnKeywordSearch.Size = new System.Drawing.Size(83, 27);
            this.btnKeywordSearch.TabIndex = 11;
            this.btnKeywordSearch.UseVisualStyleBackColor = false;
            this.btnKeywordSearch.UseWaitCursor = true;
            this.btnKeywordSearch.Click += new System.EventHandler(this.btnKeywordSearch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(327, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "OR";
            this.label5.UseWaitCursor = true;
            // 
            // txtKeywordList
            // 
            this.txtKeywordList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeywordList.Location = new System.Drawing.Point(158, 102);
            this.txtKeywordList.Name = "txtKeywordList";
            this.txtKeywordList.Size = new System.Drawing.Size(364, 21);
            this.txtKeywordList.TabIndex = 9;
            this.txtKeywordList.UseWaitCursor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Keyword Search";
            this.label4.UseWaitCursor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSubmit.BackgroundImage")));
            this.btnSubmit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Location = new System.Drawing.Point(315, 140);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(80, 28);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_FollowKeyWordsFileUpload
            // 
            this.btn_FollowKeyWordsFileUpload.BackColor = System.Drawing.Color.Transparent;
            this.btn_FollowKeyWordsFileUpload.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btn_FollowKeyWordsFileUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_FollowKeyWordsFileUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FollowKeyWordsFileUpload.Location = new System.Drawing.Point(563, 37);
            this.btn_FollowKeyWordsFileUpload.Name = "btn_FollowKeyWordsFileUpload";
            this.btn_FollowKeyWordsFileUpload.Size = new System.Drawing.Size(83, 27);
            this.btn_FollowKeyWordsFileUpload.TabIndex = 4;
            this.btn_FollowKeyWordsFileUpload.UseVisualStyleBackColor = false;
            this.btn_FollowKeyWordsFileUpload.Click += new System.EventHandler(this.btn_FollowKeyWordsFileUpload_Click);
            // 
            // txtUsernameList
            // 
            this.txtUsernameList.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsernameList.Location = new System.Drawing.Point(158, 40);
            this.txtUsernameList.Name = "txtUsernameList";
            this.txtUsernameList.Size = new System.Drawing.Size(364, 21);
            this.txtUsernameList.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Username";
            // 
            // FrmCampaign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(731, 466);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tw Cen MT", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmCampaign";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Campaign";
            this.Load += new System.EventHandler(this.FrmCampaign_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpBoxCampaignDetaills.ResumeLayout(false);
            this.grpBoxCampaignDetaills.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCampaignSetData;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TextBox txtNewCampaignName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmboxCampiagnName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpBoxCampaignDetaills;
        private System.Windows.Forms.TextBox txtUsernameList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_FollowKeyWordsFileUpload;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnViewCampaign;
        private System.Windows.Forms.Button btnKeywordSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtKeywordList;
        private System.Windows.Forms.Label label4;
    }
}