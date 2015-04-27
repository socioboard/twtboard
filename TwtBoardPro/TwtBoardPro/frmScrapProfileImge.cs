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
using System.Drawing.Drawing2D;

namespace twtboardpro
{
    public partial class frmScrapProfileImge : Form
    {
        GlobusHttpHelper objGlobusHttpHelper = new GlobusHttpHelper();
        private System.Drawing.Image image;
        public frmScrapProfileImge()
        {
            InitializeComponent();
        }

        List<string> lstscrapeUserNameForImage = new List<string>();
        List<Thread> lstThreadStopProcess = new List<Thread>();
        private void btnBrowseUserName_Click(object sender, EventArgs e)
        {
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtScrapeUserName.Text = ofd.FileName;
                        lstscrapeUserNameForImage.Clear();
                        lstscrapeUserNameForImage = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => [ Uploaded " + lstscrapeUserNameForImage.Count + " Names Or ID To Scrape Profile Image ]");
                    }
                }

            }
            catch { }
        }



        public void AddtoLoggerscrapeUserNameorIDForImages(string log)
        {
            try
            {
                if (lstProcessLoggerForImage.InvokeRequired)
                {
                    lstProcessLoggerForImage.Invoke(new MethodInvoker(delegate
                    {
                        lstProcessLoggerForImage.Items.Add(log);
                        lstProcessLoggerForImage.SelectedIndex = lstProcessLoggerForImage.Items.Count - 1;
                    }));
                }
                else
                {
                    lstProcessLoggerForImage.Items.Add(log);
                    lstProcessLoggerForImage.SelectedIndex = lstProcessLoggerForImage.Items.Count - 1;
                }
            }
            catch (Exception)
            {
            }
        }
        private void rad_UserName_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_UserName.Checked)
            {
                rad_UserID.Checked = false;
            }
            else
            {
                rad_UserID.Checked = true;
            }
        }

        private void rad_UserID_CheckedChanged(object sender, EventArgs e)
        {
            if (rad_UserID.Checked)
            {
                rad_UserName.Checked = false;
            }
            else
            {
                rad_UserName.Checked = true;
            }
        }
       
        private void btnBrowseUploadUserInfo_Click(object sender, EventArgs e)
        {
            btnStartScraping.Enabled = true;
            try
            {
                using (System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "Text Files (*.txt)|*.txt";
                    ofd.InitialDirectory = Application.StartupPath;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        txtScrapeUserName.Text = ofd.FileName;
                        lstscrapeUserNameForImage.Clear();
                        lstscrapeUserNameForImage = Globussoft.GlobusFileHelper.ReadFiletoStringList(ofd.FileName);
                        AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => [ Uploaded " + lstscrapeUserNameForImage.Count + " Names Or ID to Scrape Profile Image ]");
                       
                    }
                }

            }
            catch { }
        }

        private void btnStartScraping_Click(object sender, EventArgs e)
        {

            btnStartScraping.Enabled = false;
            AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Processing...  ]");
            bool CheckNetConn = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (CheckNetConn)
            {
                new Thread(() =>
                {
                    StartScrapingImage();
                }).Start();

            }
            else
            {
                MessageBox.Show("Your Internet Connection is disabled ! or not working, Please Check Your Internet Connection...");
                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => [ Your Internet Connection is disabled ! or not working, Please Check Your Intenet connection]");
            }
        }
        string FileLocation = string.Empty;
        private void StartScrapingImage()
        {
            lstThreadStopProcess.Add(Thread.CurrentThread);
            lstThreadStopProcess.Distinct();
            Thread.CurrentThread.IsBackground = true;
            try
            {


                foreach (string item in lstscrapeUserNameForImage)
                {
                    if (rad_UserName.Checked)
                    {
                        try
                        {
                            string url = "http://www.twitter.com/" + item;
                            AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Scraping Image For @" + item + "  ]");
                            string pagesource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");
                            if (string.IsNullOrEmpty(pagesource))
                            {
                                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] =>Please enter valid User Name  ]");
                            }


                            else
                            {
                                string profileImageUrl = objGlobusHttpHelper.getBetween(pagesource, "class=\"ProfileAvatar-image \"", "alt=").Replace("src", "").Replace("=", "").Replace("\"", "").Trim();
                               
                                if (!string.IsNullOrEmpty(profileImageUrl))
                                {
                                    AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Got Url :-" + profileImageUrl + "  ]");
                                    AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Saving Image of " + item + " in " + Globals.path_ScrapedImageFolder + " ]");
                                    //List<string> lstStoreImagesurl = new List<string>();
                                    //lstStoreImagesurl.Add(str);
                                    //foreach (string item1 in lstStoreImagesurl)
                                    {
                                        SaveImageWithUrl(profileImageUrl, Globals.path_ScrapedImageFolder + "\\" + item + ".jpg");
                                    }
                                }
                                else
                                {
                                    AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => No image Url found for user :-" + item + "  ]");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                    else if (rad_UserID.Checked)
                    {
                        try
                        {
                            string url = "https://twitter.com/intent/user?user_id=" + item;
                            AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Scraping Image For @" + item + "  ]");
                            string pagesource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");

                            if (string.IsNullOrEmpty(pagesource))
                            {
                                pagesource = objGlobusHttpHelper.getHtmlfromUrl(new Uri(url), "", "");
                            }
                            if (pagesource == null)
                            {
                                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] =>Please enter valid User Id   ]");
                            }
                            else
                            {
                                string profileImageUrl = objGlobusHttpHelper.getBetween(pagesource, "class=\"photo", "alt=").Replace("src", "").Replace("=", "").Replace("\"", "").Trim();
                                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Got Url :-" + profileImageUrl + "  ]");
                                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Saving Image of " + item + " in C:\\Users\\GLB-264\\Desktop\\twtboardpro ]");
                                //List<string> lstStoreImagesurl = new List<string>();
                                //lstStoreImagesurl.Add(str);
                                //foreach (string item1 in lstStoreImagesurl)
                                {
                                    SaveImageWithUrl(profileImageUrl, Globals.path_ScrapedImageFolder + "\\" + item + ".jpg");
                                }
                            }
                        }
                        catch { }

                    }
                    else
                    {
                        MessageBox.Show("Please check Username or User Id Option");
                        try
                        {
                            btnStartScraping.Invoke(new MethodInvoker(delegate() { btnStartScraping.Enabled = true; }));
                        }
                        catch { }
                        return;
                    }


                }
               //  MessageBox.Show("Please check Username or User Id Option");
            }
            catch
            { };
            AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Process completed  ]");
           AddtoLoggerscrapeUserNameorIDForImages("[ -----------------------------------------------------------------------------------------------------------------------]");
          

        }
 
        private void SaveImageWithUrl(string imgeUri, string saveto)
        {
            try
            {

                using (WebClient webClient = new WebClient())
                {
                    using (Stream stream = webClient.OpenRead(imgeUri))
                    {
                        using (Bitmap bitmap = new Bitmap(stream))
                        {
                            stream.Flush();
                            stream.Close();
                            bitmap.Save(saveto);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void frmScrapProfileImge_Load(object sender, EventArgs e)
        {
            image = Properties.Resources.app_bg; 
        }

        private void btnStopScraing_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Thread item in lstThreadStopProcess)
                {
                    try
                    {
                        item.Abort();
                    }
                    catch
                    {
                    }
                }
                AddtoLoggerscrapeUserNameorIDForImages("[ " + DateTime.Now + " ] => Process Stopped  ]");
                AddtoLoggerscrapeUserNameorIDForImages("[ -----------------------------------------------------------------------------------------------------------------------]");
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        private void frmScrapProfileImge_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;

            g = e.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(image, 0, 0, this.Width, this.Height);
        }

     }
}
