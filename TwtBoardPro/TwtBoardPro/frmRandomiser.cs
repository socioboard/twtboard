using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft;
using BaseLib;
using System.Threading;
using Randomiser;
using System.IO;
using System.Drawing.Drawing2D;

namespace twtboardpro
{
    public partial class frmRandomiser : Form
    {
        clsDB_Randomiser obj_clsDB_Randomiser = new clsDB_Randomiser();
        clsDBQueryManager objclsSettingDB = new clsDBQueryManager();

        int numberOfThreads=10;
        int numberOfPostingQuotes = 5;
        int numberOfPostingReTweetes = 5;
        int numberOfPostingNormalTweets = 5;
        int numberOfPostingFakeReplies = 5;
        int afternumberOfPosting = 5;
        int minDelay = 5;
        int maxDelay = 10;
        int totalAccount = 0;

        bool useMention = false;

        List<string> lstQuotes = new List<string>();
        List<string> lstNormalTweets = new List<string>();
        List<string> lstFakeReplies = new List<string>();
        List<string> lstReTweet = new List<string>();
        List<string> lstFakeScreenName = new List<string>();
        List<string> lstMentions = new List<string>();
        List<string> lstFollow = new List<string>();

        public static Boolean stopRandomiser = false;
        List<Thread> lstTdRandomiserThread = new List<Thread>();

        private static bool IsCloseCalledForRandomiser = false;
        private System.Drawing.Image image;

        public frmRandomiser()
        {
            InitializeComponent();
            image = Properties.Resources.app_bg;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {

                IsCloseCalledForRandomiser = false;
                Randomiser.Randomiser.IsRandomiserStop = false;
                lstTdRandomiserThread.Clear();

                if ((lstQuotes.Count > 0) && (!string.IsNullOrEmpty(txtQuotes.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "Quotes", StringEncoderDecoder.Encode(txtQuotes.Text));
                    }
                    catch
                    {
                    }
                }

                if ((lstNormalTweets.Count > 0) && (!string.IsNullOrEmpty(txtNormalTweets.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "NormalTweets", StringEncoderDecoder.Encode(txtNormalTweets.Text));
                    }
                    catch
                    {
                    }
                }

                if ((lstReTweet.Count > 0) && (!string.IsNullOrEmpty(txtReTweets.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "Retweets", StringEncoderDecoder.Encode(txtReTweets.Text));
                    }
                    catch
                    {
                    }
                }

                if ((lstFakeScreenName.Count > 0) && (!string.IsNullOrEmpty(txtFakeScreenName.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "FakeScreenName", StringEncoderDecoder.Encode(txtFakeScreenName.Text));
                    }
                    catch
                    {
                    }
                }

                if ((lstFakeReplies.Count > 0) && (!string.IsNullOrEmpty(txtFakeReplies.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "FakeReplies", StringEncoderDecoder.Encode(txtFakeReplies.Text));
                    }
                    catch
                    {
                    }
                }

                if ((lstMentions.Count > 0) && (!string.IsNullOrEmpty(txtLoadMentions.Text)))
                {
                    try
                    {
                        objclsSettingDB.InsertOrUpdateSetting("Randomiser", "Mentions", StringEncoderDecoder.Encode(txtLoadMentions.Text));
                    }
                    catch
                    {
                    }
                }

                if (chkUseMention.Checked)
                {
                    try
                    {
                        useMention = true;
                    }
                    catch
                    {
                    }
                }

                if (!string.IsNullOrEmpty(txtNoOfPostingQuotes.Text))
                {
                    try
                    {
                        numberOfPostingQuotes = Convert.ToInt32(txtNoOfPostingQuotes.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In NoOfPostingQuotes Box ! ]");
                        txtNoOfPostingQuotes.Text = (5).ToString();
                    }
                    
                }
                else
                {
                    numberOfPostingQuotes = 5;
                }

                if (!string.IsNullOrEmpty(txtNoOfPostingNormalTweets.Text))
                {
                    try
                    {
                        numberOfPostingNormalTweets = Convert.ToInt32(txtNoOfPostingNormalTweets.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In NoOfPostingNormalTweets Box ! ]");
                        txtNoOfPostingNormalTweets.Text = (5).ToString();
                    }
                    
                }
                else
                {
                    numberOfPostingNormalTweets = 5;
                }

                if (!string.IsNullOrEmpty(txtNoOfPostingReTweets.Text))
                {
                    try
                    {
                        numberOfPostingReTweetes = Convert.ToInt32(txtNoOfPostingReTweets.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In NoOfPostingReTweets Box ! ]");
                        txtNoOfPostingReTweets.Text = (5).ToString();
                    }
                    
                }
                else
                {
                    numberOfPostingReTweetes = 5;
                }

                if (!string.IsNullOrEmpty(txtNoOfPostingFakeReplies.Text))
                {
                    try
                    {
                        numberOfPostingFakeReplies = Convert.ToInt32(txtNoOfPostingFakeReplies.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In NoOfPostingFakeReplies Box ! ]");
                        txtNoOfPostingFakeReplies.Text = (5).ToString();
                    }
                    
                }
                else
                {
                    numberOfPostingFakeReplies = 5;
                }

                if (!string.IsNullOrEmpty(txtAfterNoOfPosting.Text))
                {
                    try
                    {
                        afternumberOfPosting = Convert.ToInt32(txtAfterNoOfPosting.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In AfterNoOfPosting Box ! ]");
                        txtAfterNoOfPosting.Text = (5).ToString();
                    }

                }
                else
                {
                    afternumberOfPosting = 5;
                }

                if (!string.IsNullOrEmpty(txtMinDelay.Text))
                {
                    try
                    {
                        minDelay = Convert.ToInt32(txtMinDelay.Text)*60*1000;
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In MinDelay Box ! ]");
                        txtMinDelay.Text = (5).ToString();
                    }
                    
                }
                else
                {
                    minDelay = 5*60*1000;
                }

                if (!string.IsNullOrEmpty(txtMaxDelay.Text))
                {
                    try
                    {
                        maxDelay = Convert.ToInt32(txtMaxDelay.Text)*60*1000;
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In MaxDelay Box ! ]");
                        txtMaxDelay.Text = (10).ToString();
                    }
                }
                else
                {
                    maxDelay = 10*60*1000;
                }

                if (!string.IsNullOrEmpty(txtNumberOfThreads.Text))
                {
                    try
                    {
                        numberOfThreads = Convert.ToInt32(txtNumberOfThreads.Text);
                    }
                    catch
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Enter Nueric value In NumberOfThreads Box ! ]");
                        txtNumberOfThreads.Text = (10).ToString();
                    }
                }
                else
                {
                    numberOfThreads = 10;
                }

                if (!string.IsNullOrEmpty(txtUploadFollowUsername.Text) && lstFollow.Count > 0)
                {
                    objclsSettingDB.InsertOrUpdateSetting("Randomiser", "Follow", StringEncoderDecoder.Encode(txtUploadFollowUsername.Text));
                }

                Thread obj_Thread = new Thread(Start_Randomiser);
                obj_Thread.Start();
            }
            catch
            {
            }
        }

        private void Start_Randomiser()
        {
            try
            {
                //numberOfThreads = 7;
                //int numberOfUnfollows = 5;
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    //totalAccount = TweetAccountContainer.dictionary_TweetAccount.Keys.Count;
                    Randomiser.Randomiser.ToatlAccount = TweetAccountContainer.dictionary_TweetAccount.Keys.Count;

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {

                        try
                        {                           
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(Start_RandomiserMultithreaded), new object[] { item, "" });

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

        private void Start_RandomiserMultithreaded(object parameters)
        {
            try
            {
                if (!IsCloseCalledForRandomiser)
                {
                    try
                    {
                        Thread.CurrentThread.IsBackground = true;
                    }
                    catch
                    {
                    }
                    Array paramsArray = new object[2];

                    paramsArray = (Array)parameters;

                    KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


                    List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

                    TweetAccountManager tweetAccountManager = keyValue.Value;

                    Randomiser.Randomiser obj_Randomiser = new Randomiser.Randomiser(tweetAccountManager.Username, tweetAccountManager.userID, tweetAccountManager.Password, tweetAccountManager.Screen_name, tweetAccountManager.proxyAddress, tweetAccountManager.proxyPort, tweetAccountManager.proxyUsername, tweetAccountManager.proxyPassword);

                    if (lstQuotes.Count > 0)
                    {
                        obj_Randomiser.Quotes = lstQuotes;
                    }
                    else
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The Quotes ! ]");
                        return;
                    }

                    if (lstReTweet.Count > 0)
                    {
                        obj_Randomiser.ReTweets = lstReTweet;
                    }
                    else
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The ReTweets ! ]");
                        return;
                    }

                    if (lstNormalTweets.Count > 0)
                    {
                        obj_Randomiser.NormalTweets = lstNormalTweets;
                    }
                    else
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The Normal Tweets ! ]");
                        return;
                    }

                    if (lstFakeReplies.Count > 0)
                    {
                        obj_Randomiser.FakeReplies = lstFakeReplies;
                    }
                    else
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The Fake Replies ! ]");
                        return;
                    }

                    if (lstFakeReplies.Count > 0)
                    {
                        obj_Randomiser.FakeScreenName = lstFakeScreenName;
                    }
                    else
                    {
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The Fake Screen Name ! ]");
                        return;
                    }

                    if (chkUseMention.Checked)
                    {
                        if (lstMentions.Count > 0)
                        {
                            obj_Randomiser.Mentions = lstMentions;

                            obj_Randomiser.useMention = chkUseMention.Checked;
                        }
                        else
                        {
                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload The Mentions ! ]");
                            return;
                        }
                    }


                    if (chkboxUseFollow.Checked)
                    {
                        if (lstFollow.Count > 0)
                        {
                            obj_Randomiser.Follow = lstFollow;

                            obj_Randomiser.useFollow = chkboxUseFollow.Checked;
                        }
                        else
                        {
                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Upload Follow Username ]");
                            return;
                        }


                        #region UseFollowSetting
                        if (chkUseFollowSetting.Checked)
                        {
                            try
                            {
                                // Only one object can be created of  clsUseFollowSetting
                                clsUseFollowSetting obj_clsUseFollowSetting = clsUseFollowSetting.GetObject();
                                obj_Randomiser._DontFollowUsersThatUnfollowedBefore = obj_clsUseFollowSetting._DontFollowUsersThatUnfollowedBefore;

                                obj_Randomiser._DontFollowUsersWithNoPicture = obj_clsUseFollowSetting._DontFollowUsersWithNoPicture;

                                #region User Without Picture
                                if (obj_Randomiser._DontFollowUsersWithNoPicture)
                                {
                                    List<string> tempdata = new List<string>();
                                    try
                                    {
                                        foreach (string newitem in lstFollow)
                                        {
                                            tempdata.Add(newitem);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_FollowerErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With No Picture --> " + ex.Message, Globals.Path_TwtErrorLogs);
                                    }
                                    lstFollow.Clear();

                                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Checking Profile Image in Extracted User id's ]");

                                    foreach (string item in tempdata)
                                    {
                                        string containsIamge = TwitterDataScrapper.GetPhotoFromUsername(item);
                                        Thread.Sleep(2000);
                                        if (containsIamge == "true")
                                        {
                                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + item + " Contains Image ]");
                                            lstFollow.Add(item);
                                        }
                                        else if (containsIamge == "false")
                                        {
                                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + item + " Not Contains Image ]");
                                            ///Add in blacklist table
                                        }
                                        else if (containsIamge == "Rate limit exceeded")
                                        {
                                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Cannot Make Request. Rate limit exceeded ]");
                                            AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Please Try After Some Time ]");
                                        }
                                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Sleep For 4 Seconds ]");
                                        Thread.Sleep(4000);
                                    }
                                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFollow.Count + " Users Contain Profile Image ]");
                                }

                                #endregion

                                obj_Randomiser._DontFollowUsersWithFollowingsFollowersRatio = obj_clsUseFollowSetting._DontFollowUsersWithFollowingsFollowersRatio;
                                obj_Randomiser._FollowingsFollowersRatio = obj_clsUseFollowSetting._FollowingsFollowersRatio;

                                obj_Randomiser._NoOFfollow = obj_clsUseFollowSetting._NoOFfollow;
                                obj_Randomiser._MaximumFollow = obj_clsUseFollowSetting._MaximumFollow;

                                obj_Randomiser._DontFollowUsersWithManyLinks = obj_clsUseFollowSetting._DontFollowUsersWithManyLinks;
                                obj_Randomiser._NoOfLinks = obj_clsUseFollowSetting._NoOfLinks;

                                #region followUserWithmanyLinks
                                if (obj_Randomiser._DontFollowUsersWithManyLinks)
                                {
                                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Checking For Links in Tweets ]");
                                    if (!string.IsNullOrEmpty(obj_Randomiser._NoOfLinks.ToString()) && NumberHelper.ValidateNumber(obj_Randomiser._NoOfLinks.ToString()))
                                    {
                                        TwitterDataScrapper.Percentage = Convert.ToInt32(obj_Randomiser._NoOfLinks.ToString());
                                    }
                                    else
                                    {
                                        TwitterDataScrapper.Percentage = 40;
                                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Setting Default Percentage : 40% ]");
                                    }
                                    List<string> tempdata = new List<string>();
                                    try
                                    {
                                        foreach (string newitem in lstFollow)
                                        {
                                            tempdata.Add(newitem);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_FollowerErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartFollowing() -- User With too Many Links --> " + ex.Message, Globals.Path_TwtErrorLogs);
                                    }
                                    lstFollow.Clear();
                                    foreach (string item in tempdata)
                                    {
                                        try
                                        {
                                            bool toomanyLinks = TwitterDataScrapper.GetStatusLinks(item);
                                            if (toomanyLinks)
                                            {
                                                lstFollow.Add(item);
                                                AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Added User id : " + item + " To Follow List ]");
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                #endregion

                                obj_Randomiser._UseGroups = obj_clsUseFollowSetting._UseGroups;
                                obj_Randomiser._UseGroup = obj_clsUseFollowSetting._UseGroup;

                                if (obj_Randomiser._UseGroups)
                                {
                                    if (tweetAccountManager.GroupName != obj_Randomiser._UseGroup)
                                    {
                                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + keyValue.Key + " Group Name Does Not Match ]");
                                        return;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        } 

                        #endregion

                        #region UseFollowBySearchKeyword
                        if (chkUseFollowBySearchKeyword.Checked)
                        {
                            try
                            {
                                clsFollowBySearchKeywords obj_clsFollowBySearchKeywords = clsFollowBySearchKeywords.GetObject();

                                obj_Randomiser._UseFollowbySearchKeyword = true;

                                obj_Randomiser._Followbysinglekeywordperaccount = obj_clsFollowBySearchKeywords._Followbysinglekeywordperaccount;

                                obj_Randomiser._NoFollowByPerAccount=obj_clsFollowBySearchKeywords._NoFollowByPerAccount;

                                if (obj_clsFollowBySearchKeywords.lstKeywords.Count > 0)
                                {
                                    obj_Randomiser.lstKeywords = obj_clsFollowBySearchKeywords.lstKeywords;
                                }
                                else
                                {
                                    MessageBox.Show("Please Upload The Follow By Search Keyword File !");
                                    return;    
                                }
                            }
                            catch
                            {
                            }
                        }
                        #endregion

                        //if (chkBox_NoOFfollowPerTime.Checked)
                        {
                            try
                            {
                                obj_Randomiser._NoOFfollowPerTime = true;
                                obj_Randomiser.noOFfollowPerTime = Convert.ToInt32(txtMaxNoOfFollowPerTime.Text);
                            }
                            catch
                            {
                                txtMaxNoOfFollowPerTime.Text = (2).ToString();
                                obj_Randomiser.noOFfollowPerTime = 2;//Convert.ToInt32(txtMaxNoOfFollowPerTime.Text);
                            }
                        }
                    }

                    

                    if (!string.IsNullOrEmpty(txtNoOfPostingQuotes.Text))
                    {
                        try
                        {
                            obj_Randomiser.QuotesPosting = numberOfPostingQuotes;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.QuotesPosting = 5;
                    }

                    if (!string.IsNullOrEmpty(txtNoOfPostingNormalTweets.Text))
                    {
                        try
                        {
                            obj_Randomiser.NormalTweetsPosting = numberOfPostingNormalTweets;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.NormalTweetsPosting = 5;
                    }

                    if (!string.IsNullOrEmpty(txtNoOfPostingReTweets.Text))
                    {
                        try
                        {
                            obj_Randomiser.ReTweetsPosting = numberOfPostingReTweetes;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.ReTweetsPosting = 5;
                    }

                    if (!string.IsNullOrEmpty(txtFakeReplies.Text))
                    {
                        try
                        {
                            obj_Randomiser.FakeRepliesPosting = numberOfPostingFakeReplies;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.FakeRepliesPosting = 5;
                    }

                    if (!string.IsNullOrEmpty(txtAfterNoOfPosting.Text))
                    {
                        try
                        {
                            obj_Randomiser.AfterNoOfPosting = afternumberOfPosting;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.AfterNoOfPosting = 5;
                    }

                    if (!string.IsNullOrEmpty(txtMinDelay.Text))
                    {
                        try
                        {
                            obj_Randomiser.MinTimeDelay = minDelay;
                        }
                        catch
                        {

                        }

                    }
                    else
                    {
                        obj_Randomiser.MinTimeDelay = 5 * 60 * 1000;
                    }

                    if (!string.IsNullOrEmpty(txtMaxDelay.Text))
                    {
                        try
                        {
                            obj_Randomiser.MaxTimeDelay = maxDelay;
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        obj_Randomiser.MaxTimeDelay = 10 * 60 * 1000;
                    }

                    //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                    tweetAccountManager.logEvents.addToLogger += new EventHandler(Randomiser_AddToLogger);

                    if (!tweetAccountManager.IsLoggedIn)
                    {
                        tweetAccountManager.Login();
                    }

                    if (tweetAccountManager.AccountStatus == "Account Suspended")
                    {
                        clsDBQueryManager database = new clsDBQueryManager();
                        database.UpdateSuspendedAcc(tweetAccountManager.Username);

                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Account Suspended With User Name : " + tweetAccountManager.Username + " ]");
                        return;
                    }

                    tweetAccountManager.LoginRandomiser(ref obj_Randomiser);

                    tweetAccountManager.logEvents.addToLogger -= Randomiser_AddToLogger;
                }
            }
            catch
            {
            }
        }

        private void btnQuotes_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtQuotes.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]
                        

                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstQuotes.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstQuotes.Count + " Quotes Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }
                
            }
            catch
            {
            }
        }

        private void Randomiser_AddToLogger(object sender, EventArgs e)
        {
            try
            {
                if (e is EventsArgs)
                {
                    EventsArgs eventArgs = e as EventsArgs;
                    AddToRandomiserLog(eventArgs.log);
                }
            }
            catch
            {
            }
        }

        private void AddToRandomiserLog(string log)
        {

            try
            {
                if (lstbRandomiserLogger.InvokeRequired)
                {
                    lstbRandomiserLogger.Invoke(new MethodInvoker(delegate
                    {
                        lstbRandomiserLogger.Items.Add(log);
                       // lstbRandomiserLogger.SelectedIndex = lstbRandomiserLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    lstbRandomiserLogger.Items.Add(log);
                    //lstbRandomiserLogger.SelectedIndex = lstbRandomiserLogger.Items.Count - 1;
                }
            }
            catch { }

        }

        private void btnNormalTweets_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtNormalTweets.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]


                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstNormalTweets.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstNormalTweets.Count + " Normal Tweets Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }

            }
            catch
            {
            }
        }

        private void btnLoadRetweets_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtReTweets.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]


                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstReTweet.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstReTweet.Count + " ScreenName Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }

            }
            catch
            {
            }
        }

        private void btnLoadFakeReplies_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtFakeReplies.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]


                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstFakeReplies.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFakeReplies.Count + " Fake Replies Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }

            }
            catch
            {
            }
        }

        private void frmRandomiser_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDefaultsFiles();
                Randomiser.Randomiser.logEvents.addToLogger += new EventHandler(Randomiser_AddToLogger);
            }
            catch
            {
            }
        }

        private void LoadDefaultsFiles()
        {
            try
            {
                #region TD Ubdate by me

                lstQuotes.Clear();
                lstNormalTweets.Clear();
                lstFakeReplies.Clear();
                lstReTweet.Clear();
                lstFakeScreenName.Clear();
                lstMentions.Clear();
                lstFollow.Clear();

                string QuotesSettings = string.Empty;
                string NormalTweetsSettings = string.Empty;
                string FakeRepliesSettings = string.Empty;
                string ReTweetSettings = string.Empty;
                string FakeScreenNameSettings = string.Empty;
                string MentionsSettings = string.Empty;
                string FollowSettings = string.Empty;

                #endregion

                //Globussoft1.GlobusHttpHelper
                DataTable dt = objclsSettingDB.SelectSettingData();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        if ("Randomiser" == row[0].ToString())
                        {
                            if ("Quotes" == row[1].ToString())
                            {
                                QuotesSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("NormalTweets" == row[1].ToString())
                            {
                                NormalTweetsSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("FakeReplies" == row[1].ToString())
                            {
                                FakeRepliesSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("Retweets" == row[1].ToString())
                            {
                                ReTweetSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("FakeScreenName" == row[1].ToString())
                            {
                                FakeScreenNameSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("Mentions" == row[1].ToString())
                            {
                                MentionsSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }

                            if ("Follow" == row[1].ToString())
                            {
                                FollowSettings = StringEncoderDecoder.Decode(row[2].ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(QuotesSettings))
                {
                    lstQuotes = GlobusFileHelper.ReadFiletoStringList(QuotesSettings);
                    txtQuotes.Text = QuotesSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstQuotes.Count + " Quotes loaded ]");
                }

                if (File.Exists(NormalTweetsSettings))
                {
                    lstNormalTweets = GlobusFileHelper.ReadFiletoStringList(NormalTweetsSettings);
                    txtNormalTweets.Text = NormalTweetsSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstNormalTweets.Count + " NormalTweets loaded ]");
                }

                if (File.Exists(FakeRepliesSettings))
                {
                    lstFakeReplies = GlobusFileHelper.ReadFiletoStringList(FakeRepliesSettings);
                    txtFakeReplies.Text = FakeRepliesSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFakeReplies.Count + "FakeReplies loaded ]");
                }

                if (File.Exists(ReTweetSettings))
                {
                    lstReTweet = GlobusFileHelper.ReadFiletoStringList(ReTweetSettings);
                    txtReTweets.Text = ReTweetSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstReTweet.Count + "ReTweets loaded ]");
                }

                if (File.Exists(FakeScreenNameSettings))
                {
                    lstFakeScreenName = GlobusFileHelper.ReadFiletoStringList(FakeScreenNameSettings);
                    txtFakeScreenName.Text = FakeScreenNameSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFakeScreenName.Count + "FakeScreenName loaded ]");
                }

                if (File.Exists(MentionsSettings))
                {
                    lstMentions = GlobusFileHelper.ReadFiletoStringList(MentionsSettings);
                    txtLoadMentions.Text = MentionsSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstMentions.Count + "Mentions loaded ]");
                }

                if (File.Exists(FollowSettings))
                {
                    lstFollow = GlobusFileHelper.ReadFiletoStringList(FollowSettings);
                    txtUploadFollowUsername.Text = FollowSettings;

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFollow.Count + " Follow Username loaded ]");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do You realy want to delete all data from database?", "Important", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    obj_clsDB_Randomiser.DeleteAllfromtb_Randomiser();

                    obj_clsDB_Randomiser.DeleteAllfromtb_RandomiserMention();

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ All Record Delated From Database Successfully ! ]");
                }

            }
            catch
            {
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                frmRandomiser.stopRandomiser = true;
                Randomiser.Randomiser.IsRandomiserStop = true;

                try
                {
                    IsCloseCalledForRandomiser = true;
                    List<Thread> lstTemp = new List<Thread>();
                    foreach (Thread item in lstTdRandomiserThread)
                    {
                        lstTemp.Add(item);
                    }
                    foreach (Thread item in lstTemp)
                    {
                        item.Abort();
                        lstTdRandomiserThread.Remove(item);
                    }
                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                    AddToRandomiserLog("------------------------------------------------------------------------------------------------------------------------------------------");
                }
                catch
                {
                }
            }
            catch
            {
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> temp = new List<string>();
                if (lstbRandomiserLogger.SelectedItems.Count > 0)
                {
                    foreach (var item in lstbRandomiserLogger.SelectedItems)
                    {
                        //Clipboard.SetData(DataFormats.StringFormat, item.ToString());
                        temp.Add(item.ToString());
                    }

                    string data = string.Join(Environment.NewLine, temp.ToArray());
                    Clipboard.SetData(DataFormats.StringFormat, data);
                }
            }
            catch
            {
            }
        }

        private void frmRandomiser_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                frmRandomiser.stopRandomiser = true;
                Randomiser.Randomiser.IsRandomiserStop = true;

                try
                {
                    IsCloseCalledForRandomiser = true;
                    List<Thread> lstTemp = new List<Thread>();
                    foreach (Thread item in lstTdRandomiserThread)
                    {
                        lstTemp.Add(item);
                    }
                    foreach (Thread item in lstTemp)
                    {
                        item.Abort();
                        lstTdRandomiserThread.Remove(item);
                    }
                   // AddToRandomiser("Process Stopped..............!");
                    lstbRandomiserLogger.Items.Clear();
                }
                catch
                {
                }
            }
            catch
            {
            }
        }

        private void btnLoadFakeScreenName_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtFakeScreenName.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]


                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstFakeScreenName.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFakeScreenName.Count + " Fake Screen Name Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }

            }
            catch
            {
            }
        }

        private void btnLoadMentions_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtLoadMentions.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        ///Modified [1.0.0.3]


                        foreach (string item in templist)
                        {
                            //if (!lstMessage.Contains(item))
                            {
                                lstMentions.Add(item);
                            }
                        }

                        //Console.WriteLine(lstGrpKeywords.Count + " Group Keywords loaded");
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstMentions.Count + " Mentions Loaded ! ]");
                        //AddToGrpPages(lstGrpKeywords.Count + " Group Keywords loaded");
                    }

                }

            }
            catch
            {
            }
        }

        private void btnUploadFollowUsername_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtUploadFollowUsername.Text = ofd.FileName;
                        List<string> templist = GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        templist = templist.Distinct().ToList();
                        
                        foreach (string item in templist)
                        {
                            lstFollow.Add(item);
                        }
                        lstFollow = lstFollow.Distinct().ToList();
                        AddToRandomiserLog("[ " + DateTime.Now + " ] => [ " + lstFollow.Count + " Follow Username Loaded ! ]");
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void btnGetMentions_Click(object sender, EventArgs e)
        {
            try
            {
                IsCloseCalledForRandomiser = false;
                Randomiser.Randomiser.IsRandomiserStop = false;
                lstTdRandomiserThread.Clear();

                Thread obj_Thread = new Thread(Start_GetMentions);
                obj_Thread.Start();
            }
            catch
            {
            }
        }

        private void Start_GetMentions()
        {
            try
            {
                //numberOfThreads = 7;
                //int numberOfUnfollows = 5;
                if (TweetAccountContainer.dictionary_TweetAccount.Count > 0)
                {
                    //totalAccount = TweetAccountContainer.dictionary_TweetAccount.Keys.Count;
                    Randomiser.Randomiser.ToatlAccount = TweetAccountContainer.dictionary_TweetAccount.Keys.Count;

                    ThreadPool.SetMaxThreads(numberOfThreads, 5);

                    foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                    {
                        try
                        {                            
                            ThreadPool.SetMaxThreads(numberOfThreads, 5);

                            ThreadPool.QueueUserWorkItem(new WaitCallback(Start_GetMentionsMultithreaded), new object[] { item, "" });

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

        private void Start_GetMentionsMultithreaded(object parameters)
        {
            try
            {
                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);


                List<string> list_userIDsToFollow = new List<string>();//(List<string>)paramsArray.GetValue(1);

                TweetAccountManager tweetAccountManager = keyValue.Value;

                //tweetAccountManager.unFollower.logEvents.addToLogger += new EventHandler(logEvents_UnFollower_addToLogger);
                tweetAccountManager.logEvents.addToLogger += new EventHandler(Randomiser_AddToLogger);

                if (!tweetAccountManager.IsLoggedIn)
                {
                    tweetAccountManager.Login();
                }

                if (tweetAccountManager.AccountStatus == "Account Suspended")
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(tweetAccountManager.Username);

                    AddToRandomiserLog("[ " + DateTime.Now + " ] => [ Account Suspended With User Name : " + tweetAccountManager.Username + " ]");
                    return;
                }

                tweetAccountManager.GetMentions();

                tweetAccountManager.logEvents.addToLogger -= Randomiser_AddToLogger;
            }
            catch
            {
            }
        }

      
        private void chkUseFollowSetting_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                frmUseFollowSetting obj_frmUseFollowSetting = new frmUseFollowSetting();
                obj_frmUseFollowSetting.Show();
            }
            catch
            {
            }
        }

        private void chkUseFollowBySearchKeyword_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                frmUseFollowBySearchKeyword obj_frmUseFollowBySearchKeyword = new frmUseFollowBySearchKeyword();
                obj_frmUseFollowBySearchKeyword.Show();
            }
            catch
            {
            }
        }

        private void frmRandomiser_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the background.
            //g.FillRectangle(Brushes.Yellow, new Rectangle(new Point(0, 0), this.ClientSize));

            //// Draw the image.
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

              
    }
}
