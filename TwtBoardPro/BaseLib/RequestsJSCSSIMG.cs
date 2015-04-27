using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Net;
using Globussoft;

namespace BaseLib
{
    public class RequestsJSCSSIMG
    {
        public static string proxyAddress = string.Empty;
        public static string proxyPort = string.Empty;
        public static string proxyUsername = string.Empty;
        public static string proxyPassword = string.Empty;

        public static void SetProxy(string proxyAddrss, string proxyPrt, string proxyUsrname, string proxyPasswrd)
        {
            ///Set Proxy
            proxyAddress = proxyAddrss;
            proxyPort = proxyPrt;
            proxyUsername = proxyUsrname;
            proxyPassword = proxyPasswrd;
        }

        public static void RequestJSCSSIMG(string pageSource, ref GlobusHttpHelper HttpHelper)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<string> listURLs = new List<string>();

            try
            {
                //CSS Request
                foreach (string item in GetHrefsFromString(pageSource))
                {
                    if (item.Contains(".css"))
                    {
                        string cssSource = item.Replace(" ", "").Trim();
                        try
                        {
                            //string res = HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                            listURLs.Add(cssSource);
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(500);
                            try
                            {
                                string res = HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                //JS Request
                string[] scriptArr = Regex.Split(pageSource, "/script>");
                foreach (string item in scriptArr)
                {
                    try
                    {
                        if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                        {
                            int startIndx = item.LastIndexOf("src=") + "src=".Length + 1;
                            int endIndx = item.IndexOf(">", startIndx) - 1;
                            string jsSource = item.Substring(startIndx, endIndx - startIndx);
                            //if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                            if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak.") || jsSource.StartsWith("http://profile.ak.") || jsSource.StartsWith("https://profile.ak.") || jsSource.StartsWith("http://s-profile.ak.") || jsSource.StartsWith("https://s-profile.ak."))
                            {
                                try
                                {
                                    //string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                                    listURLs.Add(jsSource);
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(500);
                                    try
                                    {
                                        string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                string[] moreScriptArray = Regex.Split(pageSource, "\"src\":");
                foreach (string item in moreScriptArray)
                {
                    try
                    {
                        int startIndx = 1;
                        int endIndx = item.IndexOf("\"", startIndx);
                        string jsSource = item.Substring(startIndx, endIndx - startIndx).Replace("\\", "");
                        if (jsSource.Contains(".js"))
                        {
                            //string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource)); 
                            listURLs.Add(jsSource);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                ///IMG Request
                string[] imageArr = Regex.Split(pageSource, "<img");
                foreach (string item in imageArr)
                {
                    try
                    {
                        if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                        {
                            int startIndx = item.IndexOf("src=") + "src=".Length + 1;
                            int endIndx = item.IndexOf("\"", startIndx + 1);
                            string jsSource = item.Substring(startIndx, endIndx - startIndx);
                            //if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                            if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak.") || jsSource.StartsWith("http://profile.ak.") || jsSource.StartsWith("https://profile.ak.") || jsSource.StartsWith("http://s-profile.ak.") || jsSource.StartsWith("https://s-profile.ak."))
                            {
                                if (jsSource.Contains(".png") || jsSource.Contains(".gif") || jsSource.Contains(".jpg") || jsSource.Contains(".jpeg"))
                                {
                                    try
                                    {
                                        //string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                                        listURLs.Add(jsSource);
                                    }
                                    catch (Exception)
                                    {
                                        Thread.Sleep(500);
                                        try
                                        {
                                            string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                listURLs = listURLs.Distinct().ToList();
                foreach (string item in listURLs)
                {
                    try
                    {
                        string res = HttpHelper.getHtmlfromUrl(new Uri(item));
                    }
                    catch { };
                }

            }
            catch { };
        }

        public static void RequestJSCSSIMG(string pageSource, ref Chilkat.Http http)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            List<string> listURLs = new List<string>();

            ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
           

            //CSS Request
            foreach (string item in GetHrefsFromString(pageSource))
            {
                if (item.Contains(".css"))
                {
                    string cssSource = item.Replace(" ", "").Trim();
                    try
                    {
                        //string res = HttpHelper.GetHtmlProxy(cssSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                        listURLs.Add(cssSource);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            string res = HttpHelper.GetHtmlProxy(cssSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            //JS Request
            string[] scriptArr = Regex.Split(pageSource, "/script>");
            foreach (string item in scriptArr)
            {
                try
                {
                    if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                    {
                        int startIndx = item.LastIndexOf("src=") + "src=".Length + 1;
                        int endIndx = item.IndexOf(">", startIndx) - 1;
                        string jsSource = item.Substring(startIndx, endIndx - startIndx);
                        //if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                        if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak.") || jsSource.StartsWith("http://profile.ak.") || jsSource.StartsWith("https://profile.ak.") || jsSource.StartsWith("http://s-profile.ak.") || jsSource.StartsWith("https://s-profile.ak."))
                        {
                            try
                            {
                                //string res = HttpHelper.GetHtmlProxy(jsSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                                listURLs.Add(jsSource);
                            }
                            catch (Exception)
                            {
                                Thread.Sleep(500);
                                try
                                {
                                    string res = HttpHelper.GetHtmlProxy(jsSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            string[] moreScriptArray = Regex.Split(pageSource, "\"src\":");
            foreach (string item in moreScriptArray)
            {
                try
                {
                    int startIndx = 1;
                    int endIndx = item.IndexOf("\"", startIndx);
                    string jsSource = item.Substring(startIndx, endIndx - startIndx).Replace("\\", "");
                    if (jsSource.Contains(".js"))
                    {
                        //string res = HttpHelper.GetHtmlProxy(jsSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                        listURLs.Add(jsSource);
                    }
                }
                catch (Exception)
                {
                }
            }

            ///IMG Request
            string[] imageArr = Regex.Split(pageSource, "<img");
            foreach (string item in imageArr)
            {
                try
                {
                    if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                    {
                        int startIndx = item.IndexOf("src=") + "src=".Length + 1;
                        int endIndx = item.IndexOf("\"", startIndx + 1);
                        string jsSource = item.Substring(startIndx, endIndx - startIndx);
                        //if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                        if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak.") || jsSource.StartsWith("http://profile.ak.") || jsSource.StartsWith("https://profile.ak.") || jsSource.StartsWith("http://s-profile.ak.") || jsSource.StartsWith("https://s-profile.ak."))
                        {
                            if (jsSource.Contains(".png") || jsSource.Contains(".gif") || jsSource.Contains(".jpg") || jsSource.Contains(".jpeg"))
                            {
                                try
                                {
                                    //string res = HttpHelper.GetHtmlProxy(jsSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                                    listURLs.Add(jsSource);
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(500);
                                    try
                                    {
                                        string res = HttpHelper.GetHtmlProxy(jsSource, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);// HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                catch { };
            }

            listURLs = listURLs.Distinct().ToList();
            foreach (string item in listURLs)
            {
                try
                {
                    string res = HttpHelper.GetHtmlProxy(item, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref http);
                }
                catch { };
            }

        }

        public static List<string> GetHrefsFromString(string HtmlData)
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
    }
}
