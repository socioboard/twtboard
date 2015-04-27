using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using BaseLib;

namespace Globussoft
{
    public class GlobusHttpHelper
    {
        CookieCollection gCookies;
        public HttpWebRequest gRequest;
        public HttpWebResponse gResponse;

        //public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";
        public string UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";   //"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)";
        int Timeout = 90000;

        string proxyAddress = string.Empty;
        int port = 80;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        public GlobusHttpHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public Uri GetResponseData()
        {
            return gResponse.ResponseUri;
        }

        public void getUserAgentForMobileVersion()
        {
            if (Globals.IsMobileVersion)
            {
                UserAgent = "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_0 like Mac OS X; en-us) AppleWebKit/532.9 (KHTML, like Gecko) Version/4.0.5 Mobile/8A293 Safari/6531.22.7";
            }
            else 
            {
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:20.0) Gecko/20100101 Firefox/20.0";   //"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)";
            }
        }
        public string getHtmlfromUrl(Uri url, string Referes, string Token)
        {
            try
            {
                getUserAgentForMobileVersion();
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                //gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";// "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                gRequest.UserAgent = UserAgent;
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

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

                    using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();
                        return responseString; 
                    }
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                //return null;
                if (ex.Message.Contains("The remote server returned an error: (429)") || ex.Message.Contains("Too Many Requests") || ex.Message.Contains("Client Error (429)"))
                {
                    return "Too Many Requestes";
                }
                return null;
            }

        }

        public string getHtmlfromUrlMobile(Uri url, string Referes, string Token, string MobileAgent)
        {
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);
                gRequest.UserAgent = UserAgent;
                if (!string.IsNullOrEmpty(MobileAgent))
                {
                    gRequest.UserAgent = MobileAgent;// "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                }
                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

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

                    using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();
                        return responseString;
                    }
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string getHtmlfromUrl(Uri url, string Referes, string Token, string UserAgent)
        {
            try
            {
                setExpect100Continue();
                gRequest = (HttpWebRequest)WebRequest.Create(url);

                gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";// "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
                if (!string.IsNullOrEmpty(UserAgent))
                {
                    gRequest.UserAgent = UserAgent;
                }

                gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
                //gRequest.Headers["Cache-Control"] = "max-age=0";
                gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
                //gRequest.Connection = "keep-alive";
                gRequest.Timeout = Timeout;

                gRequest.KeepAlive = true;

                gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

                gRequest.Method = "GET";
                //gRequest.AllowAutoRedirect = false;
                ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

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

                    using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                    {
                        string responseString = reader.ReadToEnd();
                        return responseString;
                    }
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string getHtmlfromUrlProxyChecker(Uri url, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {
            string responseString = string.Empty;
              string responseURI = string.Empty;
            try
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

                    responseURI = gResponse.ResponseUri.AbsoluteUri;

                    StreamReader reader = new StreamReader(gResponse.GetResponseStream());
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    return responseString;
                }
                else
                {
                    return "Error";
                }
            }
            catch
            {
            }
            return responseString;
        }
   
        public string getHtmlfromUrlProxy(Uri url, string Referes, string proxyAddress, string strport, string proxyUsername, string proxyPassword)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";//"Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";
            gRequest.Timeout = Timeout;

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            ///Set Proxy
            this.proxyAddress = proxyAddress;
            if (BaseLib.Globals.IdCheck.IsMatch(strport) && !string.IsNullOrEmpty(strport))
            {
                this.port = int.Parse(strport);
            }
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }
            }
            else
            {
                return "Error";
            }

        }

        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, string strport, string proxyUsername, string proxyPassword, string Referes, string Token)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

            gRequest.UserAgent = UserAgent;// "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";]
            gRequest.Headers.Add("Javascript-enabled", "true");
            gRequest.Timeout = Timeout;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            ///Set Proxy
            this.proxyAddress = proxyAddress;
            if (BaseLib.Globals.IdCheck.IsMatch(strport) && !string.IsNullOrEmpty(strport))
            {
                this.port = int.Parse(strport);
            }
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }
            }
            else
            {
                return "Error";
            }

        }

        public string getHtmlfromUrlProxy(Uri url, string proxyAddress, string strport, string proxyUsername, string proxyPassword, string Referes, string Token, string UserAgent)
        {
            setExpect100Continue();
            gRequest = (HttpWebRequest)WebRequest.Create(url);
            //gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";

            gRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24";//"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)";;
            if (!string.IsNullOrEmpty(UserAgent))
            {
                gRequest.UserAgent = UserAgent;
            }
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            //gRequest.Headers["Cache-Control"] = "max-age=0";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";
            //gRequest.Connection = "keep-alive";
            gRequest.Timeout = Timeout;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
                gRequest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            ///Set Proxy
            this.proxyAddress = proxyAddress;
            if (BaseLib.Globals.IdCheck.IsMatch(strport) && !string.IsNullOrEmpty(strport))
            {
                this.port = int.Parse(strport);
            }
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }
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
            gRequest.UserAgent = UserAgent;
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }
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

        public string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc, bool IsLocalFile, ref string status)
        {
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/settings/profile");

            gRequest.UserAgent = UserAgent;
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            gRequest.KeepAlive = true;

            //gRequest.AllowAutoRedirect = false;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string tempBoundary = boundary + "\r\n";
            byte[] tempBoundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            //gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.ContentType = "multipart/form-data; boundary=" + tempBoundary;
            gRequest.Method = "POST";
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            gRequest.Referer = "https://twitter.com/settings/profile";

            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);

                //gRequest.CookieContainer.Add(new Cookie("__utma", "43838368.370306257.1336542481.1336542481.1336542481.1", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmb", "43838368.25.10.1336542481", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmc", "43838368", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmz", "43838368.1336542481.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", "twitter.com"));
            }
            #endregion

            using (Stream rs = gRequest.GetRequestStream())
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                int temp = 0;
                foreach (string key in nvc.Keys)
                {
                    if (temp == 0)
                    {
                        rs.Write(tempBoundarybytes, 0, tempBoundarybytes.Length);
                        temp++;
                    }
                    else
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                    }
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);


                if (IsLocalFile)
                {
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"" + file + "\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, "profile_image[uploaded_data]", file, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);

                    if (!string.IsNullOrEmpty(file))
                    {
                        FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            rs.Write(buffer, 0, bytesRead);
                        }
                        fileStream.Close();
                    }
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
            }

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                //wresp = gRequest.GetResponse();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    status = "okay";
                    return responseString;
                }
               
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
            finally
            {
                gRequest = null;
            }
            //}

        }

        public string HttpUploadFileBackground(string url, string file, string paramName, string contentType, NameValueCollection nvc, bool IsLocalFile, ref string status)
        {
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ////log.Debug(string.Format("Uploading {0} to {1}", file, url));
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            //string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            gRequest = (HttpWebRequest)WebRequest.Create("https://twitter.com/settings/design/update");

            gRequest.UserAgent = UserAgent;
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //gRequest.Headers["Accept-Charset"] = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            gRequest.KeepAlive = true;

            //gRequest.AllowAutoRedirect = false;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string tempBoundary = boundary + "\r\n";
            byte[] tempBoundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "\r\n");

            //gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            gRequest.ContentType = "multipart/form-data; boundary=" + tempBoundary;
            gRequest.Method = "POST";
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            gRequest.Referer = "https://twitter.com/settings/design/update";

            //ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);

                //gRequest.CookieContainer.Add(new Cookie("__utma", "43838368.370306257.1336542481.1336542481.1336542481.1", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmb", "43838368.25.10.1336542481", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmc", "43838368", "/", "twitter.com"));
                //gRequest.CookieContainer.Add(new Cookie("__utmz", "43838368.1336542481.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)", "/", "twitter.com"));
            }
            #endregion

            using (Stream rs = gRequest.GetRequestStream())
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                int temp = 0;
                foreach (string key in nvc.Keys)
                {
                    if (temp == 0)
                    {
                        rs.Write(tempBoundarybytes, 0, tempBoundarybytes.Length);
                        temp++;
                    }
                    else
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                    }
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);



                if (IsLocalFile)
                {
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"" + file + "\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, "media_data[]", file, contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);

                    if (!string.IsNullOrEmpty(file))
                    {
                        FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[4096];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            rs.Write(buffer, 0, bytesRead);
                        }
                        fileStream.Close();
                    }
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
            }

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                //wresp = gRequest.GetResponse();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    status = "okay";
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
            finally
            {
                gRequest = null;
            }
            //}

        }

        public void MultiPartImageUpload(string profileUsername, string profileLocation, string profileURL, string profileDescription, string localImagePath, string authenticity_token, ref string status)
        {
            bool IsLocalFile = true;

            #region <Change Other details of account ....>
            try
            {
                ///Make post data
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("authenticity_token", authenticity_token);
                nvc.Add("_method", "PUT");
                nvc.Add("user[name]", profileUsername);
                nvc.Add("user[location]", profileLocation);
                nvc.Add("user[url]", profileURL);
                nvc.Add("user[description]", profileDescription);

                /// call Http request method for posting image 
                HttpUploadFile("https://twitter.com/settings/profile", localImagePath, "profile_image[uploaded_data]", "image/jpeg", nvc, IsLocalFile, ref status);
            }
            catch (Exception ex) { GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> MultiPartImageUpload()  --> " + ex.Message, BaseLib.Globals.Path_ProfileManagerErroLog); }

            #endregion

            #region <Chang profile image of Account...>
            if (!string.IsNullOrEmpty(localImagePath))
            {
                try
                {
                    FileInfo info = new FileInfo(localImagePath);

                    NameValueCollection nvc1 = new NameValueCollection();

                    string abc = (Convert.ToBase64String(File.ReadAllBytes(localImagePath)));

                    string _base64 = BaseLib.StringEncoderDecoder.EncodeBase64String(abc);

                    string Postdata = "authenticity_token=" + authenticity_token + "&fileData=" + _base64 + "&fileName=" + info.Name + "&height=240&offsetLeft=0&offsetTop=0&page_context=settings&scribeContext%5Bcomponent%5D=profile_image_upload&scribeElement=upload&section_context=profile&uploadType=avatar&width=240";

                    postFormData1(new Uri("https://twitter.com/settings/profile/profile_image_update"), (Postdata), "https://twitter.com/settings/profile", "", "XMLHttpRequest", "", "");
                }
                catch (Exception ex)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> MultiPartImageUpload()  --> " + ex.Message, BaseLib.Globals.Path_ProfileManagerErroLog);
                }
            }

            #endregion

        }

        public void MultiPartImageUploadForBackgroundImage(string localImagePath, string authenticity_token, ref string status)
        {
            bool IsLocalFile = true;

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("authenticity_token", authenticity_token);
            //nvc.Add("_method", "PUT");
            nvc.Add("profile_theme", "1");
            nvc.Add("user[profile_default]", "");
            nvc.Add("user[profile_use_background_image]", "true");
            nvc.Add("media_file_name", "image4.jpg");
            //nvc.Add("media_data[]", "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");
            nvc.Add("name[media_empty]", "");
            nvc.Add("name[profile_background_tile]", "1");
            nvc.Add("name[profile_background_color]", "#00000");
            nvc.Add("name[profile_link_color]", "#0084B4");
            HttpUploadFileBackground("https://twitter.com/settings/design", localImagePath, "media_data[]", "", nvc, true, ref status);
        }

        public string postFormData(Uri formActionUrl, string postData, string Referes, string Token, string XRequestedWith, string XPhx, string Origin)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = UserAgent;//"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            

            //gRequest.Accept = "application/json, text/javascript, */*; q=0.01";

            //gRequest.ContentType= "application/x-www-form-urlencoded; charset=UTF-8";

            //gRequest.Headers["Accept-Language"] = "en-US,en;q=0.8";

            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.Timeout = Timeout;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            gRequest.Headers.Add("Javascript-enabled", "true");
            gRequest.Headers.Add("XRequestedWith", "XMLHttpRequest");

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
            }
            if (!string.IsNullOrEmpty(XRequestedWith))
            {
                gRequest.Headers.Add("X-Requested-With", XRequestedWith);
            }
            if (!string.IsNullOrEmpty(XPhx))
            {
                gRequest.Headers.Add("X-PHX", XPhx);
            }
            if (!string.IsNullOrEmpty(Origin))
            {
                gRequest.Headers.Add("Origin", Origin);
            }

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
              //  Console.WriteLine(ex);
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }
            }
            else
            {
                return "Error in posting data";
            }
        }

        public string postFormData1(Uri formActionUrl, string postData, string Referes, string Token, string XRequestedWith, string XPhx, string Origin)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = UserAgent;//"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = "application/json, text/javascript, */*; q=0.01";

            gRequest.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded; charset=UTF-8";
            gRequest.Timeout = Timeout;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


            //gRequest.Headers.Add("Javascript-enabled", "true");

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = "https://twitter.com/settings/profile";
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
            }
            if (!string.IsNullOrEmpty(XRequestedWith))
            {
                gRequest.Headers.Add("X-Requested-With", XRequestedWith);
            }
            if (!string.IsNullOrEmpty(XPhx))
            {
                gRequest.Headers.Add("X-PHX", XPhx);
            }
            if (!string.IsNullOrEmpty(Origin))
            {
                gRequest.Headers.Add("Origin", Origin);
            }

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


                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }

            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataMobileData(Uri formActionUrl, string postData, string Referes, string Token, string XRequestedWith, string XPhx, string Origin, string UserAgent)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);

            gRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:12.0) Gecko/20100101 Firefox/12.0";//"User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16";

            if (!string.IsNullOrEmpty(UserAgent))
            {
                gRequest.UserAgent = UserAgent;
            }

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.Timeout = Timeout;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
            if (!string.IsNullOrEmpty(Token))
            {
                gRequest.Headers.Add("X-CSRFToken", Token);
            }
            if (!string.IsNullOrEmpty(XRequestedWith))
            {
                gRequest.Headers.Add("X-Requested-With", XRequestedWith);
            }
            if (!string.IsNullOrEmpty(XPhx))
            {
                gRequest.Headers.Add("X-PHX", XPhx);
            }
            if (!string.IsNullOrEmpty(Origin))
            {
                gRequest.Headers.Add("Origin", Origin);
            }

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



                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }

            }
            else
            {
                return "Error in posting data";
            }

        }

        public string postFormDataProxy(Uri formActionUrl, string postData, string Referes, string proxyAddress, int port, string proxyUsername, string proxyPassword)
        {

            gRequest = (HttpWebRequest)WebRequest.Create(formActionUrl);
            gRequest.UserAgent = UserAgent;

            gRequest.CookieContainer = new CookieContainer();// gCookiesContainer;
            gRequest.Method = "POST";
            gRequest.Accept = " text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8, */*";
            gRequest.KeepAlive = true;
            gRequest.ContentType = @"application/x-www-form-urlencoded";
            gRequest.Timeout = Timeout;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (!string.IsNullOrEmpty(Referes))
            {
                gRequest.Referer = Referes;
            }
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

                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    return responseString;
                }

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
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ParseJson_DoubleQuotes(string data, string paramName)
        {
            int startIndx = data.IndexOf(paramName) + paramName.Length + 3;
            int endIndx = data.IndexOf("\"", startIndx);

            string value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");
            return value;
        }

        public static string ParseJson(string data, string paramName)
        {
            #region old code
            //string value = string.Empty;
            //int startIndx = data.IndexOf(paramName);
            //if (startIndx > 0)
            //{
            //    int indexstart = startIndx + paramName.Length + 3;
            //    int endIndx = data.IndexOf("\"", startIndx);

            //    value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");

            //    if (value.Contains(paramName))
            //    {
            //        try
            //        {
            //            string[] getOuthentication = System.Text.RegularExpressions.Regex.Split(data, "postAuthenticityToken");
            //            //value = getOuthentication[1].Substring(getOuthentication[1].IndexOf(":") + 2, getOuthentication[1].IndexOf(",\"currentUser") - getOuthentication[1].IndexOf(":") - 3);
            //            string[] authenticity = Regex.Split(getOuthentication[1], ",");
            //            value = authenticity[0].Replace("\"", "").Replace(":", "");
            //        }
            //        catch { };
            //    }

            //    return value;
            //}
            //else
            //{
            //    string[] array = Regex.Split(data , "<input type=\"hidden\"");
            //    foreach (string item in array)
            //    {
            //        if(item.Contains("authenticity_token"))
            //        {
            //            int startindex = item.IndexOf("value=\"");
            //            if (startindex > 0)
            //            {
            //                string start = item.Substring(startindex).Replace("value=\"", "");
            //                int endIndex = start.IndexOf("\"");
            //                string end = start.Substring(0, endIndex);
            //                value = end;
            //                break;
            //            }
            //        }
            //    }

            //}
            //return value; 
            #endregion
            int startIndx = data.IndexOf(paramName) + paramName.Length + 3;
            int endIndx = data.IndexOf("\"", startIndx);

            string value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");
            return value;
        }


        public static string parseText(string item)
        {
            try
            {
                int startIndex = item.IndexOf("text\":\"");
                string StartString = item.Substring(startIndex).Replace("text\":\"", "");
                int endIndex = StartString.IndexOf("\",\"");
                string EndString = StartString.Substring(0, endIndex);

                return EndString;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.WriteStringToTextfile("Error --> Error in parsing tweet text" + ex.Message, BaseLib.Globals.Path_TwtErrorLogs);
                return "";
            }
        }

        public static string ParseEncodedJson(string data, string paramName)
        {
            data = data.Replace("&quot;", "\"");
            int startIndx = data.IndexOf("\"" + paramName + "\"") + ("\"" + paramName + "\"").Length + 1;
            int endIndx = data.IndexOf("\"", startIndx);

            string value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");
            return value;
        }


        public string GetDataWithTagValueByTagAndAttributeName(string pageSrcHtml, string TagName, string AttributeName)
        {
            string dataDescription = string.Empty;
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;

                string dataDescriptionValue = string.Empty;


                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************



                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    dataDescriptionValue = dataDescriptionValue + dataDescription;
                    //    string text = xNode.AccumulateTagContent("text", "script|style");
                    //    lstData.Add(text);

                    //    //** Get Data Under Tag All  Html value * *********************************
                    //    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                    //if (dataDescription.Length > 500)
                    //{
                    //    break;
                    //}
                }
                #endregion
                return dataDescriptionValue;
            }
            catch (Exception)
            {
                return dataDescription = null;

            }
        }

        public List<string> GetTextDataByTagAndAttributeName(string pageSrcHtml, string TagName, string AttributeName)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    string text = xNode.AccumulateTagContent("text", "script|style");
                    lstData.Add(text);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "class", AttributeName);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        }

        public List<string> GetTextDataByTagAndAttributeID(string pageSrcHtml, string TagName, string AttributeName)
        {
            List<string> lstData = new List<string>();
            try
            {
                bool success = false;
                string xHtml = string.Empty;

                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();

                //*** Check DLL working or not **********************
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //** Convert Data Html to XML ******************************************* 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                #region Data Save in list From using XML Tag and Attribut
                string DescriptionMain = string.Empty;
                string dataDescription = string.Empty;

                xBeginSearchAfter = null;

                xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "id", AttributeName);
                while ((xNode != null))
                {
                    //** Get Data Under Tag only Text Value**********************************
                    dataDescription = xNode.GetXml();//.AccumulateTagContent("text", "script|style");

                    string text = xNode.AccumulateTagContent("text", "script|style");
                    lstData.Add(text);

                    //** Get Data Under Tag All  Html value * *********************************
                    //dataDescription = xNode.GetXml();

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForAttribute(xBeginSearchAfter, TagName, "id", AttributeName);
                }
                #endregion
                return lstData;
            }
            catch (Exception)
            {
                return lstData = null;

            }
        }

        public List<string> GetDataTag(string pageSrcHtml, string TagName)
        {
            bool success = false;
            string xHtml = string.Empty;
            List<string> list = new List<string>();

            try
            {


                Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
                success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
                if ((success != true))
                {
                    Console.WriteLine(htmlToXml.LastErrorText);
                    return null;
                }

                htmlToXml.Html = pageSrcHtml;

                //xHtml contain xml data 
                xHtml = htmlToXml.ToXml();

                //******************************************
                Chilkat.Xml xNode = default(Chilkat.Xml);
                Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                Chilkat.Xml xml = new Chilkat.Xml();
                xml.LoadXml(xHtml);

                xBeginSearchAfter = null;
                xNode = xml.SearchForTag(xBeginSearchAfter, TagName);
                while ((xNode != null))
                {

                    string TagText = xNode.AccumulateTagContent("text", "script|style");

                    list.Add(TagText);

                    xBeginSearchAfter = xNode;
                    xNode = xml.SearchForTag(xBeginSearchAfter, TagName);

                }
                //xHtml.
            }
            catch (Exception ex)
            {
                //GlobusLogHelper.log.Error("Error : " + ex.StackTrace);
            }
            return list;
        }
        public string HttpUploadImageFileWithMessage(string url, string file, string paramName, string contentType, NameValueCollection nvc, bool IsLocalFile, ref string status)
        {

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString();


            gRequest = (HttpWebRequest)WebRequest.Create(url);

            gRequest.UserAgent = UserAgent;
            gRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            gRequest.Headers["Accept-Language"] = "en-us,en;q=0.5";

            gRequest.KeepAlive = true;

            gRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            string tempBoundary = boundary + "";//boundary + "\r\n";

            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            byte[] tempBoundarybytes = System.Text.Encoding.ASCII.GetBytes("--" + tempBoundary + "\r\n");

            gRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            //gRequest.ContentType = "multipart/form-data; boundary=" + tempBoundary;
            gRequest.Method = "POST";
            gRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

            gRequest.Referer = "https://twitter.com/";

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            ChangeProxy(proxyAddress, port, proxyUsername, proxyPassword);

            gRequest.CookieContainer = new CookieContainer(); //gCookiesContainer;

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }
            #endregion

            using (Stream rs = gRequest.GetRequestStream())
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                int temp = 0;
                foreach (string key in nvc.Keys)
                {
                    if (key == "media_empty")
                    {
                        continue;
                    }
                    if (temp == 0)
                    {
                        rs.Write(tempBoundarybytes, 0, tempBoundarybytes.Length);
                        temp++;
                    }
                    else
                    {
                        rs.Write(boundarybytes, 0, boundarybytes.Length);
                    }
                    string formitem = string.Format(formdataTemplate, key, nvc[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
                rs.Write(boundarybytes, 0, boundarybytes.Length);



                if (IsLocalFile)
                {
                    string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    string header = string.Format(headerTemplate, "media_empty", "", contentType);
                    byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                    rs.Write(headerbytes, 0, headerbytes.Length);
                }

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
            }

            #region CookieManagment

            if (this.gCookies != null && this.gCookies.Count > 0)
            {
                gRequest.CookieContainer.Add(gCookies);
            }

            #endregion

            WebResponse wresp = null;
            try
            {
                //wresp = gRequest.GetResponse();
                gResponse = (HttpWebResponse)gRequest.GetResponse();
                using (StreamReader reader = new StreamReader(gResponse.GetResponseStream()))
                {
                    string responseString = reader.ReadToEnd();
                    status = "okay";
                    return responseString;
                }
            }
            catch (Exception ex)
            {
                //log.Error("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                return null;
            }
            finally
            {
                gRequest = null;
            }
            //}

        }

        public string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

    }
}
