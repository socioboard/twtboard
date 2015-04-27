namespace twtboardpro
{
    partial class FrmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSettings));
            this.btnSaveDBC = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDBCPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDBCUsername = new System.Windows.Forms.TextBox();
            this.txtDBpath = new System.Windows.Forms.TextBox();
            this.dbcGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbPath = new System.Windows.Forms.GroupBox();
            this.chkCopyLoggerData = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseMobileVersion = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSaveDelaySetting = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.txtGlobalMinDelay = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txtGlobalMaxDelay = new System.Windows.Forms.TextBox();
            this.chkUseGlobalDelay = new System.Windows.Forms.CheckBox();
            this.dbcGroupBox.SuspendLayout();
            this.gbPath.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSaveDBC
            // 
            this.btnSaveDBC.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveDBC.BackgroundImage")));
            this.btnSaveDBC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSaveDBC.Location = new System.Drawing.Point(127, 73);
            this.btnSaveDBC.Name = "btnSaveDBC";
            this.btnSaveDBC.Size = new System.Drawing.Size(85, 28);
            this.btnSaveDBC.TabIndex = 21;
            this.btnSaveDBC.UseVisualStyleBackColor = true;
            this.btnSaveDBC.Click += new System.EventHandler(this.btnSaveDBC_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "DBC Password:";
            // 
            // txtDBCPassword
            // 
            this.txtDBCPassword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBCPassword.Location = new System.Drawing.Point(127, 46);
            this.txtDBCPassword.Name = "txtDBCPassword";
            this.txtDBCPassword.Size = new System.Drawing.Size(368, 21);
            this.txtDBCPassword.TabIndex = 19;
            this.txtDBCPassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(21, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "DBC Username:";
            // 
            // txtDBCUsername
            // 
            this.txtDBCUsername.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBCUsername.Location = new System.Drawing.Point(127, 19);
            this.txtDBCUsername.Name = "txtDBCUsername";
            this.txtDBCUsername.Size = new System.Drawing.Size(368, 21);
            this.txtDBCUsername.TabIndex = 17;
            // 
            // txtDBpath
            // 
            this.txtDBpath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBpath.Location = new System.Drawing.Point(152, 38);
            this.txtDBpath.Name = "txtDBpath";
            this.txtDBpath.Size = new System.Drawing.Size(368, 21);
            this.txtDBpath.TabIndex = 14;
            // 
            // dbcGroupBox
            // 
            this.dbcGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.dbcGroupBox.Controls.Add(this.label4);
            this.dbcGroupBox.Controls.Add(this.btnSaveDBC);
            this.dbcGroupBox.Controls.Add(this.label5);
            this.dbcGroupBox.Controls.Add(this.txtDBCUsername);
            this.dbcGroupBox.Controls.Add(this.txtDBCPassword);
            this.dbcGroupBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dbcGroupBox.Location = new System.Drawing.Point(12, 12);
            this.dbcGroupBox.Name = "dbcGroupBox";
            this.dbcGroupBox.Size = new System.Drawing.Size(534, 105);
            this.dbcGroupBox.TabIndex = 22;
            this.dbcGroupBox.TabStop = false;
            this.dbcGroupBox.Text = "Death By Captcha";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Your twtboardpro DB is located here ";
            // 
            // gbPath
            // 
            this.gbPath.BackColor = System.Drawing.Color.Transparent;
            this.gbPath.Controls.Add(this.chkCopyLoggerData);
            this.gbPath.Controls.Add(this.txtDBpath);
            this.gbPath.Controls.Add(this.label1);
            this.gbPath.Location = new System.Drawing.Point(12, 121);
            this.gbPath.Name = "gbPath";
            this.gbPath.Size = new System.Drawing.Size(534, 71);
            this.gbPath.TabIndex = 23;
            this.gbPath.TabStop = false;
            this.gbPath.Text = "DB Location and Copy Logger Data";
            // 
            // chkCopyLoggerData
            // 
            this.chkCopyLoggerData.AutoSize = true;
            this.chkCopyLoggerData.Location = new System.Drawing.Point(14, 40);
            this.chkCopyLoggerData.Name = "chkCopyLoggerData";
            this.chkCopyLoggerData.Size = new System.Drawing.Size(130, 17);
            this.chkCopyLoggerData.TabIndex = 15;
            this.chkCopyLoggerData.Text = "Copy Logger Data";
            this.chkCopyLoggerData.UseVisualStyleBackColor = true;
            this.chkCopyLoggerData.CheckedChanged += new System.EventHandler(this.chkCopyLoggerData_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.chkUseMobileVersion);
            this.groupBox1.Location = new System.Drawing.Point(12, 198);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(163, 50);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mobile Version";
            // 
            // chkUseMobileVersion
            // 
            this.chkUseMobileVersion.AutoSize = true;
            this.chkUseMobileVersion.Location = new System.Drawing.Point(21, 20);
            this.chkUseMobileVersion.Name = "chkUseMobileVersion";
            this.chkUseMobileVersion.Size = new System.Drawing.Size(126, 17);
            this.chkUseMobileVersion.TabIndex = 0;
            this.chkUseMobileVersion.Text = "UseMobileVersion";
            this.chkUseMobileVersion.UseVisualStyleBackColor = true;
            this.chkUseMobileVersion.CheckedChanged += new System.EventHandler(this.chkUseMobileVersion_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnSaveDelaySetting);
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.txtGlobalMinDelay);
            this.groupBox2.Controls.Add(this.label31);
            this.groupBox2.Controls.Add(this.txtGlobalMaxDelay);
            this.groupBox2.Controls.Add(this.chkUseGlobalDelay);
            this.groupBox2.Location = new System.Drawing.Point(181, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(365, 50);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Global Delay";
            // 
            // btnSaveDelaySetting
            // 
            this.btnSaveDelaySetting.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSaveDelaySetting.BackgroundImage")));
            this.btnSaveDelaySetting.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSaveDelaySetting.Location = new System.Drawing.Point(273, 14);
            this.btnSaveDelaySetting.Name = "btnSaveDelaySetting";
            this.btnSaveDelaySetting.Size = new System.Drawing.Size(85, 28);
            this.btnSaveDelaySetting.TabIndex = 62;
            this.btnSaveDelaySetting.UseVisualStyleBackColor = true;
            this.btnSaveDelaySetting.Click += new System.EventHandler(this.btnSaveDelaySetting_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(109, 21);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(44, 13);
            this.label29.TabIndex = 58;
            this.label29.Text = " From ";
            // 
            // txtGlobalMinDelay
            // 
            this.txtGlobalMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlobalMinDelay.ForeColor = System.Drawing.Color.Black;
            this.txtGlobalMinDelay.Location = new System.Drawing.Point(152, 16);
            this.txtGlobalMinDelay.Name = "txtGlobalMinDelay";
            this.txtGlobalMinDelay.Size = new System.Drawing.Size(38, 21);
            this.txtGlobalMinDelay.TabIndex = 60;
            this.txtGlobalMinDelay.Text = "20";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(196, 20);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(18, 13);
            this.label31.TabIndex = 61;
            this.label31.Text = "to";
            // 
            // txtGlobalMaxDelay
            // 
            this.txtGlobalMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGlobalMaxDelay.ForeColor = System.Drawing.Color.Black;
            this.txtGlobalMaxDelay.Location = new System.Drawing.Point(225, 17);
            this.txtGlobalMaxDelay.Name = "txtGlobalMaxDelay";
            this.txtGlobalMaxDelay.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txtGlobalMaxDelay.Size = new System.Drawing.Size(40, 21);
            this.txtGlobalMaxDelay.TabIndex = 59;
            this.txtGlobalMaxDelay.Text = "25";
            // 
            // chkUseGlobalDelay
            // 
            this.chkUseGlobalDelay.AutoSize = true;
            this.chkUseGlobalDelay.Location = new System.Drawing.Point(10, 20);
            this.chkUseGlobalDelay.Name = "chkUseGlobalDelay";
            this.chkUseGlobalDelay.Size = new System.Drawing.Size(99, 17);
            this.chkUseGlobalDelay.TabIndex = 0;
            this.chkUseGlobalDelay.Text = "Global Delay";
            this.chkUseGlobalDelay.UseVisualStyleBackColor = true;
            this.chkUseGlobalDelay.CheckedChanged += new System.EventHandler(this.chkUseGlobalDelay_CheckedChanged);
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(558, 260);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbPath);
            this.Controls.Add(this.dbcGroupBox);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "twtboardpro Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSettings_FormClosed);
            this.Load += new System.EventHandler(this.FrmSettings_Load_1);
            this.dbcGroupBox.ResumeLayout(false);
            this.dbcGroupBox.PerformLayout();
            this.gbPath.ResumeLayout(false);
            this.gbPath.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSaveDBC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDBCPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDBCUsername;
        private System.Windows.Forms.TextBox txtDBpath;
        private System.Windows.Forms.GroupBox dbcGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseMobileVersion;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkUseGlobalDelay;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtGlobalMinDelay;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtGlobalMaxDelay;
        private System.Windows.Forms.Button btnSaveDelaySetting;
        private System.Windows.Forms.CheckBox chkCopyLoggerData;

    }
}