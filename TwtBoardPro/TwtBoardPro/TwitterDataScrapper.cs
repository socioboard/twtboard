using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BaseLib;
using System.Data;
using Newtonsoft.Json.Linq;
using Globussoft;
using System.Threading;
using Chilkat;
using System.IO;

namespace twtboardpro
{
    public class TwitterDataScrapper
    {
        public struct StructTweetIDs
        {
            public string ID_Tweet { get; set; }

            public string ID_Tweet_User { get; set; }

            public string username__Tweet_User { get; set; }

            public string wholeTweetMessage { get; set; }
        }

        public static List<StructTweetIDs> lst_structTweetIDs { get; set; }

        //public static List<string> lstTweetIds;

        public static int noOfRecords = 20;

        public static int noOfstatus = 20;

        public static int Percentage = 40;

        public static int NoOfFollowingsToBeunfollowed = 0;

        Globussoft.GlobusHttpHelper globushttpHelper = new Globussoft.GlobusHttpHelper();
        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();

        public static int TweetExtractCount = 10;

        public static int RetweetExtractcount = 10;

        public static bool RemoveRTMSg = false;

        public  bool RetweetFromUserName = false;

        public static bool removeAtMentions = false;

        public  BaseLib.Events logEvents = new BaseLib.Events();

        public  int CounterDataNo = 0;

        public static Queue<string> queTweetId = new Queue<string>();
        public static bool IsRetweetWithFovieteWithImages = false;

        //public static int blackListedUserCount = 0;
        //public static int whiteListedUserCount = 0;

        public static string GetUserIDFromUsername(string username, out string Status)
        {
            string GetStatus = string.Empty;
            Globussoft.GlobusHttpHelper httpHelper = new Globussoft.GlobusHttpHelper();

            clsDBQueryManager DB = new clsDBQueryManager();
            DataSet ds = DB.GetUserId(username);
            string user_id = string.Empty;

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                    {
                        user_id = dataRow.ItemArray[0].ToString();
                        Status = "No Error";
                        return user_id;
                    }
                }
            }
            catch { };

            try
            {
                #region gs httpHelper code
                //if (username.Contains("@"))
                //{

                //}
                //string pagesource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/users/show.xml?screen_name=" + username), "api.twitter.com", ""); 
                #endregion
                string id = string.Empty;
                string pagesource = string.Empty;

                ChilkatHttpHelpr httpHelper1 = new ChilkatHttpHelpr();
                if (NumberHelper.ValidateNumber(username))
                {
                    pagesource = httpHelper1.GetHtml("https://api.twitter.com/1/users/show.xml?user_id=" + username);
                    if (string.IsNullOrEmpty(pagesource))
                    {
                        pagesource = httpHelper1.GetHtml("https://api.twitter.com/1/users/show.xml?user_id=" + username);
                    }
                }
                else
                {
                    pagesource = httpHelper1.GetHtml("https://api.twitter.com/1/users/show.xml?screen_name=" + username);
                    if (string.IsNullOrEmpty(pagesource))
                    {
                        pagesource = httpHelper1.GetHtml("https://api.twitter.com/1/users/show.xml?screen_name=" + username);
                    }
                }

                if (!pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour.") && !pagesource.Contains("Sorry, that page does not exist") && !pagesource.Contains("User has been suspended"))
                {
                    if (NumberHelper.ValidateNumber(username))
                    {
                        int length = pagesource.IndexOf("</screen_name>");
                        username = pagesource.Substring(pagesource.IndexOf("<screen_name>"), length - pagesource.IndexOf("<screen_name>")).Replace("<screen_name>", "");
                        user_id = username;
                        GetStatus = "No Error";
                    }
                    else
                    {
                        int length = pagesource.IndexOf("</id>");
                        id = pagesource.Substring(pagesource.IndexOf("<id>"), length - pagesource.IndexOf("<id>")).Replace("<id>", "");
                        user_id = id;
                        GetStatus = "No Error";
                    }
                }
                else if (pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    GetStatus = "Rate limit exceeded";
                }
                else if (pagesource.Contains("Sorry, that page does not exist"))
                {
                    GetStatus = "Sorry, that page does not exist";
                }
                else if (pagesource.Contains("User has been suspended"))
                {
                    GetStatus = "User has been suspended";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserIDFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserIDFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                GetStatus = "Error";
            }
            Status = GetStatus;
            return user_id;
        }

        public static string GetUsernameToUserID_New(string username, out string Status, ref GlobusHttpHelper HttpHelper)
        {
            string GetStatus = string.Empty;

            clsDBQueryManager DB = new clsDBQueryManager();
            DataSet ds = DB.GetUserId(username);
            string user_id = string.Empty;

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                    {
                        user_id = dataRow.ItemArray[0].ToString();
                        Status = "No Error";
                        return user_id;
                    }
                }
            }
            catch { };

            try
            {
                string id = string.Empty;
                string pagesource = string.Empty;

                pagesource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + username), "", "");

                if (!pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour.") && !pagesource.Contains("Sorry, that page does not exist") && !pagesource.Contains("User has been suspended"))
                {
                    //int startindex = pagesource.IndexOf("\"stats js-mini-profile-stats \" data-user-id=\"");
                    ////string start = pagesource.Substring(startindex).Replace("stats js-mini-profile-stats\" data-user-id=\"", "");
                    //string start = pagesource.Substring(startindex).Replace("\"stats js-mini-profile-stats \" data-user-id=\"", "");
                    //int endindex = start.IndexOf("\"");
                    //string end = start.Substring(0, endindex);
                    //user_id = end;

                    try
                    {
                        int startindex = pagesource.IndexOf("profile_id");
                        string start = pagesource.Substring(startindex).Replace("profile_id", "");
                        int endindex = start.IndexOf(",");
                        string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Trim();
                        user_id = end.Trim();
                    }
                    catch { }

                    if (string.IsNullOrEmpty(user_id))
                    {
                        try
                        {
                            int startindex = pagesource.IndexOf("ProfileTweet-authorDetails\">");
                            string start = pagesource.Substring(startindex).Replace("ProfileTweet-authorDetails\">", "");
                            int endindex = start.IndexOf("\">");
                            string end = start.Substring(start.IndexOf("data-user-id="), endindex - start.IndexOf("data-user-id=")).Replace("data-user-id=", "").Replace("\"", "");
                            user_id = end.Trim();
                        }
                        catch { }
                    }
                }
                else if (pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    GetStatus = "Rate limit exceeded";
                }
                else if (pagesource.Contains("Sorry, that page does not exist"))
                {
                    GetStatus = "Sorry, that page does not exist";
                }
                else if (pagesource.Contains("User has been suspended"))
                {
                    GetStatus = "User has been suspended";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserIDFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserIDFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                GetStatus = "Error";
            }
            Status = "No Error";
            return user_id;
        }

        public static string GetUserNameFromUserId(string userid)
        {
            string username = string.Empty;

            ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();
            clsDBQueryManager DB = new clsDBQueryManager();
            DataSet ds = DB.GetUserName(userid);
            string user_id = string.Empty;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                {
                    user_id = dataRow.ItemArray[0].ToString();
                    return user_id;
                }
            }
            try
            {
                string PageSource = string.Empty;
                if (!string.IsNullOrEmpty(userid) && NumberHelper.ValidateNumber(userid))
                {
                    PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.xml?user_id=" + userid + "&include_entities=true");
                    if (!PageSource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                    {
                        try
                        {
                            int startIndex = PageSource.IndexOf("<screen_name>");
                            if (startIndex > 0)
                            {
                                string Start = PageSource.Substring(startIndex);
                                int endIndex = Start.IndexOf("</screen_name>");
                                string End = Start.Substring(0, endIndex);
                                username = End.Replace("<screen_name>", "");
                            }

                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserNameFromUserId() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserNameFromUserId() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    else
                    {
                        username = "Rate Limit Exceeded";
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserNameFromUserId() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserNameFromUserId() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return username;
        }

        public static string GetUserNameFromUserId_New(string UserId, out string Status, ref GlobusHttpHelper HttpHelper)
        {
            string _UserName = string.Empty;


            clsDBQueryManager DB = new clsDBQueryManager();
            DataSet ds = DB.GetUserName(UserId);

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                    {
                        _UserName = dataRow.ItemArray[0].ToString();
                        Status = "No Error";
                        return _UserName;
                    }
                }
            }
            catch { };



            try
            {
                string pagesource = string.Empty;

                string Url = "https://twitter.com/account/redirect_by_id?id=" + UserId;


                pagesource = HttpHelper.getHtmlfromUrl(new Uri(Url), "", "");

                if (!pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour.") && !pagesource.Contains("Sorry, that page does not exist") && !pagesource.Contains("User has been suspended"))
                {
                    int startindex = pagesource.IndexOf("screen_name");
                    string start = pagesource.Substring(startindex).Replace("screen_name", "");
                    int endindex = start.IndexOf("}");
                    string end = start.Substring(0, endindex);
                    _UserName = end.Replace("<s>", string.Empty).Replace("</s>", string.Empty).Replace("</span>", string.Empty).Replace("&quot;", string.Empty).Replace(":", string.Empty);

                    if (string.IsNullOrEmpty(_UserName))
                    {
                        _UserName = string.Empty;
                        string[] Arr_Name = (HttpHelper.gResponse.ResponseUri.ToString()).Split('/');
                        _UserName = Arr_Name[Arr_Name.Count() - 1];
                    }
                }
                else if (pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    Status = "Rate limit exceeded";
                }
                else if (pagesource.Contains("Sorry, that page does not exist"))
                {
                    Status = "Sorry, that page does not exist";
                }
                else if (pagesource.Contains("User has been suspended"))
                {
                    Status = "User has been suspended";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserIDFromUsername() -- " + UserId + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserIDFromUsername() -- " + UserId + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                Status = "Error";
            }
            Status = "No Error";

            return _UserName;
        }

        public static bool GetStatusLinks(string username)
        {
            int counter = 0;
            Globussoft.GlobusHttpHelper httpHelper = new Globussoft.GlobusHttpHelper();
            string user_id = string.Empty;
            bool tooManyLinks = true;
            try
            {
                string text = string.Empty;
                string PageSource = string.Empty;
                //http://a0.twimg.com/sticky/default_profile_images/default_profile_0_normal.png
                if (!string.IsNullOrEmpty(username) && NumberHelper.ValidateNumber(username))
                {
                    PageSource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/statuses/user_timeline.xml?include_entities=true&inc%E2%80%8C%E2%80%8Blude_rts=true&include_rts=1&user_id=" + username + "&count=" + noOfstatus), "", "");
                }
                else
                {
                    PageSource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/statuses/user_timeline.xml?include_entities=true&inc%E2%80%8C%E2%80%8Blude_rts=true&include_rts=1&screen_name=" + username + "&count=" + noOfstatus), "", "");
                }

                if (!string.IsNullOrEmpty(PageSource) && PageSource.Contains("Sorry, that page does not exist"))
                {
                    return tooManyLinks;
                }

                if (!string.IsNullOrEmpty(PageSource))
                {
                    string[] statusArray = Regex.Split(PageSource, "<status>");
                    statusArray = statusArray.Skip(1).ToArray();


                    foreach (string Status in statusArray)
                    {
                        try
                        {
                            int indexStart = Status.IndexOf("<text>");
                            if (Status.Contains("<profile_image_url>"))
                            {
                                string start = Status.Substring(indexStart);
                                int endIndex = start.IndexOf("</text>");
                                string end = start.Substring(0, endIndex);
                                text = end.Replace("<text>", "");
                            }

                            List<string> GetHrefs = GlobusRegex.GetHrefsFromString(text);

                            if (GetHrefs.Count >= 1)
                            {
                                counter++;
                            }

                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetStatusLinks() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetStatusLinks() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                    //FollowingsFollowersRatio_user_id = (count_Followings_user_id * 100) / count_Followers_user_id;
                    int PerLinks = (counter * 100) / 20;
                    //double PerLinks = a * 100;

                    if (PerLinks > Percentage)
                    {
                        tooManyLinks = false;
                    }
                }
                else
                {
                    tooManyLinks = false;
                }

            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetStatusLinks() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetStatusLinks() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            return tooManyLinks;
        }

        public static string GetPhotoFromUsername(string username)
        {
            //Globussoft.GlobusHttpHelper httpHelper = new Globussoft.GlobusHttpHelper();
            ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();

            string user_id = string.Empty;
            string containsImage = "false";
            try
            {
                string ImageLink = string.Empty;
                string PageSource = string.Empty;
                //http://a0.twimg.com/sticky/default_profile_images/default_profile_0_normal.png
                if (!string.IsNullOrEmpty(username) && NumberHelper.ValidateNumber(username))
                {
                    //PageSource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/users/show.xml?user_id=" + username + "&include_entities=true"), "", "");
                    PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.xml?user_id=" + username + "&include_entities=true");
                }
                else
                {
                    //PageSource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/users/show.xml?screen_name=" + username + "&include_entities=true"), "", "");
                    PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.xml?screen_name=" + username + "&include_entities=true");
                }

                if (!string.IsNullOrEmpty(PageSource))
                {
                    try
                    {
                        int indexStart = PageSource.IndexOf("<profile_image_url>");
                        if (PageSource.Contains("<profile_image_url>"))
                        {
                            string start = PageSource.Substring(indexStart);
                            int endIndex = start.IndexOf("</profile_image_url>");
                            string end = start.Substring(0, endIndex);
                            ImageLink = end.Replace("<profile_image_url>", "");
                        }

                        if (!string.IsNullOrEmpty(ImageLink) && ImageLink.Contains("/sticky/default_profile_images/default_profile"))
                        {
                            containsImage = "false";
                        }
                        else if (PageSource.Contains("Sorry, that page does not exist"))
                        {
                            containsImage = "false";
                        }
                        else
                        {
                            containsImage = "true";
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                else if (PageSource.Contains("Clients may not make more than 150 requests per hour") && PageSource.Contains("Rate limit exceeded"))
                {
                    containsImage = "Rate limit exceeded";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return containsImage;
        }

        public static string GetPhotoFromUsername_New(string username)
        {
            //Globussoft.GlobusHttpHelper httpHelper = new Globussoft.GlobusHttpHelper();
            ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();

            string user_id = string.Empty;
            string containsImage = "false";
            try
            {
                string ImageLink = string.Empty;
                string PageSource = string.Empty;
                if (!string.IsNullOrEmpty(username) && NumberHelper.ValidateNumber(username))
                {
                    PageSource = httpHelper.GetHtml("https://twitter.com/account/redirect_by_id?id=" + username);
                }
                else
                {
                    PageSource = httpHelper.GetHtml("https://twitter.com/" + username);
                }

                if (!string.IsNullOrEmpty(PageSource))
                {
                    try
                    {
                        int indexStart = PageSource.IndexOf("profile-picture media-thumbnail");
                        if (PageSource.Contains("profile-picture media-thumbnail"))
                        {
                            string start = PageSource.Substring(indexStart);
                            int endIndex = start.IndexOf(" </a>");
                            string end = start.Substring(0, endIndex);

                            if (!end.Contains(".png"))
                            {
                                int StartIndex = end.IndexOf("src=");
                                string start1 = end.Substring(StartIndex);
                                int EndIndex = start1.IndexOf(">");
                                string ImageLinkData = start1.Substring(0, EndIndex);
                                ImageLink = System.Text.RegularExpressions.Regex.Split(ImageLinkData, "alt")[0].Replace("src=\"", string.Empty).Replace("\"", string.Empty);
                            }
                            else
                            {
                                ImageLink = string.Empty;
                            }
                        }

                        if (!string.IsNullOrEmpty(ImageLink) && ImageLink.Contains("profile_images"))
                        {
                            containsImage = "true";
                        }
                        else if (string.IsNullOrEmpty(ImageLink))
                        {
                            containsImage = "false";
                        }
                        else if (PageSource.Contains("Sorry, that page does not exist"))
                        {
                            containsImage = "false";
                        }
                        else
                        {
                            containsImage = "false";
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }
                else if (PageSource.Contains("Clients may not make more than 150 requests per hour") && PageSource.Contains("Rate limit exceeded"))
                {
                    containsImage = "Rate limit exceeded";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return containsImage;
        }


        public List<StructTweetIDs> KeywordStructData(string keyword)
        {
            try
            {
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

                //string searchURL = "https://twitter.com/phoenix_search.phoenix?q=" + keyword + "&count=" + noOfRecords + "&include_entities=1&include_available_features=1&contributor_details=true&page=null&mode=relevance&query_source=typed_query";
                if (noOfRecords > 0 && noOfRecords != 20)
                {
                    searchURL = "http://search.twitter.com/search.json?q=" + Uri.EscapeDataString(keyword) + "&result_type=mixed&count=" + noOfRecords;
                }
                else
                {
                    //searchURL = "https://api.twitter.com/1.1/search/tweets.json?q=" + keyword + "&result_type=mixed&count=100";
                    //http://search.twitter.com/search.json?q=blue%20angels&rpp=5&include_entities=true&result_type=mixed
                    searchURL = "http://search.twitter.com/search.json?q=" + Uri.EscapeDataString(keyword) + "&result_type=mixed&count=" + noOfRecords;
                }
            startAgain:
                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }
                    if (res_Get_searchURL.Contains("\"next_page\":") && counter < noOfRecords)
                    {
                        try
                        {
                            int startindex = res_Get_searchURL.IndexOf("\"next_page\":");
                            if (startindex > 0)
                            {
                                string start = res_Get_searchURL.Substring(startindex).Replace("\"next_page\":\"", "");
                                int endIndex = start.IndexOf("\",");
                                string end = start.Substring(0, endIndex).Replace("from_user_id\":", "");
                                searchURL = "http://search.twitter.com/search.json" + end;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        return lst_structTweetIDs;
                    }
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }

                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {

                    string[] splitRes = Regex.Split(res_Get_searchURL, "{\"created_at\""); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (noOfRecords > counter)
                        {
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("\"id_str\"");
                            string start = item.Substring(startIndex);
                            int endIndex = start.IndexOf("\",");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user_id = "";
                        try
                        {
                            int startIndex = item.IndexOf("from_user_id\":");
                            string start = item.Substring(startIndex);
                            int endIndex = start.IndexOf(",\"from_user_id_str");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            from_user_id = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user = "";
                        try
                        {
                            if (item.Contains("\"screen_name\""))
                            {
                                int startindex = item.IndexOf("\"screen_name\"");
                                string start = item.Substring(startindex);
                                int endIndex = start.IndexOf(",\"");
                                string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                from_user = end;
                            }
                            else
                            {
                                int startindex = item.IndexOf("\"from_user\"");
                                string start = item.Substring(startindex);
                                int endIndex = start.IndexOf(",\"");
                                string end = start.Substring(0, endIndex).Replace("from_user", "").Replace("\"", "").Replace(",\"from", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                from_user = end;
                            }
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string text = string.Empty;

                        try
                        {
                            int startindex = item.IndexOf("\"text\":");
                            string start = item.Substring(startindex).Replace("\"text\":", "");
                            int endIndex = start.IndexOf(",\"");
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf("}");
                            }
                            string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", string.Empty);
                            text = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        try
                        {
                            int startindex = item.IndexOf("\"text\":");
                            string start = item.Substring(startindex).Replace("\"text\":", "");
                            int endIndex = start.IndexOf(",\"");
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf("}");
                            }
                            string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", string.Empty);
                            text = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        if (id != "null")
                        {
                            structTweetIDs.ID_Tweet = id;
                            structTweetIDs.ID_Tweet_User = from_user_id;
                            structTweetIDs.username__Tweet_User = from_user;
                            structTweetIDs.wholeTweetMessage = text;

                            lst_structTweetIDs.Add(structTweetIDs);

                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(id + ":" + from_user_id, Globals.Path_keywordFollowerScrapedData);
                        }


                    }
                }
                if (lst_structTweetIDs.Count < noOfRecords)
                {
                    goto startAgain;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        #region NewKeywordStructData commented region
        //public List<StructTweetIDs> NewKeywordStructData(string keyword)
        //{
        //    try
        //    {
        //        BaseLib.GlobusRegex regx = new GlobusRegex();
        //        int counter = 0;
        //        lst_structTweetIDs = new List<StructTweetIDs>();
        //        string res_Get_searchURL = string.Empty;
        //        string searchURL = string.Empty;
        //        string maxid = string.Empty;
        //        string TweetId = string.Empty;
        //        string text = string.Empty;

        //        if (keyword.Trim().Contains(" "))
        //        {
        //            keyword = keyword.Replace(" ", "+");
        //        }

        //    startAgain:
        //        if (counter == 0)
        //        {
        //            //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&include_available_features=1&include_entities=1&q=" + Uri.EscapeDataString(keyword);
        //            //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&composed_count=0&count=" + noOfRecords + "&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
        //            //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
        //            //counter++;

        //            //searchURL = "https://twitter.com/search?q=ranbir&mode=relevance&src=typd";

        //            //searchURL = "https://twitter.com/search?q=" + keyword + "&mode=relevance&src=typd";
        //            searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000";

        //            counter++;

        //        }

        //        else
        //        {
        //            //if (res_Get_searchURL.Contains("has_more_items\":false"))
        //            //{
        //            //    return lst_structTweetIDs;
        //            //}
        //            //searchURL = "https://twitter.com/i/search/timeline?type=relevance&src=typd&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
        //            //searchURL = "https://twitter.com/i/search/timeline?q=ranbir&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=TWEET-372646612473876480-374800735134687234";

        //            searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=" + TweetId;

        //        }


        //        try
        //        {
        //            res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

        //            if (string.IsNullOrEmpty(res_Get_searchURL))
        //            {
        //                res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //            }

        //            try
        //            {
        //                //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //                string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
        //                splitRes = splitRes.Skip(1).ToArray();
        //                foreach (string item in splitRes)
        //                {

        //                    int startIndex = item.IndexOf("TWEET-");
        //                    string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
        //                    int endIndex = start.IndexOf("\"");
        //                    string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
        //                    TweetId = end;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }

        //        catch (Exception ex)
        //        {
        //            System.Threading.Thread.Sleep(2000);
        //            res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        }
        //        // && !res_Get_searchURL.Contains("has_more_items\":false")
        //        if (!string.IsNullOrEmpty(res_Get_searchURL))
        //        {
        //            //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
        //            string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

        //            splitRes = splitRes.Skip(1).ToArray();


        //            foreach (string item in splitRes)
        //            {
        //                if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
        //                {
        //                    //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
        //                    //string DataHtml = (string)avc["items_html"];
        //                }
        //                else
        //                {
        //                    continue;
        //                }
        //                string modified_Item = "\"from_user\"" + item;

        //                string id = "";
        //                try
        //                {
        //                    int startIndex = item.IndexOf("data-user-id=");
        //                    string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
        //                    int endIndex = start.IndexOf("\\\"");
        //                    string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
        //                    id = end;
        //                }
        //                catch (Exception ex)
        //                {
        //                    id = "null";
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                string from_user_id = "";
        //                try
        //                {
        //                    int startIndex = item.IndexOf("data-screen-name=\\\"");
        //                    string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
        //                    int endIndex = start.IndexOf("\\\"");
        //                    string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
        //                    from_user_id = end;
        //                }
        //                catch (Exception ex)
        //                {
        //                    from_user_id = "null";
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                string tweetUserid = string.Empty;
        //                try
        //                {
        //                    int startIndex = item.IndexOf("=\\\"");
        //                    string start = item.Substring(startIndex).Replace("=\\\"", "");
        //                    int endIndex = start.IndexOf("\\\"");
        //                    string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
        //                    tweetUserid = end;
        //                }
        //                catch (Exception ex)
        //                {
        //                    from_user_id = "null";
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                ///Tweet Text 
        //                try
        //                {
        //                    int startindex = item.IndexOf("js-tweet-text tweet-text\"");
        //                    string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "");
        //                    int endindex = start.IndexOf("</p>");
        //                    string end = start.Substring(0, endindex);
        //                    end = regx.StripTagsRegex(end);
        //                    text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
        //                    text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

        //                    string[] array = Regex.Split(text, "http");
        //                    text = string.Empty;
        //                    foreach (string itemData in array)
        //                    {
        //                        if (!itemData.Contains("t.co"))
        //                        {
        //                            string data = string.Empty;
        //                            if (itemData.Contains("//"))
        //                            {
        //                                data = "http" + itemData;
        //                                if (!text.Contains(itemData.Replace(" ", "")))
        //                                {
        //                                    text += data;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (!text.Contains(itemData.Replace(" ", "")))
        //                                {
        //                                    text += itemData;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }


        //                StructTweetIDs structTweetIDs = new StructTweetIDs();

        //                if (id != "null")
        //                {
        //                    structTweetIDs.ID_Tweet = tweetUserid;
        //                    structTweetIDs.ID_Tweet_User = id;
        //                    structTweetIDs.username__Tweet_User = from_user_id;
        //                    structTweetIDs.wholeTweetMessage = text;
        //                    lst_structTweetIDs.Add(structTweetIDs);
        //                    Log(tweetUserid);
        //                    Log(id);
        //                    Log(from_user_id);
        //                    Log("-------------------------------------------------------------------------------------------------------------------------------");
        //                }
        //                if (lst_structTweetIDs.Count >= noOfRecords)
        //                {
        //                    return lst_structTweetIDs;
        //                }
        //            }
        //            lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
        //            if (lst_structTweetIDs.Count < noOfRecords)
        //            {
        //                maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;
        //                goto startAgain;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }

        //    return lst_structTweetIDs;
        //} 
        #endregion

        public List<StructTweetIDs> NewKeywordStructData(string keyword)
        {
            try
            {
                BaseLib.GlobusRegex regx = new GlobusRegex();
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;
                string TweetId = string.Empty;
                string text = string.Empty;

                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

            startAgain:

                #region <Old Get request URl>>
                //if (counter == 0)
                //{
                //    //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&include_available_features=1&include_entities=1&q=" + Uri.EscapeDataString(keyword);
                //    //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&composed_count=0&count=" + noOfRecords + "&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
                //    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                //    //counter++;

                //    //searchURL = "https://twitter.com/search?q=ranbir&mode=relevance&src=typd";

                //    //searchURL = "https://twitter.com/search?q=" + keyword + "&mode=relevance&src=typd";
                //    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000";

                //    counter++;

                //}

                //else
                //{
                //    if (res_Get_searchURL.Contains("has_more_items\":false"))
                //    {
                //        return lst_structTweetIDs;
                //    }
                //    //searchURL = "https://twitter.com/i/search/timeline?type=relevance&src=typd&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
                //    //searchURL = "https://twitter.com/i/search/timeline?q=ranbir&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=TWEET-372646612473876480-374800735134687234";

                //    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=" + TweetId;

                //}
                #endregion

                if (counter == 0)
                {
                    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                    counter++;
                }
                else
                {
                    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "&scroll_cursor=" + TweetId;
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }

                    try
                    {
                        //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                        string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                        splitRes = splitRes.Skip(1).ToArray();
                        foreach (string item in splitRes)
                        {

                            int startIndex = item.IndexOf("TWEET-");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            TweetId = end;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                // && !res_Get_searchURL.Contains("has_more_items\":false")
                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                    string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                        {
                            //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                            //string DataHtml = (string)avc["items_html"];
                        }
                        else
                        {
                            continue;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-user-id=");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user_id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            from_user_id = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string tweetUserid = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("=\\\"");
                            string start = item.Substring(startIndex).Replace("=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        ///Tweet Text 
                        try
                        {


                            int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                            if (startindex == -1)
                            {
                                startindex = 0;
                                startindex = item.IndexOf("js-tweet-text tweet-text");
                            }

                            string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "").Replace("js-tweet-text tweet-text tweet-text-rtl\"", "");
                            int endindex = start.IndexOf("</p>");

                            if (endindex == -1)
                            {
                                endindex = 0;
                                endindex = start.IndexOf("stream-item-footer");
                            }

                            string end = start.Substring(0, endindex);
                            end = regx.StripTagsRegex(end);
                            text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                            text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = ("http" + itemData).Replace(" span ", string.Empty);
                                        if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                                        {
                                            text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("js-tweet-text tweet-text", "");
                                        }
                                    }
                                }
                            }
                        }
                        catch { };



                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        if (id != "null")
                        {
                            structTweetIDs.ID_Tweet = tweetUserid;
                            structTweetIDs.ID_Tweet_User = id;
                            structTweetIDs.username__Tweet_User = from_user_id;
                            structTweetIDs.wholeTweetMessage = text;
                            lst_structTweetIDs.Add(structTweetIDs);
                            Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                            Log("-------------------------------------------------------------------------------------------------------------------------------");
                        }
                       
                        if (lst_structTweetIDs.Count >= noOfRecords)
                        {
                            return lst_structTweetIDs;
                        }
                        lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                    }

                    if (lst_structTweetIDs.Count >noOfRecords)
                    {
                        maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                        if (res_Get_searchURL.Contains("has_more_items\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                        {
                            goto startAgain;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        public List<StructTweetIDs> NewKeywordStructData1(string keyword)
        {
            try
            {
                BaseLib.GlobusRegex regx = new GlobusRegex();
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;
                string TweetId = string.Empty;
                string text = string.Empty;

                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

            startAgain:

                #region <Old Get request URl>>
                //if (counter == 0)
                //{
                //    //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&include_available_features=1&include_entities=1&q=" + Uri.EscapeDataString(keyword);
                //    //searchURL = "https://twitter.com/i/search/realtime?type=relevance&src=typd&composed_count=0&count=" + noOfRecords + "&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
                //    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                //    //counter++;

                //    //searchURL = "https://twitter.com/search?q=ranbir&mode=relevance&src=typd";

                //    //searchURL = "https://twitter.com/search?q=" + keyword + "&mode=relevance&src=typd";
                //    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000";

                //    counter++;

                //}

                //else
                //{
                //    if (res_Get_searchURL.Contains("has_more_items\":false"))
                //    {
                //        return lst_structTweetIDs;
                //    }
                //    //searchURL = "https://twitter.com/i/search/timeline?type=relevance&src=typd&include_available_features=1&include_entities=1&max_id=" + maxid + "&q=" + keyword;
                //    //searchURL = "https://twitter.com/i/search/timeline?q=ranbir&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=TWEET-372646612473876480-374800735134687234";

                //    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&mode=relevance&composed_count=0&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=30000&latent_count=25&refresh_cursor=" + TweetId;

                //}
                #endregion

                if (!RetweetFromUserName)
                {
                    if (counter == 0)
                    {
                        //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime";

                        
                        counter++;
                    }
                    else
                    {

                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";

                        //29-4-2014 only for client it is changed
                        //searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&mode=users&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                    }
                }
                else 
                {
                        searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + RetweetExtractcount + "&include_available_features=1&include_entities=1";                   
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }

                    try
                    {
                        //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                        string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                        //splitRes = splitRes.Skip(1).ToArray();
                        foreach (string item in splitRes)
                        {
                            if (item.Contains("refresh_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                                

                                ////only for client 29/4
                                ////int startIndex = item.IndexOf("TWEET-");

                                //int startIndex = item.IndexOf("USER-");
                                //string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                //int endIndex = start.IndexOf("\"");
                                //string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                //TweetId = end;
                            }
                            if (item.Contains("scroll_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                // && !res_Get_searchURL.Contains("has_more_items\":false")
                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                    string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                        {
                            //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                            //string DataHtml = (string)avc["items_html"];
                        }
                        else
                        {
                            continue;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-user-id=");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user_id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            from_user_id = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string tweetUserid = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("=\\\"");
                            string start = item.Substring(startIndex).Replace("=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        ///Tweet Text 
                        try
                        {


                            int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                            if (startindex == -1)
                            {
                                startindex = 0;
                                startindex = item.IndexOf("js-tweet-text tweet-text");
                            }

                            string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "").Replace("js-tweet-text tweet-text tweet-text-rtl\"", "");
                            int endindex = start.IndexOf("</p>");

                            if (endindex == -1)
                            {
                                endindex = 0;
                                endindex = start.IndexOf("stream-item-footer");
                            }

                            string end = start.Substring(0, endindex);
                            end = regx.StripTagsRegex(end);
                            text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                            text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = ("http" + itemData).Replace(" span ", string.Empty);
                                        if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                                        {
                                            text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("js-tweet-text tweet-text", "");
                                        }
                                    }
                                }
                            }
                        }
                        catch { };



                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        if (id != "null")
                        {
                            structTweetIDs.ID_Tweet = tweetUserid;
                            structTweetIDs.ID_Tweet_User = id;
                            structTweetIDs.username__Tweet_User = from_user_id;
                            structTweetIDs.wholeTweetMessage = text;
                            lst_structTweetIDs.Add(structTweetIDs);
                            Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                            Log("-------------------------------------------------------------------------------------------------------------------------------");
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(id + ":" + from_user_id, Globals.Path_keywordFollowerScrapedData);
                        }
                       
                        lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                       
                        if (lst_structTweetIDs.Count >= noOfRecords)
                        {
                            return lst_structTweetIDs;
                        }
                    
                    }

                    if (lst_structTweetIDs.Count <= noOfRecords)
                    {
                        maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                        if (res_Get_searchURL.Contains("has_moreitems\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                        {
                            goto startAgain;
                        }
                    }
                    else
                    {
                        if (res_Get_searchURL.Contains("has_more_items\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                            goto startAgain;
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }


        public List<StructTweetIDs> NewKeywordStructDataForSearchByKeyword(string keyword)
        {
            try
            {
                BaseLib.GlobusRegex regx = new GlobusRegex();
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;
                string TweetId = string.Empty;
                string text = string.Empty;

                string ProfileName = string.Empty;
                string Location = string.Empty;
                string Bio = string.Empty;
                string website = string.Empty;
                string NoOfTweets = string.Empty;
                string Followers = string.Empty;
                string Followings = string.Empty;

                
                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

            startAgain:

                if (!RetweetFromUserName)
                {
                    if (counter == 0)
                    {
                        //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime";

                        
                        counter++;
                    }
                    else
                    {

                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";

                        //29-4-2014 only for client it is changed
                        //searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&mode=users&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                    }
                }
                else 
                {
                        searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + RetweetExtractcount + "&include_available_features=1&include_entities=1";                   
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }

                    try
                    {
                        //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                        string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                        //splitRes = splitRes.Skip(1).ToArray();
                        foreach (string item in splitRes)
                        {
                            if (item.Contains("refresh_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                                
                                
                            }
                            if (item.Contains("scroll_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                // && !res_Get_searchURL.Contains("has_more_items\":false")
                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                    string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                        {
                            //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                            //string DataHtml = (string)avc["items_html"];
                        }
                        else
                        {
                            continue;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-user-id=");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user_id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            from_user_id = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string tweetUserid = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("=\\\"");
                            string start = item.Substring(startIndex).Replace("=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        ///Tweet Text 
                        try
                        {


                            int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                            if (startindex == -1)
                            {
                                startindex = 0;
                                startindex = item.IndexOf("js-tweet-text tweet-text");
                            }

                            string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "").Replace("js-tweet-text tweet-text tweet-text-rtl\"", "");
                            int endindex = start.IndexOf("</p>");

                            if (endindex == -1)
                            {
                                endindex = 0;
                                endindex = start.IndexOf("stream-item-footer");
                            }

                            string end = start.Substring(0, endindex);
                            end = regx.StripTagsRegex(end);
                            text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                            text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = ("http" + itemData).Replace(" span ", string.Empty);
                                        if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                                        {
                                            text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("js-tweet-text tweet-text", "");
                                        }
                                    }
                                }
                            }
                        }
                        catch { };



                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        if (id != "null")
                        {
                            structTweetIDs.ID_Tweet = tweetUserid;
                            structTweetIDs.ID_Tweet_User = id;
                            structTweetIDs.username__Tweet_User = from_user_id;
                            structTweetIDs.wholeTweetMessage = text;
                            lst_structTweetIDs.Add(structTweetIDs);
                            Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                            Log("-------------------------------------------------------------------------------------------------------------------------------");
                        }


                        if (!File.Exists(Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv"))
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO , LOCATION , WEBSITE , NO OF TWEETS , FOLLOWERS , FOLLOWINGS", Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");
                        }

                        {

                            ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                            string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + from_user_id), "", "");

                            string Responce = ProfilePageSource;

                            #region Convert HTML to XML

                            string xHtml = objChilkat.ConvertHtmlToXml(Responce);
                            Chilkat.Xml xml = new Chilkat.Xml();
                            xml.LoadXml(xHtml);

                            Chilkat.Xml xNode = default(Chilkat.Xml);
                            Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                            #endregion

                            int counterdata = 0;
                            xBeginSearchAfter = null;
                            string dataDescription = string.Empty;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                            while ((xNode != null))
                            {
                                xBeginSearchAfter = xNode;
                                if (counterdata == 0)
                                {
                                    ProfileName = xNode.AccumulateTagContent("text", "script|style");
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
                                // xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "u-textUserColor");
                            }

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
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//location profile-field");
                            while ((xNode != null))
                            {
                                xBeginSearchAfter = xNode;
                                Location = xNode.AccumulateTagContent("text", "script|style");
                                break;
                            }

                            int counterData = 0;
                            xBeginSearchAfter = null;
                            dataDescription = string.Empty;
                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");//location profile-field");
                            while ((xNode != null))
                            {
                                xBeginSearchAfter = xNode;
                                if (counterData == 0)
                                {
                                    // NoOfTweets = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileNav-value");
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


                            if (!string.IsNullOrEmpty(from_user_id) && tweetUserid != "null")
                            {
                                string Id_user = tweetUserid.Replace("}]", string.Empty).Trim();
                                Globals.lstScrapedUserIDs.Add(Id_user);
                                GlobusFileHelper.AppendStringToTextfileNewLine(id + "," + from_user_id + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", ""), Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");
                                GlobusFileHelper.AppendStringToTextfileNewLine (from_user_id, Globals.Path_KeywordScrapedListData + "-" + keyword + ".txt");

                                Log("[ " + DateTime.Now + " ] => [ " + from_user_id + "," + Id_user + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ]");
                            }
                        }



                        lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                       
                        if (lst_structTweetIDs.Count >= noOfRecords)
                        {
                            return lst_structTweetIDs;
                        }
                    
                    }

                    if (lst_structTweetIDs.Count <= noOfRecords)
                    {
                        maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                        if (res_Get_searchURL.Contains("has_moreitems\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                        {
                            goto startAgain;
                        }
                    }
                    else
                    {
                        if (res_Get_searchURL.Contains("has_more_items\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                            goto startAgain;
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        public List<StructTweetIDs> NewKeywordStructDataSearchByPeople(string keyword)
        {
            try
            {
                BaseLib.GlobusRegex regx = new GlobusRegex();
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;
                string TweetId = string.Empty;
                string text = string.Empty;

                string ProfileName = string.Empty;
                string Location = string.Empty;
                string Bio = string.Empty;
                string website = string.Empty;
                string NoOfTweets = string.Empty;
                string Followers = string.Empty;
                string Followings = string.Empty;

                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

            startAgain:

                

                if (!RetweetFromUserName)
                {
                    if (counter == 0)
                    {
                        //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                        //searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime";

                        //29-4-2014 only for client it has been changed
                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&mode=users";
                        counter++;
                    }
                    else
                    {

                        //searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";

                        //29-4-2014 only for client it is changed
                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&mode=users&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                    }
                }
                else
                {
                    searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + RetweetExtractcount + "&include_available_features=1&include_entities=1";
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }

                    try
                    {
                        //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                        string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                        //splitRes = splitRes.Skip(1).ToArray();
                        foreach (string item in splitRes)
                        {
                            if (item.Contains("refresh_cursor"))
                            {
                               

                                int startIndex = item.IndexOf("USER-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                            if (item.Contains("scroll_cursor"))
                            {
                                int startIndex = item.IndexOf("USER-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                // && !res_Get_searchURL.Contains("has_more_items\":false")
                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                    string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                        {
                            //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                            //string DataHtml = (string)avc["items_html"];
                        }
                        else
                        {
                            //continue;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-user-id=");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string from_user_id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            from_user_id = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string tweetUserid = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("=\\\"");
                            string start = item.Substring(startIndex).Replace("=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end;
                        }
                        catch (Exception ex)
                        {
                            from_user_id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        
                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        if (id != "null")
                        {
                            structTweetIDs.ID_Tweet = tweetUserid;
                            structTweetIDs.ID_Tweet_User = id;
                            structTweetIDs.username__Tweet_User = from_user_id;
                            structTweetIDs.wholeTweetMessage = text;
                            lst_structTweetIDs.Add(structTweetIDs);
                            Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                            Log("-------------------------------------------------------------------------------------------------------------------------------");




                            if (!File.Exists(Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv"))
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO , LOCATION , WEBSITE , NO OF TWEETS , FOLLOWERS , FOLLOWINGS", Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");
                            }

                            //foreach (TwitterDataScrapper.StructTweetIDs item in data)
                            {
                                

                                ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                                string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + from_user_id), "", "");

                                string Responce = ProfilePageSource;

                                #region Convert HTML to XML

                                string xHtml = objChilkat.ConvertHtmlToXml(Responce);
                                Chilkat.Xml xml = new Chilkat.Xml();
                                xml.LoadXml(xHtml);

                                Chilkat.Xml xNode = default(Chilkat.Xml);
                                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                                #endregion

                                int counterdata = 0;
                                xBeginSearchAfter = null;
                                string dataDescription = string.Empty;
                                xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                                while ((xNode != null))
                                {
                                    xBeginSearchAfter = xNode;
                                    if (counterdata == 0)
                                    {
                                        ProfileName = xNode.AccumulateTagContent("text", "script|style");
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
                                   // xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                                    xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "u-textUserColor");
                                }

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
                                xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//location profile-field");
                                while ((xNode != null))
                                {
                                    xBeginSearchAfter = xNode;
                                    Location = xNode.AccumulateTagContent("text", "script|style");
                                    break;
                                }

                                int counterData = 0;
                                xBeginSearchAfter = null;
                                dataDescription = string.Empty;
                                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");//location profile-field");
                                while ((xNode != null))
                                {
                                    xBeginSearchAfter = xNode;
                                    if (counterData == 0)
                                    {
                                       // NoOfTweets = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileNav-value");
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


                                if (!string.IsNullOrEmpty(from_user_id) && tweetUserid != "null")
                                {
                                    string Id_user = tweetUserid.Replace("}]", string.Empty).Trim();
                                    Globals.lstScrapedUserIDs.Add(Id_user);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(id + "," + from_user_id + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", ""), Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");

                                    GlobusFileHelper.AppendStringToTextfileNewLine(from_user_id, Globals.Path_KeywordScrapedListData + "-" + keyword + ".txt");
                                    Log("[ " + DateTime.Now + " ] => [ " + from_user_id + "," + Id_user + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ]");
                                }
                            }
                        }

                        lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();

                        if (lst_structTweetIDs.Count >= noOfRecords)
                        {
                            return lst_structTweetIDs;
                        }

                    }

                    if (lst_structTweetIDs.Count <= noOfRecords)
                    {
                        maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                        if (res_Get_searchURL.Contains("has_moreitems\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                        {
                            goto startAgain;
                        }
                    }
                    else
                    {
                        if (res_Get_searchURL.Contains("has_more_items\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                            goto startAgain;
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }


        public List<StructTweetIDs> NewKeywordStructDataForOnlyTweet(string keyword)
        {
            try
            {
                BaseLib.GlobusRegex regx = new GlobusRegex();
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                //lstTweetIds=new List<string>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;
                string TweetId = string.Empty;
                string text = string.Empty;

                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }

            startAgain:

                if (!RetweetFromUserName)
                {
                    if (counter == 0)
                    {

                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime";
                        counter++;
                    }
                    else
                    {

                        searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                    }
                }
                else
                {
                    searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + RetweetExtractcount + "&include_available_features=1&include_entities=1";
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }

                    try
                    {
                       
                        string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                        
                        foreach (string item in splitRes)
                        {
                            if (item.Contains("refresh_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                            if (item.Contains("scroll_cursor"))
                            {
                                int startIndex = item.IndexOf("TWEET-");
                                string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                int endIndex = start.IndexOf("\"");
                                string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                TweetId = end;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(2000);
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                // && !res_Get_searchURL.Contains("has_more_items\":false")
                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                    string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();


                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                        {
                           
                        }
                        else
                        {
                            continue;
                        }
                        string modified_Item = "\"from_user\"" + item;

                        string id = "";
                        try
                        {
                            int startIndex = item.IndexOf("data-user-id=");
                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            id = end;
                        }
                        catch (Exception ex)
                        {
                            id = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        
                        string tweetUserid = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("=\\\"");
                            string start = item.Substring(startIndex).Replace("=\\\"", "");
                            int endIndex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end;
                        }
                        catch (Exception ex)
                        {
                            tweetUserid = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        try
                        {
                            int startindex = item.IndexOf("js-tweet-text tweet-text");
                            string start = item.Substring(startindex).Replace("js-tweet-text tweet-text", "");
                            int endindex = start.IndexOf("</p>");
                            if (endindex == -1)
                            {
                                endindex = 0;
                                endindex = start.IndexOf("stream-item-footer");
                            }
                            string end = start.Substring(0, endindex);
                            end = regx.StripTagsRegex(end);
                           
                            text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "").Replace("dir=\"ltr\"", "");
                            text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "").Replace("&amp;", "&").Replace("&#39;", "'").Replace("&lt;", "<").Replace("&gt;", ">");


                            //string[] array = Regex.Split(text, "http");
                            //text = string.Empty;
                            //foreach (string itemData in array)
                            //{
                            //    if (!itemData.Contains("t.co"))
                            //    {
                            //        string data = string.Empty;
                            //        if (itemData.Contains("//"))
                            //        {
                            //            data = ("http" + itemData).Replace(" span ", string.Empty);
                            //            if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                            //            {
                            //                text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (!text.Contains(itemData.Replace(" ", "")))
                            //            {
                            //                text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                            //            }
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        StructTweetIDs structTweetIDs = new StructTweetIDs();
                        if (!IsRetweetWithFovieteWithImages)
                        {
                            if (id != "null")
                            {
                                structTweetIDs.ID_Tweet = tweetUserid;
                                structTweetIDs.ID_Tweet_User = id;
                                lst_structTweetIDs.Add(structTweetIDs);
                                //Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                                //Log("-------------------------------------------------------------------------------------------------------------------------------");
                            }

                            lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();

                            queTweetId.Enqueue(tweetUserid);
                        }
                        else
                        {
                            if (text.Contains("http://t.co"))
                            {
                                if (id != "null")
                                {
                                    structTweetIDs.ID_Tweet = tweetUserid;
                                    structTweetIDs.ID_Tweet_User = id;
                                    lst_structTweetIDs.Add(structTweetIDs);
                                    //Log("[ " + DateTime.Now + " ] => [ " + tweetUserid + " ]");
                                    //Log("-------------------------------------------------------------------------------------------------------------------------------");
                                }

                                lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();

                                queTweetId.Enqueue(tweetUserid);
                            }
                        }
                        //lstTweetIds.Add(tweetUserid);
                        //lstTweetIds = lstTweetIds.Distinct().ToList();
                        //if (lst_structTweetIDs.Count >= noOfRecords)
                        //{
                        //    return lst_structTweetIDs;
                        //}

                    }

                    if (lst_structTweetIDs.Count <= noOfRecords)
                    {
                        maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                        if (res_Get_searchURL.Contains("has_moreitems\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                        {
                            goto startAgain;
                        }
                    }
                    else
                    {
                        if (res_Get_searchURL.Contains("has_more_items\":false"))
                        {
                            return lst_structTweetIDs;
                        }
                        else
                            goto startAgain;
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        public List<StructTweetIDs> GetTweetData(string keyword)
        {
            lst_structTweetIDs = new List<StructTweetIDs>();

            try
            {
                string searchURL = string.Empty;

                if (noOfRecords > 0 && noOfRecords != 20)
                {
                    searchURL = "http://search.twitter.com/search.json?q=" + keyword + "&rpp=" + noOfRecords + "&include_entities=true&result_type=recent";
                }
                else
                {
                    searchURL = "https://twitter.com/search?q=" + keyword + "&src=typd";
                }
                //string searchURL = "http://search.twitter.com/search.json?q=" + keyword + "&include_entities=true&result_type=recent";

                string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");


                string[] splitRes = Regex.Split(res_Get_searchURL, "\"from_user\"");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                splitRes = splitRes.Skip(1).ToArray();

                foreach (string item in splitRes)
                {
                    string modified_Item = "\"from_user\"" + item;

                    string from_user = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "from_user");

                    string from_user_id = Globussoft.GlobusHttpHelper.ParseEncodedJson(modified_Item, "from_user_id");

                    string from_user_name = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "from_user_name");

                    string id = Globussoft.GlobusHttpHelper.ParseEncodedJson(modified_Item, "id");

                    string text = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "text");

                    StructTweetIDs structTweetIDs = new StructTweetIDs();

                    structTweetIDs.ID_Tweet = id;
                    structTweetIDs.ID_Tweet_User = from_user_id;
                    structTweetIDs.username__Tweet_User = from_user;
                    structTweetIDs.wholeTweetMessage = text;

                    lst_structTweetIDs.Add(structTweetIDs);
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        public List<StructTweetIDs> GetTweetData_New(string keyword)
        {
            lst_structTweetIDs = new List<StructTweetIDs>();
            string Nextcounter = "0";
            try
            {
                //StartAgain:
                string searchURL = "https://twitter.com/i/search/timeline?type=relevance&src=typd&include_available_features=1&include_entities=1&max_id=" + noOfRecords + "&q=" + Uri.EscapeDataString(keyword);
                string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), string.Empty, string.Empty);

                JObject Abc = JObject.Parse(res_Get_searchURL);
                string datahkj = string.Empty;
                datahkj = Abc["items_html"].ToString();

                string[] splitRes = Regex.Split(((string)Abc["items_html"]), "js-stream-item stream-item stream-item expanding-stream-item");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                splitRes = splitRes.Skip(1).ToArray();
                GlobusRegex regx = new GlobusRegex();
                foreach (string item in splitRes)
                {
                    string from_user = string.Empty;
                    string from_user_id = string.Empty;
                    string from_user_name = string.Empty;
                    string id = string.Empty;
                    string text = string.Empty;

                    ///Tweet ID
                    try
                    {
                        int startindex = item.IndexOf("data-item-id=\"");
                        string start = item.Substring(startindex).Replace("data-item-id=\"", "");
                        int endindex = start.IndexOf("\"");
                        string end = start.Substring(0, endindex);
                        id = end;
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> userid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> userid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }


                    ///Tweet User Screen name
                    try
                    {
                        int startindex = item.IndexOf("data-screen-name=\"");
                        string start = item.Substring(startindex).Replace("data-screen-name=\"", "");
                        int endindex = start.IndexOf("\"");
                        string end = start.Substring(0, endindex);
                        from_user_name = end;
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> from_user_name --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> from_user_name --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    ///Tweet User User-id
                    try
                    {
                        int startindex = item.IndexOf("data-user-id=\"");
                        string start = item.Substring(startindex).Replace("data-user-id=\"", "");
                        int endindex = start.IndexOf("\"");
                        string end = start.Substring(0, endindex);
                        from_user_id = end;
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    ///Tweet Text 
                    try
                    {
                        int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                        string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "");
                        int endindex = start.IndexOf("</p>");
                        string end = start.Substring(0, endindex);
                        end = regx.StripTagsRegex(end);
                        text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                        text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                        string[] array = Regex.Split(text, "http");
                        text = string.Empty;
                        foreach (string itemData in array)
                        {
                            if (!itemData.Contains("t.co"))
                            {
                                string data = string.Empty;
                                if (itemData.Contains("//"))
                                {
                                    data = "http" + itemData;
                                    if (!text.Contains(itemData.Replace(" ", "")))
                                    {
                                        text += data;
                                    }
                                }
                                else
                                {
                                    if (!text.Contains(itemData.Replace(" ", "")))
                                    {
                                        text += itemData;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    StructTweetIDs structTweetIDs = new StructTweetIDs();

                    structTweetIDs.ID_Tweet = id;
                    structTweetIDs.ID_Tweet_User = from_user_id;
                    structTweetIDs.username__Tweet_User = from_user_name;
                    structTweetIDs.wholeTweetMessage = text;
                    Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                    Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                    Log("[ " + DateTime.Now + " ] => [ " + from_user_name + " ]");
                    Log("[ " + DateTime.Now + " ] => [ " + text + " ]");
                    Log("---------------------------------------------------------------------------------------------------------------------------------------------------");
                    lst_structTweetIDs.Add(structTweetIDs);

                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(id + ":" + from_user_id, Globals.Path_keywordFollowerScrapedData);
                }

                //if (res_Get_searchURL.Contains("\"has_more_items\":true"))
                //{
                //    try
                //    {
                //        int startindex = res_Get_searchURL.IndexOf("{\"max_id\":\"");
                //        string start = res_Get_searchURL.Substring(startindex).Replace("{\"max_id\":\"", "");
                //        int endindex = start.IndexOf("\",");
                //        string end = start.Substring(0, endindex);
                //        Nextcounter = end;
                //    }
                //    catch (Exception ex)
                //    {
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                //    }
                //    //goto StartAgain;
                //}

                return lst_structTweetIDs;
            }
            catch (Exception ex)
            {
                return lst_structTweetIDs;
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        //Function for Returning TweetData to Wait and reply
        public List<StructTweetIDs> GetTweetData_WaitReply(string keyword)
        {
            try
            {
                try
                {
                    lst_structTweetIDs = new List<StructTweetIDs>();
                    //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Extracting Tweets for " + keyword + " ]");
                    string[] arraylst = new string[] { };
                    string scroll_cursor = "0";
                    GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                    for (int i = 0; i < noOfRecords; i++)
                    {
                        //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Getting " + (i + 1) + " Page Tweets ]");
                        string pgsrcs = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&scroll_cursor=" + scroll_cursor), "", "");
                        //Getting the pages
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
                        //getting the information:tweets,username,userid,tweetid
                        JObject Abc = JObject.Parse(pgsrcs);
                        string datahkj = string.Empty;
                        datahkj = Abc["items_html"].ToString();

                        string[] splitRes = Regex.Split(((string)Abc["items_html"]), "js-stream-item stream-item stream-item expanding-stream-item");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                        splitRes = splitRes.Skip(1).ToArray();
                        GlobusRegex regx = new GlobusRegex();
                        foreach (string item in splitRes)
                        {
                            string from_user = string.Empty;
                            string from_user_id = string.Empty;
                            string from_user_name = string.Empty;
                            string id = string.Empty;
                            string text = string.Empty;

                            ///Tweet ID
                            try
                            {
                                int startindex = item.IndexOf("data-item-id=\"");
                                string start = item.Substring(startindex).Replace("data-item-id=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                id = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> userid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> userid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }


                            ///Tweet User Screen name
                            try
                            {
                                int startindex = item.IndexOf("data-screen-name=\"");
                                string start = item.Substring(startindex).Replace("data-screen-name=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                from_user_name = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> from_user_name --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> from_user_name --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            ///Tweet User User-id
                            try
                            {
                                int startindex = item.IndexOf("data-user-id=\"");
                                string start = item.Substring(startindex).Replace("data-user-id=\"", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                from_user_id = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            ///Tweet Text 
                            try
                            {
                                int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                                string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "");
                                int endindex = start.IndexOf("</p>");
                                string end = start.Substring(0, endindex);
                                end = regx.StripTagsRegex(end);
                                text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                                text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                                string[] array = Regex.Split(text, "http");
                                text = string.Empty;
                                foreach (string itemData in array)
                                {
                                    if (!itemData.Contains("t.co"))
                                    {
                                        string data = string.Empty;
                                        if (itemData.Contains("//"))
                                        {
                                            data = "http" + itemData;
                                            if (!text.Contains(itemData.Replace(" ", "")))
                                            {
                                                text += data;
                                            }
                                        }
                                        else
                                        {
                                            if (!text.Contains(itemData.Replace(" ", "")))
                                            {
                                                text += itemData;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }

                            StructTweetIDs structTweetIDs = new StructTweetIDs();

                            structTweetIDs.ID_Tweet = id;
                            structTweetIDs.ID_Tweet_User = from_user_id;
                            structTweetIDs.username__Tweet_User = from_user_name;
                            structTweetIDs.wholeTweetMessage = text;
                            Log("[ " + DateTime.Now + " ] => [ " + id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_id + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + from_user_name + " ]");
                            Log("[ " + DateTime.Now + " ] => [ " + text + " ]");
                            Log("---------------------------------------------------------------------------------------------------------------------------------------------------");
                            if (text.Contains(keyword))
                            {
                                lst_structTweetIDs.Add(structTweetIDs);
                                lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                            }
                            if (lst_structTweetIDs.Count == noOfRecords)
                            {
                                break;
                            }
                        }
                        if (lst_structTweetIDs.Count == noOfRecords)
                        {
                            break;
                        }
                    }
                    //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ " + lstweete.Count + " Total distinct Tweets ]");
                    
                    //AddToTweetCreatorLogs("[ " + DateTime.Now + " ] => [ Finished Extracting Tweets for " + keyword + " ]");
                    //AddToTweetCreatorLogs("-----------------------------------------------------------------------------------------------------------------------");
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartKeywordExtracting() -->  " + ex.Message, Globals.Path_TweetCreatorErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> StartKeywordExtracting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
                return lst_structTweetIDs;
            }
            catch (Exception ex)
            {
                return lst_structTweetIDs;
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public List<string> GetTweetData_Scrape(string keyword, int NoOfTweets, out string returnStatus)
        {
            #region Old code
            //GlobusRegex regx = new GlobusRegex();
            //string status = string.Empty;
            //List<string> lst_TweetIDs = new List<string>();
            //string Nextcounter = string.Empty;

            //int counter = 0;
            //try
            //{
            //    string user_id = string.Empty;
            //    string searchURL = string.Empty;

            //StartAgain:
            //    if (NumberHelper.ValidateNumber(keyword))
            //    {
            //        //searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&id=" + keyword + "&count=" + TweetExtractCount;

            //        //break;
            //    }
            //    else
            //    {
            //        searchURL = "https://twitter.com/"+keyword;
            //        //searchURL = "https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1&max_id=236830342521171967";
            //        ////searchURL = "https://twitter.com/i/profiles/show/sachin_rt/media_timeline?max_id=248336419444101121&oldest_unread_id=0";
            //        ////searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&screen_name=" + keyword + "&count=" + TweetExtractCount;
            //    }

            //    ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();

            //    string res_Get_searchURL = HttpHelper.GetHtml(searchURL);

            //    if (res_Get_searchURL.Contains("max_id"))// && !res_Get_searchURL.Contains("Sorry, that page does not exist") && !res_Get_searchURL.Contains("Not authorized") && res_Get_searchURL.Contains("created_at") && !string.IsNullOrEmpty(res_Get_searchURL))
            //    {

            //        if (res_Get_searchURL.Contains("\"has_more_items\":true"))
            //        {
            //            try
            //            {
            //                int startindex = res_Get_searchURL.IndexOf("{\"max_id\":\"");
            //                string start = res_Get_searchURL.Substring(startindex).Replace("{\"max_id\":\"", "");
            //                int endindex = start.IndexOf("\",");
            //                string end = start.Substring(0, endindex);
            //                Nextcounter = end;
            //            }
            //            catch (Exception ex)
            //            {
            //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
            //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
            //            }
            //        }





            //        JObject Abc = JObject.Parse(res_Get_searchURL);
            //        string datahkj = string.Empty;
            //        foreach (object data in Abc)
            //        {
            //            datahkj = data.ToString();
            //        }

            //        string[] splitRes = Regex.Split(datahkj, "js-stream-item stream-item stream-item expanding-stream-item");
            //        splitRes = splitRes.Skip(1).ToArray();

            //        foreach (string splitRes_item in splitRes)
            //        {



            //            ///Tweet Text 

            //            string text = string.Empty;
            //            try
            //            {
            //                int startindex = splitRes_item.IndexOf("js-tweet-text tweet-text\\\"");
            //                string start = splitRes_item.Substring(startindex).Replace("js-tweet-text tweet-text\\\"", "");
            //                int endindex = start.IndexOf("</p>");
            //                string end = start.Substring(0, endindex);
            //                end = regx.StripTagsRegex(end);
            //                text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
            //                text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

            //                string[] array = Regex.Split(text, "http");
            //                text = string.Empty;
            //                foreach (string itemData in array)
            //                {
            //                    if (!itemData.Contains("t.co"))
            //                    {
            //                        string data = string.Empty;
            //                        if (itemData.Contains("//"))
            //                        {
            //                            data = "http" + itemData;
            //                            if (!text.Contains(itemData.Replace(" ", "")))
            //                            {
            //                                text += data;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (!text.Contains(itemData.Replace(" ", "")))
            //                            {
            //                                text += itemData;
            //                            }
            //                        }
            //                    }
            //                }

            //                //Remove RT or @ mentions 

            //                #region -----RT or @ mentions-------
            //                if (RemoveRTMSg && text.StartsWith("RT"))
            //                    continue;

            //                if (removeAtMentions && text.Contains("@"))
            //                    continue;

            //                #endregion

            //                string txtdata = keyword + ":" + (text.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty));
            //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(txtdata, Globals.Path_TweetExtractor);
            //                lst_TweetIDs.Add(txtdata);

            //                status = "No Error";

            //            }
            //            catch (Exception ex)
            //            {
            //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
            //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
            //            }
            //        }


            //        if (lst_TweetIDs.Count == NoOfTweets && !string.IsNullOrEmpty(Nextcounter))
            //        {
            //            goto StartAgain;
            //        }


            //string[] splitRes = Regex.Split(res_Get_searchURL, "{\"created_at");
            ////string[] splitRes = Regex.Split(res_Get_searchURL, "js-stream-item stream-item stream-item expanding-stream-item");
            //splitRes = splitRes.Skip(1).ToArray();

            #region
            //foreach (string item in splitRes)
            //{
            //    string Tweet = Globussoft.GlobusHttpHelper.ParseJson(item, "text");
            //    Regex regex = new Regex(@"\\u([0-9a-z]{4})", RegexOptions.IgnoreCase);
            //    Tweet = regex.Replace(Tweet, match => char.ConvertFromUtf32(Int32.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)));

            //    //Remove RT or @ mentions 

            //    #region -----RT or @ mentions-------
            //    if (RemoveRTMSg && Tweet.StartsWith("RT"))
            //        continue;

            //    if (removeAtMentions && Tweet.Contains("@"))
            //        continue;

            //    #endregion

            //    string data = keyword + ":" + (Tweet.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty));
            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_TweetExtractor);
            //    lst_TweetIDs.Add(data);
            //}
            //status = "No Error";
            #endregion


            //    }
            //    else if (res_Get_searchURL.Contains("Rate limit exceeded"))
            //    {
            //        status = "Rate limit exceeded";
            //    }
            //    else if (res_Get_searchURL.Contains("Sorry, that page does not exist"))
            //    {
            //        status = "Sorry, that page does not exist";
            //    }
            //    else if (res_Get_searchURL.Contains("Not authorized"))
            //    {
            //        status = "Not Authorized";
            //    }
            //    else if (string.IsNullOrEmpty(res_Get_searchURL))
            //    {
            //        status = "Not Authorized";
            //    }
            //    else
            //    {
            //        status = "Empty";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            //    status = "Error";
            //}
            //returnStatus = status;
            //return lst_TweetIDs;

            #endregion

            List<string> lst_Tweets = new List<string>();
            string status = string.Empty;
            //string Nextcounter = "0";
            try
            {
                #region commented code
                //StartAgain:
                //string searchURL = "https://twitter.com/i/search/timeline?type=relevance&src=typd&include_available_features=1&include_entities=1&max_id=" + Nextcounter + "&q=" + Uri.EscapeDataString(keyword);
                //string searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=40&include_available_features=1&include_entities=1&include_new_items_bar=true&interval=60000&latent_count=0&since_id=" + Nextcounter + "";
                //searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + NoOfTweets + "&include_available_features=1&include_entities=1&max_id=352346266648326145"; 
                #endregion

                int TempNoOfTweets = NoOfTweets * 1;

                string searchURL = string.Empty;

                searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + TempNoOfTweets + "&include_available_features=1&include_entities=1";

                string res_Get_searchURL = string.Empty;
                try
                {
                     res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                }
                catch (Exception ex)
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                }

                var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                string DataHtml = string.Empty;

                try
                {
                    DataHtml = (string)avc["items_html"];
                }
                catch { };

                if (string.IsNullOrEmpty(DataHtml))
                {
                    foreach (object data in avc)
                    {
                        DataHtml = data.ToString();
                    }
                }
                
                //string[] splitRes = Regex.Split(DataHtml, "js-stream-item stream-item stream-item expanding-stream-item");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                string[] splitRes = Regex.Split(DataHtml, "ProfileTweet u-textBreak js-tweet js-stream-tweet js-actionable-tweet");
                splitRes = splitRes.Skip(1).ToArray();
                GlobusRegex regx = new GlobusRegex();
                foreach (string item in splitRes)
                {
                    string text = string.Empty;
                    string tweetUserid = string.Empty;
                    ///Tweet Text 
                    try
                    {
                        int startindex = item.IndexOf("ProfileTweet-text js-tweet-text u-dir");
                        if (startindex == -1)
                        {
                            startindex = item.IndexOf("js-tweet-text tweet-text");
                        }

                        string start = item.Substring(startindex).Replace("ProfileTweet-text js-tweet-text u-dir", "").Replace("js-tweet-text tweet-text tweet-text-rtl\\\"", "");
                        int endindex = start.IndexOf("</p>");

                        if (endindex == -1)
                        {
                            endindex = 0;
                            endindex = start.IndexOf("stream-item-footer");
                        }

                        string end = start.Substring(0, endindex);
                        end = regx.StripTagsRegex(end);
                        text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "").Replace("dir=\"ltr\"", "");
                        text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "").Replace("&amp;", "&").Replace("&#39;", "'").Replace("&lt;", "<").Replace("&gt;", ">");
                        
                      
                        string[] array = Regex.Split(text, "http");
                        text = string.Empty;
                        foreach (string itemData in array)
                        {
                            if (!itemData.Contains("t.co"))
                            {
                                string data = string.Empty;
                                if (itemData.Contains("//"))
                                {
                                    data = ("http" + itemData).Replace(" span ", string.Empty);
                                    if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                                    {
                                        text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                                    }
                                }
                                else
                                {
                                    if (!text.Contains(itemData.Replace(" ", "")))
                                    {
                                        text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("\n", string.Empty).Replace("                 ", string.Empty).Replace("        lang=endata-aria-label-part=0", string.Empty);
                                    }
                                }
                            }
                        }
                        if (text.Contains("data-aria-label-part=0"))
                        {
                            text = globushttpHelper.getBetween(text + ":&$#@", "data-aria-label-part=0", ":&$#@");
                        }
                       
                        try
                        {
                            int startIndex = item.IndexOf("data-tweet-id=\"");
                            string start1 = item.Substring(startIndex).Replace("data-tweet-id=\"", "");
                            int endIndex = start1.IndexOf("\"");
                            string end1 = start1.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                            tweetUserid = end1;
                        }
                        catch (Exception ex)
                        {
                            tweetUserid = "null";
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        //Remove RT or @ mentions 

                        #region -----RT or @ mentions-------
                        if (RemoveRTMSg && item.Contains("Retweeted by"))
                            continue;

                        if (removeAtMentions && text.Contains("@"))
                            continue;

                        #endregion

                       

                        if (lst_Tweets.Count() < NoOfTweets)
                        {
                            string txtdata = keyword + ":" + tweetUserid + ":" + (text.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("js-tweet-text tweet-text", string.Empty).Replace("dir=ltr", "").Replace("lang=en                 data-aria-label-part=0", "").Trim());
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(txtdata, Globals.Path_TweetExtractor);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(text.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("js-tweet-text tweet-text", string.Empty).Replace("dir=ltr", "").Replace("lang=en                 data-aria-label-part=0", "").Trim(), Globals.Path_TweetExtractorUpload);
                            lst_Tweets.Add(txtdata);
                        }

                        status = "No Error";
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> text --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                #region commented
                //if (res_Get_searchURL.Contains("\"has_more_items\":true"))
                //{
                //    try
                //    {
                //        int startindex = res_Get_searchURL.IndexOf("{\"max_id\":\"");
                //        string start = res_Get_searchURL.Substring(startindex).Replace("{\"max_id\":\"", "");
                //        int endindex = start.IndexOf("\",");
                //        string end = start.Substring(0, endindex);
                //        Nextcounter = end;
                //    }
                //    catch (Exception ex)
                //    {
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_New() -- " + keyword + " --> res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
                //    }


                //    if (lst_Tweets.Count == NoOfTweets && !string.IsNullOrEmpty(Nextcounter))
                //    {
                //        goto StartAgain;
                //    }
                //} 
                #endregion
            }
            catch (Exception ex)
            {
                status = "Empty";
            }

            returnStatus = status;
            return lst_Tweets;
        }

        #region GetRetweetData_Scrape commented region
        //public List<string> GetRetweetData_Scrape(string keyword, out string returnStatus)
        //{
        //    string status = string.Empty;
        //    List<string> lst_ReTweetIDs = new List<string>();
        //    try
        //    {
        //        string searchURL = string.Empty;

        //        if (!NumberHelper.ValidateNumber(keyword))
        //        {
        //            searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + RetweetExtractcount + "&include_available_features=1&include_entities=1";
        //            //searchURL = "https://api.twitter.com/1/statuses/retweeted_by_user.xml?screen_name=" + keyword + "&count=" + RetweetExtractcount + "&include_entities=true";
        //            //searchURL = "https://api.twitter.com/1.1/statuses/retweeted_by_user.json?screen_name=" + keyword + "&count=" + RetweetExtractcount + "&include_entities=true";

        //        }
        //        else if (NumberHelper.ValidateNumber(keyword))
        //        {
        //            searchURL = "https://api.twitter.com/1/statuses/retweeted_by_user.xml?id=" + keyword + "&count=" + RetweetExtractcount + "&include_entities=true";
        //        }

        //        //string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //        ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
        //        string res_Get_searchURL = HttpHelper.GetHtml(searchURL);

        //       // if (!res_Get_searchURL.Contains("Rate limit exceeded") && !res_Get_searchURL.Contains("Sorry, that page does not exist") && res_Get_searchURL.Contains("created_at") && !string.IsNullOrEmpty(res_Get_searchURL))
        //        if (!res_Get_searchURL.Contains("Rate limit exceeded") && !res_Get_searchURL.Contains("Sorry, that page does not exist")  && !string.IsNullOrEmpty(res_Get_searchURL))
        //        {
        //            string[] arrGetRetweeted = Regex.Split(res_Get_searchURL, "js-tweet-text tweet-text");
        //            foreach (string item in arrGetRetweeted)
        //            {
        //                try
        //                {
        //                    if (item.Contains("Retweeted by"))
        //                    {
        //                        int Startindex = item.IndexOf("\\\"");
        //                        string start = item.Substring(Startindex);
        //                        int EndIndex = start.IndexOf("\\/p");
        //                        string End = start.Substring(0, EndIndex).Replace("\\\"", "").Replace("\\u003", "").Replace("&amp", "");
        //                        End = End.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "").Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", ""); ;

        //                    }
        //               }
        //            //    catch { }
        //            //}

        //            //string[] splitRes = Regex.Split(res_Get_searchURL, "status");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
        //            //splitRes = splitRes.Skip(1).ToArray();

        //            //foreach (string item in splitRes)
        //            //{
        //            //    string Tweet = string.Empty;
        //            //    string Tweeter = string.Empty;
        //            //    try
        //            //    {
        //            //        int startIndex = item.IndexOf("<text>");
        //            //        string start = item.Substring(startIndex);
        //            //        int endIndex = start.IndexOf("</text>");
        //            //        string end = start.Substring(0, endIndex);
        //            //        Tweet = end.Replace("<text>", "");

        //            //        int startOfInndex = Tweet.IndexOf(":");
        //            //        Tweeter = Tweet.Substring(0, startOfInndex);
        //            //        Tweet = Tweet.Replace(Tweeter, "");
        //            //    }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                //string data = keyword + ":" + Tweeter + ":" + Tweet.Replace(":", "^");
        //                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_RETweetExtractor);
        //                //lst_ReTweetIDs.Add(data);
        //            }
        //        }
        //        else if (res_Get_searchURL.Contains("Rate limit exceeded"))
        //        {
        //            status = "Rate limit exceeded";
        //        }
        //        else if (res_Get_searchURL.Contains("Sorry, that page does not exist"))
        //        {
        //            status = "Sorry, that page does not exist";
        //        }
        //        else if (res_Get_searchURL.Contains("Not authorized"))
        //        {
        //            status = "Not Authorized";
        //        }
        //        else if (string.IsNullOrEmpty(res_Get_searchURL))
        //        {
        //            status = "Not Authorized";
        //        }
        //        else
        //        {
        //            status = "Empty";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        status = "Error";
        //    }
        //    returnStatus = status;
        //    return lst_ReTweetIDs;
        //} 
        #endregion

        public List<string> GetRetweetData_Scrape(string keyword, out string returnStatus)
        {
            string status = string.Empty;
            List<string> lst_ReTweetIDs = new List<string>();
            try
            {
                string searchURL = string.Empty;

                int extendendCount = 0;

                extendendCount = RetweetExtractcount * 10;
            

                if (!NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + extendendCount + "&include_available_features=1&include_entities=1";
                    
                }
                else if (NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://api.twitter.com/1/statuses/retweeted_by_user.xml?id=" + keyword + "&count=" + extendendCount + "&include_entities=true";
                }

                //string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string res_Get_searchURL = HttpHelper.GetHtml(searchURL);
                if (string.IsNullOrEmpty(res_Get_searchURL))
                {
                    res_Get_searchURL = HttpHelper.GetHtml(searchURL);
                }
                var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                string DataHtml = string.Empty;

                try
                {
                    DataHtml = (string)avc["items_html"];
                }
                catch { };

                if (string.IsNullOrEmpty(DataHtml))
                {
                    foreach (object data in avc)
                    {
                        DataHtml = data.ToString();
                    }
                }

                string[] splitRes = Regex.Split(DataHtml, "ProfileTweet u-textBreak js-tweet js-stream-tweet js-actionable-tweet");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                splitRes = splitRes.Skip(1).ToArray();
                GlobusRegex regx = new GlobusRegex();
                foreach (string item in splitRes)
                {
                    string text = string.Empty;

                    if (item.Contains("retweeted Icon"))
                    {
                        try
                        {
                            int startindex = item.IndexOf("ProfileTweet-text js-tweet-text u-dir");

                            string start = item.Substring(startindex).Replace("ProfileTweet-text js-tweet-text u-dir", "").Replace("js-tweet-text tweet-text tweet-text-rtl\\\"", "");
                            int endindex = start.IndexOf("</p>");

                            if (endindex == -1)
                            {
                                endindex = 0;
                                endindex = start.IndexOf("stream-item-footer");
                            }

                            string end = start.Substring(0, endindex);
                            end = regx.StripTagsRegex(end);
                            text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                            text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("&#39;", "'").Replace("&amp;", "&").Replace("=&gt;", "=>").Replace("&#10;", " ").Replace("\\", "").Replace("title=", "").Replace("js-tweet-text tweet-text", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = ("http" + itemData).Replace(" span ", string.Empty);
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("lang=en", string.Empty).Replace("dir=ltr data-aria-label-part=0", string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("lang=en", string.Empty).Replace("dir=ltr data-aria-label-part=0", string.Empty).Replace("\n",string.Empty).Replace("     ",string.Empty);
                                        }
                                    }
                                }
                            }
                            if (text.Contains("data-aria-label-part=0"))
                            {
                                text = globushttpHelper.getBetween(text + ":&$#@", "data-aria-label-part=0", ":&$#@");
                            }


                            //Remove RT or @ mentions 

                            #region -----RT or @ mentions-------
                            if (RemoveRTMSg && text.StartsWith("RT"))
                                continue;

                            if (removeAtMentions && text.Contains("@"))
                                continue;

                            #endregion



                            if (lst_ReTweetIDs.Count() < RetweetExtractcount)
                            {
                                string txtdata = keyword + ":" + (text.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("js-tweet-text tweet-text", string.Empty).Replace("#", string.Empty)).Replace("dir=ltr", "");
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(txtdata, Globals.Path_RETweetExtractor);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(text.Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("js-tweet-text tweet-text", string.Empty).Replace("#", string.Empty).Replace("dir=ltr", "").Trim(), Globals.Path_RETweetExtractorUpload);
                                lst_ReTweetIDs.Add(txtdata);
                            }

                            status = "No Error";
                        }

                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                    
                }
               
            }
            catch (Exception ex)
            {
                status = "Empty";
            }

            returnStatus = status;
            return lst_ReTweetIDs;

        }

        public string GetUserLastTweetDate(string userid)
        {
            string date = "";

            try
            {
                string user_id = string.Empty;

                string searchURL = string.Empty;

                if (!NumberHelper.ValidateNumber(userid))
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&id=" + user_id + "&count=" + TweetExtractCount;
                }
                else if (NumberHelper.ValidateNumber(userid))
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&screen_name=" + user_id + "&count=" + TweetExtractCount;
                }

                //string searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&id=" + user_id + "&count=" + TweetExtractCount;
                ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();
                //string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                string res_Get_searchURL = httpHelper.GetHtml(searchURL);

                string[] splitRes = Regex.Split(res_Get_searchURL, "{\"created_at");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                splitRes = splitRes.Skip(1).ToArray();

                foreach (string item in splitRes)
                {
                    //string text = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "<text>");
                    string modified_Item = "{\"created_at" + item;

                    date = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "created_at");//Globussoft.GlobusHttpHelper.parseText(item);
                    if (date.Contains("+"))
                    {
                        date = date.Remove(date.IndexOf("+")).Trim();
                    }
                    if (!string.IsNullOrEmpty(date))
                    {
                        return date;
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetUserLastTweetDate() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetUserLastTweetDate() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return date;
        }

        public List<string> GetFollowers(string userID, out string ReturnStatus)
        {
            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            try
            {
            StartAgain:
                if (NumberHelper.ValidateNumber(userID))
                {
                    FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=" + cursor + "&id=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                }
                else
                {
                    FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=" + cursor + "&screen_name=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                }

                #region gs http helper code
                //https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro
                //string Data = globushttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                #endregion

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string Data = HttpHelper.GetHtml(FollowingUrl);

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return new List<string>();
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    int FirstPoint = Data.IndexOf("[");
                    int SecondPoint = Data.IndexOf("]");

                    string FollowingIds = Data.Substring(FirstPoint, SecondPoint - FirstPoint).Replace("[", string.Empty).Replace("]", string.Empty);

                    List<string> tempid = FollowingIds.Split(',').ToList();

                    foreach (string item in tempid)
                    {
                        lstIds.Add(item);
                    }


                    if (Data.Contains("next_cursor_str"))
                    {
                        int startindex = Data.IndexOf("\"next_cursor_str\":");
                        string start = Data.Substring(startindex).Replace("\"next_cursor_str\":", "");
                        int lastindex = start.IndexOf("\",\"");
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }
                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                return lstIds;
            }
        }

        public List<string> GetFollowers1(string userID,string Screen_name, out string ReturnStatus, Globussoft.GlobusHttpHelper Ghelper)
        {
            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            try
            {

                Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
                HttpHelper = Ghelper;

                StartAgain:
                if (NumberHelper.ValidateNumber(userID))
                {
                    string UserPageUrl = string.Empty;
                    if (cursor == "-1")
                    {
                        //UserPageUrl = "https://twitter.com/account/redirect_by_id?id=" + userID;
                        UserPageUrl = "https://twitter.com/" + Screen_name;
                    }
                    else
                    {
                        UserPageUrl = "https://twitter.com/" + Screen_name + "/followers/users?cursor=" + cursor + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
                    }
                    //FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=" + cursor + "&id=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                    string GetUSerPage = HttpHelper.getHtmlfromUrl(new Uri(UserPageUrl), "", "");

                    FollowingUrl = HttpHelper.gResponse.ResponseUri + "/followers";

                }
                else
                {
                    FollowingUrl = "https://twitter.com/" + userID + "/followers";
                }

                String DataCursor = string.Empty;


                string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                String DataCursor1 = string.Empty;

                if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {

                    String[] DataDivArr;
                    if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-item stream-item stream-item");
                    }
                    else
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-item");
                    }

                    foreach (var DataDivArr_item in DataDivArr)
                    {
                        if (DataDivArr_item.Contains("data-cursor"))
                        {
                            String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
                            DataCursor1 = DataCurso.Substring(DataCurso.IndexOf("="), DataCurso.IndexOf(">")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
                        }
                        if (DataDivArr_item.Contains("<!DOCTYPE html>") || DataDivArr_item.Contains("cursor"))
                        {
                            continue;
                        }

                        if (DataDivArr_item.Contains("data-screen-name") && DataDivArr_item.Contains(" data-user-id"))
                        {
                            int endIndex = 0;
                            int startIndex = DataDivArr_item.IndexOf("data-screen-name");
                            try
                            {
                                endIndex = DataDivArr_item.IndexOf(">");
                            }
                            catch { }

                            if (endIndex == -1)
                            {
                                endIndex = DataDivArr_item.IndexOf("data-feedback-token");
                            }

                            string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);

                            string _SCRNameID = string.Empty;
                            string _SCRName = string.Empty;
                            try
                            {
                                 _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-feedback-token", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                                 _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-user-id", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());

                            }
                            catch { }
                            if (TweetAccountManager.noOfUnfollows > lstIds.Count)
                            {
                                if (!string.IsNullOrEmpty(_SCRName))
                                {
                                    lstIds.Add(_SCRName + ":" + _SCRNameID);
                                }
                            }
                           

                        }

                    }
                    

                    if (TweetAccountManager.noOfUnfollows != lstIds.Count)
                    {


                        if (Data.Contains("data-cursor"))
                        {
                            int startindex = Data.IndexOf("data-cursor");
                            string start = Data.Substring(startindex).Replace("data-cursor", "");
                            int lastindex = start.IndexOf("<div class=\"stream profile-stream\">");
                            if (lastindex == -1)
                            {
                                lastindex = start.IndexOf("\n");
                            }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Trim();
                            cursor = end;


                            if (cursor != "0")
                            {


                                goto StartAgain;
                            }
                        }

                        if (Data.Contains("cursor"))
                        {
                            int startindex = Data.IndexOf("cursor");
                            string start = Data.Substring(startindex).Replace("cursor", "");
                            int lastindex = -1;
                            
                                lastindex = start.IndexOf(",");
                                if (lastindex > 40)
                                {
                                    lastindex = start.IndexOf("\n");

                                }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(":", string.Empty).Trim();
                            cursor = end;
                            if (cursor != "0")
                            {
                                goto StartAgain;
                            }
                        }

                    }
                    //FollowingUrl = "https://twitter.com/Iloveindia6SihF/followers/users?cursor=" + DataCursor1 + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
                    //FollowingUrl = "https://twitter.com/Fergie/followers/users?cursor=" + DataCursor1 + "&include_available_features=1&include_entities=1&is_forward=true";

                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return new List<string>();
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                return lstIds;
            }
        }

        public List<string> GetFollowYourFollowers(string userID, string Screen_name, out string ReturnStatus, Globussoft.GlobusHttpHelper Ghelper)
        {

            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            try
            {

                Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
                HttpHelper = Ghelper;

            StartAgain:
                if (NumberHelper.ValidateNumber(userID))
                {
                    string UserPageUrl = string.Empty;
                    if (cursor == "-1")
                    {
                        UserPageUrl = "https://twitter.com/account/redirect_by_id?id=" + userID;
                    }
                    else
                    {
                        UserPageUrl = "https://twitter.com/" + Screen_name + "/followers/users?cursor=" + cursor + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
                    }
                    //FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=" + cursor + "&id=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                    string GetUSerPage = HttpHelper.getHtmlfromUrl(new Uri(UserPageUrl), "", "");

                    FollowingUrl = HttpHelper.gResponse.ResponseUri + "/followers";

                }
                else
                {
                    FollowingUrl = "https://twitter.com/" + userID + "/followers";
                }

                String DataCursor = string.Empty;


                string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                String DataCursor1 = string.Empty;

                if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    String[] DataDivArr = System.Text.RegularExpressions.Regex.Split(Data, "user-actions btn-group not-following not-muting can-dm  ");
                    if (DataDivArr.Count() == 1)
                    {
                        DataDivArr = System.Text.RegularExpressions.Regex.Split(Data, "user-actions btn-group not-following not-muting can-dm ");
                    }

                   
                    foreach (var DataDivArr_item in DataDivArr)
                    {
                        if (DataDivArr_item.Contains("data-cursor"))
                        {
                            String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
                            DataCursor1 = DataCurso.Substring(DataCurso.IndexOf("="), DataCurso.IndexOf(">")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
                        }
                        if (DataDivArr_item.Contains("<!DOCTYPE html>") || DataDivArr_item.Contains("cursor"))
                        {
                            continue;
                        }

                        if (DataDivArr_item.Contains("data-screen-name") && DataDivArr_item.Contains(" data-user-id"))
                        {
                            int endIndex = 0;
                            int startIndex = DataDivArr_item.IndexOf(" data-user-id");
                            try
                            {
                                endIndex = DataDivArr_item.IndexOf(">");
                            }
                            catch { }

                            if (endIndex == -1)
                            {
                                endIndex = DataDivArr_item.IndexOf("data-feedback-token");
                            }
                            if (endIndex == -1)
                            {
                                endIndex = DataDivArr_item.IndexOf(" data-protected=\\\"false");
                            }

                            try
                            {
                                string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);


                                string _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-screen-name", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                                string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-name", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                          
                                //string _SCRNameID = System.Text.RegularExpressions.Regex.Split(GetDataStr, "data-screen-name")[0].Replace("data-user-id=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("n", "").Trim();
                                //string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name"), GetDataStr.IndexOf("data-name=", GetDataStr.IndexOf("data-screen-name")) - GetDataStr.IndexOf("data-screen-name")).Replace("data-screen-name", string.Empty).Replace("\\\"", string.Empty).Replace("\"", string.Empty).Replace(",", "").Replace("=", "").Trim());

                                if (TweetAccountManager.noOfUnfollows > lstIds.Count)
                                {
                                    //lstIds.Add(_SCRName + ":" + _SCRNameID);
                                    lstIds.Add(_SCRName);
                                }
                            }
                            catch { }

                        }

                    }


                    if (TweetAccountManager.noOfUnfollows != lstIds.Count)
                    {


                        if (Data.Contains("data-cursor"))
                        {
                            int startindex = Data.IndexOf("data-cursor");
                            string start = Data.Substring(startindex).Replace("data-cursor", "");
                            int lastindex = start.IndexOf("<div class=\"stream profile-stream\">");
                            if (lastindex == -1)
                            {
                                lastindex = start.IndexOf("\n");
                            }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Trim();
                            cursor = end;


                            if (cursor != "0")
                            {


                                goto StartAgain;
                            }
                        }

                        if (Data.Contains("cursor"))
                        {
                            int startindex = Data.IndexOf("cursor");
                            string start = Data.Substring(startindex).Replace("cursor", "");
                            int lastindex = start.IndexOf(",");
                            if (lastindex > 40)
                            {
                                 lastindex = start.IndexOf("\n");
                            }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(":", string.Empty).Trim();
                            cursor = end;
                            if (cursor != "0")
                            {
                                goto StartAgain;
                            }
                        }

                    }
                    //FollowingUrl = "https://twitter.com/Iloveindia6SihF/followers/users?cursor=" + DataCursor1 + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
                    //FollowingUrl = "https://twitter.com/Fergie/followers/users?cursor=" + DataCursor1 + "&include_available_features=1&include_entities=1&is_forward=true";

                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return new List<string>();
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                return lstIds;
            }
        }

         
        public List<string> GetFollowers_New(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        {
            Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;

            try
            {

            StartAgain:
                //Thread.Sleep(1000);
                string[] splitRes = new string[] { }; 
                if (counter == 0)
                {
                    string aa = "https://twitter.com/" + userID + "/followers";
                    Data = HttpHelper.getHtmlfromUrl(new Uri(aa), "", "");
                    if (Data.Contains("<div class=\"stream-container"))
                    {
                        splitRes = Regex.Split(Data, "<div class=\"stream-container");
                    }
                    else
                    {
                        splitRes = Regex.Split(Data, "<div class=\"GridTimeline-items");
                    }
                    splitRes = splitRes.Skip(1).ToArray();

                    if (splitRes[0].Contains("<div class=\"stream profile-stream\">"))
                    {
                        splitRes = Regex.Split(splitRes[0], "<div class=\"stream profile-stream\">");
                        cursor = splitRes[0].Replace("\"", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("data-cursor=", string.Empty).Trim();

                    }
                    else
                    {
                        cursor = (splitRes[0].Substring(splitRes[0].IndexOf("data-cursor="), splitRes[0].IndexOf(">", splitRes[0].IndexOf("data-cursor=")) - splitRes[0].IndexOf("data-cursor=")).Replace("data-cursor=", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace(",", "").Trim());
                    }
                    counter++;
                    cursor = "-1";
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                    

                    //Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }
                else
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                    if (string.IsNullOrEmpty(Data))
                    {
                        
                        for (int i = 1; i <= 3; i++)
                        {
                            Thread.Sleep(3000);
                            Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                            if (!string.IsNullOrEmpty(Data))
                            {
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(Data))
                        //{
                        //    Log(" pagesource not found ");
                        //}
                    }
                    if (Data=="Too Many Requestes")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15*60*1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                        if (string.IsNullOrEmpty(Data) || Data=="Too Many Requestes")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
                            Thread.Sleep(5 * 60 * 1000);
                            for (int i = 1; i <= 3; i++)
                            {
                                Thread.Sleep(3000);
                                Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                                if (!string.IsNullOrEmpty(Data))
                                {
                                    break;
                                }
                            }
                            //if (string.IsNullOrEmpty(Data))
                            //{
                            //    Log(" pagesource not found ");
                            //}
                        }
                    }

                    var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Data);
                    cursor = string.Empty;
                    //string DataHtml = (string)avc["items_html"];
                    cursor = (string)avc["cursor"];
                }

                if (cursor == "0")
                {
                    Thread.Sleep(2000);
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return lstIds;
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    string[] arraydata;
                    string startWith = string.Empty;
                    if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                         arraydata = Regex.Split(Data, "js-stream-item stream-item stream-item");
                         startWith = "";
                    }
                    else 
                    {
                       arraydata = Regex.Split(Data, "js-stream-item");
                       startWith = "\\\" role=\\\"listitem\\\"";

                    }
                    arraydata = arraydata.Skip(1).ToArray();
                    foreach (string id in arraydata)
                    {
                        if (!id.StartsWith(startWith))
                        {
                            continue;
                        }
                       

                        string userid = string.Empty;
                        string username = string.Empty;
                        if (cursor == "0")
                        {
                            try
                            {

                                int startindex = id.IndexOf("data-user-id=");
                                string start = id.Substring(startindex).Replace("data-user-id=", string.Empty);
                                int endindex = start.IndexOf("data-feedback-token");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                userid = end;

                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                int startindex = id.IndexOf("data-item-id=\\\"");
                                string start = id.Substring(startindex).Replace("data-item-id=\\\"", string.Empty);
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                userid = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }


                        if (cursor == "0")
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=");
                                string start = id.Substring(startindex).Replace("data-screen-name=", string.Empty);
                                int endindex = start.IndexOf("data-user-id=");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                username = end;

                            }
                            catch { }
                        }
                        else
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=\\\"");
                                string start = id.Substring(startindex).Replace("data-screen-name=\\\"", "");
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                username = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }

                       

                        if (CounterDataNo > 0)
                        {
                            if (lstIds.Count == CounterDataNo)
                            {
                                ReturnStatus = "No Error";
                                lstIds = lstIds.Distinct().ToList();
                                return lstIds;
                            }
                            else
                            {
                                Globals.lstScrapedUserIDs.Add(userid);
                                lstIds.Add(userid + ":" + username);
                                lstIds = lstIds.Distinct().ToList();

                                if (username.Contains("/span") || userid.Contains("/span"))
                                { 
                                
                                }

                                Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowersList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(username, Globals.Path_ScrapedFollowersListtxt);  //for txt format.
                            }
                        }

                    

                        try
                        {
                            //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userid))
                            //{
                            //    string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + username + "' ,'" + userid + "') ";
                            //    DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                            //}
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }

                  

                    if (Data.Contains("\"has_more_items\":true"))
                    {
                        int startindex = Data.IndexOf("\"cursor\":");
                        string start = Data.Substring(startindex).Replace("\"cursor\":", "");
                        int lastindex = start.IndexOf("\",\"");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf("\"}");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                lstIds = lstIds.Distinct().ToList();
                return lstIds;
            }
        }

        public List<string> GetFollowers_New_ForMobileVersion(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        {
            Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;

            try
            {

            StartAgain:
                //Thread.Sleep(1000);
                string[] splitRes = new string[] { };
                if (counter == 0)
                {
                    string aa = "https://mobile.twitter.com/" + userID + "/followers";
                    Data = HttpHelper.getHtmlfromUrl(new Uri(aa), "", "");
                    if (Data.Contains("class=\"w-button-more\""))
                    {
                        int startindex = Data.IndexOf("cursor=");
                        string start = Data.Substring(startindex).Replace("cursor=", "");
                        int lastindex = start.IndexOf(">");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf(">");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                    }
                    else
                    {
                        //splitRes = Regex.Split(Data, "<div class=\"GridTimeline-items");
                    }
                    
                    counter++;
                    

                    //Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }
                else
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor="+cursor), "", "");
                    if (string.IsNullOrEmpty(Data))
                    {

                        for (int i = 1; i <= 3; i++)
                        {
                            Thread.Sleep(3000);
                            Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
                            if (!string.IsNullOrEmpty(Data))
                            {
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(Data))
                        //{
                        //    Log(" pagesource not found ");
                        //}
                    }
                    if (Data == "Too Many Requestes")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
                            Thread.Sleep(5 * 60 * 1000);
                            for (int i = 1; i <= 3; i++)
                            {
                                Thread.Sleep(3000);
                                Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
                                if (!string.IsNullOrEmpty(Data))
                                {
                                    break;
                                }
                            }
                            //if (string.IsNullOrEmpty(Data))
                            //{
                            //    Log(" pagesource not found ");
                            //}
                        }
                    }

                    //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Data);
                    //cursor = string.Empty;
                    //string DataHtml = (string)avc["items_html"];
                    //cursor = (string)avc["cursor"];
                }

                if (cursor == "0")
                {
                    Thread.Sleep(2000);
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
                }

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return lstIds;
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    string[] arraydata = null;
                    string startWith = string.Empty;
                    if (Data.Contains("<strong class=\"fullname\">"))
                    {
                        arraydata = Regex.Split(Data, "<strong class=\"fullname\">");
                        //startWith = "";
                    }
                    else
                    {
                        //arraydata = Regex.Split(Data, "js-stream-item");
                        //startWith = "\\\" role=\\\"listitem\\\"";

                    }
                    arraydata = arraydata.Skip(1).ToArray();
                    
                    foreach (string id in arraydata)
                    {
                        
                        string userid = string.Empty;
                        string username = string.Empty;
                        if (cursor == "0")
                        {
                            try
                            {

                                int startindex = id.IndexOf("id=");
                                string start = id.Substring(startindex).Replace("id=", string.Empty);
                                int endindex = start.IndexOf("&amp;");
                                string end = start.Substring(0, endindex);
                                userid = end;

                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                int startindex = id.IndexOf("id=");
                                string start = id.Substring(startindex).Replace("id=", string.Empty);
                                int endindex = start.IndexOf("&amp;");
                                string end = start.Substring(0, endindex);
                                userid = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }


                        if (cursor == "0")
                        {

                            try
                            {

                                int startindex = id.IndexOf("@</span>");
                                string start = id.Substring(startindex).Replace("@</span>", string.Empty);
                                int endindex = start.IndexOf("</span></a>");
                                string end = start.Substring(0, endindex);
                                username = end;

                            }
                            catch { }
                        }
                        else
                        {

                            try
                            {

                                int startindex = id.IndexOf("@</span>");
                                string start = id.Substring(startindex).Replace("@</span>", string.Empty);
                                int endindex = start.IndexOf("</span></a>");
                                string end = start.Substring(0, endindex);
                                username = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }



                        if (CounterDataNo > 0)
                        {
                            if (lstIds.Count == CounterDataNo)
                            {
                                ReturnStatus = "No Error";
                                lstIds = lstIds.Distinct().ToList();
                                return lstIds;
                            }
                            else
                            {
                                Globals.lstScrapedUserIDs.Add(userid);
                                lstIds.Add(userid + ":" + username);
                                lstIds = lstIds.Distinct().ToList();

                                if (username.Contains("/span") || userid.Contains("/span"))
                                {

                                }

                                Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                //GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowersList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + username, Globals.Path_ScrapedFollowersList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(username, Globals.Path_ScrapedFollowersListtxt);
                            }
                        }



                        try
                        {

                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }



                    if (Data.Contains("Show more people"))
                    {
                        int startindex = Data.IndexOf("cursor=");
                        string start = Data.Substring(startindex).Replace("cursor=", "");
                        int lastindex = start.IndexOf(">");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf(">");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (!Data.Contains("Show more people"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                lstIds = lstIds.Distinct().ToList();
                return lstIds;
            }
        }


        public List<string> GetFollowings_NewForMobileVersion(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        {

            Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;

            try
            {

            StartAgain:
                //Thread.Sleep(1000);
                string[] splitRes = new string[] { };
                if (counter == 0)
                {
                    string aa = "https://mobile.twitter.com/" + userID + "/following";
                    Data = HttpHelper.getHtmlfromUrl(new Uri(aa), "", "");
                    if (Data.Contains("class=\"w-button-more\""))
                    {
                        int startindex = Data.IndexOf("cursor=");
                        string start = Data.Substring(startindex).Replace("cursor=", "");
                        int lastindex = start.IndexOf(">");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf(">");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                    }
                    else
                    {
                        //splitRes = Regex.Split(Data, "<div class=\"GridTimeline-items");
                    }

                    counter++;


                    //Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }
                else
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                    if (string.IsNullOrEmpty(Data))
                    {

                        for (int i = 1; i <= 3; i++)
                        {
                            Thread.Sleep(3000);
                            Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                            if (!string.IsNullOrEmpty(Data))
                            {
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(Data))
                        //{
                        //    Log(" pagesource not found ");
                        //}
                    }
                    if (Data == "Too Many Requestes")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
                            Thread.Sleep(5 * 60 * 1000);
                            for (int i = 1; i <= 3; i++)
                            {
                                Thread.Sleep(3000);
                                Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                                if (!string.IsNullOrEmpty(Data))
                                {
                                    break;
                                }
                            }
                            //if (string.IsNullOrEmpty(Data))
                            //{
                            //    Log(" pagesource not found ");
                            //}
                        }
                    }

                    //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Data);
                    //cursor = string.Empty;
                    //string DataHtml = (string)avc["items_html"];
                    //cursor = (string)avc["cursor"];
                }

                if (cursor == "0")
                {
                    Thread.Sleep(2000);
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                }

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return lstIds;
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    string[] arraydata = null;
                    string startWith = string.Empty;
                    if (Data.Contains("<strong class=\"fullname\">"))
                    {
                        arraydata = Regex.Split(Data, "<strong class=\"fullname\">");
                        //startWith = "";
                    }
                    else
                    {
                        //arraydata = Regex.Split(Data, "js-stream-item");
                        //startWith = "\\\" role=\\\"listitem\\\"";

                    }
                    arraydata = arraydata.Skip(1).ToArray();

                    foreach (string id in arraydata)
                    {

                        string userid = string.Empty;
                        string username = string.Empty;
                        if (cursor == "0")
                        {
                            try
                            {

                                int startindex = id.IndexOf("id=");
                                string start = id.Substring(startindex).Replace("id=", string.Empty);
                                int endindex = start.IndexOf("&amp;");
                                string end = start.Substring(0, endindex);
                                userid = end;

                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                int startindex = id.IndexOf("id=");
                                string start = id.Substring(startindex).Replace("id=", string.Empty);
                                int endindex = start.IndexOf("&amp;");
                                string end = start.Substring(0, endindex);
                                userid = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }


                        if (cursor == "0")
                        {

                            try
                            {

                                int startindex = id.IndexOf("@</span>");
                                string start = id.Substring(startindex).Replace("@</span>", string.Empty);
                                int endindex = start.IndexOf("</span></a>");
                                string end = start.Substring(0, endindex);
                                username = end;

                            }
                            catch { }
                        }
                        else
                        {

                            try
                            {

                                int startindex = id.IndexOf("@</span>");
                                string start = id.Substring(startindex).Replace("@</span>", string.Empty);
                                int endindex = start.IndexOf("</span></a>");
                                string end = start.Substring(0, endindex);
                                username = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }



                        if (CounterDataNo > 0)
                        {
                            if (lstIds.Count == CounterDataNo)
                            {
                                ReturnStatus = "No Error";
                                lstIds = lstIds.Distinct().ToList();
                                return lstIds;
                            }
                            else
                            {
                                Globals.lstScrapedUserIDs.Add(userid);
                                lstIds.Add(userid + ":" + username);
                                lstIds = lstIds.Distinct().ToList();

                                if (username.Contains("/span") || userid.Contains("/span"))
                                {

                                }

                                Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                              //  GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowingsList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + username, Globals.Path_ScrapedFollowingsList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(username, Globals.Path_ScrapedFollowingsListtxt);
                            }
                        }


                        try
                        {

                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }



                    if (Data.Contains("Show more people"))
                    {
                        int startindex = Data.IndexOf("cursor=");
                        string start = Data.Substring(startindex).Replace("cursor=", "");
                        int lastindex = start.IndexOf(">");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf(">");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (!Data.Contains("Show more people"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                lstIds = lstIds.Distinct().ToList();
                return lstIds;
            }
        }



        public List<string> GetFollowings(string userID, out string ReturnStatus)
        {
            try
            {
                string cursor = "-1";
                string FollowingUrl = string.Empty;
            StartAgain:

                if (NumberHelper.ValidateNumber(userID))
                {
                    FollowingUrl = "https://api.twitter.com/1/following/ids.json?cursor=" + cursor + "&id=" + userID + "";//"https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro";
                }
                else
                {
                    FollowingUrl = "https://api.twitter.com/1/following/ids.json?cursor=" + cursor + "&screen_name=" + userID + "";//"https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro";
                }

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string Data = HttpHelper.GetHtml(FollowingUrl);//.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                if (Data.Contains("that page does not exist"))
                {
                    ReturnStatus = "that page does not exist";
                    return new List<string>();
                }
                else if (!Data.Contains("Rate limit exceeded"))
                {
                    int FirstPoint = Data.IndexOf("[");
                    int SecondPoint = Data.IndexOf("]");

                    string FollowingIds = Data.Substring(FirstPoint, SecondPoint - FirstPoint).Replace("[", string.Empty).Replace("]", string.Empty);

                    List<string> lstIds = FollowingIds.Split(',').ToList();

                    if (Data.Contains("next_cursor_str"))
                    {
                        int startindex = Data.IndexOf("\"next_cursor_str\":");
                        string start = Data.Substring(startindex).Replace("\"next_cursor_str\":", "");
                        int lastindex = start.IndexOf("\",\"");
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowings() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowings() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                return new List<string>();
            }
        }

        #region commented on 23/9 by puja
        //public List<string> GetFollowings1(string userID, string Screen_name, out string ReturnStatus, Globussoft.GlobusHttpHelper Ghelper)
        //{
        //    string cursor = "-1";
        //    string FollowingUrl = string.Empty;
        //    List<string> lstIds = new List<string>();
        //    try
        //    {

        //        Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
        //        HttpHelper = Ghelper;
        //    StartAgain:
        //        if (NumberHelper.ValidateNumber(userID))
        //        {
        //            string UserPageUrl = string.Empty;
        //            if (cursor == "-1")
        //            {
        //                //UserPageUrl = "https://twitter.com/account/redirect_by_id?id=" + userID;
        //                UserPageUrl = "https://twitter.com/" + Screen_name;
        //            }
        //            else
        //            {
        //                UserPageUrl = "https://twitter.com/" + Screen_name + "/following/users?cursor=" + cursor + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
        //            }

        //            //string UserPageUrl = "https://twitter.com/account/redirect_by_id?id=" + userID;
        //            ////FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=" + cursor + "&id=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
        //            string GetUSerPage = HttpHelper.getHtmlfromUrl(new Uri(UserPageUrl), "", "");

        //            FollowingUrl = HttpHelper.gResponse.ResponseUri + "/following";

        //        }
        //        else
        //        {
        //            FollowingUrl = "https://twitter.com/" + userID + "/following";
        //        }

        //        String DataCursor = string.Empty;


        //        string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

        //        String DataCursor1 = string.Empty;

        //        if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
        //        {
        //            #region

        //            //String[] DataDivArr = System.Text.RegularExpressions.Regex.Split(Data, "username js-action-profile-name");

        //            //foreach (var DataDivArr_item in DataDivArr)
        //            //{
        //            //try
        //            //{
        //            //    if (DataDivArr_item.Contains("data-cursor"))
        //            //    {
        //            //        String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
        //            //        DataCursor1 = DataCurso.Substring(DataCurso.IndexOf("="), DataCurso.IndexOf(">")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
        //            //    }
        //            //    if (DataDivArr_item.Contains("<!DOCTYPE html>"))
        //            //    {
        //            //        continue;
        //            //    }

        //            //    string ScreenName = DataDivArr_item.Substring(DataDivArr_item.IndexOf(">@"), DataDivArr_item.IndexOf("<")).Replace(">", string.Empty).Replace("<", string.Empty).Replace("@", string.Empty);

        //            //    if (!string.IsNullOrEmpty(ScreenName))
        //            //    {
        //            //        lstIds.Add(ScreenName);
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //            //    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //            //}
        //            //}
        //            #endregion

        //            string[] DataDivArr;
        //            if (Data.Contains("js-stream-item stream-item stream-item"))
        //            {
        //                DataDivArr = Regex.Split(Data, "js-stream-item stream-item stream-item");
        //            }
        //            else
        //            {
        //                DataDivArr = Regex.Split(Data, "js-stream-item");
        //            }

        //            foreach (var DataDivArr_item in DataDivArr)
        //            {
        //                if (DataDivArr_item.Contains("data-cursor"))
        //                {
        //                    String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
        //                    DataCursor1 = DataCurso.Substring(DataCurso.IndexOf("="), DataCurso.IndexOf(">")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
        //                }
        //                if (DataDivArr_item.Contains("<!DOCTYPE html>") || DataDivArr_item.Contains("cursor"))
        //                {
        //                    continue;
        //                }

        //                if (DataDivArr_item.Contains("data-screen-name") && DataDivArr_item.Contains(" data-user-id"))
        //                {
        //                    int endIndex = 0;
        //                    int startIndex = DataDivArr_item.IndexOf("data-screen-name");
        //                    try
        //                    {
        //                        endIndex = DataDivArr_item.IndexOf(">");
        //                    }
        //                    catch { }

        //                    if (endIndex == -1)
        //                    {
        //                        endIndex = DataDivArr_item.IndexOf("data-feedback-token");
        //                    }

        //                    string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);

        //                    string _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-feedback-token", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
        //                    string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-user-id", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());

        //                    //string _SCRName = System.Text.RegularExpressions.Regex.Split(GetDataStr, "data-user-id=")[0].Replace("data-screen-name=", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Trim();
        //                    //string _SCRNameID = System.Text.RegularExpressions.Regex.Split(GetDataStr, "data-user-id=")[1].Replace(">", string.Empty).Replace(" \n", string.Empty).Replace("\"", string.Empty).Replace("\\", string.Empty).Replace("data", string.Empty).Replace("-feedback-token= -impression-id=", string.Empty).Trim();
        //                    if (TweetAccountManager.noOfUnfollows > lstIds.Count)
        //                    {
        //                        lstIds.Add(_SCRName + ":" + _SCRNameID);
        //                    }
        //                }

        //            }

        //            if (TweetAccountManager.noOfUnfollows != lstIds.Count)
        //            {


        //                if (Data.Contains("data-cursor"))
        //                {
        //                    int startindex = Data.IndexOf("data-cursor");
        //                    string start = Data.Substring(startindex).Replace("data-cursor", "");
        //                    int lastindex = start.IndexOf("<div class=\"stream profile-stream\">");
        //                    if (lastindex == -1)
        //                    {
        //                        lastindex = start.IndexOf("\n");
        //                    }
        //                    string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Trim();
        //                    cursor = end;

        //                    if (cursor != "0")
        //                    {
        //                        goto StartAgain;
        //                    }
        //                }

        //                if (Data.Contains("cursor"))
        //                {
        //                    int startindex = Data.IndexOf("cursor");
        //                    string start = Data.Substring(startindex).Replace("cursor", "");
        //                    int lastindex = -1;
        //                    lastindex = start.IndexOf(",");

        //                    if (lastindex > 50)
        //                    {
        //                        lastindex = start.IndexOf("\n");
        //                    }
        //                    string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(":", string.Empty).Trim();
        //                    cursor = end;
        //                    if (cursor != "0")
        //                    {
        //                        goto StartAgain;
        //                    }
        //                }

        //            }


        //            ReturnStatus = "No Error";
        //            return lstIds;
        //        }
        //        else if (Data.Contains("401 Unauthorized"))
        //        {
        //            ReturnStatus = "Account is Suspended. ";
        //            return new List<string>();
        //        }
        //        else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
        //        {
        //            ReturnStatus = "Sorry, that page does not exist :" + userID;
        //            return lstIds;
        //        }
        //        else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
        //        {
        //            ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
        //            return lstIds;
        //        }
        //        else
        //        {
        //            ReturnStatus = "Error";
        //            return lstIds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers1() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        ReturnStatus = "Error";
        //        return lstIds;
        //    }
        //} 
        #endregion
        //New method for scrping following user details on 23/9 by puja
        public List<string> GetFollowings_New(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        {
           
            Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;

            try
            {

            StartAgain:
                //Thread.Sleep(1000);
                string[] splitRes = new string[] { };
                if (counter == 0)
                {
                    string aa = "https://twitter.com/" + userID + "/following";
                    Data = HttpHelper.getHtmlfromUrl(new Uri(aa), "", "");
                    if (Data.Contains("<div class=\"stream-container"))
                    {
                        splitRes = Regex.Split(Data, "<div class=\"stream-container");
                    }
                    else
                    {
                        splitRes = Regex.Split(Data, "<div class=\"GridTimeline-items");
                    }
                    splitRes = splitRes.Skip(1).ToArray();

                    if (splitRes[0].Contains("<div class=\"stream profile-stream\">"))
                    {
                        splitRes = Regex.Split(splitRes[0], "<div class=\"stream profile-stream\">");
                        cursor = splitRes[0].Replace("\"", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("data-cursor=", string.Empty).Trim();

                    }
                    else
                    {
                        try
                        {
                            cursor = (splitRes[0].Substring(splitRes[0].IndexOf("data-cursor="), splitRes[0].IndexOf(">", splitRes[0].IndexOf("data-cursor=")) - splitRes[0].IndexOf("data-cursor=")).Replace("data-cursor=", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace(",", "").Trim());
                        }
                        catch { };
                    }
                    counter++;
                    cursor = "-1";
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");


                    //Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }
                else
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                    if (string.IsNullOrEmpty(Data))
                    {

                        for (int i = 1; i <= 3; i++)
                        {
                            Thread.Sleep(3000);
                            Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                            if (!string.IsNullOrEmpty(Data))
                            {
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(Data))
                        //{
                        //    Log(" pagesource not found ");
                        //}
                    }
                    if (Data == "Too Many Requestes")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
                            Thread.Sleep(5 * 60 * 1000);
                            for (int i = 1; i <= 3; i++)
                            {
                                Thread.Sleep(3000);
                                Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                                if (!string.IsNullOrEmpty(Data))
                                {
                                    break;
                                }
                            }
                            //if (string.IsNullOrEmpty(Data))
                            //{
                            //    Log(" pagesource not found ");
                            //}
                        }
                    }

                    var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Data);
                    cursor = string.Empty;
                    //string DataHtml = (string)avc["items_html"];
                    cursor = (string)avc["cursor"];
                }

                if (cursor == "0")
                {
                    Thread.Sleep(2000);
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following"), "", "");
                }

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return lstIds;
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    string[] arraydata;
                    string startWith = string.Empty;
                    if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                        arraydata = Regex.Split(Data, "js-stream-item stream-item stream-item");
                        startWith = "";
                    }
                    else
                    {
                        arraydata = Regex.Split(Data, "js-stream-item");
                        startWith = "\\\" role=\\\"listitem\\\"";

                    }
                    arraydata = arraydata.Skip(1).ToArray();
                    foreach (string id in arraydata)
                    {
                        if (!id.StartsWith(startWith))
                        {
                            continue;
                        }


                        string userid = string.Empty;
                        string username = string.Empty;
                        if (cursor == "0")
                        {
                            try
                            {

                                int startindex = id.IndexOf("data-user-id=");
                                string start = id.Substring(startindex).Replace("data-user-id=", string.Empty);
                                int endindex = start.IndexOf("data-feedback-token");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                userid = end;

                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                int startindex = id.IndexOf("data-item-id=\\\"");
                                string start = id.Substring(startindex).Replace("data-item-id=\\\"", string.Empty);
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                userid = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }


                        if (cursor == "0")
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=");
                                string start = id.Substring(startindex).Replace("data-screen-name=", string.Empty);
                                int endindex = start.IndexOf("data-user-id=");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                username = end;

                            }
                            catch { }
                        }
                        else
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=\\\"");
                                string start = id.Substring(startindex).Replace("data-screen-name=\\\"", "");
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                username = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }



                        if (CounterDataNo > 0)
                        {
                            if (lstIds.Count == CounterDataNo)
                            {
                                ReturnStatus = "No Error";
                                lstIds = lstIds.Distinct().ToList();
                                return lstIds;
                            }
                            else
                            {
                                Globals.lstScrapedUserIDs.Add(userid);
                                lstIds.Add(userid + ":" + username);
                                lstIds = lstIds.Distinct().ToList();

                                if (username.Contains("/span") || userid.Contains("/span"))
                                {

                                }

                                Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID  + "," + username, Globals.Path_ScrapedFollowingsList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(username, Globals.Path_ScrapedFollowingsListtxt);  //for txt format.
                            }
                        }



                        try
                        {
                            //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userid))
                            //{
                            //    string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + username + "' ,'" + userid + "') ";
                            //    DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                            //}
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }



                    if (Data.Contains("\"has_more_items\":true"))
                    {
                        int startindex = Data.IndexOf("\"cursor\":");
                        string start = Data.Substring(startindex).Replace("\"cursor\":", "");
                        int lastindex = start.IndexOf("\",\"");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf("\"}");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                lstIds = lstIds.Distinct().ToList();
                return lstIds;
            }
        }

        public List<string> GetFollowings_NewForUnfollower(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        {
            if (CounterDataNo <= 0)
            {
                CounterDataNo = TweetAccountManager.noOfUnfollows;
            }
            Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;

            try
            {

            StartAgain:
                //Thread.Sleep(1000);
                string[] splitRes = new string[] { };
                if (counter == 0)
                {
                    string aa = "https://twitter.com/" + userID + "/following";
                    Data = HttpHelper.getHtmlfromUrl(new Uri(aa), "", "");
                    if (Data.Contains("<div class=\"stream-container"))
                    {
                        splitRes = Regex.Split(Data, "<div class=\"stream-container");
                    }
                    else
                    {
                        splitRes = Regex.Split(Data, "<div class=\"GridTimeline-items");
                    }
                    splitRes = splitRes.Skip(1).ToArray();

                    if (splitRes[0].Contains("<div class=\"stream profile-stream\">"))
                    {
                        splitRes = Regex.Split(splitRes[0], "<div class=\"stream profile-stream\">");
                        cursor = splitRes[0].Replace("\"", string.Empty).Replace("\n", string.Empty).Replace(">", string.Empty).Replace("data-cursor=", string.Empty).Trim();

                    }
                    else
                    {
                        cursor = (splitRes[0].Substring(splitRes[0].IndexOf("data-cursor="), splitRes[0].IndexOf(">", splitRes[0].IndexOf("data-cursor=")) - splitRes[0].IndexOf("data-cursor=")).Replace("data-cursor=", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace(",", "").Trim());
                    }
                    counter++;
                    cursor = "-1";
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");


                    //Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers"), "", "");
                }
                else
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                    if (string.IsNullOrEmpty(Data))
                    {

                        for (int i = 1; i <= 3; i++)
                        {
                            Thread.Sleep(3000);
                            Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                            if (!string.IsNullOrEmpty(Data))
                            {
                                break;
                            }
                        }
                        //if (string.IsNullOrEmpty(Data))
                        //{
                        //    Log(" pagesource not found ");
                        //}
                    }
                    if (Data == "Too Many Requestes")
                    {
                        Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
                            Thread.Sleep(5 * 60 * 1000);
                            for (int i = 1; i <= 3; i++)
                            {
                                Thread.Sleep(3000);
                                Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
                                if (!string.IsNullOrEmpty(Data))
                                {
                                    break;
                                }
                            }
                            //if (string.IsNullOrEmpty(Data))
                            //{
                            //    Log(" pagesource not found ");
                            //}
                        }
                    }

                    var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(Data);
                    cursor = string.Empty;
                    //string DataHtml = (string)avc["items_html"];
                    cursor = (string)avc["cursor"];
                }

                if (cursor == "0")
                {
                    Thread.Sleep(2000);
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following"), "", "");
                }

                if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return lstIds;
                }
                else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {
                    string[] arraydata;
                    string startWith = string.Empty;
                    if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                        arraydata = Regex.Split(Data, "js-stream-item stream-item stream-item");
                        startWith = "";
                    }
                    else
                    {
                        arraydata = Regex.Split(Data, "js-stream-item");
                        startWith = "\\\" role=\\\"listitem\\\"";

                    }
                    arraydata = arraydata.Skip(1).ToArray();
                    foreach (string id in arraydata)
                    {
                        if (!id.StartsWith(startWith))
                        {
                            continue;
                        }


                        string userid = string.Empty;
                        string username = string.Empty;
                        if (cursor == "0")
                        {
                            try
                            {

                                int startindex = id.IndexOf("data-user-id=");
                                string start = id.Substring(startindex).Replace("data-user-id=", string.Empty);
                                int endindex = start.IndexOf("data-feedback-token");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                userid = end;

                            }
                            catch { }

                        }
                        else
                        {
                            try
                            {
                                int startindex = id.IndexOf("data-item-id=\\\"");
                                string start = id.Substring(startindex).Replace("data-item-id=\\\"", string.Empty);
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                userid = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userid + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }


                        if (cursor == "0")
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=");
                                string start = id.Substring(startindex).Replace("data-screen-name=", string.Empty);
                                int endindex = start.IndexOf("data-user-id=");
                                string end = start.Substring(0, endindex).Replace("\"", string.Empty).Trim();
                                username = end;

                            }
                            catch { }
                        }
                        else
                        {

                            try
                            {

                                int startindex = id.IndexOf("data-screen-name=\\\"");
                                string start = id.Substring(startindex).Replace("data-screen-name=\\\"", "");
                                int endindex = start.IndexOf("\\\"");
                                string end = start.Substring(0, endindex);
                                username = end;
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                            }
                        }



                        if (CounterDataNo > 0)
                        {
                            if (lstIds.Count == CounterDataNo)
                            {
                                ReturnStatus = "No Error";
                                lstIds = lstIds.Distinct().ToList();
                                return lstIds;
                            }
                            else
                            {
                                Globals.lstScrapedUserIDs.Add(userid);
                                lstIds.Add(username + ":" + userid);
                                lstIds = lstIds.Distinct().ToList();

                                if (username.Contains("/span") || userid.Contains("/span"))
                                {

                                }

                                Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowersList);
                                GlobusFileHelper.AppendStringToTextfileNewLine(username, Globals.Path_ScrapedFollowingsListtxt);
                            }
                        }



                        try
                        {
                            //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userid))
                            //{
                            //    string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + username + "' ,'" + userid + "') ";
                            //    DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                            //}
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + username + " --> database --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }



                    if (Data.Contains("\"has_more_items\":true"))
                    {
                        int startindex = Data.IndexOf("\"cursor\":");
                        string start = Data.Substring(startindex).Replace("\"cursor\":", "");
                        int lastindex = start.IndexOf("\",\"");
                        if (lastindex < 0)
                        {
                            lastindex = start.IndexOf("\"}");
                        }
                        string end = start.Substring(0, lastindex).Replace("\"", "");
                        cursor = end;
                        if (cursor != "0")
                        {
                            goto StartAgain;
                        }
                    }

                    ReturnStatus = "No Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    lstIds = lstIds.Distinct().ToList();
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                lstIds = lstIds.Distinct().ToList();
                return lstIds;
            }
        }



        //public List<string> GetFollowings_New(string userID, out string ReturnStatus, ref Globussoft.GlobusHttpHelper HttpHelper)
        //{
        //    Log("[ " + DateTime.Now + " ] => [ Searching For Following For " + userID + " ]");
        //    string cursor = "-1";
        //    string FollowingUrl = string.Empty;
        //    List<string> lstIds = new List<string>();
        //    try
        //    {
        //    StartAgain:
        //        string Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/following/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
        //        if (string.IsNullOrEmpty(Data))
        //        {

        //            for (int i = 1; i <= 3; i++)
        //            {
        //                Thread.Sleep(3000);
        //                Data = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + userID + "/followers/users?cursor=" + cursor + "&include_available_features=1&include_entities=1&is_forward=true"), "", "");
        //                if (!string.IsNullOrEmpty(Data))
        //                {
        //                    break;
        //                }
        //            }

        //        }
        //        if (Data.Contains("401 Unauthorized"))
        //        {
        //            ReturnStatus = "Account is Suspended. ";
        //            return lstIds;
        //        }
        //        else if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
        //        {
        //            //string[] arraydata = Regex.Split(Data, "js-stream-item stream-item stream-item");
        //            string[] arraydata;
        //            string startWitn = string.Empty;
        //            if (Data.Contains("js-stream-item stream-item stream-item"))
        //            {
        //                arraydata = Regex.Split(Data, "js-stream-item stream-item stream-item");
        //                startWitn = "";
        //            }
        //            else
        //            {
        //                arraydata = Regex.Split(Data, "js-stream-item");
        //                startWitn = "\\\" role=\\\"listitem\\\"";

        //            }

        //            arraydata = arraydata.Skip(1).ToArray();
        //            foreach (string id in arraydata)
        //            {
        //                string userid = string.Empty;
        //                string username = string.Empty;
        //                if (!id.StartsWith(startWitn))
        //                {
        //                    continue;
        //                }
        //                try
        //                {
        //                    int startindex = id.IndexOf("data-item-id=\\\"");
        //                    string start = id.Substring(startindex).Replace("data-item-id=\\\"", "");
        //                    int endindex = start.IndexOf("\\\"");
        //                    string end = start.Substring(0, endindex);
        //                    userid = end;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowings_New() -- " + userID + " --> userid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowings_New() -- " + userID + " --> userid --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }

        //                try
        //                {
        //                    int startindex = id.IndexOf("data-screen-name=\\\"");
        //                    string start = id.Substring(startindex).Replace("data-screen-name=\\\"", "");
        //                    int endindex = start.IndexOf("\\\"");
        //                    string end = start.Substring(0, endindex);
        //                    username = end;
        //                }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowings_New() -- " + userID + " --> username --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowings_New() -- " + userID + " --> username --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }
                      

        //                if (CounterDataNo > 0)
        //                {
        //                    if (lstIds.Count == CounterDataNo)
        //                    {
        //                        ReturnStatus = "No Error";
        //                        lstIds = lstIds.Distinct().ToList();
        //                        return lstIds;
        //                    }
        //                    else
        //                    {
        //                        lstIds.Add(userid + ":" + username);
        //                        Globals.lstScrapedUserIDs.Add(userid);
        //                        Log("[ " + DateTime.Now + " ] => [ " + username + ":" + userid + " ]");
        //                        GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowingsList);
        //                    }
        //                }

                     
        //                try
        //                {
        //                    //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userid))
        //                    //{
        //                    //    string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + username + "' ,'" + userid + "') ";
        //                    //    DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
        //                    //}
        //                }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowings_New() -- " + userID + " --> Database --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowings_New() -- " + userID + " --> DataBase --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }
        //            }

                    

        //            if (Data.Contains("\"has_more_items\":true"))
        //            {
        //                int startindex = Data.IndexOf("\"cursor\":");
        //                string start = Data.Substring(startindex).Replace("\"cursor\":", "");
        //                int lastindex = start.IndexOf("\",\"");
        //                if (lastindex < 0)
        //                {
        //                    lastindex = start.IndexOf("\"}");
        //                }
        //                string end = start.Substring(0, lastindex).Replace("\"", "");
        //                cursor = end;
        //                if (cursor != "0")
        //                {
        //                    if (NoOfFollowingsToBeunfollowed > 0)
        //                    {
        //                        lstIds = lstIds.Distinct().ToList();
        //                        //commented by prabhat
        //                        //if (NoOfFollowingsToBeunfollowed == lst_structTweetIDs.Count)
        //                        if (NoOfFollowingsToBeunfollowed == lstIds.Count)
        //                        {
        //                            ReturnStatus = "No Error";
        //                            return lstIds;
        //                        }
        //                    }
        //                    goto StartAgain;
        //                }
        //            }
        //            ReturnStatus = "No Error";
        //            lstIds = lstIds.Distinct().ToList();
        //            return lstIds;
        //        }
        //        else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
        //        {
        //            ReturnStatus = "Sorry, that page does not exist :" + userID;
        //            lstIds = lstIds.Distinct().ToList();
        //            return lstIds;
        //        }
        //        else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
        //        {
        //            ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-" + userID;
        //            lstIds = lstIds.Distinct().ToList();
        //            return lstIds;
        //        }
        //        else
        //        {
        //            ReturnStatus = "Error";
        //            lstIds = lstIds.Distinct().ToList();
        //            return lstIds;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowings_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowings_New() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        ReturnStatus = "Error";
        //        lstIds = lstIds.Distinct().ToList();
        //        return lstIds;
        //    }
        //}

        public List<string> GetHashTags(out string returnStatus)
        {
            List<string> HashTags = new List<string>();
            try
            {
                //string pagesource = globushttpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/trends/daily.json"), "", "");
                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string pagesource = HttpHelper.GetHtml("https://api.twitter.com/1/trends/daily.json");

                if (!pagesource.Contains("Rate limit exceeded"))
                {
                    string[] array = Regex.Split(pagesource, "\"name\":");
                    array = array.Skip(1).ToArray();
                    foreach (string item in array)
                    {
                        try
                        {
                            int startindex = item.IndexOf("\"");
                            string Start = item.Substring(startindex);
                            int endIndex = Start.IndexOf("\",");
                            string End = Start.Substring(0, endIndex).Replace("\"", "");

                            string HashTag = End;

                            if (HashTag.Contains("#"))
                            {
                                HashTags.Add(HashTag);
                            }
                        }
                        catch (Exception ex)
                        {
                            returnStatus = "Error";
                            return new List<string>();
                        }
                    }
                    returnStatus = "No Error";
                    return HashTags;
                }
                else
                {
                    returnStatus = "Error";
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                returnStatus = "Error";
                return new List<string>();
            }
        }

        public List<string> GetHashTags_New(out string returnStatus)
        {
            List<string> HashTags = new List<string>();
            string authenticityToken = string.Empty;
            string Woeid = string.Empty;
            List<string> lstWoeid = new List<string>();
            Dictionary<string, string> dicRemoveDuplicate = new Dictionary<string, string>();
           
            try
            {
                //string pagesource = globushttpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/trends/daily.json"), "", "");
                Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
                string twtPage = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

                try
                {
                    int startindex = twtPage.IndexOf("name=\"authenticity_token\"");
                    string start = twtPage.Substring(startindex).Replace("name=\"authenticity_token\"", "");
                    int endindex = start.IndexOf("\">");
                    string end = start.Substring(0, endindex).Replace("value=\"", "");
                    authenticityToken = end.Trim();
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }


                string pagesource = HttpHelper.postFormData(new Uri("https://twitter.com/trends/dialog"), "authenticity_token=" + authenticityToken + "&pc=true&woeid=0", "https://twitter.com/", "", "", "", "");

                string[] arrayDataWoied = Regex.Split(pagesource, "data-woeid");
                arrayDataWoied = arrayDataWoied.Skip(1).ToArray();
                foreach (string item in arrayDataWoied)
                {

                    try
                    {
                        int startindex = item.IndexOf("=\\\"");
                        string start = item.Substring(startindex).Replace("=\\\"", "");
                        int endindex = start.IndexOf("\\\"");
                        string end = start.Substring(0, endindex).Replace("value=\"", "");
                        Woeid = end;
                        lstWoeid.Add(Woeid);
                        lstWoeid = lstWoeid.Distinct().ToList();
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                foreach (string tempWoeid in lstWoeid)
                {
                    string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + tempWoeid + "&pc=true&personalized=false&src=module&woeid=" + tempWoeid + ""), "https://twitter.com/", "");
                    //string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + tempWoeid + "&pc=true&src=module"), "https://twitter.com/", "");
                    string[] datatrendname = Regex.Split(HastagString, "data-trend-name=");
                    datatrendname = datatrendname.Skip(1).ToArray();

                    foreach (string trend in datatrendname)
                    {
                        try
                        {
                            if (!trend.Contains("#\\"))
                            {
                                int startindex = trend.IndexOf("\\\"");
                                string start = trend.Substring(startindex).Replace("\\\"", "");
                                int endindex = start.IndexOf("\\");
                                string end = start.Substring(0, endindex).Replace("value=\"", "").Replace("\\\"", "");
                                if (!string.IsNullOrEmpty(end))
                                {
                                    try
                                    {
                                        dicRemoveDuplicate.Add(end, end);
                                        HashTags.Add(end);
                                        Log("[ " + DateTime.Now + " ] => [ " + end + " ]");
                                    }
                                    catch (Exception)
                                    {
                                    }
                                   
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }


                #region Old Code
                //pagesource = HttpHelper.postFormData(new Uri("https://twitter.com/trends/dialog"), "authenticity_token=" + authenticityToken + "&pc=true&woeid=" + Woeid, "https://twitter.com/", "", "", "", "");

                //string[] datatrendname = Regex.Split(pagesource, "data-woeid=");
                //datatrendname = datatrendname.Skip(2).ToArray();

                //foreach (string trend in datatrendname)
                //{
                //    try
                //    {
                //        int startindex = trend.IndexOf("\\\"");
                //        string start = trend.Substring(startindex).Replace("\\\"", "");
                //        int endindex = start.IndexOf("\\");
                //        string end = start.Substring(0, endindex).Replace("value=\"", "").Replace("\\\"", "");

                //        String datawoeid = end;

                //        //https://twitter.com/trends?k=23424848&pc=true&personalized=false&src=module&woeid=23424848

                //        string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + Woeid + "&pc=true&personalized=false&src=module&woeid=" + Woeid + ""), "https://twitter.com/", "");

                //        if (!string.IsNullOrEmpty(end))
                //        {
                //            HashTags.Add(end);
                //            Log(end);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                //    }
                //}

                #endregion

                //}
                returnStatus = "No Error";
                return HashTags;
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                returnStatus = "Error";
                return HashTags;
            }
        }

        public List<StructTweetIDs> GetTweetData_ByUserName(string keyword)
        {
            lst_structTweetIDs = new List<StructTweetIDs>();
            string user_id = string.Empty;

            try
            {
                string searchURL = string.Empty;

                if (NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&user_id =" + keyword + "&count=" + TweetExtractCount;
                }
                else
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&screen_name=" + keyword + "&count=" + TweetExtractCount;
                }
                string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");


                string[] splitRes = Regex.Split(res_Get_searchURL, "{\"created_at");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                splitRes = splitRes.Skip(1).ToArray();

                foreach (string item in splitRes)
                {
                    string modified_Item = "\"from_user\"" + item;
                    string text = string.Empty;
                    string TweeterUserId = string.Empty;
                    string TweeterUserScreanName = string.Empty;
                    string Tweetid = Globussoft.GlobusHttpHelper.ParseEncodedJson(modified_Item, "id");

                    try
                    {
                        int startIndex = item.IndexOf("\"text\":");
                        string start = item.Substring(startIndex).Replace("\"text\":", "");
                        int endIndex = start.IndexOf(",\"");
                        string end = start.Substring(0, endIndex);
                        text = end.Replace("\"", string.Empty).Trim();
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() --> text -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() --> text -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    //get tweet user ID
                    try
                    {
                        string item1 = Regex.Split(res_Get_searchURL, "user\":")[1];
                        int startIndex = item1.IndexOf("{\"id\":");
                        string start = item1.Substring(startIndex);
                        int endIndex = start.IndexOf(",\"id_str");
                        string end = start.Substring(0, endIndex);
                        TweeterUserId = end.Replace("{\"id\":", string.Empty).Trim();
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() -->  TweeterUserId -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() -->  TweeterUserId -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        //get tweet user screan Name
                        int startIndex = item.IndexOf("screen_name");
                        string start = item.Substring(startIndex);
                        int endIndex = start.IndexOf(",\"");
                        string end = start.Substring(0, endIndex);
                        TweeterUserScreanName = end.Replace("screen_name\":\"", string.Empty).Replace("\"", string.Empty).Trim();
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() --> TweeterUserScreanName -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() --> TweeterUserScreanName -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    StructTweetIDs structTweetIDs = new StructTweetIDs();

                    structTweetIDs.ID_Tweet = Tweetid;
                    structTweetIDs.ID_Tweet_User = TweeterUserId;
                    structTweetIDs.username__Tweet_User = TweeterUserScreanName;
                    structTweetIDs.wholeTweetMessage = text;

                    lst_structTweetIDs.Add(structTweetIDs);
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return lst_structTweetIDs;
        }

        public List<StructTweetIDs> TweetExtractor_ByUserName_New(string keyword)
        {
            lst_structTweetIDs = new List<StructTweetIDs>();
            string user_name = string.Empty;
            int i = 0;
            try
            {
                string HomePagedata = string.Empty;
            startAgain:
                if (i == 0)
                {
                    HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1"), "", "");
                }
                else
                {
                    if (HomePagedata.Contains("\"has_more_items\":true"))
                    {
                        HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1&max_id=" + lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet), "", "");
                    }
                    else
                    {
                        return lst_structTweetIDs;
                    }
                }

                if (!string.IsNullOrEmpty(HomePagedata))
                {
                    JObject Abc = JObject.Parse(HomePagedata);
                    string datahkj = string.Empty;
                    foreach (object data in Abc)
                    {
                        datahkj = data.ToString();
                    }

                    string[] splitRes = Regex.Split(datahkj, "ProfileTweet u-textBreak js-tweet js-stream-tweet js-actionable-tweet");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                    splitRes = splitRes.Skip(1).ToArray();

                    foreach (string item in splitRes)
                    {
                        string modified_Item = string.Empty;
                        string text = string.Empty;
                        string TweeterUserId = string.Empty;
                        string TweeterUserScreanName = string.Empty;
                        string Tweetid = string.Empty;

                        ///Tweet ID
                        try
                        {
                            int startindex = item.IndexOf("data-item-id=\\\"");
                            string start = item.Substring(startindex).Replace("data-item-id=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            Tweetid = end;
                        }
                        catch (Exception ex)
                        {

                        }


                        ///Tweet User Screen name
                        try
                        {
                            int startindex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startindex).Replace("data-screen-name=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            TweeterUserScreanName = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        ///Tweet User User-id
                        try
                        {
                            int startindex = item.IndexOf("data-user-id=\\\"");
                            string start = item.Substring(startindex).Replace("data-user-id=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            TweeterUserId = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        ///Tweet Text 
                        try
                        {
                            int startindex = item.IndexOf("\\\"js-tweet-text tweet-text\\\"");
                            string start = item.Substring(startindex).Replace("\\\"js-tweet-text tweet-text\\\"", "");
                            int endindex = start.IndexOf("</p>");
                            string end = start.Substring(0, endindex);
                            text = end.Replace("class=\\\"invisible\\\"", "").Replace("<b", "").Replace("</b", "").Replace("<s", "").Replace("</s", "").Replace("class=\\\"twitter-atreply pretty-link\\\" dir=\\\"ltr\\\"", "").Replace(">", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"invisible\\\">", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("&nbsp;", "").Replace("</a", "").Replace("</span", "").Replace("<span", "").Replace("<a href=", "").Replace("rel=nofollow dir=ltr data-expanded-url=", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=\\\"", "").Replace("class=\\\"twitter-timeline-link\\\" target=\\\"_blank\\\" title=\\\"", "");
                            text = text.Replace("\"", "").Replace("<", "").Replace("\\\"", "").Replace("\\", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = "http" + itemData;
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += data;
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        structTweetIDs.ID_Tweet = Tweetid;
                        structTweetIDs.ID_Tweet_User = TweeterUserId;
                        structTweetIDs.username__Tweet_User = TweeterUserScreanName;
                        structTweetIDs.wholeTweetMessage = text;
                        Log("[ " + DateTime.Now + " ] => [ " + Tweetid + " ]");
                        Log("[ " + DateTime.Now + " ] => [ " + TweeterUserId + " ]");
                        Log("[ " + DateTime.Now + " ] => [ " + TweeterUserScreanName + " ]");
                        Log("[ " + DateTime.Now + " ] => [ " + text + " ]");
                        Log("-----------------------------------------------------------------------------------------------------------------------------------------------------");
                        lst_structTweetIDs.Add(structTweetIDs);
                    }
                }
                i++;
                goto startAgain;
                return lst_structTweetIDs;
            }
            catch (Exception ex)
            {
                return lst_structTweetIDs;
            }
        }

        public List<StructTweetIDs> TweetExtractor_ByUserName_New_New(string keyword)
        
        {
            lst_structTweetIDs = new List<StructTweetIDs>();
            string user_name = string.Empty;
            int i = 0;
            try
            {
                string HomePagedata = string.Empty;
                //startAgain:
                //if (i == 0)
                //{
                //HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1"), "", "");
                //&composed_count=0&count=" + noOfRecords + ""

                HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1&composed_count=0&count=" + noOfRecords + ""), "", "");
                //}
                //else
                //{
                //    if (HomePagedata.Contains("\"has_more_items\":true"))
                //    {
                //        HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1&max_id=" + lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet), "", "");
                //    }
                //    else
                //    {
                //        return lst_structTweetIDs;
                //    }
                //}
                
                if (!string.IsNullOrEmpty(HomePagedata))
                {
                    JObject Abc = JObject.Parse(HomePagedata);
                    string datahkj = string.Empty;
                    foreach (object data in Abc)
                    {
                        datahkj = data.ToString();
                    }

                    //string[] splitRes = Regex.Split(datahkj, "js-stream-item stream-item stream-item expanding-stream-item");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                    string[] splitRes = Regex.Split(datahkj, "ProfileTweet u-textBreak js-tweet js-stream-tweet js-actionable-tweet");
                    splitRes = splitRes.Skip(1).ToArray();

                    foreach (string item in splitRes)
                    {
                        string modified_Item = string.Empty;
                        string text = string.Empty;
                        string TweeterUserId = string.Empty;
                        string TweeterUserScreanName = string.Empty;
                        string Tweetid = string.Empty;

                        ///Tweet ID
                        try
                        {
                            int startindex = item.IndexOf("data-item-id=\\\"");
                            string start = item.Substring(startindex).Replace("data-item-id=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            Tweetid = end;
                        }
                        catch (Exception ex)
                        {

                        }


                        ///Tweet User Screen name
                        try
                        {
                            int startindex = item.IndexOf("data-screen-name=\\\"");
                            string start = item.Substring(startindex).Replace("data-screen-name=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            TweeterUserScreanName = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        ///Tweet User User-id
                        try
                        {
                            int startindex = item.IndexOf("data-user-id=\\\"");
                            string start = item.Substring(startindex).Replace("data-user-id=\\\"", "");
                            int endindex = start.IndexOf("\\\"");
                            string end = start.Substring(0, endindex);
                            TweeterUserId = end;
                        }
                        catch (Exception ex)
                        {

                        }

                        ///Tweet Text 
                        try
                        {
                            //int startindex = item.IndexOf("\\\"js-tweet-text tweet-text\\\"");
                            //string start = item.Substring(startindex).Replace("\\\"js-tweet-text tweet-text\\\"", "");
                            //int endindex = start.IndexOf("</p>");
                            //string end = start.Substring(0, endindex);
                            //text = end.Replace("class=\\\"invisible\\\"", "").Replace("<b", "").Replace("</b", "").Replace("<s", "").Replace("</s", "").Replace("class=\\\"twitter-atreply pretty-link\\\" dir=\\\"ltr\\\"", "").Replace(">", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"invisible\\\">", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("&nbsp;", "").Replace("</a", "").Replace("</span", "").Replace("<span", "").Replace("<a href=", "").Replace("rel=nofollow dir=ltr data-expanded-url=", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=\\\"", "").Replace("class=\\\"twitter-timeline-link\\\" target=\\\"_blank\\\" title=\\\"", "");
                            //text = text.Replace("\"", "").Replace("<", "").Replace("\\\"", "").Replace("\\", "");

                            int startindex = item.IndexOf("ProfileTweet-text js-tweet-text u-dir");
                            string start = item.Substring(startindex).Replace("ProfileTweet-text js-tweet-text u-dir", "");
                            int endindex = start.IndexOf("</p>");
                            string end = start.Substring(0, endindex);
                            text = end.Replace("class=\\\"invisible\\\"", "").Replace("<b", "").Replace("</b", "").Replace("<s", "").Replace("</s", "").Replace("class=\\\"twitter-atreply pretty-link\\\" dir=\\\"ltr\\\"", "").Replace(">", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"invisible\\\">", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("&nbsp;", "").Replace("</a", "").Replace("</span", "").Replace("<span", "").Replace("<a href=", "").Replace("rel=nofollow dir=ltr data-expanded-url=", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=\\\"", "").Replace("class=\\\"twitter-timeline-link\\\" target=\\\"_blank\\\" title=\\\"", "");
                            text = text.Replace("\"", "").Replace("<", "").Replace("\\\"", "").Replace("\\", "");

                            string[] array = Regex.Split(text, "http");
                            text = string.Empty;
                            foreach (string itemData in array)
                            {
                                if (!itemData.Contains("t.co"))
                                {
                                    string data = string.Empty;
                                    if (itemData.Contains("//"))
                                    {
                                        data = "http" + itemData;
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += data;
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        StructTweetIDs structTweetIDs = new StructTweetIDs();

                        structTweetIDs.ID_Tweet = Tweetid;
                        structTweetIDs.ID_Tweet_User = TweeterUserId;
                        structTweetIDs.username__Tweet_User = TweeterUserScreanName;
                        structTweetIDs.wholeTweetMessage = text;

                        if (lst_structTweetIDs.Count < noOfRecords)
                        {
                            lst_structTweetIDs.Add(structTweetIDs);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //i++;
                //goto startAgain;
                return lst_structTweetIDs;
            }
            catch (Exception ex)
            {
                return lst_structTweetIDs;
            }
        }

        public  string countNoOfTweet(string keyword)
        {
            //GlobusHttpHelper globushttpHelper = new GlobusHttpHelper();
            GlobusRegex rgx = new GlobusRegex();
            string user_name = string.Empty;
            string PageSource = string.Empty;
            string TweetCount = string.Empty;
            PageSource = globushttpHelper.getHtmlfromUrl(new Uri("http://www.twitter.com/" + keyword), " ", " ");

            if (!PageSource.Contains("Account suspended") && !PageSource.Contains("currently suspended")  && !PageSource.Contains("account-suspended"))
            {

                try
                {
                    int indexStart = PageSource.IndexOf("data-element-term=\"tweet_stats");
                    string start = PageSource.Substring(indexStart).Replace("data-element-term=\"tweet_stats", "");
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
                    TweetCount = rgx.StripTagsRegex(end).Replace("data-nav=\"tweet\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                }
                catch { }
                if (string.IsNullOrEmpty(TweetCount))
                {
                    try
                    {
                        int indexStart = PageSource.IndexOf("statuses_count");
                        string start = PageSource.Substring(indexStart).Replace("statuses_count", "");
                        int indexEnd = start.IndexOf(",");
                        string end = start.Substring(0, indexEnd).Replace("&quot", "").Replace(":", "").Replace(",", "").Replace(";", "").Replace("/","").Replace("\"","").Trim();
                        
                        TweetCount = rgx.StripTagsRegex(end).Replace("data-nav=\"tweet\"", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace(",", string.Empty).Trim();

                    }

                    catch { }
                }
               
            }
            return TweetCount;
        }

        /// <summary> Get User details for While/blacklist User 
        /// Get User details for While/blacklist User 
        /// </summary>
        /// <param name="username">User Screan Name OR User ID</param>
        /// <returns>Details of Users</returns>
        /// 
        #region Get User details for While/blacklist User
        //public static Dictionary<string, string> GetUserDetails(string username, out string status)
        //{
        ////    ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();

        //    Dictionary<string, string> dataLst = new Dictionary<string, string>();

        ////    try
        ////    {
        ////        string PageSource = null;

        ////        if (!string.IsNullOrEmpty(username) && NumberHelper.ValidateNumber(username))
        ////        {
        ////            //PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.json?user_id=" + username + "&include_entities=true");
        ////        }
        ////        else
        ////        {
        ////            //PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.json?screen_name=" + username + "&include_entities=true");

        ////            string url = string.Format("https://twitter.com/" + username);
        ////            PageSource = httpHelper.GetHtml(url);
        ////        }



        ////        if (PageSource.Contains("error\":\"User has been suspended"))
        ////        {
        ////            status = "User has been suspended";
        ////            return dataLst;
        ////        }
        ////        else if (PageSource.Contains("message\":\"Sorry, that page does not exist"))
        ////        {
        ////            status = "Sorry, that page does not exist";
        ////            return dataLst;
        ////        }
        ////        else if (PageSource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
        ////        {
        ////            status = "Rate limit exceeded. Clients may not make more than 150 requests per hour.";
        ////            return dataLst;
        ////        }


        ////        if (!string.IsNullOrEmpty(PageSource))
        ////        {
        ////            string id = string.Empty;
        ////            string name = string.Empty;
        ////            string screen_name = string.Empty;
        ////            string location = string.Empty;
        ////            string NoOfTweet = string.Empty;
        ////            string followers_count = string.Empty;
        ////            string friends_count = string.Empty;
        ////            string profile_image_url = string.Empty;


        ////            if (PageSource.Contains("data-user-id="))
        ////            {
        ////                String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(PageSource, "data-user-id=");

        ////                if (getDataInArr.Count() > 0)
        ////                {
        ////                    foreach (var item in getDataInArr)
        ////                    {
        ////                        int startindex = item.IndexOf("<strong>");
        ////                        int endindex = item.IndexOf("</strong>"); ;
        ////                        NoOfTweet = item.Substring(startindex, endindex - startindex).Replace("<strong>", string.Empty).Replace(",", string.Empty);
        ////                    }
        ////                }

        ////            }

        ////            if (PageSource.Contains("tweet_stats"))
        ////            {
        ////                String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(PageSource, "tweet_stats");

        ////                if (getDataInArr.Count() > 0)
        ////                {
        ////                    int startindex = getDataInArr[1].IndexOf("<strong>");
        ////                    int endindex = getDataInArr[1].IndexOf("</strong>"); ;
        ////                    NoOfTweet = getDataInArr[1].Substring(startindex, endindex - startindex).Replace("<strong>", string.Empty).Replace(",", string.Empty);
        ////                }

        ////            }

        ////            if (PageSource.Contains("following_stats"))
        ////            {
        ////                String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(PageSource, "following_stats");

        ////                if (getDataInArr.Count() > 0)
        ////                {
        ////                    int startindex = getDataInArr[1].IndexOf("<strong>");
        ////                    int endindex = getDataInArr[1].IndexOf("</strong>"); ;
        ////                    friends_count = getDataInArr[1].Substring(startindex, endindex - startindex).Replace("<strong>", string.Empty).Replace(",", string.Empty);
        ////                }
        ////            }

        ////            if (PageSource.Contains("follower_stats"))
        ////            {
        ////                String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(PageSource, "follower_stats");

        ////                if (getDataInArr.Count() > 0)
        ////                {
        ////                    int startindex = getDataInArr[1].IndexOf("<strong>");
        ////                    int endindex = getDataInArr[1].IndexOf("</strong>");
        ////                    followers_count = getDataInArr[1].Substring(startindex, endindex - startindex).Replace("<strong>", string.Empty).Replace(",", string.Empty);
        ////                }
        ////            }

        ////            if (PageSource.Contains("data-url"))
        ////            {
        ////                String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(PageSource, "data-url");

        ////                if (getDataInArr.Count() > 0)
        ////                {
        ////                    int startindex = getDataInArr[1].IndexOf("=\"");
        ////                    int endindex = getDataInArr[1].IndexOf(">"); ;
        ////                    profile_image_url = getDataInArr[1].Substring(startindex, endindex - startindex).Replace("target=\"_blank", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
        ////                }
        ////            }

        ////            //string id = jobj["id"].ToString().Replace("\"", string.Empty);
        ////            dataLst.Add("id", id);
        ////            //string name = jobj["name"].ToString().Replace("\"", string.Empty);
        ////            dataLst.Add("name", name);
        ////            //string screen_name = jobj["screen_name"].ToString().Replace("\"", string.Empty);
        ////            dataLst.Add("screen_name", screen_name);
        ////            //string location = jobj["location"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("location", location);
        ////            //string description = jobj["description"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("description", description);
        ////            //string followers_count = jobj["followers_count"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("followers_count", followers_count);
        ////            //string friends_count = jobj["friends_count"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("friends_count", friends_count);
        ////            //string statuses_count = jobj["statuses_count"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("statuses_count", statuses_count);
        ////            //string created_at = jobj["created_at"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("created_at", created_at);
        ////            //string time_zone = jobj["time_zone"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("time_zone", time_zone);
        ////            //string profile_image_url = jobj["profile_image_url"].ToString().Replace("\"", string.Empty);
        ////            //dataLst.Add("profile_image_url", profile_image_url);
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        ////        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        ////    }

        ////    //Assign Status 
        //    status = "Ok";
        //    return dataLst;
        //}


        public static Dictionary<string, string> GetUserDetails(string username, out string status)
        {
            TwitterDataScrapper twiterDataScrapper = new TwitterDataScrapper();
            Dictionary<string, string> dataLst = new Dictionary<string, string>();
           

            string id = string.Empty;
            string name = string.Empty;
            string screen_name = string.Empty;
            string location = string.Empty;
            string description = string.Empty;
            string NoOfTweet = string.Empty;
            string followers_count = string.Empty;
            string following_count = string.Empty;
            string profile_image_url = string.Empty;

            //string ProfileName = string.Empty;
            //string Location = string.Empty;
            //string Bio = string.Empty;
            string website = string.Empty;
            //string NoOfTweets = string.Empty;
            //string Followers = string.Empty;
            //string Followings = string.Empty;
            //try
            //{

            ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + username), "", "");

            string Responce = ProfilePageSource;

            #region Convert HTML to XML

            string xHtml = objChilkat.ConvertHtmlToXml(Responce);
            Chilkat.Xml xml = new Chilkat.Xml();
            xml.LoadXml(xHtml);

            Chilkat.Xml xNode = default(Chilkat.Xml);
            Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
            #endregion


            //Get User Id From page 
            if (!(ProfilePageSource == null))
            {
                if (ProfilePageSource.Contains("data-user-id="))
                {
                    try
                    {
                        String[] getDataInArr = System.Text.RegularExpressions.Regex.Split(ProfilePageSource, "data-user-id=");

                        if (getDataInArr.Count() > 0)
                        {
                            foreach (var item in getDataInArr)
                            {
                                if (item.Contains("<h1"))
                                
                                {
                                    int startindex = (1);
                                    int endindex = item.IndexOf(">") - 1; ;
                                    id = item.Substring(startindex, endindex - startindex).Replace("<strong>", string.Empty).Replace(",", string.Empty);
                                    if (id.Contains("\n"))
                                    {
                                        string[] arr =System.Text.RegularExpressions.Regex.Split(id,"\""); 
                                        id=arr[0];
                                    }
                                    dataLst.Add("id", id);
                                    break;
                                }
                            }
                            //whiteListedUserCount++;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            //else
            //{
            //    blackListedUserCount++;
            //}

            int counterdata = 0;
            xBeginSearchAfter = null;
            string dataDescription = string.Empty;
            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNameTruncated-link u-textInheritColor js-nav js-action-profile-name");//"profile-field");
            while ((xNode != null))
            {
                try
                {
                    string str = xNode.GetXml();
                    xBeginSearchAfter = xNode;
                    if (counterdata == 1)
                    {
                        website = xNode.AccumulateTagContent("text", "script|style");
                        counterdata++;
                    }
                    else if (counterdata == 0)
                    {
                        name = xNode.AccumulateTagContent("text", "script|style");
                        dataLst.Add("name", name);
                        counterdata++;
                    }
                    else
                    {
                        break;
                    }
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNameTruncated-link u-textInheritColor js-nav js-action-profile-name");//"profile-field");
                }
                catch (Exception)
                {
                }
            }

            //Get Screen name and ID 

            xBeginSearchAfter = null;
            //string dataDescription = string.Empty;
            xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "u-linkComplex-target");
            while ((xNode != null))
            {
                string str = xNode.GetXml();
                xBeginSearchAfter = xNode;
                screen_name = xNode.AccumulateTagContent("text", "script|style").Replace("@", string.Empty);
                dataLst.Add("screen_name", screen_name);
                break;
            }

            xBeginSearchAfter = null;
            xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//"location profile-field");
            while ((xNode != null))
            {
                xBeginSearchAfter = xNode;
                location = xNode.AccumulateTagContent("text", "script|style");
                dataLst.Add("location", location);
                break;
            }

            xBeginSearchAfter = null;
            dataDescription = string.Empty;
            xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "ProfileHeaderCard-bio u-dir");
            while ((xNode != null))
            {
                xBeginSearchAfter = xNode;
                description = xNode.AccumulateTagContent("text", "script|style");
                dataLst.Add("description", description);
                break;
            }

            int counterData = 0;
            xBeginSearchAfter = null;
             xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");
             xBeginSearchAfter = xNode;
                if (counterData == 0)
                {
                    NoOfTweet = xNode.AccumulateTagContent("text", "script|style").Replace("Tweets", string.Empty).Replace(",", string.Empty).Replace("Tweet",string.Empty);
                    dataLst.Add("statuses_count", NoOfTweet);
                    counterData++;
                }
            //dataDescription = string.Empty;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-openSignupDialog js-nonNavigable u-textUserColor");
                xBeginSearchAfter = xNode;
                if (counterData == 1)
                {
                    following_count = xNode.AccumulateTagContent("text", "script|style").Replace(" Following", string.Empty).Replace(",", string.Empty).Replace("Following", string.Empty);
                    dataLst.Add("friends_count", following_count);
                    counterData++;
                }
                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-openSignupDialog js-nonNavigable u-textUserColor");
            //while ((xNode != null))
            //{
            //    //xBeginSearchAfter = xNode;
                //if (counterData == 0)
                //{
                //    NoOfTweet = xNode.AccumulateTagContent("text", "script|style").Replace("Tweets", string.Empty).Replace(",", string.Empty).Replace("Tweet",string.Empty);
                //    dataLst.Add("statuses_count", NoOfTweet);
                //    counterData++;
                //}
                xBeginSearchAfter = xNode;
                 if (counterData == 2)
                {
                    followers_count = xNode.AccumulateTagContent("text", "script|style").Replace("Followers", string.Empty).Replace(",", string.Empty).Replace("Follower", string.Empty);
                    dataLst.Add("followers_count", followers_count);
                    counterData++;
                }
                
                    
                //else
                //{
                //    break;
                //}
                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");//"js-nav");
           // }
            status = "Ok";
            //}
            //catch (Exception)
            //{
            //    status = "";
            //}
            return dataLst;
        }

        #endregion

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            //logEvents_static.LogText(eventArgs);
            logEvents.LogText(eventArgs);
        }

    }
}
