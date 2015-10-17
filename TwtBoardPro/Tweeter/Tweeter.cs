using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Net;
using System.Web;
using BaseLib;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;


namespace Tweeter
{
    public class Tweeter
    {
        public Events logEvents = new Events();

        public void Tweet(ref Globussoft.GlobusHttpHelper globusHttpHelper, string postAuthenticityToken, string tweetMessage, out string status)
         {
             string tweetdata = string.Empty;
             string tweetUrl = string.Empty;
             string postaTweet = string.Empty;
             try
             {
                 //string abc = tweetMessage.Replace("�", " ");
                 //Post Tweet To Account
                  tweetdata = "authenticity_token=" + postAuthenticityToken + "&place_id=&status=" + Uri.EscapeDataString(tweetMessage);
                  tweetUrl = "https://twitter.com/i/tweet/create";

                  postaTweet = globusHttpHelper.postFormData(new Uri(tweetUrl), tweetdata, "https://twitter.com/", "XMLHttpRequest", "", "", "");

                 
                 ///Old Twitter 1.0 API code
                 //string TweetData = "include_entities=true&status=" + HttpUtility.UrlEncode(tweetMessage) + "&post_authenticity_token=" + postAuthenticityToken;
                 //string res_PostTweet = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/statuses/update.json"), TweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");
                 /////

                 var message = (Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(postaTweet))["message"];

                 //string ss=Uri.en

                 if (postaTweet.Contains("Your Tweet was posted!") || message.ToString().Contains("आपका ट्वीट पोस्ट हो गया") || postaTweet.Contains("tweet_id") || message.Contains("tweet_id") || message.ToString().Contains("Your Tweet was posted!") || postaTweet.Contains(message.ToString()))
                 {
                     status = "posted";
                 }
                 else if (postaTweet.Contains("Your Tweet to") && postaTweet.Contains("has been sent"))
                 {
                     status = "posted";
                 }

                 else
                 {
                     status = "not posted";
                 }
             }
             catch (Exception ex)
             {

                 try
                 {
                     if (ex.Message.Contains("Stream was not readable"))
                     {
                         if (!string.IsNullOrEmpty(Globals.DBCUsername) && !string.IsNullOrEmpty(Globals.DBCPassword))
                         {
                             string _tempCaptcha = globusHttpHelper.getHtmlfromUrl(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=0.88776721409522&lang=en"), "", "");
                             string captchaChallenge = globusHttpHelper.getBetween(_tempCaptcha, "challenge : '", ",").Replace("challenge : '", "").Replace("'", "");
                             string ImageUrl = "https://www.google.com/recaptcha/api/image?c=" + captchaChallenge;

                             WebClient webclient = new WebClient();
                             string captchaText = string.Empty;
                             if (!string.IsNullOrEmpty(captchaChallenge))
                             {
                                 try
                                 {

                                     //WebIP IPObj = new WebIP("http://192.227.234.242:80");
                                     //IPObj.Credentials = CredentialCache.DefaultCredentials;
                                     //webclient.IP = IPObj;

                                     byte[] args = webclient.DownloadData(ImageUrl);

                                     string[] arr1 = new string[] { Globals.DBCUsername, Globals.DBCPassword, "" };

                                     captchaText = DecodeDBC(arr1, args);
                                 }
                                 catch { }
                             }

                             if (!string.IsNullOrEmpty(captchaText))
                             {
                                 string mainUrl = "https://twitter.com/account/challenge?challenge=" + captchaChallenge + "&challenge_name=Captcha&response=" + captchaText.Replace(" ", "+");
                                 string response = globusHttpHelper.getHtmlfromUrl(new Uri(mainUrl), "", "", "");

                                 postaTweet = globusHttpHelper.postFormData(new Uri(tweetUrl), tweetdata, "https://twitter.com/", "XMLHttpRequest", "", "", "");

                                 var message = (Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(postaTweet))["message"];

                                 //string ss=Uri.en

                                 if (postaTweet.Contains("Your Tweet was posted!") || message.ToString().Contains("आपका ट्वीट पोस्ट हो गया") || postaTweet.Contains("tweet_id") || message.Contains("tweet_id") || message.ToString().Contains("Your Tweet was posted!") || postaTweet.Contains(message.ToString()))
                                 {
                                     status = "posted";
                                 }
                                 else if (postaTweet.Contains("Your Tweet to") && postaTweet.Contains("has been sent"))
                                 {
                                     status = "posted";
                                 }

                                 else
                                 {
                                     status = "not posted";
                                 }
                             }
                             else
                             {
                                 status = "not posted";
                             }
                         }
                         else
                         {
                             status = "not posted";
                         }
                     }
                     else
                     {

                         status = "not posted";
                         Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() -- Tweet() --> " + ex.Message, Globals.Path_FollowerErroLog);
                         Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Tweeter() -- Tweet()  --> " + ex.Message, Globals.Path_TwtErrorLogs);
                     }
                 }
                 catch (Exception exp)
                 {
                      status = "not posted";
                      Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() -- Tweet() --> " + exp.Message, Globals.Path_FollowerErroLog);
                         
                 }
             }
        }

        public string DecodeDBC(string[] args, byte[] imageBytes)
        {
            try
            {
                // Put your DBC username & password here:
                DeathByCaptchaTweet.Client client = (DeathByCaptchaTweet.Client)new DeathByCaptchaTweet.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                    Log("[ " + DateTime.Now + " ] => [ Please check your DBC Account Details ]");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    Log("[ " + DateTime.Now + " ] => [ You have 0 Balance in your DBC Account ]");
                    return null;
                }

                for (int i = 2, l = args.Length; i < l; i++)
                {
                    Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                    // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                    // file name, file object, stream, or a vector of bytes, and desired
                    // solving timeout (in seconds) here:
                    DeathByCaptchaTweet.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptchaTweet.Client.DefaultTimeout);
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
                        Log("[ " + DateTime.Now + " ] => [ CAPTCHA was not solved ]");
                        Console.WriteLine("CAPTCHA was not solved");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                //GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- DecodeDBC()  --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                //GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  DecodeDBC() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
            return null;
        }
       
        //Code added by Lijo For tweeting with images in the wait and reply module
        public void TweetMessageWithImage(ref Globussoft.GlobusHttpHelper globusHttpHelper, string postAuthenticityToken, string tweetMessage, string ImageFilePath, out string status)
        {
            string status1 = string.Empty;
            bool IsLocalFile = true;
            string MediaId = string.Empty;
            ///Read Image Data in Byte 
            ///convert byte in base 64 string 
            try
            {
                string _base64String = Convert.ToBase64String(File.ReadAllBytes(ImageFilePath));

                try
                {                    
                    _base64String = StringEncoderDecoder.EncodeBase64String(_base64String);
                }
                catch
                {


                }
                ///call method for posting 
                ///
                string txid = (UnixTimestampFromDateTime(System.DateTime.Now) * 1000).ToString();

                string postData = "authenticity_token=" + postAuthenticityToken + "&iframe_callback=&media=" + _base64String + "&upload_id=" + txid + "&origin=https%3A%2F%2Ftwitter.com";
                string response_ = globusHttpHelper.postFormData(new Uri("https://upload.twitter.com/i/media/upload.iframe?origin=https%3A%2F%2Ftwitter.com"), postData, "https://twitter.com/", "", "", "", "https://twitter.com/");

                MediaId = globusHttpHelper.getBetween(response_, "snowflake_media_id\":", ",").Replace("snowflake_media_id\":", "").Trim();

                string finalpostdata = "authenticity_token=" + postAuthenticityToken + "&media_ids=" + MediaId + "&place_id=&status=" + tweetMessage.Replace(" ", "+") + "&tagged_users=";

                response_ = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/tweet/create"), finalpostdata, "https://twitter.com/", "", "XMLHttpRequest", "", "https://twitter.com/");

                //globusHttpHelper.HttpUploadImageFileWithMessage("https://upload.twitter.com/i/tweet/create_with_media.iframe", ImageFilePath, "media_data[]", "application/octet-stream", nvc, true, ref status1);

                //if (status1 == "okay")
                //{
                //    status1 = "posted";
                //}

                if (response_.Contains("tweet_id"))
                {
                    status1 = "posted";
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImagePostData() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImagePostData() --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            status = status1;
        }
        
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }

        public void ReTweet(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string tweetID, string tweetMessage, out string status)
        {
            try
            {
                string TweetId = tweetID;// "197682704844734464";
                string ReTweetData = "authenticity_token=" + postAuthenticityToken + "&id=" + tweetID;
                string ReTweetPostUrl = "https://twitter.com/i/tweet/retweet";
                string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://twitter.com/", postAuthenticityToken, "XMLHttpRequest", "true", "");

                 //string TweetId = tweetID;// "197682704844734464";
                //string ReTweetData = "post_authenticity_token=" + postAuthenticityToken;
                //string ReTweetPostUrl = "https://api.twitter.com/1/statuses/retweet/" + TweetId + ".json";
                //string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");

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
        public void ReTweetAndFollow(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string tweetID, string tweetMessage, string UserID, out string status)
        {
            Follower.Follower objFollower = new Follower.Follower();
            try
            {
                string TweetId = tweetID;// "197682704844734464";
                string ReTweetData = "authenticity_token=" + postAuthenticityToken + "&id=" + tweetID;
                string ReTweetPostUrl = "https://twitter.com/i/tweet/retweet";

                string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://twitter.com/", postAuthenticityToken, "XMLHttpRequest", "true", "");

                string pagesource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com"), "", "");
                //string TweetId = tweetID;// "197682704844734464";
                //string ReTweetData = "post_authenticity_token=" + postAuthenticityToken;
                //string ReTweetPostUrl = "https://api.twitter.com/1/statuses/retweet/" + TweetId + ".json";
                //string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");

                status = "posted";

                objFollower.FollowUsingProfileID_New(ref globusHttpHelper, pgSrc, postAuthenticityToken, UserID, out status);

            }
            catch (Exception ex)
            {
                status = "not posted";
                //Log("Method>>ReTweet  --- class>>Tweeter.cs : ReTweet Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }



        public void Reply(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string tweetID, string screenName, string tweetMessage, out string status)
        {
            try
            {
                //https://twitter.com/i/tweet/create
                string TweetId = tweetID;//"197551187803906048";
                string ReplyData = "authenticity_token=" + postAuthenticityToken + "&in_reply_to_status_id=" + TweetId + "&place_id=&status=%40" + screenName + "+" + Uri.EscapeDataString(tweetMessage); //"in_reply_to_status_id=" + TweetId + "&include_entities=true&status=%40" + "screenname to reply to" + "+" + tweetMessage + "&post_authenticity_token=" + postAuthenticityToken;
                string res_Post_Reply = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/tweet/create"), ReplyData, "https://twitter.com/" + screenName + "/status/" + tweetID, postAuthenticityToken, "XMLHttpRequest", "true", "");

                status = "posted";
                ///Reply For twitter api 1.0
                //string TweetId = tweetID;// "197682704844734464";
                //string ReTweetData = "post_authenticity_token=" + postAuthenticityToken;
                //string ReTweetPostUrl = "https://api.twitter.com/1/statuses/retweet/" + TweetId + ".json";
                //string a6 = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");
                //string TweetId = tweetID;//"197551187803906048";
                //string ReplyData = "in_reply_to_status_id=" + TweetId + "&include_entities=true&status=%40" + screenName + "+" + HttpUtility.UrlEncode(tweetMessage) + "&post_authenticity_token=" + postAuthenticityToken; ;//"in_reply_to_status_id=" + TweetId + "&include_entities=true&status=%40" + "screenname to reply to" + "+" + tweetMessage + "&post_authenticity_token=" + postAuthenticityToken;
                //string res_Post_Reply = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/statuses/update.json"), ReplyData, "https://api.twitter.com/receiver.html", postAuthenticityToken, "XMLHttpRequest", "true", "");
            }
            catch (Exception ex)
            {
                status = "not posted";
                //Log("Method>>  Reply  --- class>>  Tweeter.cs : Reply Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Reply() --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Reply() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

        public void ReTweetByUrl(ref Globussoft.GlobusHttpHelper globusHttpHelper, string postAuthenticityToken, string retweetID, out string status)
        {
            try
            {
                //Post Tweet To Account

                //authenticity_token=8a67c8aeefc40985cd2ce4cd0ff01a9ae17b5dd2&id=277327596696518656

                string ReTweetData = "authenticity_token=" + postAuthenticityToken + "&id=" + retweetID;
                string ReTweetPostUrl = "https://twitter.com/i/tweet/retweet";
                string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "", postAuthenticityToken, "XMLHttpRequest", "true", "");

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

        public void FavoriteByUrl(ref Globussoft.GlobusHttpHelper globusHttpHelper, string postAuthenticityToken, string retweetID, out string status)
        {
            try
            {
                //Post Tweet To Account

                //authenticity_token=8a67c8aeefc40985cd2ce4cd0ff01a9ae17b5dd2&id=277327596696518656

                string ReTweetData = "authenticity_token=" + postAuthenticityToken + "&id=" + retweetID;
                string ReTweetPostUrl = "https://twitter.com/i/tweet/favorite";
                string res_Post_Retweet = globusHttpHelper.postFormData(new Uri(ReTweetPostUrl), ReTweetData, "", postAuthenticityToken, "XMLHttpRequest", "true", "");

                if (res_Post_Retweet.Contains("\"stat\":\"favorite\""))
                {
                    status = "posted";
                }
                else
                {
                    status = "not posted";
                }
            }
            catch (Exception ex)
            {
                status = "not posted";
                //Log("Method>>ReTweet  --- class>>Tweeter.cs : ReTweet Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Tweeter() - ReTweet --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }
       
        private void Log(string message)
        {
            EventsArgs eArgs = new EventsArgs(message);
            logEvents.LogText(eArgs);
        }
    }


}
