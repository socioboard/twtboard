namespace twtboardpro
{
    partial class Frm_EmailVerification
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_EmailVerification));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnResendVerification = new System.Windows.Forms.Button();
            this.btnStop_EmailVarification = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_startVerification = new System.Windows.Forms.Button();
            this.Btn_FileBrowse = new System.Windows.Forms.Button();
            this.txt_NonVeryfiedEmailAccountFilePath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstboxLogger_EmailVerification = new System.Windows.Forms.ListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnResendVerification);
            this.groupBox1.Controls.Add(this.btnStop_EmailVarification);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_startVerification);
            this.groupBox1.Controls.Add(this.Btn_FileBrowse);
            this.groupBox1.Controls.Add(this.txt_NonVeryfiedEmailAccountFilePath);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 133);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Email Verification";
            // 
            // btnResendVerification
            // 
            this.btnResendVerification.BackgroundImage = global::twtboardpro.Properties.Resources.resend_verification;
            this.btnResendVerification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnResendVerification.Location = new System.Drawing.Point(277, 73);
            this.btnResendVerification.Name = "btnResendVerification";
            this.btnResendVerification.Size = new System.Drawing.Size(154, 29);
            this.btnResendVerification.TabIndex = 77;
            this.btnResendVerification.UseVisualStyleBackColor = true;
            this.btnResendVerification.Click += new System.EventHandler(this.btnResendVerification_Click);
            // 
            // btnStop_EmailVarification
            // 
            this.btnStop_EmailVarification.BackColor = System.Drawing.Color.Transparent;
            this.btnStop_EmailVarification.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btnStop_EmailVarification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStop_EmailVarification.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop_EmailVarification.ForeColor = System.Drawing.Color.Black;
            this.btnStop_EmailVarification.Location = new System.Drawing.Point(448, 72);
            this.btnStop_EmailVarification.Name = "btnStop_EmailVarification";
            this.btnStop_EmailVarification.Size = new System.Drawing.Size(88, 29);
            this.btnStop_EmailVarification.TabIndex = 76;
            this.btnStop_EmailVarification.UseVisualStyleBackColor = false;
            this.btnStop_EmailVarification.Click += new System.EventHandler(this.btnStop_EmailVarification_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Load Accounts:";
            // 
            // btn_startVerification
            // 
            this.btn_startVerification.BackgroundImage = global::twtboardpro.Properties.Resources.account_verification;
            this.btn_startVerification.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_startVerification.Location = new System.Drawing.Point(106, 73);
            this.btn_startVerification.Name = "btn_startVerification";
            this.btn_startVerification.Size = new System.Drawing.Size(154, 30);
            this.btn_startVerification.TabIndex = 2;
            this.btn_startVerification.UseVisualStyleBackColor = true;
            this.btn_startVerification.Click += new System.EventHandler(this.btn_startVerification_Click);
            // 
            // Btn_FileBrowse
            // 
            this.Btn_FileBrowse.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.Btn_FileBrowse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Btn_FileBrowse.Location = new System.Drawing.Point(547, 42);
            this.Btn_FileBrowse.Name = "Btn_FileBrowse";
            this.Btn_FileBrowse.Size = new System.Drawing.Size(83, 29);
            this.Btn_FileBrowse.TabIndex = 1;
            this.Btn_FileBrowse.UseVisualStyleBackColor = true;
            this.Btn_FileBrowse.Click += new System.EventHandler(this.Btn_FileBrowse_Click);
            // 
            // txt_NonVeryfiedEmailAccountFilePath
            // 
            this.txt_NonVeryfiedEmailAccountFilePath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_NonVeryfiedEmailAccountFilePath.Location = new System.Drawing.Point(106, 47);
            this.txt_NonVeryfiedEmailAccountFilePath.Name = "txt_NonVeryfiedEmailAccountFilePath";
            this.txt_NonVeryfiedEmailAccountFilePath.ReadOnly = true;
            this.txt_NonVeryfiedEmailAccountFilePath.Size = new System.Drawing.Size(435, 21);
            this.txt_NonVeryfiedEmailAccountFilePath.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.lstboxLogger_EmailVerification);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(13, 152);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(664, 162);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Process Logger";
            // 
            // lstboxLogger_EmailVerification
            // 
            this.lstboxLogger_EmailVerification.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstboxLogger_EmailVerification.FormattingEnabled = true;
            this.lstboxLogger_EmailVerification.Location = new System.Drawing.Point(7, 20);
            this.lstboxLogger_EmailVerification.Name = "lstboxLogger_EmailVerification";
            this.lstboxLogger_EmailVerification.Size = new System.Drawing.Size(651, 134);
            this.lstboxLogger_EmailVerification.TabIndex = 0;
            // 
            // Frm_EmailVerification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 331);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Frm_EmailVerification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Email Verification";
            this.Load += new System.EventHandler(this.Frm_EmailVerification_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Frm_EmailVerification_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txt_NonVeryfiedEmailAccountFilePath;
        private System.Windows.Forms.Button Btn_FileBrowse;
        private System.Windows.Forms.Button btn_startVerification;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstboxLogger_EmailVerification;
        private System.Windows.Forms.Button btnStop_EmailVarification;
        private System.Windows.Forms.Button btnResendVerification;
    }
}