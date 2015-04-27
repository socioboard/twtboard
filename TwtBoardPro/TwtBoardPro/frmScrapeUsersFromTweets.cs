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
using System.IO;
using Globussoft;

namespace twtboardpro
{
    public partial class frmScrapeUsersFromTweets : Form
    {
        public frmScrapeUsersFromTweets()
        {
            InitializeComponent();
        }

        #region GlobalVariables
        bool CheckNetConn = false;
        Thread thread_StartMemberScrape = null;
        int noOfRecords = 0;
        public List<Thread> lstIsStopScrapMember = new List<Thread>();
        Globussoft.GlobusHttpHelper globushttpHelper = new Globussoft.GlobusHttpHelper();
        private System.Drawing.Image image;
        List<string> lstUrl = new List<string>();
        Thread thread_StartScrape = null;
        #endregion

        private void frmScrapeUsersFromTweets_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg;
        }

        private void frmScrapeUsersFromTweets_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

        private void btnUploadTweetsUrl_Click(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtUploadTweetsUrl.Text = ofd.FileName;
                    //Globals.EmailsFilePath = ofd.FileName;
                    lstUrl = new List<string>();
                    lstUrl = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                    AddToLog_ScrapMember(lstUrl.Count + " Urls Uploaded");

                }
                else
                {
                    lstUrl = new List<string>();
                    txtUploadTweetsUrl.Text = string.Empty; 
                    AddToLog_ScrapMember(lstUrl.Count + " Urls Uploaded");
                }
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

        private void btnStartScrap_Click(object sender, EventArgs e)
        {
            CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                try
                {
                    if (lstUrl.Count > 0)
                    {
                        //ScrapeMemberSeacrh();
                        thread_StartScrape = new Thread(() =>
                        {
                            ScrapeMemberSeacrh();
                        });
                        thread_StartScrape.Start();
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
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                int counter = 0;
                TweetAccountManager TweetLogin = new TweetAccountManager();
               

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


                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {
                    AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Login process starting]");
                    if (counter > TweetAccountContainer.dictionary_TweetAccount.Count)
                    {
                        counter = 0;
                    }

                    TweetLogin = new TweetAccountManager();
                    TweetLogin.Username = item.Key;
                    TweetLogin.Password = item.Value.Password;
                    TweetLogin.proxyAddress = item.Value.proxyAddress;
                    TweetLogin.proxyPort = item.Value.proxyPort;
                    TweetLogin.proxyUsername = item.Value.proxyUsername;
                    TweetLogin.proxyPassword = item.Value.proxyPassword;
                    TweetLogin.Login();

                    if (!TweetLogin.IsLoggedIn)
                    {
                        continue;
                    }
                    else
                    {
                        AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Login process successful]");
                        globusHttpHelper = TweetLogin.globusHttpHelper;
                        counter++;
                        break;
                    }
                }


                if (!TweetLogin.IsLoggedIn)
                {
                    AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Please Upload Atleast One Working Account To Get Details ]");
                    return;
                }


                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Scraping User ]");

                
                    foreach (string item in lstUrl)
                    {
                        string tempItem = string.Empty;
                        if (!item.Contains("https://twitter.com"))
                        {
                            tempItem = "https://twitter.com" + item;
                        }
                        else
                        {
                            tempItem = item;
                        }

                        List<string> lstMember = new List<string>();
                        string returnStatus = string.Empty;
                        getTweetUsers(item, ref globusHttpHelper);
                        //lstMember = GetMembers(item, ref globusHttpHelper, out returnStatus);
                        AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [ Scraped Member for Url " + item + "]");
                    }

            }
            catch { }
        }


        public List<string> GetMembers(string TweetUrl, ref GlobusHttpHelper HttpHelper,  out string ReturnStatus)
        {
            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string userID;
            string Screen_name;
            int counter = 0;
            try
            {

             //   string numResult = Regex.Match(TweetUrl, @"\d+").Value;
                TweetUrl = TweetUrl + "@@@";

                string numResult = getBetween(TweetUrl, "status/", "@@@"); 

            StartAgain:

                String DataCursor = string.Empty;
                if (counter == 0)
                {
                    FollowingUrl = "https://twitter.com/i/katyperry/conversation/"+numResult+"?include_available_features=1&include_entities=1&max_position=0";//TweetUrl;
                    counter++;
                }
                else
                {
                    FollowingUrl = "https://twitter.com/i/katyperry/conversation/" + numResult + "?include_available_features=1&include_entities=1&max_position=" + cursor.Trim();
                }


                string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                if (string.IsNullOrEmpty(Data))
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                }

                if (string.IsNullOrEmpty(Data))
                {
                    AddToLog_ScrapMember("Either Url is Invalid or PageSource is getting Null or Empty.");

                    ReturnStatus = "Error";
                    return lstIds;
                }
                String DataCursor1 = string.Empty;

                if (!Data.Contains("Rate limit exceeded") && !Data.Contains("{\"errors\":[{\"message\":\"Sorry, that page does not exist\",\"code\":34}]}") && !string.IsNullOrEmpty(Data))
                {

                    String[] DataDivArr;
                    if (Data.Contains("js-stream-tweet js-actionable-tweet"))
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-tweet js-actionable-tweet");
                    }
                    else
                    {
                        DataDivArr = Regex.Split(Data, "simple-tweet tweet");
                    }

                    foreach (var DataDivArr_item in DataDivArr)
                    {
                        if (DataDivArr_item.Contains("min_position"))
                        {
                            //String DataCurso = System.Text.RegularExpressions.Regex.Split(Data, "data-cursor")[1];
                            DataCursor1 = DataDivArr_item.Substring(DataDivArr_item.IndexOf("min_position\":"), DataDivArr_item.IndexOf(",")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("min_position", string.Empty).Replace(":", "").Replace(",", "").Trim();
                            
                        }
                        if (DataDivArr_item.Contains("<!DOCTYPE html>") || DataDivArr_item.Contains("min_position"))
                        {
                            continue;
                        }

                        if (DataDivArr_item.Contains("data-screen-name"))
                        {
                            int endIndex = 0;
                            int startIndex = DataDivArr_item.IndexOf("data-screen-name");
                            try
                            {
                                endIndex = DataDivArr_item.IndexOf("data-name");
                            }
                            catch { }

                            if (endIndex == -1)
                            {
                                endIndex = DataDivArr_item.IndexOf("data-feedback-token");
                            }

                            string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);

                            //string _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-feedback-token", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                            string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-user-id", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                            if (_SCRName.Contains(" "))
                            { 
                                _SCRName = _SCRName.Split(' ')[0];
                            }

                            if (noOfRecords > lstIds.Count)
                            {
                                lstIds.Add(_SCRName );
                                lstIds = lstIds.Distinct().ToList();
                                AddToLog_ScrapMember("[ " + DateTime.Now + " ] => ["  + _SCRName + " ]");
                                if (!File.Exists(Globals.Path_ScrapedMembersList))
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(" UserName , Url", Globals.Path_ScrapedMembersList);
                                }
                                GlobusFileHelper.AppendStringToTextfileNewLine( _SCRName + "," + TweetUrl, Globals.Path_ScrapedMembersList);

                            }


                        }

                    }


                    if (noOfRecords != lstIds.Count)
                    {


                        if (Data.Contains("min_position"))
                        {
                            DataCursor1 = Data.Substring(Data.IndexOf("min_position\":"), Data.IndexOf(",")).Replace(">", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty).Replace("min_position", string.Empty).Replace(":", string.Empty).Replace(",", string.Empty).Trim();
                            cursor = DataCursor1;

                            if (cursor.Contains("null") || cursor.Contains("Null"))
                            {
                                ReturnStatus = "No Error";
                                return lstIds;
                            }

                            if (cursor != "0")
                            {


                                goto StartAgain;
                            }
                        }

                        if (Data.Contains("\"has_more_items\":true"))
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

        public static string getBetween(string strSource, string strStart, string strEnd)
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
        public void getTweetUsers(string Url, ref GlobusHttpHelper HttpHelper)
        { 
            string cursor = "-1";
            string FollowingUrl = string.Empty;
            List<string> lstIds = new List<string>();
            string userID;
            string Screen_name;
            int counter = 0;
            try
            {
                Url=Url+"@@@";

                string numResult = getBetween(Url, "status/", "@@@");    //Regex.Match(Url, @"\d+").Value;
                


                FollowingUrl = "https://twitter.com/i/activity/retweeted_popup?id=" + numResult;

                string Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                if (string.IsNullOrEmpty(Data))
                {
                    Data = HttpHelper.getHtmlfromUrl(new Uri(FollowingUrl), "", "");
                }

                if (string.IsNullOrEmpty(Data))
                {
                    AddToLog_ScrapMember("Either Url is Invalid or PageSource is getting Null or Empty.");

                    //ReturnStatus = "Error";
                    return ;
                }

                String[] DataDivArr = null;
                if (Data.Contains("js-stream-item stream-item stream-item"))
                    {
                        DataDivArr = Regex.Split(Data, "js-stream-item stream-item stream-item");
                    }

                foreach (var DataDivArr_item in DataDivArr)
                {
                    if (DataDivArr_item.Contains("data-screen-name"))
                    {
                        int endIndex = 0;
                        int startIndex = DataDivArr_item.IndexOf("data-screen-name");
                        try
                        {
                            endIndex = DataDivArr_item.IndexOf("data-name");
                        }
                        catch { }

                        if (endIndex == -1)
                        {
                            endIndex = DataDivArr_item.IndexOf("data-feedback-token");
                        }

                        string GetDataStr = DataDivArr_item.Substring(startIndex, endIndex);

                        //string _SCRNameID = (GetDataStr.Substring(GetDataStr.IndexOf("data-user-id"), GetDataStr.IndexOf("data-feedback-token", GetDataStr.IndexOf("data-user-id")) - GetDataStr.IndexOf("data-user-id")).Replace("data-user-id", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                        string _SCRName = (GetDataStr.Substring(GetDataStr.IndexOf("data-screen-name="), GetDataStr.IndexOf("data-user-id", GetDataStr.IndexOf("data-screen-name=")) - GetDataStr.IndexOf("data-screen-name=")).Replace("data-screen-name=", string.Empty).Replace("=", string.Empty).Replace("\"", "").Replace("\\\\n", string.Empty).Replace("data-screen-name=", string.Empty).Replace("\\", "").Trim());
                        if (_SCRName.Contains(" "))
                        {
                            _SCRName = _SCRName.Split(' ')[0];
                        }

                        //if (noOfRecords > lstIds.Count)
                        {
                            lstIds.Add(_SCRName);
                            lstIds = lstIds.Distinct().ToList();
                            AddToLog_ScrapMember("[ " + DateTime.Now + " ] => [" + _SCRName + " ]");
                            if (!File.Exists(Globals.Path_ScrapedMembersList))
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(" UserName , Url", Globals.Path_ScrapedMembersList);
                            }
                            GlobusFileHelper.AppendStringToTextfileNewLine(_SCRName + "," + Url, Globals.Path_ScrapedMembersList);

                        }


                    }
                }
            }
            catch { }
        }
    }
}
