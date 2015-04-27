namespace Randomiser
{
    partial class frmUseFollowBySearchKeyword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUseFollowBySearchKeyword));
            this.groupBox63 = new System.Windows.Forms.GroupBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.btn_FollowKeyWordsFileUpload = new System.Windows.Forms.Button();
            this.chk_followbysinglekeywordperaccount = new System.Windows.Forms.CheckBox();
            this.txt_FollowByPerAccount = new System.Windows.Forms.TextBox();
            this.lb_FollowByPerAccount = new System.Windows.Forms.Label();
            this.txt_FollowBySearchKey = new System.Windows.Forms.TextBox();
            this.groupBox63.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox63
            // 
            this.groupBox63.Controls.Add(this.btnSet);
            this.groupBox63.Controls.Add(this.btn_FollowKeyWordsFileUpload);
            this.groupBox63.Controls.Add(this.chk_followbysinglekeywordperaccount);
            this.groupBox63.Controls.Add(this.txt_FollowByPerAccount);
            this.groupBox63.Controls.Add(this.lb_FollowByPerAccount);
            this.groupBox63.Controls.Add(this.txt_FollowBySearchKey);
            this.groupBox63.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox63.Location = new System.Drawing.Point(19, 21);
            this.groupBox63.Name = "groupBox63";
            this.groupBox63.Size = new System.Drawing.Size(398, 155);
            this.groupBox63.TabIndex = 1;
            this.groupBox63.TabStop = false;
            this.groupBox63.Text = "Follow By Search keyWords";
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.Color.Transparent;
            this.btnSet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSet.Location = new System.Drawing.Point(260, 91);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(114, 25);
            this.btnSet.TabIndex = 8;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btn_FollowKeyWordsFileUpload
            // 
            this.btn_FollowKeyWordsFileUpload.BackColor = System.Drawing.Color.Transparent;
            this.btn_FollowKeyWordsFileUpload.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_FollowKeyWordsFileUpload.BackgroundImage")));
            this.btn_FollowKeyWordsFileUpload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_FollowKeyWordsFileUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_FollowKeyWordsFileUpload.Location = new System.Drawing.Point(260, 42);
            this.btn_FollowKeyWordsFileUpload.Name = "btn_FollowKeyWordsFileUpload";
            this.btn_FollowKeyWordsFileUpload.Size = new System.Drawing.Size(114, 25);
            this.btn_FollowKeyWordsFileUpload.TabIndex = 7;
            this.btn_FollowKeyWordsFileUpload.UseVisualStyleBackColor = false;
            this.btn_FollowKeyWordsFileUpload.Click += new System.EventHandler(this.btn_FollowKeyWordsFileUpload_Click);
            // 
            // chk_followbysinglekeywordperaccount
            // 
            this.chk_followbysinglekeywordperaccount.AutoSize = true;
            this.chk_followbysinglekeywordperaccount.Location = new System.Drawing.Point(12, 128);
            this.chk_followbysinglekeywordperaccount.Name = "chk_followbysinglekeywordperaccount";
            this.chk_followbysinglekeywordperaccount.Size = new System.Drawing.Size(148, 20);
            this.chk_followbysinglekeywordperaccount.TabIndex = 6;
            this.chk_followbysinglekeywordperaccount.Text = "Follow all Keywords ";
            this.chk_followbysinglekeywordperaccount.UseVisualStyleBackColor = true;
            // 
            // txt_FollowByPerAccount
            // 
            this.txt_FollowByPerAccount.Location = new System.Drawing.Point(216, 93);
            this.txt_FollowByPerAccount.Name = "txt_FollowByPerAccount";
            this.txt_FollowByPerAccount.Size = new System.Drawing.Size(37, 22);
            this.txt_FollowByPerAccount.TabIndex = 4;
            this.txt_FollowByPerAccount.Text = "5";
            // 
            // lb_FollowByPerAccount
            // 
            this.lb_FollowByPerAccount.AutoSize = true;
            this.lb_FollowByPerAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_FollowByPerAccount.Location = new System.Drawing.Point(9, 95);
            this.lb_FollowByPerAccount.Name = "lb_FollowByPerAccount";
            this.lb_FollowByPerAccount.Size = new System.Drawing.Size(141, 16);
            this.lb_FollowByPerAccount.TabIndex = 3;
            this.lb_FollowByPerAccount.Text = "Follow By Per Account";
            // 
            // txt_FollowBySearchKey
            // 
            this.txt_FollowBySearchKey.Location = new System.Drawing.Point(12, 42);
            this.txt_FollowBySearchKey.Name = "txt_FollowBySearchKey";
            this.txt_FollowBySearchKey.ReadOnly = true;
            this.txt_FollowBySearchKey.Size = new System.Drawing.Size(242, 22);
            this.txt_FollowBySearchKey.TabIndex = 0;
            // 
            // frmUseFollowBySearchKeyword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 262);
            this.Controls.Add(this.groupBox63);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUseFollowBySearchKeyword";
            this.Text = "frmUseFollowBySearchKeyword";
            this.Load += new System.EventHandler(this.frmUseFollowBySearchKeyword_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmUseFollowBySearchKeyword_Paint);
            this.groupBox63.ResumeLayout(false);
            this.groupBox63.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox63;
        private System.Windows.Forms.CheckBox chk_followbysinglekeywordperaccount;
        private System.Windows.Forms.TextBox txt_FollowByPerAccount;
        private System.Windows.Forms.Label lb_FollowByPerAccount;
        private System.Windows.Forms.TextBox txt_FollowBySearchKey;
        private System.Windows.Forms.Button btn_FollowKeyWordsFileUpload;
        private System.Windows.Forms.Button btnSet;
    }
}