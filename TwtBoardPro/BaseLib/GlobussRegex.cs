using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;


namespace BaseLib
{
    public class GlobusRegex
    {

        public string GetDataFromString(string HtmlData, string RegexPattern)
        {
            string strData = string.Empty;
            Regex anchorTextExtractor = new Regex(RegexPattern);
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                strData = url.Value;
            }
            return strData;
        }

        public List<string> GetListOfDataFromString(string HtmalData, string RegexPattern)
        {
            List<string> lstData = new List<string>();
            var regex = new Regex(RegexPattern, RegexOptions.Compiled);
            foreach (Match url in regex.Matches(HtmalData))
            {
                lstData.Add(url.Value);
            }
            return lstData;
        }
        //
        public string GetAnchorTag(string HtmlData)
        {
            string AnchorUrl = null;

            Regex anchorTextExtractor = new Regex(@"<a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                AnchorUrl = url.Value;
            }
            return AnchorUrl;
        }

        public List<string> GetAnchorTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();

            Regex anchorTextExtractor = new Regex(@"<a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
        }

        public string StripTagsRegex(string HtmlData)
        {
            return Regex.Replace(HtmlData, "<.*?>", string.Empty);
        }

        //
        public string GetUrlFromString(string HtmlData)
        {
            string strUrl = string.Empty;
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                strUrl = url.Value;
            }
            return strUrl;
        }

        public string GetHttpsUrlFromString(string HtmlData)
        {
            string strUrl = string.Empty;
            var regex = new Regex(@"https://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                strUrl = url.Value;
            }
            return strUrl;
        }

        public List<string> GetUrlsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            foreach (Match url in regex.Matches(ModifiedString))
            {
                lstUrl.Add(url.Value);
            }
            return lstUrl;
        }

        public List<string> GetEmailsFromString(string HtmlData)
        {
            List<string> lstEmail = new List<string>();
            var regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var PageData = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                lstEmail.Add(email.Value);
            }
            return lstEmail;
        }

        public string GetEmailFromString(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.Compiled);
            var PageData = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }

        public string GetH1Tag(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"<h1.*>(.*)</h1>", RegexOptions.Compiled);
            var PageData = HtmlData;//.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }

        public string GetH3Tag(string HtmlData)
        {
            string strEmail = string.Empty;
            var regex = new Regex(@"<h3.*>(.*)</h3>", RegexOptions.Compiled);
            var PageData = HtmlData;//.Replace("\"", " ").Replace("<", " ").Replace(">", " ").Replace(":", " ");
            foreach (Match email in regex.Matches(PageData))
            {
                strEmail = email.Value;
            }
            return strEmail;
        }

        public string GetText(string HtmlData)
        {
            string AnchorUrl = null;

            Regex anchorTextExtractor = new Regex(@"^\d{2}-\d{3}-\d{4}$", RegexOptions.Compiled);
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                AnchorUrl = url.Value;
            }
            return AnchorUrl;
        }

        public List<string> GetInputControlsNameAndValueInPage(string HtmlData)
        {
            List<string> lst = new List<string>();
            string strRegExPatten = "<\\s*input.*?name\\s*=\\s*\"(?<Name>.*?)\".*?value\\s*=\\s*\"(?<Value>.*?)\".*?>";
            Regex reg = new Regex(strRegExPatten, RegexOptions.Multiline);
            MatchCollection mc = reg.Matches(HtmlData);
            string strTemp = string.Empty;
            foreach (Match m in mc)
            {
                strTemp = strTemp + m.Groups["Name"].Value + "=" + m.Groups["Value"].Value + "&";
                lst.Add(strTemp);
            }
            return lst;
        }

        public List<string> GetHrefUrlTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();
            Regex anchorTextExtractor = new Regex(@"href=[""'](?<url>[^""^']+[.]*)[""'].*>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
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

        public string GetHrefUrlTag(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();
            Regex anchorTextExtractor = new Regex(@"href=[""'](?<url>[^""^']+[.]*)");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                return url.Value;
            }
            return null;
        }

        public List<string> GetLIAnchorTags(string HtmlData)
        {
            List<string> lstAnchorUrl = new List<string>();

            Regex anchorTextExtractor = new Regex(@"<li><a.*href=[""'](?<url>[^""^']+[.]*)[""'].*>(?<name>[^<]+[.]*)</a></li>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                lstAnchorUrl.Add(url.Value);
            }
            return lstAnchorUrl;
        }

        public string GetBoldTag(string HtmlData)
        {
            string Tag = string.Empty;
            Regex anchorTextExtractor = new Regex(@"<b\b[^>]*>(.*?)</b>");
            foreach (Match url in anchorTextExtractor.Matches(HtmlData))
            {
                Tag = url.Value;
                return Tag;
            }
            return Tag;
        }

        public static bool ValidateNumber(string inputString)
        {
            Regex IdCheck = new Regex("^[0-9]*$");
            if (!string.IsNullOrEmpty(inputString) && IdCheck.IsMatch(inputString))
            {
                return true;
            }
            return false;
        }

        public string GetAnchorUrl(string Url)
        {
            string url = GetUrlFromString(Url);
            if (!string.IsNullOrEmpty(url))
            {
                string myString = string.Empty;
                Regex r = new Regex("(http://[^ ]+)");
                myString = r.Replace(Url, "<a href=\"$1\" target=\"_blank\">$1</a>");
                string NewString = Url.Replace(Url, myString);
                return NewString;
            }
            else
            {
                return Url;
            }
        }

        /// <summary>
        /// Count Part Of String Occurrences in String
        /// </summary>
        /// <param name="text">String</param>
        /// <param name="pattern">Part Of String</param>
        /// <returns></returns>
        public int CountStringOccurrences(string WholeText, string Pattern)
        {
            try
            {
                int count = 0;
                Regex r = new Regex(Pattern, RegexOptions.IgnoreCase);
                Match m = r.Match(WholeText);
                while (m.Success)
                {
                    count++;
                }
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int CountExactStringMatches(string WholeText, string Pattern)
        {
            Pattern = "\b" + Pattern.ToLower() + "\b";
            MatchCollection mc = Regex.Matches(WholeText.ToLower(), Pattern);
            return mc.Count;
        }
    }
}
