using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
//using BaseLib;

namespace Globussoft
{
    public class GlobusHttpHelper
    {

        CookieCollection gCookies;
        HttpWebRequest gRequest;
        HttpWebResponse gResponse;

        public string getHtmlfromUrl(Uri url)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);

            gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            gRequest.Method = "GET";
            //gRequest.AllowAutoRedirect = false;
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);

                try
                {
                    //gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0 - 2078004405 - 1321685323158", "/"));
                    //gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1322116799.1322206824.9", "/"));
                    //gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                    //gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                    //gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                }
                catch (Exception ex)
                {

                }
            }
            //Get Response for this request url

            setExpect100Continue();
            gResponse = (HttpWebResponse)gRequest.GetResponse();

            //check if the status code is http 200 or http ok
            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                //check if response object has any cookies or not
                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                return responseString;
            }
            else
            {
                return "Error";
            }

        }

        string proxyAddress = string.Empty;
        int port = 80;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        public Uri GetResponseData()
        {
            return gResponse.ResponseUri;
        }

        public string getHtmlfromUrl(Uri url, string Referes, string Token)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);

            gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            gRequest.Method = "GET";
            //gRequest.AllowAutoRedirect = false;
            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);
            ChangeProxy("127.0.0.1", 8890, proxyUsername, proxyPassword);

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }



            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }
            //Get Response for this request url

            setExpect100Continue();
            gResponse = (HttpWebResponse)gRequest.GetResponse();

            //check if the status code is http 200 or http ok
            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                //check if response object has any cookies or not
                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                return responseString;
            }
            else
            {
                return "Error";
            }

        }


        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

            gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            ///Set Proxy
            this.proxyAddress = proxyAddress;
            this.port = port;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            gRequest.Method = "GET";
            //gRequest.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);

                //try
                //{
                //    gRequest.CookieContainer.Add(url, new Cookie("__qca", "P0-2078004405-1321685323158", "/"));
                //    gRequest.CookieContainer.Add(url, new Cookie("__utma", "101828306.1814567160.1321685324.1321697619.1321858563.3", "/"));
                //    gRequest.CookieContainer.Add(url, new Cookie("__utmz", "101828306.1321685324.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/"));
                //    gRequest.CookieContainer.Add(url, new Cookie("__utmb", "101828306.2.10.1321858563", "/"));
                //    gRequest.CookieContainer.Add(url, new Cookie("__utmc", "101828306", "/"));
                //}
                //catch (Exception ex)
                //{

                //}
            }
            //Get Response for this request url

            setExpect100Continue();
            gResponse = (HttpWebResponse)gRequest.GetResponse();

            //check if the status code is http 200 or http ok
            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);


                //check if response object has any cookies or not
                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                return responseString;
            }
            else
            {
                return "Error";
            }

        }

        public string getHtmlfromAsx(Uri url)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.4) Gecko/2008102920 Firefox/3.0.4";
            gRequest.CookieContainer = new CookieContainer();//new CookieContainer();
            gRequest.ContentType = "video/x-ms-asf";

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
                setExpect100Continue();
            }
            //Get Response for this request url
            gResponse = (HttpWebResponse)gRequest.GetResponse();
            setExpect100Continue();
            //check if the status code is http 200 or http ok
            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                setExpect100Continue();
                //check if response object has any cookies or not
                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }

                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error";
            }
        }

        //public void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        //{

        //    ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
        //    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        //    //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
        //    byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        //    gRequest = (HttpWebRequest)WebRequest.Create(url);
        //    gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
        //    gRequest.Method = "POST";
        //    gRequest.KeepAlive = true;
        //    gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

        //    //ChangeProxy("127.0.0.1", 8888, "", "");

        //    gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

        //    #region CookieManagment

        //    if (this.gCookies != null && this.gCookies.Count > 0)
        //    {
        //        gRequest.CookieContainer.Add(gCookies);
        //    }
        //    #endregion

        //    Stream rs = gRequest.GetRequestStream();

        //    string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        //    foreach (string key in nvc.Keys)
        //    {
        //        rs.Write(boundarybytes, 0, boundarybytes.Length);
        //        string formitem = string.Format(formdataTemplate, key, nvc[key]);
        //        byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
        //        rs.Write(formitembytes, 0, formitembytes.Length);
        //    }
        //    rs.Write(boundarybytes, 0, boundarybytes.Length);

        //    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        //    string header = string.Format(headerTemplate, paramName, file, contentType);
        //    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        //    rs.Write(headerbytes, 0, headerbytes.Length);

        //    FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        //    byte[] buffer = new byte[4096];
        //    int bytesRead = 0;
        //    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        //    {
        //        rs.Write(buffer, 0, bytesRead);
        //    }
        //    fileStream.Close();

        //    byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        //    rs.Write(trailer, 0, trailer.Length);
        //    rs.Close();

        //    #region CookieManagment

        //    if (this.gCookies != null && this.gCookies.Count > 0)
        //    {
        //        gRequest.CookieContainer.Add(gCookies);
        //    }

        //    #endregion

        //    WebResponse wresp = null;
        //    try
        //    {
        //        wresp = gRequest.GetResponse();
        //        Stream stream2 = wresp.GetResponseStream();
        //        StreamReader reader2 = new StreamReader(stream2);
        //        //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
        //    }
        //    catch (Exception ex)
        //    {
        //        //log.Error("Error uploading file", ex);
        //        if (wresp != null)
        //        {
        //            wresp.Close();
        //            wresp = null;
        //        }
        //    }
        //    finally
        //    {
        //        gRequest = null;
        //    }
        //    //}

        //}

        public bool HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, string proxyAddress, int proxyPort, string proxyUsername, string proxyPassword)
        {

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create(url);
            gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.Method = "POST";
            gRequest.KeepAlive = true;
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            ChangeProxy(proxyAddress, proxyPort, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }
            #endregion

            Stream rs = gRequest.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                wresp = gRequest.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                return true;
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return false;
            }
            finally
            {
                gRequest = null;
            }
            //}

        }

        public void MultiPartImageUpload(string Username, string Password, string localImagePath)
        {
            ///Login to FB

            string valueLSD = "name=" + "\"lsd\"";
            string pageSource = getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));
            //string pageSource = getHtmlfromUrlProxy(new Uri("https://www.facebook.com/login.php"));
            int startIndex = pageSource.IndexOf(valueLSD) + 18;
            string value = pageSource.Substring(startIndex, 5);

            string ResponseLogin = postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");


            string profileSource = getHtmlfromUrl(new Uri("http://www.facebook.com/editprofile.php?sk=basic"));

            string valuepost_form_id = "name=" + "\"post_form_id\"";
            string valuefb_dtsg = "name=" + "\"fb_dtsg\"";
            ///Setting Post Data Params...
            int sIndex = profileSource.IndexOf(valuepost_form_id) + 27;
            string post_form_id = profileSource.Substring(sIndex, 32);

            int s1Index = profileSource.IndexOf(valuefb_dtsg) + 22;
            string fb_dtsg = profileSource.Substring(s1Index, 8);

            int s2Index = profileSource.IndexOf("user") + 5;
            string userId = profileSource.Substring(s2Index, 15);

            if (ResponseLogin.Contains("http://www.facebook.com/profile.php?id="))
            {
                string[] arrUser = System.Text.RegularExpressions.Regex.Split(ResponseLogin, "href");
                foreach (String itemUser in arrUser)
                {
                    if (!itemUser.Contains("<!DOCTYPE"))
                    {
                        if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
                        {

                            string[] arrhre = itemUser.Split('"');
                            userId = arrhre[1].Replace("http://www.facebook.com/profile.php?id=", "");
                            if (userId.Contains("&"))
                            {
                                string[] Arr = userId.Split('&');
                                userId = Arr[0];
                            }

                            break;

                        }
                    }
                }
            }

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("post_form_id", post_form_id);
            nvc.Add("fb_dtsg", fb_dtsg);
            nvc.Add("id", userId);
            nvc.Add("type", "profile");
            nvc.Add("return", "/ajax/profile/picture/upload_iframe.php?pic_type=1&id=" + userId);

            //UploadFilesToRemoteUrl("http://upload.facebook.com/pic_upload.php ", new string[] { @"C:\Users\Globus-n2\Desktop\Windows Photo Viewer Wallpaper.jpg" }, "", nvc);
            //HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc, proxyAddress, );
        }

        public bool MultiPartImageUpload(string Username, string Password, string localImagePath, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            ///Login to FB

            //string valueLSD = "name=" + "\"lsd\"";

            int intProxyPort = 80;

            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            string pageSource = getHtmlfromUrlProxy(new Uri("https://www.facebook.com/login.php"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);
            //int startIndex = pageSource.IndexOf(valueLSD) + 18;
            string value = GlobusHttpHelper.GetParamValue(pageSource, "lsd");

            string ResponseLogin = postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");

            ///Setting Post Data Params...

            string userId = GlobusHttpHelper.Get_UserID(ResponseLogin);

            string pgSrc_Profile = getHtmlfromUrl(new Uri("https://www.facebook.com/profile.php?id=" + userId + ""));

            string profileSource = getHtmlfromUrl(new Uri("https://www.facebook.com/ajax/timeline/profile_pic_selector.php?profile_id=" + userId + "&__a=1&__user=" + userId + ""));

        

            //GlobusHttpHelper httpHelper = new GlobusHttpHelper();
            /////Get User ID
            //ProfileIDExtractor idExtracter = new ProfileIDExtractor();
            //idExtracter.ExtractFriendIDs(ref httpHelper, ref userId);


            string fb_dtsg = GlobusHttpHelper.GetParamValue(ResponseLogin, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(ResponseLogin, "fb_dtsg");
            }


            string last_action_id = Globussoft.GlobusHttpHelper.ParseJson(pgSrc_Profile, "last_action_id");

            string postData = "last_action_id=" + last_action_id + "&fb_dtsg=" + fb_dtsg + "&__user="+userId+"&phstamp=165816810252768712174";

            string res = postFormData(new Uri("https://www.facebook.com/ajax/mercury/thread_sync.php?__a=1"), postData);

            NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("post_form_id", post_form_id);
            nvc.Add("fb_dtsg", fb_dtsg);
            nvc.Add("id", userId);
            nvc.Add("type", "profile");
            //nvc.Add("return", "/ajax/profile/picture/upload_iframe.php?pic_type=1&id=" + userId);
            nvc.Add("return", "/ajax/timeline/profile_pic_upload.php?pic_type=1&id=" + userId);

            //UploadFilesToRemoteUrl("http://upload.facebook.com/pic_upload.php ", new string[] { @"C:\Users\Globus-n2\Desktop\Windows Photo Viewer Wallpaper.jpg" }, "", nvc);
            //HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc);
            if (HttpUploadFile("https://upload.facebook.com/pic_upload.php ", localImagePath, "pic", "image/jpeg", nvc, proxyAddress, intProxyPort, proxyUsername, proxyPassword))
            //if (HttpUploadFile("http://upload.facebook.com/pic_upload.php ", localImagePath, "file", "image/jpeg", nvc, proxyAddress, intProxyPort, proxyUsername, proxyPassword))
            {
                return true;
            }
            return false;

        }

        public string postFormData(Uri formActionUrl, string postData)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormData(Uri formActionUrl, string postData, string Referes, string Token)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }
            ///Modified BySumit 18-11-2011
            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);
            ChangeProxy("127.0.0.1", 8890, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataProxy(Uri formActionUrl, string postData, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = "User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";

            ///Modified BySumit 18-11-2011
            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            #region CookieManagement
            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                setExpect100Continue();
                gRequest.CookieContainer.Add(gCookies);
            }

            //logic to postdata to the form
            try
            {
                setExpect100Continue();
                string postdata = string.Format(postData);
                byte[] postBuffer = System.Text.Encoding.GetEncoding(1252).GetBytes(postData);
                gRequest.ContentLength = postBuffer.Length;
                Stream postDataStream = gRequest.GetRequestStream();
                postDataStream.Write(postBuffer, 0, postBuffer.Length);
                postDataStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Logger.LogText("Internet Connectivity Exception : "+ ex.Message,null);
            }
            //post data logic ends

            //Get Response for this request url
            try
            {
                gResponse = (HttpWebResponse)gRequest.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Logger.LogText("Response from "+formActionUrl + ":" + ex.Message,null);
            }



            //check if the status code is http 200 or http ok

            if (gResponse.StatusCode == HttpStatusCode.OK)
            {
                //get all the cookies from the current request and add them to the response object cookies
                setExpect100Continue();
                gResponse.Cookies = gRequest.CookieContainer.GetCookies(gRequest.RequestUri);
                //check if response object has any cookies or not
                //Added by sandeep pathak
                //gCookiesContainer = gRequest.CookieContainer;  

                if (gResponse.Cookies.Count > 0)
                {
                    //check if this is the first request/response, if this is the response of first request gCookies
                    //will be null
                    if (this.gCookies == null)
                    {
                        gCookies = gResponse.Cookies;
                    }
                    else
                    {
                        foreach (Cookie oRespCookie in gResponse.Cookies)
                        {
                            bool bMatch = false;
                            foreach (Cookie oReqCookie in this.gCookies)
                            {
                                if (oReqCookie.Name == oRespCookie.Name)
                                {
                                    oReqCookie.Value = oRespCookie.Value;
                                    bMatch = true;
                                    break; // 
                                }
                            }
                            if (!bMatch)
                                this.gCookies.Add(oRespCookie);
                        }
                    }
                }
            #endregion



                StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                string responseString = reader.ReadToEnd();
                reader.Close();
                //Console.Write("Response String:" + responseString);
                return responseString;
            }
            else
            {
                return "Error in posting data";
            }

        }

        public void setExpect100Continue()
        {
            if (ServicePointManager.Expect100Continue == true)
            {
                ServicePointManager.Expect100Continue = false;
            }
        }

        public void setExpect100ContinueToTrue()
        {
            if (ServicePointManager.Expect100Continue == false)
            {
                ServicePointManager.Expect100Continue = true;
            }
        }

        public void ChangeProxy(string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            try
            {
                WebProxy myproxy = new WebProxy(proxyAddress, port);
                myproxy.BypassProxyOnLocal = false;

                if (!string.IsNullOrEmpty(proxyUsername) && !string.IsNullOrEmpty(proxyPassword))
                {
                    myproxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                }
                gRequest.Proxy = myproxy;
            }
            catch (Exception ex)
            {

            }

        }

        public static string GetParamValue(string pgSrc, string paramName)
        {
            try
            {
                string temp1 = "name=\\\\\\\""+paramName+"\\\\\\\"";
                string temp = "name=\\\"" + paramName + "\\\"";

                if (pgSrc.Contains("name='" + paramName + "'"))
                {
                    string param = "name='" + paramName + "'";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endparamName = pgSrc.IndexOf("'", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endparamName - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\"" + paramName + "\""))
                {
                    string param = "name=\"" + paramName + "\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\\\"" + paramName + "\\\""))
                {
                    string param = "name=\\\"" + paramName + "\\\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=\\", startparamName) + "value=\\".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\\\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }

                else if (pgSrc.Contains("name=\\\\\\\"" + paramName + "\\\\\\\""))
                {
                    string param = "name=\\\\\\\"" + paramName + "\\\\\\\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=\\\\\\\"", startparamName) + "value=\\\\\\\"".Length;
                    int endcommentPostID = pgSrc.IndexOf("\\\\\\\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        #region Get Parameter Values commonly used

        public static string Get_UserID(string pgSrc)
        {
            string UserID = GlobusHttpHelper.GetParamValue(pgSrc, "user");//pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            if (string.IsNullOrEmpty(UserID))
            {
                UserID = GlobusHttpHelper.ParseJson(pgSrc, "user");
            }
            return UserID;
        }

        public static string Get_fb_dtsg(string pgSrc)
        {
            string fb_dtsg = GlobusHttpHelper.GetParamValue(pgSrc, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(pgSrc, "fb_dtsg");
            }
            return fb_dtsg;
        }

        public static string Get_post_form_id(string pgSrc)
        {
            string post_form_id = GlobusHttpHelper.GetParamValue(pgSrc, "post_form_id");//pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            if (string.IsNullOrEmpty(post_form_id))
            {
                post_form_id = GlobusHttpHelper.ParseJson(pgSrc, "post_form_id");
            }
            return post_form_id;
        }


        public List<string> GetHrefsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
            }

            var regexhttps = new Regex(@"https://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedStringHttps = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regexhttps.Matches(ModifiedStringHttps))
            {
                lstUrl.Add(url.Value);
            }

            return lstUrl;
        }

        #endregion


        public static string ParseJson(string data, string paramName)
        {
            try
            {
                int startIndx = data.IndexOf(paramName) + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string ParseEncodedJson(string data, string paramName)
        {
            try
            {
                data = data.Replace("&quot;", "\"");
                int startIndx = data.IndexOf("\"" + paramName + "\"") + ("\"" + paramName + "\"").Length + 1;
                int endIndx = data.IndexOf("\"", startIndx);

                string value = data.Substring(startIndx, endIndx - startIndx);
                value = value.Replace(",", "");
                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string ParseEncodedJsonPageID(string data, string paramName)
        {
            try
            {
                data = data.Replace("&quot;", "\"");

                string value = ParseJson(data, paramName);

                return value;
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}
