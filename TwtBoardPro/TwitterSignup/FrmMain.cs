using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Globussoft;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Events;
using System.Web;
using Twitter;
using EmailActivator;
using Globussoft;

namespace TwitterSignup
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {

        }

            GlobusHttpHelper globusHelper = new GlobusHttpHelper();
            string ImageURL = string.Empty;
            string authenticitytoken = string.Empty;
            string capcthavalue = string.Empty;

            private void Form1_Load(object sender, EventArgs e)
            {
            }

            static public string DecodeDBC(string[] args, byte[] imageBytes)
            {

                try
                {
                    // Put your DBC username & password here:
                    //Client client = (Client)new HttpClient(args[0], args[1]);
                    DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                    client.Verbose = true;

                    Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);

                    for (int i = 2, l = args.Length; i < l; i++)
                    {
                        Console.WriteLine("Solving CAPTCHA {0}", args[i]);

                        // Upload a CAPTCHA and poll for its status.  Put the CAPTCHA image
                        // file name, file object, stream, or a vector of bytes, and desired
                        // solving timeout (in seconds) here:
                        DeathByCaptcha.Captcha captcha = client.Decode(imageBytes, 2 * DeathByCaptcha.Client.DefaultTimeout);
                        if (null != captcha)
                        {
                            Console.WriteLine("CAPTCHA {0:D} solved: {1}", captcha.Id, captcha.Text);

                            //// Report an incorrectly solved CAPTCHA.
                            //// Make sure the CAPTCHA was in fact incorrectly solved, do not
                            //// just report it at random, or you might be banned as abuser.
                            //if (client.Report(captcha))
                            //{
                            //    Console.WriteLine("Reported as incorrectly solved");
                            //}
                            //else
                            //{
                            //    Console.WriteLine("Failed reporting as incorrectly solved");
                            //}

                            return captcha.Text;
                        }
                        else
                        {
                            Console.WriteLine("CAPTCHA was not solved");
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return null;
            }

            private void button1_Click_1(object sender, System.EventArgs e)
            {
                string username = txtusername.Text;
                string password = txtPass.Text;
                if (username.Count() < 15 || password.Count() > 6)
                {
                    //SignupMultiThreaded();
                }
                else
                {
                    if (username.Count() > 15)
                    {
                        MessageBox.Show("Username Must Not be greater than 15 char");
                    }
                    else if (password.Count() < 6)
                    {
                        MessageBox.Show("Password Must Not be less than 6 char");
                    }
                }
            }

            public void SignupMultiThreaded(object parameters)
            {
                Array paramsArray = new object[3];
                paramsArray = (Array)parameters;

                string Email = string.Empty;
                string Password = string.Empty;

                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;

                string emailData = (string)paramsArray.GetValue(0);
                string username = (string)paramsArray.GetValue(1);
                string name = (string)paramsArray.GetValue(2);

                try
                {
                    Email = emailData.Split(':')[0];
                    Password = emailData.Split(':')[1];
                }
                catch (Exception ex){ AddToListBox(ex.Message); }

                if (emailData.Split(':').Length > 5)
                {
                    proxyAddress = emailData.Split(':')[2];
                    proxyPort = emailData.Split(':')[3];
                    proxyUsername = emailData.Split(':')[4];
                    proxyPassword = emailData.Split(':')[5];
                }
                else if (emailData.Split(':').Length == 4)
                {
                    proxyAddress = emailData.Split(':')[2];
                    proxyPort = emailData.Split(':')[3];
                }

                try
                {
                    if (!(username.Count() < 15 || Password.Count() > 6))
                    {
                        if (username.Count() > 15)
                        {
                            AddToListBox("Username Must Not be greater than 15 char");
                        }
                        else if (Password.Count() < 6)
                        {
                            AddToListBox("Password Must Not be less than 6 char");
                        }
                    }
                }
                catch { }

                Random randm = new Random();
                double cachestop = randm.NextDouble();

                string textUrl = globusHelper.getHtmlfromUrlProxy(new Uri("https://twitter.com/signup"), proxyAddress, proxyPort, proxyUsername, proxyPassword, "", "");
                string pagesource1 = globusHelper.getHtmlfromUrl(new Uri("https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"), "", "");
                string pagesource2 = globusHelper.getHtmlfromUrl(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=" + cachestop + "&lang=en"), "", "");
                try
                {
                    int IndexStart = pagesource2.IndexOf("challenge :");
                    string Start = pagesource2.Substring(IndexStart);
                    int IndexEnd = Start.IndexOf("',");
                    string End = Start.Substring(0, IndexEnd).Replace("challenge :", "").Replace("'", "").Replace(" ", "");
                    capcthavalue = End;
                    ImageURL = "https://www.google.com/recaptcha/api/image?c=" + End;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("1 :" + ex.StackTrace);
                }

                WebClient webclient = new WebClient();
                webclient.DownloadFile(ImageURL, Application.LocalUserAppDataPath + "\\Image.jpg");

                try
                {
                    int StartIndex = textUrl.IndexOf("phx-signup-form");
                    string Start = textUrl.Substring(StartIndex);
                    int EndIndex = Start.IndexOf("name=\"authenticity_token");
                    string End = Start.Substring(0, EndIndex).Replace("phx-signup-form", "").Replace("method=\"POST\"", "").Replace("action=\"https://twitter.com/account/create\"", "");
                    authenticitytoken = End.Replace("class=\"\">", "").Replace("<input type=\"hidden\"", "").Replace("class=\"\">", "").Replace("value=\"", "").Replace("\n", "").Replace("\"", "").Replace(" ", "");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("2 :" + ex.StackTrace);
                }
                try
                {
                    bool Created = true;
                    string url = "https://twitter.com/users/email_available?suggest=1&username=&full_name=&email=" + Email.Replace("@", "%40").Replace(" ", "") + "&suggest_on_username=true&context=signup";
                    string EmailCheck = globusHelper.getHtmlfromUrl(new Uri(url), "https://twitter.com/signup", "");
                    string Usernamecheck = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/users/username_available?suggest=1&username=" + username + "&full_name=" + name + "&email=&suggest_on_username=true&context=signup"), "https://twitter.com/signup", "");

                    if (EmailCheck.Contains("Email has already been taken. An email can only be used on one Twitter account at a time"))
                    {
                        Created = false;
                    }
                    else if (Usernamecheck.Contains("Username has already been taken"))
                    {
                        Created = false;
                    }
                    else if (EmailCheck.Contains("You cannot have a blank email address"))
                    {
                        Created = false;
                    }

                    if (Created)
                    {
                        byte[] args = webclient.DownloadData(ImageURL);

                        string[] arr1 = new string[] { "indianbill007", "sumit1234", "" };

                        string captchaText = DecodeDBC(arr1, args);
                        string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + name + "&user%5Bemail%5D=" + Email.Replace(" ", "") + "&user%5Buser_password%5D=" + Password + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
                        string AccountcraetePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");

                        if (AccountcraetePageSource.Contains("id=\"signout-form\"") && AccountcraetePageSource.Contains("/logout"))
                        {
                            MessageBox.Show("Account created");
                        }


                        if (Created)
                        {
                            ClsEmailActivator EmailActivate = new ClsEmailActivator();
                            bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
                            if (verified)
                            {
                                AddToListBox("Account Verified");
                            }
                        }
                    }
                    else
                    {

                        if (EmailCheck.Contains("Email has already been taken. An email can only be used on one Twitter account at a time"))
                        {
                            AddToListBox("Email has already been taken. An email can only be used on one Twitter account at a time");
                        }
                        else if (Usernamecheck.Contains("Username has already been taken"))
                        {
                            AddToListBox("Username has already been taken");
                        }
                        else if(EmailCheck.Contains("You cannot have a blank email address"))
                        {
                            AddToListBox("You cannot have a blank email address");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("3 :" + ex.StackTrace);
                }
            }

            private void AddToListBox(string p)
            {
                //if ()
                //{

                //}
            }
            /// <summary>
            /// This Method Decodes The Captcha Image
            /// </summary>
            /// <returns>It Returns Decode String Of Captcha Image</returns>
            private string decaptcha(string ImagePath)
            {
                unsafe
                {
                    int id;
                    int ret;
                    int pic_size;

                    int[] p_pict_to;
                    int[] p_pict_type;
                    int size_buf;
                    int[] major_id;
                    int[] minor_id;

                    float[] balance;

                    if (Decaptcher.DecaptcherInit() == -1) return "";

                    id = Decaptcher.CCprotoInit();

                    if (id == -1) return "";

                    ret = Decaptcher.CCprotoLogin(id, Globals.DeCaptcherHost, int.Parse(Globals.DeCaptcherPort), Globals.DeCaptcherUsername, Globals.DeCaptcherUsername.Length, Globals.DeCaptcherPassword, Globals.DeCaptcherPassword.Length);

                    if (ret < 0) return "";

                    balance = new float[1];
                    fixed (float* balance1 = &balance[0])

                        ret = Decaptcher.CCprotoBalance(id, balance1);

                    if (ret < 0) return "";

                    FileStream fs = new FileStream(ImagePath, FileMode.Open);

                    byte[] buffer = new byte[fs.Length];
                    pic_size = (int)fs.Length;
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    byte[] captcha1 = new byte[256];
                    p_pict_to = new int[1];
                    p_pict_type = new int[1];

                    major_id = new int[1];
                    minor_id = new int[1];

                    p_pict_to[0] = 0;
                    p_pict_type[0] = 0;
                    size_buf = 255;

                    fixed (int* p_pict_to1 = &p_pict_to[0])
                    fixed (int* p_pict_type1 = &p_pict_type[0])
                    fixed (int* major_id1 = &major_id[0])
                    fixed (int* minor_id1 = &minor_id[0])
                    fixed (byte* captcha = &captcha1[0])
                    fixed (byte* bufPass = &buffer[0])
                    {
                        //ret = CCprotoPicture(id,bufPass,pic_size, captcha);
                        ret = Decaptcher.CCprotoPicture2(id, bufPass, pic_size, p_pict_to1, p_pict_type1, captcha, size_buf, major_id1, minor_id1);
                        if (ret == -5)
                        {
                            Console.WriteLine("Decapther Server is overloaded Waiting for 1 sec and re-trying...");
                        }
                    }

                    string s = "";
                    s = new string(Encoding.ASCII.GetChars(captcha1));
                    string result = s.Replace("\0", "");
                    //if (result.ToString() == "")
                    //{
                    //    decaptcha();
                    //}

                    Decaptcher.CCprotoClose(id);
                    //this.Invoke(objDecaptchaDelegate, s, true);
                    //Logger.LogText("Captcha Text Fetched Successfully", null);
                    return result;
                }
            }

        }



        public class MyPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
            {
                return true; // Return true to force the certificate to be accepted.
            }
        }

}
