namespace FaceDominator
{
    partial class FrmAccountCreatorHTTP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAccountCreatorHTTP));
            this.btnSwitchToNextAccount = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtCaptcha = new System.Windows.Forms.TextBox();
            this.pictureCaptcha = new System.Windows.Forms.PictureBox();
            this.lstLogger = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumberOfThreads = new System.Windows.Forms.TextBox();
            this.panelSingleThreaded = new System.Windows.Forms.Panel();
            this.panelSMultiThreaded = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoSingleThreaded = new System.Windows.Forms.RadioButton();
            this.rdoMassCreator = new System.Windows.Forms.RadioButton();
            this.BtnProxyAssigner = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAccountsPerProxy = new System.Windows.Forms.TextBox();
            this.chkRandomFirstAndLastNames = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCaptcha)).BeginInit();
            this.panelSingleThreaded.SuspendLayout();
            this.panelSMultiThreaded.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSwitchToNextAccount
            // 
            this.btnSwitchToNextAccount.Location = new System.Drawing.Point(5, 347);
            this.btnSwitchToNextAccount.Name = "btnSwitchToNextAccount";
            this.btnSwitchToNextAccount.Size = new System.Drawing.Size(258, 40);
            this.btnSwitchToNextAccount.TabIndex = 25;
            this.btnSwitchToNextAccount.Text = "SwitchToNextAccount";
            this.btnSwitchToNextAccount.UseVisualStyleBackColor = true;
            this.btnSwitchToNextAccount.Visible = false;
            this.btnSwitchToNextAccount.Click += new System.EventHandler(this.btnSwitchToNextAccount_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Please enter Captcha :";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(131, 160);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(134, 20);
            this.btnSubmit.TabIndex = 23;
            this.btnSubmit.Text = "Submit Captcha";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtCaptcha
            // 
            this.txtCaptcha.Location = new System.Drawing.Point(131, 120);
            this.txtCaptcha.Name = "txtCaptcha";
            this.txtCaptcha.Size = new System.Drawing.Size(183, 20);
            this.txtCaptcha.TabIndex = 22;
            // 
            // pictureCaptcha
            // 
            this.pictureCaptcha.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureCaptcha.ErrorImage = null;
            this.pictureCaptcha.InitialImage = null;
            this.pictureCaptcha.Location = new System.Drawing.Point(31, 3);
            this.pictureCaptcha.Name = "pictureCaptcha";
            this.pictureCaptcha.Size = new System.Drawing.Size(307, 97);
            this.pictureCaptcha.TabIndex = 21;
            this.pictureCaptcha.TabStop = false;
            // 
            // lstLogger
            // 
            this.lstLogger.FormattingEnabled = true;
            this.lstLogger.Location = new System.Drawing.Point(435, 3);
            this.lstLogger.Name = "lstLogger";
            this.lstLogger.Size = new System.Drawing.Size(474, 394);
            this.lstLogger.TabIndex = 20;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnStart.Location = new System.Drawing.Point(223, 231);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(179, 40);
            this.btnStart.TabIndex = 28;
            this.btnStart.Text = "Start Account Creation";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "No. Of Threads";
            // 
            // txtNumberOfThreads
            // 
            this.txtNumberOfThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtNumberOfThreads.Location = new System.Drawing.Point(137, 21);
            this.txtNumberOfThreads.Name = "txtNumberOfThreads";
            this.txtNumberOfThreads.Size = new System.Drawing.Size(111, 20);
            this.txtNumberOfThreads.TabIndex = 47;
            // 
            // panelSingleThreaded
            // 
            this.panelSingleThreaded.Controls.Add(this.pictureCaptcha);
            this.panelSingleThreaded.Controls.Add(this.txtCaptcha);
            this.panelSingleThreaded.Controls.Add(this.btnSubmit);
            this.panelSingleThreaded.Controls.Add(this.label2);
            this.panelSingleThreaded.Location = new System.Drawing.Point(435, 4);
            this.panelSingleThreaded.Name = "panelSingleThreaded";
            this.panelSingleThreaded.Size = new System.Drawing.Size(344, 188);
            this.panelSingleThreaded.TabIndex = 49;
            this.panelSingleThreaded.Visible = false;
            // 
            // panelSMultiThreaded
            // 
            this.panelSMultiThreaded.Controls.Add(this.txtNumberOfThreads);
            this.panelSMultiThreaded.Controls.Add(this.label3);
            this.panelSMultiThreaded.Location = new System.Drawing.Point(24, 7);
            this.panelSMultiThreaded.Name = "panelSMultiThreaded";
            this.panelSMultiThreaded.Size = new System.Drawing.Size(255, 54);
            this.panelSMultiThreaded.TabIndex = 50;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdoSingleThreaded);
            this.groupBox4.Controls.Add(this.rdoMassCreator);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox4.Location = new System.Drawing.Point(24, 67);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(257, 54);
            this.groupBox4.TabIndex = 54;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode Selection";
            // 
            // rdoSingleThreaded
            // 
            this.rdoSingleThreaded.AutoSize = true;
            this.rdoSingleThreaded.Location = new System.Drawing.Point(21, 24);
            this.rdoSingleThreaded.Name = "rdoSingleThreaded";
            this.rdoSingleThreaded.Size = new System.Drawing.Size(103, 17);
            this.rdoSingleThreaded.TabIndex = 19;
            this.rdoSingleThreaded.TabStop = true;
            this.rdoSingleThreaded.Text = "Single Threaded";
            this.rdoSingleThreaded.UseVisualStyleBackColor = true;
            this.rdoSingleThreaded.CheckedChanged += new System.EventHandler(this.rdoSingleThreaded_CheckedChanged);
            // 
            // rdoMassCreator
            // 
            this.rdoMassCreator.AutoSize = true;
            this.rdoMassCreator.Location = new System.Drawing.Point(153, 24);
            this.rdoMassCreator.Name = "rdoMassCreator";
            this.rdoMassCreator.Size = new System.Drawing.Size(87, 17);
            this.rdoMassCreator.TabIndex = 20;
            this.rdoMassCreator.TabStop = true;
            this.rdoMassCreator.Text = "Mass Creator";
            this.rdoMassCreator.UseVisualStyleBackColor = true;
            this.rdoMassCreator.CheckedChanged += new System.EventHandler(this.rdoMassCreator_CheckedChanged);
            // 
            // BtnProxyAssigner
            // 
            this.BtnProxyAssigner.BackColor = System.Drawing.SystemColors.MenuBar;
            this.BtnProxyAssigner.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.BtnProxyAssigner.Location = new System.Drawing.Point(22, 231);
            this.BtnProxyAssigner.Name = "BtnProxyAssigner";
            this.BtnProxyAssigner.Size = new System.Drawing.Size(179, 40);
            this.BtnProxyAssigner.TabIndex = 55;
            this.BtnProxyAssigner.Text = "Proxy Assigner on the Fly";
            this.BtnProxyAssigner.UseVisualStyleBackColor = false;
            this.BtnProxyAssigner.Click += new System.EventHandler(this.BtnProxyAssigner_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(285, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 27;
            this.label1.Text = "Accounts Per Proxy :";
            this.label1.Visible = false;
            // 
            // txtAccountsPerProxy
            // 
            this.txtAccountsPerProxy.Location = new System.Drawing.Point(393, 15);
            this.txtAccountsPerProxy.Name = "txtAccountsPerProxy";
            this.txtAccountsPerProxy.Size = new System.Drawing.Size(41, 20);
            this.txtAccountsPerProxy.TabIndex = 26;
            this.txtAccountsPerProxy.Visible = false;
            // 
            // chkRandomFirstAndLastNames
            // 
            this.chkRandomFirstAndLastNames.AutoSize = true;
            this.chkRandomFirstAndLastNames.Location = new System.Drawing.Point(24, 150);
            this.chkRandomFirstAndLastNames.Name = "chkRandomFirstAndLastNames";
            this.chkRandomFirstAndLastNames.Size = new System.Drawing.Size(168, 17);
            this.chkRandomFirstAndLastNames.TabIndex = 56;
            this.chkRandomFirstAndLastNames.Text = "Random First and Last Names";
            this.chkRandomFirstAndLastNames.UseVisualStyleBackColor = true;
            // 
            // FrmAccountCreatorHTTP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 399);
            this.Controls.Add(this.chkRandomFirstAndLastNames);
            this.Controls.Add(this.BtnProxyAssigner);
            this.Controls.Add(this.panelSMultiThreaded);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAccountsPerProxy);
            this.Controls.Add(this.btnSwitchToNextAccount);
            this.Controls.Add(this.lstLogger);
            this.Controls.Add(this.panelSingleThreaded);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmAccountCreatorHTTP";
            this.Text = "FrmAccountCreatorHTTP";
            this.Load += new System.EventHandler(this.FrmAccountCreatorHTTP_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCaptcha)).EndInit();
            this.panelSingleThreaded.ResumeLayout(false);
            this.panelSingleThreaded.PerformLayout();
            this.panelSMultiThreaded.ResumeLayout(false);
            this.panelSMultiThreaded.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSwitchToNextAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtCaptcha;
        private System.Windows.Forms.PictureBox pictureCaptcha;
        private System.Windows.Forms.ListBox lstLogger;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNumberOfThreads;
        private System.Windows.Forms.Panel panelSingleThreaded;
        private System.Windows.Forms.Panel panelSMultiThreaded;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoSingleThreaded;
        private System.Windows.Forms.RadioButton rdoMassCreator;
        private System.Windows.Forms.Button BtnProxyAssigner;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAccountsPerProxy;
        private System.Windows.Forms.CheckBox chkRandomFirstAndLastNames;
    }
}