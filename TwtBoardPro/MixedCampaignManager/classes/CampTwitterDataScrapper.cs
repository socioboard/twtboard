using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Text.RegularExpressions;
using System.Data;
using Globussoft;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace MixedCampaignManager.classes
{
    public class CampTwitterDataScrapper
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
        
        public static int _noOfRecords = 20;

        public static int noOfstatus = 20;

        public static int Percentage = 40;

        Globussoft.GlobusHttpHelper globushttpHelper = new Globussoft.GlobusHttpHelper();
        public static int TweetExtractCount = 10;

        public static int RetweetExtractcount = 10;

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
                if (splitRes.Count() == 1)
                {
                    splitRes = Regex.Split(res_Get_searchURL, "ProfileTweet-actionButton u-textUserColorHover dropdown-toggle js-tooltip js-dropdown-toggle");
                }
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
                        text = end.Replace("\"", string.Empty).Replace("&#39;", "'").Trim();
                    }
                    catch (Exception ex)
                    {
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() --> text -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() --> text -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() -->  TweeterUserId -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() -->  TweeterUserId -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        //get tweet user screan Name
                        int startIndex = item.IndexOf("screen_name");
                        string start = item.Substring(startIndex);
                        int endIndex = start.IndexOf(",\"");
                        string end = start.Substring(0, endIndex);
                        TweeterUserScreanName = end.Replace("screen_name\":\"", string.Empty).Replace("\"", string.Empty).Replace("&#39;", "'").Trim();
                    }
                    catch (Exception ex)
                    {
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() --> TweeterUserScreanName -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() --> TweeterUserScreanName -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetTweetData_ByUserName() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetTweetData_ByUserName() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
                //startAgain:
                //if (i == 0)
                {
                //HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1"), "", "");
                //&composed_count=0&count=" + noOfRecords + ""

                //HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + keyword + "/timeline?include_available_features=1&include_entities=1&composed_count=0&count=" + noOfRecords + ""), "", "");
                    HomePagedata = globushttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + Uri.EscapeDataString(keyword) + "/timeline/with_replies?composed_count=0&count=" + noOfRecords + "&include_available_features=1&include_entities=1"),"","");
                }
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

                    string[] splitRes = Regex.Split(datahkj, "ProfileTweet u-textBreak js-tweet js-stream-tweet js-actionable-tweet");//Regex.Split(res_Get_searchURL, "{\"created_at\"");
                    if (splitRes.Count() == 1)
                    {
                        splitRes = Regex.Split(datahkj, "ProfileTweet-actionButton u-textUserColorHover dropdown-toggle js-tooltip js-dropdown-toggle");
                        if (splitRes.Count() == 1)
                        {
                            splitRes = Regex.Split(datahkj, "js-stream-item stream-item stream-item expanding-stream-item");
                        }
                    }
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
                            #region code commented by PUJA
                            //int startindex = item.IndexOf("ProfileTweet-text js-tweet-text u-dir");
                            //string start = item.Substring(startindex).Replace("ProfileTweet-text js-tweet-text u-dir", "");
                            //int endindex = start.IndexOf("</p>");
                            //string end = start.Substring(0, endindex);
                            //text = end.Replace("class=\\\"invisible\\\"", "").Replace("<b", "").Replace("</b", "").Replace("<s", "").Replace("</s", "").Replace("class=\\\"twitter-atreply pretty-link\\\" dir=\\\"ltr\\\"", "").Replace(">", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"invisible\\\">", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("&nbsp;", "").Replace("</a", "").Replace("</span", "").Replace("<span", "").Replace("<a href=", "").Replace("rel=nofollow dir=ltr data-expanded-url=", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=\\\"", "").Replace("class=\\\"twitter-timeline-link\\\" target=\\\"_blank\\\" title=\\\"", "").Replace("class=\\\"twitter-timeline-link u-isHiddenVisually\\\" data-pre-embedded=\\\"true\\\" dir=\\\"ltr\\\"","").Trim();
                            //text = text.Replace("<", "").Replace("\\\"", "").Replace("\\n","").Replace("\"","").Trim();

                            //text = text.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                            //text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "").Replace("&amp;", "&").Replace("&#39;", "'").Replace("&lt;", "<").Replace("&gt;", ">").Replace("\n", string.Empty).Replace("..", string.Empty).Replace("\n \"", string.Empty).Replace("\\n", string.Empty).Replace("\\", string.Empty).Replace("js-tweet-text tweet-text", string.Empty).Replace("#", string.Empty).Replace("dir=ltr", "").Trim();

                            //text = text.Replace("\"", "").Replace("<", "").Replace("\\\"", "").Replace("\\", "");

                            //string[] array = Regex.Split(text, "http");
                            //text = string.Empty;
                            //foreach (string itemData in array)
                            //{
                            //    if (!itemData.Contains("t.co"))
                            //    {
                            //        string data = string.Empty;
                            //        if (itemData.Contains("//"))
                            //        {
                            //            data = "http" + itemData;
                            //            if (!text.Contains(itemData.Replace(" ", "")))
                            //            {
                            //                text += data;
                            //            }
                            //        }
                            //        else
                            //        {
                            //            if (!text.Contains(itemData.Replace(" ", "")))
                            //            {
                            //                text += itemData;
                            //            }
                            //        }
                            //    }
                            //    if (text.Contains("data-aria-label-part=0"))
                            //    {
                            //        text = globushttpHelper.getBetween(text + ":&$#@", "data-aria-label-part=0", ":&$#@");
                            //    }
                            //} 
                            #endregion
                GlobusRegex regx = new GlobusRegex();
               // foreach (string item1 in splitRes)
                
                        string tweetUserid = string.Empty;                                      
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

                            if (text.Contains("data-aria-label-part=0"))
                            {
                                text = globushttpHelper.getBetween(text + ":&$#@", "data-aria-label-part=0", ":&$#@");
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

                        //lst_structTweetIDs.Add(structTweetIDs);
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
                    searchURL = "http://search.twitter.com/search.json?q=" + keyword + "&rpp=100&include_entities=true&result_type=recent";
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

        //public List<StructTweetIDs> GetTweetData_New(string keyword)
        //{
        //    try
        //    {

        //        int counter = 0;
        //        lst_structTweetIDs = new List<StructTweetIDs>();
        //        string res_Get_searchURL = string.Empty;
        //        string searchURL = string.Empty;
        //        string maxid = string.Empty;
        //        if (keyword.Trim().Contains(" "))
        //        {
        //            keyword = keyword.Replace(" ", "+");
        //        }
        //            searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";

        //        try
        //        {
        //            res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

        //            if (string.IsNullOrEmpty(res_Get_searchURL))
        //            {
        //                res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Threading.Thread.Sleep(2000);
        //            res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --  res_Get_searchURL --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " -- res_Get_searchURL --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        }
                
        //        if (!string.IsNullOrEmpty(res_Get_searchURL))
        //        {
        //            object DEserizedData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
        //            string DataHtml = (string)((JObject)DEserizedData)["items_html"];
        //            string[] splitRes = Regex.Split(DataHtml, "data-item-id"); 

        //            splitRes = splitRes.Skip(1).ToArray();

        //            GlobusRegex regx = new GlobusRegex();
        //            foreach (string item in splitRes)
        //            {
        //                if (item.Contains("data-screen-name=") && !item.Contains("follow-button") && !item.Contains("Following"))
        //                {
                           
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
        //                    string start = item.Substring(startIndex).Replace("data-user-id=", "");
        //                    int endIndex = start.IndexOf("data-is-reply-to");
        //                    if (endIndex==-1)
        //                    {
        //                         endIndex = start.IndexOf("data-expanded-footer=");
        //                    }
        //                    if (endIndex == -1)
        //                    {
        //                        endIndex = start.IndexOf(">");
        //                    }
        //                    string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "").Replace("\n", string.Empty);
        //                    if (end.Contains(" "))
        //                    {
        //                        end = end.Split(' ')[0];
        //                    }
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
        //                    int startIndex = item.IndexOf("data-screen-name=");
        //                    string start = item.Substring(startIndex).Replace("data-screen-name=", "");
        //                    int endIndex = start.IndexOf("data-name");
        //                    if (endIndex>100)
        //                    {
        //                        endIndex = 0;
        //                        endIndex = start.IndexOf("data-user");
        //                    }
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
        //                    int startIndex = item.IndexOf("=\"");
        //                    string start = item.Substring(startIndex).Replace("=\"", "");
        //                    int endIndex = start.IndexOf("\"");
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
        //                string text = string.Empty;
        //                try
        //                {

        //                    int startindex = item.IndexOf("js-tweet-text tweet-text\"");
        //                    if (startindex == -1)
        //                    {
        //                        startindex = 0;
        //                        startindex = item.IndexOf("js-tweet-text tweet-text");
        //                    }

        //                    string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "").Replace("js-tweet-text tweet-text tweet-text-rtl\"", "");
        //                    int endindex = start.IndexOf("</p>");

        //                    if (endindex == -1)
        //                    {
        //                        endindex = 0;
        //                        endindex = start.IndexOf("stream-item-footer");
        //                    }

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
        //                                data = ("http" + itemData).Replace(" span ", string.Empty);
        //                                if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
        //                                {
        //                                    text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (!text.Contains(itemData.Replace(" ", "")))
        //                                {
        //                                    text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch { };


        //                StructTweetIDs structTweetIDs = new StructTweetIDs();

        //                if (id != "null")
        //                {
        //                    structTweetIDs.ID_Tweet = tweetUserid;
        //                    structTweetIDs.ID_Tweet_User = id;
        //                    structTweetIDs.username__Tweet_User = from_user_id;
        //                    structTweetIDs.wholeTweetMessage = text;
        //                    lst_structTweetIDs.Add(structTweetIDs);
        //                }
        //                if (lst_structTweetIDs.Count >= noOfRecords)
        //                {
        //                    return lst_structTweetIDs;
        //                }
        //            }
        //            lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetPhotoFromUsername() -- " + keyword + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //    }

        //    return lst_structTweetIDs;
        //}

        public List<StructTweetIDs> GetTweetData_New(string keyword)
        {
            try
            {

                int counter = 0;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;

                string TweetId = string.Empty;


                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }
            startAgain:
                //if (counter == 0)
                //{
                //    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                //    counter++;
                //}
                //else
                //{
                //    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "&scroll_cursor=" + TweetId;
                //}
                if (counter == 0)
                {
                    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime";
                    counter++;
                }
                else
                {
                    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "&scroll_cursor=" + TweetId;

                    searchURL = "https://twitter.com/i/search/timeline?q=" + keyword + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
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

                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    object DEserizedData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                    string DataHtml = (string)((JObject)DEserizedData)["items_html"];
                    string[] splitRes = Regex.Split(DataHtml, "data-item-id");

                    splitRes = splitRes.Skip(1).ToArray();

                    GlobusRegex regx = new GlobusRegex();
                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("follow-button") && !item.Contains("Following"))
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
                            string start = item.Substring(startIndex).Replace("data-user-id=", "");
                            int endIndex = start.IndexOf("data-is-reply-to");
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf("data-expanded-footer=");
                            }
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf(">");
                            }
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "").Replace("\n", string.Empty);
                            if (end.Contains(" "))
                            {
                                end = end.Split(' ')[0];
                            }
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
                            int startIndex = item.IndexOf("data-screen-name=");
                            string start = item.Substring(startIndex).Replace("data-screen-name=", "");
                            int endIndex = start.IndexOf("data-name");
                            if (endIndex > 100)
                            {
                                endIndex = 0;
                                endIndex = start.IndexOf("data-user");
                            }
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
                            int startIndex = item.IndexOf("=\"");
                            string start = item.Substring(startIndex).Replace("=\"", "");
                            int endIndex = start.IndexOf("\"");
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
                        string text = string.Empty;
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
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
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
                        }
                        if (lst_structTweetIDs.Count >= _noOfRecords)
                        {
                            return lst_structTweetIDs;
                        }
                    }
                    lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                }

                if (lst_structTweetIDs.Count > _noOfRecords)
                {
                    if (res_Get_searchURL.Contains("has_more_items\":false"))
                    {
                        return lst_structTweetIDs;
                    }
                    else
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


        public List<StructTweetIDs> GetTweetData_New_ForCampaign(string keyword,int noOfReplies)
        {
            try
            {

                int counter = 0;
                int counterNoOfReplies = noOfReplies;
                lst_structTweetIDs = new List<StructTweetIDs>();
                string res_Get_searchURL = string.Empty;
                string searchURL = string.Empty;
                string maxid = string.Empty;

                string TweetId = string.Empty;
                string max_position = string.Empty;

                if (keyword.Trim().Contains(" "))
                {
                    keyword = keyword.Replace(" ", "+");
                }
            startAgain:
                //if (counter == 0)
                //{
                //    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                //    counter++;
                //}
                //else
                //{
                //    searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "&scroll_cursor=" + TweetId;
                //}
                if (counter == 0)
                {
                    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "";
                   // searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime";
                    searchURL = "https://twitter.com/search?q=" + Uri.EscapeDataString(keyword) + "&src=typd";
                    counter++;
                }
                else
                {
                    //searchURL = "https://twitter.com/i/search/timeline?type=recent&src=typd&include_available_features=1&include_entities=1&max_id=0&q=" + keyword + "&composed_count=0&count=" + noOfRecords + "&scroll_cursor=" + TweetId;

                    //searchURL = "https://twitter.com/i/search/timeline?q=" + Uri.EscapeDataString(keyword) + "&src=typd&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                    searchURL = "https://twitter.com/i/search/timeline?vertical=news&q=" + Uri.EscapeDataString(keyword) + "&src=typd&include_available_features=1&include_entities=1&max_position=" + max_position;
                    counter++;
                }

                try
                {
                    res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");

                    if (string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        res_Get_searchURL = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                    }
                    if (!string.IsNullOrEmpty(res_Get_searchURL))
                    {
                        try
                        {
                            max_position = Utils.getBetween(res_Get_searchURL, "data-max-position=\"", "\"");
                            if (string.IsNullOrEmpty(max_position))
                            {
                                max_position = Utils.getBetween(res_Get_searchURL, "\"min_position\":\"", "\"");
                            }
                        }
                        catch { };
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

                if (!string.IsNullOrEmpty(res_Get_searchURL))
                {
                    string[] splitRes = { };
                    if (counter == 1)
                    {
                        splitRes = Regex.Split(res_Get_searchURL, "data-item-id");
                    }
                    else
                    {
                        object DEserizedData = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                        string DataHtml = (string)((JObject)DEserizedData)["items_html"];
                        splitRes = Regex.Split(DataHtml, "data-item-id");
                    }

                    splitRes = splitRes.Skip(1).ToArray();

                    GlobusRegex regx = new GlobusRegex();
                    foreach (string item in splitRes)
                    {
                        if (item.Contains("data-screen-name=") && !item.Contains("follow-button") && !item.Contains("Following"))
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
                            string start = item.Substring(startIndex).Replace("data-user-id=", "");
                            int endIndex = start.IndexOf("data-is-reply-to");
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf("data-expanded-footer=");
                            }
                            if (endIndex == -1)
                            {
                                endIndex = start.IndexOf(">");
                            }
                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "").Replace("\n", string.Empty);
                            if (end.Contains(" "))
                            {
                                end = end.Split(' ')[0];
                            }
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
                            int startIndex = item.IndexOf("data-screen-name=");
                            string start = item.Substring(startIndex).Replace("data-screen-name=", "");
                            int endIndex = start.IndexOf("data-name");
                            if (endIndex > 100)
                            {
                                endIndex = 0;
                                endIndex = start.IndexOf("data-user");
                            }
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
                            int startIndex = item.IndexOf("=\"");
                            string start = item.Substring(startIndex).Replace("=\"", "");
                            int endIndex = start.IndexOf("\"");
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
                        string text = string.Empty;
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
                                            text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("lang=en data-aria-label-part=0",string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        if (!text.Contains(itemData.Replace(" ", "")))
                                        {
                                            text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("lang=en data-aria-label-part=0",string.Empty);
                                        }
                                    }
                                }
                            }
                            if (text.Contains("data-aria-label-part=0"))
                            {
                                text = globushttpHelper.getBetween(text + ":&$#@", "data-aria-label-part=0", ":&$#@");
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
                        }
                        if (lst_structTweetIDs.Count >= counterNoOfReplies)
                        {
                            return lst_structTweetIDs;
                        }
                    }
                    lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();
                }

                if (lst_structTweetIDs.Count > counterNoOfReplies)
                {
                    if (res_Get_searchURL.Contains("has_more_items\":false"))
                    {
                        return lst_structTweetIDs;
                    }
                    else
                        goto startAgain;
                }
                else
                {
                    //if (res_Get_searchURL.Contains("has_more_items\":false"))
                    //{
                    //    return lst_structTweetIDs;
                    //}
                    //else
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

        public static string GetUserIDFromUsername_New(string username, out string Status, ref GlobusHttpHelper HttpHelper)
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
                   // pagesource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + username), "", "");

                    try
                    {
                        int startindex = pagesource.IndexOf("profile_id");
                        string start = pagesource.Substring(startindex).Replace("profile_id", "");
                        int endindex = start.IndexOf(",");
                        string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Trim();
                        user_id = end.Trim();
                    }
                    catch
                    {
                    }

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
                        catch
                        {
                        }
                    }

                    if (string.IsNullOrEmpty(user_id))
                    {
                        try
                        {
                            int startindex = pagesource.IndexOf("stats js-mini-profile-stats \" data-user-id=\"");
                            if (startindex == -1)
                            {
                                startindex = pagesource.IndexOf("user-actions btn-group not-following not-muting \" data-user-id=\"");
                            }
                            if (startindex == -1)
                            {
                                startindex = pagesource.IndexOf("user-actions btn-group not-following not-muting protected\" data-user-id=\"");
                            }
                            string start = pagesource.Substring(startindex).Replace("stats js-mini-profile-stats \" data-user-id=\"", "").Replace("user-actions btn-group not-following not-muting \" data-user-id=\"", "").Replace("user-actions btn-group not-following not-muting protected\" data-user-id=\"", "").Trim();
                            //int endindex = start.IndexOf("\">");
                            int endindex = start.IndexOf("\"");
                            string end = start.Substring(0, endindex);
                            user_id = end.Replace("\"", "");
                        }
                        catch
                        {
                        }
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

        public int CounterDataNo = 0;
        public List<string> GetFollowers_New_ForMobileVersion(string userID, out string ReturnStatus)
        {
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            //Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
            string cursor = "0";
            int counter = 0;
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string Data = string.Empty;
            string FindCursor = string.Empty;
            //Globals.IsMobileVersion = true;
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
                    Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
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
                        //Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/followers?cursor=" + cursor), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            //Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
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

                                //Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowersList + "_" + userID + ".csv");
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

        public List<string> GetFollowings_NewForMobileVersion(string userID, out string ReturnStatus)
        {
            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
            //Log("[ " + DateTime.Now + " ] => [ Searching For Followers For " + userID + " ]");
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
                        //Log("[ " + DateTime.Now + " ] => [ Wait for 15 minutes For furthur Scraping because Twitter banned for scraping. its already too many requestes. ]");
                        Thread.Sleep(15 * 60 * 1000);
                        Data = HttpHelper.getHtmlfromUrl(new Uri("https://mobile.twitter.com/" + userID + "/following?cursor=" + cursor), "", "");
                        if (string.IsNullOrEmpty(Data) || Data == "Too Many Requestes")
                        {
                            //Log("[ " + DateTime.Now + " ] => [ Wait for 5 minutes more For furthur Scraping. ]");
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

                                //Log("[ " + DateTime.Now + " ] => [ " + userid + ":" + username + " ]");
                                //write to csv
                                GlobusFileHelper.AppendStringToTextfileNewLine(userID + "," + userid + "," + username, Globals.Path_ScrapedFollowingsList + "_" + userID + ".csv");
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


    }
}
