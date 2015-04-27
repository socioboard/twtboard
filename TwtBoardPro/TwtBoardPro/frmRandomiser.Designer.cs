namespace twtboardpro
{
    partial class frmRandomiser
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRandomiser));
            this.txtQuotes = new System.Windows.Forms.TextBox();
            this.lblQuotes = new System.Windows.Forms.Label();
            this.txtNormalTweets = new System.Windows.Forms.TextBox();
            this.lblNormalTweets = new System.Windows.Forms.Label();
            this.txtReTweets = new System.Windows.Forms.TextBox();
            this.lblReTweets = new System.Windows.Forms.Label();
            this.txtFakeReplies = new System.Windows.Forms.TextBox();
            this.lblFakeReplies = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnQuotes = new System.Windows.Forms.Button();
            this.btnUploadFollowUsername = new System.Windows.Forms.Button();
            this.txtUploadFollowUsername = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnLoadFakeScreenName = new System.Windows.Forms.Button();
            this.txtFakeScreenName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnLoadMentions = new System.Windows.Forms.Button();
            this.txtLoadMentions = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNoOfPostingFakeReplies = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNoOfPostingReTweets = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNoOfPostingNormalTweets = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNoOfPostingQuotes = new System.Windows.Forms.TextBox();
            this.btnLoadFakeReplies = new System.Windows.Forms.Button();
            this.btnLoadRetweets = new System.Windows.Forms.Button();
            this.btnNormalTweets = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAfterNoOfPosting = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNoOfPostingQuotes = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNumberOfThreads = new System.Windows.Forms.TextBox();
            this.chkTimeDelay = new System.Windows.Forms.CheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.txtMaxDelay = new System.Windows.Forms.TextBox();
            this.txtMinDelay = new System.Windows.Forms.TextBox();
            this.chkUseMention = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnGetMentions = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnClearDatabase = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstbRandomiserLogger = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtNoofFollowPerTime = new System.Windows.Forms.Label();
            this.txtMaxNoOfFollowPerTime = new System.Windows.Forms.TextBox();
            this.txt_MaximumFollow = new System.Windows.Forms.TextBox();
            this.chkUseFollowBySearchKeyword = new System.Windows.Forms.CheckBox();
            this.chkUseFollowSetting = new System.Windows.Forms.CheckBox();
            this.chkboxUseFollow = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtQuotes
            // 
            this.txtQuotes.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQuotes.Location = new System.Drawing.Point(183, 20);
            this.txtQuotes.Name = "txtQuotes";
            this.txtQuotes.ReadOnly = true;
            this.txtQuotes.Size = new System.Drawing.Size(509, 21);
            this.txtQuotes.TabIndex = 63;
            // 
            // lblQuotes
            // 
            this.lblQuotes.AutoSize = true;
            this.lblQuotes.BackColor = System.Drawing.Color.Transparent;
            this.lblQuotes.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuotes.Location = new System.Drawing.Point(7, 24);
            this.lblQuotes.Name = "lblQuotes";
            this.lblQuotes.Size = new System.Drawing.Size(83, 13);
            this.lblQuotes.TabIndex = 62;
            this.lblQuotes.Text = "Load Quotes:";
            // 
            // txtNormalTweets
            // 
            this.txtNormalTweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNormalTweets.Location = new System.Drawing.Point(183, 49);
            this.txtNormalTweets.Name = "txtNormalTweets";
            this.txtNormalTweets.ReadOnly = true;
            this.txtNormalTweets.Size = new System.Drawing.Size(509, 21);
            this.txtNormalTweets.TabIndex = 66;
            // 
            // lblNormalTweets
            // 
            this.lblNormalTweets.AutoSize = true;
            this.lblNormalTweets.BackColor = System.Drawing.Color.Transparent;
            this.lblNormalTweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNormalTweets.Location = new System.Drawing.Point(7, 51);
            this.lblNormalTweets.Name = "lblNormalTweets";
            this.lblNormalTweets.Size = new System.Drawing.Size(128, 13);
            this.lblNormalTweets.TabIndex = 65;
            this.lblNormalTweets.Text = "Load Normal Tweets:";
            // 
            // txtReTweets
            // 
            this.txtReTweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReTweets.Location = new System.Drawing.Point(183, 77);
            this.txtReTweets.Name = "txtReTweets";
            this.txtReTweets.ReadOnly = true;
            this.txtReTweets.Size = new System.Drawing.Size(509, 21);
            this.txtReTweets.TabIndex = 69;
            // 
            // lblReTweets
            // 
            this.lblReTweets.AutoSize = true;
            this.lblReTweets.BackColor = System.Drawing.Color.Transparent;
            this.lblReTweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReTweets.Location = new System.Drawing.Point(7, 80);
            this.lblReTweets.Name = "lblReTweets";
            this.lblReTweets.Size = new System.Drawing.Size(179, 13);
            this.lblReTweets.TabIndex = 68;
            this.lblReTweets.Text = "Load ReTweets Screen Name:";
            // 
            // txtFakeReplies
            // 
            this.txtFakeReplies.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFakeReplies.Location = new System.Drawing.Point(183, 140);
            this.txtFakeReplies.Name = "txtFakeReplies";
            this.txtFakeReplies.ReadOnly = true;
            this.txtFakeReplies.Size = new System.Drawing.Size(509, 21);
            this.txtFakeReplies.TabIndex = 72;
            // 
            // lblFakeReplies
            // 
            this.lblFakeReplies.AutoSize = true;
            this.lblFakeReplies.BackColor = System.Drawing.Color.Transparent;
            this.lblFakeReplies.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFakeReplies.Location = new System.Drawing.Point(7, 145);
            this.lblFakeReplies.Name = "lblFakeReplies";
            this.lblFakeReplies.Size = new System.Drawing.Size(115, 13);
            this.lblFakeReplies.TabIndex = 71;
            this.lblFakeReplies.Text = "Load Fake Replies:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btnQuotes);
            this.groupBox1.Controls.Add(this.btnUploadFollowUsername);
            this.groupBox1.Controls.Add(this.txtUploadFollowUsername);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.btnLoadFakeScreenName);
            this.groupBox1.Controls.Add(this.txtFakeScreenName);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnLoadMentions);
            this.groupBox1.Controls.Add(this.txtLoadMentions);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtNoOfPostingFakeReplies);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNoOfPostingReTweets);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtNoOfPostingNormalTweets);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtNoOfPostingQuotes);
            this.groupBox1.Controls.Add(this.txtQuotes);
            this.groupBox1.Controls.Add(this.btnLoadFakeReplies);
            this.groupBox1.Controls.Add(this.lblQuotes);
            this.groupBox1.Controls.Add(this.txtFakeReplies);
            this.groupBox1.Controls.Add(this.lblFakeReplies);
            this.groupBox1.Controls.Add(this.lblNormalTweets);
            this.groupBox1.Controls.Add(this.btnLoadRetweets);
            this.groupBox1.Controls.Add(this.txtNormalTweets);
            this.groupBox1.Controls.Add(this.txtReTweets);
            this.groupBox1.Controls.Add(this.btnNormalTweets);
            this.groupBox1.Controls.Add(this.lblReTweets);
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(845, 235);
            this.groupBox1.TabIndex = 74;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Randomiser Files";
            // 
            // btnQuotes
            // 
            this.btnQuotes.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnQuotes.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnQuotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnQuotes.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuotes.Location = new System.Drawing.Point(698, 16);
            this.btnQuotes.Name = "btnQuotes";
            this.btnQuotes.Size = new System.Drawing.Size(80, 27);
            this.btnQuotes.TabIndex = 64;
            this.btnQuotes.UseVisualStyleBackColor = false;
            this.btnQuotes.Click += new System.EventHandler(this.btnQuotes_Click);
            // 
            // btnUploadFollowUsername
            // 
            this.btnUploadFollowUsername.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnUploadFollowUsername.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnUploadFollowUsername.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnUploadFollowUsername.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadFollowUsername.Location = new System.Drawing.Point(698, 200);
            this.btnUploadFollowUsername.Name = "btnUploadFollowUsername";
            this.btnUploadFollowUsername.Size = new System.Drawing.Size(80, 27);
            this.btnUploadFollowUsername.TabIndex = 97;
            this.btnUploadFollowUsername.UseVisualStyleBackColor = false;
            this.btnUploadFollowUsername.Click += new System.EventHandler(this.btnUploadFollowUsername_Click);
            // 
            // txtUploadFollowUsername
            // 
            this.txtUploadFollowUsername.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUploadFollowUsername.Location = new System.Drawing.Point(183, 203);
            this.txtUploadFollowUsername.Name = "txtUploadFollowUsername";
            this.txtUploadFollowUsername.ReadOnly = true;
            this.txtUploadFollowUsername.Size = new System.Drawing.Size(509, 21);
            this.txtUploadFollowUsername.TabIndex = 96;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(7, 206);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 13);
            this.label15.TabIndex = 95;
            this.label15.Text = "Load Username:";
            // 
            // btnLoadFakeScreenName
            // 
            this.btnLoadFakeScreenName.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnLoadFakeScreenName.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnLoadFakeScreenName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadFakeScreenName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFakeScreenName.Location = new System.Drawing.Point(698, 106);
            this.btnLoadFakeScreenName.Name = "btnLoadFakeScreenName";
            this.btnLoadFakeScreenName.Size = new System.Drawing.Size(80, 26);
            this.btnLoadFakeScreenName.TabIndex = 94;
            this.btnLoadFakeScreenName.UseVisualStyleBackColor = false;
            this.btnLoadFakeScreenName.Click += new System.EventHandler(this.btnLoadFakeScreenName_Click);
            // 
            // txtFakeScreenName
            // 
            this.txtFakeScreenName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFakeScreenName.Location = new System.Drawing.Point(183, 107);
            this.txtFakeScreenName.Name = "txtFakeScreenName";
            this.txtFakeScreenName.ReadOnly = true;
            this.txtFakeScreenName.Size = new System.Drawing.Size(509, 21);
            this.txtFakeScreenName.TabIndex = 93;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(7, 112);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(151, 13);
            this.label13.TabIndex = 92;
            this.label13.Text = "Load Fake Screen Name:";
            // 
            // btnLoadMentions
            // 
            this.btnLoadMentions.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnLoadMentions.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnLoadMentions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadMentions.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadMentions.Location = new System.Drawing.Point(698, 170);
            this.btnLoadMentions.Name = "btnLoadMentions";
            this.btnLoadMentions.Size = new System.Drawing.Size(80, 26);
            this.btnLoadMentions.TabIndex = 88;
            this.btnLoadMentions.UseVisualStyleBackColor = false;
            this.btnLoadMentions.Click += new System.EventHandler(this.btnLoadMentions_Click);
            // 
            // txtLoadMentions
            // 
            this.txtLoadMentions.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoadMentions.Location = new System.Drawing.Point(183, 172);
            this.txtLoadMentions.Name = "txtLoadMentions";
            this.txtLoadMentions.ReadOnly = true;
            this.txtLoadMentions.Size = new System.Drawing.Size(509, 21);
            this.txtLoadMentions.TabIndex = 87;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(7, 175);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 86;
            this.label12.Text = "Load Mentions:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(948, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(205, 15);
            this.label6.TabIndex = 85;
            this.label6.Text = "( Please Enter Numeric Value )";
            this.label6.Visible = false;
            // 
            // txtNoOfPostingFakeReplies
            // 
            this.txtNoOfPostingFakeReplies.Location = new System.Drawing.Point(842, 148);
            this.txtNoOfPostingFakeReplies.Name = "txtNoOfPostingFakeReplies";
            this.txtNoOfPostingFakeReplies.Size = new System.Drawing.Size(100, 21);
            this.txtNoOfPostingFakeReplies.TabIndex = 84;
            this.txtNoOfPostingFakeReplies.Text = "5";
            this.txtNoOfPostingFakeReplies.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(948, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 15);
            this.label4.TabIndex = 82;
            this.label4.Text = "( Please Enter Numeric Value )";
            this.label4.Visible = false;
            // 
            // txtNoOfPostingReTweets
            // 
            this.txtNoOfPostingReTweets.Location = new System.Drawing.Point(842, 79);
            this.txtNoOfPostingReTweets.Name = "txtNoOfPostingReTweets";
            this.txtNoOfPostingReTweets.Size = new System.Drawing.Size(100, 21);
            this.txtNoOfPostingReTweets.TabIndex = 81;
            this.txtNoOfPostingReTweets.Text = "5";
            this.txtNoOfPostingReTweets.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(948, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 15);
            this.label1.TabIndex = 79;
            this.label1.Text = "( Please Enter Numeric Value )";
            this.label1.Visible = false;
            // 
            // txtNoOfPostingNormalTweets
            // 
            this.txtNoOfPostingNormalTweets.Location = new System.Drawing.Point(842, 49);
            this.txtNoOfPostingNormalTweets.Name = "txtNoOfPostingNormalTweets";
            this.txtNoOfPostingNormalTweets.Size = new System.Drawing.Size(100, 21);
            this.txtNoOfPostingNormalTweets.TabIndex = 78;
            this.txtNoOfPostingNormalTweets.Text = "5";
            this.txtNoOfPostingNormalTweets.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(948, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 15);
            this.label2.TabIndex = 76;
            this.label2.Text = "( Please Enter Numeric Value )";
            this.label2.Visible = false;
            // 
            // txtNoOfPostingQuotes
            // 
            this.txtNoOfPostingQuotes.Location = new System.Drawing.Point(842, 19);
            this.txtNoOfPostingQuotes.Name = "txtNoOfPostingQuotes";
            this.txtNoOfPostingQuotes.Size = new System.Drawing.Size(100, 21);
            this.txtNoOfPostingQuotes.TabIndex = 75;
            this.txtNoOfPostingQuotes.Text = "5";
            this.txtNoOfPostingQuotes.Visible = false;
            // 
            // btnLoadFakeReplies
            // 
            this.btnLoadFakeReplies.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnLoadFakeReplies.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnLoadFakeReplies.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadFakeReplies.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFakeReplies.Location = new System.Drawing.Point(698, 137);
            this.btnLoadFakeReplies.Name = "btnLoadFakeReplies";
            this.btnLoadFakeReplies.Size = new System.Drawing.Size(80, 27);
            this.btnLoadFakeReplies.TabIndex = 73;
            this.btnLoadFakeReplies.UseVisualStyleBackColor = false;
            this.btnLoadFakeReplies.Click += new System.EventHandler(this.btnLoadFakeReplies_Click);
            // 
            // btnLoadRetweets
            // 
            this.btnLoadRetweets.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnLoadRetweets.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnLoadRetweets.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLoadRetweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadRetweets.Location = new System.Drawing.Point(698, 76);
            this.btnLoadRetweets.Name = "btnLoadRetweets";
            this.btnLoadRetweets.Size = new System.Drawing.Size(80, 26);
            this.btnLoadRetweets.TabIndex = 70;
            this.btnLoadRetweets.UseVisualStyleBackColor = false;
            this.btnLoadRetweets.Click += new System.EventHandler(this.btnLoadRetweets_Click);
            // 
            // btnNormalTweets
            // 
            this.btnNormalTweets.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnNormalTweets.BackgroundImage = global::twtboardpro.Properties.Resources.Browse;
            this.btnNormalTweets.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNormalTweets.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNormalTweets.Location = new System.Drawing.Point(698, 47);
            this.btnNormalTweets.Name = "btnNormalTweets";
            this.btnNormalTweets.Size = new System.Drawing.Size(80, 26);
            this.btnNormalTweets.TabIndex = 67;
            this.btnNormalTweets.UseVisualStyleBackColor = false;
            this.btnNormalTweets.Click += new System.EventHandler(this.btnNormalTweets_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(239, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 13);
            this.label10.TabIndex = 91;
            this.label10.Text = "( Enter Number Only )";
            // 
            // txtAfterNoOfPosting
            // 
            this.txtAfterNoOfPosting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAfterNoOfPosting.Location = new System.Drawing.Point(182, 41);
            this.txtAfterNoOfPosting.Name = "txtAfterNoOfPosting";
            this.txtAfterNoOfPosting.Size = new System.Drawing.Size(40, 21);
            this.txtAfterNoOfPosting.TabIndex = 90;
            this.txtAfterNoOfPosting.Text = "5";
            this.txtAfterNoOfPosting.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 43);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(148, 13);
            this.label11.TabIndex = 89;
            this.label11.Text = "Start After No Of Posting";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(513, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 15);
            this.label7.TabIndex = 83;
            this.label7.Text = "No Of Posting FakeReplies";
            this.label7.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(501, -65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 15);
            this.label5.TabIndex = 80;
            this.label5.Text = "No Of Posting ReTweets";
            this.label5.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(501, -95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 15);
            this.label3.TabIndex = 77;
            this.label3.Text = "No Of Posting Normal Tweets";
            this.label3.Visible = false;
            // 
            // lblNoOfPostingQuotes
            // 
            this.lblNoOfPostingQuotes.AutoSize = true;
            this.lblNoOfPostingQuotes.BackColor = System.Drawing.Color.Transparent;
            this.lblNoOfPostingQuotes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.95F, System.Drawing.FontStyle.Bold);
            this.lblNoOfPostingQuotes.Location = new System.Drawing.Point(501, -124);
            this.lblNoOfPostingQuotes.Name = "lblNoOfPostingQuotes";
            this.lblNoOfPostingQuotes.Size = new System.Drawing.Size(144, 15);
            this.lblNoOfPostingQuotes.TabIndex = 74;
            this.lblNoOfPostingQuotes.Text = "No Of Posting Quotes";
            this.lblNoOfPostingQuotes.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.lblNoOfPostingQuotes);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtNumberOfThreads);
            this.groupBox2.Controls.Add(this.chkTimeDelay);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.label25);
            this.groupBox2.Controls.Add(this.txtMaxDelay);
            this.groupBox2.Controls.Add(this.txtMinDelay);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(12, 308);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(845, 56);
            this.groupBox2.TabIndex = 75;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Time Delay && Threads";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(620, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(212, 21);
            this.label8.TabIndex = 84;
            this.label8.Text = "( Please Enter Numeric Value )";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(466, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 83;
            this.label9.Text = "No. Of Threads";
            // 
            // txtNumberOfThreads
            // 
            this.txtNumberOfThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberOfThreads.Location = new System.Drawing.Point(575, 22);
            this.txtNumberOfThreads.Name = "txtNumberOfThreads";
            this.txtNumberOfThreads.Size = new System.Drawing.Size(39, 21);
            this.txtNumberOfThreads.TabIndex = 82;
            this.txtNumberOfThreads.Text = "10";
            this.txtNumberOfThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkTimeDelay
            // 
            this.chkTimeDelay.AutoSize = true;
            this.chkTimeDelay.Checked = true;
            this.chkTimeDelay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTimeDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTimeDelay.Location = new System.Drawing.Point(10, 25);
            this.chkTimeDelay.Name = "chkTimeDelay";
            this.chkTimeDelay.Size = new System.Drawing.Size(91, 17);
            this.chkTimeDelay.TabIndex = 81;
            this.chkTimeDelay.Text = "Time Delay";
            this.chkTimeDelay.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(258, 23);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(208, 21);
            this.label24.TabIndex = 80;
            this.label24.Text = "( Random Delay in Minutes )";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(180, 25);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(18, 13);
            this.label25.TabIndex = 79;
            this.label25.Text = "to";
            // 
            // txtMaxDelay
            // 
            this.txtMaxDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxDelay.Location = new System.Drawing.Point(219, 23);
            this.txtMaxDelay.Name = "txtMaxDelay";
            this.txtMaxDelay.Size = new System.Drawing.Size(33, 21);
            this.txtMaxDelay.TabIndex = 78;
            this.txtMaxDelay.Text = "60";
            this.txtMaxDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMinDelay
            // 
            this.txtMinDelay.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMinDelay.Location = new System.Drawing.Point(132, 23);
            this.txtMinDelay.Name = "txtMinDelay";
            this.txtMinDelay.Size = new System.Drawing.Size(35, 21);
            this.txtMinDelay.TabIndex = 77;
            this.txtMinDelay.Text = "45";
            this.txtMinDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkUseMention
            // 
            this.chkUseMention.AutoSize = true;
            this.chkUseMention.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseMention.Location = new System.Drawing.Point(9, 21);
            this.chkUseMention.Name = "chkUseMention";
            this.chkUseMention.Size = new System.Drawing.Size(101, 17);
            this.chkUseMention.TabIndex = 85;
            this.chkUseMention.Text = "Use Mentions";
            this.chkUseMention.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnGetMentions);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.btnClearDatabase);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(404, 369);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(453, 72);
            this.groupBox3.TabIndex = 76;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Actions";
            // 
            // btnGetMentions
            // 
            this.btnGetMentions.BackgroundImage = global::twtboardpro.Properties.Resources.Get_Mentions;
            this.btnGetMentions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGetMentions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetMentions.Location = new System.Drawing.Point(329, 22);
            this.btnGetMentions.Name = "btnGetMentions";
            this.btnGetMentions.Size = new System.Drawing.Size(109, 30);
            this.btnGetMentions.TabIndex = 3;
            this.btnGetMentions.UseVisualStyleBackColor = true;
            this.btnGetMentions.Click += new System.EventHandler(this.btnGetMentions_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImage = global::twtboardpro.Properties.Resources.btn_stop;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStop.Location = new System.Drawing.Point(100, 22);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 29);
            this.btnStop.TabIndex = 2;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnClearDatabase
            // 
            this.btnClearDatabase.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClearDatabase.BackgroundImage")));
            this.btnClearDatabase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnClearDatabase.Location = new System.Drawing.Point(190, 22);
            this.btnClearDatabase.Name = "btnClearDatabase";
            this.btnClearDatabase.Size = new System.Drawing.Size(133, 29);
            this.btnClearDatabase.TabIndex = 1;
            this.btnClearDatabase.UseVisualStyleBackColor = true;
            this.btnClearDatabase.Click += new System.EventHandler(this.btnClearDatabase_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackgroundImage = global::twtboardpro.Properties.Resources.start;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnStart.Location = new System.Drawing.Point(12, 22);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 29);
            this.btnStart.TabIndex = 0;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.lstbRandomiserLogger);
            this.groupBox4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(12, 453);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(845, 171);
            this.groupBox4.TabIndex = 77;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Logger";
            // 
            // lstbRandomiserLogger
            // 
            this.lstbRandomiserLogger.ContextMenuStrip = this.contextMenuStrip1;
            this.lstbRandomiserLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbRandomiserLogger.FormattingEnabled = true;
            this.lstbRandomiserLogger.Location = new System.Drawing.Point(6, 14);
            this.lstbRandomiserLogger.Name = "lstbRandomiserLogger";
            this.lstbRandomiserLogger.ScrollAlwaysVisible = true;
            this.lstbRandomiserLogger.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstbRandomiserLogger.Size = new System.Drawing.Size(834, 147);
            this.lstbRandomiserLogger.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.chkUseMention);
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.txtAfterNoOfPosting);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(12, 368);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(386, 71);
            this.groupBox5.TabIndex = 87;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Other Options";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.txtNoofFollowPerTime);
            this.groupBox6.Controls.Add(this.txtMaxNoOfFollowPerTime);
            this.groupBox6.Controls.Add(this.txt_MaximumFollow);
            this.groupBox6.Controls.Add(this.chkUseFollowBySearchKeyword);
            this.groupBox6.Controls.Add(this.chkUseFollowSetting);
            this.groupBox6.Controls.Add(this.chkboxUseFollow);
            this.groupBox6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.Location = new System.Drawing.Point(12, 241);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(845, 61);
            this.groupBox6.TabIndex = 88;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Follow";
            // 
            // txtNoofFollowPerTime
            // 
            this.txtNoofFollowPerTime.AutoSize = true;
            this.txtNoofFollowPerTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoofFollowPerTime.Location = new System.Drawing.Point(538, 30);
            this.txtNoofFollowPerTime.Name = "txtNoofFollowPerTime";
            this.txtNoofFollowPerTime.Size = new System.Drawing.Size(133, 13);
            this.txtNoofFollowPerTime.TabIndex = 93;
            this.txtNoofFollowPerTime.Text = "No Of Follow Per Time";
            // 
            // txtMaxNoOfFollowPerTime
            // 
            this.txtMaxNoOfFollowPerTime.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMaxNoOfFollowPerTime.Location = new System.Drawing.Point(698, 26);
            this.txtMaxNoOfFollowPerTime.Name = "txtMaxNoOfFollowPerTime";
            this.txtMaxNoOfFollowPerTime.Size = new System.Drawing.Size(44, 21);
            this.txtMaxNoOfFollowPerTime.TabIndex = 92;
            this.txtMaxNoOfFollowPerTime.Text = "2";
            this.txtMaxNoOfFollowPerTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txt_MaximumFollow
            // 
            this.txt_MaximumFollow.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.txt_MaximumFollow.Location = new System.Drawing.Point(960, 24);
            this.txt_MaximumFollow.Name = "txt_MaximumFollow";
            this.txt_MaximumFollow.Size = new System.Drawing.Size(44, 22);
            this.txt_MaximumFollow.TabIndex = 90;
            this.txt_MaximumFollow.Text = "10";
            // 
            // chkUseFollowBySearchKeyword
            // 
            this.chkUseFollowBySearchKeyword.AutoSize = true;
            this.chkUseFollowBySearchKeyword.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseFollowBySearchKeyword.Location = new System.Drawing.Point(309, 28);
            this.chkUseFollowBySearchKeyword.Name = "chkUseFollowBySearchKeyword";
            this.chkUseFollowBySearchKeyword.Size = new System.Drawing.Size(203, 17);
            this.chkUseFollowBySearchKeyword.TabIndex = 89;
            this.chkUseFollowBySearchKeyword.Text = "Use Follow By Search Keyword";
            this.chkUseFollowBySearchKeyword.UseVisualStyleBackColor = true;
            this.chkUseFollowBySearchKeyword.Visible = false;
            this.chkUseFollowBySearchKeyword.CheckedChanged += new System.EventHandler(this.chkUseFollowBySearchKeyword_CheckedChanged);
            // 
            // chkUseFollowSetting
            // 
            this.chkUseFollowSetting.AutoSize = true;
            this.chkUseFollowSetting.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkUseFollowSetting.Location = new System.Drawing.Point(160, 27);
            this.chkUseFollowSetting.Name = "chkUseFollowSetting";
            this.chkUseFollowSetting.Size = new System.Drawing.Size(130, 17);
            this.chkUseFollowSetting.TabIndex = 88;
            this.chkUseFollowSetting.Text = "Use Follow Setting";
            this.chkUseFollowSetting.UseVisualStyleBackColor = true;
            this.chkUseFollowSetting.CheckedChanged += new System.EventHandler(this.chkUseFollowSetting_CheckedChanged);
            // 
            // chkboxUseFollow
            // 
            this.chkboxUseFollow.AutoSize = true;
            this.chkboxUseFollow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkboxUseFollow.Location = new System.Drawing.Point(51, 27);
            this.chkboxUseFollow.Name = "chkboxUseFollow";
            this.chkboxUseFollow.Size = new System.Drawing.Size(86, 17);
            this.chkboxUseFollow.TabIndex = 87;
            this.chkboxUseFollow.Text = "Use Follow";
            this.chkboxUseFollow.UseVisualStyleBackColor = true;
            // 
            // frmRandomiser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 630);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmRandomiser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Randomiser";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRandomiser_FormClosed);
            this.Load += new System.EventHandler(this.frmRandomiser_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmRandomiser_Paint);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnQuotes;
        private System.Windows.Forms.TextBox txtQuotes;
        private System.Windows.Forms.Label lblQuotes;
        private System.Windows.Forms.Button btnNormalTweets;
        private System.Windows.Forms.TextBox txtNormalTweets;
        private System.Windows.Forms.Label lblNormalTweets;
        private System.Windows.Forms.Button btnLoadRetweets;
        private System.Windows.Forms.TextBox txtReTweets;
        private System.Windows.Forms.Label lblReTweets;
        private System.Windows.Forms.Button btnLoadFakeReplies;
        private System.Windows.Forms.TextBox txtFakeReplies;
        private System.Windows.Forms.Label lblFakeReplies;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNoOfPostingFakeReplies;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNoOfPostingReTweets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNoOfPostingNormalTweets;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNoOfPostingQuotes;
        private System.Windows.Forms.Label lblNoOfPostingQuotes;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtNumberOfThreads;
        private System.Windows.Forms.CheckBox chkTimeDelay;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtMaxDelay;
        private System.Windows.Forms.TextBox txtMinDelay;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lstbRandomiserLogger;
        private System.Windows.Forms.Button btnClearDatabase;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAfterNoOfPosting;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnLoadMentions;
        private System.Windows.Forms.TextBox txtLoadMentions;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnLoadFakeScreenName;
        private System.Windows.Forms.TextBox txtFakeScreenName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnGetMentions;
        private System.Windows.Forms.CheckBox chkUseMention;
        private System.Windows.Forms.Button btnUploadFollowUsername;
        private System.Windows.Forms.TextBox txtUploadFollowUsername;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox chkUseFollowBySearchKeyword;
        private System.Windows.Forms.CheckBox chkUseFollowSetting;
        private System.Windows.Forms.CheckBox chkboxUseFollow;
        private System.Windows.Forms.TextBox txt_MaximumFollow;
        private System.Windows.Forms.TextBox txtMaxNoOfFollowPerTime;
        private System.Windows.Forms.Label txtNoofFollowPerTime;
    }
}