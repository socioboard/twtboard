using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft;
using System.Text.RegularExpressions;
using BaseLib;
using System.IO;


namespace twtboardpro
{
    public partial class AdvancedSearch : Form
    {
        public AdvancedSearch()
        {
            InitializeComponent();
        }
        GlobusHttpHelper _GlobusHttpHelper = new GlobusHttpHelper();
        string Language = string.Empty;
        string AllOfTheseWords = string.Empty;
        string ThisExtractPhrase = string.Empty;
        string AnyOfTheseWords = string.Empty;
        string NoneOfTheseWords = string.Empty;
        string TheseHashTags = string.Empty;
        string FromTheseAccounts = string.Empty;
        string MentionTheseAccounts = string.Empty;
        string ToTheseAccounts = string.Empty;
        string NearThisPlace = string.Empty;
        string _selectedLanguage = string.Empty;
        public static List<twtboardpro.TwitterDataScrapper.StructTweetIDs> lst_structTweetIDs { get; set; }
        private void cmbWrittenIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ComboBox it = (ComboBox)cmbWrittenIn.SelectedItem;
                // Language= it.ToString();
                Language = cmbWrittenIn.SelectedItem.ToString();
                string[] Languages = Regex.Split(Language, " ");
                Language = Languages[0];
            }
            catch
            { }

            string Lang = "Hindi (हिंदी):hi,English (English):en,Amharic (አማርኛ):am,Arabic (العربية):ar,Bulgarian (Български):bg,Bengali (বাংলা):bg,Tibetan (བོད་སྐད):bo,Cherokee (ᏣᎳᎩ):chr,Danish (Dansk):da,German (Deutsch):de,Maldivian (ދިވެހި):dv,Greek (Ελληνικά):el,Spanish (Español):es,Persian (فارسی):fa,Finnish (Suomi):fi,French (Français):fr,Gujarati (ગુજરાતી):gu,Hebrew (עברית):iw,Hungarian (Magyar):hu,Armenian (Հայերեն):hy,Indonesian (Bahasa Indonesia):in,Icelandic (Íslenska):is,Italian (Italiano):it,Inuktitut (ᐃᓄᒃᑎᑐᑦ):iu,Japanese (日本語):ja,Georgian (ქართული):ka,Khmer (ខ្មែរ):km,Kannada (ಕನ್ನಡ):kn,Korean (한국어):ko,Lao (ລາວ):lo,Lithuanian (Lietuvių):lt,Malayalam (മലയാളം):ml,Myanmar (မြန်မာဘာသာ):my,Nepali (नेपाली):ne,Dutch (Nederlands):nl,Norwegian (Norsk):no,Oriya (ଓଡ଼ିଆ):or,Panjabi (ਪੰਜਾਬੀ):pa,Polish (Polski):pl,Portuguese (Português):pt,Russian (Русский):ru,Sinhala (සිංහල):si,Swedish (Svenska):sv,Tamil (தமிழ்):ta,Telugu (తెలుగు):te,Thai (ไทย):th,Tagalog (Tagalog):tl,Turkish (Türkçe):tr,Urdu (ﺍﺭﺩﻭ):ur,Vietnamese (Tiếng Việt):vi,Chinese (中文):zh";
            string[] Language1 = Regex.Split(Lang, ",");
            foreach (string _lang in Language1)
            {
                if (_lang.Contains(Language))
                {
                    string[] selectedLanguage = Regex.Split(_lang, ":");
                    _selectedLanguage = selectedLanguage[1];
                    _selectedLanguage = "%20lang%3A" + _selectedLanguage;

                }
            }

        }



        private void btnStart_Searching_Click(object sender, EventArgs e)
        {


            AllOfTheseWords = (txtAllofTheseKeywords.Text).ToString();
            ThisExtractPhrase = (txtThisExactPhrase.Text).ToString();
            AnyOfTheseWords = (txtAnyOfTheseWords.Text).ToString();
            TheseHashTags = (txtTheseHashTags.Text).ToString();
            NoneOfTheseWords = (txtNoneofTheseWords.Text).ToString();
            FromTheseAccounts = (txtFromTheseAccounts.Text).ToString();
            ToTheseAccounts = (txtToTheseAccounts.Text).ToString();
            MentionTheseAccounts = (txtMentioningTheseAccounts.Text).ToString();
            NearThisPlace = (txtNearThisPlace.Text).ToString();

            
            AddToLog_AdvancedSearch("[ " + DateTime.Now + " ] => Process Started");

            try
            {
                if (string.IsNullOrEmpty(ThisExtractPhrase))
                {
                    ThisExtractPhrase = "";
                }
                else
                {
                    ThisExtractPhrase = "%20%22" + ThisExtractPhrase;
                }
            }
            catch { }

            try
            {

                if (string.IsNullOrEmpty(AnyOfTheseWords))
                {
                    AnyOfTheseWords = "";
                }
                else
                {
                    AnyOfTheseWords = "%22%20" + AnyOfTheseWords;
                }
            }
            catch
            { }


            try
            {
                if (string.IsNullOrEmpty(TheseHashTags))
                {
                    TheseHashTags = "";
                }
                else
                {
                    TheseHashTags = "%20%23" + TheseHashTags;
                }
            }
            catch
            { }


            try
            {
                if (string.IsNullOrEmpty(NoneOfTheseWords))
                {
                    NoneOfTheseWords = "";
                }
                else
                {
                    NoneOfTheseWords = "%20-" + NoneOfTheseWords;
                }
            }
            catch
            { }


            try
            {
                if (string.IsNullOrEmpty(FromTheseAccounts))
                {
                    FromTheseAccounts = "";
                }
                else
                {
                    FromTheseAccounts = "%20from%3A" + FromTheseAccounts;
                }
            }
            catch
            { }


            try
            {
                if (string.IsNullOrEmpty(ToTheseAccounts))
                {
                    ToTheseAccounts = "";
                }
                else
                {
                    ToTheseAccounts = "%20to%3A" + ToTheseAccounts;
                }
            }
            catch
            { }


            try
            {
                if (string.IsNullOrEmpty(MentionTheseAccounts))
                {
                    MentionTheseAccounts = "";
                }
                else
                {
                    MentionTheseAccounts = "%20%40" + MentionTheseAccounts;
                }
            }
            catch
            { }

            try
            {
                if (string.IsNullOrEmpty(NearThisPlace))
                {
                    NearThisPlace = "";
                }
                else
                {
                    NearThisPlace = "%20near%3A%22" + NearThisPlace;
                }
            }
            catch
            { }




            try
            {
                if (!string.IsNullOrEmpty(txtAllofTheseKeywords.Text))
                {
                    #region Commented
                    //try
                    //{
                    //    string Url = "https://twitter.com/search?f=realtime&q=" + AllOfTheseWords + ThisExtractPhrase + AnyOfTheseWords + NoneOfTheseWords + TheseHashTags + _selectedLanguage + FromTheseAccounts + ToTheseAccounts + MentionTheseAccounts + NearThisPlace + "%22%20within%3A15mi&src=typd";
                    //    string response = _GlobusHttpHelper.getHtmlfromUrl(new Uri(Url), "", "");
                    //}
                    //catch { } public List<StructTweetIDs> NewKeywordStructDataForSearchByKeyword(string keyword) 
                    #endregion
                    {
                        try
                        {
                            BaseLib.GlobusRegex regx = new GlobusRegex();
                           
                            int counter = 0;
                            string res_Get_searchURL = string.Empty;
                            string searchURL = string.Empty;
                            string maxid = string.Empty;
                            string TweetId = string.Empty;
                            string text = string.Empty;

                            string ProfileName = string.Empty;
                            string Location = string.Empty;
                            string Bio = string.Empty;
                            string website = string.Empty;
                            string NoOfTweets = string.Empty;
                            string Followers = string.Empty;
                            string Followings = string.Empty;
                            int noOfRecords = 0;
                            try
                            {
                                noOfRecords = int.Parse(txtNoOfRecords.Text);
                            }
                            catch { }


                        startAgain:


                            if (counter == 0)
                            {
                                searchURL = "https://twitter.com/i/search/timeline?q=" + AllOfTheseWords + ThisExtractPhrase + AnyOfTheseWords + NoneOfTheseWords + TheseHashTags + _selectedLanguage + FromTheseAccounts + ToTheseAccounts + MentionTheseAccounts + NearThisPlace + "%22%20within%3A15mi&src=typd" + "&f=realtime";
                                counter++;
                            }
                            else
                            {

                                searchURL = "https://twitter.com/i/search/timeline?q=" + AllOfTheseWords + ThisExtractPhrase + AnyOfTheseWords + NoneOfTheseWords + TheseHashTags + _selectedLanguage + FromTheseAccounts + ToTheseAccounts + MentionTheseAccounts + NearThisPlace + "%22%20within%3A15mi&src=typd" + "&f=realtime&include_available_features=1&include_entities=1&last_note_ts=0&oldest_unread_id=0&scroll_cursor=" + TweetId + "";
                            }


                            try
                            {
                                res_Get_searchURL = _GlobusHttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                                 AddToLog_AdvancedSearch("[ " + DateTime.Now + " ] => Finding results for entered details ");

                                if (string.IsNullOrEmpty(res_Get_searchURL))
                                {
                                    res_Get_searchURL = _GlobusHttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                                }

                                try
                                {
                                    //string sjss = globushttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");
                                    string[] splitRes = Regex.Split(res_Get_searchURL, "refresh_cursor");
                                    //splitRes = splitRes.Skip(1).ToArray();
                                    foreach (string item in splitRes)
                                    {
                                        if (item.Contains("refresh_cursor"))
                                        {
                                            int startIndex = item.IndexOf("TWEET-");
                                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                            int endIndex = start.IndexOf("\"");
                                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                            TweetId = end;


                                        }
                                        if (item.Contains("scroll_cursor"))
                                        {
                                            int startIndex = item.IndexOf("TWEET-");
                                            string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                            int endIndex = start.IndexOf("\"");
                                            string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                            TweetId = end;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }

                            catch (Exception ex)
                            {
                                System.Threading.Thread.Sleep(2000);
                                res_Get_searchURL = _GlobusHttpHelper.getHtmlfromUrl(new Uri(searchURL), "", "");


                            }
                            // && !res_Get_searchURL.Contains("has_more_items\":false")
                            if (!string.IsNullOrEmpty(res_Get_searchURL))
                            {
                                //string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id"); //Regex.Split(res_Get_searchURL, "\"in_reply_to_status_id_str\"");
                                string[] splitRes = Regex.Split(res_Get_searchURL, "data-item-id");

                                splitRes = splitRes.Skip(1).ToArray();


                                foreach (string item in splitRes)
                                {
                                    if (item.Contains("data-screen-name=") && !item.Contains("js-actionable-user js-profile-popup-actionable"))
                                    {
                                        //var avc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(res_Get_searchURL);
                                        //string DataHtml = (string)avc["items_html"];
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    string modified_Item = "\"from_user\"" + item;

                                    string id = "";
                                    try
                                    {
                                        int startIndex = item.IndexOf("data-user-id=");
                                        string start = item.Substring(startIndex).Replace("data-user-id=\\\"", "");
                                        int endIndex = start.IndexOf("\\\"");
                                        string end = start.Substring(0, endIndex).Replace("id_str", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("}", "").Replace("]", "");
                                        id = end;
                                        //lst_structTweetIDs.Add(id);
                                        AddToLog_AdvancedSearch("[ " + DateTime.Now + " ] => User Id " + id);
                                    }
                                    catch (Exception ex)
                                    {
                                        id = "null";
                                        //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- id -- " + keyword + " --> " + ex.Message, Globals.Path_TwitterDataScrapper);

                                    }

                                    string from_user_id = "";
                                    try
                                    {
                                        int startIndex = item.IndexOf("data-screen-name=\\\"");
                                        string start = item.Substring(startIndex).Replace("data-screen-name=\\\"", "");
                                        int endIndex = start.IndexOf("\\\"");
                                        string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                                        from_user_id = end;
                                        AddToLog_AdvancedSearch("[ " + DateTime.Now + " ] => User ScreenName " + from_user_id);
                                    }
                                    catch (Exception ex)
                                    {
                                        from_user_id = "null";
                                        // Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetPhotoFromUsername() -- " + keyword + " -- from_user_id --> " + ex.Message, Globals.Path_TwitterDataScrapper);

                                    }

                                    string tweetUserid = string.Empty;
                                    try
                                    {
                                        int startIndex = item.IndexOf("=\\\"");
                                        string start = item.Substring(startIndex).Replace("=\\\"", "");
                                        int endIndex = start.IndexOf("\\\"");
                                        string end = start.Substring(0, endIndex).Replace("from_user_id\":", "").Replace("\"", "").Replace(":", "").Replace("{", "").Replace("_str", "").Replace("user", "").Replace("}", "").Replace("]", "");
                                        tweetUserid = end;
                                        AddToLog_AdvancedSearch("[ " + DateTime.Now + " ] => Tweet Id " + tweetUserid);
                                    }
                                    catch (Exception ex)
                                    {
                                        from_user_id = "null";


                                    }

                                    ///Tweet Text 
                                    #region Commented
                                    //try
                                    //{


                                    //    int startindex = item.IndexOf("js-tweet-text tweet-text\"");
                                    //    if (startindex == -1)
                                    //    {
                                    //        startindex = 0;
                                    //        startindex = item.IndexOf("js-tweet-text tweet-text");
                                    //    }

                                    //    string start = item.Substring(startindex).Replace("js-tweet-text tweet-text\"", "").Replace("js-tweet-text tweet-text tweet-text-rtl\"", "");
                                    //    int endindex = start.IndexOf("</p>");

                                    //    if (endindex == -1)
                                    //    {
                                    //        endindex = 0;
                                    //        endindex = start.IndexOf("stream-item-footer");
                                    //    }

                                    //    string end = start.Substring(0, endindex);
                                    //    end = regx.StripTagsRegex(end);
                                    //    text = end.Replace("&nbsp;", "").Replace("a href=", "").Replace("/a", "").Replace("<span", "").Replace("</span", "").Replace("class=\\\"js-display-url\\\"", "").Replace("class=\\\"tco-ellipsis\\\"", "").Replace("class=\\\"invisible\\\"", "").Replace("<strong>", "").Replace("target=\\\"_blank\\\"", "").Replace("class=\\\"twitter-timeline-link\\\"", "").Replace("</strong>", "").Replace("rel=\\\"nofollow\\\" dir=\\\"ltr\\\" data-expanded-url=", "");
                                    //    text = text.Replace("&quot;", "").Replace("<", "").Replace(">", "").Replace("\"", "").Replace("\\", "").Replace("title=", "");

                                    //    string[] array = Regex.Split(text, "http");
                                    //    text = string.Empty;
                                    //    foreach (string itemData in array)
                                    //    {
                                    //        if (!itemData.Contains("t.co"))
                                    //        {
                                    //            string data = string.Empty;
                                    //            if (itemData.Contains("//"))
                                    //            {
                                    //                data = ("http" + itemData).Replace(" span ", string.Empty);
                                    //                if (!text.Contains(itemData.Replace(" ", "")))// && !data.Contains("class") && !text.Contains(data))
                                    //                {
                                    //                    text += data.Replace("u003c", string.Empty).Replace("u003e", string.Empty);
                                    //                }
                                    //            }
                                    //            else
                                    //            {
                                    //                if (!text.Contains(itemData.Replace(" ", "")))
                                    //                {
                                    //                    text += itemData.Replace("u003c", string.Empty).Replace("u003e", string.Empty).Replace("js-tweet-text tweet-text", "");
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //catch { };
                                    
                                    #endregion


                                    twtboardpro.TwitterDataScrapper.StructTweetIDs structTweetIDs = new twtboardpro.TwitterDataScrapper.StructTweetIDs();

                                    if (id != "null")
                                    {
                                        structTweetIDs.ID_Tweet = tweetUserid;
                                        structTweetIDs.ID_Tweet_User = id;
                                        structTweetIDs.username__Tweet_User = from_user_id;
                                        structTweetIDs.wholeTweetMessage = text;
                                        lst_structTweetIDs.Add(structTweetIDs);
                                    }


                                    //if (!File.Exists(Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv"))
                                    //{
                                    //    GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO , LOCATION , WEBSITE , NO OF TWEETS , FOLLOWERS , FOLLOWINGS", Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");
                                    //}

                                    {

                                        ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                                        GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                                        string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + from_user_id), "", "");

                                        string Responce = ProfilePageSource;

                                        #region Convert HTML to XML

                                        string xHtml = objChilkat.ConvertHtmlToXml(Responce);
                                        Chilkat.Xml xml = new Chilkat.Xml();
                                        xml.LoadXml(xHtml);

                                        Chilkat.Xml xNode = default(Chilkat.Xml);
                                        Chilkat.Xml xBeginSearchAfter = default(Chilkat.Xml);
                                        #endregion

                                        int counterdata = 0;
                                        xBeginSearchAfter = null;
                                        string dataDescription = string.Empty;
                                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                                        while ((xNode != null))
                                        {
                                            xBeginSearchAfter = xNode;
                                            if (counterdata == 0)
                                            {
                                                ProfileName = xNode.AccumulateTagContent("text", "script|style");
                                                counterdata++;
                                            }
                                            else if (counterdata == 1)
                                            {
                                                website = xNode.AccumulateTagContent("text", "script|style");
                                                counterdata++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            
                                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "u-textUserColor");
                                        }

                                        xBeginSearchAfter = null;
                                        dataDescription = string.Empty;
                                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "p", "class", "ProfileHeaderCard-bio u-dir");//bio profile-field");
                                        while ((xNode != null))
                                        {
                                            xBeginSearchAfter = xNode;
                                            Bio = xNode.AccumulateTagContent("text", "script|style").Replace("&#39;", "'").Replace("&#13;&#10;", string.Empty).Trim();
                                            break;
                                        }

                                        xBeginSearchAfter = null;
                                        dataDescription = string.Empty;
                                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileHeaderCard-locationText u-dir");//location profile-field");
                                        while ((xNode != null))
                                        {
                                            xBeginSearchAfter = xNode;
                                            Location = xNode.AccumulateTagContent("text", "script|style");
                                            break;
                                        }

                                        int counterData = 0;
                                        xBeginSearchAfter = null;
                                        dataDescription = string.Empty;
                                        xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");//location profile-field");
                                        while ((xNode != null))
                                        {
                                            xBeginSearchAfter = xNode;
                                            if (counterData == 0)
                                            {
                                                // NoOfTweets = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "ProfileNav-value");
                                                NoOfTweets = xNode.AccumulateTagContent("text", "script|style").Replace("Tweets", string.Empty).Replace(",", string.Empty).Replace("Tweet", string.Empty);
                                                counterData++;
                                            }
                                            else if (counterData == 1)
                                            {
                                                Followings = xNode.AccumulateTagContent("text", "script|style").Replace(" Following", string.Empty).Replace(",", string.Empty).Replace("Following", string.Empty);
                                                counterData++;
                                            }
                                            else if (counterData == 2)
                                            {
                                                Followers = xNode.AccumulateTagContent("text", "script|style").Replace("Followers", string.Empty).Replace(",", string.Empty).Replace("Follower", string.Empty);
                                                counterData++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "js-nav");
                                            xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-openSignupDialog js-nonNavigable u-textUserColor");
                                        }


                                        if (!string.IsNullOrEmpty(from_user_id) && tweetUserid != "null")
                                        {
                                            string Id_user = tweetUserid.Replace("}]", string.Empty).Trim();
                                            Globals.lstScrapedUserIDs.Add(Id_user);
                                            // GlobusFileHelper.AppendStringToTextfileNewLine(id + "," + from_user_id + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", ""), Globals.Path_KeywordScrapedListData + "-" + keyword + ".csv");
                                            // Log("[ " + DateTime.Now + " ] => [ " + from_user_id + "," + Id_user + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ]");
                                        }
                                    }


                                    
                                    lst_structTweetIDs = lst_structTweetIDs.Distinct().ToList();

                                    if (lst_structTweetIDs.Count >= noOfRecords)
                                    {
                                       // return lst_structTweetIDs;
                                    }

                                }

                                if (lst_structTweetIDs.Count <= noOfRecords)
                                {
                                    maxid = lst_structTweetIDs[lst_structTweetIDs.Count - 1].ID_Tweet;

                                    if (res_Get_searchURL.Contains("has_moreitems\":false"))
                                    {
                                       
                                    }
                                    else
                                    {
                                        goto startAgain;
                                    }
                                }
                                else
                                {
                                    if (res_Get_searchURL.Contains("has_more_items\":false"))
                                    {
                                        
                                    }
                                    else
                                        goto startAgain;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                   
                }
            }


            catch
            { }


        }

        private void AddToLog_AdvancedSearch(string log)
        {
            try
            {
                if (lstAdvancedsearch.InvokeRequired)
                {
                    lstAdvancedsearch.Invoke(new MethodInvoker(delegate
                    {
                        lstAdvancedsearch.Items.Add(log);
                        lstAdvancedsearch.SelectedIndex = lstAdvancedsearch.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstAdvancedsearch.Items.Add(log);
                    lstAdvancedsearch.SelectedIndex = lstAdvancedsearch.Items.Count - 1;
                }
            }
            catch { }
        }

        void logEvents_Follower_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToLog_AdvancedSearch(eArgs.log);
            }
        }

       
    }

}         
        

    




