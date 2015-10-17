using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using BaseLib;
using System.Data;

namespace ReplyInterface
{
    public class ReplyInterface
    {
        #region Global Oblect
        clsDB_ReplyInterface obj_clsDB_ReplyInterface = new clsDB_ReplyInterface();

        public static BaseLib.Events logEvents = new BaseLib.Events();
        #endregion

        #region Global Variable

        List<string> lstAjaxStatusId = new List<string>();
        string UserName = string.Empty;
        string UserId = string.Empty;
        string Password = string.Empty;
        string Screen_Name = string.Empty;
        string IPAddress = string.Empty;
        string IPPort = string.Empty;
        string IPUsername = string.Empty;
        string IPpassword = string.Empty;
        #endregion

        public ReplyInterface()
        {
        }

        public ReplyInterface(string Username, string userId, string Password, string Screen_name, string IPAddress, string IPPort, string IPUsername, string IPpassword)
        {
            this.UserName = Username;
            this.UserId = userId;
            this.Password = Password;
            this.Screen_Name = Screen_name;
            this.IPAddress = IPAddress;
            this.IPPort = IPPort;
            this.IPUsername = IPUsername;
            this.IPpassword = IPpassword;
        }


        public void GetScrapTweetAndReply(ref Globussoft.GlobusHttpHelper globusHttpHelper, ref string userId, ref string userName, ref string Screen_name, ref string postAuthenticityToken)
        {
            try
            {

                Log("[ " + DateTime.Now + " ] => [ Starting ScrapTweetAndReply.............With User Name : " + UserName + " ]");

                string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");

                List<string> lstStatusId = GetstatusId(pageSource, globusHttpHelper, ref Screen_name);

                if (lstStatusId.Count > 0)
                {
                    try
                    {
                        int lastIndex = lstStatusId.Count - 1;
                        string max_Id = lstStatusId[lastIndex];

                        List<string> lstAjaxStatusId = GetStatusIdThroughAjax(ref globusHttpHelper, Screen_name, max_Id);

                        lstStatusId.AddRange(lstAjaxStatusId);

                        lstStatusId = lstStatusId.Distinct().ToList();
                    }
                    catch
                    {
                    }
                }

                Log("[ " + DateTime.Now + " ] => [ Total StatusId = " + lstStatusId.Count + " With User Name : " + UserName + " ]");

                int counter = 0;

                int countDataBeforeSaving = 0;
                try
                {
                    
                    DataSet ds = obj_clsDB_ReplyInterface.SelectFromtb_ReplyCampaign(StringEncoderDecoder.Encode(UserName));
                    countDataBeforeSaving = ds.Tables[0].Rows.Count;
                }
                catch
                {
                }

                foreach (string item in lstStatusId)
                {
                    try
                    {


                        string url = "https://twitter.com/" + Screen_name + "/status/" + item;

                        string pageSourceOfstatusId = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");

                        List<string> lstTweet = GetTweet(globusHttpHelper, pageSourceOfstatusId);
                        
                        List<string> lstTweetUserName = GetTweetUserName(globusHttpHelper, pageSourceOfstatusId);
                        
                        List<string> lstTweetUserId = GetTweetUserId(globusHttpHelper, pageSourceOfstatusId);

                        List<string> lstTweetTime = GetTweetTime(globusHttpHelper, pageSourceOfstatusId);

                        List<string> lstReply = GetReply(globusHttpHelper, pageSourceOfstatusId);

                        List<string> lstReplyUserId = GetReplyUserId(globusHttpHelper, pageSourceOfstatusId);
                        // lstReplyUserId = lstReplyUserId.Distinct().ToList();

                        List<string> lstReplyUserName = GetReplyUserName(globusHttpHelper, pageSourceOfstatusId);

                        List<string> lstReplyTime = GetReplyTime(globusHttpHelper, pageSourceOfstatusId);

                        int index = 0;
                        foreach (string ReplyUserIditem in lstReplyUserId)
                        {
                            try
                            {


                                #region Variable
                                string statusId = string.Empty;
                                string strPostAuthenticityToken = string.Empty;
                                string UserId = string.Empty;
                                string screenName = string.Empty;
                                string tweet = string.Empty;
                                string tweetUserId = string.Empty;
                                string tweetUserName = string.Empty;
                                string tweetTime = string.Empty;
                                string reply = string.Empty;
                                string replyUserId = string.Empty;
                                string replyUserName = string.Empty;
                                string replyTime = string.Empty;
                                #endregion

                                if (!string.IsNullOrEmpty(ReplyUserIditem) && !string.IsNullOrWhiteSpace(ReplyUserIditem))
                                {
                                    replyUserId = ReplyUserIditem;

                                    foreach (string Tweetitem in lstTweet)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(Tweetitem) && !string.IsNullOrWhiteSpace(Tweetitem))
                                            {
                                                tweet = Tweetitem;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    foreach (string TweetUserNameitem in lstTweetUserName)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(TweetUserNameitem) && !string.IsNullOrWhiteSpace(TweetUserNameitem))
                                            {
                                                tweetUserName = TweetUserNameitem;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    foreach (string TweetUserIditem in lstTweetUserId)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(TweetUserIditem) && !string.IsNullOrWhiteSpace(TweetUserIditem))
                                            {
                                                tweetUserId = TweetUserIditem;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    foreach (string lstTweetTimeitem in lstTweetTime)
                                    {
                                        try
                                        {
                                            if (!string.IsNullOrEmpty(lstTweetTimeitem) && !string.IsNullOrWhiteSpace(lstTweetTimeitem))
                                            {
                                                tweetTime = lstTweetTimeitem;
                                                break;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }

                                    try
                                    {
                                        reply = lstReply[index];
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        replyUserName = lstReplyUserName[index];
                                    }
                                    catch
                                    {
                                    }

                                    try
                                    {
                                        replyTime = lstReplyTime[index];
                                    }
                                    catch
                                    {
                                    }

                                    index++;


                                    statusId = item;

                                    strPostAuthenticityToken = postAuthenticityToken;

                                    UserId = userId;

                                    UserName = userName;

                                    screenName = Screen_Name;

                                    if (!string.IsNullOrEmpty(statusId) && !string.IsNullOrWhiteSpace(statusId) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrEmpty(replyUserId) && !string.IsNullOrWhiteSpace(replyUserId) && !string.IsNullOrEmpty(replyUserName) && !string.IsNullOrWhiteSpace(replyUserName) && !string.IsNullOrEmpty(reply) && !string.IsNullOrWhiteSpace(reply) && !string.IsNullOrEmpty(strPostAuthenticityToken) && !string.IsNullOrWhiteSpace(strPostAuthenticityToken)) //!string.IsNullOrEmpty(UserId) && !string.IsNullOrWhiteSpace(UserId) && 
                                    {
                                        try
                                        {
                                            DataSet ds = obj_clsDB_ReplyInterface.SelectStatusIdUserNameTweetAndReplyFromtb_ReplyCampaign(StringEncoderDecoder.Encode(statusId), StringEncoderDecoder.Encode(UserName), StringEncoderDecoder.Encode(tweet), StringEncoderDecoder.Encode(reply));

                                            if (ds.Tables[0].Rows.Count < 1)
                                            {
                                                obj_clsDB_ReplyInterface.InserIntotb_ReplyCampaign(StringEncoderDecoder.Encode(statusId), StringEncoderDecoder.Encode(strPostAuthenticityToken), StringEncoderDecoder.Encode(UserId), StringEncoderDecoder.Encode(userName), StringEncoderDecoder.Encode(screenName), StringEncoderDecoder.Encode(tweetUserId), StringEncoderDecoder.Encode(tweetUserName), StringEncoderDecoder.Encode(replyUserId), StringEncoderDecoder.Encode(replyUserName), StringEncoderDecoder.Encode(tweet), StringEncoderDecoder.Encode(tweetTime), StringEncoderDecoder.Encode(reply), StringEncoderDecoder.Encode(replyTime), StringEncoderDecoder.Encode('0'.ToString()));

                                                counter++;
                                                Log("[ " + DateTime.Now + " ] => [ " + counter + " Record Saved In Data Base With User Name : " + UserName + " ]");
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

                    }
                    catch
                    {
                    }
                }

                int countDataAfterSaving = 0;
                try
                {

                    DataSet ds = obj_clsDB_ReplyInterface.SelectFromtb_ReplyCampaign(StringEncoderDecoder.Encode(UserName));
                    countDataAfterSaving = ds.Tables[0].Rows.Count;



                    if (countDataBeforeSaving == countDataAfterSaving)
                    {
                        Log("[ " + DateTime.Now + " ] => [ No New Reply With User Name : " + userName + " ] ");
                    }
                }
                catch
                {
                }

                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLTED With User Name : " + userName + " ]");
                Log("------------------------------------------------------------------------------------------------------------------------------------------");
            }
            catch
            {
            }
        }

        private List<string> GetstatusId(string pagesource, Globussoft.GlobusHttpHelper globusHttpHelper, ref string screenName)
        {

            List<string> lstStatusId = new List<string>();
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Starting GetstatusId..............With User Name : " + UserName + " ]");

                string[] arrStatusId = Regex.Split(pagesource, "/" + screenName + "/status/");
                foreach (string item in arrStatusId)
                {
                    try
                    {
                        if (!item.Contains("<!DOCTYPE html>"))
                        {


                            string statusId = string.Empty;
                            string statusIdValue = item.Substring(0, 30);
                            if (statusIdValue.Contains("&"))
                            {

                                try
                                {
                                    statusId = statusIdValue.Substring(0, statusIdValue.IndexOf("&"));
                                    string[] arrCheckStatusId = Regex.Split(statusId, "[a-zA-Z]");
                                    lstStatusId.Add(arrCheckStatusId[0]);
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                try
                                {
                                    statusId = statusIdValue.Substring(0, statusIdValue.IndexOf("\""));
                                    string[] arrCheckStatusId = Regex.Split(statusId, "[a-zA-Z]");
                                    lstStatusId.Add(arrCheckStatusId[0]);
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
            }
            catch
            {
            }
            lstStatusId = lstStatusId.Distinct().ToList();

            //Log("Total StatusId = "+lstStatusId.Count +" With User Name : "+UserName);

            return lstStatusId;
        }

        private List<string> GetTweet(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstTweet = new List<string>();
            try
            {
                //string pageSource=globusHttpHelper.getHtmlfromUrl(new Uri(url),"","");
                if (pageSource.Contains("permalink-inner permalink-tweet-container") && pageSource.Contains("js-tweet-text tweet-text "))
                {
                    //string[] arrTweet = Regex.Split(pageSource, "js-tweet-text tweet-text");
                    List<string> lstTweetUserName = globusHttpHelper.GetTextDataByTagAndAttributeName(pageSource, "p", "js-tweet-text tweet-text ");
                    if (lstTweetUserName.Count > 0)
                    {
                        foreach (string item in lstTweetUserName)
                        {
                            try
                            {
                                string tweet = item.Replace("&quot;", string.Empty).Replace("&amp;", string.Empty);
                                lstTweet.Add(tweet);
                            }
                            catch
                            {
                            }
                        }
                    }

                }
            }
            catch
            {
            }
            return lstTweet;
        }

        private List<string> GetTweetUserName(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstTweetUserName = new List<string>();
            try
            {
                //string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");
                if (pageSource.Contains("permalink-inner permalink-tweet-container"))
                {
                    // string[] arrTweetUsername = Regex.Split(pageSource, "permalink-inner permalink-tweet-container");
                    string TweetUserName = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "permalink-inner permalink-tweet-container");
                    if (TweetUserName.Contains("username js-action-profile-name"))
                    {
                        List<string> lst_TweetUserName = globusHttpHelper.GetTextDataByTagAndAttributeName(TweetUserName, "span", "username js-action-profile-name");
                        lstTweetUserName.AddRange(lst_TweetUserName);
                    }

                }
            }
            catch
            {
            }
            lstTweetUserName = lstTweetUserName.Distinct().ToList();
            return lstTweetUserName;
        }

        private List<string> GetTweetUserId(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstTweetId = new List<string>();
            try
            {
                //string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");
                if (pageSource.Contains("permalink-inner permalink-tweet-container"))
                {
                    string TweetUserId = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "permalink-inner permalink-tweet-container");

                    if (TweetUserId.Contains("data-user-id="))
                    {
                        try
                        {
                            string tweetUserId1 = TweetUserId.Substring(TweetUserId.IndexOf("data-user-id="), TweetUserId.IndexOf(" ", TweetUserId.IndexOf("data-user-id=")) - TweetUserId.IndexOf("data-user-id=")).Replace("data-user-id=", string.Empty).Replace("\"", string.Empty).Trim();
                            lstTweetId.Add(tweetUserId1);
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
            lstTweetId = lstTweetId.Distinct().ToList();
            return lstTweetId;
        }

        private List<string> GetReply(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstReply = new List<string>();
            try
            {
                //string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");

                if (pageSource.Contains("tweets-wrapper"))
                {
                    try
                    {
                        string TweetUserId = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "tweets-wrapper");
                        if (TweetUserId.Contains("js-tweet-text"))
                        {
                            List<string> lstTweetReply = globusHttpHelper.GetTextDataByTagAndAttributeName(TweetUserId, "p", "js-tweet-text");
                            foreach (string item in lstTweetReply)
                            {
                                lstReply.Add(item.Replace("&amp;", string.Empty).Replace("quot;", string.Empty));
                            }
                            //lstReply.AddRange(lstTweetReply);
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
            return lstReply;
        }

        private List<string> GetReplyUserId(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstReplyUserId = new List<string>();
            try
            {

                //string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");

                if (pageSource.Contains("tweets-wrapper"))
                {
                    try
                    {
                        string ReplyUserId = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "tweets-wrapper");
                        if (ReplyUserId.Contains("data-user-id="))
                        {
                            try
                            {
                                string[] arrDataUserId = Regex.Split(ReplyUserId, "data-user-id=");
                                foreach (string item in arrDataUserId)
                                {
                                    try
                                    {
                                        if (!item.Contains("<div class=\"tweets-wrapper\">") && item.Contains(" "))
                                        {
                                            string ReplyUserId1 = item.Substring(0, item.IndexOf(" ")).Replace("\"", string.Empty).Replace(">", string.Empty).Trim();
                                            string[] arrReplyUserId1 = Regex.Split(ReplyUserId1, "[a-zA-Z]");
                                            lstReplyUserId.Add(arrReplyUserId1[0]);
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
                }

            }
            catch
            {
            }
            //lstReplyUserId=lstReplyUserId.Distinct().ToList();
            return lstReplyUserId;
        }

        private List<string> GetReplyUserName(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstReplyUserName = new List<string>();
            try
            {

                //string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");

                if (pageSource.Contains("tweets-wrapper"))
                {
                    try
                    {
                        string ReplyUserName = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "tweets-wrapper");
                        if (ReplyUserName.Contains("username js-action-profile-name"))
                        {
                            try
                            {
                                List<string> lst_ReplyUserName = globusHttpHelper.GetTextDataByTagAndAttributeName(ReplyUserName, "span", "username js-action-profile-name");

                                foreach (string item in lst_ReplyUserName)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(item) && !string.IsNullOrWhiteSpace(item))
                                        {
                                            lstReplyUserName.Add(item);
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
                }

            }
            catch
            {
            }
            lstReplyUserName = lstReplyUserName.Distinct().ToList();
            return lstReplyUserName;
        }

        public void Reply(ref Globussoft.GlobusHttpHelper globusHttpHelper, string postAuthenticityToken, string tweetID, string tweetUserName, string screenName, string tweetMessage, string userName)
        {
            try
            {
                if (userName == UserName)
                {
                    string strpostAuthenticityToken = string.Empty;

                    string get_twitter_first = globusHttpHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/"), IPAddress, IPPort, IPUsername, IPpassword, string.Empty, string.Empty);

                    if (!string.IsNullOrEmpty(get_twitter_first) && !string.IsNullOrWhiteSpace(get_twitter_first))
                    {
                        strpostAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                    }
                    //Reply
                    //string TweetId = tweetID;// "197682704844734464";
                    //string ReTweetData = "post_authenticity_token=" + postAuthenticityToken;
                    //string ReTweetPostUrl = "https://api.twitter.com/1/statuses/retweet/" + TweetId + ".json";
                    //string a6 = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");
                    if (!string.IsNullOrEmpty(tweetMessage) && !string.IsNullOrWhiteSpace(tweetMessage))
                    {
                        string TweetId = tweetID;//"197551187803906048";
                        string ReplyData = "in_reply_to_status_id=" + TweetId + "&include_entities=true&status=" + screenName + " " + tweetUserName + " " + HttpUtility.UrlEncode(tweetMessage) + "&post_authenticity_token=" + strpostAuthenticityToken;//"in_reply_to_status_id=" + TweetId + "&include_entities=true&status=%40" + "screenname to reply to" + "+" + tweetMessage + "&post_authenticity_token=" + postAuthenticityToken;
                        string res_Post_Reply = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/statuses/update.json"), ReplyData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");

                        //status = "posted";
                        Log("[ " + DateTime.Now + " ] => [ Message Posted Sucessfully Where Status Id = " + tweetID + " With User Name : " + userName + " ]");

                        obj_clsDB_ReplyInterface.DeleteRecordsAfterReplyFromtb_ReplyCampaign(StringEncoderDecoder.Encode(tweetID), StringEncoderDecoder.Encode(userName));
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Message Couldn't Post  Sucessfully .Since Message Is Empty Where Status Id = " + tweetID + " With User Name : " + userName + " ]");
                    }
                }
            }
            catch (Exception ex)
            {
                //status = "not posted";
                Log("[ " + DateTime.Now + " ] => [ Method>>  Reply  --- class>>  Tweeter.cs : Reply Exception With User Name : " + UserName + "    " + ex.Message + " ]");
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply() --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Reply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
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


        private List<string> GetStatusIdThroughAjax(ref Globussoft.GlobusHttpHelper globusHttpHelper, string screenName, string MaxId)
        {
            string max_id = MaxId;
        repeatAjaxRequests:
            try
            {
                string status = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/i/profiles/show/" + screenName + "/timeline/with_replies?include_available_features=1&include_entities=1&max_id=" + max_id), "", "");

                if (status.Contains(@"/status")) //    /status
                {
                    try
                    {
                        string[] arrStatusId = Regex.Split(status, @"\\\/" + screenName + @"\\\/status\\\/");

                        foreach (string item in arrStatusId)
                        {
                            try
                            {
                                if (!item.Contains("{\"max_id\":"))
                                {


                                    string statusId = string.Empty;
                                    string statusIdValue = item.Substring(0, 30);
                                    if (statusIdValue.Contains("&"))
                                    {

                                        try
                                        {
                                            statusId = statusIdValue.Substring(2, statusIdValue.IndexOf("&")).Replace("&", string.Empty).Trim();
                                            string[] arrCheckStatusId = Regex.Split(statusId, "[^0-9]");

                                            foreach (string item1 in arrCheckStatusId)
                                            {
                                                try
                                                {
                                                    if (item1.Length > 3)
                                                    {
                                                        lstAjaxStatusId.Add(item1);
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
                                    else
                                    {
                                        try
                                        {
                                            statusId = statusIdValue.Substring(2, statusIdValue.IndexOf("\"", 2)).Replace("\"", string.Empty).Trim();
                                            string[] arrCheckStatusId = Regex.Split(statusId, "[^0-9]");

                                            foreach (string item2 in arrCheckStatusId)
                                            {
                                                try
                                                {
                                                    if (item2.Length > 3)
                                                    {
                                                        lstAjaxStatusId.Add(item2);
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
                                    lstAjaxStatusId = lstAjaxStatusId.Distinct().ToList();
                                }
                            }
                            catch
                            {
                            }
                        }

                        if (status.Contains("max_id\":"))
                        {
                            try
                            {
                                string max_id1 = status.Substring(status.IndexOf("max_id\":"), status.IndexOf(",", status.IndexOf("max_id\":")) - status.IndexOf("max_id\":")).Replace("max_id\":", string.Empty).Replace("\"", string.Empty).Trim();
                                string[] arrMaxId = Regex.Split(max_id, "[^0-9]");
                                foreach (string item in arrMaxId)
                                {
                                    try
                                    {
                                        if (item.Length > 3)
                                        {
                                            max_id = max_id1;
                                            goto repeatAjaxRequests;
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
                    catch
                    {
                    }
                }


            }
            catch
            {
            }
            lstAjaxStatusId = lstAjaxStatusId.Distinct().ToList();
            return lstAjaxStatusId;
        }

        private List<string> GetTweetTime(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstTweetTime = new List<string>();
            try
            {
                if (pageSource.Contains("permalink-inner permalink-tweet-container"))
                {
                    // string[] arrTweetUsername = Regex.Split(pageSource, "permalink-inner permalink-tweet-container");
                    string TweetUserName = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "permalink-inner permalink-tweet-container");

                    if (TweetUserName.Contains("client-and-actions") || TweetUserName.Contains("_timestamp js-short-timestamp "))
                    {
                        if (TweetUserName.Contains("_timestamp js-short-timestamp "))
                        {
                            List<string> lstTime = globusHttpHelper.GetTextDataByTagAndAttributeName(TweetUserName, "span", "_timestamp js-short-timestamp ");

                            foreach (string item in lstTime)
                            {
                                try
                                {
                                    if (item.Contains("-"))
                                    {
                                        string[] arrItem = Regex.Split(item, "-");
                                        if (arrItem.Count() > 1)
                                        {
                                            lstTweetTime.Add(arrItem[1].Trim());
                                        }
                                    }
                                    else
                                    {
                                        lstTweetTime.Add(item.Trim());
                                    }
                                }
                                catch
                                {
                                }
                            }

                        }

                        //if (lstTweetTime.Count < 1)
                        {
                            if (TweetUserName.Contains("client-and-actions"))
                            {
                                List<string> lstTime = globusHttpHelper.GetTextDataByTagAndAttributeName(TweetUserName, "div", "client-and-actions");

                                foreach (string item in lstTime)
                                {
                                    try
                                    {
                                        if (item.Contains("-"))
                                        {
                                            string[] arrItem = Regex.Split(item, "-");
                                            if (arrItem.Count() > 1)
                                            {
                                                lstTweetTime.Add(arrItem[1].Trim());
                                            }
                                        }
                                        else
                                        {
                                            lstTweetTime.Add(item.Trim());
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return lstTweetTime;
        }

        private List<string> GetReplyTime(Globussoft.GlobusHttpHelper globusHttpHelper, string pageSource)
        {
            List<string> lstReplyTime = new List<string>();

            try
            {
                if (pageSource.Contains("tweets-wrapper"))
                {
                    try
                    {
                        string ReplyUserName = globusHttpHelper.GetDataWithTagValueByTagAndAttributeName(pageSource, "div", "tweets-wrapper");

                        if (ReplyUserName.Contains("client-and-actions") || ReplyUserName.Contains("_timestamp js-short-timestamp "))
                        {
                            if (ReplyUserName.Contains("_timestamp js-short-timestamp "))
                            {
                                List<string> lstTime = globusHttpHelper.GetTextDataByTagAndAttributeName(ReplyUserName, "span", "_timestamp js-short-timestamp ");

                                foreach (string item in lstTime)
                                {
                                    try
                                    {
                                        if (item.Contains("-"))
                                        {
                                            string[] arrItem = Regex.Split(item, "-");
                                            if (arrItem.Count() > 1)
                                            {
                                                lstReplyTime.Add(arrItem[1].Trim());
                                            }
                                        }
                                        else
                                        {
                                            lstReplyTime.Add(item.Trim());
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }

                            }

                            if (ReplyUserName.Contains("client-and-actions"))
                            {
                                List<string> lstTime = globusHttpHelper.GetTextDataByTagAndAttributeName(ReplyUserName, "div", "client-and-actions");

                                foreach (string item in lstTime)
                                {
                                    try
                                    {
                                        if (item.Contains("-"))
                                        {
                                            string[] arrItem = Regex.Split(item, "-");
                                            if (arrItem.Count() > 1)
                                            {
                                                lstReplyTime.Add(arrItem[1].Trim());
                                            }
                                        }
                                        else
                                        {
                                            lstReplyTime.Add(item.Trim());
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }

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

            return lstReplyTime;
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            //logEvents_static.LogText(eventArgs);
            logEvents.LogText(eventArgs);
        }
    }
}
