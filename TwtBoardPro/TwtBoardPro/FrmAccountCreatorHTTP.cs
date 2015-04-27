using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BaseLib;
using System.Net;
using System.IO;
using System.Threading;
using Globussoft;
using AccountCreator;
using OpenPOP.POP3;
using System.Text.RegularExpressions;

namespace FaceDominator
{
    public partial class FrmAccountCreatorHTTP : Form
    {
        public FrmAccountCreatorHTTP()
        {
            InitializeComponent();
        }

        public List<string> listEmails = new List<string>();

        public List<string> listFirstName = new List<string>();

        public List<string> listLastName = new List<string>();

        public string SexSelect = string.Empty;

        public static bool UseDeCaptcher = false;
        public static bool UseDBC = false;

        int accountCounter = -1;
        int tryCaptchaCounter = 0;

        int retryCounter = 0;   //If unable to load Captcha, this counter gets incremented, After 3 unsuccessful trials, SwitchToNextAccount is called
                                //Is Reset to 0 whenever an account is switched

        public static int minDelay = 12;
        public static int maxDelay = 40;

        //string FirstName = string.Empty;
        //string LastName = string.Empty;
        //string Email = string.Empty;
        //string Password = string.Empty;

        string _proxyAddress = string.Empty;
        string _proxyPort = string.Empty;
        string _proxyUsername = string.Empty;
        string _proxyPassword = string.Empty;

        #region Account Creator Variable Decalaration

        public static Events LogEventsFacebookCreator = new Events();

        string FirstName = string.Empty;
        string LastName = string.Empty;
        string _Email = string.Empty;
        string _Password = string.Empty;
        string DOB = string.Empty;

        string post_form_id = string.Empty;
        string lsd = string.Empty;
        string reg_instance = string.Empty;
        string firstname = string.Empty;
        string lastname = string.Empty;
        string reg_email__ = string.Empty;
        string reg_email_confirmation__ = string.Empty;
        string reg_passwd__ = string.Empty;
        string sex = string.Empty;
        string birthday_month = string.Empty;
        string birthday_day = string.Empty;
        string birthday_year = string.Empty;
        string captcha_persist_data = string.Empty;
        string captcha_session = string.Empty;
        string extra_challenge_params = string.Empty;
        string recaptcha_public_key = string.Empty;
        string authp_pisg_nonce_tt = null;
        string authp = string.Empty;
        string psig = string.Empty;
        string nonce = string.Empty;
        string tt = string.Empty;
        string time = string.Empty;
        string challenge = string.Empty;
        string CaptchaSummit = string.Empty;
        #endregion

        public List<string> lstPrivateProxies = new List<string>();

        private readonly POPClient popClient = new POPClient();

        Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

        string captchaSrc = string.Empty;

        string decaptchaImagePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\FaceDominator";

        Thread createAccountsThread = null;
        private void FrmAccountCreatorHTTP_Load(object sender, EventArgs e)
        {
            //GoToNextAccount();

            //Regex IdCheck1 = new Regex("^[0-9]*$");

            //int numberOfThreads = 20;

            //if (IdCheck1.IsMatch(txtNumberOfThreads.Text) && !string.IsNullOrEmpty(txtNumberOfThreads.Text))
            //{
            //    numberOfThreads = int.Parse(txtNumberOfThreads.Text);
            //}

            ////Set ThreadPoolThreads
            //ThreadPool.SetMaxThreads(numberOfThreads, 4);

            ////ThreadPool.QueueUserWorkItem(new WaitCallback(GoToNextAccount), )

            //StartAccountCreationMultiThreaded();

            FrmProxyAssigner.startFlyCreationEvent.addToLogger += new EventHandler(startFlyCreationEvent_addToLogger);
            ChilkatIMAP.LogEvents.addToLogger+=new EventHandler(Log_Imap);

            queEmails = new Queue<string>();

            foreach (string item in listEmails)
            {
                queEmails.Enqueue(item);
            }

            panelSingleThreaded.Visible = false;
            panelSMultiThreaded.Visible = false;

        }

        

        private void StartAccountCreationMultiThreaded()
        {
            //foreach (string email in listEmails)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod), email);
            //}

            int counter_Names = 0;
            foreach (string email in listEmails)
            {
                if (counter_Names < listFirstName.Count && counter_Names < listLastName.Count)
                {
                    
                }
                else
                {
                    counter_Names = 0;
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod), new object[] { email, counter_Names });
                counter_Names++;
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod), email);
            }
        }

        private void ThreadPoolMethod(object parameters)
        {
            try
            {
                //string email = (string)parameters;

                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;

                string email = (string)paramsArray.GetValue(0);

                int count_Names = (int)paramsArray.GetValue(1);

                GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

                string post_form_id = string.Empty;
                string lsd = string.Empty;
                string reg_instance = string.Empty;
                string firstname = string.Empty;
                string lastname = string.Empty;
                string reg_email__ = string.Empty;
                string reg_email_confirmation__ = string.Empty;
                string reg_passwd__ = string.Empty;
                string sex = string.Empty;
                string birthday_month = string.Empty;
                string birthday_day = string.Empty;
                string birthday_year = string.Empty;
                string captcha_persist_data = string.Empty;
                string captcha_session = string.Empty;
                string extra_challenge_params = string.Empty;
                string recaptcha_public_key = string.Empty;
                string authp_pisg_nonce_tt = null;
                string authp = string.Empty;
                string psig = string.Empty;
                string nonce = string.Empty;
                string tt = string.Empty;
                string time = string.Empty;
                string challenge = string.Empty;
                string CaptchaSummit = string.Empty;

                string Email = string.Empty;
                string Password = string.Empty;

                string proxyAddress = string.Empty;
                string proxyPort = string.Empty;
                string proxyUsername = string.Empty;
                string proxyPassword = string.Empty;

                try
                {
                    Email = email.Split(':')[0];
                    Password = email.Split(':')[1];

                    firstname = listFirstName[count_Names];
                    lastname = listLastName[count_Names];
                }
                catch (Exception ex){ AddToListBox(ex.Message); }

                AddToListBox("Creating Account with " + Email);

                string responseMessage = string.Empty;
                string captchaSrcMulti = string.Empty;

                #region Commented
                // captchaSrcMulti = GetCaptchaImageMulti(email, ref HttpHelper, ref post_form_id, ref lsd,
                //ref reg_instance,
                //ref firstname,
                //ref lastname,
                //ref reg_email__,
                //ref reg_email_confirmation__,
                //ref reg_passwd__,
                //ref sex,
                //ref birthday_month,
                //ref birthday_day,
                //ref birthday_year,
                //ref captcha_persist_data,
                //ref captcha_session,
                //ref extra_challenge_params,
                //ref recaptcha_public_key,
                //ref authp_pisg_nonce_tt,
                //ref authp,
                //ref psig,
                //ref nonce,
                //ref tt,
                //ref time,
                //ref challenge,
                //ref CaptchaSummit);

                // if (email.Split(':').Length > 5)
                // {
                //     proxyAddress = email.Split(':')[2];
                //     proxyPort = email.Split(':')[3];
                //     proxyUsername = email.Split(':')[4];
                //     proxyPassword = email.Split(':')[5];
                //     //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
                // }
                // else if (email.Split(':').Length == 4)
                // {
                //     //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
                //     proxyAddress = email.Split(':')[2];
                //     proxyPort = email.Split(':')[3];
                // } 
                #endregion

                 GetCaptchaImageMultiModified(email, ref HttpHelper, ref post_form_id, ref lsd,
               ref reg_instance,
               ref firstname,
               ref lastname,
               ref reg_email__,
               ref reg_email_confirmation__,
               ref reg_passwd__,
               ref sex,
               ref birthday_month,
               ref birthday_day,
               ref birthday_year,
               ref captcha_persist_data,
               ref captcha_session,
               ref extra_challenge_params,
               ref recaptcha_public_key,
               ref authp_pisg_nonce_tt,
               ref authp,
               ref psig,
               ref nonce,
               ref tt,
               ref time,
               ref challenge,
               ref CaptchaSummit);

                 if (email.Split(':').Length > 5)
                 {
                     proxyAddress = email.Split(':')[2];
                     proxyPort = email.Split(':')[3];
                     proxyUsername = email.Split(':')[4];
                     proxyPassword = email.Split(':')[5];
                     //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
                 }
                 else if (email.Split(':').Length == 4)
                 {
                     //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
                     proxyAddress = email.Split(':')[2];
                     proxyPort = email.Split(':')[3];
                 }

                //if (!string.IsNullOrEmpty(captchaSrcMulti))
                //{
                    #region Old Captcha Method
                 //   if (UseDBC)
                 //   {
                 //       System.Net.WebClient webClient = new System.Net.WebClient();
                 //       byte[] imageBytes = webClient.DownloadData(captchaSrcMulti);

                 //       //Getting Captcha Text
                 //       string captchaText = DecodeDBC(new string[] { Globals.DBCUsername, Globals.DBCPassword, "" }, imageBytes);

                 //       if (!string.IsNullOrEmpty(captchaText))
                 //       {
                 //           SummitCaptchaMulti(captchaText, ref HttpHelper, post_form_id, lsd,
                 //reg_instance,
                 //firstname,
                 //lastname,
                 //reg_email__,
                 //reg_email_confirmation__,
                 //reg_passwd__,
                 //sex,
                 //birthday_month,
                 //birthday_day,
                 //birthday_year,
                 //captcha_persist_data,
                 //captcha_session,
                 //extra_challenge_params,
                 //recaptcha_public_key,
                 //authp_pisg_nonce_tt = null,
                 //authp,
                 //psig,
                 //nonce,
                 //tt,
                 //time,
                 //challenge,
                 //CaptchaSummit);
                 //       }
                 //       else
                 //       {
                 //           AddToListBox("Captcha text NULL for : " + Email);
                 //       }

                 //   }
                 //   else if (UseDeCaptcher)
                 //   {
                 //       DownloadImageViaWebClient(captchaSrcMulti, Email.Split('@')[0]);
                 //       System.Threading.Thread.Sleep(15000);

                 //       //Getting Captcha Text using Decaptcher
                 //       string Captchatext = decaptcha(decaptchaImagePath + "\\decaptcha" + Email.Split('@')[0]);

                 //       try
                 //       {
                 //           File.Delete(decaptchaImagePath + "\\decaptcha" + Email.Split('@')[0]);
                 //       }
                 //       catch (Exception) { }


                 //       if (!string.IsNullOrEmpty(Captchatext))
                 //       {
                 //           SummitCaptchaMulti(Captchatext, ref HttpHelper, post_form_id, lsd,
                 //reg_instance,
                 //firstname,
                 //lastname,
                 //reg_email__,
                 //reg_email_confirmation__,
                 //reg_passwd__,
                 //sex,
                 //birthday_month,
                 //birthday_day,
                 //birthday_year,
                 //captcha_persist_data,
                 //captcha_session,
                 //extra_challenge_params,
                 //recaptcha_public_key,
                 //authp_pisg_nonce_tt = null,
                 //authp,
                 //psig,
                 //nonce,
                 //tt,
                 //time,
                 //challenge,
                 //CaptchaSummit);
                 //       }
                 //       else
                 //       {
                 //           AddToListBox("Captcha text NULL for : " + Email);
                 //       }
                 //   } 
                    #endregion

                    //string url_Registration = "https://www.facebook.com/ajax/register.php?__a=5&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&asked_to_login=0&ab_test_data=&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&recaptcha_challenge_field=" + challenge + "&captcha_response=" + "" + "&ignore=captcha|pc&__user=0";
                 string url_Registration = "https://www.facebook.com/ajax/register.php?__a=4&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&asked_to_login=0&ab_test_data=AAAAAAAAAAAA%2FA%2FAAAAA%2FAAAAAAAAAAAAAAAAAAAA%2FAA%2FfAAfABAAD&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&captcha_response=" + "" + "&ignore=captcha%7Cpc&__user=0";

                    string res_Registration = HttpHelper.getHtmlfromUrl(new Uri(url_Registration));

                    //reg_passwd__ = Uri.UnescapeDataString(reg_passwd__);

                    FinishRegistrationMultiThreaded(res_Registration, Email, Password, proxyAddress, proxyPort, proxyUsername, proxyPassword, ref HttpHelper);
                //}
            }
            catch (Exception ex)
            {
                AddToListBox(ex.Message);
            }
        }

        private void FinishRegistrationMultiThreaded(string responseRegistration, string email, string password, string proxyAddress, string proxyPort, string proxyUser, string proxyPassword, ref GlobusHttpHelper HttpHelper)
        {
            if (!string.IsNullOrEmpty(responseRegistration) && responseRegistration.Contains("registration_succeeded"))
            {
                AddToListBox("Registration Succeeded");
                try
                {
                    string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + email));

                    /// JS, CSS, Image Requests
                    RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);
                }
                catch { };

                VerifiyAccountMultiThreaded(email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);
            }
            //else if (!string.IsNullOrEmpty(responseRegistration) && responseRegistration.Contains("It looks like you already have an account on Facebook"))
            else
            {
                AddToListBox("It looks like you already have an account on Facebook");
                try
                {
                    string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + email));

                    /// JS, CSS, Image Requests
                    RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);
                }
                catch { };

                VerifiyAccountMultiThreaded(email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);
            }


            #region Old Code
            //else if (!string.IsNullOrEmpty(responseRegistration) && responseRegistration.Contains("The text you entered didn't match the security check"))
            //{
            //    //AddToListBox("The text you entered didn't match the security check. Retrying..");
            //    //accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
            //    ////And we don't want to switch the account now
            //    //GoToNextAccount();
            //}
            //else if (!string.IsNullOrEmpty(responseRegistration) && responseRegistration.Contains("You took too much time"))
            //{
            //    //AddToListBox("You took too much time in submitting captcha. Retrying..");
            //    //accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
            //    ////And we don't want to switch the account now
            //    //GoToNextAccount();
            //}
            //else if (!string.IsNullOrEmpty(responseRegistration) && responseRegistration.Contains("to an existing account"))
            //{
            //    //AddToListBox("This email is associated to an existing account");
            //    //accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
            //    ////And we don't want to switch the account now
            //    //GoToNextAccount();
            //}
            //else
            //{
            //    if (retryCounter <= 2)
            //    {
            //        ////AddToListBox("Error in submitting captcha. Retrying..");
            //        //accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
            //        //And we don't want to switch the account now
            //        ////GoToNextAccount();
            //    }
            //    else
            //    {
            //        //retryCounter = 0;  //Reset the retryCounter as we're switching account now
            //        //AddToListBox("Couldn't create account with " + Email + "---Switching to next account");
            //        //GoToNextAccount();
            //    }
            //} 
            #endregion
        }

        /// <summary>
        /// Email Verifies account once successfully created
        /// Also inserts account to Database
        /// Verifies Yahoo, Gmail & Hotmail
        /// </summary>
        public void VerifiyAccountMultiThreaded(string email, string password, string proxyAddress, string proxyPort, string proxyUser, string proxyPassword, ref GlobusHttpHelper HttpHelper)
        {
            try
            {
                POPClient popClient = new POPClient();

                AddToListBox("Verifying through Email: " + email);
                AddToListBox("It may take some time, please wait...");

                #region Gmail
                if (email.Contains("@gmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop.gmail.com", int.Parse("995"), true);
                    popClient.Authenticate(email, password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                        string subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    EmailVerificationMultithreaded(href1, staticUrl, email_open_log_picUrl, email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }
                        else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    EmailVerificationMultithreaded(href1, staticUrl, email_open_log_picUrl, email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }

                    }
                }
                #endregion

                #region Yahoo
                else if (email.Contains("@yahoo"))
                {
                    ChilkatIMAP imap = new ChilkatIMAP();

                    imap.proxyAddress = proxyAddress;
                    imap.proxyPort = proxyPort;
                    imap.proxyUser = proxyUser;
                    imap.proxyPass = proxyPassword;
                    imap.GetFBMails(email, password);
                }



                #endregion

                #region Hotmail
                else if (email.Contains("@hotmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop3.live.com", int.Parse("995"), true);
                    popClient.Authenticate(email, password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                        string subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    EmailVerificationMultithreaded(href1, staticUrl, email_open_log_picUrl, email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }
                        else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    EmailVerificationMultithreaded(href1, staticUrl, email_open_log_picUrl, email, password, proxyAddress, proxyPort, proxyUser, proxyPassword, ref HttpHelper);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                if (ex.Message.Contains("InvalidPasswordException"))
                {
                    //AddToListBox("Invalid Password :" + Email);
                    AddToListBox(ex.Message + "--- " + email);
                }
            }
            finally
            {
                //Write to text file
                //Also insert in Database
                try
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(email + ":" + password + ":" + proxyAddress + ":" + proxyPort + ":" + proxyUser + ":" + proxyPassword, Path.Combine(Globals.FD_DesktopPath, "CreatedAccounts.txt"));
                    DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + email + "','" + password + "','" + proxyAddress + "','" + proxyPort + "','" + proxyUser + "','" + proxyPassword + "','" + "" + "','" + "" + "','" + AccountStatus.Status(ProfileStatus.AccountCreated) + "')", "tb_FBAccount");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Makes Http Request to Confirmation URL from Mail, also requests other JS, CSS URLs
        /// </summary>
        /// <param name="ConfirmationUrl"></param>
        /// <param name="gif"></param>
        /// <param name="logpic"></param>
        public void EmailVerificationMultithreaded(string ConfirmationUrl, string gif, string logpic, string email, string password, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, ref GlobusHttpHelper HttpHelper)
        {
            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            string pgSrc_ConfirmationUrl = HttpHelper.getHtmlfromUrlProxy(new Uri(ConfirmationUrl), proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            string valueLSD = "name=" + "\"lsd\"";
            string pgSrc_Login = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            int startIndex = pgSrc_Login.IndexOf(valueLSD) + 18;
            string value = pgSrc_Login.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            string ResponseLogin = HttpHelper.postFormDataProxy(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + email.Split('@')[0] + "%40" + email.Split('@')[1] + "&pass=" + password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            pgSrc_ConfirmationUrl = HttpHelper.getHtmlfromUrl(new Uri(ConfirmationUrl));

            try
            {
                string pgSrc_Gif = HttpHelper.getHtmlfromUrl(new Uri(gif));
            }
            catch { }
            try
            {
                string pgSrc_Logpic = HttpHelper.getHtmlfromUrl(new Uri(logpic + "&s=a"));
            }
            catch { }
            try
            {
                string pgSrc_Logpic = HttpHelper.getHtmlfromUrl(new Uri(logpic));
            }
            catch { }

            //** User Id ***************//////////////////////////////////
            string UsreId = string.Empty;
            if (string.IsNullOrEmpty(UsreId))
            {
                UsreId = GlobusHttpHelper.ParseJson(ResponseLogin, "user");
            }

            //*** Post Data **************//////////////////////////////////
            string fb_dtsg = GlobusHttpHelper.GetParamValue(ResponseLogin, "fb_dtsg");//pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);
            if (string.IsNullOrEmpty(fb_dtsg))
            {
                fb_dtsg = GlobusHttpHelper.ParseJson(ResponseLogin, "fb_dtsg");
            }

            string post_form_id = GlobusHttpHelper.GetParamValue(ResponseLogin, "post_form_id");//pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            if (string.IsNullOrEmpty(post_form_id))
            {
                post_form_id = GlobusHttpHelper.ParseJson(ResponseLogin, "post_form_id");
            }

            string pgSrc_email_confirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            string pgSrc_contact_importer = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));


            #region Skipping Code

            ///Code for skipping additional optional Page
            try
            {
                string phstamp = "165816812085115" + RandomNumberGenerator.GenerateRandom(10848130, 10999999);

                string postDataSkipFirstStep = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=friend_requests&next_step_name=contact_importer&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp;

                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), postDataSkipFirstStep);
                Thread.Sleep(1000);
            }
            catch { }

            pgSrc_contact_importer = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted/?step=contact_importer"));


            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not

            //string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;

            //if (pgSrc_contact_importer.Contains("Are your friends already on Facebook?") && pgSrc_contact_importer.Contains("Skip this step"))
            if (true)
            {
                string phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);

                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=contact_importer&next_step_name=classmates_coworkers&previous_step_name=friend_requests&skip=Skip%20this%20step&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp + "";
                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                pgSrc_contact_importer = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));

                Thread.Sleep(1000);

                //pgSrc_contact_importer = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted/?step=classmates_coworkers"));
            }
            //if ((pgSrc_contact_importer.Contains("Fill out your Profile Info") || pgSrc_contact_importer.Contains("Fill out your Profile info")) && pgSrc_contact_importer.Contains("Skip"))
            if (true)
            {
                string phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);

                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=info&hs[school][id][0]=&hs[school][text][0]=&hs[start_year][text][0]=-1&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[start_year][text][0]=-1&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp + "";
                string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                ///Post Data Parsing
                Dictionary<string, string> lstfriend_browser_id = new Dictionary<string, string>();

                string[] initFriendArray = Regex.Split(postRes, "FriendStatus.initFriend");

                int tempCount = 0;
                foreach (string item in initFriendArray)
                {
                    if (tempCount == 0)
                    {
                        tempCount++;
                        continue;
                    }
                    if (tempCount > 0)
                    {
                        int startIndx = item.IndexOf("(\\") + "(\\".Length + 1;
                        int endIndx = item.IndexOf("\\", startIndx);
                        string paramValue = item.Substring(startIndx, endIndx - startIndx);
                        lstfriend_browser_id.Add("friend_browser_id[" + (tempCount - 1) + "]=", paramValue);
                        tempCount++;
                    }
                }

                string partPostData = string.Empty;
                foreach (var item in lstfriend_browser_id)
                {
                    partPostData = partPostData + item.Key + item.Value + "&";
                }

                phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);

                string newPostData1 = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=pymk&hs[school][id][0]=&hs[school][text][0]=&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&" + partPostData + "phstamp=" + phstamp + "";//"post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=classmates_coworkers&next_step_name=upload_profile_pic&previous_step_name=contact_importer&current_pane=pymk&friend_browser_id[0]=100002869910855&friend_browser_id[1]=100001857152486&friend_browser_id[2]=575678600&friend_browser_id[3]=100003506761599&friend_browser_id[4]=563402235&friend_browser_id[5]=1268675170&friend_browser_id[6]=1701838026&friend_browser_id[7]=623640106&friend_browser_id[8]=648873235&friend_browser_id[9]=100000151781814&friend_browser_id[10]=657007597&friend_browser_id[11]=1483373867&friend_browser_id[12]=778266161&friend_browser_id[13]=1087830021&friend_browser_id[14]=100001333876108&friend_browser_id[15]=100000534308531&friend_browser_id[16]=1213205246&friend_browser_id[17]=45608778&friend_browser_id[18]=100003080150820&friend_browser_id[19]=892195716&friend_browser_id[20]=100001238774509&friend_browser_id[21]=45602360&friend_browser_id[22]=100000054900916&friend_browser_id[23]=100001308090108&friend_browser_id[24]=100000400766182&friend_browser_id[25]=100001159247338&friend_browser_id[26]=1537081666&friend_browser_id[27]=100000743261988&friend_browser_id[28]=1029373920&friend_browser_id[29]=1077680976&friend_browser_id[30]=100000001266475&friend_browser_id[31]=504487658&friend_browser_id[32]=82600225&friend_browser_id[33]=1023509811&friend_browser_id[34]=100000128061486&friend_browser_id[35]=100001853125513&friend_browser_id[36]=576201748&friend_browser_id[37]=22806492&friend_browser_id[38]=100003232772830&friend_browser_id[39]=1447942875&friend_browser_id[40]=100000131241521&friend_browser_id[41]=100002076794734&friend_browser_id[42]=1397169487&friend_browser_id[43]=1457321074&friend_browser_id[44]=1170969536&friend_browser_id[45]=18903839&friend_browser_id[46]=695329369&friend_browser_id[47]=1265734280&friend_browser_id[48]=698096805&friend_browser_id[49]=777678515&friend_browser_id[50]=529685319&hs[school][id][0]=&hs[school][text][0]=&hs[year][text][0]=-1&hs[entry_id][0]=&college[entry_id][0]=&college[school][id][0]=0&college[school][text][0]=&college[year][text][0]=-1&college[type][0]=college&work[employer][id][0]=0&work[employer][text][0]=&work[entry_id][0]=&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=100003556207009&phstamp=1658167541109987992266";
                string postRes1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData1);

                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));

                Thread.Sleep(1000);

                //pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));

                string image_Get = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/images/wizard/nuxwizard_profile_picture.gif"));

                try
                {
                    phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);
                    string newPostData2 = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step=upload_profile_pic&step_name=upload_profile_pic&previous_step_name=classmates_coworkers&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp + "";
                    string postRes2 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData2);
                }
                catch { }
                try
                {
                    phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);
                    string newPostData3 = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step=upload_profile_pic&step_name=upload_profile_pic&previous_step_name=classmates_coworkers&submit=Save%20%26%20Continue&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp + "";
                    string postRes3 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData3);
                }
                catch { }

            }
            if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            {
                string phstamp = "16581677684757" + RandomNumberGenerator.GenerateRandom(5104244, 9999954);
                string newPostData = "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&step_name=upload_profile_pic&previous_step_name=classmates_coworkers&skip=Skip&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId + "&phstamp=" + phstamp + "";
                try
                {
                    string postRes = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/growth/nux/wizard/steps.php?__a=1"), newPostData);

                    pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
                    pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php?ref=wizard"));
                }
                catch { }

            }
            #endregion
            if (pageSrc3.Contains("complete the sign-up process"))
            {
            }
            if (pgSrc_contact_importer.Contains("complete the sign-up process"))
            {
            }
            #endregion

            ////**Post Message For User***********************/////////////////////////////////////////////////////

            try
            {

                string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

                if (pageSourceHome.Contains("complete the sign-up process"))
                {
                    Console.WriteLine("Account is not verified for : " + email);
                }
                else
                {
                }
            }
            catch { }

            AddToListBox("Email verification completed for : " + email);
            //LoggerVerify("Email verification completed for : " + Email);
        }

        /// <summary>
        /// Returns Captcha Image from Facebook
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public string GetCaptchaImageMulti(string email, ref GlobusHttpHelper HttpHelper, ref string post_form_id, ref string lsd,
           ref string reg_instance,
           ref string firstname,
           ref string lastname,
           ref string reg_email__,
           ref string reg_email_confirmation__,
           ref string reg_passwd__,
           ref string sex,
           ref string birthday_month,
           ref string birthday_day,
           ref string birthday_year,
           ref string captcha_persist_data,
           ref string captcha_session,
           ref string extra_challenge_params,
           ref string recaptcha_public_key,
           ref string authp_pisg_nonce_tt,
           ref string authp,
           ref string psig,
           ref string nonce,
           ref string tt,
           ref string time,
           ref string challenge,
           ref string CaptchaSummit)
        {
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUsername = string.Empty;
            string proxyPassword = string.Empty;

            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Email = string.Empty;
            string Password = string.Empty;
            string DOB = string.Empty;

            Email = email.Split(':')[0];
            Password = email.Split(':')[1];

            if (email.Split(':').Length > 5)
            {
                proxyAddress = email.Split(':')[2];
                proxyPort = email.Split(':')[3];
                proxyUsername = email.Split(':')[4];
                proxyPassword = email.Split(':')[5];
                //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
            }
            else if (email.Split(':').Length == 4)
            {
                //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
                proxyAddress = email.Split(':')[2];
                proxyPort = email.Split(':')[3];
            }

            if (listFirstName.Count > 0)
            {
                try
                {
                    FirstName = listFirstName[RandomNumberGenerator.GenerateRandom(0, listFirstName.Count)];
                }
                catch (Exception ex)
                {
                    FirstName = string.Empty;
                }
            }
            if (listLastName.Count > 0)
            {
                try
                {
                    LastName = listLastName[RandomNumberGenerator.GenerateRandom(0, listLastName.Count)];
                }
                catch (Exception ex)
                {
                    LastName = string.Empty;
                }
            }

           


            #region Get Params

            //string post_form_id = string.Empty;
            //string lsd = string.Empty;
            //string reg_instance = string.Empty;
            //string firstname = string.Empty;
            //string lastname = string.Empty;
            //string reg_email__ = string.Empty;
            //string reg_email_confirmation__ = string.Empty;
            //string reg_passwd__ = string.Empty;
            //string sex = string.Empty;
            //string birthday_month = string.Empty;
            //string birthday_day = string.Empty;
            //string birthday_year = string.Empty;
            //string captcha_persist_data = string.Empty;
            //string captcha_session = string.Empty;
            //string extra_challenge_params = string.Empty;
            //string recaptcha_public_key = string.Empty;
            //string authp_pisg_nonce_tt = null;
            //string authp = string.Empty;
            //string psig = string.Empty;
            //string nonce = string.Empty;
            //string tt = string.Empty;
            //string time = string.Empty;
            //string challenge = string.Empty;
            //string CaptchaSummit = string.Empty;

            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            AddToListBox("Fetching Captcha");
            LogFacebookCreator("Fetching Captcha");
            //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();  //Create new instance

            string pageSource = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.facebook.com/"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);


            #region CSS, JS, & Pixel requests to avoid Socket Detection

            ///JS, CSS, Image Requests
            //RequestJSCSSIMG(pageSource, ref HttpHelper);
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSource, ref HttpHelper);


            //try
            //{
            //    string req1 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yC/r/6n91uRFZJAi.js"));
            //}
            //catch (Exception)
            //{
            //}
            //try
            //{
            //    string req2 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yd/r/dpT-tcRYFZy.js"));
            //}
            //catch (Exception)
            //{
            //}

            ///Pixel request
            string reg_instanceValue = GetParamValue(pageSource, "reg_instance");
            //string asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 4000).ToString();
            string asyncSignal = string.Empty;
            try
            {
                asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 8000).ToString();
            }
            catch (Exception)
            {


            }
            string pixel = HttpHelper.getHtmlfromUrl(new Uri("http://pixel.facebook.com/ajax/register/logging.php?action=form_focus&reg_instance=" + reg_instanceValue + "&asyncSignal=" + asyncSignal + "&__user=0"));
            #endregion



            //Delay after loading Sign Up
            //Thread.Sleep(12000);

            //string Response1 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string Response12 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string pageSource12 = HttpHelper.getHtmlfromUrlProxy(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yS/r/STeWPW2kh0m.png"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //**** update by ritesh 20-9-11  *****/////////////////////////////////

            //*** For post_form_id ********////////////////////////////////////////////////
            AddToListBox("searching the captcha data" + Email);
            if (pageSource.Contains("post_form_id"))
            {
                string post_id = pageSource.Substring(pageSource.IndexOf("post_form_id"), 200);
                string[] Arr1 = post_id.Split('"');
                post_form_id = Arr1[2];
            }
            if (pageSource.Contains("lsd"))
            {
                string lsd_val = pageSource.Substring(pageSource.IndexOf("lsd"), 100);
                string[] Arr_lsd = lsd_val.Split('"');
                lsd = Arr_lsd[2];
            }
            if (pageSource.Contains("reg_instance"))
            {
                string reg_instance_val = pageSource.Substring(pageSource.IndexOf("reg_instance"), 200);
                string[] Arr_reg = reg_instance_val.Split('"');
                reg_instance = Arr_reg[4];
            }
            firstname = FirstName.Replace(" ", "%20");
            lastname = LastName.Replace(" ", "%20");
            reg_email__ = Email.Replace("@", "%40");
            reg_email_confirmation__ = Email.Replace("@", "%40");
            reg_passwd__ = Password.Replace("@", "%40");
            sex = SexSelect;
            birthday_month = RandomNumberGenerator.GenerateRandom(1, 12).ToString();
            birthday_day = RandomNumberGenerator.GenerateRandom(1, 28).ToString();
            birthday_year = RandomNumberGenerator.GenerateRandom(1980, 1990).ToString();


            if (pageSource.Contains("captcha_persist_data"))
            {
                string captcha_persist_data_val = pageSource.Substring(pageSource.IndexOf("captcha_persist_data"), 500);
                string[] Arr_captcha_persist_data_val = captcha_persist_data_val.Split('"');
                captcha_persist_data = Arr_captcha_persist_data_val[4];
            }
            if (pageSource.Contains("captcha_session"))
            {
                string captcha_session_val = pageSource.Substring(pageSource.IndexOf("captcha_session"), 200);
                string[] Arr_captcha_session_val = captcha_session_val.Split('"');
                captcha_session = Arr_captcha_session_val[4];
            }
            if (pageSource.Contains("extra_challenge_params"))
            {
                string extra_challenge_params_val = pageSource.Substring(pageSource.IndexOf("extra_challenge_params"), 500);
                string[] Arr_extra_challenge_params_val = extra_challenge_params_val.Split('"');
                authp_pisg_nonce_tt = Arr_extra_challenge_params_val[4];
                extra_challenge_params = Arr_extra_challenge_params_val[4];
                extra_challenge_params = extra_challenge_params.Replace("=", "%3D");
                extra_challenge_params = extra_challenge_params.Replace("&amp;", "%26");
            } 
             
            #endregion

            //AddToListBox("get the first url" + Email);

            ///Delay after filling info
            //int delay = 0;
            //try
            //{
            //    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay) * 1000;
            //    if (delay < 4000)
            //    {
            //        delay = RandomNumberGenerator.GenerateRandom(8000, 12000);
            //    }
            //}
            //catch (Exception)
            //{
            //}

            //AddToListBox("Delaying for " + delay / 1000 + " seconds");
            //Thread.Sleep(delay);
            //Thread.Sleep(RandomNumberGenerator.GenerateRandom(4000, 8000));


            //////////////////////////////*****Gets Captcha URL****////////////////////////////////////////////////
            string pageSourceCaptcha = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/ajax/register.php?__a=4&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&ab_test_data=&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&captcha_response=&ignore=captcha%7Cpc&__user=0"));

            //JS, CSS, Image Requests
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha, ref HttpHelper);

            if (!pageSourceCaptcha.Contains("There is an existing account associated with this email"))
            {

                if (pageSource.Contains("RegUtil.recaptcha_public_key"))
                {
                    string recaptcha_public_key_val = pageSource.Substring(pageSource.IndexOf("RegUtil.recaptcha_public_key"), 200);
                    string[] Arr_recaptcha_public_key = recaptcha_public_key_val.Split('"');
                    recaptcha_public_key = Arr_recaptcha_public_key[1];
                }
                if (authp_pisg_nonce_tt != null)
                {
                    string[] ArrpisgTemp = authp_pisg_nonce_tt.Split('=');
                    authp = ArrpisgTemp[1];
                    authp = authp.Replace("&amp;psig", "");
                    psig = ArrpisgTemp[2];
                    psig = psig.Replace("&amp;nonce", "");
                    nonce = ArrpisgTemp[3];
                    nonce = nonce.Replace("&amp;tt", "");
                    tt = ArrpisgTemp[4];
                    tt = tt.Replace("&amp;time", "");
                    time = ArrpisgTemp[5];
                    time = time.Substring(0, 10);
                }

                AddToListBox("loading captcha" + Email);

                string pageSourceCaptcha1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.google.com/recaptcha/api/challenge?k=" + recaptcha_public_key + "&ajax=1&xcachestop=0.4159115800857506&authp=" + authp + "&psig=" + psig + "&nonce=" + nonce + "&tt=" + tt + "&time=" + time + "&new_audio_default=1"));

                ////JS, CSS, Image Requests
                //RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha1, ref HttpHelper);

                // string challenge = string.Empty;
                if (pageSourceCaptcha1.Contains("challenge"))
                {

                    string challenge_val = pageSourceCaptcha1.Substring(pageSourceCaptcha1.IndexOf("challenge"), 300);
                    string[] Arr_challenge = challenge_val.Split('\'');
                    challenge = Arr_challenge[1];
                }

                return "http://www.google.com/recaptcha/api/image?c=" + challenge;
                //string pageSourceCaptcha2 = HttpHelper.getHtmlfromUrlImage(new Uri("http://www.google.com/recaptcha/api/image?c=" + challenge)); 
            }
            else
            {
                //responseMessage = "There is an existing account with " + Email;
                AddToListBox("There is an existing account with " + Email);
                return null;
            }
        }

        /// <summary>
        /// Returns Captcha Image from Facebook
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public void GetCaptchaImageMultiModified(string email, ref GlobusHttpHelper HttpHelper, ref string post_form_id, ref string lsd,
           ref string reg_instance,
           ref string firstname,
           ref string lastname,
           ref string reg_email__,
           ref string reg_email_confirmation__,
           ref string reg_passwd__,
           ref string sex,
           ref string birthday_month,
           ref string birthday_day,
           ref string birthday_year,
           ref string captcha_persist_data,
           ref string captcha_session,
           ref string extra_challenge_params,
           ref string recaptcha_public_key,
           ref string authp_pisg_nonce_tt,
           ref string authp,
           ref string psig,
           ref string nonce,
           ref string tt,
           ref string time,
           ref string challenge,
           ref string CaptchaSummit)
        {
            string proxyAddress = string.Empty;
            string proxyPort = string.Empty;
            string proxyUsername = string.Empty;
            string proxyPassword = string.Empty;

            string FirstName = string.Empty;
            string LastName = string.Empty;
            string Email = string.Empty;
            string Password = string.Empty;
            string DOB = string.Empty;

            Email = email.Split(':')[0];
            Password = email.Split(':')[1];

            if (email.Split(':').Length > 5)
            {
                proxyAddress = email.Split(':')[2];
                proxyPort = email.Split(':')[3];
                proxyUsername = email.Split(':')[4];
                proxyPassword = email.Split(':')[5];
                //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
            }
            else if (email.Split(':').Length == 4)
            {
                //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
                proxyAddress = email.Split(':')[2];
                proxyPort = email.Split(':')[3];
            }

            FirstName = firstname;
            LastName = lastname;

            if (chkRandomFirstAndLastNames.Checked)
            {
                #region Random First & Last Names
                if (listFirstName.Count > 0)
                {
                    try
                    {
                        FirstName = listFirstName[RandomNumberGenerator.GenerateRandom(0, listFirstName.Count)];
                    }
                    catch (Exception ex)
                    {
                        FirstName = string.Empty;
                    }
                }
                if (listLastName.Count > 0)
                {
                    try
                    {
                        LastName = listLastName[RandomNumberGenerator.GenerateRandom(0, listLastName.Count)];
                    }
                    catch (Exception ex)
                    {
                        LastName = string.Empty;
                    }
                }
                #endregion 
            }


            #region Get Params

            //string post_form_id = string.Empty;
            //string lsd = string.Empty;
            //string reg_instance = string.Empty;
            //string firstname = string.Empty;
            //string lastname = string.Empty;
            //string reg_email__ = string.Empty;
            //string reg_email_confirmation__ = string.Empty;
            //string reg_passwd__ = string.Empty;
            //string sex = string.Empty;
            //string birthday_month = string.Empty;
            //string birthday_day = string.Empty;
            //string birthday_year = string.Empty;
            //string captcha_persist_data = string.Empty;
            //string captcha_session = string.Empty;
            //string extra_challenge_params = string.Empty;
            //string recaptcha_public_key = string.Empty;
            //string authp_pisg_nonce_tt = null;
            //string authp = string.Empty;
            //string psig = string.Empty;
            //string nonce = string.Empty;
            //string tt = string.Empty;
            //string time = string.Empty;
            //string challenge = string.Empty;
            //string CaptchaSummit = string.Empty;

            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(proxyPort) && IdCheck.IsMatch(proxyPort))
            {
                intProxyPort = int.Parse(proxyPort);
            }

            AddToListBox("Fetching Captcha");
            LogFacebookCreator("Fetching Captcha");
            //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();  //Create new instance

            string pageSource = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.facebook.com/"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);


            #region CSS, JS, & Pixel requests to avoid Socket Detection

            ///JS, CSS, Image Requests
            //RequestJSCSSIMG(pageSource, ref HttpHelper);
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSource, ref HttpHelper);


            //try
            //{
            //    string req1 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yC/r/6n91uRFZJAi.js"));
            //}
            //catch (Exception)
            //{
            //}
            //try
            //{
            //    string req2 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yd/r/dpT-tcRYFZy.js"));
            //}
            //catch (Exception)
            //{
            //}

            ///Pixel request
            string reg_instanceValue = GetParamValue(pageSource, "reg_instance");
            //string asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 4000).ToString();
            string asyncSignal = string.Empty;
            try
            {
                asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 8000).ToString();
            }
            catch (Exception)
            {


            }
            string pixel = HttpHelper.getHtmlfromUrl(new Uri("http://pixel.facebook.com/ajax/register/logging.php?action=form_focus&reg_instance=" + reg_instanceValue + "&asyncSignal=" + asyncSignal + "&__user=0"));
            #endregion



            //Delay after loading Sign Up
            //Thread.Sleep(12000);

            //string Response1 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string Response12 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string pageSource12 = HttpHelper.getHtmlfromUrlProxy(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yS/r/STeWPW2kh0m.png"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //**** update by ritesh 20-9-11  *****/////////////////////////////////

            //*** For post_form_id ********////////////////////////////////////////////////
            AddToListBox("searching the captcha data" + Email);
            if (pageSource.Contains("post_form_id"))
            {
                string post_id = pageSource.Substring(pageSource.IndexOf("post_form_id"), 200);
                string[] Arr1 = post_id.Split('"');
                post_form_id = Arr1[2];
            }
            if (pageSource.Contains("lsd"))
            {
                string lsd_val = pageSource.Substring(pageSource.IndexOf("lsd"), 100);
                string[] Arr_lsd = lsd_val.Split('"');
                lsd = Arr_lsd[2];
            }
            if (pageSource.Contains("reg_instance"))
            {
                string reg_instance_val = pageSource.Substring(pageSource.IndexOf("reg_instance"), 200);
                string[] Arr_reg = reg_instance_val.Split('"');
                reg_instance = Arr_reg[4];
            }
            firstname = FirstName.Replace(" ", "%20");
            lastname = LastName.Replace(" ", "%20");
            reg_email__ = Email.Replace("@", "%40");
            reg_email_confirmation__ = Email.Replace("@", "%40");
            //reg_passwd__ = Password.Replace("@", "%40");
            reg_passwd__ = Uri.EscapeDataString(Password);//.Replace("@", "%40");
            sex = SexSelect;
            birthday_month = RandomNumberGenerator.GenerateRandom(1, 12).ToString();
            birthday_day = RandomNumberGenerator.GenerateRandom(1, 28).ToString();
            birthday_year = RandomNumberGenerator.GenerateRandom(1980, 1990).ToString();


            if (pageSource.Contains("captcha_persist_data"))
            {
                string captcha_persist_data_val = pageSource.Substring(pageSource.IndexOf("captcha_persist_data"), 500);
                string[] Arr_captcha_persist_data_val = captcha_persist_data_val.Split('"');
                captcha_persist_data = Arr_captcha_persist_data_val[4];
            }
            if (pageSource.Contains("captcha_session"))
            {
                string captcha_session_val = pageSource.Substring(pageSource.IndexOf("captcha_session"), 200);
                string[] Arr_captcha_session_val = captcha_session_val.Split('"');
                captcha_session = Arr_captcha_session_val[4];
            }
            if (pageSource.Contains("extra_challenge_params"))
            {
                string extra_challenge_params_val = pageSource.Substring(pageSource.IndexOf("extra_challenge_params"), 500);
                string[] Arr_extra_challenge_params_val = extra_challenge_params_val.Split('"');
                authp_pisg_nonce_tt = Arr_extra_challenge_params_val[4];
                extra_challenge_params = Arr_extra_challenge_params_val[4];
                extra_challenge_params = extra_challenge_params.Replace("=", "%3D");
                extra_challenge_params = extra_challenge_params.Replace("&amp;", "%26");
            }

            //challenge = GlobusHttpHelper.GetParamValue(pageSource, "extra_challenge_params");

            #endregion

            //AddToListBox("get the first url" + Email);

            ///Delay after filling info
            //int delay = 0;
            //try
            //{
            //    delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay) * 1000;
            //    if (delay < 4000)
            //    {
            //        delay = RandomNumberGenerator.GenerateRandom(8000, 12000);
            //    }
            //}
            //catch (Exception)
            //{
            //}

            //AddToListBox("Delaying for " + delay / 1000 + " seconds");
            //Thread.Sleep(delay);
            //Thread.Sleep(RandomNumberGenerator.GenerateRandom(4000, 8000));


            ////////////////////////////////*****Gets Captcha URL****////////////////////////////////////////////////
            //string pageSourceCaptcha = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/ajax/register.php?__a=4&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&ab_test_data=&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&captcha_response=&ignore=captcha%7Cpc&__user=0"));

            ////JS, CSS, Image Requests
            //RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha, ref HttpHelper);

            //if (!pageSourceCaptcha.Contains("There is an existing account associated with this email"))
            //{

            //    if (pageSource.Contains("RegUtil.recaptcha_public_key"))
            //    {
            //        string recaptcha_public_key_val = pageSource.Substring(pageSource.IndexOf("RegUtil.recaptcha_public_key"), 200);
            //        string[] Arr_recaptcha_public_key = recaptcha_public_key_val.Split('"');
            //        recaptcha_public_key = Arr_recaptcha_public_key[1];
            //    }
            //    if (authp_pisg_nonce_tt != null)
            //    {
            //        string[] ArrpisgTemp = authp_pisg_nonce_tt.Split('=');
            //        authp = ArrpisgTemp[1];
            //        authp = authp.Replace("&amp;psig", "");
            //        psig = ArrpisgTemp[2];
            //        psig = psig.Replace("&amp;nonce", "");
            //        nonce = ArrpisgTemp[3];
            //        nonce = nonce.Replace("&amp;tt", "");
            //        tt = ArrpisgTemp[4];
            //        tt = tt.Replace("&amp;time", "");
            //        time = ArrpisgTemp[5];
            //        time = time.Substring(0, 10);
            //    }

            //    AddToListBox("loading captcha" + Email);

            //    string pageSourceCaptcha1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.google.com/recaptcha/api/challenge?k=" + recaptcha_public_key + "&ajax=1&xcachestop=0.4159115800857506&authp=" + authp + "&psig=" + psig + "&nonce=" + nonce + "&tt=" + tt + "&time=" + time + "&new_audio_default=1"));

            //    ////JS, CSS, Image Requests
            //    //RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha1, ref HttpHelper);

            //    // string challenge = string.Empty;
            //    if (pageSourceCaptcha1.Contains("challenge"))
            //    {

            //        string challenge_val = pageSourceCaptcha1.Substring(pageSourceCaptcha1.IndexOf("challenge"), 300);
            //        string[] Arr_challenge = challenge_val.Split('\'');
            //        challenge = Arr_challenge[1];
            //    }

            //    return "http://www.google.com/recaptcha/api/image?c=" + challenge;
            //    //string pageSourceCaptcha2 = HttpHelper.getHtmlfromUrlImage(new Uri("http://www.google.com/recaptcha/api/image?c=" + challenge)); 
            //}
            //else
            //{
            //    //responseMessage = "There is an existing account with " + Email;
            //    AddToListBox("There is an existing account with " + Email);
            //    return null;
            //}
        }


        private void SummitCaptchaMulti(string captchaText, ref GlobusHttpHelper HttpHelper,  string post_form_id, string lsd,
            string reg_instance,
            string firstname,
            string lastname,
            string reg_email__,
            string reg_email_confirmation__,
            string reg_passwd__,
            string sex,
            string birthday_month,
            string birthday_day,
            string birthday_year,
            string captcha_persist_data,
            string captcha_session,
            string extra_challenge_params,
            string recaptcha_public_key,
            string authp_pisg_nonce_tt,
            string authp,
            string psig,
            string nonce,
            string tt,
            string time,
            string challenge,
            string CaptchaSummit)
        {
            captchaText = captchaText.Replace(" ", "%20");
            string strUrl = "https://www.facebook.com/ajax/register.php?__a=5&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&ab_test_data=DDAHLDHSSh%2FhZdZLLSDAdZDLLHDODLHHHHADAAAANR%2FmiZKICC%2FHB%2F&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&recaptcha_challenge_field=" + challenge + "&captcha_response=" + captchaText + "&__user=0";
            string pageSourceSummitCaptcha = HttpHelper.getHtmlfromUrl(new Uri(strUrl));

            /// JS, CSS, Image Requests
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceSummitCaptcha, ref HttpHelper);

            if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("registration_succeeded"))
            {
                AddToListBox("Registration Succeeded");
                string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + reg_email__));

                // JS, CSS, Image Requests
                //RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);
            }
            else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("It looks like you already have an account on Facebook"))
            {
                strUrl = strUrl.Replace("https://www.facebook.com/ajax/register.php?__a=5&post_form_id=", "https://www.facebook.com/ajax/register.php?__a=6&post_form_id=");
                pageSourceSummitCaptcha = HttpHelper.getHtmlfromUrl(new Uri(strUrl));

                string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + reg_email__));

                // JS, CSS, Image Requests
                //RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);
            }
            else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("The text you entered didn't match the security check"))
            {
                AddToListBox("The text you entered didn't match the security check. Retrying..");
            }
            else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("You took too much time"))
            {
                AddToListBox("You took too much time in submitting captcha. Retrying..");
            }
            else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("to an existing account"))
            {
                AddToListBox("This email is associated to an existing account");
            }
           
            else
            {
                AddToListBox("Couldn't create account with " + reg_email__ + "");
            }
            
        }

       

        private void AddToListBox(string log)
        {
            try
            {
                if (lstLogger.InvokeRequired)
                {
                    lstLogger.Invoke(new MethodInvoker(delegate
                               {
                                   lstLogger.Items.Add(log);
                                   lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                               })); 
                }
                else
                {
                    lstLogger.Items.Add(log);
                    lstLogger.SelectedIndex = lstLogger.Items.Count - 1;
                }
            }
            catch (Exception)
            {
            }
        }

        #region Multithreaded
        ///// <summary>
        ///// This is the Parent Method, it calls all the Required Methods
        ///// Just call this once
        ///// </summary>
        //public void GoToNextAccount(object objImageCount)
        //{
        //    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        //    ///Modified 
        //    if (listEmails[0].Split(':').Length >= 2 && listEmails.Count > 0)
        //    {
        //        accountCounter++;
        //        retryCounter++;
        //        //AddToListBox("Switching to next Account");
        //        GoToNextAccountThread((string)objImageCount);
        //    }
        //    else
        //    {
        //        Console.WriteLine("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
        //                           "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
        //        AddToListBox("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
        //                        "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
        //        MessageBox.Show("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
        //                        "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
        //        this.Close();
        //    }
        //}

        ///// <summary>
        ///// Method GoToNextAccount calls this in Thread createAccountsThread
        ///// </summary>
        //private void GoToNextAccountThread(string imageCount)
        //{
        //    try
        //    {
        //        //this.Invoke(new MethodInvoker(delegate{
        //        //if (accountCounter < listEmails.Count && accountCounter < listFirstName.Count && accountCounter < listLastName.Count)
        //        if (accountCounter < listEmails.Count)
        //        {
        //            if (listEmails[accountCounter].Split(':').Length >= 2)
        //            {
        //                Email = listEmails[accountCounter].Split(':')[0];
        //                Password = listEmails[accountCounter].Split(':')[1];
        //                if (listFirstName.Count > 0)
        //                {
        //                    try
        //                    {
        //                        FirstName = listFirstName[RandomNumberGenerator.GenerateRandom(0, listFirstName.Count)];
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        FirstName = string.Empty;

        //                    }
        //                }
        //                if (listLastName.Count > 0)
        //                {
        //                    try
        //                    {
        //                        LastName = listLastName[RandomNumberGenerator.GenerateRandom(0, listLastName.Count)];
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        LastName = string.Empty;
        //                    }
        //                }

        //                if (listEmails[accountCounter].Split(':').Length > 5)
        //                {
        //                    proxyAddress = listEmails[accountCounter].Split(':')[2];
        //                    proxyPort = listEmails[accountCounter].Split(':')[3];
        //                    proxyUsername = listEmails[accountCounter].Split(':')[4];
        //                    proxyPassword = listEmails[accountCounter].Split(':')[5];
        //                    //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
        //                }
        //                else if (listEmails[accountCounter].Split(':').Length == 4)
        //                {
        //                    //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
        //                    proxyAddress = listEmails[accountCounter].Split(':')[2];
        //                    proxyPort = listEmails[accountCounter].Split(':')[3];
        //                }
        //                else if (listEmails[accountCounter].Split(':').Length < 4)
        //                {
        //                    //MessageBox.Show("Proxies not loaded with emails \n Accounts will be created without proxies");
        //                    //this.Close();
        //                    //return;
        //                }

        //                AddToListBox("Creating Account with " + Email);

        //                string responseMessage = string.Empty;

        //                string captchaSrc = string.Empty;

        //                captchaSrc = GetCaptchaImage(ref responseMessage);

        //                if (string.IsNullOrEmpty(captchaSrc))
        //                {
        //                    AddToListBox(responseMessage + "\n Switching to next account");
        //                    //new Thread(() => { GoToNextAccount(); }).Start();
        //                    //GoToNextAccount();
        //                    return;
        //                }

        //                if (UseDBC)
        //                {
        //                    System.Net.WebClient webClient = new System.Net.WebClient();
        //                    byte[] imageBytes = webClient.DownloadData(captchaSrc);

        //                    //Getting Captcha Text
        //                    string captchaText = DecodeDBC(new string[] { Globals.DBCUsername, Globals.DBCPassword, "" }, imageBytes);

        //                    if (!string.IsNullOrEmpty(captchaText))
        //                    {
        //                        txtCaptcha.Invoke(new MethodInvoker(delegate
        //                        {
        //                            txtCaptcha.Text = captchaText;
        //                        }));
        //                        SummitCaptcha(captchaText);
        //                    }
        //                    else
        //                    {
        //                        AddToListBox("Captcha text NULL for : " + Email);
        //                    }

        //                }
        //                else if (UseDeCaptcher)
        //                {
        //                    DownloadImageViaWebClient(captchaSrc, imageCount);
        //                    System.Threading.Thread.Sleep(15000);

        //                    //Getting Captcha Text using Decaptcher
        //                    string Captchatext = decaptcha(decaptchaImagePath + "\\decaptcha");

        //                    if (!string.IsNullOrEmpty(Captchatext))
        //                    {
        //                        txtCaptcha.Invoke(new MethodInvoker(delegate
        //                        {
        //                            txtCaptcha.Text = Captchatext;
        //                        }));
        //                        SummitCaptcha(Captchatext);
        //                    }
        //                    else
        //                    {
        //                        AddToListBox("Captcha text NULL for : " + Email);
        //                    }
        //                }
        //                else
        //                {
        //                    txtCaptcha.Invoke(new MethodInvoker(delegate
        //                    {
        //                        txtCaptcha.Text = "";
        //                    }));
        //                    pictureCaptcha.Invoke(new MethodInvoker(delegate
        //                    {
        //                        pictureCaptcha.LoadAsync(captchaSrc);
        //                    }));
        //                }
        //            }
        //            else
        //            {
        //                Console.WriteLine("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
        //                            "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
        //                AddToListBox("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
        //                                "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
        //            }
        //            //pictureCaptcha.LoadAsync(captchaSrc);

        //        }
        //        else
        //        {
        //            Console.WriteLine("Please upload more emails and/or First & Last Names");
        //            AddToListBox("Please upload more emails and/or First & Last Names");
        //            MessageBox.Show("Please upload more emails and/or First & Last Names");
        //            return;
        //            //this.Close();
        //        }

        //        //}));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //        if (ex.Message == "Unable to connect to the remote server")
        //        {
        //            AddToListBox("Unable to connect to the remote server");
        //        }
        //        if (ex.Message == "The remote name could not be resolved: 'www.facebook.com'")
        //        {
        //            AddToListBox("The remote name could not be resolved: 'www.facebook.com'");
        //        }
        //        else
        //        {
        //            AddToListBox(ex.Message);
        //        }
        //        //AddToListBox("Switching to next account");
        //        //GoToNextAccount();
        //    }

        //}
        
        #endregion

        #region Commented AccountCreation Single Threaded
        /// <summary>
        /// This is the Parent Method, it calls all the Required Methods
        /// Just call this once
        /// </summary>
        public void GoToNextAccount()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            #region Previous Code
            //if (listEmails[0].Split(':').Length >= 2 && listEmails.Count > 0)
            //{
            //    accountCounter++;
            //    retryCounter++;
            //    //AddToListBox("Switching to next Account");
            //    createAccountsThread = new Thread(GoToNextAccountThread);
            //    createAccountsThread.Start(); 
            //}
            //else
            //{
            //    Console.WriteLine("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
            //                       "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
            //    AddToListBox("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
            //                    "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
            //    MessageBox.Show("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
            //                    "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
            //    this.Close();
            //} 
            #endregion

            ///Modified 
            if (listEmails[0].Split(':').Length >= 2 && listEmails.Count > 0)
            {
                accountCounter++;
                retryCounter++;
                //AddToListBox("Switching to next Account");

                int abortCounter = 0;

                if (createAccountsThread == null)
                {
                    createAccountsThread = new Thread(GoToNextAccountThread);
                }

                ///Stops the createAccountsThread and starts a new instance of createAccountsThread
                new Thread(() =>
                {
                    if (createAccountsThread.ThreadState == ThreadState.Running || createAccountsThread.ThreadState == ThreadState.WaitSleepJoin) { createAccountsThread.Abort(); }
                    while (createAccountsThread.ThreadState == ThreadState.AbortRequested)
                    {
                        //wait a little bit 
                        if (abortCounter < 40)
                        {
                            Thread.Sleep(500);
                            abortCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    createAccountsThread = new Thread(GoToNextAccountThread);
                    createAccountsThread.Start();
                }).Start();

                //createAccountsThread = new Thread(GoToNextAccountThread);
                //createAccountsThread.Start();

            }
            else
            {
                Console.WriteLine("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
                                   "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
                AddToListBox("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
                                "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
                MessageBox.Show("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
                                "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
                this.Close();
            }
        } 
        


        /// <summary>
        /// Method GoToNextAccount calls this in Thread createAccountsThread
        /// </summary>
        private void GoToNextAccountThread()
        {
            try
            {
                //this.Invoke(new MethodInvoker(delegate{
                //if (accountCounter < listEmails.Count && accountCounter < listFirstName.Count && accountCounter < listLastName.Count)
                if (accountCounter < listEmails.Count)
                {
                    if (listEmails[accountCounter].Split(':').Length >= 2)
                    {
                        _Email = listEmails[accountCounter].Split(':')[0];
                        _Password = listEmails[accountCounter].Split(':')[1];
                        if (listFirstName.Count > 0)
                        {
                            try
                            {
                                FirstName = listFirstName[RandomNumberGenerator.GenerateRandom(0, listFirstName.Count)];
                            }
                            catch (Exception ex)
                            {
                                FirstName = string.Empty;

                            }
                        }
                        if (listLastName.Count > 0)
                        {
                            try
                            {
                                LastName = listLastName[RandomNumberGenerator.GenerateRandom(0, listLastName.Count)];
                            }
                            catch (Exception ex)
                            {

                                LastName = string.Empty;
                            }
                        }

                        if (listEmails[accountCounter].Split(':').Length > 5)
                        {
                            _proxyAddress = listEmails[accountCounter].Split(':')[2];
                            _proxyPort = listEmails[accountCounter].Split(':')[3];
                            _proxyUsername = listEmails[accountCounter].Split(':')[4];
                            _proxyPassword = listEmails[accountCounter].Split(':')[5];
                            //AddToListBox("Setting proxy " + proxyAddress + ":" + proxyPort);
                        }
                        else if (listEmails[accountCounter].Split(':').Length == 4)
                        {
                            //MessageBox.Show("Private proxies not loaded with emails \n Accounts will be created with public proxies");
                            _proxyAddress = listEmails[accountCounter].Split(':')[2];
                            _proxyPort = listEmails[accountCounter].Split(':')[3];
                        }
                        else if (listEmails[accountCounter].Split(':').Length < 4)
                        {
                            //MessageBox.Show("Proxies not loaded with emails \n Accounts will be created without proxies");
                            //this.Close();
                            //return;
                        }

                        AddToListBox("Creating Account with " + _Email);

                        string responseMessage = string.Empty;

                        captchaSrc = GetCaptchaImage(ref responseMessage);

                        if (string.IsNullOrEmpty(captchaSrc))
                        {
                            AddToListBox(responseMessage + "\n Switching to next account");
                            //new Thread(() => { GoToNextAccount(); }).Start();
                            GoToNextAccount();
                            return;
                        }

                        if (UseDBC)
                        {
                            System.Net.WebClient webClient = new System.Net.WebClient();
                            byte[] imageBytes = webClient.DownloadData(captchaSrc);

                            //Getting Captcha Text
                            string captchaText = DecodeDBC(new string[] { Globals.DBCUsername, Globals.DBCPassword, "" }, imageBytes);

                            if (!string.IsNullOrEmpty(captchaText))
                            {
                                txtCaptcha.Invoke(new MethodInvoker(delegate
                                {
                                    txtCaptcha.Text = captchaText;
                                }));
                                SummitCaptcha(captchaText);
                            }
                            else
                            {
                                AddToListBox("Captcha text NULL for : " + _Email);
                            }

                        }
                        else if (UseDeCaptcher)
                        {
                            DownloadImageViaWebClient(captchaSrc);
                            System.Threading.Thread.Sleep(15000);

                            //Getting Captcha Text using Decaptcher
                            string Captchatext = decaptcha(decaptchaImagePath + "\\decaptcha");

                            if (!string.IsNullOrEmpty(Captchatext))
                            {
                                txtCaptcha.Invoke(new MethodInvoker(delegate
                                {
                                    txtCaptcha.Text = Captchatext;
                                }));
                                SummitCaptcha(Captchatext);
                            }
                            else
                            {
                                AddToListBox("Captcha text NULL for : " + _Email);
                            }
                        }
                        else
                        {
                            txtCaptcha.Invoke(new MethodInvoker(delegate
                            {
                                txtCaptcha.Text = "";
                            }));
                            pictureCaptcha.Invoke(new MethodInvoker(delegate
                            {
                                pictureCaptcha.LoadAsync(captchaSrc);
                            }));
                        }
                    }
                    else
                    {
                        Console.WriteLine("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
                                    "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
                        AddToListBox("The Email file is in Invalid format. The correct format is \n" + "email:password \n OR" +
                                        "email:password:proxyip:proxyport \n OR \n email:password:proxyip:proxyport:proxyusername:proxypassword");
                    }
                    //pictureCaptcha.LoadAsync(captchaSrc);

                }
                else
                {
                    Console.WriteLine("Please upload more emails and/or First & Last Names");
                    AddToListBox("Please upload more emails and/or First & Last Names");
                    MessageBox.Show("Please upload more emails and/or First & Last Names");
                    return;
                    //this.Close();
                }

                //}));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                if (ex.Message == "Unable to connect to the remote server")
                {
                    AddToListBox("Unable to connect to the remote server");
                }
                if (ex.Message == "The remote name could not be resolved: 'www.facebook.com'")
                {
                    AddToListBox("The remote name could not be resolved: 'www.facebook.com'");
                }
                else
                {
                    AddToListBox(ex.Message);
                }
                AddToListBox("Switching to next account");
                GoToNextAccount();
            }

        }

        private void DownloadImageViaWebClient(string ImageURL)
        {
            WebClient wc = new WebClient();
            wc.DownloadFileAsync(new Uri(ImageURL), decaptchaImagePath + "\\decaptcha");
        }

        #endregion

        /// <summary>
        /// Returns Captcha Image from Facebook
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <returns></returns>
        public string GetCaptchaImage(ref string responseMessage)
        {
            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(_proxyPort) && IdCheck.IsMatch(_proxyPort))
            {
                intProxyPort = int.Parse(_proxyPort);
            }

            AddToListBox("Fetching Captcha");
            LogFacebookCreator("Fetching Captcha");
            HttpHelper = new GlobusHttpHelper();  //Create new instance
            string pageSource = HttpHelper.getHtmlfromUrlProxy(new Uri("http://www.facebook.com/"), _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);


            #region CSS, JS, & Pixel requests to avoid Socket Detection
            ////**** update by ritesh 20-9-11  *****/////////////////////////////////
            #region Commented, done in next line
            ////CSS Request
            //foreach (string item in GetHrefsFromString(pageSource))
            //{
            //    if (item.Contains(".css"))
            //    {
            //        string cssSource = item.Replace(" ", "").Trim();
            //        try
            //        {
            //            string res = HttpHelper.getHtmlfromUrl(new Uri(cssSource));
            //        }
            //        catch (Exception)
            //        {
            //        }
            //    }
            //}

            ////JS Request
            //string[] scriptArr = Regex.Split(pageSource, "/script>");
            //foreach (string item in scriptArr)
            //{
            //    if (item.Contains("http://static.ak.fbcdn.net"))
            //    {
            //        int startIndx = item.LastIndexOf("src=") + "src=".Length + 1;
            //        int endIndx = item.IndexOf(">", startIndx) - 1;
            //        string jsSource = item.Substring(startIndx, endIndx - startIndx);
            //        if (jsSource.StartsWith("http://static.ak.fbcdn.net"))
            //        {
            //            try
            //            {
            //                string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
            //            }
            //            catch (Exception)
            //            {
            //            }
            //        }
            //    }
            //} 
            #endregion

            ///JS, CSS, Image Requests
            //RequestJSCSSIMG(pageSource, ref HttpHelper);
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSource, ref HttpHelper);


            try
            {
                string req1 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yC/r/6n91uRFZJAi.js"));
            }
            catch (Exception)
            {
            }
            try
            {
                string req2 = HttpHelper.getHtmlfromUrl(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yd/r/dpT-tcRYFZy.js"));
            }
            catch (Exception)
            {
            }

            ///Pixel request
            string reg_instanceValue = GetParamValue(pageSource, "reg_instance");
            //string asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 4000).ToString();
            string asyncSignal = string.Empty;
            try
            {
                asyncSignal = RandomNumberGenerator.GenerateRandom(3000, 8000).ToString();
            }
            catch (Exception)
            {
                
               
            }
            string pixel = HttpHelper.getHtmlfromUrl(new Uri("http://pixel.facebook.com/ajax/register/logging.php?action=form_focus&reg_instance=" + reg_instanceValue + "&asyncSignal=" + asyncSignal + "&__user=0")); 
            #endregion

            

            //Delay after loading Sign Up
            //Thread.Sleep(12000);

            //string Response1 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string Response12 = HttpHelper.postFormDataProxy(new Uri("http://ocsp.digicert.com/"), "", proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //string pageSource12 = HttpHelper.getHtmlfromUrlProxy(new Uri("http://static.ak.fbcdn.net/rsrc.php/v1/yS/r/STeWPW2kh0m.png"), proxyAddress, intProxyPort, proxyUsername, proxyPassword);

            //**** update by ritesh 20-9-11  *****/////////////////////////////////

            //*** For post_form_id ********////////////////////////////////////////////////
            AddToListBox("searching the captcha data" + _Email);
            if (pageSource.Contains("post_form_id"))
            {
                string post_id = pageSource.Substring(pageSource.IndexOf("post_form_id"), 200);
                string[] Arr1 = post_id.Split('"');
                post_form_id = Arr1[2];
            }
            if (pageSource.Contains("lsd"))
            {
                string lsd_val = pageSource.Substring(pageSource.IndexOf("lsd"), 100);
                string[] Arr_lsd = lsd_val.Split('"');
                lsd = Arr_lsd[2];
            }
            if (pageSource.Contains("reg_instance"))
            {
                string reg_instance_val = pageSource.Substring(pageSource.IndexOf("reg_instance"), 200);
                string[] Arr_reg = reg_instance_val.Split('"');
                reg_instance = Arr_reg[4];
            }
            firstname = FirstName.Replace(" ", "%20");
            lastname = LastName.Replace(" ", "%20");
            reg_email__ = _Email.Replace("@", "%40");
            reg_email_confirmation__ = _Email.Replace("@", "%40");
            reg_passwd__ = _Password.Replace("@", "%40");
            sex = SexSelect;
            birthday_month = RandomNumberGenerator.GenerateRandom(1, 12).ToString();
            birthday_day = RandomNumberGenerator.GenerateRandom(1, 28).ToString();
            birthday_year = RandomNumberGenerator.GenerateRandom(1980, 1990).ToString();


            if (pageSource.Contains("captcha_persist_data"))
            {
                string captcha_persist_data_val = pageSource.Substring(pageSource.IndexOf("captcha_persist_data"), 500);
                string[] Arr_captcha_persist_data_val = captcha_persist_data_val.Split('"');
                captcha_persist_data = Arr_captcha_persist_data_val[4];
            }
            if (pageSource.Contains("captcha_session"))
            {
                string captcha_session_val = pageSource.Substring(pageSource.IndexOf("captcha_session"), 200);
                string[] Arr_captcha_session_val = captcha_session_val.Split('"');
                captcha_session = Arr_captcha_session_val[4];
            }
            if (pageSource.Contains("extra_challenge_params"))
            {
                string extra_challenge_params_val = pageSource.Substring(pageSource.IndexOf("extra_challenge_params"), 500);
                string[] Arr_extra_challenge_params_val = extra_challenge_params_val.Split('"');
                authp_pisg_nonce_tt = Arr_extra_challenge_params_val[4];
                extra_challenge_params = Arr_extra_challenge_params_val[4];
                extra_challenge_params = extra_challenge_params.Replace("=", "%3D");
                extra_challenge_params = extra_challenge_params.Replace("&amp;", "%26");
            }

            //AddToListBox("get the first url" + Email);

            ///Delay after filling info
            int delay = 0;
            try
            {
                delay = RandomNumberGenerator.GenerateRandom(minDelay, maxDelay)*1000;
                if (delay < 4000)
                {
                    delay = RandomNumberGenerator.GenerateRandom(8000, 12000);
                }
            }
            catch (Exception)
            {
            }

            AddToListBox("Delaying for " + delay/1000 + " seconds");
            Thread.Sleep(delay);

            ////////////////////////////////*****Gets Captcha URL****////////////////////////////////////////////////
            string pageSourceCaptcha = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/ajax/register.php?__a=3&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&ab_test_data=&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&captcha_response=&ignore=captcha%7Cpc&__user=0"));

            //JS, CSS, Image Requests
            RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha, ref HttpHelper);

            if (!pageSourceCaptcha.Contains("There is an existing account associated with this email"))
            {
               
                if (pageSource.Contains("RegUtil.recaptcha_public_key"))
                {
                    string recaptcha_public_key_val = pageSource.Substring(pageSource.IndexOf("RegUtil.recaptcha_public_key"), 200);
                    string[] Arr_recaptcha_public_key = recaptcha_public_key_val.Split('"');
                    recaptcha_public_key = Arr_recaptcha_public_key[1];
                }
                if (authp_pisg_nonce_tt != null)
                {
                    string[] ArrpisgTemp = authp_pisg_nonce_tt.Split('=');
                    authp = ArrpisgTemp[1];
                    authp = authp.Replace("&amp;psig", "");
                    psig = ArrpisgTemp[2];
                    psig = psig.Replace("&amp;nonce", "");
                    nonce = ArrpisgTemp[3];
                    nonce = nonce.Replace("&amp;tt", "");
                    tt = ArrpisgTemp[4];
                    tt = tt.Replace("&amp;time", "");
                    time = ArrpisgTemp[5];
                    time = time.Substring(0, 10); 
                }

                AddToListBox("loading captcha" + _Email);

                string pageSourceCaptcha1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.google.com/recaptcha/api/challenge?k=" + recaptcha_public_key + "&ajax=1&xcachestop=0.4159115800857506&authp=" + authp + "&psig=" + psig + "&nonce=" + nonce + "&tt=" + tt + "&time=" + time + "&new_audio_default=1"));

                ////JS, CSS, Image Requests
                //RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceCaptcha1, ref HttpHelper);

                // string challenge = string.Empty;
                if (pageSourceCaptcha1.Contains("challenge"))
                {

                    string challenge_val = pageSourceCaptcha1.Substring(pageSourceCaptcha1.IndexOf("challenge"), 300);
                    string[] Arr_challenge = challenge_val.Split('\'');
                    challenge = Arr_challenge[1];
                }

                return "http://www.google.com/recaptcha/api/image?c=" + challenge;
                //string pageSourceCaptcha2 = HttpHelper.getHtmlfromUrlImage(new Uri("http://www.google.com/recaptcha/api/image?c=" + challenge)); 
            }
            else
            {
                responseMessage = "There is an existing account with " + _Email;
                AddToListBox("There is an existing account with " + _Email);
                return null;
            }
        }


        private void RequestJSCSSIMG(string pageSource, ref GlobusHttpHelper HttpHelper)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            //CSS Request
            foreach (string item in GetHrefsFromString(pageSource))
            {
                if (item.Contains(".css"))
                {
                    string cssSource = item.Replace(" ", "").Trim();
                    try
                    {
                        string res = HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            string res = HttpHelper.getHtmlfromUrl(new Uri(cssSource));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            //JS Request
            string[] scriptArr = Regex.Split(pageSource, "/script>");
            foreach (string item in scriptArr)
            {
                if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                {
                    int startIndx = item.LastIndexOf("src=") + "src=".Length + 1;
                    int endIndx = item.IndexOf(">", startIndx) - 1;
                    string jsSource = item.Substring(startIndx, endIndx - startIndx);
                    if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                    {
                        try
                        {
                            string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(500);
                            try
                            {
                                string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }
            }

            ///IMG Request
            string[] imageArr = Regex.Split(pageSource, "<img");
            foreach (string item in imageArr)
            {
                if (item.Contains("static.ak.") || item.Contains("profile.ak."))
                {
                    int startIndx = item.IndexOf("src=") + "src=".Length + 1;
                    int endIndx = item.IndexOf("\"", startIndx + 1);
                    string jsSource = item.Substring(startIndx, endIndx - startIndx);
                    if (jsSource.StartsWith("http://static.ak.") || jsSource.StartsWith("https://static.ak.") || jsSource.StartsWith("http://s-static.ak.") || jsSource.StartsWith("https://s-static.ak."))
                    {
                        if (jsSource.Contains(".png") || jsSource.Contains(".gif") || jsSource.Contains(".jpg") || jsSource.Contains(".jpeg"))
                        {
                            try
                            {
                                string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                            }
                            catch (Exception)
                            {
                                Thread.Sleep(500);
                                try
                                {
                                    string res = HttpHelper.getHtmlfromUrl(new Uri(jsSource));
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Gets Param value from PageSource
        /// </summary>
        /// <param name="pgSrc">PageSource</param>
        /// <param name="paramName">Name of Parameter to find value of</param>
        /// <returns></returns>
        private string GetParamValue(string pgSrc, string paramName)
        {
            try
            {
                if (pgSrc.Contains("name='" + paramName + "'"))
                {
                    string param = "name='" + paramName + "'";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endparamName = pgSrc.IndexOf("'", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endparamName - startparamName);
                    return valueparamName;
                }
                else if (pgSrc.Contains("name=\"" + paramName + "\""))
                {
                    string param = "name=\"" + paramName + "\"";
                    int startparamName = pgSrc.IndexOf(param) + param.Length;
                    startparamName = pgSrc.IndexOf("value=", startparamName) + "value=".Length + 1;
                    int endcommentPostID = pgSrc.IndexOf("\"", startparamName);
                    string valueparamName = pgSrc.Substring(startparamName, endcommentPostID - startparamName);
                    return valueparamName;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
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
                        AddToListBox("Decapther Server is overloaded Waiting for 1 sec and re-trying...");
                        //Thread.Sleep(1000);
                        //decaptcha();

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

        private void btnSwitchToNextAccount_Click(object sender, EventArgs e)
        {
            AddToListBox("Switching to next Account");
            GoToNextAccount();
            //createAccountsThread = new Thread(GoToNextAccount);
            //createAccountsThread.Start();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCaptcha.Text))
            {
                SummitCaptcha(txtCaptcha.Text);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (rdoSingleThreaded.Checked)
            {
                //AddToListBox("Switching to next Account");
                //createAccountsThread = new Thread(GoToNextAccount);
                //createAccountsThread.Start(); 

                panelSingleThreaded.Visible = true;
                panelSMultiThreaded.Visible = false;

                GoToNextAccount();
            }
            else if(rdoMassCreator.Checked)
            {
                Regex IdCheck1 = new Regex("^[0-9]*$");

                //if ((string.IsNullOrEmpty(Globals.DeCaptcherUsername) && string.IsNullOrEmpty(Globals.DeCaptcherPassword) && string.IsNullOrEmpty(Globals.DeCaptcherHost) && string.IsNullOrEmpty(Globals.DeCaptcherPort)) && (string.IsNullOrEmpty(Globals.DBCUsername) && string.IsNullOrEmpty(Globals.DBCPassword)))
                //{
                //    MessageBox.Show("Please load Death By Captcha or DeCaptcher Details from Settings Menu");
                //    return;
                //}
                


                int numberOfThreads = 20;

                if (IdCheck1.IsMatch(txtNumberOfThreads.Text) && !string.IsNullOrEmpty(txtNumberOfThreads.Text))
                {
                    numberOfThreads = int.Parse(txtNumberOfThreads.Text);
                }

                //Set ThreadPoolThreads
                ThreadPool.SetMaxThreads(numberOfThreads, 4);

                panelSingleThreaded.Visible = false;
                panelSMultiThreaded.Visible = true;

                StartAccountCreationMultiThreaded();
            }
            else
            {
                MessageBox.Show("Please select an option whether SingleThreaded or MassCreator");
            }
        }


        /// <summary>
        /// Submits captcha, also checks status after submitting
        /// If Required, refreshes the captcha
        /// After 3 unsuccessful trials, swithches to next account
        /// </summary>
        /// <param name="strCaptcha"></param>
        public void SummitCaptcha(string strCaptcha)
        {
            new Thread(() =>
            {
                CaptchaSummit = strCaptcha.Replace(" ", "%20");
                string strUrl = "https://www.facebook.com/ajax/register.php?__a=5&post_form_id=" + post_form_id + "&lsd=" + lsd + "&reg_instance=" + reg_instance + "&locale=en_US&terms=on&abtest_registration_group=1&referrer=&md5pass=&validate_mx_records=1&ab_test_data=DDAHLDHSSh%2FhZdZLLSDAdZDLLHDODLHHHHADAAAANR%2FmiZKICC%2FHB%2F&firstname=" + firstname + "&lastname=" + lastname + "&reg_email__=" + reg_email__ + "&reg_email_confirmation__=" + reg_email_confirmation__ + "&reg_passwd__=" + reg_passwd__ + "&sex=" + sex + "&birthday_month=" + birthday_month + "&birthday_day=" + birthday_day + "&birthday_year=" + birthday_year + "&captcha_persist_data=" + captcha_persist_data + "&captcha_session=" + captcha_session + "&extra_challenge_params=" + extra_challenge_params + "&recaptcha_type=password&recaptcha_challenge_field=" + challenge + "&captcha_response=" + CaptchaSummit + "&__user=0";


                bool ReSubmit = true;
                pagesource:
                string pageSourceSummitCaptcha = HttpHelper.getHtmlfromUrl(new Uri(strUrl));

                /// JS, CSS, Image Requests
                RequestsJSCSSIMG.RequestJSCSSIMG(pageSourceSummitCaptcha, ref HttpHelper);
                
                if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("registration_succeeded"))
                {
                    AddToListBox("Registration Succeeded");
                    string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + _Email));

                    /// JS, CSS, Image Requests
                    RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);

                    VerifiyAccountModified();
                    AddToListBox("Switching to next Account");
                    GoToNextAccount(); 
                }
                else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("It looks like you already have an account on Facebook"))
                {
                    if (ReSubmit)
                    {
                         //ReSubmit = false;
                         //goto pagesource;
                        pageSourceSummitCaptcha = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/settings?ref=mb"));
                    }

                    AddToListBox("It looks like you already have an account on Facebook");
                    string res = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/c.php?email=" + _Email));

                    /// JS, CSS, Image Requests
                    RequestsJSCSSIMG.RequestJSCSSIMG(res, ref HttpHelper);

                    VerifiyAccountModified();
                    AddToListBox("Switching to next Account");
                    GoToNextAccount(); 
                }
                else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("The text you entered didn't match the security check"))
                {
                    AddToListBox("The text you entered didn't match the security check. Retrying..");
                    accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
                                        //And we don't want to switch the account now
                    GoToNextAccount();
                }
                else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("You took too much time"))
                {
                    AddToListBox("You took too much time in submitting captcha. Retrying..");
                    accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
                                       //And we don't want to switch the account now
                    GoToNextAccount();
                }
                else if (!string.IsNullOrEmpty(pageSourceSummitCaptcha) && pageSourceSummitCaptcha.Contains("to an existing account"))
                {
                    AddToListBox("This email is associated to an existing account");
                    accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
                    //And we don't want to switch the account now
                    GoToNextAccount();
                }
                else
                {
                    if (retryCounter <= 2)
                    {
                        AddToListBox("Error in submitting captcha. Retrying..");
                        accountCounter--;  //Decrement Counter as it'll be incremented in next line => GoToNextAccount()
                                           //And we don't want to switch the account now
                        GoToNextAccount(); 
                    }
                    else
                    {
                        retryCounter = 0;  //Reset the retryCounter as we're switching account now
                        AddToListBox("Couldn't create account with " + _Email + "---Switching to next account");
                        GoToNextAccount();
                    }
                }
            }).Start();
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

        private void DownloadImageViaWebClient(string ImageURL, string additionalPath)
        {
            WebClient wc = new WebClient();
            wc.DownloadFileAsync(new Uri(ImageURL), decaptchaImagePath + "\\decaptcha" + additionalPath);
        }


        /// <summary>
        /// Email Verifies account once successfully created
        /// Also inserts account to Database
        /// Verifies Yahoo, Gmail & Hotmail
        /// </summary>
        public void VerifiyAccountModified()
        {
            try
            {
                AddToListBox("Verifying through Email: " + _Email);
                AddToListBox("It may take some time, please wait...");

                #region Gmail
                if (_Email.Contains("@gmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop.gmail.com", int.Parse("995"), true);
                    popClient.Authenticate(_Email, _Password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                        string subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    LoginVerfy(href1, staticUrl, email_open_log_picUrl);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }
                        else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    LoginVerfy(href1, staticUrl, email_open_log_picUrl);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }

                    }
                }
                #endregion

                #region Yahoo
                else if (_Email.Contains("@yahoo"))
                {
                    ChilkatIMAP imap = new ChilkatIMAP();

                    imap.proxyAddress = _proxyAddress;
                    imap.proxyPort = _proxyPort;
                    imap.proxyUser = _proxyUsername;
                    imap.proxyPass = _proxyPassword;
                    imap.GetFBMails(_Email, _Password);
                }



                #endregion

                #region Hotmail
                else if (_Email.Contains("@hotmail"))
                {
                    if (popClient.Connected)
                        popClient.Disconnect();
                    popClient.Connect("pop3.live.com", int.Parse("995"), true);
                    popClient.Authenticate(_Email, _Password);
                    int Count = popClient.GetMessageCount();

                    for (int i = Count; i >= 1; i--)
                    {
                        OpenPOP.MIME.Message Message = popClient.GetMessage(i);

                        string subject = Message.Headers.Subject;

                        if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    LoginVerfy(href1, staticUrl, email_open_log_picUrl);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }
                        else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
                        {
                            foreach (string href in GetUrlsFromString(Message.MessageBody[0]))
                            {
                                try
                                {
                                    string staticUrl = string.Empty;
                                    string email_open_log_picUrl = string.Empty;

                                    string strBody = Message.MessageBody[0];
                                    string[] arr = Regex.Split(strBody, "src=");
                                    foreach (string item in arr)
                                    {
                                        if (!item.Contains("<!DOCTYPE"))
                                        {
                                            if (item.Contains("static"))
                                            {
                                                string[] arrStatic = item.Split('"');
                                                staticUrl = arrStatic[1];
                                            }
                                            if (item.Contains("email_open_log_pic"))
                                            {
                                                string[] arrlog_pic = item.Split('"');
                                                email_open_log_picUrl = arrlog_pic[1];
                                                email_open_log_picUrl = email_open_log_picUrl.Replace("amp;", "");
                                                break;
                                            }
                                        }
                                    }

                                    string href1 = href.Replace("&amp;report=1", "");
                                    href1 = href.Replace("amp;", "");

                                    LoginVerfy(href1, staticUrl, email_open_log_picUrl);

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.StackTrace);
                                }
                            }
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                if (ex.Message.Contains("InvalidPasswordException"))
                {
                    //AddToListBox("Invalid Password :" + Email);
                }
            }
            finally
            {
                ///Write to text file
                //Also insert in Database
                try
                {
                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
                    DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + _Email + "','" + _Password + "','" + _proxyAddress + "','" + _proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + AccountStatus.Status(ProfileStatus.AccountCreated) + "')", "tb_FBAccount");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }   
        }

        

        public List<string> GetUrlsFromString(string HtmlData)
        {
            List<string> lstUrl = new List<string>();
            //var regex = new Regex(@"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
            //var ModifiedString = HtmlData.Replace("\"", " ").Replace("<", " ").Replace(">", " ");
            //foreach (Match url in regex.Matches(ModifiedString))
            //{
            //    lstUrl.Add(url.Value);
            //}

            // *** Change by ritesh 10-9-11 ********************************************
            try
            {
                string strData = HtmlData;
                string[] arr = Regex.Split(strData, "\n");

                foreach (string strhref in arr)
                {
                    if (!strhref.Contains("<!DOCTYPE"))
                    {
                        if (strhref.Contains("http://www.facebook.com/"))
                        {
                            string tempArr = strhref.Replace("\r", "");
                            lstUrl.Add(tempArr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            // *** Change by ritesh 10-9-11 ********************************************
            return lstUrl;
        }

        public List<string> GetHrefsFromString(string HtmlData)
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

        /// <summary>
        /// Makes Http Request to Confirmation URL from Mail, also requests other JS, CSS URLs
        /// </summary>
        /// <param name="ConfemUrl"></param>
        /// <param name="gif"></param>
        /// <param name="logpic"></param>
        public void LoginVerfy(string ConfemUrl, string gif, string logpic)
        {
            Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

            int intProxyPort = 80;
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(_proxyPort) && IdCheck.IsMatch(_proxyPort))
            {
                intProxyPort = int.Parse(_proxyPort);
            }

            string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(ConfemUrl), _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);
            //string PageSourse1 = HttpHelper.getHtmlfromUrlProxy(new Uri(url), "127.0.0.1", 8888, "", "");

            string valueLSD = "name=" + "\"lsd\"";
            string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            int startIndex = pageSource.IndexOf(valueLSD) + 18;
            string value = pageSource.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            //string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Email.Split('@')[0] + "%40" + Email.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
            string ResponseLogin = HttpHelper.postFormDataProxy(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + _Email.Split('@')[0] + "%40" + _Email.Split('@')[1] + "&pass=" + _Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "", _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);
            //string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + "rani.khanna" + "%40" + "hotmail.com" + "&pass=" + "s15121985" + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");
            /////ssss gif &s=a parse com  &s=a//////////////////////////
            string PageSourse12 = HttpHelper.getHtmlfromUrl(new Uri(ConfemUrl));
            string PageSourse13 = HttpHelper.getHtmlfromUrl(new Uri(gif));
            string PageSourse14 = HttpHelper.getHtmlfromUrl(new Uri(logpic + "&s=a"));
            string PageSourse15 = HttpHelper.getHtmlfromUrl(new Uri(logpic));


            string PageSourceConfirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            string pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));
            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not

            string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;

            if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
            {
                pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
            }
            if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
            {
                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
            }
            if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            {
                pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
            }
            if (pageSrc3.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through " + Username);

            }
            if (pageSourceCheck.Contains("complete the sign-up process"))
            {
                //LoggerWallPoste("not varified through Email" + Username);
            }
            #endregion
            //** FB Account Check email varified or not ***********************************************************************************//

            string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

            //** User Id ***************//////////////////////////////////
            string UsreId = string.Empty;
            string ProFilePost = string.Empty;
            ////**Post Message For User***********************/////////////////////////////////////////////////////
            int count = 0;
            if (pageSourceHome.Contains("http://www.facebook.com/profile.php?id="))
            {
                string[] arrUser = Regex.Split(pageSourceHome, "href");
                foreach (String itemUser in arrUser)
                {
                    if (!itemUser.Contains("<!DOCTYPE"))
                    {
                        if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
                        {

                            string[] arrhre = itemUser.Split('"');
                            ProFilePost = arrhre[1];
                            break;


                        }
                    }
                }
            }
            if (ProFilePost.Contains("http://www.facebook.com/profile.php?id="))
            {
                UsreId = ProFilePost.Replace("http://www.facebook.com/profile.php?id=", "");
            }



            //*** User Id **************//////////////////////////////////

            //*** Post Data **************//////////////////////////////////
            string fb_dtsg = pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);

            string post_form_id = pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
            string[] Arr = post_form_id.Split('"');
            post_form_id = Arr[4];
            post_form_id = post_form_id.Replace("\\", "");
            post_form_id = post_form_id.Replace("\\", "");
            post_form_id = post_form_id.Replace("\\", "");
            //string Response1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId);
            string Response1 = HttpHelper.postFormDataProxy(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId, _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);

            string Response2 = HttpHelper.postFormDataProxy(new Uri("http://www.facebook.com/ajax/httponly_cookies.php?dc=snc2&__a=1"), "keys[0]=1150335208&post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId, _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);

            string PageSourceCon = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/ajax/contextual_help.php?__a=1&set_name=welcome&__user=" + UsreId));


            string pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            pageSourceCheck1111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

            if (pageSourceCheck1111.Contains("complete the sign-up process"))
            {
                Console.WriteLine("Account is not verified for : " + _Email);
                AddToListBox("Account is not verified for : " + _Email);
            }

            string pageSource11 = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

            startIndex = pageSource.IndexOf(valueLSD) + 18;
            value = pageSource.Substring(startIndex, 5);

            //Log("Logging in with " + Username);

            string ResponseLogin11 = HttpHelper.postFormDataProxy(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + _Email.Split('@')[0] + "%40" + _Email.Split('@')[1] + "&pass=" + _Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "", _proxyAddress, intProxyPort, _proxyUsername, _proxyPassword);

            string PageSourceConfirmed11 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

            if (PageSourceConfirmed11.Contains("complete the sign-up process"))
            {
                Console.WriteLine("Account is not verified for : " + _Email);
                AddToListBox("Account is not verified for : " + _Email);
            }

            //LoggerVerify("Email verification completed for : " + Email);
        }

        #region Commented
        //public void VerifiyAccountModified()
        //{
        //    try
        //    {
        //        AddToListBox("Verifying thruogh Email");
        //        Thread.Sleep(10000);

        //        #region Gmail
        //        if (Email.Contains("@gmail"))
        //        {
        //            if (popClient.Connected)
        //                popClient.Disconnect();
        //            popClient.Connect("pop.gmail.com", int.Parse("995"), true);
        //            popClient.Authenticate(Email, Password);
        //            int Count = popClient.GetMessageCount();

        //            for (int i = Count; i >= 1; i--)
        //            {
        //                OpenPOP.MIME.Message Message = popClient.GetMessage(i);

        //                if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
        //                {
        //                    foreach (string item in GetUrlsFromString(Message.MessageBody[0]))
        //                    {
        //                        try
        //                        {
        //                            //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        //                            //string res = HttpHelper.getHtmlfromUrl(new Uri(item));
        //                            string href = item.Replace("&amp;report=1", "");
        //                            href = href.Replace("amp;", "");
        //                            System.Diagnostics.Process.Start("iexplore.exe", href);
        //                            LoginVerfy(href);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.StackTrace);
        //                        }
        //                    }
        //                    /////Write to text file
        //                    /////Also insert in Database
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //                    //DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //                    //return;
        //                }
        //                else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
        //                {
        //                    foreach (string item in GetUrlsFromString(Message.MessageBody[0]))
        //                    {
        //                        try
        //                        {
        //                            //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        //                            //string res = HttpHelper.getHtmlfromUrl(new Uri(item));
        //                            string href = item.Replace("&amp;report=1", "");
        //                            href = href.Replace("amp;", "");
        //                            System.Diagnostics.Process.Start("iexplore.exe", href);
        //                            LoginVerfy(href);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.StackTrace);
        //                        }
        //                    }
        //                    /////Write to text file
        //                    /////Also insert in Database
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //                    //DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //                    return;
        //                }
        //            }
        //            return;
        //        }
        //        #endregion

        //        #region Yahoo
        //        else if (Email.Contains("@yahoo"))
        //        {
        //            ChilkatIMAP imap = new ChilkatIMAP();
        //            imap.proxyAddress = proxyAddress;
        //            imap.proxyPort = proxyPort;
        //            imap.proxyUser = proxyUsername;
        //            imap.proxyPass = proxyPassword;
        //            imap.GetFBMails(Email, Password);

        //            /////Write to text file
        //            /////Also insert in Database
        //            //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //            //DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //            return;
        //        }



        //        #endregion

        //        #region Hotmail
        //        else if (Email.Contains("@hotmail"))
        //        {
        //            if (popClient.Connected)
        //                popClient.Disconnect();
        //            popClient.Connect("pop3.live.com", int.Parse("995"), true);
        //            popClient.Authenticate(Email, Password);
        //            //popClient.Authenticate("riya.shingh@hotmail.com", "ghdb634657rt");
        //            int Count = popClient.GetMessageCount();

        //            for (int i = Count; i >= 1; i--)
        //            {
        //                OpenPOP.MIME.Message Message = popClient.GetMessage(i);

        //                string subject = Message.Headers.Subject;

        //                if (Message.Headers.Subject.Contains("Action Required: Confirm Your Facebook Account"))
        //                {
        //                    foreach (string item in GetUrlsFromString(Message.MessageBody[0]))
        //                    {
        //                        try
        //                        {
        //                            //GlobusHttpHelper HttpHelper = new GlobusHttpHelper();
        //                            //string res = HttpHelper.getHtmlfromUrl(new Uri(item));
        //                            string href = item.Replace("&amp;report=1", "");
        //                            href = href.Replace("amp;", "");
        //                            System.Diagnostics.Process.Start("iexplore.exe", href);
        //                            LoginVerfy(href);

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.StackTrace);
        //                        }
        //                    }
        //                    /////Write to text file
        //                    /////Also insert in Database
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //                    //DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //                    //return;
        //                }
        //                else if (Message.Headers.Subject.Contains("Just one more step to get started on Facebook"))
        //                {
        //                    foreach (string item in GetUrlsFromString(Message.MessageBody[0]))
        //                    {
        //                        try
        //                        {
        //                            string href = item.Replace("&amp;report=1", "");
        //                            href = href.Replace("amp;", "");
        //                            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        //                            //string res1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/email_open_log_pic.php?c=1546187195&t=21&k=kjdm1_ijw1pi&s=a"));
        //                            //string res2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/email_open_log_pic.php?c=1546187195&t=21&k=kjdm1_ijw1pi"));
        //                            //string res3 = HttpHelper.getHtmlfromUrl(new Uri(href));
        //                            System.Diagnostics.Process.Start("iexplore.exe", href);
        //                            LoginVerfy(href);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.StackTrace);
        //                        }
        //                    }
        //                    /////Write to text file
        //                    /////Also insert in Database
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //                    //DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //                    //return;
        //                }
        //                else if (subject == "Finish your registration on Facebook")
        //                {
        //                    foreach (string item in GetUrlsFromString(Message.MessageBody[0]))
        //                    {
        //                        try
        //                        {
        //                            string href = item.Replace("&amp;report=1", "");
        //                            href = href.Replace("amp;", "");
        //                            GlobusHttpHelper HttpHelper = new GlobusHttpHelper();

        //                            //string res1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/email_open_log_pic.php?c=1546187195&t=21&k=kjdm1_ijw1pi&s=a"));
        //                            //string res2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/email_open_log_pic.php?c=1546187195&t=21&k=kjdm1_ijw1pi"));
        //                            //string res3 = HttpHelper.getHtmlfromUrl(new Uri(href));
        //                            System.Diagnostics.Process.Start("iexplore.exe", href);
        //                            LoginVerfy(href);
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.StackTrace);
        //                        }
        //                    }
        //                    //return;
        //                }
        //            }
        //            //return;
        //        }
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //        if (ex.Message.Contains("InvalidPasswordException"))
        //        {
        //            AddToListBox("Invalid Password :" + Email);
        //        }
        //    }
        //    finally
        //    {
        //        ///Write to text file
        //        ///Also insert in Database
        //        try
        //        {
        //            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + "ProxyAddress" + ":" + "ProxyPort" + ":" + "ProxyUser" + ":" + "ProxyPass", Application.StartupPath + "\\CreatedAccounts.txt");
        //            DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Email + "','" + Password + "','" + proxyAddress + "','" + proxyPort + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "0" + "')", "tb_FBAccount");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.StackTrace);
        //        }
        //    }
        //}

        //public void LoginVerfy(string url)
        //{
        //    string Username = Email;
        //    Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();
        //    string PageSourse1 = HttpHelper.getHtmlfromUrl(new Uri(url));

        //    string valueLSD = "name=" + "\"lsd\"";
        //    string pageSource = HttpHelper.getHtmlfromUrl(new Uri("https://www.facebook.com/login.php"));

        //    int startIndex = pageSource.IndexOf(valueLSD) + 18;
        //    string value = pageSource.Substring(startIndex, 5);

        //    //Log("Logging in with " + Username);

        //    string ResponseLogin = HttpHelper.postFormData(new Uri("https://www.facebook.com/login.php?login_attempt=1"), "charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "&locale=en_US&email=" + Username.Split('@')[0] + "%40" + Username.Split('@')[1] + "&pass=" + Password + "&persistent=1&default_persistent=1&charset_test=%E2%82%AC%2C%C2%B4%2C%E2%82%AC%2C%C2%B4%2C%E6%B0%B4%2C%D0%94%2C%D0%84&lsd=" + value + "");

        //    string PageSourceConfirmed = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/?email_confirmed=1"));

        //    string pageSourceCheck = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=contact_importer"));
        //    //** FB Account Check email varified or not ***********************************************************************************//
        //    #region  FB Account Check email varified or not

        //    string pageSrc1 = string.Empty;
        //    string pageSrc2 = string.Empty;
        //    string pageSrc3 = string.Empty;
        //    string pageSrc4 = string.Empty;
        //    string substr1 = string.Empty;

        //    if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
        //    {
        //        pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
        //    }
        //    if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
        //    {
        //        pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
        //    }
        //    if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
        //    {
        //        pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
        //    }
        //    if (pageSrc3.Contains("complete the sign-up process"))
        //    {
        //        //LoggerWallPoste("not varified through " + Username);
        //        AddToListBox("not varified through Email" + Username);

        //    }
        //    if (pageSourceCheck.Contains("complete the sign-up process"))
        //    {
        //        //LoggerWallPoste("not varified through Email" + Username);
        //        AddToListBox("not varified through Email" + Username);
        //    }
        //    #endregion
        //    //** FB Account Check email varified or not ***********************************************************************************//

        //    string pageSourceHome = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/home.php"));

        //    //** User Id ***************//////////////////////////////////
        //    string UsreId = string.Empty;
        //    string ProFilePost = string.Empty;
        //    ////**Post Message For User***********************/////////////////////////////////////////////////////
        //    int count = 0;
        //    if (pageSourceHome.Contains("http://www.facebook.com/profile.php?id="))
        //    {
        //        string[] arrUser = Regex.Split(pageSourceHome, "href");
        //        foreach (String itemUser in arrUser)
        //        {
        //            if (!itemUser.Contains("<!DOCTYPE"))
        //            {
        //                if (itemUser.Contains("http://www.facebook.com/profile.php?id="))
        //                {

        //                    string[] arrhre = itemUser.Split('"');
        //                    ProFilePost = arrhre[1];
        //                    break;


        //                }
        //            }
        //        }
        //    }
        //    if (ProFilePost.Contains("http://www.facebook.com/profile.php?id="))
        //    {
        //        UsreId = ProFilePost.Replace("http://www.facebook.com/profile.php?id=", "");
        //    }



        //    //*** User Id **************//////////////////////////////////

        //    //*** Post Data **************//////////////////////////////////
        //    string fb_dtsg = pageSourceHome.Substring(pageSourceHome.IndexOf("fb_dtsg") + 16, 8);

        //    string post_form_id = pageSourceHome.Substring(pageSourceHome.IndexOf("post_form_id"), 200);
        //    string[] Arr = post_form_id.Split('"');
        //    post_form_id = Arr[4];
        //    post_form_id = post_form_id.Replace("\\", "");
        //    post_form_id = post_form_id.Replace("\\", "");
        //    post_form_id = post_form_id.Replace("\\", "");
        //    string Response1 = HttpHelper.postFormData(new Uri("http://www.facebook.com/desktop/notifier/transfer.php?__a=1"), "post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId);

        //    string Response2 = HttpHelper.postFormData(new Uri("http://www.facebook.com/ajax/httponly_cookies.php?dc=snc2&__a=1"), "keys[0]=1150335208&post_form_id=" + post_form_id + "&fb_dtsg=" + fb_dtsg + "&lsd&post_form_id_source=AsyncRequest&__user=" + UsreId);

        //    string PageSourceCon = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/ajax/contextual_help.php?__a=1&set_name=welcome&__user=" + UsreId));

        //    string pageSourceCheck111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));

        //    pageSourceCheck111 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));
        //    if (pageSourceCheck111.Contains("complete the sign-up process"))
        //    {
        //        //LoggerWallPoste("not varified through Email" + Username);
        //        AddToListBox("not varified through Email" + Username);
        //    }

        //}
        #endregion

        private void WCRequest(string src)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            string pgSrcArmor = wc.DownloadString(src);
        }

        private void LogFacebookCreator(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            LogEventsFacebookCreator.LogText(eventArgs);
        }

        private void Log_Imap(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                AddToListBox(eArgs.log);
            }
            
        }

        private void rdoSingleThreaded_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoSingleThreaded.Checked)
            {
                 //panelSingleThreaded.Visible = true;
                 panelSMultiThreaded.Visible = false;
            }
        }

        private void rdoMassCreator_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoMassCreator.Checked)
            {
                panelSingleThreaded.Visible = false;
                panelSMultiThreaded.Visible = true;
                panelSMultiThreaded.Show();
            }
        }


        #region Proxy On The Fly

        public static Queue<string> queEmails { get; set; }

        public static Queue<string> queWorkingProxies { get; set; }

        public static readonly object proxyListLockr = new object();

        public void StartFlyAccountCreation(int numberOfAccountCreationThreads)
        {
            ThreadPool.SetMaxThreads(numberOfAccountCreationThreads, 5);

            while (true)
            {
                Thread.Sleep(3 * 1000);

                if (queEmails.Count <= 0)
                {
                    //MessageBox.Show("Please upload Emails");
                    return;
                }

                lock (proxyListLockr)
                {
                    if (queWorkingProxies.Count == 0)
                    {
                        Monitor.Wait(proxyListLockr);
                    }

                    string proxy = queWorkingProxies.Dequeue();

                    string rawEmail = queEmails.Dequeue();

                    string email = rawEmail + ":" + proxy;

                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadPoolMethod), email);
                    Thread.Sleep(100);
                }
            }
        }

        void startFlyCreationEvent_addToLogger(object sender, EventArgs e)
        {
            int numberOfThreads = 8;
           

            if (e is EventsArgs)
            {
                EventsArgs args = e as EventsArgs;

                Regex IdCheck1 = new Regex("^[0-9]*$");

                if (IdCheck1.IsMatch(args.log) && !string.IsNullOrEmpty(args.log))
                {
                    numberOfThreads = int.Parse(args.log);
                }
                if (numberOfThreads < 4)
                {
                    numberOfThreads = 4;
                }
            }
            new Thread(() =>
            {
                StartFlyAccountCreation(numberOfThreads);
            }).Start();
        }

        #endregion

        private void BtnProxyAssigner_Click(object sender, EventArgs e)
        {
            FrmProxyAssigner frmProxyAssigner = new FrmProxyAssigner();
            frmProxyAssigner.listEmails = listEmails;
            frmProxyAssigner.Show();
        }


        
    }
}
