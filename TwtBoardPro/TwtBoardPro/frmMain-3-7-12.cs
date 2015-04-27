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
//using System.Reflection;


namespace TwtDominator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        List<string> lstscrapeUsername = new List<string>();
        List<string> lstUserNameID = new List<string>();
        List<string> lstScrapedUserID = new List<string>();
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

                listKeywords = new List<string>();

                TwitterSignup.TwitterSignUp.logEvents.addToLogger += new EventHandler(logEvents_Signup_addToLogger);

                EmailActivator.ClsEmailActivator.loggerEvents.addToLogger += new EventHandler(loggerEvents_EmailActivator_addToLogger);

                TweetAccountManager.logEvents_static.addToLogger += new EventHandler(logEvents_static_addToLogger);
                //TweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_GeneralLogger); 

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
            using (OpenFileDialog ofd = new OpenFileDialog())
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
                    list_listTargetURLs = Split(listUserIDs, 1);
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
                    list_userIDsToFollow = tweetAccountManager.GetNonFollowings(); 
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
            }
        }

        #region Tweet

        public List<string> listTweetMessages { get; set; }

        public List<string> listReplyMessages { get; set; }

        public List<string> listKeywords { get; set; }

        private void btnUploadTweetMessage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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
                tweetAccountManager.Tweet(lst_tweetMessage, tweetMinDealy, tweetMaxDealy);

                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
            }
        }

        private void btnStartReTweeting_Click(object sender, EventArgs e)
        {
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
                    catch { }

                    

                    try
                    {
                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadpoolMethod_Retweet), new object[] {item});

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
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

                tweetAccountManager.ReTweet("", retweetMinDealy, retweetMaxDealy);

                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
            }
        }

        private void btnStartReplying_Click(object sender, EventArgs e)
        {
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
                    catch { }

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

                tweetAccountManager.Reply(tweetMessage, replyMinDealy, replyMaxDealy);

                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
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
                    listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages);
                }

                if (chkUseSpinnedMessages.Checked)
                {
                    listTweetMessages = SpinnedListGenerator.GetSpinnedList(listTweetMessages);
                }

                TweetAccountManager.listTweetMessages = listTweetMessages;
                TweetAccountManager.listReplyMessages = listReplyMessages;

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
            if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
            {
                numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
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
            using (OpenFileDialog ofd = new OpenFileDialog())
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
                            if (stats == "doesn’t exist" || stats == "404")
                            {
                                AddToLog_Checker("Account : " + item + " does Not Exist");
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_NonExistingIDs);
                            }
                            if (stats == "suspended")
                            {
                                AddToLog_Checker("Account : " + item + " does Not Exist");
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_SuspendedIDs);
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


        public EventHandler logEvents_addToLogger1 { get; set; }

        public List<string> listUserNames { get; set; }

        List<string> listNames = new List<string>();

        List<string> ValidPublicProxies = new List<string>();

        List<string> listEmails = new List<string>();

        clsDBQueryManager objclsSettingDB = new clsDBQueryManager();


        private void btnName_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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

        private void btnUsernames_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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
                        if (!listUserNames.Contains(item))
                        {
                            listUserNames.Add(item);
                        }
                    }
                    Console.WriteLine(listUserNames.Count + " Usernames loaded");
                    AddToListAccountsLogs(listUserNames.Count + " Usernames loaded");


                }
            }
        }

        private void btnEmails_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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

                

                Thread thread_StartSignup = new Thread(StartSignUpMultithreaded);
                thread_StartSignup.Start();
            }
            catch{ }
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
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting Public Proxy From Data Base >>>> " + ex.Message + " || DateTime :- " + DateTime.Now , Globals.Path_TwtErrorLogs);
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



                    if (counter_Username < listUserNames.Count)
                    {
                    }
                    else
                    {
                        AddToListAccountsLogs("*********** /n All Usernames have been taken /n ***********");
                        break;
                    }
                    if (counter_Name < listNames.Count)
                    {
                    }
                    else
                    {
                        counter_Name = 0;
                    }


                    string username = string.Empty;
                    string name = string.Empty;

                    try
                    {
                        username = listUserNames[counter_Username];
                    }
                    catch(Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting UserName for Twt Account Creator >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        name = listNames[counter_Name];
                    }
                    catch(Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting Name for Twt Account Creator >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    



                    name = listNames[0];
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(twitterSignUp.SignupMultiThreaded), new object[] { email, username, name, Proxy});
                    counter_Name++;
                    counter_Username++;
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> In StartSignUpMultithreaded() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
        }


        private void btnExportAccounts_Click(object sender, EventArgs e)
        {

        }

        private void BtnProxyAssigner_Click(object sender, EventArgs e)
        {

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
            listEmails = GlobusFileHelper.ReadLargeFile(emailFile);
            foreach (string item in listEmails)
            {
                if (!listEmails.Contains(item))
                {
                    listEmails.Add(item);
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
                        catch { }

                        try
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
                        catch { }

                        try
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
                        catch { }

                        try
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
                        catch { }

                        try
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
                        catch { }

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

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_ProfileManager), new object[] { item, new string[]{profileUsername, profileLocation, profileURL, profileDescription, profilePic}});

                        count_profileUsername++;
                        count_profileURL++;
                        count_profileDescription++;
                        count_profileLocation++;
                        count_profilePic++;

                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
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
            }
        }

        private void btnUploadReplyFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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
            using (OpenFileDialog ofd = new OpenFileDialog())
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
            using (OpenFileDialog ofd = new OpenFileDialog())
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

        public List<string> lst_ProfileLocations { get; set; }

        public List<string> lst_ProfileUsernames { get; set; }

        public List<string> lst_ProfileURLs { get; set; }

        public List<string> lst_ProfileDescriptions { get; set; }

        public List<string> lstProfilePics { get; set; }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnEmailverification_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

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

        int countParseProxiesThreads = 0;
        Int64 workingproxiesCount = 0;
        int numberOfProxyThreads = 4;
        private static readonly object proxiesThreadLockr = new object();

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
            if (MessageBox.Show("Do you really want to delete all the Proxies from Database", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                setting.DeletePublicProxyData();
                AddToProxysLogs("All Public Proxies Cleared");
                workingproxiesCount = 0;
                lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                {
                    lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                }));
            }
        }

        private void btnClearPrivateProxies_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete all the Private Proxies from Database???", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                setting.DeletePrivateProxyData();
            }
        }

        private void btnPublicProxy_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
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
            using (OpenFileDialog ofd = new OpenFileDialog())
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
            using (OpenFileDialog ofd = new OpenFileDialog())
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
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtScrapeUpload.Text = ofd.FileName;
                    lstscrapeUsername.Clear();
                    txtScrapeUserName.Text = "";
                    lstscrapeUsername = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToScrapeLogs("Added Username To Scrape");
                }
            }
        }

        private void btnScrapeUser_Click(object sender, EventArgs e)
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
                        AddToScrapeLogs("Added " + lst_structTweetFollowersIDs.Count + " Followers to list");

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

                            }
                        }
                        

                        AddToScrapeLogs("Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword);
                        AddToScrapeLogs("Data Exported to " + Globals.Path_ScrapedFollowersList);
                        if (Globals.IsDirectedFromFollower)
                        {
                            AddToLog_Follower("Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword);
                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Following Scrape Users --> " + ex.Message, Globals.Path_TwtErrorLogs);
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

                            }
                        }
                        ////new Thread(() =>
                        ////{
                        //foreach (string data in lst_structTweetFollowingsIds)
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
                        //  }
                        //).Start();

                        AddToScrapeLogs("Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword);
                        AddToScrapeLogs("Data Exported to " + Globals.Path_ScrapedFollowingsList);
                        if (Globals.IsDirectedFromFollower)
                        {
                            AddToLog_Follower("Added " + lst_structTweetFollowersIDs.Count + " Followings from User: " + keyword);
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
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Following Scrape Users --> " + ex.Message , Globals.Path_TwtErrorLogs);
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
                    foreach (string data in Globals.lstScrapedUserIDs)
                    {
                        try
                        {
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            DataBase.InsertOrUpdateScrapeSetting(data, "");
                        }
                        catch (Exception ex)
                        {

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

        private void ScrapeKeywordSeacrh()
        {
            TwitterDataScrapper TweetData = new TwitterDataScrapper();
            TweetData.noOfRecords = 1000;

            List<TwitterDataScrapper.StructTweetIDs> data = TweetData.GetTweetData(txtScrapeKeyword.Text);

            AddToScrapeLogs(data.Count + " User ids Scraped ");

            foreach (TwitterDataScrapper.StructTweetIDs item in data)
            {
                Globals.lstScrapedUserIDs.Add(item.ID_Tweet_User);
                AddToScrapeLogs(item.ID_Tweet_User);
            }
            Globals.lstScrapedUserIDs = Globals.lstScrapedUserIDs.Distinct().ToList();

            new Thread(() =>
                    {
                        foreach (TwitterDataScrapper.StructTweetIDs item in data)
                        {
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            DataBase.InsertOrUpdateScrapeSetting(item.ID_Tweet_User, item.username__Tweet_User);
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

        private void btnExport_Click(object sender, EventArgs e)
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
                    using (OpenFileDialog ofd = new OpenFileDialog())
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
                    catch { }
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
            catch { }
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
            catch { }
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

        ProxyUtilitiesFromDataBase proxyFetcher = new ProxyUtilitiesFromDataBase();
        List<string> list_pvtProxy = new List<string>();
        private void btnAssignProxy_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Assign Private Proxies from Database???", "Confirm delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                    }
                    else
                    {
                        MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                    }
                }
                catch { }
            }
            else
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
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
                    }
                }
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
                            if (!string.IsNullOrEmpty(dRow[8].ToString()))
                            {
                                facebooker.profileStatus = int.Parse(dRow[8].ToString());
                            }

                            Globals.listAccounts.Add(facebooker.Username + ":" + facebooker.Password + ":" + facebooker.proxyAddress + ":" + facebooker.proxyPort + ":" + facebooker.proxyUsername + ":" + facebooker.proxyPassword);
                            TweetAccountContainer.dictionary_TweetAccount.Add(facebooker.Username, facebooker);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                        }

                    }
                    Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                    AddToGeneralLogs(Globals.listAccounts.Count + " Accounts loaded"); 
                }
            }
            catch { }
        }

        int accountsPerProxy = 10;  //Change this to change Number of Accounts to be set per proxy
        static int i = 0;
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

        private void txtFollowMaxDelay_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox28_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox26_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainerMain_Panel1_Paint(object sender, PaintEventArgs e)
        {

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


        public static Queue<string> queWorkingProxies { get; set; }

        public static readonly object proxyListLockr = new object();

       

        private void chkChangeExportLocation_CheckedChanged(object sender, EventArgs e)
        {

        }

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
            if (lstFakeEmailNames.Count > 0 && ((!string.IsNullOrEmpty(txtFakePassword.Text.Replace(" ","")) && (txtFakePassword.Text.Length >= 6)) || chkboxMakeRandPass.Checked) && !string.IsNullOrEmpty(txtDomain_FakeEmailCreator.Text) && (NumberHelper.ValidateNumber(txtFakeAccountsNo.Text) || string.IsNullOrEmpty(txtFakeAccountsNo.Text)))
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
                else if (string.IsNullOrEmpty(txtDomain_FakeEmailCreator.Text))
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
            if (!string.IsNullOrEmpty(txtFakeAccountsNo.Text) && NumberHelper.ValidateNumber(txtFakeAccountsNo.Text))
            {
                fakeEmailCount = Convert.ToInt32(txtFakeAccountsNo.Text);
            }
            int counter = 0;
            AddToFakeEmailCreator("Starting Fake Email Creation");
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
                    string domainName = txtDomain_FakeEmailCreator.Text.Replace("@", "");
                    string name = names;
                    string RandomNo = RandomStringGenerator.RandomNumber(5);
                    EmailId = name.ToLower() + RandomNo + "@" + domainName;
                    AddToFakeEmailCreator("Creating Email : " + EmailId);
                    if (chkboxMakeRandPass.Checked)
                    {
                        Password = RandomStringGenerator.RandomString(10);
                    }
                    else
                    {
                        Password = RemoveSpecialCharacters(txtFakePassword.Text.Replace("\\","").Replace("/","").Replace("\'","").Replace("\"",""));
                        if (!string.IsNullOrEmpty(Password) && !(Password.Length >= 6))
                        {
                            AddToFakeEmailCreator("Please Use AlphaNumeric Charachters Greater than 6 Chars");
                            return;
                        }
                    }
                    GlobusFileHelper.AppendStringToTextfileNewLine(EmailId + ":" + Password, Globals.Path_FakeEmailIds);
                    Globals.FakeEmailList.Add(EmailId + ":" + Password);
                }
                catch { }

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

        List<string> lstFakeEmailNames = new List<string>();

        int fakeEmailCount = 10;

        private void btnUploadNames_FakeEmailCreator_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtNames_FakeEmailCreator.Text = ofd.FileName;
                    lstFakeEmailNames = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToFakeEmailCreator(lstFakeEmailNames.Count() + " Email Names Uploaded");
                }
            }
        }

        private void groupBox27_Enter(object sender, EventArgs e)
        {

        }

        private void txtExportLocation_FakeEmailCreator_TextChanged(object sender, EventArgs e)
        {

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

        private void lstBoxGeneralLogs_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //public enum Module
        //{
        //    WaitAndReply, Tweet, Retweet, Reply, Follow, Unfollow, ProfileManager
        //}

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
        
        #endregion

      
        private void btnEnableProfileCreation_Click(object sender, EventArgs e)
        {

        }

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
                dictionary_Threads.Add(module + username, Thread.CurrentThread);
            }
            catch { }
        }

        private void groupBox32_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainer4_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void btnGetUserID_Click(object sender, EventArgs e)
        {
            lstScrapedUserID.Clear();
            lstUserNameID.Clear();
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                //ofd.Filter = "Text Files (*.txt)|*.txt";
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
                using (OpenFileDialog ofd = new OpenFileDialog())
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

        private void groupBox35_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox29_Enter(object sender, EventArgs e)
        {

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

        private void grpFollower_Enter(object sender, EventArgs e)
        {

        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void txtScrapeNoOfUsers_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabAccountCreator_Click(object sender, EventArgs e)
        {

        }

        private void groupBox22_Enter(object sender, EventArgs e)
        {

        }

        private void splitContainerFollower_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox36_Enter(object sender, EventArgs e)
        {

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

        private void gbGeneralLogs_Enter(object sender, EventArgs e)
        {

        }

        bool IsUsingDivideData = true;

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

      

        private void lstLoggerScrape_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabScrape_Click(object sender, EventArgs e)
        {

        }

        private void groupBox24_Enter(object sender, EventArgs e)
        {

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

        private void btnScheduleForLater_Tweeter_Click(object sender, EventArgs e)
        {
            //InsertUpdateSchedulerModule(strModule(Module.Tweet), dateTimePicker_Follower.Value.ToString());
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


        private void StartFollowing()
        {
            List<List<string>> list_listTargetURLs = new List<List<string>>();
            try
            {
                int numberOfThreads = 7;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {

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

                    //string userIDToFollow = txtUserIDtoFollow.Text;

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

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs });
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs, OtherUser, followMinDelay, followMaxDelay });

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

                //string userIDToFollow = (string)paramsArray.GetValue(1);
                List<string> list_userIDsToFollow = (List<string>)paramsArray.GetValue(1);

                bool OtherUser = (bool)paramsArray.GetValue(2);
                int strMinDelay = (int)paramsArray.GetValue(3);
                int strMaxDelay = (int)paramsArray.GetValue(4);

                //int intMinDelay = followMinDelay;
                //int intMaxDelay = followMaxDelay;
                int intMinDelay = strMinDelay;
                int intMaxDelay = strMaxDelay;

                //if (GlobusRegex.ValidateNumber(strMinDelay))
                //{
                //    intMinDelay = Convert.ToInt32(strMinDelay);
                //}
                //if (GlobusRegex.ValidateNumber(strMaxDelay))
                //{
                //    intMaxDelay = Convert.ToInt32(strMaxDelay);
                //}

                List<string> lstFollowers = new List<string>();
                List<string> lstFollowings = new List<string>();

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Follow), tweetAccountManager.Username);

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

                //if (GlobusRegex.ValidateNumber(txtFollowMinDelay.Text))
                //{
                //    followMinDelay = Convert.ToInt32(txtFollowMinDelay.Text); 
                //}
                //if (GlobusRegex.ValidateNumber(txtFollowMaxDelay.Text))
                //{
                //    followMaxDelay = Convert.ToInt32(txtFollowMaxDelay.Text); 
                //}

                tweetAccountManager.follower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;
                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }
                //tweetAccountManager.FollowUsingURLs(userIDToFollow);

                ////if (!string.IsNullOrEmpty(txtOtherfollow.Text))
                ////{
                ////    list_userIDsToFollow.Clear();
                ////    if (chkboxFollowers.Checked)
                ////    {
                ////        list_userIDsToFollow = GetFollowersUsingUserID(txtOtherfollow.Text);
                ////        OtherUser = true;
                ////    }
                ////    else if (chkboxFollowings.Checked)
                ////    {
                ////        list_userIDsToFollow = GetFollowingsUsingUserID(txtOtherfollow.Text);
                ////        OtherUser = true;
                ////    } 
                ////}

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

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

      

       

       

       

       


       

       

       

     

       
        
    }
}
