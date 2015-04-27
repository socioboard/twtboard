namespace twtboardpro
{
    partial class frmScheduler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduler));
            this.dgvScheduler = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnstartScheduler = new System.Windows.Forms.Button();
            this.btnRemoveAccomplishedTasks = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduler)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvScheduler
            // 
            this.dgvScheduler.BackgroundColor = System.Drawing.Color.AliceBlue;
            this.dgvScheduler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScheduler.Location = new System.Drawing.Point(12, 66);
            this.dgvScheduler.Name = "dgvScheduler";
            this.dgvScheduler.ReadOnly = true;
            this.dgvScheduler.Size = new System.Drawing.Size(908, 266);
            this.dgvScheduler.TabIndex = 0;
            this.dgvScheduler.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScheduler_CellContentClick);
            this.dgvScheduler.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvScheduler_UserDeletingRow);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(31, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Total Tasks in Queue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(248, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tasks Executed Successfully";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(527, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pending Tasks";
            // 
            // btnstartScheduler
            // 
            this.btnstartScheduler.BackColor = System.Drawing.Color.Transparent;
            this.btnstartScheduler.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnstartScheduler.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnstartScheduler.Location = new System.Drawing.Point(337, 353);
            this.btnstartScheduler.Name = "btnstartScheduler";
            this.btnstartScheduler.Size = new System.Drawing.Size(91, 30);
            this.btnstartScheduler.TabIndex = 4;
            this.btnstartScheduler.UseVisualStyleBackColor = false;
            this.btnstartScheduler.Click += new System.EventHandler(this.btnstartScheduler_Click);
            // 
            // btnRemoveAccomplishedTasks
            // 
            this.btnRemoveAccomplishedTasks.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveAccomplishedTasks.BackgroundImage = global::twtboardpro.Properties.Resources.Remove_Accomplished_task;
            this.btnRemoveAccomplishedTasks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRemoveAccomplishedTasks.Location = new System.Drawing.Point(434, 354);
            this.btnRemoveAccomplishedTasks.Name = "btnRemoveAccomplishedTasks";
            this.btnRemoveAccomplishedTasks.Size = new System.Drawing.Size(191, 29);
            this.btnRemoveAccomplishedTasks.TabIndex = 5;
            this.btnRemoveAccomplishedTasks.UseVisualStyleBackColor = false;
            this.btnRemoveAccomplishedTasks.Click += new System.EventHandler(this.btnRemoveAccomplishedTasks_Click);
            // 
            // frmScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(932, 408);
            this.Controls.Add(this.btnRemoveAccomplishedTasks);
            this.Controls.Add(this.btnstartScheduler);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvScheduler);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmScheduler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "twtboardpro Scheduled Tasks";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmScheduler_FormClosed);
            this.Load += new System.EventHandler(this.frmScheduler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvScheduler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvScheduler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnstartScheduler;
        private System.Windows.Forms.Button btnRemoveAccomplishedTasks;
    }
}