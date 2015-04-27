namespace twtboardpro
{
    partial class FrmStartCampaign
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStartCampaign));
            this.dgvCampaigndata = new System.Windows.Forms.DataGridView();
            this.btnStartCampagin = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaigndata)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvCampaigndata
            // 
            this.dgvCampaigndata.BackgroundColor = System.Drawing.Color.LightBlue;
            this.dgvCampaigndata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCampaigndata.Location = new System.Drawing.Point(8, 9);
            this.dgvCampaigndata.Name = "dgvCampaigndata";
            this.dgvCampaigndata.ReadOnly = true;
            this.dgvCampaigndata.Size = new System.Drawing.Size(676, 347);
            this.dgvCampaigndata.TabIndex = 0;
            // 
            // btnStartCampagin
            // 
            this.btnStartCampagin.BackColor = System.Drawing.Color.Transparent;
            this.btnStartCampagin.BackgroundImage = global::twtboardpro.Properties.Resources.start_campaign;
            this.btnStartCampagin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStartCampagin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartCampagin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCampagin.Location = new System.Drawing.Point(280, 371);
            this.btnStartCampagin.Name = "btnStartCampagin";
            this.btnStartCampagin.Size = new System.Drawing.Size(137, 38);
            this.btnStartCampagin.TabIndex = 5;
            this.btnStartCampagin.UseVisualStyleBackColor = false;
            this.btnStartCampagin.Click += new System.EventHandler(this.btnStartCampagin_Click);
            // 
            // FrmStartCampaign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(696, 421);
            this.Controls.Add(this.btnStartCampagin);
            this.Controls.Add(this.dgvCampaigndata);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmStartCampaign";
            this.Text = "FrmStartCampaign";
            this.Load += new System.EventHandler(this.FrmStartCampaign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCampaigndata)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCampaigndata;
        private System.Windows.Forms.Button btnStartCampagin;
    }
}