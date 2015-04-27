namespace twtboardpro
{
    partial class frmAccountPasswordUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAccountPasswordUpdate));
            this.grpBoxModuleAuthentication = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnUpdatePassword = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.grpBoxModuleAuthentication.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBoxModuleAuthentication
            // 
            this.grpBoxModuleAuthentication.BackColor = System.Drawing.Color.Transparent;
            this.grpBoxModuleAuthentication.Controls.Add(this.label3);
            this.grpBoxModuleAuthentication.Controls.Add(this.txtUserName);
            this.grpBoxModuleAuthentication.Controls.Add(this.txtOldPassword);
            this.grpBoxModuleAuthentication.Controls.Add(this.label1);
            this.grpBoxModuleAuthentication.Controls.Add(this.btnUpdatePassword);
            this.grpBoxModuleAuthentication.Controls.Add(this.label2);
            this.grpBoxModuleAuthentication.Controls.Add(this.txtNewPassword);
            this.grpBoxModuleAuthentication.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBoxModuleAuthentication.Location = new System.Drawing.Point(12, 12);
            this.grpBoxModuleAuthentication.Name = "grpBoxModuleAuthentication";
            this.grpBoxModuleAuthentication.Size = new System.Drawing.Size(380, 173);
            this.grpBoxModuleAuthentication.TabIndex = 1;
            this.grpBoxModuleAuthentication.TabStop = false;
            this.grpBoxModuleAuthentication.Text = "Update Your Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "UserName";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(92, 20);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Size = new System.Drawing.Size(225, 21);
            this.txtUserName.TabIndex = 5;
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.Location = new System.Drawing.Point(92, 57);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.ReadOnly = true;
            this.txtOldPassword.Size = new System.Drawing.Size(225, 21);
            this.txtOldPassword.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "OldPassword";
            // 
            // btnUpdatePassword
            // 
            this.btnUpdatePassword.BackgroundImage = global::twtboardpro.Properties.Resources.save;
            this.btnUpdatePassword.Location = new System.Drawing.Point(139, 128);
            this.btnUpdatePassword.Name = "btnUpdatePassword";
            this.btnUpdatePassword.Size = new System.Drawing.Size(79, 28);
            this.btnUpdatePassword.TabIndex = 2;
            this.btnUpdatePassword.UseVisualStyleBackColor = true;
            this.btnUpdatePassword.Click += new System.EventHandler(this.btnUpdatePassword_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "NewPassword";
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(92, 91);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(225, 21);
            this.txtNewPassword.TabIndex = 0;
            // 
            // frmAccountPasswordUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 198);
            this.Controls.Add(this.grpBoxModuleAuthentication);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAccountPasswordUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Account Password Update";
            this.Load += new System.EventHandler(this.frmAccountPasswordUpdate_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmAccountPasswordUpdate_Paint);
            this.grpBoxModuleAuthentication.ResumeLayout(false);
            this.grpBoxModuleAuthentication.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxModuleAuthentication;
        private System.Windows.Forms.Button btnUpdatePassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtOldPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
    }
}