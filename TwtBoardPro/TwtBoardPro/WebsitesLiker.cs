using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using BaseLib;
using Globussoft;
using System.Net;
using System.IO;

namespace twtboardpro
{
    public partial class WebsitesLiker : Form
    {
        public WebsitesLiker()
        {
            InitializeComponent();
        }

        // Class Variables
        List<string> LstWebsiteLink = new List<string>();
        List<string> LstMessage = new List<string>();
        List<Thread> LstWebsiteLikeThread = new List<Thread>();
        bool IsStopWebsiteLiker = false;
        TweetAccountManager TweetLogin = new TweetAccountManager();
        GlobusHttpHelper _GlobussHttpHelper = new GlobusHttpHelper();
        ChilkatHttpHelpr _ChilkatHttpHelper = new ChilkatHttpHelpr();
        int counter = 0;

        private void WebsitesLiker_Load(object sender, EventArgs e)
        {
           
        }

        private void btn_BrowseMessages_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_Message.Text = ofd.FileName;
                        LstMessage.Clear();
                        LstMessage = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Uploaded " + LstMessage.Count + " Messages ]");

                    }
                }

            }
            catch { }
        }

        private void btn_StartWebsitelike_Click(object sender, EventArgs e)
        {
            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Process started.... ]");
            btn_StartWebsitelike.Enabled = false;
            btn_StopWebsiteLike.Enabled = true;
            try
            {
                new Thread(() =>
                {
                    StartWebsiteLike();
                }).Start();
            }
            catch 
            {
               
            };
             
        }

     
        private void StartWebsiteLike()
        {
            try
            {
                if (IsStopWebsiteLiker)
                {
                    return;
                }

                LstWebsiteLikeThread.Add(Thread.CurrentThread);
                LstWebsiteLikeThread = LstWebsiteLikeThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { };

            int NoOfWebsiteLiker = 5;
            int noOfThread = 5;
            if (!string.IsNullOrEmpty(txtLikeNoOfwebsite.Text))
            {
                txtLikeNoOfwebsite.Invoke(new MethodInvoker(delegate
                {
                    NoOfWebsiteLiker = int.Parse(txtLikeNoOfwebsite.Text);
                }));
            }



            try
            {
                foreach (KeyValuePair<string, TweetAccountManager> item in TweetAccountContainer.dictionary_TweetAccount)
                {

                    try
                    {
                        ThreadPool.SetMaxThreads(noOfThread, 5);

                        ThreadPool.QueueUserWorkItem(new WaitCallback(StartWebsiteLikeMultiThread), new object[] { item, NoOfWebsiteLiker });

                        Thread.Sleep(2000);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch
            { }
         
        }

        private void StartWebsiteLikeMultiThread(object parameters)
        {
            try
            {
                if (IsStopWebsiteLiker)
                {
                    return;
                }

                LstWebsiteLikeThread.Add(Thread.CurrentThread);
                LstWebsiteLikeThread = LstWebsiteLikeThread.Distinct().ToList();
                Thread.CurrentThread.IsBackground = true;
            }
            catch
            { };
            Array paramsArray = new object[2];
            paramsArray = (Array)parameters;
            int NumberOfWebsiteTobeLiked = 0;
            KeyValuePair<string, TweetAccountManager> keyValue = (KeyValuePair<string, TweetAccountManager>)paramsArray.GetValue(0);
            NumberOfWebsiteTobeLiked = (int)paramsArray.GetValue(1);

           
            bool OtherUser = false;
            TweetAccountManager tweetAccountManager = keyValue.Value;
          

            if (!tweetAccountManager.IsLoggedIn)
            {
                AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Logging With Account : " + keyValue.Key + " ]");
                tweetAccountManager.Login();
            }

            if (!tweetAccountManager.IsLoggedIn)
            {
                AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Not Logged In With Account : " + keyValue.Key + " ]");
                return;
            }

            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Logged In With Account : " + keyValue.Key + " ]");
            foreach(string item in LstWebsiteLink)
            {
                string Pagesource = string.Empty;
                string Response = string.Empty;
                string Text = string.Empty;
                string AuthencityToken = string.Empty;
                string tw_p = string.Empty;
                string via = string.Empty;
                string OrignalReferer = string.Empty;
                try
                {
                    string Url = "https://about.twitter.com/resources/buttons#tweet";
                    Pagesource = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri(Url), "", "");
                    Response = tweetAccountManager.globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/intent/tweet?original_referer=https%3A%2F%2Fabout.twitter.com%2Fresources%2Fbuttons&text=Twitter%20Buttons%20%7C%20About&tw_p=tweetbutton&url=http%3A%2F%2Fwww.globussoft.com&via=WhitentonSkoff"), "", "");

                    try
                    {
                        Text = getBetween(Response, "name=\"text\" type=\"hidden\" value=\"", "\"");

                    }
                    catch { };

                    try
                    {
                        AuthencityToken = getBetween(Response, "authenticity_token\" value=\"", "\"");
                    }
                    catch { };

                    try
                    {
                        tw_p = getBetween(Response, "\"tw_p\" type=\"hidden\" value=\"", "\"");
                    }
                    catch { };

                    try
                    {
                        via = getBetween(Response, "\"via\" type=\"hidden\" value=\"", "\"");
                    }
                    catch
                    { };

                    try
                    {
                        OrignalReferer = getBetween(Response, "original_referer=", "&");
                    }
                    catch { };

                    try
                    {
                        string postData = "Url=" + Uri.EscapeDataString(item) + "&Text=" + Uri.EscapeDataString(Text).Replace(" ", "+") + "&original_referer=" + Uri.EscapeDataString(OrignalReferer) + "&via=" + via + "&tw_p=" + tw_p + "&authenticity_token=" + AuthencityToken + "&status=Twitter+Buttons+%7C+About" + Uri.EscapeDataString(item) + "via" + Uri.EscapeDataString(via);
                        string postUrl = "https://twitter.com/intent/tweet/update";
                        string PostRequest = tweetAccountManager.globusHttpHelper.postFormData(new Uri(postUrl), postData, "", "", "", "", "");
                        if (PostRequest.Contains("Your Tweet has been posted!"))
                        {
                            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Your Tweet has been posted ]");
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(item, Globals.Path_SuccessfullyTweetedUrls);
                        }
                        else if (PostRequest.Contains("You have already sent this Tweet"))
                        {
                            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ You have already sent this Tweet]");
                        }
                        else
                        {
                            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Your Tweet has not been posted ]");
                        }

                    }
                    catch { };


                }
                catch
                { };
        }

            AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Process Completed ........ ]");
        }



        private void ChkUseMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkUseMessage.Checked)
            {
                btn_BrowseMessages.Enabled = true;
            }
            else
            {
                btn_BrowseMessages.Enabled = false;
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
        public void AddToWebsiteLikeLog(string log)
        {
            try
            {
                if (LsWebsiteLikerLogger.InvokeRequired)
                {
                    LsWebsiteLikerLogger.Invoke(new MethodInvoker(delegate
                    {
                        LsWebsiteLikerLogger.Items.Add(log);
                        LsWebsiteLikerLogger.SelectedIndex = LsWebsiteLikerLogger.Items.Count - 1;
                    }));
                }
                else
                {
                    LsWebsiteLikerLogger.Items.Add(log);
                    LsWebsiteLikerLogger.SelectedIndex = LsWebsiteLikerLogger.Items.Count - 1;
                    
                }
            }
            catch (Exception)
            {
            }
        }

        private void btn_StopWebsiteLike_Click(object sender, EventArgs e)
        {
            try
            {
                new Thread(() =>
                {
                    StopWebsiteLike();
                }).Start();
            }
            catch { };
        }

        private void StopWebsiteLike()
        {

            btn_StopWebsiteLike.Invoke(new MethodInvoker(delegate
                   {
                       btn_StartWebsitelike.Enabled = true;
                       btn_StopWebsiteLike.Enabled = false;
                   }));
          
            IsStopWebsiteLiker = true;
            try
            {
                foreach (Thread item in LstWebsiteLikeThread)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => Process Stopped  ]");
                AddToWebsiteLikeLog("[ -----------------------------------------------------------------------------------------------------------------------]");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void btn_BrowseWebsites_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txt_WebsitesLink.Text = ofd.FileName;
                        LstWebsiteLink.Clear();
                        LstWebsiteLink = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddToWebsiteLikeLog("[ " + DateTime.Now + " ] => [ Uploaded " + LstWebsiteLink.Count + " Website for posting Tweet ]");

                    }
                }

            }
            catch { }
        }
        
             
    }
}
