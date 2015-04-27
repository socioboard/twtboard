using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BaseLib;
using System.Web;

namespace Randomiser
{
    public class RandomiserManager
    {
        #region Global Variable

        string UserName = string.Empty;
        string UserId = string.Empty;
        string Password = string.Empty;
        string Screen_Name = string.Empty;
        string ProxyAddress = string.Empty;
        string ProxyPort = string.Empty;
        string ProxyUserName = string.Empty;
        string ProxyPassword = string.Empty;

        

        #endregion

        public RandomiserManager()
        {
        }

        public RandomiserManager(string Username, string userId, string Password, string Screen_name, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
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

        public void Tweet(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string tweetMessage, out string status)
        {
            try
            {

                //string abc = tweetMessage.Replace("�", " ");
                //Post Tweet To Account
                string strpostAuthenticityToken = string.Empty;

                string get_twitter_first = globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword, string.Empty, string.Empty);

                if (!string.IsNullOrEmpty(get_twitter_first) && !string.IsNullOrWhiteSpace(get_twitter_first))
                {
                    strpostAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }

                if (!string.IsNullOrEmpty(tweetMessage) && !string.IsNullOrWhiteSpace(tweetMessage))
                {
                    if (tweetMessage.Length > 140)
                    {
                        int index = tweetMessage.Length - 140;
                        tweetMessage = tweetMessage.Remove(tweetMessage.Length - index);
                        string TweetData = "include_entities=true&status=" + HttpUtility.UrlEncode(tweetMessage) + "&post_authenticity_token=" + strpostAuthenticityToken;
                        string res_PostTweet = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/statuses/update.json"), TweetData, "https://api.twitter.com/receiver.html", strpostAuthenticityToken, "XMLHttpRequest", "true", "");

                        status = "posted";
                    }
                    else
                    {
                        string TweetData = "include_entities=true&status=" + HttpUtility.UrlEncode(tweetMessage) + "&post_authenticity_token=" + strpostAuthenticityToken;
                        string res_PostTweet = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/statuses/update.json"), TweetData, "https://api.twitter.com/receiver.html", strpostAuthenticityToken, "XMLHttpRequest", "true", "");

                        status = "posted";
                    }
                }
                else
                {
                    status = "Tweet Message Is Empty !";
                }
            }
            catch (Exception ex)
            {
                status = "not posted";
                //Log("Method>>Tweet  --- class>>Tweeter.cs : Tweet Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() -- Tweet() --> " + ex.Message, Globals.Path_FollowerErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Tweeter() -- Tweet()  --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void ReTweet(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string tweetID, string tweetMessage, out string status)
        {
            try
            {
                //Post Tweet To Account
                //ReTweet 0

                string strpostAuthenticityToken = string.Empty;

                string get_twitter_first = globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), ProxyAddress, ProxyPort, ProxyUserName, ProxyPassword, string.Empty, string.Empty);

                if (!string.IsNullOrEmpty(get_twitter_first) && !string.IsNullOrWhiteSpace(get_twitter_first))
                {
                   // strpostAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }

                string TweetId = tweetID;// "197682704844734464";
                string ReTweetData = "post_authenticity_token=" + postAuthenticityToken;
                string ReTweetPostUrl = "https://api.twitter.com/1/statuses/retweet/" + TweetId + ".json";
                string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");

                status = "posted";
            }
            catch (Exception ex)
            {
                status = "not posted";
                //Log("Method>>ReTweet  --- class>>Tweeter.cs : ReTweet Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
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
    }
}
