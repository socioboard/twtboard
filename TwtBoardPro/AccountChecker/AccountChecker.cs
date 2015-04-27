using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using twtboardpro;
using Globussoft;

namespace AccountChecker
{
    public class AccountChecker
    {

        public static List<string> lst_Usernames = new List<string>();//{ get; set; }
        //ChilkatHttpHelpr chilkatHttpHelper = new ChilkatHttpHelpr();
        Globussoft.GlobusHttpHelper globusHttpHelpr = new Globussoft.GlobusHttpHelper();

        public bool IsUserAlive(string username, out string stats)
        {
            #region MyRegion
            //string isalive = "";
            //try
            //{

            //    string website = "https://twitter.com/";
            //    string url = website + "iamsrk";//username;
            //    string html = chilkatHttpHelper.GetHtml(url);
            //    html = html.ToLower();
            //    if (!html.Contains("Sorry, that page doesn’t exist!") && html.Contains("@" + username))
            //    {
            //        stats = "doesn’t exist";
            //        return true;
            //    }
            //    else if (html.Contains("Sorry, that page doesn’t exist!"))
            //    {
            //        stats = "suspended";
            //        return false;
            //    }
            //    else
            //    {
            //        stats = "exists";
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    stats = string.Empty;
            //    return false;
            //} 
            #endregion

            string isalive = "";
            try
            {
                #region old code
                //username = "iamsrk";
                //Globussoft.GlobusHttpHelper globusHttpHelpr = new Globussoft.GlobusHttpHelper();

                //string website = "https://api.twitter.com/1/users/show.xml?screen_name=";//"https://twitter.com/";
                //if (GlobusRegex.ValidateNumber(username))
                //{
                //    website = "https://api.twitter.com/1/users/show.xml?id=";
                //}

                ////string website = site + username;//"https://twitter.com/";
                //string url = website + username;
                //string html = globusHttpHelpr.getHtmlfromUrl(new Uri(url), "", "");


                //stats = "exists";
                //return true;

                //html = html.ToLower();
                //if (!html.Contains("Sorry, that page doesn’t exist!"))
                //{
                //    stats = "exists";
                //    return true;
                //}
                //else if (html.Contains("Sorry, that page doesn’t exist!"))
                //{
                //    stats = "suspended";
                //    return false;
                //}
                //else
                //{
                //    stats = "doesn’t exist";
                //    return false;
                //} 
                #endregion

                //string website = "https://api.twitter.com/1/users/show.xml?screen_name=";
                string website = "https://twitter.com/";
                //if (GlobusRegex.ValidateNumber(username))
                //{
                //    website = "https://twitter.com/";
                //}
               
                string url = website + username;
                //string html = globusHttpHelpr.getHtmlfromUrl(new Uri(url), "", "");
                twtboardpro.ChilkatHttpHelpr httpHelper = new twtboardpro.ChilkatHttpHelpr();
                string page = httpHelper.GetHtml(url);

                if (page.Contains("suspended"))
                {
                    stats = "suspended";
                    return false;
                }
                else if (page.Contains("Sorry, that page doesn’t exist!"))
                {
                    stats = "Not exsist";
                    return false;
                }
                else if (page.Contains("Rate limit exceeded.Clients may not make more than 150 requests per hour"))
                {
                    stats = "Rate limit exceeded";
                    return false;
                }
                else
                {
                    stats = "exists"; ;
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(404) Not Found"))
                {
                    stats = "404";
                    return false;
                }
                else if (ex.Message.Contains("(403)"))
                {
                    stats = "404";
                    return false;
                }
                stats = string.Empty;
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> AccountChecker -- IsUserAlive() --> " + ex.Message, Globals.Path_AccountCheckerErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> AccountChecker -- IsUserAlive() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                return false;
            }

        }
    }
}
