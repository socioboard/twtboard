using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using BaseLib;
using MixedCampaignManager;
using System.Threading;
using System.Text.RegularExpressions;
using Globussoft;
using PopupControl;

namespace MixedCampaignManager.CustomUserControls
{
    public partial class tweetusercontrols : UserControl
    {
        Popup complex;
        TweetMentionUserUsingScrapedList complexPopup;
        

        public tweetusercontrols()
        {
            InitializeComponent();
            complex = new Popup(complexPopup = new TweetMentionUserUsingScrapedList());
            complex.Resizable = true;
            
        }

        BaseLib.NumberHelper Numberhelper = new BaseLib.NumberHelper();
        
        
        //frm_mixcampaignmanager _objfrm_mixcampaignmanager = new frm_mixcampaignmanager();
        //public BaseLib.Events logEvents = new BaseLib.Events();

        public static  BaseLib.Events logEvents = new BaseLib.Events();

        
        private void tweeusercontrols_Load(object sender, EventArgs e)
        {

        }

        private void grp_tweetsettings_Enter(object sender, EventArgs e)
        {

        }

        private void btnUploadTweetMessage_Click(object sender, EventArgs e)
        {
            txt_CmpTweetMessageFile.Text = "";
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
               
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_CmpTweetMessageFile.Invoke(new MethodInvoker(delegate
                    {
                        txt_CmpTweetMessageFile.Text = ofd.FileName;
                        classes.cls_variables._TweetMSGfilepath = ofd.FileName;
                    }));
                }
            }
            
            
        }

        private void chkboxKeepSingleMessage_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
                classes.cls_variables._IsDuplicatMSG = 1;
            else
                classes.cls_variables._IsDuplicatMSG = 0;
        }

        private void chkAllTweetsPerAccount_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                classes.cls_variables._IsAllTweetParAC = 1;
                grp_NoOfTweetParAc.Enabled = false;
                txtNoOfTweets.Enabled = false;
                classes.cls_variables._NoOfTweetsParAC = 0;
                Grp_TweetParDay.Enabled = false;
                txtMaximumTweet.Enabled = false;
                classes.cls_variables._MaxNoOfTweetsParDay = 0;

            }
            else
            {
                classes.cls_variables._IsAllTweetParAC = 0;
                grp_NoOfTweetParAc.Enabled = true;
                txtNoOfTweets.Enabled = true;
                Grp_TweetParDay.Enabled = true;
                txtMaximumTweet.Enabled = true;
            }
        }

        private void chkbosUseHashTags_CheckedChanged(object sender, EventArgs e)
        {
           
            if (((CheckBox)sender).Checked == true)
            {
                classes.cls_variables._IsHashTage = 1;
                if (Globals.HashTags.Count > 0)
                {
                    Log("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Hash Tags In List ]");
                }
                else if (Globals.HashTags.Count == 0)
                {
                    Log("[ " + DateTime.Now + " ] => [ No Hash Tags In List. Click on GetHashtags button For Hash Tag List ]");
                    MessageBox.Show("No Hash Tags In List. Click on GetHashtags button For Hash Tag List");
                    
                }
                btnGetHashTagsCampaign.Visible = true;
            }
            else
            {
                classes.cls_variables._IsHashTage = 0;
                btnGetHashTagsCampaign.Visible = false;
            }

        }


        private void btnGetHashTagsCampaign_Click(object sender, EventArgs e)
        {
            Log("[ " + DateTime.Now + " ] => [ Starting Hash Tag Scrape ]");
            new Thread(() =>
            {
                getHashTags();
            }).Start();
        }

        #region ForHashTags
        public void getHashTags()
        {
            classes.Cls_FollowStart ObjFollowProcess = new MixedCampaignManager.classes.Cls_FollowStart();

            //ObjFollowProcess.CampaignFollowLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

            Globals.HashTags.Clear();
            string returnStatus = string.Empty;
            Globals.HashTags = GetHashTags_New(out returnStatus);
            if (Globals.HashTags.Count > 0)
            {
                Log("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Trending Hash Tags ]");
                Log("[ " + DateTime.Now + " ] => [ " + Globals.HashTags.Count + " Hash Tags In List ]");
                foreach (string data in Globals.HashTags)
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(data, Globals.Path_HashtagsStore);
                }
                Log("[ " + DateTime.Now + " ] => [ Hash tag Finished ]");
                Log("-----------------------------------------------------------------------------------------------------------------------");
            }
            else
            {
                Log("[ " + DateTime.Now + " ] => [ Rate Limit Exceeded.Please Try After Some Time ]");
            }
        }
        #region commentedHashTags
        //public List<string> GetHashTags_New(out string returnStatus)
        //{
        //    List<string> HashTags = new List<string>();
        //    string authenticityToken = string.Empty;
        //    string Woeid = string.Empty;
        //    try
        //    {
        //        //string pagesource = globushttpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/trends/daily.json"), "", "");
        //        Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

        //        string twtPage = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

        //        try
        //        {
        //            int startindex = twtPage.IndexOf("name=\"authenticity_token\"");
        //            string start = twtPage.Substring(startindex).Replace("name=\"authenticity_token\"", "");
        //            int endindex = start.IndexOf("\">");
        //            string end = start.Substring(0, endindex).Replace("value=\"", "");
        //            authenticityToken = end;
        //        }
        //        catch (Exception ex)
        //        {
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        }


        //        string pagesource = HttpHelper.postFormData(new Uri("https://twitter.com/trends/dialog"), "authenticity_token=" + authenticityToken + "&pc=true&woeid=1", "https://twitter.com/", "", "", "", "");

        //        string[] arrayDataWoied = Regex.Split(pagesource, "data-woeid");
        //        arrayDataWoied = arrayDataWoied.Skip(1).ToArray();
        //        foreach (string item in arrayDataWoied)
        //        {

        //            try
        //            {
        //                int startindex = item.IndexOf("=\\\"");
        //                string start = item.Substring(startindex).Replace("=\\\"", "");
        //                int endindex = start.IndexOf("\\\"");
        //                string end = start.Substring(0, endindex).Replace("value=\"", "");
        //                Woeid = end;
        //            }
        //            catch (Exception ex)
        //            {
        //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //            }
        //            string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + Woeid + "&pc=true&personalized=false&src=module&woeid=" + Woeid + ""), "https://twitter.com/", "");

        //            string[] datatrendname = Regex.Split(HastagString, "data-trend-name=");
        //            datatrendname = datatrendname.Skip(1).ToArray();

        //            foreach (string trend in datatrendname)
        //            {
        //                try
        //                {
        //                    if (!trend.Contains("#\\"))
        //                    {
        //                        int startindex = trend.IndexOf("\\\"");
        //                        string start = trend.Substring(startindex).Replace("\\\"", "");
        //                        int endindex = start.IndexOf("\\");
        //                        string end = start.Substring(0, endindex).Replace("value=\"", "").Replace("\\\"", "");
        //                        if (!string.IsNullOrEmpty(end))
        //                        {
        //                            HashTags.Add(end);
        //                            Log("[ " + DateTime.Now + " ] => [ " + end + " ]");
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //                }
        //            }


        //        }
        //        returnStatus = "No Error";
        //        return HashTags;
        //    }
        //    catch (Exception ex)
        //    {
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwitterDataScrapper);
        //        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        //        returnStatus = "Error";
        //        return HashTags;
        //    }
        //} 
        #endregion

        public List<string> GetHashTags_New(out string returnStatus)
        {
            List<string> HashTags = new List<string>();
            string authenticityToken = string.Empty;
            string Woeid = string.Empty;
            List<string> lstWoeid = new List<string>();
            Dictionary<string, string> dicRemoveDuplicate = new Dictionary<string, string>();

            try
            {
                //string pagesource = globushttpHelper.getHtmlfromUrl(new Uri("https://api.twitter.com/1/trends/daily.json"), "", "");
                Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
                string twtPage = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

                try
                {
                    int startindex = twtPage.IndexOf("name=\"authenticity_token\"");
                    string start = twtPage.Substring(startindex).Replace("name=\"authenticity_token\"", "");
                    int endindex = start.IndexOf("\">");
                    string end = start.Substring(0, endindex).Replace("value=\"", "");
                    authenticityToken = end.Trim();
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- authenticityToken --> " + ex.Message, Globals.Path_TwtErrorLogs);
                }


                string pagesource = HttpHelper.postFormData(new Uri("https://twitter.com/trends/dialog"), "authenticity_token=" + authenticityToken + "&pc=true&woeid=0", "https://twitter.com/", "", "", "", "");

                string[] arrayDataWoied = Regex.Split(pagesource, "data-woeid");
                arrayDataWoied = arrayDataWoied.Skip(1).ToArray();
                foreach (string item in arrayDataWoied)
                {

                    try
                    {
                        int startindex = item.IndexOf("=\\\"");
                        string start = item.Substring(startindex).Replace("=\\\"", "");
                        int endindex = start.IndexOf("\\\"");
                        string end = start.Substring(0, endindex).Replace("value=\"", "");
                        Woeid = end;
                        lstWoeid.Add(Woeid);
                        lstWoeid = lstWoeid.Distinct().ToList();
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                }

                foreach (string tempWoeid in lstWoeid)
                {
                    string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + tempWoeid + "&pc=true&personalized=false&src=module&woeid=" + tempWoeid + ""), "https://twitter.com/", "");
                    //string HastagString = HttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/trends?k=" + tempWoeid + "&pc=true&src=module"), "https://twitter.com/", "");
                    string[] datatrendname = Regex.Split(HastagString, "data-trend-name=");
                    datatrendname = datatrendname.Skip(1).ToArray();

                    foreach (string trend in datatrendname)
                    {
                        try
                        {
                            if (!trend.Contains("#\\"))
                            {
                                int startindex = trend.IndexOf("\\\"");
                                string start = trend.Substring(startindex).Replace("\\\"", "");
                                int endindex = start.IndexOf("\\");
                                string end = start.Substring(0, endindex).Replace("value=\"", "").Replace("\\\"", "");
                                if (!string.IsNullOrEmpty(end))
                                {
                                    try
                                    {
                                        dicRemoveDuplicate.Add(end, end);
                                        HashTags.Add(end);
                                        Log("[ " + DateTime.Now + " ] => [ " + end + " ]");
                                    }
                                    catch (Exception)
                                    {
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() -- Woeid --> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                    }
                }


                //}
                returnStatus = "No Error";
                return HashTags;
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwitterDataScrapper);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> GetHashTags_New() --> " + ex.Message, Globals.Path_TwtErrorLogs);
                returnStatus = "Error";
                return HashTags;
            }
        }
        #endregion

        private void ChkboxTweetPerday_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                classes.cls_variables._IsTweetParDay = 1;
               Grp_TweetParDay.Invoke(new MethodInvoker(delegate
               {
                   label85.Visible = true;
                   txtMaximumTweet.Visible = true;
                   grp_NoOfTweetParAc.Enabled = false;
               }));
            }
            else
            {
                classes.cls_variables._IsTweetParDay = 0;
                Grp_TweetParDay.Invoke(new MethodInvoker(delegate
                {
                    label85.Visible = false;
                    txtMaximumTweet.Visible = false;
                    grp_NoOfTweetParAc.Enabled = true;
                }));
            }

        }

        private void txtMaximumTweet_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int res;
                var IsNumber = int.TryParse(((TextBox)sender).Text, out res);
                if (IsNumber)
                {
                    if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 10)
                        classes.cls_variables._MaxNoOfTweetsParDay = int.Parse((((TextBox)sender).Text));
                    else if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) < 10)
                    {
                        classes.cls_variables._MaxNoOfTweetsParDay = int.Parse((((TextBox)sender).Text));
                    }
                    else
                    {
                        MessageBox.Show("Please Enter Correct value in Text box.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter numeric value in Text box.");
                    txtMaximumTweet.Text = "10";
                }
            }
            catch { };
        }

        private void txtNoOfTweets_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) > 10)
                    classes.cls_variables._NoOfTweetsParAC = int.Parse((((TextBox)sender).Text));
                else
                    classes.cls_variables._NoOfTweetsParAC = 10;
            }
            catch
            { }
            //try
            //{`
            //    int res;
            //    var IsNumber = int.TryParse(((TextBox)sender).Text, out res);
            //    if (IsNumber)
            //    {
            //        if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) >= 10)
            //            classes.cls_variables._NoOfTweetsParAC = int.Parse((((TextBox)sender).Text));
            //        else if (!String.IsNullOrEmpty(((TextBox)sender).Text) && int.Parse(((TextBox)sender).Text) < 10)
            //        {
            //            classes.cls_variables._NoOfTweetsParAC = int.Parse((((TextBox)sender).Text));
            //        }
            //        else
            //        {
            //            MessageBox.Show("Please Enter Correct value in Text box.");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please enter numeric value in Text box.");
            //        txtNoOfTweets.Text = "10";
            //    }
            //}
            //catch (Exception err)
            //{

            //}
        }

        private void txt_CmpTweetMessageFile_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(((TextBox)sender).Text))
                classes.cls_variables._TweetMSGfilepath = (((TextBox)sender).Text);
        }

        private void tweetusercontrols_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;

            // Draw the image.
            //g.DrawImage(Properties.Resources.app_bg, 0, 0, this.Width, this.Height);
        }

        private void chk_TweetWithImage_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                grp_TweetImageFile.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._Istweetwithimage = 1;
                    grp_TweetImageFile.Visible = true;

                }));
            }
            else
            {
                grp_TweetImageFile.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._Istweetwithimage = 0;
                    grp_TweetImageFile.Visible = false;
                }));
            }

        }

        //Image Folder Path ...

        private void btn_BrowseTweetImage_Click(object sender, EventArgs e)
        {
            txt_TweetImageFilePath.Text = "";
            using (System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog())
            {
                //ofd.fil = "Text Files (*.txt)|*.txt";
                ofd.SelectedPath = Application.StartupPath + "\\Profile";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_TweetImageFilePath.Text = ofd.SelectedPath;
                    classes.cls_variables._TweetImageFolderPath = ofd.SelectedPath;
                    txt_TweetImageFilePath.Enabled = true;

                }
            }
        }


        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            logEvents.LogText(eventArgs);
        }

        private void chkCampTweetMentionUser_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                groupBox1.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._IsTweetUploadUserList = 1;
                    MixedCampaignManager.classes.cls_variables._IsTweetMentionUserScrapedList = 0;
                    txt_CmpTweetMentionUserList.Visible = true;
                    btnUploadTweetMentionUserList.Visible = true;
                    chkTweetMentionUserScrapedList.Visible = false;
                    chkTweetMentionUserScrapedList.Checked = false;
                }));

                
                //complex.Show(sender as CheckBox);
            }
            else
            {
                groupBox1.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._IsTweetUploadUserList = 0;
                    txt_CmpTweetMentionUserList.Visible = false;
                    btnUploadTweetMentionUserList.Visible = false;
                    chkTweetMentionUserScrapedList.Visible = true;

                }));
            }
        }

        private void btnUploadTweetMentionUserList_Click(object sender, EventArgs e)
        {
            txt_CmpTweetMentionUserList.Text = "";
            using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txt_CmpTweetMentionUserList.Invoke(new MethodInvoker(delegate
                    {
                        txt_CmpTweetMentionUserList.Text = ofd.FileName;
                        classes.cls_variables._TweetUploadUserList = ofd.FileName;
                    }));
                }
            }
        }

        private void chkTweetMentionUserScrapedList_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                groupBox1.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._IsTweetMentionUserScrapedList = 1;
                    MixedCampaignManager.classes.cls_variables._IsTweetUploadUserList = 0;
                    chkCampTweetMentionUser.Visible = false;
                    chkCampTweetMentionUser.Checked = false;
                }));


                complex.Show(sender as CheckBox);
            }
            else
            {
                groupBox1.Invoke(new MethodInvoker(delegate
                {
                    MixedCampaignManager.classes.cls_variables._IsTweetMentionUserScrapedList = 0;
                    chkCampTweetMentionUser.Visible = true;
                    //chkTweetMentionUserScrapedList.Visible = true;


                }));
            }
        }

        private void txt_TweetImageFilePath_TextChanged(object sender, EventArgs e)
        {

            
            //if (!String.IsNullOrEmpty(((TextBox)sender).Text))
            //    classes.cls_variables._TweetImageFolderPath = (((TextBox)sender).Text);
        }
       

    }
}
