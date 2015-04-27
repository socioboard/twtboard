using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using twtboardpro;
using BaseLib;
using System.Threading;
using Globussoft;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using System.Net;
using ProxySettings;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using Follower;

namespace twtboardpro
{
    public partial class frmDirectMessage : Form
    {
        #region MyRegion
        bool CheckNetConn = false;
        public int counter_Account = 0;

        int followMinDelay = 0;
        int followMaxDelay = 0;
        public static Dictionary<string, Thread> dictionary_Threads = new Dictionary<string, Thread>();
        string threadNaming_Follow_ = "Follow_";
        bool IsStopDirectMessages = false;
        List<Thread> lstIsStopDirectMessages = new List<Thread>(); 
        #endregion

        private System.Drawing.Image image;
        public List<string> listMessage = new List<string>();
        public List<Thread> lstIsStopMessaging = new List<Thread>();
        List<string> list_userIDsToFollowForDirectMessage = new List<string>();
        bool IsAllowUniqueMessage = false;
        public frmDirectMessage()
        {
            InitializeComponent();
        }

        private void btnUploadDirectMessage_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtDirectMessage.Text = ofd.FileName;
                        //Globals.EmailsFilePath = ofd.FileName;
                        listMessage = new List<string>();
                        listMessage = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        //Console.WriteLine(listMessage.Count + " User IDs to Follow loaded");
                        AddToLog_follow(listMessage.Count + " Messages loaded");
                        //txtPathUserIDs.Text = "";
                    }
                    else
                    {
                        listMessage = new List<string>();
                        txtDirectMessage.Text = string.Empty;
                        AddToLog_follow(listMessage.Count + " Messages loaded");
                    }
                }
            }
            catch { }
        }

        private void btnStartMessaging_Click(object sender, EventArgs e)
        {
             CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    AddToLog_follow("[ " + DateTime.Now + " ] => [ Starting Message Process ]");
                    if (!string.IsNullOrEmpty(txtDirectMessageMinDelay.Text) && !string.IsNullOrEmpty(txtDirectMessageMaxDelay.Text)
                         && NumberHelper.ValidateNumber(txtDirectMessageMinDelay.Text) && NumberHelper.ValidateNumber(txtDirectMessageMaxDelay.Text) && !string.IsNullOrEmpty(txtDirectMessage.Text) && listMessage.Count > 0)
                    {
                        Thread thread_StartUnFollowing = null;
                        int abortCounter = 0;

                        if (chkFollowers.Checked)
                        {
                            if (list_userIDsToFollowForDirectMessage.Count == 0)
                            {
                                AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Upload Followers File ]");
                                return;
                            }
                        }

                        if (thread_StartUnFollowing != null)
                        {
                            if (thread_StartUnFollowing.ThreadState == ThreadState.Running || thread_StartUnFollowing.ThreadState == ThreadState.WaitSleepJoin) { thread_StartUnFollowing.Abort(); }
                            while (thread_StartUnFollowing.ThreadState == ThreadState.AbortRequested)
                            {
                                //wait a little bit 
                                if (abortCounter < 40)
                                {
                                    Thread.Sleep(500);
                                    abortCounter++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        IsStopDirectMessages = false;


                        if (!IsStopDirectMessages)
                        {
                            thread_StartUnFollowing = new Thread(() =>
                            {
                                StartMessaging();
                            });
                            thread_StartUnFollowing.Start();
                        }


                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtDirectMessageMinDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter Minimum Delay ]");
                            MessageBox.Show("Please Enter Minimum Delay");
                        }
                        else if (string.IsNullOrEmpty(txtDirectMessageMaxDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter Maximum Delay ]");
                            MessageBox.Show("Please Enter Maximum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtDirectMessageMinDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Minimum Delay ]");
                            MessageBox.Show("Please Enter a Number in Minimum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtDirectMessageMaxDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Maximum Delay ]");
                            MessageBox.Show("Please Enter a Number in Maxiimum Delay");
                        }
                        else if (string.IsNullOrEmpty(txtDirectMessage.Text) && listMessage.Count == 0)
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Upload message file ]");
                            MessageBox.Show("Please Upload message file");
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartfollowing_Click() --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartfollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
        }


        private void StartMessaging()
        {
            try
            {
                int numberOfThreads = 7;
                int numberOffollows = 5;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                counter_Account = TweetAccountContainer.dictionary_TweetAccount.Count();
                // stor messaage in list 
                TweetAccountManager.lstDirectMessageText = listMessage;

                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtDirectMessageThreads.Text) && Globals.IdCheck.IsMatch(txtDirectMessageThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtDirectMessageThreads.Text);
                        //TweetAccountManager.noOfUnfollows = numberOffollows;
                    }

                    if (!string.IsNullOrEmpty(txtNoOfUsers.Text) && Globals.IdCheck.IsMatch(txtNoOfUsers.Text))
                    {
                        numberOffollows = int.Parse(txtNoOfUsers.Text);
                        if (numberOffollows > 200)
                        {
                            TweetAccountManager.noOfUnfollows = 200 ;
                        }
                        else 
                        {
                            TweetAccountManager.noOfUnfollows = numberOffollows;
                        }
                    }
                    else
                    {
                        try
                        {
                            txtNoOfUsers.Invoke(new MethodInvoker(delegate
                            {
                                txtNoOfUsers.Text = "5";
                            }
                             ));
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_follow("[ " + DateTime.Now + " ] => [ Messaging Process Stopped ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartMessaging() --  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartMessaging_click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartMessagingMultithreaded), new object[] { item, txtNoOfUsers.Text });

                            Thread.Sleep(2000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                            if (ex.Message == "Thread was being aborted.")
                            {
                                //AddToLog_follow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartMessaging() -- Foreach loop Dictinary--> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartMessaging() --Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                   
                    this.Invoke(new MethodInvoker(delegate
                    {
                        frmAccounts objFrmAccounts = new frmAccounts();
                        objFrmAccounts.Show();
                    }));

                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {
                   // AddToLog_follow("[ " + DateTime.Now + " ] => [ following Process Stopped ]");
                }
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing()  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }


        private void StartMessagingMultithreaded(object parameters)
        {
            try
            {
                if (IsStopDirectMessages)
                {
                    return;
                }

                if (!IsStopDirectMessages)
                {
                    lstIsStopMessaging.Add(Thread.CurrentThread);
                    lstIsStopMessaging.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                List<string> list_userIDsToFollow = new List<string>();
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;
                int NoOfFollwos = 0;
                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);
                string NofUnfollows = (string)paramsArray.GetValue(1);
                if (!string.IsNullOrEmpty(NofUnfollows) && NumberHelper.ValidateNumber(NofUnfollows))
                {
                    NoOfFollwos = Int32.Parse(NofUnfollows);
                }
                bool OtherUser = false;
                //frmMain_NewUI.IsFollowerScreenName = true;
                
                GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
                TweetAccountManager tweetAccountManager = keyValue.Value;
                tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }

                if (!tweetAccountManager.IsLoggedIn)
                {
                    AddToLog_follow("[ " + DateTime.Now + " ] => [ Not Logged In With Account : " + keyValue.Key + " ]");
                    return;
                }
                if (GlobusRegex.ValidateNumber(txtDirectMessageMinDelay.Text))
                {
                    followMinDelay = Convert.ToInt32(txtDirectMessageMinDelay.Text);
                }
                if (GlobusRegex.ValidateNumber(txtDirectMessageMaxDelay.Text))
                {
                    followMaxDelay = Convert.ToInt32(txtDirectMessageMaxDelay.Text);
                }
                //TwitterDataScrapper objTwitterDataScrapper = new TwitterDataScrapper();
                
                if (list_userIDsToFollowForDirectMessage.Count > 0 && chkFollowers.Checked)
                {
                    
                    list_userIDsToFollow.AddRange(list_userIDsToFollowForDirectMessage);
                }
                else
                {
                    list_userIDsToFollow = tweetAccountManager.GetFollowerListForDirectMessage();
                   
                }
                
                if (list_userIDsToFollow.Count > 0)
                {
                    tweetAccountManager.PostDirectMessage(list_userIDsToFollow, followMinDelay, followMaxDelay, OtherUser);
                }
                else
                {
                    AddToLog_follow("[ " + DateTime.Now + " ] => [ No user id is available for Account : " + keyValue.Key + " ]");
                }

                tweetAccountManager.unFollower.logEvents.addToLogger -= logEvents_Follower_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Follower_addToLogger;
                
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowingMultithreaded()  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartUnFollowingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            finally
            {
                counter_Account--;
                if (counter_Account == 0)
                {
                    if (btnStartMessaging.InvokeRequired)
                    {
                        btnStartMessaging.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_follow("---------------------------------------------------------------------------------------------------------------------------");
                            btnStartMessaging.Cursor = Cursors.Default;

                        }));
                    }
                }
            }

        }

        private void btnUploadFollowerUserDirectMessage_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtFollowerUserDirectMessaging.Text = ofd.FileName;
                        //Globals.EmailsFilePath = ofd.FileName;
                        list_userIDsToFollowForDirectMessage = new List<string>();
                        list_userIDsToFollowForDirectMessage = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        //Console.WriteLine(list_userIDsToFollowForDirectMessage.Count + " User IDs to Follow loaded");
                        AddToLog_follow(list_userIDsToFollowForDirectMessage.Count + " User IDs loaded to send Direct Message");
                        //txtPathUserIDs.Text = "";
                    }
                    else
                    {
                        list_userIDsToFollowForDirectMessage = new List<string>();
                        txtFollowerUserDirectMessaging.Text = string.Empty;
                        AddToLog_follow(listMessage.Count + " User IDs to Follow loaded");
                    }
                }
            }
            catch { }
        }


        void logEvents_Follower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_follow(eArgs.log);
            }
        }
        private void AddToLog_follow(string log)
        {
            try
            {
                if (lstDirectMessageSend.InvokeRequired)
                {
                    lstDirectMessageSend.Invoke(new MethodInvoker(delegate
                    {
                        lstDirectMessageSend.Items.Add(log);
                        lstDirectMessageSend.SelectedIndex = lstDirectMessageSend.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstDirectMessageSend.Items.Add(log);
                    lstDirectMessageSend.SelectedIndex = lstDirectMessageSend.Items.Count - 1;
                }
            }
            catch { }
        }

        private void frmDirectMessage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);

        }

        public void frmDirectMessage_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
        }

        private void chkFollowers_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFollowers.Checked)
            {
                txtFollowerUserDirectMessaging.Enabled = true;
                btnUploadFollowerUserDirectMessage.Enabled = true;
            }
            else
            {
                txtFollowerUserDirectMessaging.Enabled = false;
                btnUploadFollowerUserDirectMessage.Enabled = false;
            }
        }

        private void btnStop_DirectMessageThreads_Click(object sender, EventArgs e)
        {
            try
            {
                IsStopDirectMessages = true;

                //for (int i = 0; i < 2; i++)
                {
                    foreach (Thread item in lstIsStopMessaging)
                    {
                        try
                        {
                            item.Abort();
                        }
                        catch
                        {
                        }
                    }
                    Thread.Sleep(1000);
                }
                AddToLog_follow("------------------------------------------------------------------------------------------------------------------------------------");
                AddToLog_follow("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_follow("------------------------------------------------------------------------------------------------------------------------------------");
                btnStartMessaging.Cursor = Cursors.Default;
                //MessageBox.Show("Process Has Been Stopped !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void frmDirectMessage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Unfollower.IsSendDirectMessage_NewUi = true;
        }

        public void chkUniqueMessage_CheckedChanged(object sender, EventArgs e)
        {
            IsAllowUniqueMessage = true;

            TweetAccountManager.IsUniqueMessagePostForDirectMessage = true;
        }
    }
}
