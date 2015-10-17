using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft;
using System.Threading;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using BaseLib;
using System.Text.RegularExpressions;
using Follower;

namespace twtboardpro
{
    public partial class App_Follow : Form
    {
        private System.Drawing.Image image;
        public App_Follow()
        {
            InitializeComponent();
        }
        enum AppFollowStatus
        {
            AppFollowload, ConnectPage, RedirectClicked, SignInClicked, LoggedOut, LoggedOutSucceed, IPCatch, SuccessfullyLogout,IPNotWorked ,None
        }

        public int count = 0;
        List<string> lstProxies = new List<string>();
        List<string> App_Follow_listUserName = new List<string>();
        List<string> App_Follow_listUserPssword = new List<string>();
        List<string> App_Follow_IP =new List<string>();
        List<string> App_Follow_IPPort =new List<string>();
        List<Thread> lst_App_FollowThreads = new List<Thread>();
        int App_FollowCounter = 0;
        
        bool IsStopApp_Follow;
        AppFollowStatus appFollowStatus;
        string IPDetails = string.Empty;

        private void App_Follow_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
        }

        

        public void AddtoLoggerViewAccounts(string log)
        {
            try
            {
                if (lstProcessLoggerForAccounts.InvokeRequired)
                {
                    lstProcessLoggerForAccounts.Invoke(new MethodInvoker(delegate
                    {
                        lstProcessLoggerForAccounts.Items.Add(log);
                        lstProcessLoggerForAccounts.SelectedIndex = lstProcessLoggerForAccounts.Items.Count - 1;
                    }));
                }
                else
                {
                    lstProcessLoggerForAccounts.Items.Add(log);
                    lstProcessLoggerForAccounts.SelectedIndex = lstProcessLoggerForAccounts.Items.Count - 1;
                }
            }
            catch (Exception)
            {
            }
        }

        public List<string> App_Follow_listNames { get; set; }

        private void btn_App_FollowAccountCreation_Click(object sender, EventArgs e)
        {
            //timerAppsWebManager.Start(); 
            new Thread(() =>
            {
                StartAppFollow();
            }).Start();

        }

        int counter_Account = 0;

        List<Thread> lstThreadAppFollow = new List<Thread>();
        bool IsstopAppFollow = false;
        private void StartAppFollow()
        {

            #region commented
            //foreach (var App_Follow_listNames_item in App_Follow_listNames)
            //{
            //    string[] EmailArr = App_Follow_listNames_item.Split(':');



            //    App_Follow_listUserName.Add(EmailArr[0]);
            //    App_Follow_listUserPssword.Add(EmailArr[1]);
            //    if (EmailArr.Length > 2)
            //    {
            //        App_Follow_IP.Add(EmailArr[2]);
            //        App_Follow_IPPort.Add(EmailArr[3]);
            //    }
            //}
            //NavigateApp_Follow();          
            
            #endregion
            try
            {
                if (IsstopAppFollow)
                {
                    return;
                }

                lstThreadAppFollow.Add(Thread.CurrentThread);
                lstThreadAppFollow = lstThreadAppFollow.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { }



            try
            {
                int numberOfThreads = 7;
                int numberOffollows = 5;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                 counter_Account = TweetAccountContainer.dictionary_TweetAccount.Count();
                // stor messaage in list 
                
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    if (counter_Account > 0)
                    {
                        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                        {

                            try
                            {
                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(StartArabFollowMultithreaded), new object[] { item });

                                Thread.Sleep(2000);
                            }
                            catch (Exception ex)
                            {
                                //ErrorLogger.AddToErrorLogText(ex.Message);
                                if (ex.Message == "Thread was being aborted.")
                                {
                                    //AddToLog_follow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                                }
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartArabFollow() -- Foreach loop Dictinary--> " + ex.Message, Globals.Path_UnfollowerErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartArabFollow() --Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartArabFollow()  --> " + ex.Message, Globals.Path_ErrorLogForAccountManager);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartArabFollow() --> " + ex.Message, Globals.Path_ErrorLogForAccountManager);
            }
        }


        private void StartArabFollowMultithreaded(object parameters)
        {
            try
            {
                if (IsstopAppFollow)
                {
                    return;
                }

                lstThreadAppFollow.Add(Thread.CurrentThread);
                lstThreadAppFollow = lstThreadAppFollow.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { }
            
            try
            {


                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
                TweetAccountManager tweetAccountManager = keyValue.Value;
                tweetAccountManager.logEventsArabFollower.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                //tweetAccountManager.logEventsArabFollower.addToLogger += logEvents_Follower_addToLogger;

                //if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.LoginArabFollow();
                }
            }
            catch { }

            finally
            {
                counter_Account--;
                if (counter_Account == 0)
                {
                    AddtoLoggerViewAccounts("Process completed.");
                }
            }
        }


        public void logEvents_Follower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddtoLoggerViewAccounts(eArgs.log);
            }
        }


     private void App_Follow_Paint(object sender, PaintEventArgs e)
     {
         Graphics g;

         g = e.Graphics;

         g.SmoothingMode = SmoothingMode.HighQuality;
         g.DrawImage(image, 0, 0, this.Width, this.Height);
     }

     private void btn_App_FollowAccountCreatorStop_Click(object sender, EventArgs e)
     {
         new Thread(() =>
         {
             StopThreadAppFollow();
         }).Start();
     }

     public void StopThreadAppFollow()
      {
          try
            {
                foreach (Thread item in lstThreadAppFollow)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                foreach(Thread _item in TweetAccountManager.lstThreadStopAppFollowLogIn)
                {
                    try
                    {
                        _item.Abort();
                    }
                    catch
                    {
                    }
                }
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

     private void App_Follow_FormClosed(object sender, FormClosedEventArgs e)
     {
         Unfollower.IsArabFollowOpen = true;
     }
     }

    }

