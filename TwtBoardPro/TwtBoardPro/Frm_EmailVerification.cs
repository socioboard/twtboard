using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EmailActivator;
using BaseLib;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Text.RegularExpressions;

namespace twtboardpro
{
    public partial class Frm_EmailVerification : Form
    {
        bool CheckNetConn = false;
        public Frm_EmailVerification()
        {
            InitializeComponent();
            image = Properties.Resources.app_bg;
        }

        private void Frm_EmailVerification_Load(object sender, EventArgs e)
        {


        }


        private void AddToEmailVerificationLog(string log)
        {

            try
            {
                if (lstboxLogger_EmailVerification.InvokeRequired)
                {
                    lstboxLogger_EmailVerification.Invoke(new MethodInvoker(delegate
                    {
                        lstboxLogger_EmailVerification.Items.Add(log);
                        lstboxLogger_EmailVerification.SelectedIndex = lstboxLogger_EmailVerification.Items.Count - 1;
                    }));
                }
                else
                {
                    lstboxLogger_EmailVerification.Items.Add(log);
                    lstboxLogger_EmailVerification.SelectedIndex = lstboxLogger_EmailVerification.Items.Count - 1;
                }
            }
            catch { }

        }

        List<string> Lst_NonVerifiedEmailAccount;
        static bool _IsEmailVarification = false;
        List<Thread> lstEmailVarificationThread = new List<Thread>();
        private System.Drawing.Image image;

        private void Btn_FileBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_NonVeryfiedEmailAccountFilePath.Text = ofd.FileName;
                        Lst_NonVerifiedEmailAccount = new List<string>();
                        Lst_NonVerifiedEmailAccount = Globussoft.GlobusFileHelper.ReadLargeFile(ofd.FileName);
                        Console.WriteLine(Lst_NonVerifiedEmailAccount.Count + " Accounts loaded");
                        AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ " + Lst_NonVerifiedEmailAccount.Count + " Accounts loaded ]");
                    }
                }
            }
            catch
            {
            }
        }

        int counter = 0;

        private void btn_startVerification_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                lstEmailVarificationThread.Clear();
                if (_IsEmailVarification)
                {
                    _IsEmailVarification = false;
                }

                btn_startVerification.Cursor = Cursors.AppStarting;
                ThreadPool.SetMaxThreads(5, 5);
                new Thread(() =>
                    {
                        AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Start Process of Email Verification ]");
                        foreach (var item in Lst_NonVerifiedEmailAccount)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartVerification), new object[] { item });
                            //StartVerification(new object[] { item });
                        }
                    }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartVerification(object parameters )
        {
            try
            {
                lstEmailVarificationThread.Add(Thread.CurrentThread);
                lstEmailVarificationThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;

                string Email = string.Empty;
                string Password = string.Empty;
                string username = string.Empty;
                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;
                string Proxy = string.Empty;
                string tempEmail = string.Empty;


                #region commented by prabhat 07.12.13
                //string emailData = (string)paramsArray.GetValue(0);
                //if (paramsArray.Length>1)
                //{
                //    Proxy = (string)paramsArray.GetValue(1);
                //}

                //if (!emailData.Contains(':'))
                //{
                //    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Wrong Format Of Emails :- " + emailData + " ]");
                //    return;
                //}


                //Email = emailData.Split(':')[0];
                //Password = emailData.Split(':')[1];

                //AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Going for Email Verification : " + Email + " ]");

                //if (!string.IsNullOrEmpty(Proxy))
                //{
                //    try
                //    {
                //        string[] ProxyData = Proxy.Split(':');
                //        if (ProxyData.Count() == 2)
                //        {
                //            proxyAddress = ProxyData[0];
                //            proxyPort = ProxyData[1];
                //        }
                //        if (ProxyData.Count() == 4)
                //        {
                //            proxyAddress = ProxyData[0];
                //            proxyPort = ProxyData[1];
                //            proxyUsername = ProxyData[2];
                //            proxyPassword = ProxyData[3];
                //        }
                //    }
                //    catch (Exception)
                //    {
                //    }
                //} 
                #endregion

                counter = Lst_NonVerifiedEmailAccount.Count();
                string item = paramsArray.GetValue(0).ToString();


                try
                {
                    string[] arrItem = Regex.Split(item, ":");
                    tempEmail = arrItem[0];
                    if (arrItem.Length == 2)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                    }
                    else if (arrItem.Length == 4)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                        proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        proxyPort = arrItem[3];
                    }
                    else if (arrItem.Length == 6)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                        proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        proxyPort = arrItem[3];//item.Value._ProxyPort;
                        proxyUsername = arrItem[4];//item.Value._ProxyUsername;
                        proxyPassword = arrItem[5];//item.Value._ProxyPassword;
                    }
                    else
                    {
                        AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Wrong Format For Email Password ]");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }


                Globussoft.GlobusHttpHelper globusHelper = new Globussoft.GlobusHttpHelper();
                TweetAccountManager Accountmanager = new TweetAccountManager();

                Accountmanager.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                ClsEmailActivator EmailActivate = new ClsEmailActivator();

                try
                {
                    Accountmanager.globusHttpHelper = globusHelper;
                    Accountmanager.Username = Email.Replace(" ", "").Replace("\0", "");
                    Accountmanager.Password = Password;
                    Accountmanager.proxyAddress = proxyAddress;
                    Accountmanager.proxyPort = proxyPort;
                    Accountmanager.proxyUsername = proxyUsername;
                    Accountmanager.proxyPassword = proxyPassword;
                    Accountmanager.Login();

                    if (Accountmanager.IsLoggedIn)
                    {
                        string postData = ("authenticity_token=" + Accountmanager.postAuthenticityToken).Trim();
                        string postdataPageSource = Accountmanager.globusHttpHelper.postFormData(new Uri("https://twitter.com/account/resend_confirmation_email"), postData, "	https://twitter.com/", "", "", "", "");
                    }
                }
                catch { };


                bool verified = EmailActivate.EmailVerification(Email, Password, ref globusHelper);

                if (verified)
                {
                    try
                    {
                        string pageresponce = Accountmanager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

                        //if (pageresponce.Contains("btn resend-confirmation-email-link"))
                        //{
                        //    verified = false;
                        //}
                    }
                    catch (Exception)
                    {
                        AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Verified Account  login Failed  : " + Email + " ]");
                    }
                }
                if (verified && Accountmanager.IsLoggedIn)
                {

                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_SuccessfullyVerifiedAccounts);
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Account Verified : " + Email + " ]");
                }
                else
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_VerificationFailedAccounts);
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Account Couldn't be Email Verified : " + Email + " ]");
                }
            }
            catch (Exception ex)
            {
                //AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
            }
            finally 
            {
                counter--;
                if (counter == 0)
                {
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToEmailVerificationLog("------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToEmailVerificationLog(eArgs.log);
            }
        }

        private void Frm_EmailVerification_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btnStop_EmailVarification_Click(object sender, EventArgs e)
        {
            try
            {
                _IsEmailVarification = true;
                List<Thread> lstTemp = lstEmailVarificationThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }

                AddToEmailVerificationLog("-------------------------------------------------------------------------------------------------------------------------------");
                AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToEmailVerificationLog("-------------------------------------------------------------------------------------------------------------------------------");
                btn_startVerification.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void btnResendVerification_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                lstEmailVarificationThread.Clear();
                if (_IsEmailVarification)
                {
                    _IsEmailVarification = false;
                }

                btn_startVerification.Cursor = Cursors.AppStarting;
                ThreadPool.SetMaxThreads(5, 5);
                new Thread(() =>
                {
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Start Process of Resend Email for Verification ]");
                    foreach (var item in Lst_NonVerifiedEmailAccount)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartResendEmailForVerification), new object[] { item });
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartResendEmailForVerification(object parameters)
        {
            try
            {
                lstEmailVarificationThread.Add(Thread.CurrentThread);
                lstEmailVarificationThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;

                string Email = string.Empty;
                string Password = string.Empty;
                string username = string.Empty;
                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;
                string Proxy = string.Empty;
                string tempEmail = string.Empty;
                string postdataPageSource = string.Empty;
                        
                counter = Lst_NonVerifiedEmailAccount.Count();
                string item = paramsArray.GetValue(0).ToString();


                try
                {
                    string[] arrItem = Regex.Split(item, ":");
                    tempEmail = arrItem[0];
                    if (arrItem.Length == 2)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                    }
                    else if (arrItem.Length == 4)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                        proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        proxyPort = arrItem[3];
                    }
                    else if (arrItem.Length == 6)
                    {
                        Email = arrItem[0]; //item.Key;
                        Password = arrItem[1];//item.Value._Password;
                        proxyAddress = arrItem[2];//item.Value._ProxyAddress;
                        proxyPort = arrItem[3];//item.Value._ProxyPort;
                        proxyUsername = arrItem[4];//item.Value._ProxyUsername;
                        proxyPassword = arrItem[5];//item.Value._ProxyPassword;
                    }
                    else
                    {
                        AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Wrong Format For Email Password ]");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error >>> " + ex.StackTrace);
                }


                Globussoft.GlobusHttpHelper globusHelper = new Globussoft.GlobusHttpHelper();
                TweetAccountManager Accountmanager = new TweetAccountManager();

                Accountmanager.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                ClsEmailActivator EmailActivate = new ClsEmailActivator();

                try
                {
                    Accountmanager.globusHttpHelper = globusHelper;
                    Accountmanager.Username = Email.Replace(" ", "").Replace("\0", "");
                    Accountmanager.Password = Password;
                    Accountmanager.proxyAddress = proxyAddress;
                    Accountmanager.proxyPort = proxyPort;
                    Accountmanager.proxyUsername = proxyUsername;
                    Accountmanager.proxyPassword = proxyPassword;
                    Accountmanager.Login();

                    if (Accountmanager.IsLoggedIn)
                    {
                        string postData = ("authenticity_token=" + Accountmanager.postAuthenticityToken).Trim();
                        postdataPageSource = Accountmanager.globusHttpHelper.postFormData(new Uri("https://twitter.com/account/resend_confirmation_email"), postData, "	https://twitter.com/", "", "", "", "");
                    }
                }
                catch { };


                if (postdataPageSource.Contains("A confirmation email has been sent to you."))
                {
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Email send to confirmation for Account : " + Email + " ]");
                }
                
                else
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_VerificationFailedAccounts);
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ Email not send to confirmation for Account : " + Email + " ]");
                }
            }
            catch (Exception ex)
            {
                //AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
            }
            finally
            {
                counter--;
                if (counter == 0)
                {
                    AddToEmailVerificationLog("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToEmailVerificationLog("------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }


    }
}
