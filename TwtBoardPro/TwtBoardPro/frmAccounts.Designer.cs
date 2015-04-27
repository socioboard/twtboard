namespace twtboardpro
{
    partial class frmAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccounts));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBoxProfileDetails = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkListBoxExportAccount = new System.Windows.Forms.CheckedListBox();
            this.chkExportAccounts = new System.Windows.Forms.CheckBox();
            this.lblAcccountStatus = new System.Windows.Forms.Label();
            this.btnExportAccounts = new System.Windows.Forms.Button();
            this.PnlPicLodder = new System.Windows.Forms.Panel();
            this.PicLodder = new System.Windows.Forms.PictureBox();
            this.ButonClearProxies = new System.Windows.Forms.Button();
            this.btnAssignProxy = new System.Windows.Forms.Button();
            this.btnClearAccounts = new System.Windows.Forms.Button();
            this.btnLoadAccounts = new System.Windows.Forms.Button();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.gbAccountsDetail = new System.Windows.Forms.GroupBox();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.lblRequestThreads = new System.Windows.Forms.Label();
            this.txtAccountsPerProxy = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxProfileDetails.SuspendLayout();
            this.PnlPicLodder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicLodder)).BeginInit();
            this.gbAccountsDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBoxProfileDetails);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Panel2.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.splitContainer1.Panel2.Controls.Add(this.gbAccountsDetail);
            this.splitContainer1.Panel2.Controls.Add(this.lstLogger);
            this.splitContainer1.Panel2.Controls.Add(this.lblRequestThreads);
            this.splitContainer1.Panel2.Controls.Add(this.txtAccountsPerProxy);
            this.splitContainer1.Size = new System.Drawing.Size(992, 562);
            this.splitContainer1.SplitterDistance = 279;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBoxProfileDetails
            // 
            this.groupBoxProfileDetails.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxProfileDetails.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.groupBoxProfileDetails.Controls.Add(this.btnRefresh);
            this.groupBoxProfileDetails.Controls.Add(this.chkListBoxExportAccount);
            this.groupBoxProfileDetails.Controls.Add(this.chkExportAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.lblAcccountStatus);
            this.groupBoxProfileDetails.Controls.Add(this.btnExportAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.PnlPicLodder);
            this.groupBoxProfileDetails.Controls.Add(this.ButonClearProxies);
            this.groupBoxProfileDetails.Controls.Add(this.btnAssignProxy);
            this.groupBoxProfileDetails.Controls.Add(this.btnClearAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.btnLoadAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.textBox12);
            this.groupBoxProfileDetails.Controls.Add(this.label10);
            this.groupBoxProfileDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProfileDetails.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxProfileDetails.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProfileDetails.Name = "groupBoxProfileDetails";
            this.groupBoxProfileDetails.Size = new System.Drawing.Size(279, 562);
            this.groupBoxProfileDetails.TabIndex = 2;
            this.groupBoxProfileDetails.TabStop = false;
            this.groupBoxProfileDetails.Text = "Profile Details";
            this.groupBoxProfileDetails.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBoxProfileDetails_Paint);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(92, 253);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(132, 23);
            this.btnRefresh.TabIndex = 59;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkListBoxExportAccount
            // 
            this.chkListBoxExportAccount.Enabled = false;
            this.chkListBoxExportAccount.FormattingEnabled = true;
            this.chkListBoxExportAccount.Items.AddRange(new object[] {
            "Public Proxies Account",
            "Private Proxies Account",
            "Without Proxies Account"});
            this.chkListBoxExportAccount.Location = new System.Drawing.Point(76, 321);
            this.chkListBoxExportAccount.Name = "chkListBoxExportAccount";
            this.chkListBoxExportAccount.Size = new System.Drawing.Size(164, 52);
            this.chkListBoxExportAccount.TabIndex = 58;
            this.chkListBoxExportAccount.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkListBoxExportAccount_ItemCheck);
            // 
            // chkExportAccounts
            // 
            this.chkExportAccounts.AutoSize = true;
            this.chkExportAccounts.Location = new System.Drawing.Point(55, 299);
            this.chkExportAccounts.Name = "chkExportAccounts";
            this.chkExportAccounts.Size = new System.Drawing.Size(205, 17);
            this.chkExportAccounts.TabIndex = 57;
            this.chkExportAccounts.Text = "Export Accounts With Condition";
            this.chkExportAccounts.UseVisualStyleBackColor = true;
            this.chkExportAccounts.CheckedChanged += new System.EventHandler(this.chkExportAccounts_CheckedChanged);
            // 
            // lblAcccountStatus
            // 
            this.lblAcccountStatus.AutoSize = true;
            this.lblAcccountStatus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcccountStatus.Location = new System.Drawing.Point(52, 383);
            this.lblAcccountStatus.Name = "lblAcccountStatus";
            this.lblAcccountStatus.Size = new System.Drawing.Size(19, 13);
            this.lblAcccountStatus.TabIndex = 56;
            this.lblAcccountStatus.Text = "...";
            // 
            // btnExportAccounts
            // 
            this.btnExportAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnExportAccounts.BackgroundImage = global::twtboardpro.Properties.Resources.export_accounts;
            this.btnExportAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExportAccounts.Location = new System.Drawing.Point(92, 204);
            this.btnExportAccounts.Name = "btnExportAccounts";
            this.btnExportAccounts.Size = new System.Drawing.Size(132, 30);
            this.btnExportAccounts.TabIndex = 18;
            this.btnExportAccounts.UseVisualStyleBackColor = false;
            this.btnExportAccounts.Click += new System.EventHandler(this.btnExportAccounts_Click);
            // 
            // PnlPicLodder
            // 
            this.PnlPicLodder.BackgroundImage = global::twtboardpro.Properties.Resources.Loder;
            this.PnlPicLodder.Controls.Add(this.PicLodder);
            this.PnlPicLodder.Location = new System.Drawing.Point(92, 399);
            this.PnlPicLodder.Name = "PnlPicLodder";
            this.PnlPicLodder.Size = new System.Drawing.Size(127, 16);
            this.PnlPicLodder.TabIndex = 55;
            this.PnlPicLodder.Visible = false;
            // 
            // PicLodder
            // 
            this.PicLodder.Image = global::twtboardpro.Properties.Resources.Loder;
            this.PicLodder.Location = new System.Drawing.Point(0, 0);
            this.PicLodder.Name = "PicLodder";
            this.PicLodder.Size = new System.Drawing.Size(128, 16);
            this.PicLodder.TabIndex = 0;
            this.PicLodder.TabStop = false;
            this.PicLodder.Visible = false;
            // 
            // ButonClearProxies
            // 
            this.ButonClearProxies.BackColor = System.Drawing.Color.Transparent;
            this.ButonClearProxies.BackgroundImage = global::twtboardpro.Properties.Resources.clear_proxies;
            this.ButonClearProxies.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ButonClearProxies.Location = new System.Drawing.Point(92, 157);
            this.ButonClearProxies.Name = "ButonClearProxies";
            this.ButonClearProxies.Size = new System.Drawing.Size(132, 30);
            this.ButonClearProxies.TabIndex = 17;
            this.ButonClearProxies.UseVisualStyleBackColor = false;
            this.ButonClearProxies.Click += new System.EventHandler(this.ButonClearProxies_Click);
            // 
            // btnAssignProxy
            // 
            this.btnAssignProxy.BackColor = System.Drawing.Color.White;
            this.btnAssignProxy.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAssignProxy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAssignProxy.Location = new System.Drawing.Point(92, 430);
            this.btnAssignProxy.Name = "btnAssignProxy";
            this.btnAssignProxy.Size = new System.Drawing.Size(114, 27);
            this.btnAssignProxy.TabIndex = 16;
            this.btnAssignProxy.Text = "Assign Proxy";
            this.btnAssignProxy.UseVisualStyleBackColor = false;
            this.btnAssignProxy.Visible = false;
            this.btnAssignProxy.Click += new System.EventHandler(this.btnAssignProxy_Click);
            // 
            // btnClearAccounts
            // 
            this.btnClearAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnClearAccounts.BackgroundImage = global::twtboardpro.Properties.Resources.clear_accounts;
            this.btnClearAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClearAccounts.Location = new System.Drawing.Point(92, 113);
            this.btnClearAccounts.Name = "btnClearAccounts";
            this.btnClearAccounts.Size = new System.Drawing.Size(132, 30);
            this.btnClearAccounts.TabIndex = 15;
            this.btnClearAccounts.UseVisualStyleBackColor = false;
            this.btnClearAccounts.Click += new System.EventHandler(this.btnClearAccounts_Click);
            // 
            // btnLoadAccounts
            // 
            this.btnLoadAccounts.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadAccounts.BackgroundImage = global::twtboardpro.Properties.Resources.load_accouunts;
            this.btnLoadAccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadAccounts.Location = new System.Drawing.Point(92, 70);
            this.btnLoadAccounts.Name = "btnLoadAccounts";
            this.btnLoadAccounts.Size = new System.Drawing.Size(132, 30);
            this.btnLoadAccounts.TabIndex = 9;
            this.btnLoadAccounts.UseVisualStyleBackColor = false;
            this.btnLoadAccounts.Click += new System.EventHandler(this.btnLoadAccounts_Click);
            // 
            // textBox12
            // 
            this.textBox12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.Location = new System.Drawing.Point(92, 30);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(147, 21);
            this.textBox12.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(6, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Accounts File:";
            // 
            // gbAccountsDetail
            // 
            this.gbAccountsDetail.Controls.Add(this.dgvAccounts);
            this.gbAccountsDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbAccountsDetail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAccountsDetail.Location = new System.Drawing.Point(0, 0);
            this.gbAccountsDetail.Name = "gbAccountsDetail";
            this.gbAccountsDetail.Size = new System.Drawing.Size(709, 415);
            this.gbAccountsDetail.TabIndex = 56;
            this.gbAccountsDetail.TabStop = false;
            this.gbAccountsDetail.Text = "Accounts Details";
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccounts.Location = new System.Drawing.Point(3, 17);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.ReadOnly = true;
            this.dgvAccounts.Size = new System.Drawing.Size(703, 395);
            this.dgvAccounts.TabIndex = 57;
            this.dgvAccounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccounts_CellContentClick);
            // 
            // lstLogger
            // 
            this.lstLogger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.Location = new System.Drawing.Point(0, 415);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(709, 147);
            this.lstLogger.TabIndex = 54;
            // 
            // lblRequestThreads
            // 
            this.lblRequestThreads.AutoSize = true;
            this.lblRequestThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestThreads.Location = new System.Drawing.Point(7, 379);
            this.lblRequestThreads.Name = "lblRequestThreads";
            this.lblRequestThreads.Size = new System.Drawing.Size(162, 13);
            this.lblRequestThreads.TabIndex = 53;
            this.lblRequestThreads.Text = "No. Of accounts per proxy:";
            this.lblRequestThreads.Visible = false;
            // 
            // txtAccountsPerProxy
            // 
            this.txtAccountsPerProxy.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountsPerProxy.Location = new System.Drawing.Point(173, 375);
            this.txtAccountsPerProxy.Name = "txtAccountsPerProxy";
            this.txtAccountsPerProxy.Size = new System.Drawing.Size(45, 21);
            this.txtAccountsPerProxy.TabIndex = 17;
            this.txtAccountsPerProxy.Visible = false;
            // 
            // frmAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(992, 562);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accounts";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAccounts_FormClosed);
            this.Load += new System.EventHandler(this.frmAccounts_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxProfileDetails.ResumeLayout(false);
            this.groupBoxProfileDetails.PerformLayout();
            this.PnlPicLodder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicLodder)).EndInit();
            this.gbAccountsDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtAccountsPerProxy;
        private System.Windows.Forms.Label lblRequestThreads;
        private System.Windows.Forms.ListBox lstLogger;
        private System.Windows.Forms.Panel PnlPicLodder;
        private System.Windows.Forms.PictureBox PicLodder;
        private System.Windows.Forms.GroupBox groupBoxProfileDetails;
        private System.Windows.Forms.Button btnExportAccounts;
        private System.Windows.Forms.Button ButonClearProxies;
        private System.Windows.Forms.Button btnAssignProxy;
        private System.Windows.Forms.Button btnClearAccounts;
        private System.Windows.Forms.Button btnLoadAccounts;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox gbAccountsDetail;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.Label lblAcccountStatus;
        private System.Windows.Forms.CheckedListBox chkListBoxExportAccount;
        private System.Windows.Forms.CheckBox chkExportAccounts;
        private System.Windows.Forms.Button btnRefresh;

    }
}