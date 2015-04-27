using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class LogOutAccount
    {

        public bool LogoutChilkat(ref Chilkat.Http http)
        {


            try
            {
                string pageSource1 = http.QuickGetStr("http://www.facebook.com/");

                if (pageSource1.Contains("\"h\"") && pageSource1.Contains("post_form_id") && pageSource1.Contains("fb_dtsg"))
                {
                    string h = string.Empty;
                    string post_form_id = string.Empty;
                    string fb_dtsg = string.Empty;

                    if (pageSource1.Contains("\"h\""))
                    {
                        string strTemp = pageSource1.Substring(pageSource1.IndexOf("\"h\""), 200);
                        string[] ArrTemp = strTemp.Split('"');
                        h = ArrTemp[3];
                    }
                    if (pageSource1.Contains("post_form_id") && pageSource1.Contains("fb_dtsg"))
                    {
                        string strTemp = pageSource1.Substring(pageSource1.IndexOf("post_form_id"), 200);
                        string[] ArrTemp = strTemp.Split('"');
                        post_form_id = ArrTemp[2];
                        fb_dtsg = ArrTemp[6];
                    }

                    Chilkat.HttpRequest reqLogout = new Chilkat.HttpRequest();
                    reqLogout.UsePost();
                    //req.Path = "/login.php?login_attempt=1";
                    reqLogout.RemoveAllParams();
                    reqLogout.AddHeader("User-Agent", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.16) Gecko/20110319 Firefox/3.6.16");
                    //req.SetFromUrl("http://www.facebook.com/login.php?login_attempt=1");

                    reqLogout.AddParam("post_form_id", post_form_id);
                    reqLogout.AddParam("fb_dtsg", fb_dtsg);
                    reqLogout.AddParam("ref", "mb");
                    reqLogout.AddParam("h", h);

                    Chilkat.HttpResponse respUsingPostURLEncoded = http.PostUrlEncoded("http://www.facebook.com/logout.php", reqLogout);
                    string ResponseLoginPostURLEncoded = respUsingPostURLEncoded.BodyStr;

                    string pageSource12 = http.QuickGetStr("http://www.facebook.com/");


                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                 return false;
                Console.WriteLine(ex.Message);
            }

        }

        public bool LogoutHttpHelper(ref Globussoft.GlobusHttpHelper HttpHelper)
        {
            try
            {
                string pageSource1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

                if (pageSource1.Contains("\"h\"") && pageSource1.Contains("post_form_id") && pageSource1.Contains("fb_dtsg"))
                {
                    string h = string.Empty;
                    string post_form_id = string.Empty;
                    string fb_dtsg = string.Empty;

                    if (pageSource1.Contains("\"h\""))
                    {
                        string strTemp = pageSource1.Substring(pageSource1.IndexOf("\"h\""), 200);
                        string[] ArrTemp = strTemp.Split('"');
                        h = ArrTemp[3];
                    }
                    if (pageSource1.Contains("post_form_id") && pageSource1.Contains("fb_dtsg"))
                    {
                        string strTemp = pageSource1.Substring(pageSource1.IndexOf("post_form_id"), 200);
                        string[] ArrTemp = strTemp.Split('"');
                        post_form_id = ArrTemp[2];
                        fb_dtsg = ArrTemp[6];

                    }

                    string ResponseLogout = HttpHelper.postFormData(new Uri("http://www.facebook.com/logout.php"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&ref=mb&h=" + h);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                Console.WriteLine(ex.Message);
            }

        }
    }
}
