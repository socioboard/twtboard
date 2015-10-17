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
//using Newtonsoft.Json.Linq;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Web;
using System.Globalization;
using WatiN.Core;
using System.Net;
using IPSettings;
using Microsoft.Win32;
//using System.Reflection;
using System.Drawing.Drawing2D;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using Follower;
using System.Collections;


//using System.Diagnostics;


namespace twtboardpro
{
    public partial class frmMain_NewUI : System.Windows.Forms.Form
    {
        public frmMain_NewUI()
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

        #region GLobal Declaration
        Dictionary<string, List<string>> DMList = new Dictionary<string, List<string>>();
        List<string> lstscrapeUsername = new List<string>();
        List<string> lstscrapeUsernameInFollower = new List<string>();
        List<string> lstscrapeWhoTofoollow = new List<string>();
        List<string> lstUserNameID = new List<string>();
        List<string> lstScrapedUserID = new List<string>();
        List<string> lstFakeEmailNames = new List<string>();
        List<string> lstDirectMessage = new List<string>();
        List<string> lstUserId_Tweet = new List<string>();
        List<string> lstUserId_Retweet = new List<string>();
        List<string> lstPublicIPWOTest = new List<string>();
        List<string> listOfTwitterEmailIdAndPassword = new List<string>();
        List<string> lstGroupNames = new List<string>();
        List<string> lstSearchByKeywords = new List<string>();
        public List<string> listUserIDs = new List<string>();// { get; set; }

        public bool Ismentionsingleuser = false;
        public bool IsMentionUserWithScrapedList = false;
        public bool Ismentionrendomuser = false;
        public static List<string> LstRunningIP_IPModule = new List<string>();
        public static readonly object Locker_LstRunningIP_IPModule = new object();
        static int TweetExtractCount = 10;
        static int RetweetExtractCount = 10;
        int counter_Name = 0;
        int counter_Username = 0;
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
        int workingPvtProxiesCount = 0;
        int countParsePvtProxiesThreads = 0;
        int numberOfIPThreads = 4;
        public int threadcountForFinishMSG = 0;
        public int threadcountForFinishMsgPvt = 0;
        int accountsPerIP = 10;  //Change this to change Number of Accounts to be set per IP
        static int i = 0;
        bool IsUsingDivideData = true;
        bool CheckNetConn = false;
        /// <summary>
        /// Thread declaration
        /// </summary>
        Thread thread_StartScrape = null;
        Thread thread_StartKeywordScrape = null;
        Thread thread_StartScrapeInFollower = null;
        Thread thread_AddingKeywordScrape = null;
        Thread thread_UsernameToUserid = null;
        Thread thread_UsernameValue = null;
        Thread thread_IPCheck = null;

        GlobusHttpHelper globusHelper;


        /// <summary>
        /// Declaring Nvc objects
        /// </summary>
        System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();


        public List<string> listTweetMessages { get; set; }

        public List<string> listTweetMessagesForReply { get; set; }

        public List<string> listReplyMessages { get; set; }

        public List<string> listKeywords { get; set; }

        public List<string> lst_ProfileLocations { get; set; }

        public List<string> lst_ProfileUsernames { get; set; }

        public List<string> lst_ProfileURLs { get; set; }

        public List<string> lst_ProfileDescriptions { get; set; }

        public List<string> lstProfilePics { get; set; }

        public EventHandler logEvents_addToLogger1 { get; set; }

        public List<string> listUserNames { get; set; }


        public List<string> lstBackgroundPics { get; set; }

        List<string> listNames = new List<string>();

        List<string> ValidPublicProxies = new List<string>();

        List<string> listEmails = new List<string>();

        clsDBQueryManager objclsSettingDB = new clsDBQueryManager();

        private static readonly object proxiesThreadLockr = new object();

        private static readonly object WhoTofollowThreadLock = new object();

        private static readonly object pvtProxiesThreadLockr = new object();

        IPUtilitiesFromDataBase IPFetcher = new IPUtilitiesFromDataBase();

        List<string> list_pvtIP = new List<string>();

        public static Queue<string> queWorkingProxies = new Queue<string>();

        public static Queue<string> queWorkingPvtProxies = new Queue<string>();

        public Queue<string> QueueEmails = new Queue<string>();

        public static DataSet ds = new DataSet();

        public static readonly object IPListLockr = new object();

        public static List<string> lst_unfolloweruserlist = new List<string>();

        public readonly object lockerThreadsTweetFeature = new object();
        int countTweetMessageAccount = 0;

        #endregion

        private void myAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccounts frmaccounts = new frmAccounts();
            frmaccounts.Show();
        }

        private System.Drawing.Image image;
        private System.Drawing.Image recSideImage;

        private void frmMain_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
            recSideImage = Properties.Resources.rsz_app_bg;
            //dateTimePicker_Follow.MinDate = DateTime.Now;
            dateTimePicker_tweeterStart.MinDate = dateTimePicker_TwetterEnd.MinDate = DateTime.Now;
            dateTimePicker_WaitandreplyStart.MinDate = DateTime.Now;
            chkUseDivide.CheckState = CheckState.Unchecked;
            txtScrapeNoOfUsers.Enabled = false;
            txtMaximumTweet.Enabled = false;
            txtMaximumNoRetweet.Enabled = false;
            txtTweetUseGroup.Enabled = false;
            txtUseGroup.Enabled = false;
            txtIPUrl.Enabled = false;
            grpFollowByKeyword.Enabled = false;
            grpFollowerSetting.Enabled = false;
            chkCheckDatabaseInEvery2Minutes.Enabled = false;
            chkAutoFavorite.Enabled = false;
            //Panel_AccountCreatorithpProfiling.Enabled = false;
          //  gbAccCreatorProfileSetting.Enabled = false;
            listUserNames = new List<string>();
            listUserIDs = new List<string>();
            queWorkingProxies = new Queue<string>();
            listTweetMessages = new List<string>();
            listReplyMessages = new List<string>();
            listKeywords = new List<string>();
            grpFollowerSheduling.Enabled = false;

            

            CreateAppDirectories();
            CopyDatabase();

          //  Tb_AccountManager.TabPages.Remove(tabAccountCreator);
            Tb_AccountManager.TabPages.Remove(tabProfileManager);
         //   Tb_AccountManager.TabPages.Remove(tabFakeEmailCreator);
           // Tb_AccountManager.TabPages.Remove(tabAccountBrowser);


            clsDBQueryManager DataBase = new clsDBQueryManager();
            DataBase.DeleteScrapeSettings();


            LoadDefaultsCaptchaData();
           // LoadDefaultsAccountCreator();
            LoadDefaultsProfileData();
            LoadDefaultFollowerFiles();
            LoadDefaultAccountCheckerFiles();
            LoadDefaultTweeterFiles();
            LoadDefaultWaitnReplyFiles();
            LoadDefaultScrapingFiles();
            LoadDefaultTweetCreatorFiles();
           // LoadDefaultFakeEmailCreatorFile();
            LoadDefaultWhilteList();
        //    LoadDefaultsMobAccountCreator();
            ReloadAccountsFromDataBase();

         //   Tb_AccountManager.TabPages.Remove(tabMobileAccountCreator);
           // Tb_AccountManager.TabPages.Remove(tabAccountBrowser);
            //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
            TwitterSignup.TwitterSignUp.logEvents.addToLogger += new EventHandler(logEvents_Signup_addToLogger);

            EmailActivator.ClsEmailActivator.loggerEvents.addToLogger += new EventHandler(loggerEvents_EmailActivator_addToLogger);

            frmAccounts.AccountsLogEvents.addToLogger += new EventHandler(AccountsLogEvents_addToLogger);

            frmScheduler.Event_StartScheduler.raiseScheduler += new EventHandler(event_StartScheduler_raiseScheduler);

            frmScheduler.SchedulerLogger.addToLogger += new EventHandler(Schedulerlogger_addToLogger);

            FrmStartCampaign.startFlyCreationEvent.addToLogger += new EventHandler(startFlyCreationEvent_addToLogger);

            //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);
            FrmSettings.EnableDelaySettings.addToLogger += new EventHandler(logEvents_EnabledFollowDelaySetting);
            FrmSettings.DisableDelaySettings.addToLogger += new EventHandler(logEvents_DisabledFollowDelaySetting);

            FrmSettings.EnableDelaySettings.addToLogger += new EventHandler(logEvents_EnabledUnFollowDelaySetting);
            FrmSettings.DisableDelaySettings.addToLogger += new EventHandler(logEvents_DisabledUnFollowDelaySetting);

            new Thread(() =>
            {
                CheckingProxies();
            }).Start();

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10 * 30000);
                    ClearLogsIfExceeds();
                }
            }).Start();
        }

        #region followerDelaySetting
        public void DisableFollowDelayControl(string temp)
        {
            grpFollowerDelaySetting.Enabled = false;
            grpTweetDelaySetting.Enabled = false;
        }
        public void EnableFollowDelayControl(string temp)
        {
            grpFollowerDelaySetting.Enabled = true;
            grpTweetDelaySetting.Enabled = true;
        }

        void logEvents_EnabledFollowDelaySetting(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                EnableFollowDelayControl(eArgs.log);
            }
        }
        void logEvents_DisabledFollowDelaySetting(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                DisableFollowDelayControl(eArgs.log);
            }
        } 
        #endregion

        #region UnfollowerDelaySetting
        public void DisableUnFollowDelayControl(string temp)
        {
            txtUnfollowMinDelay.Enabled = false;
            txtUnfollowMaxDelay.Enabled = false;
        }
        public void EnableUnFollowDelayControl(string temp)
        {
            txtUnfollowMinDelay.Enabled = true;
            txtUnfollowMaxDelay.Enabled = true;
        }

        void logEvents_EnabledUnFollowDelaySetting(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                EnableUnFollowDelayControl(eArgs.log);
            }
        }
        void logEvents_DisabledUnFollowDelaySetting(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                DisableUnFollowDelayControl(eArgs.log);
            }
        }
        #endregion

        private void LoadDefaultAccountCheckerFiles()
        {
            try
            {
                #region TD Ubdate by me
                AccountChecker.AccountChecker.lst_Usernames.Clear();
                listOfTwitterEmailIdAndPassword.Clear();
                lstGroupNames.Clear();

                string UsernamesFilePathSettings = string.Empty;
                string EmailListFilePathSettings = string.Empty;
                string UploadGroupNamesSettings = string.Empty;
                string UploadEmailFilePath = string.Empty;

                //if (!string.IsNullOrEmpty(txtNames.Text) && !string.IsNullOrEmpty(txtUsernames.Text) && !string.IsNullOrEmpty(txtEmails.Text))
                //{
                //    objclsSettingDB.InsertOrUpdateSetting("AccountCreator", "Name", StringEncoderDecoder.Encode(txtNames.Text));
                #endregion

                //Globussoft1.GlobusHttpHelper "AccountChecker", "UploadGroupNames", StringEncoderDecoder.Encode(txtUploadGroupNames.Text)
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("AccountChecker" == row[0].ToString())
                        {
                            if ("UsernamesFilePath" == row[1].ToString())
                            {
                                UsernamesFilePathSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("EmailListFilePath" == row[1].ToString())
                            {
                                EmailListFilePathSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("UploadGroupNames" == row[1].ToString())
                            {
                                UploadGroupNamesSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("ACEmailFilePath" == row[1].ToString())
                            {
                                UploadEmailFilePath = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(UploadEmailFilePath))
                {
                    //Lst_EmailsForAccountChecking = GlobusFileHelper.ReadFiletoStringList(UploadEmailFilePath);
                    //txt_AccountCheckerEmailFile.Text = UploadEmailFilePath;

                    //AddToLog_Checker(Lst_EmailsForAccountChecking.Count + " Emails loaded");
                }

                if (File.Exists(UsernamesFilePathSettings))
                {
                    AccountChecker.AccountChecker.lst_Usernames = GlobusFileHelper.ReadFiletoStringList(UsernamesFilePathSettings);
                    txtUsernamesFilePath.Text = UsernamesFilePathSettings;

                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + AccountChecker.AccountChecker.lst_Usernames.Count + " UsernamesFilePath loaded ]");
                }

                if (File.Exists(EmailListFilePathSettings))
                {
                    listOfTwitterEmailIdAndPassword = GlobusFileHelper.ReadFiletoStringList(EmailListFilePathSettings);
                    txt_EmailListFilePath.Text = EmailListFilePathSettings;

                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + listUserNames.Count + " EmailListFilePath loaded ]");
                }

                if (File.Exists(UploadGroupNamesSettings))
                {
                    lstGroupNames = GlobusFileHelper.ReadFiletoStringList(UploadGroupNamesSettings);
                    txtUploadGroupNames.Text = UploadGroupNamesSettings;

                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + lstGroupNames.Count + " UploadGroupNames loaded ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultFollowerFiles()
        {
            try
            {
                #region TD Ubdate by me
                FollowtweetKeywordList.Clear();
                listUserIDs.Clear();

                string FollowtweetKeywordSettings = string.Empty;
                string UserIDsSettings = string.Empty;


                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("Follower" == row[0].ToString())
                        {
                            if ("PathUserIDs" == row[1].ToString())
                            {
                                UserIDsSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("FollowBySearchKey" == row[1].ToString())
                            {
                                FollowtweetKeywordSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(FollowtweetKeywordSettings))
                {
                    FollowtweetKeywordList = GlobusFileHelper.ReadFiletoStringList(FollowtweetKeywordSettings);
                    txt_FollowBySearchKey.Text = FollowtweetKeywordSettings;

                    //AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + FollowtweetKeywordList.Count + " FollowtweetKeyword loaded ]");
                }

                if (File.Exists(UserIDsSettings))
                {
                    listUserIDs = GlobusFileHelper.ReadFiletoStringList(UserIDsSettings);
                    txtPathUserIDs.Text = UserIDsSettings;

                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + listUserIDs.Count + " UserIDs loaded ]");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultTweeterFiles()
        {
            try
            {
                #region TD Ubdate by me

                //listTweetMessages = new List<string>();
                //listTweetMessages.Clear();
                //listReplyMessages.Clear();

                string TweetKeywordSetting = string.Empty;
                string ReplyKeywordSetting = string.Empty;
                #endregion

                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("Tweeter" == row[0].ToString())
                        {
                            if ("TweetMessageFilePath" == row[1].ToString())
                            {
                                TweetKeywordSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(TweetKeywordSetting))
                {
                    //listTweetMessages = GlobusFileHelper.ReadFiletoStringList(TweetKeywordSetting);
                    //listReplyMessages = GlobusFileHelper.ReadFiletoStringList(TweetKeywordSetting);
                    //AddToLog_Tweet(listTweetMessages.Count + " Tweet Message loaded");
                    //AddToLog_Tweet(listReplyMessages.Count + " Reply Message loaded");
                    txtTweetMessageFile.Text = TweetKeywordSetting;


                    //Tweet MAG's ......Change by Abhishek (14/1/13)
                    List<string> listTweetMessages1 = new List<string>();
                    listTweetMessages1 = GlobusFileHelper.ReadFiletoStringList(TweetKeywordSetting);

                    listTweetMessages1.ForEach(tmsg =>
                    {
                        if (tmsg.Length <= 140)
                        {
                            listTweetMessages.Add(tmsg.ToString());
                        }
                    });

                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + listTweetMessages.Count + " Tweet Message loaded ]");
                    if (listTweetMessages.Count < listTweetMessages1.Count)
                    {
                        int count = (listTweetMessages1.Count - listTweetMessages.Count);

                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + count + " Tweet Message are greater than the limit of 140. ]");
                    }


                    ////Reply MAG's .....Change by Abhishek (14/1/13)
                    //List<string> listReplyMessages1 = new List<string>();
                    //listReplyMessages1 = GlobusFileHelper.ReadFiletoStringList(ReplyKeywordSetting);

                    //listReplyMessages1.ForEach(Rmsg =>
                    //{
                    //    if (Rmsg.Length <= 140)
                    //    {
                    //        listReplyMessages.Add(Rmsg.ToString());
                    //    }
                    //});

                    //AddToLog_Tweet(listReplyMessages.Count + " Reply Message loaded");
                    //if (listReplyMessages.Count < listReplyMessages1.Count)
                    //{
                    //    AddToLog_Tweet(listReplyMessages1.Count - listReplyMessages.Count + " Reply Message are greater than the limit of 140.");
                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void LoadDefaultWaitnReplyFiles()
        {
            try
            {
                #region TD Ubdate by me
                //listTweetMessages.Clear();
                //listReplyMessages.Clear();

                string TweetKeywordSetting = string.Empty;
                string ReplyKeywordSetting = string.Empty;
                string KeywordSetting = string.Empty;
                #endregion

                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("WaitAndReply" == row[0].ToString())
                        {
                            if ("ReplyMsgFile" == row[1].ToString())
                            {
                                ReplyKeywordSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("TweetMsgFile" == row[1].ToString())
                            {
                                TweetKeywordSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("KeywordMsgFile" == row[1].ToString())
                            {
                                KeywordSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(ReplyKeywordSetting))
                {
                    if (File.Exists(ReplyKeywordSetting))
                    {
                        listReplyMessages = GlobusFileHelper.ReadFiletoStringList(ReplyKeywordSetting);
                        txtReplyMsgFile.Text = ReplyKeywordSetting;
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listReplyMessages.Count + " Reply Messages Loaded ]");


                    }
                    else
                    {
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + ReplyKeywordSetting + " File Does Not Exsist ]");
                    }
                }
                if (File.Exists(TweetKeywordSetting))
                {
                    if (File.Exists(TweetKeywordSetting))
                    {
                        listTweetMessagesForReply = GlobusFileHelper.ReadFiletoStringList(TweetKeywordSetting);
                        txtTweetMsgFile.Text = TweetKeywordSetting;
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listTweetMessagesForReply.Count + " Tweet Messages Loaded ]");

                    }
                    else
                    {
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + TweetKeywordSetting + " File Does Not Exsist ]");
                    }
                }
                if (File.Exists(KeywordSetting))
                {
                    if (File.Exists(KeywordSetting))
                    {
                        listKeywords = GlobusFileHelper.ReadFiletoStringList(KeywordSetting);
                        txtKeywordFile.Text = KeywordSetting;
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listKeywords.Count + " Keyword Loaded ]");
                    }
                    else
                    {
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + KeywordSetting + " File Does Not Exsist ]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        public void LoadDefaultScrapingFiles()
        {
            try
            {
                #region TD Ubdate by me

                string FollowerFollowingSetting = string.Empty;
                string UserIdSetting = string.Empty;

                #endregion

                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("UserScrape" == row[0].ToString())
                        {
                            if ("ScrapeFollowerFollowing" == row[1].ToString())
                            {
                                FollowerFollowingSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("ScrapeUserID" == row[1].ToString())
                            {
                                UserIdSetting = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(FollowerFollowingSetting))
                {
                    lstscrapeUsername = GlobusFileHelper.ReadFiletoStringList(FollowerFollowingSetting);
                    txtScrapeUpload.Text = FollowerFollowingSetting;
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + lstscrapeUsername.Count + " Usernames Loaded ]");
                }
                else
                {
                    //AddToScrapeLogs(FollowerFollowingSetting + " File Does Not Exsist");
                }

                if (File.Exists(UserIdSetting))
                {
                    lstScrapedUserID = GlobusFileHelper.ReadFiletoStringList(UserIdSetting);
                    txtUsernameToUserIDPath.Text = UserIdSetting;
                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + lstScrapedUserID.Count + " Tweet Messages Loaded ]");
                }
                else
                {
                    //AddToScrapeLogs(UserIdSetting + " File Does Not Exsist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void LoadDefaultTweetCreatorFiles()
        {
            try
            {
                #region TD Ubdate by me
                string SpinnedMessageInput = string.Empty;
                string SpinnedMessageOutput = string.Empty;
                string TweetExtractor = string.Empty;
                string ReTweetExtractor = string.Empty;
                #endregion

                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("TweetCreator" == row[0].ToString())
                        {
                            if ("SpinnedMessageTextInput" == row[1].ToString())
                            {
                                SpinnedMessageInput = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("SpinnedMessageTextOutPut" == row[1].ToString())
                            {
                                SpinnedMessageOutput = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("TweetExtractor" == row[1].ToString())
                            {
                                TweetExtractor = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                            if ("TweetExtractor" == row[1].ToString())
                            {
                                ReTweetExtractor = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(SpinnedMessageInput))
                {
                    list_TweetCreatorMessages = GlobusFileHelper.ReadFiletoStringList(SpinnedMessageInput);
                    txtTweetCreatorMessageFile.Text = SpinnedMessageInput;

                    if (list_TweetCreatorMessages.Count > 0)
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + list_TweetCreatorMessages.Count + " Input Message Loaded");
                    }
                }
                else
                {
                    //AddToTweetCreatorLogs(SpinnedMessageInput + " File Does Not Exsist");
                }

                if (File.Exists(SpinnedMessageOutput))
                {
                    txtTweetCreatorExportFile.Text = SpinnedMessageOutput;
                    path_TweetCreatorExportFile = SpinnedMessageOutput;
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + SpinnedMessageOutput + " Output File Loaded");
                }
                else
                {
                    //AddToTweetCreatorLogs(SpinnedMessageOutput + " File Does Not Exsist");
                }

                if (File.Exists(TweetExtractor))
                {
                    lstUserId_Tweet = GlobusFileHelper.ReadFiletoStringList(TweetExtractor);
                    txtExtractorFile.Text = TweetExtractor;

                    if (lstUserId_Tweet.Count > 0)
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstUserId_Tweet.Count + " User Id Loaded");
                    }
                }
                else
                {
                    //AddToTweetCreatorLogs(TweetExtractor + " File Does Not Exsist");
                }

                if (File.Exists(ReTweetExtractor))
                {
                    lstUserId_Retweet = GlobusFileHelper.ReadFiletoStringList(ReTweetExtractor);
                    txtRetweetFile.Text = ReTweetExtractor;
                    if (lstUserId_Tweet.Count > 0)
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstUserId_Tweet.Count + " User Id Loaded");
                    }
                }
                else
                {
                    //AddToTweetCreatorLogs(ReTweetExtractor + " File Does Not Exsist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }



        public void LoadDefaultWhilteList()
        {
            try
            {
                #region TD Ubdate by me
                string FakeEmailFirstName = string.Empty;
                string FakeEmailLastName = string.Empty;
                #endregion

                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("WhileListBlackList" == row[0].ToString())
                        {
                            if ("UploadUserName" == row[1].ToString())
                            {
                                FakeEmailFirstName = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(FakeEmailFirstName))
                {
                    lst_ofScrapeduserAccounts = GlobusFileHelper.ReadFiletoStringList(FakeEmailFirstName);
                    txt_AccountListFileUpload.Text = FakeEmailFirstName;
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts.Count + " User Loaded");
                }
                else
                {
                    //AddToTweetCreatorLogs(FakeEmailFirstName + " File Does Not Exsist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void CheckingProxies()
        {
            try
            {
                clsDBQueryManager Db = new clsDBQueryManager();
                DataSet ds = Db.SelectPublicIPData();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ThreadPool.SetMaxThreads(25, 5);

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolCheckProxies), new object[] { dr });

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckingProxies()  --> " + ex.Message, Globals.Path_IPSettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckingProxies() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void ThreadPoolCheckProxies(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                DataRow dr = (DataRow)paramsArray.GetValue(0);

                clsDBQueryManager db = new clsDBQueryManager();

                string IPAddress = dr[0].ToString();
                string IPPort = dr[1].ToString();
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;
                int IsPublic = 0;
                int Working = 0;

                IPChecker pc = new IPChecker(IPAddress, IPPort, IPUsername, IPpassword, 0);
                bool RunningIP = pc.CheckIP();

                if (!RunningIP)
                {
                    DataBaseHandler Db = new DataBaseHandler();
                    DataBaseHandler.DeleteQuery("DELETE FROM tb_IP WHERE IPAddress ='" + IPAddress + "' AND IPPort ='" + IPPort + "'", "tb_IP");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolCheckProxies()  --> " + ex.Message, Globals.Path_IPSettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolCheckProxies() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        #region Checking OS
        static bool is64BitProcess = (IntPtr.Size == 8);
        static bool is64BitOperatingSystem = is64BitProcess || InternalCheckIsWow64();

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        public static bool InternalCheckIsWow64()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

       

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
                //StartScheduler(eArgs.module);
            }
        }

        void DataScraperlogger_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToScrapeLogs(eArgs.log);
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
                    StartWaitAndReply();
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
                //if (btnStart_FakeEmailCreator.InvokeRequired)
                //{
                //    lstFakeEmailCreatorLogger.Invoke(new MethodInvoker(delegate
                //    {
                //        lstFakeEmailCreatorLogger.Items.Add(log);
                //        lstFakeEmailCreatorLogger.SelectedIndex = lstFakeEmailCreatorLogger.Items.Count - 1;
                //    }));
                //}
                //else
                //{
                //    lstFakeEmailCreatorLogger.Items.Add(log);
                //    lstFakeEmailCreatorLogger.SelectedIndex = lstFakeEmailCreatorLogger.Items.Count - 1;
                //}

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
                        //if (lstBoxAccountsLogs.Items.Count >= 10000)
                        //{
                        //    lstBoxAccountsLogs.Items.Clear();
                        //}
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

        void LogEvents_IP_Logs(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToIPsLogs(eArgs.log);
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

       


        void logEvents_Mobile_Signup_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToMobileLogs(eArgs.log);
            }
        }

        private void AddToMobileLogs(string log)
        {
            try
            {
                //if (lstLoggerMobileAccCreator.InvokeRequired)
                //{
                //    lstLoggerMobileAccCreator.Invoke(new MethodInvoker(delegate
                //    {
                //        lstLoggerMobileAccCreator.Items.Add(log);
                //        lstLoggerMobileAccCreator.SelectedIndex = lstLoggerMobileAccCreator.Items.Count - 1;
                //    }));
                //}
                //else
                //{
                //    lstLoggerMobileAccCreator.Items.Add(log);
                //    lstLoggerMobileAccCreator.SelectedIndex = lstLoggerMobileAccCreator.Items.Count - 1;
                //}
            }
            catch { }
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
            if (!Directory.Exists(Globals.path_DesktopUsedTweetedImage))
            {
                Directory.CreateDirectory(Globals.path_DesktopUsedTweetedImage);
            }
            if (!Directory.Exists(Globals.path_ScrapedImageFolder))
            {
                Directory.CreateDirectory(Globals.path_ScrapedImageFolder);
            }
        }

        private void CopyDatabase()
        {

            string startUpDB = Application.StartupPath + "\\DB_twtboardpro.db";
            string localAppDataDB = Globals.path_AppDataFolder + "\\DB_twtboardpro.db";

            string startUpDB64 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\DB_twtboardpro.db";

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
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\twtboardpro");
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
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\twtboardpro");
                            File.Copy(startUpDB64, localAppDataDB);
                        }
                    }
                }
            }
        }

        #region Follow




        private void btnPathUserIDs_Click(object sender, EventArgs e)
        {
            try
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
                    else
                    {
                        listUserIDs = new List<string>();
                        txtPathUserIDs.Text = string.Empty;
                        AddToLog_Follower(listUserIDs.Count + " User IDs to Follow loaded");
                    }
                }
            }
            catch { }
        }

        public bool FollowUsrnameFollowerAndFriends = false;
        int counter_AccountFollwer = 0;
        public bool IsStart_ScrapUserInFollower = false;
        private void btnStartFollowing_Click(object sender, EventArgs e)
        {
            Globals.totalcountFollower = 0;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                new Thread(() =>
                {
                    startFollowingProcessThread();
                }).Start();
                
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }


        public void startFollowingProcessThread()
        {
            try
            {
                //if (Globals.IsGlobalDelay)
                //{
                //    if (Globals.IsCheckValueOfDelay)
                //    {
                //        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Global Delay Setting is Active. ]");
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please Enter valid global Delay and check global setting option.");
                //        return;
                //    }
                //}
                

                if (!string.IsNullOrEmpty(txtPathUserIDs.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("Follower", "PathUserIDs", StringEncoderDecoder.Encode(txtPathUserIDs.Text));
                }

                if ((!string.IsNullOrEmpty(txtUserIDtoFollow.Text) || listUserIDs.Count > 0 || !string.IsNullOrEmpty(txtOtherfollow.Text) && chkFollowers.Checked || !string.IsNullOrEmpty(txtOtherfollow.Text) && chkFollowings.Checked || Globals.lstScrapedUserIDs.Count > 0) && !string.IsNullOrEmpty(txtFollowMaxDelay.Text) && !string.IsNullOrEmpty(txtFollowMinDelay.Text) && NumberHelper.ValidateNumber(txtFollowMaxDelay.Text) && NumberHelper.ValidateNumber(txtFollowMinDelay.Text))
                {

                    if (!Chk_IsFollowerUserId.Checked && !Chk_IsFollowerScreanName.Checked)
                    {
                        MessageBox.Show("Please check between screen name or UserID option.");
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please check between screen name or UserID option. ]");
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtOtherfollow.Text) && chkFollowers.Checked && chkFollowings.Checked)
                    {
                        FollowUsrnameFollowerAndFriends = true;
                    }
                    if (!string.IsNullOrEmpty(txtUserIDtoFollow.Text))
                    {
                        listUserIDs.Clear();
                        listUserIDs.Add(txtUserIDtoFollow.Text);
                    }

                    if (!string.IsNullOrEmpty(txtUserIDtoFollow.Text) && chkboxScrapeFollowersInFollower.Checked || !string.IsNullOrEmpty(txtUserIDtoFollow.Text) && chkboxScrapeFollowingsInFollower.Checked)
                    {
                        //listUserIDs.Clear();
                        //listUserIDs.Add(txtUserIDtoFollow.Text);

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


                    if (chkboxUseGroups.Checked)
                    {
                        if (string.IsNullOrEmpty(txtUseGroup.Text))
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Group Name ]");
                            return;
                        }
                    }

                    if (TweetAccountManager.IsTweetedInXdays)
                    {
                        if (!string.IsNullOrEmpty(txtNotFollowUsersThatNotTweetedinlastdays.Text.Trim()) && NumberHelper.ValidateNumber(txtNotFollowUsersThatNotTweetedinlastdays.Text))
                        {
                            TweetAccountManager.daysTweetedInxdays = txtNotFollowUsersThatNotTweetedinlastdays.Text.Trim();
                        }
                        else
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please enter proper days. ]");
                            return;
                        }
                    }

                    if (IsStart_ScrapUserInFollower)
                    {
                        if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                        {
                            //IsStart_ScrapUserInFollower = false;
                            //thread_StartScrapeInFollower = new Thread(() =>
                            //{
                            
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Starting Scraping User ]");

                            //if (btnStartFollowing.InvokeRequired)
                            //{
                            //    btnStartFollowing.Invoke(new MethodInvoker(delegate
                            //      {
                            //          AddToLog_Follower("[ " + DateTime.Now + " ] => [ Starting Scraping User ]");
                            //          btnStartFollowing.Cursor = Cursors.Default;
                            //      }));
                            //}

                            threadStartScrapeInFollower();
                            //listUserIDs.RemoveAt(0);
                            if (listUserIDs.Count == 0)
                            {
                                return;
                            }
                            //});
                            //thread_StartScrapeInFollower.Start();
                        }
                        else
                        {
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts To Start Scraping ]");
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

                                if (!IsFastfollow)
                                {
                                    Thread.Sleep(500);
                                }
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
                    if (string.IsNullOrEmpty(txtUserIDtoFollow.Text) || !(listUserIDs.Count > 0) || string.IsNullOrEmpty(txtOtherfollow.Text) && !chkFollowers.Checked || string.IsNullOrEmpty(txtOtherfollow.Text) && !chkFollowings.Checked)
                    {
                        MessageBox.Show("Please enter the proper value in related using field.");
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
                    else if (!string.IsNullOrEmpty(txtOtherfollow.Text))
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
        private void threadStartScrapeInFollower()
        {
            TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
            try
            {
                List<string> lst_structTweetFollowersIDs = new List<string>();
                List<string> lst_structTweetFollowingsIds = new List<string>();
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                string user_id = string.Empty;
                int counter = 0;
                TweetAccountManager TweetLogin = new TweetAccountManager();

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }

                    TweetLogin = new TweetAccountManager();
                    TweetLogin.Username = item.Key;
                    TweetLogin.Password = item.Value.Password;
                    TweetLogin.IPAddress = item.Value.IPAddress;
                    TweetLogin.IPPort = item.Value.IPPort;
                    TweetLogin.IPUsername = item.Value.IPUsername;
                    TweetLogin.IPpassword = item.Value.IPpassword;
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Logging Process start for scrapping User ]");
                    TweetLogin.Login();
                    if (!TweetLogin.IsLoggedIn)
                    {
                        continue;
                    }
                    else
                    {
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Logged in successful]");
                        globusHttpHelper = TweetLogin.globusHttpHelper;
                        counter++;
                        break;
                    }
                }
                string ReturnStatus = string.Empty;

                if (!TweetLogin.IsLoggedIn)
                {
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Upload Atleast One Working Account To Get Details ]");
                    return;
                }


                //foreach (string keyword in listUserIDs)//foreach (string keyword in lstscrapeUsername)
                //{
                   string  keyword = txtUserIDtoFollow.Text.Trim();
                    
                    dataScrapeer.CounterDataNo = 500;
                    listUserIDs.Clear();

                    txtlimitScrapeUsersINFollower.Invoke(new MethodInvoker(delegate
                    {
                        if (!string.IsNullOrEmpty(txtlimitScrapeUsersINFollower.Text) && NumberHelper.ValidateNumber(txtlimitScrapeUsersINFollower.Text))
                        {
                            dataScrapeer.CounterDataNo = Convert.ToInt32(txtlimitScrapeUsersINFollower.Text);
                        }
                    }));

                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }

                    if (chkboxScrapeFollowersInFollower.Checked)
                    {
                        try
                        {
                            if (!File.Exists(Globals.Path_ScrapedFollowersList))
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowersUserID , Followers User Name", Globals.Path_ScrapedFollowersList);
                            }
                            string returnStatus = string.Empty;
                            dataScrapeer.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                         //   lst_structTweetFollowersIDs = dataScrapeer.GetFollowers_New(keyword.Trim(), out ReturnStatus, ref globusHttpHelper);
                            lst_structTweetFollowersIDs = dataScrapeer.GetFollowers_New_WithNo_Followers(keyword.Trim(), out ReturnStatus, ref globusHttpHelper);

                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Followers Scrapped ]");
                            //TwitterDataScrapper.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
                                       
                            if (lst_structTweetFollowersIDs.Count > 0)
                            {
                                //listUserIDs.Clear();
                                //string item=Globals.Path_ScrapedFollowersList.Split(':')[0];
                                if (Chk_IsFollowerUserId.Checked)
                                {
                                    foreach (string item in lst_structTweetFollowersIDs)
                                    {
                                        string userid = item.Split(':')[0];
                                        listUserIDs.Add(userid);
                                    }
                                }

                                else if (Chk_IsFollowerScreanName.Checked)
                                {
                                    foreach (string item in lst_structTweetFollowersIDs)
                                    {
                                        string username = item.Split(':')[1];
                                        listUserIDs.Add(username);
                                    }
                                }

                                if (lst_structTweetFollowersIDs.Count > 0)
                                {
                                    //listUserIDs.AddRange(lst_structTweetFollowersIDs);
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword + " ]");
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowersList);
                                }

                            }
                            else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Sorry, That User does not exist =>" + ReturnStatus.Split(':')[1] + " ]");
                                //continue;
                            }
                            else if (ReturnStatus == "Account is Suspended. ")
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Account " + keyword + "  is Suspended. ]");
                            }
                            else if (ReturnStatus == "Error")
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                            }
                            else
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + keyword + " does not have any Followers ]");
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    if (chkboxScrapeFollowingsInFollower.Checked)
                    {
                        try
                        {
                            if (!File.Exists(Globals.Path_ScrapedFollowingsList))
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowersUserID , Followers User Name", Globals.Path_ScrapedFollowingsList);
                            }
                            string returnStaus = string.Empty;
                            dataScrapeer.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                            lst_structTweetFollowingsIds = dataScrapeer.GetFollowings_New(keyword.Trim(), out returnStaus, ref globusHttpHelper);
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Following Scrapped ]");
                            if (lst_structTweetFollowingsIds.Count > 0)
                            {

                                //listUserIDs.Clear();
                                if (Chk_IsFollowerUserId.Checked)
                                {
                                    foreach (string item in lst_structTweetFollowingsIds)
                                    {
                                        string userid = item.Split(':')[0];
                                        listUserIDs.Add(userid);
                                    }
                                }

                                else if (Chk_IsFollowerScreanName.Checked)
                                {
                                    foreach (string item in lst_structTweetFollowingsIds)
                                    {
                                        string username = item.Split(':')[1];
                                        listUserIDs.Add(username);
                                    }
                                }
                               // listUserIDs.AddRange(lst_structTweetFollowingsIds);
                                //AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Adding To DataBase and File ]");
                                if (lst_structTweetFollowingsIds.Count > 0)
                                {

                                  AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword + " ]");
                                   AddToLog_Follower("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowingsList + " ]");

                                }

                            }
                            else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Sorry, That User does not exist =>" + ReturnStatus.Split(':')[1] + " ]");
                                //continue;
                            }
                            else if (ReturnStatus == "Account is Suspended. ")
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Account " + keyword + "  is Suspended. ]");
                            }
                            else if (ReturnStatus == "Error")
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                            }
                            else
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + keyword + " User does not exist ]");
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                    //break;
                    //Globals.lstScrapedUserIDsInFollower = Globals.lstScrapedUserIDsInFollower.Distinct().ToList();
            

            }
            catch (Exception)
            {

            }
            finally
            {
                IsStart_ScrapUser = true;
                dataScrapeer.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
            }
        }




        private void threadStartScrapeInTweetMentionUser()
        {
            TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
            try
            {
                List<string> lst_structTweetFollowersIDs = new List<string>();
                List<string> lst_structTweetFollowingsIds = new List<string>();
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                string user_id = string.Empty;
                int counter = 0;
                TweetAccountManager TweetLogin = new TweetAccountManager();

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }

                    TweetLogin = new TweetAccountManager();
                    TweetLogin.Username = item.Key;
                    TweetLogin.Password = item.Value.Password;
                    TweetLogin.IPAddress = item.Value.IPAddress;
                    TweetLogin.IPPort = item.Value.IPPort;
                    TweetLogin.IPUsername = item.Value.IPUsername;
                    TweetLogin.IPpassword = item.Value.IPpassword;
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Logging Process start for scrapping User ]");
                    TweetLogin.Login();
                    if (!TweetLogin.IsLoggedIn)
                    {
                        continue;
                    }
                    else
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Logged in successful]");
                        globusHttpHelper = TweetLogin.globusHttpHelper;
                        counter++;
                        break;
                    }
                }
                string ReturnStatus = string.Empty;

                if (!TweetLogin.IsLoggedIn)
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Atleast One Working Account To Get Details ]");
                    return;
                }


                //foreach (string keyword in listUserIDs)//foreach (string keyword in lstscrapeUsername)
                //{
                string keyword = txtTweetScrapUserData.Text.Trim();

                dataScrapeer.CounterDataNo = 100;
                listUserIDs.Clear();

                txtlimitScrapeUsersINFollower.Invoke(new MethodInvoker(delegate
                {
                    if (!string.IsNullOrEmpty(txtTweetEnteNoofUser.Text) && NumberHelper.ValidateNumber(txtTweetEnteNoofUser.Text))
                    {
                        dataScrapeer.CounterDataNo = Convert.ToInt32(txtTweetEnteNoofUser.Text);
                    }
                }));

                if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                {
                    counter = 0;
                }

                if (chkTweetScrapFollowers.Checked)
                {
                    try
                    {
                        if (!File.Exists(Globals.Path_ScrapedFollowersList))
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowersUserID , Followers User Name", Globals.Path_ScrapedFollowersList);
                        }
                        string returnStatus = string.Empty;
                        dataScrapeer.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                        lst_structTweetFollowersIDs = dataScrapeer.GetFollowers_New(keyword.Trim(), out ReturnStatus, ref globusHttpHelper);
                        dataScrapeer.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Followers Scrapped ]");
                        //TwitterDataScrapper.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);

                        if (lst_structTweetFollowersIDs.Count > 0)
                        {
                            //listUserIDs.Clear();
                            //string item=Globals.Path_ScrapedFollowersList.Split(':')[0];
                            //if (Chk_IsFollowerUserId.Checked)
                            {
                                foreach (string item in lst_structTweetFollowersIDs)
                                {
                                    string userid = item.Split(':')[1];
                                    lst_mentionUser.Add(userid);
                                }
                            }


                            if (lst_structTweetFollowersIDs.Count > 0)
                            {
                                //listUserIDs.AddRange(lst_structTweetFollowersIDs);
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword + " ]");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowersList);
                            }

                        }
                        else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Sorry, That User does not exist =>" + ReturnStatus.Split(':')[1] + " ]");
                            //continue;
                        }
                        else if (ReturnStatus == "Account is Suspended. ")
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Account " + keyword + "  is Suspended. ]");
                        }
                        else if (ReturnStatus == "Error")
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                        }
                        else
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + keyword + " does not have any Followers ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeUser_Click() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                if (chkRetweetScrapFollowings.Checked)
                {
                    try
                    {
                        if (!File.Exists(Globals.Path_ScrapedFollowingsList))
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowersUserID , Followers User Name", Globals.Path_ScrapedFollowingsList);
                        }
                        string returnStaus = string.Empty;
                        
                        dataScrapeer.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                        lst_structTweetFollowingsIds = dataScrapeer.GetFollowings_New(keyword.Trim(), out returnStaus, ref globusHttpHelper);
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Following Scrapped ]");
                        if (lst_structTweetFollowingsIds.Count > 0)
                        {

                            //listUserIDs.Clear();
                            //if (Chk_IsFollowerUserId.Checked)
                            {
                                foreach (string item in lst_structTweetFollowingsIds)
                                {
                                    string userid = item.Split(':')[1];
                                    lst_mentionUser.Add(userid);
                                }
                            }

                            
                            if (lst_structTweetFollowingsIds.Count > 0)
                            {

                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword + " ]");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowingsList + " ]");

                            }

                        }
                        else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Sorry, That User does not exist =>" + ReturnStatus.Split(':')[1] + " ]");
                            //continue;
                        }
                        else if (ReturnStatus == "Account is Suspended. ")
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Account " + keyword + "  is Suspended. ]");
                        }
                        else if (ReturnStatus == "Error")
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                        }
                        else
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + keyword + " User does not exist ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                

            }
            catch (Exception)
            {

            }
            finally
            {
                IsStart_ScrapUser = true;
                dataScrapeer.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);
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
                string UserScrapListData = string.Empty;
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
                    UserScrapListData = data[4];
                }
                catch { }
                try
                {
                    strChkFollowers = data[5];
                }
                catch { }
                try
                {
                    strChkFollowings = data[6];
                }
                catch { }
                try
                {
                    strchkUseDivide = data[7];
                }
                catch { }
                try
                {
                    strrdBtnDivideEqually = data[8];
                }
                catch { }
                try
                {
                    strrdBtnDivideByGivenNo = data[9];
                }
                catch { }
                try
                {
                    ScrapeNoOfUsers = data[10];
                }
                catch { }
                try
                {
                    FollowMinDelay = data[11];
                }
                catch { }
                try
                {
                    FollowMaxDelay = data[12];
                }
                catch { }
                try
                {
                    NoOfFollowThreads = data[13];
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

                //List<string> listUserIDs = new List<string>();
                if (!string.IsNullOrEmpty(PathUserIDs))
                {
                    listUserIDs = Globussoft.GlobusFileHelper.ReadFiletoStringList(PathUserIDs);
                }
                if (!string.IsNullOrEmpty(userIDToFollow))
                {
                    listUserIDs.Add(userIDToFollow);
                }
                if (Globals.lstScrapedUserIDs.Count > 0 && chkboxScrapedLst.Checked)
                {
                    listUserIDs.Clear();
                    foreach (string data1 in Globals.lstScrapedUserIDs)
                    {
                        listUserIDs.Add(data1);
                    }
                }

                if (IsStart_ScrapUserInFollower)
                {
                    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                    {
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Starting Scraping User ]");

                        threadStartScrapeInFollower();
                        if (listUserIDs.Count == 0)
                        {
                            return;
                        }
                        
                    }
                    else
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts To Start Scraping ]");
                    }
                }
                //bool otherUser = false;
                //if (otherUser == "1")
                //{
                //    otherUser = true;
                //}
                if (!string.IsNullOrEmpty(UserOtherNumber))
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
        public int counter_Account = 0;
        private void btnStartUnfollowing_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Starting Unfollow Process ]");
                    if (!string.IsNullOrEmpty(txtUnfollowMinDelay.Text) && !string.IsNullOrEmpty(txtUnfollowMaxDelay.Text)
                        && NumberHelper.ValidateNumber(txtUnfollowMinDelay.Text) && NumberHelper.ValidateNumber(txtUnfollowMaxDelay.Text))
                    {
                        Thread thread_StartUnFollowing = null;
                        int abortCounter = 0;

                        //check Unfollower Table 
                        //If unfollower table is not available in Db 
                        // Then its check Table and create on there..
                        #region

                        if (!BaseLib.DataBaseHandler.CreateUnfollowerTable())
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Unfollow table is not create in DB. ]");
                            return;
                        }
                        #endregion


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
                        if (string.IsNullOrEmpty(txtUnfollowMinDelay.Text))
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Please Enter Minimum Delay ]");
                            MessageBox.Show("Please Enter Minimum Delay");
                        }
                        else if (string.IsNullOrEmpty(txtUnfollowMaxDelay.Text))
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Please Enter Maximum Delay ]");
                            MessageBox.Show("Please Enter Maximum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtUnfollowMinDelay.Text))
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Minimum Delay ]");
                            MessageBox.Show("Please Enter a Number in Minimum Delay");
                        }
                        else if (!NumberHelper.ValidateNumber(txtUnfollowMaxDelay.Text))
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Please Enter a Number in Maximum Delay ]");
                            MessageBox.Show("Please Enter a Number in Maxiimum Delay");
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartUnfollowing_Click() --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStartUnfollowing_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void StartUnFollowing()
        {
            try
            {
                int numberOfThreads = 7;
                int numberOfUnfollows = 5;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                counter_Account = TweetAccountContainer.dictionary_TweetAccount.Count();

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
                                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
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

                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartUnFollowingMultithreaded), new object[] { item, txtNoOfUnfollows.Text });

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            //ErrorLogger.AddToErrorLogText(ex.Message);
                            if (ex.Message == "Thread was being aborted.")
                            {
                                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartUnFollowing() -- Foreach loop Dictinary--> " + ex.Message, Globals.Path_UnfollowerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartUnFollowing() --Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload Twitter Accounts from Menu");
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts from Menu ]");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
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
                int NoOfFollwos = 0;
                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);
                string NofUnfollows = (string)paramsArray.GetValue(1);
                if (!string.IsNullOrEmpty(NofUnfollows) && NumberHelper.ValidateNumber(NofUnfollows))
                {
                    NoOfFollwos = Int32.Parse(NofUnfollows);
                }

                //string userIDToFollow = (string)paramsArray.GetValue(1);
                List<string> list_userIDsToFollow = new List<string>();

                TweetAccountManager tweetAccountManager = keyValue.Value;
                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Unfollowing Process Started For Account : " + keyValue.Key + " ]");
                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Unfollow), tweetAccountManager.Username);
                tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_UnFollower_addToLogger;
                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }

                if (!tweetAccountManager.IsLoggedIn)
                {
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Not Logged In With Account : " + keyValue.Key + " ]");
                    return;
                }
                //AddToLog_Unfollow("Checking for Persons Not Followed Back");

                //Check Test box and anf useing feature box is checked 
                if (!string.IsNullOrWhiteSpace(txt_FilePathunfollowUserslist.Text))
                {
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Wait process is running. ]");
                    //get user deatils from list 
                    clsDBQueryManager queryManager = new clsDBQueryManager();
                    DataTable dt_AlreadyFollowed = queryManager.SelectFollowData(tweetAccountManager.Username);
                    foreach (string item in frmMain_NewUI.lst_unfolloweruserlist)
                    {
                        try
                        {
                            if (chk_dontCheckDbForUnfollow.Checked == false)
                            {
                                try
                                {
                                    if (GlobusRegex.ValidateNumber(item))
                                    {
                                        continue;
                                    }
                                    string Status = string.Empty;

                                    String UserTimelinePageSource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + item), "", "");

                                    ///Get user ID 
                                    ///
                                    string userID = string.Empty;

                                    //try
                                    //{
                                    //    int startIndx = UserTimelinePageSource.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                                    //    int endIndx = UserTimelinePageSource.IndexOf("\"", startIndx);
                                    //    userID = UserTimelinePageSource.Substring(startIndx, endIndx - startIndx);
                                    //}
                                    //catch { }

                                    try
                                    {
                                        userID = string.Empty;

                                        #region commentedRegionForUserId
                                        //string[] useridarr = System.Text.RegularExpressions.Regex.Split(UserTimelinePageSource, "data-user-id=");
                                        //foreach (string useridarr_item in useridarr)
                                        //{
                                        //    if (useridarr_item.Contains("profile-field"))
                                        //    {
                                        //        userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("<h1") - 3).Replace("\">\n", string.Empty).Trim();
                                        //        list_userIDsToFollow.Add(userID);
                                        //        break;
                                        //    }
                                        //} 
                                        #endregion

                                        try
                                        {
                                            int startindex = UserTimelinePageSource.IndexOf("profile_id");
                                            string start = UserTimelinePageSource.Substring(startindex).Replace("profile_id", "");
                                            int endindex = start.IndexOf(",");
                                            string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Trim();
                                            userID = end.Trim();
                                            list_userIDsToFollow.Add(userID);
                                        }
                                        catch { }
                                    }
                                    catch { }
                                }
                                catch { };
                            }
                            else
                            {
                                //Get user data
                                // if uploaded list value is user name 
                                //get id from user name 
                                if (GlobusRegex.ValidateNumber(item))
                                {
                                    try
                                    {
                                        DataRow[] filteredRows = dt_AlreadyFollowed.Select(string.Format("{0} LIKE '%{1}%'", "following_id", item));
                                        if (filteredRows.Count() > 0)
                                        {
                                            list_userIDsToFollow.Add(filteredRows[0].ItemArray[2].ToString());
                                        }
                                        else
                                        {
                                            AddToLog_UnFollower("[ " + DateTime.Now + " ] => [ " + item + " Does not Followed by " + tweetAccountManager.Username + " ]");
                                        }
                                    }
                                    catch { };
                                }
                                else
                                {
                                    try
                                    {
                                        string outstr = string.Empty;
                                        DataRow[] filteredRows = dt_AlreadyFollowed.Select(string.Format("{0} LIKE '%{1}%'", "following_username", item));
                                        if (filteredRows.Count() > 0)
                                        {
                                            if (!string.IsNullOrEmpty(filteredRows[0].ItemArray[2].ToString()))
                                            {
                                                list_userIDsToFollow.Add(filteredRows[0].ItemArray[2].ToString());
                                            }
                                            else if (filteredRows.Count() > 1 && !string.IsNullOrEmpty(filteredRows[1].ItemArray[2].ToString()))
                                            {
                                                list_userIDsToFollow.Add(filteredRows[1].ItemArray[2].ToString());
                                            }
                                            else
                                            {
                                                AddToLog_UnFollower("[ " + DateTime.Now + " ] => [ " + item + " ID unavaliable.  ]");
                                            }
                                        }
                                        else
                                        {
                                            AddToLog_UnFollower("[ " + DateTime.Now + " ] => [ " + item + " Does not Followed by " + tweetAccountManager.Username + " ]");
                                            //string status = string.Empty;
                                            //var abc = TwitterDataScrapper.GetUsernameToUserID_New(item, out  status, ref tweetAccountManager.globusHttpHelper);
                                            //list_userIDsToFollow.Add(abc.ToString());
                                        }
                                    }
                                    catch { };
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (list_userIDsToFollow.Count == TweetAccountManager.noOfUnfollows)
                        {
                            break;
                        }
                    }
                }
                else if (rdoUnfollowNotFollowing.Checked)
                {
                    if (chkUnfollowDateFilter.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtUnfollowFilterDays.Text) && GlobusRegex.ValidateNumber(txtUnfollowFilterDays.Text))
                        {
                            int days = int.Parse(txtUnfollowFilterDays.Text);
                            list_userIDsToFollow = tweetAccountManager.GetNonFollowingsBeforeSpecifiedDate(days, ref tweetAccountManager);
                        }
                    }
                    else
                    {
                        list_userIDsToFollow = tweetAccountManager.GetNonFollowings();
                    }
                }
                else
                {
                    TwitterDataScrapper DataScraper = new TwitterDataScrapper();
                    string Returnstatus = string.Empty;
                    TwitterDataScrapper.NoOfFollowingsToBeunfollowed = NoOfFollwos;
                    DataScraper.CounterDataNo = NoOfFollwos;
                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Scraping Following For : " + tweetAccountManager.Username + " ]");
                    //DataScraper.logEvents.addToLogger += new EventHandler();
                    DataScraper.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                    list_userIDsToFollow = DataScraper.GetFollowings_New(tweetAccountManager.Screen_name, out Returnstatus, ref tweetAccountManager.globusHttpHelper);

                    if (list_userIDsToFollow.Count != 0)
                    {
                        List<string> temp_userIDsToFollow = new List<string>();

                        list_userIDsToFollow.ForEach(s =>
                        {
                            if (s.Contains(':'))
                            {
                                temp_userIDsToFollow.Add(s.Split(':')[0]);
                            }
                            else
                            {
                                temp_userIDsToFollow.Add(s.ToString());
                            }
                        });

                        if (temp_userIDsToFollow.Count != 0)
                        {
                            list_userIDsToFollow.Clear();
                            list_userIDsToFollow.AddRange(temp_userIDsToFollow);
                        }
                    }

                    AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ " + list_userIDsToFollow.Count + " Following Scraped For : " + tweetAccountManager.Username + " ]");
                    TwitterDataScrapper.NoOfFollowingsToBeunfollowed = 0;
                    DataScraper.logEvents.addToLogger -= new EventHandler(logEvents_UnFollower_addToLogger);
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

            finally
            {
                counter_Account--;
                if (counter_Account == 0)
                {
                    if (btnStartUnfollowing.InvokeRequired)
                    {
                        btnStartUnfollowing.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_Unfollow("---------------------------------------------------------------------------------------------------------------------------");
                            btnStartUnfollowing.Cursor = Cursors.Default;

                        }));
                    }
                }
            }

        }

        private void chkUnfollowerRemoveFromDataBase_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUnfollowerRemoveFromDataBase.Checked)
                {
                    grpRemoveDataFromDataBase.Enabled = true;

                }
                else
                {
                    grpRemoveDataFromDataBase.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        List<string> lstUnfollowerListForRemoveDataBase = new List<string>();
        private void btnUnfollowerRemoveFollowList_Click(object sender, EventArgs e)
        {
            try
            {
              
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtUnfollowerUploadFollowerName.Text = ofd.FileName;
                        lstUnfollowerListForRemoveDataBase = new List<string>();

                        lstUnfollowerListForRemoveDataBase = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        Console.WriteLine(lstUnfollowerListForRemoveDataBase.Count + " Usernames loaded");
                        AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ " + lstUnfollowerListForRemoveDataBase.Count + " Usernames loaded ]");

                    }
                }
            }
            catch { }
        }

        private void btnUnfollowerStartRemovingData_Click(object sender, EventArgs e)
        {
            //StartRemovingFollowerlListFromDatabase();
            new Thread(() =>
            {
                StartRemovingFollowerlListFromDatabase();
            }).Start();
        }

        private void StartRemovingFollowerlListFromDatabase()
        {
            try
            {
                if (string.IsNullOrEmpty(txtUnfollowerUserName.Text.Trim()))
                {
                    MessageBox.Show("Please Enter User Name");
                    return;
                }

                if (string.IsNullOrEmpty(txtUnfollowerUploadFollowerName.Text.Trim()) || lstUnfollowerListForRemoveDataBase.Count == 0)
                {
                    MessageBox.Show("Please Upload Follower Name");
                    return;
                }

                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Process is Running .... ]");
                clsDBQueryManager objclsDBQueryManager = new clsDBQueryManager();
                foreach (string FollowerItem in lstUnfollowerListForRemoveDataBase)
                {
                    objclsDBQueryManager.DeleteFollowDUsernameID_List(txtUnfollowerUserName.Text.Trim(), FollowerItem);
                    objclsDBQueryManager.DeleteFollowDUsernameIDFromTb_User_Followr_Details(FollowerItem);
                }
                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ Process Completed. ]");

            }
            catch { }
        }


        #region Tweet


        private void btnUploadTweetMessage_Click(object sender, EventArgs e)
        {
            Thread _threadData = new Thread(UploadTweetData);
            _threadData.SetApartmentState(ApartmentState.STA);
            _threadData.Start();

        }

        private void UploadTweetData()
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
                        txtTweetMessageFile.Invoke(new MethodInvoker(delegate
                        {
                            txtTweetMessageFile.Text = ofd.FileName;

                        }));

                        listTweetMessages = new List<string>();

                        List<string> listTweetMessages1 = new List<string>();
                        listTweetMessages1 = Globussoft.GlobusFileHelper.ReadTweetFiletoStringList(ofd.FileName);

                        listTweetMessages1.ForEach(tmsg =>
                        {
                            if (useCheckLength.Checked)
                            {
                                if (tmsg.Length <= 140)
                                {
                                    listTweetMessages.Add(tmsg.ToString());
                                }
                            }
                            else
                            {
                                listTweetMessages.Add(tmsg.ToString());
                            }
                        });
                        Globals.Path_tweetMessagePath = ofd.FileName;
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + listTweetMessages.Count + " Tweet Message loaded ]");
                        if (listTweetMessages.Count < listTweetMessages1.Count)
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + (listTweetMessages1.Count - listTweetMessages.Count).ToString() + " Tweet Message are greater than the limits of 140. ]");
                        }
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
            Interlocked.Increment(ref countTweetMessageAccount);
            TweetAccountManager tweetAccountManager = new TweetAccountManager();
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                //string userIDToFollow = (string)paramsArray.GetValue(1);
                //string tweetMessage = (string)paramsArray.GetValue(1);
                List<string> lst_tweetMessage = (List<string>)paramsArray.GetValue(1);

                //TweetAccountManager tweetAccountManager = keyValue.Value;
                tweetAccountManager = keyValue.Value;

                try
                {
                    Thread.CurrentThread.Name = "Tweet_" + tweetAccountManager.Username;
                    Thread.CurrentThread.IsBackground = true;
                    dictionary_Threads.Add("Tweet_" + tweetAccountManager.Username, Thread.CurrentThread);
                }
                catch { }

                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting Tweet For Account :" + keyValue.Key + " ]");

                if (IsTweetScheduled)
                {
                    try
                    {
                        DateTime d1 = dateTimePicker_tweeterStart.Value;
                        DateTime d2 = dateTimePicker_TwetterEnd.Value;
                        TimeSpan t = d2 - DateTime.Now;

                        if (t.TotalSeconds <= 0)
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Time Already Completed Stopping For Account : " + tweetAccountManager.Username + " ]");
                            return;
                        }
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
                            TotalTweets = TweetAccountManager.NoOfTweetPerDay - tweetAccountManager.AlreadyTweeted;
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

                if (chkBoxUseGroup.Checked && !string.IsNullOrEmpty(txtTweetUseGroup.Text))
                {
                    if ((txtTweetUseGroup.Text).ToLower() == (tweetAccountManager.GroupName).ToLower())
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Group Name Matching ]");
                    }
                    else if (txtTweetUseGroup.Text != tweetAccountManager.GroupName)

                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Group Name does not Match. ]");
                        return;
                    }
                }


                if (chkbosUseHashTags.Checked)
                {
                    tweetAccountManager.UseHashTags = true;
                    foreach (string Data in Globals.HashTags)
                    {
                        tweetAccountManager.que_TweetMessages_Hashtags.Enqueue(Data);
                    }
                }
                else
                {
                    tweetAccountManager.UseHashTags = false;
                }

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Tweet), tweetAccountManager.Username);

                ///Adding Logs to Logger
                tweetAccountManager.tweeter.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;

               


                if (!string.IsNullOrEmpty(txtMinDelay_Tweet.Text) && NumberHelper.ValidateNumber(txtMinDelay_Tweet.Text))
                {
                    tweetMinDealy = Convert.ToInt32(txtMinDelay_Tweet.Text);
                }
                if (!string.IsNullOrEmpty(txtMaxDelay_Tweet.Text) && NumberHelper.ValidateNumber(txtMaxDelay_Tweet.Text))
                {
                    tweetMaxDealy = Convert.ToInt32(txtMaxDelay_Tweet.Text);
                }
                //tweetAccountManager.Login();
                //tweetAccountManager.Tweet(tweetMessage);

                if (ChkboxTweetPerday.Checked)
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Checking Tweets Per Day ]");
                    TweetAccountManager.NoOfTweetPerDay = 0;
                    TweetAccountManager.TweetPerDay = true;
                    if (!string.IsNullOrEmpty(txtMaximumTweet.Text) && NumberHelper.ValidateNumber(txtMaximumTweet.Text))
                    {
                        TweetAccountManager.NoOfTweetPerDay = Convert.ToInt32(txtMaximumTweet.Text);
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + TweetAccountManager.NoOfTweetPerDay + " Maximum No Of Tweets Per Day ]");
                    }
                    else
                    {
                        TweetAccountManager.NoOfTweetPerDay = 10;
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Setting Maximum No Of Tweets Per Day as 10 ]");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Key, "Tweet");

                    int TodayTweet = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    tweetAccountManager.AlreadyTweeted = TodayTweet;
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + TodayTweet + " Already tweeted today ]");
                    if (TodayTweet >= TweetAccountManager.NoOfTweetPerDay)
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Already Tweeted " + TweetAccountManager.NoOfTweetPerDay + " ]");
                        return;
                    }
                }


                if (chkAllTweetsPerAccount.Checked)
                {
                    foreach (string item in lst_tweetMessage)
                    {
                        tweetAccountManager.que_TweetMessages_PerAccount.Enqueue(item);
                    }
                }

                tweetAccountManager.Tweet(lst_tweetMessage, tweetMinDealy, tweetMaxDealy);

                //tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                //tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweetingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            finally
            {
                counter_AccountFollwer--;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                Interlocked.Decrement(ref countTweetMessageAccount);

                lock (lockerThreadsTweetFeature)
                {
                    Monitor.Pulse(lockerThreadsTweetFeature);
                }

                if (counter_AccountFollwer == 0)
                {
                    if (btnStartFollowing.InvokeRequired)
                    {
                        Globals.TweetRunningText = string.Empty;
                        GlobusFileHelper.AppendStringToTextfileNewLine("Module Tweet count Total: " + Globals.totalcountTweet, Globals.path_CountNoOfProcessDone);
                        btnStartFollowing.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_Follower("---------------------------------------------------------------------------------------------------------------------------");

                        }));
                    }
                }
            }
        }

        private void chkReplyBySpecificTweet_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkReplyBySpecificTweet.Checked)
                {
                    btnReplyBySpecificTweet.Visible = true;
                    btnStartReTweeting.Enabled = false;
                    btnUploadRetweetKeyword.Enabled = false;
                }
                else
                {
                    btnReplyBySpecificTweet.Visible = false;
                    btnStartReTweeting.Enabled = true;
                    btnUploadRetweetKeyword.Enabled = true;
                }
            }
            catch { }
        }

        List<string> lstReplyBySpecificTweet = new List<string>();
        public static List<TwitterDataScrapper.StructTweetIDs> lst_structTweetIDs { get; set; }
        private void btnReplyBySpecificTweet_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtTweetKeyword.Text = ofd.FileName;
                        lst_structTweetIDs = new List<TwitterDataScrapper.StructTweetIDs>();

                        lstReplyBySpecificTweet = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        TwitterDataScrapper.StructTweetIDs structTweetIDs = new TwitterDataScrapper.StructTweetIDs();
                        foreach (string item in lstReplyBySpecificTweet)
                        {
                            string _item = item + "@@@";

                            string tweetResult = getBetween(_item, "status/", "@@@");
                            string userResult = getBetween(_item, "twitter.com/", "/");
                            structTweetIDs.ID_Tweet = tweetResult;
                            structTweetIDs.username__Tweet_User = userResult;
                            lst_structTweetIDs.Add(structTweetIDs);
                        }
                        
                        Console.WriteLine(lstReplyBySpecificTweet.Count + " Tweets loaded");
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + lstReplyBySpecificTweet.Count + " Tweets loaded ]");
                    }
                }
            }
            catch
            {
            }
        }

        List<string> lstKeywordRetweetUpload = new List<string>();
        private void btnUploadRetweetKeyword_Click(object sender, EventArgs e)
        {
            try
            {

                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtTweetKeyword.Text = ofd.FileName;
                        lstKeywordRetweetUpload = new List<string>();

                        lstKeywordRetweetUpload = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        Console.WriteLine(lstKeywordRetweetUpload.Count + " Items loaded");
                        AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ " + lstKeywordRetweetUpload.Count + " Items loaded ]");

                    }
                }
            }
            catch { }
        }

        #region ForRetweetDivideData
        public static List<List<TwitterDataScrapper.StructTweetIDs>> Split(List<TwitterDataScrapper.StructTweetIDs> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList<TwitterDataScrapper.StructTweetIDs>())
                .ToList();
        }
        List<List<TwitterDataScrapper.StructTweetIDs>> list_lstTargetTweet = new List<List<TwitterDataScrapper.StructTweetIDs>>(); 
        #endregion

        private void btnStartReTweeting_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            if (chkRetweetDivideTweets.Checked)
            {
                TweetAccountManager.IsRetweetDivideRetweet = true;
            }
            else
            {
                TweetAccountManager.IsRetweetDivideRetweet = false;
            }

            List<TwitterDataScrapper.StructTweetIDs> static_lst_Struct_TweetDataTemp = new List<TwitterDataScrapper.StructTweetIDs>();
            list_lstTargetTweet = new List<List<TwitterDataScrapper.StructTweetIDs>>();
            if (CheckNetConn)
            {
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting ReTweeting ]");

                if (chkboxReplyPerDay.Checked)
                {
                    MessageBox.Show("Please Check Retweet Per Day Of No Of Retweet");
                    return;
                }
                RetweetExtractCount = Convert.ToInt32(txtNoOfRetweets.Text);
                TwitterDataScrapper.RetweetExtractcount = RetweetExtractCount;
                if (!string.IsNullOrEmpty(txtTweetKeyword.Text))
                {
                    string tweetKeyword = txtTweetKeyword.Text;
                    if (TweetAccountContainer.dictionary_TweetAccount.Count <= 0)
                    {
                        MessageBox.Show("Please Upload Twitter Accounts to Start ReTweet Feature");
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts to Start ReTweet Feature ]");
                        return;
                    }
                    if (chk_retweetbyUser.Checked)
                    {
                        new Thread(() =>
                        {
                            AddThreadToDictionary(strModule(Module.Retweet), "GettingRetweetsByUsername");
                            try
                            {
                                Thread.CurrentThread.Name = "ReTweet_GettingRetweetsByUsername";
                                Thread.CurrentThread.IsBackground = true;
                                dictionary_Threads.Add("ReTweet_GettingRetweetsByUsername", Thread.CurrentThread);
                            }
                            catch { }
                            //get All tweets from entered user name 
                            //Scrap Tweets using Username
                            TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                            TweetAccountManager.static_lst_Struct_TweetData = new List<TwitterDataScrapper.StructTweetIDs>();
                            //tweetScrapper.logEvents.addToLogger += new EventHandler();
                            tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                            if (chkCheckDatabaseInEvery2Minutes.Checked == true || chkAutoFavorite.Checked == true || chkRetweetDivideTweets.Checked)
                            {
                                TwitterDataScrapper.noOfRecords = (int.Parse(txtNoOfRetweets.Text));
                            }
                            else
                            {
                                TwitterDataScrapper.noOfRecords = (int.Parse(txtNoOfRetweets.Text) * TweetAccountContainer.dictionary_TweetAccount.Count);
                            }
                            //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New(tweetKeyword);
                            //tweetScrapper.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);
                            tweetScrapper.RetweetFromUserName = true;

                            foreach (string _ReplyKeywordTemp in lstKeywordRetweetUpload)
                            {

                                static_lst_Struct_TweetDataTemp = tweetScrapper.TweetExtractor_ByUserName_New_New(_ReplyKeywordTemp);
                                TweetAccountManager.static_lst_Struct_TweetData.AddRange(static_lst_Struct_TweetDataTemp);
                            }

                            //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New_New(tweetKeyword);
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [  We found " + TweetAccountManager.static_lst_Struct_TweetData.Count + " records from keyword. ]");

                            StartReTweeting();
                        }).Start();
                    }
                    else
                    {
                        new Thread(() =>
                        {
                            {
                                
                                //Scrap Tweets using Keyword
                                AddThreadToDictionary(strModule(Module.Retweet), "GettingRetweetsByKeyword");
                                try
                                {
                                    Thread.CurrentThread.Name = "ReTweet_ByKeyword";
                                    Thread.CurrentThread.IsBackground = true;
                                    dictionary_Threads.Add("ReTweet_ByKeyword", Thread.CurrentThread);
                                }
                                catch { }

                                TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                                if (chkBoxRetweetAndFollow.Checked)
                                {
                                    tweetScrapper.chkStatusRetweetAndFollow = true;
                                }
                                if (chkCheckDatabaseInEvery2Minutes.Checked == true || chkAutoFavorite.Checked == true)
                                {
                                    TwitterDataScrapper.noOfRecords = (int.Parse(txtNoOfRetweets.Text));
                                }
                                else
                                {
                                    TwitterDataScrapper.noOfRecords = (int.Parse(txtNoOfRetweets.Text) * TweetAccountContainer.dictionary_TweetAccount.Count);
                                }
                                //tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                TweetAccountManager.static_lst_Struct_TweetData = new List<TwitterDataScrapper.StructTweetIDs>();
                                //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData_New1(tweetKeyword);
                                //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData(tweetKeyword);
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please wait we are fetching the tweet data. ]");
                                //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData(tweetKeyword);


                                foreach (string _ReplyKeywordTemp in lstKeywordRetweetUpload)
                                {

                                    static_lst_Struct_TweetDataTemp = tweetScrapper.NewKeywordStructData1(_ReplyKeywordTemp);
                                    TweetAccountManager.static_lst_Struct_TweetData.AddRange(static_lst_Struct_TweetDataTemp);
                                }
                                //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData1(tweetKeyword);
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ We found " + TweetAccountManager.static_lst_Struct_TweetData.Count + " records from keyword. ]");

                                tweetScrapper.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);

                                
                                StartReTweeting();
                            }
                        }).Start();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter Tweet Search Keyword");

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        void StartReTweeting()
        {
            try
            {
                int numberOfThreads = 7;
                //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
                TweetAccountManager.que_lst_Struct_TweetData.Clear();
                foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                {
                    TweetAccountManager.que_lst_Struct_TweetData.Enqueue(item);
                }

                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    if (!string.IsNullOrEmpty(txtNoOfTweetThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetThreads.Text))
                    {
                        numberOfThreads = int.Parse(txtNoOfTweetThreads.Text);
                    }

                    if (chkCheckDatabaseInEvery2Minutes.Checked == true || chkAutoFavorite.Checked == true)
                    {
                        numberOfThreads = TweetAccountContainer.dictionary_TweetAccount.Count;
                    }

                    if (TweetAccountManager.IsRetweetDivideRetweet)
                    {
                        // lst_Struct_ReplyDataTemp = CamptweetScrapper.GetTweetData_New_ForCampaign(_ReplyKeywordTemp, noOfRecordsForUniqueMessage);

                        int splitNo = TweetAccountManager.static_lst_Struct_TweetData.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
                        if (splitNo == 0)
                        {
                            splitNo = RandomNumberGenerator.GenerateRandom(0, TweetAccountManager.static_lst_Struct_TweetData.Count - 1);
                        }
                        list_lstTargetTweet = Split(TweetAccountManager.static_lst_Struct_TweetData, splitNo);

                    }


                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    int LstCounter = 0;

                    if (!TweetAccountManager.IsRetweetDivideRetweet)
                    {
                        Thread static_lst_Struct_TweetDataMethodNewThread = new Thread(() => static_lst_Struct_TweetDataMethod());
                        static_lst_Struct_TweetDataMethodNewThread.Start();
                        //  static_lst_Struct_TweetDataMethod();
                    }



                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        if (!string.IsNullOrEmpty(txtNoOfRetweets.Text) && Globals.IdCheck.IsMatch(txtNoOfRetweets.Text))
                        {
                            TweetAccountManager tweet = item.Value;
                            //tweet.noOfRetweets = int.Parse(txtNoOfRetweets.Text);
                            tweet.noOfRetweets = int.Parse(txtNoOfRetweets.Text) * lstKeywordRetweetUpload.Count;
                        }
                        string tweetMessage = "";
                        if (chkBoxUseGroup.Checked)
                        {

                            if (!string.IsNullOrEmpty(txtTweetUseGroup.Text))
                            {
                                if (((TweetAccountManager)item.Value).GroupName != txtTweetUseGroup.Text)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Enter a Group name in Use Group text box .");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter a Group name in Use Group text box . ]");
                                break;
                            }
                        }

                        if (!TweetAccountManager.IsRetweetDivideRetweet)
                        {
                            try
                            {

                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadpoolMethod_Retweet), new object[] { item });

                                Thread.Sleep(1000);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReTweeting() -- ThreadPoolMethod_Retweet--> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReTweeting() -- ThreadPoolMethod_Retweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        else
                        {
                            try
                            {
                                TweetAccountManager.static_lst_Struct_TweetData = list_lstTargetTweet[LstCounter];
                                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadpoolMethod_Retweet), new object[] { item, TweetAccountManager.static_lst_Struct_TweetData });

                                Thread.Sleep(1000);
                                LstCounter++;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReTweeting() -- ThreadPoolMethod_Retweet--> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReTweeting() -- ThreadPoolMethod_Retweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload twitter Account and Wall Message");
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account and Wall Message ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReTweeting() -- ThreadPoolMethod_Retweet--> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReTweeting() -- ThreadPoolMethod_Retweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void static_lst_Struct_TweetDataMethod()
        {
            while (true)
            {
                //try
                //{
                //    List<Thread> threadList = ThreadpoolMethod_RetweetThreads;
                //    foreach (Thread tr in threadList)
                //    {
                //        try
                //        {
                //            tr.Suspend();

                //        }
                //        catch { };

                //    }
                //}
                //catch{};

                TweetAccountManager.static_lst_Struct_TweetData.Clear();
                List<string> twtKeyword = GlobusFileHelper.ReadFiletoStringList(txtTweetKeyword.Text);
                TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                List<TwitterDataScrapper.StructTweetIDs> strList;
                foreach (string keyword in twtKeyword)
                {
                    try
                    {
                        strList = tweetScrapper.TweetExtractor_ByUserName_New_New(keyword);
                        TweetAccountManager.static_lst_Struct_TweetData.Add(strList[0]);
                    }
                    catch { };
                }

                //try
                //{
                //    List<Thread> threadList = ThreadpoolMethod_RetweetThreads;
                //    foreach (Thread tr in threadList)
                //    {
                //        try
                //        {
                //            tr.Resume();

                //        }
                //        catch { };
                //    }
                //}
                //catch { };
                AddToLog_Tweet("[ " + DateTime.Now + " ] => No of Retweets Found From All Keyword :  " + TweetAccountManager.static_lst_Struct_TweetData.Count);
                AddToLog_Tweet("[ " + DateTime.Now + " ] => searching for new tweet,process will start again after 2 minute");
                Thread.Sleep(2 * 60 * 1000);



            }

        }



        private void  ThreadpoolMethod_Retweet(object parameters)
        {
            TweetAccountManager tweetAccountManager = new TweetAccountManager();
            TweetAccountManager objTweetAccountManager = new TweetAccountManager();
            try
            {
                Array paramsArray = new object[1];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);
                List<TwitterDataScrapper.StructTweetIDs> lst_DivideTweets = new List<TwitterDataScrapper.StructTweetIDs>();
                if (TweetAccountManager.IsRetweetDivideRetweet)
                {
                     lst_DivideTweets = (List<TwitterDataScrapper.StructTweetIDs>)paramsArray.GetValue(1);
                }
                tweetAccountManager = keyValue.Value;
                try
                 {
                    objTweetAccountManager.TweetAccountManager1(keyValue.Value.Username, keyValue.Value.Password);                  
                }
                catch { };

                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting ReTweet For Account : " + keyValue.Key + " ]");

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Retweet), tweetAccountManager.Username);
                try
                {
                    //Thread.CurrentThread.Name = "ReTweet_" + tweetAccountManager.Username;
                    //Thread.CurrentThread.IsBackground = true;
                    //dictionary_Threads.Add("ReTweet_" + tweetAccountManager.Username, Thread.CurrentThread);
                    AddThreadToDictionary(strModule(Module.Retweet), tweetAccountManager.Username);
                }
                catch { }

                //Create logger Event for lof MSg's previous.
                //tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                //tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;

                objTweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                //tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;
                objTweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;


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
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Setting Maximum No Of ReTweets Per Day as 10 ]");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Key, "ReTweet");

                    int TodayReTweet = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    tweetAccountManager.AlreadyRetweeted = TodayReTweet;

                    if (TodayReTweet >= TweetAccountManager.NoOFRetweetPerDay)
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Already Retweeted " + tweetAccountManager.AlreadyRetweeted + " ]");
                        return;
                    }
                }

                if (chkCheckDatabaseInEvery2Minutes.Checked == true || chkAutoFavorite.Checked == true)
                {
                    while (true)
                    {
                        try
                        {
                            string count_tweet = string.Empty;
                            string count_tweet1 = string.Empty;

                            TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                            TwitterDataScrapper.StructTweetIDs item1 = new TwitterDataScrapper.StructTweetIDs();
                            TweetAccountManager.que_lst_Struct_TweetData.Clear();
                            Queue<TwitterDataScrapper.StructTweetIDs> tempQueue = new Queue<TwitterDataScrapper.StructTweetIDs>();
                            TwitterDataScrapper.noOfRecords = 1;
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching tweets for  " + keyValue.Key + " ]");
                            //count_tweet = tweetScrapper.countNoOfTweet(txtTweetKeyword.Text.Trim());
                            #region sonu commented code
                        //    List<string> twtKeyword = GlobusFileHelper.ReadFiletoStringList(txtTweetKeyword.Text);

                        //startAgain:
                        //    foreach (string keyword in twtKeyword)
                        //    {
                        //        count_tweet = tweetScrapper.countNoOfTweet(keyword);

                        //        //count_tweet = tweetScrapper.countNoOfTweet(GlobusFileHelper.ReadFiletoStringList);
                        //    #endregion



                        //        TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New_New(keyword);

                        //        int count = TweetAccountContainer.dictionary_TweetAccount.Count();
                        //        foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                        //        {
                        //            //for (int i = 1; i <= count * TweetAccountManager.static_lst_Struct_TweetData.Count(); i++)
                        //            {
                        //                TweetAccountManager.que_lst_Struct_TweetData.Enqueue(item);
                        //                tempQueue.Enqueue(item);
                        //            }

                        //        }
                        //        try
                        //        {
                        //            if (TweetAccountManager.que_lst_Struct_TweetData.Count > 0)
                        //            {
                        //                item1 = tempQueue.Dequeue();
                        //            }
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            ErrorLogger.AddToErrorLogText(ex.Message);
                        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                        //            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        //        }

                        //        try
                        //        {
                        //            clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                        //            DataSet Ds = DbQueryManager.SelectMessageDataForRetweet(keyValue.Key, item1.ID_Tweet, "ReTweet");
                        //            int count_NO_RoWs = Ds.Tables[0].Rows.Count;
                        //            if (count_NO_RoWs == 0)
                        //            {
                        //                if (chkCheckDatabaseInEvery2Minutes.Checked)
                        //                {
                        //                    tweetAccountManager.ReTweet("", retweetMinDealy, retweetMaxDealy);
                        //                }
                        //                if (chkAutoFavorite.Checked && tweetAccountManager.IsNotSuspended && tweetAccountManager.IsLoggedIn)
                        //                {
                        //                    string TUri = item1.ID_Tweet.ToString();
                        //                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                        //                    FavoriteOfUrl(new object[] { TUri, keyValue, tweetAccountManager });
                        //                }
                        //            }

                        //            //AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching new tweets will start after 3 minutes from " + tweetAccountManager.Username + " ]");
                        //            //Thread.Sleep(3 * 60 * 1000);
                        //            count_tweet1 = tweetScrapper.countNoOfTweet(keyword);
                        //            if (Convert.ToInt32(count_tweet) == Convert.ToInt32(count_tweet1))
                        //            {
                        //                TwitterDataScrapper.noOfRecords = 1;
                        //            }
                        //            else if (Convert.ToInt32(count_tweet1) > Convert.ToInt32(count_tweet))
                        //            {
                        //                TwitterDataScrapper.noOfRecords = Convert.ToInt32(count_tweet1) - Convert.ToInt32(count_tweet);
                        //            }
                        //            else
                        //            {
                        //                TwitterDataScrapper.noOfRecords = 1;
                        //            }
                        //            TweetAccountManager.static_lst_Struct_TweetData.Clear();
                        //            TweetAccountManager.que_lst_Struct_TweetData.Clear();
                        //            tempQueue.Clear();
                        //            count_tweet = count_tweet1;
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            ErrorLogger.AddToErrorLogText(ex.Message);
                        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                        //            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        //        }
                        //        //goto startAgain;
                        //    }
                        //    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching new tweets will start after 3 minutes from " + tweetAccountManager.Username + " ]");
                        //    Thread.Sleep(3 * 60 * 1000);
                        //    goto startAgain;
                            #endregion


                            #region sonu edited
                            List<string> twtKeyword = GlobusFileHelper.ReadFiletoStringList(txtTweetKeyword.Text);
                        //  TweetAccountManager objTweetAccountManager = new TweetAccountManager();
                        startAgain:
                            try
                            {

                                // foreach (string keyword in twtKeyword)
                                //  {
                                //    count_tweet = tweetScrapper.countNoOfTweet(keyword);

                                //count_tweet = tweetScrapper.countNoOfTweet(GlobusFileHelper.ReadFiletoStringList);
                            #endregion



                                //  TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New_New(keyword);
                                try
                                {
                                    int count = TweetAccountContainer.dictionary_TweetAccount.Count();
                                }
                                catch { };
                                //  TweetAccountManager.que_lst_Struct_TweetData.Clear();

                                // que_lst_Struct_TweetData1
                                List<TwitterDataScrapper.StructTweetIDs> static_lst_Struct_TweetData12 = TweetAccountManager.static_lst_Struct_TweetData;
                                foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData12)
                                {
                                    //for (int i = 1; i <= count * TweetAccountManager.static_lst_Struct_TweetData.Count(); i++)
                                    {
                                        try
                                        {
                                            objTweetAccountManager.que_lst_Struct_TweetData1.Enqueue(item);
                                            TweetAccountManager.que_lst_Struct_TweetData.Enqueue(item);
                                            tempQueue.Enqueue(item);
                                        }
                                        catch { };
                                    }

                                }
                                try
                                {
                                    if (objTweetAccountManager.que_lst_Struct_TweetData1.Count > 0)
                                    {
                                        Queue<TwitterDataScrapper.StructTweetIDs> que_lst_Struct_TweetData12 = new Queue<TwitterDataScrapper.StructTweetIDs>();
                                        que_lst_Struct_TweetData12.Clear();
                                        foreach (TwitterDataScrapper.StructTweetIDs item in objTweetAccountManager.que_lst_Struct_TweetData1)
                                        {
                                            item1 = item;
                                            clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                                            DataSet Ds = DbQueryManager.SelectMessageDataForRetweet(keyValue.Key, item1.ID_Tweet, "ReTweet");
                                            int count_NO_RoWs = Ds.Tables[0].Rows.Count;
                                            if (count_NO_RoWs == 0)
                                            {
                                                que_lst_Struct_TweetData12.Enqueue(item);
                                            }

                                        }
                                        try
                                        {
                                            objTweetAccountManager.que_lst_Struct_TweetData1.Clear();
                                            objTweetAccountManager.que_lst_Struct_TweetData1 = que_lst_Struct_TweetData12;
                                        }
                                        catch { };
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.AddToErrorLogText(ex.Message);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                                }

                                try
                                {
                                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                                    DataSet Ds = DbQueryManager.SelectMessageDataForRetweet(keyValue.Key, item1.ID_Tweet, "ReTweet");
                                    int count_NO_RoWs = Ds.Tables[0].Rows.Count;
                                    if (objTweetAccountManager.que_lst_Struct_TweetData1.Count > 0)
                                    {
                                        //if (chkCheckDatabaseInEvery2Minutes.Checked)
                                        //{
                                        //    try
                                        //    {
                                        //        objTweetAccountManager.ReTweet1(ref objTweetAccountManager, retweetMinDealy, retweetMaxDealy);
                                        //    }
                                        //    catch { };
                                        //}
                                        //if (chkAutoFavorite.Checked && tweetAccountManager.IsNotSuspended && tweetAccountManager.IsLoggedIn)
                                        //{
                                        //    string TUri = item1.ID_Tweet.ToString();
                                        //    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                        //    FavoriteOfUrl(new object[] { TUri, keyValue, tweetAccountManager });
                                        //}


                                        if (chkCheckDatabaseInEvery2Minutes.Checked)
                                        {
                                            try
                                            {
                                                objTweetAccountManager.ReTweet1(ref objTweetAccountManager, retweetMinDealy, retweetMaxDealy);
                                            }
                                            catch { };
                                        }
                                        if (chkAutoFavorite.Checked && tweetAccountManager.IsNotSuspended && tweetAccountManager.IsLoggedIn)
                                        {
                                            string TUri = item1.ID_Tweet.ToString();
                                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                            FavoriteOfUrl(new object[] { TUri, keyValue, tweetAccountManager });
                                        }
                                        else if (chkAutoFavorite.Checked)
                                        {
                                            tweetAccountManager.Login();
                                            if (tweetAccountManager.IsLoggedIn)
                                            {
                                                string TUri = item1.ID_Tweet.ToString();
                                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Added To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                                FavoriteOfUrl(new object[] { TUri, keyValue, tweetAccountManager });
                                            }

                                        }
                                    }

                                    // AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching new tweets will start after 3 minutes from " + tweetAccountManager.Username + " ]");
                                    //Thread.Sleep(2 * 60 * 1000);
                                    //count_tweet1 = tweetScrapper.countNoOfTweet(txtTweetKeyword.Text.Trim());
                                    //  count_tweet1 = tweetScrapper.countNoOfTweet(keyword);
                                    try
                                    {
                                        if (Convert.ToInt32(count_tweet) == Convert.ToInt32(count_tweet1))
                                        {
                                            TwitterDataScrapper.noOfRecords = 1;
                                        }
                                        else if (Convert.ToInt32(count_tweet1) > Convert.ToInt32(count_tweet))
                                        {
                                            TwitterDataScrapper.noOfRecords = Convert.ToInt32(count_tweet1) - Convert.ToInt32(count_tweet);
                                        }
                                        else
                                        {
                                            TwitterDataScrapper.noOfRecords = 1;
                                        }
                                    }
                                    catch { };
                                    // TweetAccountManager.static_lst_Struct_TweetData.Clear();
                                    // TweetAccountManager.que_lst_Struct_TweetData.Clear();
                                    try
                                    {
                                        objTweetAccountManager.que_lst_Struct_TweetData1.Clear();
                                        tempQueue.Clear();
                                        count_tweet = count_tweet1;
                                    }
                                    catch { };
                                }
                                catch (Exception ex)
                                {
                                    ErrorLogger.AddToErrorLogText(ex.Message);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                                }

                                //  }
                                //   AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching new tweets will start after 3 minutes from " + tweetAccountManager.Username + " ]");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [Delay For Account - " + tweetAccountManager.Username + " 2 Minutes ]");
                                Thread.Sleep(2 * 60 * 1000);
                            }
                            catch { };
                            goto startAgain;




                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    
                    }
                }
                else
                {

                    if (TweetAccountManager.IsRetweetDivideRetweet)
                    {
                        tweetAccountManager.ReTweetDivideRetweet(lst_DivideTweets, retweetMinDealy, retweetMaxDealy);
                    }
                    else
                    {
                        tweetAccountManager.ReTweet("", retweetMinDealy, retweetMaxDealy);
                    }
                }

                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadpoolMethod_Retweet() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            finally
            {
                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
        }

        private void btnStartReplying_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            try
            {
                TweetAccountManager.static_lst_Struct_TweetData.Clear();
            }
            catch
            { }

            List<TwitterDataScrapper.StructTweetIDs> static_lst_Struct_TweetDataTemp = new List<TwitterDataScrapper.StructTweetIDs>();
            

            if (CheckNetConn)
            {
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting Replying ]");
                if (chkboxRetweetPerDay.Checked)
                {
                    MessageBox.Show("Please Check Reply Per Day or No Of Reply");
                    return;
                }

                if (chkReplyBySpecificTweet.Checked)
                {
                    foreach (TwitterDataScrapper.StructTweetIDs item in lst_structTweetIDs)
                    {
                        TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Enqueue(item);
                    }

                    if (TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Count > 0 && listTweetMessages.Count >= 1)
                    {
                        StartReplying();
                    }
                    else
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Tweet file or reply message file.  ]");
                        return;
                    }
                }

                else
                {

                    if (listTweetMessages.Count >= 1 && !string.IsNullOrEmpty(txtTweetKeyword.Text.Trim()))
                    //if (!string.IsNullOrEmpty(txtTweetKeyword.Text))
                    {
                        string tweetKeyword = txtTweetKeyword.Text;
                        if (!chk_retweetbyUser.Checked)
                        {
                            new Thread(() =>
                            {

                                try
                                {
                                    AddThreadToDictionary(strModule(Module.Reply), "GettingRetweetsByKeyword");
                                }
                                catch { };

                                TweetAccountManager.ReplyKeyword = txtTweetKeyword.Text;
                                TweetAccountManager.static_lst_Struct_TweetData = new List<TwitterDataScrapper.StructTweetIDs>();
                                TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                                //tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                // TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData_New(txtTweetKeyword.Text);


                                if (File.Exists(txtTweetKeyword.Text.Trim()))
                                {
                                    foreach (string _ReplyKeywordTemp in lstKeywordRetweetUpload)
                                    {

                                        static_lst_Struct_TweetDataTemp = tweetScrapper.NewKeywordStructData1(_ReplyKeywordTemp);
                                        TweetAccountManager.static_lst_Struct_TweetData.AddRange(static_lst_Struct_TweetDataTemp);
                                    }
                                }
                                else
                                {
                                    TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData1(tweetKeyword);
                                }

                                //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData1(tweetKeyword);
                                tweetScrapper.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + TweetAccountManager.static_lst_Struct_TweetData.Count + " Tweets found ]");
                                //AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + TweetAccountManager.static_lst_Struct_TweetData.Count + " Tweets From Keyword : " + txtTweetKeyword.Text + " ]");
                                foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                                {
                                    TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Enqueue(item);
                                }

                                if (TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Count > 0)
                                {
                                    StartReplying();
                                }
                                else
                                {
                                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Data is not available from searching Keyword :- " + txtTweetKeyword.Text + " ]");
                                }
                            }).Start();
                        }

                        else
                        {

                            new Thread(() =>
                            {

                                try
                                {
                                    AddThreadToDictionary(strModule(Module.Reply), "GettingRetweetsByUsername");
                                }
                                catch { };

                                TweetAccountManager.ReplyKeyword = txtTweetKeyword.Text;
                                TweetAccountManager.static_lst_Struct_TweetData = new List<TwitterDataScrapper.StructTweetIDs>();
                                TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                                //tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                tweetScrapper.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                                // TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData_New(txtTweetKeyword.Text);

                                if (File.Exists(txtTweetKeyword.Text.Trim()))
                                {
                                    static_lst_Struct_TweetDataTemp = new List<TwitterDataScrapper.StructTweetIDs>();
                                    foreach (string _ReplyKeywordTemp in lstKeywordRetweetUpload)
                                    {
                                        
                                        static_lst_Struct_TweetDataTemp = tweetScrapper.TweetExtractor_ByUserName_New_New(_ReplyKeywordTemp);
                                        TweetAccountManager.static_lst_Struct_TweetData.AddRange(static_lst_Struct_TweetDataTemp);
                                    }
                                }
                                else
                                {
                                    static_lst_Struct_TweetDataTemp = tweetScrapper.TweetExtractor_ByUserName_New_New(tweetKeyword);
                                }

                                TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New_New(tweetKeyword);
                                tweetScrapper.logEvents.addToLogger -= new EventHandler(logEvents_Tweet_addToLogger);
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + TweetAccountManager.static_lst_Struct_TweetData.Count + " Tweets found ]");
                                foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                                {
                                    TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Enqueue(item);
                                }

                                if (TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Count > 0)
                                {
                                    StartReplying();
                                }
                                else
                                {
                                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Data is not available from searching Keyword :- " + txtTweetKeyword.Text + " ]");
                                }
                            }).Start();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please upload Tweet Messages File & put a Tweet Search Keyword");
                    }
                }
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

                listTweetMessages = listTweetMessages.Distinct().ToList();

                foreach (string item in listTweetMessages)
                {
                    TweetAccountManager.que_ReplyMessages.Enqueue(item);
                }

                ThreadPool.SetMaxThreads(numberOfThreads, 5);
                counter_AccountFollwer = TweetAccountContainer.dictionary_TweetAccount.Count;
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    //string tweetMessage = "";
                    //try
                    //{
                    //    tweetMessage = listTweetMessages[RandomNumberGenerator.GenerateRandom(0, listTweetMessages.Count - 1)];
                    //}
                    //catch(Exception ex)
                    //{
                    //    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartReplying() --> " + ex.Message, Globals.Path_TweetingErroLog);
                    //    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartReplying() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    //}

                    if (!string.IsNullOrEmpty(txtNoOfRetweets.Text) && Globals.IdCheck.IsMatch(txtNoOfRetweets.Text))
                    {
                        item.Value.noOfRetweets = int.Parse(txtNoOfRetweets.Text);
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
                        //    string IPAddress = tweetAccountManager.IPAddress;
                        //    string IPPort = tweetAccountManager.IPPort;
                        //    string IPUsername = tweetAccountManager.IPUsername;
                        //    string IPpassword = tweetAccountManager.IPpassword;

                        //    tweetAccountManager = new TweetAccountManager();
                        //    tweetAccountManager.Username = accountUser;
                        //    tweetAccountManager.Password = accountPass;
                        //    tweetAccountManager.IPAddress = IPAddress;
                        //    tweetAccountManager.IPPort = IPPort;
                        //    tweetAccountManager.IPUsername = IPUsername;
                        //    tweetAccountManager.IPpassword = IPpassword;

                        //    tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_Tweet_addToLogger);
                        //    //tweetAccountManager.logEvents.addToLogger += logEvents_Tweet_addToLogger;
                        //    tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_Tweet_addToLogger;

                        //    tweetAccountManager.Reply(tweetMessage);

                        //    tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                        //})); 
                        #endregion

                        ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_Reply), new object[] { item.Value });

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
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
            }
        }

        private void ThreadPoolMethod_Reply(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                TweetAccountManager keyValue = (TweetAccountManager)paramsArray.GetValue(0);


                TweetAccountManager tweetAccountManager = keyValue;//keyValue.Value;

                //try
                //{
                //    dictionary_Threads.Add(tweetAccountManager.Username, Thread.CurrentThread);
                //}
                //catch { };

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Reply), tweetAccountManager.Username);

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
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Adding Default Maximum No Of Retweet 10 ]");
                    }

                    clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                    DataSet Ds = DbQueryManager.SelectMessageData(keyValue.Username, "Reply");

                    int TodayReply = Ds.Tables["tb_MessageRecord"].Rows.Count;
                    tweetAccountManager.AlreadyReply = TodayReply;
                    if (TodayReply >= TweetAccountManager.NoOFReplyPerDay)
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Already Replied " + TodayReply + " ]");
                        return;
                    }
                }


                if (chkCheckDatabaseInEvery2Minutes.Checked == true)
                {
                    while (true)
                    {

                        TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                        TwitterDataScrapper.StructTweetIDs item1 = new TwitterDataScrapper.StructTweetIDs();
                        TweetAccountManager.que_lst_Struct_TweetData.Clear();
                        Queue<TwitterDataScrapper.StructTweetIDs> tempQueue = new Queue<TwitterDataScrapper.StructTweetIDs>();
                        TwitterDataScrapper.noOfRecords = 1;
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Searching tweets for  " + keyValue.Username + " ]");
                        TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.TweetExtractor_ByUserName_New_New(txtTweetKeyword.Text.Trim());

                        int count = TweetAccountContainer.dictionary_TweetAccount.Count();
                        foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                        {
                            for (int i = 1; i <= count; i++)
                            {
                                TweetAccountManager.que_lst_Struct_TweetData.Enqueue(item);
                                tempQueue.Enqueue(item);
                            }

                        }
                        if (TweetAccountManager.que_lst_Struct_TweetData.Count > 0)
                        {
                            item1 = tempQueue.Dequeue();
                        }
                        clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                        DataSet Ds = DbQueryManager.SelectMessageDataForRetweet(keyValue.Username, item1.ID_Tweet, "Reply");
                        int count_NO_RoWs = Ds.Tables[0].Rows.Count;
                        if (count_NO_RoWs == 0)
                        {
                            tweetAccountManager.Reply(listTweetMessages, replyMinDealy, replyMaxDealy);
                        }

                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Search tweets after 3 minutes ");
                        Thread.Sleep(3 * 60 * 1000);

                    }
                }
                else
                {

                    tweetAccountManager.Reply("", replyMinDealy, replyMaxDealy);
                }

                tweetAccountManager.Reply("", replyMinDealy, replyMaxDealy);
                //tweetAccountManager.getmentions();
                tweetAccountManager.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_Tweet_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_Reply()  --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_Reply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                counter_AccountFollwer--;
                

                if (counter_AccountFollwer == 0)
                {
                    if (btnStartFollowing.InvokeRequired)
                    {
                        btnStartReplying.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_Tweet("---------------------------------------------------------------------------------------------------------------------------");

                        }));
                    }
                }
            }
        }

        List<string> lst_mentionUser = new List<string>();

        private void btnStartTweeting_Click(object sender, EventArgs e)
        {
            Globals.totalcountTweet = 0;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txtTweetMessageFile.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("Tweeter", "TweetMessageFilePath", StringEncoderDecoder.Encode(txtTweetMessageFile.Text));
                }

                if (chkBoxUseGroup.Checked)
                {
                    if (string.IsNullOrEmpty(txtTweetUseGroup.Text))
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter Group Name To be Tweeted On ]");
                        return;
                    }
                }
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting Tweeting ]");
                if (chkAllTweetsPerAccount.Checked)
                {
                    TweetAccountManager.AllTweetsPerAccount = true;
                }
                else
                {
                    TweetAccountManager.AllTweetsPerAccount = false;
                }

                if (!NumberHelper.ValidateNumber(txtMinDelay_Tweet.Text))
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Minimum Delay ]");
                    MessageBox.Show("Please Enter Valid Value in Minimum Delay");
                    txtMinDelay_Tweet.Focus();
                    return;
                }
                else if (!NumberHelper.ValidateNumber(txtMaxDelay_Tweet.Text))
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter Valid Value in Maximum Delay ]");
                    MessageBox.Show("Please Enter Valid Value in Maximum Delay");
                    txtMaxDelay_Tweet.Focus();
                    return;
                }

                if (CheckBoxScrapTweets.Checked)
                {
                    if (ScrapTweetsForTweetModule.Count() > 0)
                    {
                        listTweetMessages.Clear();
                        listTweetMessages = ScrapTweetsForTweetModule;
                        listTweetMessages.Distinct();
                    }
                    else
                    {
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ No scraped Tweet is Found, Please Scrap Tweets First ]");
                        return;
                    }

                }

                if (listTweetMessages.Count >= 1)
                {
                    new Thread(() =>
                    {
                        StartTweeting();
                    }).Start();
                }
                else
                {
                    MessageBox.Show("Please Upload Tweet Messages File");
                }
            }
            else
            {
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
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

                    if (!chkboxKeepSingleMessage.Checked)
                    {
                        listTweetMessages = listTweetMessages.Distinct().ToList();
                    }

                    if (IsMentionUserWithScrapedList)
                    {
                        if (!string.IsNullOrEmpty(txtTweetScrapUserData.Text.Trim()))
                        {
                            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                            {
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting Scraping User ]");


                                threadStartScrapeInTweetMentionUser();
                                
                                if (lst_mentionUser.Count == 0)
                                {
                                    return;
                                }
                                
                            }
                            else
                            {
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts To Start Scraping ]");
                                return;
                            }
                        }
                        else
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter User Name to Scrap User. ]");
                            return;
                        }

                            
                    }

                    try
                    {
                        bool delaystatus = true;
                        if (!string.IsNullOrEmpty(txtMinDelay_Tweet.Text) && NumberHelper.ValidateNumber(txtMinDelay_Tweet.Text))
                        {
                            tweetMinDealy = Convert.ToInt32(txtMinDelay_Tweet.Text);
                            delaystatus = false;
                        }

                        if (!string.IsNullOrEmpty(txtMaxDelay_Tweet.Text) && NumberHelper.ValidateNumber(txtMaxDelay_Tweet.Text))
                        {
                            tweetMaxDealy = Convert.ToInt32(txtMaxDelay_Tweet.Text);
                            delaystatus = false;
                        }

                        if (tweetMinDealy > tweetMaxDealy)
                        {
                            MessageBox.Show("Min value should be less than max delay");
                            delaystatus = false;
                            return;
                        }
                    }
                    catch { };

                    #region mention user
                    if (Ismentionsingleuser && lst_mentionUser.Count != 0)
                    {
                        int countNoofMentionUser = 1;
                        if (NumberHelper.ValidateNumber(txtNumberOfIPThreads.Text))
                        {
                            countNoofMentionUser = Convert.ToInt32(txtTweetMentionNoOfUser.Text.Trim());
                            if (countNoofMentionUser <= 0)
                            {
                                MessageBox.Show("Please enter correct value in Mention NO of User.");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please enter correct value in Mention NO of User.]");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter proper value in Mention NO of User.");
                            AddToLog_Tweet("Please enter proper value in Mention NO of User.");
                            return;
                        }

                        List<string> TempMsgList = new List<string>();
                        TempMsgList.Clear();
                        bool _RepeatusernameForAllMsg = false;
                        try
                        {
                            if (lst_mentionUser.Count < listTweetMessages.Count)
                            {
                                DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    _RepeatusernameForAllMsg = true;
                                }
                                else
                                {
                                }
                            }
                        }
                        catch { };


                        //
                        int UsernameCounter = 0;
                        int MsgListCounter = 0;
                        int CatchCounter = 0;

                        
                        while (true)
                        {
                            string NewMSg = string.Empty;
                            int i = 1;
                            try
                            {
                                while (i <= countNoofMentionUser)
                                {
                                     NewMSg += "@" + lst_mentionUser[UsernameCounter] + " " ;
                                     UsernameCounter++;
                                    i++;
                                }
                                NewMSg = NewMSg  + listTweetMessages[MsgListCounter];
                                TempMsgList.Add(NewMSg);
                                //UsernameCounter++;
                                MsgListCounter++;
                            }
                            catch
                            {
                                if (_RepeatusernameForAllMsg)
                                {
                                    UsernameCounter = 0;
                                }
                                if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                {
                                    break;
                                }
                                CatchCounter++;
                            };

                            if (MsgListCounter >= listTweetMessages.Count)
                            {
                                break;
                            }
                        }

                        // Check if Mention adding is Succesfull.
                        if (TempMsgList.Count > 0)
                        {
                            listTweetMessages.Clear();
                            listTweetMessages.AddRange(TempMsgList);
                        }
                        else
                        {
                            MessageBox.Show("mention Message creation is Failure.");
                        }
                    }


                    if (Ismentionrendomuser && lst_mentionUser.Count != 0)
                    {
                        List<string> TempMsgList = new List<string>();
                        int MsgListCounter = 0;
                        int CatchCounter = 0;
                        while (true)
                        {
                            try
                            {
                                Random rnd = new Random();
                                int Rno = rnd.Next(lst_mentionUser.Count);
                                string randomuser = lst_mentionUser[Rno];
                                string NewMSg = "@" + randomuser + " " + listTweetMessages[MsgListCounter];
                                TempMsgList.Add(NewMSg);
                                MsgListCounter++;
                            }
                            catch
                            {
                                CatchCounter++;
                                if (CatchCounter == 3)
                                {
                                    break;
                                }
                            };

                            if (MsgListCounter >= listTweetMessages.Count)
                            {
                                break;
                            }
                        }

                        // Check if Mention adding is Succesfull.
                        if (TempMsgList.Count > 0)
                        {
                            listTweetMessages.Clear();
                            listTweetMessages.AddRange(TempMsgList);
                        }
                        else
                        {
                            MessageBox.Show("mention Message creation is Failure.");
                        }
                    }

                    else
                    {
                        #region mention user From ScrapedData
                        if (IsMentionUserWithScrapedList && lst_mentionUser.Count != 0)
                        {
                            int countNoofMentionUser = 1;
                            if (NumberHelper.ValidateNumber(txtTweetScrapMentionUser.Text.Trim()))
                            {
                                countNoofMentionUser = Convert.ToInt32(txtTweetScrapMentionUser.Text.Trim());
                                if (countNoofMentionUser <= 0)
                                {
                                    MessageBox.Show("Please enter correct value in Mention NO of User.");
                                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please enter correct value in Mention NO of User.]");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please enter proper value in Mention NO of User.");
                                AddToLog_Tweet("Please enter proper value in Mention NO of User.");
                                return;
                            }

                            List<string> TempMsgList = new List<string>();
                            TempMsgList.Clear();
                            bool _RepeatusernameForAllMsg = false;
                            try
                            {
                                if (lst_mentionUser.Count < listTweetMessages.Count)
                                {
                                    DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        _RepeatusernameForAllMsg = true;
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            catch { };


                            //
                            int UsernameCounter = 0;
                            int MsgListCounter = 0;
                            int CatchCounter = 0;


                            while (true)
                            {
                                string NewMSg = string.Empty;
                                int i = 1;
                                try
                                {
                                    while (i <= countNoofMentionUser)
                                    {
                                        NewMSg += "@" + lst_mentionUser[UsernameCounter] + " ";
                                        UsernameCounter++;
                                        i++;
                                    }
                                    NewMSg = NewMSg + listTweetMessages[MsgListCounter];
                                    TempMsgList.Add(NewMSg);
                                    //UsernameCounter++;
                                    MsgListCounter++;
                                }
                                catch
                                {
                                    if (_RepeatusernameForAllMsg)
                                    {
                                        UsernameCounter = 0;
                                    }
                                    if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                    {
                                        break;
                                    }
                                    CatchCounter++;
                                };

                                if (MsgListCounter >= listTweetMessages.Count)
                                {
                                    break;
                                }
                            }

                            // Check if Mention adding is Succesfull.
                            if (TempMsgList.Count > 0)
                            {
                                listTweetMessages.Clear();
                                listTweetMessages.AddRange(TempMsgList);
                            }
                            else
                            {
                                MessageBox.Show("mention Message creation is Failure.");
                            }
                        }


                        #endregion
                    }
                    #endregion

                     foreach (string item in listTweetMessages)                   
                     {
                        TweetAccountManager.que_TweetMessages.Enqueue(item);
                      }

                    TweetAccountManager.FileTweetPath = txtTweetMessageFile.Text;
                    //ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    ThreadPool.SetMaxThreads(numberOfThreads, numberOfThreads);

                    Globals.TweetRunningText = "TweetModule";
                    counter_AccountFollwer = TweetAccountContainer.dictionary_TweetAccount.Count;
                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        try
                        {
                            string tweetMessage = string.Empty;

                            //ThreadPool.SetMaxThreads(numberOfThreads, 5);
                            if (countTweetMessageAccount >= numberOfThreads)
                            {
                                lock (lockerThreadsTweetFeature)
                                {
                                    Monitor.Wait(lockerThreadsTweetFeature);
                                }
                            }
                            Thread objStartTweet = new Thread(StartTweetingMultithreaded);
                            objStartTweet.IsBackground = true;
                            objStartTweet.Start(new object[] { item, listTweetMessages });
                            //ThreadPool.QueueUserWorkItem(new WaitCallback(StartTweetingMultithreaded), new object[] { item, listTweetMessages });
                            //  StartTweetingMultithreaded(new object[] { item, listTweetMessages });

                            Thread.Sleep(1000);
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweeting() -- Foreach loop Dictinary --> " + ex.Message, Globals.Path_TweetingErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweeting() -- Foreach loop Dictinary --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload Twitter Account");
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweeting() --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartTweeting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }


        #endregion
        int count_AccountWaitAndReply;
        
        private void btnStartWaitnReply_Click(object sender, EventArgs e)
        {
            #region commented code
            //if (!string.IsNullOrEmpty(txtReplyMsgFile.Text))
            //{
            //    objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "ReplyMsgFile", StringEncoderDecoder.Encode(txtReplyMsgFile.Text));
            //}
            //if (!string.IsNullOrEmpty(txtTweetMsgFile.Text))
            //{
            //    objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "TweetMsgFile", StringEncoderDecoder.Encode(txtTweetMsgFile.Text));
            //}
            //if (!string.IsNullOrEmpty(txtKeywordFile.Text))
            //{
            //    objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "KeywordMsgFile", StringEncoderDecoder.Encode(txtKeywordFile.Text));
            //}

            //if (!string.IsNullOrEmpty(txtKeyword.Text) || listKeywords.Count > 0)
            //{
            //    if (listKeywords.Count == 0)
            //    {
            //        listKeywords.Add(txtKeyword.Text);
            //    }
            //    if (!string.IsNullOrEmpty(txtNoOfTweetsperReply.Text) && Globals.IdCheck.IsMatch(txtNoOfTweetsperReply.Text))
            //    {
            //        TweetAccountManager.noOfTweetsPerReply = int.Parse(txtNoOfTweetsperReply.Text);
            //    }
            //    if (!string.IsNullOrEmpty(txtNoOfRepliesPerKeyword.Text) && Globals.IdCheck.IsMatch(txtNoOfRepliesPerKeyword.Text))
            //    {
            //        TweetAccountManager.noOfRepliesPerKeyword = int.Parse(txtNoOfRepliesPerKeyword.Text);
            //    }
            //    if (!string.IsNullOrEmpty(txtInterval_WaitReply.Text) && Globals.IdCheck.IsMatch(txtInterval_WaitReply.Text))
            //    {
            //        TweetAccountManager.WaitAndReplyInterval = int.Parse(txtInterval_WaitReply.Text) * 1000 * 2 * 30;//mins
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please enter a Search Keyword");
            //    return;
            //}

            //if (!(listTweetMessages.Count == 0 || listReplyMessages.Count == 0))
            //{
            //    if (chkUseSpinnedMessages.Checked)
            //    {
            //        //List<string> tempList = new List<string>();
            //        //foreach (string item in listReplyMessages)
            //        //{
            //        //    tempList.Add(item);
            //        //}
            //        //listReplyMessages.Clear();
            //        //foreach (string item in tempList)
            //        //{
            //        //    List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item);
            //        //    listReplyMessages.AddRange(tempSpunList);
            //        //}
            //        ///
            //        ///code on 7th 11 2012 , changed by gargi mishra
            //        ///
            //        //listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages);
            //        ////

            //        listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages, '|');

            //    }

            //    if (chkUseSpinnedMessages.Checked)
            //    {
            //        listTweetMessages = SpinnedListGenerator.GetSpinnedList(listTweetMessages);
            //    }

            //    listTweetMessages = listTweetMessages.Distinct().ToList();
            //    listReplyMessages = listReplyMessages.Distinct().ToList();

            //    TweetAccountManager.listTweetMessages = listTweetMessages;
            //    TweetAccountManager.listReplyMessages = listReplyMessages;
            //    TweetAccountManager.que_TweetMessages_WaitAndReply.Clear();
            //    TweetAccountManager.que_ReplyMessages_WaitAndReply.Clear();

            //    foreach (string item in listTweetMessages)
            //    {
            //        TweetAccountManager.que_TweetMessages_WaitAndReply.Enqueue(item);
            //    }
            //    foreach (string item in listReplyMessages)
            //    {
            //        TweetAccountManager.que_ReplyMessages_WaitAndReply.Enqueue(item);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please upload Tweet Messages and Replies Files");
            //    return;
            //} 
            #endregion
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                new Thread(() =>
                {
                    StartWaitAndReply();
                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void StartWaitAndReply()
        {

            #region New Modified code

            if (!string.IsNullOrEmpty(txtReplyMsgFile.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "ReplyMsgFile", StringEncoderDecoder.Encode(txtReplyMsgFile.Text));
            }
            if (!string.IsNullOrEmpty(txtTweetMsgFile.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "TweetMsgFile", StringEncoderDecoder.Encode(txtTweetMsgFile.Text));
            }
            if (!string.IsNullOrEmpty(txt_TweetImageFilePath.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "KeywordMsgFile", StringEncoderDecoder.Encode(txt_TweetImageFilePath.Text));
            }
            if (!string.IsNullOrEmpty(txtKeywordFile.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "KeywordMsgFile", StringEncoderDecoder.Encode(txtKeywordFile.Text));
            }

            if (chkWaitandReplyKeyword.Checked || chkWaitandReplyKeyWordFile.Checked)
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
                    if (!string.IsNullOrEmpty(txtNoOfRecords.Text) && Globals.IdCheck.IsMatch(txtNoOfRecords.Text))
                    {
                        TwitterDataScrapper.noOfRecords = int.Parse(txtNoOfRecords.Text);
                    }
                    if (!string.IsNullOrEmpty(txtNoOfRepliesPerKeyword.Text) && Globals.IdCheck.IsMatch(txtNoOfRepliesPerKeyword.Text))
                    {
                        TweetAccountManager.noOfRepliesPerKeyword = int.Parse(txtNoOfRepliesPerKeyword.Text);
                    }
                    if (!string.IsNullOrEmpty(txtInterval_WaitReply.Text) && Globals.IdCheck.IsMatch(txtInterval_WaitReply.Text))
                    {
                        TweetAccountManager.WaitAndReplyInterval = int.Parse(txtInterval_WaitReply.Text) * 1000 * 2 * 30;//mins
                    }
                    if (TweetAccountManager.waitAndReplyIsIntervalInsec)
                    {
                        TweetAccountManager.waitAndReplyMinInterval = int.Parse(txtWaitandReplyMinDelay.Text);
                        TweetAccountManager.waitAndReplyMaxInterval = int.Parse(txtWaitandReplyMaxDelay.Text);
                    }
                }
                else if (string.IsNullOrEmpty(txtKeyword.Text))
                {
                    MessageBox.Show("Please enter a Search Keyword");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please check single keyword or multiple keyword option.");
                return;
            }

            if ((listTweetMessagesForReply == null))
            {
                MessageBox.Show("Please upload Tweet Messages Files");
                return;
            }
            if ((listReplyMessages == null))
            {
                MessageBox.Show("Please upload Reply Messages Files");
                return;
            }

            if (!(listTweetMessagesForReply.Count == 0 || listReplyMessages.Count == 0))
            {
                #region MyRegion
                //if (chkUseSpinnedMessages.Checked)
                //{
                //        #region commented code
                //        //List<string> tempList = new List<string>();
                //        //foreach (string item in listReplyMessages)
                //        //{
                //        //    tempList.Add(item);
                //        //}
                //        //listReplyMessages.Clear();
                //        //foreach (string item in tempList)
                //        //{
                //        //    List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item);
                //        //    listReplyMessages.AddRange(tempSpunList);
                //        //}
                //        ///
                //        ///code on 7th 11 2012 , changed by gargi mishra
                //        ///
                //        //listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages); 
                //        #endregion
                //        listReplyMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages, '|');
                //        listTweetMessages = SpinnedListGenerator.GetSpinnedList(listReplyMessages, '|');
                //} 
                #endregion

                if (!chk_WaitandReply_UseDuplicateMessage.Checked)
                {
                    listTweetMessagesForReply = listTweetMessagesForReply.Distinct().ToList();
                    
                }
                listReplyMessages = listReplyMessages.Distinct().ToList();
                    TweetAccountManager.listTweetMessages = listTweetMessagesForReply;
                    TweetAccountManager.listReplyMessages = listReplyMessages;
                    TweetAccountManager.que_TweetMessages_WaitAndReply.Clear();
                    TweetAccountManager.que_ReplyMessages_WaitAndReply.Clear();

                    foreach (string item in listTweetMessagesForReply)
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


            #endregion

            int numberOfThreads = 7;
            if (!string.IsNullOrEmpty(txtWaitReplyThreads.Text) && Globals.IdCheck.IsMatch(txtWaitReplyThreads.Text))
            {
                numberOfThreads = int.Parse(txtWaitReplyThreads.Text);
            }


            int count_Keyword = 0;
            count_AccountWaitAndReply = TweetAccountContainer.dictionary_TweetAccount.Count;
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    if (count_Keyword >= listKeywords.Count)
                    {
                        count_Keyword = 0;
                    }
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_WaitAndReply), new object[] {item,count_Keyword});
                    count_Keyword++;
                }
            }
            else
            {
                MessageBox.Show("Please Upload Twitter Accounts");
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts ]");
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

                if (chkWaitandReplyUseGroup.Checked && !string.IsNullOrEmpty(txtWaitandReplyUseGroup.Text))
                {
                    if ((txtWaitandReplyUseGroup.Text).ToLower() == (tweetAccountManager.GroupName).ToLower())
                    {
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Group Name Matching ]");
                    }
                    else if (txtWaitandReplyUseGroup.Text != tweetAccountManager.GroupName)
                    {
                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Group Name does not Match. ]");
                        return;
                    }
                }

                tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_WaitReply_addToLogger);
                tweetAccountManager.tweeter.logEvents.addToLogger += logEvents_WaitReply_addToLogger;

                //tweetAccountManager.StartWaitAndReply(txtKeyword.Text);
                tweetAccountManager.StartWaitAndReply(Keyword);

                tweetAccountManager.logEvents.addToLogger -= logEvents_WaitReply_addToLogger;
                tweetAccountManager.tweeter.logEvents.addToLogger -= logEvents_WaitReply_addToLogger;
            }
            catch (Exception ex)
            {
                //ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_WaitAndReply() --  --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_WaitAndReply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                count_AccountWaitAndReply--;
                if (count_AccountWaitAndReply == 0)
                {
                    if (btnStartWaitnReply.InvokeRequired)
                    {
                        btnStartWaitnReply.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_WaitReply("-------------------------------------------------------------------------------------------------------------------------------");
                            btnStartWaitnReply.Cursor = Cursors.Default;
                        }));
                    }
                }
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
        private void chkWaitandReplyUseGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWaitandReplyUseGroup.Checked)
            {
                txtWaitandReplyUseGroup.Enabled = true;
            }
            else if (!chkWaitandReplyUseGroup.Checked)
            {
                txtWaitandReplyUseGroup.Enabled = false;
            }

        }

        private void chkWaitandReplyKeyWordFile_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkWaitandReplyKeyWordFile.Checked)
                {
                    chkWaitandReplyKeyword.Checked = false;
                    txtKeywordFile.Enabled = true;
                    btnKeywordFile.Enabled = true;
                }
                else
                {
                    txtKeywordFile.Enabled = false;
                    btnKeywordFile.Enabled = false;
                }

            }
            catch { }
        }

        private void chkWaitandReplyKeyword_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkWaitandReplyKeyword.Checked)
                {
                    chkWaitandReplyKeyWordFile.Checked = false;
                    txtKeyword.Enabled = true;

                }
                else
                {
                    txtKeyword.Enabled = false;
                }

            }
            catch { }
        }

        private void chkWaitandReplyUseDelay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkWaitandReplyUseDelay.Checked)
            {
                TweetAccountManager.waitAndReplyIsIntervalInsec = true;
                txtInterval_WaitReply.Enabled = false;
                txtWaitandReplyMinDelay.Enabled = true;
                txtWaitandReplyMaxDelay.Enabled = true;
            }
            else
            {
                TweetAccountManager.waitAndReplyIsIntervalInsec = false;
                txtInterval_WaitReply.Enabled = true;
                txtWaitandReplyMinDelay.Enabled = false;
                txtWaitandReplyMaxDelay.Enabled =  false;
            }
        } 

       

        private void btnUploadUsernames_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtUsernamesFilePath.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("AccountChecker", "UsernamesFilePath", StringEncoderDecoder.Encode(txtUsernamesFilePath.Text));
                }
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
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + AccountChecker.AccountChecker.lst_Usernames.Count + " Usernames loaded ]");

                    }
                }
            }
            catch { }
        }

        private void btn_StartAccountCheckerByUserName_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerByUserName.Enabled = true;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (AccountChecker.AccountChecker.lst_Usernames.Count != 0 && !string.IsNullOrEmpty(txtUsernamesFilePath.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(txt_NoOfThread.Text) && !NumberHelper.ValidateNumber(txt_NoOfThread.Text))
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Enter number of Thread. ]");
                        return;
                    }
                    new Thread(() =>
                    {
                        CheckAccounts();
                    }).Start();
                }
                else
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Upload User name File. ]");

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public readonly object lockerCheckAccount = new object();
        public int countCheckAccount = 0;
        int counterCheckAccount = 0;
        private void CheckAccounts()
        {
            try
            {
                accountCheckerThreadStart = true;
                Lst_accountCheckerThread.Add(Thread.CurrentThread);
                Lst_accountCheckerThread = Lst_accountCheckerThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception)
            {
            }

            int NoOfThreads = int.Parse(txt_NoOfThread.Text);

            List<List<string>> Temp_UserNameList = new List<List<string>>();
            Temp_UserNameList = Split(AccountChecker.AccountChecker.lst_Usernames, NoOfThreads);
            counterCheckAccount = AccountChecker.AccountChecker.lst_Usernames.Count();
            //ThreadPool.SetMaxThreads(NoOfThreads, NoOfThreads);
            //foreach (string item in AccountChecker.AccountChecker.lst_Usernames)

            foreach (List<string> lst_UserNameList in Temp_UserNameList)
            {
                foreach (string item in lst_UserNameList)
                {
                    if (countCheckAccount >= NoOfThreads)
                    {
                        lock (lockerCheckAccount)
                        {
                            Monitor.Wait(lockerCheckAccount);
                        }

                    }
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(StartUsernamechecker), new object[] { lst_UserNameList });

                    Thread ThreadCheckAccount = new Thread(StartUsernamechecker);
                    ThreadCheckAccount.Start(new object[] { item });
                }
            }
        }

        public void StartUsernamechecker(object parameters)
        {

            try
            {
                accountCheckerThreadStart = true;
                Lst_accountCheckerThread.Add(Thread.CurrentThread);
                Lst_accountCheckerThread = Lst_accountCheckerThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception)
            {
            }

            try
            {
                Interlocked.Increment(ref countCheckAccount);
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                //List<string> lstItem = (List<string>)paramsArray.GetValue(0);
                string item = (string)paramsArray.GetValue(0);
                //foreach (string item in lstItem)
                {
                    try
                    {
                        //Check all special carector which is not supported in twiter 
                        if (!item.Contains(":"))
                        {
                            //Regex objNotNaturalPattern = new Regex(@"[^\w\*]");
                            Regex objNotNaturalPattern = new Regex(@"/@(\w+)/");
                            if (objNotNaturalPattern.IsMatch(item))
                            {
                                AddToLog_Checker("[ " + DateTime.Now + " ] => [ User name is not Correct : " + item + " ]");
                                //continue;
                                return;
                            }
                        }

                        string stats;

                        AccountChecker.AccountChecker accountChecker = new AccountChecker.AccountChecker();

                        string username = item.Split(':')[0];

                        if (accountChecker.IsUserAlive(username, out stats))
                        {
                            AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + username + " Exists ]");
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_ExistingIDs);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(stats))
                            {
                                if (stats == "doesn’t exist" || stats == "404" || stats == "403" || stats == "Not exsist")
                                {
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + item + " does Not Exist ]");
                                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_NonExistingIDs);
                                }
                                if (stats == "suspended")
                                {
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + item + " Suspended ]");
                                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.path_SuspendedIDs);
                                }
                                if (stats == "Rate limit exceeded")
                                {
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + item + " Rate limit exceeded ]");
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded ]");
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Try After Some Time ]");
                                }
                            }
                            else
                            {
                                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Couldn't check : " + item + " ]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckAccounts() --> " + ex.Message, Globals.Path_AccountCheckerErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckAccounts() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                lock (lockerCheckAccount)
                {
                    Monitor.Pulse(lockerCheckAccount);
                }

                Interlocked.Decrement(ref countCheckAccount);

                counterCheckAccount--;
                if (counterCheckAccount == 0)
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                }
            }
        }



        /// <summary>
        /// Check Accounts by Email Id .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        List<string> Lst_EmailsForAccountChecking = new List<string>();
        private void btn_EmailAccountCheck_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_AccountCheckerEmailFile.Text))
            {
                //objclsSettingDB.InsertOrUpdateSetting("AccountChecker", "ACEmailFilePath", StringEncoderDecoder.Encode(txt_AccountCheckerEmailFile.Text));
            }
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                int IncorrectEmail = 0;
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    txt_AccountCheckerEmailFile.Text = ofd.FileName;
                    Lst_EmailsForAccountChecking = new List<string>();
                    new Thread(() =>
                    {
                        //txtNames.Invoke(new MethodInvoker(delegate
                        //{
                        //    //txt_EmailListFilePath.Text = ofd.FileName;
                        //}));

                        List<string> _tempList = new List<string>();
                        Lst_EmailsForAccountChecking.Clear();
                        _tempList.AddRange(Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName));

                        foreach (var _tempList_item in _tempList)
                        {
                            if (_tempList_item.Contains(":"))
                            {

                                string CheckEmail = _tempList_item;
                                CheckEmail = CheckEmail.Split(':')[0];
                                CheckEmail = CheckEmail.Replace(":", "").Replace(".", "");
                                bool ISEmail = NumberHelper.ValidateNumber(CheckEmail);

                                if (!ISEmail)
                                {
                                    Lst_EmailsForAccountChecking.Add(_tempList_item);
                                }
                                else
                                {
                                    IncorrectEmail = IncorrectEmail + 1;
                                }
                            }
                        }
                        //Lst_EmailsForAccountChecking = Globussoft.GlobusFileHelper.ReadLargeFile(ofd.FileName);

                        Console.WriteLine(Lst_EmailsForAccountChecking.Count + " Emails loaded");
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + Lst_EmailsForAccountChecking.Count + " Correct Emails loaded ]");
                        if (IncorrectEmail > 0)
                        {
                            AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + IncorrectEmail + " Incorrect Emails Uploaded ]");
                        }
                    }).Start();

                }
            }
        }

        public bool IsEmailProcessStart = true;
        private void btn_StartAccountCheckerByEmail_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerByEmail.Enabled = true;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (Lst_EmailsForAccountChecking.Count != 0 && !string.IsNullOrEmpty(txt_AccountCheckerEmailFile.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(txt_NoOfThread.Text) && !NumberHelper.ValidateNumber(txt_NoOfThread.Text))
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Enter number of Thread. ]");
                        return;
                    }

                    new Thread(() =>
                    {
                        if (IsEmailProcessStart)
                        {
                            IsEmailProcessStart = false;
                            ThreadCount = 0;
                            CheckAccountsByEmail();
                        }
                        else
                        {
                            MessageBox.Show("Please wait process is already running.");
                        }
                    }).Start();
                }
                else
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please upload Email File. ]");

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        int ThreadCount = 0;

        static SemaphoreSlim _TheadSem;
        public void CheckAccountsByEmail()
        {
            try
            {
                accountCheckerThreadStartByEmail = true;
                Lst_accountCheckerThreadByEmail.Add(Thread.CurrentThread);
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception)
            {
            }

            AddToLog_Checker("[ " + DateTime.Now + " ] => [ Start process... ]");

            int NoOfThreads = int.Parse(txt_NoOfThread.Text);

            List<List<string>> Temp_Lst_EmailsForAccountChecking = new List<List<string>>();
            Temp_Lst_EmailsForAccountChecking = Split(Lst_EmailsForAccountChecking, NoOfThreads);

            _TheadSem = new SemaphoreSlim(NoOfThreads, NoOfThreads);
            foreach (List<string> lst_TempEmailList in Temp_Lst_EmailsForAccountChecking)
            {
                foreach (string _item in lst_TempEmailList)
                {
                    _TheadSem.Wait();
                    new Thread(() =>
                    {
                        startAccountCheckingByEmail(_item, NoOfThreads);

                    }).Start();
                    Thread.Sleep(1500);
                }

                Thread.Sleep(3000);
            }

        }

        public void startAccountCheckingByEmail(string Lst_EmailsForAccountChecking_item, int NoOfThreads)
        {
            try
            {
                accountCheckerThreadStartByEmail = true;
                Lst_accountCheckerThreadByEmail.Add(Thread.CurrentThread);
                Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception)
            {
            }

            string email = string.Empty;
            string EmailPass = string.Empty;
            string IPAddress = string.Empty;
            string port = string.Empty;
            string IPUsername = string.Empty;
            string IPpass = string.Empty;

            try
            {
                //Check all special carector which is not supported in twiter 
                if (!Lst_EmailsForAccountChecking_item.Contains(":"))
                {
                    Regex objNotNaturalPattern = new Regex(@"[^\w\*]");
                    if (objNotNaturalPattern.IsMatch(Lst_EmailsForAccountChecking_item))
                    {
                        ThreadCount++;
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ User name is not Correct : " + Lst_EmailsForAccountChecking_item + " ]");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(Lst_EmailsForAccountChecking_item))
                {
                    string[] EmailArr = Lst_EmailsForAccountChecking_item.Split(':');

                    if (EmailArr.Count() == 2)
                    {
                        email = EmailArr[0];
                        EmailPass = EmailArr[1];
                    }
                    else if (EmailArr.Count() == 4)
                    {
                        email = EmailArr[0];
                        EmailPass = EmailArr[1];
                        IPAddress = EmailArr[2];
                        port = EmailArr[3];
                    }
                    else if (EmailArr.Count() == 6)
                    {
                        email = EmailArr[0];
                        EmailPass = EmailArr[1];
                        IPAddress = EmailArr[2];
                        port = EmailArr[3];
                        IPUsername = EmailArr[4];
                        IPpass = EmailArr[5];
                    }
                    else
                    {
                        ThreadCount++;
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Uploaded Email Not in Correct Format ]");
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(EmailPass))
                {

                    TweetAccountManager AccountManager = new TweetAccountManager();

                    AccountManager.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);
                    // AccountManager.logEvents.addToLogger += logEvents_addToLogger;


                    AccountManager.Username = email;
                    AccountManager.Password = EmailPass;
                    AccountManager.IPAddress = IPAddress;
                    AccountManager.IPPort = port;
                    AccountManager.IPUsername = IPUsername;
                    AccountManager.IPpassword = IPpass;

                    AccountManager.Login();

                    //ThreadCount++;

                    if (AccountManager.IsLoggedIn && AccountManager.IsNotSuspended && !AccountManager.Isnonemailverifiedaccounts)
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + AccountManager.Username + " is Active ]");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(AccountManager.Username + ":" + EmailPass + ":" + IPAddress + ":" + port + ":" + IPUsername + ":" + IPpass, Globals.path_ActiveEmailAccounts);
                    }
                    else if (!AccountManager.IsLoggedIn && AccountManager.globusHttpHelper.gResponse.ResponseUri.AbsoluteUri.Contains("https://twitter.com/login/captcha"))
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + AccountManager.Username + " Asking for captcha ]");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(AccountManager.Username + ":" + EmailPass + ":" + IPAddress + ":" + port + ":" + IPUsername + ":" + IPpass, Globals.path_EmailisNotCurrectOrIncorrectPass);
                    }
                    //Not necesary its allready done on followercount() method in account login
                    //else if (!AccountManager.IsNotSuspended)
                    //{
                    //    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + AccountManager.Username + " is suspended ]");
                    //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(AccountManager.Username + ":" + EmailPass + ":" + IPAddress + ":" + port + ":" + IPUsername + ":" + IPpass, Globals.path_SuspendedEmailAccounts);
                    //}
                    else if (AccountManager.Isnonemailverifiedaccounts)
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account : " + AccountManager.Username + " is Required Email Verification ]");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(AccountManager.Username + ":" + EmailPass + ":" + IPAddress + ":" + port + ":" + IPUsername + ":" + IPpass, Globals.path_RequiredEmailVerificationAccounts);
                    }
                    else
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Email is not exist/incorrect pass : " + AccountManager.Username + " ]");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(AccountManager.Username + ":" + EmailPass + ":" + IPAddress + ":" + port + ":" + IPUsername + ":" + IPpass, Globals.path_EmailisNotCurrectOrIncorrectPass);
                    }

                    AccountManager.logEvents.addToLogger -= new EventHandler(logEvents_addToLogger);
                    AccountManager.logEvents.addToLogger -= logEvents_addToLogger;

                }
            }
            catch (Exception ex)
            {
                //ThreadCount++;
                //AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckAccountsByEmail() --> " + ex.Message, Globals.Path_AccountCheckerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckAccounts() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                ThreadCount++;
                if (Lst_EmailsForAccountChecking.Count == ThreadCount)
                {
                    IsEmailProcessStart = true;
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED for Account checker by Email. ]");
                    AddToLog_Checker("------------------------------------------------------------------------------------------------------------------------------------------");
                }
                _TheadSem.Release();
            }
        }

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_Checker(eArgs.log);
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

        void logEvents_Unfollow_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_Unfollow(eArgs.log);
            }
        }

        private void AddToLog_Unfollow(string log)
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



        private void btnUsernames_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                 //   txtUsernames.Text = ofd.FileName;
                    listUserNames = new List<string>();
                    listUserNames.Clear();

                    listUserNames = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    Console.WriteLine(listUserNames.Count + " Usernames loaded");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ " + listUserNames.Count + " Usernames loaded ]");


                }
            }
        }

   

        private void ThreadMethod_CheckEmails(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;

                string Email = string.Empty;//"allliesams@gmail.com";

                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                string emailData = (string)paramsArray.GetValue(0);
                string IP = (string)paramsArray.GetValue(1);

                Email = emailData.Split(':')[0];

                if (!string.IsNullOrEmpty(IP))
                {
                    try
                    {
                        string[] IPData = IP.Split(':');
                        if (IPData.Count() == 2)
                        {
                            IPAddress = IPData[0];
                            IPPort = IPData[1];
                        }
                        if (IPData.Count() == 4)
                        {
                            IPAddress = IPData[0];
                            IPPort = IPData[1];
                            IPUsername = IPData[2];
                            IPpassword = IPData[3];
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                GlobusHttpHelper gHttpHelpr = new GlobusHttpHelper();

                string res_MainPage = gHttpHelpr.getHtmlfromUrlIP(new Uri("https://twitter.com/account/resend_password"), "", IPAddress, IPPort, IPUsername, IPpassword);

                string postAuthenticityToken = TweetAccountManager.PostAuthenticityToken(res_MainPage, "postAuthenticityToken");

                string postData = "authenticity_token=" + postAuthenticityToken + "&email_or_phone=" + Uri.EscapeDataString(Email) + "&screen_name=";

                string postResponse = gHttpHelpr.postFormData(new Uri("https://twitter.com/account/resend_password"), postData, "", "", "", "", "");

                string responseURL = gHttpHelpr.gResponse.ResponseUri.AbsoluteUri;

                responseURL = responseURL.ToLower();

                //if (responseURL.Contains(DateTime.Now + " twitter.com/account/password_reset_sent ]"))
                if (responseURL.Contains("twitter.com/account/password_reset_sent"))
                {
                    //has account
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email, Globals.path_DesktopFolder + "\\ExistingEmail_EmailChecker.txt");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ Existing Email : " + Email + " ]");
                }
                else if (responseURL.Contains("captcha") || responseURL.Contains("security"))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email, Globals.path_DesktopFolder + "\\AskingSecurityEmail_EmailChecker.txt");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ Asking Security with Email : " + Email + " ]");
                }
                else if (responseURL.Contains("twitter.com/account/resend_password") && postResponse.Contains("robots"))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email, Globals.path_DesktopFolder + "\\TemporarilyLocked_EmailChecker.txt");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ We've temporarily locked your ability to reset passwords. Please chillax for a few, then try again. : " + Email + " ]");
                }
                else if (postResponse.Contains("प्रतीत होता है की आपने एक अबैध ईमेल पता या फ़ोन नंबर भरा है. कृपया पुनः प्रयास करें.") || postResponse.Contains("It looks like you entered an invalid email address or phone number. Please try again."))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email, Globals.path_DesktopFolder + "\\TemporarilyLocked_EmailChecker.txt");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ invalid email address: " + Email + " ]");
                }
                else
                {
                    //no account
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email, Globals.path_DesktopFolder + "\\NonExistingEmail_EmailChecker.txt");
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ NON Existing Email : " + Email + " ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

 

        public static bool NoOfdataQueueControler = true;
        TwitterSignup.TwitterSignUp twitterSignUpforManualCaptcha = new TwitterSignup.TwitterSignUp();
      

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
                    AddToListProfile("[ " + DateTime.Now + " ] => [ " + lstProfilePics.Count + " Profile Images Loaded ]");

                }
                if (Directory.Exists(ProfileFolderSettings))
                {
                    txtAccountsFolder.Text = ProfileFolderSettings;
                    LoadProfileData(ProfileFolderSettings);
                }

                #region commented code
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
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }



        private void LoadEmails(string emailFile)
        {
            #region commented code
            //List<string> tempList = GlobusFileHelper.ReadLargeFile(emailFile);
            //foreach (string item in tempList)
            //{
            //    string newitem = item.Replace("\0", "").Replace(" ", "");
            //    if (!string.IsNullOrEmpty(newitem))
            //    {
            //        if (!listEmails.Contains(newitem))
            //        {
            //            listEmails.Add(newitem);
            //        }
            //    }
            //} 
            #endregion

            listEmails = GlobusFileHelper.ReadFiletoStringList(emailFile);
            if (Globals.IsFreeVersion)
            {
                try
                {
                    listEmails.RemoveRange(5, listEmails.Count - 5);
                    FrmFreeTrial frm_freetrail = new FrmFreeTrial();
                    frm_freetrail.TopMost = true;
                    frm_freetrail.BringToFront();
                    frm_freetrail.Show();
                }
                catch (Exception)
                {
                }
            }
        }

        private void AddToListAccountsLogs(string log)
        {
            try
            {
                //if (lstBoxAccountsLogs.InvokeRequired)
                //{
                //    lstBoxAccountsLogs.Invoke(new MethodInvoker(delegate
                //    {
                //        lstBoxAccountsLogs.Items.Add(log);
                //        lstBoxAccountsLogs.SelectedIndex = lstBoxAccountsLogs.Items.Count - 1;
                //    }));
                //}
                //else
                //{
                //    lstBoxAccountsLogs.Items.Add(log);
                //    lstBoxAccountsLogs.SelectedIndex = lstBoxAccountsLogs.Items.Count - 1;
                //}
            }
            catch { }
        }

        private void AddToIPsLogs(string log)
        {
            try
            {
                if (lstLoggerIP.InvokeRequired)
                {
                    lstLoggerIP.Invoke(new MethodInvoker(delegate
                    {
                        lstLoggerIP.Items.Add(log);
                        lstLoggerIP.SelectedIndex = lstLoggerIP.Items.Count - 1;
                    }));
                }
                else
                {
                    lstLoggerIP.Items.Add(log);
                    lstLoggerIP.SelectedIndex = lstLoggerIP.Items.Count - 1;
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

        private void AddToIPAccountCreationLog(string log)
        {
            try
            {
                //if (lstLoggerBrowserAccountCreation.InvokeRequired)
                //{
                //    lstLoggerBrowserAccountCreation.Invoke(new MethodInvoker(delegate
                //    {
                //        lstLoggerBrowserAccountCreation.Items.Add(log);
                //        lstLoggerBrowserAccountCreation.SelectedIndex = lstLoggerBrowserAccountCreation.Items.Count - 1;
                //    }));
                //}
                //else
                //{
                //    lstLoggerBrowserAccountCreation.Items.Add(log);
                //    lstLoggerBrowserAccountCreation.SelectedIndex = lstLoggerBrowserAccountCreation.Items.Count - 1;
                //}
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

        private void ButonTest_Click(object sender, EventArgs e)
        {
            try
            {
                UserControl abc = new UserControl();
                abc.Show();
                GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                string res = httpHelper.getHtmlfromUrl(new Uri("http://www.wotif.com/hotel/View?hotel=W1711"), "", "");
            }
            catch { }
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
                try
                {
                    foreach (string item in files)
                    {
                        if (item.Contains("ProfileLocations.txt"))
                        {
                            //lst_ProfileLocations = GlobusFileHelper.ReadFiletoStringList(item);//ReadLargeFile
                            lst_ProfileLocations = GlobusFileHelper.ReadLargeFile(item);
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_ProfileLocations.Count + " Locations Loaded ]");
                        }

                        if (item.Contains("ProfileUsernames.txt"))
                        {
                            //lst_ProfileUsernames = GlobusFileHelper.ReadFiletoStringList(item);
                            lst_ProfileUsernames = GlobusFileHelper.ReadLargeFile(item);
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_ProfileUsernames.Count + " User Names Loaded ]");
                        }

                        if (item.Contains("ProfileURLs.txt"))
                        {
                            //lst_ProfileURLs = GlobusFileHelper.ReadFiletoStringList(item);
                            lst_ProfileURLs = GlobusFileHelper.ReadLargeFile(item);
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_ProfileURLs.Count + " ProfileURLs  Loaded ]");
                        }

                        if (item.Contains("ProfileDescriptions.txt"))
                        {
                            //lst_ProfileDescriptions = GlobusFileHelper.ReadFiletoStringList(item);
                            lst_ProfileDescriptions = GlobusFileHelper.ReadLargeFile(item);
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded ]");

                        }
                    }
                }
                catch { }

                if (lst_ProfileLocations.Count <= 0)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ No Profile Locations Uploaded From Text File ]");
                }
                if (lst_ProfileUsernames.Count <= 0)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ No UserNames Uploaded From Text File ]");
                }
                if (lst_ProfileURLs.Count <= 0)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ No Profile Url's Uploaded From Text File ]");
                }
                if (lst_ProfileDescriptions.Count <= 0)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ No Profile Descriptions Uploaded From Text File ]");
                }
            }
            else
            {
                AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Text Files. Folder is Empty ]");
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
            try
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

                        }
                        if (lstProfilePics.Count == 0)
                        {
                            AddToListProfile("[ " + DateTime.Now + " ] => [ Image File is not in correct Extention (Jpg, gif, png) ]");
                            MessageBox.Show("Image File is not in correct Extention (Jpg, gif, png)");
                        }

                        Console.WriteLine(lstProfilePics.Count + " Proflile Pics loaded");
                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + lstProfilePics.Count + " Profile Images loaded ]");
                    }
                }
            }
            catch { }
        }

        private void btnStartProfileCreation_Click(object sender, EventArgs e)
        {

            #region commented code
            //if (!string.IsNullOrEmpty(txtProfilePicsFolder.Text) || !string.IsNullOrEmpty(txtAccountsFolder.Text))
            //{
            //    objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "PicsFolder", StringEncoderDecoder.Encode(txtProfilePicsFolder.Text));
            //    objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "ProfileFolder", StringEncoderDecoder.Encode(txtAccountsFolder.Text));
            //    //if ((lst_ProfileLocations.Count > 0) && (lst_ProfileUsernames.Count > 0) && (lst_ProfileDescriptions.Count > 0) && (lst_ProfileURLs.Count > 0))
            //    //{
            //    //    new Thread(() => StartProfileCreation()).Start();
            //    //}
            //    if (chkboxPics.Checked && lstProfilePics.Count!=0)
            //    {
            //        if (lstProfilePics.Count <= 0)
            //        {
            //            AddToListProfile(lstProfilePics.Count + " Pics Loaded");
            //            AddToListProfile("Please Add Pic In folder");
            //            MessageBox.Show(lstProfilePics.Count + " Pics Loaded");
            //        }
            //        else
            //        {
            //            //new Thread(() => StartProfileCreation()).Start();
            //        }
            //    }
            //    else if (chkboxUsername.Checked && lst_ProfileUsernames.Count != 0)
            //    {
            //        if (lst_ProfileUsernames.Count <= 0)
            //        {
            //            AddToListProfile(lst_ProfileUsernames.Count + " Usernames Loaded");
            //            AddToListProfile("Please Add UserNames");
            //            MessageBox.Show(lst_ProfileUsernames.Count + " Usernames Loaded");
            //        }
            //        else
            //        {
            //            //new Thread(() => StartProfileCreation()).Start();
            //        }
            //    }
            //    else if (chkboxProfileUrl.Checked && lst_ProfileURLs.Count != 0)
            //    {
            //        if (lst_ProfileURLs.Count <= 0)
            //        {
            //            AddToListProfile(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
            //            AddToListProfile("Please Add Profile Url's");
            //            MessageBox.Show(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
            //        }
            //        else
            //        {
            //            //new Thread(() => StartProfileCreation()).Start();
            //        }
            //    }
            //    else if (chkboxDescription.Checked && lst_ProfileDescriptions.Count != 0)
            //    {
            //        if (lst_ProfileDescriptions.Count <= 0)
            //        {
            //            AddToListProfile(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
            //            AddToListProfile("Please Add DEscription");
            //            MessageBox.Show(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
            //        }
            //        else
            //        {
            //            //new Thread(() => StartProfileCreation()).Start();
            //        }
            //    }
            //    else if (chkboxProfileLocation.Checked && lst_ProfileLocations.Count != 0)
            //    {
            //        if (lst_ProfileLocations.Count <= 0)
            //        {
            //            AddToListProfile(lst_ProfileLocations.Count + " Locations Loaded");
            //            AddToListProfile("Please Add Locations");
            //            MessageBox.Show(lst_ProfileLocations.Count + " Locations Loaded");
            //        }
            //        else
            //        {
            //            //new Thread(() => StartProfileCreation()).Start();
            //        }
            //    }
            //    else
            //    {
            //        //if (lst_ProfileLocations.Count <=0)
            //        //{
            //        //    AddToListProfile(lst_ProfileLocations.Count + " Locations Loaded");
            //        //    AddToListProfile("Please Add Locations");
            //        //    MessageBox.Show(lst_ProfileLocations.Count + " Locations Loaded");
            //        //}
            //        //else if (lst_ProfileUsernames.Count <= 0)
            //        //{
            //        //    AddToListProfile(lst_ProfileUsernames.Count + " Usernames Loaded");
            //        //    AddToListProfile("Please Add UserNames");
            //        //    MessageBox.Show(lst_ProfileUsernames.Count + " Usernames Loaded");
            //        //}
            //        //else if (lst_ProfileURLs.Count <= 0)
            //        //{
            //        //    AddToListProfile(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
            //        //    AddToListProfile("Please Add Profile Url's");
            //        //    MessageBox.Show(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
            //        //}
            //        //else if (lst_ProfileDescriptions.Count <= 0)
            //        //{
            //        //    AddToListProfile(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
            //        //    AddToListProfile("Please Add DEscription");
            //        //    MessageBox.Show(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
            //        //}

            //        AddToListProfile(" Please Select Checkbox as your requirement.");
            //        AddToListProfile("Please Select Checkbox as your requirement.");
            //        MessageBox.Show("Please Select Checkbox as your requirement.");
            //    }
            //    //btnStartProfileCreation.Enabled = false;
            //}
            //else
            //{
            //    MessageBox.Show("Please load Profile Data");
            //} 
            #endregion

            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txtProfilePicsFolder.Text) || !string.IsNullOrEmpty(txtAccountsFolder.Text))
                {
                    try
                    {
                        AddToListProfile("[ " + DateTime.Now + " ] => [ Starting Profile Manager ]");

                        objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "PicsFolder", StringEncoderDecoder.Encode(txtProfilePicsFolder.Text));
                        objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "ProfileFolder", StringEncoderDecoder.Encode(txtAccountsFolder.Text));

                        #region
                        //if (((lstProfilePics.Count > 0) && chkboxPics.Checked) || ((lst_ProfileLocations.Count > 0) && chkboxProfileLocation.Checked) || ((lst_ProfileUsernames.Count > 0) && chkboxUsername.Checked) || ((lst_ProfileDescriptions.Count > 0) && chkboxDescription.Checked) || ((lst_ProfileURLs.Count > 0) && chkboxProfileUrl.Checked))
                        //{
                        //    new Thread(() => StartProfileCreation()).Start();
                        //}
                        #endregion

                        if ((lstProfilePics.Count > 0) || (lst_ProfileLocations.Count > 0) || (lst_ProfileUsernames.Count > 0) || (lst_ProfileDescriptions.Count > 0) || (lst_ProfileURLs.Count > 0))
                        {
                            if (!chkboxPics.Checked && !chkboxUsername.Checked && !chkboxProfileUrl.Checked && !chkboxDescription.Checked && !chkboxProfileLocation.Checked)
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Please check any below given options. ]");
                                MessageBox.Show("Please check any below given options.");
                            }
                            else
                            {
                                new Thread(() => StartProfileCreation()).Start();
                            }
                        }

                        else
                        {
                            if ((lstProfilePics.Count == 0) && (lst_ProfileLocations.Count == 0) && (lst_ProfileUsernames.Count == 0) && (lst_ProfileDescriptions.Count == 0) && (lst_ProfileURLs.Count == 0))
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Folder is Empty ]");
                                MessageBox.Show("Folder is Empty");
                            }
                            else if (lst_ProfileLocations.Count <= 0)
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_ProfileLocations.Count + " Locations Loaded ]");
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Please Add Locations ]");
                                MessageBox.Show(lst_ProfileLocations.Count + " Locations Loaded");
                            }
                            else if (lst_ProfileUsernames.Count <= 0)
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [  " + lst_ProfileUsernames.Count + " Usernames Loaded ]");
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Please Add UserNames ]");
                                MessageBox.Show(lst_ProfileUsernames.Count + " Usernames Loaded");
                            }
                            else if (lst_ProfileURLs.Count <= 0)
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [  " + lst_ProfileURLs.Count + " ProfileURLs  Loaded ]");
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Please Add Profile Url's ]");
                                MessageBox.Show(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                            }
                            else if (lst_ProfileDescriptions.Count <= 0)
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [  " + lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded ]");
                                AddToListProfile("[ " + DateTime.Now + " ] => [  Please Add DEscription ]");
                                MessageBox.Show(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
                            }

                            #region
                            //if (((lstProfilePics.Count == 0) && chkboxPics.Checked))
                            //{
                            //    AddToListProfile("Folder is Empty");
                            //    MessageBox.Show("Folder is Empty");
                            //}
                            //if ((lst_ProfileLocations.Count <= 0) && chkboxProfileLocation.Checked)
                            //{
                            //    AddToListProfile(lst_ProfileLocations.Count + " Locations Loaded");
                            //    AddToListProfile("Please Add Locations");
                            //    MessageBox.Show(lst_ProfileLocations.Count + " Locations Loaded");
                            //}
                            //if ((lst_ProfileUsernames.Count <= 0) && chkboxUsername.Checked)
                            //{
                            //    AddToListProfile(lst_ProfileUsernames.Count + " Usernames Loaded");
                            //    AddToListProfile("Please Add UserNames");
                            //    MessageBox.Show(lst_ProfileUsernames.Count + " Usernames Loaded");
                            //}
                            //if ((lst_ProfileDescriptions.Count <= 0) && chkboxDescription.Checked)
                            //{
                            //    AddToListProfile(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
                            //    AddToListProfile("Please Add DEscription");
                            //    MessageBox.Show(lst_ProfileDescriptions.Count + " Profile Descriptions  Loaded");
                            //}
                            //if ((lst_ProfileURLs.Count <= 0) && chkboxProfileUrl.Checked)
                            //{
                            //    AddToListProfile(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                            //    AddToListProfile("Please Add Profile Url's");
                            //    MessageBox.Show(lst_ProfileURLs.Count + " ProfileURLs  Loaded");
                            //}

                            //if (!chkboxPics.Checked && !chkboxUsername.Checked && !chkboxProfileUrl.Checked && !chkboxDescription.Checked && !chkboxProfileLocation.Checked)
                            //{
                            //    AddToListProfile("Please check any below given options and upload files.");
                            //    MessageBox.Show("Please check any below given options and upload files.");
                            //}
                            #endregion

                        }
                        //btnStartProfileCreation.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText(ex.Message);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnStartProfileCreation_Click --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                    }
                }
                else
                {
                    MessageBox.Show("Please load Profile Data");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToListProfile("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
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
                    AddToListProfile("[ " + DateTime.Now + " ] => [ Editing Profile For " + item.Key + " ]");
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
                                if (lst_ProfileUsernames.Count != 0)
                                {
                                    if (count_profileUsername < lst_ProfileUsernames.Count)
                                    {
                                        profileUsername = lst_ProfileUsernames[count_profileUsername];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Username : " + profileUsername + " ]");
                                    }
                                    else
                                    {
                                        count_profileUsername = 0;
                                        profileUsername = lst_ProfileUsernames[count_profileUsername];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Username : " + profileUsername + " ]");
                                    }
                                }
                                else
                                {
                                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload User Name ]");
                                    return;
                                }
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
                                if (lst_ProfileURLs.Count != 0)
                                {
                                    if (count_profileURL < lst_ProfileURLs.Count)
                                    {
                                        profileURL = lst_ProfileURLs[count_profileURL];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [  " + item.Key + " >>>> Url : " + profileURL + " ]");
                                    }
                                    else
                                    {
                                        count_profileURL = 0;
                                        profileURL = lst_ProfileURLs[count_profileURL];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Url : " + profileURL + " ]");
                                    }
                                }
                                else
                                {
                                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Profile URL ]");
                                    return;
                                }
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
                                if (lst_ProfileDescriptions.Count != 0)
                                {
                                    if (count_profileDescription < lst_ProfileDescriptions.Count)
                                    {
                                        profileDescription = lst_ProfileDescriptions[count_profileDescription];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Description : " + profileDescription + " ]");
                                    }
                                    else
                                    {
                                        count_profileDescription = 0;
                                        profileDescription = lst_ProfileDescriptions[count_profileDescription];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Description : " + profileDescription + " ]");
                                    }
                                }
                                else
                                {
                                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Profile Descriptions ]");
                                    return;
                                }
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
                                if (lst_ProfileLocations.Count != 0)
                                {
                                    if (count_profileLocation < lst_ProfileLocations.Count)
                                    {
                                        profileLocation = lst_ProfileLocations[count_profileLocation];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Location : " + profileLocation + " ]");
                                    }
                                    else
                                    {
                                        count_profileLocation = 0;
                                        profileLocation = lst_ProfileLocations[count_profileLocation];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Location : " + profileLocation + " ]");
                                    }
                                }
                                else
                                {
                                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Profile Locations. ]");
                                    return;
                                }
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
                                if (lstProfilePics.Count != 0)
                                {
                                    if (count_profilePic < lstProfilePics.Count)
                                    {
                                        profilePic = lstProfilePics[count_profilePic];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Picture : " + profilePic + " ]");
                                    }
                                    else
                                    {
                                        count_profilePic = 0;
                                        profilePic = lstProfilePics[count_profilePic];
                                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Picture : " + profilePic + " ]");
                                    }

                                    //// Load the Image
                                    //using (System.Drawing.Image objImage = System.Drawing.Image.FromFile(profilePic))
                                    //{
                                    //    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                                    //    objImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    //    var size =stream.Length;

                                    //    if (true)
                                    //    {

                                    //    }
                                    //}
                                }
                                else
                                {
                                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Profile Pic's. ]");
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogger.AddToErrorLogText(ex.Message);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxPics --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxPics --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        #region Commented
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                        //{

                        //TweetAccountManager tweetAccountManager = item.Value;

                        //string accountUser = tweetAccountManager.Username;
                        //string accountPass = tweetAccountManager.Password;
                        //string IPAddress = tweetAccountManager.IPAddress;
                        //string IPPort = tweetAccountManager.IPPort;
                        //string IPUsername = tweetAccountManager.IPUsername;
                        //string IPpassword = tweetAccountManager.IPpassword;

                        //tweetAccountManager = new TweetAccountManager();
                        //tweetAccountManager.Username = accountUser;
                        //tweetAccountManager.Password = accountPass;
                        //tweetAccountManager.IPAddress = IPAddress;
                        //tweetAccountManager.IPPort = IPPort;
                        //tweetAccountManager.IPUsername = IPUsername;
                        //tweetAccountManager.IPpassword = IPpassword;

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
                //MessageBox.Show("Please Upload Twitter Account");
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                btnStartProfileCreation.Invoke(new MethodInvoker(delegate
                {
                    frmAccounts objFrmAccounts = new frmAccounts();
                    objFrmAccounts.Show();
                }));
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

                #region commneted
                //string accountUser = tweetAccountManager.Username;
                //string accountPass = tweetAccountManager.Password;
                //string IPAddress = tweetAccountManager.IPAddress;
                //string IPPort = tweetAccountManager.IPPort;
                //string IPUsername = tweetAccountManager.IPUsername;
                //string IPpassword = tweetAccountManager.IPpassword;

                //tweetAccountManager = new TweetAccountManager();
                //tweetAccountManager.Username = accountUser;
                //tweetAccountManager.Password = accountPass;
                //tweetAccountManager.IPAddress = IPAddress;
                //tweetAccountManager.IPPort = IPPort;
                //tweetAccountManager.IPUsername = IPUsername;
                //tweetAccountManager.IPpassword = IPpassword; 
                #endregion

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
                //GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_ProfileManager() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }
        List<string> listReplyMessages1 = new List<string>();
        private void btnUploadReplyFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkUseSpinnedMessages.Checked)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtReplyMsgFile.Text = ofd.FileName;
                            new Thread(() =>
                            {
                                try
                                {
                                    listReplyMessages = new List<string>();
                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Process is running... ]");
                                    listReplyMessages1 = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                                    listReplyMessages1.ForEach(Rmsg =>
                                    {
                                        if (Rmsg.Length <= 140)
                                        {
                                            listReplyMessages.Add(Rmsg.ToString());
                                        }
                                    });

                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listReplyMessages.Count + " Reply Message loaded ]");

                                    if (listReplyMessages.Count < listReplyMessages1.Count)
                                    {
                                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + (listReplyMessages1.Count - listReplyMessages.Count).ToString() + " Reply Messages contains characters greater than the limit of 140. ]");
                                    }
                                }
                                catch { };
                            }).Start();
                        }
                    }
                }
                else if (chkUseSpinnedMessages.Checked)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtReplyMsgFile.Text = ofd.FileName;
                                
                            
                                try
                                {
                                    listReplyMessages = new List<string>();
                                    listReplyMessages1 = GlobusFileHelper.ReadLargeFileForSpinnedMessage(ofd.FileName);
                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Process is running... ]");
                                    new Thread(() =>
                                    {
                                    foreach (var item in listReplyMessages1)
                                    {
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(GetSpinnedListItemReply), new object[] { item });
                                        Thread.Sleep(50);
                                      }
                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listReplyMessages.Count + " Reply Message loaded ]");
                                    if (listReplyMessages.Count < listReplyMessages1.Count)
                                    {
                                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + (listReplyMessages1.Count - listReplyMessages.Count).ToString() + " Reply Messages contains characters greater than the limit of 140. ]");
                                    }
                                    }).Start();
                                    
                                }
                                catch(Exception ex)
                                {
 
                                }
                            
                        }
                    }
                }
                else
                    MessageBox.Show("Please select and option between Regular file and Spintax");
            }
            catch (Exception)
            {

                AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ File loadding is failed. ]");
            }
        }
        private void GetSpinnedListItemReply(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string item = paramsArray.GetValue(0).ToString();
                listReplyMessages1 = SpinnedListGenerator.GetSpinnedList(new List<string> { item });
                new Thread(() =>
                            {
                                foreach (string _item in listReplyMessages1)
                                {
                                    try
                                    {
                                        listReplyMessages1.ForEach(Rmsg =>
                                        {
                                            if (Rmsg.Length <= 140)
                                            {
                                                listReplyMessages.Add(Rmsg.ToString());
                                            }
                                        });
                                        //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Spinned Message : " + item + " ]");
                                        //GlobusFileHelper.AppendStringToTextfileNewLine(_item, path_TweetCreatorExportFile);
                                    }
                                    catch (Exception ex)
                                    {
                                        //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile + " ]");
                                    }
                                }
                            }).Start();
            }
            catch (Exception ex)
            {
                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStart_TweetCreator_Click() --> " + ex.Message, Globals.Path_ErrortweetCreator);
            }
        }
        private void GetSpinnedListItemTweets(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string item = paramsArray.GetValue(0).ToString();
                listTweetMessages1 = SpinnedListGenerator.GetSpinnedList(new List<string> { item });
                new Thread(() =>
                {
                    foreach (string _item in listTweetMessages1)
                    {
                        try
                        {
                            listTweetMessages1.ForEach(Rmsg =>
                            {
                                if (Rmsg.Length <= 140)
                                {
                                    listTweetMessagesForReply.Add(Rmsg.ToString());
                                }
                            });
                            //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Spinned Message : " + item + " ]");
                            //GlobusFileHelper.AppendStringToTextfileNewLine(_item, path_TweetCreatorExportFile);
                        }
                        catch (Exception ex)
                        {
                            //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile + " ]");
                        }
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStart_TweetCreator_Click() --> " + ex.Message, Globals.Path_ErrortweetCreator);
            }
        }

        private List<string> GetUniqueLinksList(List<string> inputList)
        {
            List<string> tempList = new List<string>();
            inputList.ForEach(l => tempList.Add(MakeLinksUnique(l.ToString())));
            //foreach (string item in inputList)
            //{
            //    string uniqueLink_Data = MakeLinksUnique(item);
            //    tempList.Add(uniqueLink_Data);
            //}

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
        List<string> listTweetMessages1 = new List<string>();
        private void btnUploadTweetFile_Click(object sender, EventArgs e)
        {
            //#region commentedCodeByPuja
            try
            {
                if (!chkUseSpinnedMessages.Checked)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtTweetMsgFile.Text = ofd.FileName;
                            new Thread(() =>
                            {
                                try
                                {
                                    listTweetMessagesForReply = new List<string>();
                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Process is running... ]");
                                    listTweetMessages1 = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                                    listTweetMessages1.ForEach(Rmsg =>
                                    {
                                        if (Rmsg.Length <= 140)
                                        {
                                            listTweetMessagesForReply.Add(Rmsg.ToString());
                                        }
                                    });

                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listTweetMessagesForReply.Count + " Tweet Message loaded ]");

                                    if (listTweetMessagesForReply.Count < listTweetMessages1.Count)
                                    {
                                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + (listTweetMessages1.Count - listTweetMessagesForReply.Count).ToString() + " Tweet Messages contains characters greater than the limit of 140. ]");
                                    }
                                }
                                catch { };
                            }).Start();
                        }
                    }
                }
                else if (chkUseSpinnedMessages.Checked)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txtTweetMsgFile.Text = ofd.FileName;


                            try
                            {
                                listTweetMessagesForReply = new List<string>();
                                listTweetMessages1 = GlobusFileHelper.ReadLargeFileForSpinnedMessage(ofd.FileName);
                                AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Process is running... ]");
                                new Thread(() =>
                                {
                                    foreach (var item in listTweetMessages1)
                                    {
                                        ThreadPool.QueueUserWorkItem(new WaitCallback(GetSpinnedListItemTweets), new object[] { item });
                                        Thread.Sleep(50);
                                    }
                                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listReplyMessages.Count + " Tweet Message loaded ]");
                                    if (listTweetMessagesForReply.Count < listTweetMessages1.Count)
                                    {
                                        AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + (listTweetMessages1.Count - listTweetMessagesForReply.Count).ToString() + " Tweet Messages contains characters greater than the limit of 140. ]");
                                    }
                                }).Start();

                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                }
                else
                    MessageBox.Show("Please select and option between Regular file and Spintax");
            }
            catch (Exception)
            {

                AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ File loadding is failed. ]");
            } 
            //#endregion
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
                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ " + listKeywords.Count + " Keywords loaded ]");
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

        //private void btnEmailverification_Click(object sender, EventArgs e)
        //{
        //    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
        //    {
        //        ofd.Filter = "Text Files (*.txt)|*.txt";
        //        ofd.InitialDirectory = Application.StartupPath;
        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            txtNames.Text = ofd.FileName;
        //            listNames = new List<string>();
        //            listNames.Clear();

        //            List<string> templist = GlobusFileHelper.ReadLargeFile(ofd.FileName);//GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
        //            foreach (string item in templist)
        //            {
        //                if (!listNames.Contains(item))
        //                {
        //                    listNames.Add(item);
        //                }
        //            }
        //            Console.WriteLine(listNames.Count + " Names loaded");
        //            AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ " + listNames.Count + " Names loaded ]");

        //        }
        //    }
        //}


        List<Thread> LstPublicIP = new List<Thread>();
        bool IsStopPublicIP = false;

        private void btnTestPublicIP_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    workingproxiesCount = 0;
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Existing Proxies Saved To : ]");
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + Globals.Path_ExsistingProxies + " ]");
                    List<string> lstProxies = new List<string>();
                    if (chkboxUseUrlIP.Checked && !string.IsNullOrEmpty(txtIPUrl.Text))
                    {
                        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                        string pageSource = HttpHelper.getHtmlfromUrlIP(new Uri(txtIPUrl.Text), "", "", "", "", "");
                        string[] array = Regex.Split(pageSource, "\n");
                        foreach (string item in array)
                        {
                            string IP = item.Replace("\r", "");
                            lstProxies.Add(item);
                        }
                    }
                    else if (!string.IsNullOrEmpty(txtPublicIP.Text.Trim()))
                    {
                        lstProxies = GlobusFileHelper.ReadFiletoStringList(txtPublicIP.Text);
                    }
                    else
                    {
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Please Upload Either Url or Load IP Files ]");
                        return;
                    }
                    //lstProxies = Globussoft.GlobusFileHelper.ReadFiletoStringList(txtPublicIP.Text);
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + lstProxies.Count() + " Public Proxies Uploaded ]");
                    new Thread(() =>
                    {
                        GetValidProxies(lstProxies);
                    }).Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void GetValidProxies(List<string> lstProxies)
        {

            try
            {
                if (IsStopPublicIP)
                {
                    return;
                }
                LstPublicIP.Add(Thread.CurrentThread);
                LstPublicIP = LstPublicIP.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch { }
            //try
            //{
            //    Thread.CurrentThread.Name = "main";
            //    //dictionary_Threads.Add("IP_", Thread.CurrentThread);
            //    dictionary_Threads.Add("IP_", Thread.CurrentThread);
            //}
            //catch { };

            numberOfIPThreads = 25;
            threadcountForFinishMSG = lstProxies.Count;
            if (!string.IsNullOrEmpty(txtNumberOfIPThreads.Text) && GlobusRegex.ValidateNumber(txtNumberOfIPThreads.Text))
            {
                numberOfIPThreads = int.Parse(txtNumberOfIPThreads.Text);
            }

            //WaitCallback waitCallBack = new WaitCallback(ThreadPoolMethod_Proxies);
            foreach (string item in lstProxies)
            {
                if (countParseProxiesThreads >= numberOfIPThreads)
                {
                    lock (proxiesThreadLockr)
                    {
                        Monitor.Wait(proxiesThreadLockr);
                    }
                }

                ///Code for checking and then adding proxies to FinalIPList...
                //ThreadPool.QueueUserWorkItem(waitCallBack, item);

                Thread GetStartProcessForChAngeAcPassword = new Thread(ThreadPoolMethod_Proxies);
                //GetStartProcessForChAngeAcPassword.Name = "AcPassChange" + "_" + item.Key;
                GetStartProcessForChAngeAcPassword.IsBackground = true;
                GetStartProcessForChAngeAcPassword.Start(item);
               
                //Thread.Sleep(500);
            }

            //AddToIPsLogs(ValidPublicProxies.Count() + " Public Proxies Valid");
        }

        private void ThreadPoolMethod_Proxies(object objIP)
        {
            try
            {
                LstPublicIP.Add(Thread.CurrentThread);
                LstPublicIP = LstPublicIP.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                Interlocked.Increment(ref countParseProxiesThreads);
                //countParseProxiesThreads++;

                string item = (string)objIP;
                int IsPublic = 0;
                int Working = 0;
                string LoggedInIp = string.Empty;

                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                string account = item;

                int DataCount = account.Split(':').Length;

                if (DataCount == 1)
                {
                    IPAddress = account.Split(':')[0];
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP Not In correct Format ]");
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + account + " ]");
                    return;
                }
                if (DataCount == 2)
                {
                    IPAddress = account.Split(':')[0];
                    IPPort = account.Split(':')[1];
                }
                else if (DataCount > 2)
                {
                    IPAddress = account.Split(':')[0];
                    IPPort = account.Split(':')[1];
                    IPUsername = account.Split(':')[2];
                    IPpassword = account.Split(':')[3];
                    //AddToIPsLogs("IP Not In correct Format");
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + account + " ]");
                    return;
                }

                try
                {
                    dictionary_Threads.Add("IP_" + IPAddress, Thread.CurrentThread);
                }
                catch { };


                IPChecker IPChecker = new IPChecker(IPAddress, IPPort, IPUsername, IPpassword, IsPublic);
                if (IPChecker.CheckIP())
                {
                    //lock (((System.Collections.ICollection)listWorkingProxies).SyncRoot)
                    {
                        //if (!listWorkingProxies.Contains(IP))
                        {
                            workingproxiesCount++;
                            //listWorkingProxies.Add(IP);
                            lock (IPListLockr)
                            {
                                queWorkingProxies.Enqueue(item);
                                Monitor.Pulse(IPListLockr);
                            }
                            AddToIPsLogs("[ " + DateTime.Now + " ] => [ Added " + item + " to working proxies list ]");

                            lock (Locker_LstRunningIP_IPModule)
                            {
                                LstRunningIP_IPModule.Add(item);
                            }

                            Globals.EnquequeWorkingProxiesForSignUp(item);

                            //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.FilePathWorkingProxies);
                        }
                    }
                }
                else
                {
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Non Working IP: " + IPAddress + ":" + IPPort + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExistingProxies);
                }

                #region Commented code
                //string pageSource = string.Empty;
                //try
                //{
                //    GlobusHttpHelper httpHelper = new GlobusHttpHelper();
                //    pageSource = httpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), "", IPAddress, IPPort, IPUsername, IPpassword);
                //}
                //catch (Exception ex)
                //{
                //    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExsistingProxies);
                //}
                //if (pageSource.Contains("class=\"signin\"") && pageSource.Contains("class=\"signup\"") && pageSource.Contains("Twitter"))
                //{
                //    using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                //    {
                //        //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + IPAddress + "'", con))
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
                //            string InsertQuery = "Insert into tb_IP values('" + IPAddress + "','" + IPPort + "','" + IPUsername + "','" + IPpassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                //            DataBaseHandler.InsertQuery(InsertQuery, "tb_IP");
                //        }
                //    }
                //    ValidPublicProxies.Add(item);
                //} 

                #endregion
            }
            catch (Exception ex)
            {
                //AddToIPsLogs(ex.Message);
            }
            finally
            {
                lock (proxiesThreadLockr)
                {
                    //countParseProxiesThreads--;
                    Interlocked.Decrement(ref countParseProxiesThreads);
                    Monitor.Pulse(proxiesThreadLockr);
                }

                threadcountForFinishMSG--;

                if (threadcountForFinishMSG == 0)
                {

                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToIPsLogs("-----------------------------------------------------------------------------------------------------------------------");
                }


            }
        }

        private void btnClearPublicProxies_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (MessageBox.Show("Do you really want to delete all the Proxies from Database", "IP", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        clsDBQueryManager setting = new clsDBQueryManager();
                        setting.DeletePublicIPData();
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ All Public Proxies Deleted from the DataBase ]");
                        workingproxiesCount = 0;
                        lbltotalworkingproxies.Invoke(new MethodInvoker(delegate
                        {
                            lbltotalworkingproxies.Text = "Total Working Proxies : " + workingproxiesCount.ToString();
                        }));
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void btnClearPrivateProxies_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (MessageBox.Show("Do you really want to delete all the Private Proxies from Database???", "IP", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        new Thread(() =>
                        {
                            clsDBQueryManager setting = new clsDBQueryManager();
                            setting.DeletePrivateIPData();
                            AddToIPsLogs("[ " + DateTime.Now + " ] => [ All Private Proxies Deleted from the DataBase ]");
                        }).Start();
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void btnPublicIP_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtPublicIP.Text = ofd.FileName;

                        List<string> publicIP = new List<string>();
                        publicIP = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Public IP FileUploaded ]");
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + publicIP.Count + " Public IP Uploaded ]");
                        if (!string.IsNullOrEmpty(txtPublicIP.Text))
                        {
                            objclsSettingDB.InsertOrUpdateSetting("WaitAndReply", "ReplyMsgFile", StringEncoderDecoder.Encode(txtPublicIP.Text));
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnPvtIPFour_Click(object sender, EventArgs e)
        {
            try
            {
                int IsPublic = 0;
                int Working = 0;
                string LoggedInIp = string.Empty;
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    List<string> lstValidIPList = new List<string>();
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {

                        txtPvtIPFour.Text = ofd.FileName;
                        List<string> pvtIP = new List<string>();
                        pvtIP = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);


                        new Thread(() =>
                        {
                            #region start IP Insert process in DB ..

                            foreach (string IPlst in pvtIP)
                            {
                                string account = IPlst;
                                string IPAddress = string.Empty;
                                string IPPort = string.Empty;
                                string IPUsername = string.Empty;
                                string IPpassword = string.Empty;

                                int DataCount = account.Split(':').Length;

                                using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                                {
                                    //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + IPAddress + "'", con))
                                    using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                                    {
                                        if (DataCount == 4)
                                        {
                                            try
                                            {
                                                lstValidIPList.Add(IPlst);
                                                string[] Data = account.Split(':');
                                                IPAddress = Data[0];
                                                IPPort = Data[1];
                                                IPUsername = Data[2];
                                                IPpassword = Data[3];
                                                LoggedInIp = "NoIP";
                                                IsPublic = 1;
                                                Working = 1;
                                                string InsertQuery = "Insert into tb_IP values('" + IPAddress + "','" + IPPort + "','" + IPUsername + "','" + IPpassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                                DataBaseHandler.InsertQuery(InsertQuery, "tb_IP");
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                        else
                                        {
                                            AddToIPsLogs("[ " + DateTime.Now + " ] => [ Only Private Proxies allowed using this option ]");
                                        }
                                    }
                                }
                            }

                            AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + lstValidIPList.Count() + " Private Proxies File Uploaded ]");

                            #endregion

                        }).Start();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnPvtIPThree_Click(object sender, EventArgs e)
        {
            int IsPublic = 0;
            int Working = 0;
            string LoggedInIp = string.Empty;
            List<string> lstValidIPList = new List<string>();
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtPvtIPThree.Text = ofd.FileName;
                    List<string> pvtIP = new List<string>();
                    pvtIP = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    foreach (string IPlst in pvtIP)
                    {
                        string account = IPlst;
                        string IPAddress = string.Empty;
                        string IPPort = string.Empty;
                        string IPIp = string.Empty;
                        string IPUsername = string.Empty;
                        string IPpassword = string.Empty;

                        int DataCount = account.Split(':').Length;

                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + IPAddress + "'", con))
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                if (DataCount == 3)
                                {
                                    lstValidIPList.Add(IPlst);
                                    string[] Data = account.Split(':');
                                    IPAddress = Data[0];
                                    IPPort = Data[1];
                                    IPUsername = "";
                                    IPpassword = "";
                                    LoggedInIp = Data[2];
                                    IsPublic = 1;
                                    Working = 1;
                                    string InsertQuery = "Insert into tb_IP values('" + IPAddress + "','" + IPPort + "','" + IPUsername + "','" + IPpassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                                    DataBaseHandler.InsertQuery(InsertQuery, "tb_IP");
                                }
                            }
                        }
                    }
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + lstValidIPList.Count() + " Private Proxies File Uploaded ]");
                }
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
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Added " + lstscrapeUsername.Count + " Username To Scrape ]");
                }
            }
        }

        protected bool IsStart_ScrapUser = true;

        private void btnScrapeUser_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (!string.IsNullOrEmpty(txtScrapeUpload.Text))
                    {
                        objclsSettingDB.InsertOrUpdateSetting("UserScrape", "ScrapeFollowerFollowing", StringEncoderDecoder.Encode(txtScrapeUpload.Text));
                    }

                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Starting Scraping Users ]");
                    Globals.lstScrapedUserIDs.Clear();
                    if (!string.IsNullOrEmpty(txtScrapeUserName.Text) || (lstscrapeUsername.Count > 0) || !string.IsNullOrEmpty(txtScrapeKeyword.Text))
                    {
                        if (!string.IsNullOrEmpty(txtScrapeUserName.Text))
                        {
                            lstscrapeUsername.Clear();
                            lstscrapeUsername.Add(txtScrapeUserName.Text);
                            txtScrapeUpload.Text = "";
                        }
                        //Thread thread_StartScrape = null;
                        if (IsStart_ScrapUser)
                        {
                            if (!Globals.IsMobileVersion)
                            {
                                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                                {
                                    IsStart_ScrapUser = false;
                                    thread_StartScrape = new Thread(() =>
                                    {
                                        threadStartScrape();
                                    });
                                    thread_StartScrape.Start();
                                }
                                else
                                {
                                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts To Start Scraping ]");
                                }
                            }
                            else
                            {
                                IsStart_ScrapUser = false;
                                thread_StartScrape = new Thread(() =>
                                {
                                    threadStartScrape();
                                });
                                thread_StartScrape.Start();
                            }
                        }
                    }
                    else
                    {
                        if ((string.IsNullOrEmpty(txtScrapeUserName.Text) && (lstscrapeUsername.Count <= 0) && string.IsNullOrEmpty(txtScrapeKeyword.Text)))
                        {
                            MessageBox.Show("Please Fill In Appropriate Details");
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Fill In Appropriate Details ]");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeUser_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeUser_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void threadStartScrape()
        {
            TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
            try
            {
                List<string> lst_structTweetFollowersIDs = new List<string>();
                List<string> lst_structTweetFollowingsIds = new List<string>();
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                string user_id = string.Empty;
                int counter = 0;
                TweetAccountManager TweetLogin = new TweetAccountManager();
                string ReturnStatus = string.Empty;
                Globals.IsMobileVersion = true;
                if (!Globals.IsMobileVersion)
                {
                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                        {
                            counter = 0;
                        }

                        TweetLogin = new TweetAccountManager();
                        TweetLogin.Username = item.Key;
                        TweetLogin.Password = item.Value.Password;
                        TweetLogin.IPAddress = item.Value.IPAddress;
                        TweetLogin.IPPort = item.Value.IPPort;
                        TweetLogin.IPUsername = item.Value.IPUsername;
                        TweetLogin.IPpassword = item.Value.IPpassword;
                        TweetLogin.Login();

                        if (!TweetLogin.IsLoggedIn)
                        {
                            continue;
                        }
                        else
                        {
                            globusHttpHelper = TweetLogin.globusHttpHelper;
                            counter++;
                            break;
                        }
                    }
                   

                    if (!TweetLogin.IsLoggedIn)
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Atleast One Working Account To Get Details ]");
                        return;
                    }
                    
                }

                
                foreach (string keyword in lstscrapeUsername)
                {
                    
                    dataScrapeer.CounterDataNo = 500;

                    txtlimitScrapeUsers.Invoke(new MethodInvoker(delegate
                    {
                        if (!string.IsNullOrEmpty(txtlimitScrapeUsers.Text) && NumberHelper.ValidateNumber(txtlimitScrapeUsers.Text))
                        {
                            dataScrapeer.CounterDataNo = Convert.ToInt32(txtlimitScrapeUsers.Text);
                        }
                    }));

                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }

                    if (chkboxScrapeFollowers.Checked)
                    {
                        try
                        {
                            if (!File.Exists(Globals.Path_ScrapedFollowersList))
                            {
                               // GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowersUserID , Followers User Name", Globals.Path_ScrapedFollowersList);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id ,  Followers User Name", Globals.Path_ScrapedFollowersList);
                            }

                            try
                            {
                                dataScrapeer.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                                if (!Globals.IsMobileVersion)
                                {
                                    lst_structTweetFollowersIDs = dataScrapeer.GetFollowers_New(keyword.Trim(), out ReturnStatus, ref globusHttpHelper);
                                }
                                else
                                {
                                    lst_structTweetFollowersIDs = dataScrapeer.GetFollowers_New_ForMobileVersion(keyword.Trim(), out ReturnStatus, ref globusHttpHelper);
                                }
                                dataScrapeer.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
                            }
                            catch (Exception ex)
                            {

                            }
                            if (lst_structTweetFollowersIDs.Count > 0)
                            {
                                //AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Adding To DataBase and File ]");
                                //AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers to list ]");

                                //foreach (string data in lst_structTweetFollowersIDs)
                                //{
                                //    try
                                //    {
                                //        string[] arr_data = data.Split(':');
                                //        Globals.lstScrapedUserIDs.Add(arr_data[0]);
                                //        GlobusFileHelper.AppendStringToTextfileNewLine(keyword + "," + arr_data[0] + "," + arr_data[1], Globals.Path_ScrapedFollowersList);
                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowersIDs foreach  --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowersIDs foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                                //    }
                                //}

                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword + " ]");
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowersList);
                                if (Globals.IsDirectedFromFollower)
                                {
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + keyword + " ]");
                                    Thread.Sleep(1000);
                                }
                            }
                            else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Sorry, That User does not exist =>" + ReturnStatus.Split(':')[1] + " ]");
                                continue;
                            }
                            else if (ReturnStatus == "Account is Suspended. ")
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Account " + keyword + "  is Suspended. ]");
                            }
                            else if (ReturnStatus == "Error")
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                            }
                            else
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + keyword + " User does not exist ]");
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
                              //  GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , FollowingsUserID , Following User Name", Globals.Path_ScrapedFollowingsList);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Name/Id , Following User Name", Globals.Path_ScrapedFollowingsList);

                            }
                            string returnStaus = string.Empty;
                            string Screen_name = string.Empty;
                            dataScrapeer.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                           // GlobusHttpHelper globusHttpHelper1 = new GlobusHttpHelper();
                            if (!Globals.IsMobileVersion)
                            {
                                lst_structTweetFollowingsIds = dataScrapeer.GetFollowings_New(keyword.Trim(), out returnStaus, ref  globusHttpHelper);

                            }
                            else
                            {
                                lst_structTweetFollowingsIds = dataScrapeer.GetFollowings_NewForMobileVersion(keyword.Trim(), out returnStaus, ref  globusHttpHelper);
                            }

                            if (lst_structTweetFollowingsIds.Count > 0)
                            {
                                
                                if (lst_structTweetFollowingsIds.Count > 0)
                                {
                                    
                                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword + " ]");
                                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowingsList + " ]");
                                    if (Globals.IsDirectedFromFollower)
                                    {
                                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowingsIds.Count + " Followings from User: " + keyword + " ]");
                                        Thread.Sleep(1000);
                                        //Tb_AccountManager.Invoke(new MethodInvoker(delegate
                                        //{
                                        //    Tb_AccountManager.SelectedIndex = 2;
                                        //}));
                                        //tabMain.SelectedIndex = 2;
                                    }
                                }
                                else if (returnStaus == "Error")
                                {
                                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                                    break;
                                }
                            }
                            else
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + keyword + " User does not have any followings ]");
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() -- lst_structTweetFollowingsIds foreach --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    Globals.lstScrapedUserIDs = Globals.lstScrapedUserIDs.Distinct().ToList();
                }

                Globals.IsMobileVersion = false;
                new Thread(() =>
                {
                    try
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
                                DataBase.InsertOrUpdateScrapeSetting(data, "", "");
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> lstScrapedUserIDs --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> lstScrapedUserIDs --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }

                }).Start();


                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddToScrapeLogs("------------------------------------------------------------------------------------------------------------------------------------------");

                //if (IsUserScrapedDatalist)
                //{
                //    Tb_AccountManager.Invoke(new MethodInvoker(delegate
                //    {
                //        Tb_AccountManager.SelectedIndex = 0;
                //        //Tb_AccountManager.SelectedTab.Name = "tabFollower";
                //    }));
                //}
            }
            catch (Exception)
            {

            }
            finally
            {
                IsStart_ScrapUser = true;
                dataScrapeer.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
            }
        }


        public static bool IsItNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }


        private void btnUpload_SearchByKeyword_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtScrapeKeyword.Text = ofd.FileName;
                    lstSearchByKeywords.Clear();

                    lstSearchByKeywords = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Added " + lstSearchByKeywords.Count + " Keyword To Scrape ]");
                }
            }
        }

        private void btnScrapeKeyword_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (!string.IsNullOrEmpty(txtScrapeKeyword.Text))
                    {
                        thread_StartKeywordScrape = new Thread(() =>
                        {
                            ScrapeKeywordSeacrh();
                        });
                        thread_StartKeywordScrape.Start();
                    }
                    else
                    {
                        MessageBox.Show("Please input a Keyword");
                    }
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeKeyword_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
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

                txtRecords.Invoke(new MethodInvoker(delegate
                    {

                        if (!string.IsNullOrEmpty(txtRecords.Text.Trim()) && NumberHelper.ValidateNumber(txtRecords.Text.Trim()))
                        {
                            TwitterDataScrapper.noOfRecords = Convert.ToInt32(txtRecords.Text.Trim());
                            if (TwitterDataScrapper.noOfRecords == 0)
                            {
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Do not put Zero value ]");
                                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Default number of records is 100 ]");
                                TwitterDataScrapper.noOfRecords = 100;
                            }
                        }
                        else
                        {
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ please enter value in number of users ]");
                            return;
                        }
                    }));

                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Scrape by Keyword ]");
                //List<TwitterDataScrapper.StructTweetIDs> data = TweetData.GetTweetData(txtScrapeKeyword.Text);
                //TweetData.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                TweetData.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                List<TwitterDataScrapper.StructTweetIDs> data = new List<TwitterDataScrapper.StructTweetIDs>();


                foreach (string itemKeyword in lstSearchByKeywords)
                {
                    if (!chkSearchByKeyWordByPeople.Checked)
                    {
                        data = TweetData.NewKeywordStructDataForSearchByKeyword(itemKeyword.Trim());
                        data = data.Distinct().ToList();
                    }
                    else
                    {
                        data = TweetData.NewKeywordStructDataSearchByPeople(itemKeyword.Trim());
                    } 
                }

                //TweetData.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
                TweetData.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
                data = DistinctDataList(data);

                if (!(data.Count() > 0))
                {
                    //AddToScrapeLogs("Request Not Completed");
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Requesting For 100 User ids ]");
                    data = TweetData.NewKeywordStructData(txtScrapeKeyword.Text);
                }

               // AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + data.Count + " User ids Scraped ]");

                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Wait Till Data Is Retrieving ]");

                #region commentedRegion
                //if (!chkSearchByKeyWordByPeople.Checked)
                //{
                //    if (!File.Exists(Globals.Path_KeywordScrapedListData + "-" + txtScrapeKeyword.Text + ".csv"))
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO , LOCATION , WEBSITE , NO OF TWEETS , FOLLOWERS , FOLLOWINGS", Globals.Path_KeywordScrapedListData + "-" + txtScrapeKeyword.Text + ".csv");
                //    }

                //    foreach (TwitterDataScrapper.StructTweetIDs item in data)
                //    {
                //        string ProfileName = string.Empty;
                //        string Location = string.Empty;
                //        string Bio = string.Empty;
                //        string website = string.Empty;
                //        string NoOfTweets = string.Empty;
                //        string Followers = string.Empty;
                //        string Followings = string.Empty;

                //        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                //        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                //        string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + item.username__Tweet_User), "", "");

                //        string Responce = ProfilePageSource;

                //        #region Convert HTML to XML

                //        string xHtml = objChilkat.ConvertHtmlToXml(Responce);
                //        Chilkat.Xml xml = new Chilkat.Xml();
                //        xml.LoadXml(xHtml);

                //        Chilkat.Xml xNode = default(Chilkat.Xml);
                //        Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                //        #endregion

                //        int counterdata = 0;
                //        xBeginSearchAfter = null;
                //        string dataDescription = string.Empty;
                //        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                //        xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                //        while ((xNode != null))
                //        {
                //            xBeginSearchAfter = xNode;
                //            if (counterdata == 0)
                //            {
                //                ProfileName = xNode.AccumulateTagContent("text", "script|style");
                //                counterdata++;
                //            }
                //            else if (counterdata == 1)
                //            {
                //                website = xNode.AccumulateTagContent("text", "script|style");
                //                if (website.Contains("Twitter Status"))
                //                {
                //                    website = "N/A";
                //                }
                //                counterdata++;
                //            }
                //            else
                //            {
                //                break;
                //            }
                //            //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                //            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "u-textUserColor");
                //        }

                //        xBeginSearchAfter = null;
                //        dataDescription = string.Empty;
                //        xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "ProfileHeaderCard-bio u-dir");//bio profile-field");
                //        while ((xNode != null))
                //        {
                //            xBeginSearchAfter = xNode;
                //            Bio = xNode.AccumulateTagContent("text", "script|style").Replace("&#39;", "'").Replace("&#13;&#10;", string.Empty).Trim();
                //            break;
                //        }

                //        xBeginSearchAfter = null;
                //        dataDescription = string.Empty;
                //        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "location profile-field");
                //        xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//location profile-field");
                //        while ((xNode != null))
                //        {
                //            xBeginSearchAfter = xNode;
                //            Location = xNode.AccumulateTagContent("text", "script|style");
                //            break;
                //        }

                //        int counterData = 0;
                //        xBeginSearchAfter = null;
                //        dataDescription = string.Empty;
                //        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "data-element-term", "tweet_stats");
                //        xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");
                //        while ((xNode != null))
                //        {
                //            xBeginSearchAfter = xNode;
                //            if (counterData == 0)
                //            {
                //                NoOfTweets = xNode.AccumulateTagContent("text", "script|style").Replace("Tweets", string.Empty).Replace(",", string.Empty).Replace("Tweet", string.Empty);
                //                counterData++;
                //            }
                //            else if (counterData == 1)
                //            {
                //                Followings = xNode.AccumulateTagContent("text", "script|style").Replace(" Following", string.Empty).Replace(",", string.Empty).Replace("Following", string.Empty);
                //                counterData++;
                //            }
                //            else if (counterData == 2)
                //            {
                //                Followers = xNode.AccumulateTagContent("text", "script|style").Replace("Followers", string.Empty).Replace(",", string.Empty).Replace("Follower", string.Empty);
                //                counterData++;
                //            }
                //            else
                //            {
                //                break;
                //            }
                //            //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "js-nav");
                //            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-openSignupDialog js-nonNavigable u-textUserColor");
                //        }


                //        if (!string.IsNullOrEmpty(item.username__Tweet_User) && item.ID_Tweet_User != "null")
                //        {
                //            string Id_user = item.ID_Tweet_User.Replace("}]", string.Empty).Trim();
                //            Globals.lstScrapedUserIDs.Add(Id_user);
                //            GlobusFileHelper.AppendStringToTextfileNewLine(Id_user + "," + item.username__Tweet_User + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", ""), Globals.Path_KeywordScrapedListData + "-" + txtScrapeKeyword.Text + ".csv");
                //            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Id_user + "," + item.username__Tweet_User + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ]");
                //        }
                //    }

                //} 
                #endregion

                //AddToScrapeLogs("Retrieving data");
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Adding Data To DataBase ]");
                Globals.lstScrapedUserIDs = Globals.lstScrapedUserIDs.Distinct().ToList();


                thread_AddingKeywordScrape = new Thread(() =>
                {
                    foreach (TwitterDataScrapper.StructTweetIDs item in data)
                    {
                        if (!string.IsNullOrEmpty(item.username__Tweet_User) && item.ID_Tweet_User != "null")
                        {
                            //AddToScrapeLogs(item.ID_Tweet_User);
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            DataBase.InsertOrUpdateScrapeSetting(item.ID_Tweet_User, item.username__Tweet_User, item.ID_Tweet);
                        }
                    }

                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Exported location :- " + Globals.Path_KeywordScrapedList + " ]");
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToScrapeLogs("------------------------------------------------------------------------------------------------------------------------------------------");

                });

                thread_AddingKeywordScrape.Start();

                if (Globals.IsDirectedFromFollower)
                {
                    Thread.Sleep(1000);
                    Globals.IsDirectedFromFollower = false;
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + data.Count + " User ids Scraped and Added To Follow List ]");
                    Tb_AccountManager.Invoke(new MethodInvoker(delegate
                    {
                        Tb_AccountManager.SelectedIndex = 2;
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



        #region Tweet Creator

        List<string> list_TweetCreatorMessages = new List<string>();
        List<string> lst_TweetExtrcator = new List<string>();
        List<string> list_Spinned_TweetCreatorMessages = new List<string>();

        string path_TweetCreatorExportFile = string.Empty;

        private void btnTweetCreatorMessageFile_Click(object sender, EventArgs e)
        {
            try
            {
                Thread _threadTweetCreatorMessageFileUpload = new Thread(TweetCreatorMessageFileUpload);
                _threadTweetCreatorMessageFileUpload.SetApartmentState(ApartmentState.STA);
                _threadTweetCreatorMessageFileUpload.Start();
            }
            catch { }

        }

        private void TweetCreatorMessageFileUpload()
        {
            try
            {
                string path_TweetCreatorMessageFile = string.Empty;
                txtTweetCreatorMessageFile.Invoke(new MethodInvoker(delegate
                {
                    path_TweetCreatorMessageFile = GlobusFileHelper.LoadTextFileUsingOFD();

                    if (!string.IsNullOrEmpty(path_TweetCreatorMessageFile))
                    {
                        txtTweetCreatorMessageFile.Text = path_TweetCreatorMessageFile;
                        list_TweetCreatorMessages = GlobusFileHelper.ReadLargeFileForSpinnedMessage(path_TweetCreatorMessageFile);
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + list_TweetCreatorMessages.Count + " Tweets Loaded ]");
                        //list_TweetCreatorMessageFile = SpinnedListGenerator.GetSpinnedList(list_TweetCreatorMessageFile);
                    }
                }));
            }
            catch (Exception ex)
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
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Export File : " + path_TweetCreatorExportFile + " ]");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btntxtTweetCreatorExportFile_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btntxtTweetCreatorExportFile_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        int countlist_TweetCreatorMessages = 0;
        private void btnStart_TweetCreator_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(path_TweetCreatorExportFile) && list_TweetCreatorMessages.Count > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(txtTweetCreatorMessageFile.Text))
                        {
                            objclsSettingDB.InsertOrUpdateSetting("TweetCreator", "SpinnedMessageTextInput", StringEncoderDecoder.Encode(txtTweetCreatorMessageFile.Text));
                        }
                        if (!string.IsNullOrEmpty(txtTweetCreatorExportFile.Text))
                        {
                            objclsSettingDB.InsertOrUpdateSetting("TweetCreator", "SpinnedMessageTextOutPut", StringEncoderDecoder.Encode(txtTweetCreatorExportFile.Text));
                        }

                        ThreadPool.SetMaxThreads(5, 5);
                        new Thread(() =>
                        {
                            countlist_TweetCreatorMessages = list_TweetCreatorMessages.Count;
                            foreach (var item in list_TweetCreatorMessages)
                            {
                                ThreadPool.QueueUserWorkItem(new WaitCallback(GetStartSpinnedListItem), new object[] { item });
                                Thread.Sleep(500);
                            }
                        }).Start();


                        //list_Spinned_TweetCreatorMessages = SpinnedListGenerator.GetSpinnedList(list_TweetCreatorMessages, '|');

                        //foreach (string item in list_Spinned_TweetCreatorMessages)
                        //{
                        //    try
                        //    {
                        //        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Spinned Message : " + item + " ]");
                        //        GlobusFileHelper.AppendStringToTextfileNewLine(item, path_TweetCreatorExportFile);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + ex.Message + " ]");
                        //    }
                        //}
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile + " ]");
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStart_TweetCreator_Click() --> " + ex.Message, Globals.Path_ErrortweetCreator);
                    }
                }
                else
                {
                    MessageBox.Show("Please upload Tweet Messages File", "Error");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void GetStartSpinnedListItem(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;
                string item = paramsArray.GetValue(0).ToString();
                int count = 0;
                List<string> lstCheckDuplicate = new List<string>();
                if (item.Length > 150)
                {
                    while (true)
                    {
                        string spinnedItem = SpinnedListGenerator.spinLargeText(new Random(), item);

                        if (lstCheckDuplicate.Contains(spinnedItem))
                        {
                            continue;
                        }

                        count++;
                        lstCheckDuplicate.Add(spinnedItem);
                        lstCheckDuplicate = lstCheckDuplicate.Distinct().ToList();
                        if (string.IsNullOrEmpty(spinnedItem) || count > 10000)
                        {
                            break;
                        }
                        try
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Spinned Message : " + spinnedItem + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(spinnedItem, path_TweetCreatorExportFile);
                        }
                        catch (Exception ex)
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile + " ]");
                        }

                    }
                }
                else
                {
                    list_Spinned_TweetCreatorMessages = SpinnedListGenerator.GetSpinnedList(new List<string> { item });

                    foreach (string _item in list_Spinned_TweetCreatorMessages)
                    {
                        try
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Spinned Message : " + _item + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(_item, path_TweetCreatorExportFile);
                        }
                        catch (Exception ex)
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Generated " + list_Spinned_TweetCreatorMessages.Count + " Spinned Messages and Exported to : " + path_TweetCreatorExportFile + " ]");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnStart_TweetCreator_Click() --> " + ex.Message, Globals.Path_ErrortweetCreator);
            }

            finally
            {
                countlist_TweetCreatorMessages--;
                if (countlist_TweetCreatorMessages == 0)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Process completed. ]");
                }
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

        private void btnAssignIP_Click(object sender, EventArgs e)
        {
            UploadPrivateIP();
        }

        private void UploadPrivateIP()
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    //if (!string.IsNullOrEmpty(txtPvtIPFour.Text))
                    {
                        if (MessageBox.Show("Assign Private Proxies from Database???", "IP", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                List<string> lstProxies = IPFetcher.GetPrivateProxies();
                                if (lstProxies.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(txtAccountsPerIP.Text) && GlobusRegex.ValidateNumber(txtAccountsPerIP.Text))
                                    {
                                        accountsPerIP = int.Parse(txtAccountsPerIP.Text);
                                    }
                                    new Thread(() =>
                                    {
                                        IPFetcher.AssignProxiesToAccounts(lstProxies, accountsPerIP);//AssignProxiesToAccounts(lstProxies);
                                        ReloadAccountsFromDataBase();
                                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                                    }).Start();
                                }
                                else
                                {
                                    MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnAssignIP_Click() --> " + ex.Message, Globals.Path_IPSettingErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnAssignIP_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        else
                        {
                            #region commentedRegion
                            //using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                            //{
                            //    ofd.Filter = "Text Files (*.txt)|*.txt";
                            //    ofd.InitialDirectory = Application.StartupPath;
                            //    if (ofd.ShowDialog() == DialogResult.OK)
                            //    {
                            //        list_pvtIP = new List<string>();

                            //        list_pvtIP = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                            //        if (!string.IsNullOrEmpty(txtAccountsPerIP.Text) && GlobusRegex.ValidateNumber(txtAccountsPerIP.Text))
                            //        {
                            //            accountsPerIP = int.Parse(txtAccountsPerIP.Text);
                            //        }
                            //        IPFetcher.AssignProxiesToAccounts(list_pvtIP, accountsPerIP);//AssignProxiesToAccounts(lstProxies);
                            //        ReloadAccountsFromDataBase();
                            //        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                            //    }
                            //} 
                            #endregion
                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("Please Select IP File To Assign IP");
                    //    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Please Select IP File To Assign IP ]");
                    //}
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
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
                            facebooker.Screen_name = dRow[2].ToString();
                            facebooker.FollowerCount = dRow[3].ToString();
                            facebooker.IPAddress = dRow[5].ToString();
                            facebooker.IPPort = dRow[6].ToString();
                            facebooker.IPUsername = dRow[7].ToString();
                            facebooker.IPpassword = dRow[8].ToString();
                            if (!string.IsNullOrEmpty(dRow[10].ToString()))
                            {
                                facebooker.profileStatus = int.Parse(dRow[10].ToString());
                            }

                            if (!string.IsNullOrEmpty(facebooker.Username))
                            {
                                Globals.listAccounts.Add(facebooker.Username + ":" + facebooker.Password + ":" + facebooker.IPAddress + ":" + facebooker.IPPort + ":" + facebooker.IPUsername + ":" + facebooker.IPpassword);
                                TweetAccountContainer.dictionary_TweetAccount.Add(facebooker.Username, facebooker);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.StackTrace);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message, Globals.Path_IPSettingErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() -- Rows From DB --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                    }
                    Console.WriteLine(Globals.listAccounts.Count + " Accounts loaded");
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ " + Globals.listAccounts.Count + " Accounts loaded ]");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ReloadAccountsFromDataBase() --> " + ex.Message, Globals.Path_IPSettingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ReloadAccountsFromDataBase() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        /// <summary>
        /// Assigns "accountsPerIP" number of proxies to accounts in Database, only picks up only those accounts where IPAddress is Null or Empty
        /// </summary>
        private void AssignProxiesToAccounts()
        {


        }

        #region Public Data Gathering

        public List<string> GetFollowersUsingUserID(string userID)
        {
            List<string> list_Followers = new List<string>();

            TwitterDataScrapper followerScrapper = new TwitterDataScrapper();

            string returnstatus = string.Empty;
            list_Followers = followerScrapper.GetFollowers(userID, out returnstatus);
            //AddToLog_Follower(returnstatus);
            return list_Followers;
        }

        /// <summary>
        /// Get Followers ID from page source 
        /// when APi is not working ...
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="Ghelper"></param>
        /// <returns></returns>

        public List<string> GetFollowersUsingUserID1(string userID, GlobusHttpHelper Ghelper)
        {
            List<string> list_Followers = new List<string>();

            TwitterDataScrapper followerScrapper = new TwitterDataScrapper();

            string returnstatus = string.Empty;

            list_Followers = followerScrapper.GetFollowers_New(userID, out returnstatus, ref Ghelper);
            //list_Followers = followerScrapper.GetFollowers1(userID, out returnstatus, Ghelper);
            //AddToLog_Follower(returnstatus);GetFollowers_New
            return list_Followers;
        }


        public List<string> GetFollowingsUsingUserID(string userID)
        {
            List<string> list_Followings = new List<string>();

            TwitterDataScrapper followingScrapper = new TwitterDataScrapper();
            string returnstatus = string.Empty;
            list_Followings = followingScrapper.GetFollowings(userID, out returnstatus);
            //AddToLog_Follower(returnstatus);
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



        List<string> lstFakeEmailLastNames = new List<string>();

 

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

                case Module.AccountCreation:
                    return threadNaming_AccountCreation_;

                case Module.WhoToScrap:
                    return threadNaming_WhoToFollow_;
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
        string threadNaming_AccountCreation_ = "AccountCreator_";
        string threadNaming_WhoToFollow_ = "WhoToFollow_";
        #endregion

        #region Stop Wait and Reply

        public static Dictionary<string, Thread> dictionary_Threads = new Dictionary<string, Thread>();

        private void btnStopWaitReplyThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Wait and Reply Threads, it may take some time ]");
                StopThreads(strModule(Module.WaitAndReply));
            }).Start();
        }

        public void StopThreads(string module)
        {
            Dictionary<string, Thread> tempdictionary_Threads = new Dictionary<string, Thread>();
            //listTweetMessages.Clear();
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
                        if (threadName == module)
                        {
                            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Aborting : " + key + " ]");
                            Thread thread = item.Value;
                            int abortCounter = 0;
                            if (thread != null)
                            {
                                thread.Abort();
                                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Aborted : " + key + " ]");
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
                        AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Aborting : " + key + " ]");
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
                            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Aborted : " + key + " ]");
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
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping ProfileManager Threads, it may take some time ]");
                StopThreads(strModule(Module.ProfileManager));
            }).Start();
        }

        private void btnStop_FollowThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Follow Threads, it may take some time ]");
                StopThreads(strModule(Module.Follow));
            }).Start();
        }

        private void btnStop_TweetThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Tweet Threads, it may take some time ]");
                StopThreads(strModule(Module.Tweet));
            }).Start();

        }

        private void btnStop_RetweetReplyThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Retweet Threads, it may take some time ]");
                StopThreads(strModule(Module.Retweet));
            }).Start();

            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Reply Threads, it may take some time ]");
                StopThreads(strModule(Module.Reply));
            }).Start();

        }

        private void btnStop_UnFollowThreads_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Unfollow Threads, it may take some time ]");
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

        string exportPathUserNametoUserId = string.Empty;
        private void btnGetUserID_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtScrapeUpload.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("UserScrape", "ScrapeUserID", StringEncoderDecoder.Encode(txtUsernameToUserIDPath.Text));
                }
                lstScrapedUserID.Clear();
                lstUserNameID.Clear();
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtUsernameToUserIDPath.Text = ofd.FileName;

                        lstUserNameID = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        //AddToScrapeLogs("Scrapped Data will be exported to : " + exportPath);
                    }
                }
                exportPathUserNametoUserId = string.Empty;
                if (MessageBox.Show("Default Stored File In Location : " + Globals.Path_ScrapedUserID + " \n Choose New File!!!", "Choose Location", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        //ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            exportPathUserNametoUserId = ofd.FileName;
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Scrapped Data will be exported to : " + exportPathUserNametoUserId + " ]");
                        }
                    }
                }
                else
                {
                    exportPathUserNametoUserId = Globals.Path_ScrapedUserID;
                    if (!File.Exists(exportPathUserNametoUserId))
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("UserName,UserID", exportPathUserNametoUserId);
                    }
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ No username uploaded ]");
                }


                //AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Scraping Username to userid ]");

                //thread_UsernameToUserid = new Thread(() =>
                //{
                //    GetUserNameToID(exportPathUserNametoUserId);
                //}
                //);
                //thread_UsernameToUserid.Start();
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnGetUserID_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnGetUserID_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }


        private void btnGetUsernameTouserId_Click(object sender, EventArgs e)
        {
            try
            {
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Scraping Username to userid ]");
                thread_UsernameToUserid = new Thread(() =>
                    {
                        GetUserNameToID(exportPathUserNametoUserId);
                    }
                    );
                thread_UsernameToUserid.Start();
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnGetUserID_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnGetUserID_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }


        public void GetUserNameToID(string exportPath)
        {
            try
            {
                int counter = 0;
                TweetAccountManager TweetLogin = new TweetAccountManager();
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }
                    TweetLogin = new TweetAccountManager();
                    TweetLogin.Username = item.Key;
                    TweetLogin.Password = item.Value.Password;
                    TweetLogin.IPAddress = item.Value.IPAddress;
                    TweetLogin.IPPort = item.Value.IPPort;
                    TweetLogin.IPUsername = item.Value.IPUsername;
                    TweetLogin.IPpassword = item.Value.IPpassword;
                    TweetLogin.Login();
                    if (!TweetLogin.IsNotSuspended)
                    {
                        continue;
                    }
                    else
                    {
                        globusHttpHelper = TweetLogin.globusHttpHelper;
                        counter++;
                        break;
                    }
                }
                string ReturnStatus = string.Empty;

                if (!TweetLogin.IsNotSuspended)
                {
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Atleast One Working Account To Get Details ]");
                    return;
                }
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Starting Scraping For Userids ]");
                foreach (string Username in lstUserNameID)
                {
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Username : " + Username + " ]");
                    string returnStatus = string.Empty;
                    string UserId = TwitterDataScrapper.GetUsernameToUserID_New(Username, out returnStatus, ref globusHttpHelper);
                    if (returnStatus == "No Error" && !string.IsNullOrEmpty(UserId))
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Username + " >>> " + UserId + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + "," + UserId, exportPath);
                        lstScrapedUserID.Add(UserId);
                    }
                    else if (returnStatus == "Rate limit exceeded")
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Username + " >>> " + returnStatus + " ]");
                        break;
                    }
                    else if (returnStatus == "Sorry, that page does not exist")
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Username + " >>> " + returnStatus + " ]");
                    }
                    else if (returnStatus == "User has been suspended")
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Username + " >>> " + returnStatus + " ]");
                    }
                }
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + lstScrapedUserID.Count + " UserId Scraped ]");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserNameToID() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserNameToID() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        //bool IsUserScrapedDatalist = false;
        private void chkboxScrapedLst_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxScrapedLst.Checked)
            {
                IsStart_ScrapUserInFollower = true;
                //IsStart_ScrapUserInFollower = true;
                grpFollowerSheduling.Enabled = true;
                if (Globals.lstScrapedUserIDs.Count > 0)
                {
                //    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added " + Globals.lstScrapedUserIDs.Count + " User Ids ]");
                }
                else
                {
                    //AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Scrape Data ]");
                   // AddToLog_Follower("[ " + DateTime.Now + " ] => [ You Will Be Redirected To Scrape Users Tab ]");
                    Globals.IsDirectedFromFollower = true;
                    //IsStart_ScrapUserInFollower = false;
                    //IsStart_ScrapUserInFollower = false;
                    //Thread.Sleep(1000);
                    //Tb_AccountManager.SelectedIndex = 6;
                    //Tb_AccountManager.SelectedTab.Name = "tabScrape";
                }
            }
            else if (!chkboxScrapedLst.Checked)
            {
                grpFollowerSheduling.Enabled = false;
                IsStart_ScrapUserInFollower = false;
                IsStart_ScrapUserInFollower = false;
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
                grpFollowerFollowCount.Enabled = false;
            }
            else if (!chkUseDivide.Checked)
            {
                IsUsingDivideData = false;
                rdBtnDivideByGivenNo.Enabled = false;
                rdBtnDivideEqually.Enabled = false;
                txtScrapeNoOfUsers.Enabled = false;
                grpFollowerFollowCount.Enabled = true;
            }
        }

        private void btnScheduleForLater_Follower_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Please upload all Relevant Data used for running Follow Module. These Data will be used when scheduled task is run");
            if (dateTimePickerStartTime_Follow.Value < DateTime.Now)
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Time Already Over. Please Select Time After The Present Time ]");
                return;
            }
            if (dateTimePickerEndTime_Follow.Value < DateTime.Now && (dateTimePickerEndTime_Follow.Value - dateTimePickerStartTime_Follow.Value).TotalMinutes < 0)
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Time Already Over. Please Select Time After The Present Time ]");
                return;
            }

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
                    queryManager.InsertUpdateTBScheduler("", "Follow", dateTimePickerStartTime_Follow.Value.ToString(), IsScheduledDaily);

                    if (scheduler != null && scheduler.IsDisposed == false)
                    {
                        scheduler.LoadDataGrid();
                    }

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


                    GlobusFileHelper.WriteStringToTextfile(txtUserIDtoFollow.Text + "<:>" + txtPathUserIDs.Text + "<:>" + txtOtherfollow.Text + "<:>" + txtUserOtherNumber.Text + "<:>" + IsStart_ScrapUserInFollower + "<:>" + strChkFollowers + "<:>" + strChkFollowings + "<:>" + strchkUseDivide + "<:>" + strrdBtnDivideEqually + "<:>" + strrdBtnDivideByGivenNo + "<:>" + txtScrapeNoOfUsers.Text + "<:>" + txtFollowMinDelay.Text + "<:>" + txtFollowMaxDelay.Text + "<:>" + txtNoOfFollowThreads.Text, Globals.Path_FollowSettings);

                    MessageBox.Show("Task Scheduled");
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Task Scheduled ]");
                    ///Updates new Paths in tb_Setting
                    //if (!string.IsNullOrEmpty(txtNames.Text) && !string.IsNullOrEmpty(txtUsernames.Text) && !string.IsNullOrEmpty(txtEmails.Text))
                    //{
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Name", StringEncoderDecoder.Encode(txtNames.Text));
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Username", StringEncoderDecoder.Encode(txtUsernames.Text));
                    //    objclsSettingDB.InsertOrUpdateSetting("Follow", "Email", StringEncoderDecoder.Encode(txtEmails.Text));
                    //}
                }
                catch (Exception ex)
                {
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Error in Task Scheduling : " + ex.Message + " ]");
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

            if (dateTimePicker_tweeterStart.Value < DateTime.Now)
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Time Already Over. Please Select Time After The Present Time ]");
                return;
            }
            if (dateTimePicker_TwetterEnd.Value < DateTime.Now && (dateTimePicker_TwetterEnd.Value - dateTimePicker_tweeterStart.Value).TotalMinutes < 0)
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Time Already Over. Please Select Time After The Present Time ]");
                return;
            }
            try
            {
                if (CheckTweeting())
                {
                    string IsScheduledDaily = "0";
                    if (chkboxTweetScheduledDaily.Checked)
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

                    // GlobusFileHelper.WriteStringToTextfile(txtTweetMessageFile.Text, Globals.Path_TweetSettings);

                    MessageBox.Show("Task Scheduled");
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Task Scheduled ]");
                    IsTweetScheduled = true;
                }
                else
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Add All Data ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScheduleForLater_Tweeter_Click --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ FollowSettings File : " + Globals.Path_FollowSettings + " isn't in the correct format ]");
            }
            //}
        }

        public void ReadTweetSettings()
        {
            //string[] settingData = ReadSettingsTextFile(Globals.Path_TweetSettings);

            try
            {
                //LoadSetting_Tweet(settingData);
                LoadSetting_Tweet();
            }
            catch (Exception ex)
            {
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ TweetSetting File : " + Globals.Path_TweetSettings + " isn't in the correct format ]");
            }

        }

        //public void LoadSetting_Tweet(string[] SettingData)
        public void LoadSetting_Tweet()
        {
            ReloadAccountsFromDataBase();
            //listTweetMessages = GlobusFileHelper.ReadFiletoStringList(SettingData[0]);
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


                    if (IsMentionUserWithScrapedList)
                    {
                        if (!string.IsNullOrEmpty(txtTweetScrapUserData.Text.Trim()))
                        {
                            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                            {
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Starting Scraping User ]");


                                threadStartScrapeInTweetMentionUser();

                                if (lst_mentionUser.Count == 0)
                                {
                                    return;
                                }

                            }
                            else
                            {
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Twitter Accounts To Start Scraping ]");
                                return;
                            }
                        }
                        else
                        {
                            AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Enter User Name to Scrap User. ]");
                            return;
                        }


                    }



                    #region mention user
                    if (Ismentionsingleuser && lst_mentionUser.Count != 0)
                    {
                        int countNoofMentionUser = 1;
                        if (NumberHelper.ValidateNumber(txtNumberOfIPThreads.Text))
                        {
                            countNoofMentionUser = Convert.ToInt32(txtTweetMentionNoOfUser.Text.Trim());
                            if (countNoofMentionUser <= 0)
                            {
                                MessageBox.Show("Please enter correct value in Mention NO of User.");
                                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please enter correct value in Mention NO of User.]");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please enter proper value in Mention NO of User.");
                            AddToLog_Tweet("Please enter proper value in Mention NO of User.");
                            return;
                        }

                        List<string> TempMsgList = new List<string>();
                        TempMsgList.Clear();
                        bool _RepeatusernameForAllMsg = false;
                        try
                        {
                            if (lst_mentionUser.Count < listTweetMessages.Count)
                            {
                                DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    _RepeatusernameForAllMsg = true;
                                }
                                else
                                {
                                }
                            }
                        }
                        catch { };


                        //
                        int UsernameCounter = 0;
                        int MsgListCounter = 0;
                        int CatchCounter = 0;


                        while (true)
                        {
                            string NewMSg = string.Empty;
                            int i = 1;
                            try
                            {
                                while (i <= countNoofMentionUser)
                                {
                                    NewMSg += "@" + lst_mentionUser[UsernameCounter] + " ";
                                    UsernameCounter++;
                                    i++;
                                }
                                NewMSg = NewMSg + listTweetMessages[MsgListCounter];
                                TempMsgList.Add(NewMSg);
                                //UsernameCounter++;
                                MsgListCounter++;
                            }
                            catch
                            {
                                if (_RepeatusernameForAllMsg)
                                {
                                    UsernameCounter = 0;
                                }
                                if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                {
                                    break;
                                }
                                CatchCounter++;
                            };

                            if (MsgListCounter >= listTweetMessages.Count)
                            {
                                break;
                            }
                        }

                        // Check if Mention adding is Succesfull.
                        if (TempMsgList.Count > 0)
                        {
                            listTweetMessages.Clear();
                            listTweetMessages.AddRange(TempMsgList);
                        }
                        else
                        {
                            MessageBox.Show("mention Message creation is Failure.");
                        }
                    }


                    if (Ismentionrendomuser && lst_mentionUser.Count != 0)
                    {
                        List<string> TempMsgList = new List<string>();
                        int MsgListCounter = 0;
                        int CatchCounter = 0;
                        while (true)
                        {
                            try
                            {
                                Random rnd = new Random();
                                int Rno = rnd.Next(lst_mentionUser.Count);
                                string randomuser = lst_mentionUser[Rno];
                                string NewMSg = "@" + randomuser + " " + listTweetMessages[MsgListCounter];
                                TempMsgList.Add(NewMSg);
                                MsgListCounter++;
                            }
                            catch
                            {
                                CatchCounter++;
                                if (CatchCounter == 3)
                                {
                                    break;
                                }
                            };

                            if (MsgListCounter >= listTweetMessages.Count)
                            {
                                break;
                            }
                        }

                        // Check if Mention adding is Succesfull.
                        if (TempMsgList.Count > 0)
                        {
                            listTweetMessages.Clear();
                            listTweetMessages.AddRange(TempMsgList);
                        }
                        else
                        {
                            MessageBox.Show("mention Message creation is Failure.");
                        }
                    }

                    else
                    {
                        #region mention user From ScrapedData
                        if (IsMentionUserWithScrapedList && lst_mentionUser.Count != 0)
                        {
                            int countNoofMentionUser = 1;
                            if (NumberHelper.ValidateNumber(txtTweetScrapMentionUser.Text.Trim()))
                            {
                                countNoofMentionUser = Convert.ToInt32(txtTweetScrapMentionUser.Text.Trim());
                                if (countNoofMentionUser <= 0)
                                {
                                    MessageBox.Show("Please enter correct value in Mention NO of User.");
                                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please enter correct value in Mention NO of User.]");
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please enter proper value in Mention NO of User.");
                                AddToLog_Tweet("Please enter proper value in Mention NO of User.");
                                return;
                            }

                            List<string> TempMsgList = new List<string>();
                            TempMsgList.Clear();
                            bool _RepeatusernameForAllMsg = false;
                            try
                            {
                                if (lst_mentionUser.Count < listTweetMessages.Count)
                                {
                                    DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        _RepeatusernameForAllMsg = true;
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                            catch { };


                            //
                            int UsernameCounter = 0;
                            int MsgListCounter = 0;
                            int CatchCounter = 0;


                            while (true)
                            {
                                string NewMSg = string.Empty;
                                int i = 1;
                                try
                                {
                                    while (i <= countNoofMentionUser)
                                    {
                                        NewMSg += "@" + lst_mentionUser[UsernameCounter] + " ";
                                        UsernameCounter++;
                                        i++;
                                    }
                                    NewMSg = NewMSg + listTweetMessages[MsgListCounter];
                                    TempMsgList.Add(NewMSg);
                                    //UsernameCounter++;
                                    MsgListCounter++;
                                }
                                catch
                                {
                                    if (_RepeatusernameForAllMsg)
                                    {
                                        UsernameCounter = 0;
                                    }
                                    if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                    {
                                        break;
                                    }
                                    CatchCounter++;
                                };

                                if (MsgListCounter >= listTweetMessages.Count)
                                {
                                    break;
                                }
                            }

                            // Check if Mention adding is Succesfull.
                            if (TempMsgList.Count > 0)
                            {
                                listTweetMessages.Clear();
                                listTweetMessages.AddRange(TempMsgList);
                            }
                            else
                            {
                                MessageBox.Show("mention Message creation is Failure.");
                            }
                        }


                        #endregion
                    }
                    #endregion

                    TweetAccountManager.que_TweetMessages.Clear();
                    foreach (string item in listTweetMessages)
                    {
                        
                        TweetAccountManager.que_TweetMessages.Enqueue(item);
                    }


                    Globals.TweetRunningText = "TweetModule";
                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        try
                        {
                            string tweetMessage = string.Empty;

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
                    MessageBox.Show("Please Upload Twitter Account");
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                }
            }
            catch { }
        }

        #region New Updated Code StartFollowing By Sonu

        private void StartFollowing()
        {
            try
            {
                List<string> RemoveList = new List<string>();
                List<List<string>> list_listTargetURLs = new List<List<string>>();
                string pathUserIds = ""; ;
                try
                {
                    if (string.IsNullOrEmpty(txtUserIDtoFollow.Text) && !string.IsNullOrEmpty(txtPathUserIDs.Text))
                    {
                        pathUserIds = txtPathUserIDs.Text;
                    }
                }
                catch { };


                try
                {
                    int numberOfThreads = 7;
                    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                    {
                        int count_AccountsUsed = 0;

                        int index = 0;

                        #region Other User No
                        //bool OtherUser = false;
                        if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
                        {
                            if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
                            {
                                TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
                                //OtherUser = true;
                            }
                            else
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number ]");
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
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Default Value 80 set in FollowingsFollowers Ratio ]");
                                    //return;
                                }
                            }
                        }
                        #endregion

                        #region Not Tweeted For #  no of Days
                        //if (chkDontFollowUsersWhoHavntTweetedForLong.Checked)
                        //{
                        //    TweetAccountManager.UseDateLastTweeted = true;
                        //    if (!string.IsNullOrEmpty(txtLastTweetDays.Text))
                        //    {
                        //        if (NumberHelper.ValidateNumber(txtLastTweetDays.Text))
                        //        {
                        //            TweetAccountManager.LastTweetDays = Convert.ToInt32(txtLastTweetDays.Text);
                        //        }
                        //        else
                        //        {
                        //            AddToLog_Follower("Default Days 50 set in Required Last Tweet Days");
                        //            //return;
                        //        }
                        //    }
                        //}
                        //if (chkDontFollowUsersThatUnfollowedBefore.Checked)
                        //{
                        //    TweetAccountManager.UseUnfollowedBeforeFilter = true;
                        //}
                        #endregion

                        #region Option For screen Name
                        if ((IsFollowerScreenName != true && IsfollowerUserId != true) || (IsFollowerScreenName != false && IsfollowerUserId != false))
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Select any one option From Screen Name Or User Id. ]");
                            MessageBox.Show("Please Select any one option From Screen Name Or User Id.");
                            return;
                        }
                        #endregion

                        #region Get follower  Follow  Id
                        //when we fill any usrname in test box 
                        //and follow of these user followes and friends ..

                        if (!string.IsNullOrEmpty(txtOtherfollow.Text))
                        {
                            listUserIDs.Clear();
                            if (chkFollowers.Checked)
                            {
                                threadStartScrape();
                            }
                            if (chkFollowings.Checked)
                            {
                                listUserIDs.AddRange(GetFollowersUsingUserID(txtOtherfollow.Text));
                            }

                            if (listUserIDs.Count == 0)
                            {
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ No Ids in the Scraped List ]");
                                return;
                            }
                        }
                        #endregion

                        #region GetUsernameOrUserID
                        //foreach (string user_id_toFollow in listUserIDs)
                        //{
                        //    string user_id = string.Empty;
                        //    string Username = string.Empty;
                        //    string Screen_name = string.Empty;
                        //    clsDBQueryManager DB = new clsDBQueryManager();
                        //    //if (!GlobusRegex.ValidateNumber(user_id_toFollow))//(!IsItNumber(user_id_toFollow))
                        //    if (IsFollowerScreanName && !FollowUsrnameFollowerAndFriends)
                        //    {
                        //        string returnStatus = string.Empty;
                        //        user_id = TwitterDataScrapper.GetUserIDFromUsername(user_id_toFollow, out returnStatus);
                        //        if (returnStatus == "No Error" && !string.IsNullOrEmpty(user_id))
                        //        {
                        //            AddToLog_Follower(user_id_toFollow + " >>> " + user_id);
                        //        }
                        //        else if (returnStatus == "Rate limit exceeded")
                        //        {
                        //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
                        //        }
                        //        else if (returnStatus == "Sorry, that page does not exist")
                        //        {
                        //            RemoveList.Add(user_id_toFollow);
                        //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
                        //        }
                        //        else if (returnStatus == "User has been suspended")
                        //        {
                        //            RemoveList.Add(user_id_toFollow);
                        //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
                        //        }

                        //        if (!string.IsNullOrEmpty(user_id))
                        //        {
                        //            if (!NumberHelper.ValidateNumber(user_id))
                        //            {
                        //                DB.InsertUserNameId(user_id, user_id_toFollow);
                        //            }
                        //            else
                        //            {
                        //                DB.InsertUserNameId(user_id_toFollow, user_id);
                        //            }
                        //        }
                        //    }
                        //    //else
                        //    else if (IsfollowerUserId || FollowUsrnameFollowerAndFriends)
                        //    {
                        //        user_id = user_id_toFollow;
                        //        Screen_name = TwitterDataScrapper.GetUserNameFromUserId(user_id);
                        //        if (Screen_name == "Rate Limit Exceeded")
                        //        {
                        //            AddToLog_Follower(user_id_toFollow + " >>> " + Screen_name);
                        //        }
                        //        else
                        //        {
                        //            AddToLog_Follower(Screen_name + " >>> " + user_id);
                        //        }
                        //    }
                        //    else
                        //    {

                        //    }
                        //    //AddToLog_Follower("Sleep For 4 Seconds");

                        //    //if user is check fast follow option then delay is not working on that condition ...!!
                        //    if (!IsFastfollow)
                        //    {
                        //        Thread.Sleep(1500);
                        //    }
                        //}

                        //foreach (string lst in RemoveList)
                        //{
                        //    try
                        //    {
                        //        listUserIDs.Remove(lst);
                        //    }
                        //    catch (Exception ex)
                        //    {

                        //    }
                        //}

                        #endregion

                        #region User Without Picture
                        if (chkDontFollowUsersWithNoPicture.Checked)
                        {
                            if (rdBtnDivideEqually.Checked)
                            {
                                TweetAccountManager.noOFFollows = listUserIDs.Count();
                            }
                            TweetAccountManager.IscontainPicture = true;
                            AddToLog_Follower("Get Followes/Followings from targeted user.");
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

                            foreach (string tempdataitem in tempdata)
                            {
                                string containsIamge = TwitterDataScrapper.GetPhotoFromUsername_New(tempdataitem);
                                //Thread.Sleep(1000);
                                if (containsIamge == "true")
                                {
                                    AddToLog_Follower(tempdataitem + " Contains Image");
                                    listUserIDs.Add(tempdataitem);
                                    if ((TweetAccountManager.noOFFollows == listUserIDs.Count) && (!IsUsingDivideData))
                                    {
                                        break;
                                    }
                                }
                                else if (containsIamge == "false")
                                {
                                    AddToLog_Follower(tempdataitem + " Not Contains Image");
                                    ///Add in blacklist table
                                }
                                else if (containsIamge == "Rate limit exceeded")
                                {
                                    AddToLog_Follower("Cannot Make Request. Rate limit exceeded");
                                    AddToLog_Follower("Please Try After Some Time");
                                }

                                if (listUserIDs.Count == TweetAccountManager.noOFFollows)
                                {
                                    break;
                                }
                                //AddToLog_Follower("Sleep For 4 Seconds");
                                Thread.Sleep(1500);
                            }
                            AddToLog_Follower(listUserIDs.Count + " Users Contain Profile Image");
                        }




                        #endregion

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
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number ]");
                                return;
                            }
                        }

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
                            TweetAccountManager.noOFFollows = splitNo;
                        }

                        if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
                        {
                            numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
                        }

                        ThreadPool.SetMaxThreads(numberOfThreads, 5);



                        #endregion

                        #region followUserWithmanyLinks
                        if (chkDontFollowUsersWithManyLinks.Checked)
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Checking For Links in Tweets ]");
                            if (!string.IsNullOrEmpty(txtNoOfLinks.Text) && NumberHelper.ValidateNumber(txtNoOfLinks.Text))
                            {
                                TwitterDataScrapper.Percentage = Convert.ToInt32(txtNoOfLinks.Text);
                            }
                            else
                            {
                                TwitterDataScrapper.Percentage = 40;
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Setting Default Percentage : 40% ]");
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
                            foreach (string tempdataitem in tempdata)
                            {
                                bool toomanyLinks = TwitterDataScrapper.GetStatusLinks(tempdataitem);
                                if (toomanyLinks)
                                {
                                    listUserIDs.Add(tempdataitem);
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added User id : " + tempdataitem + " To Follow List ]");
                                }
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
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number In Maximum follow per Id ]");
                                    return;
                                }
                            }
                        }
                        #endregion

                        Globals.FollowerRunningText = "FollowerModule";
                        counter_AccountFollwer = TweetAccountContainer.dictionary_TweetAccount.Count;
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

                                ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs, OtherUser, followMinDelay, followMaxDelay, pathUserIds });

                                //if user is check fast follow option then delay is not working on that condition ...!!
                                if (!IsFastfollow)
                                {
                                    Thread.Sleep(1000);
                                }
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
                        AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                    }
                }
                catch (Exception ex)
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

        #endregion



        #region previous method of StartFollowing
        //private void StartFollowing()
        //{
        //    try
        //    {
        //        List<string> RemoveList = new List<string>();
        //        List<List<string>> list_listTargetURLs = new List<List<string>>();

        //        try
        //        {
        //            int numberOfThreads = 7;
        //            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
        //            {
        //                int count_AccountsUsed = 0;

        //                int index = 0;

        //                #region Other User No
        //                //bool OtherUser = false;
        //                if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
        //                {
        //                    if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
        //                    {
        //                        TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
        //                        //OtherUser = true;
        //                    }
        //                    else
        //                    {
        //                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number ]");
        //                        return;
        //                    }
        //                }
        //                #endregion

        //                #region Follower Following ratio
        //                if (chkDontFollowUsersWithFollowingsFollowersRatio.Checked)
        //                {
        //                    TweetAccountManager.UseRatioFilter = true;
        //                    if (!string.IsNullOrEmpty(txtFollowingsFollowersRatio.Text))
        //                    {
        //                        if (NumberHelper.ValidateNumber(txtFollowingsFollowersRatio.Text))
        //                        {
        //                            TweetAccountManager.FollowingsFollowersRatio = Convert.ToInt32(txtFollowingsFollowersRatio.Text);
        //                        }
        //                        else
        //                        {
        //                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Default Value 80 set in FollowingsFollowers Ratio ]");
        //                            //return;
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Not Tweeted For #  no of Days
        //                //if (chkDontFollowUsersWhoHavntTweetedForLong.Checked)
        //                //{
        //                //    TweetAccountManager.UseDateLastTweeted = true;
        //                //    if (!string.IsNullOrEmpty(txtLastTweetDays.Text))
        //                //    {
        //                //        if (NumberHelper.ValidateNumber(txtLastTweetDays.Text))
        //                //        {
        //                //            TweetAccountManager.LastTweetDays = Convert.ToInt32(txtLastTweetDays.Text);
        //                //        }
        //                //        else
        //                //        {
        //                //            AddToLog_Follower("Default Days 50 set in Required Last Tweet Days");
        //                //            //return;
        //                //        }
        //                //    }
        //                //}
        //                if (chkDontFollowUsersThatUnfollowedBefore.Checked)
        //                {
        //                    TweetAccountManager.UseUnfollowedBeforeFilter = true;
        //                }
        //                #endregion

        //                #region Option For screen Name
        //                if ((IsFollowerScreenName != true && IsfollowerUserId != true) || (IsFollowerScreenName != false && IsfollowerUserId != false))
        //                {
        //                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Select any one option From Screen Name Or User Id. ]");
        //                    MessageBox.Show("Please Select any one option From Screen Name Or User Id.");
        //                    return;
        //                }
        //                #endregion

        //                #region Get follower  Follow  Id
        //                //when we fill any usrname in test box 
        //                //and follow of these user followes and friends ..

        //                if (!string.IsNullOrEmpty(txtOtherfollow.Text))
        //                {
        //                    listUserIDs.Clear();
        //                    if (chkFollowers.Checked)
        //                    {
        //                        threadStartScrape();
        //                    }
        //                    if (chkFollowings.Checked)
        //                    {
        //                        listUserIDs.AddRange(GetFollowersUsingUserID(txtOtherfollow.Text));
        //                    }

        //                    if (listUserIDs.Count == 0)
        //                    {
        //                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ No Ids in the Scraped List ]");
        //                        return;
        //                    }
        //                }
        //                #endregion

        //                #region GetUsernameOrUserID
        //                //foreach (string user_id_toFollow in listUserIDs)
        //                //{
        //                //    string user_id = string.Empty;
        //                //    string Username = string.Empty;
        //                //    string Screen_name = string.Empty;
        //                //    clsDBQueryManager DB = new clsDBQueryManager();
        //                //    //if (!GlobusRegex.ValidateNumber(user_id_toFollow))//(!IsItNumber(user_id_toFollow))
        //                //    if (IsFollowerScreanName && !FollowUsrnameFollowerAndFriends)
        //                //    {
        //                //        string returnStatus = string.Empty;
        //                //        user_id = TwitterDataScrapper.GetUserIDFromUsername(user_id_toFollow, out returnStatus);
        //                //        if (returnStatus == "No Error" && !string.IsNullOrEmpty(user_id))
        //                //        {
        //                //            AddToLog_Follower(user_id_toFollow + " >>> " + user_id);
        //                //        }
        //                //        else if (returnStatus == "Rate limit exceeded")
        //                //        {
        //                //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
        //                //        }
        //                //        else if (returnStatus == "Sorry, that page does not exist")
        //                //        {
        //                //            RemoveList.Add(user_id_toFollow);
        //                //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
        //                //        }
        //                //        else if (returnStatus == "User has been suspended")
        //                //        {
        //                //            RemoveList.Add(user_id_toFollow);
        //                //            AddToLog_Follower(user_id_toFollow + " >>> " + returnStatus);
        //                //        }

        //                //        if (!string.IsNullOrEmpty(user_id))
        //                //        {
        //                //            if (!NumberHelper.ValidateNumber(user_id))
        //                //            {
        //                //                DB.InsertUserNameId(user_id, user_id_toFollow);
        //                //            }
        //                //            else
        //                //            {
        //                //                DB.InsertUserNameId(user_id_toFollow, user_id);
        //                //            }
        //                //        }
        //                //    }
        //                //    //else
        //                //    else if (IsfollowerUserId || FollowUsrnameFollowerAndFriends)
        //                //    {
        //                //        user_id = user_id_toFollow;
        //                //        Screen_name = TwitterDataScrapper.GetUserNameFromUserId(user_id);
        //                //        if (Screen_name == "Rate Limit Exceeded")
        //                //        {
        //                //            AddToLog_Follower(user_id_toFollow + " >>> " + Screen_name);
        //                //        }
        //                //        else
        //                //        {
        //                //            AddToLog_Follower(Screen_name + " >>> " + user_id);
        //                //        }
        //                //    }
        //                //    else
        //                //    {

        //                //    }
        //                //    //AddToLog_Follower("Sleep For 4 Seconds");

        //                //    //if user is check fast follow option then delay is not working on that condition ...!!
        //                //    if (!IsFastfollow)
        //                //    {
        //                //        Thread.Sleep(1500);
        //                //    }
        //                //}

        //                //foreach (string lst in RemoveList)
        //                //{
        //                //    try
        //                //    {
        //                //        listUserIDs.Remove(lst);
        //                //    }
        //                //    catch (Exception ex)
        //                //    {

        //                //    }
        //                //}

        //                #endregion

        //                #region User Without Picture
        //                if (chkDontFollowUsersWithNoPicture.Checked)
        //                {
        //                    TweetAccountManager.IscontainPicture = true;
        //                    //AddToLog_Follower("Get Followes/Followings from targeted user.");
        //                    //List<string> tempdata = new List<string>();
        //                    //try
        //                    //{
        //                    //    foreach (string newitem in listUserIDs)
        //                    //    {
        //                    //        tempdata.Add(newitem);
        //                    //    }
        //                    //}
        //                    //catch (Exception ex)
        //                    //{
        //                    //    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                    //    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                    //}
        //                    //listUserIDs.Clear();

        //                    //AddToLog_Follower("Checking Profile Image in Extracted User id's");

        //                    //foreach (string tempdataitem in tempdata)
        //                    //{
        //                    //    string containsIamge = TwitterDataScrapper.GetPhotoFromUsername_New(tempdataitem);
        //                    //    //Thread.Sleep(1000);
        //                    //    if (containsIamge == "true")
        //                    //    {
        //                    //        AddToLog_Follower(tempdataitem + " Contains Image");
        //                    //        listUserIDs.Add(tempdataitem);
        //                    //        if ((TweetAccountManager.noOFFollows == listUserIDs.Count) && (!IsUsingDivideData))
        //                    //        {
        //                    //            break;
        //                    //        }
        //                    //    }
        //                    //    else if (containsIamge == "false")
        //                    //    {
        //                    //        AddToLog_Follower(tempdataitem + " Not Contains Image");
        //                    //        ///Add in blacklist table
        //                    //    }
        //                    //    else if (containsIamge == "Rate limit exceeded")
        //                    //    {
        //                    //        AddToLog_Follower("Cannot Make Request. Rate limit exceeded");
        //                    //        AddToLog_Follower("Please Try After Some Time");
        //                    //    }

        //                    //    if (listUserIDs.Count == TweetAccountManager.noOFFollows)
        //                    //    {
        //                    //        break;
        //                    //    }
        //                    //    //AddToLog_Follower("Sleep For 4 Seconds");
        //                    //    Thread.Sleep(1500);
        //                    //}
        //                    //AddToLog_Follower(listUserIDs.Count + " Users Contain Profile Image");
        //                }




        //                #endregion

        //                bool OtherUser = false;
        //                if (!string.IsNullOrEmpty(txtUserOtherNumber.Text))
        //                {
        //                    if (NumberHelper.ValidateNumber(txtUserOtherNumber.Text))
        //                    {
        //                        TweetAccountManager.noOFFollows = Convert.ToInt32(txtUserOtherNumber.Text);
        //                        OtherUser = true;
        //                        //txtUserOtherNumber.Text = "";
        //                    }
        //                    else
        //                    {
        //                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number ]");
        //                        return;
        //                    }
        //                }

        //                #region User Divude Checked
        //                if (chkUseDivide.Checked || IsUsingDivideData)
        //                {
        //                    int splitNo = 0;
        //                    if (rdBtnDivideEqually.Checked)
        //                    {
        //                        splitNo = listUserIDs.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
        //                    }
        //                    else if (rdBtnDivideByGivenNo.Checked)
        //                    {
        //                        if (!string.IsNullOrEmpty(txtScrapeNoOfUsers.Text) && NumberHelper.ValidateNumber(txtScrapeNoOfUsers.Text))
        //                        {
        //                            int res = Convert.ToInt32(txtScrapeNoOfUsers.Text);
        //                            splitNo = res;//listUserIDs.Count / res;
        //                        }
        //                    }
        //                    if (splitNo == 0)
        //                    {
        //                        splitNo = RandomNumberGenerator.GenerateRandom(0, listUserIDs.Count - 1);
        //                    }
        //                    list_listTargetURLs = Split(listUserIDs, splitNo);
        //                    TweetAccountManager.noOFFollows = splitNo;
        //                }

        //                if (!string.IsNullOrEmpty(txtNoOfFollowThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfFollowThreads.Text))
        //                {
        //                    numberOfThreads = int.Parse(txtNoOfFollowThreads.Text);
        //                }

        //                ThreadPool.SetMaxThreads(numberOfThreads, 5);



        //                #endregion

        //                #region followUserWithmanyLinks
        //                if (chkDontFollowUsersWithManyLinks.Checked)
        //                {
        //                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Checking For Links in Tweets ]");
        //                    if (!string.IsNullOrEmpty(txtNoOfLinks.Text) && NumberHelper.ValidateNumber(txtNoOfLinks.Text))
        //                    {
        //                        TwitterDataScrapper.Percentage = Convert.ToInt32(txtNoOfLinks.Text);
        //                    }
        //                    else
        //                    {
        //                        TwitterDataScrapper.Percentage = 40;
        //                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Setting Default Percentage : 40% ]");
        //                    }
        //                    List<string> tempdata = new List<string>();
        //                    try
        //                    {
        //                        foreach (string newitem in listUserIDs)
        //                        {
        //                            tempdata.Add(newitem);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                    }
        //                    listUserIDs.Clear();
        //                    foreach (string tempdataitem in tempdata)
        //                    {
        //                        bool toomanyLinks = TwitterDataScrapper.GetStatusLinks(tempdataitem);
        //                        if (toomanyLinks)
        //                        {
        //                            listUserIDs.Add(tempdataitem);
        //                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Added User id : " + tempdataitem + " To Follow List ]");
        //                        }
        //                    }
        //                }
        //                #endregion

        //                #region Follow Per Day
        //                //Code Added by Abhishek 

        //                TweetAccountManager.NoOfFollowPerDay_ChkBox = false;
        //                if (chkBox_NoOFfollow.Checked)
        //                {
        //                    if (!string.IsNullOrEmpty(txt_MaximumFollow.Text))
        //                    {
        //                        if (NumberHelper.ValidateNumber(txt_MaximumFollow.Text))
        //                        {
        //                            TweetAccountManager.NoOfFollowPerDay = Convert.ToInt32(txt_MaximumFollow.Text);
        //                            TweetAccountManager.NoOfFollowPerDay_ChkBox = true;
        //                            //txtUserOtherNumber.Text = "";
        //                        }
        //                        else
        //                        {
        //                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Enter a Number In Maximum follow per Id ]");
        //                            return;
        //                        }
        //                    }
        //                }
        //                #endregion

        //                Globals.FollowerRunningText = "FollowerModule";
        //                counter_AccountFollwer = TweetAccountContainer.dictionary_TweetAccount.Count;
        //                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
        //                {
        //                    try
        //                    {
        //                        ThreadPool.SetMaxThreads(numberOfThreads, 5);

        //                        if (chkUseDivide.Checked || IsUsingDivideData)
        //                        {
        //                            listUserIDs = list_listTargetURLs[index];
        //                        }

        //                        if (GlobusRegex.ValidateNumber(txtFollowMinDelay.Text))
        //                        {
        //                            followMinDelay = Convert.ToInt32(txtFollowMinDelay.Text);
        //                        }
        //                        if (GlobusRegex.ValidateNumber(txtFollowMaxDelay.Text))
        //                        {
        //                            followMaxDelay = Convert.ToInt32(txtFollowMaxDelay.Text);
        //                        }

        //                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowingMultithreaded), new object[] { item, listUserIDs, OtherUser, followMinDelay, followMaxDelay });

        //                        //if user is check fast follow option then delay is not working on that condition ...!!
        //                        if (!IsFastfollow)
        //                        {
        //                            Thread.Sleep(1000);
        //                        }
        //                        count_AccountsUsed++;
        //                        index++;
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Please Upload Twitter Account");
        //                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_FollowerErroLog);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- foreach loop Foreach Dictiinoary --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() --  list_listTargetURLs --> " + ex.Message, Globals.Path_FollowerErroLog);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- list_listTargetURLs --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}
        #endregion

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
                            Globals.FollowerRunningText = "FollowerModule";
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
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                }
            }
            catch { }
            #endregion
        }

        private void StartFollowingMultithreaded(object parameters)
        {
            TweetAccountManager tweetAccountManager = new TweetAccountManager();
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

                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Starting Follower For Account : " + keyValue.Key + " ]");

                tweetAccountManager = keyValue.Value;
                TweetAccountManager.FileFollowUrlPath = txtPathUserIDs.Text.ToString();

                if (chkboxUseGroups.Checked)
                {
                    if (tweetAccountManager.GroupName != txtUseGroup.Text)
                    {
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + keyValue.Key + " Group Name Does Not Match ]");
                        return;
                    }
                }

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.Follow), tweetAccountManager.Username);
                tweetAccountManager.follower.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += logEvents_Follower_addToLogger;

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }

                if (tweetAccountManager.AccountStatus == "Account Suspended")
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(tweetAccountManager.Username);
                    return;
                }

                try
                {
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + list_userIDsToFollow.Count() + " user id in List ]");
                }
                catch (Exception)
                {
                }

                if (list_userIDsToFollow.Count > 0)
                {

                    tweetAccountManager.FollowUsingURLs(list_userIDsToFollow, intMinDelay, intMaxDelay, OtherUser);
                }
                else
                {
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ No ID's To Follow ]");
                }

                // tweetAccountManager.follower.logEvents.addToLogger -= logEvents_Follower_addToLogger;
                //tweetAccountManager.logEvents.addToLogger -= logEvents_Follower_addToLogger;
            }
            catch (Exception ex)
            {
                ErrorLogger.AddToErrorLogText(ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowingMultithreaded() --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            finally
            {
                counter_AccountFollwer--;
                //TweetAccountManager tweetAccountManager = new TweetAccountManager();
                tweetAccountManager.follower.logEvents.addToLogger -= new EventHandler(logEvents_Follower_addToLogger);
                tweetAccountManager.logEvents.addToLogger -= logEvents_Follower_addToLogger;

                if (counter_AccountFollwer == 0)
                {
                    if (btnStartFollowing.InvokeRequired)
                    {
                        btnStartFollowing.Invoke(new MethodInvoker(delegate
                        {
                            Globals.FollowerRunningText = string.Empty;
                            GlobusFileHelper.AppendStringToTextfileNewLine("Module Follow count Total: " + Globals.totalcountFollower , Globals.path_CountNoOfProcessDone);
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_Follower("---------------------------------------------------------------------------------------------------------------------------");

                        }));
                    }
                }
            }
        }

        frmScheduler scheduler = new frmScheduler();
        private void schedulerToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void btnExtractTweet_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (lstUserId_Tweet.Count > 0 || !string.IsNullOrEmpty(txtTweetName.Text.Replace(" ", "").Replace("\0", "")))
                {
                    if (!string.IsNullOrEmpty(txtExtractorFile.Text))
                    {
                        objclsSettingDB.InsertOrUpdateSetting("TweetCreator", "TweetExtractor", StringEncoderDecoder.Encode(txtExtractorFile.Text));
                    }
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
                    if (lstUserId_Tweet.Count <= 0 || string.IsNullOrEmpty(txtTweetName.Text.Replace(" ", "").Replace("\0", "")))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Please Enter a Username/Userid Name or File. ]");
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void TweetDataExtract()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTweetExtractCount.Text) && NumberHelper.ValidateNumber(txtTweetExtractCount.Text))
                {
                    TweetExtractCount = Convert.ToInt32(txtTweetExtractCount.Text);

                    if (TweetExtractCount > 2500)
                    {
                        TwitterDataScrapper.TweetExtractCount = 2100;
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting Default No Of Tweet : 2100 ]");
                    }
                    else
                    {
                        TwitterDataScrapper.TweetExtractCount = TweetExtractCount;
                    }
                }
                else
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Entered Count incorrect.Setting Default Count 10 ]");
                }

                //Remove Options of RT or @ mentions MSG's 

                if (chk_RTMSG.Checked)
                {
                    TwitterDataScrapper.RemoveRTMSg = true;
                }

                if (chk_RemoveAtMsg.Checked)
                {
                    TwitterDataScrapper.removeAtMentions = true;
                }

                int counter = 0;
                TwitterDataScrapper DataScraper = new TwitterDataScrapper();
                foreach (string item in lstUserId_Tweet)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting Tweets For " + item + " ]");
                    string ReturnStatus = string.Empty;
                    List<string> TweetData = new List<string>();

                    //if (TwitterDataScrapper.RemoveRTMSg == true)
                    //{
                    //    TweetData = DataScraper.GetOnlyTweetData_Scrape(item, TweetExtractCount, out ReturnStatus);
                    //}
                    //else
                    //{
                    TweetData = DataScraper.GetTweetData_Scrape(item, TweetExtractCount, out ReturnStatus);
                    //}
                    //List<TwitterDataScrapper.StructTweetIDs> tweetData = DataScraper.TweetExtractor_ByUserName_New(item);

                    if (ReturnStatus.Contains("No Error"))
                    {
                        if (TweetData.Count > 0)
                        {
                            foreach (string newItem in TweetData)
                            {
                                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + newItem + " ]");
                            }
                        }
                        else
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Sorry No Tweets For " + item + " ]");
                        }
                    }
                    else if (ReturnStatus.Contains("Rate limit exceeded"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Rate limit exceeded ]");
                        break;
                    }
                    else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>> Sorry, that page does not exist ]");
                    }
                    else if (ReturnStatus.Contains("Empty"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>>> Return Empty ]");
                    }
                    else
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>> Error in Request ]");
                    }
                }
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Finished Extracting Tweets ]");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Tweets Stored In -" + Globals.Path_TweetExtractor + " ]");
                AddToTweetCreatorLogs("-----------------------------------------------------------------------------------------------------------------------");

                //if (txtTweetName.InvokeRequired)
                //{
                //    txtTweetName.Invoke(new MethodInvoker(delegate
                //    {
                //        txtTweetName.Text = "";
                //    }));
                //}

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

                List<string> tempFile = GlobusFileHelper.ReadFiletoStringList(txtExtractorFile.Text);

                foreach (string item in tempFile)
                {
                    string tempData = item.Replace("\0", "").Replace(" ", "");
                    if (!string.IsNullOrEmpty(tempData))
                    {
                        lstUserId_Tweet.Add(item);
                    }
                }

                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [  " + lstUserId_Tweet.Count + " Usernames/Userid Uploaded ]");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void chkboxImportPublicIP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkboxImportPublicIP.Checked)
                {
                    if (chkboxUseUrlIP.Checked && !string.IsNullOrEmpty(txtIPUrl.Text))
                    {
                        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                        string pageSource = HttpHelper.getHtmlfromUrlIP(new Uri(txtIPUrl.Text), "", "", "", "", "");
                        string[] array = Regex.Split(pageSource, "\n");
                        foreach (string item in array)
                        {
                            lstPublicIPWOTest.Add(item.Replace("\r", ""));
                        }
                    }
                    else if (!string.IsNullOrEmpty(txtPublicIP.Text))
                    {
                        lstPublicIPWOTest = GlobusFileHelper.ReadFiletoStringList(txtPublicIP.Text);
                    }
                    else
                    {
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Please Upload Either Url or Load IP Files ]");
                        return;
                    }

                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + lstPublicIPWOTest.Count + " Public Proxies Loaded ]");
                    if (lstPublicIPWOTest.Count > 0)
                    {
                        new Thread(() =>
                        {
                            foreach (string item in lstPublicIPWOTest)
                            {
                                ImportingIP(item);
                            }
                        }
                        ).Start();
                    }
                    else
                    {
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Sorry No Proxies Available ]");
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        bool IsStopImportingIP = false;
        public void ImportingIP(string item)
        {
            try
            {
                if (IsStopImportingIP)
                {
                    return;
                }
                LstPublicIP.Add(Thread.CurrentThread);
                LstPublicIP = LstPublicIP.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch { }
            string IPAddress = string.Empty;
            string IPPort = string.Empty;
            string IPUsername = string.Empty;
            string IPpassword = string.Empty;
            int Working = 0;
            int IsPublic = 0;
            string LoggedInIp = string.Empty;

            string account = item;
            int DataCount = account.Split(':').Length;

            if (DataCount == 1)
            {
                IPAddress = account.Split(':')[0];
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP Not In correct Format ]");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + account + " ]");
                return;
            }
            if (DataCount == 2)
            {
                IPAddress = account.Split(':')[0];
                IPPort = account.Split(':')[1];
            }
            else if (DataCount > 2)
            {
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP Not In correct Format ]");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + account + " ]");
                return;
            }
            try
            {
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Added Proxies -> " + IPAddress + ":" + IPPort + " ]");
                string InsertQuery = "Insert into tb_IP values('" + IPAddress + "','" + IPPort + "','" + IPUsername + "','" + IPpassword + "', " + Working + "," + IsPublic + " , '" + LoggedInIp + "')";
                DataBaseHandler.InsertQuery(InsertQuery, "tb_IP");
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(IPAddress + ":" + IPPort, Globals.Path_ExsistingProxies);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error Importing Public IP W/o testing --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void txtAsssignPublicIP_Click(object sender, EventArgs e)
        {
            IPUpload();
        }

        private void IPUpload()
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (MessageBox.Show("Assign Public Proxies from Database???", "IP", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            List<string> lstProxies = IPFetcher.GetPublicProxies();
                            if (lstProxies.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(txtAccountsPerIP.Text) && GlobusRegex.ValidateNumber(txtAccountsPerIP.Text))
                                {
                                    accountsPerIP = int.Parse(txtAccountsPerIP.Text);
                                }
                                IPFetcher.AssignProxiesToAccounts(lstProxies, accountsPerIP);//AssignProxiesToAccounts(lstProxies);
                                ReloadAccountsFromDataBase();
                                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                            }
                            else
                            {
                                MessageBox.Show("Please assign private proxies from Proxies Tab in Main Page OR Upload a proxies Text File");
                            }
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> txtAsssignPublicIP_Click  --> " + ex.Message, Globals.Path_IPSettingErroLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> txtAsssignPublicIP_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                    else
                    {
                        #region CommentedRegion
                        //using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                        //{
                        //    ofd.Filter = "Text Files (*.txt)|*.txt";
                        //    ofd.InitialDirectory = Application.StartupPath;
                        //    if (ofd.ShowDialog() == DialogResult.OK)
                        //    {
                        //        list_pvtIP = new List<string>();

                        //        list_pvtIP = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                        //        if (!string.IsNullOrEmpty(txtAccountsPerIP.Text) && GlobusRegex.ValidateNumber(txtAccountsPerIP.Text))
                        //        {
                        //            accountsPerIP = int.Parse(txtAccountsPerIP.Text);
                        //        }
                        //        IPFetcher.AssignProxiesToAccounts(list_pvtIP, accountsPerIP);//AssignProxiesToAccounts(lstProxies);
                        //        ReloadAccountsFromDataBase();
                        //        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Proxies Assigned To Accounts ]");
                        //    }
                        //} 
                        #endregion
                    }
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void btnRemoveDuplicates_Click(object sender, EventArgs e)
        {
            foreach (string _item in lstSearchByKeywords)
            {
                if (File.Exists(Globals.Path_KeywordScrapedListData + "-" + _item + ".csv"))
                {
                    List<string> KeywordScpareList = GlobusFileHelper.ReadFiletoStringList(Globals.Path_KeywordScrapedListData + "-" + _item + ".csv");
                    if (KeywordScpareList.Count > 0)
                    {
                        KeywordScpareList = KeywordScpareList.Distinct().ToList();
                        foreach (string item in KeywordScpareList)
                        {
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Adding Data : " + item + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_KeywordScrapedListWithoutDuplicates);
                        }
                    }
                    else
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ File : " + Globals.Path_KeywordScrapedListData + " ]");
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ File is blank. ]");
                    }

                }
                else
                {
                    MessageBox.Show("File Does Not Exsist");
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ File : " + Globals.Path_KeywordScrapedListData + " ]");
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Does Not Exist ]");
                }
            }
        }

        #region Direct Messaging Commented Code - Change in Api So Could Not Send DM.

        //private void ChkBoxSelectAll_CheckedChanged_1(object sender, EventArgs e)
        //{
        //    if (ChkBoxSelectAll.Checked == true)
        //    {
        //        for (int i = 0; i < chklstDirectMessage.Items.Count; i++)
        //        {
        //            chklstDirectMessage.SetItemChecked(i, true);
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < chklstDirectMessage.Items.Count; i++)
        //        {
        //            chklstDirectMessage.SetItemChecked(i, false);
        //        }
        //    }
        //}
        //private void AddToDMLog(string log)
        //{
        //    try
        //    {
        //        if (lstDMLogger.InvokeRequired)
        //        {
        //            lstDMLogger.Invoke(new MethodInvoker(delegate
        //            {
        //                lstDMLogger.Items.Add(log);
        //                lstDMLogger.SelectedIndex = lstDMLogger.Items.Count - 1;
        //            }));
        //        }
        //        else
        //        {
        //            lstDMLogger.Items.Add(log);
        //            lstDMLogger.SelectedIndex = lstDMLogger.Items.Count - 1;
        //        }
        //    }
        //    catch { }
        //}

        //private void splitContainer5_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics g;

        //    g = e.Graphics;

        //    g.SmoothingMode = SmoothingMode.HighQuality;

        //    // Draw the background.
        //    //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

        //    //// Draw the image.
        //    g.DrawImage(image, 0, 0, splitContainer5.Width, splitContainer5.Height);
        //}
        /////////////////////////////////////////////////////////
        //private void btnStart_Click(object sender, EventArgs e)
        //{
        //    DMList.Clear();
        //    AddToDMLog("Starting Search For People");
        //    AddToDMLog("To Send Direct Messages");
        //    new Thread(() =>
        //        {
        //            SendDirectMessage();
        //        }
        //    ).Start();

        //}



        //public void SendDirectMessage()
        //{
        //    try
        //    {
        //    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
        //    {
        //        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
        //        {
        //            ThreadPool.SetMaxThreads(5, 5);

        //            ThreadPool.QueueUserWorkItem(new WaitCallback(StartDMMultiThreaded), new object[] { item });

        //            Thread.Sleep(1000);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please Add Accounts");
        //        AddToDMLog("Please Add Accounts");
        //    }
        //    }
        //    catch(Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        //public void StartDMMultiThreaded(object parameter)
        //{
        //    try
        //    {
        //        Array paramsArray = new object[1];

        //        paramsArray = (Array)parameter;

        //        KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

        //        TweetAccountManager AccountManager = item.Value;

        //        AccountManager.Username = item.Value.Username;
        //        AccountManager.Password = item.Value.Password;
        //        AccountManager.IPAddress = item.Value.IPAddress;
        //        AccountManager.IPPort = item.Value.IPPort;
        //        AccountManager.IPUsername = item.Value.IPUsername;
        //        AccountManager.IPpassword = item.Value.IPpassword;

        //        AddToDMLog("Logging In With Email : " + item.Value.Username);
        //        if (!AccountManager.IsLoggedIn)
        //        {
        //            AccountManager.Login();
        //        }



        //        if (AccountManager.IsLoggedIn)
        //        {
        //            AddToDMLog("Logged In With Email :" + AccountManager.Username);
        //            TwitterDataScrapper dataScrape = new TwitterDataScrapper();
        //            string returnStatus = string.Empty;
        //            List<string> DMFollowers = dataScrape.GetFollowers(AccountManager.userID, out returnStatus);
        //            //List<string> DMFollowings = dataScrape.GetFollowings(AccountManager.userID);
        //            //List<string> nonFollowings = DMFollowings.Except(DMFollowers).ToList();
        //            //List<string> FollowingnFollower = DMFollowings.Except(nonFollowings).ToList();
        //            AddToDMLog("Adding User's With Direct Message Option in Email : " + item.Value.Username);
        //            DMList.Add(AccountManager.userID, DMFollowers);

        //            if (cmbboxUserID.InvokeRequired)
        //            {
        //                new Thread(() =>
        //                    {
        //                        cmbboxUserID.Invoke(new MethodInvoker(delegate
        //                        {
        //                            cmbboxUserID.Items.Add(AccountManager.userID + ":" + AccountManager.Username);
        //                        }));
        //                    }
        //                ).Start();
        //            }
        //        }
        //        else
        //        {
        //            AddToDMLog("Account : " + AccountManager.Username + " Not Logged In");
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartDMMultiThreaded() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartDMMultiThreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        //private void cmbboxUserID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    chklstDirectMessage.Items.Clear();
        //    string GetUserID = cmbboxUserID.SelectedItem.ToString();
        //    foreach (KeyValuePair<string, List<string>> item in DMList)
        //    {
        //        if (GetUserID.Contains(item.Key))
        //        {
        //            List<string> DMUserIDs = item.Value;
        //            AddToDMLog(DMUserIDs.Count() + " Usernames in List");
        //            AddToDMLog("Please Wait While We Add Username To Send Direct Message");

        //            foreach (string Value in DMUserIDs)
        //            {
        //                ThreadPool.SetMaxThreads(20, 5);
        //                ThreadPool.QueueUserWorkItem(new WaitCallback(StartLoadUserId), new object[] { Value });
        //            }
        //        }
        //    }
        //}

        //public void StartLoadUserId(object parameter)
        //{
        //    try
        //    {
        //        Array paramsArray = new object[1];

        //        paramsArray = (Array)parameter;

        //        string Value = (string)paramsArray.GetValue(0);

        //        string GetUsername = TwitterDataScrapper.GetUserNameFromUserId(Value);
        //        if (!GetUsername.Contains("Rate Limit Exceeded"))
        //        {
        //            AddToDMLog("Adding Username : " + GetUsername);
        //            chklstDirectMessage.Invoke(new MethodInvoker(delegate
        //            {
        //                chklstDirectMessage.Items.Add(GetUsername);
        //            }));
        //        }
        //        else
        //        {
        //            AddToDMLog("Rate Limit Excedded.");
        //            AddToDMLog("Please Try After Some time");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartLoadUserId() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartLoadUserId() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        //private void btnSendDM_Click_1(object sender, EventArgs e)
        //{
        //    if (chklstDirectMessage.CheckedItems.Count > 0 && lstDirectMessage.Count > 0)
        //    {

        //        new Thread(() =>
        //            MethodSendDirectMessage()
        //            ).Start();
        //    }
        //    else
        //    {
        //        if (chklstDirectMessage.CheckedItems.Count <= 0)
        //        {
        //            AddToDMLog("Please Select At Least One Username");
        //        }
        //        else if (lstDirectMessage.Count <= 0)
        //        {
        //            AddToDMLog("Please Upload Message");
        //        }
        //    }
        //}

        //public void MethodSendDirectMessage()
        //{
        //    try
        //    {
        //        int numberOfThreads = 0;
        //        if (!string.IsNullOrEmpty(txtThreadNoDM.Text) && NumberHelper.ValidateNumber(txtThreadNoDM.Text))
        //        {
        //            numberOfThreads = Convert.ToInt32(txtThreadNoDM.Text);
        //        }
        //        else
        //        {
        //            AddToDMLog("Thread Value Not Correct. Assining Default Value = 5");
        //            numberOfThreads = 5;
        //        }

        //        if (lstDirectMessage.Count > 0)
        //        {
        //            foreach (string Message in lstDirectMessage)
        //            {
        //                TweetAccountManager.que_DirectMessage.Enqueue(Message);
        //            }
        //        }
        //        else
        //        {
        //            AddToDMLog("Please Add Message Text");
        //            return;
        //        }

        //        if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
        //        {
        //            foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
        //            {
        //                string value = string.Empty;
        //                cmbboxUserID.Invoke(new MethodInvoker(delegate
        //                    {
        //                        value = cmbboxUserID.SelectedItem.ToString();
        //                    }));

        //                if (value.Contains(item.Key))
        //                {
        //                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

        //                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartDirectMessagingMultithreaded), new object[] { item });

        //                    Thread.Sleep(1000);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> MethodSendDirectMessage() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> MethodSendDirectMessage() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        //public void StartDirectMessagingMultithreaded(object parameter)
        //{
        //    try
        //    {
        //        int MinimumDelay = 15;
        //        int MaximumDelay = 25;
        //        Array paramsArray = new object[1];

        //        paramsArray = (Array)parameter;

        //        KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

        //        TweetAccountManager AccountManager = item.Value;

        //        AccountManager.Username = item.Value.Username;
        //        AccountManager.Password = item.Value.Password;
        //        AccountManager.IPAddress = item.Value.IPAddress;
        //        AccountManager.IPPort = item.Value.IPPort;
        //        AccountManager.IPUsername = item.Value.IPUsername;
        //        AccountManager.IPpassword = item.Value.IPpassword;

        //        if (!AccountManager.IsLoggedIn)
        //        {
        //            AccountManager.Login();
        //        }


        //        List<string> SelectedItem = new List<string>();


        //            if (chklstDirectMessage.InvokeRequired)
        //            {
        //                chklstDirectMessage.Invoke(new MethodInvoker(delegate
        //                {
        //                    foreach (string Userid in chklstDirectMessage.CheckedItems)
        //                    {
        //                        SelectedItem.Add(Userid);
        //                    }

        //                }));
        //            }

        //        int MaximumMsgPerDay = 10;
        //        if (!string.IsNullOrEmpty(txtMessagePerDay.Text) && NumberHelper.ValidateNumber(txtMessagePerDay.Text))
        //        {
        //            MaximumMsgPerDay = Convert.ToInt32(txtMessagePerDay.Text);
        //        }
        //        clsDBQueryManager DbQueryManager = new clsDBQueryManager();
        //        DataSet Ds = DbQueryManager.SelectMessageData(item.Value.Username, "Message");

        //        int TodayMessage = Ds.Tables["tb_MessageRecord"].Rows.Count;
        //        AddToLog_Tweet(TodayMessage + " Already Messaged today");

        //        #region Delay
        //        if (!string.IsNullOrEmpty(txtMinDMDelay.Text) && NumberHelper.ValidateNumber(txtMinDMDelay.Text))
        //        {
        //            MinimumDelay = Convert.ToInt32(txtMinDMDelay.Text);
        //        }
        //        else
        //        {
        //            AddToDMLog("Minimum Delay Value Incorrect");
        //            AddToDMLog("Assinging Defauly Value = 15");
        //        }

        //        if (!string.IsNullOrEmpty(txtMaxDMDelay.Text) && NumberHelper.ValidateNumber(txtMaxDMDelay.Text))
        //        {
        //            MaximumDelay = Convert.ToInt32(txtMaxDMDelay.Text);
        //        }
        //        else
        //        {
        //            AddToDMLog("Minimum Delay Value Incorrect");
        //            AddToDMLog("Assinging Defauly Value = 25");
        //        }
        //        #endregion

        //        foreach (string Userid in SelectedItem)
        //        {
        //            if (TodayMessage >= MaximumMsgPerDay)
        //            {
        //                AddToDMLog("Already Messaged " + TodayMessage);
        //                break;
        //            }
        //            AddToDMLog("Direct Message To " + Userid);
        //            int Delay = RandomNumberGenerator.GenerateRandom(MinimumDelay, MaximumDelay);
        //            string MessagePosted = AccountManager.DirectMessage(Userid);
        //            if (MessagePosted.Contains("Success"))
        //            {
        //                string[] Array = Regex.Split(MessagePosted, ":");
        //                AddToDMLog("Message Posted :"+ Array[1]);
        //            }
        //            else if (MessagePosted.Contains("Error"))
        //            {
        //                AddToDMLog("Error in Post");
        //            }
        //            else
        //            {
        //                AddToDMLog("Message Not Posted");
        //            }
        //            AddToDMLog("Delay For : " + Delay);
        //            Thread.Sleep(Delay);
        //            TodayMessage++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartDirectMessagingMultithreaded() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartDirectMessagingMultithreaded() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        //private void btnUploadDM_Click_1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
        //        {
        //            ofd.Filter = "Text Files (*.txt)|*.txt";
        //            ofd.InitialDirectory = Application.StartupPath;
        //            if (ofd.ShowDialog() == DialogResult.OK)
        //            {
        //                txtUploadDM.Text = ofd.FileName;
        //                List<string> tempList = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
        //                foreach (string item in tempList)
        //                {
        //                    string newitem = item;
        //                    if (!string.IsNullOrEmpty(newitem))
        //                    {
        //                        lstDirectMessage.Add(newitem);
        //                    }
        //                }

        //                AddToDMLog(lstDirectMessage.Count() + " Messages Uploaded");
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_DMErroLog);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnUploadDM_Click_1() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }
        //}

        /////////////////////////////////////////////////

        #endregion

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
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Entered Count incorrect.Setting Default Count 10 ]");
                }

                TwitterDataScrapper DataScraper = new TwitterDataScrapper();
                foreach (string item in lstUserId_Retweet)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting ReTweets For " + item + " ]");
                    string ReturnStatus = string.Empty;
                    List<string> TweetData = DataScraper.GetRetweetData_Scrape(item, out ReturnStatus);

                    if (ReturnStatus.Contains("No Error"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + TweetData.Count + " Retweet From User : " + item + " ]");
                        if (TweetData.Count > 0)
                        {
                            //new Thread(() =>
                            //{
                            foreach (string newItem in TweetData)
                            {
                                string[] arry = Regex.Split(newItem, ":");
                                if (arry.Length == 3)
                                {
                                    //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + arry[0] + " : " + arry[1] + " ]");
                                    //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + arry[2] + " ]");                                   
                                    clsDBQueryManager DataBase = new clsDBQueryManager();
                                    DataBase.InsertDataRetweet(arry[0], arry[1], arry[2]);
                                }
                                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + newItem + " ]");
                            }
                            //}).Start();
                        }
                        else
                        {
                            AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Sorry No ReTweets For " + item + " ]");
                        }
                    }
                    else if (ReturnStatus.Contains("Rate limit exceeded"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Rate limit exceeded ]");
                        break;
                    }
                    else if (ReturnStatus.Contains("Sorry, that page does not exist"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>> Sorry, that page does not exist ]");
                    }
                    else if (ReturnStatus.Contains("Empty"))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>>> Return Empty ]");
                    }
                    else
                    {
                        //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + item + " >>> Errror in Request ]");
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ No item Found From User " + item + " ]");
                    }
                }

                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ ReTweets Stored In -" + Globals.Path_RETweetExtractor + " ]");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Finished Extracting ReTweets ]");
                AddToTweetCreatorLogs("-----------------------------------------------------------------------------------------------------------------------");

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
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ File Does Not Exsists ]");
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
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ File Does Not Exist ]");
                        txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                    }
                }
                else
                {
                    txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                }
            }
        }

        private void chkboxRetweetPerDay_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxRetweetPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = true;
                grpTweetNoOfRetweetReply.Enabled = false;
            }
            else if (!chkboxRetweetPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = false;
                grpTweetNoOfRetweetReply.Enabled = true;
            }
            if (chkboxReplyPerDay.Checked == true && chkboxRetweetPerDay.Checked == false)
            {
                txtMaximumNoRetweet.Enabled = true;
                grpTweetNoOfRetweetReply.Enabled = false;
            }
        }

        private void chkboxReplyPerDay_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkboxReplyPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = true;
                grpTweetNoOfRetweetReply.Enabled = false;
            }
            else if (!chkboxReplyPerDay.Checked)
            {
                txtMaximumNoRetweet.Enabled = false;
                grpTweetNoOfRetweetReply.Enabled = true;
            }
            if (chkboxReplyPerDay.Checked == false && chkboxRetweetPerDay.Checked == true)
            {
                txtMaximumNoRetweet.Enabled = true;
                grpTweetNoOfRetweetReply.Enabled = false;
            }
        }

        private void chkboxSetting_CheckedChanged_1(object sender, EventArgs e)
        {
            if (chkboxSetting.Checked)
            {
                grpFollowerSetting.Enabled = true;
                //grpFollowByKeyword.Enabled = false;
                //chkbox_FollowByKeyWord.Checked = false;
            }
            else if (!chkboxSetting.Checked)
            {
                grpFollowerSetting.Enabled = false;
            }
        }

        private void tabMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                Brush _TextBrush;

                // Get the item from the collection.
                TabPage _TabPage = Tb_AccountManager.TabPages[e.Index];

                // Get the real bounds for the tab rectangle.
                Rectangle _TabBounds = Tb_AccountManager.GetTabRect(e.Index);

                if (e.State == DrawItemState.Selected)
                {
                    // Draw a different background color, and don't paint a focus rectangle.
                    Brush background_brush1 = new SolidBrush(Color.FromArgb(247,126,75));//(95, 181, 232));

                    _TextBrush = new SolidBrush(Color.White);

                    g.FillRectangle(background_brush1, e.Bounds);
                    //g.DrawImage(recSideImage, e.Bounds);
                }
                else
                {
                    _TextBrush = new System.Drawing.SolidBrush(Color.Brown);
                    g.FillRectangle(Brushes.Brown, e.Bounds);
                    e.DrawBackground();
                }

                // Use our own font. Because we CAN.
                Font _TabFont = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);


                //Set On tab For hiding Side Space of Tab button....

                Brush background_brush = new SolidBrush(Color.FromArgb(86, 137, 194));

                Rectangle LastTabRect = Tb_AccountManager.GetTabRect(Tb_AccountManager.TabPages.Count - 1);

                Rectangle rect = new Rectangle();

                rect.Location = new Point(0, LastTabRect.Bottom + 3);

                rect.Size = new Size(_TabPage.Right - (_TabPage.Width + 3), _TabPage.Height);

                //e.Graphics.FillRectangle(background_brush, rect);
                e.Graphics.DrawImage(recSideImage, rect);


                // Draw string. Center the text.
                StringFormat _StringFlags = new StringFormat();
                _StringFlags.Alignment = StringAlignment.Center;
                _StringFlags.LineAlignment = StringAlignment.Center;
                g.DrawString(_TabPage.Text, _TabFont, _TextBrush, _TabBounds, new StringFormat(_StringFlags));
            }
            catch (Exception)
            {
            }
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

        private void chkTweetUsedIdToScrap_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTweetUsedIdToScrap.Checked)
            {
                grpTweetMentionSetting.Visible = false;
                groupBox4.Visible = true;
                IsMentionUserWithScrapedList = true;
                
            }
            else
            {
               
                grpTweetMentionSetting.Visible = true;
                groupBox4.Visible = false;
                IsMentionUserWithScrapedList = false;
                //label135.Visible = false;
                //txtTweetScrapUserData.Visible = false;
                //chkTweetScrapFollowers.Visible = false;
                //chkRetweetScrapFollowings.Visible = false;
                //label134.Visible = false;
                //txtTweetEnteNoofUser.Visible = false;
            }
        }

        

        private void tabMain_TabIndexChanged(object sender, EventArgs e)
        {
            this.Update();
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Tb_AccountManager.SelectedIndex == 0)
            //{
            //    labelAccountcreator.Text = "Account Creator";
            //}
            //else if (Tb_AccountManager.SelectedIndex == 1)
            //{
            //    labelAccountcreator.Text = "Profile Manager";
            //}
            if (Tb_AccountManager.SelectedIndex == 0)
            {
                labelAccountcreator.Text = "Follower";                
            }
            else if (Tb_AccountManager.SelectedIndex == 1)
            {
                labelAccountcreator.Text = "Tweet";                
            }
            else if (Tb_AccountManager.SelectedIndex == 2)
            {
                labelAccountcreator.Text = "Account Actions";
            }
            else if (Tb_AccountManager.SelectedIndex == 3)
            {
                labelAccountcreator.Text = "Unfollower";
            }
            else if (Tb_AccountManager.SelectedIndex == 4)
            {
                labelAccountcreator.Text = "Wait & Reply";
            }
            else if (Tb_AccountManager.SelectedIndex == 5)
            {
                labelAccountcreator.Text = "IP Setting";
            }
            else if (Tb_AccountManager.SelectedIndex == 6)
            {
                labelAccountcreator.Text = "Scrape Users";
            }
            else if (Tb_AccountManager.SelectedIndex == 7)
            {
                labelAccountcreator.Text = "Tweet Creator";
            }
            //else if (Tb_AccountManager.SelectedIndex == 8)
            //{
            //    labelAccountcreator.Text = "Fake Email Creator";
            //}

            //else if (Tb_AccountManager.SelectedIndex == 8)
            //{
            //    labelAccountcreator.Text = "Account By Browser";
            //}
            else if (Tb_AccountManager.SelectedIndex == 8)
            {
                labelAccountcreator.Text = "Filter Users";
            }
            if (Tb_AccountManager.SelectedIndex == 9)
            {
                labelAccountcreator.Text = "Retweet & Favorite";
            }
            //if (Tb_AccountManager.SelectedIndex == 12)
            //{
            //    labelAccountcreator.Text = "Mobile A/c Creation";
            //}
            if (Tb_AccountManager.SelectedIndex == 10)
            {
                labelAccountcreator.Text = "Account Manager";
            }

            ////else if (Tb_AccountManager.SelectedIndex == 11)
            ////{
            ////    labelAccountcreator.Text = "Account By Browser";
            ////}
            //else if (Tb_AccountManager.SelectedIndex == 11)
            //{
            //    labelAccountcreator.Text = "Filter Users";
            //}
            //if (Tb_AccountManager.SelectedIndex == 12)
            //{
            //    labelAccountcreator.Text = "Retweet & Favorite";
            //}
            //if (Tb_AccountManager.SelectedIndex == 13)
            //{
            //    labelAccountcreator.Text = "Mobile A/c Creation";
            //}
            //if (Tb_AccountManager.SelectedIndex == 14)
            //{
            //    labelAccountcreator.Text = "Account Manager";
            //}
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
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            exportPath = ofd.FileName;
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Scrapped Data will be exported to : " + exportPath + " ]");
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
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Data Exported to : " + exportPath + " ]");
            }
            else
            {
                //No Data
                MessageBox.Show("Please Scrape Data First");
                //
            }
        }

        ////Web Browwser TD start
        List<string> LstAccountCreationName = new List<string>();
        List<string> LstAccountcreationUsername = new List<string>();
        List<string> LstAccountcreationEmailPassword = new List<string>();
        int countForInstance = 0;



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

        public string DecodeDBC(string[] args, byte[] imageBytes, out DeathByCaptcha.Client client, out DeathByCaptcha.Captcha captcha)
        {
            try
            {
                // Put your DBC username & password here:
                //DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Please check your DBC Account Details ]");
                    captcha = null;
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ You have 0 Balance in your DBC Account ]");
                    captcha = null;
                    return null;
                }

                for (int i = 2, l = args.Length; i < l; i++)
                {
                    Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                    // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                    // file name, file object, stream, or a vector of bytes, and desired
                    // solving timeout (in seconds) here:
                    //DeathByCaptcha.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
                    captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
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

            client = null;
            captcha = null;
            return null;
        }

        public void SetIP(string servername, string portnumber)
        {
            try
            {
                //GlobusHttpHelper objhelper = new GlobusHttpHelper();

                //string responce = objhelper.getHtmlfromUrlIP(new Uri("http://www.google.co.in/"), servername, int.Parse(portnumber), "", "");

                string key = "Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings";
                string serverName = servername;//your IP server name;

                string port = portnumber; //your IP port;

                string IP = serverName + ":" + port;

                RegistryKey RegKey = Registry.CurrentUser.OpenSubKey(key, true);

                RegKey.SetValue("IPServer", IP);

                RegKey.SetValue("IPEnable", 1);
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
                     //   txtBrowserName.Text = ofd.FileName;
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
                    AddToIPAccountCreationLog("[ " + DateTime.Now + " ] => [ " + LstAccountCreationName.Count() + " Name in List ]");
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
                       // txtBrowserEmail.Text = ofd.FileName;
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
                    AddToIPAccountCreationLog("[ " + DateTime.Now + " ] => [ " + LstAccountcreationEmailPassword.Count() + " Email in List ]");
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
                      //  txtBrowserUsername.Text = ofd.FileName;
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
                    AddToIPAccountCreationLog("[ " + DateTime.Now + " ] => [ " + LstAccountcreationUsername.Count() + " Username in List ]");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnBrowserName_Click() --> " + ex.Message, Globals.Path_DMErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnBrowserName_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            #region commented
            //try
            //{
            //    DialogResult dialogresult = MessageBox.Show("Sure you want to exit twtboardpro App", "twtboardpro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //    if (dialogresult == DialogResult.Yes)
            //    {
            //        try
            //        {
            //            //StopAllProcess();
            //            foreach (var item in System.Diagnostics.Process.GetProcesses())
            //            {
            //                try
            //                {
            //                    if (item.ProcessName.Contains("TD_LicensingManager"))
            //                    {
            //                        item.Kill();

            //                    }
            //                }
            //                catch (Exception ex)
            //                {
            //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FormClosed--> " + ex.Message, Globals.Path_TwtErrorLogs);
            //                }
            //            }
            //            Application.ExitThread();
            //            Application.Exit();

            //            var prc1 = System.Diagnostics.Process.GetProcesses();
            //            foreach (var item in prc1)
            //            {
            //                try
            //                {
            //                    if (item.ProcessName.Contains("TD_LicensingManager"))
            //                    {
            //                        item.Kill();
            //                    }
            //                }
            //                catch
            //                {
            //                }
            //            }

            //            Application.Exit();
            //        }
            //        catch (Exception)
            //        {
            //            Application.ExitThread();
            //            Application.Exit();
            //        }
            //    }
            //    else
            //    {

            //        e.Cancel = true;
            //        this.Activate();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FormClosed--> " + ex.Message, Globals.Path_TwtErrorLogs);
            //} 
            #endregion
        }

        private void frmMain_NewUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialogresult = MessageBox.Show("Sure you want to exit twtboardpro App", "twtboardpro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogresult == DialogResult.Yes)
                {
                    try
                    {
                        //StopAllProcess();
                        foreach (var item in System.Diagnostics.Process.GetProcesses())
                        {
                            try
                            {
                                if (item.ProcessName.Contains("twtBoard_LicensingManager"))
                                {
                                    item.Kill();

                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FormClosed--> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        Application.ExitThread();
                        Application.Exit();

                        var prc1 = System.Diagnostics.Process.GetProcesses();
                        foreach (var item in prc1)
                        {
                            try
                            {
                                if (item.ProcessName.Contains("twtBoard_LicensingManager"))
                                {
                                    item.Kill();
                                }
                            }
                            catch
                            {
                            }
                        }

                        Application.Exit();
                    }
                    catch (Exception)
                    {
                        Application.ExitThread();
                        Application.Exit();
                    }
                }
                else
                {

                    e.Cancel = true;
                    this.Activate();

                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FormClosed--> " + ex.Message, Globals.Path_TwtErrorLogs);
            } 
        }

        private void splitContainerMain_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerMain.Width, splitContainerMain.Height);
        }

        private void frmMain_NewUI_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void tabAccountCreator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
          //  g.DrawImage(image, 0, 0, tabAccountCreator.Width, tabAccountCreator.Height);
        }

        private void splitContainer_ProfileManager_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainer_ProfileManager.Width, splitContainer_ProfileManager.Height);
        }

        private void grpFollower_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, grpFollower.Width, grpFollower.Height);

        }

        private void splitContainerTweet_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerTweet.Width, splitContainerTweet.Height);
        }

        private void splitContainer1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the  image.
            g.DrawImage(image, 0, 0, splitContainer1.Width, splitContainer1.Height);
        }

        private void splitContainerUnfollow_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerUnfollow.Width, splitContainerUnfollow.Height);
        }

        private void splitContainerUnfollow_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerUnfollow.Width, splitContainerUnfollow.Height);

        }

        private void splitContainer2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainer2.Width, splitContainer2.Height);

        }

        private void tabIP_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tabIP.Width, tabIP.Height);
        }

        private void tabScrape_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tabScrape.Width, tabScrape.Height);
        }

        private void tabTweetCreator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tabTweetCreator.Width, tabTweetCreator.Height);
        }

        private void splitContainer4_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
         //   g.DrawImage(image, 0, 0, splitContainer4.Width, splitContainer4.Height);
        }

        private void tabAccountBrowser_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
          //  g.DrawImage(image, 0, 0, tabAccountBrowser.Width, tabAccountBrowser.Height);
        }

        private void splitContainer_ScrapeUsers_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainer_ScrapeUsers.Width, splitContainer_ScrapeUsers.Height);
        }

        private void splitContainerMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerMain.Width, splitContainerMain.Height);
            
        }

        private void tabProfileManager_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tabProfileManager.Width, tabProfileManager.Height);
        }

        private void splitContainerFollower_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, splitContainerFollower.Width, splitContainerFollower.Height);
        }



        private void btnUploadRetweetUserID_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if ((lstUserId_Retweet.Count > 0 && !string.IsNullOrEmpty(txtRetweetFile.Text)) || !string.IsNullOrEmpty(txtRetweetSingle.Text.Replace(" ", "").Replace("\0", "")))
                {
                    if (!string.IsNullOrEmpty(txtRetweetFile.Text))
                    {
                        objclsSettingDB.InsertOrUpdateSetting("TweetCreator", "ReTweetExtractor", StringEncoderDecoder.Encode(txtRetweetFile.Text));
                    }
                    if (!string.IsNullOrEmpty(txtRetweetSingle.Text))
                    {
                        lstUserId_Retweet.Clear();
                        lstUserId_Retweet.Add(txtRetweetSingle.Text);
                    }
                    new Thread(() =>
                    {
                        RetweetDataExtractor();
                    }
                    ).Start();
                }
                else
                {
                    if ((lstUserId_Retweet.Count <= 0 || string.IsNullOrEmpty(txtRetweetFile.Text)) || string.IsNullOrEmpty(txtRetweetSingle.Text.Replace(" ", "").Replace("\0", "")))
                    {
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Please Enter a Username/Userid Name or File. ]");
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        private void btn_RetweetExtract_Click(object sender, EventArgs e)
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

                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstUserId_Retweet.Count + " Usernames/Userid Uploaded ]");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnTweetExctractor_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        //*****Get Follow according to search Key word *************

        private void chkbox_FollowByKeyWord_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_FollowByKeyWord.Checked)
            {
                grpFollowByKeyword.Enabled = true;
                grpFollowerSetting.Enabled = false;
                //chkboxSetting.Checked = false;
                btn_FollowByKeyWordStart.Enabled = true;
                btn_searchBykeyword.Enabled = true;
                btn_FollowKeyWordsFileUpload.Enabled = true;
                btnStartFollowing.Enabled = false;
                btnStop_FollowThreads.Enabled = false;
            }
            else
            {
                grpFollowByKeyword.Enabled = false;
                btn_FollowByKeyWordStart.Enabled = false;
                btn_searchBykeyword.Enabled = false;
                btn_FollowKeyWordsFileUpload.Enabled = false;
                btnStartFollowing.Enabled = true;
                btnStop_FollowThreads.Enabled = true;
            }
        }

        //*****Get Follow according to search Key word *************

        private void chk_FollowBySearchKeyWord_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkbox_FollowByKeyWord.Checked)
            //{
            //    panel_FollowBySearch.Visible = true;
            //    panel_FollowBySearch.BringToFront();
            //}
            //else
            //{
            //    panel_FollowBySearch.Visible = false;
            //    panel_FollowBySearch.SendToBack();
            //}
        }

        Dictionary<string, Thread> Dic_Thread = new Dictionary<string, Thread>();
        int NoOfLoadAccount = 0;
        public static bool IsFollowByKeyWordStart = true;      

        public  void btn_FollowByKeyWordStart_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txt_FollowBySearchKey.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("Follower", "FollowBySearchKey", StringEncoderDecoder.Encode(txt_FollowBySearchKey.Text));
                }

                if (IsFollowByKeyWordStart)
                {
                    IsFollowByKeyWordStart = false;

                  //  new Thread(() =>
                 //   {

                        Dic_Thread.Clear();

                        Thread.CurrentThread.Name = "Thread_FollowByKeyword";
                        Dic_Thread.Add(Thread.CurrentThread.Name, Thread.CurrentThread);
                        //TweetAccountManager TweetAccountManager = new TweetAccountManager();
                        //TwitterDataScrapper TwitterDataScrapper = new TwitterDataScrapper();
                        //int FollowLimitCounter = 0;
                        //int AccountCounter = 0;

                        FollowtweetKeywordList.ForEach(i => { searchkeywordqueue.Enqueue(i); });

                        int NoOfThreads = int.Parse(txtNoOfFollowThreads.Text);
                        int DelayStart = 0;
                        int DelayEnd = 0;
                        bool _IsValue = false;

                        _IsValue = int.TryParse((txtFollowMinDelay.Text), out DelayStart);
                        if (!_IsValue)
                        {
                            IsFollowByKeyWordStart = true;
                            return;
                        }

                        _IsValue = false;
                        _IsValue = int.TryParse((txtFollowMaxDelay.Text), out DelayEnd);
                        if (!_IsValue)
                        {
                            IsFollowByKeyWordStart = true;
                            return;
                        }

                        string SeachKey = string.Empty;
                        List<TwitterDataScrapper.StructTweetIDs> KeywordStructData = new List<TwitterDataScrapper.StructTweetIDs>();
                        TwitterDataScrapper TwitterDataScrapper = new TwitterDataScrapper();

                        if (FollowtweetKeywordList.Count != 0)
                        {
                            //TwitterDataScrapper.noOfRecords = int.Parse(txt_FollowByPerAccount.Text);
                            txt_FollowByPerAccount.Invoke(new MethodInvoker(delegate
                            {
                                if (!string.IsNullOrEmpty(txt_FollowByPerAccount.Text))
                                {
                                    TwitterDataScrapper.noOfRecords = int.Parse(txt_FollowByPerAccount.Text);
                                }
                                else
                                {
                                    TwitterDataScrapper.noOfRecords = 5;
                                }
                            }));
                            if (!chk_followbysinglekeywordperaccount.Checked)
                            {
                                try
                                {
                                    if (searchkeywordqueue.Count == 0)
                                    {
                                        return;
                                    }
                                    SeachKey = searchkeywordqueue.Dequeue().ToString();//FollowtweetKeywordList[counterThreadsFollowByKeyWord];
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Start Scraping User for keyword= " + SeachKey + " ]");
                                   
                                    KeywordStructData = TwitterDataScrapper.GetTweetData_New(SeachKey);

                                    if (KeywordStructData.Count == 0)
                                    {
                                        KeywordStructData = TwitterDataScrapper.NewKeywordStructData(SeachKey);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);

                                foreach (string SeachKey_item in FollowtweetKeywordList)
                                {
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Start Scraping User for keyword= " + SeachKey_item + " . ]");
                                    List<TwitterDataScrapper.StructTweetIDs> KeywordStructData1 = new List<TwitterDataScrapper.StructTweetIDs>();
                                    int datacounter = 0;
                                    KeywordStructData1 = TwitterDataScrapper.NewKeywordStructData1(SeachKey_item);

                                    if (KeywordStructData1.Count == 0)
                                    {
                                        KeywordStructData1 = TwitterDataScrapper.KeywordStructData(SeachKey_item);
                                    }

                                    if (KeywordStructData1.Count == 0)
                                    {
                                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ Key Word is not Exist/suspended. ]");
                                    }

                                    foreach (var KeywordStructData1_item in KeywordStructData1)
                                    {
                                        if (datacounter == TwitterDataScrapper.noOfRecords)
                                        {
                                            break;
                                        }
                                        else
                                        {
                                            KeywordStructData.Add(KeywordStructData1_item);
                                            datacounter++;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Upload Keywords");
                            //break;
                        }

                        List<List<TwitterDataScrapper.StructTweetIDs>> list_lstTargetUsers = new List<List<TwitterDataScrapper.StructTweetIDs>>();
                        int index = 0;

                        if (chkUseDivide.Checked || IsUsingDivideData)
                        {
                            int splitNo = 0;
                            if (rdBtnDivideEqually.Checked)
                            {
                                splitNo = KeywordStructData.Count / TweetAccountContainer.dictionary_TweetAccount.Count;
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
                                splitNo = RandomNumberGenerator.GenerateRandom(0, KeywordStructData.Count - 1);
                            }
                            list_lstTargetUsers = Split(KeywordStructData, splitNo);
                            
                        }


                        if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                        {
                            NoOfLoadAccount = TweetAccountContainer.dictionary_TweetAccount.Count;
                            try
                            {
                                double Num;
                                bool isNum = double.TryParse((txt_FollowByPerAccount.Text.Trim()), out Num);

                                if (isNum)
                                {
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Start process from follow by search keyword. ]");
                                    ThreadPool.SetMaxThreads(NoOfThreads, 5);
                                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                                    {
                                        #region << Old Code >>

                                        //string profileUsername = string.Empty;
                                        //string profileUserpass = string.Empty;

                                        //profileUsername = item.Key;
                                        //profileUserpass = item.Value.Password;


                                        ////******search profile By keyWords
                                        //string SeachKey = string.Empty;
                                        //List<TwitterDataScrapper.StructTweetIDs> KeywordStructData = new List<TwitterDataScrapper.StructTweetIDs>();
                                        //if (FollowtweetKeywordList.Count != 0)
                                        //{
                                        //    if (!chk_followbysinglekeywordperaccount.Checked)
                                        //    {
                                        //        try
                                        //        {
                                        //            SeachKey = FollowtweetKeywordList[AccountCounter];
                                        //            KeywordStructData = TwitterDataScrapper.GetTweetData_New(SeachKey);

                                        //            if (KeywordStructData.Count == 0)
                                        //            {
                                        //                KeywordStructData = TwitterDataScrapper.NewKeywordStructData(SeachKey);
                                        //            }
                                        //        }
                                        //        catch (Exception)
                                        //        {
                                        //        }
                                        //    }
                                        //    else
                                        //    {

                                        //        foreach (string SeachKey_item in FollowtweetKeywordList)
                                        //        {
                                        //            List<TwitterDataScrapper.StructTweetIDs> KeywordStructData1 = new List<TwitterDataScrapper.StructTweetIDs>();
                                        //            int datacounter = 0;
                                        //            KeywordStructData1 = TwitterDataScrapper.NewKeywordStructData(SeachKey_item);

                                        //            if (KeywordStructData1.Count == 0)
                                        //            {
                                        //                KeywordStructData1 = TwitterDataScrapper.KeywordStructData(SeachKey_item);
                                        //            }

                                        //            if (KeywordStructData1.Count == 0)
                                        //            {
                                        //                AddToLog_Follower("Key Word is not Exist/suspended.");
                                        //            }

                                        //            foreach (var KeywordStructData1_item in KeywordStructData1)
                                        //            {
                                        //                if (datacounter == int.Parse(txt_FollowByPerAccount.Text))
                                        //                {
                                        //                    break;
                                        //                }
                                        //                else
                                        //                {
                                        //                    KeywordStructData.Add(KeywordStructData1_item);
                                        //                    datacounter++;
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    MessageBox.Show("Please Upload Keywords");
                                        //    break;
                                        //}


                                        ////*************
                                        //try
                                        //{
                                        //    if (KeywordStructData.Count > 0)
                                        //    {
                                        //        TweetAccountManager AccountManager = (TweetAccountManager)item.Value;
                                        //        AccountManager.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);

                                        //        int DelayStart = 0;
                                        //        int DelayEnd = 0;
                                        //        bool _IsValue = false;

                                        //        _IsValue = int.TryParse((txtFollowMinDelay.Text), out DelayStart);
                                        //        if (!_IsValue)
                                        //        {
                                        //            return;
                                        //        }

                                        //        _IsValue = false;
                                        //        _IsValue = int.TryParse((txtFollowMaxDelay.Text), out DelayEnd);
                                        //        if (!_IsValue)
                                        //        {
                                        //            return;
                                        //        }

                                        //        if (!AccountManager.IsLoggedIn)
                                        //        {
                                        //            AccountManager.Login();
                                        //        }

                                        //        foreach (var item1 in KeywordStructData)
                                        //        {
                                        //            if (!chk_followbysinglekeywordperaccount.Checked)
                                        //            {
                                        //                //accordint to get Follow users limits 
                                        //                if (FollowLimitCounter >= (int.Parse(txt_FollowByPerAccount.Text)))
                                        //                {
                                        //                    FollowLimitCounter = 0;
                                        //                    break;
                                        //                }
                                        //                else
                                        //                {
                                        //                    FollowLimitCounter++;
                                        //                }
                                        //            }

                                        //            //Get follow from user
                                        //            string AccountId = item1.ID_Tweet_User;

                                        //            ///Return if Suspended
                                        //            if (AccountManager.AccountStatus == "Account Suspended")
                                        //            {
                                        //                AddToLog_Follower(profileUsername + " : Suspended");
                                        //                break;
                                        //            }
                                        //            else if ((AccountManager.globusHttpHelper.gResponse.ResponseUri.ToString().ToLower()).Contains("captcha"))
                                        //            {
                                        //                AddToLog_Follower(profileUsername + " : Asking for captcha.");
                                        //                break;
                                        //            }

                                        //            getFollowUserBySearch(new object[] { AccountManager, AccountId });

                                        //            int Delay = RandomNumberGenerator.GenerateRandom(DelayStart, DelayEnd);
                                        //            AddToLog_Follower("Delay :- " + Delay + " Seconds.");
                                        //            Thread.Sleep(Delay);
                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        //Message List is Empty

                                        //        AddToLog_Follower("Key Word File is Empty or Wrong Formate");

                                        //        break;
                                        //    }
                                        //}
                                        //catch (Exception)
                                        //{

                                        //}

                                        //if (AccountCounter > TweetAccountContainer.dictionary_TweetAccount.Count)
                                        //{
                                        //    AccountCounter = 0;
                                        //}
                                        //else
                                        //{
                                        //    AccountCounter++;
                                        //}

                                        #endregion

                                        if (counterThreadsFollowByKeyWord >= NoOfThreads)
                                        {
                                            lock (lockerThreadsFollowByKeyWord)
                                            {
                                                Monitor.Wait(lockerThreadsFollowByKeyWord);
                                            }
                                        }


                                        if (chkUseDivide.Checked || IsUsingDivideData)
                                        {
                                            KeywordStructData = list_lstTargetUsers[index];
                                        }
                                        //ThreadPool.QueueUserWorkItem(new WaitCallback(StartFollowByKeyWord), new object[] { item, DelayStart, DelayEnd });
                                        Thread threadGetStartProcessForfollow = new Thread(StartFollowByKeyWord);
                                        threadGetStartProcessForfollow.Name = "Thread_FollowByKeyword" + "_" + item.Key;
                                        threadGetStartProcessForfollow.IsBackground = true;
                                        threadGetStartProcessForfollow.Start(new object[] { item, DelayStart, DelayEnd, KeywordStructData });

                                        index++;
                                        Thread.Sleep(1000);

                                    }
                                }//isNum If End
                                else
                                {
                                    MessageBox.Show("Please enter No of follow By per account");
                                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please enter No of follow By per account ]");
                                }
                            }
                            catch (Exception)
                            {

                            }
                            finally
                            {
                                //if (FollowtweetKeywordList.Count == 0)
                                //{
                                //    AddToLog_Follower("Please Upload Keywords");
                                //}
                                //else
                                //{
                                //    AddToLog_Follower("Follow By key Word Process is Finished.");
                                //}
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Upload Twitter Account");
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Please Upload Twitter Account ]");
                        }

                        IsFollowByKeyWordStart = true;
                  //  }).Start();
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        Queue<string> searchkeywordqueue = new Queue<string>();
        public readonly object lockerThreadsFollowByKeyWord = new object();
        public int counterThreadsFollowByKeyWord = 0;
        public void StartFollowByKeyWord(Object Param)
        {
            try
            {
                try
                {
                    Dic_Thread.Add(Thread.CurrentThread.Name, Thread.CurrentThread);
                }
                catch { };

                Interlocked.Increment(ref counterThreadsFollowByKeyWord);  //lockerThreadsFollowByKeyWord
                Array ParamArray = (Array)Param;

                string profileUsername = string.Empty;
                string profileUserpass = string.Empty;
                int FollowLimitCounter = 0;
                //int AccountCounter = 0;


                TweetAccountManager TweetAccountManager = new TweetAccountManager();
                TwitterDataScrapper TwitterDataScrapper = new TwitterDataScrapper();

                KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)ParamArray.GetValue(0);

                int DelayStart = (int)ParamArray.GetValue(1);
                int DelayEnd = (int)ParamArray.GetValue(2);
                List<TwitterDataScrapper.StructTweetIDs> KeywordStructData = (List<TwitterDataScrapper.StructTweetIDs>)ParamArray.GetValue(3);

                profileUsername = item.Key;
                profileUserpass = item.Value.Password;


                //******search profile By keyWords
                string SeachKey = string.Empty;
                //List<TwitterDataScrapper.StructTweetIDs> KeywordStructData = new List<TwitterDataScrapper.StructTweetIDs>();
                #region commented
                //if (FollowtweetKeywordList.Count != 0)
                //{
                //    TwitterDataScrapper.noOfRecords = int.Parse(txt_FollowByPerAccount.Text);
                //    if (!chk_followbysinglekeywordperaccount.Checked)
                //    {
                //        try
                //        {
                //            if (searchkeywordqueue.Count == 0)
                //            {
                //                return;
                //            }

                //            SeachKey = searchkeywordqueue.Dequeue().ToString();//FollowtweetKeywordList[counterThreadsFollowByKeyWord];
                //            KeywordStructData = TwitterDataScrapper.GetTweetData_New(SeachKey);

                //            if (KeywordStructData.Count == 0)
                //            {
                //                KeywordStructData = TwitterDataScrapper.NewKeywordStructData(SeachKey);
                //            }
                //        }
                //        catch (Exception)
                //        {
                //        }
                //    }
                //    else
                //    {
                //        //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);

                //        foreach (string SeachKey_item in FollowtweetKeywordList)
                //        {
                //            List<TwitterDataScrapper.StructTweetIDs> KeywordStructData1 = new List<TwitterDataScrapper.StructTweetIDs>();
                //            int datacounter = 0;
                //            KeywordStructData1 = TwitterDataScrapper.NewKeywordStructData1(SeachKey_item);

                //            if (KeywordStructData1.Count == 0)
                //            {
                //                KeywordStructData1 = TwitterDataScrapper.KeywordStructData(SeachKey_item);
                //            }

                //            if (KeywordStructData1.Count == 0)
                //            {
                //                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Key Word is not Exist/suspended. ]");
                //            }

                //            foreach (var KeywordStructData1_item in KeywordStructData1)
                //            {
                //                if (datacounter == int.Parse(txt_FollowByPerAccount.Text))
                //                {
                //                    break;
                //                }
                //                else
                //                {
                //                    KeywordStructData.Add(KeywordStructData1_item);
                //                    datacounter++;
                //                }
                //            }
                //        }
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("Please Upload Keywords");
                //    //break;
                //}
                
                #endregion
                //*************
                try
                {
                    if (KeywordStructData.Count > 0)
                    {
                        TweetAccountManager AccountManager = (TweetAccountManager)item.Value;
                        AccountManager.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);

                        if (!AccountManager.IsLoggedIn)
                        {
                            AccountManager.Login();
                        }

                        if (!AccountManager.IsLoggedIn || !AccountManager.IsNotSuspended)
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Not Logged in From Account =  " + AccountManager.Username + " . ]");
                            return;
                        }

                        ///Return if Suspended
                        if (AccountManager.AccountStatus == "Account Suspended")
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + profileUsername + " : Suspended ]");
                            return;
                        }
                        else if ((AccountManager.globusHttpHelper.gResponse.ResponseUri.ToString().ToLower()).Contains("captcha"))
                        {
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ " + profileUsername + " : Asking for captcha. ]");
                            return;
                        }

                        foreach (var item1 in KeywordStructData)
                        {
                            if (!chk_followbysinglekeywordperaccount.Checked)
                            {
                                //accordint to get Follow users limits 
                                if (FollowLimitCounter >= (int.Parse(txt_FollowByPerAccount.Text)))
                                {
                                    FollowLimitCounter = 0;
                                    break;
                                }
                                else
                                {
                                    FollowLimitCounter++;
                                }
                            }

                            //Get follow from user
                            string AccountId = item1.ID_Tweet_User;

                            //get Follow 
                            getFollowUserBySearch(new object[] { AccountManager, AccountId });

                            int Delay = RandomNumberGenerator.GenerateRandom((DelayStart * 1000), (DelayEnd * 1000));
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Delay :- " + TimeSpan.FromMilliseconds(Delay).Seconds + " Seconds. ]");
                            Thread.Sleep(Delay);
                        }
                    }
                    else
                    {
                        //Message List is Empty

                        //AddToLog_Follower("Key Word File is Empty or Wrong Formate");
                        //break;
                    }
                }
                catch (Exception)
                {

                }

            }
            catch (Exception)
            {
            }
            finally
            {
                Interlocked.Decrement(ref counterThreadsFollowByKeyWord);
                lock (lockerThreadsFollowByKeyWord)
                {
                    Monitor.Pulse(lockerThreadsFollowByKeyWord);
                }
                if (counterThreadsFollowByKeyWord == 0)
                {
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToLog_Follower("-----------------------------------------------------------------------------------------------------------------------");
                }
            }

        }

        public void getFollowUserBySearch(object parameter)
        {
            try
            {
                if (FollowtweetKeywordList.Count > 0)
                {
                    string USerIdToFollow = string.Empty;

                    Array paramsArray = new object[1];

                    paramsArray = (Array)parameter;

                    //KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                    TweetAccountManager tweetAccountManager = (TweetAccountManager)paramsArray.GetValue(0);

                    USerIdToFollow = (string)paramsArray.GetValue(1);


                    if (!tweetAccountManager.IsLoggedIn)
                    {
                        tweetAccountManager.Login();
                    }

                    ///Return if Suspended
                    if (tweetAccountManager.AccountStatus == "Account Suspended")
                    {
                        //AddToLog_Follower(tweetAccountManager.Username + " : Suspended");
                        return;
                    }

                    if (tweetAccountManager.IsLoggedIn)
                    {
                        GlobusHttpHelper globusHttpHelper = new Globussoft.GlobusHttpHelper();

                        //string abc =  globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/typeahead.json?count=10&q=abc"), "https://twitter.com/","");
                        globusHttpHelper = tweetAccountManager.globusHttpHelper;

                        Follower.Follower Follower = new global::Follower.Follower();
                        string status = "";
                        Follower.FollowUsingProfileID_New(ref tweetAccountManager.globusHttpHelper, "", tweetAccountManager.postAuthenticityToken, USerIdToFollow, out status);

                        if (status == "followed")
                        {
                            Console.WriteLine("followed");
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Followed User >>> " + USerIdToFollow + " From " + tweetAccountManager.Username + " ]");

                            string username = string.Empty;
                            string userid = string.Empty;
                            try
                            {
                                if (NumberHelper.ValidateNumber(USerIdToFollow))
                                {
                                    string outStatus = string.Empty;
                                    username = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUserIdToUserName_New(USerIdToFollow, out outStatus, ref globusHttpHelper);
                                    userid = USerIdToFollow;
                                }
                                else
                                {
                                    string outStatus = string.Empty;
                                    userid = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUsernameToUserID_New(USerIdToFollow, out outStatus, ref globusHttpHelper);
                                    username = USerIdToFollow;
                                }

                                objclsSettingDB.InsertUpdateFollowTable(tweetAccountManager.Username, userid, username);
                            }
                            catch (Exception)
                            {
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(tweetAccountManager.Username + ":" + tweetAccountManager.Password + ":" + username + ":" + userid + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_SuccessfullyFollowAccounts);
                        }
                        else if (status == "Already Followed")
                        {
                            Console.WriteLine("Already followed From =>" + tweetAccountManager.Username);
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Already Followed User >>> " + USerIdToFollow + " From " + tweetAccountManager.Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(tweetAccountManager.Username + ":" + tweetAccountManager.Password + ":" + "" + ":" + USerIdToFollow + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_FailedToFollowAccounts);
                        }

                        else
                        {
                            Console.WriteLine("not followed From =>" + tweetAccountManager.Username);
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Not Followed User >>> " + USerIdToFollow + " From " + tweetAccountManager.Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(tweetAccountManager.Username + ":" + tweetAccountManager.Password + ":" + "" + ":" + USerIdToFollow + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_FailedToFollowAccounts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " Mathod:- getFollowUserBySearch -->Error :- " + ex.Message, Globals.Path_FollowerErroLog);
            }
        }

        List<string> FollowtweetKeywordList = new List<string>();

        private void btn_FollowKeyWordsFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_FollowBySearchKey.Text = ofd.FileName;
                        FollowtweetKeywordList = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                    }
                }
            }
            catch { }

            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Keywords Uploaded.. ]");
            AddToLog_Follower("[ " + DateTime.Now + " ] => [ No of Keywords =" + FollowtweetKeywordList.Count + " ]");
        }

        private void chkbosUseHashTags_CheckedChanged(object sender, EventArgs e)
        {
            #region commented
            //if (Globals.HashTags.Count > 0)
            //{
            //    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Hash Tags In List ]");
            //}
            //else if (Globals.HashTags.Count == 0)
            //{
            //    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ No Hash Tags In List. Redirecting To Scrape Users For Hash Tag List ]");
            //    MessageBox.Show("No Hash Tags In List. Redirecting To Scrape Users For Hash Tag List");
            //    Thread.Sleep(3000);
            //    Tb_AccountManager.SelectedIndex = 6;
            //} 
            #endregion
            if (chkbosUseHashTags.Checked)
            {
                if (Globals.HashTags.Count > 0)
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Hash Tags In List ]");
                }
                else if (Globals.HashTags.Count == 0)
                {
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ No Hash Tags In List. Redirecting To Scrape Users For Hash Tag List ]");
                    DialogResult dr = MessageBox.Show("No Hash Tags In List. Redirecting To Scrape Users For Hash Tag List", "Measssage", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dr == DialogResult.OK)
                    {
                        //Thread.Sleep(3000);

                        Tb_AccountManager.SelectedIndex = 6;
                    }

                }
            }
            else
            {

            }

        }

        List<Thread> lstThreadGetHashTags = new List<Thread>();
        private void btnGetHashTags_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Starting Hash Tag Scrape ]");
                new Thread(() =>
                {
                    getHashTags();
                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void getHashTags()
        {
            TwitterDataScrapper dataScrape = new TwitterDataScrapper();
            try
            {
                try
                {
                    lstThreadGetHashTags.Add(Thread.CurrentThread);
                    Thread.CurrentThread.IsBackground = true;
                    lstThreadGetHashTags = lstThreadGetHashTags.Distinct().ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }

                Globals.HashTags.Clear();
              
                string returnStatus = string.Empty;
                //dataScrape.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                dataScrape.logEvents.addToLogger += new EventHandler(DataScraperlogger_addToLogger);
                Globals.HashTags = dataScrape.GetHashTags_New(out returnStatus);
                dataScrape.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
                //dataScrape.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);

                try
                {
                    if (Globals.HashTags.Count > 0)
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Trending Hash Tags ]");
                        AddToLog_Tweet("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Hash Tags In List ]");
                        foreach (string data in Globals.HashTags)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_HashtagsStore);
                        }
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Hash tag Finished ]");
                        AddToScrapeLogs("-----------------------------------------------------------------------------------------------------------------------");

                        if (chkbosUseHashTags.Checked)
                        {
                            btnGetHashTags.Invoke(new MethodInvoker(delegate
                            {
                                Tb_AccountManager.SelectedIndex = 1;
                            }));
                        }
                    }
                    else
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
                    }
                }
                catch { }
            }
            catch { }
            finally
            {
                dataScrape.logEvents.addToLogger -= new EventHandler(DataScraperlogger_addToLogger);
            }
        }

        private void chkBoxUseGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxUseGroup.Checked)
            {
                txtTweetUseGroup.Enabled = true;
            }
            else if (!chkBoxUseGroup.Checked)
            {
                txtTweetUseGroup.Enabled = false;
            }
        }

        #region Get user name from Account  in accunt actions Module

        private void btn_EmailListUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //Get Upload Email list 
                int IncorrectEmail = 0;
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_EmailListFilePath.Text = ofd.FileName;
                        List<string> _tempList = new List<string>();
                        listOfTwitterEmailIdAndPassword.Clear();
                        _tempList.AddRange(Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName));

                        foreach (var _tempList_item in _tempList)
                        {
                            if (_tempList_item.Contains(":"))
                            {
                                string CheckEmail = _tempList_item;
                                CheckEmail = CheckEmail.Split(':')[0];
                                CheckEmail = CheckEmail.Replace(":", "").Replace(".", "");
                                bool ISEmail = NumberHelper.ValidateNumber(CheckEmail);

                                if (!ISEmail)
                                {
                                    listOfTwitterEmailIdAndPassword.Add(_tempList_item);
                                }
                                else
                                {
                                    IncorrectEmail = IncorrectEmail + 1;
                                }
                            }
                        }
                    }
                }

                AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + listOfTwitterEmailIdAndPassword.Count + " Correct Emails Uploaded ]");

                if (IncorrectEmail > 0)
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + IncorrectEmail + " Incorrect Emails Uploaded ]");
                }


            }
            catch (Exception ex)
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Emails uploding Fail ]");
            }
        }

        private void btn_CheckEmailIdStart_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerUserNameFromAccount.Enabled = true;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txt_EmailListFilePath.Text))
                {
                    objclsSettingDB.InsertOrUpdateSetting("AccountChecker", "EmailListFilePath", StringEncoderDecoder.Encode(txt_EmailListFilePath.Text));
                }

                if (listOfTwitterEmailIdAndPassword.Count > 0)
                {
                    new Thread(() =>
                    {
                        getLoginandCheckAccount();
                    }).Start();
                }
                else
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Upload Email Text File ]");
                    MessageBox.Show("Please Upload Email Text File ");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void getLoginandCheckAccount()
        {
            AddToLog_Checker("[ " + DateTime.Now + " ] => [ Process start ]");

            try
            {
                Lst_GetUserNameFromAccThreads.Add(Thread.CurrentThread);
                GetUserNameFromAccThreadStart = true;
                Thread.CurrentThread.IsBackground = true;
            }
            catch { };


            try
            {
                //Get individual Email from Email list 

                foreach (string item in listOfTwitterEmailIdAndPassword)
                {
                    TweetAccountManager tweetAccountManager = new TweetAccountManager();

                    GlobusHttpHelper globushttpHelper1 = new GlobusHttpHelper();

                    if (item.Contains(":") && !string.IsNullOrEmpty(item.Split(':')[0]) && !string.IsNullOrEmpty(item.Split(':')[1]))
                    {
                        string userEmail = item.Split(':')[0];
                        string userEmailPass = item.Split(':')[1];


                        tweetAccountManager.Username = userEmail;
                        tweetAccountManager.Password = userEmailPass;

                        tweetAccountManager.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);
                        if (!tweetAccountManager.IsLoggedIn)
                        {
                            tweetAccountManager.Login();
                        }

                        if (tweetAccountManager.IsLoggedIn)
                        {
                            //AddToLog_Checker("Log in Success " + userEmail);

                            if (!tweetAccountManager.IsNotSuspended)
                            {
                                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account Is suspended :-" + userEmail + " ]");
                                continue;
                            }

                            String userID = tweetAccountManager.userID;

                            String ScreanName = tweetAccountManager.Screen_name;

                            if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(ScreanName))
                            {
                                if (!File.Exists(Globals.Path_CheckAccountByEmail))
                                {
                                    string Header = "Email" + "," + "User Id" + "," + "Screan Name";
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Header, Globals.Path_CheckAccountByEmail);
                                }

                                string CheckAccountByEmail_Data = userEmail + "," + userID + "," + ScreanName;
                                if (!string.IsNullOrEmpty(userEmail))
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(CheckAccountByEmail_Data, Globals.Path_CheckAccountByEmail);
                                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + userEmail + "-" + userID + "-" + ScreanName + " ]");
                                }
                            }

                            #region Old code from API 1
                            //string URL = string.Empty;
                            ////get data from API Url ...
                            //if (!string.IsNullOrEmpty(tweetAccountManager.userID))
                            //{
                            //    string userID1 = tweetAccountManager.userID;
                            //    URL = "https://api.twitter.com/1/users/show.json?user_id=" + userID1 + "&include_entities=true";
                            //}
                            //else if (!string.IsNullOrEmpty(tweetAccountManager.Screen_name))
                            //{
                            //    string user_screen_name = tweetAccountManager.Screen_name;
                            //    URL = "https://api.twitter.com/1/users/show.json?screen_name=" + user_screen_name + "&include_entities=true";
                            //}
                            //else
                            //{
                            //    AddToLog_Checker("User Id or Screan name is Not avaliable For :- " + userEmail);
                            //    //continue;
                            //}


                            //try
                            //{
                            //    string Data = globushttpHelper1.getHtmlfromUrl(new Uri(URL), "", "");


                            //    //parse data from Json..
                            //    JObject JData = JObject.Parse(Data);
                            //    string ScreanName = (string)JData["screen_name"];
                            //    string userID = ((int)JData["id"]).ToString();
                            //    //get write data in CSV file ...
                            //    if (!string.IsNullOrEmpty(userID) && !string.IsNullOrEmpty(ScreanName))
                            //    {
                            //        if (!File.Exists(Globals.Path_CheckAccountByEmail))
                            //        {
                            //            string Header = "Email" + "," + "User Id" + "," + "Screan Name";
                            //            GlobusFileHelper.AppendStringToTextfileNewLine(Header, Globals.Path_CheckAccountByEmail);
                            //        }

                            //        string CheckAccountByEmail_Data = userEmail + "," + userID + "," + ScreanName;
                            //        if (!string.IsNullOrEmpty(userEmail))
                            //        {
                            //            GlobusFileHelper.AppendStringToTextfileNewLine(CheckAccountByEmail_Data, Globals.Path_CheckAccountByEmail);
                            //            AddToLog_Checker(userEmail + "-" + userID + "-" + ScreanName);
                            //        }
                            //    }
                            //}
                            //catch (Exception)
                            //{

                            //}
                            #endregion
                        }
                    }
                    else
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account Format is wrong for :- " + item + " ]");
                    }
                }
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Account Checking Finished ]");
                
                AddToLog_Checker("-----------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception)
            {
            }

            finally
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Finished Getting Username For all Emails ]");
                AddToLog_Checker("-----------------------------------------------------------------------------------------------------------------------");
            }
        }

        #endregion

        #region Assign Groups To Accounts

        private void btnGroupName_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtUploadGroupNames.Text = ofd.FileName;
                    lstGroupNames = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ " + lstGroupNames.Count + " Groups Uploaded ]");
                }
                else
                {
                    AddToLog_Checker("[ " + DateTime.Now + " ] => ["+ " You have not uploaded any group name ]");
                }

            }
            
        }

        private void btnAssignGroup_Click(object sender, EventArgs e)
        {
            btn_StopAccountAcc.Enabled = true;
            if (!string.IsNullOrEmpty(txtUploadGroupNames.Text))
            {
                objclsSettingDB.InsertOrUpdateSetting("AccountChecker", "UploadGroupNames", StringEncoderDecoder.Encode(txtUploadGroupNames.Text));
            }

            if (lstGroupNames.Count > 0)
            {
                new Thread(() =>
                {
                    AssignGroupsToAccounts();
                }).Start();
            }
            else
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Upload Group Names ]");
            }
        }

        public void AssignGroupsToAccounts()
        {
            try
            {
                Lst_AssignGroupThreads.Add(Thread.CurrentThread);
                AssignGroupThreadStart = true;
                Thread.CurrentThread.IsBackground = true;
            }
            catch { };

            try
            {
                int CountPerGroup = 0;
                int counter = 0;
                int listCounter = 0;
                bool IscounterPerGroup = true;
                txtAccountPerGroup.Invoke(new MethodInvoker(delegate
                {
                    if (!string.IsNullOrEmpty(txtAccountPerGroup.Text) && NumberHelper.ValidateNumber(txtAccountPerGroup.Text))
                    {
                        CountPerGroup = Convert.ToInt32(txtAccountPerGroup.Text);
                    }
                    else 
                    {
                        IscounterPerGroup = false;
                    }
                }));

                if (IscounterPerGroup)
                {
                    if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                    {
                        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                        {
                            if (listCounter <= lstGroupNames.Count - 1)
                            {
                                if (counter < CountPerGroup)
                                {

                                }
                                else
                                {
                                    counter = 0;
                                    listCounter++;
                                    if (listCounter == lstGroupNames.Count)
                                    {
                                        break;
                                    }
                                }

                                string GroupName = string.Empty;
                                GroupName = lstGroupNames[listCounter];
                                clsDBQueryManager Db = new clsDBQueryManager();
                                Db.InsertAccountGroupName(item.Key, GroupName);
                                try
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(item.Key + " , " + GroupName, Globals.path_Group_Name);
                                }
                                catch { };
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please Upload Accounts To Assign Accounts ]");
                    }
                }
                else 
                {
                    MessageBox.Show("Please enter valid number in assign accounts per group.");
                    AddToLog_Checker("[ " + DateTime.Now + " ] => [ Please enter valid number in assign accounts per group. ]");
                    return;
                }

                AddToLog_Checker("[ " + DateTime.Now + " ] => [ Finished Adding Groups To Accounts ]");
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddToLog_Checker("-----------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> AssignGroupsToAccounts() --> " + ex.Message, Globals.Path_AccountCheckerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> AssignGroupsToAccounts() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }



        private void btnClearGroupNames_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                DeleteAllGroupNames();
            }).Start();
        }


        public void DeleteAllGroupNames()
        {
            clsDBQueryManager db = new clsDBQueryManager();
            DataSet ds = db.SelectAllData();

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dRow in dt.Rows)
                {
                    try
                    {
                        db.UpdateGroupName();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                AddToLog_Checker("[ " + DateTime.Now + " ] => [ All Group Names Removed. ]");
            }
            else
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ No Accounts Group Name To Be Removed. ]");
            }
        }


        #endregion

        #region waitandreplyscheduler

        private void btn_waitandreplyscheduler_Click(object sender, EventArgs e)
        {
            try
            {
                //InsertUpdateSchedulerModule(strModule(Module.Tweet), dateTimePicker_Follower.Value.ToString());
                //MessageBox.Show("Please Upload All Data To Start Wait And Reply");

                if (CheckWaitAndReply())
                {
                    string IsScheduledDaily = "0";
                    if (chkScheduleDaily_waitandreply.Checked)
                    {
                        IsScheduledDaily = "1";
                    }
                    clsDBQueryManager queryManager = new clsDBQueryManager();
                    queryManager.InsertUpdateTBScheduler("", "WaitAndReply_", dateTimePicker_WaitandreplyStart.Value.ToString(), IsScheduledDaily);

                    if (scheduler != null && scheduler.IsDisposed == false)
                    {
                        scheduler.LoadDataGrid();
                    }



                    string wait_replyFilename = txtReplyMsgFile.Text;
                    string wait_replytweetfilename = txtTweetMsgFile.Text;
                    string wait_replyKeyword = txtKeyword.Text;
                    string wait_replyKeywordFile = txtKeywordFile.Text;

                    string strchkUseSpinnedMessages = "0";
                    if (chkUseSpinnedMessages.Checked)
                    {
                        strchkUseSpinnedMessages = "1";
                    }

                    string strtweetsperreply = "4";
                    if (txtNoOfTweetsperReply.Text != strtweetsperreply)
                    {
                        strtweetsperreply = txtNoOfTweetsperReply.Text;
                    }


                    string strNoOfRepliesPerKeyword = "10";
                    if (txtNoOfRepliesPerKeyword.Text != strNoOfRepliesPerKeyword && int.Parse(txtNoOfRepliesPerKeyword.Text) != 0)
                    {
                        strNoOfRepliesPerKeyword = txtNoOfRepliesPerKeyword.Text;
                    }

                    string strInterval_WaitReply = "10";
                    if (txtInterval_WaitReply.Text != strInterval_WaitReply && int.Parse(txtInterval_WaitReply.Text) != 0)
                    {
                        strInterval_WaitReply = txtInterval_WaitReply.Text;
                    }

                    string strWaitReplyThreads = "7";
                    if (txtWaitReplyThreads.Text != txtWaitReplyThreads.Text && int.Parse(txtWaitReplyThreads.Text) != 0)
                    {
                        strWaitReplyThreads = txtWaitReplyThreads.Text;
                    }

                    GlobusFileHelper.WriteStringToTextfile(wait_replyFilename + "<:>"
                        + wait_replytweetfilename + "<:>" + wait_replyKeyword + "<:>" + wait_replyKeywordFile + "<:>" + chkUseSpinnedMessages + "<:>"
                        + strtweetsperreply + "<:>" + strNoOfRepliesPerKeyword + "<:>" + strInterval_WaitReply + "<:>" + strWaitReplyThreads, Globals.Path_waitandreplySetting);

                    MessageBox.Show("Task Scheduled");
                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Task Scheduled ]");


                }
                else
                {
                    AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Please Add All Data ]");
                }

            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btn_waitandreplyscheduler_Click() -- ScheduleFor Later Click --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btn_waitandreplyscheduler_Click() -- ScheduleFor Later Click --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
            }
        }


        public bool CheckWaitAndReply()
        {
            if ((!string.IsNullOrEmpty(txtReplyMsgFile.Text) || listReplyMessages.Count > 0 || !string.IsNullOrEmpty(txtTweetMsgFile.Text) || listTweetMessages.Count > 0) && !string.IsNullOrEmpty(txtInterval_WaitReply.Text) && NumberHelper.ValidateNumber(txtInterval_WaitReply.Text) && !string.IsNullOrEmpty(txtNoOfRepliesPerKeyword.Text) && NumberHelper.ValidateNumber(txtNoOfRepliesPerKeyword.Text) && !string.IsNullOrEmpty(txtInterval_WaitReply.Text) && NumberHelper.ValidateNumber(txtInterval_WaitReply.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please Fill All Data Required For Reply");
                AddToLog_WaitReply("[ " + DateTime.Now + " ] => [ Please Fill All Data Required For Reply ]");
                return false;
            }

        }

        #endregion


        public static bool IsAccountopen = true;

        private void accountsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //frmAccounts frmaccounts = new frmAccounts();
            //frmaccounts.Show();

            if (IsAccountopen)
            {
                IsAccountopen = false;
                frmAccounts frmaccounts = new frmAccounts();
                frmaccounts.Show();
            }
            else
            {
                MessageBox.Show("Account Form is already Open.", "Alert");
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Unfollower.IsSettingOpen_NewUi)
            {
                //if (frmSettings == null)
                {
                    frmSettings = new FrmSettings();
                }
                Unfollower.IsSettingOpen_NewUi = false;
                frmSettings.Show();
            }
            else
            {
                MessageBox.Show("Setting Form is already Open.", "Alert");
            }
        }

        private void schedulerToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //frmScheduler scheduler = new frmScheduler();
            if (scheduler != null && scheduler.IsDisposed == false)
            {
                if (Unfollower.IsSheduler_NewUi)
                {
                    scheduler.Show();
                    Unfollower.IsSheduler_NewUi = false;
                }
                else
                {
                    MessageBox.Show("scheduler Form is already Open.", "Alert");
                }
            }
            else
            {
                if (Unfollower.IsSheduler_NewUi)
                {
                    Unfollower.IsSheduler_NewUi = false;
                    scheduler = new frmScheduler();
                    scheduler.Show();
                }
                else
                {
                    MessageBox.Show("Sheduler Form is already Open.", "Alert");
                }
            }
        }

        private void btnstratBAckgroundPicChnage_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                AddToListProfile("[ " + DateTime.Now + " ] => [ Starting Background Image Editing ]");
                if (!string.IsNullOrEmpty(txtBackgroundPicPath.Text) && lstBackgroundPics.Count > 0)
                {
                    objclsSettingDB.InsertOrUpdateSetting("ProfileManager", "BackgroundPic", StringEncoderDecoder.Encode(txtBackgroundPicPath.Text));
                    new Thread(() => StartBackgroundImageChange()).Start();
                }
                else
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload Background Images ]");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartBackgroundImageChange()
        {
            int numberOfThreads = 7;
            //TweetAccountContainer.dictionary_TweetAccount.Add("key", new TweetAccountManager());
            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                if (!string.IsNullOrEmpty(txtNoOfProfileThreads.Text) && Globals.IdCheck.IsMatch(txtNoOfProfileThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNoOfProfileThreads.Text);
                }

                int Count_BackgroundImage = 0;
                string BackgroundIamge = string.Empty;

                ThreadPool.SetMaxThreads(numberOfThreads, 5);

                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ Starting Editing Background Image : " + item.Key + " ]");
                    ThreadPool.SetMaxThreads(numberOfThreads, 5);
                    try
                    {
                        if (Count_BackgroundImage < lstBackgroundPics.Count)
                        {
                            BackgroundIamge = lstBackgroundPics[Count_BackgroundImage];
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Background Image : " + BackgroundIamge + " ]");
                        }
                        else
                        {
                            Count_BackgroundImage = 0;
                            BackgroundIamge = lstBackgroundPics[Count_BackgroundImage];
                            AddToListProfile("[ " + DateTime.Now + " ] => [ " + item.Key + " >>>> Background Image : " + BackgroundIamge + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.AddToErrorLogText(ex.Message);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartProfileCreation() -- chkboxUsername --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartProfileCreation()  -- chkboxUsername --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod_BackgroundImageManager), new object[] { item, BackgroundIamge });
                    Count_BackgroundImage++;
                }
            }
            else
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Please Upload Accounts ]");
            }
        }

        public void ThreadPoolMethod_BackgroundImageManager(object parameter)
        {
            try
            {
                Thread.CurrentThread.IsBackground = true;

                Array paramsArray = new object[2];

                paramsArray = (Array)parameter;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);

                string BackgroundIamge = (string)paramsArray.GetValue(1);

                TweetAccountManager AccountManager = keyValue.Value;

                //Add to Threads Dictionary
                AddThreadToDictionary(strModule(Module.ProfileManager), AccountManager.Username);

                AccountManager.logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);
                AccountManager.profileUpdater.logEvents.addToLogger += logEvents_Profile_addToLogger;

                AccountManager.BackgroundPic(BackgroundIamge);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ThreadPoolMethod_BackgroundImageManager() --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ThreadPoolMethod_BackgroundImageManager() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnBackGrowndPic_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog ofd = new FolderBrowserDialog())
                {
                    ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtBackgroundPicPath.Text = ofd.SelectedPath;
                        lstBackgroundPics = new List<string>();
                        string[] picsArray = Directory.GetFiles(ofd.SelectedPath);
                        foreach (string picFile in picsArray)
                        {
                            if (picFile.Contains(".jpg") || picFile.Contains(".gif") || picFile.Contains(".png"))
                            {
                                lstBackgroundPics.Add(picFile);
                            }
                            else
                            {
                                AddToListProfile("[ " + DateTime.Now + " ] => [ Not Correct Image File ]");
                                AddToListProfile("[ " + DateTime.Now + " ] => [ " + picFile + " ]");
                            }
                        }

                        Console.WriteLine(lstBackgroundPics.Count + " Background Pics loaded");
                        AddToListProfile("[ " + DateTime.Now + " ] => [ " + lstBackgroundPics.Count + " Background Images loaded ]");
                    }
                }
            }
            catch { }
        }

        private void btnStopBackgroundPIc_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping ProfileManager Threads, it may take some time ]");
                StopThreads(strModule(Module.ProfileManager));
            }).Start();
        }



        private void btnStopAccountCreation_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {

                    new Thread(() =>
                    {
                        try
                        {
                            TwitterSignup.TwitterSignUp.AbortThreads();
                        }
                        catch { };
                    }).Start();
                    StopThreads(strModule(Module.AccountCreation));
                }).Start();

                //AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Account Creation Threads, it may take some time ]");
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
            }
            catch { }
        }

        private void chkboxUseGroups_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxUseGroups.Checked)
            {
                txtUseGroup.Enabled = true;
            }
            else if (!chkboxUseGroups.Checked)
            {
                txtUseGroup.Enabled = false;
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Scraping Follower/Following Stopped ]");
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Thread.It may Take Some Time Please Wait ]");
            try
            {
                if (thread_StartScrape != null)
                {
                    thread_StartScrape.Abort();
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Process is stopped ]");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnStopScrapeKeyword_Click(object sender, EventArgs e)
        {
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Scraping Keyword Stopped ]");
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Thread.It may Take Some Time Please Wait ]");
            try
            {
                if (thread_StartKeywordScrape != null)
                {
                    thread_StartKeywordScrape.Abort();
                }
                if (thread_AddingKeywordScrape != null)
                {
                    thread_AddingKeywordScrape.Abort();
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnStopUsernameToUserID_Click(object sender, EventArgs e)
        {
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Scraping Username To Userid Stopped ]");
            AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Thread.It may Take Some Time Please Wait ]");
            try
            {
                if (thread_UsernameToUserid != null)
                {
                    thread_UsernameToUserid.Abort();
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Process is stopped ]");
                }
                if (thread_UsernameValue != null)
                {
                    thread_UsernameValue.Abort();
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Process is stopped ]");
                }
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                List<Thread> tempThread = lstThreadGetHashTags;
                foreach (Thread item in tempThread)
                {
                    item.Abort();
                }

                AddToScrapeLogs("Process Stopped !");
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Process is stopped ]");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.Message);
            }
        }

        private void chkboxUseUrlIP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkboxUseUrlIP.Checked)
            {
                txtIPUrl.Enabled = true;
            }
            else if (!chkboxUseUrlIP.Checked)
            {
                txtIPUrl.Enabled = false;
            }
        }

        private void campaignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCampaign frmCamp = new FrmCampaign();
            frmCamp.Show();
        }

        void startFlyCreationEvent_addToLogger(object sender, EventArgs e)
        {
            try
            {
                int numberOfThreads = 8;

                if (e is EventsArgs)
                {
                    EventsArgs args = e as EventsArgs;

                    Regex IdCheck1 = new Regex("^[0-9]*$");

                    if (IdCheck1.IsMatch(args.log) && !string.IsNullOrEmpty(args.log))
                    {
                        numberOfThreads = int.Parse(args.log);
                    }
                    if (numberOfThreads < 4)
                    {
                        numberOfThreads = 4;
                    }
                }
                new Thread(() =>
                {
                    StartCampaign();
                }).Start();
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Campaign --> startFlyCreationEvent_addToLogger() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        public void StartCampaign()
        {
            try
            {
                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Starting Campaign ]");
                string KeywordList = string.Empty;
                string UsernameList = string.Empty;
                if (Globals.Array[3] == "Username")
                {
                    Globals.Campaign_Name = Globals.Array[0];
                    Globals.IsCampaign = true;
                    AddToLog_Follower("[ " + DateTime.Now + " ] => [ Getting Followers & Following ]");
                    UsernameList = Globals.Array[1];
                    ReloadAccountsFromDataBase();
                    Globussoft.GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                    if (File.Exists(UsernameList))
                    {
                        List<string> LstUsernameCampiagn = GlobusFileHelper.ReadLargeFile(UsernameList);
                        listUserIDs = new List<string>();
                        List<string> ListUsername = new List<string>();
                        foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                        {
                            if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                            {
                                counter = 0;
                            }
                            TweetAccountManager TweetLogin = new TweetAccountManager();
                            TweetLogin.Username = item.Key;
                            TweetLogin.Password = item.Value.Password;
                            TweetLogin.IPAddress = item.Value.IPAddress;
                            TweetLogin.IPPort = item.Value.IPPort;
                            TweetLogin.IPUsername = item.Value.IPUsername;
                            TweetLogin.IPpassword = item.Value.IPpassword;
                            TweetLogin.Login();
                            if (!TweetLogin.IsNotSuspended)
                            {
                                continue;
                            }
                            globusHttpHelper = TweetLogin.globusHttpHelper;
                            counter++;
                            break;
                        }
                        foreach (string keyword in LstUsernameCampiagn)
                        {
                            TwitterDataScrapper dataScrapeer = new TwitterDataScrapper();
                            try
                            {
                                string FollowerStatus = string.Empty;
                                string FollowingStatus = string.Empty;
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Scraping Followers For : " + keyword + " ]");
                                dataScrapeer.CounterDataNo = 2000;
                                dataScrapeer.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                                List<string> followerList = dataScrapeer.GetFollowers_New(keyword, out FollowerStatus, ref globusHttpHelper);

                                if (followerList.Count != 0)
                                {
                                    try
                                    {
                                        List<string> Temp_followers = new List<string>();
                                        followerList.ForEach(s =>
                                        {
                                            if (s.Contains(":"))
                                            {
                                                Temp_followers.Add(s.Split(':')[0]);
                                            }
                                            else
                                            {
                                                Temp_followers.Add(s.ToString());
                                            }
                                        });

                                        if (Temp_followers.Count != 0)
                                        {
                                            followerList.Clear();
                                            followerList.AddRange(Temp_followers);
                                        }
                                    }
                                    catch { };
                                }
                                followerList.ForEach(s => s.Split(':'));
                                dataScrapeer.logEvents.addToLogger -= new EventHandler(logEvents_Follower_addToLogger);
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Follower For " + keyword + " >> " + followerList.Count + " ]");
                                if (FollowerStatus == "No Error")
                                {
                                    foreach (string lst in followerList)
                                    {
                                        listUserIDs.Add(lst);
                                    }
                                }
                                Thread.Sleep(5000);
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Scraping Followings For : " + keyword + " ]");
                                dataScrapeer.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                                List<string> followingList = dataScrapeer.GetFollowings_New(keyword, out FollowingStatus, ref globusHttpHelper);
                                dataScrapeer.logEvents.addToLogger -= new EventHandler(logEvents_Follower_addToLogger);
                                AddToLog_Follower("[ " + DateTime.Now + " ] => [ Following For " + keyword + " >> " + followingList.Count + " ]");
                                if (FollowingStatus == "No Error")
                                {
                                    foreach (string lst in followerList)
                                    {
                                        listUserIDs.Add(lst);
                                    }
                                }
                                Thread.Sleep(5000);
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartingCampaign() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartingCampaign() -- chkboxScrapeFollowers.Checked --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }

                        StartFollowing();
                    }
                    else
                    {
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ File : " + UsernameList + ". Does Not Exsist ]");
                    }
                }
                else if (Globals.Array[3] == "Keyword")
                {
                    KeywordList = Globals.Array[2];
                    ReloadAccountsFromDataBase();
                    if (File.Exists(KeywordList))
                    {
                        List<string> KeywordListdata = GlobusFileHelper.ReadLargeFile(KeywordList);
                        List<TwitterDataScrapper.StructTweetIDs> ids = new List<TwitterDataScrapper.StructTweetIDs>();
                        foreach (string id in KeywordListdata)
                        {
                            TwitterDataScrapper dataScrpeer = new TwitterDataScrapper();
                            TwitterDataScrapper.noOfRecords = 2000;
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Scraping Users From Keyword : " + id + " ]");
                            dataScrpeer.logEvents.addToLogger += new EventHandler(logEvents_Follower_addToLogger);
                            ids = dataScrpeer.NewKeywordStructData(id);
                            dataScrpeer.logEvents.addToLogger -= new EventHandler(logEvents_Follower_addToLogger);
                            AddToLog_Follower("[ " + DateTime.Now + " ] => [ Total Users From Keyword : " + id + " : " + ids.Count + " ]");
                            foreach (TwitterDataScrapper.StructTweetIDs lst in ids)
                            {
                                listUserIDs.Add(lst.ID_Tweet_User);
                            }
                        }
                        StartFollowing();
                    }
                    else
                    {
                        AddToLog_Follower("[ " + DateTime.Now + " ] => [ File : " + KeywordList + ".Does Not Exsist ]");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Campaign --> startFlyCreationEvent_addToLogger() --> " + ex.Message, Globals.Path_CampaignManager);
            }
        }

        private void chkboxChangeExportTweet_CheckedChanged_1(object sender, EventArgs e)
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
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ File Does Not Exsists ]");
                        txtTweetExportLocation.Text = Globals.Path_TweetExtractor;
                    }
                }
                else
                {
                    txtTweetExportLocation.Text = Globals.Path_TweetExtractor;
                }
            }
        }

        private void chkboxExportLocationRetweet_CheckedChanged_1(object sender, EventArgs e)
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
                        AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ File Does Not Exsists ]");
                        txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                    }
                }
                else
                {
                    txtExportLocationRewteet.Text = Globals.Path_RETweetExtractor;
                }
            }
        }



        private void btn_searchBykeyword_Click(object sender, EventArgs e)
        {
            Dictionary<string, Thread> Dic_Temp = new Dictionary<string, Thread>();
            try
            {
                foreach (KeyValuePair<string, Thread> item in Dic_Thread)
                {
                    try
                    {
                        Dic_Temp.Add(item.Key, item.Value);
                    }
                    catch
                    {
                    }
                }
                foreach (KeyValuePair<string, Thread> item in Dic_Temp)
                {
                    try
                    {
                        item.Value.Abort();
                        Dic_Thread.Remove(item.Key);

                        AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Aborting : " + item.Key + " ]");
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }

        private void replyInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //frmReplyInterface obj_frmReplyInterface = new frmReplyInterface();
                //obj_frmReplyInterface.Show();
            }
            catch
            {
            }
        }

        private void replyInteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmReplyInterface obj_frmReplyInterface = new frmReplyInterface();
                obj_frmReplyInterface.Show();
            }
            catch
            {
            }
        }


        /// <summary>
        /// Tab White/black listed User 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        List<string> lst_ofScrapeduserAccounts = new List<string>();
        List<string> TemP_Lst_ofScrapeduserAccounts = new List<string>();
        List<string> ListofLocationsettingFile = new List<string>();
        List<string> ListofProfileInfosettingFile = new List<string>();
        Thread thread_StartFilter = null;

        private void btnUersListUpload_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_AccountListFileUpload.Invoke(new
                    MethodInvoker(delegate
                    {
                        txt_AccountListFileUpload.Text = ofd.FileName;
                    }));

                    lst_ofScrapeduserAccounts = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                }
            }
            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts.Count + " Users Id uploaded. ]");
        }

        private void btn_uploadPofileInfoSettingFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtbox_ProfileInfoFilePath.Invoke(new
                    MethodInvoker(delegate
                    {
                        txtbox_ProfileInfoFilePath.Text = ofd.FileName;
                    }));

                    ListofProfileInfosettingFile = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                }
            }
            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + ListofProfileInfosettingFile.Count + " Profile info Uploaded. ]");

        }

        private void btn_uploadLocationSettingFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtbox_LocationFilePath.Invoke(new
                    MethodInvoker(delegate
                    {
                        txtbox_LocationFilePath.Text = ofd.FileName;
                    }));

                    ListofLocationsettingFile = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                }
            }
            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + ListofLocationsettingFile.Count + " Location Uploaded. ]");
        }

        private void chk_scrapeBykeywordSettingLocation_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_scrapeBykeywordSettingLocation.Checked)
            {
                txtbox_LocationFilePath.Enabled = true;
                btn_uploadLocationSettingFile.Enabled = true;
            }
            else 
            {
                txtbox_LocationFilePath.Enabled = false;
                btn_uploadLocationSettingFile.Enabled = false;
            }
        }
        private void btn_WhileAndBlacklistCheckStart_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                //if (!string.IsNullOrEmpty(txtNames_FakeEmailCreator.Text))
                //{
                //    objclsSettingDB.InsertOrUpdateSetting("WhileListBlackList", "UploadUserName", StringEncoderDecoder.Encode(txt_AccountListFileUpload.Text));
                //}


                if (!string.IsNullOrEmpty(txt_AccountListFileUpload.Text) && !string.IsNullOrEmpty(txt_AccountListFileUpload.Text.Trim()) || chk_scrapedUserCSVFileUpload.Checked)
                {
                    if (chk_scrapeBykeywordSettingTweets.Checked && !string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMin.Text.Trim()) && !string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMin.Text.Trim()) && NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMin.Text.Trim()) && NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMax.Text.Trim())
                        || chk_scrapeBykeywordSettingLocation.Checked && ListofLocationsettingFile.Count != 0 && pathValidation(txtbox_LocationFilePath.Text.Trim())
                        || chk_scrapeBykeywordSettingProfileInfo.Checked && ListofProfileInfosettingFile.Count != 0 && pathValidation(txtbox_ProfileInfoFilePath.Text.Trim())
                        || chk_FilterByFollowers.Checked && !string.IsNullOrEmpty(txt_FilterbyFollowersMin.Text.Trim()) && !string.IsNullOrEmpty(txt_FilterbyFollowersMax.Text.Trim()) && NumberHelper.ValidateNumber(txt_FilterbyFollowersMax.Text.Trim()) && NumberHelper.ValidateNumber(txt_FilterbyFollowersMin.Text.Trim())
                        || chk_FilterByFollowing.Checked && !string.IsNullOrEmpty(txt_FilterbyFollowingMin.Text.Trim()) && !string.IsNullOrEmpty(txt_FilterbyFollowingMax.Text.Trim()) && NumberHelper.ValidateNumber(txt_FilterbyFollowingMin.Text.Trim()) && NumberHelper.ValidateNumber(txt_FilterbyFollowingMax.Text.Trim()))
                    {
                        AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Filtration process is started. ]");

                        thread_StartFilter = new Thread(() =>
                        {
                            checkUser();
                        });
                        thread_StartFilter.Start();
                    }
                    else
                    {
                        AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Please choose at least one filtering option and required data. ]");
                        MessageBox.Show("Please choose at least one filtering option and required data.");
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload a file.");
                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Please Upload a file. ]");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void checkUser()
        {
            try
            {
                int tweetMaxCount=0, tweetMinCount=0, followerMinCount=0, followerMaxCount=0, followingMinCount=0, followingMaxCount=0;
                if (chk_scrapeBykeywordSettingTweets.Checked)
                {
                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Starting Filter by tweet wise]");
                    try
                    {
                        bool delaystatus = true;
                        if (!string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMin.Text) && NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMin.Text))
                        {
                            tweetMinCount = Convert.ToInt32(txt_ScrapebySearchkeywordsettingMin.Text);
                            delaystatus = false;
                        }

                        if (!string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMax.Text) && NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMax.Text))
                        {
                            tweetMaxCount = Convert.ToInt32(txt_ScrapebySearchkeywordsettingMax.Text);
                            delaystatus = false;
                        }

                        if (tweetMinCount > tweetMaxCount)
                        {
                            MessageBox.Show("Min Tweet value should be less than max Tweet");
                            delaystatus = false;
                            return;
                        }
                    }
                    catch { };

                }
                if (chk_FilterByFollowers.Checked)
                {
                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Starting Filter For Followers ]");

                    try
                    {
                        bool delaystatus = true;
                        if (!string.IsNullOrEmpty(txt_FilterbyFollowersMin.Text) && NumberHelper.ValidateNumber(txt_FilterbyFollowersMin.Text))
                        {
                            followerMinCount = Convert.ToInt32(txt_FilterbyFollowersMin.Text);
                            delaystatus = false;
                        }

                        if (!string.IsNullOrEmpty(txt_FilterbyFollowersMax.Text) && NumberHelper.ValidateNumber(txt_FilterbyFollowersMax.Text))
                        {
                            followerMaxCount = Convert.ToInt32(txt_FilterbyFollowersMax.Text);
                            delaystatus = false;
                        }

                        if (followerMinCount > followerMaxCount)
                        {
                            MessageBox.Show("Min Followers value should be less than max Followers");
                            delaystatus = false;
                            return;
                        }
                    }
                    catch { };
                }
                if (chk_FilterByFollowing.Checked)
                {
                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Starting Filter For followings ]");

                    try
                    {
                        bool delaystatus = true;
                        if (!string.IsNullOrEmpty(txt_FilterbyFollowingMin.Text) && NumberHelper.ValidateNumber(txt_FilterbyFollowingMin.Text))
                        {
                            followingMinCount = Convert.ToInt32(txt_FilterbyFollowingMin.Text);
                            delaystatus = false;
                        }

                        if (!string.IsNullOrEmpty(txt_FilterbyFollowingMax.Text) && NumberHelper.ValidateNumber(txt_FilterbyFollowingMax.Text))
                        {
                            followingMaxCount = Convert.ToInt32(txt_FilterbyFollowingMax.Text);
                            delaystatus = false;
                        }

                        if (followingMinCount > followingMaxCount)
                        {
                            MessageBox.Show("Min Following value should be less than max Following");
                            delaystatus = false;
                            return;
                        }
                    }
                    catch { };
                }

                //AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Starting Filter For Followers ]");
                Dictionary<string, string> usersDetails = new Dictionary<string, string>();
                //List<TwitterDataScrapper.StructTweetIDs> usersDetails = new List<TwitterDataScrapper.StructTweetIDs>();
                List<string> WhiteListUser = new List<string>();
                int countWhiteListedUser = 0;
                int countBlackListedUser = 0;
                int countUserNotExist = 0;
                TemP_Lst_ofScrapeduserAccounts.Clear();
                TemP_Lst_ofScrapeduserAccounts.AddRange(lst_ofScrapeduserAccounts);
                foreach (string lst_ofScrapeduserAccounts_item in lst_ofScrapeduserAccounts)
                {
                    try
                    {
                        usersDetails.Clear();
                        string status = string.Empty;
                        AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Getting Details For " + lst_ofScrapeduserAccounts_item + " ]");
                        usersDetails = TwitterDataScrapper.GetUserDetails(lst_ofScrapeduserAccounts_item, out status);

                        if (status.Contains("Rate limit exceeded"))
                        {
                            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + status + " ]");
                            continue;
                        }
                        else if (!status.Contains("Ok"))
                        {
                            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + status + " :- " + lst_ofScrapeduserAccounts_item + " ]");
                            continue;
                        }

                        if (usersDetails.Count != 0 && status.Contains("Ok"))
                        {
                            string userStatus = string.Empty;
                            try
                            {
                                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Comparing Details For " + lst_ofScrapeduserAccounts_item + " ]");
                                getCompareToCriteriaOfUser(usersDetails, lst_ofScrapeduserAccounts_item, out  userStatus);
                                if (userStatus == "WhiteListedUser")
                                {
                                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts_item + " White listed user. ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(lst_ofScrapeduserAccounts_item, Globals.Path_WhiteListedUser);
                                    countWhiteListedUser++;
                                }
                                else
                                {
                                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts_item + " Black listed user. ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(lst_ofScrapeduserAccounts_item, Globals.Path_BlackListedUser);
                                    countBlackListedUser++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Getting And Checking User details --> " + ex.Message, Globals.Path_FilterUsersLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting And Checking User details --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        else
                        {

                            AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts_item + " does not exist ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(lst_ofScrapeduserAccounts_item, Globals.Path_UserNotExistListedUser);
                            countUserNotExist++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Getting And Checking User details --> " + ex.Message, Globals.Path_FilterUsersLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Getting And Checking User details --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + countWhiteListedUser + " White listed user ]");
                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + countBlackListedUser + " Black listed user ]");
                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + countUserNotExist + "  user does not exist ]");





            }
            catch (Exception ex)
            {
                AddToLog_FilterUser(ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> checkUser() --> " + ex.Message, Globals.Path_FilterUsersLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> checkUser() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                //txt_AccountListFileUpload.Invoke(new MethodInvoker(delegate
                //{
                //    txt_AccountListFileUpload.Text = "";
                //}));

                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                AddToLog_FilterUser("-----------------------------------------------------------------------------------------------------------------------");
            }

        }

        public void getCompareToCriteriaOfUser(Dictionary<string, string> userDetails, string userId, out string status)
        {
            bool statse_tweetsNo = false;
            bool status_location = false;
            bool status_profileinfo = false;
            string Tempstatus = string.Empty;
            //Filtered by No of tweets 
            #region---Filter by No of tweets---
            if (chk_scrapeBykeywordSettingTweets.Checked && !string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMin.Text) && !string.IsNullOrEmpty(txt_ScrapebySearchkeywordsettingMin.Text))
            {
                try
                {
                    int result;
                    if (int.TryParse(txt_ScrapebySearchkeywordsettingMin.Text, out result))
                    {
                        if (int.TryParse(txt_ScrapebySearchkeywordsettingMax.Text, out result))
                        {
                            int mintweets = int.Parse(txt_ScrapebySearchkeywordsettingMin.Text);
                            int maxtweets = int.Parse(txt_ScrapebySearchkeywordsettingMax.Text);
                            int noOfTweets = int.Parse(userDetails["statuses_count"]);
                            if (noOfTweets >= mintweets && noOfTweets <= maxtweets)
                            {
                                if (NumberHelper.ValidateNumber(userId))
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["id"]);

                                else
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["screen_name"]);

                                statse_tweetsNo = true;
                                status = "WhiteListedUser";
                                Tempstatus = "WhiteListedUser";
                                //return;
                            }
                            else
                            {
                                status = "BlackListedUser";
                                Tempstatus = "BlackListedUser";
                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }

            }
            #endregion


            //Filtered by No of Followers 
            #region---Filter by No of Followers---
            if (chk_FilterByFollowers.Checked && !string.IsNullOrEmpty(txt_FilterbyFollowersMax.Text) && !string.IsNullOrEmpty(txt_FilterbyFollowersMin.Text))
            {
                try
                {
                    int result;
                    if (int.TryParse(txt_FilterbyFollowersMin.Text, out result))
                    {
                        if (int.TryParse(txt_FilterbyFollowersMax.Text, out result))
                        {
                            int minfollowers = int.Parse(txt_FilterbyFollowersMin.Text);
                            int maxfollowers = int.Parse(txt_FilterbyFollowersMax.Text);
                            int noOffollowers = int.Parse(userDetails["followers_count"]);
                            if (noOffollowers >= minfollowers && noOffollowers <= maxfollowers)
                            {
                                if (NumberHelper.ValidateNumber(userId))
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["id"]);
                                else
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["screen_name"]);

                                //statse_tweetsNo = true;
                                status = "WhiteListedUser";
                                Tempstatus = "WhiteListedUser";
                                //return;
                            }
                            else
                            {
                                status = "BlackListedUser";
                                Tempstatus = "BlackListedUser";
                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            #endregion


            //Filtered by No of Following 
            #region---Filter by No of Following---
            if (chk_FilterByFollowing.Checked && !string.IsNullOrEmpty(txt_FilterbyFollowingMax.Text) && !string.IsNullOrEmpty(txt_FilterbyFollowingMin.Text))
            {
                try
                {
                    int result;
                    if (int.TryParse(txt_FilterbyFollowingMin.Text, out result))
                    {
                        if (int.TryParse(txt_FilterbyFollowingMax.Text, out result))
                        {
                            int minfollowing = int.Parse(txt_FilterbyFollowingMin.Text);
                            int maxfollowing = int.Parse(txt_FilterbyFollowingMax.Text);
                            int noOffollowing = int.Parse(userDetails["friends_count"]);
                            if (noOffollowing >= minfollowing && noOffollowing <= maxfollowing)
                            {
                                if (NumberHelper.ValidateNumber(userId))
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["id"]);
                                else
                                    TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["screen_name"]);

                                //statse_tweetsNo = true;
                                status = "WhiteListedUser";
                                Tempstatus = "WhiteListedUser";
                                //return;
                            }
                            else
                            {
                                status = "BlackListedUser";
                                Tempstatus = "BlackListedUser";
                                return;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            #endregion


            //Filtered By location
            #region ----Filter By location-----
            if (chk_scrapeBykeywordSettingLocation.Checked && ListofLocationsettingFile.Count != 0)
            {
                try
                {
                    foreach (string ListofLocationsettingFile_item in ListofLocationsettingFile)
                    {
                        if ((ListofLocationsettingFile_item.ToLower()).Equals(userDetails["location"].ToLower()))
                        {
                            status_location = true;
                            Tempstatus = "WhiteListedUser";
                        }
                    }

                    if (!status_location)
                    {
                        if (NumberHelper.ValidateNumber(userId))
                            TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["id"]);
                        else
                            TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["screen_name"]);
                    }
                }
                catch (Exception)
                {
                }
            }
            #endregion


            //Filtered by profile info
            #region-----Filter by profile info------

            if (chk_scrapeBykeywordSettingProfileInfo.Checked && ListofProfileInfosettingFile.Count != 0)
            {
                try
                {
                    foreach (string ListofProfileInfosettingFile_item in ListofProfileInfosettingFile)
                    {
                        if ((ListofProfileInfosettingFile_item.ToLower()).Contains(userDetails["description"].ToLower()))
                        {
                            status_profileinfo = true;
                        }
                    }
                    if (!status_profileinfo)
                    {
                        if (NumberHelper.ValidateNumber(userId))
                            TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["id"]);
                        else
                            TemP_Lst_ofScrapeduserAccounts.Remove(userDetails["screen_name"]);
                    }
                }
                catch (Exception)
                {
                }
            }
            #endregion
            //status = "posted";
            status = Tempstatus;
            return;
        }

        private void AddToLog_FilterUser(string log)
        {
            try
            {
                if (lstFilterUsersLogger.InvokeRequired)
                {
                    lstFilterUsersLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstFilterUsersLogger.Items.Add(log);
                        lstFilterUsersLogger.SelectedIndex = lstFilterUsersLogger.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstFilterUsersLogger.Items.Add(log);
                    lstFilterUsersLogger.SelectedIndex = lstFilterUsersLogger.Items.Count - 1;
                }
            }
            catch { }
        }

        private void chk_scrapedUserCSVFileUpload_CheckedChanged(object sender, EventArgs e)
        {
            List<string> usersId = new List<string>();

            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "(*.csv)|*.csv";
                ofd.InitialDirectory = Globals.path_DesktopFolder;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_AccountListFileUpload.Text = ofd.FileName;
                    usersId = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                }
            }

            if (usersId.Count > 1)
            {
                foreach (string usersId_item in usersId)
                {
                    if (usersId_item.Contains("Name/Id , FollowersUserID") || usersId_item.Contains("User_ID , FollowingsUserID"))
                    {
                        continue;
                    }

                    if (usersId_item.Contains(","))
                    {
                        string IDs = usersId_item.Split(',')[1];
                        lst_ofScrapeduserAccounts.Add(IDs);
                    }
                }
                AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ " + lst_ofScrapeduserAccounts.Count + " User id's loaded. ]");
            }
        }

        public bool pathValidation(string value)
        {
            bool pathValidate = false;
            try
            {
                Regex regexObj = new Regex(@"(([a-z]:|\\\\[a-z0-9_.$]+\\[a-z0-9_.$]+)?(\\?(?:[^\\/:*?""<>|\r\n]+\\)+)[^\\/:*?""<>|\r\n]+)");
                Match matchResult = regexObj.Match(value);

                if (matchResult.Success)
                {
                    pathValidate = true;
                }
            }
            catch (Exception)
            {
            }
            return pathValidate;
        }

        private void tab_WhiteAndBlackListUers_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tab_WhiteAndBlackListUers.Width, tab_WhiteAndBlackListUers.Height);

        }

        private void mentionsReplyInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmMentionReplyInterface obj_frmMentionReplyInterface = new frmMentionReplyInterface();
                obj_frmMentionReplyInterface.Show();
            }
            catch
            {
            }
        }

        private void randomiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmRandomiser obj_Randomiser = new frmRandomiser();
                obj_Randomiser.Show();
            }
            catch
            {
            }
        }

        //Stop Account Proces 

        List<Thread> Lst_accountCheckerThread = new List<Thread>();
        bool accountCheckerThreadStart = false;

        //Account check by Email 

        List<Thread> Lst_accountCheckerThreadByEmail = new List<Thread>();
        bool accountCheckerThreadStartByEmail = false;


        List<Thread> Lst_GetUserNameFromAccThreads = new List<Thread>();
        bool GetUserNameFromAccThreadStart = false;

        List<Thread> Lst_AssignGroupThreads = new List<Thread>();
        bool AssignGroupThreadStart = false;

        private void btn_StopAccountAcc_Click(object sender, EventArgs e)
        {
            btn_StopAccountAcc.Enabled = false;
            try
            {
                List<Thread> TempList = new List<Thread>();

                #region commentedRegion
                //if (accountCheckerThreadStart)
                //{
                //    try
                //    {
                //        foreach (Thread Lst_accountCheckerThreaditem in Lst_accountCheckerThread)
                //        {
                //            TempList.Add(Lst_accountCheckerThreaditem);
                //        }
                //        accountCheckerThreadStart = false;
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}
                //if (accountCheckerThreadStartByEmail)
                //{
                //    try
                //    {
                //        foreach (Thread Lst_accountCheckerThreadByEmailitem in Lst_accountCheckerThreadByEmail)
                //        {
                //            TempList.Add(Lst_accountCheckerThreadByEmailitem);
                //        }
                //        accountCheckerThreadStartByEmail = false;
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}

                //if (GetUserNameFromAccThreadStart)
                //{
                //    try
                //    {
                //        foreach (Thread Lst_GetUserNameFromAccThreads_item in Lst_GetUserNameFromAccThreads)
                //        {
                //            TempList.Add(Lst_GetUserNameFromAccThreads_item);
                //        }
                //        GetUserNameFromAccThreadStart = false;
                //    }
                //    catch (Exception)
                //    {
                //    }
                //} 
                #endregion

                if (AssignGroupThreadStart)
                {
                    try
                    {
                        foreach (Thread Lst_AssignGroupThreads_item in Lst_AssignGroupThreads)
                        {
                            TempList.Add(Lst_AssignGroupThreads_item);
                        }
                        AssignGroupThreadStart = false;
                    }
                    catch (Exception)
                    {
                    }
                }


                //Get abort all Start Threads...
                foreach (Thread TempList_item in TempList)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_Checker("------------------------------------------------------------------------------------------------------------------------------------------");
            }

        }


        private void btnStopAccountCheckerByUserName_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerByUserName.Enabled = false;
            try
            {
                List<Thread> TempList = new List<Thread>();
                if (accountCheckerThreadStart)
                {
                    try
                    {
                        foreach (Thread Lst_accountCheckerThreaditem in Lst_accountCheckerThread)
                        {
                            TempList.Add(Lst_accountCheckerThreaditem);
                        }
                        accountCheckerThreadStart = false;
                    }
                    catch (Exception)
                    {
                    }
                }


                //Get abort all Start Threads...
                foreach (Thread TempList_item in TempList)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_Checker("------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }

        private void btnStopAccountCheckerByEmail_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerByEmail.Enabled = false;
            try
            {
                List<Thread> TempList = new List<Thread>();

                if (accountCheckerThreadStartByEmail)
                {
                    try
                    {
                        foreach (Thread Lst_accountCheckerThreadByEmailitem in Lst_accountCheckerThreadByEmail)
                        {
                            TempList.Add(Lst_accountCheckerThreadByEmailitem);
                        }
                        accountCheckerThreadStartByEmail = false;
                    }
                    catch (Exception)
                    {
                    }
                }


                //Get abort all Start Threads...
                foreach (Thread TempList_item in TempList)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_Checker("------------------------------------------------------------------------------------------------------------------------------------------");
            }

        }

        private void btnStopAccountCheckerUserNameFromAccount_Click(object sender, EventArgs e)
        {
            btnStopAccountCheckerUserNameFromAccount.Enabled = false;
            try
            {
                List<Thread> TempList = new List<Thread>();

                if (GetUserNameFromAccThreadStart)
                {
                    try
                    {
                        foreach (Thread Lst_GetUserNameFromAccThreads_item in Lst_GetUserNameFromAccThreads)
                        {
                            TempList.Add(Lst_GetUserNameFromAccThreads_item);
                        }
                        GetUserNameFromAccThreadStart = false;
                    }
                    catch (Exception)
                    {
                    }
                }


                //Get abort all Start Threads...
                foreach (Thread TempList_item in TempList)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                AddToLog_Checker("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_Checker("------------------------------------------------------------------------------------------------------------------------------------------");
            }

        }

        private void btn_StopIPTesting_Click(object sender, EventArgs e)
        {

            try
            {
                IsStopPublicIP = true;
                IsStopImportingIP = true;
                List<Thread> lstTemp = LstPublicIP.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }


                AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP Testing is Aborted ]");


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
            //try
            //{
            //    StopThreads("IP");
            //    AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP testing is Abort. ]");
            //}
            //catch (Exception)
            //{
            //}
        }

        List<string> _lstFollowUserName_AccountCreator = new List<string>();



        private void AccountCreatorProfileData(string folderPath)
        {
            string[] files = System.IO.Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                foreach (string item in files)
                {
                    if (item.Contains("ProfileLocations.txt"))
                    {
                        TwitterSignup.TwitterSignUp._lstAcLocation = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ " + TwitterSignup.TwitterSignUp._lstAcLocation.Count + " Locations Loaded ]");
                    }

                    //if (item.Contains("ProfileUsernames.txt"))
                    //{
                    //    lst_ProfileUsernames = GlobusFileHelper.ReadFiletoStringList(item);
                    //    AddToListAccountsLogs(lst_ProfileUsernames.Count + " Usernames Loaded");
                    //}

                    if (item.Contains("ProfileURLs.txt"))
                    {
                        TwitterSignup.TwitterSignUp._lstAcProfileURL = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ " + TwitterSignup.TwitterSignUp._lstAcProfileURL.Count + " ProfileURLs  Loaded ]");
                    }

                    if (item.Contains("ProfileDescriptions.txt"))
                    {
                        TwitterSignup.TwitterSignUp._lstAcUseDescription = GlobusFileHelper.ReadFiletoStringList(item);
                        AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ " + TwitterSignup.TwitterSignUp._lstAcUseDescription.Count + " Profile Descriptions  Loaded ]");
                    }
                }

                if (TwitterSignup.TwitterSignUp._lstAcLocation.Count <= 0)
                {
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ No Profile Locations Uploaded From Text File ]");
                }
                if (TwitterSignup.TwitterSignUp._lstAcProfileURL.Count <= 0)
                {
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ No Profile Url's Uploaded From Text File ]");
                }
                if (TwitterSignup.TwitterSignUp._lstAcUseDescription.Count <= 0)
                {
                    AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ No Profile Descriptions Uploaded From Text File ]");
                }
            }
            else
            {
                AddToListAccountsLogs("[ " + DateTime.Now + " ] => [ Please Upload Text Files. Folder is Empty ]");
            }
        }

        //public void startProfilingAfterAccountCreation()
        //{

        //    //********Check location ...!!

        //    if (chk_ACLocation.Checked)
        //    {
        //        TwitterSignup.TwitterSignUp.AC_IsUseLoaction = true;
        //    }


        //    // *******Check profile Usl....!!

        //    if (chk_AcProfileUrl.Checked)
        //    {
        //        TwitterSignup.TwitterSignUp.AC_IsUseProfileURL = true;
        //    }

        //    //***********Check discription ..!!

        //    if (chk_AcDescription.Checked)
        //    {
        //        TwitterSignup.TwitterSignUp.AC_IsUseDescription = true;
        //    }


        //    //***********Check Profile Images..!!

        //    if (chk_ACProfileImage.Checked)
        //    {
        //        TwitterSignup.TwitterSignUp.AC_IsUseProfileImage = true;
        //    }
        //}

        #region Retweet/fevorite On Tweet URL

        private void tab_RetweetOrFevorite_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, tab_WhiteAndBlackListUers.Width, tab_WhiteAndBlackListUers.Height);
        }

        private void btn_UploadTweetUrls_Click(object sender, EventArgs e)
        {
            txt_RetweetUrlFilePath.Text = "";
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_RetweetUrlFilePath.Text = ofd.FileName;

                    new Thread(() =>
                    {
                        try
                        {
                            lst_tweetUrl = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ " + lst_tweetUrl.Count + "   URL's Loaded. ]");
                        }
                        catch (Exception ex)
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Selected Item is not loaded >> Error :- " + ex.Message + " ]");
                        }
                    }).Start();
                }
            }
        }


        static bool _IsretweetURL = false;
        static bool _Isfevorite = false;
        public static List<string> lst_tweetUrl = new List<string>();
        public static Queue<string> _QTweetUrl = new Queue<string>();
        List<Thread> lstReTweetThread = new List<Thread>();
        static bool RetweetOrFavorite_ProcessIsStart = true;
        private void btn_start_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                lstReTweetThread.Clear();
                if (_Isfevorite)
                {
                    _Isfevorite = false;
                }
                if (RetweetOrFavorite_ProcessIsStart)
                {
                    RetweetOrFavorite_ProcessIsStart = false;
                    btn_start.Cursor = Cursors.AppStarting;
                    new Thread(() =>
                    {
                        startRetweetAndFavorite();
                        RetweetOrFavorite_ProcessIsStart = true;
                    }).Start();
                }
                else
                {
                    MessageBox.Show("Process Already Started.");
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public int GetProcessor()
        {
            int processor = 1;
            try
            {

                processor = Environment.ProcessorCount;
            }
            catch (Exception ex)
            {

            }
            return processor;
        }

        int counter_Retweet = 0;
        public void startRetweetAndFavorite()
        {
            try
            {                
                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Starting Process ]");

                int processorCount = GetProcessor();
                int maxThread = 25 * processorCount;


                string TweetUrl = string.Empty;
                int noofThread = 5;

                //options is checked or not 
                if (!chk_retweetOfUrl.Checked && !chk_Favorite.Checked)
                {
                    //if not checked any option 
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Please check Favorite option. ]");
                    return;
                }

                if (chkFavoriteWithImages.Checked)
                {
                    TwitterDataScrapper.IsRetweetWithFovieteWithImages = true;
                }
                else 
                {
                    TwitterDataScrapper.IsRetweetWithFovieteWithImages = false;
                }
                //Check Url is loaded ...
                if (!chkUsingKeyWord.Checked)
                {
                    if (!string.IsNullOrEmpty(txt_TweetUrl.Text.Trim()) && lst_tweetUrl.Count == 0)
                    {
                        lst_tweetUrl.Add(txt_TweetUrl.Text.Trim());
                    }
                    else if (string.IsNullOrEmpty(txt_TweetUrl.Text.Trim()) && lst_tweetUrl.Count == 0)
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Please enter Tweet URL. ]");
                        return;
                    }
                }
                else
                {

                    //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(retweetOfUrl_logEvents_addToLogger);
                    //tweetScrapper.noOfRecords = ((int.Parse(txtCountFavoriteandRetweet.Text.Trim()) * TweetAccountContainer.dictionary_TweetAccount.Count));
                    //tweetScrapper.noOfRecords = ((int.Parse(txtCountFavoriteandRetweet.Text.Trim())));
                    //TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.NewKeywordStructData1(txt_TweetUrl.Text.Trim());
                    if (string.IsNullOrEmpty(txt_TweetUrl.Text.Trim()) && lst_tweetUrl.Count == 0)
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Please enter Keyword. ]");
                        return;
                    }

                    try
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartProcessOfGettingTweetId));
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- Thread Pool RT & Fav; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
                    }
                }

                //checking For No Of Threads...
                if (!string.IsNullOrEmpty(txtThreadRTFav.Text))
                {
                    if (NumberHelper.ValidateNumber(txtThreadRTFav.Text))
                    {
                        noofThread = Convert.ToInt32(txtThreadRTFav.Text);
                    }
                }

                counter_Retweet = TweetAccountContainer.dictionary_TweetAccount.Count;
                //Check Account is Loaded ....!!
                if (TweetAccountContainer.dictionary_TweetAccount.Count == 0)
                {
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Please Upload accounts. ]");
                    MessageBox.Show("Please Upload accounts.", "Account Upload");
                    return;
                }


                ThreadPool.SetMaxThreads(maxThread, noofThread);
                                
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    try
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartProcessOfRetweetOrFavorite), item);
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- Thread Pool RT & Fav; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- Thread Pool RT & Fav ; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }

        }

        public void StartProcessOfGettingTweetId(object ac)
        {
            TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
            //TwitterDataScrapper.logEvents.addToLogger += new EventHandler(retweetOfUrl_logEvents_addToLogger);
            //tweetScrapper.noOfRecords = ((int.Parse(txtCountFavoriteandRetweet.Text.Trim()) * TweetAccountContainer.dictionary_TweetAccount.Count));
            try
            {
                TwitterDataScrapper.noOfRecords = (int.Parse(txtCountFavoriteandRetweet.Text.Trim())) * TweetAccountContainer.dictionary_TweetAccount.Count;
                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [Getting Tweets  For KeyWord " + txt_TweetUrl.Text.Trim() + " ]");
                tweetScrapper.NewKeywordStructDataForOnlyTweet(txt_TweetUrl.Text.Trim());
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- StartProcessOfGettingTweetId :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }

        }
        public void StartProcessOfRetweetOrFavorite(object AC)
        {
            int favCount = 0;
            int RetweetCount = 0;
            try
            {
                if (!_Isfevorite)
                {
                    lstReTweetThread.Add(Thread.CurrentThread);
                    lstReTweetThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;
                }
            }
            catch
            {
            }
            TweetAccountManager tweetAccountManager = new TweetAccountManager();
            KeyValuePair<string, TweetAccountManager> item = (KeyValuePair<string, TweetAccountManager>)AC;
            tweetAccountManager = item.Value;
            tweetAccountManager.logEvents.addToLogger += new EventHandler(retweetOfUrl_logEvents_addToLogger);
            try
            {
                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Starting Process For Account " + tweetAccountManager.Username + " ]");
                //Login from account ..
                tweetAccountManager.Login();

                //check if account is not loged In ..
                if (!tweetAccountManager.IsLoggedIn)
                {
                    //tweetAccountManager.Login();
                }
                //check Coount is loged in or Is not suspended ..
                if (tweetAccountManager.IsLoggedIn)
                {
                    //AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Logged In With " + tweetAccountManager.Username + " ]");
                    if (tweetAccountManager.IsNotSuspended)
                    {
                        if (!chkUsingKeyWord.Checked)
                        {
                            foreach (string TUri in lst_tweetUrl)
                            {
                                try
                                {
                                    if (chk_retweetOfUrl.Checked)
                                    {
                                        retweetOfUrl(new object[] { TUri, item, tweetAccountManager });
                                    }
                                    if (chk_Favorite.Checked)
                                    {
                                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Adding To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                        FavoriteOfUrl(new object[] { TUri, item, tweetAccountManager });
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                        else
                        {

                            //foreach (TwitterDataScrapper.StructTweetIDs TUri in TweetAccountManager.static_lst_Struct_TweetData)
                            if (TwitterDataScrapper.queTweetId.Count > 0)
                            {
                                while (TwitterDataScrapper.queTweetId.Count > 0)
                                {
                                    try
                                    {
                                        //if (chk_retweetOfUrl.Checked)
                                        //{
                                        //    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Retweeted : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                        //    retweetOfUrl(new object[] { TUri.ID_Tweet, item, tweetAccountManager });
                                        //}
                                        string TUri = TwitterDataScrapper.queTweetId.Dequeue();
                                        if (chk_Favorite.Checked)
                                        {
                                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Added To Favorite : " + TUri + " from " + tweetAccountManager.Username + " ]");
                                            favCount=FavoriteOfUrl(new object[] { TUri, item, tweetAccountManager },favCount);
                                        }
                                        if (favCount >= int.Parse(txtCountFavoriteandRetweet.Text))
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                            else
                            {
                                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Zero tweet found from keyword " + txt_TweetUrl.Text + " ]");
                            }
                        }
                    }
                    else
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Is suspended  >" + item.Key + " ]");
                    }
                }
                else
                {
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Not Logged With " + item.Key + " ]");
                }

                //this.Invoke(new MethodInvoker(delegate
                //{
                //txt_TweetUrl.Text = "";
                //lst_tweetUrl.Clear();
                //}));
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- StartProcessOfRetweetOrFavorite- 1; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }

            finally
            {
                //AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Completed Retweet and Favorite From " + tweetAccountManager.Username + " ]");
                tweetAccountManager.logEvents.addToLogger -= new EventHandler(retweetOfUrl_logEvents_addToLogger);
                //txt_RetweetUrlFilePath.Text = "";
                counter_Retweet--;
                if (counter_Retweet == 0)
                {
                    if (btn_start.InvokeRequired)
                    {
                        //btn_start.Cursor = Cursors.AppStarting;
                        btn_start.Invoke(new MethodInvoker(delegate
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                            AddToLog_RetweetAndFavorite("---------------------------------------------------------------------------------------------------------------------------");
                            btn_start.Cursor = Cursors.Default;
                            lst_tweetUrl.Clear();
                            //txt_TweetUrl.Text = "";

                        }));
                    }

                }

            }
        }



        public void retweetOfUrl(object param)
        {
            string reTweetUrlPageSource = string.Empty;
            string ReTweetId = string.Empty;
            string tweetStatus = "";
            int minDelay = 20;
            int maxDelay = 25;
            int delay = 10 * 1000;
            if (GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text) && GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text))
            {
                minDelay = Convert.ToInt32(txtRetweetFavMinDelay.Text);
                maxDelay = Convert.ToInt32(txtRetweetFavMaxDelay.Text);
            }
            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);


            TweetAccountManager tweetAccountManager = new TweetAccountManager();

            try
            {
                Tweeter.Tweeter tweeter = new Tweeter.Tweeter();

                Array ParamArr = (Array)param;

                string RetweetUrl = (string)ParamArr.GetValue(0);

                KeyValuePair<string, TweetAccountManager> keyValue_Item = (KeyValuePair<string, TweetAccountManager>)ParamArr.GetValue(1);

                tweetAccountManager = (TweetAccountManager)ParamArr.GetValue(2);

                try
                {
                    //Get Id of tweet from URL..
                    if (RetweetUrl.Contains("/"))
                    {
                        string[] SplitReTweetUrl = RetweetUrl.Split('/');
                        ReTweetId = SplitReTweetUrl[SplitReTweetUrl.Count() - 1];

                    }
                    else
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Formate   > " + RetweetUrl + " ]");
                    }
                }
                catch (Exception)
                {
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Formate   > " + RetweetUrl + " ]");
                }

                if (!string.IsNullOrEmpty(RetweetUrl))
                {

                    //check Coount is loged in or Is not suspended ..
                    if (tweetAccountManager.IsLoggedIn)
                    {
                        if (tweetAccountManager.IsNotSuspended)
                        {
                            try
                            {
                                //Get the page Source from URL...
                                //reTweetUrlPageSource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri(RetweetUrl), "", "");

                                //finally post retweet on Given Url ...
                                tweeter.ReTweetByUrl(ref tweetAccountManager.globusHttpHelper, tweetAccountManager.postAuthenticityToken, ReTweetId, out tweetStatus);

                                //check the return value from with condition ...if its succesfully retweeted or not ...
                                if (tweetStatus == "posted")
                                {
                                    //RetweetCount++;
                                    //Prnt in logger is posted..
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Retweeted :" + RetweetUrl + " from " + tweetAccountManager.Username + " ]");
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Success fully Retweeted Form Account > " + keyValue_Item.Key);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_SucessfullyRetweetToUrlFromId);
                                }
                                else
                                {
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Fail/Already Retweeted From Account  > " + keyValue_Item.Key + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_FailureRetweetToUrlFromId);
                                }
                                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Retweet delay for "+delay+" secs ]");
                                //AddToLog_RetweetAndFavorite("Retweet delay for "+delay+" secs");
                                Thread.Sleep(delay * 1000);
                            }
                            catch (Exception ex)
                            {
                                //AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Error :-  > " + ex.Message + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- retweetOfUrl- 1; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
                            }
                        }
                        else
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Is suspended  >" + keyValue_Item.Key + " ]");
                        }
                    }
                    else
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Login Faile   >" + keyValue_Item.Key + " ]");
                    }
                }
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- retweetOfUrl- 2; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }

        }


        public void FavoriteOfUrl(object param)
        {
            string reTweetUrlPageSource = string.Empty;
            string ReTweetId = string.Empty;
            string tweetStatus = "";
            int minDelay = 20;
            int maxDelay = 25;
            int delay = 10 * 1000;
            if (GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text) && GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text))
            {
                minDelay = Convert.ToInt32(txtRetweetFavMinDelay.Text);
                maxDelay = Convert.ToInt32(txtRetweetFavMaxDelay.Text);
            }
            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);

            TweetAccountManager tweetAccountManager = new TweetAccountManager();
            try
            {
                Tweeter.Tweeter tweeter = new Tweeter.Tweeter();

                Array ParamArr = (Array)param;


                //get the values from pramameter ....!!
                string RetweetUrl = (string)ParamArr.GetValue(0);

                tweetAccountManager = (TweetAccountManager)ParamArr.GetValue(2);

                KeyValuePair<string, TweetAccountManager> keyValue_Item = (KeyValuePair<string, TweetAccountManager>)ParamArr.GetValue(1);

                try
                {
                    //Get Id of tweet from URL..
                    if (!chkUsingKeyWord.Checked)
                    {
                        if (RetweetUrl.Contains("/"))
                        {
                            string[] SplitReTweetUrl = RetweetUrl.Split('/');
                            ReTweetId = SplitReTweetUrl[SplitReTweetUrl.Count() - 1];
                        }
                        else
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                            ReTweetId = RetweetUrl;
                        }
                    }
                    else
                    {
                        //AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                        ReTweetId = RetweetUrl;
                    }
                }
                catch (Exception)
                {
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                }
                if (!string.IsNullOrEmpty(RetweetUrl))
                {
                    //check Coount is loged in or Is not suspended ..
                    if (tweetAccountManager.IsLoggedIn)
                    {
                        if (tweetAccountManager.IsNotSuspended)
                        {
                            try
                            {
                                //Get the page Source from URL...
                                //reTweetUrlPageSource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri(RetweetUrl), "", "");


                                //Finally Send favorit on Given URL ...
                                tweeter.FavoriteByUrl(ref tweetAccountManager.globusHttpHelper, tweetAccountManager.postAuthenticityToken, ReTweetId, out tweetStatus);

                                if (tweetStatus == "posted")
                                {
                                    //Prnt in logger is posted..
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Successfully Favorite sending From Account > " + keyValue_Item.Key + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_SucessfullyFavoriteToUrlFromId);
                                }
                                else
                                {
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Failed Favorite sending From Account > " + keyValue_Item.Key + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_FailureFavoriteToUrlFromId);

                                }
                                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Favorite delay for " + delay + " secs ] ");
                                Thread.Sleep(delay * 1000);
                            }
                            catch (Exception ex)
                            {
                                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- FavoriteOfUrl-1 ; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
                            }
                        }
                        else
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Is suspended  >" + keyValue_Item.Key + " ]");
                        }
                    }
                    else
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Login Faile   >" + keyValue_Item.Key + " ]");
                    }
                }
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- FavoriteOfUrl- 2; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }
        }
        public int FavoriteOfUrl(object param, int favCount)
        {
            string reTweetUrlPageSource = string.Empty;
            string ReTweetId = string.Empty;
            string tweetStatus = "";
            int minDelay = 20;
            int maxDelay = 25;
            int delay = 10 * 1000;
            if (GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text) && GlobusRegex.ValidateNumber(txtRetweetFavMinDelay.Text))
            {
                minDelay = Convert.ToInt32(txtRetweetFavMinDelay.Text);
                maxDelay = Convert.ToInt32(txtRetweetFavMaxDelay.Text);
            }
            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);

            TweetAccountManager tweetAccountManager = new TweetAccountManager();
            try
            {
                Tweeter.Tweeter tweeter = new Tweeter.Tweeter();

                Array ParamArr = (Array)param;


                //get the values from pramameter ....!!
                string RetweetUrl = (string)ParamArr.GetValue(0);

                tweetAccountManager = (TweetAccountManager)ParamArr.GetValue(2);

                KeyValuePair<string, TweetAccountManager> keyValue_Item = (KeyValuePair<string, TweetAccountManager>)ParamArr.GetValue(1);

                try
                {
                    //Get Id of tweet from URL..
                    if (!chkUsingKeyWord.Checked)
                    {
                        if (RetweetUrl.Contains("/"))
                        {
                            string[] SplitReTweetUrl = RetweetUrl.Split('/');
                            ReTweetId = SplitReTweetUrl[SplitReTweetUrl.Count() - 1];
                        }
                        else
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                            ReTweetId = RetweetUrl;
                        }
                    }
                    else
                    {
                        //AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                        ReTweetId = RetweetUrl;
                    }
                }
                catch (Exception)
                {
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ URL is in wrong Format   > " + RetweetUrl + " ]");
                }
                if (!string.IsNullOrEmpty(RetweetUrl))
                {
                    //check Coount is loged in or Is not suspended ..
                    if (tweetAccountManager.IsLoggedIn)
                    {
                        if (tweetAccountManager.IsNotSuspended)
                        {
                            try
                            {
                                //Get the page Source from URL...
                                //reTweetUrlPageSource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri(RetweetUrl), "", "");


                                //Finally Send favorit on Given URL ...
                                tweeter.FavoriteByUrl(ref tweetAccountManager.globusHttpHelper, tweetAccountManager.postAuthenticityToken, ReTweetId, out tweetStatus);

                                if (tweetStatus == "posted")
                                {
                                    favCount++;
                                    //Prnt in logger is posted..
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Successfully Favorite sending From Account > " + keyValue_Item.Key + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_SucessfullyFavoriteToUrlFromId);
                                }
                                else
                                {
                                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Failed Favorite sending From Account > " + keyValue_Item.Key + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(keyValue_Item.Key + ":" + tweetAccountManager.Password + ":" + tweetAccountManager.IPAddress + ":" + tweetAccountManager.IPPort + ":" + tweetAccountManager.IPUsername + ":" + tweetAccountManager.IPpassword, Globals.path_FailureFavoriteToUrlFromId);

                                }
                                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Favorite delay for " + delay + " secs ]");
                                //AddToLog_RetweetAndFavorite("Favorite delay for " + delay + " secs");
                                Thread.Sleep(delay * 1000);
                            }
                            catch (Exception ex)
                            {
                                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- FavoriteOfUrl-1 ; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
                            }
                        }
                        else
                        {
                            AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Is suspended  >" + keyValue_Item.Key + " ]");
                        }
                    }
                    else
                    {
                        AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Account Login Faile   >" + keyValue_Item.Key + " ]");
                    }
                }
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite("Error :-  > " + ex.Message);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error :- Mathod Name :- FavoriteOfUrl- 2; Error Text :- " + ex.Message, Globals.path_LogErrorFromRetweetAndFavorit);
            }
            return favCount;
        }

        private void chk_RetweetAndFavoritePublicIP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chk_RetweetAndFavoritePublicIP.Checked == true)
                {
                    IPUpload();
                    chk_RetweetAndFavoritePrivateIP.Checked = false;
                    clsDBQueryManager SetDb = new clsDBQueryManager();
                    DataSet ds = new DataSet();
                    ds = SetDb.SelectPublicIPData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        //MessageBox.Show("Please Import IP Files. We Are redirecting you to IP Tab");
                        //Tb_AccountManager.SelectedIndex = 5;
                        DialogResult dr = MessageBox.Show("Please Import IP Files. We Are redirecting you to IP Tab", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            Tb_AccountManager.SelectedIndex = 5;
                        }
                        else
                        {
                            //do whatever you want
                        }
                    }
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ " + ds.Tables[0].Rows.Count + " Public Proxies loaded from DataBase ]");
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ " + ds.Tables[0].Rows.Count + " Public Proxies loaded from DataBase ]");

                }
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite(ex.Message);
            }
        }


        private void chk_RetweetAndFavoritePrivateIP_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chk_RetweetAndFavoritePrivateIP.Checked == true)
                {
                    UploadPrivateIP();
                    chk_RetweetAndFavoritePublicIP.Checked = false;
                    clsDBQueryManager SetDb = new clsDBQueryManager();
                    DataSet ds = new DataSet();
                    ds = SetDb.SelectPrivateIPData();
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        //MessageBox.Show("Please Import Private IP Files. We Are redirecting you to IP Tab");
                        //Tb_AccountManager.SelectedIndex = 5;
                        DialogResult dr = MessageBox.Show("Please Import Private IP Files. We Are redirecting you to IP Tab", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            Tb_AccountManager.SelectedIndex = 5;
                        }
                        else
                        {
                            //do whatever you want
                        }
                    }
                    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ " + ds.Tables[0].Rows.Count + " Private Proxies loaded from DataBase ]");
                    AddToGeneralLogs("[ " + DateTime.Now + " ] => [ " + ds.Tables[0].Rows.Count + " Private Proxies loaded from DataBase ]");

                }
            }
            catch (Exception ex)
            {
                //AddToLog_RetweetAndFavorite(ex.Message);
            }
        }


        void retweetOfUrl_logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_RetweetAndFavorite(eArgs.log);
            }
        }



        private void AddToLog_RetweetAndFavorite(string log)
        {
            try
            {
                if (lst_retweetAndFavoritelogger.InvokeRequired)
                {
                    lst_retweetAndFavoritelogger.Invoke(new MethodInvoker(delegate
                    {
                        lst_retweetAndFavoritelogger.Items.Add(log);
                        lst_retweetAndFavoritelogger.SelectedIndex = lst_retweetAndFavoritelogger.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lst_retweetAndFavoritelogger.Items.Add(log);
                    lst_retweetAndFavoritelogger.SelectedIndex = lst_retweetAndFavoritelogger.Items.Count - 1;
                }
            }
            catch { }
        }



        #endregion


        public static bool IsfollowerUserId = false;
        private void Chk_IsFollowerUserId_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_IsFollowerUserId.Checked == true)
            {
                Chk_IsFollowerScreanName.Checked = false;
                IsfollowerUserId = true;
            }
            else
            {
                Chk_IsFollowerUserId.Checked = false;
                IsfollowerUserId = false;
            }

        }


        public static bool IsFollowerScreenName = false;
        private void Chk_IsFollowerScreanName_CheckedChanged(object sender, EventArgs e)
        {
            if (Chk_IsFollowerScreanName.Checked == true)
            {
                Chk_IsFollowerUserId.Checked = false;
                IsFollowerScreenName = true;
            }
            else
            {
                Chk_IsFollowerScreanName.Checked = false;
                IsFollowerScreenName = false;
            }
        }


        public static bool IsFastfollow = false;
        private void chk_FastFollow_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_FastFollow.Checked == true)
            {
                IsFastfollow = true;
            }
            else
            {
                IsFastfollow = false;
            }
        }

        //private void chkUseFollow_AccountCreator_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkUseFollow_AccountCreator.Checked)
        //    {
        //        txtFollowUsernameFile_AccountCreator.Enabled = true;
        //        btnFollowUsernameBrowseFile_AccountCreator.Enabled = true;
        //        labelFollowUsername.Enabled = true;
        //    }
        //    else
        //    {
        //        txtFollowUsernameFile_AccountCreator.Enabled = false;
        //        btnFollowUsernameBrowseFile_AccountCreator.Enabled = false;
        //        labelFollowUsername.Enabled = false;
        //    }
        //}


        List<string> lstMobEmail = new List<string>();
        List<string> lstMobUsername = new List<string>();
        List<string> lstMobUserAgent = new List<string>();
        List<string> lstMobName = new List<string>();
        List<string> lstMobTweet = new List<string>();
        List<string> lstMobUrl = new List<string>();
        List<string> lstMobLocation = new List<string>();
        List<string> lstMobBio = new List<string>();
        Queue<string> queMobTweet = new Queue<string>();
        int CountUsername = 0;
        int CountName = 0;
        int CountMobAgent = 0;
        int CountTweet = 0;
        int CountUrl = 0;
        int CountLocation = 0;
        int CountBio = 0;

        public readonly object locker_que_TweetMessage = new object();
        public readonly object lockr_queueRunningProxies_Mobile = new object();
        public Queue<string> qRunningIP_Mobile = new Queue<string>();
        int counter = 0;
        int AccountCounter = 0;




        private void FillRunningIPInQueue()
        {
            try
            {
                List<string> lstTemp = frmMain_NewUI.LstRunningIP_IPModule.Distinct().ToList();

                AddToMobileLogs("[ " + DateTime.Now + " ] => [ " + lstTemp.Count + " IP Loaded ! ]");
                if (frmMain_NewUI.LstRunningIP_IPModule.Count > 0)
                {
                    foreach (string item in lstTemp)
                    {
                        try
                        {
                            lock (lockr_queueRunningProxies_Mobile)
                            {
                                qRunningIP_Mobile.Enqueue(item);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ There Is No Running IP. So Please Upload The New IP And Start Process Aggain ! ]");
                    //return;
                }
            }
            catch
            {
            }
        }

        private void CheckAndFillIPInQueue()
        {
            try
            {
                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                int isPublic = 0;

                if (frmMain_NewUI.LstRunningIP_IPModule.Count > 0)
                {
                    List<string> lstTemp = frmMain_NewUI.LstRunningIP_IPModule.Distinct().ToList();

                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ " + lstTemp.Count + " IP Loaded ! ]");

                    foreach (string item in lstTemp)
                    {
                        try
                        {
                            IPAddress = item.Split(':')[0];
                            IPPort = item.Split(':')[1];
                            IPUsername = item.Split(':')[2];
                            IPpassword = item.Split(':')[3];

                            IPChecker obj_IPChecker = new IPChecker(IPAddress, IPPort, IPUsername, IPpassword, isPublic);

                            if (obj_IPChecker.CheckIP())
                            {
                                qRunningIP_Mobile.Enqueue(item);
                            }

                            lock (frmMain_NewUI.Locker_LstRunningIP_IPModule)
                            {
                                frmMain_NewUI.LstRunningIP_IPModule.Remove(item);
                            }

                            AddToMobileLogs("[ " + DateTime.Now + " ] => [ Not Running IP >>> " + item + " ]");
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ There Is No Running IP. So Please Upload The New IP And Start Process Aggain ! ]");
                    // return;
                }
            }
            catch
            {
            }
        }



        public void AccountCreator(object objemailpass)
        {
            try
            {
                string emailData = (string)objemailpass;

                //emailData = "sinha.gpos@td.com:vAq8YqxSad";

                string[] arr = Regex.Split(emailData, ":");
                string email = string.Empty;
                string pass = string.Empty;

                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                string username = string.Empty;
                string name = string.Empty;
                string mobileagent = string.Empty;
                string capcthaResponse = "";
                string capcthaChallenge = "";
                string Tweet = "";
                string URl = "";
                string Location = "";
                string Bio = "";

                string IPItem = string.Empty;

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

                lock (lockr_queueRunningProxies_Mobile)
                {
                    if (qRunningIP_Mobile.Count > 0)
                    {
                        try
                        {
                            IPItem = qRunningIP_Mobile.Dequeue();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        CheckAndFillIPInQueue();
                    }
                }

                if (arr.Length == 2)
                {
                    email = arr[0];
                    pass = arr[1];
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Starting Email : " + email + " ]");
                }
                if (arr.Length == 1)
                {
                    return;
                }
                if (arr.Length == 2)
                {
                    email = emailData.Split(':')[0].Replace("\0", "");
                    pass = emailData.Split(':')[1].Replace("\0", "");
                }
                else if (arr.Length == 4)
                {
                    email = emailData.Split(':')[0].Replace("\0", "");
                    pass = emailData.Split(':')[1].Replace("\0", "");
                    IPAddress = emailData.Split(':')[2];
                    IPPort = emailData.Split(':')[3];
                }
                else if (arr.Length == 6)
                {
                    email = emailData.Split(':')[0].Replace("\0", "");
                    pass = emailData.Split(':')[1].Replace("\0", "");
                    IPAddress = emailData.Split(':')[2];
                    IPPort = emailData.Split(':')[3];
                    IPUsername = emailData.Split(':')[4];
                    IPpassword = emailData.Split(':')[5];
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ " + emailData + " Not In correct Format ]");
                    return;
                }


                if (!string.IsNullOrEmpty(IPItem))
                {
                    try
                    {
                        IPAddress = IPItem.Split(':')[0];
                        IPPort = IPItem.Split(':')[1];
                        IPUsername = IPItem.Split(':')[2];
                        IPpassword = IPItem.Split(':')[3];
                    }
                    catch
                    {
                    }
                }


                if (CountUsername >= lstMobUsername.Count)
                {
                    CountUsername = 0;
                }
                username = lstMobUsername[CountUsername];
                CountUsername++;


                if (CountName >= lstMobName.Count)
                {
                    CountName = 0;
                }
                name = lstMobName[CountName];
                CountName++;

                if (CountMobAgent >= lstMobUserAgent.Count)
                {
                    CountMobAgent = 0;
                }
                mobileagent = lstMobUserAgent[CountMobAgent];
                CountMobAgent++;

                if (CountTweet >= lstMobTweet.Count)
                {
                    CountTweet = 0;
                }
                Tweet = lstMobTweet[CountTweet];
                CountTweet++;


                if (CountUrl >= lstMobUrl.Count)
                {
                    CountUrl = 0;
                }
                URl = lstMobUrl[CountUrl];
                CountUrl++;

                if (CountLocation >= lstMobLocation.Count)
                {
                    CountUrl = 0;
                }
                Location = lstMobLocation[CountLocation];
                CountUrl++;

                if (CountBio >= lstMobBio.Count)
                {
                    CountBio = 0;
                }
                Bio = lstMobBio[CountBio];
                CountBio++;



                int tempCount_usernameCheckLoop = 0;
            usernameCheckLoop:
                string url1 = "https://twitter.com/users/email_available?suggest=1&username=&full_name=&email=" + Uri.EscapeDataString(email.Replace(" ", "")) + "&suggest_on_username=true&context=signup";
                string EmailCheck = HttpHelper.getHtmlfromUrlIP(new Uri(url1), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup,", "", mobileagent);
                string Usernamecheck = HttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/users/username_available?suggest=1&username=" + username + "&full_name=" + name + "&email=&suggest_on_username=true&context=signup"), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup,", "", mobileagent);

                if (EmailCheck.Contains("Email has already been taken. An email can only be used on one Twitter account at a time"))
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Email : " + email + " has already been taken. An email can only be used on one Twitter account at a time ]");
                    return;
                }
                else if (Usernamecheck.Contains("Username has already been taken"))
                {
                    //Created = false;
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Username : " + username + " has already been taken ]");
                    if (username.Count() > 12)
                    {
                        username = username.Remove(8); //Removes the extra characters
                    }

                    if (username.Count() > 15)
                    {
                        username = username.Remove(13); //Removes the extra characters
                    }

                    username = username + RandomStringGenerator.RandomStringAndNumber(5);

                    tempCount_usernameCheckLoop++;
                    if (tempCount_usernameCheckLoop < 5)
                    {
                        goto usernameCheckLoop;
                    }
                }
                else if (EmailCheck.Contains("You cannot have a blank email address"))
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ You cannot have a blank email address ]");
                }
                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Redirecting To Twitter Signup ]");
                string url = "https://mobile.twitter.com/signup";

                string pagesource1 = HttpHelper.getHtmlfromUrlIP(new Uri(url), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup,", "", mobileagent);

                string authenticity_token = string.Empty;
                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Getting Captcha Chanllenge ]");

                capcthaChallenge = GetCaptchaMobSrc(capcthaChallenge, pagesource1);

                int tempCaptchaCounter = 0;

            getCaptcha:


                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Getting Authenticity Token ]");

                try
                {
                    int startindex = pagesource1.IndexOf("authenticity_token");
                    string start = pagesource1.Substring(startindex);
                    int endindex = start.IndexOf("\" />");
                    string end = start.Substring(0, endindex).Replace("authenticity_token\" type=\"hidden\" value=\"", "");
                    authenticity_token = end;
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> AccountCreator() --> Authenticity token --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                }

                #region Captcha Solving

                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Download Captcha Image ]");

                WebClient webclient = new WebClient();
                byte[] args = webclient.DownloadData(capcthaChallenge);
                string[] arr1 = new string[] { BaseLib.Globals.DBCUsername, BaseLib.Globals.DBCPassword, "" };
                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Getting Captcha Response ]");

                DeathByCaptcha.Client clnt = null;
                DeathByCaptcha.Captcha captcha = null;

                capcthaResponse = DecodeDBC(arr1, args, out clnt, out captcha);

                if (string.IsNullOrEmpty(capcthaResponse))
                {
                    Thread.Sleep(1000);
                    capcthaResponse = DecodeDBC(arr1, args, out clnt, out captcha);

                    if (string.IsNullOrEmpty(capcthaResponse))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Empty Captcha Returned from Decaptcher ]");

                        // Report an incorrectly solved CAPTCHA.
                        // Make sure the CAPTCHA was in fact incorrectly solved, do not
                        // just report it at random, or you might be banned as abuser.
                        if (captcha != null)
                        {
                            if (clnt.Report(captcha))
                            {
                                Console.WriteLine("Reported as incorrectly solved");
                            }
                            else
                            {
                                Console.WriteLine("Failed reporting as incorrectly solved");
                            }
                        }
                        else
                        {
                            AddToMobileLogs("[ " + DateTime.Now + " ] => [ Captcha Null ]");
                        }
                    }
                }

                #endregion

                string postdata = "authenticity_token=" + authenticity_token + "&oauth_signup_client%5Bfullname%5D=" + username + "&oauth_signup_client%5Bphone_number%5D=" + email.Split('@')[0].Replace("+", "%2B") + "%40" + email.Split('@')[1] + "&oauth_signup_client%5Bpassword%5D=" + pass + "&captcha_response_field=" + capcthaResponse + "&captcha_method=2&captcha_challenge_field=" + capcthaChallenge.Replace("https://mobile.twitter.com/signup/captcha/", "").Replace(".gif", "") + "&commit=Sign+up+for+Twitter";
                string pagfinal = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/signup"), postdata, "https://mobile.twitter.com/signup", "", "", "", "", mobileagent);
                string pagfinal2 = HttpHelper.getHtmlfromUrlMobile(new Uri("http://mobile.twitter.com/home"), "https://mobile.twitter.com/signup", "", mobileagent);
                #region Tweet

                bool temp_IsCreated = false;

                if (pagfinal.Contains("signout") || pagfinal.Contains("https://mobile.twitter.com/signup/screen_name") || pagfinal.Contains("/signup/screen_name") || !pagfinal2.Contains("http://mobile.twitter.com/welcome/interests"))
                {
                    if (pagfinal.Contains("https://mobile.twitter.com/signup/screen_name"))
                    {
                        string screenName = GlobusHttpHelper.GetParamValue(pagfinal, "settings[screen_name]");
                        string secondPostData = "authenticity_token=" + authenticity_token + "&suggestion=0%3A&settings%5Bscreen_name%5D=" + screenName + "&commit=Continue";
                        string res_secondPost = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/signup/screen_name "), secondPostData, "https://mobile.twitter.com/signup/screen_name", "", "", "", "", mobileagent);
                    }

                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account Created : " + email + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, BaseLib.Globals.Path_MobEmailCreator);
                    int counter_profile = 0;
                profilling:
                    string pageProfileData = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/settings/profile"), "https://mobile.twitter.com/account", "");
                    string postdataProfile = "authenticity_token=" + authenticity_token + "&settings%5Bfullname%5D=" + name + "&settings%5Burl%5D=" + url + "&settings%5Blocation%5D=" + Location + "&settings%5Bdescription%5D=" + Bio + "&commit=Save";
                    string postProfileData = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/settings/profile"), postdataProfile, "https://mobile.twitter.com/settings/profile", "", "", "", "", mobileagent);
                    pageProfileData = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/settings/profile"), "https://mobile.twitter.com/account", "");
                    string PostUrl = GlobusHttpHelper.GetParamValue(pageProfileData, "settings[url]");
                    if (!string.IsNullOrEmpty(PostUrl))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account :" + email + " Profiled ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass, BaseLib.Globals.Path_MobEmailProfiled);
                    }
                    else
                    {
                        if (counter_profile <= 5)
                        {
                            Thread.Sleep(5000);
                            AddToMobileLogs("[ " + DateTime.Now + " ] => [ Trying Profiling Once Again for Account : " + email + " ]");
                            counter_profile++;
                            goto profilling;

                        }
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account :" + email + " Not Profiled ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass, BaseLib.Globals.Path_NotMobEmailProfiled);
                    }


                    string pagesource3 = HttpHelper.getHtmlfromUrlIP(new Uri("https://mobile.twitter.com/compose/tweet"), IPAddress, IPPort, IPUsername, IPpassword, "https://mobile.twitter.com/compose/tweet", "", mobileagent);
                    string TweetPost = "authenticity_token=" + authenticity_token + "&tweet%5Btext%5D=" + Uri.EscapeDataString(Tweet) + "&commit=Tweet";
                    string pagesourcepost = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/"), TweetPost, "https://mobile.twitter.com/compose/tweet", "", "", "", "", mobileagent);
                    string secondpagesourcepost = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/"), "https://mobile.twitter.com/", "", mobileagent);

                    if (pagesourcepost.Contains("Tweet sent!") || secondpagesourcepost.Contains("Tweet sent!") || pagesourcepost.Contains(Tweet))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_MobTweeted);
                    }
                    else if (secondpagesourcepost.Contains("Whoops! You already tweeted that") || pagesourcepost.Contains("Whoops! You already tweeted that…"))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ You Already Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_NotMobEmailTweeted);
                    }
                    else
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Not Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_NotMobEmailTweeted);
                    }
                    temp_IsCreated = true;
                    return;
                }
                else if (pagfinal.Contains("Redirecting to: <a href=\"https://mobile.twitter.com/signup/screen_name"))
                {
                    Thread.Sleep(300);
                    pagfinal = HttpHelper.getHtmlfromUrlIP(new Uri("https://mobile.twitter.com/signup/screen_name"), IPAddress, IPPort, IPUsername, IPpassword, "https://mobile.twitter.com/compose/tweet", "", mobileagent);
                }
                else if (pagfinal.Contains("captcha invalid-field invalid-captcha-response-field")) //wrong captcha
                {
                    tempCaptchaCounter++;
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Wrong Captcha : " + email + " ]");

                    // Report an incorrectly solved CAPTCHA.
                    // Make sure the CAPTCHA was in fact incorrectly solved, do not
                    // just report it at random, or you might be banned as abuser.
                    if (captcha != null)
                    {
                        if (clnt.Report(captcha))
                        {
                            Console.WriteLine("Reported as incorrectly solved");
                        }
                        else
                        {
                            Console.WriteLine("Failed reporting as incorrectly solved");
                        }
                    }

                    if (tempCaptchaCounter <= 3)
                    {
                        capcthaChallenge = GetCaptchaMobSrc("", pagfinal);
                        goto getCaptcha;
                    }
                }

                if (pagfinal.Contains("settings[screen_name]") && !temp_IsCreated)
                {
                    string screenName = GlobusHttpHelper.GetParamValue(pagfinal, "settings[screen_name]");

                    string secondPostData = "authenticity_token=" + authenticity_token + "&suggestion=0%3A&settings%5Bscreen_name%5D=" + screenName + "&commit=Continue";
                    string res_secondPost = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/signup/screen_name "), secondPostData, "https://mobile.twitter.com/signup/screen_name", "", "", "", "", mobileagent);

                    #region Profile and tweet

                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account Created : " + email + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, BaseLib.Globals.Path_MobEmailCreator);
                    int counter_profile = 0;
                profilling:
                    string pageProfileData = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/settings/profile"), "https://mobile.twitter.com/account", "");
                    string postdataProfile = "authenticity_token=" + authenticity_token + "&settings%5Bfullname%5D=" + name + "&settings%5Burl%5D=" + url + "&settings%5Blocation%5D=" + Location + "&settings%5Bdescription%5D=" + Bio + "&commit=Save";
                    string postProfileData = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/settings/profile"), postdataProfile, "https://mobile.twitter.com/settings/profile", "", "", "", "", mobileagent);
                    pageProfileData = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/settings/profile"), "https://mobile.twitter.com/account", "");
                    string PostUrl = GlobusHttpHelper.GetParamValue(pageProfileData, "settings[url]");
                    if (!string.IsNullOrEmpty(PostUrl))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account :" + email + " Profiled ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass, BaseLib.Globals.Path_MobEmailProfiled);
                    }
                    else
                    {
                        if (counter_profile <= 5)
                        {
                            Thread.Sleep(5000);
                            AddToMobileLogs("[ " + DateTime.Now + " ] => [ Trying Profiling Once Again for Account : " + email + " ]");
                            counter_profile++;
                            goto profilling;

                        }

                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account :" + email + " Not Profiled ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass, BaseLib.Globals.Path_NotMobEmailProfiled);
                    }

                    string pagesource3 = HttpHelper.getHtmlfromUrlIP(new Uri("https://mobile.twitter.com/compose/tweet"), IPAddress, IPPort, IPUsername, IPpassword, "https://mobile.twitter.com/compose/tweet", "", mobileagent);
                    string TweetPost = "authenticity_token=" + authenticity_token + "&tweet%5Btext%5D=" + Uri.EscapeDataString(Tweet) + "&commit=Tweet";
                    string pagesourcepost = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/"), TweetPost, "https://mobile.twitter.com/compose/tweet", "", "", "", "", mobileagent);
                    string secondpagesourcepost = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/"), "https://mobile.twitter.com/", "", mobileagent);
                    if (pagesourcepost.Contains("Tweet sent!") || secondpagesourcepost.Contains("Tweet sent!") || pagesourcepost.Contains(Tweet))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_MobTweeted);
                    }
                    else if (secondpagesourcepost.Contains("Whoops! You already tweeted that") || pagesourcepost.Contains("Whoops! You already tweeted that"))
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ You Already Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_NotMobEmailTweeted);
                    }
                    else
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Not Tweeted " + Tweet + " From " + email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + Tweet, BaseLib.Globals.Path_NotMobEmailTweeted);
                    }
                    #endregion
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Account Not Created : " + email + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + pass, BaseLib.Globals.Path_NotMobEmailCreator);
                }
                #endregion
            }
            catch (Exception ex)
            {
                if (ex.Message == "Unable to connect to remote server")
                {
                    //AddToMobileLogs(ex.Message);
                }
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> AccountCreator() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
            }
            finally
            {
                counter++;
                if (AccountCounter == counter)
                {
                    MessageBox.Show("Finished Account Creation");
                }
            }
        }

        private static string GetCaptchaMobSrc(string capcthaChallenge, string pagesource1)
        {
            try
            {
                int startindex = pagesource1.IndexOf("https://mobile.twitter.com/signup/captcha/");
                string start = pagesource1.Substring(startindex);
                int endindex = start.IndexOf("\"");
                string end = start.Substring(0, endindex);
                capcthaChallenge = end;
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> AccountCreator() --> capcthaChallenge --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
            }
            return capcthaChallenge;
        }



        private void btnMobTweeting_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (lstMobTweet.Count > 0 && lstMobEmail.Count > 0)
                {
                    foreach (string data in lstMobTweet)
                    {
                        queMobTweet.Enqueue(data);
                    }

                    CountMobAgent = 0;
                    new Thread(() =>
                    {
                        foreach (string email in lstMobEmail)
                        {
                            //AccountCreator(email);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(StartMobTweeting), email);
                        }
                    }).Start();

                }
                else
                {
                    if (lstMobTweet.Count == 0)
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Please Upload Tweets ]");
                    }
                    else if (lstMobEmail.Count == 0)
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ Please Upload Email ]");
                    }
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToMobileLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartMobTweeting(object objemailpass)
        {
            try
            {
                string emailData = (string)objemailpass;

                string[] arr = Regex.Split(emailData, ":");
                string email = string.Empty;
                string pass = string.Empty;
                string mobileagent = string.Empty;
                string authenticity_token = string.Empty;
                string Tweet = string.Empty;

                if (arr.Length == 2)
                {
                    email = arr[0];
                    pass = arr[1];
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ " + emailData + " Not In Correct Format ]");
                    return;
                }


                if (CountMobAgent >= lstMobUserAgent.Count)
                {
                    CountMobAgent = 0;
                }
                mobileagent = lstMobUserAgent[CountMobAgent];
                CountMobAgent++;

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                //Login
                string pageFirst = HttpHelper.getHtmlfromUrlMobile(new Uri("https://mobile.twitter.com/session/new"), "https://mobile.twitter.com/signup", "", mobileagent);

                try
                {
                    int startindex = pageFirst.IndexOf("authenticity_token");
                    string start = pageFirst.Substring(startindex);
                    int endindex = start.IndexOf("\" />");
                    string end = start.Substring(0, endindex).Replace("authenticity_token\" type=\"hidden\" value=\"", "");
                    authenticity_token = end;
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartMobTweeting() --> Authenticity token --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                }

                string postdata = "authenticity_token=" + authenticity_token + "&username=" + Uri.EscapeDataString(email) + "&password=" + pass + "&commit=Sign+in";
                string LoginFirst = HttpHelper.postFormDataMobileData(new Uri("https://mobile.twitter.com/session"), postdata, "https://mobile.twitter.com/session/new", "", "", "", "", mobileagent);

                if (LoginFirst.Contains("/session/destroy") && LoginFirst.Contains("/settings"))
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Logged In With Account :" + email + " ]");
                    if (queMobTweet.Count > 0)
                    {

                    }
                    else
                    {
                        AddToMobileLogs("[ " + DateTime.Now + " ] => [ No Tweets For Account : " + email + " ]");
                        return;
                    }
                }
                else
                {
                    AddToMobileLogs("[ " + DateTime.Now + " ] => [ Cannot Login With Account :" + email + " ]");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartMobTweeting() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
            }
        }

        private void tabMobileAccountCreator_Paint(object sender, PaintEventArgs e)
        {

            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btnstartextracting_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                int count = 20;
                if (lst_TweetExtrcator.Count <= 0)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Please Upload Keyword File ]");
                    return;
                }

                if (!string.IsNullOrEmpty(txtKeywordPageCount.Text))
                {
                    if (NumberHelper.ValidateNumber(txtKeywordPageCount.Text))
                    {
                        count = Convert.ToInt32(txtKeywordPageCount.Text);
                    }
                }
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Starting Extracting Tweets ]");

                if (chkBoxScrapeLiveTweet.Checked)
                {
                    new Thread(() =>
                    {
                        foreach (string keyword in lst_TweetExtrcator)
                        {
                            StartKeywordExtractingForLiveTweet(keyword, count);

                        }
                    }).Start();
                }
                else
                {
                    new Thread(() =>
                    {
                        foreach (string keyword in lst_TweetExtrcator)
                        {
                            StartKeywordExtracting(keyword, count);

                        }
                    }).Start();
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartKeywordExtracting(string keyword, int count)
        {
            try
            {
                keyword = keyword.Replace("#", "%23");
                List<string> lstweete = new List<string>();
                keyword = keyword.Replace("%23","#");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting Tweets for " + keyword + " ]");
                keyword = keyword.Replace("#", "%23");
                string[] arraylst = new string[] { };
                string scroll_cursor = "0";
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                for (int i = 0; i < count; i++)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Getting " + (i + 1) + " Page Tweets ]");
                    string pgsrcs = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&scroll_cursor=" + scroll_cursor), "", "");

                    try
                    {
                        int startindex = pgsrcs.IndexOf("scroll_cursor");
                        string start = pgsrcs.Substring(startindex).Replace("scroll_cursor", string.Empty);
                        int endindex = start.IndexOf("refresh_cursor");
                        string end = string.Empty;

                        if (endindex >= 0)
                        {

                            end = start.Substring(0, endindex);
                            scroll_cursor = end.Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                        }
                        else
                        {
                            endindex = start.IndexOf("\"}");
                            end = start.Substring(0, endindex);
                            scroll_cursor = end;
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> Getting Maxid --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> Getting Maxid  --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    arraylst = Regex.Split(pgsrcs, "tweet-text");
                    arraylst = arraylst.Skip(1).ToArray();
                    foreach (string data in arraylst)
                    {
                        try
                        {
                            int stratindex = data.IndexOf("\"");
                            string start = data.Substring(stratindex);
                            Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
                            start = regex.Replace(start, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)));
                            string abc = start.Substring(start.IndexOf(">"), start.IndexOf("<div") - start.IndexOf(">") - 1);
                            BaseLib.GlobusRegex baselib = new GlobusRegex();
                            string TweetText = baselib.StripTagsRegex(abc);
                            try
                            {
                                if (TweetText.Contains("http:\\") || TweetText.Contains("pic.twitter.com"))
                                {
                                    if (TweetText.Contains("http:\\"))
                                    {
                                        string[] array = Regex.Split(TweetText, "http:");
                                        TweetText = array[0];
                                    }
                                    else if (TweetText.Contains("pic.twitter.com"))
                                    {
                                        string[] array = Regex.Split(TweetText, "pic.twitter.com");
                                        TweetText = array[0];
                                    }
                                }

                                TweetText = TweetText.Replace("&quot", string.Empty).Replace("&gt", string.Empty).Replace("&lt", string.Empty).Replace("&amp;", string.Empty).Replace("\n", string.Empty).Replace("\\n", string.Empty).Replace("&nbsp;", string.Empty).Replace(">", string.Empty).Replace("&#39;", string.Empty).Replace("/", "");//&nbsp;\n    \n          
                                if (!string.IsNullOrEmpty(TweetText) && TweetText.Length <= 100)
                                {
                                    lstweete.Add(TweetText);
                                    ScrapTweetsForTweetModule.Add(TweetText);
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> RemovingLinks --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> RemovingLinks --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> Getting Tweet Message --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> Getting Tweet Message --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }

                lstweete = lstweete.Distinct().ToList();
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstweete.Count + " Total distinct Tweets ]");
                foreach (string Tweet in lstweete)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + Tweet + " ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Tweet, Globals.path_StoreKeywordTweetExtractor + "_" + keyword.Replace("%23", "#") + ".txt");
                }
                keyword = keyword.Replace("%23", "#");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Finished Extracting Tweets for " + keyword + " ]");
                AddToTweetCreatorLogs("-----------------------------------------------------------------------------------------------------------------------");

                if (CheckBoxScrapTweetsIsChecked)
                {
                    ScrapTweetsForTweetModule = lstweete;
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Scraped No of Tweets " + ScrapTweetsForTweetModule.Count() + " ]");
                    CheckBoxScrapTweetsIsChecked = false;
                    this.Invoke(new MethodInvoker(delegate { Tb_AccountManager.SelectedIndex = 1; }));
                }

            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() -->  " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }


        public void StartKeywordExtractingForLiveTweet(string keyword, int count)
        {
            try
            {
                keyword = keyword.Replace("#", "%23");
                List<string> lstweete = new List<string>();
                keyword = keyword.Replace("%23", "#");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting Tweets for " + keyword + " ]");
                keyword = keyword.Replace("#", "%23");
                string[] arraylst = new string[] { };
                string scroll_cursor = "0";
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                ScrapTweetsForTweetModule.Clear();
                string tweetURL = string.Empty;
                string max_position = string.Empty;

                for (int i = 1; i < count; i++)
                {
                    string pgsrcs = string.Empty;
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Getting " + (i) + " Page Live Tweets ]");

                    if (i == 1)
                    {
                        //https://twitter.com/search?f=tweets&vertical=default&q=computer&src=typd
                        pgsrcs = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/search?f=tweets&vertical=default&q=" + keyword + "&src=typd"), "", "");
                    }
                    else
                    {
                        // pgsrcs = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&scroll_cursor=" + scroll_cursor), "", "");
                        pgsrcs = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?vertical=default&q=" + keyword + "&src=typd&include_available_features=1&include_entities=1&last_note_ts=1435160952&max_position=" + max_position), "", "");
                        //https://twitter.com/i/search/timeline?f=tweets&vertical=default&q=computer&src=typd&include_available_features=1&include_entities=1&last_note_ts=1435161052&max_position=
                    }

                    try
                    {
                        //int startindex = pgsrcs.IndexOf("scroll_cursor");
                        //string start = pgsrcs.Substring(startindex).Replace("scroll_cursor", string.Empty);
                        //int endindex = start.IndexOf("refresh_cursor");
                        //string end = string.Empty;

                        //if (endindex >= 0)
                        //{

                        //    end = start.Substring(0, endindex);
                        //    scroll_cursor = end.Replace("\\", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Trim();
                        //}
                        //else
                        //{
                        //    endindex = start.IndexOf("\"}");
                        //    end = start.Substring(0, endindex);
                        //    scroll_cursor = end;
                        //}
                        try
                        {
                            max_position = Utils.getBetween(pgsrcs, "data-max-position=\"", "\"");
                            if (string.IsNullOrEmpty(max_position))
                            {
                                max_position = Utils.getBetween(pgsrcs, "min_position\":\"", "\"");
                            }
                        }
                        catch { };
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> Getting Maxid --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> Getting Maxid  --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    //arraylst = Regex.Split(pgsrcs, "tweet-text");//TweetTextSize  js-tweet-text tweet-text
                    arraylst = Regex.Split(pgsrcs, "TweetTextSize  js-tweet-text tweet-text");
                    arraylst = arraylst.Skip(1).ToArray();
                    foreach (string data in arraylst)
                    {
                        string dataname = Utils.getBetween(data, "<a href=\"", "\"");
                        if (string.IsNullOrEmpty(dataname))
                        {
                            dataname = Utils.getBetween(data, "href=\\\"\\/hashtag\\/", "?");
                        }
                        try
                        {
                            string TweetText = string.Empty;
                            //int stratindex = data.IndexOf("\"");
                            //string start = data.Substring(stratindex);
                            //Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
                            //start = regex.Replace(start, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)));
                            //string abc = start.Substring(start.IndexOf(">"), start.IndexOf("<div") - start.IndexOf(">") - 1);
                            //BaseLib.GlobusRegex baselib = new GlobusRegex();
                            //string TweetText = baselib.StripTagsRegex(abc);

                            TweetText = Utils.getBetween(data, ">", "<");
                            if (string.IsNullOrEmpty(TweetText))
                            {
                                try
                                {
                                    string[] getTweetText = Regex.Split(data, "href=");
                                    if (getTweetText[0].Contains("div class=\\\"expanded-content js-tweet-details-dropdown"))//div class=\"expanded-content js-tweet-details-dropdown
                                    {
                                        if (!getTweetText[0].Contains("div class=\\\"expanded-content js-tweet-details-dropdown"))
                                        {
                                            TweetText = Utils.getBetween(getTweetText[0] + "\"", "data-aria-label-part=\\\"0\\\"", "\"");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");
                                        }
                                        else
                                        {
                                            TweetText = Utils.getBetween(getTweetText[0], "data-aria-label-part=\\\"0\\\"", "div class=\\\"expanded-content js-tweet-details-dropdown");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");

                                        }
                                    }
                                    if (getTweetText[1].Contains("class=\\\"twitter-atreply pretty-link js-nav\\\" dir=\\\"ltr\\\""))
                                    {
                                        if (!getTweetText[1].Contains("div class=\"expanded-content js-tweet-details-dropdown"))
                                        {
                                            TweetText = TweetText + Utils.getBetween(getTweetText[1] + "\"", "dir=\\\"ltr\\\"", "\"");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");
                                        }
                                        else
                                        {
                                            TweetText = TweetText + Utils.getBetween(getTweetText[1], "dir=\\\"ltr\\\"", "div class=\\\"expanded-content js-tweet-details-dropdown");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");
                                        }
                                    }
                                    if (getTweetText[2].Contains("class=\\\"twitter-atreply pretty-link js-nav\\\" dir=\\\"ltr\\\""))
                                    {
                                        if (!getTweetText[2].Contains("div class=\"expanded-content js-tweet-details-dropdown"))
                                        {
                                            TweetText = TweetText + Utils.getBetween(getTweetText[2] + "\"", "dir=\\\"ltr\\\"", "\"");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");
                                        }
                                        else
                                        {
                                            TweetText = TweetText + Utils.getBetween(getTweetText[2], "dir=\\\"ltr\\\"", "div class=\\\"expanded-content js-tweet-details-dropdown");
                                            TweetText = TweetText.Replace("strong", "").Replace("\\u003e", "").Replace("\\u003ca", "").Replace("\\u003cs", "").Replace("\\u003c", "").Replace("\\u003cb", "").Replace("/s", "").Replace("&#39;", "'").Replace("&#10;", "");
                                        }
                                    }
                                }
                                catch { };


                                //TweetText = Utils.getBetween(data, "data-aria-label-part=", "class=\\\"twitter-atreply pretty-link js-nav");// class=\"twitter-atreply pretty-link js-nav
                                //if (string.IsNullOrEmpty(TweetText))
                                //{
                                //    TweetText = Utils.getBetween(data, "data-aria-label-part=\"0\"", "</p>");
                                //    if (string.IsNullOrEmpty(TweetText))
                                //    {
                                //        TweetText = Utils.getBetween(data, "data-aria-label-part=", "class=\\\"expanded-content js-tweet-details-dropdown");//class=\"expanded-content js-tweet-details-dropdown
                                //    }
                                //}


                            }

                            TweetText = TweetText.Replace("\\u003c", "").Replace("\\u003e", "").Replace("\\u003cb", "").Replace("strong", "").Replace("div", "").Replace("\n", "").Replace("\\", "").Replace("\"0", "").Trim();
                            TweetText = TweetText.Replace("<a href=\"/" + dataname + "\" class=\"twitter-atreply pretty-link js-nav\" dir=\"ltr\" >", "").Replace("</s>", "").Replace("<s>", "").Replace("<a>", "").Replace("</a>", "").Replace("</b>", "").Replace("<b>", "").Replace("<strong>", "").Replace("</strong>", "");
                            try
                            {
                                try
                                {
                                    if (TweetText.Contains("http:\\") || TweetText.Contains("pic.twitter.com") || TweetText.Contains("class=\"twitter-emoji"))
                                    {
                                        if (TweetText.Contains("http:\\"))
                                        {
                                            string[] array = Regex.Split(TweetText, "http:");
                                            TweetText = array[0];
                                        }
                                        else if (TweetText.Contains("pic.twitter.com"))
                                        {
                                            string[] array = Regex.Split(TweetText, "pic.twitter.com");
                                            TweetText = array[0];
                                        }
                                        else if (TweetText.Contains("class=\"twitter-emoji"))
                                        {
                                            string[] array = Regex.Split(TweetText, "class=\"twitter-emoji");
                                            TweetText = TweetText + array[0];
                                        }
                                    }
                                }
                                catch { };
                                TweetText = TweetText.Replace("href=\"/hashtag/" + dataname + "?src=hash\"", "").Replace("data-query-source=\"hashtag_click\"", "").Replace("class=\"twitter-hashtag pretty-link js-nav\" dir=\"ltr\"", "").Replace("\n", "").Replace("n", "");
                                // string tweetURL = string.Empty;
                                try
                                {
                                    string[] getURL = Regex.Split(data, "data-card-url="); //details with-icn js-details\" href=\"\
                                    if (getURL.Count() == 1)
                                    {
                                        // getURL = Regex.Split(data, "");
                                        try
                                        {
                                            tweetURL = Utils.getBetween(data, "details with-icn js-details\" href=\"\\", "\\");
                                        }
                                        catch { };
                                    }
                                    else
                                    {
                                        try
                                        {
                                            tweetURL = Utils.getBetween(getURL[1], "twitter.com\\", "\\");
                                        }
                                        catch { };
                                    }
                                }
                                catch { };
                                TweetText = TweetText.Replace("&quot", string.Empty).Replace("&gt", string.Empty).Replace("&lt", string.Empty).Replace("&amp;", string.Empty).Replace("\n", string.Empty).Replace("\\n", string.Empty).Replace("&nbsp;", string.Empty).Replace(">", string.Empty).Replace("&#39;", string.Empty).Replace("/", "");//&nbsp;\n    \n          
                                if (!string.IsNullOrEmpty(TweetText) && TweetText.Length <= 100)
                                {
                                    lstweete.Add(TweetText);
                                    ScrapTweetsForTweetModule.Add(TweetText);
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> RemovingLinks --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> RemovingLinks --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() --> Getting Tweet Message --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> Getting Tweet Message --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }

                lstweete = lstweete.Distinct().ToList();
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstweete.Count + " Total distinct Tweets ]");
                //ScrapTweetsForTweetModule
                foreach (string Tweet in lstweete)
                {
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + Tweet + " ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Tweet, Globals.path_StoreKeywordTweetExtractor + "_" + keyword.Replace("%23", "#") + ".txt");
                }
                keyword = keyword.Replace("%23", "#");
                AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Finished Extracting Tweets for " + keyword + " ]");
                AddToTweetCreatorLogs("-----------------------------------------------------------------------------------------------------------------------");

                if (CheckBoxScrapTweetsIsChecked)
                {
                    ScrapTweetsForTweetModule = lstweete;
                    AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Scraped No of Tweets " + ScrapTweetsForTweetModule.Count() + " ]");
                    CheckBoxScrapTweetsIsChecked = false;
                    this.Invoke(new MethodInvoker(delegate { Tb_AccountManager.SelectedIndex = 1; }));
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() -->  " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

        }


        private void btnKeywordTweetExtrcator_Click(object sender, EventArgs e)
        {
            try
            {
                string path_TweetCreatorMessageFile = GlobusFileHelper.LoadTextFileUsingOFD();
                if (!string.IsNullOrEmpty(path_TweetCreatorMessageFile))
                {
                    txtKeywordTweetExtractor.Text = path_TweetCreatorMessageFile;
                    lst_TweetExtrcator = GlobusFileHelper.ReadLargeFile(path_TweetCreatorMessageFile);
                    AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lst_TweetExtrcator.Count + " Keyword Loaded ]");
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnKeywordTweetExtrcator_Click() --> " + ex.Message, Globals.Path_TweetCreatorErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnKeywordTweetExtrcator_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public static List<string> LstRunningPvtIP_IPModule = new List<string>();
        public static readonly object Locker_LstRunningPvtIP_IPModule = new object();
        public int threadcountForFinishMSGPvtProxies = 0;
        /// <summary>
        /// CHecking Valid Private Proxies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        List<Thread> LstAssignPrivateIP = new List<Thread>();
        bool IsStopPrivateIP = false;
        private void btnTestPvtProxies_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    workingproxiesCount = 0;
                    //Storing Valid Private Proxies in Txt file
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Existing Proxies Saved To : ]");
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + Globals.Path_ExsistingPvtProxies + " ]");
                    List<string> lstProxies = new List<string>();
                    if (!string.IsNullOrEmpty(txtPvtIPFour.Text.Trim()))
                    {
                        lstProxies = GlobusFileHelper.ReadFiletoStringList(txtPvtIPFour.Text);
                    }
                    else
                    {
                        AddToIPsLogs("[ " + DateTime.Now + " ] => [ Please Upload Either Url or Load IP Files ]");
                        return;
                    }
                    //lstProxies = Globussoft.GlobusFileHelper.ReadFiletoStringList(txtPublicIP.Text);
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ " + lstProxies.Count() + " Private Proxies Uploaded ]");
                    new Thread(() =>
                    {
                        GetValidPrivateProxies(lstProxies);
                    }).Start();
                }
                catch (Exception)
                {

                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToIPsLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        /// <summary>
        /// Checking Private Proxies in diffrent thread
        /// </summary>
        /// <param name="lstProxies"></param>
        private void GetValidPrivateProxies(List<string> lstProxies)
        {
            try
            {
                if (IsStopPrivateIP)
                {
                    return;
                }
                LstAssignPrivateIP.Add(Thread.CurrentThread);
                LstAssignPrivateIP = LstAssignPrivateIP.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

            }
            catch
            { }

            //try
            //{
            //    dictionary_Threads.Add("IP", Thread.CurrentThread);
            //}
            //catch { };

            numberOfIPThreads = 25;
            threadcountForFinishMSG = lstProxies.Count;
            if (!string.IsNullOrEmpty(txtNumberOfIPThreads.Text) && GlobusRegex.ValidateNumber(txtNumberOfIPThreads.Text))
            {
                numberOfIPThreads = int.Parse(txtNumberOfIPThreads.Text);
            }

            WaitCallback waitCallBack = new WaitCallback(ThreadPoolMethod_PvtProxies);
            foreach (string item in lstProxies)
            {
                if (countParseProxiesThreads >= numberOfIPThreads)
                {
                    lock (pvtProxiesThreadLockr)
                    {
                        Monitor.Wait(pvtProxiesThreadLockr);
                    }
                }

                ///Code for checking and then adding proxies to FinalIPList...
                ThreadPool.QueueUserWorkItem(waitCallBack, item);
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Thread pool Private Proxies
        /// </summary>
        /// <param name="objIP"></param>
        private void ThreadPoolMethod_PvtProxies(object objIP)
        {
            try
            {
                LstAssignPrivateIP.Add(Thread.CurrentThread);
                LstAssignPrivateIP = LstAssignPrivateIP.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

                countParsePvtProxiesThreads++;

                string item = (string)objIP;
                int IsPublic = 1;
                int Working = 0;
                string LoggedInIp = string.Empty;

                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                string account = item;

                int DataCount = account.Split(':').Length;

                if (DataCount == 4)
                {
                    IPAddress = account.Split(':')[0];
                    IPPort = account.Split(':')[1];
                    IPUsername = account.Split(':')[2];
                    IPpassword = account.Split(':')[3];
                    //AddToIPsLogs(account);
                }
                else
                {
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ IP Not In correct Format ]");
                    return;
                }

                try
                {
                    dictionary_Threads.Add("IP_" + IPAddress, Thread.CurrentThread);
                }
                catch { };

                IPChecker IPChecker = new IPChecker(IPAddress, IPPort, IPUsername, IPpassword, IsPublic);
                if (IPChecker.CheckPvtIP())
                {
                    workingproxiesCount++;
                    lock (IPListLockr)
                    {
                        queWorkingPvtProxies.Enqueue(item);
                        Monitor.Pulse(IPListLockr);
                    }
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Added " + item + " to working proxies list ]");

                    lock (LstRunningPvtIP_IPModule)
                    {
                        LstRunningPvtIP_IPModule.Add(item);
                    }
                }
                else
                {
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Non Working IP: " + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_Non_ExistingProxies);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                lock (pvtProxiesThreadLockr)
                {
                    countParsePvtProxiesThreads--;
                    Monitor.Pulse(pvtProxiesThreadLockr);
                }

                threadcountForFinishMsgPvt--;

                if (countParsePvtProxiesThreads == 0)
                {
                    AddToIPsLogs("[ " + DateTime.Now + " ] => [ Process of IP testing is finished. ]");
                    AddToIPsLogs("-----------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        private void emailVerificationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Frm_EmailVerification frmEmailVerification = new Frm_EmailVerification();
                frmEmailVerification.Show();
            }
            catch (Exception)
            {
            }
        }

        private void tabFakeEmailCreator_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void splitContainer4_Panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btn_browseMentionuser_Click(object sender, EventArgs e)
        {
            lst_mentionUser.Clear();
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_mentionuserfilepath.Text = ofd.FileName;
                        lst_mentionUser = GlobusFileHelper.ReadLargeFile(ofd.FileName);
                    }
                }

                if (lst_mentionUser.Count > 0)
                {
                    //chk_Mentionrendomuser.Checked = true;
                    chk_mentionwithsingle.Checked = true;
                }
            }
            catch { }
        }

        private void chk_mentionwithsingle_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_mentionwithsingle.Checked == true)
            {
                chk_Mentionrendomuser.Checked = false;
                Ismentionsingleuser = true;
                Ismentionrendomuser = false;

                txt_mentionuserfilepath.Enabled = false;
                btn_browseMentionuser.Enabled = true;
                txtTweetMentionNoOfUser.Enabled = true;
            }
            else
            {
                txt_mentionuserfilepath.Enabled = false;
                btn_browseMentionuser.Enabled = false;
                txtTweetMentionNoOfUser.Enabled = false;

                Ismentionsingleuser = false;
            }
        }

        private void chk_Mentionrendomuser_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_Mentionrendomuser.Checked == true)
            {
                chk_mentionwithsingle.Checked = false;
                Ismentionsingleuser = false;
                Ismentionrendomuser = true;
            }
            else
            {
                Ismentionrendomuser = false;
            }
        }

        #region GlobalVariableForWhoToScrap
        int counterFoScrpeWhoToFollow = 0;
        List<Thread> lstWhoToFollowScrapeThread = new List<Thread>();
        bool Is_Stop_ScrapeWhoToFollow = false;
        bool ProcessStopped = false; 
        #endregion

        private void btnWhoToFollowScrape_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                //StartWhoToFollowScrape();
                new Thread(() =>
                {
                    StartWhoToFollowScrape();
                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }
       
        private void StartWhoToFollowScrape()
        {
            try
            {
                Is_Stop_ScrapeWhoToFollow = false;
                if (Is_Stop_ScrapeWhoToFollow)
                {
                    return;
                }
                lstWhoToFollowScrapeThread.Add(Thread.CurrentThread);
                lstWhoToFollowScrapeThread = lstWhoToFollowScrapeThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;

            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }

            int NoOfPages = 5;

            if (!string.IsNullOrEmpty(txtWhoTofolllowNoOfPages.Text) && NumberHelper.ValidateNumber(txtWhoTofolllowNoOfPages.Text))
            {
                NoOfPages = Int32.Parse(txtWhoTofolllowNoOfPages.Text);
            }

            if (lstscrapeWhoTofoollow.Count <= 0)
            {
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please choose name file from browse option. ]");
                return;
            }

            ThreadPool.SetMaxThreads(5, 5);

            if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(StartAddtoFollow), new object[] { item, NoOfPages });
                }
            }
            else
            {
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Please Upload Accounts ]");
            }
        }

        public void StartAddtoFollow(object data)
        {
            try
            {
                try
                {
                    if (Is_Stop_ScrapeWhoToFollow)
                    {
                        return;
                    }
                    lstWhoToFollowScrapeThread.Add(Thread.CurrentThread);
                    lstWhoToFollowScrapeThread = lstWhoToFollowScrapeThread.Distinct().ToList();
                    Thread.CurrentThread.IsBackground = true;

                }
                catch
                { }

                Array paramsArray = new object[2];

                paramsArray = (Array)data;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);
                int NoOFPages = (int)paramsArray.GetValue(1);
                TweetAccountManager tweetAccountManager = keyValue.Value;

                AddThreadToDictionary(strModule(Module.WhoToScrap), tweetAccountManager.Username);

                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Logging In With " + keyValue.Key + " ]");
                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                    if (!tweetAccountManager.IsLoggedIn)
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Not Logged In With " + keyValue.Key + " ]");
                        return;
                    }
                }
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Logged In With " + keyValue.Key + " ]");

                if (tweetAccountManager.AccountStatus == "Account Suspended")
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(tweetAccountManager.Username);

                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Account Suspended With User Name : " + tweetAccountManager.Username + " ]");
                    return;
                }

                foreach (string Keyword in lstscrapeWhoTofoollow)
                {
                    ScrapeWhotoFollow(tweetAccountManager, Keyword, NoOFPages);
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartAddtoFollow() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartAddtoFollow() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                counterFoScrpeWhoToFollow++;
                if (counterFoScrpeWhoToFollow == TweetAccountContainer.dictionary_TweetAccount.Count)
                {
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    AddToScrapeLogs("------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        private void ScrapeWhotoFollow(TweetAccountManager tweetAccountManager, string Keyword, int NoOfPages)
        {
            try
            {
                AddThreadToDictionary(strModule(Module.WhoToScrap), tweetAccountManager.Username);
                int counter = 0;
                int PageCount = 1;
               // int NoOfData = NoOfPages * 20;
                List<string> username = new List<string>();
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Getting Users To Scrape Data For ]");
                while (counter < NoOfPages)
                {
                   // string pagsource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/who_to_follow/suggestions/search/users?q=" + Keyword + "&cursor=" + PageCount + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                    string pagsource = "";
                    if (PageCount == 1)
                    {
                         pagsource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?q=" + Keyword + "&cursor=" + PageCount + "&include_available_features=1&include_entities=1&is_forward=true"), "", ""); // https://twitter.com/who_to_follow/suggestions/search/users?q=software&cursor=1&include_available_features=1&include_entities=1&is_forward=true
                    }
                    else
                    {
                        //if (pagsource.Contains("\"has_more_items\":true"))
                        string uri = "https://twitter.com/i/search/timeline?q=" + Keyword + "&mode=users&include_available_features=1&include_entities=1&last_note_ts=555&scroll_cursor=USER-0-" + PageCount * 20;
                        pagsource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?q=" + Keyword + "&mode=users&include_available_features=1&include_entities=1&last_note_ts=555&scroll_cursor=USER-0-" + PageCount * 20), "", "");
                    }
                  //  if (pagsource.Contains("has-more-items"))
                    if (pagsource.Contains("has_more_items"))
                    {
                        PageCount++;
                         string[] Aray = Regex.Split(pagsource, "js-stream-item stream-item");
                        //string[] Aray = Regex.Split(pagsource, "js-stream-item stream-item stream-user-item");  //js-stream-item stream-item
                      //  string[] Aray = Regex.Split(pagsource, "fullname js-action-profile-name");
                        Aray = Aray.Skip(1).ToArray();
                        foreach (string item in Aray)
                        {
                            string Userid = string.Empty;
                            string Username = string.Empty;
                            try
                            {
                                //int startindex = item.IndexOf("=\\\"");
                                int startindex = item.IndexOf("data-user-id=\"");
                                int startindexForItem = item.IndexOf("data-item-id=\\\"");
                                if (startindex >= 0 && PageCount==2)
                                {
                                    string start = item.Substring(startindex).Replace("data-user-id=\"", "");
                                    //string start = item.Substring(startindex).Replace("=\\\"", "");
                                    //int endindex = start.IndexOf("\\\"");
                                    int endindex = start.IndexOf("\"");
                                    string end = start.Substring(0, endindex);
                                    Userid = end;
                                }
                                else if (startindexForItem >= 0)
                                {
                                    string start = item.Substring(startindexForItem).Replace("data-item-id=\\\"", "");
                                    //string start = item.Substring(startindex).Replace("=\\\"", "");
                                    //int endindex = start.IndexOf("\\\"");
                                    int endindex = start.IndexOf("\\\"");
                                    string end = start.Substring(0, endindex);
                                    Userid = end;
                                   
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            try
                            {
                                //int startindex = item.IndexOf("data-screen-name=\\\"");
                                int startindex = item.IndexOf("data-screen-name=\"");
                                int startindex2 = item.IndexOf("data-screen-name=\\\"");
                                if (startindex >= 0)
                                {
                                    //string start = item.Substring(startindex).Replace("data-screen-name=\\\"", "");
                                    string start = item.Substring(startindex).Replace("data-screen-name=\"", "");
                                    //int endindex = start.IndexOf("\\\"");
                                    int endindex = start.IndexOf("\"");
                                    string end = start.Substring(0, endindex);
                                    Username = end;
                                    username.Add(end);
                                }

                                if (startindex2 >= 0)
                                {
                                    //string start = item.Substring(startindex).Replace("data-screen-name=\\\"", "");
                                    string start = item.Substring(startindex2).Replace("data-screen-name=\\\"", "");
                                    //int endindex = start.IndexOf("\\\"");
                                    int endindex = start.IndexOf("\\\"");
                                    string end = start.Substring(0, endindex);
                                    Username = end;
                                    username.Add(end);
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            try
                            {
                                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Userid))
                                {
                                    string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + Username + "' ,'" + Userid + "') ";
                                    DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() --> Database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() --> DataBase --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + Username + " ::  " + Userid + " ]");
                        }
                        username = username.Distinct().ToList();
                        counter++;
                    }
                    else
                    {
                        AddToScrapeLogs("[ " + DateTime.Now + " ] => [ No More Pages To Scrape For Keyword : " + Keyword + " ]");
                        break;
                    }
                }

                if (!File.Exists(Globals.Path_KeywordScrapedListData + ".csv"))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO  , LOCATION , WEBSITE , NOOFTWEETS , FOLLOWING , FOLLOWERS ", Globals.Path_KeywordScrapedListData + ".csv");
                }

                foreach (string UserIds in username)
                {
                    try
                    {
                        string ProfileName = string.Empty;
                        string Location = string.Empty;
                        string Bio = string.Empty;
                        string website = string.Empty;
                        string NoOfTweets = string.Empty;
                        string Followers = string.Empty;
                        string Followings = string.Empty;
                        string userids = string.Empty;
                        string TweetUsername = string.Empty;
                        string Userid = string.Empty;
                        string Username = string.Empty;
                        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                        string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + UserIds), "", "");

                        string Responce = ProfilePageSource;

                        #region Convert HTML to XML

                        string xHtml = objChilkat.ConvertHtmlToXml(Responce);
                        Chilkat.Xml xml = new Chilkat.Xml();
                        xml.LoadXml(xHtml);

                        Chilkat.Xml xNode = default(Chilkat.Xml);
                        Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                        #endregion

                        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "ul", "class", "stats js-mini-profile-stats");
                        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                        //userids = xNode.GetAttrValue("data-user-id");
                        //userids = xNode.AccumulateTagContent("text", "script|style");
                        if (Responce.Contains("has-more-items"))
                        {
                            PageCount++;
                            //string[] Aray = Regex.Split(Responce, "js-stream-item stream-item stream-item");
                            //Aray = Aray.Skip(1).ToArray();
                            //foreach (string item in Aray)
                            {

                                try
                                {
                                    #region commented
                                    //int startindex = item.IndexOf("=\\\"");
                                    //int startindex = item.IndexOf("data-user-id=");
                                    //if (startindex >= 0)
                                    //{
                                    //    string start = item.Substring(startindex).Replace("data-user-id=\"", "");
                                    //    //string start = item.Substring(startindex).Replace("=\\\"", "");
                                    //    //int endindex = start.IndexOf("\\\"");
                                    //    int endindex = start.IndexOf("\"");
                                    //    string end = start.Substring(0, endindex);
                                    //    userids = end;
                                    //    break;
                                    //} 
                                    #endregion

                                    int startindex = Responce.IndexOf("profile_id");
                                    string start = Responce.Substring(startindex).Replace("profile_id", "");
                                    int endindex = start.IndexOf(",");
                                    string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Trim();
                                    userids = end.Trim();
                                    TweetUsername = UserIds;

                                }

                                catch (Exception ex)
                                {
                                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() 1--> " + ex.Message, Globals.Path_TwtErrorLogs);
                                }
                            }
                        }
                        int counterdata = 0;
                        xBeginSearchAfter = null;
                        string dataDescription = string.Empty;
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            if (counterdata == 0)
                            {
                                ProfileName = xNode.AccumulateTagContent("text", "script|style").Replace("Verified account", "");
                                counterdata++;
                            }
                            else if (counterdata == 1)
                            {
                                website = xNode.AccumulateTagContent("text", "script|style");
                                counterdata++;
                            }
                            else
                            {
                                break;
                            }
                            //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "u-textUserColor");
                        }

                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "screen-name");
                        //while ((xNode != null))
                        //{
                        //    xBeginSearchAfter = xNode;
                        //    TweetUsername = xNode.AccumulateTagContent("text", "script|style");
                        //    break;
                        //}


                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "ProfileHeaderCard-bio u-dir");//bio profile-field");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            Bio = xNode.AccumulateTagContent("text", "script|style").Replace("&#39;", "'").Replace("&#13;&#10;", string.Empty).Trim();
                            break;
                        }

                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "location profile-field");
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            Location = xNode.AccumulateTagContent("text", "script|style");
                            break;
                        }

                        int counterData = 0;
                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "ProfileHeaderCard-bio u-dir");//bio profile-field");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            Bio = xNode.AccumulateTagContent("text", "script|style").Replace("&#39;", "'").Replace("&#13;&#10;", string.Empty).Trim();
                            break;
                        }

                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "location profile-field");
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//location profile-field");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            Location = xNode.AccumulateTagContent("text", "script|style");
                            break;
                        }

                        xBeginSearchAfter = null;
                        dataDescription = string.Empty;
                        //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "data-element-term", "tweet_stats");
                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");
                        while ((xNode != null))
                        {
                            xBeginSearchAfter = xNode;
                            if (counterData == 0)
                            {
                                NoOfTweets = xNode.AccumulateTagContent("text", "script|style").Replace("Tweets", string.Empty).Replace(",", string.Empty).Replace("Tweet", string.Empty);
                                counterData++;
                            }
                            else if (counterData == 1)
                            {
                                Followings = xNode.AccumulateTagContent("text", "script|style").Replace(" Following", string.Empty).Replace(",", string.Empty).Replace("Following", string.Empty);
                                counterData++;
                            }
                            else if (counterData == 2)
                            {
                                Followers = xNode.AccumulateTagContent("text", "script|style").Replace("Followers", string.Empty).Replace(",", string.Empty).Replace("Follower", string.Empty);
                                counterData++;
                            }
                            else
                            {
                                break;
                            }
                            //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "js-nav");
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-openSignupDialog js-nonNavigable u-textUserColor");
                        }


                        if (!string.IsNullOrEmpty(userids))
                        {
                            lock (WhoTofollowThreadLock)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(userids + "," + TweetUsername + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", ""), Globals.Path_KeywordScrapedListData + ".csv");
                            }
                            AddToScrapeLogs("[ " + DateTime.Now + " ] => [ " + userids + "," + TweetUsername + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ]");
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() 3--> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() 3--> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Finished Scraping For " + tweetAccountManager.Username + " ]");
                AddToIPsLogs("-----------------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> ScrapeWhotoFollow() 2--> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> ScrapeWhotoFollow() 2--> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        private void btnUploadWhoTofollow_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtUploadWhoTofollow.Text = ofd.FileName;
                    lstscrapeWhoTofoollow.Clear();
                    lstscrapeWhoTofoollow = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToScrapeLogs("[ " + DateTime.Now + " ] => [ Uploaded " + lstscrapeWhoTofoollow.Count + " Names To Scrape ]");
                }
            }
        }

        private void btnStopWhoToFollow_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping Follow Threads, it may take some time ]");
                //StopThreads(strModule(Module.WhoToScrap));
                stopProcessWhoToFollow();
            }).Start();

            

        }

        private void stopProcessWhoToFollow()
        {
            try
            {
                ProcessStopped = true;
                Is_Stop_ScrapeWhoToFollow = true;
                List<Thread> lstTemp = lstWhoToFollowScrapeThread.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }

                }
                AddToScrapeLogs("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
            }

            catch { }
        }

        private void chkUseDBC_CheckedChanged(object sender, EventArgs e)
        {
            #region commented
            //FrmSettings frm_setting = new FrmSettings();

            //if (chkUseDBC.Checked)
            //{
            //    chk_manulcapcha.Checked = false;
            //}

            //if (chkUseDBC.Checked)
            //{
            //    frm_setting.Show();
            //}
            //else
            //{
            //    frm_setting.Hide();

            //} 
            #endregion

            //if (chkUseDBC.Checked)
            //{
            //    FrmSettings frm_setting = new FrmSettings();
            //    frm_setting.ShowDialog();
            //    //chkUseDBC.Checked = false;
            //}

            //else
            //{
            //    FrmSettings frm_setting = new FrmSettings();
            //    frm_setting.Close();
            //}
        }

        private void frm_setting_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //private void rdo_UnfollowbyUserId_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (rdo_UnfollowbyUserId.Checked == true)
        //        {
        //            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
        //            {
        //                lst_unfolloweruserlist.Clear();
        //                ofd.Filter = "Text Files (*.txt)|*.txt";
        //                ofd.InitialDirectory = Application.StartupPath;
        //                if (ofd.ShowDialog() == DialogResult.OK)
        //                {
        //                    txt_FilePathunfollowUserslist.Text = ofd.FileName;
        //                    lst_unfolloweruserlist = GlobusFileHelper.ReadLargeFile(ofd.FileName);
        //                }
        //                AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ " + lst_unfolloweruserlist.Count + " Unfollowers Uploaded ]");
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        private void txt_ScrapebySearchkeywordsettingMin_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMin.Text))
            {
                txt_ScrapebySearchkeywordsettingMin.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        private void txt_ScrapebySearchkeywordsettingMax_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_ScrapebySearchkeywordsettingMax.Text))
            {
                txt_ScrapebySearchkeywordsettingMax.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        private void txt_FilterbyFollowersMin_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_FilterbyFollowersMin.Text))
            {
                txt_FilterbyFollowersMin.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        private void txt_FilterbyFollowersMax_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_FilterbyFollowersMax.Text))
            {
                txt_FilterbyFollowersMax.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        private void txt_FilterbyFollowingMin_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_FilterbyFollowingMin.Text))
            {
                txt_FilterbyFollowingMin.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        private void txt_FilterbyFollowingMax_TextChanged(object sender, EventArgs e)
        {
            if (!NumberHelper.ValidateNumber(txt_FilterbyFollowingMax.Text))
            {
                txt_FilterbyFollowingMax.Text = "";
                MessageBox.Show("Please Enter Only No");
            }
        }

        //private void chk_manulcapcha_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chk_manulcapcha.Checked)
        //    {
        //        chkUseDBC.Checked = false;
        //    }
        //}

        private void useFollowerFollowingFromUsername_CheckedChanged(object sender, EventArgs e)
        {
            //IsUsingFollowerFollowing = true;
        }


        private void campainManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (!Globals.IsFreeVersion)
                {
                    if (Unfollower.IsCampaignManagerOpen)
                    {
                        MixedCampaignManager.frm_mixcampaignmanager frmCampManager = new MixedCampaignManager.frm_mixcampaignmanager();
                        frmCampManager.Show();
                        Unfollower.IsCampaignManagerOpen = false;
                    }
                    else
                    {
                        MessageBox.Show("Campaign Form is already Open.", "Alert");
                    }
                }
                else
                {

                    frmForPremimumUsersOnly frmFreeTrial = new frmForPremimumUsersOnly();
                    frmFreeTrial.TopMost = true;
                    frmFreeTrial.BringToFront();
                    frmFreeTrial.ShowDialog();

                }
            }
            catch { }
        }

        #region <<change all prevous details of account>>

        List<string> lst_AcManagerUserName = new List<string>();

        List<string> lst_AcManagerEmail = new List<string>();

        bool _AcManagerSelectRandomValuesFromList = false;

        int _CountThreadAccountManagerForChAngeEmailOrScreenName = 0;

        readonly object locker_AccountManagerForChAngeEmailOrScreenName = new object();

        private void btn_BrowseFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_AcManagerFolderPath.Text = ofd.SelectedPath;
                    lstBackgroundPics = new List<string>();
                    string[] FilesPath = Directory.GetFiles(ofd.SelectedPath);

                    if (FilesPath.Count() > 0)
                    {
                        new Thread(() =>
                        {
                            UploadAcManagerFiles(FilesPath);
                        }).Start();
                    }
                }
            }
        }

        public void UploadAcManagerFiles(string[] FilesPath)
        {
            foreach (string filePath in FilesPath)
            {
                if (filePath.Contains("Emails.txt") || filePath.Contains("email.txt") || filePath.Contains("Email.txt"))
                {
                    lst_AcManagerEmail = GlobusFileHelper.ReadFiletoStringList(filePath);
                }
                else if (filePath.Contains("UserName.txt") || filePath.Contains("User Name.txt") || filePath.Contains("user name.txt") || filePath.Contains("username.txt"))
                {
                    lst_AcManagerUserName = GlobusFileHelper.ReadFiletoStringList(filePath);
                }
            }

            //Print Count of emails and user names

            AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + lst_AcManagerEmail.Count + "  Emails Uploaded. ]");
            AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + lst_AcManagerUserName.Count + " User Names uploaded. ]");
        }

        private void btn_StartAcManager_Click(object sender, EventArgs e)
        {
            btn_stopAcManager.Enabled = true;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                if (!string.IsNullOrEmpty(txt_AcManagerFolderPath.Text.Trim()))
                {
                    new Thread(() =>
                    {
                        StartAccountManagerProcess();
                    }).Start();
                }
                else
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please upload Proper file ]");
                }

            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartAccountManagerProcess()
                 {
            int NoOfThread = 0;
            int Startdelay = 0;
            int EndDelay = 0;
            bool isChangeScreenName = false;
            bool isChangeEmail = false;

            if (chk_AcManagerUserName.Checked && lst_AcManagerUserName.Count == 0)
            {
                // if option is checked but list is blank
                // then print a MSg in logger 
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please Upload User screen name. ]");
                return;
            }
            else
            {
                isChangeScreenName = true;
            }

            if (chk_AcManagerEmail.Checked && lst_AcManagerEmail.Count == 0)
            {
                // if option is checked but list is blank
                // then print a MSg in logger 
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please Upload Email Id. ]");
                return;
            }
            else
            {
                isChangeEmail = true;
            }

            if (chk_AcManagerSelectRandomValuesfromList.Checked)
            {
                // check rendome values from list 

                _AcManagerSelectRandomValuesFromList = true;
            }

            ///Delay Time 

            if (!String.IsNullOrEmpty(txt_ModuleAccountManagerdelayStart.Text) && !String.IsNullOrEmpty(txt_ModuleAccountManagerdelayEnd.Text))
            {
                Startdelay = int.Parse(txt_ModuleAccountManagerdelayStart.Text);
                EndDelay = int.Parse(txt_ModuleAccountManagerdelayEnd.Text);
            }


            //Check Account is uploaded ...
            if (TweetAccountContainer.dictionary_TweetAccount.Count == 0)
            {
                //Please Upload Accounts ..
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please Upload an Accounts from account tab. ]");
                return;
            }

            //Threads 
            if (BaseLib.NumberHelper.ValidateNumber(txt_AcPassNumberOfThread.Text))
            {
                NoOfThread = int.Parse(txt_AcPassNumberOfThread.Text);
            }

            // If Both files are not null or not empty 
            foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
            {
                ((TweetAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_addToAcManagerLogger);
                //GetChangeAccountDetails(new object[] { item });

                ///Manager Threads from user given Number of threads ..
                if (_CountThreadAccountManagerForChAngeEmailOrScreenName >= NoOfThread)
                {
                    lock (locker_AccountManagerForChAngeEmailOrScreenName)
                    {
                        Monitor.Wait(locker_AccountManagerForChAngeEmailOrScreenName);
                    }
                }

                Thread GetStartProcessForChAngeAcPassword = new Thread(GetChangeAccountDetails);
                GetStartProcessForChAngeAcPassword.Name = "AccountManager" + "_" + item.Key;
                GetStartProcessForChAngeAcPassword.IsBackground = true;
                GetStartProcessForChAngeAcPassword.Start(new object[] { item, Startdelay, EndDelay, isChangeScreenName, isChangeEmail });

                //Thread.Sleep(1000); 

            }
        }

        public void GetChangeAccountDetails(Object Param)
        {
            try
            {
                accntManagerThreadStart = true;
                Lst_accntManagerThread.Add(Thread.CurrentThread);
                Lst_accntManagerThread = Lst_accntManagerThread.Distinct().ToList();
                //Thread.CurrentThread.IsBackground = true;
            }
            catch (Exception)
            {
            }
            try
            {
                ///Incease counter of thread ...

                Interlocked.Increment(ref _CountThreadAccountManagerForChAngeEmailOrScreenName);

                // get value from param 
                Array _ParamValue = new object[2];
                _ParamValue = (Array)Param;


                KeyValuePair<string, TweetAccountManager> _KeyValue = (KeyValuePair<string, TweetAccountManager>)_ParamValue.GetValue(0);

                int StartDelay = (int)_ParamValue.GetValue(1);
                int EndDelay = (int)_ParamValue.GetValue(2);
                bool isChangeScreenName = (bool)_ParamValue.GetValue(3); ;
                bool isChangeEmail = (bool)_ParamValue.GetValue(4); ;


                TweetAccountManager AcManager = (TweetAccountManager)_KeyValue.Value;

                AcManager.Login();

                if (!AcManager.IsLoggedIn)
                {
                    // Account is not loggedin 
                    return;
                }
                if (!AcManager.IsNotSuspended)
                {
                    // account is suspended.. 
                    return;
                }


                //Create profile manager class object and Call Method for postig values 

                ProfileManager.ProfileManager Profilemanger = new ProfileManager.ProfileManager();

                Profilemanger.logEvents.addToLogger += new EventHandler(logEvents_addToAcManagerLogger);

                #region
                //if (_AcManagerSelectRandomValuesFromList)
                //{
                //    // get a rendome number for email and user name 
                //    String _email = string.Empty;
                //    String _UserScreenName = string.Empty;

                //    int _RendomeNumberForUserName = 0;
                //    int _RendomeNumberForEmail = 0;

                //    if (lst_AcManagerEmail.Count != 0 && isChangeEmail)
                //    {
                //        // Get Values From list 

                //        _RendomeNumberForEmail = (BaseLib.RandomNumberGenerator.GenerateRandom(0, lst_AcManagerEmail.Count));
                //        _email = lst_AcManagerEmail[_RendomeNumberForEmail];
                //    }

                //    if (lst_AcManagerUserName.Count != 0 && isChangeScreenName)
                //    {
                //        // Get Values From list 

                //        _RendomeNumberForUserName = (BaseLib.RandomNumberGenerator.GenerateRandom(0, lst_AcManagerUserName.Count));
                //        _UserScreenName = lst_AcManagerUserName[_RendomeNumberForUserName];
                //    }

                //    try
                //    {
                //        //Profilemanger.UpdateProfileAccount(AcManager.postAuthenticityToken, _UserScreenName, _email, AcManager.Password, ref AcManager.globusHttpHelper);

                //        //call update account method for changing details of account..

                //        bool _IsPassChanged = false;

                //        _IsPassChanged = Profilemanger.UpdateProfileAccount(AcManager.postAuthenticityToken, _UserScreenName, _email, AcManager.Password, ref AcManager.globusHttpHelper);


                //        if (_IsPassChanged)
                //        {
                //            //Print in logger 
                //            //Add In Text file 
                //            string Content = _email + ":" + AcManager.Password + ":" + _UserScreenName;
                //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_SuccessFillyChangeUserAndEmail);
                //        }
                //        else
                //        {
                //            //Email Or user screen name changing failure 
                //            string Content = AcManager.Username + ":" + AcManager.Password + ":" + _UserScreenName;
                //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_FailedChangingUserAndEmail);
                //        }

                //        ///get rendome Delay time ..
                //        int DelayTime = BaseLib.RandomNumberGenerator.GenerateRandom(StartDelay, EndDelay);
                //        AddToLog_AcManager("Delay For " + DelayTime);
                //        Thread.Sleep(DelayTime);

                //    }
                //    catch (Exception ex)
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine("Method :- GetChangeAccountDetails >>  Message :- " + ex.Message + "  >> Error :- " + ex.StackTrace, Globals.Path_ErrorLogForAccountManager);

                //    }


                //}
                //else
                //{
                //    int _CountUserName = 0;
                //    int _CountEmail = 0;
                //    //while (true)
                //    {
                //        String _email = string.Empty;
                //        String _UserScreenName = string.Empty;

                //        if (lst_AcManagerEmail.Count != 0)
                //        {
                //            _CountEmail = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), AcManager.Username);

                //            if (lst_AcManagerEmail.Count >= _CountEmail)
                //            {
                //                _email = lst_AcManagerEmail[_CountEmail];
                //            }
                //            else
                //            {
                //                _email = lst_AcManagerEmail[0];
                //            }
                //        }

                //        if (lst_AcManagerEmail.Count != 0 && isChangeEmail)
                //        {

                //            _CountUserName = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), AcManager.Username);

                //            if (lst_AcManagerUserName.Count >= _CountUserName)
                //            {
                //                _UserScreenName = lst_AcManagerUserName[_CountUserName];
                //            }
                //            else
                //            {
                //                _UserScreenName = lst_AcManagerUserName[0];
                //            }
                //        }

                //        try
                //        {
                //            //call update account method for changing details of account..

                //            bool _IsPassChanged = false;

                //            _IsPassChanged = Profilemanger.UpdateProfileAccount(AcManager.postAuthenticityToken, _UserScreenName, _email, AcManager.Password, ref AcManager.globusHttpHelper);


                //            if (_IsPassChanged)
                //            {
                //                //Print in logger 
                //                //Add In Text file 
                //                string Content = _email + ":" + AcManager.Password + ":" + _UserScreenName;
                //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_SuccessFillyChangeUserAndEmail);
                //            }
                //            else
                //            {
                //                //Email Or user screen name changing failure 
                //                string Content = AcManager.Username + ":" + AcManager.Password + ":" + _UserScreenName;
                //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_FailedChangingUserAndEmail);
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            GlobusFileHelper.AppendStringToTextfileNewLine("Method :- GetChangeAccountDetails >>  Message :- " + ex.Message + "  >> Error :- " + ex.StackTrace, Globals.Path_ErrorLogForAccountManager);

                //        }

                //        ///get rendome Delay time ..
                //        int DelayTime = BaseLib.RandomNumberGenerator.GenerateRandom(StartDelay, EndDelay);
                //        AddToLog_AcManager("Delay For " + DelayTime);
                //        Thread.Sleep(DelayTime);
                //    }
                //}
                #endregion



                //Initalize Variables for Email and Screen name ..
                String _email = string.Empty;
                String _UserScreenName = string.Empty;

                // get a rendome number for email and user name 
                if (_AcManagerSelectRandomValuesFromList)
                {
                    int _RendomeNumberForUserName = 0;
                    int _RendomeNumberForEmail = 0;

                    if (lst_AcManagerEmail.Count != 0 && isChangeEmail)
                    {
                        // Get Values From list 

                        _RendomeNumberForEmail = (BaseLib.RandomNumberGenerator.GenerateRandom(0, lst_AcManagerEmail.Count));
                        _email = lst_AcManagerEmail[_RendomeNumberForEmail];
                    }

                    if (lst_AcManagerUserName.Count != 0 && isChangeScreenName)
                    {
                        // Get Values From list 

                        _RendomeNumberForUserName = (BaseLib.RandomNumberGenerator.GenerateRandom(0, lst_AcManagerUserName.Count));
                        _UserScreenName = lst_AcManagerUserName[_RendomeNumberForUserName];
                    }
                }
                else
                {
                    int _CountScreenName = 0;
                    int _CountEmail = 0;

                    if (lst_AcManagerEmail.Count != 0 && isChangeEmail)
                    {
                        _CountEmail = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), AcManager.Username);

                        if (lst_AcManagerEmail.Count >= _CountEmail)
                        {
                            _email = lst_AcManagerEmail[_CountEmail];
                        }
                        else
                        {
                            _email = lst_AcManagerEmail[0];
                        }
                    }

                    if (lst_AcManagerUserName.Count != 0 && isChangeScreenName)
                    {

                        _CountScreenName = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), AcManager.Username);

                        if (lst_AcManagerUserName.Count >= _CountScreenName)
                        {
                            _UserScreenName = lst_AcManagerUserName[_CountScreenName];
                        }
                        else
                        {
                            _UserScreenName = lst_AcManagerUserName[0];
                        }
                    }
                }

                try
                {

                    //call update account method for changing details of account..

                    bool Status = false;
                    string _Responce = string.Empty;

                    Status = Profilemanger.UpdateProfileAccount(AcManager.postAuthenticityToken, _UserScreenName, _email, AcManager.Password, ref AcManager.globusHttpHelper, out _Responce);

                    if (Status)
                    {

                        if (!string.IsNullOrEmpty(_UserScreenName) && chk_AcManagerUserName.Checked)
                        {
                            //Print in logger 
                            //Add In Text file 
                            AddToLog_AcManager("[" + DateTime.Now + " ] => [ " + _UserScreenName + "  has changed in :-" + AcManager.Username + " ]");
                            //string Content = _email + ":" + AcManager.Password + ":" + _UserScreenName;
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(_UserScreenName, Globals.Path_SuccessFullyChangeUserName);
                            //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(_email, Globals.Path_SuccessFillyChangeEmail);
                        }

                        if (!string.IsNullOrEmpty(_email) && chk_AcManagerEmail.Checked)
                        {
                            AddToLog_AcManager("[" + DateTime.Now + " ] => [ " + _email + "  has changed in :-" + AcManager.Username + " ]");
                            //string Content = _email + ":" + AcManager.Password + ":" + _UserScreenName;
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(_email, Globals.Path_SuccessFullyChangeEmail);
                            //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_SuccessFillyChangeUserAndEmail);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_UserScreenName) && chk_AcManagerUserName.Checked)
                        {
                            AddToLog_AcManager("[" + DateTime.Now + " ] => [ " + _UserScreenName + "  has not changed in :-" + AcManager.Username + " ]");
                        }
                        AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + _Responce + " From :- " + AcManager.Username + " ]");
                        //Email Or user screen name changing failure 
                        string Content = AcManager.Username + ":" + AcManager.Password;
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Content, Globals.Path_FailedChangingUserAndEmail);
                    }
                        
                    
                    ///get rendome Delay time ..
                    int DelayTime = BaseLib.RandomNumberGenerator.GenerateRandom(StartDelay, EndDelay);
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Delay For " + DelayTime + "  sec]");
                    Thread.Sleep(DelayTime * 1000);
                    AddToLog_AcManager("[ " + DateTime.Now + " ]=> PROCESS COMPLETED.");

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("Method :- GetChangeAccountDetails >>  Message :- " + ex.Message + "  >> Error :- " + ex.StackTrace, Globals.Path_ErrorLogForAccountManager);

                }
                finally
                {
                    Profilemanger.logEvents.addToLogger -= new EventHandler(logEvents_addToAcManagerLogger);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Method :- GetChangeAccountDetails >>  Message :- " + ex.Message + "  >> Error :- " + ex.StackTrace, Globals.Path_ErrorLogForAccountManager);
            }
            finally
            {
                ///Decrement  counter of thread ...

                Interlocked.Decrement(ref _CountThreadAccountManagerForChAngeEmailOrScreenName);


                lock (locker_AccountManagerForChAngeEmailOrScreenName)
                {
                    Monitor.Pulse(locker_AccountManagerForChAngeEmailOrScreenName);
                }
            }
        }

        void logEvents_addToAcManagerLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_AcManager(eArgs.log);
            }
        }

        private void AddToLog_AcManager(string log)
        {
            try
            {
                if (lstLogger_AcManager.InvokeRequired)
                {
                    lstLogger_AcManager.Invoke(new MethodInvoker(delegate
                    {
                        lstLogger_AcManager.Items.Add(log);
                        lstLogger_AcManager.SelectedIndex = lstLogger_AcManager.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstLogger_AcManager.Items.Add(log);
                    lstLogger_AcManager.SelectedIndex = lstLogger_AcManager.Items.Count - 1;
                }
            }
            catch { }
        }

        private void tb_AccountManger_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        #endregion

        #region << Account Password change >>

        List<string> lst_NewPassword = new List<string>();

        bool _IsChooseRandomPassword = false;

        public int _CountThreadChangeAcPass = 0;

        public readonly object lockerThreadsChangeAcPass = new object();

        private void btn_AcManagerNewPasswordFile_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_AcManagerNewPassFilePath.Text = ofd.FileName;
                    lst_NewPassword = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                }
            }
            AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + lst_NewPassword.Count + " Password Uploaded. ]");
        }

        private void btn_StartPassChange_Click(object sender, EventArgs e)
        {
            btn_StopPassChange.Enabled = true;
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                new Thread(() =>
                {
                    StartPassChange();
                }).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }

        }

        public void StartPassChange()
        {
            int NoOfThread = 0;

            if (lst_NewPassword.Count == 0)
            {
                //Please Upload new password list 
                return;
            }

            //Check Account is uploaded ...
            if (TweetAccountContainer.dictionary_TweetAccount.Count == 0)
            {
                //Please Upload Accounts ..
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please Upload an Accounts from account tab. ]");
                return;
            }

            //Threads 
            if (BaseLib.NumberHelper.ValidateNumber(txt_AcPassNumberOfThread.Text))
            {
                NoOfThread = int.Parse(txt_AcPassNumberOfThread.Text);
            }

            //Choose random Password
            if (chk_RandomPass.Checked)
            {
                _IsChooseRandomPassword = true;
            }

            // If Both files are not null or not empty 
            foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
            {
                ((TweetAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_addToAcManagerLogger);
                //GetChangeAccountPass(new object[] { item });

                if (_CountThreadChangeAcPass >= NoOfThread)
                {
                    lock (lockerThreadsChangeAcPass)
                    {
                        Monitor.Wait(lockerThreadsChangeAcPass);
                    }
                }

                Thread GetStartProcessForChAngeAcPassword = new Thread(GetChangeAccountPass);
                GetStartProcessForChAngeAcPassword.Name = "AcPassChange" + "_" + item.Key;
                GetStartProcessForChAngeAcPassword.IsBackground = true;
                GetStartProcessForChAngeAcPassword.Start(new object[] { item });

                //Thread.Sleep(1000);
            }
        }

        public void GetChangeAccountPass(Object Param)
        {

            try
            {
                accntPasswordThreadStart = true;
                Lst_accntPasswordThread.Add(Thread.CurrentThread);
                Lst_accntPasswordThread = Lst_accntPasswordThread.Distinct().ToList();

            }
            catch (Exception)
            {
            }
            try
            {
                Interlocked.Increment(ref _CountThreadChangeAcPass);
                // get value from param 
                Array _ParamValue = new object[2];
                _ParamValue = (Array)Param;

                KeyValuePair<string, TweetAccountManager> _KeyValue = (KeyValuePair<string, TweetAccountManager>)_ParamValue.GetValue(0);

                TweetAccountManager AcManager = (TweetAccountManager)_KeyValue.Value;

                AcManager.Login();

                if (!AcManager.IsLoggedIn)
                {
                    // Account is not loggedin 
                    return;
                }
                if (!AcManager.IsNotSuspended)
                {
                    // account is suspended.. 
                    return;
                }

                string _PassWord = string.Empty;
                if (_IsChooseRandomPassword)
                {
                    //Get random password from list 
                    int _RendomePass = 0;
                    _RendomePass = (BaseLib.RandomNumberGenerator.GenerateRandom(0, lst_NewPassword.Count));
                    _PassWord = lst_NewPassword[_RendomePass];
                }
                else
                {
                    //get key index from dictionary 
                    int _Passindex = 0;
                    if (TweetAccountContainer.dictionary_TweetAccount.Count >= lst_NewPassword.Count)
                    {
                        _Passindex = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), AcManager.Username);

                    }
                    _PassWord = lst_NewPassword[_Passindex];
                }

                //Create profile manager class object and Call Method for postig values 

                ProfileManager.ProfileManager Profilemanger = new ProfileManager.ProfileManager();
                Profilemanger.logEvents.addToLogger += new EventHandler(logEvents_addToAcManagerLogger);

                //StartAgain:
                bool _IsPassChanged = false;
                string message = string.Empty;

                _IsPassChanged = Profilemanger.UpdateAccountPass(AcManager.postAuthenticityToken, AcManager.Password, _PassWord, ref AcManager.globusHttpHelper, out message);

                if (_IsPassChanged)
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + AcManager.Username + ":- " + message + " ]");
                    //write new password in text file 
                    string content = AcManager.Username + ":" + _PassWord;
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(content, BaseLib.Globals.Path_SuccessFullyPasswordChange);
                }
                else
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ " + AcManager.Username + ":- " + message + " ]");
                    //Path_FailedChangingPass
                    string content = AcManager.Username + ":" + _PassWord;
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(content, BaseLib.Globals.Path_SuccessFullyPasswordChange);
                    //goto StartAgain;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Interlocked.Decrement(ref _CountThreadChangeAcPass);
                lock (lockerThreadsChangeAcPass)
                {
                    Monitor.Pulse(lockerThreadsChangeAcPass);
                }
            }
        }


        private void btnDisableEmailNotification_Click(object sender, EventArgs e)
        {
            try
            {
                CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

                if (CheckNetConn)
                {
                    new Thread(() =>
                    {
                        startDisableEmailNotification();
                    }).Start();
                }
                else
                {
                    MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        int counterForstartDisableEmailNotification = 1;
        public void startDisableEmailNotification()
        {
            try
            {
                if (TweetAccountContainer.dictionary_TweetAccount.Count == 0)
                {
                    //Please Upload Accounts ..
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Please Upload an Accounts from account tab. ]");
                    return;
                }

                //Threads 
                counterForstartDisableEmailNotification = TweetAccountContainer.dictionary_TweetAccount.Count;

                // If Both files are not null or not empty 
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    ((TweetAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_addToAcManagerLogger);
                    //GetChangeAccountPass(new object[] { item });

                    Thread GetStartProcessForChAngeAcPassword = new Thread(GetEmailDisableProcess);
                    GetStartProcessForChAngeAcPassword.Name = "AcDiableEmailVerification" + "_" + item.Key;
                    GetStartProcessForChAngeAcPassword.IsBackground = true;
                    GetStartProcessForChAngeAcPassword.Start(new object[] { item });

                    //Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            { 
            
            }
        }

        public void GetEmailDisableProcess(object Param)
        {
            try
            {
                //accntPasswordThreadStart = true;
               // Lst_accntPasswordThread.Add(Thread.CurrentThread);
               // Lst_accntPasswordThread = Lst_accntPasswordThread.Distinct().ToList();

            }
            catch (Exception)
            {
            }
            try
            {
                // get value from param 
                Array _ParamValue = new object[2];
                _ParamValue = (Array)Param;

                KeyValuePair<string, TweetAccountManager> _KeyValue = (KeyValuePair<string, TweetAccountManager>)_ParamValue.GetValue(0);

                TweetAccountManager AcManager = (TweetAccountManager)_KeyValue.Value;

                AcManager.Login();

                if (!AcManager.IsLoggedIn)
                {
                    // Account is not loggedin 
                    return;
                }
                if (!AcManager.IsNotSuspended)
                {
                    // account is suspended.. 
                    return;
                }
                if (string.IsNullOrEmpty(AcManager.IPPort))
                {
                    AcManager.IPPort = "0";
                }
                string pagesource = AcManager.globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/settings/notifications"),"",AcManager.IPAddress,AcManager.IPPort,AcManager.IPUsername,AcManager.IPpassword);
                string postData = "authenticity_token="+AcManager.postAuthenticityToken+"&user%5Bsend_twitter_emails%5D=0";
                pagesource = AcManager.globusHttpHelper.postFormDataIP(new Uri("https://twitter.com/settings/notifications/update"), postData, "https://twitter.com/settings/notifications", AcManager.IPAddress, Convert.ToInt16(AcManager.IPPort), AcManager.IPUsername, AcManager.IPpassword);
                if (pagesource.Contains("Thanks, your notification settings have been saved."))
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ SuccessFully Disabled The Email Notification with account " + _KeyValue.Key +" ]");
                }
                else 
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ Failed to  Disabled The Email Notification. " + _KeyValue.Key + " ]");
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                //Interlocked.Decrement(ref _CountThreadChangeAcPass);
                //lock (lockerThreadsChangeAcPass)
                //{
                //    Monitor.Pulse(lockerThreadsChangeAcPass);
                //}
                counterForstartDisableEmailNotification--;
                if (counterForstartDisableEmailNotification == 0)
                {
                    AddToLog_AcManager("[ " + DateTime.Now + " ] => [ process completed  ]");
                }
            }
        }
        private void btnStopDisableEmailNotification_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region << Header Image >>

        List<string> lst_HeaderImageFile = new List<string>();

        bool _IsChooseRandomImage = false;

        public int _CountThreadChangeProfileHeaderImage = 0;

        public readonly object lockerThreadsChangeHeaderImage = new object();

        private void btn_BrowseHeaderImageFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                ofd.SelectedPath = Application.StartupPath + "\\Profile\\Pics";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_HeaderImageFolderPath.Text = ofd.SelectedPath;
                    UploadHeaderImageFiles(ofd.SelectedPath);
                }
            }
        }

        public void UploadHeaderImageFiles(string PicFolderPath)
        {
            /// get Image path from folder 
            string[] picsArray = Directory.GetFiles(PicFolderPath);

            /// Add Path in List 
            foreach (string item in picsArray)
            {
                lst_HeaderImageFile.Add(item);
            }

            /// Print Number of Image Path
            AddToListProfile("[ " + DateTime.Now + " ] => [ " + lst_HeaderImageFile.Count() + " Images Uploaded ]");
        }

        private void btn_StartChangeHeaderImages_Click(object sender, EventArgs e)
        {
            ///Call Funtion in new thread ...
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                new Thread(() => StartToChangeProfileHeaderImage()).Start();
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_Tweet("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }

        public void StartToChangeProfileHeaderImage()
        {
            int NoOfThread = 0;

            //Check image list not empty 
            if (lst_HeaderImageFile.Count == 0)
            {
                //please choose any image folder 
                //please add image 
                return;
            }

            ///Check Account is uploaded ...
            if (TweetAccountContainer.dictionary_TweetAccount.Count == 0)
            {
                ///Please Upload Accounts ..
                AddToListProfile("[ " + DateTime.Now + " ] => [ Please Upload an Accounts from account tab. ]");
                return;
            }

            ///check is random image ...
            if (chk_IsChangeRandomImage.Checked == true)
            {
                _IsChooseRandomImage = true;
            }

            //Get number of Threads 
            if (BaseLib.NumberHelper.ValidateNumber(txt_AcPassNumberOfThread.Text))
            {
                NoOfThread = int.Parse(txt_AcPassNumberOfThread.Text);
            }

            /// Get user users from account dicronary .
            foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
            {
                ((TweetAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);

                if (_CountThreadChangeProfileHeaderImage >= NoOfThread)
                {
                    lock (lockerThreadsChangeHeaderImage)
                    {
                        Monitor.Wait(lockerThreadsChangeHeaderImage);
                    }
                }

                ///Create a new the for current value ..
                Thread GetStartProcessForChAngeAcPassword = new Thread(GetChangeHeaderImages);
                GetStartProcessForChAngeAcPassword.Name = "ProfileManager" + "_" + item.Key;
                GetStartProcessForChAngeAcPassword.IsBackground = true;
                GetStartProcessForChAngeAcPassword.Start(new object[] { item });

                Thread.Sleep(1000);
            }
        }

        public void GetChangeHeaderImages(object Param)
        {
            try
            {
                /// Incremant of Thread counter ...
                Interlocked.Increment(ref _CountThreadChangeProfileHeaderImage);



                // get value from param 
                Array _ParamValue = new object[2];
                _ParamValue = (Array)Param;

                KeyValuePair<string, TweetAccountManager> _KeyValue = (KeyValuePair<string, TweetAccountManager>)_ParamValue.GetValue(0);

                TweetAccountManager HeaderAccountManager = (TweetAccountManager)_KeyValue.Value;

                /// Get logging account 
                HeaderAccountManager.Login();

                if (!HeaderAccountManager.IsLoggedIn)
                {
                    // Account is not loggedin 
                    return;
                }
                if (!HeaderAccountManager.IsNotSuspended)
                {
                    // account is suspended.. 
                    return;
                }

                ///Add Data In Dictionary for stop processes.
                try
                {
                    dictionary_Threads.Add(Thread.CurrentThread.Name, Thread.CurrentThread);
                }
                catch { };


                ///create profile image object
                ///for calling related members ..!!
                ProfileManager.ProfileManager profilemanager = new ProfileManager.ProfileManager();
                profilemanager.logEvents.addToLogger += new EventHandler(logEvents_Profile_addToLogger);

                string imageFilePath = string.Empty;

                if (_IsChooseRandomImage)
                {
                    ///Get random image from image list box 
                    int RandomNumber = RandomNumberGenerator.GenerateRandom(0, lst_HeaderImageFile.Count);
                    imageFilePath = lst_HeaderImageFile[RandomNumber];
                }
                else
                {
                    /// Get image file path from list 
                    /// as according to account sequence .

                    int _Imageindex = 0;
                    if ((TweetAccountContainer.dictionary_TweetAccount.Count <= lst_HeaderImageFile.Count) && lst_HeaderImageFile.Count > 1)
                    {
                        _Imageindex = Array.IndexOf(TweetAccountContainer.dictionary_TweetAccount.Keys.ToArray(), HeaderAccountManager.Username);
                    }

                    imageFilePath = lst_HeaderImageFile[_Imageindex];
                }


                /// Call image change funtion from profile manager..
                bool IsSuccesFullyChanged = profilemanager.ImageUploadForProfileHeader(HeaderAccountManager.postAuthenticityToken, HeaderAccountManager.Screen_name, imageFilePath, ref HeaderAccountManager.globusHttpHelper);


                if (IsSuccesFullyChanged)
                {
                    AddToListProfile("[ " + DateTime.Now + " ] => [ Header image is changed to " + HeaderAccountManager.Username + " ]");

                    /// Write Details of account 
                    /// shen header is changed 
                    GlobusFileHelper.AppendStringToTextfileNewLine(HeaderAccountManager.Username + ":" + HeaderAccountManager.Password + ":" + imageFilePath, Globals.Path_SuccessfullyChangedHeaderImage);
                }
                else
                {
                    /// Log failed process 
                    /// and write in text file ..
                    AddToListProfile("[ " + DateTime.Now + " ] => [ Header image is not changed to " + HeaderAccountManager.Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(HeaderAccountManager.Username + ":" + HeaderAccountManager.Password + ":" + imageFilePath, Globals.Path_FailedHeaderImageChanging);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Method :- GetChangeHeaderImages >>  Message :- " + ex.Message + "  >> Error :- " + ex.StackTrace, Globals.Path_ErrorLogForHeaderImage);
            }
            finally
            {
                /// Decremant of Thread counter ...
                Interlocked.Decrement(ref _CountThreadChangeProfileHeaderImage);
                lock (lockerThreadsChangeHeaderImage)
                {
                    Monitor.Pulse(lockerThreadsChangeHeaderImage);
                }
            }
        }

        private void btn_StopChangeHeaderImage_Click(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                AddToGeneralLogs("[ " + DateTime.Now + " ] => [ Stopping ProfileManager Threads, it may take some time ]");
                StopThreads(strModule(Module.ProfileManager));
            }).Start();
        }

        #endregion



        private void btnStopRetweetAndFavorite_Click(object sender, EventArgs e)
        {
            //new Thread(() =>
            //{
            //    AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ Stopping Tweet Threads, it may take some time ]");
            //    StopThreads(strModule(Module.Retweet));
            //}).Start();

            try
            {
                _Isfevorite = true;
                List<Thread> lstTemp = lstReTweetThread.Distinct().ToList();
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

                AddToLog_RetweetAndFavorite("-------------------------------------------------------------------------------------------------------------------------------");
                AddToLog_RetweetAndFavorite("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_RetweetAndFavorite("-------------------------------------------------------------------------------------------------------------------------------");
                btn_start.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        private void grpRetweetandFavoriteSubmitAction_Enter(object sender, EventArgs e)
        {

        }

        //private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Unfollower.IsAbout_NewUi)
        //        {
        //            Unfollower.IsAbout_NewUi = false;
        //            frmLicensingDetails obj_frmLicensingDetails = new frmLicensingDetails();
        //            obj_frmLicensingDetails.Show();
        //        }
        //        else
        //        {

        //            MessageBox.Show("About Form is already Open.", "Alert");
        //        }
        //    }
        //    catch { }
        //}

        private void followYourFollowersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Unfollower.IsFollowYourFollowers_NewUi)
                {
                    Unfollower.IsFollowYourFollowers_NewUi = false;
                    frmFollowYourFollowers obj_frmFollowYourFollowers = new frmFollowYourFollowers();
                    obj_frmFollowYourFollowers.Show();
                }
                else
                {
                    MessageBox.Show("Follow Your Followers Form is already Open.", "Alert");
                }
            }
            catch { }

        }

        private void DirectMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Unfollower.IsSendDirectMessage_NewUi)
                {
                    Unfollower.IsSendDirectMessage_NewUi = false;
                    frmDirectMessage obj_frmDirectMessage = new frmDirectMessage();
                    obj_frmDirectMessage.Show();
                }
                else
                {
                    MessageBox.Show("Send Direct Message Form is already Open.", "Alert");
                }
            }
            catch { }
        }


        private void chkUsingKeyWord_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUsingKeyWord.Checked)
            {
                chk_retweetOfUrl.Visible = false;
            }
            else
            {
                chk_retweetOfUrl.Visible = true; ;
            }
        }

        private void chk_scrapeBykeywordSettingTweets_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_scrapeBykeywordSettingTweets.Checked)
            {
                txt_ScrapebySearchkeywordsettingMin.Enabled = true;
                txt_ScrapebySearchkeywordsettingMax.Enabled = true;
            }
            else
            {
                txt_ScrapebySearchkeywordsettingMin.Enabled = false;
                txt_ScrapebySearchkeywordsettingMax.Enabled = false;
            }
        }

        private void chk_FilterByFollowers_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_FilterByFollowers.Checked)
            {
                txt_FilterbyFollowersMin.Enabled = true;
                txt_FilterbyFollowersMax.Enabled = true;
            }
            else
            {
                txt_FilterbyFollowersMin.Enabled = false;
                txt_FilterbyFollowersMax.Enabled = false;
            }
        }

        private void chk_FilterByFollowing_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_FilterByFollowing.Checked)
            {
                txt_FilterbyFollowingMax.Enabled = true;
                txt_FilterbyFollowingMin.Enabled = true;
            }
            else
            {
                txt_FilterbyFollowingMax.Enabled = false;
                txt_FilterbyFollowingMin.Enabled = false;
            }
        }

        private void ScrapMemberUsingUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                frmScrapMemberUsingUrl objfrmScrapMemberUsingUrl = new frmScrapMemberUsingUrl();
                objfrmScrapMemberUsingUrl.Show();
            }
            catch { }
        }

        private void ScrapImageUsingUserId_Click(object sender, EventArgs e)
        {
            try
            {
                frmScrapProfileImge objfrmScrapProfileImge = new frmScrapProfileImge();
                objfrmScrapProfileImge.Show();
            }
            catch { }
        }
       
        private void Arab_Follow_Click(object sender, EventArgs e)
        {
            if (Unfollower.IsArabFollowOpen)
            {
                Unfollower.IsArabFollowOpen = false;
                App_Follow ArabFollow = new App_Follow();
                ArabFollow.Show();
                
            }
            else
            {
                MessageBox.Show("Arab_Follow Form is already Open.", "Alert");
            }
        }

        List<Thread> Lst_accntManagerThread = new List<Thread>();

        bool accntManagerThreadStart = false;

        List<Thread> Lst_accntPasswordThread = new List<Thread>();

        bool accntPasswordThreadStart = false;

        private void btn_stopAcManager_Click(object sender, EventArgs e)
        {
            try
            {
                List<Thread> Templst = new List<Thread>();
                if (accntManagerThreadStart)
                {
                    try
                    {
                        foreach (Thread Lst_accntManagerThreaditem in Lst_accntManagerThread)
                        {
                            Templst.Add(Lst_accntManagerThreaditem);
                        }
                        accntManagerThreadStart = false;
                    }
                    catch (Exception)
                    {
                    }
                }

                //Get abort all Start Threads...
                foreach (Thread TempList_item in Templst)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                btn_stopAcManager.Enabled = false;
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_AcManager("------------------------------------------------------------------------------------------------------------------------------------------");
            }

        }

        private void btn_StopPassChange_Click(object sender, EventArgs e)
        {
            try
            {
                List<Thread> Templst = new List<Thread>();
                if (accntPasswordThreadStart)
                {
                    try
                    {
                        foreach (Thread Lst_accntPasswordThreaditem in Lst_accntPasswordThread)
                        {
                            Templst.Add(Lst_accntPasswordThreaditem);
                        }
                        accntPasswordThreadStart = false;
                    }
                    catch (Exception)
                    {
                    }
                }


                //Get abort all Start Threads...
                foreach (Thread TempList_item in Templst)
                {
                    TempList_item.Abort();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                btn_StopPassChange.Enabled = false;
                AddToLog_AcManager("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_AcManager("------------------------------------------------------------------------------------------------------------------------------------------");
            }

        }

        private void btn_WhiteAndBlacklistedUserStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (thread_StartFilter != null)
                {
                    thread_StartFilter.Abort();
                    AddToLog_FilterUser("[ " + DateTime.Now + " ] => [ Filter Process is stopped ]");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

      


        private void chkAllTweetsPerAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllTweetsPerAccount.Checked)
            {
                txtNoOfTweets.Enabled = false;
            }
            else
            {
                txtNoOfTweets.Enabled = true;
            }
        }

        private void chk_retweetbyUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_retweetbyUser.Checked)
            {
                chkCheckDatabaseInEvery2Minutes.Enabled = true;
                chkAutoFavorite.Enabled = true;
            }
           else
            {
                chkCheckDatabaseInEvery2Minutes.Enabled = false;
                chkAutoFavorite.Enabled = false;
            }

        }
        
        private void chk_TweetWithImage_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_TweetWithImage.Checked == true)
            {
                txt_TweetImageFilePath.Enabled = true;
                btn_BrowseTweetImage.Enabled = true;
                TweetAccountManager.IsTweetWithImage = true;
            }
            else
            {
                txt_TweetImageFilePath.Enabled = false;
                btn_BrowseTweetImage.Enabled = false;
                TweetAccountManager.IsTweetWithImage = false;
            }
        }

        private void btn_BrowseTweetImage_Click(object sender, EventArgs e)
        {
            txt_TweetImageFilePath.Text = "";
            using (System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog())
            {
                //ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.SelectedPath = Application.StartupPath + "\\Profile";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_TweetImageFilePath.Text = ofd.SelectedPath;
                    TweetAccountManager.ImageFolderPath = ofd.SelectedPath;
                }
            }
        }

        private void btnAssignPrivateIPStop_Click(object sender, EventArgs e)
        {

            try
            {
                IsStopPrivateIP = true;
                List<Thread> lstTemp = LstAssignPrivateIP.Distinct().ToList();
                foreach (Thread item in lstTemp)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error >>> " + ex.StackTrace);
                    }
                }


                AddToIPsLogs("[ " + DateTime.Now + " ] => [IP testing is Aborted]");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }


        }

        private void btnBrowseUnfollowers_Click(object sender, EventArgs e)
        {
            try
            {
               // if (rdo_UnfollowbyUserId.Checked == true)
                {
                    using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                    {
                        lst_unfolloweruserlist.Clear();
                        ofd.Filter = "Text Files (*.txt)|*.txt";
                        ofd.InitialDirectory = Application.StartupPath;
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            txt_FilePathunfollowUserslist.Text = ofd.FileName;
                            lst_unfolloweruserlist = GlobusFileHelper.ReadLargeFile(ofd.FileName);
                        }
                        AddToLog_Unfollow("[ " + DateTime.Now + " ] => [ " + lst_unfolloweruserlist.Count + " Unfollowers Uploaded ]");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnScrapUsersFromTweetID_Click(object sender, EventArgs e)
        {
            try
            {
                frmScrapeUsersFromTweets frmScrapUsers = new frmScrapeUsersFromTweets();
                frmScrapUsers.Show();
            }
            catch (Exception)
            {
            }
        }

        private void newFeaturesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ApplicationFollow_Click(object sender, EventArgs e)
        {
            try
            {
                frmApplicationFollow frmfrmApplicationFollow = new frmApplicationFollow();
                frmfrmApplicationFollow.Show();
            }
            catch (Exception)
            {
            }
        }

        private void chkUnfollowDateFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnfollowDateFilter.Checked)
            {
                txtUnfollowFilterDays.Enabled = true;
            }
            else
            {
                txtUnfollowFilterDays.Enabled = false;
            }

        }

        private void advanceSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                AdvancedSearch frmAdvancesearch = new AdvancedSearch();
                frmAdvancesearch.Show();
            }
            catch (Exception)
            {
            }
        }

        private void chkReplyPerTweet_CheckedChanged(object sender, EventArgs e)
        {
            TweetAccountManager.IsReplyPerTweet = true;
        }

        private void chkUniqueReply_CheckedChanged(object sender, EventArgs e)
        {
            TweetAccountManager.IsreplyUniqueTweet = true;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        private void websiteLikerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebsitesLiker frmWebSiteLiker = new WebsitesLiker();
                frmWebSiteLiker.Show();
            }
            catch { }
        }

        private void picBoxBanner_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://pvadomination.com/");
            }
            catch { }
        }

        private void chkUserFollowonxxxDays_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkUserFollowonxxxDays.Checked)
                {
                    TweetAccountManager.IsTweetedInXdays = true;
                }
                else
                {
                    TweetAccountManager.IsTweetedInXdays = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void lstLogger_Follower_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Globals.IsCopyLoggerData)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    if (lstLogger_Follower.SelectedItem != null)
                    {
                        foreach (object row in lstLogger_Follower.SelectedItems)
                        {
                            sb.Append(row.ToString());
                            sb.AppendLine();
                        }
                    }
                    if (!(sb.Length == 0))
                    {
                        sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    }
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            }
        }

        private void lstLogger_Tweet_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Globals.IsCopyLoggerData)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    if (lstLogger_Tweet.SelectedItem != null)
                    {
                        foreach (object row in lstLogger_Tweet.SelectedItems)
                        {
                            sb.Append(row.ToString());
                            sb.AppendLine();
                        }
                    }
                    if (!(sb.Length == 0))
                    {
                        sb.Remove(sb.Length - 1, 1); // Just to avoid copying last empty row
                    }
                    Clipboard.SetData(System.Windows.Forms.DataFormats.Text, sb.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } 
            }
        }

        private void chkCheckDatabaseInEvery2Minutes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckDatabaseInEvery2Minutes.Checked)
            {
                txtTweetKeyword.ReadOnly = false;
                btnUploadRetweetKeyword.Visible = false;
                chkRetweetDivideTweets.Enabled = false;
            }
            else
            {
                txtTweetKeyword.ReadOnly = true;
                btnUploadRetweetKeyword.Visible = true;
                chkRetweetDivideTweets.Enabled = true;
            }

        }

        private void splitContainerTweet_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public static bool CheckBoxScrapTweetsIsChecked = false;
        public static List<string> ScrapTweetsForTweetModule = new List<string>();
        private void CheckBoxScrapTweets_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxScrapTweets.Checked == true)
            {
                CheckBoxScrapTweetsIsChecked = true;
                Tb_AccountManager.SelectedIndex = 7;
                btnUploadTweetMessage.Enabled = false;
                txtTweetMessageFile.Text = "";
            }
            else
            {
                CheckBoxScrapTweetsIsChecked = false;
                btnUploadTweetMessage.Enabled = true;
            }
        }


        
        

        

        

       
    }
}
