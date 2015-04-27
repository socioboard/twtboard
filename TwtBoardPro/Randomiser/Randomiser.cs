using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Threading;
using System.Data;
using System.Text.RegularExpressions;
using Follower;
using Globussoft;


namespace Randomiser
{
    public class Randomiser
    {
         public static BaseLib.Events logEvents = new BaseLib.Events();

         RandomiserManager obj_RandomiserManager = new RandomiserManager();
         clsDB_Randomiser obj_clsDB_Randomiser = new clsDB_Randomiser();
         Follower.Follower Obj_Follow = new Follower.Follower();
        
        #region Global Variable

        string UserName = string.Empty;
        string UserId = string.Empty;
        string Password = string.Empty;
        string Screen_Name = string.Empty;
        string ProxyAddress = string.Empty;
        string ProxyPort = string.Empty;
        string ProxyUserName = string.Empty;
        string ProxyPassword = string.Empty;

        int count = 1;
        int MentionsCount = 0;
        bool isMentionsStop = false;

        public static int ToatlAccount = 0;
        public static int TotalPostingCount = 1;

        private Queue<string> qQuotes = new Queue<string>();
        private Queue<string> qNormalTweets = new Queue<string>();
        private Queue<string> qReTweets = new Queue<string>();
        private Queue<string> qFakeReplies = new Queue<string>();
        private Queue<string> qMentions = new Queue<string>();
        private Queue<string> qFakeScreenName = new Queue<string>();
        private Queue<string> qFakeFollower = new Queue<string>();
        List<string> AlreadyFollowing = new List<string>();

        List<string> lstProcess = new List<string>();
        public List<RandomiserTwitterDataScrapper.StructTweetIDs> static_lst_Struct_TweetData { get; set; }

        public static bool IsRandomiserStop=false;


        #region Global Instance Variable For Follower Setting
        public bool _DontFollowUsersThatUnfollowedBefore = false;
        public bool _DontFollowUsersWithNoPicture = false;
        public bool _DontFollowUsersWithManyLinks = false;
        public bool _DontFollowUsersWithFollowingsFollowersRatio = false;
        public bool _NoOFfollow = false;
        public bool _UseGroups = false;
        public int _NoOfLinks = 40;
        public int _FollowingsFollowersRatio = 80;
        public int _MaximumFollow = 10;
        public string _UseGroup = string.Empty;
        public bool _NoOFfollowPerTime = false;
        public int noOFfollowPerTime = 2;

        
        #endregion

        #region Global Variable for Follow By Search Keyword
        public bool _UseFollowbySearchKeyword = false;
        public bool _Followbysinglekeywordperaccount = false;
        public int _NoFollowByPerAccount = 5;
        public List<string> lstKeywords = new List<string>();

        private Queue<string> qKeyword = new Queue<string>();
        #endregion

        #endregion

        public bool useMention
        {
            get;
            set;
        }

        public bool useFollow
        {
            get;
            set;
        }

        public int MinTimeDelay
        {
            get;
            set;
        }

        public int MaxTimeDelay
        {
            get;
            set;
        }

        public int QuotesPosting
        {
            get;
            set;
        }

        public int NormalTweetsPosting
        {
            get;
            set;
        }

        public int ReTweetsPosting
        {
            get;
            set;
        }

        public int FakeRepliesPosting
        {
            get;
            set;
        }

        public int AfterNoOfPosting
        {
            get;
            set;
        }

        public List<string> Quotes
        {
            get;
            set;
        }

        public List<string> NormalTweets
        {
            get;
            set;
        }

        public List<string> ReTweets
        {
            get;
            set;
        }

        public List<string> FakeReplies
        {
            get;
            set;
        }

        public List<string> FakeScreenName
        {
            get;
            set;
        }

        public List<string> Mentions
        {
            get;
            set;
        }

        public List<string> Follow
        {
            get;
            set;
        }

        public Randomiser()
        {
        }

        public Randomiser(string Username, string userId, string Password, string Screen_name, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            this.UserName = Username;
            this.UserId = userId;
            this.Password = Password;
            this.Screen_Name = Screen_name;
            this.ProxyAddress = proxyAddress;
            this.ProxyPort = proxyPort;
            this.ProxyUserName = proxyUsername;
            this.ProxyPassword = proxyPassword;
        }

        public void StartRandomiser(ref Globussoft.GlobusHttpHelper globusHttpHelper, ref string userId, ref string userName, ref string Screen_name, ref string postAuthenticityToken)
        {
            try
            {
                foreach (string FollowerUsername in Follow)
                {
                    qFakeFollower.Enqueue(FollowerUsername);
                }

                foreach (string item in lstKeywords)
                {
                    try
                    {
                        qKeyword.Enqueue(item);
                    }
                    catch
                    {
                    }
                }

                string Status = string.Empty;
                RandomiserTwitterDataScrapper DataScrape = new RandomiserTwitterDataScrapper();
                AlreadyFollowing = DataScrape.GetFollowings(Screen_name, out Status);

                int quotesCount = 1;
                int NormalTweetsCount = 1;
                int ReTweetsCount = 1;
                int FakeRepliesCount = 1;
                //int MentionsCount = 0;

                bool isQuotesCompleted = false;
                bool isNormalTweetsCompleted = false;
                bool isReTweetsCompleted = false;
                bool isFakeRepliesCompleted = false;

                while (true)
                {
                    try
                    {
                        if (IsRandomiserStop)
                        {
                            break;
                        }

                        //if (TotalPostingCount > (ToatlAccount * QuotesPosting) + (ToatlAccount * NormalTweetsPosting) + (ToatlAccount * ReTweetsPosting) + (ToatlAccount * FakeRepliesPosting))
                        //{
                        //    break;
                        //}

                        try
                        {
                            if (lstProcess.Count < 1)
                            {
                                try
                                {
                                    lstProcess.Clear();
                                    SetProcess();
                                }
                                catch
                                {
                                }
                            }

                            string process = lstProcess[new Random().Next(0, lstProcess.Count)];

                            switch (process)
                            {
                                case "Quotes":
                                    try
                                    {

                                        //if (quotesCount <= QuotesPosting)
                                        {
                                            try
                                            {
                                                //Call SetMention()
                                                if (useMention)
                                                {
                                                    if (AfterNoOfPosting > 0)
                                                    {
                                                        if (MentionsCount == AfterNoOfPosting)
                                                        {
                                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                            MentionsCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                        MentionsCount = 0;
                                                    }
                                                }

                                                Log("[ " + DateTime.Now + " ] => [ Starting Quotes Process With User Name : " + UserName + " ]");

                                                List<string> lstQuotes = Quotes;
                                                lstQuotes = lstQuotes.Distinct().ToList();

                                                if (lstQuotes.Count > 0)
                                                {
                                                    if (qQuotes.Count < 1)
                                                    {
                                                        try
                                                        {
                                                            lstQuotes = lstQuotes.Distinct().ToList();
                                                            foreach (string item in lstQuotes)
                                                            {
                                                                qQuotes.Enqueue(item);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    string quotes = qQuotes.Dequeue(); //lstQuotes[new Random().Next(0, lstQuotes.Count)];

                                                    string status = string.Empty;

                                                    obj_RandomiserManager.Tweet(ref globusHttpHelper, "", "", quotes, out status);

                                                    if (status == "posted")
                                                    {
                                                        //Quotes.Remove(quotes);
                                                        quotesCount++;

                                                        Log("[ " + DateTime.Now + " ] => [ Quotes : " + quotes + "  Posted Successfully With User Name : " + UserName + " ]");

                                                        TotalPostingCount++;

                                                        MentionsCount++;
                                                        

                                                        /// Manage dataBase

                                                        obj_clsDB_Randomiser.QTF_InserIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode("Quotes"), StringEncoderDecoder.Encode(quotes), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                                        ///Delay
                                                        int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                                        Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");

                                                        Thread.Sleep(delay);
                                                       
                                                    }
                                                    else
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ Quotes : " + quotes + " Couldn't Post Successfully With User Name : " + UserName + " ]");
                                                    }
                                                }
                                                else
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ All Quotes Have Been Used With User Name : " + UserName + " ]");
                                                    TotalPostingCount++;
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (!isQuotesCompleted)
                                        //    {
                                        //        Log(QuotesPosting + " Quotes Posting Completed With User Name : " + UserName);

                                        //        isQuotesCompleted = true;
                                        //        //Log((1).ToString() + " minutes Delay With User Name : " + UserName);
                                        //        //Thread.Sleep(60 * 1000);
                                        //    }

                                        //}
                                    }
                                    catch
                                    {
                                    }

                                    break;

                                case "NormalTweets":
                                    try
                                    {


                                        //if (NormalTweetsCount <= NormalTweetsPosting)
                                        {
                                            try
                                            {
                                                //Call SetMention()
                                                if (useMention)
                                                {
                                                    if (AfterNoOfPosting > 0)
                                                    {
                                                        if (MentionsCount == AfterNoOfPosting)
                                                        {
                                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                            MentionsCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                        MentionsCount = 0;
                                                    }
                                                }

                                                Log("[ " + DateTime.Now + " ] => [ Starting NormalTweets Process With User Name : " + UserName + " ]");

                                                List<string> lstNormalTweets = NormalTweets;
                                                lstNormalTweets = lstNormalTweets.Distinct().ToList();

                                                if (lstNormalTweets.Count > 0)
                                                {
                                                    if (qNormalTweets.Count < 1)
                                                    {
                                                        try
                                                        {
                                                            lstNormalTweets = lstNormalTweets.Distinct().ToList();
                                                            foreach (string item in lstNormalTweets)
                                                            {
                                                                qNormalTweets.Enqueue(item);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    string normalTweets = qNormalTweets.Dequeue();//lstNormalTweets[new Random().Next(0, lstNormalTweets.Count)];

                                                    string status = string.Empty;

                                                    obj_RandomiserManager.Tweet(ref globusHttpHelper, "", "", normalTweets, out status);

                                                    if (status== "posted")
                                                    {
                                                        //NormalTweets.Remove(normalTweets);
                                                        NormalTweetsCount++;

                                                        Log("[ " + DateTime.Now + " ] => [ NormalTweets : " + normalTweets + "  Posted Successfully With User Name : " + UserName + " ]");

                                                        TotalPostingCount++;

                                                        MentionsCount++;

                                                       
                                                        /// Manage dataBase

                                                        obj_clsDB_Randomiser.QTF_InserIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode("NormalTweets"), StringEncoderDecoder.Encode(normalTweets), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                                        ///Delay
                                                        int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                                        Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");


                                                        Thread.Sleep(delay);

                                                        
                                                    }
                                                    else
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ NormalTweets : " + normalTweets + " Couldn't Post Successfully With User Name : " + UserName + " ]");
                                                    }
                                                }
                                                else
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ All NormalTweets Have Been Used With User Name : " + UserName + " ]");
                                                    TotalPostingCount++;
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (!isNormalTweetsCompleted)
                                        //    {
                                        //        Log(NormalTweetsPosting + " NormalTweets Posting Completed With User Name : " + UserName);

                                        //        isNormalTweetsCompleted = true;
                                        //        //Log((1).ToString() + " minutes Delay With User Name : " + UserName);
                                        //        //Thread.Sleep(60 * 1000);
                                        //    }
                                        //}
                                    }
                                    catch
                                    {
                                    }

                                    break;

                                case "ReTweets":
                                    try
                                    {


                                        //if (ReTweetsCount <= ReTweetsPosting)
                                        {
                                            try
                                            {
                                                //Call SetMention()
                                                if (useMention)
                                                {
                                                    if (AfterNoOfPosting > 0)
                                                    {
                                                        if (MentionsCount == AfterNoOfPosting)
                                                        {
                                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                            MentionsCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                        MentionsCount = 0;
                                                    }
                                                }

                                                Log("[ " + DateTime.Now + " ] => [ Starting ReTweets Process With User Name : " + UserName + " ]");

                                                List<string> lstReTweets = ReTweets;
                                                lstReTweets.Remove(Screen_name);
                                                lstReTweets = lstReTweets.Distinct().ToList();

                                                if (lstReTweets.Count > 0)
                                                {

                                                    if (qReTweets.Count < 1)
                                                    {
                                                        try
                                                        {
                                                            lstReTweets = lstReTweets.Distinct().ToList();
                                                            foreach (string item in lstReTweets)
                                                            {
                                                                qReTweets.Enqueue(item);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    string screenName = qReTweets.Dequeue();//lstReTweets[new Random().Next(0, lstReTweets.Count)];

                                                    RandomiserTwitterDataScrapper tweetScrapper = new RandomiserTwitterDataScrapper();
                                                    static_lst_Struct_TweetData = tweetScrapper.GetTweetData_ByUserName(screenName);

                                                    // List<string> lst_TempTweetId = new List<string>();

                                                    string tweetId = string.Empty;

                                                    string tweet = string.Empty;

                                                    string tweetStatus = string.Empty;

                                                    foreach (RandomiserTwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                                                    {
                                                        try
                                                        {
                                                            //lst_TempTweetId.Add(item.ID_Tweet);

                                                            tweetId = item.ID_Tweet;

                                                            tweet = item.wholeTweetMessage;

                                                            break;
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    if (!string.IsNullOrEmpty(tweetId))
                                                    {
                                                        try
                                                        {
                                                            obj_RandomiserManager.ReTweet(ref globusHttpHelper, "", postAuthenticityToken, tweetId, "", out tweetStatus);

                                                            if (tweetStatus == "posted")
                                                            {

                                                                //ReTweets.Remove(screenName);
                                                                ReTweetsCount++;

                                                                Log("[ " + DateTime.Now + " ] => [ ReTweets Id : " + tweetId + " Reply With User Name : " + UserName + " ]");

                                                                TotalPostingCount++;

                                                                MentionsCount++;

                                                                
                                                                /// Manage dataBase

                                                                obj_clsDB_Randomiser.ReTweet_InserIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode("ReTweets"), StringEncoderDecoder.Encode(tweetId), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                                                ///Delay
                                                                int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                                                Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");

                                                                Thread.Sleep(delay);

                                                            }
                                                            else
                                                            {
                                                                Log("[ " + DateTime.Now + " ] => [ ReTweets Id : " + tweetId + " Couldn't Reply With User Name : " + UserName + " ]");
                                                            }

                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    //if (lst_TempTweetId.Count > 0)
                                                    //{
                                                    //    //string tweetId = lst_TempTweetId[new Random().Next(0, lst_TempTweetId.Count)];
                                                    //    lst_TempTweetId.Clear();

                                                    //    //Retweet
                                                    //}
                                                    else
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ There Is No Tweet Id With User name : " + UserName + " And Screen Name : " + screenName + " ]");
                                                    }

                                                }
                                                else
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ There Is No Screen Name In ReTweets File.Please Make Sure ReTweets File Don't Contains LoginId Screen Name ! ]");
                                                    TotalPostingCount++;
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (!isReTweetsCompleted)
                                        //    {
                                        //        Log(ReTweetsPosting + " ReTweets Posting Completed With User Name : " + UserName);

                                        //        isReTweetsCompleted=true;
                                        //        //Log((1).ToString() + " minutes Delay With User Name : " + UserName);
                                        //        //Thread.Sleep(60 * 1000);
                                        //    }
                                        //}

                                    }
                                    catch
                                    {
                                    }

                                    break;

                                case "FakeReplies":
                                    try
                                    {
                                        //if (FakeRepliesCount <= FakeRepliesPosting)
                                        {
                                            try
                                            {
                                                //Call SetMention()
                                                if (useMention)
                                                {
                                                    if (AfterNoOfPosting > 0)
                                                    {
                                                        if (MentionsCount == AfterNoOfPosting)
                                                        {
                                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                            MentionsCount = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                        MentionsCount = 0;
                                                    }
                                                }

                                                Log("[ " + DateTime.Now + " ] => [ Starting FakeReplies Process With User Name : " + UserName + " ]");

                                                List<string> lstFakeReplies = FakeReplies;
                                                lstFakeReplies = lstFakeReplies.Distinct().ToList();

                                                if (lstFakeReplies.Count > 0 && FakeScreenName.Count > 0)
                                                {

                                                    if (qFakeReplies.Count < 1)
                                                    {
                                                        try
                                                        {
                                                            lstFakeReplies = lstFakeReplies.Distinct().ToList();
                                                            foreach (string item in lstFakeReplies)
                                                            {
                                                                qFakeReplies.Enqueue(item);
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    if (qFakeScreenName.Count < 1)
                                                    {
                                                        try
                                                        {
                                                            FakeScreenName = FakeScreenName.Distinct().ToList();
                                                            foreach (string item in FakeScreenName)
                                                            {
                                                                if (item != Screen_Name)
                                                                {
                                                                    qFakeScreenName.Enqueue(item);
                                                                }
                                                            }
                                                        }
                                                        catch
                                                        {
                                                        }
                                                    }

                                                    string fakeReply = qFakeReplies.Dequeue();//lstFakeReplies[new Random().Next(0, lstFakeReplies.Count)];
                                                    string fakeScreenName = qFakeScreenName.Dequeue();//FakeScreenName[new Random().Next(0, FakeScreenName.Count)];

                                                    string fakeReplyTweets = string.Empty;
                                                    if(fakeScreenName !=Screen_Name)
                                                    {
                                                        fakeReplyTweets = "@" + fakeScreenName + " " + fakeReply;
                                                    }
                                                    string status = string.Empty;

                                                    if (!string.IsNullOrEmpty(fakeReplyTweets))
                                                    {
                                                        obj_RandomiserManager.Tweet(ref globusHttpHelper, "", "", fakeReplyTweets, out status);
                                                    }

                                                    if (status == "posted")
                                                    {
                                                        //FakeReplies.Remove(fakeReply);
                                                        FakeRepliesCount++;

                                                        Log("[ " + DateTime.Now + " ] => [ FakeReplies : " + fakeReplyTweets + "  Posted Successfully With User Name : " + UserName + " ]");

                                                        TotalPostingCount++;

                                                        
                                                        MentionsCount++;

                                                        
                                                        /// Manage dataBase

                                                        obj_clsDB_Randomiser.QTF_InserIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode("FakeReplies"), StringEncoderDecoder.Encode(fakeReply), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                                        ///Delay
                                                        int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                                        Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Mins Delay With User Name : " + UserName + " ]");

                                                        Thread.Sleep(delay);

                                                    }
                                                    else
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ FakeReplies : " + fakeReplyTweets + " Couldn't Post Successfully With User Name : " + UserName + " ]");
                                                    }
                                                }
                                                else
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ All FakeReplies Have Been Used With User Name : " + UserName + " ]");
                                                    TotalPostingCount++;
                                                }

                                            }
                                            catch
                                            {
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (!isFakeRepliesCompleted)
                                        //    {
                                        //        Log(FakeRepliesPosting + " FakeReplies Posting Completed With User Name : " + UserName);

                                        //        isFakeRepliesCompleted = true;
                                        //        //Log((1).ToString() + " minutes Delay With User Name : " + UserName);
                                        //        //Thread.Sleep(60 * 1000);
                                        //    }
                                        //}
                                    }
                                    catch
                                    {
                                    }
                                    break;
                                #region Mention commneted
                                //case "Mentions":
                                //    try
                                //    {
                                //        if (MentionsCount >= AfterNoOfPosting)
                                //        {
                                //            MentionsCount = 0;
                                //            try
                                //            {
                                //                Log("Starting Mentions Process With User Name : " + UserName);

                                //                List<string> lstMentions = Mentions;
                                //                lstMentions = lstMentions.Distinct().ToList();

                                //                if (lstMentions.Count > 0)
                                //                {

                                //                    if (qMentions.Count < 1)
                                //                    {
                                //                        try
                                //                        {
                                //                            lstMentions = lstMentions.Distinct().ToList();
                                //                            foreach (string item in lstMentions)
                                //                            {

                                //                                    qMentions.Enqueue(item);

                                //                            }
                                //                        }
                                //                        catch
                                //                        {
                                //                        }
                                //                    }

                                //                    string mentions = qMentions.Dequeue(); //lstMentions[new Random().Next(0, lstMentions.Count)];

                                //                    DataSet ds = new DataSet();
                                //                    ds = obj_clsDB_Randomiser.Selecttb_RandomiserMention(StringEncoderDecoder.Encode(UserName));

                                //                    List<string> lstMentionName = new List<string>();

                                //                    if (ds.Tables["tb_RandomiserMention"].Rows.Count > 0)
                                //                    {
                                //                        foreach (DataRow item in ds.Tables["tb_RandomiserMention"].Rows)
                                //                        {
                                //                            string MentionName = item["TweetUserName"].ToString();
                                //                            if (item["TweetUserName"].ToString() != "@" + Screen_Name && !string.IsNullOrEmpty(item["TweetUserName"].ToString()))
                                //                            {
                                //                                lstMentionName.Add(item["TweetUserName"].ToString());
                                //                            }

                                //                            //if (item["ReplyUserName"].ToString() != "@" + Screen_Name && !string.IsNullOrEmpty(item["ReplyUserName"].ToString()))
                                //                            //{
                                //                            //    lstMentionName.Add(item["ReplyUserName"].ToString());
                                //                            //}
                                //                        }

                                //                        lstMentionName = lstMentionName.Distinct().ToList();
                                //                    }
                                //                    else
                                //                    {
                                //                        Log("Please Click On Get Mentions Button or There Is No New Tweet User Name ! With User Name : "+UserName);
                                //                        break;
                                //                    }

                                //                    string mentionName = string.Empty;

                                //                    if (lstMentionName.Count > 0)
                                //                    {
                                //                        mentionName = lstMentionName[new Random().Next(0, lstMentionName.Count)];
                                //                    }
                                //                    else
                                //                    {
                                //                        Log("There is No New Tweet User Name With User Name :" + UserName);
                                //                        break;
                                //                    }


                                //                    mentions = mentionName+" " + mentions;

                                //                    string status = string.Empty;

                                //                    obj_RandomiserManager.Tweet(ref globusHttpHelper, "", "", mentions, out status);

                                //                    if (status.Contains("posted"))
                                //                    {
                                //                        //Mentions.Remove(mentions);
                                //                        //NormalTweetsCount++;

                                //                        Log("Mentions : " + mentions + "  Posted Successfully With User Name : " + UserName);

                                //                        //TotalPostingCount++;

                                //                        MentionsCount=0;
                                //                        /// Manage dataBase

                                //                        obj_clsDB_Randomiser.Mention_InsertIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(mentionName), StringEncoderDecoder.Encode("Mention"), StringEncoderDecoder.Encode(mentions), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                //                        obj_clsDB_Randomiser.Updatetb_RandomiserMention(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(mentionName));

                                //                        ///Delay
                                //                        int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                //                        Log((delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName);


                                //                        Thread.Sleep(delay);


                                //                    }
                                //                    else
                                //                    {
                                //                        Log("Mentions : " + mentions + " Couldn't Post Successfully With User Name : " + UserName);
                                //                    }
                                //                }
                                //                else
                                //                {
                                //                    Log("All Mentions Have Been Used With User Name : " + UserName);
                                //                    //TotalPostingCount++;
                                //                }
                                //            }
                                //            catch
                                //            {
                                //            }
                                //        }

                                //    }
                                //    catch
                                //    {
                                //    }

                                //    break; 
                                #endregion
                                case "Follow":
                                    if (useMention)
                                    {
                                        if (AfterNoOfPosting > 0)
                                        {
                                            if (MentionsCount == AfterNoOfPosting)
                                            {
                                                SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                MentionsCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                            MentionsCount = 0;
                                        }
                                    }
                                    Log("[ " + DateTime.Now + " ] => [ Starting Follow Process With User Name : " + UserName + " ]");
                                    //bool isAlreadyFollowed = lst_AlreadyFollowings.Exists(s => (s == FollowedUsername));

                                    //if (!isAlreadyFollowed)  //If not already being followed, follow now
                                    //{
 
                                    //}
                                    if (qFakeFollower.Count > 0)
                                    {
                                        int Count_RemainFollow = 0;
                                        int countFollow = 0;

                                        // No Of Foolow per time Condition
                                        for (countFollow = 0; countFollow < noOFfollowPerTime; countFollow++)
                                        {

                                            try
                                            {

                                                string FollowedUsername = qFakeFollower.Dequeue();
                                                string username = string.Empty;
                                                string userid = string.Empty;
                                                bool isFollowed = false;
                                                if (!string.IsNullOrEmpty(FollowedUsername))
                                                {
                                                    string date = DateTime.Today.ToString();
                                                    DataTable dt_SelectFollowDataAccordingDate = obj_clsDB_Randomiser.SelectFollowDataAccordingDate(UserName, date);

                                                    int countRows_SelectFollowDataAccordingDate = dt_SelectFollowDataAccordingDate.Rows.Count;

                                                    // Check No of follow per Day Condition
                                                    if (_NoOFfollow)
                                                    {
                                                        if (countRows_SelectFollowDataAccordingDate < _MaximumFollow)
                                                        {
                                                            Count_RemainFollow = _MaximumFollow - countRows_SelectFollowDataAccordingDate;
                                                        }
                                                        else
                                                        {
                                                            Log("[ " + DateTime.Now + " ] => [ Count Follow According Date Is Greater Than Maximum Follow !  With User Name : " + UserName + " ]");
                                                            return;
                                                        }
                                                    }

                                                    //Follower.Follower Obj_Follow = new Follower.Follower();
                                                    bool isAlreadyFollowed = true;
                                                    string status = string.Empty;
                                                    DataTable dt = obj_clsDB_Randomiser.SelectFollowData(UserName);

                                                    foreach (DataRow dr in dt.Rows)
                                                    {
                                                        if (dr.ItemArray[2].ToString().Contains(FollowedUsername) || dr.ItemArray[3].ToString().Contains(FollowedUsername))
                                                        {
                                                            Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + UserName + " ]");
                                                            isFollowed = true;
                                                        }
                                                    }

                                                    #region FollowerollowingRatio
                                                    if (_DontFollowUsersWithFollowingsFollowersRatio)
                                                    {
                                                        int FollowingsFollowersRatio_user_id = 0;
                                                        RandomiserTwitterDataScrapper dataScrapper = new RandomiserTwitterDataScrapper();
                                                        try
                                                        {
                                                            string returnstatusFollower = string.Empty;
                                                            string returnstatusFollowing = string.Empty;
                                                            List<string> Following = dataScrapper.GetFollowings(FollowedUsername, out returnstatusFollowing);
                                                            List<string> Follower = dataScrapper.GetFollowers(FollowedUsername, out returnstatusFollower);
                                                            int count_Followings_user_id = Following.Count;
                                                            int count_Followers_user_id = Follower.Count;

                                                            FollowingsFollowersRatio_user_id = (count_Followings_user_id * 100) / count_Followers_user_id;
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- UseRatioFilter --> " + ex.Message, Globals.Path_FollowerErroLog);
                                                        }

                                                        if (!(FollowingsFollowersRatio_user_id >= _FollowingsFollowersRatio)) //If FollowingsFollowersRatio_user_id is less than Required, continue with next user_id_toFollow
                                                        {
                                                            Log("[ " + DateTime.Now + " ] => [ Not Followed as FollowingsFollowersRatio : " + FollowingsFollowersRatio_user_id + " ]");
                                                            continue;
                                                        }
                                                    }
                                                    #endregion

                                                    if (!isFollowed)
                                                    {
                                                        if (NumberHelper.ValidateNumber(FollowedUsername))
                                                        {
                                                            username = RandomiserTwitterDataScrapper.GetUserNameFromUserId(FollowedUsername);
                                                            userid = FollowedUsername;
                                                        }
                                                        else
                                                        {
                                                            string outStatus = string.Empty;
                                                            userid = RandomiserTwitterDataScrapper.GetUserIDFromUsername(FollowedUsername, out outStatus);
                                                            username = FollowedUsername;
                                                        }
                                                        if (_DontFollowUsersThatUnfollowedBefore)
                                                        {
                                                            isAlreadyFollowed = AlreadyFollowing.Exists(s => (s == FollowedUsername));
                                                        }
                                                        if (!isAlreadyFollowed)
                                                        {
                                                            Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + UserName + " ]");
                                                            continue;
                                                        }

                                                        // Follow 
                                                        Obj_Follow.FollowUsingProfileID(ref globusHttpHelper, "", postAuthenticityToken, FollowedUsername, out status);

                                                        if (status == "followed")
                                                        {
                                                            Log("[ " + DateTime.Now + " ] => [ Followed " + FollowedUsername + " From " + UserName + " ]");
                                                            obj_clsDB_Randomiser.Follow_InserIntotb_Randomiser(UserName, "Followed", FollowedUsername, DateTime.Now.ToString(), "0", "0");
                                                            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(username))
                                                            {
                                                                obj_clsDB_Randomiser.InsertUpdateFollowTable(UserName, userid, username);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            qFakeFollower.Enqueue(FollowedUsername);
                                                            Log("[ " + DateTime.Now + " ] => [ Not Followed " + FollowedUsername + " From " + UserName + " ]");
                                                        }

                                                        //Delay
                                                        int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                                        Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");

                                                        Thread.Sleep(delay);
                                                    }
                                                    else
                                                    {
                                                        Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + UserName + " ]");
                                                    }



                                                }
                                                else
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ No Username Available ]");
                                                }
                                            }
                                            catch
                                            {
                                                }
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ All Users Followed With " + UserName + " ]");
                                    }
                                    
                                    break;

                                case "FollowBySearchKeyword":
                                    if (useMention)
                                    {
                                        if (AfterNoOfPosting > 0)
                                        {
                                            if (MentionsCount == AfterNoOfPosting)
                                            {
                                                SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                                MentionsCount = 0;
                                            }
                                        }
                                        else
                                        {
                                            SetMention(ref  globusHttpHelper, ref  userId, ref  userName, ref  Screen_name, ref  postAuthenticityToken);

                                            MentionsCount = 0;
                                        }
                                    }
                                    Log("[ " + DateTime.Now + " ] => [ Starting Follow By Search Keyword Process With User Name : " + UserName + " ]");

                                    RandomiserTwitterDataScrapper obj_RandomiserTwitterDataScrapper = new RandomiserTwitterDataScrapper();
                                    List<RandomiserTwitterDataScrapper.StructTweetIDs> KeywordStructData = new List<RandomiserTwitterDataScrapper.StructTweetIDs>();

                                    string statusKeyword = string.Empty;

                                    if (_Followbysinglekeywordperaccount)
                                    {
                                        try
                                        {
                                            string SeachKey = qKeyword.Dequeue();//lstKeywords[0];//[AccountCounter];
                                            KeywordStructData = obj_RandomiserTwitterDataScrapper.KeywordStructData(SeachKey);

                                            if (KeywordStructData.Count == 0)
                                            {
                                                KeywordStructData = obj_RandomiserTwitterDataScrapper.KeywordStructData(SeachKey);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        foreach (string SeachKey_item in lstKeywords)
                                        {
                                            try
                                            {
                                                List<RandomiserTwitterDataScrapper.StructTweetIDs> KeywordStructData1 = new List<RandomiserTwitterDataScrapper.StructTweetIDs>();
                                                int datacounter = 0;
                                                KeywordStructData1 = obj_RandomiserTwitterDataScrapper.KeywordStructData(SeachKey_item);

                                                if (KeywordStructData1.Count == 0)
                                                {
                                                    KeywordStructData1 = obj_RandomiserTwitterDataScrapper.KeywordStructData(SeachKey_item);
                                                }


                                                foreach (var KeywordStructData1_item in KeywordStructData1)
                                                {
                                                    if (datacounter > (_NoFollowByPerAccount))
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

                                            catch
                                            {
                                            }
                                        }
                                    }

                                    foreach (RandomiserTwitterDataScrapper.StructTweetIDs item in KeywordStructData)
                                    {
                                        try
                                        {
                                            Obj_Follow.FollowUsingProfileID(ref globusHttpHelper, "", postAuthenticityToken, item.ID_Tweet_User, out statusKeyword);

                                            if (statusKeyword == "followed")
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ Followed " + item.ID_Tweet + " From " + UserName + " ]");
                                                obj_clsDB_Randomiser.Follow_InserIntotb_Randomiser(UserName, "Followed", item.ID_Tweet_User, DateTime.Now.ToString(), "0", "0");
                                                //if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(username))
                                                //{
                                                //    obj_clsDB_Randomiser.InsertUpdateFollowTable(UserName, userid, username);
                                                //}
                                            }
                                            else
                                            {
                                                //qFakeFollower.Enqueue(FollowedUsername);
                                                //Log("Not Followed " + FollowedUsername + " From " + UserName);
                                            }

                                            //Delay
                                            int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                            Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");

                                            Thread.Sleep(delay);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                        catch
                        {
                        }
                    }
                    catch
                    {
                    }
                }

                Log("[ " + DateTime.Now + " ] => [ All Process Stopp ed ...With User Name : " + UserName + " ]");
                    //Thread.Sleep(60*1000);
                    //Log(((60*1000 )/ (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName);
            }
            catch
            {
            }
        }

        public void SetProcess()
        {
            try
            {
                lstProcess.Add("Quotes");
                lstProcess.Add("NormalTweets");
                lstProcess.Add("ReTweets");
                lstProcess.Add("FakeReplies");
                //lstProcess.Add("Mentions");
                if (useFollow)
                {
                    lstProcess.Add("Follow");
                }
                if (_UseFollowbySearchKeyword)
                {
                    lstProcess.Add("FollowBySearchKeyword");
                }
            }
            catch
            {
            }
        }

        private void SetMention(ref Globussoft.GlobusHttpHelper globusHttpHelper, ref string userId, ref string userName, ref string Screen_name, ref string postAuthenticityToken)
        {
            try
            {
                if (!isMentionsStop)
                {
                    //if (MentionsCount == AfterNoOfPosting)
                    {
                        MentionsCount = 0;
                        try
                        {
                            Log("[ " + DateTime.Now + " ] => [ Starting Mentions Process With User Name : " + UserName + " ]");

                            List<string> lstMentions = Mentions;
                            lstMentions = lstMentions.Distinct().ToList();

                            if (lstMentions.Count > 0)
                            {

                                if (qMentions.Count < 1)
                                {
                                    try
                                    {
                                        lstMentions = lstMentions.Distinct().ToList();
                                        foreach (string item in lstMentions)
                                        {

                                            qMentions.Enqueue(item);

                                        }
                                    }
                                    catch
                                    {
                                    }
                                }

                                string mentions = qMentions.Dequeue(); //lstMentions[new Random().Next(0, lstMentions.Count)];

                                DataSet ds = new DataSet();
                                ds = obj_clsDB_Randomiser.Selecttb_RandomiserMention(StringEncoderDecoder.Encode(UserName));

                                List<string> lstMentionName = new List<string>();

                                if (ds.Tables["tb_RandomiserMention"].Rows.Count > 0)
                                {
                                    foreach (DataRow item in ds.Tables["tb_RandomiserMention"].Rows)
                                    {
                                        string MentionName = item["TweetUserName"].ToString();
                                        if (item["TweetUserName"].ToString() != "@" + Screen_Name && !string.IsNullOrEmpty(item["TweetUserName"].ToString()))
                                        {
                                            lstMentionName.Add(item["TweetUserName"].ToString());
                                        }

                                        //if (item["ReplyUserName"].ToString() != "@" + Screen_Name && !string.IsNullOrEmpty(item["ReplyUserName"].ToString()))
                                        //{
                                        //    lstMentionName.Add(item["ReplyUserName"].ToString());
                                        //}
                                    }

                                    lstMentionName = lstMentionName.Distinct().ToList();
                                }
                                else
                                {
                                    if (!isMentionsStop)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Please Click On Get Mentions Button or There Is No New Tweet User Name ! With User Name : " + UserName + " ]");

                                        isMentionsStop = true;
                                    }
                                    return;
                                }

                                string mentionName = string.Empty;

                                if (lstMentionName.Count > 0)
                                {
                                    mentionName = lstMentionName[new Random().Next(0, lstMentionName.Count)];
                                }
                                else
                                {
                                    if (!isMentionsStop)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ There is No New Tweet User Name With User Name :" + UserName + " ]");
                                        isMentionsStop = true;
                                    }
                                    return;
                                }

                                mentions = mentionName + " " + mentions;

                                string status = string.Empty;

                                obj_RandomiserManager.Tweet(ref globusHttpHelper, "", "", mentions, out status);

                                if (status.Contains("posted"))
                                {
                                    //Mentions.Remove(mentions);
                                    //NormalTweetsCount++;

                                    Log("[ " + DateTime.Now + " ] => [ Mentions : " + mentions + "  Posted Successfully With User Name : " + UserName + " ]");

                                    //TotalPostingCount++;
                                    MentionsCount = 0;
                                    /// Manage dataBase

                                    obj_clsDB_Randomiser.Mention_InsertIntotb_Randomiser(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(mentionName), StringEncoderDecoder.Encode("Mention"), StringEncoderDecoder.Encode(mentions), StringEncoderDecoder.Encode(DateTime.Now.ToString()), StringEncoderDecoder.Encode("0"), StringEncoderDecoder.Encode("0"));

                                    obj_clsDB_Randomiser.Updatetb_RandomiserMention(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(mentionName));

                                    ///Delay
                                    int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                                    Log("[ " + DateTime.Now + " ] => [ " + (delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName + " ]");


                                    Thread.Sleep(delay);


                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Mentions : " + mentions + " Couldn't Post Successfully With User Name : " + UserName + " ]");
                                }
                            }
                            else
                            {
                                if (!isMentionsStop)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ There Is No Mentions Uploaded With User Name : " + UserName + " ]");
                                    isMentionsStop = true;
                                }
                                //TotalPostingCount++;
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

        public void GetMentions(ref Globussoft.GlobusHttpHelper globusHttpHelper, ref string userId, ref string userName, ref string Screen_name, ref string postAuthenticityToken)
        {
            try
            {
                string tweetId = string.Empty;
                string tweetUserName = string.Empty;
                string tweetMessage = string.Empty;
                string maxId = string.Empty;

                Log("[ " + DateTime.Now + " ] => [ Starting Scrap Mentions.............With User Name : " + UserName + " ]");

                string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/mentions"), "", "");

                DataSet ds_BeforeInsertionCount = obj_clsDB_Randomiser.SelectAllfromtb_RandomiserMention();
                int countBeforeInsertion = ds_BeforeInsertionCount.Tables["tb_RandomiserMention"].Rows.Count;

                if (pageSource.Contains("stream-container "))
                {
                    string streamContainer = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "stream-container ");

                    string[] arrContent=Regex.Split(streamContainer, "content");

                    foreach (string item in arrContent)
                    {
                        try
                        {
                            if (item.Contains("username js-action-profile-name"))
                            {
                                try
                                {
                                    List<string> lst_TweetUserName = globusHttpHelper.GetTextDataByTagAndAttributeName(item, "span", "username js-action-profile-name");

                                    if (lst_TweetUserName.Count > 0)
                                    {
                                        foreach (string item2 in lst_TweetUserName)
                                        {
                                            try
                                            {
                                                if(!string.IsNullOrEmpty(item2))
                                                {
                                                    tweetUserName = item2;
                                                    break;
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

                            if (item.Contains("js-tweet-text"))
                            {
                                try
                                {
                                    List<string> lstTweetReply = globusHttpHelper.GetTextDataByTagAndAttributeName(item, "p", "js-tweet-text");

                                    if (lstTweetReply.Count > 0)
                                    {
                                        foreach (string item3 in lstTweetReply)
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(item3))
                                                {
                                                    tweetMessage = item3;
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

                            if (item.Contains("data-user-id="))
                            {
                                try
                                {
                                    string tweetUserId1 = item.Substring(item.IndexOf("data-user-id="), item.IndexOf(" ", item.IndexOf("data-user-id=")) - item.IndexOf("data-user-id=")).Replace("data-user-id=", string.Empty).Replace("\"", string.Empty).Trim();

                                    string[] ArrtweetUserId = Regex.Split(tweetUserId1, "[^0-9]");

                                    foreach (string item1 in ArrtweetUserId)
                                    {
                                        try
                                        {
                                            if (item1.Length > 3)
                                            {
                                                tweetId = item1;
                                            }
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

                            if (item.Contains("data-max-id="))
                            {
                                try
                                {
                                    string data_max_id = item.Substring(item.IndexOf("data-max-id="), item.IndexOf(" ", item.IndexOf("data-max-id=")) - item.IndexOf("data-max-id=")).Replace("data-max-id=", string.Empty).Replace("\"", string.Empty).Trim();

                                    string[] arrdata_max_id = Regex.Split(data_max_id, "[^0-9]");

                                    foreach (string item4 in arrdata_max_id)
                                    {
                                        try
                                        {
                                            if (item4.Length > 3)
                                            {
                                                maxId = item4;
                                            }
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
                        }
                        catch
                        {
                        }

                        
                        DataSet ds2 = obj_clsDB_Randomiser.Selecttb_RandomiserMention(UserName, tweetUserName, tweetMessage);
                        if (ds2.Tables["tb_RandomiserMention"].Rows.Count < 1)
                        {

                            if (!string.IsNullOrEmpty(tweetUserName))
                            {
                                obj_clsDB_Randomiser.Mention_InsertIntotb_RandomiserMention(StringEncoderDecoder.Encode(userName), StringEncoderDecoder.Encode(Screen_Name), StringEncoderDecoder.Encode(tweetId), StringEncoderDecoder.Encode(tweetUserName), StringEncoderDecoder.Encode(tweetMessage));

                                Log("[ " + DateTime.Now + " ] => [ " + count + "Record Saved in Database With User Name : " + UserName + " ]");

                                count++;
                            }
                        }

                    }
                }

                DataSet ds_AfterInsertionCount = obj_clsDB_Randomiser.SelectAllfromtb_RandomiserMention();
                int countAfterInsertion = ds_AfterInsertionCount.Tables["tb_RandomiserMention"].Rows.Count;

                if (countBeforeInsertion == countAfterInsertion)
                {
                    Log("[ " + DateTime.Now + " ] => [ There Is No New Mentions With User Name : " + UserName + " ]");
                }

                /// Ajax Request
                GetMentions_ThroughAjax(ref globusHttpHelper, ref userId, ref userName, ref Screen_name, ref postAuthenticityToken, maxId);

                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED With User Name : " + UserName + " ]");
                Log("------------------------------------------------------------------------------------------------------------------------------------------");

            }
            catch
            {
            }
        }

        private void GetMentions_ThroughAjax(ref Globussoft.GlobusHttpHelper globusHttpHelper, ref string userId, ref string userName, ref string Screen_name, ref string postAuthenticityToken, string MaxId)
        {
            string tweetId = string.Empty;
            string tweetUserName = string.Empty;
            string tweetMessage = string.Empty;
            string max_id = MaxId;
        
            try
            {
                string status = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/mentions/timeline?include_available_features=1&include_entities=1&max_id=" + max_id), "", "");
                               
                if (status.Contains("div class=\\\"content"))
                {
                    string[] arrContent = Regex.Split(status, @"div class=\\\""content");

                    foreach (string item in arrContent)
                    {
                        try
                        {
                            if (item.Contains("username js-action-profile-name"))
                            {
                                try
                                {
                                    try
                                    {
                                        tweetUserName = item.Substring(item.IndexOf("username js-action-profile-name"), (item.IndexOf("div", item.IndexOf("username js-action-profile-name")) - item.IndexOf("username js-action-profile-name"))).Replace("username js-action-profile-name", string.Empty).Replace("\"", string.Empty).Replace("u003E", string.Empty).Replace("u003Cs", string.Empty).Replace("/s", string.Empty).Replace("u003Cb", string.Empty).Replace("/b", string.Empty).Replace("/span", string.Empty).Replace("n", string.Empty).Replace("/a", string.Empty).Replace("\\\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\u003C\\\u003C\\\\", string.Empty).Replace(@"\pa\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\\u003C\\/", string.Empty).Replace(@"\u003C", string.Empty).Replace(@"\u003C\u003C\pa", string.Empty).Replace(@"\u003C\", string.Empty).Replace(@"\", string.Empty).Trim();

                                        //Testing
                                        string findlastPa = string.Empty;
                                        if (tweetUserName.Contains("pa"))
                                        {
                                            findlastPa = findlastPa + tweetUserName[tweetUserName.Length - 2].ToString();
                                            findlastPa = findlastPa + tweetUserName[tweetUserName.Length - 1].ToString();

                                            if (findlastPa == "pa")
                                            {
                                                tweetUserName = tweetUserName.Replace("pa", string.Empty).Trim();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    List<string> lst_TweetUserName = globusHttpHelper.GetTextDataByTagAndAttributeName(item, "span", "username js-action-profile-name");

                                    if (lst_TweetUserName.Count > 0)
                                    {
                                        foreach (string item2 in lst_TweetUserName)
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(item2))
                                                {
                                                    tweetUserName = item2;
                                                    break;
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

                            if (item.Contains("js-tweet-text"))
                            {
                                try
                                {

                                    try
                                    {
                                        tweetMessage = item.Substring(item.IndexOf("js-tweet-text"), (item.IndexOf("div", item.IndexOf("js-tweet-text")) - item.IndexOf("js-tweet-text"))).Replace("js-tweet-text", string.Empty).Replace("\"", string.Empty).Replace("u003E", string.Empty).Replace("u003Cs", string.Empty).Replace("/s", string.Empty).Replace("u003Cb", string.Empty).Replace("/b", string.Empty).Replace("/span", string.Empty).Replace("n", string.Empty).Replace("/a", string.Empty).Replace("\\\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\u003C\\\u003C\\\\", string.Empty).Replace(@"\pa\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\\u003C\\/", string.Empty).Replace(@"\u003C", string.Empty).Replace(@"\u003C\u003C\pa", string.Empty).Replace(@"\u003C\", string.Empty).Replace(@"\", string.Empty).Trim();

                                        if (tweetMessage.Contains("twitter-atreply pretty-lik dir=ltr "))
                                        {
                                            try
                                            {
                                                //int startIndex = tweetMessage.IndexOf("twitter-atreply pretty-lik");
                                                tweetMessage = tweetMessage.Substring(tweetMessage.IndexOf("twitter-atreply pretty-lik dir=ltr"), (tweetMessage.Length) - (tweetMessage.IndexOf(@"twitter-atreply pretty-lik dir=ltr "))).Replace(@"twitter-atreply pretty-lik dir=ltr", string.Empty).Replace("\"", string.Empty).Replace("u003E", string.Empty).Replace("u003Cs", string.Empty).Replace("/s", string.Empty).Replace("u003Cb", string.Empty).Replace("/b", string.Empty).Replace("/span", string.Empty).Replace("n", string.Empty).Replace("/a", string.Empty).Replace("\\\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\u003C\\\u003C\\\\", string.Empty).Replace(@"\pa\\", string.Empty).Replace("\u003C\\\\", string.Empty).Replace("\\u003C\\/", string.Empty).Replace(@"\u003C", string.Empty).Replace(@"\u003C\u003C\pa", string.Empty).Replace(@"\u003C\", string.Empty).Replace(@"\", string.Empty).Trim();

                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    List<string> lstTweetReply = globusHttpHelper.GetTextDataByTagAndAttributeName(item, "p", "js-tweet-text");

                                    if (lstTweetReply.Count > 0)
                                    {
                                        foreach (string item3 in lstTweetReply)
                                        {
                                            try
                                            {
                                                if (!string.IsNullOrEmpty(item3))
                                                {
                                                    tweetMessage = item3;
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

                            if (item.Contains("data-user-id="))
                            {
                                try
                                {
                                    string tweetUserId1 = item.Substring(item.IndexOf("data-user-id=")+13, (item.IndexOf("\"", item.IndexOf("data-user-id=")+13) - item.IndexOf("data-user-id=")+13)).Replace("data-user-id=", string.Empty).Replace("\"", string.Empty).Trim();

                                    string[] ArrtweetUserId = Regex.Split(tweetUserId1, "[^0-9]");

                                    foreach (string item1 in ArrtweetUserId)
                                    {
                                        try
                                        {
                                            if (item1.Length > 3)
                                            {
                                                tweetId = item1;
                                                break;
                                            }
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


                        }
                        catch
                        {
                        }

                        DataSet ds2 = obj_clsDB_Randomiser.Selecttb_RandomiserMention(StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(tweetUserName), StringEncoderDecoder.Encode(tweetMessage));
                        if (ds2.Tables["tb_RandomiserMention"].Rows.Count < 1)
                        {

                            if (!string.IsNullOrEmpty(tweetUserName)) // && tweetUserName !="@"+Screen_Name
                            {
                                obj_clsDB_Randomiser.Mention_InsertIntotb_RandomiserMention(StringEncoderDecoder.Encode(userName), StringEncoderDecoder.Encode(Screen_Name), StringEncoderDecoder.Encode(tweetId), StringEncoderDecoder.Encode(tweetUserName), StringEncoderDecoder.Encode(tweetMessage));

                                Log("[ " + DateTime.Now + " ] => [ " + count + "Record Saved in Database With User Name : " + UserName + " ]");

                                count++;
                            }
                        }
                    }

                    if (status.Contains("\"max_id\":"))
                    {
                        try
                        {
                            string max_id2 = status.Substring(status.IndexOf("\"max_id\":"), status.Length - status.IndexOf("\"max_id\":"));
                            string max_id1 = status.Substring(status.IndexOf("\"max_id\":") + 10, (status.IndexOf("\"", (status.IndexOf("\"max_id\":") + 10)) - (status.IndexOf("\"max_id\":") + 10))).Replace("\"max_id\":", string.Empty).Replace("\"", string.Empty).Trim();
                            string[] arrMaxId = Regex.Split(max_id, "[^0-9]");
                            foreach (string item in arrMaxId)
                            {
                                try
                                {
                                    if (item.Length > 5)
                                    {
                                        max_id = max_id1;

                                        GetMentions_ThroughAjax(ref globusHttpHelper, ref userId, ref userName, ref Screen_name, ref postAuthenticityToken, max_id);
                                        //List<string> lstNextAjaxId = GetStatusIdThroughAjax(ref globusHttpHelper, screenName, item);
                                        //break;

                                    }
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
                }
            }
            catch
            {
            }
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            //logEvents_static.LogText(eventArgs);
            logEvents.LogText(eventArgs);
        }

    }
}
