namespace twtboardpro
{
    partial class frmReplyInterface
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReplyInterface));
            this.btnTweetAndReply = new System.Windows.Forms.Button();
            this.btnReadMessageFromDataBase = new System.Windows.Forms.Button();
            this.grdReplyInterface = new System.Windows.Forms.DataGridView();
            this.grbLogger = new System.Windows.Forms.GroupBox();
            this.lstReplyInterfaceLogger = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnReply = new System.Windows.Forms.Button();
            this.lstbMessages = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtEditMessage = new System.Windows.Forms.TextBox();
            this.btnBrowseLoadMsg = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ddlSelectMessage = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumberOfThreads = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClearDataBase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdReplyInterface)).BeginInit();
            this.grbLogger.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTweetAndReply
            // 
            this.btnTweetAndReply.BackColor = System.Drawing.Color.Transparent;
            this.btnTweetAndReply.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTweetAndReply.Location = new System.Drawing.Point(118, 64);
            this.btnTweetAndReply.Name = "btnTweetAndReply";
            this.btnTweetAndReply.Size = new System.Drawing.Size(144, 42);
            this.btnTweetAndReply.TabIndex = 1;
            this.btnTweetAndReply.Text = "Scrap Tweet&&Reply";
            this.btnTweetAndReply.UseVisualStyleBackColor = false;
            this.btnTweetAndReply.Click += new System.EventHandler(this.btnTweetAndReply_Click);
            // 
            // btnReadMessageFromDataBase
            // 
            this.btnReadMessageFromDataBase.BackColor = System.Drawing.Color.Transparent;
            this.btnReadMessageFromDataBase.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReadMessageFromDataBase.Location = new System.Drawing.Point(262, 64);
            this.btnReadMessageFromDataBase.Name = "btnReadMessageFromDataBase";
            this.btnReadMessageFromDataBase.Size = new System.Drawing.Size(144, 42);
            this.btnReadMessageFromDataBase.TabIndex = 5;
            this.btnReadMessageFromDataBase.Text = "Read Message from Database";
            this.btnReadMessageFromDataBase.UseVisualStyleBackColor = false;
            this.btnReadMessageFromDataBase.Click += new System.EventHandler(this.btnReadMessageFromDataBase_Click);
            // 
            // grdReplyInterface
            // 
            this.grdReplyInterface.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdReplyInterface.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            this.grdReplyInterface.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdReplyInterface.Location = new System.Drawing.Point(118, 112);
            this.grdReplyInterface.Name = "grdReplyInterface";
            this.grdReplyInterface.Size = new System.Drawing.Size(478, 127);
            this.grdReplyInterface.TabIndex = 10;
            this.grdReplyInterface.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdReplyInterface_CellContentClick);
            this.grdReplyInterface.Click += new System.EventHandler(this.grdReplyInterface_Click);
            this.grdReplyInterface.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdReplyInterface_MouseClick);
            // 
            // grbLogger
            // 
            this.grbLogger.BackColor = System.Drawing.Color.Transparent;
            this.grbLogger.Controls.Add(this.lstReplyInterfaceLogger);
            this.grbLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbLogger.Location = new System.Drawing.Point(12, 245);
            this.grbLogger.Name = "grbLogger";
            this.grbLogger.Size = new System.Drawing.Size(608, 212);
            this.grbLogger.TabIndex = 11;
            this.grbLogger.TabStop = false;
            this.grbLogger.Text = "Logger";
            // 
            // lstReplyInterfaceLogger
            // 
            this.lstReplyInterfaceLogger.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstReplyInterfaceLogger.FormattingEnabled = true;
            this.lstReplyInterfaceLogger.Location = new System.Drawing.Point(6, 19);
            this.lstReplyInterfaceLogger.Name = "lstReplyInterfaceLogger";
            this.lstReplyInterfaceLogger.ScrollAlwaysVisible = true;
            this.lstReplyInterfaceLogger.Size = new System.Drawing.Size(596, 173);
            this.lstReplyInterfaceLogger.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(658, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Edit Message";
            this.label8.Visible = false;
            // 
            // btnReply
            // 
            this.btnReply.BackColor = System.Drawing.Color.Transparent;
            this.btnReply.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReply.Location = new System.Drawing.Point(745, 449);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(90, 36);
            this.btnReply.TabIndex = 21;
            this.btnReply.Text = "Reply";
            this.btnReply.UseVisualStyleBackColor = false;
            this.btnReply.Visible = false;
            this.btnReply.Click += new System.EventHandler(this.btnReply_Click);
            // 
            // lstbMessages
            // 
            this.lstbMessages.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstbMessages.FormattingEnabled = true;
            this.lstbMessages.HorizontalScrollbar = true;
            this.lstbMessages.Location = new System.Drawing.Point(644, 27);
            this.lstbMessages.Name = "lstbMessages";
            this.lstbMessages.ScrollAlwaysVisible = true;
            this.lstbMessages.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstbMessages.Size = new System.Drawing.Size(427, 212);
            this.lstbMessages.TabIndex = 24;
            this.lstbMessages.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(641, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Messages";
            this.label7.Visible = false;
            // 
            // txtEditMessage
            // 
            this.txtEditMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEditMessage.Location = new System.Drawing.Point(745, 286);
            this.txtEditMessage.Multiline = true;
            this.txtEditMessage.Name = "txtEditMessage";
            this.txtEditMessage.Size = new System.Drawing.Size(326, 157);
            this.txtEditMessage.TabIndex = 20;
            this.txtEditMessage.Visible = false;
            // 
            // btnBrowseLoadMsg
            // 
            this.btnBrowseLoadMsg.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnBrowseLoadMsg.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseLoadMsg.Location = new System.Drawing.Point(523, 7);
            this.btnBrowseLoadMsg.Name = "btnBrowseLoadMsg";
            this.btnBrowseLoadMsg.Size = new System.Drawing.Size(48, 20);
            this.btnBrowseLoadMsg.TabIndex = 27;
            this.btnBrowseLoadMsg.Text = ".....";
            this.btnBrowseLoadMsg.UseVisualStyleBackColor = false;
            this.btnBrowseLoadMsg.Click += new System.EventHandler(this.btnBrowseLoadMsg_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(118, 9);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(386, 21);
            this.textBox1.TabIndex = 26;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(10, 12);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(87, 13);
            this.lblMessage.TabIndex = 25;
            this.lblMessage.Text = "Load Message";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(641, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 29;
            this.label6.Text = "Select Message";
            this.label6.Visible = false;
            // 
            // ddlSelectMessage
            // 
            this.ddlSelectMessage.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlSelectMessage.FormattingEnabled = true;
            this.ddlSelectMessage.Location = new System.Drawing.Point(742, 243);
            this.ddlSelectMessage.Name = "ddlSelectMessage";
            this.ddlSelectMessage.Size = new System.Drawing.Size(329, 21);
            this.ddlSelectMessage.TabIndex = 28;
            this.ddlSelectMessage.Text = "---Select---";
            this.ddlSelectMessage.Visible = false;
            this.ddlSelectMessage.SelectedIndexChanged += new System.EventHandler(this.ddlSelectMessage_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 52;
            this.label4.Text = "No. Of Threads";
            // 
            // txtNumberOfThreads
            // 
            this.txtNumberOfThreads.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumberOfThreads.Location = new System.Drawing.Point(118, 37);
            this.txtNumberOfThreads.Name = "txtNumberOfThreads";
            this.txtNumberOfThreads.Size = new System.Drawing.Size(72, 21);
            this.txtNumberOfThreads.TabIndex = 51;
            this.txtNumberOfThreads.Text = "5";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(207, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 13);
            this.label1.TabIndex = 53;
            this.label1.Text = "( Please Enter Numeric Value )";
            // 
            // btnClearDataBase
            // 
            this.btnClearDataBase.BackColor = System.Drawing.Color.Transparent;
            this.btnClearDataBase.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearDataBase.Location = new System.Drawing.Point(407, 64);
            this.btnClearDataBase.Name = "btnClearDataBase";
            this.btnClearDataBase.Size = new System.Drawing.Size(144, 42);
            this.btnClearDataBase.TabIndex = 54;
            this.btnClearDataBase.Text = "Clear DataBase";
            this.btnClearDataBase.UseVisualStyleBackColor = false;
            this.btnClearDataBase.Click += new System.EventHandler(this.btnClearDataBase_Click);
            // 
            // frmReplyInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::twtboardpro.Properties.Resources.app_bg;
            this.ClientSize = new System.Drawing.Size(1091, 488);
            this.Controls.Add(this.btnClearDataBase);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNumberOfThreads);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ddlSelectMessage);
            this.Controls.Add(this.btnBrowseLoadMsg);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnReply);
            this.Controls.Add(this.lstbMessages);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtEditMessage);
            this.Controls.Add(this.grbLogger);
            this.Controls.Add(this.grdReplyInterface);
            this.Controls.Add(this.btnReadMessageFromDataBase);
            this.Controls.Add(this.btnTweetAndReply);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmReplyInterface";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReplyInterface";
            this.Load += new System.EventHandler(this.frmReplyInterface_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdReplyInterface)).EndInit();
            this.grbLogger.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTweetAndReply;
        private System.Windows.Forms.Button btnReadMessageFromDataBase;
        private System.Windows.Forms.DataGridView grdReplyInterface;
        private System.Windows.Forms.GroupBox grbLogger;
        private System.Windows.Forms.ListBox lstReplyInterfaceLogger;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnReply;
        private System.Windows.Forms.ListBox lstbMessages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtEditMessage;
        private System.Windows.Forms.Button btnBrowseLoadMsg;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox ddlSelectMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNumberOfThreads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClearDataBase;

    }
}