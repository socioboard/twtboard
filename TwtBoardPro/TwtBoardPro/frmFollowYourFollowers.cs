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
using IPSettings;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using Follower;


namespace twtboardpro
{
    public partial class frmFollowYourFollowers : Form
    {

            #region obj

            private System.Drawing.Image image;
            bool CheckNetConn = false;
            public int counter_Account = 0;

            int followMinDelay = 0;
            int followMaxDelay = 0;
            public static Dictionary<string, Thread> dictionary_Threads = new Dictionary<string, Thread>();
            string threadNaming_Follow_ = "Follow_";
            bool IsStopFollowYourFollowers = false;
            List<Thread> lstIsStopFollowYourFollowers = new List<Thread>();

            #endregion

        public frmFollowYourFollowers()
        {
            InitializeComponent();
        }

        private void btnStartFollow_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    AddToLog_follow("[ " + DateTime.Now + " ] => [ Starting follow Process ]");
                    if (!string.IsNullOrEmpty(txtfollowMinDelay.Text) && !string.IsNullOrEmpty(txtfollowMaxDelay.Text)
                        && NumberHelper.ValidateNumber(txtfollowMinDelay.Text) && NumberHelper.ValidateNumber(txtfollowMaxDelay.Text))
                    {
                        Thread thread_StartUnFollowing = null;
                        int abortCounter = 0;


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
                        IsStopFollowYourFollowers = false;

                        if (!IsStopFollowYourFollowers)
                        {
                            thread_StartUnFollowing = new Thread(() =>
                            {
                                StartFollowing();
                            });
                            thread_StartUnFollowing.Start();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtfollowMinDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter Minimum Delay ]");
                            MessageBox.Show("Please Enter Minimum Delay");
                        }
                        else if (string.IsNullOrEmpty(txtfollowMaxDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter Maximum Delay ]");
                            MessageBox.Show("Please Enter Maximum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtfollowMinDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Minimum Delay ]");
                            MessageBox.Show("Please Enter a Number in Minimum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtfollowMaxDelay.Text))
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Maximum Delay ]");
                            MessageBox.Show("Please Enter a Number in Maxiimum Delay");
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


        private void StartFollowing()
        {
           
            
            try
            {

                if (IsStopFollowYourFollowers)
                {
                    return;
                }

                // if (!IsStopFollowYourFollowers)
                {
                    lstIsStopFollowYourFollowers.Add(Thread.CurrentThread);
                    lstIsStopFollowYourFollowers.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }
                int numberOfThreads = 7;
                int numberOffollows = 5;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                counter_Account = TweetAccountContainer.dictionary_TweetAccount.Count();
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtNoOffollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOffollowThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtNoOffollowThreads.Text);
                    }

                    if (!string.IsNullOrEmpty(txtNoOffollows.Text) && Globals.IdCheck.IsMatch(txtNoOffollows.Text))
                    {
                        numberOffollows = int.Parse(txtNoOffollows.Text);
                        TweetAccountManager.noOfUnfollows = numberOffollows;
                    }
                    else
                    {
                        try
                        {
                            txtNoOffollows.Invoke(new MethodInvoker(delegate
                            {
                                txtNoOffollows.Text = "5";
                            }
                             ));
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_follow("[ " + DateTime.Now + " ] => [ following Process Stopped ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() --  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartfollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartUnFollowingMultithreaded), new object[] { item, txtNoOffollows.Text });

                            Thread.Sleep(2000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_follow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- Foreach loop Dictinary--> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() --Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Please Upload Twitter Accounts from Menu");
                    //AddToLog_follow("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts from Menu ]");
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
                    AddToLog_follow("[ " + DateTime.Now + " ] => [ following Process Stopped ]");
                }
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing()  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

       
        private void StartUnFollowingMultithreaded(object parameters)
        {
            try
            {
                if (IsStopFollowYourFollowers)
                {
                    return;
                }

               // if (!IsStopFollowYourFollowers)
                {
                    lstIsStopFollowYourFollowers.Add(Thread.CurrentThread);
                    lstIsStopFollowYourFollowers.Distinct();
                    Thread.CurrentThread.IsBackground = true;
                }

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
                frmMain_NewUI.IsFollowerScreenName = true;
                List<string> list_userIDsToFollow = new List<string>();

                TweetAccountManager tweetAccountManager = keyValue.Value;
                AddToLog_follow("[ " + DateTime.Now + " ] => [following Process Started For Account : " + keyValue.Key + " ]");
                //Add to Threads Dictionary
                //AddThreadToDictionary(strModule(Module.Follow), tweetAccountManager.Username);
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
                //////////////temp mehtod
                { 
                        
                    
                }
                //AddToLog_Unfollow("Checking for Persons Not Followed Back");

                 Thread thread_UnFollowing = null;

                //Check Test box and anf useing feature box is checked 
                TwitterDataScrapper objTwitterDataScrapper = new TwitterDataScrapper();
                //if (rdoFollowYourFollowers.Checked)

                //thread_UnFollowing = new Thread(() =>
                //{
                //    list_userIDsToFollow = tweetAccountManager.GetFollowYourFollowersList();
                
                //});

                list_userIDsToFollow = tweetAccountManager.GetFollowYourFollowersList();
                
                if (GlobusRegex.ValidateNumber(txtfollowMinDelay.Text))
                {
                    followMinDelay = Convert.ToInt32(txtfollowMinDelay.Text);
                }
                if (GlobusRegex.ValidateNumber(txtfollowMaxDelay.Text))
                {
                    followMaxDelay = Convert.ToInt32(txtfollowMaxDelay.Text);
                }
                if (list_userIDsToFollow.Count > 0)
                {
                    tweetAccountManager.FollowUsingURLs(list_userIDsToFollow, followMinDelay, followMaxDelay, OtherUser);
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
                    if (btnStartFollow.InvokeRequired)
                    {
                        btnStartFollow.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_follow("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_follow("---------------------------------------------------------------------------------------------------------------------------");
                            btnStartFollow.Cursor = Cursors.Default;

                        }));
                    }
                }
            }

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
                if (lstFollowYourFollwers.InvokeRequired)
                {
                    lstFollowYourFollwers.Invoke(new MethodInvoker(delegate
                    {
                        lstFollowYourFollwers.Items.Add(log);
                        lstFollowYourFollwers.SelectedIndex = lstFollowYourFollwers.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstFollowYourFollwers.Items.Add(log);
                    lstFollowYourFollwers.SelectedIndex = lstFollowYourFollwers.Items.Count - 1;
                }
            }
            catch { }
        }

        private void AddThreadToDictionary(string module, string username)
        {
            ///Name thread and put in threads collection
            try
            {
                Thread.CurrentThread.Name = module + username;
                Thread.CurrentThread.IsBackground = true;
                dictionary_Threads.Add(module + username, Thread.CurrentThread);
            }
            catch { }
        }

        public string strModule(Module module)
        {
            switch (module)
            {
              
                case Module.Follow:
                    return threadNaming_Follow_;

                default:
                    return "";
            }
        }

        private void btnStop_FollowThreads_Click(object sender, EventArgs e)
        {
            try
            {
                IsStopFollowYourFollowers = true;
            

                for (int i = 0; i < 2; i++)
                {
                    List<Thread> Temp = lstIsStopFollowYourFollowers;
                    foreach (Thread item in Temp)
                    {
                        try
                        {
                           // item.Abort();
                            lstIsStopFollowYourFollowers.Remove(item);
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
                btnStartFollow.Cursor = Cursors.Default;
                //MessageBox.Show("Process Has Been Stopped !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            //new Thread(() =>
            //{
            //    AddToLog_follow("[ " + DateTime.Now + " ] => [ Stopping Follow Threads, it may take some time ]");
            //    StopThreads(strModule(Module.Follow));
            //}).Start();
        }

        public void StopThreads(string module)
        {
            Dictionary<string, Thread> tempdictionary_Threads = new Dictionary<string, Thread>();
            foreach (KeyValuePair<string, Thread> item in dictionary_Threads)
            {
                try
                {
                    tempdictionary_Threads.Add(item.Key, item.Value);
                }
                catch { }
            }
            try
            {
                foreach (KeyValuePair<string, Thread> item in tempdictionary_Threads)
                {
                    try
                    {
                        string key = item.Key;
                        string threadName = Regex.Split(key, "_")[0];
                        module = module.Replace("_", "");
                        //Thread thread = item.Value;
                        if (threadName == module)
                        {
                            //AddToLog_follow("[ " + DateTime.Now + " ] => [ Aborting : " + key + " ]");
                            //thread.Abort();
                            Thread thread = item.Value;
                            int abortCounter = 0;

                            if (thread != null)
                            {
                                thread.Abort();
                                //}
                                //AddToLog_follow("[ " + DateTime.Now + " ] => [ Aborted : " + key + " ]");
                                dictionary_Threads.Remove(key);

                            }

                        }

                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Abort in Profile Manager Foreach Loop " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void frmFollowYourFollowers_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
        }

        private void frmFollowYourFollowers_FormClosed(object sender, FormClosedEventArgs e)
        {
            Unfollower.IsFollowYourFollowers_NewUi = true;
        }

        private void frmFollowYourFollowers_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

       


    }
}
