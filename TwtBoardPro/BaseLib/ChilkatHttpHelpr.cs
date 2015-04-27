using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Windows.Forms;

namespace BaseLib
{
    public class ChilkatHttpHelpr
    {
        public string proxyAddress = string.Empty;
        public string proxyPort = string.Empty;
        public string proxyUsername = string.Empty;
        public string proxyPassword = string.Empty;


        ///Chilkat Http object...
        public Chilkat.Http http = new Chilkat.Http();

        ///Chilkat Http Request to be used in Http Post...
        Chilkat.HttpRequest req = new Chilkat.HttpRequest();

        private Dictionary<string, string> postDataDictionary = new Dictionary<string, string>();


        public void SetHttp(ref Chilkat.Http http)
        {
            this.http = http;
        }

        public string ConvertHtmlToXml(string PageSrcHtml)
        {
            ////  Convert the HTML to XML:
            bool success = false;
            string xHtml = string.Empty;

            Chilkat.HtmlToXml htmlToXml = new Chilkat.HtmlToXml();
            success = htmlToXml.UnlockComponent("THEBACHtmlToXml_7WY3A57sZH3O");
            if ((success != true))
            {
                Console.WriteLine(htmlToXml.LastErrorText);
                return null;
            }

            htmlToXml.Html = PageSrcHtml;

            //xHtml contain xml data 
            xHtml = htmlToXml.ToXml();

            Chilkat.Xml xml = new Chilkat.Xml();
            xml.LoadXml(xHtml);
            //xHtml.
            return xHtml;
        }

        private void ChangeProxy()
        {
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                http.ProxyDomain = proxyAddress;
                if (!string.IsNullOrEmpty(proxyPort))
                {
                    Regex IdCheck = new Regex("^[0-9]*$");

                    if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
                    {
                        http.ProxyPort = int.Parse(proxyPort);
                    }
                    else
                    {
                        proxyPort = "80";
                        http.ProxyPort = int.Parse(proxyPort);
                    }
                }
                //http.ProxyPort = int.Parse(proxyPort);
            }
            if (!string.IsNullOrEmpty(proxyUsername))
            {
                http.ProxyLogin = proxyUsername;
                http.ProxyPassword = proxyPassword;
            }
        }

        public string GetHtml(string URL)
        {
            string response = string.Empty;

            ChangeProxy();

            if (!http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06"))
            {
            }

            ///Save Cookies...
            http.CookieDir = "memory";
            //http.CookieDir = Application.StartupPath + "\\cookies";
            http.SendCookies = true;
            http.SaveCookies = true;

            http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
            http.SetRequestHeader("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            http.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24");
            http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            //http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            http.SetRequestHeader("Connection", "keep-alive");

            http.AllowGzip = true;

            response = http.QuickGetStr(URL);

            return response;
        }

        public string GetHtml(string URL, ref Chilkat.Http http)
        {
            string response = string.Empty;

            ChangeProxy();

            if (!http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06"))
            {
            }

            ///Save Cookies...
            http.CookieDir = "memory";
            //http.CookieDir = Application.StartupPath + "\\cookies";
            http.SendCookies = true;
            http.SaveCookies = true;

            http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
            http.SetRequestHeader("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            http.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24");
            http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            //http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            //http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            http.SetRequestHeader("Connection", "keep-alive");

            http.AllowGzip = true;

            response = http.QuickGetStr(URL);

            return response;
        }

        public string GetHtmlProxy(string URL, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            string response = string.Empty;

            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;

            ChangeProxy();

            if (!http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06"))
            {
            }

            ///Save Cookies...
            http.CookieDir = "memory";
            http.SendCookies = true;
            http.SaveCookies = true;

            http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
            http.SetRequestHeader("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            http.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24");
            http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            response = http.QuickGetStr(URL);

            return response;
        }

        public string GetHtmlProxy(string URL, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, ref Chilkat.Http http)
        {
            string response = string.Empty;

            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;

            ChangeProxy();

            if (!http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06"))
            {
            }

            ///Save Cookies...
            http.CookieDir = "memory";
            http.SendCookies = true;
            http.SaveCookies = true;

            http.SetRequestHeader("Accept-Encoding", "gzip,deflate");
            http.SetRequestHeader("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            http.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.24) Gecko/20111103 Firefox/3.6.24");
            http.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            response = http.QuickGetStr(URL);

            return response;
        }

        public void SetCookie(string rawCookieStr, ref Chilkat.Http http)
        {
            http.SetRequestHeader("Cookie", rawCookieStr); 
        }

        public string PostData(string URL, string postData, string referer)
        {
            string response = string.Empty;

            ChangeProxy();

            //http.SetRequestHeader("Cookie", "PREF=ID=39a7dcb4769a70b5:U=01e856263f78e316:FF=0:TM=1310809062:LM=1311756586:GM=1:S=qiyaqpZc0QJwSHyD;NID=49=Ljnrtc5KZDOFfgRVn1Tt6G6MdaeISmX4vOzE7MSbouPD_4ze6OoSuCMWXlH0Jy7fnAlEYzdYxs4V7JP2DXnKgDxVQMKYY60yWoeCgFIwTL2WWBfmxJZNml5pYdudn5Zw;GAPS=1:tS3_AUBl7WpcKSXAXYMVdDiWuMdX0Q:rAR7mbwZ6PlPnJt-;GALX=7iQKVcsEdN4;GoogleAccountsLocale_session=en_GB;SID=DQAAAIUAAAD1r3xd8mtKrnHFMI4AdB1fuFDSG6YQ98V-_olltRKYgRSHMAgKC9eU88xa6oIay8EgmaV8PRZl1uxi_Q7Hx3etXAUCd42mHJph5YHU015yCzmHUoGdeVpzpFtvPc4xFpnu1Hl2PqXSN4tIlDAY_Qg6vs8I8eoWpiVPPXkLR1WaKcw54W9s1Yx1PxN7C5Nazmc;LSID=blogger|s.IN:DQAAAIYAAACEkfAmZiRHYaawY6Ol0Oguk5C58mprkWFDmf4StK2d_UZDRC6baudw3SoGKkQGBEsB4r2wRrklsK9O1XChX7Pj-UnTPMaDxSz4ADM5O69S-LQhLJx10-1kdeomZSC2NIOWxIpHhoWn1FPrpQrFDgisVLfVsHsjD7l5ASyaLzgzaIxx-XZfuQHaCRgnMRF09vo;GAUSR=lachelle.longenecker.jvqq@hotmail.com;HSID=AuAY8LSZuG828am_4;SSID=AYAPgLxKQx6efsNxV;APISID=j799BJ0rwj5vK8S3/Aid730sB7T-6RWKbq;SAPISID=BnxZXhygC3cPGeSV/AVHrAqmtZgl16WiOi");

            req.RemoveAllParams();

            req.UsePost();
            //req.Path = "/login.php?login_attempt=1";

            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16");
            req.AddHeader("Accept-Encoding", "gzip,deflate");
            req.AddHeader("Accept-Charset", "ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            //req.AddHeader("Cookie", "PREF=ID=39a7dcb4769a70b5:U=01e856263f78e316:FF=0:TM=1310809062:LM=1311756586:GM=1:S=qiyaqpZc0QJwSHyD;NID=49=Ljnrtc5KZDOFfgRVn1Tt6G6MdaeISmX4vOzE7MSbouPD_4ze6OoSuCMWXlH0Jy7fnAlEYzdYxs4V7JP2DXnKgDxVQMKYY60yWoeCgFIwTL2WWBfmxJZNml5pYdudn5Zw;GAPS=1:tS3_AUBl7WpcKSXAXYMVdDiWuMdX0Q:rAR7mbwZ6PlPnJt-;GALX=7iQKVcsEdN4;GoogleAccountsLocale_session=en_GB;SID=DQAAAIUAAAD1r3xd8mtKrnHFMI4AdB1fuFDSG6YQ98V-_olltRKYgRSHMAgKC9eU88xa6oIay8EgmaV8PRZl1uxi_Q7Hx3etXAUCd42mHJph5YHU015yCzmHUoGdeVpzpFtvPc4xFpnu1Hl2PqXSN4tIlDAY_Qg6vs8I8eoWpiVPPXkLR1WaKcw54W9s1Yx1PxN7C5Nazmc;LSID=blogger|s.IN:DQAAAIYAAACEkfAmZiRHYaawY6Ol0Oguk5C58mprkWFDmf4StK2d_UZDRC6baudw3SoGKkQGBEsB4r2wRrklsK9O1XChX7Pj-UnTPMaDxSz4ADM5O69S-LQhLJx10-1kdeomZSC2NIOWxIpHhoWn1FPrpQrFDgisVLfVsHsjD7l5ASyaLzgzaIxx-XZfuQHaCRgnMRF09vo;GAUSR=lachelle.longenecker.jvqq@hotmail.com;HSID=AuAY8LSZuG828am_4;SSID=AYAPgLxKQx6efsNxV;APISID=j799BJ0rwj5vK8S3/Aid730sB7T-6RWKbq;SAPISID=BnxZXhygC3cPGeSV/AVHrAqmtZgl16WiOi");

            #region PostData

            string[] arrPostData = Regex.Split(postData, "&");

            postDataDictionary.Clear();

            foreach (string item in arrPostData)
            {
                try
                {
                    postDataDictionary.Add(item.Split('=')[0], item.Split('=')[1]);
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in postDataDictionary)
            {
                req.AddParam(item.Key, item.Value);
            }
            
            #endregion

            ///Set Referer
            if (!string.IsNullOrEmpty(referer))
            {
                req.AddHeader("Referer", referer); 
            }

            Chilkat.HttpResponse respUsingPostURLEncoded = http.PostUrlEncoded(URL, req);
            string ResponseLoginPostURLEncoded = respUsingPostURLEncoded.BodyStr;

            response = ResponseLoginPostURLEncoded;

            return response;
        }



    }
}
