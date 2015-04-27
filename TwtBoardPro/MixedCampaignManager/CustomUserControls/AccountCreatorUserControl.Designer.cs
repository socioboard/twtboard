namespace MixedCampaignManager.CustomUserControls
{
    partial class AccountCreatorUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox65 = new System.Windows.Forms.GroupBox();
            this.chk_SmProfileImage = new System.Windows.Forms.CheckBox();
            this.chk_SmProfileUrl = new System.Windows.Forms.CheckBox();
            this.chk_SmProfileDescription = new System.Windows.Forms.CheckBox();
            this.chk_SmLocation = new System.Windows.Forms.CheckBox();
            this.groupBox25 = new System.Windows.Forms.GroupBox();
            this.chk_SmUseOnlyBoth = new System.Windows.Forms.CheckBox();
            this.chk_SmUseOnlyNumbers = new System.Windows.Forms.CheckBox();
            this.chk_SmUseOnlyString = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_AccountProfileDetails = new System.Windows.Forms.Button();
            this.btn_AccountProfileImage = new System.Windows.Forms.Button();
            this.lb_ImageFolder = new System.Windows.Forms.Label();
            this.lb_ProfileDetailsFilePath = new System.Windows.Forms.Label();
            this.txt_ImageFolderPath = new System.Windows.Forms.TextBox();
            this.txt_ProfileDetailsFilePath = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_AccountDetailsFilePath = new System.Windows.Forms.TextBox();
            this.lb_AccountDetailsFolder = new System.Windows.Forms.Label();
            this.btn_AccountDetails = new System.Windows.Forms.Button();
            this.groupBox65.SuspendLayout();
            this.groupBox25.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox65
            // 
            this.groupBox65.Controls.Add(this.chk_SmProfileImage);
            this.groupBox65.Controls.Add(this.chk_SmProfileUrl);
            this.groupBox65.Controls.Add(this.chk_SmProfileDescription);
            this.groupBox65.Controls.Add(this.chk_SmLocation);
            this.groupBox65.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox65.Location = new System.Drawing.Point(641, 3);
            this.groupBox65.Name = "groupBox65";
            this.groupBox65.Size = new System.Drawing.Size(133, 130);
            this.groupBox65.TabIndex = 72;
            this.groupBox65.TabStop = false;
            this.groupBox65.Text = "Change Profile details";
            // 
            // chk_SmProfileImage
            // 
            this.chk_SmProfileImage.AutoSize = true;
            this.chk_SmProfileImage.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_SmProfileImage.ForeColor = System.Drawing.Color.Black;
            this.chk_SmProfileImage.Location = new System.Drawing.Point(6, 83);
            this.chk_SmProfileImage.Name = "chk_SmProfileImage";
            this.chk_SmProfileImage.Size = new System.Drawing.Size(91, 16);
            this.chk_SmProfileImage.TabIndex = 3;
            this.chk_SmProfileImage.Text = "Profile Image";
            this.chk_SmProfileImage.UseVisualStyleBackColor = true;
            this.chk_SmProfileImage.CheckedChanged += new System.EventHandler(this.chk_SmProfileImage_CheckedChanged);
            // 
            // chk_SmProfileUrl
            // 
            this.chk_SmProfileUrl.AutoSize = true;
            this.chk_SmProfileUrl.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_SmProfileUrl.ForeColor = System.Drawing.Color.Black;
            this.chk_SmProfileUrl.Location = new System.Drawing.Point(6, 44);
            this.chk_SmProfileUrl.Name = "chk_SmProfileUrl";
            this.chk_SmProfileUrl.Size = new System.Drawing.Size(80, 16);
            this.chk_SmProfileUrl.TabIndex = 2;
            this.chk_SmProfileUrl.Text = "Profile URL";
            this.chk_SmProfileUrl.UseVisualStyleBackColor = true;
            this.chk_SmProfileUrl.CheckedChanged += new System.EventHandler(this.chk_SmProfileUrl_CheckedChanged);
            // 
            // chk_SmProfileDescription
            // 
            this.chk_SmProfileDescription.AutoSize = true;
            this.chk_SmProfileDescription.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_SmProfileDescription.ForeColor = System.Drawing.Color.Black;
            this.chk_SmProfileDescription.Location = new System.Drawing.Point(6, 63);
            this.chk_SmProfileDescription.Name = "chk_SmProfileDescription";
            this.chk_SmProfileDescription.Size = new System.Drawing.Size(82, 16);
            this.chk_SmProfileDescription.TabIndex = 1;
            this.chk_SmProfileDescription.Text = "Description";
            this.chk_SmProfileDescription.UseVisualStyleBackColor = true;
            this.chk_SmProfileDescription.CheckedChanged += new System.EventHandler(this.chk_SmProfileDescription_CheckedChanged);
            // 
            // chk_SmLocation
            // 
            this.chk_SmLocation.AutoSize = true;
            this.chk_SmLocation.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chk_SmLocation.ForeColor = System.Drawing.Color.Black;
            this.chk_SmLocation.Location = new System.Drawing.Point(6, 24);
            this.chk_SmLocation.Name = "chk_SmLocation";
            this.chk_SmLocation.Size = new System.Drawing.Size(67, 16);
            this.chk_SmLocation.TabIndex = 0;
            this.chk_SmLocation.Text = "Location";
            this.chk_SmLocation.UseVisualStyleBackColor = true;
            this.chk_SmLocation.CheckedChanged += new System.EventHandler(this.chk_SmLocation_CheckedChanged);
            // 
            // groupBox25
            // 
            this.groupBox25.BackColor = System.Drawing.Color.Transparent;
            this.groupBox25.Controls.Add(this.chk_SmUseOnlyBoth);
            this.groupBox25.Controls.Add(this.chk_SmUseOnlyNumbers);
            this.groupBox25.Controls.Add(this.chk_SmUseOnlyString);
            this.groupBox25.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.groupBox25.Location = new System.Drawing.Point(578, 3);
            this.groupBox25.Name = "groupBox25";
            this.groupBox25.Size = new System.Drawing.Size(196, 64);
            this.groupBox25.TabIndex = 71;
            this.groupBox25.TabStop = false;
            this.groupBox25.Text = "Create Username Using";
            this.groupBox25.Visible = false;
            // 
            // chk_SmUseOnlyBoth
            // 
            this.chk_SmUseOnlyBoth.AutoSize = true;
            this.chk_SmUseOnlyBoth.BackColor = System.Drawing.Color.Transparent;
            this.chk_SmUseOnlyBoth.ForeColor = System.Drawing.Color.Black;
            this.chk_SmUseOnlyBoth.Location = new System.Drawing.Point(21, 41);
            this.chk_SmUseOnlyBoth.Name = "chk_SmUseOnlyBoth";
            this.chk_SmUseOnlyBoth.Size = new System.Drawing.Size(70, 17);
            this.chk_SmUseOnlyBoth.TabIndex = 2;
            this.chk_SmUseOnlyBoth.Text = "Use Both";
            this.chk_SmUseOnlyBoth.UseVisualStyleBackColor = false;
            this.chk_SmUseOnlyBoth.CheckedChanged += new System.EventHandler(this.chk_SmUseOnlyBoth_CheckedChanged);
            // 
            // chk_SmUseOnlyNumbers
            // 
            this.chk_SmUseOnlyNumbers.AutoSize = true;
            this.chk_SmUseOnlyNumbers.BackColor = System.Drawing.Color.Transparent;
            this.chk_SmUseOnlyNumbers.ForeColor = System.Drawing.Color.Black;
            this.chk_SmUseOnlyNumbers.Location = new System.Drawing.Point(104, 22);
            this.chk_SmUseOnlyNumbers.Name = "chk_SmUseOnlyNumbers";
            this.chk_SmUseOnlyNumbers.Size = new System.Drawing.Size(92, 17);
            this.chk_SmUseOnlyNumbers.TabIndex = 1;
            this.chk_SmUseOnlyNumbers.Text = "Only Numbers";
            this.chk_SmUseOnlyNumbers.UseVisualStyleBackColor = false;
            this.chk_SmUseOnlyNumbers.CheckedChanged += new System.EventHandler(this.chk_SmUseOnlyNumbers_CheckedChanged);
            // 
            // chk_SmUseOnlyString
            // 
            this.chk_SmUseOnlyString.AutoSize = true;
            this.chk_SmUseOnlyString.BackColor = System.Drawing.Color.Transparent;
            this.chk_SmUseOnlyString.ForeColor = System.Drawing.Color.Black;
            this.chk_SmUseOnlyString.Location = new System.Drawing.Point(21, 21);
            this.chk_SmUseOnlyString.Name = "chk_SmUseOnlyString";
            this.chk_SmUseOnlyString.Size = new System.Drawing.Size(77, 17);
            this.chk_SmUseOnlyString.TabIndex = 0;
            this.chk_SmUseOnlyString.Text = "Only String";
            this.chk_SmUseOnlyString.UseVisualStyleBackColor = false;
            this.chk_SmUseOnlyString.CheckedChanged += new System.EventHandler(this.chk_SmUseOnlyString_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_AccountProfileDetails);
            this.groupBox3.Controls.Add(this.btn_AccountProfileImage);
            this.groupBox3.Controls.Add(this.lb_ImageFolder);
            this.groupBox3.Controls.Add(this.lb_ProfileDetailsFilePath);
            this.groupBox3.Controls.Add(this.txt_ImageFolderPath);
            this.groupBox3.Controls.Add(this.txt_ProfileDetailsFilePath);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 59);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(632, 74);
            this.groupBox3.TabIndex = 70;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Profile Details";
            // 
            // btn_AccountProfileDetails
            // 
            this.btn_AccountProfileDetails.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btn_AccountProfileDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AccountProfileDetails.Location = new System.Drawing.Point(503, 44);
            this.btn_AccountProfileDetails.Name = "btn_AccountProfileDetails";
            this.btn_AccountProfileDetails.Size = new System.Drawing.Size(77, 27);
            this.btn_AccountProfileDetails.TabIndex = 11;
            this.btn_AccountProfileDetails.UseVisualStyleBackColor = true;
            this.btn_AccountProfileDetails.Click += new System.EventHandler(this.btn_AccountProfileDetails_Click);
            // 
            // btn_AccountProfileImage
            // 
            this.btn_AccountProfileImage.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btn_AccountProfileImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AccountProfileImage.Location = new System.Drawing.Point(503, 16);
            this.btn_AccountProfileImage.Name = "btn_AccountProfileImage";
            this.btn_AccountProfileImage.Size = new System.Drawing.Size(77, 27);
            this.btn_AccountProfileImage.TabIndex = 10;
            this.btn_AccountProfileImage.UseVisualStyleBackColor = true;
            this.btn_AccountProfileImage.Click += new System.EventHandler(this.btn_AccountProfileImage_Click);
            // 
            // lb_ImageFolder
            // 
            this.lb_ImageFolder.AutoSize = true;
            this.lb_ImageFolder.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ImageFolder.Location = new System.Drawing.Point(6, 20);
            this.lb_ImageFolder.Name = "lb_ImageFolder";
            this.lb_ImageFolder.Size = new System.Drawing.Size(71, 12);
            this.lb_ImageFolder.TabIndex = 7;
            this.lb_ImageFolder.Text = "Image Folder";
            // 
            // lb_ProfileDetailsFilePath
            // 
            this.lb_ProfileDetailsFilePath.AutoSize = true;
            this.lb_ProfileDetailsFilePath.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ProfileDetailsFilePath.Location = new System.Drawing.Point(6, 46);
            this.lb_ProfileDetailsFilePath.Name = "lb_ProfileDetailsFilePath";
            this.lb_ProfileDetailsFilePath.Size = new System.Drawing.Size(110, 12);
            this.lb_ProfileDetailsFilePath.TabIndex = 9;
            this.lb_ProfileDetailsFilePath.Text = "Profile Details Folder";
            // 
            // txt_ImageFolderPath
            // 
            this.txt_ImageFolderPath.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ImageFolderPath.Location = new System.Drawing.Point(113, 20);
            this.txt_ImageFolderPath.Name = "txt_ImageFolderPath";
            this.txt_ImageFolderPath.Size = new System.Drawing.Size(384, 18);
            this.txt_ImageFolderPath.TabIndex = 6;
            // 
            // txt_ProfileDetailsFilePath
            // 
            this.txt_ProfileDetailsFilePath.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_ProfileDetailsFilePath.Location = new System.Drawing.Point(110, 46);
            this.txt_ProfileDetailsFilePath.Name = "txt_ProfileDetailsFilePath";
            this.txt_ProfileDetailsFilePath.Size = new System.Drawing.Size(387, 18);
            this.txt_ProfileDetailsFilePath.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_AccountDetails);
            this.groupBox2.Controls.Add(this.txt_AccountDetailsFilePath);
            this.groupBox2.Controls.Add(this.lb_AccountDetailsFolder);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(632, 53);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Account Details";
            // 
            // txt_AccountDetailsFilePath
            // 
            this.txt_AccountDetailsFilePath.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_AccountDetailsFilePath.Location = new System.Drawing.Point(113, 23);
            this.txt_AccountDetailsFilePath.Name = "txt_AccountDetailsFilePath";
            this.txt_AccountDetailsFilePath.Size = new System.Drawing.Size(384, 18);
            this.txt_AccountDetailsFilePath.TabIndex = 0;
            this.txt_AccountDetailsFilePath.TextChanged += new System.EventHandler(this.txt_AccountDetailsFilePath_TextChanged);
            // 
            // lb_AccountDetailsFolder
            // 
            this.lb_AccountDetailsFolder.AutoSize = true;
            this.lb_AccountDetailsFolder.Font = new System.Drawing.Font("Verdana", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_AccountDetailsFolder.Location = new System.Drawing.Point(6, 26);
            this.lb_AccountDetailsFolder.Name = "lb_AccountDetailsFolder";
            this.lb_AccountDetailsFolder.Size = new System.Drawing.Size(81, 12);
            this.lb_AccountDetailsFolder.TabIndex = 1;
            this.lb_AccountDetailsFolder.Text = "Account Folder";
            // 
            // btn_AccountDetails
            // 
            this.btn_AccountDetails.BackgroundImage = global::MixedCampaignManager.Properties.Resources.bt_Browse_campaign;
            this.btn_AccountDetails.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AccountDetails.Location = new System.Drawing.Point(503, 19);
            this.btn_AccountDetails.Name = "btn_AccountDetails";
            this.btn_AccountDetails.Size = new System.Drawing.Size(77, 27);
            this.btn_AccountDetails.TabIndex = 2;
            this.btn_AccountDetails.UseVisualStyleBackColor = true;
            this.btn_AccountDetails.Click += new System.EventHandler(this.btn_AccountDetails_Click);
            // 
            // AccountCreatorUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox65);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox25);
            this.Name = "AccountCreatorUserControl";
            this.Size = new System.Drawing.Size(786, 139);
            this.Load += new System.EventHandler(this.AccountCreatorUserControl_Load);
            this.groupBox65.ResumeLayout(false);
            this.groupBox65.PerformLayout();
            this.groupBox25.ResumeLayout(false);
            this.groupBox25.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox65;
        private System.Windows.Forms.CheckBox chk_SmProfileImage;
        private System.Windows.Forms.CheckBox chk_SmProfileUrl;
        private System.Windows.Forms.CheckBox chk_SmProfileDescription;
        private System.Windows.Forms.CheckBox chk_SmLocation;
        private System.Windows.Forms.GroupBox groupBox25;
        private System.Windows.Forms.CheckBox chk_SmUseOnlyBoth;
        private System.Windows.Forms.CheckBox chk_SmUseOnlyNumbers;
        private System.Windows.Forms.CheckBox chk_SmUseOnlyString;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_AccountProfileDetails;
        private System.Windows.Forms.Button btn_AccountProfileImage;
        private System.Windows.Forms.Label lb_ImageFolder;
        private System.Windows.Forms.Label lb_ProfileDetailsFilePath;
        private System.Windows.Forms.TextBox txt_ImageFolderPath;
        private System.Windows.Forms.TextBox txt_ProfileDetailsFilePath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_AccountDetails;
        private System.Windows.Forms.TextBox txt_AccountDetailsFilePath;
        private System.Windows.Forms.Label lb_AccountDetailsFolder;

    }
}
