using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chilkat;
using System.Text.RegularExpressions;
using System.Threading;
using Globussoft;

namespace BaseLib
{
    public class ChilkatIMAP
    {
        public string Username = string.Empty;
        public string Password = string.Empty;

        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUser = string.Empty;
        public string proxyPass = string.Empty;

        Chilkat.Imap iMap = new Imap();

        public static Events EmailVerifyLogEvent = new Events();


        public string Connect(string yahooEmail, string yahooPassword)
        {
            iMap.UnlockComponent("THEBACIMAPMAILQ_OtWKOHoF1R0Q");
            iMap.Connect("imap.n.mail.yahoo.com");
            //iMap.Login("Karlawtt201@yahoo.com", "rga77qViNIV");
            iMap.Login(yahooEmail, yahooPassword);
            iMap.SelectMailbox("Inbox");

            // Get a message set containing all the message IDs
            // in the selected mailbox.
            Chilkat.MessageSet msgSet;
            msgSet = iMap.Search("ALL", true);

            // Fetch all the mail into a bundle object.
            Chilkat.Email email = new Chilkat.Email();
            //bundle = iMap.FetchBundle(msgSet);

            for (int i = msgSet.Count - 1; i > 0; i--)
            {
                email = iMap.FetchSingle(msgSet.GetId(i), true);
                if (email.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                {
                    int startIndex = email.Body.IndexOf("http://www.facebook.com/confirmemail.php?e=");
                    int endIndex = email.Body.IndexOf(">", startIndex) -1;
                    string activationLink = email.Body.Substring(startIndex, endIndex - startIndex).Replace("\r\n", "");
                    activationLink = activationLink.Replace("3D", "").Replace("hotmai=l", "hotmail").Replace("%40", "@");

                    string decodedActivationLink = Uri.UnescapeDataString(activationLink);

                    return decodedActivationLink;
                }
            }
            return string.Empty;

            //if (email.)
            //{

            //}

        }


        /// <summary>
        /// Gets into Yahoo Email and fetches Activation Link and Sends Http Request to it
        /// Calls LoginVerfy() which sends Http Request
        /// Also sends sends Request to gif URL, and 2 more URLs
        /// </summary>
        /// <param name="yahooEmail"></param>
        /// <param name="yahooPassword"></param>
        public void GetFBMails(string yahooEmail, string yahooPassword)
        {
            LoggerEmailVerify("Email verify....");
            Username = yahooEmail;
            Password = yahooPassword;
            //Username = "Karlawtt201@yahoo.com";
            //Password = "rga77qViNIV";
            iMap.UnlockComponent("THEBACIMAPMAILQ_OtWKOHoF1R0Q");

            //iMap.
            //iMap.HttpProxyHostname = "127.0.0.1";
            //iMap.HttpProxyPort = 8888;

            iMap.Connect("imap.n.mail.yahoo.com");
            iMap.Login(yahooEmail, yahooPassword);
            iMap.SelectMailbox("Inbox");

            // Get a message set containing all the message IDs
            // in the selected mailbox.
            Chilkat.MessageSet msgSet;
            //msgSet = iMap.Search("FROM \"facebookmail.com\"", true);
            msgSet = iMap.GetAllUids();

            // Fetch all the mail into a bundle object.
            Chilkat.Email email = new Chilkat.Email();
            //bundle = iMap.FetchBundle(msgSet);
            string strEmail = string.Empty;
            List<string> lstData = new List<string>();
            if (msgSet != null)
            {
                for (int i = msgSet.Count; i > 0; i--)
                {
                    email = iMap.FetchSingle(msgSet.GetId(i), true);
                    strEmail = email.Subject;
                    string emailHtml = email.GetHtmlBody();
                    lstData.Add(strEmail);
                    if (email.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                    {
                        foreach (string href in GetUrlsFromString(email.Body))
                        {
                            try
                            {
                                string staticUrl = string.Empty;
                                string email_open_log_picUrl = string.Empty;

                                string strBody = email.Body;
                                string[] arr = Regex.Split(strBody, "src=");
                                foreach (string item in arr)
                                {
                                    if (!item.Contains("<!DOCTYPE"))
                                    {
                                        if (item.Contains("static"))
                                        {
                                            string[] arrStatic = item.Split('"');
                                            staticUrl = arrStatic[1];
                                        }
                                        if (item.Contains("email_open_log_pic"))
                                        {
                                            string[] arrlog_pic = item.Split('"');
                                            email_open_log_picUrl = arrlog_pic[1];
                                            email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                            break;
                                        }
                                    }
                                }

                                string href1 = href.Replace("&amp;report=1", "");
                                href1 = href.Replace("amp;", "");

                                LoginVerfy(href1, staticUrl, email_open_log_picUrl);
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                        //return;
                    }
                    else if (email.Subject.Contains("Just one more step to get started on Facebook"))
                    {
                        foreach (string href in  GetUrlsFromString(email.Body))
                        {
                            try
                            {
                                string staticUrl = string.Empty;
                                string email_open_log_picUrl = string.Empty;

                                string strBody = email.Body;
                                string[] arr = Regex.Split(strBody, "src=");
                                foreach (string item in arr)
                                {
                                    if (!item.Contains("<!DOCTYPE"))
                                    {
                                        if (item.Contains("static"))
                                        {
                                            string[] arrStatic = item.Split('"');
                                            staticUrl = arrStatic[1];
                                        }
                                        if (item.Contains("email_open_log_pic"))
                                        {
                                            string[] arrlog_pic = item.Split('"');
                                            email_open_log_picUrl = arrlog_pic[1];
                                            email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                            break;
                                        }
                                    }
                                }


                                string href1 = href.Replace("&amp;report=1", "");
                                href1 = href.Replace("amp;", "");

                                LoginVerfy(href1, staticUrl, email_open_log_picUrl);
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.StackTrace);
                            }
                        }
                        //return;
                    }

                } 
            }
        }

        public void LoginVerifyChilkat(ref Http http)
        {
           // Chilkat.Http http1 = new Chilkat.Http();
           // http1 = http;

           // ///Chilkat Http Request to be used in Http Post...
           // Chilkat.HttpRequest req = new Chilkat.HttpRequest();

           // bool success;

           // success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
           // if (success != true)
           // {
           //     return;
           // }

           // ///Save Cookies...
           // http.CookieDir = "memory";
           // http.SendCookies = true;
           // http.SaveCookies = true;

           // string pageSource = http.QuickGetStr("http://www.facebook.com/login.php");
           // string valueLSD = "name=" + "\"lsd\"";
           // int startIndex = pageSource.IndexOf(valueLSD) + 18;
           // string value = pageSource.Substring(startIndex, 5);

           // ///Decode data as chilkat Request accepts only decoded data...
           // string charTest = "%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84";
           // string emailUser = Username.Split('@')[0] + "%40" + Username.Split('@')[1];
           // string DecodedCharTest = System.Web.HttpUtility.UrlDecode(charTest);
           // string DecodedEmail = System.Web.HttpUtility.UrlDecode(emailUser);


           // //  Build an HTTP POST Request:
           // req.UsePost();
           // //req.Path = "/login.php?login_attempt=1";

           // req.AddHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16");
           // //req.SetFromUrl("http://www.facebook.com/login.php?login_attempt=1");

           // req.AddParam("charset_test", DecodedCharTest);
           // req.AddParam("lsd", value);
           // req.AddParam("locale", "en_US");
           // req.AddParam("email", DecodedEmail);
           // req.AddParam("pass", Password);
           // req.AddParam("persistent", "1");
           // req.AddParam("default_persistent", "1");
           // req.AddParam("charset_test", DecodedCharTest);
           // req.AddParam("lsd", value);

           // Chilkat.HttpResponse respUsingPostURLEncoded = http.PostUrlEncoded("http://www.facebook.com/login.php?login_attempt=1", req);

           // if (respUsingPostURLEncoded == null)
           // {
           //     Thread.Sleep(1000);
           //     respUsingPostURLEncoded = http.PostUrlEncoded("http://www.facebook.com/login.php?login_attempt=1", req);
           // }
           // if (respUsingPostURLEncoded == null)
           // {
           //     return;
           // }

           // string ResponseLoginPostURLEncoded = respUsingPostURLEncoded.BodyStr;

           // string ResponseLogin = http.QuickGetStr("http://www.facebook.com/home.php");

           // if (ResponseLogin == null)
           // {
           //     Console.WriteLine("not login");
           // }

           // string ResponseConfirmed = http.QuickGetStr("http://www.facebook.com/?email_confirmed=1");
           //// http://www.facebook.com/desktop/notifier/transfer.php?__a=1

        }


        public void LoginVerfy(string ConfemUrl, string gif, string logpic)
        {
            Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(ConfemUrl), proxyAddress, intProxyPort, proxyUser, proxyPass);
            //string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(url), "127.0.0.1", 8888, "", "");

            string valueLSD = "name=" + "\"lsd\"";
            string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            int startIndex = pageSource.IndexOf(valueLSD) + 18;
            string value = pageSource.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            //string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Email.Split('@')[0] + "%40" + Email.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
            string ResponseLogin = HttpHelper.postFormDataProxy(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "", proxyAddress, intProxyPort, proxyUser, proxyPass);
            //string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + "rani.khanna" + "%40" + "hotmail.com" + "&pass=" + "s15121985" + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
            /////ssss gif &s=a parse com  &s=a//////////////////////////
            string PageSourse12 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));

            try
            {
                string PageSourse13 = HttpHelper.getHtmlfromUrl(new Uri(gif));
            }
            catch (Exception)
            {
            }
            try
            {
                string PageSourse14 = HttpHelper.getHtmlfromUrl(new Uri(logpic + "&s=a"));
            }
            catch (Exception)
            {
            }
            try
            {
                string PageSourse15 = HttpHelper.getHtmlfromUrl(new Uri(logpic));
            }
            catch (Exception)
            {
            }

            //** User Id ***************//////////////////////////////////
            string UsreId = string.Empty;
            string ProFilePost = string.Empty;

            //if (ResponseLogin.Contains("http://www.facebook.com/profile.php?id="))
            //{
            //    string[] arrUser = Regex.Split(ResponseLogin, "href");
            //    foreach (String itemUser in arrUser)
            //    {
            //        if (!itemUser.Contains("<!DOCTYPE"))
            //        {
            //            if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
            //            {

            //                string[] arrhre = itemUser.Split('"');
            //                ProFilePost = arrhre[1];
            //                break;


            //            }
            //        }
            //    }
            //}
            //if (ResponseLogin.Contains("http://www.facebook.com/profile.php?id="))
            //{
            //    UsreId = ProFilePost.Replace("http://www.facebook.com/profile.php?id=", "");
            //}
            if (string.IsNullOrEmpty(UsreId))
            {
                UsreId = GlobusHttpHelper.ParseJson(ResponseLogin, "user");
            }


            //*** User Id **************//////////////////////////////////

            //*** Post Data **************//////////////////////////////////
            string fb_dtsg = GlobusHttpHelper.GetParamValue(ResponseLogin, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(ResponseLogin, "fb_dtsg");
            }

            string post_form_id = GlobusHttpHelper.GetParamValue(ResponseLogin, "post_form_id");//pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            if (string.IsNullOrEmpty(post_form_id))
            {
                post_form_id = GlobusHttpHelper.ParseJson(ResponseLogin, "post_form_id");
            }

            string PageSourceConfirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            string pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));

            

            ///Code for skipping additional optional Page
            try
            {
                string postDataSkipFirstStep = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=friend_requests&next_step_name=contact_importer&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=16581681208511510848190";

                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), postDataSkipFirstStep);
                Thread.Sleep(1000);
            }
            catch (Exception)
            {
            }

            pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted/?step=contact_importer")); 


            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not

            string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;

            if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
            {
                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=contact_importer&next_step_name=classmates_coworkers&previous_step_name=friend_requests&skip=Skip%20this%20step&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=165816776847576104244";
                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));

                Thread.Sleep(1000);

                pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted/?step=classmates_coworkers")); 
            }
            if ((pageSrc1.Contains("Fill out your Profile Info") || pageSrc1.Contains("Fill out your Profile info")) && pageSrc1.Contains("Skip"))
            {
                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=info&hs[school][id][0]=&hs[school][text][0]=&hs[start_year][text][0]=-1&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[start_year][text][0]=-1&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=165816776847576104580";
                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                //pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));

                ///Post Data Parsing
                Dictionary<string, string> lstfriend_browser_id = new Dictionary<string, string>();

                string[] initFriendArray = Regex.Split(postRes, "FriendStatus.initFriend");

                int tempCount = 0;
                foreach (string item in initFriendArray)
                {
                    if (tempCount==0)
                    {
                        tempCount++;
                        continue;
                    }
                    if (tempCount > 0)
                    {
                        int startIndx = item.IndexOf("(\\") + "(\\".Length + 1;
                        int endIndx = item.IndexOf("\\", startIndx);
                        string paramValue = item.Substring(startIndx, endIndx - startIndx);
                        lstfriend_browser_id.Add("friend_browser_id[" + (tempCount-1) + "]=", paramValue);
                        tempCount++;
                    }
                }

                string partPostData = string.Empty;
                foreach (var item in lstfriend_browser_id)
                {
                    partPostData = partPostData + item.Key + item.Value + "&";
                }

                string newPostData1 = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=pymk&hs[school][id][0]=&hs[school][text][0]=&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&" + partPostData + "phstamp=1658167541109987992266";//"post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=pymk&friend_browser_id[0]=100002869910855&friend_browser_id[1]=100001857152486&friend_browser_id[2]=575678600&friend_browser_id[3]=100003506761599&friend_browser_id[4]=563402235&friend_browser_id[5]=1268675170&friend_browser_id[6]=1701838026&friend_browser_id[7]=623640106&friend_browser_id[8]=648873235&friend_browser_id[9]=100000151781814&friend_browser_id[10]=657007597&friend_browser_id[11]=1483373867&friend_browser_id[12]=778266161&friend_browser_id[13]=1087830021&friend_browser_id[14]=100001333876108&friend_browser_id[15]=100000534308531&friend_browser_id[16]=1213205246&friend_browser_id[17]=45608778&friend_browser_id[18]=100003080150820&friend_browser_id[19]=892195716&friend_browser_id[20]=100001238774509&friend_browser_id[21]=45602360&friend_browser_id[22]=100000054900916&friend_browser_id[23]=100001308090108&friend_browser_id[24]=100000400766182&friend_browser_id[25]=100001159247338&friend_browser_id[26]=1537081666&friend_browser_id[27]=100000743261988&friend_browser_id[28]=1029373920&friend_browser_id[29]=1077680976&friend_browser_id[30]=100000001266475&friend_browser_id[31]=504487658&friend_browser_id[32]=82600225&friend_browser_id[33]=1023509811&friend_browser_id[34]=100000128061486&friend_browser_id[35]=100001853125513&friend_browser_id[36]=576201748&friend_browser_id[37]=22806492&friend_browser_id[38]=100003232772830&friend_browser_id[39]=1447942875&friend_browser_id[40]=100000131241521&friend_browser_id[41]=100002076794734&friend_browser_id[42]=1397169487&friend_browser_id[43]=1457321074&friend_browser_id[44]=1170969536&friend_browser_id[45]=18903839&friend_browser_id[46]=695329369&friend_browser_id[47]=1265734280&friend_browser_id[48]=698096805&friend_browser_id[49]=777678515&friend_browser_id[50]=529685319&hs[school][id][0]=&hs[school][text][0]=&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=100003556207009&phstamp=1658167541109987992266";
                string postRes1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData1);

                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));

                Thread.Sleep(4000);

                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));


                string newPostData2 = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=upload_profile_pic&previous_step_name=classmates_coworkers&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=165816812057527766201";
                string postRes2 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

            }
            if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            {
                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=upload_profile_pic&previous_step_name=classmates_coworkers&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=165816776847576104201";
                try
                {
                    string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                    pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
                    pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php?ref=wizard"));
                }
                catch (Exception)
                {
                }
               
            }
            if (pageSrc3.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through " + Username);

            }
            if (pageSourceCheck.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through Email" + Username);
            }
            #endregion
            //** FB Account Check email varified or not ***********************************************************************************//

            string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

          
            ////**Post Message For User***********************/////////////////////////////////////////////////////
            int count = 0;
            
            //string[] Arr = post_form_id.Split('"');
            //post_form_id = Arr[4];
            //post_form_id = post_form_id.Replace("\\", "");
            //post_form_id = post_form_id.Replace("\\", "");
            //post_form_id = post_form_id.Replace("\\", "");
            //string Response1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId);
            string Response1 = HttpHelper.postFormDataProxy(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId, proxyAddress, intProxyPort, proxyUser, proxyPass);

            string Response2 = HttpHelper.postFormDataProxy(new Uri("http://www.facebook.com/ajax/httponly_cookies.php?dc=snc2&__a=1"), "keys[0]=1150335208&post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId, proxyAddress, intProxyPort, proxyUser, proxyPass);

            string PageSourceCon = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/ajax/contextual_help.php?__a=1&set_name=welcome&__user=" + UsreId));


            string pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            if (pageSourceCheck1111.Contains("complete the sign-up process"))
            {
                Console.WriteLine("Account is not verified for : " + Username);
            }

            LoggerEmailVerify("Registration Succeeded for: " + Username);
            //LoggerVerify("Email verification completed for : " + Email);
        }

        /// <summary>
        /// Sends Http Request to URL sent and also sends Request to gif URL, and 2 more URLs
        /// </summary>
        /// <param name="ConfemUrl">Facebook Confirmation URL</param>
        /// <param name="gif"></param>
        /// <param name="logpic"></param>
        public void LoginVerfyOld(string ConfemUrl,string gif,string logpic)
        {
            Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(ConfemUrl), proxyAddress, intProxyPort, proxyUser, proxyPass);
            //string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(url), "127.0.0.1", 8888, "", "");

            string valueLSD = "name=" + "\"lsd\"";
            string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            int startIndex = pageSource.IndexOf(valueLSD) + 18;
            string value = pageSource.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
            /////ssss gif &s=a parse com  &s=a//////////////////////////
            string PageSourse12 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));
            string PageSourse13 = HttpHelper.getHtmlfromUrl(new Uri(gif));
            string PageSourse14 = HttpHelper.getHtmlfromUrl(new Uri(logpic+ "&s=a"));
            string PageSourse15 = HttpHelper.getHtmlfromUrl(new Uri(logpic));

            string PageSourse16 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));


            string PageSourceConfirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            string pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));
            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not

            string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;

            if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
            {
                pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
            }
            if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
            {
                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
            }
            if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            {
                pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
            }
            if (pageSrc3.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through " + Username);

            }
            if (pageSourceCheck.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through Email" + Username);
            }
            #endregion
            //** FB Account Check email varified or not ***********************************************************************************//

            string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

            //** User Id ***************//////////////////////////////////
            string UsreId = string.Empty;
            string ProFilePost = string.Empty;
            ////**Post Message For User***********************/////////////////////////////////////////////////////
            int count = 0;
            if (pageSourceHome.Contains("http://www.facebook.com/profile.php?id="))
            {
                string[] arrUser = Regex.Split(pageSourceHome, "href");
                foreach (String itemUser in arrUser)
                {
                    if (!itemUser.Contains("<!DOCTYPE"))
                    {
                        if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
                        {
                            
                                string[] arrhre = itemUser.Split('"');
                                ProFilePost = arrhre[1];
                                break;
                            
                            
                        }
                    }
                }
            }
            if (ProFilePost.Contains("http://www.facebook.com/profile.php?id="))
            {
                UsreId = ProFilePost.Replace("http://www.facebook.com/profile.php?id=", "");
            }



            //*** User Id **************//////////////////////////////////

            //*** Post Data **************//////////////////////////////////
            string fb_dtsg = pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);

            string post_form_id = pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            string[] Arr = post_form_id.Split('"');
            post_form_id = Arr[2];
            post_form_id = post_form_id.Replace("\\", "");
            post_form_id = post_form_id.Replace("\\", "");
            post_form_id = post_form_id.Replace("\\", "");
            fb_dtsg = Arr[6];
            string Response1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id="+post_form_id+"&fb_dtsg="+fb_dtsg+"&lsd&post_form_id_source=AsyncRequest&__user="+UsreId);

            string Response2 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/httponly_cookies.php?dc=snc2&__a=1"), "keys[0]=1150335208&post_form_id="+post_form_id+"&fb_dtsg="+fb_dtsg+"&lsd&post_form_id_source=AsyncRequest&__user="+UsreId);

            string PageSourceCon = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/ajax/contextual_help.php?__a=1&set_name=welcome&__user="+UsreId));


            string pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            if (pageSourceCheck1111.Contains("complete the sign-up process"))
            {
                Console.WriteLine("the account is not verified");
                //LoggerWallPoste("not varified through Email" + Username);
            }

            string pageSource11 = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            startIndex = pageSource.IndexOf(valueLSD) + 18;
            value = pageSource.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            string ResponseLogin11 = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");

            string PageSourceConfirmed11 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            if (PageSourceConfirmed11.Contains("complete the sign-up process"))
            {
                Console.WriteLine("the account is not verified");
                //LoggerWallPoste("not varified through Email" + Username);
            }
        }

        public List<string> GetUrlsFromString(string HtmlData)
        {
            
            List<string> lstUrl = new List<string>();
            //var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            //var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            //foreach (Match url in regex.Matches(ModifiedString))
            //{
            //    lstUrl.Add(url.Value);
            //}
            //** Change Ritesh Satthya////////////////////////////////////////////
            try
            {
                string strData = HtmlData;
                string[] arr = Regex.Split(strData, "href");

                foreach (string strhref in arr)
                {
                    if (!strhref.Contains("<!DOCTYPE"))
                    {
                        string[] tempArr = strhref.Split('"');
                        lstUrl.Add(tempArr[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            ////** Change Ritesh Satthya////////////////////////////////////////////
            return lstUrl;
        }

        private void LoggerEmailVerify(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            EmailVerifyLogEvent.LogText(eventArgs);
        }
    }
}


