namespace MixedCampaignManager
{
    partial class frm_mixcampaignmanager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_mixcampaignmanager));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grp_account = new System.Windows.Forms.GroupBox();
            this.lb_AccountFile = new System.Windows.Forms.Label();
            this.txt_accountfilepath = new System.Windows.Forms.TextBox();
            this.btn_uploadaccounts = new System.Windows.Forms.Button();
            this.grp_settings = new System.Windows.Forms.GroupBox();
            this.grp_logger = new System.Windows.Forms.GroupBox();
            this.campaignLogger = new System.Windows.Forms.ListBox();
            this.chklstbox_campaign = new System.Windows.Forms.CheckedListBox();
            this.grp_listofcampaign = new System.Windows.Forms.GroupBox();
            this.dgv_campaign = new System.Windows.Forms.DataGridView();
            this.CampaignName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FeaturName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CampaignEdit = new System.Windows.Forms.DataGridViewLinkColumn();
            this.BtnOnOff = new System.Windows.Forms.DataGridViewImageColumn();
            this.CampaignPauseStop = new System.Windows.Forms.DataGridViewButtonColumn();
            this.grp_featursettings = new System.Windows.Forms.GroupBox();
            this.groupBox45 = new System.Windows.Forms.GroupBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.txt_DelayFrom = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txt_DelayTo = new System.Windows.Forms.TextBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNoOfFollowThreads = new System.Windows.Forms.TextBox();
            this.grp_CmpName = new System.Windows.Forms.GroupBox();
            this.lb_CampaignName = new System.Windows.Forms.Label();
            this.txt_CampaignName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateTimePicker_Start = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_End = new System.Windows.Forms.DateTimePicker();
            this.chkbox_IsScheduledDaily = new System.Windows.Forms.CheckBox();
            this.lb_To = new System.Windows.Forms.Label();
            this.lb_ScheduleTime = new System.Windows.Forms.Label();
            this.groupBox40 = new System.Windows.Forms.GroupBox();
            this.gbCampaignSubmitAction = new System.Windows.Forms.GroupBox();
            this.btn_savecampaign = new System.Windows.Forms.Button();
            this.btn_UpdateCampaign = new System.Windows.Forms.Button();
            this.btn_Status = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.grp_account.SuspendLayout();
            this.grp_logger.SuspendLayout();
            this.grp_listofcampaign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).BeginInit();
            this.grp_featursettings.SuspendLayout();
            this.groupBox45.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.grp_CmpName.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox40.SuspendLayout();
            this.gbCampaignSubmitAction.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp_account
            // 
            this.grp_account.BackColor = System.Drawing.Color.Transparent;
            this.grp_account.Controls.Add(this.lb_AccountFile);
            this.grp_account.Controls.Add(this.txt_accountfilepath);
            this.grp_account.Controls.Add(this.btn_uploadaccounts);
            this.grp_account.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_account.Location = new System.Drawing.Point(541, 17);
            this.grp_account.Name = "grp_account";
            this.grp_account.Size = new System.Drawing.Size(494, 61);
            this.grp_account.TabIndex = 0;
            this.grp_account.TabStop = false;
            this.grp_account.Text = "Accounts";
            // 
            // lb_AccountFile
            // 
            this.lb_AccountFile.AutoSize = true;
            this.lb_AccountFile.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_AccountFile.Location = new System.Drawing.Point(13, 28);
            this.lb_AccountFile.Name = "lb_AccountFile";
            this.lb_AccountFile.Size = new System.Drawing.Size(80, 13);
            this.lb_AccountFile.TabIndex = 3;
            this.lb_AccountFile.Text = "Account File:";
            // 
            // txt_accountfilepath
            // 
            this.txt_accountfilepath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_accountfilepath.Location = new System.Drawing.Point(99, 25);
            this.txt_accountfilepath.Name = "txt_accountfilepath";
            this.txt_accountfilepath.ReadOnly = true;
            this.txt_accountfilepath.Size = new System.Drawing.Size(282, 21);
            this.txt_accountfilepath.TabIndex = 1;
            // 
            // btn_uploadaccounts
            // 
            this.btn_uploadaccounts.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_uploadaccounts.BackgroundImage")));
            this.btn_uploadaccounts.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_uploadaccounts.Location = new System.Drawing.Point(384, 22);
            this.btn_uploadaccounts.Name = "btn_uploadaccounts";
            this.btn_uploadaccounts.Size = new System.Drawing.Size(80, 27);
            this.btn_uploadaccounts.TabIndex = 0;
            this.btn_uploadaccounts.UseVisualStyleBackColor = true;
            this.btn_uploadaccounts.Click += new System.EventHandler(this.btn_uploadaccounts_Click);
            // 
            // grp_settings
            // 
            this.grp_settings.BackColor = System.Drawing.Color.Transparent;
            this.grp_settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_settings.Location = new System.Drawing.Point(10, 3);
            this.grp_settings.Name = "grp_settings";
            this.grp_settings.Size = new System.Drawing.Size(910, 169);
            this.grp_settings.TabIndex = 1;
            this.grp_settings.TabStop = false;
            this.grp_settings.Text = "Settings";
            // 
            // grp_logger
            // 
            this.grp_logger.BackColor = System.Drawing.Color.Transparent;
            this.grp_logger.Controls.Add(this.campaignLogger);
            this.grp_logger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_logger.Location = new System.Drawing.Point(126, 458);
            this.grp_logger.Name = "grp_logger";
            this.grp_logger.Size = new System.Drawing.Size(908, 113);
            this.grp_logger.TabIndex = 3;
            this.grp_logger.TabStop = false;
            this.grp_logger.Text = "Process Logger";
            // 
            // campaignLogger
            // 
            this.campaignLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.campaignLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.campaignLogger.FormattingEnabled = true;
            this.campaignLogger.HorizontalScrollbar = true;
            this.campaignLogger.Location = new System.Drawing.Point(3, 17);
            this.campaignLogger.Name = "campaignLogger";
            this.campaignLogger.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.campaignLogger.Size = new System.Drawing.Size(902, 93);
            this.campaignLogger.TabIndex = 0;
            this.campaignLogger.SelectedIndexChanged += new System.EventHandler(this.campaignLogger_SelectedIndexChanged);
            // 
            // chklstbox_campaign
            // 
            this.chklstbox_campaign.BackColor = System.Drawing.Color.Coral;
            this.chklstbox_campaign.CheckOnClick = true;
            this.chklstbox_campaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chklstbox_campaign.IntegralHeight = false;
            this.chklstbox_campaign.Items.AddRange(new object[] {
            "Follow",
            "Tweet",
            "Retweet",
            "Reply"});
            this.chklstbox_campaign.Location = new System.Drawing.Point(7, 20);
            this.chklstbox_campaign.Name = "chklstbox_campaign";
            this.chklstbox_campaign.Size = new System.Drawing.Size(78, 97);
            this.chklstbox_campaign.TabIndex = 2;
            this.chklstbox_campaign.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklstbox_campaign_ItemCheck);
            // 
            // grp_listofcampaign
            // 
            this.grp_listofcampaign.BackColor = System.Drawing.Color.Transparent;
            this.grp_listofcampaign.Controls.Add(this.dgv_campaign);
            this.grp_listofcampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_listofcampaign.Location = new System.Drawing.Point(126, 321);
            this.grp_listofcampaign.Name = "grp_listofcampaign";
            this.grp_listofcampaign.Size = new System.Drawing.Size(456, 131);
            this.grp_listofcampaign.TabIndex = 5;
            this.grp_listofcampaign.TabStop = false;
            this.grp_listofcampaign.Text = "Campaigns";
            // 
            // dgv_campaign
            // 
            this.dgv_campaign.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_campaign.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CampaignName,
            this.FeaturName,
            this.CampaignEdit,
            this.BtnOnOff,
            this.CampaignPauseStop});
            this.dgv_campaign.Location = new System.Drawing.Point(3, 18);
            this.dgv_campaign.Name = "dgv_campaign";
            this.dgv_campaign.ReadOnly = true;
            this.dgv_campaign.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_campaign.Size = new System.Drawing.Size(450, 100);
            this.dgv_campaign.TabIndex = 0;
            this.dgv_campaign.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_campaign_CellClick);
            this.dgv_campaign.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgv_campaign_UserDeletingRow);
            // 
            // CampaignName
            // 
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CampaignName.DefaultCellStyle = dataGridViewCellStyle7;
            this.CampaignName.Frozen = true;
            this.CampaignName.HeaderText = "Campaign Name";
            this.CampaignName.Name = "CampaignName";
            this.CampaignName.ReadOnly = true;
            this.CampaignName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CampaignName.Width = 134;
            // 
            // FeaturName
            // 
            this.FeaturName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FeaturName.DefaultCellStyle = dataGridViewCellStyle8;
            this.FeaturName.Frozen = true;
            this.FeaturName.HeaderText = "Feature Name";
            this.FeaturName.Name = "FeaturName";
            this.FeaturName.ReadOnly = true;
            this.FeaturName.Width = 96;
            // 
            // CampaignEdit
            // 
            this.CampaignEdit.HeaderText = "";
            this.CampaignEdit.Name = "CampaignEdit";
            this.CampaignEdit.ReadOnly = true;
            this.CampaignEdit.Text = "Edit";
            this.CampaignEdit.UseColumnTextForLinkValue = true;
            this.CampaignEdit.Width = 60;
            // 
            // BtnOnOff
            // 
            this.BtnOnOff.HeaderText = "";
            this.BtnOnOff.Image = global::MixedCampaignManager.Properties.Resources.on;
            this.BtnOnOff.Name = "BtnOnOff";
            this.BtnOnOff.ReadOnly = true;
            this.BtnOnOff.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BtnOnOff.Width = 40;
            // 
            // CampaignPauseStop
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Transparent;
            this.CampaignPauseStop.DefaultCellStyle = dataGridViewCellStyle9;
            this.CampaignPauseStop.HeaderText = "Pause";
            this.CampaignPauseStop.Name = "CampaignPauseStop";
            this.CampaignPauseStop.ReadOnly = true;
            this.CampaignPauseStop.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CampaignPauseStop.Text = "Pause";
            this.CampaignPauseStop.UseColumnTextForButtonValue = true;
            this.CampaignPauseStop.Width = 58;
            // 
            // grp_featursettings
            // 
            this.grp_featursettings.BackColor = System.Drawing.Color.Transparent;
            this.grp_featursettings.Controls.Add(this.chklstbox_campaign);
            this.grp_featursettings.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_featursettings.Location = new System.Drawing.Point(12, 14);
            this.grp_featursettings.Name = "grp_featursettings";
            this.grp_featursettings.Size = new System.Drawing.Size(98, 243);
            this.grp_featursettings.TabIndex = 6;
            this.grp_featursettings.TabStop = false;
            this.grp_featursettings.Text = "Features";
            // 
            // groupBox45
            // 
            this.groupBox45.BackColor = System.Drawing.Color.Transparent;
            this.groupBox45.Controls.Add(this.label30);
            this.groupBox45.Controls.Add(this.label29);
            this.groupBox45.Controls.Add(this.txt_DelayFrom);
            this.groupBox45.Controls.Add(this.label31);
            this.groupBox45.Controls.Add(this.txt_DelayTo);
            this.groupBox45.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox45.ForeColor = System.Drawing.Color.Black;
            this.groupBox45.Location = new System.Drawing.Point(680, 269);
            this.groupBox45.Name = "groupBox45";
            this.groupBox45.Size = new System.Drawing.Size(231, 51);
            this.groupBox45.TabIndex = 86;
            this.groupBox45.TabStop = false;
            this.groupBox45.Text = "Delay";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Location = new System.Drawing.Point(148, 23);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 13);
            this.label30.TabIndex = 55;
            this.label30.Text = "(In Seconds)";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(9, 23);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(40, 13);
            this.label29.TabIndex = 53;
            this.label29.Text = "Delay";
            // 
            // txt_DelayFrom
            // 
            this.txt_DelayFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DelayFrom.ForeColor = System.Drawing.Color.Black;
            this.txt_DelayFrom.Location = new System.Drawing.Point(52, 20);
            this.txt_DelayFrom.Name = "txt_DelayFrom";
            this.txt_DelayFrom.Size = new System.Drawing.Size(26, 21);
            this.txt_DelayFrom.TabIndex = 56;
            this.txt_DelayFrom.Text = "20";
            this.txt_DelayFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_DelayFrom.TextChanged += new System.EventHandler(this.txt_DelayFrom_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.ForeColor = System.Drawing.Color.Black;
            this.label31.Location = new System.Drawing.Point(86, 23);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(18, 13);
            this.label31.TabIndex = 57;
            this.label31.Text = "to";
            // 
            // txt_DelayTo
            // 
            this.txt_DelayTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_DelayTo.ForeColor = System.Drawing.Color.Black;
            this.txt_DelayTo.Location = new System.Drawing.Point(109, 20);
            this.txt_DelayTo.Name = "txt_DelayTo";
            this.txt_DelayTo.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.txt_DelayTo.Size = new System.Drawing.Size(27, 21);
            this.txt_DelayTo.TabIndex = 54;
            this.txt_DelayTo.Text = "25";
            this.txt_DelayTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_DelayTo.TextChanged += new System.EventHandler(this.txt_DelayTo_TextChanged);
            // 
            // groupBox12
            // 
            this.groupBox12.BackColor = System.Drawing.Color.Transparent;
            this.groupBox12.Controls.Add(this.label1);
            this.groupBox12.Controls.Add(this.txtNoOfFollowThreads);
            this.groupBox12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox12.ForeColor = System.Drawing.Color.Black;
            this.groupBox12.Location = new System.Drawing.Point(915, 269);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(119, 51);
            this.groupBox12.TabIndex = 85;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Thread Setting";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "Threads:";
            // 
            // txtNoOfFollowThreads
            // 
            this.txtNoOfFollowThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoOfFollowThreads.ForeColor = System.Drawing.Color.Black;
            this.txtNoOfFollowThreads.Location = new System.Drawing.Point(74, 17);
            this.txtNoOfFollowThreads.Name = "txtNoOfFollowThreads";
            this.txtNoOfFollowThreads.Size = new System.Drawing.Size(34, 21);
            this.txtNoOfFollowThreads.TabIndex = 47;
            this.txtNoOfFollowThreads.Text = "7";
            this.txtNoOfFollowThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNoOfFollowThreads.Validating += new System.ComponentModel.CancelEventHandler(this.txtNoOfFollowThreads_Validating);
            // 
            // grp_CmpName
            // 
            this.grp_CmpName.BackColor = System.Drawing.Color.Transparent;
            this.grp_CmpName.Controls.Add(this.lb_CampaignName);
            this.grp_CmpName.Controls.Add(this.txt_CampaignName);
            this.grp_CmpName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grp_CmpName.Location = new System.Drawing.Point(116, 17);
            this.grp_CmpName.Name = "grp_CmpName";
            this.grp_CmpName.Size = new System.Drawing.Size(419, 61);
            this.grp_CmpName.TabIndex = 10;
            this.grp_CmpName.TabStop = false;
            this.grp_CmpName.Text = "Campaign Name";
            // 
            // lb_CampaignName
            // 
            this.lb_CampaignName.AutoSize = true;
            this.lb_CampaignName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_CampaignName.Location = new System.Drawing.Point(8, 31);
            this.lb_CampaignName.Name = "lb_CampaignName";
            this.lb_CampaignName.Size = new System.Drawing.Size(107, 13);
            this.lb_CampaignName.TabIndex = 2;
            this.lb_CampaignName.Text = "Campaign Name:";
            // 
            // txt_CampaignName
            // 
            this.txt_CampaignName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_CampaignName.Location = new System.Drawing.Point(121, 28);
            this.txt_CampaignName.Name = "txt_CampaignName";
            this.txt_CampaignName.Size = new System.Drawing.Size(274, 21);
            this.txt_CampaignName.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.grp_settings);
            this.panel1.Location = new System.Drawing.Point(114, 84);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(928, 177);
            this.panel1.TabIndex = 4;
            // 
            // dateTimePicker_Start
            // 
            this.dateTimePicker_Start.CustomFormat = "";
            this.dateTimePicker_Start.Enabled = false;
            this.dateTimePicker_Start.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_Start.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_Start.Location = new System.Drawing.Point(106, 23);
            this.dateTimePicker_Start.Name = "dateTimePicker_Start";
            this.dateTimePicker_Start.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_Start.TabIndex = 0;
            // 
            // dateTimePicker_End
            // 
            this.dateTimePicker_End.Enabled = false;
            this.dateTimePicker_End.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker_End.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker_End.Location = new System.Drawing.Point(272, 21);
            this.dateTimePicker_End.Name = "dateTimePicker_End";
            this.dateTimePicker_End.Size = new System.Drawing.Size(123, 21);
            this.dateTimePicker_End.TabIndex = 2;
            // 
            // chkbox_IsScheduledDaily
            // 
            this.chkbox_IsScheduledDaily.AutoSize = true;
            this.chkbox_IsScheduledDaily.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbox_IsScheduledDaily.Location = new System.Drawing.Point(406, 21);
            this.chkbox_IsScheduledDaily.Name = "chkbox_IsScheduledDaily";
            this.chkbox_IsScheduledDaily.Size = new System.Drawing.Size(133, 17);
            this.chkbox_IsScheduledDaily.TabIndex = 3;
            this.chkbox_IsScheduledDaily.Text = "Is Scheduled Daily";
            this.chkbox_IsScheduledDaily.UseVisualStyleBackColor = true;
            this.chkbox_IsScheduledDaily.CheckedChanged += new System.EventHandler(this.chkbox_IsScheduledDaily_CheckedChanged);
            // 
            // lb_To
            // 
            this.lb_To.AutoSize = true;
            this.lb_To.Enabled = false;
            this.lb_To.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_To.Location = new System.Drawing.Point(240, 25);
            this.lb_To.Name = "lb_To";
            this.lb_To.Size = new System.Drawing.Size(28, 13);
            this.lb_To.TabIndex = 4;
            this.lb_To.Text = "TO:";
            // 
            // lb_ScheduleTime
            // 
            this.lb_ScheduleTime.AutoSize = true;
            this.lb_ScheduleTime.BackColor = System.Drawing.Color.Transparent;
            this.lb_ScheduleTime.Enabled = false;
            this.lb_ScheduleTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_ScheduleTime.ForeColor = System.Drawing.Color.Black;
            this.lb_ScheduleTime.Location = new System.Drawing.Point(8, 25);
            this.lb_ScheduleTime.Name = "lb_ScheduleTime";
            this.lb_ScheduleTime.Size = new System.Drawing.Size(96, 13);
            this.lb_ScheduleTime.TabIndex = 5;
            this.lb_ScheduleTime.Text = "Schedule Time:";
            // 
            // groupBox40
            // 
            this.groupBox40.BackColor = System.Drawing.Color.Transparent;
            this.groupBox40.Controls.Add(this.lb_ScheduleTime);
            this.groupBox40.Controls.Add(this.lb_To);
            this.groupBox40.Controls.Add(this.chkbox_IsScheduledDaily);
            this.groupBox40.Controls.Add(this.dateTimePicker_End);
            this.groupBox40.Controls.Add(this.dateTimePicker_Start);
            this.groupBox40.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox40.ForeColor = System.Drawing.Color.Black;
            this.groupBox40.Location = new System.Drawing.Point(124, 269);
            this.groupBox40.Name = "groupBox40";
            this.groupBox40.Size = new System.Drawing.Size(550, 51);
            this.groupBox40.TabIndex = 87;
            this.groupBox40.TabStop = false;
            this.groupBox40.Text = "Scheduling";
            // 
            // gbCampaignSubmitAction
            // 
            this.gbCampaignSubmitAction.BackColor = System.Drawing.Color.Transparent;
            this.gbCampaignSubmitAction.Controls.Add(this.btn_savecampaign);
            this.gbCampaignSubmitAction.Controls.Add(this.btn_UpdateCampaign);
            this.gbCampaignSubmitAction.Controls.Add(this.btn_Status);
            this.gbCampaignSubmitAction.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCampaignSubmitAction.Location = new System.Drawing.Point(588, 321);
            this.gbCampaignSubmitAction.Name = "gbCampaignSubmitAction";
            this.gbCampaignSubmitAction.Size = new System.Drawing.Size(448, 131);
            this.gbCampaignSubmitAction.TabIndex = 88;
            this.gbCampaignSubmitAction.TabStop = false;
            this.gbCampaignSubmitAction.Text = "Submit Action";
            // 
            // btn_savecampaign
            // 
            this.btn_savecampaign.BackColor = System.Drawing.Color.Transparent;
            this.btn_savecampaign.BackgroundImage = global::MixedCampaignManager.Properties.Resources.save_compaign;
            this.btn_savecampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_savecampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_savecampaign.Location = new System.Drawing.Point(10, 52);
            this.btn_savecampaign.Name = "btn_savecampaign";
            this.btn_savecampaign.Size = new System.Drawing.Size(128, 28);
            this.btn_savecampaign.TabIndex = 8;
            this.btn_savecampaign.UseVisualStyleBackColor = false;
            this.btn_savecampaign.Click += new System.EventHandler(this.btn_savecampaign_Click);
            // 
            // btn_UpdateCampaign
            // 
            this.btn_UpdateCampaign.BackgroundImage = global::MixedCampaignManager.Properties.Resources.update_compaign;
            this.btn_UpdateCampaign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_UpdateCampaign.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_UpdateCampaign.Location = new System.Drawing.Point(160, 52);
            this.btn_UpdateCampaign.Name = "btn_UpdateCampaign";
            this.btn_UpdateCampaign.Size = new System.Drawing.Size(128, 28);
            this.btn_UpdateCampaign.TabIndex = 9;
            this.btn_UpdateCampaign.UseVisualStyleBackColor = true;
            this.btn_UpdateCampaign.Click += new System.EventHandler(this.btn_UpdateCampaign_Click);
            // 
            // btn_Status
            // 
            this.btn_Status.BackgroundImage = global::MixedCampaignManager.Properties.Resources.show_all_status;
            this.btn_Status.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Status.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Status.Location = new System.Drawing.Point(310, 52);
            this.btn_Status.Name = "btn_Status";
            this.btn_Status.Size = new System.Drawing.Size(128, 28);
            this.btn_Status.TabIndex = 11;
            this.btn_Status.UseVisualStyleBackColor = true;
            this.btn_Status.Click += new System.EventHandler(this.btn_Status_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::MixedCampaignManager.Properties.Resources.on;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // frm_mixcampaignmanager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 575);
            this.Controls.Add(this.gbCampaignSubmitAction);
            this.Controls.Add(this.grp_logger);
            this.Controls.Add(this.groupBox45);
            this.Controls.Add(this.groupBox40);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grp_account);
            this.Controls.Add(this.grp_CmpName);
            this.Controls.Add(this.grp_listofcampaign);
            this.Controls.Add(this.grp_featursettings);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_mixcampaignmanager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Campaign Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frm_mixcampaignmanager_FormClosing);
            this.Load += new System.EventHandler(this.frm_mixcampaignmanager_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frm_mixcampaignmanager_Paint);
            this.grp_account.ResumeLayout(false);
            this.grp_account.PerformLayout();
            this.grp_logger.ResumeLayout(false);
            this.grp_listofcampaign.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_campaign)).EndInit();
            this.grp_featursettings.ResumeLayout(false);
            this.groupBox45.ResumeLayout(false);
            this.groupBox45.PerformLayout();
            this.groupBox12.ResumeLayout(false);
            this.groupBox12.PerformLayout();
            this.grp_CmpName.ResumeLayout(false);
            this.grp_CmpName.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox40.ResumeLayout(false);
            this.groupBox40.PerformLayout();
            this.gbCampaignSubmitAction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grp_account;
        private System.Windows.Forms.GroupBox grp_settings;
        private System.Windows.Forms.Button btn_uploadaccounts;
        private System.Windows.Forms.TextBox txt_accountfilepath;
        private System.Windows.Forms.GroupBox grp_logger;
        private System.Windows.Forms.ListBox campaignLogger;
        private System.Windows.Forms.CheckedListBox chklstbox_campaign;
        private System.Windows.Forms.GroupBox grp_listofcampaign;
        private System.Windows.Forms.GroupBox grp_featursettings;
        private System.Windows.Forms.GroupBox groupBox45;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txt_DelayFrom;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txt_DelayTo;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNoOfFollowThreads;
        private System.Windows.Forms.Button btn_savecampaign;
        private System.Windows.Forms.Button btn_UpdateCampaign;
        private System.Windows.Forms.Label lb_AccountFile;
        private System.Windows.Forms.GroupBox grp_CmpName;
        private System.Windows.Forms.Label lb_CampaignName;
        private System.Windows.Forms.TextBox txt_CampaignName;
        private System.Windows.Forms.DataGridView dgv_campaign;
        private System.Windows.Forms.Button btn_Status;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Start;
        private System.Windows.Forms.DateTimePicker dateTimePicker_End;
        private System.Windows.Forms.CheckBox chkbox_IsScheduledDaily;
        private System.Windows.Forms.Label lb_To;
        private System.Windows.Forms.Label lb_ScheduleTime;
        private System.Windows.Forms.GroupBox groupBox40;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.GroupBox gbCampaignSubmitAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn CampaignName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeaturName;
        private System.Windows.Forms.DataGridViewLinkColumn CampaignEdit;
        private System.Windows.Forms.DataGridViewImageColumn BtnOnOff;
        private System.Windows.Forms.DataGridViewButtonColumn CampaignPauseStop;

    }
}

