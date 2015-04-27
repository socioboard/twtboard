namespace twtboardpro
{
    partial class FrmVerfyingImage
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStartVerfication = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtChildVerfiyEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartVerify = new System.Windows.Forms.Button();
            this.btnBrowseEmail = new System.Windows.Forms.Button();
            this.txtVerfiyEmail = new System.Windows.Forms.TextBox();
            this.lblVErfityEmail = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnStartVerfication);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtChildVerfiyEmail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnStartVerify);
            this.groupBox1.Controls.Add(this.btnBrowseEmail);
            this.groupBox1.Controls.Add(this.txtVerfiyEmail);
            this.groupBox1.Controls.Add(this.lblVErfityEmail);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(642, 184);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Verfiying +1 Email";
            // 
            // btnStartVerfication
            // 
            this.btnStartVerfication.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStartVerfication.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartVerfication.Location = new System.Drawing.Point(169, 138);
            this.btnStartVerfication.Name = "btnStartVerfication";
            this.btnStartVerfication.Size = new System.Drawing.Size(92, 32);
            this.btnStartVerfication.TabIndex = 7;
            this.btnStartVerfication.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Location = new System.Drawing.Point(521, 109);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 29);
            this.button1.TabIndex = 6;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtChildVerfiyEmail
            // 
            this.txtChildVerfiyEmail.Location = new System.Drawing.Point(169, 111);
            this.txtChildVerfiyEmail.Name = "txtChildVerfiyEmail";
            this.txtChildVerfiyEmail.Size = new System.Drawing.Size(349, 21);
            this.txtChildVerfiyEmail.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Twitter Created Accounts:";
            // 
            // btnStartVerify
            // 
            this.btnStartVerify.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStartVerify.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStartVerify.Location = new System.Drawing.Point(169, 56);
            this.btnStartVerify.Name = "btnStartVerify";
            this.btnStartVerify.Size = new System.Drawing.Size(92, 31);
            this.btnStartVerify.TabIndex = 3;
            this.btnStartVerify.UseVisualStyleBackColor = true;
            this.btnStartVerify.Click += new System.EventHandler(this.btnStartVerify_Click);
            // 
            // btnBrowseEmail
            // 
            this.btnBrowseEmail.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnBrowseEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrowseEmail.Location = new System.Drawing.Point(521, 25);
            this.btnBrowseEmail.Name = "btnBrowseEmail";
            this.btnBrowseEmail.Size = new System.Drawing.Size(80, 29);
            this.btnBrowseEmail.TabIndex = 2;
            this.btnBrowseEmail.UseVisualStyleBackColor = true;
            this.btnBrowseEmail.Click += new System.EventHandler(this.btnBrowseEmail_Click);
            // 
            // txtVerfiyEmail
            // 
            this.txtVerfiyEmail.Location = new System.Drawing.Point(169, 29);
            this.txtVerfiyEmail.Name = "txtVerfiyEmail";
            this.txtVerfiyEmail.Size = new System.Drawing.Size(349, 21);
            this.txtVerfiyEmail.TabIndex = 1;
            // 
            // lblVErfityEmail
            // 
            this.lblVErfityEmail.AutoSize = true;
            this.lblVErfityEmail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVErfityEmail.Location = new System.Drawing.Point(72, 33);
            this.lblVErfityEmail.Name = "lblVErfityEmail";
            this.lblVErfityEmail.Size = new System.Drawing.Size(100, 13);
            this.lblVErfityEmail.TabIndex = 0;
            this.lblVErfityEmail.Text = "Parent Email Id:";
            // 
            // FrmVerfyingImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(666, 199);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "FrmVerfyingImage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Verfying Image";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseEmail;
        private System.Windows.Forms.TextBox txtVerfiyEmail;
        private System.Windows.Forms.Label lblVErfityEmail;
        private System.Windows.Forms.Button btnStartVerify;
        private System.Windows.Forms.Button btnStartVerfication;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtChildVerfiyEmail;
        private System.Windows.Forms.Label label1;
    }
}