namespace MixedCampaignManager
{
    partial class frm_Status
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Status));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdobtn_SearchAllReport = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rdobtn_SearchReportDateTimeName = new System.Windows.Forms.RadioButton();
            this.rdobtn_SearchReportFollowerName = new System.Windows.Forms.RadioButton();
            this.rdobtn_SearchReportUserName = new System.Windows.Forms.RadioButton();
            this.rdobtn_SearchReportCampName = new System.Windows.Forms.RadioButton();
            this.Btn_Search = new System.Windows.Forms.Button();
            this.txt_searchingKeyWord = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.grpResult.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(979, 220);
            this.dataGridView1.TabIndex = 0;
            // 
            // grpResult
            // 
            this.grpResult.BackColor = System.Drawing.Color.Transparent;
            this.grpResult.Controls.Add(this.dataGridView1);
            this.grpResult.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpResult.Location = new System.Drawing.Point(12, 118);
            this.grpResult.Name = "grpResult";
            this.grpResult.Size = new System.Drawing.Size(998, 247);
            this.grpResult.TabIndex = 1;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Result";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.rdobtn_SearchAllReport);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rdobtn_SearchReportDateTimeName);
            this.groupBox2.Controls.Add(this.rdobtn_SearchReportFollowerName);
            this.groupBox2.Controls.Add(this.rdobtn_SearchReportUserName);
            this.groupBox2.Controls.Add(this.rdobtn_SearchReportCampName);
            this.groupBox2.Controls.Add(this.Btn_Search);
            this.groupBox2.Controls.Add(this.txt_searchingKeyWord);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(998, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Searching option";
            // 
            // rdobtn_SearchAllReport
            // 
            this.rdobtn_SearchAllReport.AutoSize = true;
            this.rdobtn_SearchAllReport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtn_SearchAllReport.Location = new System.Drawing.Point(590, 20);
            this.rdobtn_SearchAllReport.Name = "rdobtn_SearchAllReport";
            this.rdobtn_SearchAllReport.Size = new System.Drawing.Size(43, 17);
            this.rdobtn_SearchAllReport.TabIndex = 9;
            this.rdobtn_SearchAllReport.TabStop = true;
            this.rdobtn_SearchAllReport.Text = "All ";
            this.rdobtn_SearchAllReport.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Searching Value :";
            // 
            // rdobtn_SearchReportDateTimeName
            // 
            this.rdobtn_SearchReportDateTimeName.AutoSize = true;
            this.rdobtn_SearchReportDateTimeName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtn_SearchReportDateTimeName.Location = new System.Drawing.Point(739, 56);
            this.rdobtn_SearchReportDateTimeName.Name = "rdobtn_SearchReportDateTimeName";
            this.rdobtn_SearchReportDateTimeName.Size = new System.Drawing.Size(84, 17);
            this.rdobtn_SearchReportDateTimeName.TabIndex = 5;
            this.rdobtn_SearchReportDateTimeName.TabStop = true;
            this.rdobtn_SearchReportDateTimeName.Text = "Date Time";
            this.rdobtn_SearchReportDateTimeName.UseVisualStyleBackColor = true;
            // 
            // rdobtn_SearchReportFollowerName
            // 
            this.rdobtn_SearchReportFollowerName.AutoSize = true;
            this.rdobtn_SearchReportFollowerName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtn_SearchReportFollowerName.Location = new System.Drawing.Point(590, 56);
            this.rdobtn_SearchReportFollowerName.Name = "rdobtn_SearchReportFollowerName";
            this.rdobtn_SearchReportFollowerName.Size = new System.Drawing.Size(108, 17);
            this.rdobtn_SearchReportFollowerName.TabIndex = 4;
            this.rdobtn_SearchReportFollowerName.TabStop = true;
            this.rdobtn_SearchReportFollowerName.Text = "Follower name";
            this.rdobtn_SearchReportFollowerName.UseVisualStyleBackColor = true;
            // 
            // rdobtn_SearchReportUserName
            // 
            this.rdobtn_SearchReportUserName.AutoSize = true;
            this.rdobtn_SearchReportUserName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtn_SearchReportUserName.Location = new System.Drawing.Point(888, 20);
            this.rdobtn_SearchReportUserName.Name = "rdobtn_SearchReportUserName";
            this.rdobtn_SearchReportUserName.Size = new System.Drawing.Size(87, 17);
            this.rdobtn_SearchReportUserName.TabIndex = 3;
            this.rdobtn_SearchReportUserName.TabStop = true;
            this.rdobtn_SearchReportUserName.Text = "User name";
            this.rdobtn_SearchReportUserName.UseVisualStyleBackColor = true;
            // 
            // rdobtn_SearchReportCampName
            // 
            this.rdobtn_SearchReportCampName.AutoSize = true;
            this.rdobtn_SearchReportCampName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdobtn_SearchReportCampName.Location = new System.Drawing.Point(739, 20);
            this.rdobtn_SearchReportCampName.Name = "rdobtn_SearchReportCampName";
            this.rdobtn_SearchReportCampName.Size = new System.Drawing.Size(123, 17);
            this.rdobtn_SearchReportCampName.TabIndex = 2;
            this.rdobtn_SearchReportCampName.TabStop = true;
            this.rdobtn_SearchReportCampName.Text = "Campaign name ";
            this.rdobtn_SearchReportCampName.UseVisualStyleBackColor = true;
            // 
            // Btn_Search
            // 
            this.Btn_Search.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Btn_Search.BackgroundImage")));
            this.Btn_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Btn_Search.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Btn_Search.Location = new System.Drawing.Point(406, 41);
            this.Btn_Search.Name = "Btn_Search";
            this.Btn_Search.Size = new System.Drawing.Size(119, 28);
            this.Btn_Search.TabIndex = 1;
            this.Btn_Search.UseVisualStyleBackColor = true;
            this.Btn_Search.Click += new System.EventHandler(this.Btn_Search_Click);
            // 
            // txt_searchingKeyWord
            // 
            this.txt_searchingKeyWord.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_searchingKeyWord.Location = new System.Drawing.Point(135, 44);
            this.txt_searchingKeyWord.Name = "txt_searchingKeyWord";
            this.txt_searchingKeyWord.Size = new System.Drawing.Size(265, 21);
            this.txt_searchingKeyWord.TabIndex = 0;
            // 
            // frm_Status
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 376);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_Status";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Status";
            this.Load += new System.EventHandler(this.frm_Status_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frm_Status_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.grpResult.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox grpResult;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdobtn_SearchReportDateTimeName;
        private System.Windows.Forms.RadioButton rdobtn_SearchReportFollowerName;
        private System.Windows.Forms.RadioButton rdobtn_SearchReportUserName;
        private System.Windows.Forms.RadioButton rdobtn_SearchReportCampName;
        private System.Windows.Forms.Button Btn_Search;
        private System.Windows.Forms.TextBox txt_searchingKeyWord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdobtn_SearchAllReport;
    }
}