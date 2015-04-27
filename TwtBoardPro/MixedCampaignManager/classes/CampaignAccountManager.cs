using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using BaseLib;
using Globussoft;
using TwitterSignup;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace MixedCampaignManager.classes
{
    public class CampaignAccountManager
    {
        public string Username = string.Empty;
        public string Password = string.Empty;

        public string userID = string.Empty;
        public string postAuthenticityToken = string.Empty;

        public string Screen_name = string.Empty;
        public string FollowerCount = string.Empty;

        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUsername = string.Empty;
        public string proxyPassword = string.Empty;

        public string proxyAddress_Socks5 = string.Empty;
        public string proxyPort_Socks5 = string.Empty;
        public string proxyUsername_Socks5 = string.Empty;
        public string proxyPassword_Socks5 = string.Empty;

        public string AccountStatus = string.Empty;
        public bool IsLoggedIn = false;
        public bool IsNotSuspended = false;
        public bool Isnonemailverifiedaccounts = false;
        
        Regex IdCheck1 = new Regex("^[0-9]*$");
        public BaseLib.Events logEvents = new BaseLib.Events();
        public Globussoft.GlobusHttpHelper globusHttpHelper = new Globussoft.GlobusHttpHelper();

        public CampaignAccountManager()
        {
        }

        public CampaignAccountManager(string Username, string Password, string Screen_name, string follower_Count, int numberOfMessages, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, string currentCity, string HomeTown, string Birthday_Month, string BirthDay_Date, string BirthDay_Year, string AboutMe, string Employer, string College, string HighSchool, string Religion, string profilePic, string FamilyName, string Role, string language, string sex, string activities, string interests, string movies, string music, string books, string favoriteSports, string favoriteTeams, string GroupName, string status)
        {
            this.Username = Username;
            this.Password = Password;
            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
            this.Screen_name = Screen_name;
            this.FollowerCount = follower_Count;
            //this.GroupName = GroupName;
            this.AccountStatus = status;
            //Log("[ " + DateTime.Now + " ] => [ Login in with " + Username + " ]");
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            logEvents.LogText(eventArgs);
        }

        public void Login()
        {
            try
            {
                Log("[ " + DateTime.Now + " ] => [ Logging in with Account : " + Username + " ]");

                string ts = GenerateTimeStamp();
                string get_twitter_first = string.Empty;
                try
                {
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), proxyAddress, proxyPort, proxyUsername, proxyPassword, string.Empty, string.Empty);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(1000);
                    get_twitter_first = globusHttpHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/"), proxyAddress, proxyPort, proxyUsername, proxyPassword, string.Empty, string.Empty);
                }

                try
                {
                    postAuthenticityToken = PostAuthenticityToken(get_twitter_first, "postAuthenticityToken");
                }
                catch { }

                try
                {
                    string get_twitter_second = globusHttpHelper.postFormData(new Uri("https://twitter.com/scribe"), "log%5B%5D=%7B%22event_name%22%3A%22web%3Amobile_gallery%3Agallery%3A%3A%3Aimpression%22%2C%22noob_level%22%3Anull%2C%22internal_referer%22%3Anull%2C%22context%22%3A%22mobile_gallery%22%2C%22event_info%22%3A%22mobile_app_download%22%2C%22user_id%22%3A0%2C%22page%22%3A%22mobile_gallery%22%2C%22_category_%22%3A%22client_event%22%2C%22ts%22%3A" + ts + "%7D", "https://twitter.com/?lang=en&logged_out=1#!/download", "", "", "", "");//globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", string.Empty);

                    string get2nd = globusHttpHelper.getHtmlfromUrlProxy(new Uri("http://twitter.com/account/bootstrap_data?r=0.21632839148912897"), "https://twitter.com/", proxyAddress, proxyPort, proxyUsername, proxyPassword);

                    string get_api = globusHttpHelper.getHtmlfromUrl(new Uri("http://api.twitter.com/receiver.html"), "https://twitter.com/", "");

                }
                catch { }

                string postData = "session%5Busername_or_email%5D=" + Uri.EscapeDataString(Username) + "&session%5Bpassword%5D=" + Uri.EscapeDataString(Password) + "&scribe_log=&redirect_after_login=&authenticity_token=" + postAuthenticityToken + "";

                string response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/sessions"), postData, "https://twitter.com/", proxyAddress, proxyPort, proxyUsername, proxyPassword);

                if (response_Login.Contains("अपनी पहचान सत्यापित करें") || response_Login.Contains("आपके खाते को सुरक्षित रखेने में हमें मदद करें.") || response_Login.Contains("Help us keep your account safe.") || response_Login.Contains("Verify your identity"))
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
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeEmail&remember_me=true", proxyAddress, proxyPort, proxyUsername, proxyPassword);
                        }
                        else
                        {
                            postData = "authenticity_token=" + postAuthenticityToken + "&challenge_id=" + challenge_id + "&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&platform=web&redirect_after_login=&remember_me=true&challenge_response=" + Screen_name;
                            response_Login = globusHttpHelper.postFormData(new Uri("https://twitter.com/account/login_challenge"), postData, "https://twitter.com/account/login_challenge?platform=web&user_id=" + temp_user_id + "&challenge_type=RetypeScreenName&remember_me=true", proxyAddress, proxyPort, proxyUsername, proxyPassword);

                        }
                    }
                    catch { }
                }

                if (response_Login.Contains("signout"))
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

                    string responseURI = globusHttpHelper.gResponse.ResponseUri.ToString().ToLower();

                    if (responseURI.Contains("error"))
                    {
                        //Log("[ " + DateTime.Now + " ] => [ Login Error with " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_FailedLoginAccounts);
                        return;
                    }
                    else if (responseURI.Contains("captcha"))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Asking Captcha with " + Username + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_AskingCaptchaAccounts);
                        return;
                    }

                    IsLoggedIn = true;

                    Log("[ " + DateTime.Now + " ] => [ Logged in with Account : " + Username + " ]");
                    GetScreen_name();
                    GetFollowercount();
                    //clsDBQueryManager Db = new clsDBQueryManager();
                    //Db.InsertScreenNameFollower(Screen_name, FollowerCount, Username);
                }
                else
                {
                    IsLoggedIn = false;
                    Log("[ " + DateTime.Now + " ] => [ Logging failed from Account : " + Username + " ]");
                }
            }
            catch (Exception ex)
            {
                Log("[ " + DateTime.Now + " ] => [ Error in Login : " + Username + " ]");
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword, Globals.path_FailedLoginAccounts);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Login() --> " + Username + ":" + Password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" + proxyPassword + " --> " + ex.Message, Globals.Path_TweetAccountManager);
                return;
            }
        }

        public void GetScreen_name()
        {
            try
            {
                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");
                int startIndex = PageSource.IndexOf("data-screen-name");
                string start = PageSource.Substring(startIndex).Replace("data-screen-name=\"", "");
                int endIndex = start.IndexOf("\"");
                string end = start.Substring(0, endIndex);
                Screen_name = end;
            }
            catch (Exception ex)
            {

            }
        }

        public void GetFollowercount()
        {
            try
            {
                string URL = string.Empty;

                string PageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + Screen_name), "", "");

                URL = globusHttpHelper.gResponse.ResponseUri.AbsoluteUri;

                if (PageSource.Contains("btn resend-confirmation-email-link"))
                {
                    Isnonemailverifiedaccounts = true;
                }


                if (!PageSource.Contains("Account suspended") && !PageSource.Contains("currently suspended") && !URL.Contains("https://twitter.com/account/suspended"))
                {
                    //int indexStart = PageSource.IndexOf("data-nav=\"followers\" >");
                    //string start = PageSource.Substring(indexStart).Replace("data-nav=\"followers\" >", string.Empty);
                    //int indexEnd = start.IndexOf("</strong>");
                    //string end = start.Substring(0, indexEnd).Replace("<strong>", "").Replace(">", "").Replace("\n", "").Replace(" ", "");
                    //FollowerCount = end;
                    //AccountStatus = "Active";
                    //IsNotSuspended = true;


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
                        FollowerCount = end;
                    }
                    catch { }

                    AccountStatus = "Active";
                    IsNotSuspended = true;
                }
                else if (PageSource.Contains("Account suspended") || URL.Contains("https://twitter.com/account/suspended"))
                {
                    Log("[ " + DateTime.Now + " ] => [ " + Username + " - Account Suspended ]");
                    //reminveSuspendedAccounts(Username);
                    //clsDBQueryManager database = new clsDBQueryManager();
                    //database.UpdateSuspendedAcc(Username);
                    AccountStatus = "Account Suspended";
                    IsNotSuspended = false;
                }
            }
            catch (Exception ex)
            {

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

        public string GenerateTimeStamp()
        {
            // Default implementation of UNIX time of the current UTC time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

    }


}
