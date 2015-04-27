namespace TwtDominator
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
            this.btnExportAccounts = new System.Windows.Forms.Button();
            this.ButonClearProxies = new System.Windows.Forms.Button();
            this.btnAssignProxy = new System.Windows.Forms.Button();
            this.btnClearAccounts = new System.Windows.Forms.Button();
            this.btnLoadAccounts = new System.Windows.Forms.Button();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.lblRequestThreads = new System.Windows.Forms.Label();
            this.txtAccountsPerProxy = new System.Windows.Forms.TextBox();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBoxProfileDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
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
            this.splitContainer1.Panel2.Controls.Add(this.lstLogger);
            this.splitContainer1.Panel2.Controls.Add(this.lblRequestThreads);
            this.splitContainer1.Panel2.Controls.Add(this.txtAccountsPerProxy);
            this.splitContainer1.Panel2.Controls.Add(this.dgvAccounts);
            this.splitContainer1.Size = new System.Drawing.Size(984, 562);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 2;
            // 
            // groupBoxProfileDetails
            // 
            this.groupBoxProfileDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(238)))));
            this.groupBoxProfileDetails.Controls.Add(this.btnExportAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.ButonClearProxies);
            this.groupBoxProfileDetails.Controls.Add(this.btnAssignProxy);
            this.groupBoxProfileDetails.Controls.Add(this.btnClearAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.btnLoadAccounts);
            this.groupBoxProfileDetails.Controls.Add(this.textBox12);
            this.groupBoxProfileDetails.Controls.Add(this.label10);
            this.groupBoxProfileDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxProfileDetails.Location = new System.Drawing.Point(0, 0);
            this.groupBoxProfileDetails.Name = "groupBoxProfileDetails";
            this.groupBoxProfileDetails.Size = new System.Drawing.Size(264, 562);
            this.groupBoxProfileDetails.TabIndex = 2;
            this.groupBoxProfileDetails.TabStop = false;
            this.groupBoxProfileDetails.Text = "Profile Details";
            this.groupBoxProfileDetails.Enter += new System.EventHandler(this.groupBoxProfileDetails_Enter);
            // 
            // btnExportAccounts
            // 
            this.btnExportAccounts.BackColor = System.Drawing.Color.White;
            this.btnExportAccounts.Location = new System.Drawing.Point(83, 207);
            this.btnExportAccounts.Name = "btnExportAccounts";
            this.btnExportAccounts.Size = new System.Drawing.Size(147, 27);
            this.btnExportAccounts.TabIndex = 18;
            this.btnExportAccounts.Text = "Export Accounts";
            this.btnExportAccounts.UseVisualStyleBackColor = false;
            this.btnExportAccounts.Click += new System.EventHandler(this.btnExportAccounts_Click);
            // 
            // ButonClearProxies
            // 
            this.ButonClearProxies.BackColor = System.Drawing.Color.White;
            this.ButonClearProxies.Location = new System.Drawing.Point(83, 156);
            this.ButonClearProxies.Name = "ButonClearProxies";
            this.ButonClearProxies.Size = new System.Drawing.Size(147, 27);
            this.ButonClearProxies.TabIndex = 17;
            this.ButonClearProxies.Text = "Clear Proxies";
            this.ButonClearProxies.UseVisualStyleBackColor = false;
            this.ButonClearProxies.Click += new System.EventHandler(this.ButonClearProxies_Click);
            // 
            // btnAssignProxy
            // 
            this.btnAssignProxy.BackColor = System.Drawing.Color.White;
            this.btnAssignProxy.Location = new System.Drawing.Point(83, 417);
            this.btnAssignProxy.Name = "btnAssignProxy";
            this.btnAssignProxy.Size = new System.Drawing.Size(147, 27);
            this.btnAssignProxy.TabIndex = 16;
            this.btnAssignProxy.Text = "Assign Proxy";
            this.btnAssignProxy.UseVisualStyleBackColor = false;
            this.btnAssignProxy.Visible = false;
            this.btnAssignProxy.Click += new System.EventHandler(this.btnAssignProxy_Click);
            // 
            // btnClearAccounts
            // 
            this.btnClearAccounts.BackColor = System.Drawing.Color.White;
            this.btnClearAccounts.Location = new System.Drawing.Point(83, 104);
            this.btnClearAccounts.Name = "btnClearAccounts";
            this.btnClearAccounts.Size = new System.Drawing.Size(141, 27);
            this.btnClearAccounts.TabIndex = 15;
            this.btnClearAccounts.Text = "Clear Accounts";
            this.btnClearAccounts.UseVisualStyleBackColor = false;
            this.btnClearAccounts.Click += new System.EventHandler(this.btnClearAccounts_Click);
            // 
            // btnLoadAccounts
            // 
            this.btnLoadAccounts.BackColor = System.Drawing.Color.White;
            this.btnLoadAccounts.Location = new System.Drawing.Point(83, 56);
            this.btnLoadAccounts.Name = "btnLoadAccounts";
            this.btnLoadAccounts.Size = new System.Drawing.Size(147, 27);
            this.btnLoadAccounts.TabIndex = 9;
            this.btnLoadAccounts.Text = "Load Accounts";
            this.btnLoadAccounts.UseVisualStyleBackColor = false;
            this.btnLoadAccounts.Click += new System.EventHandler(this.btnLoadAccounts_Click);
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(83, 30);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(147, 20);
            this.textBox12.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 33);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Accounts File";
            // 
            // lstLogger
            // 
            this.lstLogger.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.Location = new System.Drawing.Point(0, 415);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(716, 147);
            this.lstLogger.TabIndex = 54;
            // 
            // lblRequestThreads
            // 
            this.lblRequestThreads.AutoSize = true;
            this.lblRequestThreads.Location = new System.Drawing.Point(12, 378);
            this.lblRequestThreads.Name = "lblRequestThreads";
            this.lblRequestThreads.Size = new System.Drawing.Size(131, 13);
            this.lblRequestThreads.TabIndex = 53;
            this.lblRequestThreads.Text = "No. Of accounts per proxy";
            this.lblRequestThreads.Visible = false;
            // 
            // txtAccountsPerProxy
            // 
            this.txtAccountsPerProxy.Location = new System.Drawing.Point(149, 375);
            this.txtAccountsPerProxy.Name = "txtAccountsPerProxy";
            this.txtAccountsPerProxy.Size = new System.Drawing.Size(45, 20);
            this.txtAccountsPerProxy.TabIndex = 17;
            this.txtAccountsPerProxy.Visible = false;
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(238)))));
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvAccounts.Location = new System.Drawing.Point(0, 0);
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.Size = new System.Drawing.Size(716, 361);
            this.dgvAccounts.TabIndex = 2;
            this.dgvAccounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccounts_CellContentClick);
            // 
            // frmAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(226)))), ((int)(((byte)(238)))));
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Accounts";
            this.Load += new System.EventHandler(this.frmAccounts_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBoxProfileDetails.ResumeLayout(false);
            this.groupBoxProfileDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBoxProfileDetails;
        private System.Windows.Forms.Button btnLoadAccounts;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.Button btnClearAccounts;
        private System.Windows.Forms.Button btnAssignProxy;
        private System.Windows.Forms.TextBox txtAccountsPerProxy;
        private System.Windows.Forms.Label lblRequestThreads;
        private System.Windows.Forms.Button ButonClearProxies;
        private System.Windows.Forms.Button btnExportAccounts;
        private System.Windows.Forms.ListBox lstLogger;

    }
}