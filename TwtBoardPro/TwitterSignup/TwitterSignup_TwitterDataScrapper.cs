using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Globussoft;

namespace TwitterSignup
{
    public class TwitterSignup_TwitterDataScrapper
    {
        public struct StructTweetIDs
        {
            public string ID_Tweet { get; set; }

            public string ID_Tweet_User { get; set; }

            public string username__Tweet_User { get; set; }

            public string wholeTweetMessage { get; set; }
        }

        public List<StructTweetIDs> lst_structTweetIDs { get; set; }

        public int noOfRecords = 20;

        public static int noOfstatus = 20;

        public static int Percentage = 40;

        Globussoft.GlobusHttpHelper globushttpHelper = new Globussoft.GlobusHttpHelper();

        public static int TweetExtractCount = 10;

        public static int RetweetExtractcount = 10;

        public static string GetUserIDFromUsername(string username, out string Status)
        {
            string GetStatus = string.Empty;
            Globussoft.GlobusHttpHelper httpHelper = new Globussoft.GlobusHttpHelper();

            clsDBQueryManager DB = new clsDBQueryManager();
            DataSet ds = DB.GetUserIdForuser_follower_details(username);
            string user_id = string.Empty;

            if (ds.Tables[0].Rows.Count > 0)
            {
                //foreach (DataRow dataRow in ds.Tables["tb_UsernameDetails"].Rows)
                foreach (DataRow dataRow in ds.Tables["tb_user_follower_details"].Rows)
                {
                    user_id = dataRow.ItemArray[0].ToString();
                    Status = "No Error";
                    return user_id;
                }
            }


            try
            {
                #region gs httpHelper code
                //if (username.Contains("@"))
                //{

                //}
                //string pagesource = httpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/users/show.xml?screen_name=" + username), "api.twitter.com", ""); 
                #endregion
                string id = string.Empty;

                ChilkatHttpHelpr httpHelper1 = new ChilkatHttpHelpr();
                string pagesource = httpHelper1.GetHtml("https://api.twitter.com/1/users/show.xml?screen_name=" + username);

                if (!pagesource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour.") && !pagesource.Contains("Sorry, that page does not exist") && !pagesource.Contains("User has been suspended"))
                {
                    int length = pagesource.IndexOf("</id>");
                    id = pagesource.Substring(pagesource.IndexOf("<id>"), length - pagesource.IndexOf("<id>")).Replace("<id>", "");
                    user_id = id;
                    GetStatus = "No Error";
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

        public static string GetUserNameFromUserId(string userid)
        {
            string username = string.Empty;

            ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();

            try
            {
                string PageSource = string.Empty;
                if (!string.IsNullOrEmpty(userid) && NumberHelper.ValidateNumber(userid))
                {
                    //PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.xml?user_id=" + userid + "&include_entities=true");
                    PageSource = httpHelper.GetHtml(("https://twitter.com/account/redirect_by_id?id=" + userid));
                    if (!PageSource.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                    {
                        try
                        {
                            //int startIndex = PageSource.IndexOf("<screen_name>");
                            //if (startIndex > 0)
                            {
                                //string Start = PageSource.Substring(startIndex);
                                //int endIndex = Start.IndexOf("</screen_name>");
                                //string End = Start.Substring(0, endIndex);
                                //username = End.Replace("<screen_name>", "");

                                int startindex = PageSource.IndexOf("user-style-");
                                string start = PageSource.Substring(startindex).Replace("user-style-", "");
                                int endindex = start.IndexOf("\"");
                                string end = start.Substring(0, endindex);
                                username = end;
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
                    int startindex = pagesource.IndexOf("stats js-mini-profile-stats\" data-user-id=\"");
                    string start = pagesource.Substring(startindex).Replace("stats js-mini-profile-stats\" data-user-id=\"", "");
                    int endindex = start.IndexOf("\",");
                    string end = start.Substring(0, endindex);
                    user_id = end;
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

        public static string GetUserIdToUserName_New(string UserId, out string Status, ref GlobusHttpHelper HttpHelper)
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
                    int endindex = start.IndexOf(">");
                    string end = start.Substring(0, endindex);
                    _UserName = end.Replace("<s>", string.Empty).Replace("</s>", string.Empty).Replace("</span>", string.Empty).Replace("&quot;", string.Empty).Replace(":", string.Empty);
                    _UserName = _UserName.Split(',')[0];
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

        public List<StructTweetIDs> KeywordStructData(string keyword)
        {
            try
            {
                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;


                string searchURL = "https://twitter.com/phoenix_search.phoenix?q=" + keyword + "&count=" + noOfRecords + "&include_entities=1&include_available_features=1&contributor_details=true&page=null&mode=relevance&query_source=typed_query";


                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
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

                    string[] splitRes = Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");//Regex.Split(res_Get_searchURL, "{\"created_at\"");

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
                            int startIndex = item.IndexOf("\"id\"");
                            string start = item.Substring(startIndex);
                            int endIndex = start.IndexOf(",\"");
                            string end = start.Substring(0, endIndex).Replace("id", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
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
                            int startindex = item.IndexOf("\"screen_name\"");
                            string start = item.Substring(startindex);
                            int endIndex = start.IndexOf(",\"");
                            string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                            from_user = end;
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
                            string end = start.Substring(0, endIndex).Replace("screen_name", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "");
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

        public List<StructTweetIDs> GetTweetData(string keyword)
        {
            lst_structTweetIDs = new List<StructTweetIDs>();

            try
            {
                string searchURL = "http://search.twitter.com/search.json?q=" + keyword + "&rpp=100&include_entities=true&result_type=recent";
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

        public List<string> GetTweetData_Scrape(string keyword, out string returnStatus)
        {
            string status = string.Empty;
            List<string> lst_TweetIDs = new List<string>();
            try
            {
                string user_id = string.Empty;

                #region old code
                //if (!NumberHelper.ValidateNumber(keyword))
                //{
                //    user_id = GetUserIDFromUsername(keyword);
                //}
                //else
                //{
                //    user_id = keyword;
                //}

                //if (!string.IsNullOrEmpty(user_id))
                //{
                //screen_name  
                #endregion

                string searchURL = string.Empty;
                if (NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&id=" + keyword + "&count=" + TweetExtractCount;
                }
                else
                {
                    searchURL = "https://api.twitter.com/1/statuses/user_timeline.json?include_entities=true&include_rts=true&screen_name=" + keyword + "&count=" + TweetExtractCount;
                }
                //string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string res_Get_searchURL = HttpHelper.GetHtml(searchURL);

                if (!res_Get_searchURL.Contains("Rate limit exceeded") && !res_Get_searchURL.Contains("Sorry, that page does not exist") && !res_Get_searchURL.Contains("Not authorized") && res_Get_searchURL.Contains("created_at") && !string.IsNullOrEmpty(res_Get_searchURL))
                {
                    string[] splitRes = Regex.Split(res_Get_searchURL, "{\"created_at");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                    splitRes = splitRes.Skip(1).ToArray();

                    foreach (string item in splitRes)
                    {
                        //string text = Globussoft.GlobusHttpHelper.ParseJson(modified_Item, "<text>");
                        string Tweet = Globussoft.GlobusHttpHelper.ParseJson(item, "text");//Globussoft.GlobusHttpHelper.parseText(item);
                        string data = keyword + ":" + Tweet;
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_TweetExtractor);
                        lst_TweetIDs.Add(data);
                    }
                    status = "No Error";
                }
                else if (res_Get_searchURL.Contains("Rate limit exceeded"))
                {
                    status = "Rate limit exceeded";
                }
                else if (res_Get_searchURL.Contains("Sorry, that page does not exist"))
                {
                    status = "Sorry, that page does not exist";
                }
                else if (res_Get_searchURL.Contains("Not authorized"))
                {
                    status = "Not Authorized";
                }
                else if (string.IsNullOrEmpty(res_Get_searchURL))
                {
                    status = "Not Authorized";
                }
                else
                {
                    status = "Empty";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                status = "Error";
            }
            returnStatus = status;
            return lst_TweetIDs;
        }

        public List<string> GetRetweetData_Scrape(string keyword, out string returnStatus)
        {
            string status = string.Empty;
            List<string> lst_ReTweetIDs = new List<string>();
            try
            {
                string searchURL = string.Empty;

                if (!NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://api.twitter.com/1/statuses/retweeted_by_user.xml?screen_name=" + keyword + "&count=" + RetweetExtractcount + "&include_entities=true";
                }
                else if (NumberHelper.ValidateNumber(keyword))
                {
                    searchURL = "https://api.twitter.com/1/statuses/retweeted_by_user.xml?id=" + keyword + "&count=" + RetweetExtractcount + "&include_entities=true";
                }

                //string res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string res_Get_searchURL = HttpHelper.GetHtml(searchURL);

                if (!res_Get_searchURL.Contains("Rate limit exceeded") && !res_Get_searchURL.Contains("Sorry, that page does not exist") && res_Get_searchURL.Contains("created_at") && !string.IsNullOrEmpty(res_Get_searchURL))
                {
                    string[] splitRes = Regex.Split(res_Get_searchURL, "<status>");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                    splitRes = splitRes.Skip(1).ToArray();

                    foreach (string item in splitRes)
                    {
                        string Tweet = string.Empty;
                        string Tweeter = string.Empty;
                        try
                        {
                            int startIndex = item.IndexOf("<text>");
                            string start = item.Substring(startIndex);
                            int endIndex = start.IndexOf("</text>");
                            string end = start.Substring(0, endIndex);
                            Tweet = end.Replace("<text>", "");

                            int startOfInndex = Tweet.IndexOf(":");
                            Tweeter = Tweet.Substring(0, startOfInndex);
                            Tweet = Tweet.Replace(Tweeter, "");
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }

                        string data = keyword + ":" + Tweeter + ":" + Tweet.Replace(":", "^");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_RETweetExtractor);
                        lst_ReTweetIDs.Add(data);
                    }
                }
                else if (res_Get_searchURL.Contains("Rate limit exceeded"))
                {
                    status = "Rate limit exceeded";
                }
                else if (res_Get_searchURL.Contains("Sorry, that page does not exist"))
                {
                    status = "Sorry, that page does not exist";
                }
                else if (res_Get_searchURL.Contains("Not authorized"))
                {
                    status = "Not Authorized";
                }
                else if (string.IsNullOrEmpty(res_Get_searchURL))
                {
                    status = "Not Authorized";
                }
                else
                {
                    status = "Empty";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetRetweetData_Scrape() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                status = "Error";
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
            try
            {
                string FollowingUrl = string.Empty;
                if (NumberHelper.ValidateNumber(userID))
                {
                    FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=-1&id=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                }
                else
                {
                    FollowingUrl = "https://api.twitter.com/1/followers/ids.json?cursor=-1&screen_name=" + userID + "";//"https://api.twitter.com/1/friends/ids.json?cursor=-1&screen_name=SocioPro";
                }

                #region gs http helper code
                //https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro
                //string Data = globushttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                #endregion

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string Data = HttpHelper.GetHtml(FollowingUrl);

                if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    int FirstPoint = Data.IndexOf("[");
                    int SecondPoint = Data.IndexOf("]");

                    string FollowingIds = Data.Substring(FirstPoint, SecondPoint - FirstPoint).Replace("[", string.Empty).Replace("]", string.Empty);

                    List<string> lstIds = FollowingIds.Split(',').ToList();

                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :" + userID;
                    return new List<string>();
                }
                else
                {
                    ReturnStatus = "Error";
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetFollowers() -- " + userID + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetFollowers() -- " + userID + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                ReturnStatus = "Error";
                return new List<string>();
            }
        }

        public List<string> GetFollowings(string userID, out string ReturnStatus)
        {
            try
            {
                string FollowingUrl = string.Empty;
                if (NumberHelper.ValidateNumber(userID))
                {
                    FollowingUrl = "https://api.twitter.com/1/following/ids.json?cursor=-1&id=" + userID + "";//"https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro";
                }
                else
                {
                    FollowingUrl = "https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=" + userID + "";//"https://api.twitter.com/1/following/ids.json?cursor=-1&screen_name=SocioPro";
                }

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                string Data = HttpHelper.GetHtml(FollowingUrl);//.getHtmlfromUrl(new Uri(FollowingUrl), "", "");

                if (!Data.Contains("Rate limit exceeded"))
                {
                    int FirstPoint = Data.IndexOf("[");
                    int SecondPoint = Data.IndexOf("]");

                    string FollowingIds = Data.Substring(FirstPoint, SecondPoint - FirstPoint).Replace("[", string.Empty).Replace("]", string.Empty);

                    List<string> lstIds = FollowingIds.Split(',').ToList();

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



        /// <summary> Get User details for While/blacklist User 
        /// Get User details for While/blacklist User 
        /// </summary>
        /// <param name="username">User Screan Name OR User ID</param>
        /// <returns>Details of Users</returns>

        #region Get User details for While/blacklist User
        public static Dictionary<string, string> GetUserDetails(string username)
        {
            ChilkatHttpHelpr httpHelper = new ChilkatHttpHelpr();

            Dictionary<string, string> dataLst = new Dictionary<string, string>();

            try
            {
                string PageSource = null;

                if (!string.IsNullOrEmpty(username) && NumberHelper.ValidateNumber(username))
                {
                    PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.json?user_id=" + username + "&include_entities=true");
                }
                else
                {
                    PageSource = httpHelper.GetHtml("https://api.twitter.com/1/users/show.json?screen_name=" + username + "&include_entities=true");
                }

                JObject jobj = JObject.Parse(PageSource);


                string id = jobj["id"].ToString().Replace("\"", string.Empty);
                dataLst.Add("id", id);
                string name = jobj["name"].ToString().Replace("\"", string.Empty);
                dataLst.Add("name", name);
                string screen_name = jobj["screen_name"].ToString().Replace("\"", string.Empty);
                dataLst.Add("screen_name", screen_name);
                string location = jobj["location"].ToString().Replace("\"", string.Empty);
                dataLst.Add("location", location);
                string description = jobj["description"].ToString().Replace("\"", string.Empty);
                dataLst.Add("description", description);
                string followers_count = jobj["followers_count"].ToString().Replace("\"", string.Empty);
                dataLst.Add("followers_count", followers_count);
                string friends_count = jobj["friends_count"].ToString().Replace("\"", string.Empty);
                dataLst.Add("friends_count", friends_count);
                string statuses_count = jobj["statuses_count"].ToString().Replace("\"", string.Empty);
                dataLst.Add("statuses_count", statuses_count);
                string created_at = jobj["created_at"].ToString().Replace("\"", string.Empty);
                dataLst.Add("created_at", created_at);
                string time_zone = jobj["time_zone"].ToString().Replace("\"", string.Empty);
                dataLst.Add("time_zone", time_zone);
                string profile_image_url = jobj["profile_image_url"].ToString().Replace("\"", string.Empty);
                dataLst.Add("profile_image_url", profile_image_url);
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + username + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }

            return dataLst;
        }

        #endregion


    }
}
