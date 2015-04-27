using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using TwtDominator;
using BaseLib;
using System.Threading;
using Globussoft;
//using Newtonsoft.Json.Linq;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using WatiN.Core;
using System.Net;
using ProxySettings;
using Microsoft.Win32;
//using System.Reflection;


namespace TwtDominator
{
    public partial class frmMain : System.Windows.Forms.Form
    {
        public frmMain()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            InitializeComponent();
            Resize += tabMain_TabIndexChanged;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor, true);
       }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            this.Update();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        Dictionary<string, List<string>> DMList = new Dictionary<string, List<string>>();
        List<string> lstscrapeUsername = new List<string>();
        List<string> lstUserNameID = new List<string>();
        List<string> lstScrapedUserID = new List<string>();
        List<string> lstFakeEmailNames = new List<string>();
        List<string> lstDirectMessage = new List<string>();
        List<string> lstUserId_Tweet = new List<string>();
        List<string> lstUserId_Retweet = new List<string>();
        List<string> lstPublicProxyWOTest = new List<string>();

        static int TweetExtractCount = 10;
        static int RetweetExtractCount = 10;
        int fakeEmailCount = 10;
        int followMinDelay = 0;
        int followMaxDelay = 0;
        int unfollowMinDelay = 0;
        int unfollowMaxDealy = 0;
        int tweetMinDealy = 0;
        int tweetMaxDealy = 0;
        int retweetMinDealy = 0;
        int retweetMaxDealy = 0;
        int replyMinDealy = 0;
        int replyMaxDealy = 0;
        int countParseProxiesThreads = 0;
        Int64 workingproxiesCount = 0;
        int numberOfProxyThreads = 4;
        int accountsPerProxy = 10;  //Change this to change Number of Accounts to be set per proxy
        static int i = 0;
        bool IsUsingDivideData = true;
       
        public List<string> listTweetMessages { get; set; }

        public List<string> listReplyMessages { get; set; }

        public List<string> listKeywords { get; set; }

        public List<string> lst_ProfileLocations { get; set; }

        public List<string> lst_ProfileUsernames { get; set; }

        public List<string> lst_ProfileURLs { get; set; }

        public List<string> lst_ProfileDescriptions { get; set; }

        public List<string> lstProfilePics { get; set; }

        public EventHandler logEvents_addToLogger1 { get; set; }

        public List<string> listUserNames { get; set; }

        List<string> listNames = new List<string>();

        List<string> ValidPublicProxies = new List<string>();

        List<string> listEmails = new List<string>();

        clsDBQueryManager objclsSettingDB = new clsDBQueryManager();
       
        private static readonly object proxiesThreadLockr = new object();

        ProxyUtilitiesFromDataBase proxyFetcher = new ProxyUtilitiesFromDataBase();

        List<string> list_pvtProxy = new List<string>();

        public static Queue<string> queWorkingProxies { get; set; }

        public static readonly object proxyListLockr = new object();

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts frmaccounts = new frmAccounts();
            frmaccounts.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            ///Validate License

            #region Old Licensing
            //LicensingManager.LicenseManager licenseManager = new LicensingManager.LicenseManager();
            //string licenseStatus = string.Empty;
            ////if (!licenseManager.ValidateCPUID(ref licenseStatus)) 
            #endregion

            if (false)
            {
                #region Old Licensing
                //if (licenseStatus == "Some Error in Status Field")
                //{
                //    MessageBox.Show("Some Error in Status Field");
                //    Application.Exit();
                //    this.Close();
                //    return;
                //}

                //string strRes = licenseStatus;
                //if (strRes != "ServerDown")
                //{
                //    LicensingManager.frmMain licenseManagerForm = new LicensingManager.frmMain();
                //    licenseManagerForm.ShowDialog();
                //    if (licenseStatus == "Error in License Validation")
                //    {
                //        MessageBox.Show("Error in License Validation");
                //    }
                //    Application.Exit();
                //}
                //else
                //{
                //    MessageBox.Show("Error Connecting to Twtdominator Server.");
                //    Application.Exit();
                //} 
                #endregion
            }
            else
            {
                //GlobusHttpHelper http = new GlobusHttpHelper();
                //EmailActivator.ClsEmailActivator email = new EmailActivator.ClsEmailActivator();
                //email.EmailVerification("KELLY_Summer7603@hotmail.com", "QwErTy1234!@#$", ref http);
                //TwitterDataScrapper tds = new TwitterDataScrapper();
                //tds.GetTweetData("harry");

                CheckVersion();

                listUserNames = new List<string>();
                listUserIDs = new List<string>();

                queWorkingProxies = new Queue<string>();

                CreateAppDirectories();
                CopyDatabase();

                clsDBQueryManager DataBase = new clsDBQueryManager();
                DataBase.DeleteScrapeSettings();

                LoadDefaultsCaptchaData();

                LoadDefaultsAccountCreator();
                LoadDefaultsProfileData();

                listTweetMessages = new List<string>();

                listReplyMessages = new List<string>();

                listKeywords = new List<string>();

                TwitterSignup.TwitterSignUp.logEvents.addToLogger += new EventHandler(logEvents_Signup_addToLogger);

                EmailActivator.ClsEmailActivator.loggerEvents.addToLogger += new EventHandler(loggerEvents_EmailActivator_addToLogger);

                TweetAccountManager.logEvents_static.addToLogger += new EventHandler(logEvents_static_addToLogger);
               
                frmAccounts.AccountsLogEvents.addToLogger += new EventHandler(AccountsLogEvents_addToLogger);

                frmScheduler.Event_StartScheduler.raiseScheduler += new EventHandler(event_StartScheduler_raiseScheduler);

                frmScheduler.SchedulerLogger.addToLogger += new EventHandler(Schedulerlogger_addToLogger);

                new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(10 * 30000);
                        ClearLogsIfExceeds();
                    }
                }).Start();
                txtScrapeNoOfUsers.Enabled = false;
                txtMaximumTweet.Enabled = false;
                txtMaximumNoRetweet.Enabled = false;
                panelSetting.Visible = false;
                //rdBtnDivideEqually.Enabled = false;
                //rdBtnDivideByGivenNo.Enabled = false;
            }

        }

        private void CheckVersion()
        {
            try
            {

                string thisVersionNumber = GetAssemblyNumber();

                string textFileLocationOnServer = "http://faced.extrem-hosting.net/TDLatestVersion.txt";//"http://cmswebusa.com/developers/SumitGupta/TDLatestVersion.txt";

                GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                string textFileData = httpHelper.getHtmlfromUrl(new Uri(textFileLocationOnServer), "", "");

                string latestVersion = Regex.Split(textFileData, "<:>")[0];
                string updateVersionPath = Regex.Split(textFileData, "<:>")[1];

                if (thisVersionNumber == latestVersion)
                {
                    MessageBox.Show("You have the Updated Version", "Information");
                }

                else
                {
                    if (MessageBox.Show("An Updated Version Available - Do you Want to Upgrade!", "Update Available", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start("iexplore", updateVersionPath);

                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                AddToGeneralLogs("Error in Auto Update Module");
            }
        }

        public string GetAssemblyNumber()
        {
            string appName = System.Reflection.Assembly.GetAssembly(this.GetType()).Location;
            System.Reflection.AssemblyName assemblyName = System.Reflection.AssemblyName.GetAssemblyName(appName);
            string versionNumber = assemblyName.Version.ToString();
            return versionNumber;
        }

        void Schedulerlogger_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToGeneralLogs(eArgs.log);
            }
        }

        void event_StartScheduler_raiseScheduler(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                //AddToGeneralLogs(eArgs.log);
                StartScheduler(eArgs.module);
            }
        }

        private void StartScheduler(Module module)
        {
            switch (module)
            {
                case Module.WaitAndReply:
                    break;
                case Module.Tweet:
                    ReadTweetSettings();
                    break;
                case Module.Retweet:
                    break;
                case Module.Reply:
                    break;
                case Module.Follow:
                    ReadFollowSettings();
                    break;
                case Module.Unfollow:
                    break;
                case Module.ProfileManager:
                    break;
                default:
                    break;
            }
        }

        void AccountsLogEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToGeneralLogs(eArgs.log);
            }
        }

        private void AddToFakeEmailCreator(string log)
        {
            try
            {
                if (btnStart_FakeEmailCreator.InvokeRequired)
                {
                    lstFakeEmailCreatorLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstFakeEmailCreatorLogger.Items.Add(log);
                        lstFakeEmailCreatorLogger.SelectedIndex = lstFakeEmailCreatorLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstFakeEmailCreatorLogger.Items.Add(log);
                    lstFakeEmailCreatorLogger.SelectedIndex = lstFakeEmailCreatorLogger.Items.Count - 1;
                }

                //lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                //{
                //    lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                //}));
            }
            catch { }
        }

        private void LoadDefaultsCaptchaData()
        {
            try
            {
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    if ("DeathByCaptcha" == row[1].ToString())
                    {
                        BaseLib.Globals.DBCUsername = row[0].ToString();
                        BaseLib.Globals.DBCPassword = row[2].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        void loggerEvents_EmailActivator_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToListAccountsLogs(eArgs.log);
            }
        }

        private void ClearLogsIfExceeds()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    {
                        if (lstBoxGeneralLogs.Items.Count >= 10000)
                        {
                            lstBoxGeneralLogs.Items.Clear();
                        }
                        if (lstBoxAccountsLogs.Items.Count >= 10000)
                        {
                            lstBoxAccountsLogs.Items.Clear();
                        }
                        if (lstLogger_Follower.Items.Count >= 10000)
                        {
                            lstLogger_Follower.Items.Clear();
                        }
                        if (lstLogger_Tweet.Items.Count >= 10000)
                        {
                            lstLogger_Tweet.Items.Clear();
                        }
                        if (lstLogger_AccountChecker.Items.Count >= 10000)
                        {
                            lstLogger_AccountChecker.Items.Clear();
                        }
                    }
                }));
            }
            catch { }
        }

        void logEvents_static_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToGeneralLogs(eArgs.log);
            }
        }

        void LogEvents_Proxy_Logs(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToProxysLogs(eArgs.log);
            }
        }

        private void AddToGeneralLogs(string log)
        {
            try
            {
                if (lstBoxGeneralLogs.InvokeRequired)
                {
                    lstBoxGeneralLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstBoxGeneralLogs.Items.Add(log);
                            lstBoxGeneralLogs.SelectedIndex = lstBoxGeneralLogs.Items.Count - 1;
                        }));
                }
                else
                {
                    lstBoxGeneralLogs.Items.Add(log);
                    lstBoxGeneralLogs.SelectedIndex = lstBoxGeneralLogs.Items.Count - 1;
                }
            }
            catch { }
        }

        void logEvents_Signup_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToListAccountsLogs(eArgs.log);
            }
        }

        private void CreateAppDirectories()
        {
            if (!Directory.Exists(Globals.path_AppDataFolder))
            {
                Directory.CreateDirectory(Globals.path_AppDataFolder);
            }
            if (!Directory.Exists(Globals.path_DesktopFolder))
            {
                Directory.CreateDirectory(Globals.path_DesktopFolder);
            }
        }

        private void CopyDatabase()
        {

            string startUpDB = Application.StartupPath + "\\DB_TwtDominator.db";
            string localAppDataDB = Globals.path_AppDataFolder + "\\DB_TwtDominator.db";

            string startUpDB64 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\DB_TwtDominator.db";

            if (!File.Exists(localAppDataDB))
            {
                ///Modified [19-10] to work with 64 Bit as well

                if (File.Exists(startUpDB))
                {
                    try
                    {
                        File.Copy(startUpDB, localAppDataDB);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Could not find a part of the path"))
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TwtDominator");
                            File.Copy(startUpDB, localAppDataDB);
                        }
                    }
                }
                else if (File.Exists(startUpDB64))   //for 64 Bit
                {
                    try
                    {
                        File.Copy(startUpDB64, localAppDataDB);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Could not find a part of the path"))
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TwtDominator");
                            File.Copy(startUpDB64, localAppDataDB);
                        }
                    }
                }
            }
        }

        #region Follow


        public List<string> listUserIDs { get; set; }

        private void btnPathUserIDs_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPathUserIDs.Text = ofd.FileName;
                    //Globals.EmailsFilePath = ofd.FileName;
                    listUserIDs = new List<string>();
                    listUserIDs = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    Console.WriteLine(listUserIDs.Count + " User IDs to Follow loaded");
                    AddToLog_Follower(listUserIDs.Count + " User IDs to Follow loaded");
                    //txtPathUserIDs.Text = "";
                }
            }
        }

        private void btnStartFollowing_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!string.IsNullOrEmpty(txtUserIDtoFollow.Text) || listUserIDs.Count > 0 || !string.IsNullOrEmpty(txtOtherfollow.Text) || Globals.lstScrapedUserIDs.Count > 0) && !string.IsNullOrEmpty(txtFollowMaxDelay.Text) && !string.IsNullOrEmpty(txtFollowMinDelay.Text) && NumberHelper.ValidateNumber(txtFollowMaxDelay.Text) && NumberHelper.ValidateNumber(txtFollowMinDelay.Text))
                {

                    if (!string.IsNullOrEmpty(txtUserIDtoFollow.Text))
                    {
                        listUserIDs.Clear();
                        listUserIDs.Add(txtUserIDtoFollow.Text);
                        txtUserIDtoFollow.Text = "";
                    }
                    else if (Globals.lstScrapedUserIDs.Count > 0 && chkboxScrapedLst.Checked)
                    {
                        listUserIDs.Clear();
                        foreach (string data in Globals.lstScrapedUserIDs)
                        {
                            listUserIDs.Add(data);
                        }
                    }

                    Thread thread_StartFollowing = null;
                    int abortCounter = 0;

                    if (thread_StartFollowing != null)
                    {
                        if (thread_StartFollowing.ThreadState == ThreadState.Running || thread_StartFollowing.ThreadState == ThreadState.WaitSleepJoin) { thread_StartFollowing.Abort(); }
                        while (thread_StartFollowing.ThreadState == ThreadState.AbortRequested)
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

                    thread_StartFollowing = new Thread(() =>
                     {
                         StartFollowing();
                         //LoadSettings_Follower();
                     });
                    thread_StartFollowing.Start();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserIDtoFollow.Text) || !(listUserIDs.Count > 0) || string.IsNullOrEmpty(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please enter User ID or Upload User IDs File to follow or Enter Username to follow Its users");
                    }
                    else if (string.IsNullOrEmpty(txtFollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter Minimum delay");
                    }
                    else if(!NumberHelper.ValidateNumber(txtFollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                    else if (string.IsNullOrEmpty(txtFollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter Maximum Delay");
                    }
                    else if (!NumberHelper.ValidateNumber(txtFollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                    else if (string.IsNullOrEmpty(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please Enter No Of Users");
                    }
                    else if (!NumberHelper.ValidateNumber(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please Enete a Number");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartFollowing_Click() --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartFollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }


        private void LoadSettings_Follower()
        {

            //LoadSettings_Follower(txtUserOtherNumber.Text, txtOtherfollow.Text, txtScrapeNoOfUsers.Text, txtNoOfFollowThreads.Text);
        }


        private void LoadSettings_Follower(string[] data)//(string txtUserOtherNumber, string txtOtherfollow, string txtScrapeNoOfUsers, string txtNoOfFollowThreads, string strOtherUser)
        {
            #region Prev Code 27th June
            //List<List<string>> list_listTargetURLs = new List<List<string>>();

            //int numberOfThreads = 7;
            //    //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            //    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            //    {

            //        //bool OtherUser = false;
            //        if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
            //        {
            //            if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
            //            {
            //                TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
            //                //OtherUser = true;
            //                //txtUserOtherNumber.Text = "";
            //            }
            //            else
            //            {
            //                AddToLog_Follower("Please Enter a Number");
            //                return;
            //            }
            //        }

            //        if (!string.IsNullOrEmpty(txtOtherfollow.Text))
            //        {
            //            listUserIDs.Clear();
            //            if (chkFollowers.Checked)
            //            {
            //                listUserIDs = GetFollowersUsingUserID(txtOtherfollow.Text);
            //                //OtherUser = true;
            //            }
            //            if (chkFollowings.Checked)
            //            {
            //                //listUserIDs = GetFollowingsUsingUserID(txtOtherfollow.Text);
            //                listUserIDs.AddRange(GetFollowingsUsingUserID(txtOtherfollow.Text));
            //                //OtherUser = true;
            //            }
            //        }

            //        if (chkUseDivide.Checked || IsUsingDivideData)
            //        {
            //            int splitNo = 0;
            //            if (rdBtnDivideEqually.Checked)
            //            {
            //                splitNo = listUserIDs.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
            //            }
            //            else if (rdBtnDivideByGivenNo.Checked)
            //            {
            //                if (!string.IsNullOrEmpty(txtScrapeNoOfUsers.Text) && NumberHelper.ValidateNumber(txtScrapeNoOfUsers.Text))
            //                {
            //                    int res = Convert.ToInt32(txtScrapeNoOfUsers.Text);
            //                    splitNo = res;//listUserIDs.Count / res;
            //                }
            //            }
            //            if (splitNo == 0)
            //            {
            //                splitNo = RandomNumberGenerator.GenerateRandom(0, listUserIDs.Count - 1);
            //            }
            //            list_listTargetURLs = Split(listUserIDs, splitNo);
            //        }

            //        if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
            //        {
            //            numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
            //        }

            //        StartFollowingModified(list_listTargetURLs, numberOfThreads);
            //    } 
            #endregion

            #region New Code 27th June

            ReloadAccountsFromDataBase();

            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {

                List<List<string>> list_listTargetURLs = new List<List<string>>();

                #region Parameters Declaration

                int numberOfThreads = 7;

                string userIDToFollow = string.Empty;
                string PathUserIDs = string.Empty;

                bool OtherUser = false;
                bool UseDivide = false;
                string otherUser = string.Empty;
                string UserOtherNumber = string.Empty;

                string strChkFollowers = string.Empty;
                string strChkFollowings = string.Empty;

                string strchkUseDivide = string.Empty;
                string strrdBtnDivideEqually = string.Empty;
                string strrdBtnDivideByGivenNo = string.Empty;

                string ScrapeNoOfUsers = string.Empty;

                string FollowMinDelay = string.Empty;
                string FollowMaxDelay = string.Empty;

                string NoOfFollowThreads = string.Empty;
                //string UserOtherNumber = string.Empty;

                #endregion


                #region Parameters Assiging

                try
                {
                    userIDToFollow = data[0];
                }
                catch { }
                try
                {
                    PathUserIDs = data[1];
                }
                catch { }
                try
                {
                    otherUser = data[2];
                }
                catch { }
                try
                {
                    UserOtherNumber = data[3];
                }
                catch { }
                try
                {
                    strChkFollowers = data[4];
                }
                catch { }
                try
                {
                    strChkFollowings = data[5];
                }
                catch { }
                try
                {
                    strchkUseDivide = data[6];
                }
                catch { }
                try
                {
                    strrdBtnDivideEqually = data[7];
                }
                catch { }
                try
                {
                    strrdBtnDivideByGivenNo = data[8];
                }
                catch { }
                try
                {
                    ScrapeNoOfUsers = data[9];
                }
                catch { }
                try
                {
                    FollowMinDelay = data[10];
                }
                catch { }
                try
                {
                    FollowMaxDelay = data[11];
                }
                catch { }
                try
                {
                    NoOfFollowThreads = data[12];
                }
                catch { }
                //try
                //{
                //    userIDToFollow = data[0];
                //}
                //catch { }


                #endregion


                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                //if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                //{

                List<string> listUserIDs = new List<string>();
                if (!string.IsNullOrEmpty(PathUserIDs))
                {
                    listUserIDs = Globussoft.GlobusFileHelper.ReadFiletoStringList(PathUserIDs);
                }
                if (!string.IsNullOrEmpty(userIDToFollow))
                {
                    listUserIDs.Add(userIDToFollow);
                }

                //bool otherUser = false;
                //if (otherUser == "1")
                //{
                //    otherUser = true;
                //}
                if (!string.IsNullOrEmpty(otherUser))
                {
                    OtherUser = true;
                    if (NumberHelper.ValidateNumber(UserOtherNumber))
                    {
                        TweetAccountManager.noOFFollows = Convert.ToInt32(UserOtherNumber);
                    }
                    else
                    {
                        //AddToLog_Follower("Please Enter a Number");
                        //return;
                        TweetAccountManager.noOFFollows = 10;
                    }
                }

                if (!string.IsNullOrEmpty(otherUser))
                {
                    listUserIDs.Clear();
                    if (strChkFollowers == "1")
                    {
                        listUserIDs = GetFollowersUsingUserID(otherUser);
                        OtherUser = true;
                    }
                    if (strChkFollowings == "1")
                    {
                        //listUserIDs = GetFollowingsUsingUserID(txtOtherfollow.Text);
                        listUserIDs.AddRange(GetFollowingsUsingUserID(otherUser));
                        OtherUser = true;
                    }
                }

                if (strchkUseDivide == "1")//(chkUseDivide.Checked || IsUsingDivideData)
                {
                    UseDivide = true;
                    int splitNo = 0;
                    if (strrdBtnDivideEqually == "1")
                    {
                        splitNo = listUserIDs.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
                    }
                    else if (strrdBtnDivideByGivenNo == "1")
                    {
                        if (!string.IsNullOrEmpty(ScrapeNoOfUsers) && NumberHelper.ValidateNumber(ScrapeNoOfUsers))
                        {
                            int res = Convert.ToInt32(ScrapeNoOfUsers);
                            splitNo = res;//listUserIDs.Count / res;
                        }
                    }
                    if (splitNo == 0)
                    {
                        splitNo = 1;//RandomNumberGenerator.GenerateRandom(0, listUserIDs.Count - 1);
                    }
                    list_listTargetURLs = Split(listUserIDs, splitNo);
                }
                else
                {
                    list_listTargetURLs = Split(listUserIDs, listUserIDs.Count);
                }

                if (!string.IsNullOrEmpty(NoOfFollowThreads) && Globals.IdCheck.IsMatch(NoOfFollowThreads))
                {
                    numberOfThreads = int.Parse(NoOfFollowThreads);
                }

                //string strOtherUser = "false";
                //if (otherUser)
                //{
                //    strOtherUser = "true";
                //}

                ////Save Settings in Text File
                //GlobusFileHelper.WriteStringToTextfile(txtUserOtherNumber + "<:>" + txtOtherfollow + "<:>" + txtScrapeNoOfUsers + "<:>" + txtNoOfFollowThreads + "<:>" + strOtherUser, Globals.Path_FollowSettings);

                StartFollowingModified(list_listTargetURLs, numberOfThreads, OtherUser, UseDivide, FollowMinDelay, FollowMaxDelay);
                //}  

            }
            else
            {
                MessageBox.Show("Please upload accounts from menu");
            }

            #endregion
        }

        private string[] ReadSettingsTextFile(string settingsFilePath)
        {
            string filedata = GlobusFileHelper.ReadStringFromTextfile(settingsFilePath);

            string[] splitSettings = Regex.Split(filedata, "<:>");

            return splitSettings;
        }



        #region Gargi Code 29th June
        //private void StartFollowingMultithreaded(object parameters)
        //{
        //    try
        //    {
        //        bool OtherUser = false;

        //        Array paramsArray = new object[3];

        //        paramsArray = (Array)parameters;

        //        KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

        //        //string userIDToFollow = (string)paramsArray.GetValue(1);
        //        List<string> list_userIDsToFollow = (List<string>)paramsArray.GetValue(1);

        //        OtherUser = (bool)paramsArray.GetValue(2);

        //        List<string> lstFollowers = new List<string>();
        //        List<string> lstFollowings = new List<string>();

        //        TweetAccountManager tweetAccountManager = keyValue.Value;

        //        //Add to Threads Dictionary
        //        AddThreadToDictionary(strModule(Module.Follow), tweetAccountManager.Username);

        //        //string accountUser = tweetAccountManager.Username;
        //        //string accountPass = tweetAccountManager.Password;
        //        //string proxyAddress = tweetAccountManager.proxyAddress;
        //        //string proxyPort = tweetAccountManager.proxyPort;
        //        //string proxyUserName = tweetAccountManager.proxyUsername;
        //        //string proxyPassword = tweetAccountManager.proxyPassword;

        //        //tweetAccountManager = new TweetAccountManager();
        //        //tweetAccountManager.Username = accountUser;
        //        //tweetAccountManager.Password = accountPass;
        //        //tweetAccountManager.proxyAddress = proxyAddress;
        //        //tweetAccountManager.proxyPort = proxyPort;
        //        //tweetAccountManager.proxyUsername = proxyUserName;
        //        //tweetAccountManager.proxyPassword = proxyPassword;

        //        if (GlobusRegex.ValidateNumber(txtFollowMinDelay.Text))
        //        {
        //            followMinDelay = Convert.ToInt32(txtFollowMinDelay.Text); 
        //        }
        //        if (GlobusRegex.ValidateNumber(txtFollowMaxDelay.Text))
        //        {
        //            followMaxDelay = Convert.ToInt32(txtFollowMaxDelay.Text); 
        //        }

        //        tweetAccountManager.follower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
        //        tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;
        //        if (!tweetAccountManager.IsLoggedIn)
        //        {
        //            tweetAccountManager.Login();
        //        }

        //        List<string> TempList = listUserIDs;

        //        foreach (string userid in TempList)
        //        {
        //            if (!string.IsNullOrEmpty(userid) && !NumberHelper.ValidateNumber(userid))
        //            {
        //                listUserIDs.Remove(userid);
        //                listUserIDs.Add(TwitterDataScrapper.GetUserIDFromUsername(userid));
        //            }
        //        }

        //        TwitterDataScrapper DataScraper = new TwitterDataScrapper();
        //        if (ChkboxNoUnfollow.Checked)
        //        {
        //            clsDBQueryManager Db = new clsDBQueryManager();

        //            List<string> Follow = DataScraper.GetFollowings(tweetAccountManager.userID);
        //            List<string> FollowedUsers = Db.SelectFollowData_List(tweetAccountManager.Username);
        //            List<string> CommonFollowers = Follow.Union(FollowedUsers).Distinct().ToList();
        //            List<string> RemainingUsers = listUserIDs.Except(CommonFollowers).ToList();
        //        }
        //        //tweetAccountManager.FollowUsingURLs(userIDToFollow);

        //        ////if (!string.IsNullOrEmpty(txtOtherfollow.Text))
        //        ////{
        //        ////    list_userIDsToFollow.Clear();
        //        ////    if (chkboxFollowers.Checked)
        //        ////    {
        //        ////        list_userIDsToFollow = GetFollowersUsingUserID(txtOtherfollow.Text);
        //        ////        OtherUser = true;
        //        ////    }
        //        ////    else if (chkboxFollowings.Checked)
        //        ////    {
        //        ////        list_userIDsToFollow = GetFollowingsUsingUserID(txtOtherfollow.Text);
        //        ////        OtherUser = true;
        //        ////    } 
        //        ////}

        //        if (list_userIDsToFollow.Count > 0)
        //        {
        //            tweetAccountManager.FollowUsingURLs(list_userIDsToFollow, followMinDelay, followMaxDelay, OtherUser);
        //            txtPathUserIDs.Invoke(new MethodInvoker(delegate
        //            {
        //                txtPathUserIDs.Text = "";
        //            }));
        //            txtUserOtherNumber.Invoke(new MethodInvoker(delegate
        //            {
        //                txtUserOtherNumber.Text = "";
        //            }));
        //            txtScrapeNoOfUsers.Invoke(new MethodInvoker(delegate
        //            {
        //                txtScrapeNoOfUsers.Text = "";
        //            }));
        //            txtOtherfollow.Invoke(new MethodInvoker(delegate
        //            {
        //                txtOtherfollow.Text = "";
        //            }));
        //            Globals.lstScrapedUserIDs.Clear();
        //            //txtPathUserIDs.Text = "";
        //        }
        //        else
        //        {
        //            AddToLog_Follower("No ID's To Follow");
        //        }

        //        tweetAccountManager.follower.logEvents.addToLogger -= logEvents_Follower_addToLogger;
        //        tweetAccountManager.logEvents.addToLogger -= logEvents_Follower_addToLogger;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.AddToErrorLogText(ex.Message);
        //    }
        //} 
        #endregion


        #endregion

        private void rdoUnfollowNotFollowing_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoUnfollowNotFollowing.Checked)
            {
                chkUnfollowDateFilter.Enabled = true;
                txtUnfollowFilterDays.Enabled = true;
            }
            else
            {
                chkUnfollowDateFilter.Enabled = false;
                txtUnfollowFilterDays.Enabled = false;
            }
        }

        private void btnStartUnfollowing_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUnfollowMinDelay.Text) && !string.IsNullOrEmpty(txtUnfollowMaxDelay.Text) && NumberHelper.ValidateNumber(txtUnfollowMinDelay.Text) && NumberHelper.ValidateNumber(txtUnfollowMaxDelay.Text))
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

                    thread_StartUnFollowing = new Thread(() =>
                    {
                        StartUnFollowing();
                    });
                    thread_StartUnFollowing.Start();
                }
                else
                {
                    if(string.IsNullOrEmpty(txtUnfollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter Minimum Delay");
                    }
                    else if(string.IsNullOrEmpty(txtUnfollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter Maximum Delay");
                    }
                    else if(!NumberHelper.ValidateNumber(txtUnfollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                    else if(!NumberHelper.ValidateNumber(txtUnfollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartUnfollowing_Click() --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartUnfollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void StartUnFollowing()
        {
            try
            {
                int numberOfThreads = 7;
                int numberOfUnfollows = 5;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtNoOfUnfollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfUnfollowThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtNoOfUnfollowThreads.Text);
                    }

                    if (!string.IsNullOrEmpty(txtNoOfUnfollows.Text) && Globals.IdCheck.IsMatch(txtNoOfUnfollows.Text))
                    {
                        numberOfUnfollows = int.Parse(txtNoOfUnfollows.Text);
                        TweetAccountManager.noOfUnfollows = numberOfUnfollows;
                    }
                    else
                    {
                        try
                        {
                            txtNoOfUnfollows.Invoke(new MethodInvoker(delegate
                            {
                                txtNoOfUnfollows.Text = "5";
                            }
                             )); 
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_Follower("Unfollowing Process Stopped");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowing() --  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartUnfollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartUnFollowingMultithreaded), new object[] { item, "" });

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_Follower("Unfollowing Process Stopped");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowing() -- Foreach loop Dictinary--> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartUnFollowing() --Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload Twitter Accounts from Menu");
                    AddToLog_Follower("Please Upload Twitter Accounts from Menu");
                }
            }
            catch(Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {
                    AddToLog_Follower("Unfollowing Process Stopped");
                }
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowing()  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartUnFollowing() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void StartUnFollowingMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                //string userIDToFollow = (string)paramsArray.GetValue(1);
                List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;


                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Unfollow), tweetAccountManager.Username);
                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string proxyAddress = tweetAccountManager.proxyAddress;
                //string proxyPort = tweetAccountManager.proxyPort;
                //string proxyUserName = tweetAccountManager.proxyUsername;
                //string proxyPassword = tweetAccountManager.proxyPassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.proxyAddress = proxyAddress;
                //tweetAccountManager.proxyPort = proxyPort;
                //tweetAccountManager.proxyUsername = proxyUserName;
                //tweetAccountManager.proxyPassword = proxyPassword;


                tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_UnFollower_addToLogger;

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }
                
                if (rdoUnfollowNotFollowing.Checked)
                {
                    //list_userIDsToFollow = tweetAccountManager.GetNonFollowings();

                    if (chkUnfollowDateFilter.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtUnfollowFilterDays.Text) && GlobusRegex.ValidateNumber(txtUnfollowFilterDays.Text))
                        {
                            int days = int.Parse(txtUnfollowFilterDays.Text);
                            list_userIDsToFollow = tweetAccountManager.GetNonFollowingsBeforeSpecifiedDate(days);
                        }
                    }
                    else
                    {
                        list_userIDsToFollow = tweetAccountManager.GetNonFollowings();
                    }
                }
                else
                {
                    list_userIDsToFollow = tweetAccountManager.GetFollowings();
                }
                if (GlobusRegex.ValidateNumber(txtUnfollowMinDelay.Text))
                {
                    unfollowMinDelay = Convert.ToInt32(txtUnfollowMinDelay.Text); 
                }
                if (GlobusRegex.ValidateNumber(txtUnfollowMaxDelay.Text))
                {
                    unfollowMaxDealy = Convert.ToInt32(txtUnfollowMaxDelay.Text); 
                }
                tweetAccountManager.UnFollowUsingURLs(list_userIDsToFollow, unfollowMinDelay, unfollowMaxDealy);

                tweetAccountManager.unFollower.logEvents.addToLogger -= logEvents_UnFollower_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_UnFollower_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowingMultithreaded()  --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartUnFollowingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

        }

        #region Tweet

        

        private void btnUploadTweetMessage_Click(object sender, EventArgs e)
        {
            try
            {
                lock (TweetAccountManager.locker_que_TweetMessage)
                {
                    TweetAccountManager.que_TweetMessages.Clear();
                }
                listTweetMessages.Clear();

                lock (TweetAccountManager.locker_que_ReplyTweetMessage)
                {
                    TweetAccountManager.que_ReplyMessages.Clear();
                }
                listReplyMessages.Clear();

                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtTweetMessageFile.Text = ofd.FileName;
                        listTweetMessages = new List<string>();

                        listTweetMessages = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        Globals.Path_tweetMessagePath = ofd.FileName;
                        Console.WriteLine(listTweetMessages.Count + " Tweet Messages loaded");
                        AddToLog_Tweet(listTweetMessages.Count + " Tweet Messages loaded");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error in tweet Upload --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void StartTweetingMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                //string userIDToFollow = (string)paramsArray.GetValue(1);
                //string tweetMessage = (string)paramsArray.GetValue(1);
                List<string> lst_tweetMessage = (List<string>)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Tweet), tweetAccountManager.Username);

                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string proxyAddress = tweetAccountManager.proxyAddress;
                //string proxyPort = tweetAccountManager.proxyPort;
                //string proxyUserName = tweetAccountManager.proxyUsername;
                //string proxyPassword = tweetAccountManager.proxyPassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.proxyAddress = proxyAddress;
                //tweetAccountManager.proxyPort = proxyPort;
                //tweetAccountManager.proxyUsername = proxyUserName;
                //tweetAccountManager.proxyPassword = proxyPassword;


                tweetAccountManager.tweeter.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;

               
               
                if (GlobusRegex.ValidateNumber(txtMinDelay_Tweet.Text))
                {
                    tweetMinDealy = Convert.ToInt32(txtMinDelay_Tweet.Text);
                }
                if (GlobusRegex.ValidateNumber(txtMaxDelay_Tweet.Text))
                {
                    tweetMaxDealy = Convert.ToInt32(txtMaxDelay_Tweet.Text);
                }
               
                //tweetAccountManager.Login();
                //tweetAccountManager.Tweet(tweetMessage);

                if (ChkboxTweetPerday.Checked)
                {
                    TweetAccountManager.NoOfTweets = 0;
                    TweetAccountManager.TweetPerDay = true;
                    if (!string.IsNullOrEmpty(txtMaximumTweet.Text) && NumberHelper.ValidateNumber(txtMaximumTweet.Text))
                    {
                        TweetAccountManager.NoOfTweetPerDay = Convert.ToInt32(txtMaximumTweet.Text);
                        AddToLog_Tweet(TweetAccountManager.NoOfTweetPerDay + " Maximum No Of Tweets Per Day");
                    }
                    else
                    {
                        TweetAccountManager.NoOfTweetPerDay = 10;
                        AddToLog_Tweet("Setting Maximum No Of Tweets Per Day as 10");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Key, "Tweet");

                    int TodayTweet = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    TweetAccountManager.AlreadyTweeted = TodayTweet;
                    AddToLog_Tweet(TodayTweet + " Already tweeted today");
                    if (TodayTweet >= TweetAccountManager.NoOfTweetPerDay)
                    {
                        AddToLog_Tweet("Already Tweeted " + TweetAccountManager.NoOfTweetPerDay);
                        return;
                    }
                }

                if (IsTweetScheduled)
                {
                    try
                    {
                        DateTime d1 = dateTimePicker_tweeterStart.Value.ToLocalTime();
                        DateTime d2 = dateTimePicker_TwetterEnd.Value.ToLocalTime();

                        TweetAccountManager.StartTime = d1;
                        TweetAccountManager.EndTime = d2;

                        TimeSpan T = d2 - d1;

                        int Delay = T.Minutes;

                        int TotalTweets = 0;

                        if (!string.IsNullOrEmpty(txtNoOfTweets.Text) && NumberHelper.ValidateNumber(txtNoOfTweets.Text))
                        {
                            TotalTweets = Convert.ToInt32(txtNoOfTweets.Text);
                        }
                        else
                        {
                            TotalTweets = TweetAccountManager.NoOfTweetPerDay - TweetAccountManager.AlreadyTweeted;
                        }

                        int TotalDelay = (Delay * 60) / TotalTweets;

                        TweetAccountManager.DelayTweet = TotalDelay;
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- Tweet Scheduled --> " + ex.Message, Globals.Path_TweetingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweetingMultithreaded() -- TweetScheduled  --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                
                tweetAccountManager.Tweet(lst_tweetMessage, tweetMinDealy, tweetMaxDealy);

                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweetingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnStartReTweeting_Click(object sender, EventArgs e)
        {
            if (chkboxReplyPerDay.Checked)
            {
                MessageBox.Show("Please Check Retweet Per Day Of No Of Retweet");
                return;
            }

            if (!string.IsNullOrEmpty(txtTweetKeyword.Text))
            {
                string tweetKeyword = txtTweetKeyword.Text;
                new Thread(() =>
                {
                    {
                        //Scrap Tweets using Keyword
                        TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                        TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData(tweetKeyword);

                        StartReTweeting(); 
                    }
                }).Start();
            }
            else
            {
                MessageBox.Show("Please enter Tweet Search Keyword");
            }
        }

        private void StartReTweeting()
        {
            int numberOfThreads = 7;
            //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNoOfTweetThreads.Text);
                }

                if (!string.IsNullOrEmpty(txtNoOfRetweets.Text) && Globals.IdCheck.IsMatch(txtNoOfRetweets.Text))
                {
                    TweetAccountManager.noOfRetweets = int.Parse(txtNoOfRetweets.Text);
                }

                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    string tweetMessage = "";
                    try
                    {
                        tweetMessage = listTweetMessages[RandomNumberGenerator.GenerateRandom(0, listTweetMessages.Count - 1)];
                    }
                    catch(Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReTweeting() --  tweetMessage --> " + ex.Message, Globals.Path_TweetingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReTweeting() -- tweetMessage --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    

                    try
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadpoolMethod_Retweet), new object[] {item});

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReTweeting() -- ThreadPoolMethod_Retweet--> " + ex.Message, Globals.Path_TweetingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReTweeting() -- ThreadPoolMethod_Retweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Upload twitter Account and Wall Message");
                AddToLog_Follower("Please Upload Twitter Account and Wall Message");
            }
        }

        private void ThreadpoolMethod_Retweet(object parameters)
        {
            try
            {
                Array paramsArray = new object[1];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Retweet), tweetAccountManager.Username);

                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string proxyAddress = tweetAccountManager.proxyAddress;
                //string proxyPort = tweetAccountManager.proxyPort;
                //string proxyUserName = tweetAccountManager.proxyUsername;
                //string proxyPassword = tweetAccountManager.proxyPassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.proxyAddress = proxyAddress;
                //tweetAccountManager.proxyPort = proxyPort;
                //tweetAccountManager.proxyUsername = proxyUserName;
                //tweetAccountManager.proxyPassword = proxyPassword;

                tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                //tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;

                if (GlobusRegex.ValidateNumber(txtMinDelay_Tweet.Text))
                {
                    retweetMinDealy = Convert.ToInt32(txtMinDelay_Tweet.Text);
                }
                if (GlobusRegex.ValidateNumber(txtMaxDelay_Tweet.Text))
                {
                    retweetMaxDealy = Convert.ToInt32(txtMaxDelay_Tweet.Text);
                }


                if (chkboxRetweetPerDay.Checked)
                {
                    TweetAccountManager.RetweetPerDay = true;
                    if (!string.IsNullOrEmpty(txtMaximumNoRetweet.Text) && NumberHelper.ValidateNumber(txtMaximumNoRetweet.Text))
                    {
                        TweetAccountManager.NoOFRetweetPerDay = Convert.ToInt32(txtMaximumNoRetweet.Text);
                    }
                    else
                    {
                        TweetAccountManager.NoOFRetweetPerDay = 10;
                        AddToLog_Tweet("Setting Maximum No Of ReTweets Per Day as 10");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Key, "ReTweet");

                    int TodayReTweet = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    TweetAccountManager.AlreadyRetweeted = TodayReTweet;

                    if (TodayReTweet >= TweetAccountManager.NoOFRetweetPerDay)
                    {
                        AddToLog_Tweet("Already Retweeted " + TweetAccountManager.AlreadyRetweeted);
                        return;
                    }

                }


                tweetAccountManager.ReTweet("", retweetMinDealy, retweetMaxDealy);

                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnStartReplying_Click(object sender, EventArgs e)
        {
            if (chkboxRetweetPerDay.Checked)
            {
                MessageBox.Show("Please Check Reply Per Day or No Of Reply");
                return;
            }
            if (listTweetMessages.Count >= 1 && !string.IsNullOrEmpty(txtTweetKeyword.Text))
            //if (!string.IsNullOrEmpty(txtTweetKeyword.Text))
            {
                string tweetKeyword = txtTweetKeyword.Text;
                new Thread(() =>
                {
                   
                    TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                    TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData(tweetKeyword);

                    StartReplying();
                }).Start();
            }
            else
            {
                MessageBox.Show ("Please upload Tweet Messages File & put a Tweet Search Keyword");
            }
        }

        private void StartReplying()
        {
            int numberOfThreads = 7;
            //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNoOfTweetThreads.Text);
                }

                if (!string.IsNullOrEmpty(txtNoOfRetweets.Text) && Globals.IdCheck.IsMatch(txtNoOfRetweets.Text))
                {
                    TweetAccountManager.noOfRetweets = int.Parse(txtNoOfRetweets.Text);
                }

                listTweetMessages = listTweetMessages.Distinct().ToList();

                foreach (string item in listTweetMessages)
                {
                    TweetAccountManager.que_ReplyMessages.Enqueue(item);
                }

                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    string tweetMessage = "";
                    try
                    {
                        tweetMessage = listTweetMessages[RandomNumberGenerator.GenerateRandom(0, listTweetMessages.Count - 1)];
                    }
                    catch(Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReplying() --> " + ex.Message, Globals.Path_TweetingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReplying() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        #region Commented
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                        //{
                        //    TweetAccountManager tweetAccountManager = item.Value;

                        //    string accountUser = tweetAccountManager.Username;
                        //    string accountPass = tweetAccountManager.Password;
                        //    string proxyAddress = tweetAccountManager.proxyAddress;
                        //    string proxyPort = tweetAccountManager.proxyPort;
                        //    string proxyUserName = tweetAccountManager.proxyUsername;
                        //    string proxyPassword = tweetAccountManager.proxyPassword;

                        //    tweetAccountManager = new TweetAccountManager();
                        //    tweetAccountManager.Username = accountUser;
                        //    tweetAccountManager.Password = accountPass;
                        //    tweetAccountManager.proxyAddress = proxyAddress;
                        //    tweetAccountManager.proxyPort = proxyPort;
                        //    tweetAccountManager.proxyUsername = proxyUserName;
                        //    tweetAccountManager.proxyPassword = proxyPassword;

                        //    tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                        //    //tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;
                        //    tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;

                        //    tweetAccountManager.Reply(tweetMessage);

                        //    tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                        //})); 
                        #endregion

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_Reply), new object[] { item, tweetMessage});

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReplying() -- Going To Function ThreadPoolMethod_Reply() --> " + ex.Message, Globals.Path_TweetingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReplying() -- Going To Function ThreadPoolMethod_Reply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Upload Twitter Account");
                AddToLog_Follower("Please Upload Twitter Account");
            }
        }

        private void ThreadPoolMethod_Reply(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                string tweetMessage = (string)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Reply), tweetAccountManager.Username);

                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string proxyAddress = tweetAccountManager.proxyAddress;
                //string proxyPort = tweetAccountManager.proxyPort;
                //string proxyUserName = tweetAccountManager.proxyUsername;
                //string proxyPassword = tweetAccountManager.proxyPassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.proxyAddress = proxyAddress;
                //tweetAccountManager.proxyPort = proxyPort;
                //tweetAccountManager.proxyUsername = proxyUserName;
                //tweetAccountManager.proxyPassword = proxyPassword;

                tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                //tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;

                if (GlobusRegex.ValidateNumber(txtMinDelay_Tweet.Text))
                {
                    replyMinDealy = Convert.ToInt32(txtMinDelay_Tweet.Text);
                }
                if (GlobusRegex.ValidateNumber(txtMaxDelay_Tweet.Text))
                {
                    replyMaxDealy = Convert.ToInt32(txtMaxDelay_Tweet.Text);
                }



                if (chkboxReplyPerDay.Checked)
                {
                    TweetAccountManager.ReplyPerDay = true;
                    if (!string.IsNullOrEmpty(txtMaximumNoRetweet.Text) && NumberHelper.ValidateNumber(txtMaximumNoRetweet.Text))
                    {
                        TweetAccountManager.NoOFReplyPerDay = Convert.ToInt32(txtMaximumNoRetweet.Text);
                    }
                    else
                    {
                        TweetAccountManager.NoOFReplyPerDay = 10;
                        AddToTweetCreatorLogs("Adding Default Maximum No Of Retweet 10");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Key, "Reply");

                    int TodayReply = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    TweetAccountManager.AlreadyReply = TodayReply;
                    if (TodayReply >= TweetAccountManager.NoOFReplyPerDay)
                    {
                        AddToLog_Tweet("Already Retweeted " + TodayReply);
                        return;
                    }
                }

                tweetAccountManager.Reply(tweetMessage, replyMinDealy, replyMaxDealy);

                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_Reply()  --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_Reply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnStartTweeting_Click(object sender, EventArgs e)
        {
            //TweetScrapper ts = new TweetScrapper();
            //ts.GetTweetData("pankaj");

            //Globussoft.GlobusHttpHelper globusHttpHelper = new Globussoft.GlobusHttpHelper();

            //string res = globusHttpHelper.getHtmlfromUrl(new Uri("https://signup.live.com/signup.aspx"), "", "");

            if (listTweetMessages.Count >= 1)
            {
                new Thread(() =>
                {
                    StartTweeting();
                }).Start();
            }
            else
            {
                MessageBox.Show("Please upload Tweet Messages File");
            }
        }



        private void StartTweeting()
        {
            try
            {
                int numberOfThreads = 7;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0 && listTweetMessages.Count >= 1)
                {
                    if (!string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtNoOfTweetThreads.Text);
                    }
                    if (!string.IsNullOrEmpty(txtNoOfTweets.Text) && Globals.IdCheck.IsMatch(txtNoOfTweets.Text))
                    {
                        TweetAccountManager.NoOfTweets = int.Parse(txtNoOfTweets.Text);
                    }

                    listTweetMessages = listTweetMessages.Distinct().ToList();

                    foreach (string item in listTweetMessages)
                    {
                        TweetAccountManager.que_TweetMessages.Enqueue(item);
                    }
                    //foreach (string item in listReplyMessages)
                    //{
                    //    TweetAccountManager.que_ReplyMessages.Enqueue(item);
                    //}

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            string tweetMessage = string.Empty;
                            //try
                            //{
                            //    tweetMessage = listTweetMessages[RandomNumberGenerator.GenerateRandom(0, listTweetMessages.Count - 1)];
                            //}
                            //catch { }

                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartTweetingMultithreaded), new object[] { item, listTweetMessages });

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweeting() -- Foreach loop Dictinary --> " + ex.Message, Globals.Path_TweetingErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweeting() -- Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("please Upload Twitter Account");
                    AddToLog_Tweet("please Upload Twitter Account");
                }
            }
            catch(Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweeting() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweeting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        
        #endregion

        private void btnStartWaitnReply_Click(object sender, EventArgs e)
        {
        
            if (!string.IsNullOrEmpty(txtKeyword.Text) || listKeywords.Count > 0)
            {
                if (listKeywords.Count == 0)
                {
                    listKeywords.Add(txtKeyword.Text);
                }
                if (!string.IsNullOrEmpty(txtNoOfTweetsperReply.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetsperReply.Text))
                {
                    TweetAccountManager.noOfTweetsPerReply = int.Parse(txtNoOfTweetsperReply.Text);
                }
                if (!string.IsNullOrEmpty(txtNoOfRepliesPerKeyword.Text) && Globals.IdCheck.IsMatch(txtNoOfRepliesPerKeyword.Text))
                {
                    TweetAccountManager.noOfRepliesPerKeyword = int.Parse(txtNoOfRepliesPerKeyword.Text);
                }
                if (!string.IsNullOrEmpty(txtInterval_WaitReply.Text) && Globals.IdCheck.IsMatch(txtInterval_WaitReply.Text))
                {
                    TweetAccountManager.WaitAndReplyInterval = int.Parse(txtInterval_WaitReply.Text) * 1000 * 2 * 30;//mins
                }
            }
            else
            {
                MessageBox.Show("Please enter a Search Keyword");
                return;
            }

            if (!(listTweetMessages.Count == 0 || listReplyMessages.Count == 0))
            {
                if (chkUseSpinnedMessages.Checked)
                {
                    //List<string> tempList = new List<string>();
                    //foreach (string item in listReplyMessages)
                    //{
                    //    tempList.Add(item);
                    //}
                    //listReplyMessages.Clear();
                    //foreach (string item in tempList)
                    //{
                    //    List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item);
                    //    listReplyMessages.AddRange(tempSpunList);
                    //}
                    ///
                    ///code on 7th 11 2012 , changed by gargi mishra
                    ///
                    //listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages);
                    ////

                    listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages, '|');

                }

                if (chkUseSpinnedMessages.Checked)
                {
                    listTweetMessages = SpinnedListGenerator.GetSpinnedList(listTweetMessages);
                }

                listTweetMessages = listTweetMessages.Distinct().ToList();
                listReplyMessages = listReplyMessages.Distinct().ToList();

                TweetAccountManager.listTweetMessages = listTweetMessages;
                TweetAccountManager.listReplyMessages = listReplyMessages;
                TweetAccountManager.que_TweetMessages_WaitAndReply.Clear();
                TweetAccountManager.que_ReplyMessages_WaitAndReply.Clear();

                foreach (string item in listTweetMessages)
                {
                    TweetAccountManager.que_TweetMessages_WaitAndReply.Enqueue(item);
                }
                foreach (string item in listReplyMessages)
                {
                    TweetAccountManager.que_ReplyMessages_WaitAndReply.Enqueue(item);
                }
            }
            else
            {
                MessageBox.Show("Please upload Tweet Messages and Replies Files");
                return;
            }

            new Thread(() =>
            {
                StartWaitAndReply();
            }).Start();
        }

        private void StartWaitAndReply()
        {
            int numberOfThreads = 7;
            if (!string.IsNullOrEmpty(txtWaitReplyThreads.Text) && Globals.IdCheck.IsMatch(txtWaitReplyThreads.Text))
            {
                numberOfThreads = int.Parse(txtWaitReplyThreads.Text);
            }


            int count_Keyword = 0;
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    if (count_Keyword >= listKeywords.Count)
                    {
                        count_Keyword = 0;
                    }
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_WaitAndReply), new object[] { item, count_Keyword });
                }
            }
            else
            {
                MessageBox.Show("Please Upload Twitter Accounts");
                AddToGeneralLogs("Please Upload Twitter Accounts");
            }
        }

        private void ThreadPoolMethod_WaitAndReply(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                int count_Keyword = (int)paramsArray.GetValue(1);

                string Keyword = listKeywords[count_Keyword];

                //string tweetMessage = (string)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.WaitAndReply), tweetAccountManager.Username);

                ///Name thread and put in threads collection
                try
                {
                    Thread.CurrentThread.Name = "WaitAndReply_" + tweetAccountManager.Username;
                    dictionary_Threads.Add("WaitAndReply_" + tweetAccountManager.Username, Thread.CurrentThread);
                }
                catch { }

                tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_WaitReply_addToLogger);
                tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_WaitReply_addToLogger;

                //tweetAccountManager.StartWaitAndReply(txtKeyword.Text);
                tweetAccountManager.StartWaitAndReply(Keyword);

                tweetAccountManager.logEvents.addToLogger -= logEvents_WaitReply_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_WaitReply_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_WaitAndReply() --  --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_WaitAndReply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        void logEvents_WaitReply_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_WaitReply(eArgs.log);
            }
        }

        private void btnUploadUsernames_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtUsernamesFilePath.Text = ofd.FileName;
                    AccountChecker.AccountChecker.lst_Usernames = new List<string>();

                    AccountChecker.AccountChecker.lst_Usernames = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    Console.WriteLine(AccountChecker.AccountChecker.lst_Usernames.Count + " Usernames loaded");
                    AddToLog_Checker(AccountChecker.AccountChecker.lst_Usernames.Count + " Usernames loaded");

                    new Thread(() =>
                    {
                        CheckAccounts();
                    }).Start();
                }
            }
        }

        private void CheckAccounts()
        {
            foreach (string item in AccountChecker.AccountChecker.lst_Usernames)
            {
                try
                {
                    string stats;

                    AccountChecker.AccountChecker accountChecker = new AccountChecker.AccountChecker();

                    string username = item.Split(':')[0];

                    if (accountChecker.IsUserAlive(username, out stats))
                    {
                        AddToLog_Checker("Account : " + item + " Exists");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_ExistingIDs);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(stats))
                        {
                            if (stats == "doesn’t exist" || stats == "404" || stats == "Not exsist")
                            {
                                AddToLog_Checker("Account : " + item + " does Not Exist");
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_NonExistingIDs);
                            }
                            if (stats == "suspended")
                            {
                                AddToLog_Checker("Account : " + item + " Suspended");
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_SuspendedIDs);
                            }
                            if (stats == "Rate limit exceeded")
                            {
                                AddToLog_Checker("Account : " + item + " Rate limit exceeded");
                                 AddToLog_Checker("Rate Limit Exceeded");
                                 AddToLog_Checker("Please Try After Some Time");
                            }
                        }
                        else
                        {
                            AddToLog_Checker("Couldn't check : " + item + "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    AddToLog_Checker(ex.Message);
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckAccounts() --> " + ex.Message, Globals.Path_AccountCheckerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckAccounts() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }

        }

        private void AddToLog_Checker(string log)
        {
            try
            {
                if (lstLogger_AccountChecker.InvokeRequired)
                {
                    lstLogger_AccountChecker.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_AccountChecker.Items.Add(log);
                        lstLogger_AccountChecker.SelectedIndex = lstLogger_AccountChecker.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_AccountChecker.Items.Add(log);
                    lstLogger_AccountChecker.SelectedIndex = lstLogger_AccountChecker.Items.Count - 1;
                }
            }
            catch { }
        }
       
        void logEvents_Follower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_Follower(eArgs.log);
            }
        }

        private void AddToLog_Follower(string log)
        {
            try
            {
                if (lstLogger_Follower.InvokeRequired)
                {
                    lstLogger_Follower.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_Follower.Items.Add(log);
                        lstLogger_Follower.SelectedIndex = lstLogger_Follower.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_Follower.Items.Add(log);
                    lstLogger_Follower.SelectedIndex = lstLogger_Follower.Items.Count - 1;
                }
            }
            catch { }
        }

        void logEvents_Tweet_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_Tweet(eArgs.log);
            }
        }

        private void AddToLog_Tweet(string log)
        {
            try
            {
                if (lstLogger_Tweet.InvokeRequired)
                {
                    lstLogger_Tweet.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_Tweet.Items.Add(log);
                        lstLogger_Tweet.SelectedIndex = lstLogger_Tweet.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_Tweet.Items.Add(log);
                    lstLogger_Tweet.SelectedIndex = lstLogger_Tweet.Items.Count - 1;
                }
            }
            catch { }
        }

        void logEvents_Profile_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToListProfile(eArgs.log);
            }
        }

        void logEvents_UnFollower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_UnFollower(eArgs.log);
            }
        }

        private void AddToLog_UnFollower(string log)
        {
            try
            {
                if (lstLogger_Unfollower.InvokeRequired)
                {
                    lstLogger_Unfollower.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_Unfollower.Items.Add(log);
                        lstLogger_Unfollower.SelectedIndex = lstLogger_Unfollower.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_Unfollower.Items.Add(log);
                    lstLogger_Unfollower.SelectedIndex = lstLogger_Unfollower.Items.Count - 1;
                }
            }
            catch { }
        }

        private void btnName_Click(object sender, EventArgs e)
        {
            
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtNames.Text = ofd.FileName;
                    listNames = new List<string>();
                    
                    //List<string> templist = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    List<string> tempFile = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in tempFile)
                    {
                        string newitem = item.Replace("\0", "").Replace(" ","");
                        if (!string.IsNullOrEmpty(newitem))
                        {
                            if (!listNames.Contains(newitem))
                            {
                                listNames.Add(newitem);
                            }
                        }
                    }
                    Console.WriteLine(listNames.Count + " Names loaded");
                    AddToListAccountsLogs(listNames.Count + " Names loaded");
                }
            }
        }

        private void btnUsernames_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtUsernames.Text = ofd.FileName;
                    listUserNames = new List<string>();
                    listUserNames.Clear();

                    List<string> templist = GlobusFileHelper.ReadLargeFile(ofd.FileName);
                    foreach (string item in templist)
                    {
                        string newitem = item.Replace("\0", "").Replace(" ", "");
                        if (!string.IsNullOrEmpty(newitem))
                        {
                            if (!listUserNames.Contains(newitem))
                            {
                                listUserNames.Add(newitem);
                            }
                        }
                    }
                    Console.WriteLine(listUserNames.Count + " Usernames loaded");
                    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");


                }
            }
        }

        private void btnEmails_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtEmails.Text = ofd.FileName;
                    Globals.EmailsFilePath = ofd.FileName;
                    listEmails = new List<string>();
                    listEmails.Clear();
                    LoadEmails(ofd.FileName);
                   
                    Console.WriteLine(listEmails.Count + " Emails loaded");
                    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                }
            }
        }

        private void btnStartAccountCreation_Click(object sender, EventArgs e)
        {
            try
            {
                ///Updates new Paths in tb_Setting
                if (!string.IsNullOrEmpty(txtNames.Text) && !string.IsNullOrEmpty(txtUsernames.Text) && !string.IsNullOrEmpty(txtEmails.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("AccountCreator", "Name", StringEncoderDecoder.Encode(txtNames.Text));
                    objclsSettingDB.InsertOrUpdateSetting("AccountCreator", "Username", StringEncoderDecoder.Encode(txtUsernames.Text));
                    objclsSettingDB.InsertOrUpdateSetting("AccountCreator", "Email", StringEncoderDecoder.Encode(txtEmails.Text));
                }

                if (!chkUseDBC.Checked)
                {
                    AddToListAccountsLogs("Please Check Death By Capctha To solve Capctha");
                    return;
                }
                else if (chkUseDBC.Checked)
                {
                    if (string.IsNullOrEmpty(Globals.DBCUsername) && string.IsNullOrEmpty(Globals.DBCPassword))
                    {
                        MessageBox.Show("Please load Death By Captcha Details from Settings Menu");
                        return;
                    }
                    else
                    {
                        AddToListAccountsLogs("Death By Captcha Details Loaded");
                        AddToListAccountsLogs("UserName : " + Globals.DBCUsername);
                    }
                }

                if (chkboxUseFakeEmailAccounts.Checked && Globals.FakeEmailList.Count <= 0)
                {
                    AddToListAccountsLogs("Please Create Fake Email Accounts First");
                    return;
                }


                //StartSignUpMultithreaded();
                Thread thread_StartSignup = new Thread(StartSignUpMultithreaded);
                thread_StartSignup.Start();
            }
            catch(Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartAccountCreation_Click() --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartAccountCreation_Click() --> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
        }

        private void StartSignUpMultithreaded()
        {
            try
            {
                clsDBQueryManager SetDb = new clsDBQueryManager();
                DataSet ds = new DataSet();
                //bool UsePublicProxy = false;
                if (chkboxPublicProxy.Checked)
                {
                    ds = SetDb.SelectPublicProxyData();
                }
                else if (chkboxPrivateProxy.Checked)
                {
                    ds = SetDb.SelectPrivateProxyData();
                }

                if (chkboxUseFakeEmailAccounts.Checked)
                {
                    listEmails = Globals.FakeEmailList;
                }

                int numberOfThreads = 10;

                if (Globals.IdCheck.IsMatch(txtNumberOfThreads.Text) && !string.IsNullOrEmpty(txtNumberOfThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNumberOfThreads.Text);
                }

                TwitterSignup.TwitterSignUp twitterSignUp = new TwitterSignup.TwitterSignUp();

                int counter_Name = 0;
                int counter_Username = 0;

                listEmails = listEmails.Distinct().ToList();

                if (listEmails.Count > 0)
                {
                    foreach (string email in listEmails)
                    {
                        string Proxy = string.Empty;
                        if (chkboxPublicProxy.Checked)
                        {
                            try
                            {
                                if (ds.Tables[0].Rows.Count != 0)
                                {
                                    int index = RandomNumberGenerator.GenerateRandom(0, ds.Tables[0].Rows.Count);
                                    DataRow dr = ds.Tables[0].Rows[index];
                                    Proxy = dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString() + ":" + dr.ItemArray[2].ToString() + ":" + dr.ItemArray[3].ToString();
                                }
                                else
                                {
                                    AddToListAccountsLogs("Please Uplaod Public Proxies");
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Getting Public Proxy From Data Bas --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting Public Proxy From Data Base >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                            }
                        }
                        else if (chkboxPrivateProxy.Checked)
                        {
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                int index = RandomNumberGenerator.GenerateRandom(0, ds.Tables[0].Rows.Count);
                                DataRow dr = ds.Tables[0].Rows[index];
                                Proxy = dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString() + ":" + dr.ItemArray[2].ToString() + ":" + dr.ItemArray[3].ToString();
                            }
                            else
                            {
                                AddToListAccountsLogs("Please Upload Private Proxy");
                                break;
                            }
                        }


                        if (listUserNames.Count > 0)
                        {
                            if (counter_Username < listUserNames.Count)
                            {
                            }
                            else
                            {
                                AddToListAccountsLogs("*********** /n All Usernames have been taken /n ***********");
                                break;
                            }
                        }
                        else
                        {
                            AddToListAccountsLogs("Please Upload Usernames To Create Twitter Accounts");
                            break;
                        }


                        if (listNames.Count > 0)
                        {
                            if (counter_Name < listNames.Count)
                            {
                            }
                            else
                            {
                                counter_Name = 0;
                            }
                        }
                        else
                        {
                            AddToListAccountsLogs("Please Upload Names To Create Twitter Accounts");
                            break;
                        }



                        string username = string.Empty;
                        string name = string.Empty;

                        try
                        {
                            username = listUserNames[counter_Username];
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Getting UserName for Twt Account Creator --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting UserName for Twt Account Creator >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                        }

                        try
                        {
                            name = listNames[counter_Name];
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Getting Name for Twt Account Creator --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting Name for Twt Account Creator >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                        }

                        ThreadPool.SetMaxThreads(numberOfThreads, 5);
                        //twitterSignUp.SignupMultiThreaded(new object[] { email, username, name, Proxy });
                        ThreadPool.QueueUserWorkItem(new WaitCallback(twitterSignUp.SignupMultiThreaded), new object[] { email, username, name, Proxy });
                        counter_Name++;
                        counter_Username++;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    MessageBox.Show("Please Add Email Accounts to Create Accounts");
                    AddToListAccountsLogs("Please Add Email Accounts to Create Accounts");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> In StartSignUpMultithreaded() --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> In StartSignUpMultithreaded() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
        }

        private void LoadDefaultsAccountCreator()
        {
            try
            {
                #region FB Ubdate by me
                listNames.Clear();
                listUserNames.Clear();
                listEmails.Clear();

                string NameSettings = string.Empty;
                string UsernameSettings = string.Empty;
                string emailSettings = string.Empty;

                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                  
                    if ("Name" == row[1].ToString())
                    {
                        NameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Username" == row[1].ToString())
                    {
                        UsernameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Email" == row[1].ToString())
                    {
                        emailSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                }
               
                if (File.Exists(NameSettings))
                {
                    listNames = GlobusFileHelper.ReadFiletoStringList(NameSettings);
                    txtNames.Text = NameSettings;
                    AddToListAccountsLogs(listNames.Count + " Names loaded");
                }
                if (File.Exists(UsernameSettings))
                {
                    listUserNames = GlobusFileHelper.ReadFiletoStringList(UsernameSettings);
                    txtUsernames.Text = UsernameSettings;
                    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");
                }
                if (File.Exists(emailSettings))
                {
                    listEmails = GlobusFileHelper.ReadFiletoStringList(emailSettings);
                    txtEmails.Text = emailSettings;
                    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultsProfileData()
        {
            try
            {

                lst_ProfileDescriptions = new List<string>();
                lst_ProfileUsernames = new List<string>();
                lst_ProfileURLs = new List<string>();
                lst_ProfileLocations = new List<string>();
                lstProfilePics = new List<string>();

                string Profile_Username_Settings = string.Empty;
                string Profile_Location_Settings = string.Empty;
                string Profile_URL_Settings = string.Empty;
                string Profile_Description_Settings = string.Empty;


                //Globussoft1.GlobusHttpHelper
                string PicsFolderSettings = string.Empty;
                string ProfileFolderSettings = string.Empty;
                DataTable dt = objclsSettingDB.SelectSettingData();
                #region MyRegion
                //foreach (DataRow row in dt.Rows)
                //{

                //    if ("Profile_Username" == row[1].ToString())
                //    {
                //        Profile_Username_Settings = StringEncoderDecoder.Decode(row[2].ToString());
                //    }
                //    if ("Profile_Location" == row[1].ToString())
                //    {
                //        Profile_Location_Settings = StringEncoderDecoder.Decode(row[2].ToString());
                //    }
                //    if ("Profile_URL" == row[1].ToString())
                //    {
                //        Profile_URL_Settings = StringEncoderDecoder.Decode(row[2].ToString());
                //    }
                //    if ("Profile_Description" == row[1].ToString())
                //    {
                //        Profile_Description_Settings = StringEncoderDecoder.Decode(row[2].ToString());
                //    }
                //} 
                #endregion

                foreach (DataRow row in dt.Rows)
                {

                    if ("PicsFolder" == row[1].ToString())
                    {
                        PicsFolderSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("ProfileFolder" == row[1].ToString())
                    {
                        ProfileFolderSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                }

                
                if (Directory.Exists(PicsFolderSettings))
                {
                    txtProfilePicsFolder.Text = PicsFolderSettings;
                    string[] picsArray = Directory.GetFiles(PicsFolderSettings);
                    lstProfilePics = picsArray.ToList();
                    AddToListProfile(lstProfilePics.Count + " Profile Images Loaded");

                }
                if (Directory.Exists(ProfileFolderSettings))
                {
                    txtAccountsFolder.Text = ProfileFolderSettings;
                    LoadProfileData(ProfileFolderSettings);
                }

                //if (File.Exists(Profile_Username_Settings))
                //{
                //    lst_ProfileUsernames = GlobusFileHelper.ReadFiletoStringList(Profile_Username_Settings);
                //    txtNames.Text = Profile_Username_Settings;
                //    AddToListAccountsLogs(listNames.Count + " Profile Usernames loaded");
                //}
                //if (File.Exists(Profile_Location_Settings))
                //{
                //    lst_ProfileLocations = GlobusFileHelper.ReadFiletoStringList(Profile_Location_Settings);
                //    txtUsernames.Text = Profile_Location_Settings;
                //    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");
                //}
                //if (File.Exists(Profile_URL_Settings))
                //{
                //    lst_ProfileURLs = GlobusFileHelper.ReadFiletoStringList(Profile_URL_Settings);
                //    txtEmails.Text = Profile_URL_Settings;
                //    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                //}
                //if (File.Exists(Profile_Description_Settings))
                //{
                //    lst_ProfileDescriptions = GlobusFileHelper.ReadFiletoStringList(Profile_URL_Settings);
                //    txtEmails.Text = Profile_URL_Settings;
                //    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultsFollowData()
        {
            try
            {
                #region FB Ubdate by me
                listNames.Clear();
                listUserNames.Clear();
                listEmails.Clear();

                string NameSettings = string.Empty;
                string UsernameSettings = string.Empty;
                string emailSettings = string.Empty;

                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {

                    if ("Name" == row[1].ToString())
                    {
                        NameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Username" == row[1].ToString())
                    {
                        UsernameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Email" == row[1].ToString())
                    {
                        emailSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                }

                if (File.Exists(NameSettings))
                {
                    listNames = GlobusFileHelper.ReadFiletoStringList(NameSettings);
                    txtNames.Text = NameSettings;
                    AddToListAccountsLogs(listNames.Count + " Names loaded");
                }
                if (File.Exists(UsernameSettings))
                {
                    listUserNames = GlobusFileHelper.ReadFiletoStringList(UsernameSettings);
                    txtUsernames.Text = UsernameSettings;
                    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");
                }
                if (File.Exists(emailSettings))
                {
                    listEmails = GlobusFileHelper.ReadFiletoStringList(emailSettings);
                    txtEmails.Text = emailSettings;
                    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultsTweetsData()
        {
            try
            {
                #region FB Ubdate by me
                listNames.Clear();
                listUserNames.Clear();
                listEmails.Clear();

                string NameSettings = string.Empty;
                string UsernameSettings = string.Empty;
                string emailSettings = string.Empty;

                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {

                    if ("Name" == row[1].ToString())
                    {
                        NameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Username" == row[1].ToString())
                    {
                        UsernameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                    if ("Email" == row[1].ToString())
                    {
                        emailSettings = StringEncoderDecoder.Decode(row[2].ToString());
                    }
                }

                if (File.Exists(NameSettings))
                {
                    listNames = GlobusFileHelper.ReadFiletoStringList(NameSettings);
                    txtNames.Text = NameSettings;
                    AddToListAccountsLogs(listNames.Count + " Names loaded");
                }
                if (File.Exists(UsernameSettings))
                {
                    listUserNames = GlobusFileHelper.ReadFiletoStringList(UsernameSettings);
                    txtUsernames.Text = UsernameSettings;
                    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");
                }
                if (File.Exists(emailSettings))
                {
                    listEmails = GlobusFileHelper.ReadFiletoStringList(emailSettings);
                    txtEmails.Text = emailSettings;
                    AddToListAccountsLogs(listEmails.Count + " Emails loaded");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadEmails(string emailFile)
        {
            List<string> tempList = GlobusFileHelper.ReadLargeFile(emailFile);
            foreach (string item in tempList)
            {
                string newitem = item.Replace("\0", "").Replace(" ", "");
                if (!string.IsNullOrEmpty(newitem))
                {
                    if (!listEmails.Contains(newitem))
                    {
                        listEmails.Add(newitem);
                    }
                }
            }
        }

        private void AddToListAccountsLogs(string log)
        {
            try
            {
                if (lstBoxAccountsLogs.InvokeRequired)
                {
                    lstBoxAccountsLogs.Invoke(new MethodInvoker(delegate
                        {
                            lstBoxAccountsLogs.Items.Add(log);
                            lstBoxAccountsLogs.SelectedIndex = lstBoxAccountsLogs.Items.Count - 1;
                        }));
                }
                else
                {
                    lstBoxAccountsLogs.Items.Add(log);
                    lstBoxAccountsLogs.SelectedIndex = lstBoxAccountsLogs.Items.Count - 1;
                }
            }
            catch { }
        }

        private void AddToProxysLogs(string log)
        {
            try
            {
                if (lstLoggerProxy.InvokeRequired)
                {
                    lstLoggerProxy.Invoke(new MethodInvoker(delegate
                    {
                        lstLoggerProxy.Items.Add(log);
                        lstLoggerProxy.SelectedIndex = lstLoggerProxy.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLoggerProxy.Items.Add(log);
                    lstLoggerProxy.SelectedIndex = lstLoggerProxy.Items.Count - 1;
                }

                lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                {
                    lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                }));
            }
            catch { }
        }

        private void AddToScrapeLogs(string log)
        {
            try
            {
                if (lstLoggerScrape.InvokeRequired)
                {
                    lstLoggerScrape.Invoke(new MethodInvoker(delegate
                    {
                        lstLoggerScrape.Items.Add(log);
                        lstLoggerScrape.SelectedIndex = lstLoggerScrape.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLoggerScrape.Items.Add(log);
                    lstLoggerScrape.SelectedIndex = lstLoggerScrape.Items.Count - 1;
                }
            }
            catch { }
        }

        private void AddToDMLog(string log)
        {
            try
            {
                if (lstDMLogger.InvokeRequired)
                {
                    lstDMLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstDMLogger.Items.Add(log);
                        lstDMLogger.SelectedIndex = lstDMLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstDMLogger.Items.Add(log);
                    lstDMLogger.SelectedIndex = lstDMLogger.Items.Count - 1;
                }
            }
            catch { }
        }

        private void AddToProxyAccountCreationLog(string log)
        {
            try
            {

                if (lstLoggerBrowserAccountCreation.InvokeRequired)
                {
                    lstLoggerBrowserAccountCreation.Invoke(new MethodInvoker(delegate
                    {
                        lstLoggerBrowserAccountCreation.Items.Add(log);
                        lstLoggerBrowserAccountCreation.SelectedIndex = lstLoggerBrowserAccountCreation.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLoggerBrowserAccountCreation.Items.Add(log);
                    lstLoggerBrowserAccountCreation.SelectedIndex = lstLoggerBrowserAccountCreation.Items.Count - 1;
                }
            }
            catch { }
        }

        FrmSettings frmSettings;

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (frmSettings == null)
            {
                frmSettings = new FrmSettings();
            }

            frmSettings.Show();        
        }

        private void btnStopAccountCreation_Click(object sender, EventArgs e)
        {

        }

        private void ButonTest_Click(object sender, EventArgs e)
        {
            UserControl abc = new UserControl();
            abc.Show();
            //TweetAccountManager tweetAccountManager = new TweetAccountManager();
            //tweetAccountManager.Username = "sociopro";
            //tweetAccountManager.Password = "qwerty1234";
            ////tweetAccountManager.proxyAddress = "173.208.131.234";
            ////tweetAccountManager.proxyPort = "8888";
            ////tweetAccountManager.proxyUsername = "usproxy";
            ////tweetAccountManager.proxyPassword = "logic";


            //tweetAccountManager.follower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
            ////tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;
            //tweetAccountManager.Login();
            ////tweetAccountManager.UpdateProfile();

            //string unfollowuserId = "569071036";
            //string unfollowpost_authenticity_token = tweetAccountManager.postAuthenticityToken;
            //string unfollowpostdata = "user_id=" + unfollowuserId + "&post_authenticity_token=" + tweetAccountManager.postAuthenticityToken;
            //////string unfollowresponse = tweetAccountManager.globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/friendships/destroy.json"), unfollowpostdata, "https://api.twitter.com/receiver.html", "", "XMLHttpRequest", "true", "");
            //string res_PostFollow = tweetAccountManager.globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/friendships/destroy.json"), unfollowpostdata, "https://api.twitter.com/receiver.html", string.Empty, "XMLHttpRequest", "true", "https://api.twitter.com");

            //ParseJson();

            GlobusHttpHelper httpHelper = new GlobusHttpHelper();
            string res = httpHelper.getHtmlfromUrl(new Uri("http://www.wotif.com/hotel/View?hotel=W1711"), "", "");
        }

        private void btnAccountsFolder_Click(object sender, EventArgs e)
        {
            lst_ProfileLocations.Clear();
            lst_ProfileDescriptions.Clear();
            lst_ProfileURLs.Clear();
            lst_ProfileUsernames.Clear();
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                //ofd.RootFolder = Environment.SpecialFolder.ProgramFiles;
                ofd.SelectedPath = Application.StartupPath + "\\Profile";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtAccountsFolder.Text = ofd.SelectedPath;
                    LoadProfileData(ofd.SelectedPath);
                }
            }
        }

        private void LoadProfileData(string folderPath)
        {
            string[] files = System.IO.Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                foreach (string item in files)
                {
                    if (item.Contains("ProfileLocations.txt"))
                    {
                        lst_ProfileLocations = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListProfile(lst_ProfileLocations.Count + " Locations Loaded");
                    }

                    if (item.Contains("ProfileUsernames.txt"))
                    {
                        lst_ProfileUsernames = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListProfile(lst_ProfileUsernames.Count + " Usernames Loaded");
                    }

                    if (item.Contains("ProfileURLs.txt"))
                    {
                        lst_ProfileURLs = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListProfile(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                    }

                    if (item.Contains("ProfileDescriptions.txt"))
                    {
                        lst_ProfileDescriptions = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListProfile(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");

                    }
                }

                if (lst_ProfileLocations.Count <= 0)
                {
                    AddToListProfile("No Profile Locations Uploaded From Text File");
                }
                if (lst_ProfileUsernames.Count <= 0)
                {
                    AddToListProfile("No UserNames Uplaoded From Text File");
                }
                if (lst_ProfileURLs.Count <= 0)
                {
                    AddToListProfile("No Profile Url's Uploaded From Text File");
                }
                if (lst_ProfileDescriptions.Count <= 0)
                {
                    AddToListProfile("No Profile Descriptions Uploaded From Text File");
                }
            }
            else
            {
                AddToListProfile("Please Upload Text Files. Folder is Empty");
            }
        }

        private void AddToListProfile(string log)
        {
            try
            {
                if (lstLogger_Profile.InvokeRequired)
                {
                    lstLogger_Profile.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_Profile.Items.Add(log);
                        lstLogger_Profile.SelectedIndex = lstLogger_Profile.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_Profile.Items.Add(log);
                    lstLogger_Profile.SelectedIndex = lstLogger_Profile.Items.Count - 1;
                }
            }
            catch { }
        }

        private void btnProfilePicsFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtProfilePicsFolder.Text = ofd.SelectedPath;
                    lstProfilePics = new List<string>();
                    string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                    foreach (string picFile in picsArray)
                    {
                        if (picFile.Contains(".jpg") || picFile.Contains(".gif") || picFile.Contains(".png"))
                        {
                            lstProfilePics.Add(picFile);
                        }
                        else
                        {
                            AddToListProfile("Not Correct Image File");
                            AddToListProfile(picFile);
                        }
                    }
                    
                    Console.WriteLine(lstProfilePics.Count + " Proflile Pics loaded");
                    AddToListProfile(lstProfilePics.Count + " Profile Images loaded");
                }
            }
        }

        private void btnStartProfileCreation_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtProfilePicsFolder.Text) && !string.IsNullOrEmpty(txtAccountsFolder.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "PicsFolder", StringEncoderDecoder.Encode(txtProfilePicsFolder.Text));
                objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "ProfileFolder", StringEncoderDecoder.Encode(txtAccountsFolder.Text));
                if ((lst_ProfileLocations.Count > 0) && (lst_ProfileUsernames.Count > 0) && (lst_ProfileDescriptions.Count > 0) && (lst_ProfileURLs.Count > 0))
                {
                    new Thread(() => StartProfileCreation()).Start();
                }
                else
                {
                    if (lst_ProfileLocations.Count <=0)
                    {
                        AddToListProfile(lst_ProfileLocations.Count + " Locations Loaded");
                        AddToListProfile("Please Add Locations");
                        MessageBox.Show(lst_ProfileLocations.Count + " Locations Loaded");
                    }
                    else if (lst_ProfileUsernames.Count <= 0)
                    {
                        AddToListProfile(lst_ProfileUsernames.Count + " Usernames Loaded");
                        AddToListProfile("Please Add UserNames");
                        MessageBox.Show(lst_ProfileUsernames.Count + " Usernames Loaded");
                    }
                    else if (lst_ProfileURLs.Count <= 0)
                    {
                        AddToListProfile(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                        AddToListProfile("Please Add Profile Url's");
                        MessageBox.Show(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                    }
                    else if (lst_ProfileDescriptions.Count <= 0)
                    {
                        AddToListProfile(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
                        AddToListProfile("Please Add DEscription");
                        MessageBox.Show(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
                    }
                }
                //btnStartProfileCreation.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please load Profile Data");
            }
        }


        ///old Profile Manager code
        //private void StartProfileCreation()
        //{
        //    int numberOfThreads = 7;
        //    //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
        //    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
        //    {
        //        if (!string.IsNullOrEmpty(txtNoOfProfileThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfProfileThreads.Text))
        //        {
        //            numberOfThreads = int.Parse(txtNoOfProfileThreads.Text);
        //        }
               
        //        ThreadPool.SetMaxThreads(numberOfThreads, 5);

        //        int count_profileDescription = 0;
        //        int count_profileUsername = 0;
        //        int count_profileURL = 0;
        //        int count_profileLocation = 0;
        //         int count_profilePic = 0;

        //        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
        //        {
        //            string profileUsername = string.Empty;
        //            string profileURL = string.Empty;
        //            string profileDescription = string.Empty;
        //            string profileLocation = string.Empty;
        //            string profilePic = string.Empty;

        //            try
        //            {
        //                try
        //                {
        //                    if (count_profileUsername < lst_ProfileUsernames.Count)
        //                    {
        //                        profileUsername = lst_ProfileUsernames[count_profileUsername];
        //                    }
        //                    else
        //                    {
        //                        count_profileUsername = 0;
        //                        profileUsername = lst_ProfileUsernames[count_profileUsername];
        //                    }
        //                }
        //                catch(Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Getting Profile Username --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Getting Profile Username --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                try
        //                {
        //                    if (count_profileURL < lst_ProfileURLs.Count)
        //                    {
        //                        profileURL = lst_ProfileURLs[count_profileURL];
        //                    }
        //                    else
        //                    {
        //                        count_profileURL = 0;
        //                        profileURL = lst_ProfileURLs[count_profileURL];
        //                    }
        //                }
        //                catch(Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Getting Profile Url --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Getting Profile Url --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                try
        //                {
        //                    if (count_profileDescription < lst_ProfileDescriptions.Count)
        //                    {
        //                        profileDescription = lst_ProfileDescriptions[count_profileDescription];
        //                    }
        //                    else
        //                    {
        //                        count_profileDescription = 0;
        //                        profileDescription = lst_ProfileDescriptions[count_profileDescription];
        //                    }
        //                }
        //                catch(Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Getting Profile Description --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Getting Profile Description --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                try
        //                {
        //                    if (count_profileLocation < lst_ProfileLocations.Count)
        //                    {
        //                        profileLocation = lst_ProfileLocations[count_profileLocation];
        //                    }
        //                    else
        //                    {
        //                        count_profileLocation = 0;
        //                        profileLocation = lst_ProfileLocations[count_profileLocation];
        //                    }
        //                }
        //                catch(Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Getting Profile Location --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Getting Profile Location --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                try
        //                {
        //                    if (count_profilePic < lstProfilePics.Count)
        //                    {
        //                        profilePic = lstProfilePics[count_profilePic];
        //                    }
        //                    else
        //                    {
        //                        count_profilePic = 0;
        //                        profilePic = lstProfilePics[count_profilePic];
        //                    }
        //                }
        //                catch(Exception ex)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Getting Profile Pics --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Getting Profile Pics --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                ThreadPool.SetMaxThreads(numberOfThreads, 5);

        //                #region Commented
        //                //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
        //                //{
                            
        //                    //TweetAccountManager tweetAccountManager = item.Value;

        //                    //string accountUser = tweetAccountManager.Username;
        //                    //string accountPass = tweetAccountManager.Password;
        //                    //string proxyAddress = tweetAccountManager.proxyAddress;
        //                    //string proxyPort = tweetAccountManager.proxyPort;
        //                    //string proxyUserName = tweetAccountManager.proxyUsername;
        //                    //string proxyPassword = tweetAccountManager.proxyPassword;

        //                    //tweetAccountManager = new TweetAccountManager();
        //                    //tweetAccountManager.Username = accountUser;
        //                    //tweetAccountManager.Password = accountPass;
        //                    //tweetAccountManager.proxyAddress = proxyAddress;
        //                    //tweetAccountManager.proxyPort = proxyPort;
        //                    //tweetAccountManager.proxyUsername = proxyUserName;
        //                    //tweetAccountManager.proxyPassword = proxyPassword;

        //                    //tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);
        //                    //tweetAccountManager.profileUpdater.logEvents.addToLogger += logEvents_Profile_addToLogger;

        //                    //tweetAccountManager.Login();
        //                    //tweetAccountManager.UpdateProfile(profileUsername, profileLocation, profileURL, profileDescription, profilePic);

        //                    //tweetAccountManager.profileUpdater.logEvents.addToLogger -= logEvents_Profile_addToLogger;
        //                    //tweetAccountManager.logEvents.addToLogger -= logEvents_Profile_addToLogger; 



        //                //}));
        //                    #endregion

        //                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_ProfileManager), new object[] { item, new string[]{profileUsername, profileLocation, profileURL, profileDescription, profilePic}});

        //                count_profileUsername++;
        //                count_profileURL++;
        //                count_profileDescription++;
        //                count_profileLocation++;
        //                count_profilePic++;

        //                Thread.Sleep(1000);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.StackTrace);
        //                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- Foreach Loop --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
        //                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() -- Foreach Loop --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please Upload Twitter Account");
        //        AddToLog_Follower("Please Upload Twitter Account");
        //    }
        //}


        /// <summary>
        /// Profile Manager change on 8th August 2012 , By Abhishek Added Setting
        /// </summary>
        private void StartProfileCreation()
        {
            int numberOfThreads = 7;
            //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtNoOfProfileThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfProfileThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNoOfProfileThreads.Text);
                }

                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                int count_profileDescription = 0;
                int count_profileUsername = 0;
                int count_profileURL = 0;
                int count_profileLocation = 0;
                int count_profilePic = 0;

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    string profileUsername = string.Empty;
                    string profileURL = string.Empty;
                    string profileDescription = string.Empty;
                    string profileLocation = string.Empty;
                    string profilePic = string.Empty;

                    try
                    {
                        try
                        {
                            //********Coded By abhishek************

                            if (chkboxUsername.Checked)
                            {
                                if (count_profileUsername < lst_ProfileUsernames.Count)
                                {
                                    profileUsername = lst_ProfileUsernames[count_profileUsername];
                                }
                                else
                                {
                                    count_profileUsername = 0;
                                    profileUsername = lst_ProfileUsernames[count_profileUsername];
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxUsername --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxUsername --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }


                        try
                        {
                            //Change By abhishek 

                            if (chkboxProfileUrl.Checked)
                            {
                                if (count_profileURL < lst_ProfileURLs.Count)
                                {
                                    profileURL = lst_ProfileURLs[count_profileURL];
                                }
                                else
                                {
                                    count_profileURL = 0;
                                    profileURL = lst_ProfileURLs[count_profileURL];
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxProfileUrl --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxProfileUrl --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        try
                        {
                            //Change by Abhishek 

                            if (chkboxDescription.Checked)
                            {
                                if (count_profileDescription < lst_ProfileDescriptions.Count)
                                {
                                    profileDescription = lst_ProfileDescriptions[count_profileDescription];
                                }
                                else
                                {
                                    count_profileDescription = 0;
                                    profileDescription = lst_ProfileDescriptions[count_profileDescription];
                                }
                            }
                            else
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxDescription --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxDescription --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        try
                        {
                            //change By abhishek 

                            if (chkboxProfileLocation.Checked)
                            {
                                if (count_profileLocation < lst_ProfileLocations.Count)
                                {
                                    profileLocation = lst_ProfileLocations[count_profileLocation];
                                }
                                else
                                {
                                    count_profileLocation = 0;
                                    profileLocation = lst_ProfileLocations[count_profileLocation];
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxProfileLocation --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxProfileLocation --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }


                        //Change By abhishek 
                        try
                        {
                            if (chkboxPics.Checked)
                            {
                                if (count_profilePic < lstProfilePics.Count)
                                {
                                    profilePic = lstProfilePics[count_profilePic];
                                }
                                else
                                {
                                    count_profilePic = 0;
                                    profilePic = lstProfilePics[count_profilePic];
                                }
                            }
                            else
                            {

                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxPics --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxPics --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }



                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        #region Commented
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                        //{

                        //TweetAccountManager tweetAccountManager = item.Value;

                        //string accountUser = tweetAccountManager.Username;
                        //string accountPass = tweetAccountManager.Password;
                        //string proxyAddress = tweetAccountManager.proxyAddress;
                        //string proxyPort = tweetAccountManager.proxyPort;
                        //string proxyUserName = tweetAccountManager.proxyUsername;
                        //string proxyPassword = tweetAccountManager.proxyPassword;

                        //tweetAccountManager = new TweetAccountManager();
                        //tweetAccountManager.Username = accountUser;
                        //tweetAccountManager.Password = accountPass;
                        //tweetAccountManager.proxyAddress = proxyAddress;
                        //tweetAccountManager.proxyPort = proxyPort;
                        //tweetAccountManager.proxyUsername = proxyUserName;
                        //tweetAccountManager.proxyPassword = proxyPassword;

                        //tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);
                        //tweetAccountManager.profileUpdater.logEvents.addToLogger += logEvents_Profile_addToLogger;

                        //tweetAccountManager.Login();
                        //tweetAccountManager.UpdateProfile(profileUsername, profileLocation, profileURL, profileDescription, profilePic);

                        //tweetAccountManager.profileUpdater.logEvents.addToLogger -= logEvents_Profile_addToLogger;
                        //tweetAccountManager.logEvents.addToLogger -= logEvents_Profile_addToLogger; 



                        //}));
                        #endregion

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_ProfileManager), new object[] { item, new string[] { profileUsername, profileLocation, profileURL, profileDescription, profilePic } });

                        count_profileUsername++;
                        count_profileURL++;
                        count_profileDescription++;
                        count_profileLocation++;
                        count_profilePic++;

                        Thread.Sleep(1500);
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText(ex.Message);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation()  --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Upload Twitter Account");
                AddToLog_Follower("Please Upload Twitter Account");
            }
        }

        private void ThreadPoolMethod_ProfileManager(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                string[] array_ProfileParameters = new string[5];
                array_ProfileParameters = (string[])paramsArray.GetValue(1);

                string profileUsername = string.Empty;
                string profileURL = string.Empty;
                string profileDescription = string.Empty;
                string profileLocation = string.Empty;
                string profilePic = string.Empty;

                profileUsername = array_ProfileParameters[0];
                profileLocation = array_ProfileParameters[1];
                profileURL = array_ProfileParameters[2];
                profileDescription = array_ProfileParameters[3];
                profilePic = array_ProfileParameters[4];


                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.ProfileManager), tweetAccountManager.Username);


                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string proxyAddress = tweetAccountManager.proxyAddress;
                //string proxyPort = tweetAccountManager.proxyPort;
                //string proxyUserName = tweetAccountManager.proxyUsername;
                //string proxyPassword = tweetAccountManager.proxyPassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.proxyAddress = proxyAddress;
                //tweetAccountManager.proxyPort = proxyPort;
                //tweetAccountManager.proxyUsername = proxyUserName;
                //tweetAccountManager.proxyPassword = proxyPassword;


                tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);
                tweetAccountManager.profileUpdater.logEvents.addToLogger += logEvents_Profile_addToLogger;

                //tweetAccountManager.Login();
                tweetAccountManager.UpdateProfile(profileUsername, profileLocation, profileURL, profileDescription, profilePic);

                tweetAccountManager.profileUpdater.logEvents.addToLogger -= logEvents_Profile_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Profile_addToLogger; 
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_ProfileManager()  --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_ProfileManager() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnUploadReplyFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtReplyMsgFile.Text = ofd.FileName;
                    listReplyMessages = new List<string>();

                    listReplyMessages = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    //Code to find links in list of messages and make them unique
                    listReplyMessages = GetUniqueLinksList(listReplyMessages);

                    
                    Console.WriteLine(listReplyMessages.Count + " Reply Messages loaded");
                    AddToLog_WaitReply(listReplyMessages.Count + " Reply Messages loaded");
                }
            }
        }

        private List<string> GetUniqueLinksList(List<string> inputList)
        {
            List<string> tempList = new List<string>();
            //foreach (string item in inputList)
            //{
            //    tempList.Add(item);                
            //}
            foreach (string item in inputList)
            {
                string uniqueLink_Data = MakeLinksUnique(item);
                tempList.Add(uniqueLink_Data);
            }

            return tempList;
        }

        private string MakeLinksUnique(string Data)
        {
            List<string> list_modified = new List<string>();

            List<string> list_hrefs = GlobusRegex.GetHrefsFromString(Data);
            foreach (string href in list_hrefs)
            {
                string unique_href = href + "?=" + RandomStringGenerator.RandomString(5);
                Data = Data.Replace(href, unique_href);
            }

            return Data;
        }

        private void btnUploadTweetFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtTweetMsgFile.Text = ofd.FileName;
                    listTweetMessages = new List<string>();

                    listTweetMessages = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    Console.WriteLine(listTweetMessages.Count + " Tweet Messages loaded");
                    AddToLog_WaitReply(listTweetMessages.Count + " Tweet Messages loaded");
                }
            }
        }

        private void btnKeywordFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtKeywordFile.Text = ofd.FileName;
                    listKeywords = new List<string>();

                    listKeywords = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    Console.WriteLine(listKeywords.Count + " Keywords loaded");
                    AddToLog_WaitReply(listKeywords.Count + " Keywords loaded");
                }
            }
        }

        private void AddToLog_WaitReply(string log)
        {
            try
            {
                if (lstLogger_WaitReply.InvokeRequired)
                {
                    lstLogger_WaitReply.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_WaitReply.Items.Add(log);
                        lstLogger_WaitReply.SelectedIndex = lstLogger_WaitReply.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_WaitReply.Items.Add(log);
                    lstLogger_WaitReply.SelectedIndex = lstLogger_WaitReply.Items.Count - 1;
                }
            }
            catch { }
        }

        private void btnEmailverification_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtNames.Text = ofd.FileName;
                    listNames = new List<string>();
                    listNames.Clear();

                    List<string> templist = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in templist)
                    {
                        if (!listNames.Contains(item))
                        {
                            listNames.Add(item);
                        }
                    }
                    Console.WriteLine(listNames.Count + " Names loaded");
                    AddToListAccountsLogs(listNames.Count + " Names loaded");


                }
            }
        }

        private void btnTestPublicProxy_Click(object sender, EventArgs e)
        {
            AddToProxysLogs("Exsisting Proxies Saved To :");
            AddToProxysLogs(Globals.Path_ExsistingProxies);
            List<string> lstProxies = new List<string>();
            if (!string.IsNullOrEmpty(txtPublicProxy.Text))
            {
                lstProxies = Globussoft.GlobusFileHelper.ReadFiletoStringList(txtPublicProxy.Text);
                AddToProxysLogs(lstProxies.Count() + " Public Proxies Uploaded");
                new Thread(() =>
                {
                    GetValidProxies(lstProxies);
                }).Start();
            }
            else
            {
                AddToProxysLogs("Please Select a text File With Public Proxies");
            }
        }

        private void GetValidProxies(List<string> lstProxies)
        {
            if (GlobusRegex.ValidateNumber(txtNumberOfProxyThreads.Text))
            {
                numberOfProxyThreads = int.Parse(txtNumberOfProxyThreads.Text);
            }

            WaitCallback waitCallBack = new WaitCallback(ThreadPoolMethod_Proxies);
            foreach (string item in lstProxies)
            {
                if (countParseProxiesThreads >= numberOfProxyThreads)
                {
                    lock (proxiesThreadLockr)
                    {
                        Monitor.Wait(proxiesThreadLockr);
                    }
                }

                ///Code for checking and then adding proxies to FinalProxyList...
                ThreadPool.QueueUserWorkItem(waitCallBack, item);
                Thread.Sleep(500);
            }

            //AddToProxysLogs(ValidPublicProxies.Count() + " Public Proxies Valid");
        }
        
        private void ThreadPoolMethod_Proxies(object objProxy)
        {
            try
            {
                countParseProxiesThreads++;

                string item = (string)objProxy;
                int IsPublic = 0;
                int Working = 0;
                string LoggedInIp = string.Empty;

                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUserName = string.Empty;
                string proxyPassword = string.Empty;

                string account = item;

                int DataCount = account.Split(':').Length;

                if (DataCount == 1)
                {
                    proxyAddress = account.Split(':')[0];
                    AddToProxysLogs("Proxy Not In correct Format");
                    AddToProxysLogs(account);
                    return;
                }
                if (DataCount == 2)
                {
                    proxyAddress = account.Split(':')[0];
                    proxyPort = account.Split(':')[1];
                }
                else if (DataCount > 2)
                {
                    //proxyAddress = account.Split(':')[0];
                    //proxyPort = account.Split(':')[1];
                    //proxyUserName = account.Split(':')[2];
                    //proxyPassword = account.Split(':')[3];
                    AddToProxysLogs("Proxy Not In correct Format");
                    AddToProxysLogs(account);
                    return;
                }

                ProxyChecker proxyChecker = new ProxyChecker(proxyAddress, proxyPort, proxyUserName, proxyPassword, IsPublic);
                if (proxyChecker.CheckProxy())
                {
                    //lock (((System.Collections.ICollection)listWorkingProxies).SyncRoot)
                    {
                        //if (!listWorkingProxies.Contains(proxy))
                        {
                            workingproxiesCount++;
                            //listWorkingProxies.Add(proxy);
                            lock (proxyListLockr)
                            {
                                queWorkingProxies.Enqueue(item);
                                Monitor.Pulse(proxyListLockr);
                            }
                            AddToProxysLogs("Added " + item + " to working proxies list");
                            //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.FilePathWorkingProxies);
                        }
                    }
                }
                else
                {
                    AddToProxysLogs("Non Working Proxy: " + proxyAddress + ":" + proxyPort);
                    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                }

                #region Commented
                //string pageSource = string.Empty;
                //try
                //{
                //    GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                //    pageSource = httpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), "", proxyAddress, proxyPort, proxyUserName, proxyPassword);
                //}
                //catch (Exception ex)
                //{
                //    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                //}
                //if (pageSource.Contains("class=\"signin\"") && pageSource.Contains("class=\"signup\"") && pageSource.Contains("Twitter"))
                //{
                //    using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                //    {
                //        //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                //        using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                //        {
                //            if (DataCount >= 2)
                //            {
                //                //0 is true
                //                IsPublic = 0;
                //            }
                //            else
                //            {
                //                //1 is false
                //                IsPublic = 1;
                //            }
                //            Working = 1;
                //            string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                //            DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                //        }
                //    }
                //    ValidPublicProxies.Add(item);
                //} 

                #endregion
            }
            catch (Exception ex)
            {
                AddToProxysLogs(ex.Message);
            }
            finally
            {
                lock (proxiesThreadLockr)
                {
                    countParseProxiesThreads--;
                    Monitor.Pulse(proxiesThreadLockr);
                }
            }
        }

        private void btnClearPublicProxies_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete all the Proxies from Database", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                setting.DeletePublicProxyData();
                AddToProxysLogs("All Public Proxies Deleted from the DataBase");
                workingproxiesCount = 0;
                lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                {
                    lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                }));
            }
        }

        private void btnClearPrivateProxies_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete all the Private Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                setting.DeletePrivateProxyData();
                AddToProxysLogs("All Private Proxies Deleted from the DataBase");
            }
        }

        private void btnPublicProxy_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPublicProxy.Text = ofd.FileName;
                    AddToProxysLogs("Public Proxy FileUploaded");
                }
            }
        }

        private void btnPvtProxyFour_Click(object sender, EventArgs e)
        {
            int IsPublic = 0;
            int Working = 0;
            string LoggedInIp = string.Empty;
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                List<string> lstValidProxyList = new List<string>();
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    txtPvtProxyFour.Text = ofd.FileName;
                    List<string> pvtProxy = new List<string>();
                    pvtProxy = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    foreach (string Proxylst in pvtProxy)
                    {
                        string account = Proxylst;
                        string proxyAddress = string.Empty;
                        string proxyPort = string.Empty;
                        string proxyUserName = string.Empty;
                        string proxyPassword = string.Empty;

                        int DataCount = account.Split(':').Length;

                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                if (DataCount == 4)
                                {
                                    lstValidProxyList.Add(Proxylst);
                                    string[] Data = account.Split(':');
                                    proxyAddress = Data[0];
                                    proxyPort = Data[1];
                                    proxyUserName = Data[2];
                                    proxyPassword = Data[3];
                                    LoggedInIp = "NoIP";
                                    IsPublic = 1;
                                    Working = 1;
                                    string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                    DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                                }
                                else
                                {
                                    AddToProxysLogs("Only Private Proxies allowed using this option");
                                }
                            }
                        }

                        
                    }
                    AddToProxysLogs(lstValidProxyList.Count() + " Private Proxies File Uploaded");
                }
            }
        }

        private void btnPvtProxyThree_Click(object sender, EventArgs e)
        {
            int IsPublic = 0;
            int Working = 0;
            string LoggedInIp = string.Empty;
            List<string> lstValidProxyList = new List<string>();
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPvtProxyThree.Text = ofd.FileName;
                    List<string> pvtProxy = new List<string>();
                    pvtProxy = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string Proxylst in pvtProxy)
                    {
                        string account = Proxylst;
                        string proxyAddress = string.Empty;
                        string proxyPort = string.Empty;
                        string ProxyIp = string.Empty;
                        string proxyUserName = string.Empty;
                        string proxyPassword = string.Empty;

                        int DataCount = account.Split(':').Length;

                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE ProxyAddress = '" + proxyAddress + "'", con))
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                if (DataCount == 3)
                                {
                                    lstValidProxyList.Add(Proxylst);
                                    string[] Data = account.Split(':');
                                    proxyAddress = Data[0];
                                    proxyPort = Data[1];
                                    proxyUserName = "";
                                    proxyPassword = "";
                                    LoggedInIp = Data[2];
                                    IsPublic = 1;
                                    Working = 1;
                                    string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUserName + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                    DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                                }
                            }
                        }
                    }
                    AddToProxysLogs(lstValidProxyList.Count() + " Private Proxies File Uploaded");
                }
            }
        }

        private void chkboxPublicProxy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkboxPublicProxy.Checked == true)
                {
                    clsDBQueryManager SetDb = new clsDBQueryManager();
                    DataSet ds = new DataSet();
                    ds = SetDb.SelectPublicProxyData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Please Import Proxy Files. We Are redirecting you to Proxy Tab");
                        tabMain.SelectedIndex = 7;
                    }
                    AddToListAccountsLogs(ds.Tables[0].Rows.Count + " Public Proxies loaded from DataBase");
                    AddToGeneralLogs(ds.Tables[0].Rows.Count + " Public Proxies loaded from DataBase");
                }
            }
            catch (Exception ex)
            {
                AddToListAccountsLogs(ex.Message);
            }
        }

        private void chkboxPrivateProxy_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkboxPrivateProxy.Checked == true)
                {
                    clsDBQueryManager SetDb = new clsDBQueryManager();
                    DataSet ds = new DataSet();
                    ds = SetDb.SelectPrivateProxyData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Please Import Private Proxy Files. We Are redirecting you to Proxy Tab");
                        tabMain.SelectedIndex = 7;
                    }
                    AddToListAccountsLogs(ds.Tables[0].Rows.Count + " Private Proxies loaded from DataBase");
                    AddToGeneralLogs(ds.Tables[0].Rows.Count + " Private Proxies loaded from DataBase");
                }
            }
            catch (Exception ex)
            {
                AddToListAccountsLogs(ex.Message);
            }
        }

        private void btnScrapeUploadUsername_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtScrapeUpload.Text = ofd.FileName;
                    lstscrapeUsername.Clear();
                    txtScrapeUserName.Text = "";
                    lstscrapeUsername = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToScrapeLogs("Added "+ lstscrapeUsername.Count +" Username To Scrape");
                }
            }
        }

        private void btnScrapeUser_Click(object sender, EventArgs e)
        {
            try
            {
                Globals.lstScrapedUserIDs.Clear();
                if (!string.IsNullOrEmpty(txtScrapeUserName.Text) || (lstscrapeUsername.Count > 0) || !string.IsNullOrEmpty(txtScrapeKeyword.Text))
                {
                    if (!string.IsNullOrEmpty(txtScrapeUserName.Text))
                    {
                        lstscrapeUsername.Clear();
                        lstscrapeUsername.Add(txtScrapeUserName.Text);
                        txtScrapeUpload.Text = "";
                    }
                    Thread thread_StartScrape = null;

                    thread_StartScrape = new Thread(() =>
                    {
                        threadStartScrape();
                    });
                    thread_StartScrape.Start();
                }
                else
                {
                    if ((string.IsNullOrEmpty(txtScrapeUserName.Text) && (lstscrapeUsername.Count <= 0) && string.IsNullOrEmpty(txtScrapeKeyword.Text)))
                    {
                        MessageBox.Show("Please Fill In Appropriate Details");
                        AddToScrapeLogs("Please Fill In Appropriate Details");
                    }
                }
            }
            catch (Exception ex)
            {
                AddToScrapeLogs(ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeUser_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeUser_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void threadStartScrape()
        {
            List<string> lst_structTweetFollowersIDs = new List<string>();
            List<string> lst_structTweetFollowingsIds = new List<string>();
            GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
            string user_id = string.Empty;
                    
            foreach (string keyword in lstscrapeUsername)
            {
                if (!GlobusRegex.ValidateNumber(keyword))//(!IsItNumber(user_id_toFollow))
                {
                    user_id = TwitterDataScrapper.GetUserIDFromUsername(keyword);
                }
                else
                {
                    user_id = keyword;
                }

                TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
                
                if (chkboxScrapeFollowers.Checked)
                {
                    try
                    {
                        if (!File.Exists(Globals.Path_ScrapedFollowersList))
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("User_ID , FollowersUserID", Globals.Path_ScrapedFollowersList);
                        }
                        lst_structTweetFollowersIDs = dataScrapeer.GetFollowers(user_id);
                        //lst_structTweetFollowingsIds = dataScrapeer.GetFollowings(user_id);

                        AddToScrapeLogs("Added " + lst_structTweetFollowersIDs.Count + " Followers to list");
                        //AddToScrapeLogs("Added " + lst_structTweetFollowingsIds.Count + " Followings to list");
                        foreach (string data in lst_structTweetFollowersIDs)
                        {
                            try
                            {
                                Globals.lstScrapedUserIDs.Add(data);
                                GlobusFileHelper.AppendStringToTextfileNewLine(user_id + "," + data, Globals.Path_ScrapedFollowersList);
                                AddToScrapeLogs(data);
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowersIDs foreach  --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowersIDs foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }

                        
                        //AddToScrapeLogs("Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword);
                        AddToScrapeLogs("Data Exported to " + Globals.Path_ScrapedFollowersList);
                        if (Globals.IsDirectedFromFollower)
                        {
                            AddToLog_Follower("Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword);
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
               
                if (chkboxScrapeFollowings.Checked)
                {
                    try
                    {
                        if (!File.Exists(Globals.Path_ScrapedFollowingsList))
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("User_ID , FollowingsUserID", Globals.Path_ScrapedFollowingsList);
                        }
                        lst_structTweetFollowingsIds = dataScrapeer.GetFollowings(user_id);
                        AddToScrapeLogs("Added " + lst_structTweetFollowingsIds.Count + " Followings to list");

                        foreach (string data in lst_structTweetFollowingsIds)
                        {
                            try
                            {
                                Globals.lstScrapedUserIDs.Add(data);
                                GlobusFileHelper.AppendStringToTextfileNewLine(user_id + "," + data, Globals.Path_ScrapedFollowingsList);
                                AddToScrapeLogs(data);
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }

                        AddToScrapeLogs("Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword);
                        AddToScrapeLogs("Data Exported to " + Globals.Path_ScrapedFollowingsList);
                        if (Globals.IsDirectedFromFollower)
                        {
                            AddToLog_Follower("Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword);
                            Thread.Sleep(1000);
                            tabMain.Invoke(new MethodInvoker(delegate
                            {
                                tabMain.SelectedIndex = 2;
                            }));
                            //tabMain.SelectedIndex = 2;
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                Globals.lstScrapedUserIDs = Globals.lstScrapedUserIDs.Distinct().ToList();
                ////new Thread(() =>
                ////{
                //foreach (string data in lst_structTweetFollowersIDs)
                //{
                //    try
                //    {
                //        clsDBQueryManager DataBase = new clsDBQueryManager();
                //        DataBase.InsertOrUpdateScrapeSetting(data, "");
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //// }
                ////).Start();
                //AddToScrapeLogs("Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword); 
            }

            new Thread(() =>
                {
                    List<string> temp = new List<string>();
                    foreach (string item in Globals.lstScrapedUserIDs)
                    {
                        temp.Add(item);
                    }

                    foreach (string data in temp)
                    {
                        try
                        {
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            DataBase.InsertOrUpdateScrapeSetting(data, "" , "");
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> lstScrapedUserIDs --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> lstScrapedUserIDs --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                ).Start();
        }

        public static bool IsItNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }

        private void btnScrapeKeyword_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtScrapeKeyword.Text))
                {
                    //if (NumberHelper.ValidateNumber(txtRecords.Text))
                    //{
                    //TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
                    //dataScrapeer.noOfRecords = 100;//Convert.ToInt32(txtRecords.Text);
                    //}
                    //else
                    //{
                    //if (NumberHelper.ValidateNumber(txtRecords.Text))
                    //{
                    //MessageBox.Show("Please Enter Number");
                    //return;
                    //}
                    //}
                    new Thread(() =>
                        {
                            ScrapeKeywordSeacrh();
                        }).Start();
                }
                else
                {
                    MessageBox.Show("Please input a Keyword");
                    //
                }
            }
            catch (Exception ex)
            {
                AddToScrapeLogs(ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public static List<TwitterDataScrapper.StructTweetIDs> DistinctDataList(List<TwitterDataScrapper.StructTweetIDs> collection)
        {
            List<TwitterDataScrapper.StructTweetIDs> distinctList = new List<TwitterDataScrapper.StructTweetIDs>();

            foreach (TwitterDataScrapper.StructTweetIDs value in collection)
            {
                if (!distinctList.Contains(value))
                    distinctList.Add(value);
            }

            return distinctList;
        }

        private void ScrapeKeywordSeacrh()
        {
            try
            {
                TwitterDataScrapper TweetData = new TwitterDataScrapper();
                if (!string.IsNullOrEmpty(txtRecords.Text) && NumberHelper.ValidateNumber(txtRecords.Text))
                {
                    TweetData.noOfRecords = Convert.ToInt32(txtRecords.Text);
                }
                else
                {
                    TweetData.noOfRecords = 100;
                }

                //List<TwitterDataScrapper.StructTweetIDs> data = TweetData.GetTweetData(txtScrapeKeyword.Text);

                List<TwitterDataScrapper.StructTweetIDs> data = TweetData.KeywordStructData(txtScrapeKeyword.Text);

                data = DistinctDataList(data);

                if (!(data.Count() > 0))
                {
                    AddToScrapeLogs("Request Not Complted");
                    AddToScrapeLogs("Requesting For 100 USer ids");
                    data = TweetData.GetTweetData(txtScrapeKeyword.Text);
                }

                AddToScrapeLogs(data.Count + " User ids Scraped ");

                AddToScrapeLogs("Please Wait Till Data Is Retrieving");
                
                int counter = 0;

                if (!File.Exists(Globals.Path_KeywordScrapedList))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("Keyword , User-id , Username" , Globals.Path_KeywordScrapedList);
                }

                foreach (TwitterDataScrapper.StructTweetIDs item in data)
                {
                    if (!string.IsNullOrEmpty(item.username__Tweet_User) && item.ID_Tweet_User != "null")
                    {
                        Globals.lstScrapedUserIDs.Add(item.ID_Tweet_User);
                        GlobusFileHelper.AppendStringToTextfileNewLine(txtScrapeKeyword.Text + "," + item.ID_Tweet_User + "," + item.username__Tweet_User, Globals.Path_KeywordScrapedList);
                        //AddToScrapeLogs(item.ID_Tweet_User);

                    }
                    
                }
                //AddToScrapeLogs("Retrieving data");
                AddToScrapeLogs("Adding Data To DataBase");
                Globals.lstScrapedUserIDs = Globals.lstScrapedUserIDs.Distinct().ToList();

                if (!File.Exists(Globals.Path_KeywordScrapedList))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("KEYWORD:USER ID:USERNAME ", Globals.Path_KeywordScrapedList);
                }

                new Thread(() =>
                {
                    foreach (TwitterDataScrapper.StructTweetIDs item in data)
                    {
                        if (!string.IsNullOrEmpty(item.username__Tweet_User) && item.ID_Tweet_User != "null")
                        {
                            AddToScrapeLogs(item.ID_Tweet_User);
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            DataBase.InsertOrUpdateScrapeSetting(item.ID_Tweet_User, item.username__Tweet_User, item.ID_Tweet);
                        }
                    }
                }).Start();

                if (Globals.IsDirectedFromFollower)
                {
                    Thread.Sleep(1000);
                    Globals.IsDirectedFromFollower = false;
                    AddToLog_Follower(data.Count + " User ids Scraped and Added To Follow List");
                    tabMain.Invoke(new MethodInvoker(delegate
                    {
                        tabMain.SelectedIndex = 2;
                    }));

                    //tabMain.SelectedIndex = 2;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeKeywordSeacrh() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeKeywordSeacrh() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        //private void btnExport_Click(object sender, EventArgs e)
        //{
        //    if (Globals.lstScrapedUserIDs.Count > 0)
        //    {
        //        string exportPath = string.Empty;
        //        if (MessageBox.Show("Choose Default Export Location : " + Globals.Path_ScrappedIDs + "!!!", "Choose Location", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {
        //            exportPath = Globals.Path_ScrappedIDs;
        //        }
        //        else
        //        {
        //            using (OpenFileDialog ofd = new OpenFileDialog())
        //            {
        //                ofd.Filter = "Text Files (*.txt)|*.txt";
        //                ofd.InitialDirectory = Application.StartupPath;
        //                if (ofd.ShowDialog() == DialogResult.OK)
        //                {
        //                    exportPath = ofd.FileName;
        //                    AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
        //                }
        //            }
        //        }
        //        foreach (string item in Globals.lstScrapedUserIDs)
        //        {
        //            try
        //            {
        //                GlobusFileHelper.AppendStringToTextfileNewLine(item, exportPath);
        //            }
        //            catch(Exception ex)
        //            {
        //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
        //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //            }
        //        }

        //        MessageBox.Show("Data Exported to : " + exportPath);
        //        AddToScrapeLogs("Data Exported to : " + exportPath); 
        //    }
        //    else
        //    {
        //        //No Data
        //        MessageBox.Show("Please Scrap Data First");
        //        //
        //    }
        //}

        #region Tweet Creator

        List<string> list_TweetCreatorMessages = new List<string>();

        List<string> list_Spinned_TweetCreatorMessages = new List<string>();

        string path_TweetCreatorExportFile = string.Empty;

        private void btnTweetCreatorMessageFile_Click(object sender, EventArgs e)
        {
            try
            {
                string path_TweetCreatorMessageFile = GlobusFileHelper.LoadTextFileUsingOFD();
                if (!string.IsNullOrEmpty(path_TweetCreatorMessageFile))
                {
                    txtTweetCreatorMessageFile.Text = path_TweetCreatorMessageFile;
                    list_TweetCreatorMessages = GlobusFileHelper.ReadLargeFile(path_TweetCreatorMessageFile);
                    AddToTweetCreatorLogs(list_TweetCreatorMessages.Count + " Tweets Loaded");
                    //list_TweetCreatorMessageFile = SpinnedListGenerator.GetSpinnedList(list_TweetCreatorMessageFile);
                }
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnTweetCreatorMessageFile_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnTweetCreatorMessageFile_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btntxtTweetCreatorExportFile_Click(object sender, EventArgs e)
        {
            try
            {
                path_TweetCreatorExportFile = GlobusFileHelper.LoadTextFileUsingOFD();
                if (string.IsNullOrEmpty(path_TweetCreatorExportFile))
                {
                    path_TweetCreatorExportFile = Globals.Path_SpinnedTweets;
                }
                else
                {
                    Globals.Path_SpinnedTweets = path_TweetCreatorExportFile; 
                }
                txtTweetCreatorExportFile.Text = path_TweetCreatorExportFile;
                //
                AddToTweetCreatorLogs("Export File : " + path_TweetCreatorExportFile);
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btntxtTweetCreatorExportFile_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btntxtTweetCreatorExportFile_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnStart_TweetCreator_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(path_TweetCreatorExportFile) && list_TweetCreatorMessages.Count > 0)
            {
                list_Spinned_TweetCreatorMessages = SpinnedListGenerator.GetSpinnedList(list_TweetCreatorMessages, '|');

                foreach (string item in list_Spinned_TweetCreatorMessages)
                {
                    try
                    {
                        AddToTweetCreatorLogs("Spinned Message : " + item);
                        GlobusFileHelper.AppendStringToTextfileNewLine(item, path_TweetCreatorExportFile);
                    }
                    catch (Exception ex)
                    {
                        AddToTweetCreatorLogs(ex.Message);
                    }
                }
                AddToTweetCreatorLogs("Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile);
            }
            else
            {
                MessageBox.Show("Please upload Tweet Messages File", "Error"); 
            }
        }

        private void AddToTweetCreatorLogs(string log)
        {
            try
            {
                if (lstLogger_TweetCreator.InvokeRequired)
                {
                    lstLogger_TweetCreator.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_TweetCreator.Items.Add(log);
                        lstLogger_TweetCreator.SelectedIndex = lstLogger_TweetCreator.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLogger_TweetCreator.Items.Add(log);
                    lstLogger_TweetCreator.SelectedIndex = lstLogger_TweetCreator.Items.Count - 1;
                }
            }
            catch { }
        }

        #endregion

        private void btnAssignProxy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPvtProxyFour.Text))
            {
                if (MessageBox.Show("Assign Private Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        List<string> lstProxies = proxyFetcher.GetPrivateProxies();
                        if (lstProxies.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                            {
                                accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                            }
                            proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                            ReloadAccountsFromDataBase();
                            AddToProxysLogs("Proxies Assigned To Accounts");
                        }
                        else
                        {
                            MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                        }
                    }
                    catch(Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnAssignProxy_Click() --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnAssignProxy_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                else
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            list_pvtProxy = new List<string>();

                            list_pvtProxy = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                            {
                                accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                            }
                            proxyFetcher.AssignProxiesToAccounts(list_pvtProxy, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                            ReloadAccountsFromDataBase();
                            AddToProxysLogs("Proxies Assigned To Accounts");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select Proxy File To Assign Proxy");
                AddToProxysLogs("Please Select Proxy File To Assign Proxy");
            }
        }

        /// <summary>
        /// Gets the Accounts from "tb_FBAccount" and adds to Dictionary
       /// </summary>
        private void ReloadAccountsFromDataBase()
        {
            try
            {
                clsFBAccount objclsFBAccount = new clsFBAccount();

                DataTable dt = objclsFBAccount.SelectAccoutsForGridView();

                if (dt.Rows.Count > 0)
                {

                    Globals.listAccounts.Clear();
                    TweetAccountContainer.dictionary_TweetAccount.Clear();


                    ///Add Twitter instances to TweetAccountContainer.dictionary_TweetAccount
                    foreach (DataRow dRow in dt.Rows)
                    {
                        try
                        {
                            TweetAccountManager facebooker = new TweetAccountManager();
                            facebooker.Username = dRow[0].ToString();
                            facebooker.Password = dRow[1].ToString();
                            facebooker.proxyAddress = dRow[2].ToString();
                            facebooker.proxyPort = dRow[3].ToString();
                            facebooker.proxyUsername = dRow[4].ToString();
                            facebooker.proxyPassword = dRow[5].ToString();
                            if (!string.IsNullOrEmpty(dRow[7].ToString()))
                            {
                                facebooker.profileStatus = int.Parse(dRow[7].ToString());
                            }

                            Globals.listAccounts.Add(facebooker.Username + ":" + facebooker.Password + ":" + facebooker.proxyAddress + ":" + facebooker.proxyPort + ":" + facebooker.proxyUsername + ":" + facebooker.proxyPassword);
                            TweetAccountContainer.dictionary_TweetAccount.Add(facebooker.Username, facebooker);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                    }
                    Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                    AddToGeneralLogs(Globals.listAccounts.Count + " Accounts loaded"); 
                }
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }
        
        /// <summary>
        /// Assigns "accountsPerProxy" number of proxies to accounts in Database, only picks up only those accounts where ProxyAddress is Null or Empty
        /// </summary>
        private void AssignProxiesToAccounts()
        {
            ////string SelectQuery = "Select * from tb_FBAccount";
            ////DataSet initialDS = DataBaseHandler.SelectQuery(SelectQuery, "tb_FBAccount");

            //DataSet ds = new DataSet();

            //if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
            //{
            //    accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
            //}
            //else
            //{
            //    MessageBox.Show("You entered invalid Accounts per Proxy... Default value \"10\" Set");
            //}

            //using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
            //{
            //    using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount", con))
            //    {
            //        ad.Fill(ds);
            //        //if (ds.Tables[0].Rows.Count == 0)
            //        //{

            //        if (ds.Tables[0].Rows.Count >= 0)
            //        {
            //            //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            //            while (i < ds.Tables[0].Rows.Count)
            //            {

            //                string account = ValidProxies[RandomNumberGenerator.GenerateRandom(0, ValidProxies.Count - 1)];

            //                string proxyAddress = string.Empty;
            //                string proxyPort = string.Empty;
            //                string proxyUserName = string.Empty;
            //                string proxyPassword = string.Empty;

            //                int DataCount = account.Split(':').Length;

            //                if (DataCount == 1)
            //                {
            //                    proxyAddress = account.Split(':')[0];
            //                }
            //                if (DataCount == 2)
            //                {
            //                    proxyAddress = account.Split(':')[0];
            //                    proxyPort = account.Split(':')[1];
            //                }
            //                else if (DataCount > 2)
            //                {
            //                    proxyAddress = account.Split(':')[0];
            //                    proxyPort = account.Split(':')[1];
            //                    proxyUserName = account.Split(':')[2];
            //                    proxyPassword = account.Split(':')[3];
            //                }

            //                string UpdateQuery = "Update tb_FBAccount Set ProxyAddress='" + proxyAddress + "', ProxyPort='" + proxyPort + "', ProxyUserName='" + proxyUserName + "', ProxyPassword='" + proxyPassword + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
            //                DataBaseHandler.UpdateQuery(UpdateQuery, "tb_FBAccount");
            //                i++;
            //            }
            //        }
            //    }
            //}

        }

        #region Public Data Gathering

        public List<string> GetFollowersUsingUserID(string userID)
        {
            List<string> list_Followers = new List<string>();

            TwitterDataScrapper followerScrapper = new TwitterDataScrapper();

            list_Followers = followerScrapper.GetFollowers(userID);

            return list_Followers;
        }

        public List<string> GetFollowingsUsingUserID(string userID)
        {
            List<string> list_Followings = new List<string>();

            TwitterDataScrapper followingScrapper = new TwitterDataScrapper();

            list_Followings = followingScrapper.GetFollowings(userID);

            return list_Followings;
        }


        #endregion

        public static string RemoveSpecialCharacters(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') || (str[i] >= 'A' && str[i] <= 'z'))
                {
                    sb.Append(str[i]);
                }
            }

            return sb.ToString();
        }

        private void btnStart_FakeEmailCreator_Click(object sender, EventArgs e)
        {
            Globals.FakeEmailList.Clear();

            if (!string.IsNullOrEmpty(txtDomain_FakeEmailCreator.Text))
            {
                string domain_Entered = txtDomain_FakeEmailCreator.Text.Trim();
                if (string.IsNullOrEmpty(domain_Entered))
                {
                    MessageBox.Show("Please upload valid Domain Name");
                    return;
                }
            }

            if (lstFakeEmailNames.Count > 0 && ((!string.IsNullOrEmpty(txtFakePassword.Text.Replace(" ", "")) && (txtFakePassword.Text.Length >= 6)) || chkboxMakeRandPass.Checked) && !string.IsNullOrEmpty(txtDomain_FakeEmailCreator.Text.Replace("\0", "").Replace(" ", "")) && (NumberHelper.ValidateNumber(txtFakeAccountsNo.Text) || string.IsNullOrEmpty(txtFakeAccountsNo.Text)))
            {
                new Thread(() =>
                {
                    StartFakeEmailCreation();
                }
                ).Start();
            }
            else
            {
                if (lstFakeEmailNames.Count <= 0)
                {
                    AddToFakeEmailCreator("Please Upload Text File With Names");
                }
                else if (string.IsNullOrEmpty(txtFakePassword.Text.Replace(" ","")) && chkboxMakeRandPass.Checked == false)
                {
                    AddToFakeEmailCreator("Please Enter Either Password");
                    AddToFakeEmailCreator("OR");
                    AddToFakeEmailCreator("Check Random Password Generator to Generate Password");
                }
                else if(!string.IsNullOrEmpty(txtFakePassword.Text.Replace(" ","")) && (txtFakePassword.Text.Length < 6))
                {
                    AddToFakeEmailCreator("Please Enter a Password Greater than 6 alphanumeric Character.");
                }
                else if (string.IsNullOrEmpty(txtDomain_FakeEmailCreator.Text.Replace("\0", "").Replace(" ", "")))
                {
                    AddToFakeEmailCreator("Please Enter a Domain Name to Create Emails");
                }
                else if (!NumberHelper.ValidateNumber(txtFakeAccountsNo.Text) || string.IsNullOrEmpty(txtFakeAccountsNo.Text))
                {
                    AddToFakeEmailCreator("Please Enter Number");
                }
            }
        }

        public void StartFakeEmailCreation()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtFakeAccountsNo.Text) && NumberHelper.ValidateNumber(txtFakeAccountsNo.Text))
                {
                    fakeEmailCount = Convert.ToInt32(txtFakeAccountsNo.Text);
                }
                int counter = 0;
                AddToFakeEmailCreator("Starting Fake Email Creation");

                lstFakeEmailNames = lstFakeEmailNames.Distinct().ToList();

                foreach (string names in lstFakeEmailNames)
                {
                    if (fakeEmailCount > counter)
                    {
                        counter++;
                    }
                    else
                    {
                        break;
                    }
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(fakeEmailCreator() , new object[] {  });
                    //fakeEmailCreator(new object[] { });
                    try
                    {
                        string Password = string.Empty;
                        string EmailId = string.Empty;
                        string domainName = txtDomain_FakeEmailCreator.Text.Replace("@", "").Replace(" ", "").Replace("\0", "");
                        string name = names;
                        string RandomNo = RandomStringGenerator.RandomNumber(5);
                        EmailId = name.ToLower() + RandomNo + "@" + domainName;
                        if (chkboxMakeRandPass.Checked)
                        {
                            Password = RandomStringGenerator.RandomString(10);
                        }
                        else
                        {
                            Password = RemoveSpecialCharacters(txtFakePassword.Text.Replace("\\", "").Replace("/", "").Replace("\'", "").Replace("\"", ""));
                            if (string.IsNullOrEmpty(Password) || (Password.Length <= 6))
                            {
                                AddToFakeEmailCreator("Please Use AlphaNumeric Charachters Greater than 6 Chars");
                                AddToFakeEmailCreator("Special Charchters will not be used as Password");
                                AddToFakeEmailCreator("Please Enter a new Password to start Again");
                                return;
                            }
                        }
                        AddToFakeEmailCreator("Created Email : " + EmailId + "|| Password : " + Password);
                        GlobusFileHelper.AppendStringToTextfileNewLine(EmailId + ":" + Password, Globals.Path_FakeEmailIds);
                        Globals.FakeEmailList.Add(EmailId + ":" + Password);
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFakeEmailCreation() -- Declaring variables --> " + ex.Message, Globals.Path_FakeEmailCraetorErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFakeEmailCreation() -- Declaring variables --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    //string InsertQuery = "Insert into tb_FBAccount values('" + EmailId + "','" + Password + "', '" + "" + "', '" + "" + "', '" + "" + "' , '" + "" + "' , '" + "" + "' , '" + "" + "' , '" + "" + "')";
                    //DataBaseHandler.InsertQuery(InsertQuery, "tb_FBAccount");

                }

                AddToFakeEmailCreator("Finished Fake Email Creation");
                if (Globals.TwtFakeAccounts)
                {
                    Globals.TwtFakeAccounts = false;
                    if (tabMain.InvokeRequired)
                    {
                        tabMain.Invoke(new MethodInvoker(delegate { tabMain.SelectedIndex = 0; }));
                    }

                    AddToListAccountsLogs(Globals.FakeEmailList.Count + " Fake Emails Loaded");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFakeEmailCreation() --> " + ex.Message, Globals.Path_FakeEmailCraetorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFakeEmailCreation() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void chkChangeExportLocation_FakeEmailCreator_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkChangeExportLocation_FakeEmailCreator.Checked)
            //{
            //    using (OpenFileDialog ofd = new OpenFileDialog())
            //    {
            //        ofd.Filter = "CSV Files (*.csv)|*.csv";
            //        ofd.InitialDirectory = Application.StartupPath;
            //        if (ofd.ShowDialog() == DialogResult.OK)
            //        {
            //            txtExportLocation_FakeEmailCreator.Text = ofd.FileName;
            //            Globals.path_LikedPages = ofd.FileName;
            //            AddFanpagePosterLogger("Export Location : " + ofd.FileName);
            //        }
            //    }
            //}
            //else
            //{
            //    AddFanpagePosterLogger("Export Location : " + Globals.path_LikedPages);
            //}
            if (chkChangeExportLocation_FakeEmailCreator.Checked)
            {
                string path_ExportLocation_FakeEmailCreator = GlobusFileHelper.LoadTextFileUsingOFD();
                if(!string.IsNullOrEmpty(path_ExportLocation_FakeEmailCreator))
                {
                    if (File.Exists(path_ExportLocation_FakeEmailCreator))
                    {
                        txtExportLocation_FakeEmailCreator.Text = path_ExportLocation_FakeEmailCreator;
                        Globals.Path_FakeEmailIds = path_ExportLocation_FakeEmailCreator;
                    }
                    else
                    {
                        AddToFakeEmailCreator("File Does Not Exsists");
                        Globals.Path_FakeEmailIds = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TwtDominator\\FakeEmailAccounts.txt";
                        txtExportLocation_FakeEmailCreator.Text = Globals.Path_FakeEmailIds;
                    }
                }
                else
                {
                    Globals.Path_FakeEmailIds = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TwtDominator\\FakeEmailAccounts.txt";
                    txtExportLocation_FakeEmailCreator.Text = Globals.Path_FakeEmailIds;
                }
            }
           
            
        }

        private void btnUploadNames_FakeEmailCreator_Click(object sender, EventArgs e)
        {
            lstFakeEmailNames.Clear();
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtNames_FakeEmailCreator.Text = ofd.FileName;
                    List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string item in tempList)
                    {
                        string newitem = item.Replace("\0", "").Replace(" ", "");
                        if(!string.IsNullOrEmpty(newitem))
                        {
                            lstFakeEmailNames.Add(newitem);
                        }
                    }
                    
                    AddToFakeEmailCreator(lstFakeEmailNames.Count() + " Email Names Uploaded");
                }
            }
        }

        private void chkboxUseFakeEmailAccounts_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxUseFakeEmailAccounts.Checked)
            {
                if (Globals.FakeEmailList.Count > 0)
                {
                    AddToGeneralLogs(Globals.FakeEmailList.Count() + " Fake Emails in List");
                    AddToListAccountsLogs(Globals.FakeEmailList.Count() + " Fake Emails in List");
                }
                else
                {
                    tabMain.SelectedIndex = 10;
                    Globals.TwtFakeAccounts = true;
                    AddToListAccountsLogs(Globals.FakeEmailList.Count() + " Fake Emails in List");
                    AddToGeneralLogs("Please Create Fake Email Accounts");
                }
            }
        }

        public string strModule(Module module)
        {
            switch (module)
            {
                case Module.WaitAndReply:
                    return threadNaming_WaitAndReply_;
                   
                case Module.Tweet:
                    return threadNaming_Tweet_;

                case Module.Retweet:
                    return threadNaming_Retweet_;

                case Module.Reply:
                    return threadNaming_Reply_;

                case Module.Follow:
                    return threadNaming_Follow_;

                case Module.Unfollow:
                    return threadNaming_Unfollow_;

                case Module.ProfileManager:
                    return threadNaming_ProfileManager_;

                default:
                    return "";
            }


   
        }

        #region Stopping Variables

        string threadNaming_WaitAndReply_ = "WaitAndReply_";
        string threadNaming_Tweet_ = "Tweet_";
        string threadNaming_Retweet_ = "Retweet_";
        string threadNaming_Reply_ = "Reply_";
        string threadNaming_Follow_ = "Follow_";
        string threadNaming_Unfollow_ = "Unfollow_";
        string threadNaming_ProfileManager_ = "ProfileManager_";

        #endregion

        #region Stop Wait and Reply

        public static Dictionary<string, Thread> dictionary_Threads = new Dictionary<string, Thread>();

        private void btnStopWaitReplyThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping Wait and Reply Threads, it may take some time");
                StopThreads(strModule(Module.WaitAndReply));
            }).Start();
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
                foreach (KeyValuePair<string, Thread> item in tempdictionary_Threads)//(KeyValuePair<string, Thread> item in dictionary_Threads)
                {
                    try
                    {
                        string key = item.Key;
                        string threadName = Regex.Split(key, "_")[0];
                        module = module.Replace("_", "");
                        //Thread thread = item.Value;
                        if (threadName == module)
                        {
                            AddToGeneralLogs("Aborting : " + key);
                            //thread.Abort();
                            Thread thread = item.Value;
                            int abortCounter = 0;

                            if (thread != null)
                            {
                                if (thread.ThreadState == ThreadState.Running || thread.ThreadState == ThreadState.WaitSleepJoin || thread.ThreadState == ThreadState.Background) { thread.Abort(); }
                                //while (thread.ThreadState == ThreadState. || thread.ThreadState == ThreadState.AbortRequested)
                                //{
                                //    //wait a little bit 
                                //    if (abortCounter < 40)
                                //    {
                                //        Thread.Sleep(500);
                                //        abortCounter++;
                                //    }
                                //    else
                                //    {
                                //        break;
                                //    }
                                //}
                                thread.Abort();
                                AddToGeneralLogs("Aborted : " + key);
                                dictionary_Threads.Remove(key);
                            }

                        }

                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Abort in Profile Manager Foreach Loop " + ex.Message , Globals.Path_TwtErrorLogs);
            }
        }

        public void StopAllProcess()
        {
            try
            {
                foreach (KeyValuePair<string, Thread> item in dictionary_Threads)//(KeyValuePair<string, Thread> item in dictionary_Threads)
                {
                    try
                    {
                        string key = item.Key;
                        string threadName = Regex.Split(key, "_")[0];
                        //Thread thread = item.Value;
                            AddToGeneralLogs("Aborting : " + key);
                            //thread.Abort();
                            Thread thread = item.Value;
                            int abortCounter = 0;

                            if (thread != null)
                            {
                                if (thread.ThreadState == ThreadState.Running || thread.ThreadState == ThreadState.WaitSleepJoin || thread.ThreadState == ThreadState.Background) { thread.Abort(); }
                                //while (thread.ThreadState == ThreadState. || thread.ThreadState == ThreadState.AbortRequested)
                                //{
                                //    //wait a little bit 
                                //    if (abortCounter < 40)
                                //    {
                                //        Thread.Sleep(500);
                                //        abortCounter++;
                                //    }
                                //    else
                                //    {
                                //        break;
                                //    }
                                //}
                                thread.Abort();
                                AddToGeneralLogs("Aborted : " + key);
                                //dictionary_Threads.Remove(key);
                            }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Stop All Process " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            dictionary_Threads.Clear();
        }


        #endregion

        private void btnStop_ProfileThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping ProfileManager Threads, it may take some time");
                StopThreads(strModule(Module.ProfileManager));
            }).Start();
        }

        private void btnStop_FollowThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping Follow Threads, it may take some time");
                StopThreads(strModule(Module.Follow));
            }).Start();
        }

        private void btnStop_TweetThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping Tweet Threads, it may take some time");
                StopThreads(strModule(Module.Tweet));
            }).Start();

        }

        private void btnStop_RetweetReplyThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping Retweet Threads, it may take some time");
                StopThreads(strModule(Module.Retweet));
            }).Start();

             new Thread(() =>
            {
                AddToGeneralLogs("Stopping Reply Threads, it may take some time");
                StopThreads(strModule(Module.Reply));
            }).Start();

        }

        private void btnStop_UnFollowThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("Stopping Unfollow Threads, it may take some time");
                StopThreads(strModule(Module.Unfollow));
            }).Start();

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

        private void btnGetUserID_Click(object sender, EventArgs e)
        {
            lstScrapedUserID.Clear();
            lstUserNameID.Clear();
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lstUserNameID = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    //AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
                }
            }
            string exportPath = string.Empty;
            if (MessageBox.Show("Default Stored File In Location : " + Globals.Path_ScrapedUserID + " \n Choose New File!!!", "Choose Location", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    //ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        exportPath = ofd.FileName;
                        AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
                    }
                }
            }
            else
            {
                exportPath = Globals.Path_ScrapedUserID;
                if (!File.Exists(exportPath))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("UserName,UserID", exportPath);
                }
                AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
            }
            
           
            GetUserNameToID(exportPath);
            
        
        }

        public void GetUserNameToID(string exportPath)
        {
            foreach (string Username in lstUserNameID)
            {
                string UserId = TwitterDataScrapper.GetUserIDFromUsername(Username);
                AddToScrapeLogs(Username + " >>> " + UserId);
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + "," + UserId, exportPath);
                lstScrapedUserID.Add(UserId);
            }
            AddToScrapeLogs(lstScrapedUserID.Count + " UserId Scraped ");
        }

        private void chkboxScrapedLst_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxScrapedLst.Checked)
            {
                if (Globals.lstScrapedUserIDs.Count > 0)
                {
                    AddToLog_Follower("Added " + Globals.lstScrapedUserIDs.Count + " Userd Ids");
                }
                else
                {
                    AddToLog_Follower("Please Scrape Data");
                    AddToLog_Follower("You Will Be Redirected To Scrape Users Tab");
                    Globals.IsDirectedFromFollower = true;
                    Thread.Sleep(1000);
                    tabMain.SelectedIndex = 8;
                }
            }
            
        }

        private void rdBtnDivideByGivenNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdBtnDivideByGivenNo.Checked && chkUseDivide.Checked)
            {
                txtScrapeNoOfUsers.Enabled = true;
            }
            else if (!rdBtnDivideByGivenNo.Checked)
            {
                txtScrapeNoOfUsers.Enabled = false;
            }
        }

        private void ChkboxUseDivide_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseDivide.Checked)
            {
                IsUsingDivideData = true;
                rdBtnDivideByGivenNo.Enabled = true;
                rdBtnDivideEqually.Enabled = true;
            }
            else if(!chkUseDivide.Checked)
            {
                IsUsingDivideData = false;
                rdBtnDivideByGivenNo.Enabled = false;
                rdBtnDivideEqually.Enabled = false;
                txtScrapeNoOfUsers.Enabled = false;
            }
        }

        private void btnScheduleForLater_Follower_Click(object sender, EventArgs e)
        {
            //ReadFollowSettings();

            MessageBox.Show("Please upload all Relevant Data used for running Follow Module. These Data will be used when scheduled task is run");

            if (FollowValidations())
            {

                try
                {
                    string IsScheduledDaily = "0";
                    if (chkScheduleDaily_Follow.Checked)
                    {
                        IsScheduledDaily = "1";
                    }
                    clsDBQueryManager queryManager = new clsDBQueryManager();
                    queryManager.InsertUpdateTBScheduler("", "Follow", dateTimePicker_Follow.Value.ToString(), IsScheduledDaily);

                    if (scheduler != null && scheduler.IsDisposed == false)
                    {
                        scheduler.LoadDataGrid();
                    }

                    //Save Settings in Text File
                    string strOtherUser = "false";
                    if (!string.IsNullOrEmpty(txtOtherfollow.Text) && !string.IsNullOrEmpty(txtUserOtherNumber.Text))
                    {
                        strOtherUser = "true";
                    }

                    string strChkFollowers = "0";
                    if (chkFollowers.Checked)
                    {
                        strChkFollowers = "1";
                    }
                    string strChkFollowings = "0";
                    if (chkFollowings.Checked)
                    {
                        strChkFollowings = "1";
                    }
                    string strchkUseDivide = "0";
                    if (chkUseDivide.Checked)
                    {
                        strchkUseDivide = "1";
                    }
                    string strrdBtnDivideEqually = "0";
                    if (rdBtnDivideEqually.Checked)
                    {
                        strrdBtnDivideEqually = "1";
                    }
                    string strrdBtnDivideByGivenNo = "0";
                    if (rdBtnDivideByGivenNo.Checked)
                    {
                        strrdBtnDivideByGivenNo = "1";
                    }

                    GlobusFileHelper.WriteStringToTextfile(txtUserIDtoFollow.Text + "<:>" + txtPathUserIDs.Text + "<:>" + txtOtherfollow.Text + "<:>" + txtUserOtherNumber.Text + "<:>" + strChkFollowers + "<:>" + strChkFollowings + "<:>" + strchkUseDivide + "<:>" + strrdBtnDivideEqually + "<:>" + strrdBtnDivideByGivenNo + "<:>" + txtScrapeNoOfUsers.Text + "<:>" + txtFollowMinDelay.Text + "<:>" + txtFollowMaxDelay.Text + "<:>" + txtNoOfFollowThreads.Text, Globals.Path_FollowSettings);

                    MessageBox.Show("Task Scheduled");
                    AddToGeneralLogs("Task Scheduled");
                    /////Updates new Paths in tb_Setting
                    //if (!string.IsNullOrEmpty(txtNames.Text) && !string.IsNullOrEmpty(txtUsernames.Text) && !string.IsNullOrEmpty(txtEmails.Text))
                    //{
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Name", StringEncoderDecoder.Encode(txtNames.Text));
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Username", StringEncoderDecoder.Encode(txtUsernames.Text));
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Email", StringEncoderDecoder.Encode(txtEmails.Text));
                    //}
                }
                catch (Exception ex)
                {
                    AddToLog_Follower("Error in Task Scheduling : " + ex.Message);
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScheduleForLater_Follower_Click() --> " + ex.Message, Globals.Path_FollowerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScheduleForLater_Follower_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
        }

        private bool FollowValidations()
        {

            try
            {
                if ((!string.IsNullOrEmpty(txtUserIDtoFollow.Text) || listUserIDs.Count > 0 || !string.IsNullOrEmpty(txtOtherfollow.Text) || Globals.lstScrapedUserIDs.Count > 0) && !string.IsNullOrEmpty(txtFollowMaxDelay.Text) && !string.IsNullOrEmpty(txtFollowMinDelay.Text) && NumberHelper.ValidateNumber(txtFollowMaxDelay.Text) && NumberHelper.ValidateNumber(txtFollowMinDelay.Text))
                {

                    if (!string.IsNullOrEmpty(txtUserIDtoFollow.Text))
                    {
                        listUserIDs.Clear();
                        listUserIDs.Add(txtUserIDtoFollow.Text);
                        //txtUserIDtoFollow.Text = "";
                    }
                    else if (Globals.lstScrapedUserIDs.Count > 0 && chkboxScrapedLst.Checked)
                    {
                        listUserIDs.Clear();
                        foreach (string data in Globals.lstScrapedUserIDs)
                        {
                            listUserIDs.Add(data);
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(txtUserIDtoFollow.Text) || !(listUserIDs.Count > 0) || string.IsNullOrEmpty(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please enter User ID or Upload User IDs File to follow or Enter Username to follow Its users");
                    }
                    else if (string.IsNullOrEmpty(txtFollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter Minimum delay");
                    }
                    else if (!NumberHelper.ValidateNumber(txtFollowMinDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                    else if (string.IsNullOrEmpty(txtFollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter Maximum Delay");
                    }
                    else if (!NumberHelper.ValidateNumber(txtFollowMaxDelay.Text))
                    {
                        MessageBox.Show("Please Enter a Number");
                    }
                    else if (string.IsNullOrEmpty(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please Enter No Of Users");
                    }
                    else if (!NumberHelper.ValidateNumber(txtOtherfollow.Text))
                    {
                        MessageBox.Show("Please Enete a Number");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        bool IsTweetScheduled = false;

        private void btnScheduleForLater_Tweeter_Click(object sender, EventArgs e)
        {
            //InsertUpdateSchedulerModule(strModule(Module.Tweet), dateTimePicker_Follower.Value.ToString());
            MessageBox.Show("Please Upload All Data To Start Tweeting");

            if (CheckTweeting())
            {
                string IsScheduledDaily = "0";
                if (chkScheduleDaily_Follow.Checked)
                {
                    IsScheduledDaily = "1";
                }
                clsDBQueryManager queryManager = new clsDBQueryManager();
                queryManager.InsertUpdateTBScheduler("", "Tweet_", dateTimePicker_tweeterStart.Value.ToString(), IsScheduledDaily);

                if (scheduler != null && scheduler.IsDisposed == false)
                {
                    scheduler.LoadDataGrid();
                }

                if (!string.IsNullOrEmpty(txtNoOfTweets.Text) && Globals.IdCheck.IsMatch(txtNoOfTweets.Text))
                {
                    TweetAccountManager.NoOfTweets = int.Parse(txtNoOfTweets.Text);
                }

                listTweetMessages = listTweetMessages.Distinct().ToList();

                foreach (string item in listTweetMessages)
                {
                    TweetAccountManager.que_TweetMessages.Enqueue(item);
                }

                GlobusFileHelper.WriteStringToTextfile(txtTweetMessageFile.Text, Globals.Path_TweetSettings);

                MessageBox.Show("Task Scheduled");
                AddToGeneralLogs("Task Scheduled");
                IsTweetScheduled = true;
            }
            else
            {
                AddToLog_Tweet("Please Add All Data");
            }



        }

        public bool CheckTweeting()
        {
            if (listTweetMessages.Count >= 1 && !string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text) && ((!string.IsNullOrEmpty(txtNoOfTweets.Text) && Globals.IdCheck.IsMatch(txtNoOfTweets.Text) || (ChkboxTweetPerday.Checked && (!string.IsNullOrEmpty(txtMaximumTweet.Text) && NumberHelper.ValidateNumber(txtMaximumTweet.Text))))))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please Fill All Data Required For Tweeting");
                return false;
            }
         }


        public void ReadFollowSettings()
        {
            string[] settingsData = ReadSettingsTextFile(Globals.Path_FollowSettings);

            //foreach (string item in settingsData)
            //{

            try
            {
                LoadSettings_Follower(settingsData);
            }
            catch (Exception ex)
            {
                AddToLog_Follower("FollowSettings File : " + Globals.Path_FollowSettings + " isn't in the correct format");
            }
            //}
        }

        public void ReadTweetSettings()
        {
            string[] settingData = ReadSettingsTextFile(Globals.Path_TweetSettings);

            try
            {
                LoadSetting_Tweet(settingData);
            }
            catch (Exception ex)
            {
                AddToLog_Tweet("TweetSetting File : " + Globals.Path_TweetSettings + " isn't in the correct format");
            }

        }

        public void LoadSetting_Tweet(string[] SettingData)
        {
            ReloadAccountsFromDataBase();
            listTweetMessages = GlobusFileHelper.ReadFiletoStringList(SettingData[0]);
            try
            {
                int numberOfThreads = 7;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0 && listTweetMessages.Count >= 1)
                {
                    if (!string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtNoOfTweetThreads.Text);
                    }
                    if (!string.IsNullOrEmpty(txtNoOfTweets.Text) && Globals.IdCheck.IsMatch(txtNoOfTweets.Text))
                    {
                        TweetAccountManager.NoOfTweets = int.Parse(txtNoOfTweets.Text);
                    }

                    listTweetMessages = listTweetMessages.Distinct().ToList();

                    foreach (string item in listTweetMessages)
                    {
                        TweetAccountManager.que_TweetMessages.Enqueue(item);
                    }

                    
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            string tweetMessage = string.Empty;
                            //try
                            //{
                            //    tweetMessage = listTweetMessages[RandomNumberGenerator.GenerateRandom(0, listTweetMessages.Count - 1)];
                            //}
                            //catch { }

                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartTweetingMultithreaded), new object[] { item, listTweetMessages });

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("please Upload Twitter Account");
                    AddToLog_Tweet("please Upload Twitter Account");
                }
            }
            catch { }
        }

        private void StartFollowing()
        {
            try
            {
                List<List<string>> list_listTargetURLs = new List<List<string>>();
                try
                {
                    int numberOfThreads = 7;
                    //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                    {

                        #region Other User No
                        //bool OtherUser = false;
                        if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
                        {
                            if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
                            {
                                TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
                                //OtherUser = true;
                                //txtUserOtherNumber.Text = "";
                            }
                            else
                            {
                                AddToLog_Follower("Please Enter a Number");
                                return;
                            }
                        } 
                        #endregion

                        #region Follower Following ratio
                        if (chkDontFollowUsersWithFollowingsFollowersRatio.Checked)
                        {
                            TweetAccountManager.UseRatioFilter = true;
                            if (!string.IsNullOrEmpty(txtFollowingsFollowersRatio.Text))
                            {
                                if (NumberHelper.ValidateNumber(txtFollowingsFollowersRatio.Text))
                                {
                                    TweetAccountManager.FollowingsFollowersRatio = Convert.ToInt32(txtFollowingsFollowersRatio.Text);
                                }
                                else
                                {
                                    AddToLog_Follower("Default Value 80 set in FollowingsFollowers Ratio");
                                    //return;
                                }
                            }
                        } 
                        #endregion

                        #region Not Tweeted For #  no of Days
                        if (chkDontFollowUsersWhoHavntTweetedForLong.Checked)
                        {
                            TweetAccountManager.UseDateLastTweeted = true;
                            if (!string.IsNullOrEmpty(txtLastTweetDays.Text))
                            {
                                if (NumberHelper.ValidateNumber(txtLastTweetDays.Text))
                                {
                                    TweetAccountManager.LastTweetDays = Convert.ToInt32(txtLastTweetDays.Text);
                                }
                                else
                                {
                                    AddToLog_Follower("Default Days 50 set in Required Last Tweet Days");
                                    //return;
                                }
                            }
                        }
                        if (chkDontFollowUsersThatUnfollowedBefore.Checked)
                        {
                            TweetAccountManager.UseUnfollowedBeforeFilter = true;
                        } 
                        #endregion

                        #region Other Follow

                        if (!string.IsNullOrEmpty(txtOtherfollow.Text))
                        {
                            listUserIDs.Clear();
                            if (chkFollowers.Checked)
                            {
                                listUserIDs = GetFollowersUsingUserID(txtOtherfollow.Text);
                                //OtherUser = true;
                            }
                            if (chkFollowings.Checked)
                            {
                                //listUserIDs = GetFollowingsUsingUserID(txtOtherfollow.Text);
                                listUserIDs.AddRange(GetFollowingsUsingUserID(txtOtherfollow.Text));
                                //OtherUser = true;
                            }
                        } 
                        #endregion

                        #region User Without Picture
                        if (chkDontFollowUsersWithNoPicture.Checked)
                        {
                            List<string> tempdata = new List<string>();
                            try
                            {
                                foreach (string newitem in listUserIDs)
                                {
                                    tempdata.Add(newitem);
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_FollowerErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                            listUserIDs.Clear();

                            AddToLog_Follower("Checking Profile Image in Extracted User id's");

                            foreach (string item in tempdata)
                            {
                                string containsIamge = TwitterDataScrapper.GetPhotoFromUsername(item);
                                if (containsIamge == "true")
                                {
                                    AddToLog_Follower(item + " Contains Image");
                                    listUserIDs.Add(item);
                                }
                                else if (containsIamge == "false")
                                {
                                    AddToLog_Follower(item + " Not Contains Image");
                                }
                                else if (containsIamge == "Rate limit exceeded")
                                {
                                    AddToLog_Follower("Cannot Make Request. Rate limit exceeded");
                                    AddToLog_Follower("Please Try After Some Time");
                                }
                            }
                        }
                        
                        #endregion

                        #region User Divude Checked
                        if (chkUseDivide.Checked || IsUsingDivideData)
                        {
                            int splitNo = 0;
                            if (rdBtnDivideEqually.Checked)
                            {
                                splitNo = listUserIDs.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
                            }
                            else if (rdBtnDivideByGivenNo.Checked)
                            {
                                if (!string.IsNullOrEmpty(txtScrapeNoOfUsers.Text) && NumberHelper.ValidateNumber(txtScrapeNoOfUsers.Text))
                                {
                                    int res = Convert.ToInt32(txtScrapeNoOfUsers.Text);
                                    splitNo = res;//listUserIDs.Count / res;
                                }
                            }
                            if (splitNo == 0)
                            {
                                splitNo = RandomNumberGenerator.GenerateRandom(0, listUserIDs.Count - 1);
                            }
                            list_listTargetURLs = Split(listUserIDs, splitNo);
                        }

                        if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
                        {
                            numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
                        }

                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        bool OtherUser = false;
                        if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
                        {
                            if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
                            {
                                TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
                                OtherUser = true;
                                //txtUserOtherNumber.Text = "";
                            }
                            else
                            {
                                AddToLog_Follower("Please Enter a Number");
                                return;
                            }
                        } 
                        #endregion

                        #region Follow Per Day
                        //Code Added by Abhishek 

                        TweetAccountManager.NoOfFollowPerDay_ChkBox = false;
                        if (chkBox_NoOFfollow.Checked)
                        {
                            if (!string.IsNullOrEmpty(txt_MaximumFollow.Text))
                            {
                                if (NumberHelper.ValidateNumber(txt_MaximumFollow.Text))
                                {
                                    TweetAccountManager.NoOfFollowPerDay = Convert.ToInt32(txt_MaximumFollow.Text);
                                    TweetAccountManager.NoOfFollowPerDay_ChkBox = true;
                                    //txtUserOtherNumber.Text = "";
                                }
                                else
                                {
                                    AddToLog_Follower("Please Enter a Number In Maximum follow per Id");
                                    return;
                                }
                            }
                        } 
                        #endregion

                        #region MyRegion
                        if (chkDontFollowUsersWithManyLinks.Checked)
                        {
                            AddToLog_Follower("Checking For Links in Tweets");
                            if (!string.IsNullOrEmpty(txtNoOfLinks.Text) && NumberHelper.ValidateNumber(txtNoOfLinks.Text))
                            {
                                TwitterDataScrapper.Percentage = Convert.ToInt32(txtNoOfLinks.Text);
                            }
                            else
                            {
                                TwitterDataScrapper.Percentage = 40;
                                AddToLog_Follower("Setting Default Percentage : 40%");
                            }
                            List<string> tempdata = new List<string>();
                            try
                            {
                                foreach (string newitem in listUserIDs)
                                {
                                    tempdata.Add(newitem);
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_FollowerErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                            listUserIDs.Clear();
                            foreach (string item in tempdata)
                            {
                                bool toomanyLinks = TwitterDataScrapper.GetStatusLinks(item);
                                if (toomanyLinks)
                                {
                                    listUserIDs.Add(item);
                                    AddToLog_Follower("Added User id : " + item + " To Follow List");
                                }
                            }
                        }
                        #endregion

                        int count_AccountsUsed = 0;

                        int index = 0;

                        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                        {

                            try
                            {
                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                if (chkUseDivide.Checked || IsUsingDivideData)
                                {
                                    listUserIDs = list_listTargetURLs[index];
                                }

                                if (GlobusRegex.ValidateNumber(txtFollowMinDelay.Text))
                                {
                                    followMinDelay = Convert.ToInt32(txtFollowMinDelay.Text);
                                }
                                if (GlobusRegex.ValidateNumber(txtFollowMaxDelay.Text))
                                {
                                    followMaxDelay = Convert.ToInt32(txtFollowMaxDelay.Text);
                                }

                                ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs, OtherUser, followMinDelay, followMaxDelay });

                                Thread.Sleep(1000);
                                count_AccountsUsed++;
                                index++;
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_FollowerErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Upload Twitter Account");
                        AddToGeneralLogs("Please Upload Twitter Account");
                    }
                }
                catch(Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_FollowerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() --  list_listTargetURLs --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- list_listTargetURLs --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void StartFollowingModified(List<List<string>> list_listTargetURLs, int numberOfThreads, bool otherUser, bool UseDivide, string followMinDelay, string followMaxDelay)
        {
            #region Prev Code 27th June
            //List<List<string>> list_listTargetURLs = new List<List<string>>();
            //try
            //{
            //    int numberOfThreads = 7;
            //    //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            //    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            //    {

            //        //bool OtherUser = false;
            //        if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
            //        {
            //            if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
            //            {
            //                TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
            //                //OtherUser = true;
            //                //txtUserOtherNumber.Text = "";
            //            }
            //            else
            //            {
            //                AddToLog_Follower("Please Enter a Number");
            //                return;
            //            }
            //        }

            //        if (!string.IsNullOrEmpty(txtOtherfollow.Text))
            //        {
            //            listUserIDs.Clear();
            //            if (chkFollowers.Checked)
            //            {
            //                listUserIDs = GetFollowersUsingUserID(txtOtherfollow.Text);
            //                //OtherUser = true;
            //            }
            //            if (chkFollowings.Checked)
            //            {
            //                //listUserIDs = GetFollowingsUsingUserID(txtOtherfollow.Text);
            //                listUserIDs.AddRange(GetFollowingsUsingUserID(txtOtherfollow.Text));
            //                //OtherUser = true;
            //            }
            //        }

            //        if (chkUseDivide.Checked || IsUsingDivideData)
            //        {
            //            int splitNo = 0;
            //            if (rdBtnDivideEqually.Checked)
            //            {
            //                splitNo = listUserIDs.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
            //            }
            //            else if (rdBtnDivideByGivenNo.Checked)
            //            {
            //                if (!string.IsNullOrEmpty(txtScrapeNoOfUsers.Text) && NumberHelper.ValidateNumber(txtScrapeNoOfUsers.Text))
            //                {
            //                    int res = Convert.ToInt32(txtScrapeNoOfUsers.Text);
            //                    splitNo = res;//listUserIDs.Count / res;
            //                }
            //            }
            //            if (splitNo == 0)
            //            {
            //                splitNo = RandomNumberGenerator.GenerateRandom(0, listUserIDs.Count - 1);
            //            }
            //            list_listTargetURLs = Split(listUserIDs, splitNo);
            //        }

            //        if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
            //        {
            //            numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
            //        }

            //        ThreadPool.SetMaxThreads(numberOfThreads, 5);

            //        //string userIDToFollow = txtUserIDtoFollow.Text;

            //        int count_AccountsUsed = 0;

            //        int index = 0;

            //        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
            //        {

            //            try
            //            {
            //                ThreadPool.SetMaxThreads(numberOfThreads, 5);

            //                if (chkUseDivide.Checked || IsUsingDivideData)
            //                {
            //                    listUserIDs = list_listTargetURLs[index];
            //                }

            //                ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs });

            //                Thread.Sleep(1000);
            //                count_AccountsUsed++;
            //                index++;
            //            }
            //            catch (Exception ex)
            //            {
            //                //ErrorLogger.AddToErrorLogText(ex.Message);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please Upload Twitter Account");
            //        AddToGeneralLogs("Please Upload Twitter Account");
            //    }
            //}
            //catch { } 
            #endregion

            #region New Code 27th June
            //List<List<string>> list_listTargetURLs = new List<List<string>>();
            try
            {
                //int numberOfThreads = 7;
                ////TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {

                    int count_AccountsUsed = 0;

                    int index = 0;

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            List<string> listUserIDs = new List<string>();

                            if (UseDivide)
                            {
                                if (index < list_listTargetURLs.Count)
                                {
                                    listUserIDs = list_listTargetURLs[index];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                listUserIDs = list_listTargetURLs[0];
                            }

                            int intfollowMinDelay = 5;
                            int intfollowMaxDelay = 10;

                            if (GlobusRegex.ValidateNumber(followMinDelay))
                            {
                                intfollowMinDelay = Convert.ToInt32(followMinDelay);
                            }
                            if (GlobusRegex.ValidateNumber(followMaxDelay))
                            {
                                intfollowMaxDelay = Convert.ToInt32(followMaxDelay);
                            }

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs, otherUser, intfollowMinDelay, intfollowMaxDelay });

                            Thread.Sleep(1000);
                            count_AccountsUsed++;
                            index++;

                           
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload Twitter Account");
                    AddToGeneralLogs("Please Upload Twitter Account");
                }
            }
            catch { }
            #endregion
        }

        private void StartFollowingMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[3];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                List<string> list_userIDsToFollow = (List<string>)paramsArray.GetValue(1);

                bool OtherUser = (bool)paramsArray.GetValue(2);
                int strMinDelay = (int)paramsArray.GetValue(3);
                int strMaxDelay = (int)paramsArray.GetValue(4);

                int intMinDelay = strMinDelay;
                int intMaxDelay = strMaxDelay;

                List<string> lstFollowers = new List<string>();
                List<string> lstFollowings = new List<string>();

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Follow), tweetAccountManager.Username);

                tweetAccountManager.follower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;
                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }
          
                AddToLog_Follower(list_userIDsToFollow.Count() + " In follower List");

                if (list_userIDsToFollow.Count > 0)
                {
                    //tweetAccountManager.FollowUsingURLs(list_userIDsToFollow, followMinDelay, followMaxDelay, OtherUser);
                    tweetAccountManager.FollowUsingURLs(list_userIDsToFollow, intMinDelay, intMaxDelay, OtherUser);
                    //txtPathUserIDs.Invoke(new MethodInvoker(delegate
                    //{
                    //    txtPathUserIDs.Text = "";
                    //}));
                    //txtUserOtherNumber.Invoke(new MethodInvoker(delegate
                    //{
                    //    txtUserOtherNumber.Text = "";
                    //}));
                    //txtScrapeNoOfUsers.Invoke(new MethodInvoker(delegate
                    //{
                    //    txtScrapeNoOfUsers.Text = "";
                    //}));
                    //txtOtherfollow.Invoke(new MethodInvoker(delegate
                    //{
                    //    txtOtherfollow.Text = "";
                    //}));
                    //Globals.lstScrapedUserIDs.Clear();
                    //txtPathUserIDs.Text = "";
                }
                else
                {
                    AddToLog_Follower("No ID's To Follow");
                }

                tweetAccountManager.follower.logEvents.addToLogger -= logEvents_Follower_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Follower_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowingMultithreaded() --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        frmScheduler scheduler = new frmScheduler();
        private void schedulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmScheduler scheduler = new frmScheduler();
            if (scheduler != null && scheduler.IsDisposed == false)
            {
                scheduler.Show();
            }
            else
            {
                scheduler = new frmScheduler();
                scheduler.Show();
            }
        }
         
        private void btnExtractTweet_Click(object sender, EventArgs e)
        {
            if (lstUserId_Tweet.Count > 0 || !string.IsNullOrEmpty(txtTweetName.Text.Replace(" ", "").Replace("\0", "")))
            {
                if (!string.IsNullOrEmpty(txtTweetName.Text))
                {
                    lstUserId_Tweet.Clear();
                    lstUserId_Tweet.Add(txtTweetName.Text);
                }
                new Thread(() =>
                {
                    TweetDataExtract();
                }
                ).Start();
            }
            else
            {
                if(lstUserId_Tweet.Count <= 0 || string.IsNullOrEmpty(txtTweetName.Text.Replace(" ", "").Replace("\0", "")))
                {
                    AddToTweetCreatorLogs("Please Enter a Username/Userid Name or File.");
                }
            }
        }

        public void TweetDataExtract()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTweetExtractCount.Text) && NumberHelper.ValidateNumber(txtTweetExtractCount.Text))
                {
                    TweetExtractCount = Convert.ToInt32(txtTweetExtractCount.Text);

                    if (TweetExtractCount > 200)
                    {
                        TwitterDataScrapper.TweetExtractCount = 200;
                        AddToTweetCreatorLogs("Extracting Default No Of Tweet : 200");
                    }
                    else
                    {
                        TwitterDataScrapper.TweetExtractCount = TweetExtractCount;
                    }
                }
                else
                {
                    AddToTweetCreatorLogs("Entered Count incorrect.Setting Default Count 10");
                }

                int counter = 0;
                TwitterDataScrapper DataScraper = new TwitterDataScrapper();
                foreach (string item in lstUserId_Tweet)
                {
                    AddToTweetCreatorLogs("Extracting Tweets For " + item);
                    List<string> TweetData = DataScraper.GetTweetData_Scrape(item);
                    if (TweetData.Count > 0)
                    {
                        foreach (string newItem in TweetData)
                        {
                            AddToTweetCreatorLogs(newItem);
                        }
                    }
                    else
                    {
                        AddToTweetCreatorLogs("Sorry No Tweets For " + item);
                    }
                }
                AddToTweetCreatorLogs("Finished Extracting Tweets");
                AddToTweetCreatorLogs("Tweets Stored In -" + Globals.Path_TweetExtractor);
                if (txtTweetName.InvokeRequired)
                {
                    txtTweetName.Invoke(new MethodInvoker(delegate
                   {
                       txtTweetName.Text = "";
                   }));
                }
                if (txtExtractorFile.InvokeRequired)
                {
                    txtExtractorFile.Invoke(new MethodInvoker(delegate
                    {
                        txtExtractorFile.Text = "";
                    }));
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetDataExtract() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TweetDataExtract() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnTweetExctractor_Click(object sender, EventArgs e)
        {
            try
            {
                lstUserId_Tweet.Clear();
                txtExtractorFile.Text = GlobusFileHelper.LoadTextFileUsingOFD();

                List<string> tempFile  = GlobusFileHelper.ReadFiletoStringList(txtExtractorFile.Text);

                foreach (string item in tempFile)
                {
                    string tempData = item.Replace("\0", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tempData))
                    {
                        lstUserId_Tweet.Add(item);
                    }
                }

                AddToTweetCreatorLogs(lstUserId_Tweet.Count + " Usernames/Userid Uploaded");
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void chkboxImportPublicProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxImportPublicProxy.Checked)
            {
                if(!string.IsNullOrEmpty(txtPublicProxy.Text.Replace("\0","").Replace(" ","")))
                {
                    lstPublicProxyWOTest = GlobusFileHelper.ReadFiletoStringList(txtPublicProxy.Text);
                    AddToProxysLogs(lstPublicProxyWOTest.Count + " Public Proxies Loaded");
                    if (lstPublicProxyWOTest.Count > 0)
                    {
                        new Thread(() =>
                            {
                                foreach (string item in lstPublicProxyWOTest)
                                {
                                    ImportingProxy(item);
                                }
                            }
                        ).Start();
                    }
                    else
                    {
                        AddToProxysLogs("Sorry No Proxies Available");
                    }
                }
                else
                {
                    AddToProxysLogs("Please Select File To Get Data Imported");
                }
            }
        }

        public void ImportingProxy(string item)
        {
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUsername = string.Empty;
            string proxyPassword = string.Empty;
            int Working = 0;
            int IsPublic = 0;
            string LoggedInIp = string.Empty;

            string account = item;
            int DataCount = account.Split(':').Length;

            if (DataCount == 1)
            {
                proxyAddress = account.Split(':')[0];
                AddToProxysLogs("Proxy Not In correct Format");
                AddToProxysLogs(account);
                return;
            }
            if (DataCount == 2)
            {
                proxyAddress = account.Split(':')[0];
                proxyPort = account.Split(':')[1];
            }
            else if (DataCount > 2)
            {
                AddToProxysLogs("Proxy Not In correct Format");
                AddToProxysLogs(account);
                return;
            }
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                {
                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                    {
                        AddToProxysLogs("Added Proxies -> " + proxyAddress + ":" + proxyPort);
                        string InsertQuery = "Insert into tb_Proxies values('" + proxyAddress + "','" + proxyPort + "','" + proxyUsername + "','" + proxyPassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                        DataBaseHandler.InsertQuery(InsertQuery, "tb_Proxies");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(proxyAddress + ":" + proxyPort, Globals.Path_ExsistingProxies);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error Importing Public Proxy W/o testing --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }
       
        private void txtAsssignPublicProxy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPublicProxy.Text))
            {
                if (MessageBox.Show("Assign Public Proxies from Database???", "Proxy", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        List<string> lstProxies = proxyFetcher.GetPublicProxies();
                        if (lstProxies.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                            {
                                accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                            }
                            proxyFetcher.AssignProxiesToAccounts(lstProxies, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                            ReloadAccountsFromDataBase();
                            AddToProxysLogs("Proxies Assigned To Accounts");
                        }
                        else
                        {
                            MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                        }
                    }
                    catch(Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> txtAsssignPublicProxy_Click  --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> txtAsssignPublicProxy_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                else
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            list_pvtProxy = new List<string>();

                            list_pvtProxy = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                            if (!string.IsNullOrEmpty(txtAccountsPerProxy.Text) && GlobusRegex.ValidateNumber(txtAccountsPerProxy.Text))
                            {
                                accountsPerProxy = int.Parse(txtAccountsPerProxy.Text);
                            }
                            proxyFetcher.AssignProxiesToAccounts(list_pvtProxy, accountsPerProxy);//AssignProxiesToAccounts(lstProxies);
                            ReloadAccountsFromDataBase();
                            AddToProxysLogs("Proxies Assigned To Accounts");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Select Proxy File To Assign Proxy");
                AddToProxysLogs("Please Select Proxy File To Assign Proxy");
            }
        }

        private void btnRemoveDuplicates_Click(object sender, EventArgs e)
        {
            if (File.Exists(Globals.Path_KeywordScrapedList))
            {
                List<string> KeywordScpareList = GlobusFileHelper.ReadFiletoStringList(Globals.Path_KeywordScrapedList);
                KeywordScpareList = KeywordScpareList.Distinct().ToList();
                foreach (string item in KeywordScpareList)
                {
                    AddToScrapeLogs("Adding Data : " + item);
                    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_KeywordScrapedListWithoutDuplicates);
                }
            }
            else
            {
                MessageBox.Show("File Does Not Exsist");
                AddToScrapeLogs("File : " + Globals.Path_KeywordScrapedList);
                AddToScrapeLogs("Does Not Exsist");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DMList.Clear();
            AddToDMLog("Starting Search For People");
            AddToDMLog("To Send Direct Messages");
            new Thread(() =>
                {
                    SendDirectMessage();
                }
            ).Start();
           
        }



        public void SendDirectMessage()
        {
            try
            {
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    ThreadPool.SetMaxThreads(5, 5);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreaded), new object[] { item });

                    Thread.Sleep(1000);
                }
            }
            else
            {
                MessageBox.Show("Please Add Accounts");
                AddToDMLog("Please Add Accounts");
            }
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void StartDMMultiThreaded(object parameter)
        {
            try
            {
                Array paramsArray = new object[1];

                paramsArray = (Array)parameter;

                KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                TweetAccountManager AccountManager = item.Value;

                AccountManager.Username = item.Value.Username;
                AccountManager.Password = item.Value.Password;
                AccountManager.proxyAddress = item.Value.proxyAddress;
                AccountManager.proxyPort = item.Value.proxyPort;
                AccountManager.proxyUsername = item.Value.proxyUsername;
                AccountManager.proxyPassword = item.Value.proxyPassword;

                AddToDMLog("Logging In With Email : " + item.Value.Username);
                if (!AccountManager.IsLoggedIn)
                {
                    AccountManager.Login();
                }

            

                if (AccountManager.IsLoggedIn)
                {
                    AddToDMLog("Logged In With Email :" + AccountManager.Username);
                    TwitterDataScrapper dataScrape = new TwitterDataScrapper();
                    List<string> DMFollowers = dataScrape.GetFollowers(AccountManager.userID);
                    //List<string> DMFollowings = dataScrape.GetFollowings(AccountManager.userID);
                    //List<string> nonFollowings = DMFollowings.Except(DMFollowers).ToList();
                    //List<string> FollowingnFollower = DMFollowings.Except(nonFollowings).ToList();
                    AddToDMLog("Adding User's With Direct Message Option in Email : " + item.Value.Username);
                    DMList.Add(AccountManager.userID, DMFollowers);

                    if (cmbboxUserID.InvokeRequired)
                    {
                        new Thread(() =>
                            {
                                cmbboxUserID.Invoke(new MethodInvoker(delegate
                                {
                                    cmbboxUserID.Items.Add(AccountManager.userID + ":" + AccountManager.Username);
                                }));
                            }
                        ).Start();
                    }
                }
                else
                {
                    AddToDMLog("Account : " + AccountManager.Username + " Not Logged In");
                }
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartDMMultiThreaded() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartDMMultiThreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void cmbboxUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            chklstDirectMessage.Items.Clear();
            string GetUserID = cmbboxUserID.SelectedItem.ToString();
            foreach (KeyValuePair<string, List<string>> item in DMList)
            {
                if (GetUserID.Contains(item.Key))
                {
                    List<string> DMUserIDs = item.Value;
                    AddToDMLog(DMUserIDs.Count() + " Usernames in List");
                    AddToDMLog("Please Wait While We Add Username To Send Direct Message");

                    foreach (string Value in DMUserIDs)
                    {
                        ThreadPool.SetMaxThreads(20, 5);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoadUserId), new object[] { Value });
                    }
                }
            }
        }

        public void StartLoadUserId(object parameter)
        {
            try
            {
                Array paramsArray = new object[1];

                paramsArray = (Array)parameter;

                string Value = (string)paramsArray.GetValue(0);

                string GetUsername = TwitterDataScrapper.GetUserNameFromUserId(Value);
                if (!GetUsername.Contains("Rate Limit Exceeded"))
                {
                    AddToDMLog("Adding Username : " + GetUsername);
                    chklstDirectMessage.Invoke(new MethodInvoker(delegate
                    {
                        chklstDirectMessage.Items.Add(GetUsername);
                    }));
                }
                else
                {
                    AddToDMLog("Rate Limit Excedded.");
                    AddToDMLog("Please Try After Some time");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartLoadUserId() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartLoadUserId() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnSendDM_Click_1(object sender, EventArgs e)
        {
            if (chklstDirectMessage.CheckedItems.Count > 0 && lstDirectMessage.Count > 0)
            {

                new Thread(() =>
                    MethodSendDirectMessage()
                    ).Start();
            }
            else
            {
                if (chklstDirectMessage.CheckedItems.Count <= 0)
                {
                    AddToDMLog("Please Select At Least One Username");
                }
                else if (lstDirectMessage.Count <= 0)
                {
                    AddToDMLog("Please Upload Message");
                }
            }
    
        }

        public void MethodSendDirectMessage()
        {
            try
            {
                int numberOfThreads = 0;
                if (!string.IsNullOrEmpty(txtThreadNoDM.Text) && NumberHelper.ValidateNumber(txtThreadNoDM.Text))
                {
                    numberOfThreads = Convert.ToInt32(txtThreadNoDM.Text);
                }
                else
                {
                    AddToDMLog("Thread Value Not Correct. Assining Default Value = 5");
                    numberOfThreads = 5;
                }

                if (lstDirectMessage.Count > 0)
                {
                    foreach (string Message in lstDirectMessage)
                    {
                        TweetAccountManager.que_DirectMessage.Enqueue(Message);
                    }
                }
                else
                {
                    AddToDMLog("Please Add Message Text");
                    return;
                }

                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        string value = string.Empty;
                        cmbboxUserID.Invoke(new MethodInvoker(delegate
                            {
                                value = cmbboxUserID.SelectedItem.ToString();
                            }));

                        if (value.Contains(item.Key))
                        {
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDirectMessagingMultithreaded), new object[] { item });

                            Thread.Sleep(1000);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> MethodSendDirectMessage() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> MethodSendDirectMessage() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void StartDirectMessagingMultithreaded(object parameter)
        {
            try
            {
                int MinimumDelay = 15;
                int MaximumDelay = 25;
                Array paramsArray = new object[1];

                paramsArray = (Array)parameter;

                KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                TweetAccountManager AccountManager = item.Value;

                AccountManager.Username = item.Value.Username;
                AccountManager.Password = item.Value.Password;
                AccountManager.proxyAddress = item.Value.proxyAddress;
                AccountManager.proxyPort = item.Value.proxyPort;
                AccountManager.proxyUsername = item.Value.proxyUsername;
                AccountManager.proxyPassword = item.Value.proxyPassword;

                if (!AccountManager.IsLoggedIn)
                {
                    AccountManager.Login();
                }


                List<string> SelectedItem = new List<string>();

               
                    if (chklstDirectMessage.InvokeRequired)
                    {
                        chklstDirectMessage.Invoke(new MethodInvoker(delegate
                        {
                            foreach (string Userid in chklstDirectMessage.CheckedItems)
                            {
                                SelectedItem.Add(Userid);
                            }

                        }));
                    }
                
                int MaximumMsgPerDay = 10;
                if (!string.IsNullOrEmpty(txtMessagePerDay.Text) && NumberHelper.ValidateNumber(txtMessagePerDay.Text))
                {
                    MaximumMsgPerDay = Convert.ToInt32(txtMessagePerDay.Text);
                }
                clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                DataSet Ds = DbQueryManager.SelectMessageData(item.Value.Username, "Message");

                int TodayMessage = Ds.Tables["tb_MessageRecord"].Rows.Count;
                AddToLog_Tweet(TodayMessage + " Already Messaged today");

                #region Delay
                if (!string.IsNullOrEmpty(txtMinDMDelay.Text) && NumberHelper.ValidateNumber(txtMinDMDelay.Text))
                {
                    MinimumDelay = Convert.ToInt32(txtMinDMDelay.Text);
                }
                else
                {
                    AddToDMLog("Minimum Delay Value Incorrect");
                    AddToDMLog("Assinging Defauly Value = 15");
                }

                if (!string.IsNullOrEmpty(txtMaxDMDelay.Text) && NumberHelper.ValidateNumber(txtMaxDMDelay.Text))
                {
                    MaximumDelay = Convert.ToInt32(txtMaxDMDelay.Text);
                }
                else
                {
                    AddToDMLog("Minimum Delay Value Incorrect");
                    AddToDMLog("Assinging Defauly Value = 25");
                }
                #endregion

                foreach (string Userid in SelectedItem)
                {
                    if (TodayMessage >= MaximumMsgPerDay)
                    {
                        AddToDMLog("Already Messaged " + TodayMessage);
                        break;
                    }
                    AddToDMLog("Direct Message To " + Userid);
                    int Delay = RandomNumberGenerator.GenerateRandom(MinimumDelay, MaximumDelay);
                    string MessagePosted = AccountManager.DirectMessage(Userid);
                    if (MessagePosted.Contains("Success"))
                    {
                        string[] Array = Regex.Split(MessagePosted, ":");
                        AddToDMLog("Message Posted :"+ Array[1]);
                    }
                    else if (MessagePosted.Contains("Error"))
                    {
                        AddToDMLog("Error in Post");
                    }
                    else
                    {
                        AddToDMLog("Message Not Posted");
                    }
                    AddToDMLog("Delay For : " + Delay);
                    Thread.Sleep(Delay);
                    TodayMessage++;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartDirectMessagingMultithreaded() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartDirectMessagingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnUploadDM_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtUploadDM.Text = ofd.FileName;
                        List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in tempList)
                        {
                            string newitem = item;
                            if (!string.IsNullOrEmpty(newitem))
                            {
                                lstDirectMessage.Add(newitem);
                            }
                        }

                        AddToDMLog(lstDirectMessage.Count() + " Messages Uploaded");
                    }
                }
            }
            catch(Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnUploadRetweetUserID_Click(object sender, EventArgs e)
        {
            try
            {
                lstUserId_Retweet.Clear();
                txtRetweetFile.Text = GlobusFileHelper.LoadTextFileUsingOFD();

                List<string> tempFile = GlobusFileHelper.ReadFiletoStringList(txtRetweetFile.Text);

                foreach (string item in tempFile)
                {
                    string tempData = item.Replace("\0", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tempData))
                    {
                        lstUserId_Retweet.Add(item);
                    }
                }

                AddToTweetCreatorLogs(lstUserId_Retweet.Count + " Usernames/Userid Uploaded");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnUploadRetweetUserID_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadRetweetUserID_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnRetweetExtract_Click(object sender, EventArgs e)
        {
            if ((lstUserId_Retweet.Count > 0 && !string.IsNullOrEmpty(txtRetweetFile.Text))|| !string.IsNullOrEmpty(txtRetweetSingle.Text.Replace(" ", "").Replace("\0", "")))
            {
                if (!string.IsNullOrEmpty(txtRetweetSingle.Text))
                {
                    lstUserId_Retweet.Clear();
                    lstUserId_Retweet.Add(txtRetweetSingle.Text);
                }
                new Thread(() =>
                {
                    //TweetDataExtract();
                    RetweetDataExtractor();
                }
                ).Start();
            }
            else
            {
                if ((lstUserId_Retweet.Count <= 0 || string.IsNullOrEmpty(txtRetweetFile.Text)) || string.IsNullOrEmpty(txtRetweetSingle.Text.Replace(" ", "").Replace("\0", "")))
                {
                    AddToTweetCreatorLogs("Please Enter a Username/Userid Name or File.");
                }
            }
        }

        public void RetweetDataExtractor()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtRetweetCount.Text) && NumberHelper.ValidateNumber(txtRetweetCount.Text))
                {
                    RetweetExtractCount = Convert.ToInt32(txtRetweetCount.Text);
                    TwitterDataScrapper.RetweetExtractcount = RetweetExtractCount;
                }
                else
                {
                    AddToTweetCreatorLogs("Entered Count incorrect.Setting Default Count 10");
                }

                TwitterDataScrapper DataScraper = new TwitterDataScrapper();
                foreach (string item in lstUserId_Retweet)
                {
                    AddToTweetCreatorLogs("Extracting ReTweets For " + item);
                    List<string> TweetData = DataScraper.GetRetweetData_Scrape(item);
                    AddToTweetCreatorLogs(TweetData.Count + " Retweet From User : " + item);
                    if (TweetData.Count > 0)
                    {

                        new Thread(() =>
                        {
                            foreach (string newItem in TweetData)
                            {
                                string[] arry = Regex.Split(newItem, ":");
                                if (arry.Length == 3)
                                {
                                    AddToTweetCreatorLogs("---------------------------------------------------------");
                                    AddToTweetCreatorLogs(arry[0] + " : " + arry[1]);
                                    AddToTweetCreatorLogs(arry[2]);
                                    clsDBQueryManager DataBase = new clsDBQueryManager();
                                    DataBase.InsertDataRetweet(arry[0], arry[1], arry[2]);
                                }
                                else
                                {

                                }
                            }
                        }).Start();
                    }
                    else
                    {
                        AddToTweetCreatorLogs("Sorry No ReTweets For " + item);
                    }
                }
                AddToTweetCreatorLogs("Finished Extracting ReTweets");
                AddToTweetCreatorLogs("ReTweets Stored In -" + Globals.Path_RETweetExtractor);
                if (txtRetweetSingle.InvokeRequired)
                {
                    txtRetweetSingle.Invoke(new MethodInvoker(delegate
                    {
                        txtRetweetSingle.Text = "";
                    }));
                }
                if (txtRetweetFile.InvokeRequired)
                {
                    txtRetweetFile.Invoke(new MethodInvoker(delegate
                    {
                        txtRetweetFile.Text = "";
                    }));
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> RetweetDataExtractor() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> RetweetDataExtractor() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void chkboxChangeExportTweet_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxChangeExportTweet.Checked)
            {
                string path_ExportLocation_FakeEmailCreator = GlobusFileHelper.LoadTextFileUsingOFD();
                if (!string.IsNullOrEmpty(path_ExportLocation_FakeEmailCreator))
                {
                    if (File.Exists(path_ExportLocation_FakeEmailCreator))
                    {
                        txtTweetExportLocation.Text = path_ExportLocation_FakeEmailCreator;
                        Globals.Path_TweetExtractor = path_ExportLocation_FakeEmailCreator;
                    }
                    else
                    {
                        AddToTweetCreatorLogs("File Does Not Exsists");
                        txtTweetExportLocation.Text = Globals.Path_TweetExtractor;
                    }
                }
                else
                {
                    txtTweetExportLocation.Text = Globals.Path_TweetExtractor;
                }
            }
        }

        private void chkboxExportLocationRetweet_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxExportLocationRetweet.Checked)
            {
                string path_ExportLocation_FakeEmailCreator = GlobusFileHelper.LoadTextFileUsingOFD();
                if (!string.IsNullOrEmpty(path_ExportLocation_FakeEmailCreator))
                {
                    if (File.Exists(path_ExportLocation_FakeEmailCreator))
                    {
                        txtExportLocationRewteet.Text = path_ExportLocation_FakeEmailCreator;
                        Globals.Path_RETweetExtractor = path_ExportLocation_FakeEmailCreator;
                    }
                    else
                    {
                        AddToTweetCreatorLogs("File Does Not Exsists");
                        txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                    }
                }
                else
                {
                    txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                }
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopAllProcess();

            foreach (var item in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (item.ProcessName == "LicensingManager")
                    {
                        item.Kill();
                    }
                }
                catch { }
            }

            //foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName("LicensingManager.exe"))
            //{
            //    try
            //    {
            //        if (process.MainModule.ModuleName.ToUpper().Equals("LicensingManager.exe"))
            //        {
            //            try
            //            {
            //                process.Kill();
            //                ///break;
            //            }
            //            catch { };
            //        }
            //    }
            //    catch { };
            //}
        }

        private void chkboxRetweetPerDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxRetweetPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = true;
                groupBox56.Enabled = false;
            }
            else if (!chkboxRetweetPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = false;
                groupBox56.Enabled = true;
            }
        }

        private void chkboxReplyPerDay_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkboxReplyPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = true;
                groupBox56.Enabled = false;
            }
            else if (!chkboxReplyPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = false;
                groupBox56.Enabled = true;
            }
        }

        private void chkboxSetting_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkboxSetting.Checked)
            {
                panelSetting.Visible = true;
                panelSetting.BringToFront();
            }
            else if (!chkboxSetting.Checked)
            {
                panelSetting.Visible = false;
                panelSetting.SendToBack();
            }
        }

        private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _TextBrush;

            // Get the item from the collection.
            TabPage _TabPage = tabMain.TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _TabBounds = tabMain.GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {
                // Draw a different background color, and don't paint a focus rectangle.
                _TextBrush = new SolidBrush(Color.White);
                g.FillRectangle(Brushes.DarkGray, e.Bounds);
            }
            else
            {
                _TextBrush = new System.Drawing.SolidBrush(Color.Black);
                g.FillRectangle(Brushes.DarkGreen, e.Bounds);
                e.DrawBackground();
            }

            // Use our own font. Because we CAN.
            Font _TabFont = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);

            // Draw string. Center the text.
            StringFormat _StringFlags = new StringFormat();
            _StringFlags.Alignment = StringAlignment.Center;
            _StringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_TabPage.Text, _TabFont, _TextBrush,
                         _TabBounds, new StringFormat(_StringFlags));
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAccounts frmaccounts = new frmAccounts();
            frmaccounts.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            {
                frmSettings = new FrmSettings();
            }

            frmSettings.Show();    
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (scheduler != null && scheduler.IsDisposed == false)
            {
                scheduler.Show();
            }
            else
            {
                scheduler = new frmScheduler();
                scheduler.Show();
            }
        }

        private void ChkboxTweetPerday_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ChkboxTweetPerday.Checked)
            {
                txtMaximumTweet.Enabled = true;
            }
            else if (!ChkboxTweetPerday.Checked)
            {
                txtMaximumTweet.Enabled = false;
            }
        }

        private void ChkBoxSelectAll_CheckedChanged_1(object sender, EventArgs e)
        {
            if (ChkBoxSelectAll.Checked == true)
            {
                for (int i = 0; i < chklstDirectMessage.Items.Count; i++)
                {
                    chklstDirectMessage.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < chklstDirectMessage.Items.Count; i++)
                {
                    chklstDirectMessage.SetItemChecked(i, false);
                }
            }
        }

        private void tabMain_TabIndexChanged(object sender, EventArgs e)
        {
            this.Update();
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedIndex == 0)
            {
                labelAccountcreator.Text = "Account Creation";
            }
            else if (tabMain.SelectedIndex == 1)
            {
                labelAccountcreator.Text = "Profile Manager";
            }
            else if (tabMain.SelectedIndex == 2)
            {
                labelAccountcreator.Text = "Follower";
            }
            else if (tabMain.SelectedIndex == 3)
            {
                labelAccountcreator.Text = "Tweet";
            }
            else if (tabMain.SelectedIndex == 4)
            {
                labelAccountcreator.Text = "Account Checker";
            }
            else if (tabMain.SelectedIndex == 5)
            {
                labelAccountcreator.Text = "Unfollower";
            }
            else if (tabMain.SelectedIndex == 6)
            {
                labelAccountcreator.Text = "Wait & Reply";
            }
            else if (tabMain.SelectedIndex == 7)
            {
                labelAccountcreator.Text = "Proxy Setting";
            }
            else if (tabMain.SelectedIndex == 8)
            {
                labelAccountcreator.Text = "Scrape Users";
            }
            else if (tabMain.SelectedIndex == 9)
            {
                labelAccountcreator.Text = "Tweet Creator";
            }
            else if (tabMain.SelectedIndex == 10)
            {
                labelAccountcreator.Text = "Fake Email Creator";
            }
            else if (tabMain.SelectedIndex == 11)
            {
                labelAccountcreator.Text = "Direct Message";
            }
        }

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            if (Globals.lstScrapedUserIDs.Count > 0)
            {
                string exportPath = string.Empty;
                if (MessageBox.Show("Choose Default Export Location : " + Globals.Path_ScrappedIDs + "!!!", "Choose Location", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    exportPath = Globals.Path_ScrappedIDs;
                }
                else
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            exportPath = ofd.FileName;
                            AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
                        }
                    }
                }
                foreach (string item in Globals.lstScrapedUserIDs)
                {
                    try
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(item, exportPath);
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                MessageBox.Show("Data Exported to : " + exportPath);
                AddToScrapeLogs("Data Exported to : " + exportPath);
            }
            else
            {
                //No Data
                MessageBox.Show("Please Scrap Data First");
                //
            }
        }

        

        ////Web Browwser TD start
        List<string> LstAccountCreationName = new List<string>();
        List<string> LstAccountcreationUsername = new List<string>();
        List<string> LstAccountcreationEmailPassword = new List<string>();
        int countForInstance = 0;
        IE explorer = null;

        private void button5_Click(object sender, EventArgs e)
        {
            //Settings.Instance.MakeNewIeInstanceVisible = false;
            Settings.Instance.AutoMoveMousePointerToTopLeft = false;
            Settings.Instance.AutoStartDialogWatcher = false;
            Settings.Instance.WaitForCompleteTimeOut = 30000;
            GlobusRegex globusRegex = new GlobusRegex();

            Thread newThread = new Thread(new ThreadStart(SignupPage));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            AddToProxyAccountCreationLog("Strating Account Creation");
            //new Thread(() =>
            //{
            //    SignupPage();
            //}).Start();
        }

        public void SignupPage()
        {
            clsDBQueryManager SetDb = new clsDBQueryManager();
            DataSet ds = new DataSet();
            //bool UsePublicProxy = false;
            int counter_Username = 0;
            foreach (string EmailPassword in LstAccountcreationEmailPassword)
            {
            StartAgain:
                try
                {
                    GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                    string[] array = Regex.Split(EmailPassword, ":");
                    string email = array[0];
                    string password = array[1];
                    string username = string.Empty;
                    string ProxyAdress = string.Empty;
                    string ProxyPort = string.Empty;
                    string Proxy = string.Empty;
                    AddToProxyAccountCreationLog("Clearing Cookies");
                    ClearCookies();
                    if (chkboxUsePublicProxies.Checked)
                    {
                        try
                        {
                            ds = SetDb.SelectPublicProxyData();
                            if (ds.Tables[0].Rows.Count != 0)
                            {
                                int index = RandomNumberGenerator.GenerateRandom(0, ds.Tables[0].Rows.Count);
                                DataRow dr = ds.Tables[0].Rows[countForInstance];
                                Proxy = dr.ItemArray[0].ToString() + ":" + dr.ItemArray[1].ToString() + ":" + dr.ItemArray[2].ToString() + ":" + dr.ItemArray[3].ToString();
                                string[] ProxyList = Regex.Split(Proxy, ":");
                                ProxyAdress = ProxyList[0];
                                ProxyPort = ProxyList[1];
                                
                                AddToProxyAccountCreationLog("Using Proxy Address : - " + ProxyAdress + ":" + ProxyPort);
                            }
                            else
                            {
                                AddToProxyAccountCreationLog("Please Uplaod Public Proxies");
                                break;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    AddToProxyAccountCreationLog("Setting Proxy");

                    Thread.Sleep(2000);

                    SetProxy(ProxyAdress, ProxyPort);

                    Thread.Sleep(2000);

                    IE explorer = new IE();

                    Thread.Sleep(1000);

                    if (LstAccountcreationUsername.Count > 0)
                    {
                        if (counter_Username < LstAccountcreationUsername.Count)
                        {
                            username = LstAccountcreationUsername[counter_Username];
                            counter_Username++;
                        }
                        else
                        {
                            AddToProxyAccountCreationLog("*********** /n All Usernames have been taken /n ***********");
                            break;
                        }
                    }
                    else
                    {
                        AddToProxyAccountCreationLog("Please Upload Usernames To Create Twitter Accounts");
                        break;
                    }

                    string name = LstAccountCreationName[countForInstance];
                    string capcthaUrl = string.Empty;

                    try
                    {
                        explorer.GoTo("https://twitter.com/signup");

                        string PageSource = explorer.Html.ToString();
                        if (PageSource.Contains("Sign out"))
                        {
                            explorer.Link(Find.ById("signout-button")).Click();
                            Thread.Sleep(2000);
                            explorer.GoTo("https://twitter.com/signup");
                            Thread.Sleep(2000);
                        }
                    }
                    catch (Exception ex)
                    {
                        AddToProxyAccountCreationLog("Taking too Long To respond.Page Not Loaded Fully");
                        break;
                    }

                    try
                    {

                        foreach (TextField item in explorer.TextFields)
                        {
                            string Html = item.OuterHtml.ToString();
                            if (item.Name == "user[name]")
                            {
                                AddToProxyAccountCreationLog("Name :" + name);
                                item.TypeText(name);
                                Thread.Sleep(1000);
                                break;
                            }
                        }

                        foreach (TextField item in explorer.TextFields)
                        {
                            if (item.Name == "user[email]")
                            {
                                AddToProxyAccountCreationLog("Email :" + email);
                                item.TypeText(email);
                                Thread.Sleep(1000);
                                break;
                            }
                        }

                        string EmailCheck = explorer.Html.ToString();
                        Thread.Sleep(8000);
                        if (EmailCheck.Contains("taken error active") || EmailCheck.Contains("invalid error active") || EmailCheck.Contains("blank error active"))
                        {
                            AddToProxyAccountCreationLog("Error In Email - " + email);
                            GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password + ":" + ProxyAdress + ":" + ProxyPort, Globals.path_DesktopFolder + "\\TwtDominator\\BrowserAccountUnsuccessfull.txt");
                        }
                        else
                        {
                            foreach (TextField item in explorer.TextFields)
                            {
                                string Html = item.OuterHtml.ToString();
                                if (item.Name == "user[user_password]")
                                {
                                    AddToProxyAccountCreationLog("Password :" + password);
                                    item.TypeText(password);
                                    Thread.Sleep(1000);
                                    break;
                                }
                            }

                            string CheckPassword = explorer.Html.ToString();

                            if (!CheckPassword.Contains("invalid error active") || !CheckPassword.Contains("blank error active") || !CheckPassword.Contains("blank error active") || CheckPassword.Contains("weak error active"))
                            {
                                AddToProxyAccountCreationLog("Password Not Secure creating Other");
                                password = password + RandomStringGenerator.RandomString(5);
                                if (password.Count() > 15)
                                {
                                    password = password.Remove(13); //Removes the extra characters
                                }

                                foreach (TextField item in explorer.TextFields)
                                {
                                    string Html = item.OuterHtml.ToString();
                                    if (item.Name == "user[user_password]")
                                    {
                                        AddToProxyAccountCreationLog("Password :" + password);
                                        item.TypeText(password);
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }



                            //AddToProxyAccountCreationLog("Username : " + username);
                            foreach (TextField item in explorer.TextFields)
                            {
                                //if (Usernamecheck.Contains(""))
                                //{
                                if (item.Name == "user[screen_name]")
                                {
                                    item.TypeText(username);
                                    Thread.Sleep(1000);
                                    break;
                                }
                                //}
                            }

                            string Usernamecheck = explorer.Html.ToString();
                            if (!Usernamecheck.Contains("taken error active") || !Usernamecheck.Contains("invalid error active") || !Usernamecheck.Contains("blank error active"))
                            {
                                if (username.Count() > 12)
                                {
                                    username = username.Remove(8); //Removes the extra characters
                                }
                                username = username + RandomStringGenerator.RandomString(5);
                                if (username.Count() > 15)
                                {
                                    username = username.Remove(13); //Removes the extra characters
                                }
                                foreach (TextField item in explorer.TextFields)
                                {
                                    if (item.Name == "user[screen_name]")
                                    {
                                        item.TypeText(username);
                                        Thread.Sleep(1000);
                                        break;
                                    }
                                }
                            }


                            Thread.Sleep(2000);

                            string PageSource = explorer.Html.ToString();

                            if (PageSource.Contains("sign-up-box"))
                            {
                                AddToProxyAccountCreationLog("Creating Twitter Account With Email : " + email);
                                explorer.Button(Find.ByValue("Create my account")).Click();
                                Thread.Sleep(2000);
                            }
                           
                                string PageSourceSignup = explorer.Html.ToString();


                                if (PageSource.Contains("signout-button") && PageSource.Contains("js-signout-button"))
                                {
                                    AddToProxyAccountCreationLog("Creating Twitter Account With Email : " + email);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password, Globals.Path_BrowserCreatedAccounts);
                                    string GetSignout = explorer.Html.ToString();
                                    if (GetSignout.Contains("Sign out"))
                                    {
                                        explorer.Link(Find.ById("signout-button")).Click();
                                        Thread.Sleep(2000);
                                        countForInstance++;
                                    }
                                }
                                else if (PageSource.Contains("Are you human?"))
                                {
                               
                                    AddToProxyAccountCreationLog("Checking For Human??");
                                
                                    Thread.Sleep(3000);
                                    try
                                    {
                                        foreach (Div Item in explorer.Divs)
                                        {
                                            string Url = Item.OuterHtml.ToString();
                                            if (Item.Id == "recaptcha_image")
                                            {
                                                int startIndex = Url.IndexOf("src=\"");
                                                string start = Url.Substring(startIndex).Replace("src=\"", "");
                                                int endIndex = start.IndexOf("\"");
                                                string end = start.Substring(0, endIndex);
                                                capcthaUrl = end;
                                                break;
                                            }
                                        }
                                        int i = 0;
                                       WebClient webclient = new WebClient();
                                   StartWebClient:
                                        Thread.Sleep(2000);
                                        try
                                        {
                                            byte[] args = webclient.DownloadData(capcthaUrl);
                                            string[] arr1 = new string[] { BaseLib.Globals.DBCUsername, BaseLib.Globals.DBCPassword, "" };
                                            string CapcthaString = DecodeDBC(arr1, args);
                                            foreach (TextField item in explorer.TextFields)
                                            {

                                                string Html = item.OuterHtml.ToString();
                                                if (item.Id == "recaptcha_response_field")
                                                {
                                                    AddToProxyAccountCreationLog("Adding Capctha Response");
                                                    item.TypeText(CapcthaString);
                                                    break;
                                                }
                                            }
                                            explorer.Button(Find.ByValue("Create my account")).Click();
                                        }
                                        catch (Exception ex)
                                        {
                                            i++;
                                            if (ex.Message.Contains("An exception occurred during a WebClient request."))
                                            {
                                                AddToProxyAccountCreationLog("Error In Capctha Download Trying Again - " + i);
                                                if (i <= 3)
                                                {
                                                    goto StartWebClient;
                                                }
                                                else
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password + ":" + ProxyAdress + ":" + ProxyPort, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TwtDominator\\UnsuccessfullBrowserAcounts.txt");
                                                }
                                            }
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                       
                                    }

                                    string ConfirmId = explorer.Html.ToString();

                                    AddToProxyAccountCreationLog("Confirming created Account");
                                    if (!ConfirmId.Contains("Sign out"))
                                    {
                                        AddToProxyAccountCreationLog("Account Not Created : " + email + ":" + password);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password + ":" + ProxyAdress + ":" + ProxyPort, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TwtDominator\\UnsuccessfullBrowserAcounts.txt");
                                    }
                                    else if (ConfirmId.Contains("Sign out"))
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password + ":" + ProxyAdress + ":" + ProxyPort, Globals.Path_BrowserCreatedAccounts);
                                        explorer.GoTo("https://twitter.com/");
                                        explorer.Link(Find.ById("signout-button")).Click();
                                        Thread.Sleep(1000);
                                    }
                                }
                            }
                        
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "The remote server returned an error: (504) Gateway Timeout.")
                        {
                            AddToProxyAccountCreationLog("Remote Server Returned Error - Time out");
                        }
                        explorer.Close();
                        AddToProxyAccountCreationLog("Closing Explorer Due To Error");
                        goto StartAgain;
                    }
                         
                    explorer.Close();
                    countForInstance++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error :: - " + DateTime.Now.ToString() + " || Error - " + ex.Message);
                        if (ex.Message.Contains("Creating an instance of the COM component with"))
                        {
                            goto StartAgain;
                        }
                    }
            }
        
        }

        public void ClearCookies()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            string[] dirs = Directory.GetFiles(path);

            //for deleting files
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                try
                {
                    dir.Delete(true); //delete subdirectories and files
                }
                catch (Exception ex)
                {

                }
            }
        }

        public string DecodeDBC(string[] args, byte[] imageBytes)
        {

            try
            {
                // Put your DBC username & password here:
                DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                    //Log("Please check your DBC Account Details");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    //Log("You have 0 Balance in your DBC Account");
                    return null;
                }

                for (int i = 2, l = args.Length; i < l; i++)
                {
                    Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                    // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                    // file name, file object, stream, or a vector of bytes, and desired
                    // solving timeout (in seconds) here:
                    DeathByCaptcha.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
                    if (null != captcha)
                    {
                        Console.WriteLine("CAPTCHA {0:D} solved: {1}", captcha.Id, captcha.Text);

                        //// Report an incorrectly solved CAPTCHA.
                        //// Make sure the CAPTCHA was in fact incorrectly solved, do not
                        //// just report it at random, or you might be banned as abuser.
                        //if (client.Report(captcha))
                        //{
                        //    Console.WriteLine("Reported as incorrectly solved");
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Failed reporting as incorrectly solved");
                        //}

                        return captcha.Text;
                    }
                    else
                    {
                        //Log("CAPTCHA was not solved");
                        Console.WriteLine("CAPTCHA was not solved");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public void SetProxy(string servername, string portnumber)
        {
            try
            {
                //GlobusHttpHelper objhelper = new GlobusHttpHelper();

                //string responce = objhelper.getHtmlfromUrlProxy(new Uri("http://www.google.co.in/"), servername, int.Parse(portnumber), "", "");

                string key = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
                string serverName = servername;//your proxy server name;

                string port = portnumber; //your proxy port;

                string proxy = serverName + ":" + port;

                RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(key, true);

                RegKey.SetValue("ProxyServer", proxy);

                RegKey.SetValue("ProxyEnable", 1);
            }
            catch { };

        }

        private void btnBrowserName_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtBrowserName.Text = ofd.FileName;
                        List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in tempList)
                        {
                            string newitem = item;
                            if (!string.IsNullOrEmpty(newitem))
                            {
                                LstAccountCreationName.Add(newitem);
                            }
                        }
                    }
                    AddToProxyAccountCreationLog(LstAccountCreationName.Count() + " Name in List");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnBrowserName_Click() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnBrowserEmail_Click(object sender, EventArgs e)
        {
            LstAccountcreationEmailPassword.Clear();
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtBrowserEmail.Text = ofd.FileName;
                        List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in tempList)
                        {
                            string newitem = item;
                            if (!string.IsNullOrEmpty(newitem))
                            {
                                LstAccountcreationEmailPassword.Add(newitem);
                            }
                        }
                    }
                    AddToProxyAccountCreationLog(LstAccountcreationEmailPassword.Count() + " Name in List");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnBrowserName_Click() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnBrowserUsername_Click(object sender, EventArgs e)
        {
            LstAccountcreationUsername.Clear();
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtBrowserUsername.Text = ofd.FileName;
                        List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        foreach (string item in tempList)
                        {
                            string newitem = item;
                            if (!string.IsNullOrEmpty(newitem))
                            {
                                LstAccountcreationUsername.Add(newitem);
                            }
                        }
                    }
                    AddToProxyAccountCreationLog(LstAccountcreationUsername.Count() + " Username in List");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnBrowserName_Click() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void frmMain_FormClosed_1(object sender, FormClosedEventArgs e)
        {

        }

        ////web browser td end

       
       

       


        

           
        
       

       

        


      

       

       

       

       


       

       

       

     

       
        
    }
}
