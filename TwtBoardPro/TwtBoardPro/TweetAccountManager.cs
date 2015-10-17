using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Threading;
using System.Text.RegularExpressions;
using System.Data;
using Globussoft;
using System.IO;
using System.Windows.Forms;

namespace twtboardpro
{
    /// <summary>
    /// This Class is a complete package for a Twitter Account, contains most of the Relevant Information
    /// 1 Twitter Instance is equivalent to 1 Twitter Account
    /// Instances are stored in "TweetAccountContainer.dictionary_TweetAccount"
    /// </summary>
    public class TweetAccountManager
    {
       
        public string Username = string.Empty;
        public string Password = string.Empty;
       
        public string userID = string.Empty;
        public string postAuthenticityToken = string.Empty;

        public List<string> list_Followers = new List<string>();
        public List<string> list_Followings = new List<string>();
        public List<string> list_NonFollowings = new List<string>();

        public string pgSrc_Profile = string.Empty;

        public string IPAddress = string.Empty;
        public string IPPort = string.Empty;
        public string IPUsername = string.Empty;
        public string IPpassword = string.Empty;

        public string IPAddress_Socks5 = string.Empty;
        public string IPPort_Socks5 = string.Empty;
        public string IPUsername_Socks5 = string.Empty;
        public string IPpassword_Socks5 = string.Empty;

        public bool UseHashTags = false;
        public Queue<string> que_TweetMessages_Hashtags = new Queue<string>();
        public static readonly object locker_que_hashtags = new object();
        public static readonly object locker_que_Follower = new object();
        string Hashtags = string.Empty;

        public int profileStatus = 0;

        public Globussoft.GlobusHttpHelper globusHttpHelper = new Globussoft.GlobusHttpHelper();

        public bool IsLoggedIn = false;
        public bool IsNotSuspended = false;
        public bool Isnonemailverifiedaccounts = false;

        public static List<TwitterDataScrapper.StructTweetIDs> static_lst_Struct_TweetData { get; set; }
        public static Queue<TwitterDataScrapper.StructTweetIDs> que_lst_Struct_TweetData = new Queue<TwitterDataScrapper.StructTweetIDs>();
        public Queue<TwitterDataScrapper.StructTweetIDs> que_lst_Struct_TweetData1 = new Queue<TwitterDataScrapper.StructTweetIDs>();
        public static bool IsRetweetDivideRetweet = false;
        public static readonly object locker_qque_lst_Struct_TweetData = new object();

        public List<TwitterDataScrapper.StructTweetIDs> lst_Struct_TweetData { get; set; }

        public static List<string> listTweetMessages { get; set; }

        public static List<string> listReplyMessages { get; set; }


        public static List<string> lstDirectMessageText = new List<string>();

        //Queue<string> que_TweetMessages = new Queue<string>();
        //Queue<string> que_ReplyMessages = new Queue<string>();

        public static Queue<string> que_TweetMessages = new Queue<string>();
        public Queue<string> que_TweetMessages_PerAccount = new Queue<string>();
        public static Queue<string> que_ReplyMessages = new Queue<string>();

        public static Queue<string> que_TweetMessages_WaitAndReply = new Queue<string>();
        public static Queue<string> que_ReplyMessages_WaitAndReply = new Queue<string>();
        public static Queue<string> que_ImagePath_WaitAndReply = new Queue<string>();

        public static readonly object locker_que_TweetMessage = new object();
        public static readonly object locker_que_ReplyTweetMessage = new object();

        public static readonly object locker_que_keywyordStructData = new object();

        public static readonly object locker_que_TweetMessages_WaitAndReply = new object();
        public static readonly object locker_que_ReplyMessage_WaitAndReply = new object();

        public static int NoOfTweets = 10;

        public static Queue<string> que_DirectMessage = new Queue<string>();
        public static readonly object locker_DirectMessage = new object();


        Regex IdCheck1 = new Regex("^[0-9]*$");

        public Follower.Follower follower = new Follower.Follower();

        public Tweeter.Tweeter tweeter = new Tweeter.Tweeter();

        public ProfileManager.ProfileManager profileUpdater = new ProfileManager.ProfileManager();

        //public static BaseLib.Events logEvents_static = new BaseLib.Events();

        public Events logEvents = new Events();

        public Events logEventsArabFollower = new Events();

        public Follower.Unfollower unFollower = new Follower.Unfollower();

        public int noOfRetweets = 10;

        public static int noOfUnfollows = 5;

        public static int noOFFollows = 5;
        public static bool UseDateLastTweeted = false;
        public static bool UseRatioFilter = false;
        public static bool UseUnfollowedBeforeFilter = false;
        public static int FollowingsFollowersRatio = 80;
        public static int LastTweetDays = 80;

        public static int NoOfTweetPerDay = 10;
        public static bool TweetPerDay = false;
        public static bool AllTweetsPerAccount = false;
        public int AlreadyTweeted = 0;

        public static DateTime StartTime;
        public static DateTime EndTime;

        public static int NoOFRetweetPerDay = 10;
        public static bool RetweetPerDay = false;
        public int AlreadyRetweeted = 0;

        public static int NoOFReplyPerDay = 10;
        public static bool ReplyPerDay = false;
        public int AlreadyReply = 0;

        //added By abhsihek 
        public static int NoOfFollowPerDay { get; set; }
        public static bool NoOfFollowPerDay_ChkBox = false;
        public int RemainingNoOfFollowPerDay = 0;
        //added By abhsihek 

        public static int TotalEmailsUploded = 0;
        public static int AccountCount = 0;

        public string Screen_name = string.Empty;
        public string ProfileFullName = string.Empty;
        public string FollowerCount = string.Empty;
        public string FollwingCount = string.Empty;

        public string GroupName = string.Empty;
        public static string ReplyKeyword = string.Empty;
        public string AccountStatus = string.Empty;
        public static int DelayTweet = 0;
        public static bool IscontainPicture = false;
        public static string FileFollowUrlPath = string.Empty;
        public static string FileTweetPath = string.Empty;
        //Added By Lijo for wait and reply module
        public static string ImageFolderPath = string.Empty;
        public static bool IsTweetWithImage = false;
        //Added By Lijo for wait and reply module



        //Added for followr setting>>tweeted in x days
        public static bool IsTweetedInXdays = false;
        public static string daysTweetedInxdays = string.Empty;

        public TweetAccountManager()
        {

        }

        public void TweetAccountManager1(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }
        public TweetAccountManager(string Username, string Password, string Screen_name, string follower_Count, int numberOfMessages, string IPAddress, string IPPort, string IPUsername, string IPpassword, string currentCity, string HomeTown, string Birthday_Month, string BirthDay_Date, string BirthDay_Year, string AboutMe, string Employer, string College, string HighSchool, string Religion, string profilePic, string FamilyName, string Role, string language, string sex, string activities, string interests, string movies, string music, string books, string favoriteSports, string favoriteTeams, string GroupName, string status)
        {
            this.Username = Username;
            this.Password = Password;
            this.IPAddress = IPAddress;
            this.IPPort = IPPort;
            this.IPUsername = IPUsername;
            this.IPpassword = IPpassword;
            this.Screen_name = Screen_name;
            this.FollowerCount = follower_Count;
            this.GroupName = GroupName;
            this.AccountStatus = status;
            Log("[ " + DateTime.Now + " ] => [ Logging in with Account:" + Username + " ]");
        }

        #region Wait & Reply Variables

        public static int noOfTweetsPerReply = 4;
        public static int noOfRepliesPerKeyword = 10;
        public static int WaitAndReplyInterval = 10 * 1000 * 2 * 30; //in minutes
        public static int waitAndReplyMinInterval = 20;
        public static int waitAndReplyMaxInterval = 25;
        public static bool waitAndReplyIsIntervalInsec = false;

        #endregion

        public void Login()
        {
            #region Test Code
            //            List<string> Accounts = new List<string>();

            //            Accounts.Add("galena_tr@hotmail.com:password777");
            //Accounts.Add("adriane.s@hotmail.com:password777");
            //Accounts.Add("romulotaradao@hotmail.com:password777");
            //Accounts.Add("postocantareira@hotmail.com:password777");
            //Accounts.Add("trma_06@hotmail.com:password777");
            //Accounts.Add("dfdghf@hotmail.com:password777");
            //Accounts.Add("paul_ps2@hotmail.co.uk:password777");
            //Accounts.Add("syahmi-244@hotmail.com:password777");
            //Accounts.Add("gabriaffonso@hotmail.com:password777");
            //Accounts.Add("vivi_lemos@hotmail.com:password777");

            //foreach (string item in Accounts)
            //{

            //    string[] items = item.Split(':');

            //    string username = items[0];
            //    string password = items[1];

            //    Globussoft.GlobusHttpHelper HttpHelper1 = new Globussoft.GlobusHttpHelper();

            //   string ts = GenerateTimeStamp();

            //    string a = HttpHelper1.getHtmlfromUrlIP(new Uri("http://twitter.com/"), "173.208.131.234", 8888, "usIP", "logic");

            //    string a2 = HttpHelper1.getHtmlfromUrl(new Uri("http://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "http://twitter.com/", string.Empty);

            //    string PostData = "session%5Busername_or_email%5D=galena_tr@hotmail.com&session%5Bpassword%5D=password777&scribe_log=%5B%22%7B%5C%22event_name%5C%22%3A%5C%22web%3Afront%3Alogin_callout%3Aform%3A%3Alogin_click%5C%22%2C%5C%22noob_level%5C%22%3Anull%2C%5C%22internal_referer%5C%22%3Anull%2C%5C%22user_id%5C%22%3A0%2C%5C%22page%5C%22%3A%5C%22front%5C%22%2C%5C%22_category_%5C%22%3A%5C%22client_event%5C%22%2C%5C%22ts%5C%22%3A" + ts + "%7D%22%5D&redirect_after_login=";

            //    string a3 = HttpHelper1.postFormData(new Uri("https://twitter.com/sessions?phx=1"), PostData, "http://twitter.com/", string.Empty);
            //    string response_Login = HttpHelper1.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

            //} 

            //Globussoft1.GlobusHttpHelper HttpHelper1 = new Globussoft1.GlobusHttpHelper();

            //ChilkatHttpHelpr chilkatHttpHelpr = new ChilkatHttpHelpr();

            //string ts = GenerateTimeStamp();

            //string a = chilkatHttpHelpr.GetHtmlIP("http://twitter.com/", "", "", "neshkito", "7809062345");//HttpHelper1.getHtmlfromUrl(new Uri("http://twitter.com/"), string.Empty, string.Empty);

            ////string a1 = HttpHelper1.getHtmlfromUrl(new Uri("http://scribe.twitter.com/scribe?category=client_event&log=%7B%22context%22%3A%22front%22%2C%22event_name%22%3A%22web%3Afront%3A%3A%3Aimpression%22%7D&ts=1330691303089"),"http://twitter.com/", string.Empty);

            //string a2 = chilkatHttpHelpr.GetHtml("http://twitter.com/account/bootstrap_data?r=0.21632839148912897");//HttpHelper1.getHtmlfromUrl(new Uri("http://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "http://twitter.com/", string.Empty);

            //Username = Uri.EscapeDataString("luanapinheirolp");

            //string PostData = "session%5Busername_or_email%5D=" + Username + "&session%5Bpassword%5D=password777&scribe_log=%5B%22%7B%5C%22event_name%5C%22%3A%5C%22web%3Afront%3Alogin_callout%3Aform%3A%3Alogin_click%5C%22%2C%5C%22noob_level%5C%22%3Anull%2C%5C%22internal_referer%5C%22%3Anull%2C%5C%22user_id%5C%22%3A0%2C%5C%22page%5C%22%3A%5C%22front%5C%22%2C%5C%22_category_%5C%22%3A%5C%22client_event%5C%22%2C%5C%22ts%5C%22%3A" + ts + "%7D%22%5D&redirect_after_login=";

            //string a3 = chilkatHttpHelpr.PostData("https://twitter.com/sessions?phx=1", PostData, "http://twitter.com/");//HttpHelper1.postFormData(new Uri("https://twitter.com/sessions?phx=1"), PostData, "http://twitter.com/", string.Empty);

            //string test = string.Empty;

            //string a1 = chilkatHttpHelpr.GetHtml("http://twitter.com/");

            //Log("Logging in with " + Username);

            //GlobusHttpHelper globusHttpHelper1 = new GlobusHttpHelper();
            #endregion

            try
            {
                Log("[ " + DateTime.Now + " ] => [ Logging in with Account: " + Username + " ]");

                //Thread.Sleep(20000);
                //Password = Password.Replace("?", "%3F").Replace("&", "%26");
                string ts = GenerateTimeStamp();
                string get_twitter_first = string.Empty;
                try
                {
                   // get_twitter_first = globusHttpHelper.getHtmlfromUrl(new Uri(""), "", "");
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty,string.Empty);
                }
                catch (Exception ex)
                {
                    //string get_twitter_first = globusHttpHelper1.getHtmlfromUrlp(new Uri("http://twitter.com/"), string.Empty, string.Empty);
                    Thread.Sleep(1000);
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                }                

                try
                {
                    postAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }
                catch { }
                try
                {

                    //string get_twitter_second = globusHttpHelper.postFormData(new Uri("https://twitter.com/scribe"), "log%5B%5D=%7B%22event_name%22%3A%22web%3Amobile_gallery%3Agallery%3A%3A%3Aimpression%22%2C%22noob_level%22%3Anull%2C%22internal_referer%22%3Anull%2C%22context%22%3A%22mobile_gallery%22%2C%22event_info%22%3A%22mobile_app_download%22%2C%22user_id%22%3A0%2C%22page%22%3A%22mobile_gallery%22%2C%22_category_%22%3A%22client_event%22%2C%22ts%22%3A" + ts + "%7D", "https://twitter.com/?lang=en&logged_out=1#!/download", "", "", "", "");//globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", string.Empty);

                    //string get2nd = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);

                    //string get_api = globusHttpHelper.getHtmlfromUrl(new Uri("http://api.twitter.com/receiver.html"), "https://twitter.com/", "");

                }
                catch { }
                string postData = "session%5Busername_or_email%5D=" + Uri.EscapeDataString(Username) + "&session%5Bpassword%5D=" + Uri.EscapeDataString(Password) + "&authenticity_token=" + postAuthenticityToken + "&scribe_log=&redirect_after_login=&authenticity_token=" + postAuthenticityToken + "&remember_me=1";

                string response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/sessions"), postData, "https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);

                //response_Login = GlobusFileHelper.ReadStringFromTextfile("C:/Users/GLB-111/Desktop/new.txt");
                if (response_Login.Contains("अपनी पहचान सत्यापित करें") || response_Login.Contains("आपके खाते को सुरक्षित रखेने में हमें मदद करें.") || response_Login.Contains("Help us keep your account safe.") || response_Login.Contains("Verify your identity") || response_Login.Contains("account/login_challenge?"))
                {
                    try
                    {
                        string temp_user_id = string.Empty;
                        string challenge_id = string.Empty;
                        challenge_id = response_Login.Substring(response_Login.IndexOf("name=\"challenge_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"challenge_id\" value=")) - response_Login.IndexOf("name=\"challenge_id\" value="))).Replace("name=\"challenge_id\" value=", string.Empty).Replace("\"", "").Trim();
                        temp_user_id = response_Login.Substring(response_Login.IndexOf("name=\"user_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"user_id\" value=")) - response_Login.IndexOf("name=\"user_id\" value="))).Replace("name=\"user_id\" value=", string.Empty).Replace("\"", "").Trim();
                        if (response_Login.Contains(" name=\"challenge_type\" value=\"RetypeEmail") && response_Login.Contains("@"))
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);
                        }
                        else
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);

                        }
                    }
                    catch { }
                }

                string homePage = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com"), "", "");

                string responseURI = globusHttpHelper.gResponse.ResponseUri.ToString().ToLower();

                if (response_Login.Contains("signout") || homePage.Contains("signout"))
                {
                    postAuthenticityToken = PostAuthenticityToken(response_Login, "postAuthenticityToken");

                    try
                    {
                        int startIndx = response_Login.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                        int endIndx = response_Login.IndexOf("\"", startIndx);
                        userID = response_Login.Substring(startIndx, endIndx - startIndx);
                    }
                    catch { }

                    if (string.IsNullOrEmpty(userID))
                    {
                        userID = string.Empty;
                        string[] useridarr = System.Text.RegularExpressions.Regex.Split(response_Login, "data-user-id=");
                        foreach (string useridarr_item in useridarr)
                        {
                            if (useridarr_item.Contains("data-screen-name="))
                            {
                                userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("data-screen-name=") - 3);
                                break;
                            }
                        }
                    }

                    IsLoggedIn = true;

                    Log("[ " + DateTime.Now + " ] => [ Logged in with " + Username + " ]");
                    GetScreen_name(homePage);
                    clsDBQueryManager Db = new clsDBQueryManager();
                    GetFollowercount();
                    Db.InsertScreenNameFollower(Screen_name, FollowerCount,FollwingCount, Username,ProfileFullName);
                    //GetDirectMessageDetails(Username, Password, Screen_name, FollowerCount, FollwingCount);
                }
                else if (response_Login.Contains("error"))
                {
                    //Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                    IsLoggedIn = false;
                    IsNotSuspended = true;
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    return;
                }
                //else if (responseURI.Contains("captcha"))//(globusHttpHelper.gResponse.ResponseUri.ToString().Contains("captcha"))
                else if (response_Login.Contains("captcha"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Asking Captcha with " + Username + " ]");
                    IsLoggedIn = false;
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_AskingCaptchaAccounts);
                    return;
                }
                
                else
                {
                    IsLoggedIn = false;
                    //Log("Login Error in Account : " + Username + ":" + Password);
                }
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ Error in Login : " + Username + " ]");
                Globals.IPNotWorking = true;
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Login() --> " + Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                return;
            }
          
        }

        public static string oauthToken(string data, string paramName)
        {

            string value = string.Empty;
            int startIndx = data.IndexOf(paramName);
            if (startIndx > 0)
            {
                int indexstart = startIndx + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");

                if (value.Contains(paramName))
                {
                    try
                    {
                        string[] getOuthentication = System.Text.RegularExpressions.Regex.Split(data, "\"postAuthenticityToken\":\"");
                        string[] authenticity = Regex.Split(getOuthentication[1], ",");

                        if (authenticity[0].IndexOf("\"") > 0)
                        {
                            int indexStart1 = authenticity[0].IndexOf("\"");
                            string start = authenticity[0].Substring(0, indexStart1);
                            value = start.Replace("\"", "").Replace(":", "");
                        }
                    }
                    catch { };
                }

                return value;
            }
            else
            {
                string[] array = Regex.Split(data, "<input type=\"hidden\"");
                foreach (string item in array)
                {
                    if (item.Contains("oauth_token"))
                    {
                        int startindex = item.IndexOf("value=\"");
                        if (startindex > 0)
                        {
                            string start = item.Substring(startindex).Replace("value=\"", "");
                            int endIndex = start.IndexOf("\"");
                            string end = start.Substring(0, endIndex);
                            value = end;
                            break;
                        }
                    }
                }

            }
            return value;

        }
        bool IsStopAppfollowlogIn = false;
        public static List<Thread> lstThreadStopAppFollowLogIn = new List<Thread>();
        
        public void LoginArabFollow()
        {
            try
            {
                if (IsStopAppfollowlogIn)
                {
                    return;
                }

                lstThreadStopAppFollowLogIn.Add(Thread.CurrentThread);
                lstThreadStopAppFollowLogIn = lstThreadStopAppFollowLogIn.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { } 
            try
            {
                LogArabFollower("[ " + DateTime.Now + " ] => [ Logging in with Account: " + Username + " ]");
                string ts = GenerateTimeStamp();
                string get_twitter_first = string.Empty;
                try
                {
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"),"", IPAddress, (IPPort), IPUsername, IPpassword);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                }

                try
                {
                    postAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }
                catch (Exception ex)
                {
                   // GlobusLogHelper.log.Error("Error" + ex.Message);
                }
                try
                {

                    string get_twitter_second = globusHttpHelper.postFormData(new Uri("https://twitter.com/scribe"), "log%5B%5D=%7B%22event_name%22%3A%22web%3Amobile_gallery%3Agallery%3A%3A%3Aimpression%22%2C%22noob_level%22%3Anull%2C%22internal_referer%22%3Anull%2C%22context%22%3A%22mobile_gallery%22%2C%22event_info%22%3A%22mobile_app_download%22%2C%22user_id%22%3A0%2C%22page%22%3A%22mobile_gallery%22%2C%22_category_%22%3A%22client_event%22%2C%22ts%22%3A" + ts + "%7D", "https://twitter.com/?lang=en&logged_out=1#!/download", "", "", "", "");//globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", string.Empty);

                    string get2nd = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);

                    string get_api = globusHttpHelper.getHtmlfromUrl(new Uri("http://api.twitter.com/receiver.html"), "https://twitter.com/", "");

                }
                catch (Exception ex)
                {
                    //GlobusLogHelper.log.Error("Error" + ex.Message);
                }
                string postData = "session%5Busername_or_email%5D=" + Uri.EscapeDataString(Username) + "&session%5Bpassword%5D=" + Password + "&authenticity_token=" + postAuthenticityToken + "&scribe_log=&redirect_after_login=&authenticity_token=" + postAuthenticityToken + "&remember_me=1";

                string response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/sessions"), postData, "https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);

                if (response_Login.Contains("???? ????? ???????? ????") || response_Login.Contains("???? ???? ?? ???????? ????? ??? ???? ??? ????.") || response_Login.Contains("Help us keep your account safe.") || response_Login.Contains("Verify your identity"))
                {
                    //foreach (var Email in )
                    try
                    {
                        string temp_user_id = string.Empty;
                        string challenge_id = string.Empty;
                        challenge_id = response_Login.Substring(response_Login.IndexOf("name=\"challenge_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"challenge_id\" value=")) - response_Login.IndexOf("name=\"challenge_id\" value="))).Replace("name=\"challenge_id\" value=", string.Empty).Replace("\"", "").Trim();
                        temp_user_id = response_Login.Substring(response_Login.IndexOf("name=\"user_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"user_id\" value=")) - response_Login.IndexOf("name=\"user_id\" value="))).Replace("name=\"user_id\" value=", string.Empty).Replace("\"", "").Trim();
                        if (response_Login.Contains(" name=\"challenge_type\" value=\"RetypeEmail") && response_Login.Contains("@"))
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);
                        }
                        else
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);

                        }
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error("Error" + ex.Message);
                    }
                }
                //else
                //{
                //    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);

                //}


                string homePage = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com"), "", "");

                string responseURI = globusHttpHelper.gResponse.ResponseUri.ToString().ToLower();

                if (response_Login.Contains("signout") || homePage.Contains("signout"))
                {
                    string Apps = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/settings/applications"), "", "");
                    if (Apps.Contains("arab-follow"))
                    {
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Arab-Follow => Login => Already Authorized with Account: " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_AlreadtAuthorizedArabFollowAccounts);
                        return;
                    }
                    postAuthenticityToken = PostAuthenticityToken(response_Login, "postAuthenticityToken");

                    try
                    {
                        int startIndx = response_Login.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                        int endIndx = response_Login.IndexOf("\"", startIndx);
                        userID = response_Login.Substring(startIndx, endIndx - startIndx);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error("Error" + ex.Message);
                    }

                    if (string.IsNullOrEmpty(userID))
                    {
                        userID = string.Empty;
                        string[] useridarr = System.Text.RegularExpressions.Regex.Split(response_Login, "data-user-id=");
                        foreach (string useridarr_item in useridarr)
                        {
                            if (useridarr_item.Contains("data-screen-name="))
                            {
                                userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("data-screen-name=") - 3);
                                break;
                            }
                        }
                    }

                    IsLoggedIn = true;

                    LogArabFollower("[ " + DateTime.Now + " ] => [ Logged in with Account: " + Username + " ]");
                    string arabfollowSignUp = string.Empty;
                    string oauth_token = string.Empty;
                    string arabconnect = string.Empty;
                    try
                    {
                        //arabconnect = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com/connect.php"), IPAddress, Convert.ToInt32(IPPort), IPUsername, IPpassword);
                        arabconnect = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com/connect.php"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        arabconnect = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com/connect.php"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }


                    try
                    {
                        
                        arabfollowSignUp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com/redirect.php"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        arabfollowSignUp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://www.arab-follow.com/redirect.php"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                        Console.Write("Error" + ex.Message);
                    }


                   
                    oauth_token = oauthToken(arabfollowSignUp, "oauth_token");
                   
                    string postDataArab = "authenticity_token=" + postAuthenticityToken + "&" + oauth_token + "";
                    string posturlArab = "https://api.twitter.com/oauth/authenticate";
                    string postresponceArab = string.Empty;

                    string outhResp = string.Empty;
                    try
                    {
                        
                        outhResp = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://api.twitter.com/oauth/authenticate?" + oauth_token + "&force_login =true"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Error" + ex.Message);
                    }


                    try
                    {
                        postresponceArab = globusHttpHelper.postFormData(new Uri(posturlArab), postDataArab, "https://api.twitter.com/oauth/authenticate?" + oauth_token + "", string.Empty, string.Empty, string.Empty, "https://api.twitter.com");
                          

                    }
                    catch (Exception ex)
                    {
                        Console.Write("Error" + ex.Message);

                    }

                    string oauth_verifier = string.Empty;

                    oauth_verifier = oauthToken(postresponceArab, "oauth_verifier");

                    string twitAppResp = string.Empty;
                    try
                    {
                        
                        twitAppResp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com//callback.php?" + oauth_token + "&" + oauth_verifier + ""), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                         twitAppResp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com//callback.php?" + oauth_token + "&" + oauth_verifier + ""), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                         Console.Write("Error" + ex.Message);
                    }

                    string finalresponce = string.Empty;
                    try
                    {
                        //finalresponce = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://arab-follow.com//clearsessions.php"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                    }


                    if (twitAppResp.Contains("logout"))
                    {
                        //GlobusLogHelper.log.Info("[ Social Sites => Twitter => Arab-Follow => Login => Authorized with Account: " + Username + " ]");
                        //GlobusLogHelper.log.Debug("[ Social Sites => Twitter => Arab-Follow => Login => Authorized in with Account: " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowedArabAccounts);
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Arab-Follow => Login => Authorized finished with Account: " + Username + " ]");


                    }
                    else 
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" +Password + ":"  + ":" + IPAddress + ":" + IPPort + ":" +IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Arab-Follow => Login => Authorized failed with Account: " + Username + " ]");

                    }
                }
                else if (responseURI.Contains("error"))//(globusHttpHelper.gResponse.ResponseUri.ToString().Contains("error"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                    IsLoggedIn = false;
                   // GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, TDGlobals.path_FailedLoginAccounts);
                    return;
                }
                else if (responseURI.Contains("captcha"))//(globusHttpHelper.gResponse.ResponseUri.ToString().Contains("captcha"))
                {
                    //GlobusLogHelper.log.Info("[ Social Sites => Twitter => Login => Asking Captcha with " + Username + " ]");
                    //GlobusLogHelper.log.Debug("[ Social Sites => Twitter => Login => Asking Captcha with " + Username + " ]");
                    IsLoggedIn = false;
                    //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, TDGlobals.path_AskingCaptchaAccounts);
                    return;
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                   GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);
                   GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    IsLoggedIn = false;
                    //Log("Login Error in Account : " + Username + ":" + Password);
                }
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    
                
                return;
            }

            //GlobusLogHelper.log.Debug("[ Social Sites => Twitter => Arab-Follow => Sign In => PROCESS COMPLETED ! WITH : " + Username + " ]");
            //GlobusLogHelper.log.Info("[ Social Sites => Twitter => Arab-Follow => Sign In => PROCESS COMPLETED ! WITH : " + Username + " ]");

        }



        public void LoginApplicationFollow()
        {
            try
            {
                if (IsStopAppfollowlogIn)
                {
                    return;
                }

                lstThreadStopAppFollowLogIn.Add(Thread.CurrentThread);
                lstThreadStopAppFollowLogIn = lstThreadStopAppFollowLogIn.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { }
            try
            {
                LogArabFollower("[ " + DateTime.Now + " ] => [ Logging in with Account: " + Username + " ]");
                string ts = GenerateTimeStamp();
                string get_twitter_first = string.Empty;
                try
                {
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), "", IPAddress, (IPPort), IPUsername, IPpassword);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                }

                try
                {
                    postAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }
                catch (Exception ex)
                {
                    // GlobusLogHelper.log.Error("Error" + ex.Message);
                }
               
                string postData = "session%5Busername_or_email%5D=" + Uri.EscapeDataString(Username) + "&session%5Bpassword%5D=" + Password + "&authenticity_token=" + postAuthenticityToken + "&scribe_log=&redirect_after_login=&authenticity_token=" + postAuthenticityToken + "&remember_me=1";

                string response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/sessions"), postData, "https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);

                if (response_Login.Contains("अपनी पहचान सत्यापित करें") || response_Login.Contains("आपके खाते को सुरक्षित रखेने में हमें मदद करें.") || response_Login.Contains("Help us keep your account safe.") || response_Login.Contains("Verify your identity") || response_Login.Contains("account/login_challenge?"))
                {
                    //foreach (var Email in )
                    try
                    {
                        string temp_user_id = string.Empty;
                        string challenge_id = string.Empty;
                        challenge_id = response_Login.Substring(response_Login.IndexOf("name=\"challenge_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"challenge_id\" value=")) - response_Login.IndexOf("name=\"challenge_id\" value="))).Replace("name=\"challenge_id\" value=", string.Empty).Replace("\"", "").Trim();
                        temp_user_id = response_Login.Substring(response_Login.IndexOf("name=\"user_id\" value="), (response_Login.IndexOf("/>", response_Login.IndexOf("name=\"user_id\" value=")) - response_Login.IndexOf("name=\"user_id\" value="))).Replace("name=\"user_id\" value=", string.Empty).Replace("\"", "").Trim();
                        if (response_Login.Contains(" name=\"challenge_type\" value=\"RetypeEmail") && response_Login.Contains("@"))
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);
                        }
                        else
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&remember_me=true", IPAddress, IPPort, IPUsername, IPpassword);

                        }
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error("Error" + ex.Message);
                    }
                }
                
                string homePage = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com"), "", "");

                string responseURI = globusHttpHelper.gResponse.ResponseUri.ToString().ToLower();

                if (response_Login.Contains("signout") || homePage.Contains("signout"))
                {
                    string Apps = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/settings/applications"), "", "");
                    if (Apps.Contains("تطبيق ريتويتر الجديد"))
                    {
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Application-Follow => Login => Already Authorized with Account: " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_AlreadtAuthorizedApplicationFollowAccounts);
                        return;
                    }
                    postAuthenticityToken = PostAuthenticityToken(response_Login, "postAuthenticityToken");

                    try
                    {
                        int startIndx = response_Login.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                        int endIndx = response_Login.IndexOf("\"", startIndx);
                        userID = response_Login.Substring(startIndx, endIndx - startIndx);
                    }
                    catch (Exception ex)
                    {
                        //GlobusLogHelper.log.Error("Error" + ex.Message);
                    }

                    if (string.IsNullOrEmpty(userID))
                    {
                        userID = string.Empty;
                        string[] useridarr = System.Text.RegularExpressions.Regex.Split(response_Login, "data-user-id=");
                        foreach (string useridarr_item in useridarr)
                        {
                            if (useridarr_item.Contains("data-screen-name="))
                            {
                                userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("data-screen-name=") - 3);
                                break;
                            }
                        }
                    }

                    IsLoggedIn = true;

                    LogArabFollower("[ " + DateTime.Now + " ] => [ Logged in with Account: " + Username + " ]");
                    string arabfollowSignUp = string.Empty;
                    string oauth_token = string.Empty;
                    string arabconnect = string.Empty;

                    LogArabFollower("[ " + DateTime.Now + " ] => [ Authorization process start with Account: " + Username + " ]");
                    try
                    {
                        
                        arabconnect = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://www.twitterrt.com/login"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(1000);
                        arabconnect = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://www.twitterrt.com/login"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }

                    oauth_token = oauthToken(arabconnect, "oauth_token");

                    string postDataArab = "authenticity_token=" + postAuthenticityToken + "&" + oauth_token + "";
                    string posturlArab = "https://twitter.com/oauth/authorize";
                    string postresponceArab = string.Empty;

                    string outhResp = string.Empty;
                   
                    try
                    {
                        postresponceArab = globusHttpHelper.postFormData(new Uri(posturlArab), postDataArab, "https://twitter.com/oauth/authenticate?" + oauth_token + "", string.Empty, string.Empty, string.Empty, "https://api.twitter.com");

                    }
                    catch (Exception ex)
                    {
                        Console.Write("Error" + ex.Message);

                    }

                    string oauth_verifier = string.Empty;

                    oauth_verifier = oauthToken(postresponceArab, "oauth_verifier");

                    string twitAppResp = string.Empty;
                    try
                    {
                    
                        twitAppResp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://www.twitterrt.com/login?" + oauth_token + "&" + oauth_verifier + ""), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                    }
                    catch (Exception ex)
                    {
                        twitAppResp = globusHttpHelper.getHtmlfromUrlIP(new Uri("http://www.twitterrt.com/login?" + oauth_token + "&" + oauth_verifier + ""), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);
                        Console.Write("Error" + ex.Message);
                    }

                    string finalresponce = string.Empty;
                   
                    if (twitAppResp.Contains("logout"))
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowedApplicationAccounts);
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Appliation-Follow => Login => Authorized Successfully finished with Account: " + Username + " ]");

                    }
                    else
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedApplicationAccounts);
                        LogArabFollower("[ " + DateTime.Now + " ] => [ Appliation-Follow => Login => Authorized failed with Account: " + Username + " ]");

                    }
                }
                else if (responseURI.Contains("error"))
                {
                    Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                    IsLoggedIn = false;
                    
                    return;
                }
                else if (responseURI.Contains("captcha"))//(globusHttpHelper.gResponse.ResponseUri.ToString().Contains("captcha"))
                {
                    IsLoggedIn = false;
                   
                    return;
                }
                else
                {
                    LogArabFollower("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedApplicationAccounts);
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    IsLoggedIn = false;
                    //Log("Login Error in Account : " + Username + ":" + Password);
                }
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedArabAccounts);
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyNotFollowedApplicationAccounts);
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);

                return;
            }

           
        }


        public void GetDirectMessageDetails(string userName, string password,string screenName,string FollowerCount,string FollowingCount)
        {
            try
            {
                int count = 0;
                clsDBQueryManager Db = new clsDBQueryManager();
                DataSet ds = Db.InsertorUpdateUserDetailsForDirectMessaging("", "", "", userName, password);
                DataTable dt = ds.Tables["tb_AccountSendDirectMessage"];
                if (dt==null)
                {
                    //find count value;
                    //string followercount = dt.Rows[0]["FollowerCount"].ToString();
                    //update
                    Db.UpdateUserDetailsForDirectMessaging(Screen_name, FollowerCount, FollowingCount, userName, password);
                    count = 0;
                }
                else
                { 
                    //select.
                    string tempfollowercount = dt.Rows[0]["FollowerCount"].ToString();
                    Db.UpdateUserDetailsForDirectMessaging(Screen_name, FollowerCount, FollowingCount, userName, password);
                    
                     count = Convert.ToInt32(tempfollowercount) - Convert.ToInt32(FollowerCount);
                     if (count > 0)
                     { 
                        
                     }

                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }


        public void GetScreen_name(string homePage)
        {
            try
            {
                //string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");
                int startIndex = homePage.IndexOf("data-screen-name");
                string start = homePage.Substring(startIndex).Replace("data-screen-name=\"", "");
                int endIndex = start.IndexOf("\"");
                string end = start.Substring(0, endIndex);
                Screen_name = end;
            }
            catch (Exception ex)
            {

            }

            try
            {
                //string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");
                int startIndex = homePage.IndexOf("<b class=\"fullname\">");
                string start = homePage.Substring(startIndex).Replace("<b class=\"fullname\">", "");
                int endIndex = start.IndexOf("</b>");
                string end = start.Substring(0, endIndex);
                ProfileFullName = end;
            }
            catch (Exception ex)
            {

            }
        }

        public void GetFollowercount()
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                GlobusRegex rgx = new GlobusRegex();
                string URL = string.Empty;

                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");

                URL = globusHttpHelper.gResponse.ResponseUri.AbsoluteUri;


                if (PageSource.Contains("btn resend-confirmation-email-link"))
                {
                    Isnonemailverifiedaccounts = true;
                }

               if (!PageSource.Contains("Account suspended") && !PageSource.Contains("currently suspended") && !URL.Contains("https://twitter.com/account/suspended") && !PageSource.Contains("account-suspended"))
                {
                    //int indexStart = PageSource.IndexOf("data-nav=\"followers\"");
                    //string start = PageSource.Substring(indexStart).Replace("data-nav=\'followers\'", "");
                    //int indexEnd = start.IndexOf("</strong>");
                    //string end = start.Substring(0, indexEnd).Replace("<strong>", "").Replace(">", "").Replace("\n", "").Replace(" ", "");
                    if (PageSource.Contains("data-element-term=\"follower_stats"))
                    {
                        try
                        {
                            int indexStart = PageSource.IndexOf("data-element-term=\"follower_stats");
                            string start = PageSource.Substring(indexStart).Replace("data-element-term=\"follower_stats", "");
                            int indexEnd = start.IndexOf("</strong>");
                            string end = start.Substring(0, indexEnd).Replace("<strong>", "").Replace(">", "").Replace("\n", "").Replace(" ", "");
                            if (end.Contains("<strongtitle="))
                            {
                                try
                                {
                                    end = end.Split('\"')[6].Replace("strongtitle=", "");
                                }
                                catch { }
                            }

                            FollowerCount = rgx.StripTagsRegex(end).Replace("data-nav=\"followers\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Trim();

                        }
                        catch { }

                        try
                        {
                            int indexStart1 = PageSource.IndexOf("data-element-term=\"following_stats");
                            string start1 = PageSource.Substring(indexStart1).Replace("data-element-term=\"following_stats", "");
                            int indexEnd1 = start1.IndexOf("</strong>");
                            string end1 = start1.Substring(0, indexEnd1).Replace("<strong>", "").Replace(">", "").Replace("\n", "").Replace(" ", "");
                            if (end1.Contains("<strongtitle="))
                            {
                                try
                                {
                                    end1 = end1.Split('\"')[6].Replace("strongtitle=", "");
                                }
                                catch { }
                            }
                            FollwingCount = rgx.StripTagsRegex(end1).Replace("data-nav=\"following\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Trim();

                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            int indexStart = PageSource.IndexOf("ProfileNav-item ProfileNav-item--following");
                            string start = PageSource.Substring(indexStart).Replace("ProfileNav-item ProfileNav-item--following", "");
                            int indexEnd = start.IndexOf("data-nav=\"following");
                            string end = start.Substring(0, indexEnd).Replace("data-nav=\"following", "").Replace("<a class=\"ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav u-textUserColor\" title=", "").Replace("Following", "").Replace(",", "").Replace("\n", "").Replace("\"", "").Replace(">", "").Trim();
                            FollwingCount = rgx.StripTagsRegex(end).Replace("data-nav=\"followers\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("following", string.Empty).Trim();
                        }
                        catch { }


                        try
                        {
                            int indexStart = PageSource.IndexOf("ProfileNav-item ProfileNav-item--follower");
                            string start = PageSource.Substring(indexStart).Replace("ProfileNav-item ProfileNav-item--follower", "");
                            int indexEnd = start.IndexOf("data-nav=\"followers");
                            string end = start.Substring(1, indexEnd).Replace("data-nav=\"followers", "").Replace("<a class=\"ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav u-textUserColor\" title=", "").Replace("followers", "").Replace(",", "").Replace("\n", "").Replace("\"", "").Replace(">", "").Replace("Followers", "").Trim();

                            FollowerCount = rgx.StripTagsRegex(end).Replace("data-nav=\"followers\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("followers", string.Empty).Trim();
                            if(FollowerCount.Contains(" "))
                            {
                                FollowerCount = FollowerCount.Split(' ')[0].Trim();
                            }
                        }
                        catch { }
                    }

                    AccountStatus = "Active";
                    IsNotSuspended = true;
                }
                else if (PageSource.Contains("Account suspended") || URL.Contains("https://twitter.com/account/suspended") || PageSource.Contains("account-suspended"))
                {
                    Log("[ " + DateTime.Now + " ] => [ " + Username + " - Account Suspended ] ");
                    //reminveSuspendedAccounts(Username);
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(Username);
                    AccountStatus = "Account Suspended";
                    IsNotSuspended = false;
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuspendedEmailAccounts);

                }
            }
            catch (Exception ex)
            {

            }
        }

        public void reminveSuspendedAccounts(string RemoveUsername)
        {
            try
            {
                TweetAccountContainer.dictionary_TweetAccount.Remove(RemoveUsername);
            }
            catch (Exception)
            {
            }
        }

        public void UpdateProfile(string profileUsername, string profileLocation, string profileURL, string profileDescription, string localImagePath)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }

                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {
                        if (profileUpdater.UpdateProfile(profileUsername, profileLocation, profileURL, profileDescription, localImagePath, postAuthenticityToken, ref globusHttpHelper))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Profile Updated : " + Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyProfiledAccounts);
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [Unable to Update Profile : " + Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToProfileAccounts);
                        }
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ >>Couldn't Login with >> " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Account " + Username + " is suspended. ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuspendedEmailAccounts);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- UpdateProfile() -- " + Username + " --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  UpdateProfile() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            finally
            {
                AccountCount++;
                if (AccountCount >= TotalEmailsUploded)
                {
                    Log("[ " + DateTime.Now + " ] => [ Finished Profile Updation For " + Username + " ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        public void BackgroundPic(string localImagePath)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {
                        if (profileUpdater.UpdateBackgroundImage(localImagePath, postAuthenticityToken, ref globusHttpHelper))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Backgroud Image Updated : " + Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyProfiledAccounts);
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Unable to Update Background Image : " + Username + " ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToProfileAccounts);
                        }
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ >>Couldn't Login with >> " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ >> Account is suspended >> " + Username + " ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- BackgroundPic() -- " + Username + " --> " + ex.Message, Globals.Path_ProfileManagerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  BackgroundPic() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            finally
            {
                AccountCount++;
                if (AccountCount > TotalEmailsUploded)
                {
                    Log("[ " + DateTime.Now + " ] => [ Finished Background Pic Creation ]");
                    Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        #region Commented Follow
        //public void FollowUsingURLs(string user_id_toFollow)
        //{
        //    //Login();

        //    //if (true)
        //    try
        //    {
        //        if (IsLoggedIn)
        //        {
        //            string followStatus;

        //            follower.FollowUsingProfileID(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, out followStatus);

        //            if (followStatus == "followed")
        //            {
        //                Log(">> Followed >> " + user_id_toFollow + " by " + Username);
        //                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
        //            }
        //            else
        //            {
        //                Log(">>Couldn't Follow >> " + user_id_toFollow + " by " + Username);
        //                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.AddToErrorLogText(ex.Message);
        //    }
        //} 
        #endregion

        #region FollowUsingURLs Commented 3-7-12
        //public void FollowUsingURLs(List<string> list_user_id_toFollow , int minDelay , int maxDelay , bool OtherUser)
        //{
        //    //Login();

        //    if (!IsLoggedIn)
        //    {
        //        Login();
        //    }
        //    int counter = 0;
        //    if (IsLoggedIn)
        //    {
        //        //list_followed = Select from "tb_Follow" where user='email' 
        //        //list_user_id_toFollow = list_user_id_toFollow.Except(list_followed);
        //        clsDBQueryManager queryManager = new clsDBQueryManager();

        //        List<string> lst_AlreadyFollowings = queryManager.SelectFollowData_List(Username);

        //        //list_user_id_toFollow = list_user_id_toFollow.Except(lst_AlreadyFollowings).ToList();

        //        if (list_user_id_toFollow.Count > 0 && !string.IsNullOrEmpty(list_user_id_toFollow[0])) 
        //        {
        //            foreach (string user_id_toFollow in list_user_id_toFollow)
        //            {
        //                string followStatus;
        //                string user_id = string.Empty;
        //                string pagesource = string.Empty;
        //                ///Added By Gargi Mishra 1st June 2012
        //                ///Getting Username from User id
        //                ///Working from both username n id
        //                if (!GlobusRegex.ValidateNumber(user_id_toFollow))//(!IsItNumber(user_id_toFollow))
        //                {
        //                    user_id = TwitterDataScrapper.GetUserIDFromUsername(user_id_toFollow);
        //                }
        //                else
        //                {
        //                    user_id = user_id_toFollow;
        //                }
        //                //lst_AlreadyFollowings.Exists(s => (s == user_id_toFollow));
        //                bool isAlreadyFollowed = lst_AlreadyFollowings.Exists(s => (s == user_id));

        //                if (!isAlreadyFollowed)
        //                {
        //                    int delay = 10 * 1000;

        //                    try
        //                    {
        //                        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
        //                    }
        //                    catch { }



        //                    follower.FollowUsingProfileID(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id, out followStatus);


        //                    if (followStatus == "followed")
        //                    {
        //                        Log(">> Followed >> " + user_id_toFollow + " by " + Username);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);

        //                        //Insert Update in "tb_Follow"
        //                        queryManager.InsertUpdateFollowTable(Username, user_id, "");
        //                    }
        //                    else
        //                    {
        //                        Log(">> Couldn't Follow >> " + user_id_toFollow + " by " + Username);
        //                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
        //                    }

        //                    Log("Follow Delayed for " + delay + " Seconds");
        //                    Thread.Sleep(delay * 1000);

        //                    if (OtherUser)
        //                    {
        //                        counter++;
        //                        if (counter == noOFFollows)
        //                        {
        //                            break;
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    Log("Already Followed : " + user_id_toFollow + " with : " + Username);
        //                }
        //            }

        //            Log("Finished Following with : " + Username);
        //        }
        //        else
        //        {
        //            Log("Sorry No User To Follow");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        Log(">> Couldn't Login with>> " + Username);
        //    }
        //} 
        #endregion


        #region old commented code FollowUsingURLs
        //public void FollowUsingURLs(List<string> list_user_id_toFollow, int minDelay, int maxDelay, bool OtherUser)
        //{
        //    int counter = 0;
        //    int counterChecktotlaFollowr = 0;
        //    try
        //    {
        //        //Login();
        //        try
        //        {
        //            DataSet Dataset = new DataSet();
        //            if (NoOfFollowPerDay_ChkBox == true)
        //            {
        //                Dataset = CheckLimitationOfPerID(Username);
        //                if (Dataset != null)
        //                {
        //                    if (Dataset.Tables.Count != 0)
        //                    {
        //                        if (Dataset.Tables[0].Rows.Count != 0)
        //                        {
        //                            Log("[ " + DateTime.Now + " ] => [ No Of Follow Per Day - " + NoOfFollowPerDay + " ]");
        //                            int DataSetTableRowsCount = Dataset.Tables[0].Rows.Count;
        //                            RemainingNoOfFollowPerDay = NoOfFollowPerDay - DataSetTableRowsCount;
        //                        }
        //                        else
        //                        {
        //                            RemainingNoOfFollowPerDay = NoOfFollowPerDay;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        RemainingNoOfFollowPerDay = NoOfFollowPerDay;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                RemainingNoOfFollowPerDay = TweetAccountManager.NoOfFollowPerDay;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- NoOfFollowPerDay_ChkBox --> " + ex.Message, Globals.Path_FollowerErroLog);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() --  " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
        //        }

        //        ///Login if Not Logged in
        //        if (!IsLoggedIn)
        //        {
        //            Login();
        //        }

        //        ///Return if Suspended
        //        if (AccountStatus == "Account Suspended")
        //        {
        //            Log("[ " + DateTime.Now + " ] => [  " + Username + " : Suspended ]");
        //            return;
        //        }

               
        //        if (IsLoggedIn)
        //        {
        //            #region trial last tweet date code
        //            //if (UseDateLastTweeted)
        //            //{
        //            //    string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");
        //            //    string[] Array = Regex.Split(PageSource, "href=");

        //            //    foreach (string item in Array)
        //            //    {
        //            //        if (item.Contains(Screen_name + "/status/") && item.Contains("tweet-timestamp js-permalink js-nav"))
        //            //        {
        //            //            try
        //            //            {
        //            //                int startIndex = item.IndexOf("title=\"");
        //            //                string Start = item.Substring(startIndex).Replace("title=\"", "");
        //            //                int endIndex = Start.IndexOf("\" >");
        //            //                string End = Start.Substring(0, endIndex);

        //            //                string[] array = Regex.Split(End, " ");
        //            //                DateTime lastTweetDate = new DateTime(Convert.ToInt32(array[5]), 08, Convert.ToInt32(array[3]), 12, 12, 12);

        //            //                DateTime dt_Now = DateTime.Now;

        //            //                TimeSpan dt_Difference = lastTweetDate.Subtract(dt_Now);

        //            //                TimeSpan t = dt_Now - lastTweetDate;
        //            //                double NrOfDays = t.TotalDays;

        //            //                if (dt_Difference.Days >= LastTweetDays)
        //            //                {
        //            //                    Log("Not Followed as Last Tweeted Day is greated than specified For : " + Username);
        //            //                    continue;
        //            //                }

        //            //            }
        //            //            catch (Exception ex)
        //            //            {

        //            //            }
        //            //        }
        //            //    }
        //            //} 
        //            #endregion

        //            int NoOfFollowCounter = 0;

        //            clsDBQueryManager queryManager = new clsDBQueryManager();

        //            TwitterDataScrapper dataScrapper = new TwitterDataScrapper();

        //            ///Get list of Already Followings
        //            List<string> lst_AlreadyUserid = new List<string>(); ;
        //            List<string> lst_AlreadyUserName = new List<string>();
        //            if (list_user_id_toFollow.Count > 0 && !string.IsNullOrEmpty(list_user_id_toFollow[0]))
        //            {
        //                foreach (string user_id_toFollow in list_user_id_toFollow)
        //                {
        //                    if (NoOfFollowPerDay_ChkBox)
        //                    {
        //                        if (NoOfFollowCounter >= RemainingNoOfFollowPerDay)
        //                        {
        //                            Log("[ " + DateTime.Now + " ] => [ Finish Follow Limit " + NoOfFollowPerDay + " Today ]");
        //                            break;
        //                        }
        //                    }

        //                    string followStatus;
        //                    string user_id = string.Empty;
        //                    string pagesource = string.Empty;
        //                    string Screen_name = string.Empty;
        //                    ///Getting Username from User id
        //                    ///Working from both username n id
        //                    clsDBQueryManager DB = new clsDBQueryManager();

        //                    #region commented for choosing username and userid
        //                    //if (!GlobusRegex.ValidateNumber(user_id_toFollow))//(!IsItNumber(user_id_toFollow))
        //                    //if (frmMain_NewUI.IsFollowerScreanName)
        //                    //{
        //                    //    DataSet ds = DB.GetUserId(user_id_toFollow);
        //                    //    if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
        //                    //    {
        //                    //        foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
        //                    //        {
        //                    //            user_id = dataRow.ItemArray[0].ToString();
        //                    //        }
        //                    //    }
        //                    //}
        //                    //else if (frmMain_NewUI.IsfollowerUserId)
        //                    //{
        //                    //    user_id = user_id_toFollow;
        //                    //    DataSet ds = DB.GetUserName(user_id_toFollow);
        //                    //    if (ds.Tables[0].Rows.Count > 0)
        //                    //    {
        //                    //        foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
        //                    //        {
        //                    //            user_id = dataRow.ItemArray[0].ToString();
        //                    //        }
        //                    //    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    user_id = string.Empty;
        //                    //    Log("Please enter Valid user Id Or Screan name.  ");
        //                    //} 
        //                    #endregion

        //                    #region get check unfollow befor
        //                    if (TweetAccountManager.UseUnfollowedBeforeFilter)
        //                    {
        //                        List<string> lst_AlreadyExist = new List<string>();
        //                        if (frmMain_NewUI.IsFollowerScreenName)
        //                        {
        //                            lst_AlreadyExist = DB.SelectUnFollowDUsernameID_List(Username);
        //                            bool isAlreadyExist = lst_AlreadyExist.Exists(s => (s == user_id_toFollow));
        //                            if (isAlreadyExist)
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]");
        //                                continue;
        //                            }
        //                        }
        //                        else if (frmMain_NewUI.IsfollowerUserId && BaseLib.NumberHelper.ValidateNumber(user_id_toFollow))
        //                        {
        //                            lst_AlreadyExist = DB.SelectUnFollowedUsername_List(Username);
        //                            bool isAlreadyExist = lst_AlreadyExist.Exists(s => (s == user_id_toFollow));
        //                            if (isAlreadyExist)
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]");
        //                                continue;
        //                            }
        //                        }
        //                    }

        //                    #endregion

        //                    ///Counter For Follow
        //                    NoOfFollowCounter++;

        //                    //Check if user_id_toFollow is already being followed or not

        //                    #region user_id_toFollow is already being followed or not

        //                    bool isAlreadyFollowed = false;
        //                    Log("[ " + DateTime.Now + " ] => [ Checking For Already Followed For " + user_id_toFollow + " From " + Username + " ]");
        //                    if (frmMain_NewUI.IsFollowerScreenName)
        //                    {
        //                        lst_AlreadyUserName = queryManager.SelectFollowDUsername_List(Username);
        //                        isAlreadyFollowed = lst_AlreadyUserName.Exists(s => (s == user_id_toFollow));
        //                        Screen_name = user_id_toFollow;
        //                    }
        //                    else if (frmMain_NewUI.IsfollowerUserId && BaseLib.NumberHelper.ValidateNumber(user_id_toFollow))
        //                    {
        //                        lst_AlreadyUserid = queryManager.SelectFollowDUsernameID_List(Username);
        //                        isAlreadyFollowed = lst_AlreadyUserid.Exists(s => (s == user_id_toFollow));
        //                        user_id = user_id_toFollow;
        //                    }
        //                    else
        //                    {
        //                        Log("[ " + DateTime.Now + " ] => [ UserId/ScreenName Not In Correct Format :- " + user_id_toFollow + " ]");
        //                        return;
        //                    }

        //                    #endregion


        //                    if (!isAlreadyFollowed)  //If not already being followed, follow now
        //                    {
        //                        ///Use FollowingsFollowers Ratio Filter
        //                        if (UseRatioFilter)
        //                        {
        //                            int FollowingsFollowersRatio_user_id = 0;

        //                            //Check FollowingsFollowers Ratio of this user_id
        //                            try
        //                            {
        //                                string returnstatusFollower = string.Empty;
        //                                string returnstatusFollowing = string.Empty;
        //                                List<string> Following = dataScrapper.GetFollowings(user_id, out returnstatusFollowing);
        //                                List<string> Follower = dataScrapper.GetFollowers(user_id, out returnstatusFollower);
        //                                int count_Followings_user_id = Following.Count;
        //                                int count_Followers_user_id = Follower.Count;

        //                                FollowingsFollowersRatio_user_id = (count_Followings_user_id * 100) / count_Followers_user_id;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- UseRatioFilter --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- UseRatioFilter -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
        //                            }

        //                            if (!(FollowingsFollowersRatio_user_id >= FollowingsFollowersRatio)) //If FollowingsFollowersRatio_user_id is less than Required, continue with next user_id_toFollow
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ Not Followed as FollowingsFollowersRatio : " + FollowingsFollowersRatio_user_id + " for " + user_id_toFollow + " with : " + Username + " ]");
        //                                continue;
        //                            }
        //                        }


        //                        #region Old Unfollowed Before Code
        //                        //if (UseUnfollowedBeforeFilter)
        //                        //{
        //                        //    try
        //                        //    {
        //                        //        List<string> lst_Followings_Current = dataScrapper.GetFollowings(user_id);

        //                        //        //lst_AlreadyFollowings
        //                        //    }
        //                        //    catch { }
        //                        //} 
        //                        #endregion

        //                        #region Old Tweet Last Day Code
        //                        if (UseDateLastTweeted && !string.IsNullOrEmpty(user_id))
        //                        {
        //                            try
        //                            {
        //                                string strLastTweetDate = dataScrapper.GetUserLastTweetDate(user_id);

        //                                DateTime dt_LastTweetDate = DateTime.Parse(strLastTweetDate);

        //                                DateTime dt_Now = DateTime.Now;

        //                                TimeSpan dt_Difference = dt_Now.Subtract(dt_LastTweetDate);

        //                                if (dt_Difference.Days >= LastTweetDays)
        //                                {
        //                                    Log("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]");
        //                                    continue;
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- Last Tweeted --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- UseDateLastTweeted -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
        //                            }
        //                        }
        //                        #endregion

        //                        #region Check Profile Picture
        //                        if (IscontainPicture)
        //                        {
        //                            Log("[ " + DateTime.Now + " ] => [ Checking Profile Image For : " + user_id_toFollow + " ]");
        //                            string containsIamge = TwitterDataScrapper.GetPhotoFromUsername_New(Screen_name);
        //                            Thread.Sleep(1000);
        //                            if (containsIamge == "true")
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Contains Image ]");
        //                            }
        //                            else if (containsIamge == "false")
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Not Contains Image So Not Following ]");
        //                                continue;
        //                            }
        //                            else if (containsIamge == "Rate limit exceeded")
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ Cannot Make Request. Rate limit exceeded ]");
        //                                Log("[ " + DateTime.Now + " ] => [ Please Try After Some Time ]");
        //                                Thread.Sleep(5000);
        //                            }
        //                        }
        //                        #endregion

        //                        #region Checking Tweet in the last xx days.

        //                        if (IsTweetedInXdays)
        //                        {
        //                            Log("[ " + DateTime.Now + " ] => [checking tweeted in the last  " + daysTweetedInxdays + "  days for User :" + user_id_toFollow + " ]");
        //                            try
        //                            {
        //                                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id_toFollow), "", "");
        //                                string dateoflastTweet = globusHttpHelper.getBetween(PageSource, "ProfileTweet-timestamp js-permalink js-nav js-tooltip", ">").Replace("\"","").Trim();
        //                                try
        //                                {
        //                                    dateoflastTweet = dateoflastTweet.Split('-')[1].Trim();
        //                                    DateTime dt_LastTweetDate = DateTime.Parse(dateoflastTweet);

        //                                    DateTime dt_Now = DateTime.Now;

        //                                    TimeSpan dt_Difference = dt_Now.Subtract(dt_LastTweetDate);
        //                                    if (dt_Difference.Days >= Convert.ToInt32(daysTweetedInxdays))
        //                                    {
        //                                        Log("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]");
        //                                        continue;
        //                                    }
        //                                }
        //                                catch { }
                                        

        //                            }
        //                            catch (Exception ex)
        //                            {

        //                            }
        //                        } 

        //                        #endregion


        //                        int delay = 10 * 1000;
        //                        int delay1 = 10 * 1000;
        //                        try
        //                        {
        //                            if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
        //                            {
        //                                if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
        //                                {
        //                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
        //                                    delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
        //                                }
        //                                else
        //                                {
        //                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- delay --> " + ex.Message, Globals.Path_FollowerErroLog);
        //                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- delay -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
        //                        }

        //                        if (Globals.IsCampaign)
        //                        {
        //                            lock (locker_que_Follower)
        //                            {
        //                                if (string.IsNullOrEmpty(user_id))
        //                                {
        //                                    try
        //                                    {
        //                                        string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id), "", "");
        //                                        int startindex = PageSource.IndexOf("data-user-id=\"");
        //                                        string start = PageSource.Substring(startindex).Replace("data-user-id=\"", "");
        //                                        int endindex = start.IndexOf("\"");
        //                                        string end = start.Substring(0, endindex);
        //                                        user_id = end;
        //                                    }
        //                                    catch (Exception ex)
        //                                    {

        //                                    }
        //                                }
        //                                if (string.IsNullOrEmpty(Screen_name))
        //                                {
        //                                    try
        //                                    {
        //                                        string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/account/redirect_by_id?id=" + user_id), "", "");

        //                                        int startindex = PageSource.IndexOf("user-style-");
        //                                        string start = PageSource.Substring(startindex).Replace("user-style-", "");
        //                                        int endindex = start.IndexOf("\"");
        //                                        string end = start.Substring(0, endindex);
        //                                        Screen_name = end;
        //                                    }
        //                                    catch (Exception ex)
        //                                    {

        //                                    }
        //                                }

        //                                string strQuery = "Select * from tb_CampaignReport Where Follower = '" + user_id + "' OR ScreenName = '" + Screen_name + "' AND Campaign_Name = '" + Globals.Campaign_Name + "' ";
        //                                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_CampaignReport");
        //                                if (ds.Tables[0].Rows.Count > 0 && Globals.IsCampaign)
        //                                {
        //                                    Log("[ " + DateTime.Now + " ] => [ Already Followed Userid : " + user_id + " >>>> ScreenName : " + Screen_name + " ]");
        //                                }
        //                                else
        //                                {
        //                                    follower.FollowUsingProfileID_New(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, out followStatus);
        //                                    string Screen_Name = string.Empty;
        //                                    string userid = string.Empty;
        //                                    if (NumberHelper.ValidateNumber(user_id))
        //                                    {
        //                                        userid = user_id;
        //                                    }
        //                                    else
        //                                    {
        //                                        Screen_name = user_id;
        //                                    }
        //                                    Log("[ " + DateTime.Now + " ] => [ Followed Userid : " + user_id + " >>>> ScreenName : " + Screen_name + " ]");
        //                                    Log("[ " + DateTime.Now + " ] => [ Adding Follower in DataBase ]");
        //                                    string query = "INSERT INTO tb_CampaignReport (Campaign_Name, UserName, Follower , ScreenName) VALUES ('" + Globals.Campaign_Name + "', '" + Username + "', '" + user_id + "' , '" + Screen_name + "')";
        //                                    DataBaseHandler.InsertQuery(query, "tb_CampaignReport");
        //                                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id + ":" + Screen_name + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            follower.FollowUsingProfileID_New(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, out followStatus);
        //                            if (followStatus == "followed")
        //                            {
        //                                Globals.totalcountFollower++;
        //                                counterChecktotlaFollowr++;
        //                                Log("[ " + DateTime.Now + " ] => [ >> Followed >> " + user_id_toFollow + " by " + Username + " ]");

        //                                //Adding in text File FOr SuccessFull Paths
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id_toFollow + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
        //                                try
        //                                {
        //                                    string username = string.Empty;
        //                                    string userid = string.Empty;
        //                                    if (NumberHelper.ValidateNumber(user_id_toFollow))
        //                                    {
        //                                        username = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUserNameFromUserId(user_id_toFollow);
        //                                        userid = user_id_toFollow;
        //                                    }
        //                                    else
        //                                    {
        //                                        string outStatus = string.Empty;
        //                                        userid = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUserIDFromUsername(user_id_toFollow, out outStatus);
        //                                        username = user_id_toFollow;
        //                                    }
        //                                    queryManager.InsertUpdateFollowTable(Username, userid, username);
        //                                }
        //                                catch (Exception)
        //                                {
        //                                }

        //                                try
        //                                {
        //                                    RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
        //                                }
        //                                catch { }
        //                            }
        //                            else if (followStatus == "Already Followed")
        //                            {
        //                                Globals.totalcountFollower++;
        //                                Log("[ " + DateTime.Now + " ] => [ >> Already Followed >> " + user_id_toFollow + " by " + Username + " ]");
        //                                try
        //                                {
        //                                    RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
        //                                }
        //                                catch { }
        //                            }
        //                            else
        //                            {
        //                                Log("[ " + DateTime.Now + " ] => [ >> Couldn't Follow >> " + user_id_toFollow + " by " + Username + " ]");
        //                                //Adding in text File for Failed Followed Paths
        //                                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id + ":" + Screen_name + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
        //                            }

        //                            //if user is check fast follow option then delay is not working on that condition ...!!

        //                            if (!frmMain_NewUI.IsFastfollow)
        //                            {
        //                                if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
        //                                {
        //                                    if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
        //                                    {
        //                                        //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
        //                                        Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
        //                                        Thread.Sleep(delay * 1000);

        //                                        //delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
        //                                        Log("[ " + DateTime.Now + " ] => [ Follow Global Delayed for " + delay1 + " Seconds ]");
        //                                        Thread.Sleep(delay1 * 1000);
        //                                    }
        //                                    else 
        //                                    {
        //                                        Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
        //                                        Thread.Sleep(delay * 1000);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
        //                                    Thread.Sleep(delay * 1000);
        //                                }

                                       
        //                            }

        //                            if (OtherUser)
        //                            {
        //                                counter++;
                                        
        //                                if (counter == noOFFollows)
        //                                {
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else //Already Being Followed
        //                    {
        //                        Log("[ " + DateTime.Now + " ] => [ Already Followed : " + user_id_toFollow + " with : " + Username + " ]");
        //                        try
        //                        {
        //                            RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
        //                        }
        //                        catch { }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Log("[ " + DateTime.Now + " ] => [ Sorry No User To Follow");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            Log("[ " + DateTime.Now + " ] => [ >> Couldn't Login with>> " + Username + " ]");
        //            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() --> " + ex.Message, Globals.Path_FollowerErroLog);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
        //    }
        //    finally
        //    {
        //        Log("[ " + DateTime.Now + " ] => [ Finished Following All Users For Username : " + Username + " ]");
        //        Log("------------------------------------------------------------------------------------------------------------------------------------------");
        //        GlobusFileHelper.AppendStringToTextfileNewLine("Module Follow count : " + counterChecktotlaFollowr + " using UserName: " + Username, Globals.path_CountNoOfProcessDone);
        //    }
        //}
        #endregion

        #region New updated code of FollowUsingURLs method by sonu
        public void FollowUsingURLs(List<string> list_user_id_toFollow, int minDelay, int maxDelay, bool OtherUser)
        {
            int counter = 0;
            int counterChecktotlaFollowr = 0;
            try
            {
                //Login();
                try
                {
                    DataSet Dataset = new DataSet();
                    if (NoOfFollowPerDay_ChkBox == true)
                    {
                        Dataset = CheckLimitationOfPerID(Username);
                        if (Dataset != null)
                        {
                            if (Dataset.Tables.Count != 0)
                            {
                                if (Dataset.Tables[0].Rows.Count != 0)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ No Of Follow Per Day - " + NoOfFollowPerDay + " ]");
                                    int DataSetTableRowsCount = Dataset.Tables[0].Rows.Count;
                                    RemainingNoOfFollowPerDay = NoOfFollowPerDay - DataSetTableRowsCount;
                                }
                                else
                                {
                                    RemainingNoOfFollowPerDay = NoOfFollowPerDay;
                                }
                            }
                            else
                            {
                                RemainingNoOfFollowPerDay = NoOfFollowPerDay;
                            }
                        }
                    }
                    else
                    {
                        RemainingNoOfFollowPerDay = TweetAccountManager.NoOfFollowPerDay;
                    }

                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- NoOfFollowPerDay_ChkBox --> " + ex.Message, Globals.Path_FollowerErroLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() --  " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                }

                ///Login if Not Logged in
                if (!IsLoggedIn)
                {
                    Login();
                }

                ///Return if Suspended
                if (AccountStatus == "Account Suspended")
                {
                    Log("[ " + DateTime.Now + " ] => [  " + Username + " : Suspended ]");
                    return;
                }


                if (IsLoggedIn)
                {
                    #region trial last tweet date code
                    //if (UseDateLastTweeted)
                    //{
                    //    string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");
                    //    string[] Array = Regex.Split(PageSource, "href=");

                    //    foreach (string item in Array)
                    //    {
                    //        if (item.Contains(Screen_name + "/status/") && item.Contains("tweet-timestamp js-permalink js-nav"))
                    //        {
                    //            try
                    //            {
                    //                int startIndex = item.IndexOf("title=\"");
                    //                string Start = item.Substring(startIndex).Replace("title=\"", "");
                    //                int endIndex = Start.IndexOf("\" >");
                    //                string End = Start.Substring(0, endIndex);

                    //                string[] array = Regex.Split(End, " ");
                    //                DateTime lastTweetDate = new DateTime(Convert.ToInt32(array[5]), 08, Convert.ToInt32(array[3]), 12, 12, 12);

                    //                DateTime dt_Now = DateTime.Now;

                    //                TimeSpan dt_Difference = lastTweetDate.Subtract(dt_Now);

                    //                TimeSpan t = dt_Now - lastTweetDate;
                    //                double NrOfDays = t.TotalDays;

                    //                if (dt_Difference.Days >= LastTweetDays)
                    //                {
                    //                    Log("Not Followed as Last Tweeted Day is greated than specified For : " + Username);
                    //                    continue;
                    //                }

                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //        }
                    //    }
                    //} 
                    #endregion

                    int NoOfFollowCounter = 0;

                    clsDBQueryManager queryManager = new clsDBQueryManager();

                    TwitterDataScrapper dataScrapper = new TwitterDataScrapper();

                    ///Get list of Already Followings
                    List<string> lst_AlreadyUserid = new List<string>(); ;
                    List<string> lst_AlreadyUserName = new List<string>();
                    if (list_user_id_toFollow.Count > 0 && !string.IsNullOrEmpty(list_user_id_toFollow[0]))
                    {
                        foreach (string user_id_toFollow in list_user_id_toFollow)
                        {
                            if (NoOfFollowPerDay_ChkBox)
                            {
                                if (NoOfFollowCounter >= RemainingNoOfFollowPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Finish Follow Limit " + NoOfFollowPerDay + " Today ]");
                                    break;
                                }
                            }

                            string followStatus;
                            string user_id = string.Empty;
                            string pagesource = string.Empty;
                            string Screen_name = string.Empty;
                            ///Getting Username from User id
                            ///Working from both username n id
                            clsDBQueryManager DB = new clsDBQueryManager();

                            #region commented for choosing username and userid
                            //if (!GlobusRegex.ValidateNumber(user_id_toFollow))//(!IsItNumber(user_id_toFollow))
                            //if (frmMain_NewUI.IsFollowerScreanName)
                            //{
                            //    DataSet ds = DB.GetUserId(user_id_toFollow);
                            //    if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count > 0)
                            //    {
                            //        foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                            //        {
                            //            user_id = dataRow.ItemArray[0].ToString();
                            //        }
                            //    }
                            //}
                            //else if (frmMain_NewUI.IsfollowerUserId)
                            //{
                            //    user_id = user_id_toFollow;
                            //    DataSet ds = DB.GetUserName(user_id_toFollow);
                            //    if (ds.Tables[0].Rows.Count > 0)
                            //    {
                            //        foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                            //        {
                            //            user_id = dataRow.ItemArray[0].ToString();
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    user_id = string.Empty;
                            //    Log("Please enter Valid user Id Or Screan name.  ");
                            //} 
                            #endregion


                            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TwtDominator\\List_Not_Followed.txt";//Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TwtDominator\\DB_TwtDominator.db"

                            #region get check unfollow befor
                            if (TweetAccountManager.UseUnfollowedBeforeFilter)
                            {
                                List<string> lst_AlreadyExist = new List<string>();

                                if (frmMain_NewUI.IsFollowerScreenName)
                                {
                                    lst_AlreadyExist = DB.SelectUnFollowDUsernameID_List(Username);
                                    bool isAlreadyExist = lst_AlreadyExist.Exists(s => (s == user_id_toFollow));
                                    if (isAlreadyExist)
                                    {
                                        try
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]");
                                            GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]", path);  //BrowsedUsersPathToFollow
                                            if (!string.IsNullOrEmpty(FileFollowUrlPath))
                                            {
                                                try
                                                {
                                                    // GlobusFileHelper.RemoveTextFromFile(user_id_toFollow, FileFollowUrlPath);
                                                    RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                                }
                                                catch { };
                                            }
                                        }
                                        catch { };
                                        continue;
                                    }
                                }
                                else if (frmMain_NewUI.IsfollowerUserId && BaseLib.NumberHelper.ValidateNumber(user_id_toFollow))
                                {
                                    lst_AlreadyExist = DB.SelectUnFollowedUsername_List(Username);
                                    bool isAlreadyExist = lst_AlreadyExist.Exists(s => (s == user_id_toFollow));
                                    if (isAlreadyExist)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Is already Unfollowed before from " + Username + " ]", path);
                                        if (!string.IsNullOrEmpty(FileFollowUrlPath))
                                        {
                                            try
                                            {
                                                //  GlobusFileHelper.RemoveTextFromFile(user_id_toFollow, FileFollowUrlPath);
                                                RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                            }
                                            catch { };
                                        }
                                        continue;
                                    }
                                }
                            }

                            #endregion

                            ///Counter For Follow
                            NoOfFollowCounter++;

                            //Check if user_id_toFollow is already being followed or not

                            #region user_id_toFollow is already being followed or not

                            bool isAlreadyFollowed = false;
                            Log("[ " + DateTime.Now + " ] => [ Checking For Already Followed For " + user_id_toFollow + " From " + Username + " ]");
                            if (frmMain_NewUI.IsFollowerScreenName)
                            {
                                lst_AlreadyUserName = queryManager.SelectFollowDUsername_List(Username);
                                isAlreadyFollowed = lst_AlreadyUserName.Exists(s => (s == user_id_toFollow));
                                Screen_name = user_id_toFollow;
                            }
                            else if (frmMain_NewUI.IsfollowerUserId && BaseLib.NumberHelper.ValidateNumber(user_id_toFollow))
                            {
                                lst_AlreadyUserid = queryManager.SelectFollowDUsernameID_List(Username);
                                isAlreadyFollowed = lst_AlreadyUserid.Exists(s => (s == user_id_toFollow));
                                user_id = user_id_toFollow;
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ UserId/ScreenName Not In Correct Format :- " + user_id_toFollow + " ]");
                                return;
                            }

                            #endregion


                            if (!isAlreadyFollowed)  //If not already being followed, follow now
                            {
                                ///Use FollowingsFollowers Ratio Filter
                                if (UseRatioFilter)
                                {
                                    int FollowingsFollowersRatio_user_id = 0;

                                    //Check FollowingsFollowers Ratio of this user_id
                                    try
                                    {
                                        string returnstatusFollower = string.Empty;
                                        string returnstatusFollowing = string.Empty;
                                        List<string> Following = dataScrapper.GetFollowings(user_id, out returnstatusFollowing);
                                        List<string> Follower = dataScrapper.GetFollowers(user_id, out returnstatusFollower);
                                        int count_Followings_user_id = Following.Count;
                                        int count_Followers_user_id = Follower.Count;

                                        FollowingsFollowersRatio_user_id = (count_Followings_user_id * 100) / count_Followers_user_id;
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- UseRatioFilter --> " + ex.Message, Globals.Path_FollowerErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- UseRatioFilter -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }

                                    if (!(FollowingsFollowersRatio_user_id >= FollowingsFollowersRatio)) //If FollowingsFollowersRatio_user_id is less than Required, continue with next user_id_toFollow
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Not Followed as FollowingsFollowersRatio : " + FollowingsFollowersRatio_user_id + " for " + user_id_toFollow + " with : " + Username + " ]");
                                        continue;
                                    }
                                }


                                #region Old Unfollowed Before Code
                                //if (UseUnfollowedBeforeFilter)
                                //{
                                //    try
                                //    {
                                //        List<string> lst_Followings_Current = dataScrapper.GetFollowings(user_id);

                                //        //lst_AlreadyFollowings
                                //    }
                                //    catch { }
                                //} 
                                #endregion

                                #region Old Tweet Last Day Code
                                if (UseDateLastTweeted && !string.IsNullOrEmpty(user_id))
                                {
                                    try
                                    {
                                        string strLastTweetDate = dataScrapper.GetUserLastTweetDate(user_id);

                                        DateTime dt_LastTweetDate = DateTime.Parse(strLastTweetDate);

                                        DateTime dt_Now = DateTime.Now;

                                        TimeSpan dt_Difference = dt_Now.Subtract(dt_LastTweetDate);

                                        if (dt_Difference.Days >= LastTweetDays)
                                        {
                                            try
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]");
                                                GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]", path);
                                                if (!string.IsNullOrEmpty(FileFollowUrlPath))
                                                {
                                                    try
                                                    {
                                                        //GlobusFileHelper.RemoveTextFromFile(user_id_toFollow, FileFollowUrlPath);
                                                        RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                                    }
                                                    catch { };
                                                }
                                            }
                                            catch { };

                                            continue;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- Last Tweeted --> " + ex.Message, Globals.Path_FollowerErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- UseDateLastTweeted -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }
                                }
                                #endregion

                                #region Check Profile Picture
                                if (IscontainPicture)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Checking Profile Image For : " + user_id_toFollow + " ]");
                                    string containsIamge = TwitterDataScrapper.GetPhotoFromUsername_New(Screen_name);
                                    Thread.Sleep(1000);
                                    if (containsIamge == "true")
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Contains Image ]");
                                    }
                                    else if (containsIamge == "false")
                                    {
                                        try
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Not Contains Image So Not Following ]");
                                            GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ " + user_id_toFollow + " Not Contains Image So Not Following ]", path);
                                            if (!string.IsNullOrEmpty(FileFollowUrlPath))
                                            {
                                                try
                                                {
                                                    // GlobusFileHelper.RemoveTextFromFile(user_id_toFollow, FileFollowUrlPath);
                                                    RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                                }
                                                catch { };
                                            }
                                        }
                                        catch { };
                                        continue;
                                    }
                                    else if (containsIamge == "Rate limit exceeded")
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Cannot Make Request. Rate limit exceeded ]");
                                        Log("[ " + DateTime.Now + " ] => [ Please Try After Some Time ]");
                                        Thread.Sleep(5000);
                                    }
                                }
                                #endregion

                                #region Checking Tweet in the last xx days.

                                if (IsTweetedInXdays)
                                {
                                    Log("[ " + DateTime.Now + " ] => [checking tweeted in the last  " + daysTweetedInxdays + "  days for User :" + user_id_toFollow + " ]");
                                    try
                                    {
                                        string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id_toFollow), "", ""); //tweet-timestamp js-permalink js-nav js-tooltip" title="
                                        //string dateoflastTweet = globusHttpHelper.getBetween(PageSource, "ProfileTweet-timestamp js-permalink js-nav js-tooltip", ">").Replace("\"","").Trim();
                                        string dateoflastTweet = globusHttpHelper.getBetween(PageSource, "tweet-timestamp js-permalink js-nav js-tooltip\" title=\"", ">").Replace("\"", "").Trim();
                                        try
                                        {
                                            dateoflastTweet = dateoflastTweet.Split('-')[1].Trim();
                                            //dateoflastTweet = dateoflastTweet.Split('>')[1].Trim();
                                            DateTime dt_LastTweetDate = DateTime.Parse(dateoflastTweet);

                                            DateTime dt_Now = DateTime.Now;

                                            TimeSpan dt_Difference = dt_Now.Subtract(dt_LastTweetDate);
                                            if (dt_Difference.Days >= Convert.ToInt32(daysTweetedInxdays))
                                            {
                                                try
                                                {
                                                    Log("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]");
                                                    GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ Not Followed as Last Tweeted Day is greated than specified for :" + user_id_toFollow + " with : " + Username + " ]", path);
                                                    if (!string.IsNullOrEmpty(FileFollowUrlPath))
                                                    {
                                                        try
                                                        {
                                                            // GlobusFileHelper.RemoveTextFromFile(user_id_toFollow, FileFollowUrlPath);
                                                            RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                                        }
                                                        catch { };
                                                    }
                                                }
                                                catch { };
                                                continue;
                                            }
                                        }
                                        catch { }


                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                                #endregion


                                int delay = 10 * 1000;
                                int delay1 = 10 * 1000;
                                try
                                {
                                    if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                    {
                                        if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
                                        {
                                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                            delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                        }
                                        else
                                        {
                                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                        }
                                    }
                                    else
                                    {
                                        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- delay --> " + ex.Message, Globals.Path_FollowerErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- delay -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                }

                                if (Globals.IsCampaign)
                                {
                                    lock (locker_que_Follower)
                                    {
                                        if (string.IsNullOrEmpty(user_id))
                                        {
                                            try
                                            {
                                                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id), "", "");
                                                int startindex = PageSource.IndexOf("data-user-id=\"");
                                                string start = PageSource.Substring(startindex).Replace("data-user-id=\"", "");
                                                int endindex = start.IndexOf("\"");
                                                string end = start.Substring(0, endindex);
                                                user_id = end;
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        if (string.IsNullOrEmpty(Screen_name))
                                        {
                                            try
                                            {
                                                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/account/redirect_by_id?id=" + user_id), "", "");

                                                int startindex = PageSource.IndexOf("user-style-");
                                                string start = PageSource.Substring(startindex).Replace("user-style-", "");
                                                int endindex = start.IndexOf("\"");
                                                string end = start.Substring(0, endindex);
                                                Screen_name = end;
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }

                                        string strQuery = "Select * from tb_CampaignReport Where Follower = '" + user_id + "' OR ScreenName = '" + Screen_name + "' AND Campaign_Name = '" + Globals.Campaign_Name + "' ";
                                        DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_CampaignReport");
                                        if (ds.Tables[0].Rows.Count > 0 && Globals.IsCampaign)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Already Followed Userid : " + user_id + " >>>> ScreenName : " + Screen_name + " ]");
                                        }
                                        else
                                        {
                                            follower.FollowUsingProfileID_New(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, out followStatus);
                                            string Screen_Name = string.Empty;
                                            string userid = string.Empty;
                                            if (NumberHelper.ValidateNumber(user_id))
                                            {
                                                userid = user_id;
                                            }
                                            else
                                            {
                                                Screen_name = user_id;
                                            }
                                            Log("[ " + DateTime.Now + " ] => [ Followed Userid : " + user_id + " >>>> ScreenName : " + Screen_name + " ]");
                                            Log("[ " + DateTime.Now + " ] => [ Adding Follower in DataBase ]");
                                            string query = "INSERT INTO tb_CampaignReport (Campaign_Name, UserName, Follower , ScreenName) VALUES ('" + Globals.Campaign_Name + "', '" + Username + "', '" + user_id + "' , '" + Screen_name + "')";
                                            DataBaseHandler.InsertQuery(query, "tb_CampaignReport");
                                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id + ":" + Screen_name + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
                                        }
                                    }
                                }
                                else
                                {
                                    follower.FollowUsingProfileID_New(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, out followStatus);
                                    if (followStatus == "followed")
                                    {
                                        Globals.totalcountFollower++;
                                        counterChecktotlaFollowr++;
                                        Log("[ " + DateTime.Now + " ] => [ >> Followed >> " + user_id_toFollow + " by " + Username + " ]");

                                        //Adding in text File FOr SuccessFull Paths
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id_toFollow + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
                                        try
                                        {
                                            string username = string.Empty;
                                            string userid = string.Empty;
                                            if (NumberHelper.ValidateNumber(user_id_toFollow))
                                            {
                                                username = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUserNameFromUserId(user_id_toFollow);
                                                userid = user_id_toFollow;
                                            }
                                            else
                                            {
                                                string outStatus = string.Empty;
                                                userid = TwitterSignup.TwitterSignup_TwitterDataScrapper.GetUserIDFromUsername(user_id_toFollow, out outStatus);
                                                username = user_id_toFollow;
                                            }
                                            queryManager.InsertUpdateFollowTable(Username, userid, username);
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        try
                                        {
                                            RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                        }
                                        catch { }
                                    }
                                    else if (followStatus == "Already Followed")
                                    {
                                        Globals.totalcountFollower++;
                                        Log("[ " + DateTime.Now + " ] => [ >> Already Followed >> " + user_id_toFollow + " by " + Username + " ]");
                                        try
                                        {
                                            RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ >> Couldn't Follow >> " + user_id_toFollow + " by " + Username + " ]");
                                        //Adding in text File for Failed Followed Paths
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id + ":" + Screen_name + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
                                    }

                                    //if user is check fast follow option then delay is not working on that condition ...!!

                                    if (!frmMain_NewUI.IsFastfollow)
                                    {
                                        if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                        {
                                            if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
                                            {
                                                //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                                Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
                                                Thread.Sleep(delay * 1000);

                                                //delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                                Log("[ " + DateTime.Now + " ] => [ Follow Global Delayed for " + delay1 + " Seconds ]");
                                                Thread.Sleep(delay1 * 1000);
                                            }
                                            else
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
                                                Thread.Sleep(delay * 1000);
                                            }
                                        }
                                        else
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Follow Delayed for " + delay + " Seconds ]");
                                            Thread.Sleep(delay * 1000);
                                        }


                                    }

                                    if (OtherUser)
                                    {
                                        counter++;

                                        if (counter == noOFFollows)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            else //Already Being Followed
                            {
                                Log("[ " + DateTime.Now + " ] => [ Already Followed : " + user_id_toFollow + " with : " + Username + " ]");
                                try
                                {
                                    RemoveFollwerFromTxtFile(FileFollowUrlPath, user_id_toFollow);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("[ " + DateTime.Now + " ] => [ Already Followed : " + user_id_toFollow + " with : " + Username + " ]", path);
                                }
                                catch { }
                            }
                        }
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Sorry No User To Follow");
                        return;
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ >> Couldn't Login with>> " + Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            finally
            {
                Log("[ " + DateTime.Now + " ] => [ Finished Following All Users For Username : " + Username + " ]");
                Log("------------------------------------------------------------------------------------------------------------------------------------------");
                GlobusFileHelper.AppendStringToTextfileNewLine("Module Follow count : " + counterChecktotlaFollowr + " using UserName: " + Username, Globals.path_CountNoOfProcessDone);
            }
        }
        #endregion



        public static bool IsUniqueMessagePostForDirectMessage = false;
        public void PostDirectMessage(List<string> list_user_id_toFollow, int minDelay, int maxDelay, bool OtherUser)
        { 
             if (!IsLoggedIn)
                {
                    Login();
                }

                ///Return if Suspended
                if (AccountStatus == "Account Suspended")
                {
                    Log("[ " + DateTime.Now + " ] => [  " + Username + " : Suspended ]");
                    return;
                }
             string msgBodyCompose = string.Empty;
             Queue<string> UniqueDirectMessage = new Queue<string>();
               // code written by Puja 
             if (IsUniqueMessagePostForDirectMessage)
                        {
                            foreach (string _item in lstDirectMessageText)
                            {
                                //if (UniqueDirectMessage.Count==0)
                                {
                                    UniqueDirectMessage.Enqueue(_item);
                                }
                            }                            
                        }

                int counter = 0;
                string followStatus = string.Empty;
               
                List<string> TempList =new List<string>();
               
                TempList.AddRange(lstDirectMessageText);
            // code Written by Puja for Sending Unique Message
                if (IsLoggedIn)
                {
                    foreach (string user_id_toFollow in list_user_id_toFollow)
                    {

                        if (IsUniqueMessagePostForDirectMessage)
                        {
                            if (UniqueDirectMessage.Count==0)
                            {
                                msgBodyCompose = "";
                                Log("[ " + DateTime.Now + " ] => [ All entered messages are used ]");
                                break;

                            }
                            foreach (string item in UniqueDirectMessage)
                            {
                                //if (item == null)
                                //{
                                //    msgBodyCompose = "";
                                //    Log("[ " + DateTime.Now + " ] => [ All entered message are used ]");
                                //    break;
                                //}
                                msgBodyCompose = item;
                                UniqueDirectMessage.Dequeue();
                                break;
                            }

                        }
                        else
                        {
                            try
                            {
                                // msgBodyCompose = lstDirectMessageText[RandomNumberGenerator.GenerateRandom(0, lstDirectMessageText.Count - 1)];

                                if (counter > lstDirectMessageText.Count - 1)
                                {
                                    counter = 0;
                                }
                                msgBodyCompose = lstDirectMessageText[counter];
                                counter++;
                            }
                            catch
                            {
                            }
                        }

                        follower.SendDirectMessage(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toFollow, msgBodyCompose, Username, out followStatus);

                        if (followStatus == "Message send")
                        {
                            Log("[ " + DateTime.Now + " ] => [ >> Message "+msgBodyCompose+" sent >> " + user_id_toFollow + " by " + Username + " ]");

                            //Adding in text File FOr SuccessFull Paths
                            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id_toFollow + ":" + msgBodyCompose+ ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyDirectMessageSend);
                            

                        }
                        else if (followStatus == "Message not send")
                        {
                            Log("[ " + DateTime.Now + " ] => [ >> Message "+msgBodyCompose+" not send >> " + user_id_toFollow + " by " + Username + " ]");
                            
                        }
                        //else
                        //{
                        //    Log("[ " + DateTime.Now + " ] => [ >> Message not send >> " + user_id_toFollow + " by " + Username + " ]");
                        //    Adding in text File for Failed Followed Paths
                        //    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + user_id + ":" + Screen_name + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
                        //}
                        int delay = 0; ;
                        try
                        {
                             delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                        }
                        catch { }
                        Log("[ " + DateTime.Now + " ] => [ Message Delayed for " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);        
                                    
                    }
                }
        }
    


        public void RemoveFollwerFromTxtFile(string tempDeleteFollowFilePath, string TargetedUser_item)
        {
            lock (locker_DirectMessage)
            {
                try
                {
                    List<string> quotelist = File.ReadAllLines(tempDeleteFollowFilePath).ToList();
                    string firstItem = TargetedUser_item;
                    bool notpaddeltag = false;

                    if (quotelist.Contains(TargetedUser_item))
                    {
                        quotelist.Remove(TargetedUser_item);
                        notpaddeltag = true;
                    }

                    if (notpaddeltag)
                    {
                        File.WriteAllLines(tempDeleteFollowFilePath, quotelist.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile  --> " + ex.Message, Globals.Path_FollowerErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + "Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile --> " + ex.Message, Globals.Path_FollowerErroLog);
                }
            }

        }

        //*********Coded By Abhishek for checking If  (30/7/12)**********************
        public DataSet CheckLimitationOfPerID(string kryValue)
        {
            try
            {
                clsDBQueryManager DbQueryManager = new clsDBQueryManager();
                DataSet Ds = DbQueryManager.SelectFollowMessageData(kryValue);
                return Ds;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckLimitationOfPerID() --> " + ex.Message, Globals.Path_FollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  CheckLimitationOfPerID() -- " + kryValue + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                return new DataSet();
            }
        }

        public static bool IsItNumber(string inputvalue)
        {
            Regex isnumber = new Regex("[^0-9]");
            return !isnumber.IsMatch(inputvalue);
        }

        public void UnFollowUsingURLs(List<string> list_user_id_toUnFollow, int minDelay, int maxDelay)
        {
            //Login();

            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {
                        int counter_Unfollow = 0;

                        if (list_user_id_toUnFollow.Count > 0)
                        {
                            Log("[ " + DateTime.Now + " ] => [ You have selected : " + list_user_id_toUnFollow.Count() + " ID to be Unfollowed ]");

                            foreach (string _user_id_toUnFollow in list_user_id_toUnFollow)
                            {
                                string _ScreenName = string.Empty;
                                string user_id_toUnFollow = string.Empty;
                                if (_user_id_toUnFollow.Contains(":"))
                                {
                                    try
                                    {
                                        _ScreenName = _user_id_toUnFollow.Split(':')[0];

                                        user_id_toUnFollow = _user_id_toUnFollow.Split(':')[1];
                                    }
                                    catch { };
                                }
                                else
                                {
                                    user_id_toUnFollow = _user_id_toUnFollow;
                                }

                                if (counter_Unfollow >= noOfUnfollows)
                                {
                                    break;
                                }

                                string unFollowStatus;

                                unFollower.UnFollowUsingProfileID(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, user_id_toUnFollow, out unFollowStatus);
                                int Delay = 10;
                                try
                                {
                                    if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                    {
                                        Delay = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                    }
                                    else
                                    {
                                        Delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingURLs() -- delay --> " + ex.Message, Globals.Path_FollowerErroLog);
                                    //GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  FollowUsingURLs() -- delay -- " + Username + " -- " + user_id_toFollow + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                }

                                if (unFollowStatus == "Unfollowed" || unFollowStatus == "Already Unfollowed")
                                {
                                    counter_Unfollow++;
                                    if (unFollowStatus == "Already Unfollowed")
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ >> Already Unfollowed >> " + user_id_toUnFollow + " by " + Username + " ]");
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ >> Unfollowed >> " + user_id_toUnFollow + " by " + Username + " ]");
                                    }
                                    //Thread.Sleep(RandomNumberGenerator.GenerateRandom(3000, 5000));
                                    
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + user_id_toUnFollow, Globals.path_SuccessfullyUnfollowAccounts);
                                    clsDBQueryManager queryManager = new clsDBQueryManager();
                                    List<string> lst_AlreadyUserName = queryManager.SelectFollowDUsernameID_List(Username);
                                    bool isAlreadyFollowed = lst_AlreadyUserName.Exists(s => (s == user_id_toUnFollow));
                                    if (isAlreadyFollowed)
                                    {
                                        queryManager.DeleteFollowDUsernameID_List(Username, user_id_toUnFollow);
                                    }
                                    try
                                    {
                                        string status = string.Empty;
                                        //get screen name or id 
                                        if (NumberHelper.ValidateNumber(user_id_toUnFollow) && string.IsNullOrEmpty(_ScreenName))
                                        {
                                            _ScreenName = TwitterDataScrapper.GetUserNameFromUserId_New(user_id_toUnFollow, out status, ref globusHttpHelper);
                                        }
                                        queryManager.InserOrUpdateUnfollower(Username, user_id_toUnFollow, _ScreenName);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ >> Couldn't Unfollow >> " + user_id_toUnFollow + " by " + Username + " ]");
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + user_id_toUnFollow, Globals.path_FailedToUnfollowAccounts);
                                }
                                Log("[ " + DateTime.Now + " ] => [ Unfollow Delayed For " + Delay + " Seconds ]");
                                Thread.Sleep(Delay * 1000);
                            }
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ No Users To UnFollow ]");
                            return;
                        }
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ >> Account is suspended >>" + Username + " ]");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {
                    Log("[ " + DateTime.Now + " ] => [ Unfollowing Process Stopped ]");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Some exception in Method:UnFollowUsingURLs --> Class --> TweetAccountManager --> Unfollow: " + ex.Message + " ]");
                }
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- UnFollowUsingURLs() --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  UnFollowUsingURLs() --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            finally
            {
                Log("[ " + DateTime.Now + " ] => [ Finished Unfollowing For Username :" + Username + " ]");
                Log("------------------------------------------------------------------------------------------------------------------------------------------");

            }
        }

        public string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        #region Commented
        //public void Tweet(string tweetMessage)
        //{
        //    //Login();

        //    //if (true)

        //    if (!IsLoggedIn)
        //    {
        //        Login();
        //    }

        //    if (IsLoggedIn)
        //    {
        //        string tweetStatus;

        //        tweeter.Tweet(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, tweetMessage, out tweetStatus);

        //        if (tweetStatus == "posted")
        //        {
        //            Log(">> Tweeted >> " + tweetMessage + " by " + Username);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyTweetAccounts);
        //            Thread.Sleep(RandomNumberGenerator.GenerateRandom(3000, 5000));
        //        }
        //        else
        //        {
        //            Log(">>Couldn't Post >> " + tweetMessage + " by " + Username);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToTweetAccounts);
        //        }
        //    }
        //    else
        //    {
        //        Log("Couldn't log in with " + Username);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
        //    }

        //} 
        #endregion

        public void Tweet(List<string> lst_tweetMessage, int minDelay, int maxDelay)
        {
            //Login();

            //if (true)

            if (!IsLoggedIn)
            {
                Login();
            }



            if (IsNotSuspended)
            {
                if (IsLoggedIn)
                {
                    int counter_Tweet = 0;

                    #region <<Set delay time if Minimum or maximum delay time is not set >>
                    //Set delay time if Minimum or maximum delay time is not set 
                    if (minDelay == 0 || maxDelay == 0)
                    {
                        try
                        {
                            if (minDelay == 0)
                            {
                                minDelay = 0;
                            }
                            if (maxDelay == 0)
                            {
                                maxDelay = 10;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    #endregion

                    #region Separate Tweets per account

                    if (!AllTweetsPerAccount)
                    {
                        while (que_TweetMessages.Count > 0)
                        {
                            if (!TweetPerDay)
                            {
                                if (counter_Tweet >= NoOfTweets)
                                {
                                    break;
                                }
                            }
                            else if (TweetPerDay)
                            {
                                if (AlreadyTweeted >= NoOfTweetPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Already Tweeted " + AlreadyTweeted + " ]");
                                    break;
                                }
                            }


                            string tweetMessage = "";
                            string tweetStatus;
                            #region commented
                            //int delay = 10 * 1000;
                            //int delay = 0;
                            //try
                            //{
                            //    if (DelayTweet > 0)
                            //    {
                            //        //int SomeDelay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            //        //delay = DelayTweet - SomeDelay;

                            //        //if (delay < 0)
                            //        //{
                            //        //    delay = RandomNumberGenerator.GenerateRandom(0, 20);
                            //        //}

                            //        //TimeSpan T = EndTime - DateTime.Now;
                            //        //if (T.TotalSeconds <= 0)
                            //        //{
                            //        //    Log("Time Completed Tweeting Stopped For " + Username);
                            //        //    break;
                            //        //}

                            //        if (DelayTweet > 0)
                            //        {
                            //            int SomeDelay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            //            delay = DelayTweet - SomeDelay;
                            //            TimeSpan T = EndTime - DateTime.Now;
                            //            if (T.TotalSeconds <= 0)
                            //            {
                            //                Log("[ " + DateTime.Now + " ] => [ Time Completed Tweeting Stopped For " + Username + " ]");
                            //                break;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            //        }
                            //    }
                            //    else
                            //    {
                            //        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweet() -- DelayTweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                            //    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet() --> " + ex.Message, Globals.Path_TweetAccountManager);
                            //} 
                            #endregion

                            lock (locker_que_TweetMessage)
                            {
                                if (que_TweetMessages.Count > 0)
                                {
                                    try
                                    {
                                        tweetMessage = que_TweetMessages.Dequeue();
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- locker_que_TweetMessage --> " + ex.Message, Globals.Path_TweetingErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet() --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ All Loaded Tweet Messages Used ]");
                                    //break;
                                }
                            }

                            if (UseHashTags)
                            {
                                lock (locker_que_hashtags)
                                {

                                    if (que_TweetMessages_Hashtags.Count > 0)
                                    {
                                        try
                                        {
                                            Hashtags = que_TweetMessages_Hashtags.Dequeue();
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                        //break;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(Hashtags))
                            {
                                tweetMessage = Hashtags + " " + tweetMessage;
                            }


                            tweeter.Tweet(ref globusHttpHelper, postAuthenticityToken, tweetMessage, out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                try
                                {
                                    Log("[ " + DateTime.Now + " ] => [ >> Tweeted >> " + tweetMessage + " by " + Username + " ]");
                                    clsDBQueryManager DataBase = new clsDBQueryManager();
                                    string dbTweetMessage = StringEncoderDecoder.Encode(tweetMessage);
                                    DataBase.InsertMessageData(Username, "Tweet", "null", dbTweetMessage);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_SuccessfullyTweetAccounts);
                                    //Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                    try
                                    {
                                        RemoveFollwerFromTxtFile(FileTweetPath, tweetMessage);
                                    }
                                    catch { }
                                    //Thread.Sleep(delay * 1000);
                                    counter_Tweet++;
                                    AlreadyTweeted++;
                                    Globals.totalcountTweet++;
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- tweetStatus --> " + ex.Message, Globals.Path_TweetingErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                }
                            }
                            else
                            {
                                if (UseHashTags)
                                {
                                    que_TweetMessages_Hashtags.Enqueue(Hashtags);
                                    que_TweetMessages.Enqueue(tweetMessage);
                                }
                                else
                                {
                                    que_TweetMessages.Enqueue(tweetMessage);
                                }
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Post >> " + tweetMessage + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_FailedToTweetAccounts);
                                //Thread.Sleep(delay * 1000);
                                
                                //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);                            
                                //Thread.Sleep(delay * 1000);
                                //Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                            }

                            //int delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            int delay = 10 * 1000;
                            int delay1 = 10 * 1000;
                            try
                            {
                                if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                {
                                    if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
                                    {
                                        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                        delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);

                                        Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);

                                        Log("[ " + DateTime.Now + " ] => [ Global Delay for Tweet: " + delay1 + " Seconds ]");
                                        Thread.Sleep(delay1 * 1000);
                                    }
                                    else
                                    {
                                        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                        Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);
                                    }
                                }
                                else
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);

                                    Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);
                                }
                            }
                            catch { }
                            //Log("[ " + DateTime.Now + " ] => [ Delay for : " + delay + " Seconds ]");
                            //Thread.Sleep(delay * 1000);

                        }

                    }
                    #endregion

                    #region All Tweets per account
                    else
                    {
                        while (que_TweetMessages_PerAccount.Count > 0)
                        {
                            //if (!TweetPerDay)
                            //{
                            //    if (counter_Tweet >= NoOfTweets)
                            //    {
                            //        break;
                            //    }
                            //}
                            //else if (TweetPerDay)
                            //{
                            //    if (AlreadyTweeted >= NoOfTweetPerDay)
                            //    {
                            //        Log("Already Tweeted " + AlreadyTweeted);
                            //        break;
                            //    }
                            //}


                            string tweetMessage = "";
                            string tweetStatus;
                            int delay = 0;
                            int delay1 = 0;
                            try
                            {
                                if (DelayTweet > 0)
                                {
                                    //int SomeDelay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                    int SomeDelay = 10 * 1000;
                                    try
                                    {
                                        if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                        {
                                            SomeDelay = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                        }
                                        else
                                        {
                                            SomeDelay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                        }
                                    }
                                    catch { }

                                    delay = DelayTweet - SomeDelay;
                                    TimeSpan T = EndTime - DateTime.Now;
                                    if (T.TotalSeconds <= 0)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Time Completed Tweeting Stopped For " + Username + " ]");
                                        break;
                                    }
                                }
                                else
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweet() -- DelayTweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet() --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }

                            lock (locker_que_TweetMessage)
                            {
                                if (que_TweetMessages_PerAccount.Count > 0)
                                {
                                    try
                                    {
                                        tweetMessage = que_TweetMessages_PerAccount.Dequeue();
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- locker_que_TweetMessage --> " + ex.Message, Globals.Path_TweetingErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet() --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ All Loaded Tweet Messages Used ]");
                                    //break;
                                }
                            }

                            if (UseHashTags)
                            {
                                lock (locker_que_hashtags)
                                {

                                    if (que_TweetMessages_Hashtags.Count > 0)
                                    {
                                        try
                                        {
                                            Hashtags = que_TweetMessages_Hashtags.Dequeue();
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                        //break;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(Hashtags))
                            {
                                tweetMessage = Hashtags + " " + tweetMessage;
                            }

                            tweeter.Tweet(ref globusHttpHelper, postAuthenticityToken, tweetMessage, out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                try
                                {
                                    Log("[ " + DateTime.Now + " ] => [ >> Tweeted >> " + tweetMessage + " by " + Username + " ]");
                                    clsDBQueryManager DataBase = new clsDBQueryManager();
                                    string dbTweetMessage = StringEncoderDecoder.Encode(tweetMessage);
                                    DataBase.InsertMessageData(Username, "Tweet", "null", dbTweetMessage);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_SuccessfullyTweetAccounts);
                                    //Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                    //Thread.Sleep(delay * 1000);

                                    try
                                    {
                                        if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                        {
                                            if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
                                            {
                                                delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                                delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);

                                                Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                                Thread.Sleep(delay * 1000);

                                                Log("[ " + DateTime.Now + " ] => [ Global Delay for Tweet: " + delay1 + " Seconds ]");
                                                Thread.Sleep(delay1 * 1000);
                                            }
                                            else
                                            {
                                                delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                                Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                                Thread.Sleep(delay * 1000);
                                            }
                                        }
                                        else
                                        {
                                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);

                                            Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                            Thread.Sleep(delay * 1000);
                                        }
                                    }
                                    catch { }

                                    counter_Tweet++;
                                    AlreadyTweeted++;
                                    Globals.totalcountTweet++;
                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- tweetStatus --> " + ex.Message, Globals.Path_TweetingErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                }

                                try
                                {
                                    RemoveFollwerFromTxtFile(FileTweetPath, tweetMessage);
                                }
                                catch { }
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Post >> " + tweetMessage + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_FailedToTweetAccounts);
                                //Thread.Sleep(delay * 1000);
                                //Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");

                                try
                                {
                                    if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                    {
                                        if (Globals.FollowerRunningText == "FollowerModule" && Globals.TweetRunningText == "TweetModule")
                                        {
                                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                            delay1 = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);

                                            Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                            Thread.Sleep(delay * 1000);

                                            Log("[ " + DateTime.Now + " ] => [ Global Delay for Tweet: " + delay1 + " Seconds ]");
                                            Thread.Sleep(delay1 * 1000);
                                        }
                                        else
                                        {
                                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                            Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                            Thread.Sleep(delay * 1000);
                                        }
                                    }
                                    else
                                    {
                                        delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);

                                        Log("[ " + DateTime.Now + " ] => [ Tweet Delayed for " + delay + " Seconds ]");
                                        Thread.Sleep(delay * 1000);
                                    }
                                }
                                catch { }
                            }

                        }

                    }
                    #endregion
                    GlobusFileHelper.AppendStringToTextfileNewLine("Module Tweet count : " + counter_Tweet + " using UserName: " + Username, Globals.path_CountNoOfProcessDone);
                    Log("[ " + DateTime.Now + " ] => [ Finished Tweeting with : " + Username + " ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                }
            }
            else if (Globals.IPNotWorking)
            {
                Globals.IPNotWorking = false;
                Log("[ " + DateTime.Now + " ] => [ " + Username + ">>Error in Login. ]");
                return;
            }
            else
            {
                clsDBQueryManager database = new clsDBQueryManager();
                database.UpdateSuspendedAcc(Username);
                Log("[ " + DateTime.Now + " ] => [ " + Username + ">>Account Suspended ]");
                return;
            }

        }

        //public void DeleteMessage(string line_to_delete)
        //{
        //    try
        //    {
        //        string search_text = line_to_delete;
        //        string old;
        //        string n = "";
        //        System.IO.StreamWriter sr = new System.IO.StreamWriter();
        //        while ((old = sr.Write()) != null)
        //        {

        //        }
        //        sr.Close();
        //        System.IO.File.WriteAllText(Globals.Path_tweetMessagePath, n);
        //    }
        //    catch (Exception ex)
        //    {

        //    }


        //}

        public void ReTweet(string tweetMessage, int minDelay, int maxDelay)
        {
            frmMain_NewUI obj = (frmMain_NewUI)System.Windows.Forms.Application.OpenForms["frmMain_NewUI"];
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }

                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {

                        int counter_Retweet = 0;


                        TwitterDataScrapper.StructTweetIDs item = new TwitterDataScrapper.StructTweetIDs();
                        while (que_lst_Struct_TweetData.Count > 0)
                        {
                            //foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                            //{
                            if (!RetweetPerDay)
                            {
                                if (counter_Retweet >= noOfRetweets)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ " + counter_Retweet + " ReTweets Done For " + Username + " ]");
                                    break;
                                }
                            }
                            else if (RetweetPerDay)
                            {
                                if (AlreadyRetweeted >= NoOFRetweetPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Already Tweeted " + AlreadyRetweeted + " ]");
                                    break;
                                }
                            }

                            lock (locker_qque_lst_Struct_TweetData)
                            {
                                if (que_lst_Struct_TweetData.Count > 0)
                                {
                                    item = que_lst_Struct_TweetData.Dequeue();
                                }
                                else
                                {

                                }
                            }


                            string tweetStatus;

                            int delay = 10 * 1000;

                            try
                            {
                                //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                }
                                else
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                }
                            }
                            catch (Exception ex)
                            {
                                delay = 10;
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }

                            if (obj.chkBoxRetweetAndFollow.Checked)
                            {
                                tweeter.ReTweetAndFollow(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, item.ID_Tweet, "", item.username__Tweet_User, out tweetStatus);
                            }
                            else
                            {
                                tweeter.ReTweet(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, item.ID_Tweet, "", out tweetStatus);
                            }

                            if (tweetStatus == "posted")
                            {
                                counter_Retweet++;
                                AlreadyRetweeted++;
                                clsDBQueryManager DataBase = new clsDBQueryManager();
                                string dbTweetMessage = StringEncoderDecoder.Encode(item.wholeTweetMessage);
                                DataBase.InsertMessageData(Username, "ReTweet", item.ID_Tweet, dbTweetMessage);
                                Log("[ " + DateTime.Now + " ] => [ Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine( item.username__Tweet_User + " by " + Username , Globals.path_RetweetInformation);
                            }
                            else if (tweetStatus == "followed")
                            {
                                counter_Retweet++;
                                AlreadyRetweeted++;
                                clsDBQueryManager DataBase = new clsDBQueryManager();
                                string dbTweetMessage = StringEncoderDecoder.Encode(item.wholeTweetMessage);
                                DataBase.InsertMessageData(Username, "ReTweet", item.ID_Tweet, dbTweetMessage);
                                Log("[ " + DateTime.Now + " ] => [ Retweeted and Followed " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Retweet >> " + item.ID_Tweet_User + " by " + Username + " ]");
                                //GlobusFileHelper.AppendStringToTextfileNewLine("Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }

                            Log("[ " + DateTime.Now + " ] => [ Retweet Delayed for " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }

                        Log("[ " + DateTime.Now + " ] => [ Finished Retweeting with : " + Username + " ]");
                        Log("------------------------------------------------------------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(Username);
                    Log("[ " + DateTime.Now + " ] => [ " + Username + ">>Account Suspended ]");

                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager() - retweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
        }



        public void ReTweet1(ref TweetAccountManager objTweetAccountManager, int minDelay, int maxDelay)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }

                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {

                        int counter_Retweet = 0;


                        TwitterDataScrapper.StructTweetIDs item = new TwitterDataScrapper.StructTweetIDs();
                        while (objTweetAccountManager.que_lst_Struct_TweetData1.Count > 0)  //TweetAccountManager.que_lst_Struct_TweetData
                        {
                            //foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                            //{
                            if (!RetweetPerDay)
                            {
                                if (counter_Retweet >= noOfRetweets)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ " + counter_Retweet + " ReTweets Done For " + Username + " ]");
                                    break;
                                }
                            }
                            else if (RetweetPerDay)
                            {
                                if (AlreadyRetweeted >= NoOFRetweetPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Already Tweeted " + AlreadyRetweeted + " ]");
                                    break;
                                }
                            }

                            lock (locker_qque_lst_Struct_TweetData)
                            {
                                if (objTweetAccountManager.que_lst_Struct_TweetData1.Count > 0)
                                {
                                    item = objTweetAccountManager.que_lst_Struct_TweetData1.Dequeue();
                                }
                                else
                                {

                                }
                            }


                            string tweetStatus;

                            int delay = 10 * 1000;

                            try
                            {
                                //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                }
                                else
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                }
                            }
                            catch (Exception ex)
                            {
                                delay = 10;
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }


                            tweeter.ReTweet(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, item.ID_Tweet, "", out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                counter_Retweet++;
                                AlreadyRetweeted++;
                                clsDBQueryManager DataBase = new clsDBQueryManager();
                                string dbTweetMessage = StringEncoderDecoder.Encode(item.wholeTweetMessage);
                                DataBase.InsertMessageData(Username, "ReTweet", item.ID_Tweet, dbTweetMessage);
                                Log("[ " + DateTime.Now + " ] => [ Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Retweet >> " + item.ID_Tweet_User + " by " + Username + " ]");
                                //GlobusFileHelper.AppendStringToTextfileNewLine("Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }

                            Log("[ " + DateTime.Now + " ] => [ Retweet Delayed for " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }

                        Log("[ " + DateTime.Now + " ] => [ Finished Retweeting with : " + Username + " ]");
                        Log("------------------------------------------------------------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                        //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(Username);
                    Log("[ " + DateTime.Now + " ] => [ " + Username + ">>Account Suspended ]");

                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager() - retweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
        }



        public void ReTweetDivideRetweet(List<TwitterDataScrapper.StructTweetIDs> lst_DivideTweets, int minDelay, int maxDelay)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }

                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {

                        int counter_Retweet = 0;


                        //TwitterDataScrapper.StructTweetIDs item = new TwitterDataScrapper.StructTweetIDs();
                        //while (que_lst_Struct_TweetData.Count > 0)
                        foreach (var item in lst_DivideTweets)
                        {
                            //foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                            //{
                            if (!RetweetPerDay)
                            {
                                if (counter_Retweet >= noOfRetweets)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ " + counter_Retweet + " ReTweets Done For " + Username + " ]");
                                    break;
                                }
                            }
                            else if (RetweetPerDay)
                            {
                                if (AlreadyRetweeted >= NoOFRetweetPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Already Tweeted " + AlreadyRetweeted + " ]");
                                    break;
                                }
                            }

                            lock (locker_qque_lst_Struct_TweetData)
                            {
                                //if (que_lst_Struct_TweetData.Count > 0)
                                //{
                                //    item = que_lst_Struct_TweetData.Dequeue();
                                //}
                                //else
                                //{

                                //}
                               
                            }


                            string tweetStatus;

                            int delay = 10 * 1000;

                            try
                            {
                                //delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                if (Globals.IsGlobalDelay && Globals.IsCheckValueOfDelay)
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(Globals.MinGlobalDelay, Globals.MaxGlobalDelay);
                                }
                                else
                                {
                                    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                                }
                            }
                            catch (Exception ex)
                            {
                                delay = 10;
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }


                            tweeter.ReTweet(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, item.ID_Tweet, "", out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                counter_Retweet++;
                                AlreadyRetweeted++;
                                clsDBQueryManager DataBase = new clsDBQueryManager();
                                string dbTweetMessage = StringEncoderDecoder.Encode(item.wholeTweetMessage);
                                DataBase.InsertMessageData(Username, "ReTweet", item.ID_Tweet, dbTweetMessage);
                                Log("[ " + DateTime.Now + " ] => [ Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Retweet >> " + item.ID_Tweet_User + " by " + Username + " ]");
                                //GlobusFileHelper.AppendStringToTextfileNewLine("Retweeted " + counter_Retweet + ": >> " + item.username__Tweet_User + " by " + Username, Globals.path_RetweetInformation);
                            }

                            Log("[ " + DateTime.Now + " ] => [ Retweet Delayed for " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);
                        }

                        Log("[ " + DateTime.Now + " ] => [ Finished Retweeting with : " + Username + " ]");
                        Log("------------------------------------------------------------------------------------------------------------------------------------------");
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    clsDBQueryManager database = new clsDBQueryManager();
                    database.UpdateSuspendedAcc(Username);
                    Log("[ " + DateTime.Now + " ] => [ " + Username + ">>Account Suspended ]");

                    return;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager() - retweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  ReTweet()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
        }

        public void getmentions()
        {
            if (!IsLoggedIn)
            {
                Login();
            }
            if (IsNotSuspended)
            {
                string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");

                string[] href = Regex.Split(pageSource, "href=\"/" + Screen_name + "/status/");
                href = href.Skip(1).ToArray();
                foreach (string abc in href)
                {
                    if (abc.Contains("tweet-timestamp js-permalink js-nav") && abc.Contains("js-tweet-text"))
                    {
                        string statusid = string.Empty;
                        try
                        {
                            int startindex = abc.IndexOf("\"");
                            string start = abc.Substring(0, startindex);
                            statusid = start;
                        }
                        catch (Exception ex)
                        {

                        }

                        string StatusPageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name + "/status/" + statusid), "", "");

                        string[] getTweets = Regex.Split(StatusPageSource, "simple-tweet tweet js-stream-tweet");
                        getTweets = getTweets.Skip(1).ToArray();

                        foreach (string tweets in getTweets)
                        {
                            string TweetText = string.Empty;
                            string From_user_Screen_name = string.Empty;
                            string From_user_id = string.Empty;
                            #region Convert HTML to XML
                            Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                            bool success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                            if ((success != true))
                            {
                                Console.WriteLine(htmlToXml.LastErrorText);
                                return;
                            }

                            string xHtml = null;
                            htmlToXml.Html = tweets;

                            //xHtml contain xml data 
                            xHtml = htmlToXml.ToXml();

                            Chilkat.Xml xml = new Chilkat.Xml();
                            xml.LoadXml(xHtml);
                            //xHtml.

                            ////  Iterate over all h1 tags:
                            Chilkat.Xml xNode = default(Chilkat.Xml);
                            Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                            #endregion

                            xNode = null;
                            xBeginSearchAfter = null;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "js-tweet-text");
                            while ((xNode != null))
                            {
                                TweetText = xNode.AccumulateTagContent("text", "script|style");
                                break;
                            }

                            try
                            {
                                int startindex = tweets.IndexOf("data-screen-name");
                                string start = tweets.Substring(startindex).Replace("data-screen-name=\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                From_user_Screen_name = end;
                            }
                            catch (Exception ex)
                            {
                                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            try
                            {
                                int startindex = tweets.IndexOf("data-user-id");
                                string start = tweets.Substring(startindex).Replace("data-user-id=\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                From_user_id = end;
                            }
                            catch (Exception ex)
                            {
                                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            string strQuery = "INSERT INTO tb_ReplyCampaign (TweetId , Username , ReplyUserName , ReplyUserId , TweetText , Reply) VALUES ('" + statusid + "' , '" + Username + "' , '" + From_user_Screen_name + "' , '" + From_user_id + "' , '" + TweetText + "' , '" + TweetText + "')";
                            DataBaseHandler.InsertQuery(strQuery, "tb_ReplyCampaign");


                        }
                    }
                }

            }

        }


        public static Queue<TwitterDataScrapper.StructTweetIDs> List_of_struct_Keydatafrom_tweetData_list = new Queue<TwitterDataScrapper.StructTweetIDs>();
        public static bool IsReplyPerTweet = false;
        public static bool IsreplyUniqueTweet = false;
        public void Reply(string tweetMessage, int minDelay, int maxDelay)
        {
            //Login();
            List<string> lstTemp = new List<string>();
            //if (true)
            try
            {
                TwitterDataScrapper.StructTweetIDs TweeetData = new TwitterDataScrapper.StructTweetIDs();
                if (!IsLoggedIn)
                {
                    Login();
                }

                if (IsNotSuspended)
                {
                    if (IsLoggedIn)
                    {
                        int counter_Reply = 0;

                        //foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                        //{
                        //    List_of_struct_Keydatafrom_tweetData_list.Enqueue(item);

                        //TweeetData = List_of_struct_Keydatafrom_tweetData_list.Dequeue();

                        while (que_ReplyMessages.Count > 0)
                        {
                            int delay = 10 * 1000;
                            int counter = 0;

                            try
                            {
                                delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply()-- delay  --> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Reply()  -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }
                            // Code for Sending unique Reply 
                            // By Puja
                            lock (locker_que_ReplyTweetMessage)
                            {
                                //if (IsreplyUniqueTweet)
                                {
                                    if (que_ReplyMessages.Count > 0)
                                    {
                                        try
                                        {
                                            tweetMessage = que_ReplyMessages.Dequeue();
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply()-- locker_que_ReplyTweetMessage  --> " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Reply()-- locker_que_ReplyTweetMessage -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ All Loaded Tweet Messages Used ]");
                                        //break;
                                    }
                                }

                                //Code by Puja
                                #region Code for Reply Per Tweet
                                //if (IsReplyPerTweet)
                                //{

                                //    if (lstTemp.Count == 0)
                                //    {
                                //        foreach (string item in que_ReplyMessages)
                                //        {
                                //            lstTemp.Add(item);

                                //        }
                                //    }
                                //    if (counter > lstTemp.Count - 1)
                                //    {
                                //        counter = 0;
                                //    }
                                //    tweetMessage = lstTemp[counter];
                                //    counter++;


                                //} 
                                #endregion

                            }

                            lock (locker_que_keywyordStructData)
                            {
                                if (List_of_struct_Keydatafrom_tweetData_list.Count > 0)
                                {
                                    try
                                    {
                                        TweeetData = List_of_struct_Keydatafrom_tweetData_list.Dequeue();
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply()-- locker_que_ReplyTweetMessage  --> " + ex.Message, Globals.Path_TweetingErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Reply()-- locker_que_ReplyTweetMessage -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }
                                }
                                else
                                {

                                    TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                                    TweetAccountManager.static_lst_Struct_TweetData = tweetScrapper.GetTweetData(ReplyKeyword);
                                    foreach (TwitterDataScrapper.StructTweetIDs item in TweetAccountManager.static_lst_Struct_TweetData)
                                    {
                                        TweetAccountManager.List_of_struct_Keydatafrom_tweetData_list.Enqueue(item);
                                    }
                                    TweeetData = List_of_struct_Keydatafrom_tweetData_list.Dequeue();


                                }
                            }

                            if (!ReplyPerDay)
                            {
                                if (counter_Reply >= noOfRetweets)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Finished Replying with : " + Username + " ]");
                                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                                    break;
                                }
                            }
                            else if (ReplyPerDay)
                            {
                                if (AlreadyReply >= NoOFReplyPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Already Replied " + AlreadyReply + " ]");
                                    break;
                                }
                            }

                            //if (counter_Reply >= noOfRetweets)
                            //{
                            //    //Log("Finished Replying with : " + Username);
                            //    break;
                            //}

                            string tweetStatus;

                            tweeter.Reply(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, TweeetData.ID_Tweet, TweeetData.username__Tweet_User, tweetMessage, out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                counter_Reply++;
                                AlreadyReply++;
                                clsDBQueryManager DataBase = new clsDBQueryManager();
                                string dbTweetMessage = string.Empty;
                                try
                                {
                                     dbTweetMessage = StringEncoderDecoder.Encode(TweeetData.wholeTweetMessage);
                                }
                                catch { }
                                DataBase.InsertMessageData(Username, "Reply", TweeetData.ID_Tweet_User, dbTweetMessage);
                                Log("[ " + DateTime.Now + " ] => [ >> Replied : " + counter_Reply + " : " + tweetMessage + " by " + Username + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine( tweetMessage + " by " + Username, Globals.path_ReplyInformation);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ >>Couldn't Reply >> " + tweetMessage + " by " + Username + " ]");
                                //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
                            }

                            Log("[ " + DateTime.Now + " ] => [ Reply Delayed for " + delay + " Seconds ]");
                            Thread.Sleep(delay * 1000);

                            if (ReplyPerDay)
                            {
                                if (AlreadyReply >= NoOFReplyPerDay)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Finished Replying with : " + Username + " ]");
                                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                                    break;
                                }
                            }
                            else if (que_ReplyMessages.Count <= 0)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Finished Replying with : " + Username + " ]");
                                Log("------------------------------------------------------------------------------------------------------------------------------------------");
                                break;
                            }
                        }

                        //Log("Finished Replying with : " + Username);
                    }

                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ " + Username + ">> Account Suspended ]");
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply()  --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Reply() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
        }

        public void Reply(List<string> list_tweetMessages, int minDelay, int maxDelay)
        {
            //Login();

            //if (true)

            if (!IsLoggedIn)
            {
                Login();
            }

            if (IsNotSuspended)
            {
                if (IsLoggedIn)
                {
                    int counter_Retweet = 0;

                    foreach (TwitterDataScrapper.StructTweetIDs item in static_lst_Struct_TweetData)
                    {
                        if (counter_Retweet >= noOfRetweets)
                        {
                            //Log("Finished Replying with : " + Username);
                            break;
                        }

                        string tweetStatus;

                        int delay = 10 * 1000;
                        try
                        {
                            delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay);
                        }
                        catch { }

                        string tweetMessage = "";
                        try
                        {
                            tweetMessage = list_tweetMessages[RandomNumberGenerator.GenerateRandom(0, list_tweetMessages.Count - 1)];
                        }
                        catch (Exception ex)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Reply() -- tweetMessage -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                        }

                        tweeter.Reply(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, item.ID_Tweet, item.username__Tweet_User, tweetMessage, out tweetStatus);

                        if (tweetStatus == "posted")
                        {
                            counter_Retweet++;
                            clsDBQueryManager DataBase = new clsDBQueryManager();
                            string dbTweetMessage = string.Empty;
                            try
                            {
                                dbTweetMessage = StringEncoderDecoder.Encode(tweetMessage);
                            }
                            catch { }
                            DataBase.InsertMessageData(Username, "Reply", item.ID_Tweet, dbTweetMessage);
                            Log("[ " + DateTime.Now + " ] => [ >> Replied : " + counter_Retweet + " : " + tweetMessage + " by " + Username + " ]");
                            //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfullyFollowAccounts);
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ >>Couldn't Reply >> " + tweetMessage + " by " + Username + " ]");
                            //GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedToFollowAccounts);
                        }

                        Log("[ " + DateTime.Now + " ] => [ Reply Delayed for " + delay + " Seconds ]");
                        Thread.Sleep(delay * 1000);
                    }

                    Log("[ " + DateTime.Now + " ] => [ Finished Replying with : " + Username + " ]");
                    Log("------------------------------------------------------------------------------------------------------------------------------------------");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Couldn't log in with " + Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                }
            }
        }

        public void BlockUsers()
        {
            if (string.IsNullOrEmpty(IPPort))
            {
                IPPort = "0";
            }

            if (!IsLoggedIn)
            {
                Login();
            }

            if (IsNotSuspended)
            {
                string Page2 = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/#!/following"), "", "");
                postAuthenticityToken = GlobusHttpHelper.ParseJson(Page2, "postAuthenticityToken");
                string Page1 = globusHttpHelper.postFormDataIP(new Uri("https://api.twitter.com/1/blocks/create.json"), "user_id=556223103&post_authenticity_token=" + postAuthenticityToken, "Referer: https://api.twitter.com/receiver.html", IPAddress, Convert.ToInt32(IPPort), IPUsername, IPpassword);
            }
        }

        public static string PostAuthenticityToken(string data, string paramName)
        {
            string value = string.Empty;
            int startIndx = data.IndexOf(paramName);
            if (startIndx > 0)
            {
                int indexstart = startIndx + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");

                if (value.Contains(paramName))
                {
                    try
                    {
                        string[] getOuthentication = System.Text.RegularExpressions.Regex.Split(data, "\"postAuthenticityToken\":\"");
                        string[] authenticity = Regex.Split(getOuthentication[1], ",");

                        if (authenticity[0].IndexOf("\"") > 0)
                        {
                            int indexStart1 = authenticity[0].IndexOf("\"");
                            string start = authenticity[0].Substring(0, indexStart1);
                            value = start.Replace("\"", "").Replace(":", "");
                        }
                        //else
                        //{
                        //    //value = getOuthentication[0].Substring(getOuthentication[1].IndexOf(":") + 2, getOuthentication[1].IndexOf(",\"currentUser") - getOuthentication[1].IndexOf(":") - 3);
                        //    value = authenticity[0].Replace("\"", "").Replace(":", "");
                        //}
                    }
                    catch { };
                }

                return value;
            }
            else
            {
                string[] array = Regex.Split(data, "<input type=\"hidden\"");
                foreach (string item in array)
                {
                    if (item.Contains("authenticity_token"))
                    {
                        int startindex = item.IndexOf("value=\"");
                        if (startindex > 0)
                        {
                            string start = item.Substring(startindex).Replace("value=\"", "");
                            int endIndex = start.IndexOf("\"");
                            string end = start.Substring(0, endIndex);
                            value = end;
                            break;
                        }
                    }
                }

            }
            return value;
        }

        public List<string> GetFollowers(ref GlobusHttpHelper Httphelper)
        {
            TwitterDataScrapper followerScrapper = new TwitterDataScrapper();
            string returnStatus = string.Empty;
            list_Followers = followerScrapper.GetFollowers1(userID,Screen_name, out returnStatus, Httphelper);
            return list_Followers;
        }

        public List<string> GetFollowings(ref GlobusHttpHelper Httphelper)
        {
            TwitterDataScrapper followingScrapper = new TwitterDataScrapper();
            followingScrapper.CounterDataNo = noOfUnfollows;
            string returnStatus = string.Empty;
            //list_Followings = followingScrapper.GetFollowings_New(Screen_name, out returnStatus, ref  Httphelper);//GetFollowings_NewForUnfollower
            list_Followings = followingScrapper.GetFollowings_NewForUnfollower(Screen_name, out returnStatus, ref  Httphelper);
            return list_Followings;
        }

        
        public List<string> GetFollowerYourFollowers(ref GlobusHttpHelper Httphelper)
        {
            TwitterDataScrapper followerScrapper = new TwitterDataScrapper();
            string returnStatus = string.Empty;
          
           list_Followers = followerScrapper.GetFollowYourFollowers(userID, Screen_name, out returnStatus, Httphelper);

            return list_Followers;
        }

        public List<string> GetNonFollowings()
        {
            Log("[ " + DateTime.Now + " ] => [ Process Running.. Get Follower ID ]");
            List<string> followers = GetFollowers(ref globusHttpHelper);
            Log("[ " + DateTime.Now + " ] => [ Process Completed for Follower ID ]");

            Log("[ " + DateTime.Now + " ] => [ Process Running.. Get Following ID ]");
            List<string> followings = GetFollowings(ref globusHttpHelper);
            Log("[ " + DateTime.Now + " ] => [ Process Completed for Following ID  ]");
            List<string> nonFollowings = followings.Except(followers).ToList();
            return nonFollowings;
        }

        public List<string> GetFollowYourFollowersList()
        {
            
            List<string> followers = new List<string>();
            Log("[ " + DateTime.Now + " ] => [ Process Running.. Get Follow your Followers ID ]");
            
            followers = GetFollowerYourFollowers(ref globusHttpHelper);
            Log("[ " + DateTime.Now + " ] => [ Process Completed for Follow your Followers ID ]");
             
            return followers;
            
           
        }


        public List<string> GetFollowerListForDirectMessage()
        {
            Log("[ " + DateTime.Now + " ] => [ Process Running.. Get  Followers ID ]");
            List<string> followers = GetFollowers(ref globusHttpHelper);
            Log("[ " + DateTime.Now + " ] => [ Process Completed for  Followers ID ]");
            return followers;
        }

        public List<string> GetNonFollowingsBeforeSpecifiedDate(int noOfDays, ref TweetAccountManager AcManger)
        {
            TwitterDataScrapper followingScrapper = new TwitterDataScrapper();
            string status = string.Empty;
            followingScrapper.CounterDataNo = noOfUnfollows;
            List<string> followers = followingScrapper.GetFollowers_New(AcManger.Screen_name, out status, ref AcManger.globusHttpHelper);
            List<string> followings = followingScrapper.GetFollowings_New(AcManger.Screen_name, out status, ref AcManger.globusHttpHelper);

            List<string> nonFollowings = followings.Except(followers).ToList();

            List<string> requiredNonFollowingList = new List<string>();

            ///Get list of Already Followings
            clsDBQueryManager queryManager = new clsDBQueryManager();
            DataTable dt_AlreadyFollowed = queryManager.SelectFollowData(Username);
            foreach (DataRow item in dt_AlreadyFollowed.Rows)
            {
                string user_AlreadyFollowed = item["following_id"].ToString();

                if (nonFollowings.Exists(s => ((s.Split(':')[0]) == user_AlreadyFollowed)))
                {
                    DateTime dt_Now = DateTime.Today;

                    string DateFollowed = item["DateFollowed"].ToString();
                    DateTime dt_DateFollowed1 = Convert.ToDateTime(DateFollowed);
                    //DateTime dt_DateFollowed = DateTime.Parse(String.Format("{0:d/M/yyyy HH:mm:ss}", dt_DateFollowed1));
                    DateTime dt_DateFollowed = dt_DateFollowed1;

                    TimeSpan dt_Difference = dt_Now.Subtract(dt_DateFollowed);
                    double dt_Difference1 = dt_Difference.Days;

                    if (dt_Difference.Days >= noOfDays)
                    {
                        requiredNonFollowingList.Add(user_AlreadyFollowed);
                    }
                }
            }


            return requiredNonFollowingList;
        }

        
        public void StartWaitAndReply(string tweetKeyword)
        {
            List<string> lstTweetImages = new List<string>();
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                int counterDataScraped = 0;
               
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        TwitterDataScrapper tweetScrapper = new TwitterDataScrapper();
                        lst_Struct_TweetData = tweetScrapper.GetTweetData_WaitReply(tweetKeyword);
                        Log("[ " + DateTime.Now + " ] => [ Extracted " + lst_Struct_TweetData.Count + " Tweet Ids ]");
                        if (IsTweetWithImage)
                        {
                            if (!Directory.Exists(ImageFolderPath))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Image Folder Does't Exist. ]");
                                return;
                            }
                            ///Load Image ...
                            lstTweetImages = GetImagePathFromFolder(ImageFolderPath);

                            if (lstTweetImages.Count != 0)
                            {
                                if (lstTweetImages.Count < listTweetMessages.Count)
                                {
                                    int remaining = listTweetMessages.Count - lstTweetImages.Count;
                                    for (int i = 0; i < remaining; i++)
                                    {
                                        string Imagepath = string.Empty;
                                        try
                                        {
                                            Imagepath = lstTweetImages[i];
                                        }
                                        catch (Exception ex)
                                        {
                                            i = 0;
                                            Imagepath = lstTweetImages[i];
                                        }
                                        lstTweetImages.Add(Imagepath);
                                    }
                                }
                                else
                                {
                                    ///if  images is greater than messages. 
                                }
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Please Folder is Empty. Please add images in :-  " + ImageFolderPath + " ]");
                                return;
                            }

                            ///print number of Images ...
                            Log("[ " + DateTime.Now + " ] => [ " + lstTweetImages.Count() + " Images uploaded. ]");
                            foreach (string item in lstTweetImages)
                            {
                                que_ImagePath_WaitAndReply.Enqueue(item);
                            }
                        }
                        while (que_TweetMessages_WaitAndReply.Count > 0)
                        {
                            if (que_ReplyMessages_WaitAndReply.Count <= 0)
                            {
                                break;
                            }
                            if (que_TweetMessages_WaitAndReply.Count <= 0)
                            {
                                break;
                            }

                            #region MyRegion
                            ////Tweet
                            ////listTweetMessages = Randomiser.Shuffle(listTweetMessages).ToList();
                            //foreach (string item in listTweetMessages)
                            //{
                            //    que_TweetMessages.Enqueue(item);
                            //}
                            ////listTweetMessages = Randomiser.Shuffle(listTweetMessages).ToList();
                            //foreach (string item in listReplyMessages)
                            //{
                            //    que_ReplyMessages.Enqueue(item);
                            //} 
                            #endregion

                            int countTweetsSent = 0;
                            int interval = WaitAndReplyInterval;

                            while (countTweetsSent < noOfTweetsPerReply)
                            {
                                //Tweet
                                string tweetStatus;

                                string tweetMessage = "";
                                string ImagePath="";

                                #region MyRegion
                                //if (que_TweetMessages.Count == 0)
                                //{
                                //    //listTweetMessages = Randomiser.Shuffle(listTweetMessages).ToList();
                                //    foreach (string item in listTweetMessages)
                                //    {
                                //        que_TweetMessages.Enqueue(item);
                                //    }
                                //} 
                                #endregion

                                lock (locker_que_TweetMessages_WaitAndReply)
                                {
                                    if (que_TweetMessages_WaitAndReply.Count > 0)
                                    {
                                        try
                                        {
                                            tweetMessage = que_TweetMessages_WaitAndReply.Dequeue();
                                            ImagePath = que_ImagePath_WaitAndReply.Dequeue();
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- locker_que_TweetMessages_WaitAndReply --  --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  StartWaitAndReply() -- locker_que_TweetMessages_WaitAndReply -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [All Loaded Tweet Messages Used ]");
                                        break;
                                    }
                                }

                                if (IsTweetWithImage)
                                {
                                    tweeter.TweetMessageWithImage(ref globusHttpHelper, postAuthenticityToken, tweetMessage, ImagePath, out tweetStatus);
                                    if (tweetStatus == "posted")
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Tweeted : " + tweetMessage + " with image:"+ImagePath+" by " + Username + " ]");
                                        clsDBQueryManager DataBase = new clsDBQueryManager();
                                        string dbTweetMessage = StringEncoderDecoder.Encode(tweetMessage);
                                        DataBase.InsertMessageData(Username, "Tweet", "", dbTweetMessage);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword+":"+tweetMessage, Globals.path_SuccessfullyTweetAccounts);
                                        countTweetsSent++;
                                        if (!waitAndReplyIsIntervalInsec)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                            Thread.Sleep(interval);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                interval = RandomNumberGenerator.GenerateRandom(waitAndReplyMinInterval, waitAndReplyMaxInterval);
                                            }
                                            catch { }
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + interval + " seconds. ]");
                                            interval = interval * 1000;
                                            Thread.Sleep(interval);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Couldn't Post : " + tweetMessage + " with image:"+ImagePath+" by " + Username + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_FailedToTweetAccounts);
                                        countTweetsSent++;
                                        //Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                        //Thread.Sleep(interval);
                                        if (!waitAndReplyIsIntervalInsec)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                            Thread.Sleep(interval);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                interval = RandomNumberGenerator.GenerateRandom(waitAndReplyMinInterval, waitAndReplyMaxInterval);
                                            }
                                            catch { }
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + interval + " seconds. ]");
                                            interval = interval * 1000;
                                            Thread.Sleep(interval);
                                        }
                                    }
                                }
                                else
                                {
                                    tweeter.Tweet(ref globusHttpHelper, postAuthenticityToken, tweetMessage, out tweetStatus);
                                    if (tweetStatus == "posted")
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Tweeted : " + tweetMessage + " by " + Username + " ]");
                                        clsDBQueryManager DataBase = new clsDBQueryManager();
                                        string dbTweetMessage = StringEncoderDecoder.Encode(tweetMessage);
                                        DataBase.InsertMessageData(Username, "Tweet", "", dbTweetMessage);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_SuccessfullyTweetAccounts);
                                        countTweetsSent++;
                                        //Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                        //Thread.Sleep(interval);
                                        if (!waitAndReplyIsIntervalInsec)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                            Thread.Sleep(interval);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                interval = RandomNumberGenerator.GenerateRandom(waitAndReplyMinInterval, waitAndReplyMaxInterval);
                                            }
                                            catch { }
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + interval + " seconds. ]");
                                            interval = interval * 1000;
                                            Thread.Sleep(interval);
                                        }
                                    }
                                    else
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Couldn't Post : " + tweetMessage + " by " + Username + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + tweetMessage, Globals.path_FailedToTweetAccounts);
                                        countTweetsSent++;
                                        //Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                        //Thread.Sleep(interval);
                                        if (!waitAndReplyIsIntervalInsec)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                            Thread.Sleep(interval);
                                        }
                                        else
                                        {
                                            try
                                            {
                                                interval = RandomNumberGenerator.GenerateRandom(waitAndReplyMinInterval, waitAndReplyMaxInterval);
                                            }
                                            catch { }
                                            Log("[ " + DateTime.Now + " ] => [ Delay For : " + interval + " seconds. ]");
                                            interval = interval * 1000;
                                            Thread.Sleep(interval);
                                        }
                                    } 
                                }
                            }

                            //Reply

                            if (lst_Struct_TweetData.Count == 0)
                            {
                                lst_Struct_TweetData = tweetScrapper.GetTweetData_New(tweetKeyword);
                            }
                            if (lst_Struct_TweetData.Count != 0)
                            {
                                int counter_Reply = 1;

                                //foreach (TwitterDataScrapper.StructTweetIDs item in lst_Struct_TweetData)
                                //{
                                while (countTweetsSent == counter_Reply * noOfTweetsPerReply && que_ReplyMessages_WaitAndReply.Count > 0)
                                {
                                    string replyStatus;
                                    string replyMessage = "";

                                    #region MyRegion
                                    //if (counter_Reply >= 1)
                                    //{
                                    //    break;
                                    //}
                                    //if (que_ReplyMessages.Count == 0)
                                    //{
                                    //    //listReplyMessages = Randomiser.Shuffle(listReplyMessages).ToList();
                                    //    foreach (string ReplyMessage in listReplyMessages)
                                    //    {
                                    //        que_ReplyMessages.Enqueue(ReplyMessage);
                                    //    }
                                    //} 
                                    #endregion

                                    lock (locker_que_ReplyMessage_WaitAndReply)
                                    {
                                        if (que_ReplyMessages_WaitAndReply.Count > 0)
                                        {
                                            try
                                            {
                                                //replyMessage = listReplyMessages[RandomNumberGenerator.GenerateRandom(0, listReplyMessages.Count - 1)];
                                                replyMessage = que_ReplyMessages_WaitAndReply.Dequeue();
                                            }
                                            catch (Exception ex)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- locker_que_ReplyMessage_WaitAndReply --  --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  StartWaitAndReply() -- locker_que_ReplyMessage_WaitAndReply -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                                            }
                                        }
                                        else
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ All Loaded Reply Messages Used ]");
                                            break;
                                        }
                                    }


                                    tweeter.Reply(ref globusHttpHelper, pgSrc_Profile, postAuthenticityToken, lst_Struct_TweetData[counterDataScraped].ID_Tweet, lst_Struct_TweetData[counterDataScraped].username__Tweet_User, replyMessage, out replyStatus);

                                    if (replyStatus == "posted")
                                    {
                                        counter_Reply++;
                                        clsDBQueryManager DataBase = new clsDBQueryManager();
                                        string dbTweetMessage = StringEncoderDecoder.Encode(lst_Struct_TweetData[counterDataScraped].wholeTweetMessage);
                                        DataBase.InsertMessageData(Username, "Reply", lst_Struct_TweetData[counterDataScraped].ID_Tweet_User, lst_Struct_TweetData[counterDataScraped].wholeTweetMessage);
                                        Log("[ " + DateTime.Now + " ] => [ Replied : " + replyMessage + " by " + Username + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + replyMessage, Globals.path_SuccessfullyRepliedAccounts);
                                    }
                                    else
                                    {
                                        counter_Reply++;
                                        Log("[ " + DateTime.Now + " ] => [ Couldn't Reply : " + replyMessage + " by " + Username + " ]");
                                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + ":" + replyMessage, Globals.path_FailedRepliedAccounts);
                                    }
                                    //Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes +  " Minutes ]");
                                    //Thread.Sleep(interval);
                                    if (!waitAndReplyIsIntervalInsec)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Delay For : " + TimeSpan.FromMilliseconds(interval).Minutes + " Minutes ]");
                                        Thread.Sleep(interval);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            interval = RandomNumberGenerator.GenerateRandom(waitAndReplyMinInterval, waitAndReplyMaxInterval);
                                        }
                                        catch { }
                                        Log("[ " + DateTime.Now + " ] => [ Delay For : " + interval + " seconds. ]");
                                        interval = interval * 1000;
                                        Thread.Sleep(interval);
                                    }
                                    counterDataScraped++;
                                    if (countTweetsSent <= noOfTweetsPerReply)
                                    {
                                        countTweetsSent = 0;
                                        break;
                                    }

                                }

                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Tweet Data is not available for Tweet Keyword:- " + tweetKeyword + " ]");
                            }

                        }
                        if (que_ReplyMessages_WaitAndReply.Count <= 0)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Remaining Reply Message : " + que_ReplyMessages_WaitAndReply.Count + " ]");
                            Log("[ " + DateTime.Now + " ] => [ Finished Reply Messages ]");
                            Log("------------------------------------------------------------------------------------------------------------------------------------------");
                        }
                        if (que_TweetMessages_WaitAndReply.Count <= 0)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Remaining Tweet Message: " + que_TweetMessages_WaitAndReply.Count + " ]");
                            Log("[ " + DateTime.Now + " ] => [ Finished Tweet Messages ]");
                            Log("------------------------------------------------------------------------------------------------------------------------------------------");
                        }

                        //Log("Finished Replying with : " + Username); 
                    }
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Couldn't log in with :" + Username + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedLoginAccounts);
                }
            }
            catch (Exception ex)
            {
                //Log("[ " + DateTime.Now + " ] => [ >>Error :- TweetAccountManager  (StartWaitAndReply)>> " + ex.Message + " ]");
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- StartWaitAndReply  --> " + ex.Message, Globals.Path_WaitNreplyErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  StartWaitAndReply() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
        }

        private List<string> GetImagePathFromFolder(string folderPath)
        {
            List<string> _lstTemimageFilePath = new List<string>();

            string[] files = System.IO.Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                foreach (string item in files)
                {
                    if (item.Contains(".ini"))
                    {
                        continue;
                    }
                    _lstTemimageFilePath.Add(item);
                }
            }
            else
            {
                _lstTemimageFilePath.Clear();
            }

            return _lstTemimageFilePath;
        }

        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public string DirectMessage(string Screen_name)
        {
            string ReturnString = string.Empty;
            try
            {
                string MessageText = string.Empty;
                string PostedMessage = string.Empty;

                lock (locker_DirectMessage)
                {
                    if (que_DirectMessage.Count > 0)
                    {
                        MessageText = que_DirectMessage.Dequeue();
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [All Loaded Reply Messages Used ]");
                    }
                }

                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/#!/followers"), "", "");
                string PostAuthenticityToken = string.Empty;
                try
                {
                    int startIndex = PageSource.IndexOf("\"postAuthenticityToken\"");
                    if (startIndex > 0)
                    {
                        string Start = PageSource.Substring(startIndex);
                        int endIndex = Start.IndexOf("\"})");
                        string End = Start.Substring(0, endIndex).Replace("\"postAuthenticityToken\"", "").Replace(":", "").Replace("\"", "");
                        PostAuthenticityToken = End;
                    }
                    startIndex = PageSource.IndexOf("formAuthenticityToken");
                    if (startIndex > 0)
                    {
                        string start = PageSource.Substring(startIndex).Replace("formAuthenticityToken", "");
                        int endIndex = start.IndexOf("\",");
                        string end = start.Substring(0, endIndex).Replace("\"", "").Replace(":", "");
                        PostAuthenticityToken = end;
                    }

                }
                catch (Exception ex)
                {
                    ReturnString = "Error";
                    return ReturnString;
                }
                string Post = "https://api.twitter.com/1/direct_messages/new.json";
                string PostData = string.Empty;
                if (!NumberHelper.ValidateNumber(Screen_name))
                {
                    PostData = "screen_name=" + Screen_name + "&text=" + MessageText;
                }
                else
                {
                    PostData = "user_id=" + userID + "&text=" + MessageText;
                }

                string pageSource = globusHttpHelper.postFormData(new Uri(Post), PostData, "", string.Empty, "", "true", "");
                if (pageSource.Contains("sender\":") && pageSource.Contains(MessageText))
                {
                    ReturnString = "Success: " + MessageText;
                    clsDBQueryManager DataBase = new clsDBQueryManager();
                    string dbTweetMessage = StringEncoderDecoder.Encode(MessageText);
                    string OtherUser = Screen_name;
                    if (string.IsNullOrEmpty(Screen_name))
                    {
                        OtherUser = userID;
                    }

                    DataBase.InsertMessageData(Username, "Message", OtherUser, dbTweetMessage);
                }
            }
            catch (Exception ex)
            {
                ReturnString = "Error";
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- DirectMessage() --> " + ex.Message, Globals.Path_DMErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager -- DirectMessage() -- " + Username + " --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            return ReturnString;
        }

        public void ScrapTweetAndReply()
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        ReplyInterface.ReplyInterface obj_ReplyInterface = new ReplyInterface.ReplyInterface(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_ReplyInterface.GetScrapTweetAndReply(ref globusHttpHelper, ref userID, ref Username, ref Screen_name, ref postAuthenticityToken);
                    }
                }
            }
            catch
            {
            }
        }

        public void LoginRandomiser(ref Randomiser.Randomiser obj_Randomiser)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        //Randomiser.Randomiser obj_Randomiser = new Randomiser.Randomiser(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_Randomiser.StartRandomiser(ref globusHttpHelper, ref userID, ref Username, ref Screen_name, ref postAuthenticityToken);
                    }
                }
            }
            catch
            {
            }
        }

        public void Mention_ScrapTweetAndReply()
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        MentionsReplyInterface.MentionsReplyInterface obj_ReplyInterface = new MentionsReplyInterface.MentionsReplyInterface(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_ReplyInterface.GetScrapTweetAndReply(ref globusHttpHelper, ref userID, ref Username, ref Screen_name, ref postAuthenticityToken);
                    }
                }
            }
            catch
            {
            }
        }

        public void GetMentions()
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        Randomiser.Randomiser obj_Randomiser = new Randomiser.Randomiser(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_Randomiser.GetMentions(ref globusHttpHelper, ref userID, ref Username, ref Screen_name, ref postAuthenticityToken);
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Account Suspended With User Name :" + Username + " ]");
                    }
                }
            }
            catch
            {
            }
        }

        public void ReplyThroughReplyInterface(string postAuthenticityToken, string tweetID, string tweetUserName, string screenName, string tweetMessage, string userName)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        ReplyInterface.ReplyInterface obj_ReplyInterface = new ReplyInterface.ReplyInterface(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_ReplyInterface.Reply(ref globusHttpHelper, postAuthenticityToken, tweetID, tweetUserName, screenName, tweetMessage, userName);
                    }
                }
            }
            catch
            {
            }
        }

        public void Reply_MentionReplyInterface(string postAuthenticityToken, string tweetID, string tweetUserName, string screenName, string tweetMessage, string userName)
        {
            try
            {
                if (!IsLoggedIn)
                {
                    Login();
                }
                if (IsLoggedIn)
                {
                    if (IsNotSuspended)
                    {
                        MentionsReplyInterface.MentionsReplyInterface obj_MentionsReplyInterface = new MentionsReplyInterface.MentionsReplyInterface(Username, userID, Password, Screen_name, IPAddress, IPPort, IPUsername, IPpassword);
                        obj_MentionsReplyInterface.Reply(ref globusHttpHelper, postAuthenticityToken, tweetID, tweetUserName, screenName, tweetMessage, userName);
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

        private void LogArabFollower(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            //logEvents_static.LogText(eventArgs);
            logEventsArabFollower.LogText(eventArgs);
        }

    }
}
