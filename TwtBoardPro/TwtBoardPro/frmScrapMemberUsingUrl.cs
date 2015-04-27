using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using BaseLib;
using System.Text.RegularExpressions;
using Globussoft;
using System.IO;

namespace twtboardpro
{
    public partial class frmScrapMemberUsingUrl : Form
    {
        private System.Drawing.Image image;
        List<string> lstUrl = new List<string>();
        bool CheckNetConn = false;
        Thread thread_StartMemberScrape = null;
        int noOfRecords = 0;
        public List<Thread> lstIsStopScrapMember= new List<Thread>();
        Globussoft.GlobusHttpHelper globushttpHelper = new Globussoft.GlobusHttpHelper();
        public struct StructTweetIDs
        {
            public string ID_Tweet { get; set; }

            public string ID_Tweet_User { get; set; }

            public string username__Tweet_User { get; set; }

            public string wholeTweetMessage { get; set; }
        }

        public List<StructTweetIDs> lst_structTweetIDs { get; set; }

        public frmScrapMemberUsingUrl()
        {
            InitializeComponent();
        }

        private void frmScrapMemberUsingUrl_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
        }

        private void frmScrapMemberUsingUrl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btnUploadScrapMember_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtUploadMemberUsingUrl.Text = ofd.FileName;
                        //Globals.EmailsFilePath = ofd.FileName;
                        lstUrl = new List<string>();
                        lstUrl = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);

                       
                    }
                    else
                    {
                        lstUrl = new List<string>();
                        txtUploadMemberUsingUrl.Text = string.Empty;
                        //AddToLog_Follower(listMessage.Count + " User IDs to Follow loaded");
                    }
                }

                //if ((!chkUserInfo.Checked))
                {
                    if (txtUploadMemberUsingUrl.Text.Contains("/"))
                    {
                        AddToLog_ScrapMember(lstUrl.Count + " Url loaded");
                    }
                    else
                    {
                        AddToLog_ScrapMember(lstUrl.Count + " Username loaded");
                    }
                }
            }
            catch { }
        }

        private void btnScrapeMember_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (lstUrl.Count > 0)
                    {
                        if (!chkUserInfo.Checked)
                        {
                            thread_StartMemberScrape = new Thread(() =>
                            {
                                ScrapeMemberSeacrh();
                            });
                            thread_StartMemberScrape.Start();
                        }
                        else
                        {
                            thread_StartMemberScrape = new Thread(() =>
                            {
                                ScrapeMemberSeacrh();
                            });
                            thread_StartMemberScrape.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please input Url");
                    }
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> btnScrapeMember_Click() --> " + ex.Message, Globals.Path_ScrapeUsersErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> btnScrapeMember_Click() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }
            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection... ]");
            }
        }


        private void ScrapeMemberSeacrh()
        {
            try
            {
                lstIsStopScrapMember.Add(Thread.CurrentThread);
                lstIsStopScrapMember.Distinct();
                Thread.CurrentThread.IsBackground = true;

            }
            catch { }

            try
            {
                TwitterDataScrapper TweetData = new TwitterDataScrapper();

                txtlimitScrapeUsers.Invoke(new MethodInvoker(delegate
                    {

                        if (!string.IsNullOrEmpty(txtlimitScrapeUsers.Text.Trim()) && NumberHelper.ValidateNumber(txtlimitScrapeUsers.Text.Trim()))
                        {
                            noOfRecords = Convert.ToInt32(txtlimitScrapeUsers.Text.Trim());
                            if (noOfRecords == 0)
                            {
                                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Do not put Zero value ]");
                                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Default number of records is 50 ]");
                                noOfRecords = 50;
                            }
                        }
                        else
                        {
                            AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ please enter value in number of users ]");
                            return;
                        }
                    }));

                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Scraping User INfo ]");

                if (!chkUserInfo.Checked)
                    foreach (string item in lstUrl)
                    {
                        if (!item.Contains("https://twitter.com"))
                        {
                            AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [  You have entered invalid URL   " + item + "]");
                            continue;
                        }

                        List<string> lstMember = new List<string>();
                        string returnStatus = string.Empty;

                        lstMember = GetMembers(item, out returnStatus);
                        AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Scraped Member for Url " + item + "]");
                    }

                else 
                {
                    foreach (string item in lstUrl)
                    {
                        Thread scrapinfo = new Thread(scrapUserInfo);
                        scrapinfo.Start(new object[] { item });
                        Thread.Sleep(200);
                    }
                }

                //AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ process completed ]");
            }
            catch { }
        }


        public void scrapUserInfo(object param)
        {
            try
            {
                Array paramsArray = new object[1];

                paramsArray = (Array)param;
                string UserName = (string)paramsArray.GetValue(0);

                string userId = string.Empty;
                string ProfileName = string.Empty;
                string Location = string.Empty;
                string Bio = string.Empty;
                string website = string.Empty;
                string NoOfTweets = string.Empty;
                string Followers = string.Empty;
                string Followings = string.Empty;
                string IsProfilePIc = string.Empty;

                ChilkatHttpHelpr objChilkat = new ChilkatHttpHelpr();
                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
                string ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + UserName.Trim()), "", "");

                if (string.IsNullOrEmpty(ProfilePageSource))
                {
                    ProfilePageSource = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + UserName.Trim()), "", "");
                }
                if (string.IsNullOrEmpty(ProfilePageSource))
                {
                    AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ User  " + UserName + " is not exist or page source getting null.]");
                    return;
                }

                if (ProfilePageSource.Contains("Account suspended"))
                {
                    AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ User  " + UserName + " is suspended ]");
                    return;
                }

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
                //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
                xNode = xml.SearchForAttribute(xBeginSearchAfter, "h1", "class", "ProfileHeaderCard-name");
                while ((xNode != null))
                {
                    xBeginSearchAfter = xNode;
                    if (counterdata == 0)
                    {
                        ProfileName = xNode.AccumulateTagContent("text", "script|style");
                        if (ProfileName.Contains("Verified account"))
                        {
                            ProfileName = ProfileName.Replace("Verified account", " ");
                        }
                        counterdata++;
                    }
                    else if (counterdata == 1)
                    {
                        website = xNode.AccumulateTagContent("text", "script|style");
                        if (website.Contains("Twitter Status"))
                        {
                            website = "";
                        }
                        counterdata++;
                    }
                    else
                    {
                        break;
                    }
                    //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "profile-field");
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
                //xNode = xml.SearchForAttribute(xBeginSearchAfter, "span", "class", "location profile-field");
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
                //xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "data-element-term", "tweet_stats");
                xNode = xml.SearchForAttribute(xBeginSearchAfter, "a", "class", "ProfileNav-stat ProfileNav-stat--link u-borderUserColor u-textCenter js-tooltip js-nav");
                while ((xNode != null))
                {
                    xBeginSearchAfter = xNode;
                    if (counterData == 0)
                    {
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

                try
                {
                    int startindex = ProfilePageSource.IndexOf("profile_id");
                    string start = ProfilePageSource.Substring(startindex).Replace("profile_id", "");
                    int endindex = start.IndexOf(",");
                    string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Trim();
                    userId = end.Trim();
                    if (userId.Length > 15)
                    {
                        startindex = ProfilePageSource.IndexOf("profile_id&quot");
                        start = ProfilePageSource.Substring(startindex).Replace("profile_id&quot", "");
                        endindex = start.IndexOf(",");
                        end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":", "").Replace(";", "").Trim();
                        userId = end.Trim();
                    }
                }
                catch { }

                if (ProfilePageSource.Contains("default_profile_6_400x400") || ProfilePageSource.Contains("default_profile_5_400x400") || ProfilePageSource.Contains("default_profile_4_400x400") || ProfilePageSource.Contains("default_profile_3_400x400") || ProfilePageSource.Contains("default_profile_2_400x400") || ProfilePageSource.Contains("default_profile_1_400x400") || ProfilePageSource.Contains("default_profile_0_400x400"))
                {
                    IsProfilePIc = "No";
                }
                else
                {
                    IsProfilePIc = "Yes";
                }
                if (!File.Exists(Globals.Path_UserListInfoData))
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine("USERID , USERNAME , PROFILE NAME , BIO , LOCATION , WEBSITE , NO OF TWEETS , FOLLOWERS , FOLLOWINGS, ProfilePic", Globals.Path_UserListInfoData);
                }
                if (!string.IsNullOrEmpty(UserName))
                {
                    //string Id_user = item.ID_Tweet_User.Replace("}]", string.Empty).Trim();
                    //Globals.lstScrapedUserIDs.Add(Id_user);
                    GlobusFileHelper.AppendStringToTextfileNewLine(userId + "," + UserName + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location.Replace(",", "") + "," + website + "," + NoOfTweets.Replace(",", "").Replace("Tweets", "") + "," + Followers.Replace(",", "").Replace("Following", "") + "," + Followings.Replace(",", "").Replace("Followers", "").Replace("Follower", "") + "," + IsProfilePIc, Globals.Path_UserListInfoData);
                    AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ " + userId + "," + UserName + "," + ProfileName + "," + Bio.Replace(",", "") + "," + Location + "," + website + "," + NoOfTweets + "," + Followers + "," + Followings + " ," + IsProfilePIc + "]");
                }
            }
            catch { }
        }
        
        private void BtnStopScrapMember_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Thread item in lstIsStopScrapMember)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                Thread.Sleep(1000);

                AddToLog_ScrapMember("------------------------------------------------------------------------------------------------------------------------------------");
                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ PROCESS STOPPED ]");
                AddToLog_ScrapMember("------------------------------------------------------------------------------------------------------------------------------------");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error >>> " + ex.StackTrace);
            }
        }

        public List<string> GetMembers(string keyword,out string ReturnStatus)
        {
            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string userID;
            string Screen_name;
            int counter = 0;
            try
            {

                Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
                
            StartAgain:
                if (counter == 0)
                {
                    FollowingUrl = keyword;
                    counter++;
                }
                else
                {
                    FollowingUrl = keyword + "/timeline?cursor=" + cursor + "&cursor_index=&cursor_offset=&include_available_features=1&include_entities=1&is_forward=true";
                }


                String DataCursor = string.Empty;


                string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                if (string.IsNullOrEmpty(Data))
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                }

                if (string.IsNullOrEmpty(Data))
                {
                    AddToLog_ScrapMember("Either Url in Invalid or PageSource is getting Null or Empty.");

                    ReturnStatus = "Error";
                    return lstIds;
                }
                String DataCursor1 = string.Empty;

                if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {

                    String[] DataDivArr;
                    if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-item stream-item stream-item");
                    }
                    else
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-item");
                    }

                    foreach (var DataDivArr_item in DataDivArr)
                    {
                        if (DataDivArr_item.Contains("data-cursor"))
                        {
                            String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
                            DataCursor1 = DataCurso.Substring(DataCurso.IndexOf("="), DataCurso.IndexOf(">")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("=", string.Empty).Trim();
                        }
                        if (DataDivArr_item.Contains("<!DOCTYPE html>") || DataDivArr_item.Contains("cursor"))
                        {
                            continue;
                        }

                        if (DataDivArr_item.Contains("data-screen-name") && DataDivArr_item.Contains(" data-user-id"))
                        {
                            int endIndex = 0;
                            int startIndex = DataDivArr_item.IndexOf("data-screen-name");
                            try
                            {
                                endIndex = DataDivArr_item.IndexOf(">");
                            }
                            catch { }

                            if (endIndex == -1)
                            {
                                endIndex = DataDivArr_item.IndexOf("data-feedback-token");
                            }

                            string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);

                            string _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-feedback-token", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                            string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-user-id", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());

                            if (noOfRecords > lstIds.Count)
                            {
                                lstIds.Add(_SCRName + ":" + _SCRNameID);
                                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [" + _SCRNameID + " :: "+_SCRName+" ]");
                                if (!File.Exists(Globals.Path_ScrapedMembersList))
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine("UserID , UserName , Url", Globals.Path_ScrapedMembersList);
                                }
                                GlobusFileHelper.AppendStringToTextfileNewLine(_SCRNameID + "," + _SCRName + "," + keyword, Globals.Path_ScrapedMembersList);

                            }
                            

                        }

                    }


                    if (noOfRecords != lstIds.Count)
                    {


                        if (Data.Contains("data-cursor"))
                        {
                            int startindex = Data.IndexOf("data-cursor");
                            string start = Data.Substring(startindex).Replace("data-cursor", "");
                            int lastindex = start.IndexOf("<div class=\"stream profile-stream\">");
                            if (lastindex == -1)
                            {
                                lastindex = start.IndexOf("\n");
                            }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(">", string.Empty).Trim();
                            cursor = end;


                            if (cursor != "0")
                            {


                                goto StartAgain;
                            }
                        }

                        if (Data.Contains("cursor"))
                        {
                            int startindex = Data.IndexOf("cursor");
                            string start = Data.Substring(startindex).Replace("cursor", "");
                            int lastindex = -1;

                            lastindex = start.IndexOf(",");
                            if (lastindex > 40)
                            {
                                lastindex = start.IndexOf("\n");

                            }
                            string end = start.Substring(0, lastindex).Replace("\"", "").Replace("\n", string.Empty).Replace("=", string.Empty).Replace(":", string.Empty).Trim();
                            cursor = end;
                            if (cursor != "0")
                            {
                                goto StartAgain;
                            }
                        }

                    }
                  
                    ReturnStatus = "No Error";
                    return lstIds;
                }
                else if (Data.Contains("401 Unauthorized"))
                {
                    ReturnStatus = "Account is Suspended. ";
                    return new List<string>();
                }
                else if (Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}"))
                {
                    ReturnStatus = "Sorry, that page does not exist :";
                    return lstIds;
                }
                else if (Data.Contains("Rate limit exceeded. Clients may not make more than 150 requests per hour."))
                {
                    ReturnStatus = "Rate limit exceeded. Clients may not make more than 150 requests per hour.:-";
                    return lstIds;
                }
                else
                {
                    ReturnStatus = "Error";
                    return lstIds;
                }
            }
            catch (Exception ex)
            {
                //Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> getMembers() -- "" --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> getMembers() -- " + "" + " --> " + ex.Message, Globals.Path_TwtErrorLogs);
                //AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ You have entered invalid URL " + FollowingUrl + " ]");
               
                ReturnStatus = "Error";
               
                return lstIds;
            }
        }
        

        private void AddToLog_ScrapMember(string log)
        {
            try
            {
                if (lstScrapMember.InvokeRequired)
                {
                    lstScrapMember.Invoke(new MethodInvoker(delegate
                    {
                        lstScrapMember.Items.Add(log);
                        lstScrapMember.SelectedIndex = lstScrapMember.Items.Count - 1;
                    }
                    ));
                }
                else
                {
                    lstScrapMember.Items.Add(log);
                    lstScrapMember.SelectedIndex = lstScrapMember.Items.Count - 1;
                }
            }
            catch { }
        }

      

    }
}
