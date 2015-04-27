using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using BaseLib;
using ReplyInterface;
using Globussoft;
using System.Text.RegularExpressions;
using System.IO;

namespace twtboardpro
{
    //public partial class frmReplyInterface : Form
    //{
    //    clsDB_ReplyInterface obj_clsDB_ReplyInterface = new clsDB_ReplyInterface();
    //    Tweeter.Tweeter obj_Tweeter = new Tweeter.Tweeter();

    //    string userName = string.Empty;
    //    string statusId = string.Empty;
    //    string PostAuthenticityToken = string.Empty;
    //    string message = string.Empty;
    //    string screenName = string.Empty;
    //    string userDisplayName = string.Empty;
    //    int numberOfThreads = 7;
        
    //    public frmReplyInterface()
    //    {
    //        InitializeComponent();
    //    }

    //    private void btnTweetAndReply_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            try
    //            {
    //                numberOfThreads = Convert.ToInt32(txtNumberOfThreads.Text);
    //            }
    //            catch
    //            {
    //                MessageBox.Show("Please Enter Numeric Value !");
    //                return;
    //            }
    //            Thread Thread_ScrapTweetAndReply = new Thread(Start_ScrapTweetAndReply);
    //            Thread_ScrapTweetAndReply.Start();
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void Start_ScrapTweetAndReply()
    //    {
    //        try
    //        {
    //            //numberOfThreads = 7;
    //            //int numberOfUnfollows = 5;
    //            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
    //            {
    //                ThreadPool.SetMaxThreads(numberOfThreads, 5);

    //                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
    //                {

    //                    try
    //                    {
    //                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

    //                        ThreadPool.QueueUserWorkItem(new WaitCallback(Star_ScrapTweetAndReplyMultithreaded), new object[] { item, "" });

    //                        Thread.Sleep(1000);
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                MessageBox.Show("Please Upload The Account !"); 
    //            }

               
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void Star_ScrapTweetAndReplyMultithreaded(object parameters)
    //    {
    //        try
    //        {
    //            Array paramsArray = new object[2];

    //            paramsArray = (Array)parameters;

    //            KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

               
    //            List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

    //            TweetAccountManager tweetAccountManager = keyValue.Value;

    //            //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
    //            tweetAccountManager.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);

    //            if (!tweetAccountManager.IsLoggedIn)
    //            {
    //                tweetAccountManager.Login();
    //            }

                

    //            tweetAccountManager.ScrapTweetAndReply();

    //            tweetAccountManager.logEvents.addToLogger -= ReplyInterface_AddToLogger;
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void frmReplyInterface_Load(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            ReplyInterface.ReplyInterface.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void ReplyInterface_AddToLogger(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            if (e is EventsArgs)
    //            {
    //                EventsArgs eventArgs = e as EventsArgs;
    //                AddToReplyInterface(eventArgs.log);
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void AddToReplyInterface(string log)
    //    {

    //        try
    //        {
    //            if (lstReplyInterfaceLogger.InvokeRequired)
    //            {
    //                lstReplyInterfaceLogger.Invoke(new MethodInvoker(delegate
    //                {
    //                    lstReplyInterfaceLogger.Items.Add(log);
    //                    lstReplyInterfaceLogger.SelectedIndex = lstReplyInterfaceLogger.Items.Count - 1;
    //                }));
    //            }
    //            else
    //            {
    //                lstReplyInterfaceLogger.Items.Add(log);
    //                lstReplyInterfaceLogger.SelectedIndex = lstReplyInterfaceLogger.Items.Count - 1;
    //            }
    //        }
    //        catch { }

    //    }

    //    private void btnReadMessageFromDataBase_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            Thread obj_Thread = new Thread(BindData);
    //            obj_Thread.Start();
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void BindData()
    //    {
    //        try
    //        {
    //            DataTable datatable = CreateTable();

    //            DataSet ds=obj_clsDB_ReplyInterface.SelectDistinctAllFromtb_ReplyCampaign();
    //            DataTable dt=ds.Tables["tb_ReplyCampaign"];

    //            if (dt.Rows.Count > 0)
    //            {
    //                foreach (DataRow item in dt.Rows)
    //                {
    //                    try
    //                    {
    //                       // if (item["UserId"].ToString() != item["TweetUserId"].ToString())
    //                        {

    //                            DataRow dr = datatable.NewRow();

    //                            dr["StatusId"] = StringEncoderDecoder.Decode(item["StatusId"].ToString());
    //                            dr["PostAuthenticityToken"] = StringEncoderDecoder.Decode(item["PostAuthenticityToken"].ToString());
    //                            dr["UserId"] = StringEncoderDecoder.Decode(item["UserId"].ToString());
    //                            dr["UserName"] = StringEncoderDecoder.Decode(item["UserName"].ToString());
    //                            dr["ScreenName"] = StringEncoderDecoder.Decode(item["ScreenName"].ToString());
    //                            dr["TweetUserId"] = StringEncoderDecoder.Decode(item["TweetUserId"].ToString());
    //                            dr["TweetUserName"] = StringEncoderDecoder.Decode(item["TweetUserName"].ToString());
    //                            dr["ReplyUserId"] = StringEncoderDecoder.Decode(item["ReplyUserId"].ToString());
    //                            dr["ReplyUserName"] = StringEncoderDecoder.Decode(item["ReplyUserName"].ToString());
    //                            dr["TweetText"] = StringEncoderDecoder.Decode(item["TweetText"].ToString());
    //                            dr["Reply"] = StringEncoderDecoder.Decode(item["Reply"].ToString());
    //                            //dr["Status"] = StringEncoderDecoder.Decode(item["Status"].ToString());

    //                            datatable.Rows.Add(dr);
    //                        }


    //                    }
    //                    catch
    //                    {
    //                    }
    //                }

    //                if (datatable.Rows.Count > 0)
    //                {
    //                    try
    //                    {

    //                        grdReplyInterface.Invoke(new MethodInvoker(delegate
    //                            {

    //                                grdReplyInterface.DataSource = datatable;

    //                                //grdReplyInterface.Columns["UserName"].Visible = false;
    //                                grdReplyInterface.Columns["StatusId"].Visible = true;
    //                                grdReplyInterface.Columns["PostAuthenticityToken"].Visible = false;
    //                                grdReplyInterface.Columns["UserId"].Visible = false;
    //                                grdReplyInterface.Columns["ScreenName"].Visible = false;
    //                                //grdReplyInterface.Columns["StatusId"].Visible = false;
    //                                grdReplyInterface.Columns["TweetUserId"].Visible = false;
    //                                //grdReplyInterface.Columns["TweetUserName"].Visible = false;
    //                                grdReplyInterface.Columns["ReplyUserId"].Visible = false;
    //                                //grdReplyInterface.Columns["ReplyUserName"].Visible = false;
    //                                grdReplyInterface.Columns["TweetText"].Visible = false;
    //                                grdReplyInterface.Columns["Reply"].Visible = false;
    //                                grdReplyInterface.Columns["Status"].Visible = false;

    //                                grdReplyInterface.Refresh();
    //                                grdReplyInterface.RefreshEdit();
                                 
    //                            }));
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }

    //                else
    //                {
    //                    AddToReplyInterface("There Is No New Tweet !");
    //                }
    //            }
    //            else
    //            {
    //                AddToReplyInterface("There Is No Records In DataBase !");
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    public DataTable CreateTable()
    //    {
    //        DataTable dt_details = new DataTable();
    //        try
    //        {

    //            DataColumn col_StatusId = new DataColumn("StatusId");
    //            col_StatusId.ReadOnly = true;
    //            DataColumn col_PostAuthenticityToken = new DataColumn("PostAuthenticityToken");
    //            col_PostAuthenticityToken.ReadOnly = true;
    //            DataColumn col_UserId = new DataColumn("UserId");
    //            col_UserId.ReadOnly = true;
    //            DataColumn col_UserName = new DataColumn("UserName");
    //            col_UserName.ReadOnly = true;
    //            DataColumn col_ScreenName = new DataColumn("ScreenName");
    //            col_ScreenName.ReadOnly = true;
    //            DataColumn col_TweetUserId = new DataColumn("TweetUserId");
    //            col_TweetUserId.ReadOnly = true;
    //            DataColumn col_TweetUserName = new DataColumn("TweetUserName");
    //            col_TweetUserName.ReadOnly = true;
    //            DataColumn col_ReplyUserId = new DataColumn("ReplyUserId");
    //            col_ReplyUserId.ReadOnly = true;
    //            DataColumn col_ReplyUserName = new DataColumn("ReplyUserName");
    //            col_ReplyUserName.ReadOnly = true;
    //            DataColumn col_TweetText = new DataColumn("TweetText");
    //            col_TweetText.ReadOnly = true;
    //            DataColumn col_Reply = new DataColumn("Reply");
    //            col_Reply.ReadOnly = true;
    //            DataColumn col_Status = new DataColumn("Status");
    //            col_Status.ReadOnly = true;

    //            dt_details.Columns.Add(col_StatusId);
    //            dt_details.Columns.Add(col_PostAuthenticityToken);
    //            dt_details.Columns.Add(col_UserId);
    //            dt_details.Columns.Add(col_UserName);
    //            dt_details.Columns.Add(col_ScreenName);
    //            dt_details.Columns.Add(col_TweetUserId);
    //            dt_details.Columns.Add(col_TweetUserName);
    //            dt_details.Columns.Add(col_ReplyUserId);
    //            dt_details.Columns.Add(col_ReplyUserName);
    //            dt_details.Columns.Add(col_TweetText);
    //            dt_details.Columns.Add(col_Reply);
    //            dt_details.Columns.Add(col_Status);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.StackTrace);
    //        }

    //        return dt_details;
    //    }

    //    private void grdReplyInterface_Click(object sender, EventArgs e)
    //    {

    //    }

    //    private void grdReplyInterface_MouseClick(object sender, MouseEventArgs e)
    //    {
    //        try
    //        {


    //            label6.Visible = true;
    //            label7.Visible = true;
    //            label8.Visible = true;
    //            lstbMessages.Visible = true;
    //            txtEditMessage.Visible = true;
    //            btnReply.Visible = true;
    //            ddlSelectMessage.Visible = true;

    //            lstbMessages.Items.Clear();

    //            int rowIndex = grdReplyInterface.CurrentCell.RowIndex;

    //            statusId = grdReplyInterface.Rows[rowIndex].Cells[0].Value.ToString();
    //            PostAuthenticityToken = grdReplyInterface.Rows[rowIndex].Cells[1].Value.ToString();
    //            userName = grdReplyInterface.Rows[rowIndex].Cells[3].Value.ToString();
    //            userDisplayName = grdReplyInterface.Rows[rowIndex].Cells[6].Value.ToString();
    //            screenName = grdReplyInterface.Rows[rowIndex].Cells[8].Value.ToString();

    //            DataSet ds=obj_clsDB_ReplyInterface.SelectTweetAndReplyFromtb_ReplyCampaign(statusId, userName);

    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                bool isDispalyTweet = false;

    //                foreach (DataRow item in ds.Tables[0].Rows)
    //                {
    //                         try
    //                         {
    //                             if (!isDispalyTweet)
    //                             {
    //                                 if (!string.IsNullOrEmpty(item["TweetText"].ToString()) && !string.IsNullOrWhiteSpace(item["TweetText"].ToString()))
    //                                 {
    //                                     lstbMessages.Items.Add("Tweets ==>  " + item["TweetText"].ToString());
    //                                     lstbMessages.Items.Add(Environment.NewLine);

    //                                     isDispalyTweet = true;
    //                                 }
    //                             }

    //                            if (!string.IsNullOrEmpty(item["Reply"].ToString()) && !string.IsNullOrWhiteSpace(item["Reply"].ToString()))
    //                            {
    //                                lstbMessages.Items.Add("Reply ==>  " + item["Reply"].ToString());
    //                                lstbMessages.Items.Add(Environment.NewLine);
    //                            }
    //                         }
    //                         catch
    //                         {
    //                         }
                    
    //                }
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void grdReplyInterface_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //    {

    //    }

    //    private void btnReply_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            if (!string.IsNullOrEmpty(txtEditMessage.Text) && !string.IsNullOrEmpty(txtEditMessage.Text))
    //            {
    //                message = txtEditMessage.Text;
    //            }
    //            else
    //            {
    //                MessageBox.Show("Please Enter The Message In Edit Message Box !");
    //            }

    //            Thread Reply_Thread = new Thread(Reply);
    //            Reply_Thread.Start();
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void Reply()
    //    {
    //        try
    //        {
    //            int numberOfThreads = 7;
                
    //            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
    //            {
    //                ThreadPool.SetMaxThreads(numberOfThreads, 5);

    //                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
    //                {

    //                    try
    //                    {
    //                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

    //                        ThreadPool.QueueUserWorkItem(new WaitCallback(Start_ReplyMultithreaded), new object[] { item, "" });

    //                        Thread.Sleep(1000);
    //                    }
    //                    catch
    //                    {
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                MessageBox.Show("Please Upload The Account !");
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void Start_ReplyMultithreaded(object parameters)
    //    {
    //        try
    //        {
    //            Array paramsArray = new object[2];

    //            paramsArray = (Array)parameters;

    //            KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


    //            List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

    //            TweetAccountManager tweetAccountManager = keyValue.Value;

    //            //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
    //            tweetAccountManager.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);

    //            if (!tweetAccountManager.IsLoggedIn)
    //            {
    //                tweetAccountManager.Login();
    //            }

               

    //            tweetAccountManager.ReplyThroughReplyInterface(PostAuthenticityToken, statusId, userDisplayName ,screenName, message, userName);

    //            tweetAccountManager.logEvents.addToLogger -= ReplyInterface_AddToLogger;
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    List<string> lstMessage;
    //    private void button3_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            using (OpenFileDialog ofd = new OpenFileDialog())
    //            {
    //                ofd.Filter = "Text Files (*.txt)|*.txt";
    //                ofd.InitialDirectory = Application.StartupPath;
    //                if (ofd.ShowDialog() == DialogResult.OK)
    //                {
    //                    textBox1.Text = ofd.FileName;
    //                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
    //                    templist = templist.Distinct().ToList();
    //                    ///Modified [1.0.0.3]
    //                    lstMessage = new List<string>();

    //                    foreach (string item in templist)
    //                    {
    //                        //if (!lstMessage.Contains(item))
    //                        {
    //                            lstMessage.Add(item);
    //                        }
    //                    }

    //                    //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
    //                    AddToReplyInterface(lstMessage.Count + " Message Loaded !");
    //                    //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
    //                }

    //            }
    //            BindDataIntoDropDownList();
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void BindDataIntoDropDownList()
    //    {
    //        try
    //        {
    //            //comboBox1.Items.Clear();
    //            if (!string.IsNullOrEmpty(textBox1.Text))
    //            {
    //                List<string> templist = GlobusFileHelper.ReadFiletoStringList(textBox1.Text);
    //                templist.Distinct().ToList();
    //                ///Modified [1.0.0.3]
    //                lstMessage = new List<string>();

    //                foreach (string item in templist)
    //                {
    //                    if (!lstMessage.Contains(item))
    //                    {
    //                        lstMessage.Add(item);
    //                    }
    //                }
    //                //comboBox1.Items.Clear();
    //                ddlSelectMessage.Items.Clear();
    //                if (lstMessage.Count > 0)
    //                {

    //                    foreach (string item in lstMessage)
    //                    {
    //                        try
    //                        {
    //                            if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
    //                            {
    //                                ddlSelectMessage.Items.Add(item);
    //                            }
    //                        }
    //                        catch
    //                        {

    //                        }
    //                    }

    //                    AddToReplyInterface("Message Bound Drop Down List Successfully !");
    //                    //foreach (string lstMessageitem in lstMessage)
    //                    //{
    //                    //    try
    //                    //    {

    //                    //        string[] strTitleMessage = Regex.Split(lstMessageitem, "\r\n");
    //                    //        if (strTitleMessage.Count() > 0)
    //                    //        {
    //                    //            foreach (string item in strTitleMessage)
    //                    //            {
    //                    //                if (!string.IsNullOrWhiteSpace(item))
    //                    //                {
    //                    //                    //comboBox1.Items.Add(lstMessageitem);
    //                    //                    ddlSelectMessage.Items.Add(item);
    //                    //                    break;
    //                    //                }
    //                    //            }

    //                    //        }
    //                    //    }
    //                    //    catch
    //                    //    {
    //                    //    }
    //                    //}
    //                    ////comboBox1.SelectedIndex = 0;
    //                    //ddlSelectMessage.SelectedIndex = 0;
    //                    //foreach (string item in lstMessage)
    //                    //{
    //                    //    string[] strTitleMessage = Regex.Split(item, "\r\n");
    //                    //    try
    //                    //    {
    //                    //        foreach (string item1 in strTitleMessage)
    //                    //        {
    //                    //            try
    //                    //            {
    //                    //                if (!string.IsNullOrWhiteSpace(item1))
    //                    //                {

    //                    //                    if (item1 == ddlSelectMessage.SelectedItem.ToString())
    //                    //                    {
    //                    //                        try
    //                    //                        {
    //                    //                            txtEditMessage.Text = item.Replace(ddlSelectMessage.SelectedItem.ToString(), string.Empty).TrimStart();
    //                    //                            editMessage = txtEditMessage.Text;
    //                    //                            return;
    //                    //                        }
    //                    //                        catch (Exception)
    //                    //                        {

    //                    //                        }
    //                    //                    }

    //                    //                }
    //                    //            }
    //                    //            catch (Exception)
    //                    //            {

    //                    //            }
    //                    //        }

    //                    //    }
    //                    //    catch { }

    //                    //}

    //                }
    //            }
    //            else
    //            {
    //                MessageBox.Show("Please Browse Message File !");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine(ex.StackTrace);
    //        }
    //    }

    //    private void ddlSelectMessage_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            txtEditMessage.Text = ddlSelectMessage.SelectedItem.ToString();
    //        }
    //        catch
    //        {
    //        }
    //    }

    //    private void btnClearDataBase_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            if (MessageBox.Show("Do you really want to delete all the records from Database ?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
    //            {
    //                obj_clsDB_ReplyInterface.DeleteFromtb_ReplyCampaign();

    //                AddToReplyInterface("Records Deleted Sucessfully !");

    //                btnReadMessageFromDataBase_Click(sender, e);
    //            }
    //            else
    //            {
    //                AddToReplyInterface("Records Couldn't Delete  !");
    //            }
    //        }
    //        catch
    //        {
    //        }
    //    }
    //}


    public partial class frmReplyInterface : Form
    {
        #region global declaration
        clsDB_ReplyInterface obj_clsDB_ReplyInterface = new clsDB_ReplyInterface();
        Tweeter.Tweeter obj_Tweeter = new Tweeter.Tweeter();
        clsDBQueryManager objclsSettingDB = new clsDBQueryManager();
        List<string> lstMessage = new List<string>();

        string userName = string.Empty;
        string statusId = string.Empty;
        string PostAuthenticityToken = string.Empty;
        string message = string.Empty;
        string screenName = string.Empty;
        string userDisplayName = string.Empty;
        int numberOfThreads = 7; 
        #endregion

        #region frmReplyInterface
        public frmReplyInterface()
        {
            InitializeComponent();
        } 
        #endregion

        #region btnTweetAndReply_Click
        private void btnTweetAndReply_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    numberOfThreads = Convert.ToInt32(txtNumberOfThreads.Text);
                }
                catch
                {
                    MessageBox.Show("Please Enter Numeric Value !");
                    return;
                }
                Thread Thread_ScrapTweetAndReply = new Thread(Start_ScrapTweetAndReply);
                Thread_ScrapTweetAndReply.Start();
            }
            catch
            {
            }
        } 
        #endregion

        #region Start_ScrapTweetAndReply
        private void Start_ScrapTweetAndReply()
        {
            try
            {
                //numberOfThreads = 7;
                //int numberOfUnfollows = 5;
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(Star_ScrapTweetAndReplyMultithreaded), new object[] { item, "" });

                            Thread.Sleep(1000);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload The Account !");
                }


            }
            catch
            {
            }
        } 
        #endregion

        #region Star_ScrapTweetAndReplyMultithreaded
        private void Star_ScrapTweetAndReplyMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


                List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }

                if (tweetAccountManager.AccountStatus == "Account Suspended")
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(tweetAccountManager.Username);

                    AddToReplyInterfaceLog("Account Suspended With User Name : " + tweetAccountManager.Username);
                    return;
                }

                tweetAccountManager.ScrapTweetAndReply();

                BindData();

                tweetAccountManager.logEvents.addToLogger -= ReplyInterface_AddToLogger;
            }
            catch
            {
            }
        } 
        #endregion

        #region frmReplyInterface_Load
        private void frmReplyInterface_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDefaultsFiles();
                ReplyInterface.ReplyInterface.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);
            }
            catch
            {
            }
        } 
        #endregion

        #region LoadDefaultsFiles
        private void LoadDefaultsFiles()
        {
            try
            {
                #region TD Ubdate by me
                lstMessage.Clear();


                string MessageSettings = string.Empty;


                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    if ("ReplyInterface" == row[0].ToString())
                    {
                        if ("Message" == row[1].ToString())
                        {
                            MessageSettings = StringEncoderDecoder.Decode(row[2].ToString());
                        }
                    }
                }

                if (File.Exists(MessageSettings))
                {
                    lstMessage = GlobusFileHelper.ReadFiletoStringList(MessageSettings);
                    textBox1.Text = MessageSettings;
                    BindDataIntoDropDownList();
                    AddToReplyInterfaceLog(lstMessage.Count + " Message loaded");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        } 
        #endregion

        #region ReplyInterface_AddToLogger
        private void ReplyInterface_AddToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddToReplyInterfaceLog(eventArgs.log);
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region AddToReplyInterfaceLog
        private void AddToReplyInterfaceLog(string log)
        {

            try
            {
                if (lstReplyInterfaceLogger.InvokeRequired)
                {
                    lstReplyInterfaceLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstReplyInterfaceLogger.Items.Add(log);
                        lstReplyInterfaceLogger.SelectedIndex = lstReplyInterfaceLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstReplyInterfaceLogger.Items.Add(log);
                    lstReplyInterfaceLogger.SelectedIndex = lstReplyInterfaceLogger.Items.Count - 1;
                }
            }
            catch { }

        } 
        #endregion

        #region btnReadMessageFromDataBase_Click
        private void btnReadMessageFromDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                Thread obj_Thread = new Thread(BindData);
                obj_Thread.Start();
            }
            catch
            {
            }
        } 
        #endregion

        #region BindData
        private void BindData()
        {
            try
            {
                DataTable datatable = CreateTable();

                DataSet ds = obj_clsDB_ReplyInterface.SelectDistinctAllFromtb_ReplyCampaign();
                DataTable dt = ds.Tables["tb_ReplyCampaign"];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        try
                        {
                            // if (item["UserId"].ToString() != item["TweetUserId"].ToString())
                            {

                                DataRow dr = datatable.NewRow();

                                dr["StatusId"] = StringEncoderDecoder.Decode(item["StatusId"].ToString());
                                dr["PostAuthenticityToken"] = StringEncoderDecoder.Decode(item["PostAuthenticityToken"].ToString());
                                dr["UserId"] = StringEncoderDecoder.Decode(item["UserId"].ToString());
                                dr["FakeAccounts"] = StringEncoderDecoder.Decode(item["UserName"].ToString());
                                dr["ScreenName"] = StringEncoderDecoder.Decode(item["ScreenName"].ToString());
                                dr["TweetUserId"] = StringEncoderDecoder.Decode(item["TweetUserId"].ToString());
                                dr["FakeUserName"] = StringEncoderDecoder.Decode(item["TweetUserName"].ToString());
                                dr["ReplyUserId"] = StringEncoderDecoder.Decode(item["ReplyUserId"].ToString());
                                dr["RealUser"] = StringEncoderDecoder.Decode(item["ReplyUserName"].ToString());
                                dr["TweetText"] = StringEncoderDecoder.Decode(item["TweetText"].ToString());
                                dr["Reply"] = StringEncoderDecoder.Decode(item["Reply"].ToString());
                                //dr["Status"] = StringEncoderDecoder.Decode(item["Status"].ToString());

                                datatable.Rows.Add(dr);
                            }


                        }
                        catch
                        {
                        }
                    }

                    if (datatable.Rows.Count > 0)
                    {

                    }

                    else
                    {
                        AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ There Is No New Tweet ! ]");
                    }


                }
                else
                {
                    AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ There Is No Records In DataBase ! ]");
                }

                try
                {

                    grdReplyInterface.Invoke(new MethodInvoker(delegate
                    {

                        grdReplyInterface.DataSource = datatable;

                        //grdReplyInterface.Columns["UserName"].Visible = false;
                        grdReplyInterface.Columns["StatusId"].Visible = true;
                        grdReplyInterface.Columns["PostAuthenticityToken"].Visible = false;
                        grdReplyInterface.Columns["UserId"].Visible = false;
                        grdReplyInterface.Columns["ScreenName"].Visible = false;
                        //grdReplyInterface.Columns["StatusId"].Visible = false;
                        grdReplyInterface.Columns["TweetUserId"].Visible = false;
                        //grdReplyInterface.Columns["TweetUserName"].Visible = false;
                        grdReplyInterface.Columns["ReplyUserId"].Visible = false;
                        //grdReplyInterface.Columns["ReplyUserName"].Visible = false;
                        grdReplyInterface.Columns["TweetText"].Visible = false;
                        grdReplyInterface.Columns["Reply"].Visible = false;
                        grdReplyInterface.Columns["Status"].Visible = false;

                        grdReplyInterface.Refresh();
                        grdReplyInterface.RefreshEdit();

                    }));

                    lstbMessages.Invoke(new MethodInvoker(delegate
                    {
                        lstbMessages.Items.Clear();
                    }));

                    txtEditMessage.Invoke(new MethodInvoker(delegate
                    {
                        txtEditMessage.Clear();
                    }));
                }
                catch
                {
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region CreateTable
        public DataTable CreateTable()
        {
            DataTable dt_details = new DataTable();
            try
            {

                DataColumn col_StatusId = new DataColumn("StatusId");
                col_StatusId.ReadOnly = true;
                DataColumn col_PostAuthenticityToken = new DataColumn("PostAuthenticityToken");
                col_PostAuthenticityToken.ReadOnly = true;
                DataColumn col_UserId = new DataColumn("UserId");
                col_UserId.ReadOnly = true;
                DataColumn col_UserName = new DataColumn("FakeAccounts");
                col_UserName.ReadOnly = true;
                DataColumn col_ScreenName = new DataColumn("ScreenName");
                col_ScreenName.ReadOnly = true;
                DataColumn col_TweetUserId = new DataColumn("TweetUserId");
                col_TweetUserId.ReadOnly = true;
                DataColumn col_TweetUserName = new DataColumn("FakeUserName");
                col_TweetUserName.ReadOnly = true;
                DataColumn col_ReplyUserId = new DataColumn("ReplyUserId");
                col_ReplyUserId.ReadOnly = true;
                DataColumn col_ReplyUserName = new DataColumn("RealUser");
                col_ReplyUserName.ReadOnly = true;
                DataColumn col_TweetText = new DataColumn("TweetText");
                col_TweetText.ReadOnly = true;
                DataColumn col_Reply = new DataColumn("Reply");
                col_Reply.ReadOnly = true;
                DataColumn col_Status = new DataColumn("Status");
                col_Status.ReadOnly = true;

                dt_details.Columns.Add(col_StatusId);
                dt_details.Columns.Add(col_PostAuthenticityToken);
                dt_details.Columns.Add(col_UserId);
                dt_details.Columns.Add(col_UserName);
                dt_details.Columns.Add(col_ScreenName);
                dt_details.Columns.Add(col_TweetUserId);
                dt_details.Columns.Add(col_TweetUserName);
                dt_details.Columns.Add(col_ReplyUserId);
                dt_details.Columns.Add(col_ReplyUserName);
                dt_details.Columns.Add(col_TweetText);
                dt_details.Columns.Add(col_Reply);
                dt_details.Columns.Add(col_Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return dt_details;
        } 
        #endregion

        #region grdReplyInterface_Click
        private void grdReplyInterface_Click(object sender, EventArgs e)
        {

        } 
        #endregion

        #region grdReplyInterface_MouseClick
        private void grdReplyInterface_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                lstbMessages.Visible = true;
                txtEditMessage.Visible = true;
                btnReply.Visible = true;
                ddlSelectMessage.Visible = true;

                lstbMessages.Items.Clear();

                int rowIndex = grdReplyInterface.CurrentCell.RowIndex;

                statusId = grdReplyInterface.Rows[rowIndex].Cells[0].Value.ToString();
                PostAuthenticityToken = grdReplyInterface.Rows[rowIndex].Cells[1].Value.ToString();
                userName = grdReplyInterface.Rows[rowIndex].Cells[3].Value.ToString();
                userDisplayName = grdReplyInterface.Rows[rowIndex].Cells[6].Value.ToString();
                screenName = grdReplyInterface.Rows[rowIndex].Cells[8].Value.ToString();

                DataSet ds = obj_clsDB_ReplyInterface.SelectTweetAndReplyFromtb_ReplyCampaign(statusId, userName);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool isDispalyTweet = false;

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        try
                        {
                            if (!isDispalyTweet)
                            {
                                if (!string.IsNullOrEmpty(item["TweetText"].ToString()) && !string.IsNullOrWhiteSpace(item["TweetText"].ToString()))
                                {
                                    string strTweet = "Me ==>  " + item["TweetText"].ToString() + "              " + item["TweetDateTime"].ToString();

                                    {
                                        List<string> lstTweet = BraekString(lstbMessages.Width - 360, strTweet);

                                        foreach (string item1 in lstTweet)
                                        {
                                            lstbMessages.Items.Add(item1);

                                        }
                                        lstbMessages.Items.Add(Environment.NewLine);

                                        isDispalyTweet = true;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(item["Reply"].ToString()) && !string.IsNullOrWhiteSpace(item["Reply"].ToString()))
                            {
                                string strReply = "Them ==>  " + item["Reply"].ToString() + "            " + item["ReplyDateTime"].ToString();


                                {
                                    List<string> lstReply = BraekString(lstbMessages.Width - 360, strReply);

                                    foreach (string item1 in lstReply)
                                    {
                                        lstbMessages.Items.Add(item1);
                                    }
                                    lstbMessages.Items.Add(Environment.NewLine);
                                }
                            }

                        }
                        catch
                        {
                        }

                    }
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region BraekString
        private List<string> BraekString(int BreakingLength, string strValue)
        {
            List<string> lstBraekString = new List<string>();
            try
            {
                for (int i = 0; i < strValue.Length; i += BreakingLength)
                {
                    if ((i + BreakingLength) < strValue.Length)
                        lstBraekString.Add(strValue.Substring(i, BreakingLength));
                    else
                        lstBraekString.Add(strValue.Substring(i));
                }
            }
            catch
            {
            }
            return lstBraekString;
        } 
        #endregion

        #region grdReplyInterface_CellContentClick
        private void grdReplyInterface_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        } 
        #endregion

        #region btnReply_Click
        private void btnReply_Click(object sender, EventArgs e)
        {
            try
            {
                if ((lstMessage.Count > 0) && (!string.IsNullOrEmpty(textBox1.Text)))
                {
                    objclsSettingDB.InsertOrUpdateSetting("ReplyInterface", "Message", StringEncoderDecoder.Encode(textBox1.Text));

                }

                if (!string.IsNullOrEmpty(txtEditMessage.Text) && !string.IsNullOrEmpty(txtEditMessage.Text))
                {
                    message = txtEditMessage.Text;
                }
                else
                {
                    MessageBox.Show("Please Enter The Message In Edit Message Box !");
                    return;
                }

                Thread Reply_Thread = new Thread(Reply);
                Reply_Thread.Start();
            }
            catch
            {
            }
        } 
        #endregion

        #region Reply
        private void Reply()
        {
            try
            {
                int numberOfThreads = 7;

                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(Start_ReplyMultithreaded), new object[] { item, "" });

                            Thread.Sleep(1000);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload The Account !");
                }
            }
            catch
            {
            }
        } 
        #endregion

        #region Start_ReplyMultithreaded
        private void Start_ReplyMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


                List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                if (tweetAccountManager.Username == userName)
                {

                    if (userDisplayName.Length + screenName.Length + message.Length < 140)
                    {

                        //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                        tweetAccountManager.logEvents.addToLogger += new EventHandler(ReplyInterface_AddToLogger);

                        if (!tweetAccountManager.IsLoggedIn)
                        {
                            tweetAccountManager.Login();
                        }

                        if (tweetAccountManager.AccountStatus == "Account Suspended")
                        {
                            clsDBQueryManager database = new clsDBQueryManager();
                            database.UpdateSuspendedAcc(tweetAccountManager.Username);

                            AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ Account Suspended With User Name : " + tweetAccountManager.Username + " ]");
                            return;
                        }

                        tweetAccountManager.ReplyThroughReplyInterface(PostAuthenticityToken, statusId, userDisplayName, screenName, message, userName);

                        tweetAccountManager.logEvents.addToLogger -= ReplyInterface_AddToLogger;

                        BindData();
                    }
                    else
                    {
                        MessageBox.Show("Please Edit  The Message Of Edit Message Box . Since The Message Length Is Greater Than 140 Characters !");
                        return;
                    }
                }
            }
            catch
            {
            }

            finally
            {
                try
                {
                    // BindData();
                }
                catch
                {
                }
            }
        } 
        #endregion

        #region btnBrowseLoadMsg_Click
        private void btnBrowseLoadMsg_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        textBox1.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]

                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstMessage.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ " + lstMessage.Count + " Message Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }
                BindDataIntoDropDownList();
            }
            catch
            {
            }
        } 
        #endregion

        #region BindDataIntoDropDownList
        private void BindDataIntoDropDownList()
        {
            try
            {
                //comboBox1.Items.Clear();
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    List<string> templist = GlobusFileHelper.ReadFiletoStringList(textBox1.Text);
                    templist.Distinct().ToList();
                    ///Modified [1.0.0.3]
                    lstMessage = new List<string>();

                    foreach (string item in templist)
                    {
                        if (!lstMessage.Contains(item))
                        {
                            lstMessage.Add(item);
                        }
                    }
                    //comboBox1.Items.Clear();
                    ddlSelectMessage.Items.Clear();
                    if (lstMessage.Count > 0)
                    {

                        foreach (string item in lstMessage)
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                                {
                                    ddlSelectMessage.Items.Add(item);
                                }
                            }
                            catch
                            {

                            }
                        }

                        AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ Message Bound Drop Down List Successfully ! ]");
                        //foreach (string lstMessageitem in lstMessage)
                        //{
                        //    try
                        //    {

                        //        string[] strTitleMessage = Regex.Split(lstMessageitem, "\r\n");
                        //        if (strTitleMessage.Count() > 0)
                        //        {
                        //            foreach (string item in strTitleMessage)
                        //            {
                        //                if (!string.IsNullOrWhiteSpace(item))
                        //                {
                        //                    //comboBox1.Items.Add(lstMessageitem);
                        //                    ddlSelectMessage.Items.Add(item);
                        //                    break;
                        //                }
                        //            }

                        //        }
                        //    }
                        //    catch
                        //    {
                        //    }
                        //}
                        ////comboBox1.SelectedIndex = 0;
                        //ddlSelectMessage.SelectedIndex = 0;
                        //foreach (string item in lstMessage)
                        //{
                        //    string[] strTitleMessage = Regex.Split(item, "\r\n");
                        //    try
                        //    {
                        //        foreach (string item1 in strTitleMessage)
                        //        {
                        //            try
                        //            {
                        //                if (!string.IsNullOrWhiteSpace(item1))
                        //                {

                        //                    if (item1 == ddlSelectMessage.SelectedItem.ToString())
                        //                    {
                        //                        try
                        //                        {
                        //                            txtEditMessage.Text = item.Replace(ddlSelectMessage.SelectedItem.ToString(), string.Empty).TrimStart();
                        //                            editMessage = txtEditMessage.Text;
                        //                            return;
                        //                        }
                        //                        catch (Exception)
                        //                        {

                        //                        }
                        //                    }

                        //                }
                        //            }
                        //            catch (Exception)
                        //            {

                        //            }
                        //        }

                        //    }
                        //    catch { }

                        //}

                    }
                }
                else
                {
                    MessageBox.Show("Please Browse Message File !");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        } 
        #endregion

        #region ddlSelectMessage_SelectedIndexChanged
        private void ddlSelectMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtEditMessage.Text = ddlSelectMessage.SelectedItem.ToString();
            }
            catch
            {
            }
        } 
        #endregion

        #region btnClearDataBase_Click
        private void btnClearDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you really want to delete all the records from Database ?", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    obj_clsDB_ReplyInterface.DeleteFromtb_ReplyCampaign();

                    AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ Records Deleted Sucessfully ! ]");

                    btnReadMessageFromDataBase_Click(sender, e);
                }
                else
                {
                    AddToReplyInterfaceLog("[ " + DateTime.Now + " ] => [ Records Couldn't Delete  ! ]");
                }
            }
            catch
            {
            }
        } 
        #endregion
    }
}
