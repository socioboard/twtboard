using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using System.Drawing.Drawing2D;
using System.Threading;
using Globussoft;

namespace twtboardpro
{
    public partial class frmApplicationFollow : Form
    {
        private System.Drawing.Image image;
        List<Thread> lstThreadAppFollow = new List<Thread>();
        bool IsstopAppFollow = false;

        public frmApplicationFollow()
        {
            InitializeComponent();
        }

        private void frmApplicationFollow_Load(object sender, EventArgs e)
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

        public void logEvents_Follower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddtoLoggerViewAccounts(eArgs.log);
            }
        }

        private void frmApplicationFollow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btn_App_FollowAccountCreation_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                StartAppFollow();
            }).Start();
        }


        int counter_Account = 0;
        private void StartAppFollow()
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
                int numberOfThreads = 7;
                int numberOffollows = 5;
                
                
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
                    tweetAccountManager.LoginApplicationFollow();
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
                foreach (Thread _item in TweetAccountManager.lstThreadStopAppFollowLogIn)
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
    }
}
