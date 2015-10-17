using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using Twitter;
using Globussoft;
using System.Net;
using EmailActivator;
//using System.Web;
using System.IO;
using System.Threading;
using System.Data;
using System.Text.RegularExpressions;


namespace TwitterSignup
{
    public class TwitterSignUp
    {
        ClsDB_TwitterSignup obj_ClsDB_TwitterSignup = new ClsDB_TwitterSignup();
        TwitterSignup_TwitterDataScrapper obj_TwitterSignup_TwitterDataScrapper = new TwitterSignup_TwitterDataScrapper();
        Follower.Follower Obj_Follow = new Follower.Follower();

        public static Dictionary<string, Thread> dictionary_Threads = new Dictionary<string, Thread>();


        public static BaseLib.Events logEvents = new BaseLib.Events();

        public static BaseLib.Events addToDictionaryEvents = new BaseLib.Events();

        public static int CountOfAccounts = 0;

        public static int TotalEmailUploaded = 0;

        public string UsernameType = string.Empty;

        //Added by abhishek 

        public static bool manualcaptch = false;
        public string manualcapchaValue = string.Empty;

        public static bool _IsUseFollow = false;
        public static List<string> _lstFollowUserName = new List<string>();
        Queue<string> _QFollowUserName = new Queue<string>();
        public static readonly object Lock_Createdaccounts = new object();
        public static readonly object Lock_notCreatedaccounts = new object();

        #region Ac Creation With profiling  variable


        public static bool AC_IsUseLoaction = false;
        public static bool AC_IsUseProfileURL = false;
        public static bool AC_IsUseDescription = false;
        public static bool AC_IsUseProfileImage = false;

        public static List<string> _lstAcLocation = new List<string>();
        Queue<string> _QAcLocation = new Queue<string>();

        public static List<string> _lstAcProfileURL = new List<string>();
        Queue<string> _QAcProfileURL = new Queue<string>();

        public static List<string> _lstAcUseDescription = new List<string>();
        Queue<string> _QAcUseDescription = new Queue<string>();

        public static List<string> _lstAcProfileImage = new List<string>();
        Queue<string> _QAProfileImage = new Queue<string>();
        #endregion


        //public int _NoOfFreeAccount = 0;

        //****************

        public void SignUpFailedEmails(object objlstIP)
        {
            while (true)
            {
                try
                {
                    List<string> lstIP = (List<string>)objlstIP;

                    if (Globals.GetCountqueDataAfterEmailVerification() > 0)
                    {
                        object[] objData = Globals.DequequeDataForSignUp();
                        string IP = "";
                        if (lstIP.Count == 1)
                        {
                            IP = lstIP[0];
                        }
                        if (lstIP.Count > 1)
                        {
                            IP = lstIP[RandomNumberGenerator.GenerateRandom(0, lstIP.Count - 1)];
                        }

                        ThreadPool.QueueUserWorkItem(new WaitCallback(SignupMultiThreaded), new object[] { (string)objData[0], (string)objData[1], (string)objData[2], IP }); 
                    }
                }
                catch { }
                Thread.Sleep(5000);
            }
        }

        static int counter_SignUpFailedEmailsIPPool = 0;
        public void SignUpFailedEmailsIPPool()
        {
            while (true)
            {
                try
                {
                    string IP = "";
                   
                    if (Globals.GetCountqueDataAfterEmailVerification() > 0)
                    {
                        object[] objData = Globals.DequequeDataForSignUp();

                        IP = Globals.DequequeWorkingProxiesForSignUp();

                        Log("[ " + DateTime.Now + " ] => [ Retrying Signing Up for failed email : " + (string)objData[0] + " ]");
                        Log("[ " + DateTime.Now + " ] => [ counter_SignUpFailedEmailsIPPool : " + counter_SignUpFailedEmailsIPPool++ + " ]");
                        ThreadPool.QueueUserWorkItem(new WaitCallback(SignupMultiThreaded), new object[] { (string)objData[0], (string)objData[1], (string)objData[2], IP });
                    }
                    else
                    {
                        Thread.Sleep(5000);
                    }
                }
                catch { }
                //Thread.Sleep(5000);
            }
        }

        public void SignupMultiThreaded(object parameters)
        {
            try
            {

                Thread.CurrentThread.IsBackground = true;

                string DBCUsername = BaseLib.Globals.DBCUsername;
                string DBCPAssword = BaseLib.Globals.DBCPassword;
                GlobusHttpHelper globusHelper = new GlobusHttpHelper();
                int CaptchaCounter = 0;
                int counter_AuthToken = 0;
                string ImageURL = string.Empty;
                string authenticitytoken = string.Empty;
                string capcthavalue = string.Empty;

                Array paramsArray = new object[4];
                paramsArray = (Array)parameters;

                string Email = string.Empty;//"allliesams@gmail.com";
                string Password = string.Empty;//"1JESUS11";

                string IPAddress = string.Empty;
                string IPPort = string.Empty;
                string IPUsername = string.Empty;
                string IPpassword = string.Empty;

                string emailData = (string)paramsArray.GetValue(0);
                string username = (string)paramsArray.GetValue(1);
                string name = (string)paramsArray.GetValue(2);
                string IP = (string)paramsArray.GetValue(3);
                try
                {
                    //Log("test - " + emailData + " :: " + name);

                    #region Emails & IP Settings
                    try
                    {
                        int Count = emailData.Split(':').Length;
                        if (Count == 1)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Uploaded Emails Not In correct Format ]");
                            Log(emailData);
                            return;
                        }
                        if (Count == 2)
                        {
                            Email = emailData.Split(':')[0].Replace("\0", "");
                            Password = emailData.Split(':')[1].Replace("\0", "");
                        }
                        else if (Count == 4)
                        {
                            Email = emailData.Split(':')[0].Replace("\0", "");
                            Password = emailData.Split(':')[1].Replace("\0", "");
                            IPAddress = emailData.Split(':')[2];
                            IPPort = emailData.Split(':')[3];
                        }
                        else if (Count == 6)
                        {
                            Email = emailData.Split(':')[0].Replace("\0", "");
                            Password = emailData.Split(':')[1].Replace("\0", "");
                            IPAddress = emailData.Split(':')[2];
                            IPPort = emailData.Split(':')[3];
                            IPUsername = emailData.Split(':')[4];
                            IPpassword = emailData.Split(':')[5];
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Uploaded Emails Not In correct Format ]");
                            Log(emailData);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("8 :" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Email Pass --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Email Pass >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        RaiseEvent_AddToDictionary(Email);
                        dictionary_Threads.Add("AccountCreator_" + Email, Thread.CurrentThread);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.StackTrace); }

                    try
                    {
                        if (IP.Split(':').Length == 4)
                        {
                            IPAddress = IP.Split(':')[0];
                            IPPort = IP.Split(':')[1];
                            IPUsername = IP.Split(':')[2];
                            IPpassword = IP.Split(':')[3];
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("7 :" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- IP Split --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- IP Split >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    #endregion

                    try
                    {
                        username = username.Replace("\0", "");
                        Password = Password.Replace("\0", "");
                        if (!(username.Count() < 15 || Password.Count() > 6))
                        {
                            if (username.Count() > 15)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Username Must Not be greater than 15 character ]");
                                username = username.Remove(13); //Removes the extra characters
                            }
                            else if (Password.Count() < 6)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Password Must Not be less than 6 character ]");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("6 :" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Check Username --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Check Username >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                StartAgain:

                    Random randm = new Random();
                    double cachestop = randm.NextDouble();
                    string pagesourceGoogleCaptcha = string.Empty;
                    string signUpPage = string.Empty;
                    try
                    {
                        try
                        {
                            signUpPage = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/signup"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                        }
                        catch (Exception ex)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Error in Loading sign up page  " + IPAddress + " Exception" + ex.Message + " ]");
                        }
                        if (string.IsNullOrEmpty(signUpPage))
                        {
                            Thread.Sleep(500);
                            signUpPage = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/signup"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                        }

                        //Check if captchaAvailable, if yes, hit google captcha url
                        if (!string.IsNullOrEmpty(signUpPage) && signUpPage.Contains("captchaAvailable&quot;:true"))
                        {
                            try
                            {
                                string pagesource1 = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                            }
                            catch { }
                            try
                            {
                                pagesourceGoogleCaptcha = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=" + cachestop + "&lang=en"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                            }
                            catch { }
                            if (string.IsNullOrEmpty(pagesourceGoogleCaptcha))
                            {
                                Thread.Sleep(500);
                                pagesourceGoogleCaptcha = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=" + cachestop + "&lang=en"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        pagesourceGoogleCaptcha = string.Empty;
                        Log("[ " + DateTime.Now + " ] => [ Error in Loading sign up page  " + IPAddress + " Exception" + ex.Message + " ]");
                    }

                    if (string.IsNullOrEmpty(signUpPage))
                    {
                        NoOfNonCreatedAccounts++;
                        NoOfNonCreatedAccountsIP++;
                        Log("[ " + DateTime.Now + " ] => [ Couldn't load Sign Up Page: " + Email + " ]");
                        lock (Lock_notCreatedaccounts)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                        }

                        Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });

                        return;
                    }

                    //if captchaAvailable on signup but google captcha page source is null, retry getting captcha page source
                    if (string.IsNullOrEmpty(pagesourceGoogleCaptcha) && signUpPage.Contains("captchaAvailable&quot;:true"))
                    {
                        try
                        {
                            Thread.Sleep(500);
                            //textUrl = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/signup"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                            string pagesource1 = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                            pagesourceGoogleCaptcha = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=" + cachestop + "&lang=en"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                        }
                        catch (Exception ex)
                        {
                            if (CaptchaCounter == 0)//retry getting captcha page source only once
                            {
                                Log("[ " + DateTime.Now + " ] => [ Finding CAPTCHA Again For " + Email + " ]");
                                CaptchaCounter++;
                                goto StartAgain;
                            }
                            Console.WriteLine("Captcha Not Found For Email : " + Email + " : Error :" + ex.StackTrace);
                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Signup PageSource --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Signup PageSource >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                        }
                    }


                    try
                    {
                        int IndexStart = pagesourceGoogleCaptcha.IndexOf("challenge :");
                        if (IndexStart > 0)
                        {
                            string Start = pagesourceGoogleCaptcha.Substring(IndexStart);
                            int IndexEnd = Start.IndexOf("',");
                            string End = Start.Substring(0, IndexEnd).Replace("challenge :", "").Replace("'", "").Replace(" ", "");
                            capcthavalue = End;
                            ImageURL = "https://www.google.com/recaptcha/api/image?c=" + End;
                        }
                        else
                        {
                            if (signUpPage.Contains("captchaAvailable&quot;:true"))
                            {
                                if (CaptchaCounter == 0)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ Finding Capctha Again For " + Email + " ]");
                                    CaptchaCounter++;
                                    goto StartAgain;
                                }
                                Log("[ " + DateTime.Now + " ] => [ Cannot Find challenge Captcha on Page. Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.Path_CaptchaRequired);
                                //return;
                            }
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("1 :" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Image Url --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Image Url >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    WebClient webclient = new WebClient();
                    try
                    {
                        int intIPPort = 80;
                        if (!string.IsNullOrEmpty(IPPort) && GlobusRegex.ValidateNumber(IPPort))
                        {
                            intIPPort = int.Parse(IPPort);
                        }
                        ChangeIP_WebClient(IPAddress, intIPPort, IPUsername, IPpassword, ref webclient);
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- ChangeIP_WebClient() --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- ChangeIP_WebClient() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }

                    try
                    {
                        int StartIndex = signUpPage.IndexOf("phx-signup-form");
                        if (StartIndex > 0)
                        {
                            string Start = signUpPage.Substring(StartIndex);
                            int EndIndex = Start.IndexOf("name=\"authenticity_token");
                            string End = Start.Substring(0, EndIndex).Replace("phx-signup-form", "").Replace("method=\"POST\"", "").Replace("action=\"https://twitter.com/account/create\"", "");
                            authenticitytoken = End.Replace("class=\"\">", "").Replace("<input type=\"hidden\"", "").Replace("class=\"\">", "").Replace("value=\"", "").Replace("\n", "").Replace("\"", "").Replace(" ", "");
                        }
                        else
                        {
                            //Log("Cannot find Authenticity Token On Page for : " + Email);
                            if (counter_AuthToken == 0)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Retrying for Authenticity Token for : " + Email + " ]");
                                goto StartAgain;
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Cannot find Authenticity Token On Page, Exiting for : " + Email + " ]");
                                //Log("Couldn't create Account with : " + Email);
                                lock (Lock_notCreatedaccounts)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                                }

                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });

                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("2 :" + ex.StackTrace);
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Authenticity --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Authenticity >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                    }
                    ////Posting data
                    try
                    {
                        //Sign Up
                        string postData_SignUp = "user%5Bname%5D=" + username + "&user%5Bemail%5D=" + Uri.EscapeDataString(Email) + "&user%5Buser_password%5D=" + Password + "&context=front&authenticity_token=" + authenticitytoken + "";
                        string res_PostSignUp = globusHelper.postFormData(new Uri("https://twitter.com/signup"), postData_SignUp, "", "", "", "", "");

                        int tempCount_usernameCheckLoop = 0;
                    usernameCheckLoop:
                        int tempCount_passwordCheckLoop = 0;

                        bool Created = true;
                        string url = "https://twitter.com/users/email_available?suggest=1&username=&full_name=&email=" + Uri.EscapeDataString(Email.Replace(" ", "")) + "&suggest_on_username=true&context=signup";
                        string EmailCheck = globusHelper.getHtmlfromUrlIP(new Uri(url), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup", "");
                        string Usernamecheck = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/users/username_available?suggest=1&username=" + username + "&full_name=" + name + "&email=&suggest_on_username=true&context=signup"), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup", "");

                        if (EmailCheck.Contains("Email has already been taken. An email can only be used on one Twitter account at a time")
                            || (res_PostSignUp.Contains("You already have an account with this username and password")))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Email : " + Email + " has already been taken. An email can only be used on one Twitter account at a time ]");
                            GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", ""), Globals.path_EmailAlreadyTaken);
                            Created = false;
                        }
                        else if (Usernamecheck.Contains("Username has already been taken"))
                        {
                            //Created = false;
                            Log("[ " + DateTime.Now + " ] => [ Username : " + username + " has already been taken ]");
                            if (username.Count() > 12)
                            {
                                username = username.Remove(8); //Removes the extra characters
                            }

                            if (UsernameType == "String")
                            {
                                Log("[ " + DateTime.Now + " ] => [ Adding String To Username ]");
                                username = username + RandomStringGenerator.RandomString(5);
                            }
                            else if (UsernameType == "Numbers")
                            {
                                Log("[ " + DateTime.Now + " ] => [ Adding Numbers To Username ]");
                                username = username + RandomStringGenerator.RandomNumber(5);
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ Adding Strings & Numbers To Username ]");
                                username = username + RandomStringGenerator.RandomStringAndNumber(5);
                            }

                            if (username.Count() > 15)
                            {
                                username = username.Remove(13); //Removes the extra characters
                            }
                            tempCount_usernameCheckLoop++;
                            if (tempCount_usernameCheckLoop < 5)
                            {
                                goto usernameCheckLoop;
                            }
                        }
                        else if (EmailCheck.Contains("You cannot have a blank email address"))
                        {
                            //Log("You cannot have a blank email address");
                            Created = false;
                            Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                        }

                        if (Created)
                        {
                            string AccountCreatePageSource = string.Empty;

                            string EscapeDataString_name = Uri.EscapeDataString(name);

                            //Replace Space (which unicode value is %20) in Pluse ....

                            EscapeDataString_name = EscapeDataString_name.Replace("%20", "+");

                            if (!string.IsNullOrEmpty(ImageURL))
                            {
                                try
                                {
                                    byte[] args = webclient.DownloadData(ImageURL);

                                    string captchaText = string.Empty;

                                    if (manualcaptch)
                                    {

                                    }
                                    else
                                    {
                                        string[] arr1 = new string[] { DBCUsername, DBCPAssword, "" };

                                        captchaText = DecodeDBC(arr1, args);
                                    }
                                    //string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Uri.EscapeDataString(Email.Replace(" ", "")) + "&user%5Buser_password%5D=" + System.Web.HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + System.Web.HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
                                    string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Uri.EscapeDataString(Email.Replace(" ", "")) + "&user%5Buser_password%5D=" + System.Web.HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + System.Web.HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";

                                    try
                                    {
                                        AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");
                                    }
                                    catch { };
                                    if (string.IsNullOrEmpty(AccountCreatePageSource))
                                    {
                                        Thread.Sleep(1000);
                                        AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Log(ex.Message);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting AccountCreatePageSource --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting AccountCreatePageSource >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                                }
                            }
                            else
                            {
                                string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Email.Replace(" ", "").Replace("@", "%40") + "&user%5Buser_password%5D=" + Password + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&user%5Buse_cookie_personalization%5D=1&asked_cookie_personalization_setting=1&context=front&ad_ref=&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
                                AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");
                            }

                            //string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + name + "&user%5Bemail%5D=" + Email.Replace(" ", "") + "&user%5Buser_password%5D=" + Password + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
                            //string AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");

                            if (AccountCreatePageSource.Contains("id=\"signout-form\"") && AccountCreatePageSource.Contains("/logout") && AccountCreatePageSource.Contains("id=\"signout-button")
                                || AccountCreatePageSource.Contains("/welcome/recommendations" + username))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Account created With Email :" + Email + " ]");
                                lock (Lock_Createdaccounts)
                                {
                                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
                                }
                                NoOfSuccessfullyCreatedAccount++;
                                //After Account creation
                                if (Created)
                                {
                                    try
                                    {
                                        if (!Globals.IsUseFakeEmailAccounts)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Going for Email Verification : " + Email + " ]");
                                            Thread.Sleep(5000);

                                            ClsEmailActivator EmailActivate = new ClsEmailActivator();
                                            bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
                                            if (verified)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", "") + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_EmailVerifiedAccounts);
                                                Log("[ " + DateTime.Now + " ] => [ Account Verified : " + Email + " ]");
                                            }
                                            else
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", "") + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_NonEmailVerifiedAccounts);
                                                Log("[ " + DateTime.Now + " ] => [ Account Couldn't be Email Verified : " + Email + " ]");
                                            }

                                        }

                                        try
                                        {
                                            string checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

                                            string pstAuthToken = PostAuthenticityToken(checkPageSource, "postAuthenticityToken");

                                            #region Profilig Of new created account
                                            try
                                            {
                                                new Thread(() =>
                                                {
                                                    startProfilingAfterAccountCreation(new object[] { Email, Password, IPAddress, IPPort, IPUsername, IPpassword, pstAuthToken, globusHelper });
                                                }).Start();
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            #endregion

                                            #region for Follow

                                            try
                                            {
                                                Follow(ref globusHelper, Email, pstAuthToken);
                                            }
                                            catch (Exception)
                                            {
                                            }

                                            #endregion

                                        }
                                        catch (Exception)
                                        {
                                        }

                                        clsFBAccount insertUpdateDataBase = new clsFBAccount();
                                        insertUpdateDataBase.InsertUpdateFBAccount(Email.Replace(" ", ""), Password, username, IPAddress, IPPort, IPUsername, IPpassword, "", "");
                                    }
                                    catch (Exception ex)
                                    {
                                        //Log(ex.Message);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                                    }
                                }
                            }
                            else if (AccountCreatePageSource.Contains("/welcome/recommendations"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Account created With Email :" + Email + " ]");
                                lock (Lock_Createdaccounts)
                                {
                                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
                                }
                                NoOfSuccessfullyCreatedAccount++;
                                //After Account creation
                                if (Created)
                                {
                                    try
                                    {
                                        if (!Globals.IsUseFakeEmailAccounts)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Going for Email Verification : " + Email + " ]");
                                            Thread.Sleep(5000);

                                            ClsEmailActivator EmailActivate = new ClsEmailActivator();
                                            bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
                                            if (verified)
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", "") + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_EmailVerifiedAccounts);
                                                Log("[ " + DateTime.Now + " ] => [ Account Verified : " + Email + " ]");
                                            }
                                            else
                                            {
                                                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", "") + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_NonEmailVerifiedAccounts);
                                                Log("[ " + DateTime.Now + " ] => [ Account Couldn't be Email Verified : " + Email + " ]");
                                            }
                                        }


                                        try
                                        {
                                            string checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

                                            string pstAuthToken = PostAuthenticityToken(checkPageSource, "postAuthenticityToken");

                                            #region Profilig Of new created account
                                            try
                                            {
                                                new Thread(() =>
                                                {
                                                    startProfilingAfterAccountCreation(new object[] { Email, Password, IPAddress, IPPort, IPUsername, IPpassword, pstAuthToken, globusHelper });
                                                }).Start();
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            #endregion

                                            #region for Follow

                                            try
                                            {
                                                Follow(ref globusHelper, Email, pstAuthToken);
                                            }
                                            catch (Exception)
                                            {
                                            }

                                            #endregion

                                        }
                                        catch (Exception)
                                        {
                                        }

                                        clsFBAccount insertUpdateDataBase = new clsFBAccount();
                                        insertUpdateDataBase.InsertUpdateFBAccount(Email.Replace(" ", ""), Password, username, IPAddress, IPPort, IPUsername, IPpassword, "", "");
                                    }
                                    catch (Exception ex)
                                    {
                                        //Log(ex.Message);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
                                    }
                                }
                            }
                            else if (AccountCreatePageSource.ToLower().Contains("password is too obvious") || AccountCreatePageSource.ToLower().Contains("\"active error\">password"))
                            {
                                NoOfNonCreatedAccounts++;
                                tempCount_passwordCheckLoop = 0;
                                if (Password.Count() > 8)
                                {
                                    Password = Password.Remove(8); //Removes the extra characters
                                }
                                Password = Password + RandomStringGenerator.RandomString(3);
                                if (Password.Count() > 12)
                                {
                                    Password = Password.Remove(12); //Removes the extra characters
                                }
                                tempCount_passwordCheckLoop++;
                                if (tempCount_passwordCheckLoop < 5)
                                {
                                    goto usernameCheckLoop;
                                }
                                //Password = Password + RandomStringGenerator.RandomString(3);
                                Log("[ " + DateTime.Now + " ] => [ Please Create Accounts With Secure Password ]");
                                Log("[ " + DateTime.Now + " ] => [ Password is too obvious ]");
                                Log("[ " + DateTime.Now + " ] => [ Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
                                goto usernameCheckLoop;
                                //return;
                            }
                            else if (AccountCreatePageSource.Contains("/sessions/destroy") && AccountCreatePageSource.Contains("/signup"))
                            {
                                lock (Lock_notCreatedaccounts)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                                }
                                NoOfNonCreatedAccounts++;
                                NoOfNonCreatedAccountsIP++;
                                Log("[ " + DateTime.Now + " ] => [ You can't do that right now. ]");
                                Log("[ " + DateTime.Now + " ] => [ Sorry, please try again later ]");
                                Log("[ " + DateTime.Now + " ] => [ and Change IP to create more accounts. ]");

                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });

                                return;
                            }
                            else
                            {
                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                                lock (Lock_notCreatedaccounts)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                                }
                                NoOfNonCreatedAccounts++;
                                Log("[ " + DateTime.Now + " ] => [ Couldn't create Account ]");
                                Log("[ " + DateTime.Now + " ] => [ Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
                            }
                            //if (Created)
                            //{
                            //    Log("Going for Email Verification : " + Email);
                            //    Thread.Sleep(5000);

                            //    ClsEmailActivator EmailActivate = new ClsEmailActivator();
                            //    bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
                            //    if (verified)
                            //    {
                            //        GlobusFileHelper.AppendStringToTextfileNewLine(emailData, Globals.path_SuccessfulCreatedAccounts);
                            //        Log("Account Verified : " + Email);
                            //    }
                            //    else
                            //    {
                            //        GlobusFileHelper.AppendStringToTextfileNewLine(emailData, Globals.path_NonEmailVerifiedAccounts);
                            //        Log("Account Couldn't be Email Verified : " + Email);
                            //    }
                            //}
                        }
                        else
                        {
                            NoOfNonCreatedAccounts++;

                            if (EmailCheck.Contains("Email has already been taken. An email can only be used on one Twitter account at a time"))
                            {
                                NoOfAlreadyCreatedAccounts++;
                                Log("[ " + DateTime.Now + " ] => [ " + Email + " : Email has already been taken. ]");
                                Log("[ " + DateTime.Now + " ] => [ An email can only be used on one Twitter account at a time ]");
                                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", ""), Globals.path_EmailAlreadyTaken);
                            }
                            else if (Usernamecheck.Contains("Username has already been taken"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ " + username + " : Username has already been taken ]");
                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                            }
                            else if (EmailCheck.Contains("You cannot have a blank email address"))
                            {
                                Log("[ " + DateTime.Now + " ] => [ " + Email + " : You cannot have a blank email address ]");
                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                            }
                            else
                            {
                                ExportFailedAccounts(Email, Password, IPAddress, IPPort, IPUsername, IPpassword);
                                Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NoOfNonCreatedAccounts++;
                        lock (Lock_notCreatedaccounts)
                        {
                            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                            GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                        }
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Posting data --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Posting data >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);

                        Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                    }
                }
                catch (Exception ex)
                {
                    NoOfNonCreatedAccounts++;
                    lock (Lock_notCreatedaccounts)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
                    }
                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                    //Console.WriteLine("4 : " + ex.Message);
                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- SignupMultiThreaded Start --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                    GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- SignupMultiThreaded Start >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);

                    Globals.EnquequeDataForSignUp(new object[] { emailData, username, name, IP });
                }
                finally
                {
                    CountOfAccounts++;
                    if (CountOfAccounts >= TotalEmailUploaded)
                    {
                        Log("[ " + DateTime.Now + " ] => [ Account Creation Finished ]");
                        Log("[ " + DateTime.Now + " ] => [ No Of SuccessFully Created Accounts : " + NoOfSuccessfullyCreatedAccount + " ]");
                        Log("[ " + DateTime.Now + " ] => [ No Of Non Created Accounts : " + NoOfNonCreatedAccounts + " ]");
                    }

                    //Log("SuccessFully Created Accounts Count: " + NoOfSuccessfullyCreatedAccount);
                    //Log("Failed Accounts Count: " + NoOfNonCreatedAccounts);
                    //Log("Already Created Accounts Count: " + NoOfAlreadyCreatedAccounts);
                    //Log("Failed Proxies Count: " + NoOfNonCreatedAccountsIP);
                }
                
            }
            catch
            {
            }
        }

        private void ExportFailedAccounts(string Email, string Password, string IPAddress, string IPPort, string IPUsername, string IPpassword)
        {
            lock (Lock_notCreatedaccounts)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
                GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
            }
            Log("[ " + DateTime.Now + " ] => [ Couldn't create Account with : " + Email + " ]");
        }


        public string DecodeDBC(string[] args, byte[] imageBytes)
        {
            try
            {
                // Put your DBC username & password here:
                DeathByCaptcha.Client client = (DeathByCaptcha.Client)new DeathByCaptcha.SocketClient(args[0], args[1]);
                client.Verbose = true;

                Console.WriteLine("Your balance is {0:F2} US cents", client.Balance);//Log("Your balance is " + client.Balance + " US cents ");

                if (!client.User.LoggedIn)
                {
                    Log("[ " + DateTime.Now + " ] => [ Please check your DBC Account Details ]");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    Log("[ " + DateTime.Now + " ] => [ You have 0 Balance in your DBC Account ]");
                    return null;
                }

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
                        Log("[ " + DateTime.Now + " ] => [ CAPTCHA was not solved ]");
                        Console.WriteLine("CAPTCHA was not solved");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- DecodeDBC()  --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  DecodeDBC() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
            return null;
        }

        private void Log(string message)
        {
            EventsArgs eArgs = new EventsArgs(message);
            logEvents.LogText(eArgs);
        }

        private void RaiseEvent_AddToDictionary(string message)
        {
            EventsArgs eArgs = new EventsArgs(message);
            addToDictionaryEvents.LogText(eArgs);
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

        public void ChangeIP_WebClient(string IPAddress, int port, string IPUsername, string IPpassword, ref WebClient wc)
        {
            try
            {
                WebProxy myIP = new WebProxy(IPAddress, port);
                myIP.BypassProxyOnLocal = false;

                if (!string.IsNullOrEmpty(IPUsername) && !string.IsNullOrEmpty(IPpassword))
                {
                    myIP.Credentials = new NetworkCredential(IPUsername, IPpassword);
                }
                wc.Proxy = myIP;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  ChangeIP_WebClient() --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  ChangeIP_WebClient() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }
        }


        //Added By abhishek 

        public System.Collections.Specialized.NameValueCollection SignupManual(object parameters, GlobusHttpHelper globusHelper)
        {
            Array paramsArray = new object[4];
            paramsArray = (Array)parameters;

            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();

            string DBCUsername = BaseLib.Globals.DBCUsername;
            string DBCPAssword = BaseLib.Globals.DBCPassword;
            //GlobusHttpHelper globusHelper = new GlobusHttpHelper();


            string ImageURL = string.Empty;
            string authenticitytoken = string.Empty;
            string capcthavalue = string.Empty;



            string Email = string.Empty;//"allliesams@gmail.com";
            string Password = string.Empty;//"1JESUS11";

            string IPAddress = string.Empty;
            string IPPort = string.Empty;
            string IPUsername = string.Empty;
            string IPpassword = string.Empty;

            string emailData = (string)paramsArray.GetValue(0);
            string username = (string)paramsArray.GetValue(1);
            string name = (string)paramsArray.GetValue(2);
            string IP = (string)paramsArray.GetValue(3);
            bool Created = true;

            try
            {

                int Count = emailData.Split(':').Length;
                if (Count == 1)
                {
                    Log("[ " + DateTime.Now + " ] => [ Uploaded Emails Not In correct Format ]");
                    Log("[ " + DateTime.Now + " ] => [ " + emailData + " ]");
                }
                if (Count == 2)
                {
                    Email = emailData.Split(':')[0].Replace("\0", "");
                    Password = emailData.Split(':')[1].Replace("\0", "");
                }
                else if (Count == 4)
                {
                    Email = emailData.Split(':')[0].Replace("\0", "");
                    Password = emailData.Split(':')[1].Replace("\0", "");
                    IPAddress = emailData.Split(':')[2];
                    IPPort = emailData.Split(':')[3];
                }
                else if (Count == 6)
                {
                    Email = emailData.Split(':')[0].Replace("\0", "");
                    Password = emailData.Split(':')[1].Replace("\0", "");
                    IPAddress = emailData.Split(':')[2];
                    IPPort = emailData.Split(':')[3];
                    IPUsername = emailData.Split(':')[4];
                    IPpassword = emailData.Split(':')[5];
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Uploaded Emails Not In correct Format ]");
                    Log("[ " + DateTime.Now + " ] => [ " + emailData + " ]");
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message); Console.WriteLine("8 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Email Pass --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Email Pass >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }

            try
            {
                RaiseEvent_AddToDictionary(Email);
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }

            try
            {
                if (IP.Split(':').Length == 4)
                {
                    IPAddress = IP.Split(':')[0];
                    IPPort = IP.Split(':')[1];
                    IPUsername = IP.Split(':')[2];
                    IPpassword = IP.Split(':')[3];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("7 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- IP Split --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- IP Split >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }

            string url = "https://twitter.com/users/email_available?suggest=1&username=&full_name=&email=" + Email.Replace("@", "%40").Replace(" ", "") + "&suggest_on_username=true&context=signup";
            string EmailCheck = globusHelper.getHtmlfromUrlIP(new Uri(url), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup", "");
            if (EmailCheck.Contains("\"taken\":true"))
            {
                Log("[ " + DateTime.Now + " ] => [ Email : " + Email + " has already been taken. An email can only be used on one Twitter account at a time ]");
                Created = false;
                GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", ""), Globals.path_EmailAlreadyTaken);
                nvc.Clear();
                return nvc;
            }


            //Get User name .....
            try
            {
                username = username.Replace("\0", "");
                Password = Password.Replace("\0", "");
                if (!(username.Count() < 15 || Password.Count() > 6))
                {
                    if (username.Count() > 15)
                    {
                        Log("[ " + DateTime.Now + " ] => [ Username Must Not be greater than 15 character ]");
                        username = username.Remove(13); //Removes the extra characters
                    }
                    else if (Password.Count() < 6)
                    {
                        Log("[ " + DateTime.Now + " ] => [ Password Must Not be less than 6 char ]");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("6 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Check Username --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Check Username >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }



            Random randm = new Random();
            double cachestop = randm.NextDouble();
            string pagesource2 = string.Empty;
            string textUrl = string.Empty;


            try
            {
                textUrl = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/signup"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                string pagesource1 = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/js/recaptcha_ajax.js"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
                pagesource2 = globusHelper.getHtmlfromUrlIP(new Uri("https://www.google.com/recaptcha/api/challenge?k=6LfbTAAAAAAAAE0hk8Vnfd1THHnn9lJuow6fgulO&ajax=1&cachestop=" + cachestop + "&lang=en"), IPAddress, IPPort, IPUsername, IPpassword, "", "");
            }
            catch (Exception ex)
            {
                Console.WriteLine("5 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Signup PageSource --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Signup PageSource >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }



            try
            {
                int IndexStart = pagesource2.IndexOf("challenge :");
                if (IndexStart > 0)
                {
                    string Start = pagesource2.Substring(IndexStart);
                    int IndexEnd = Start.IndexOf("',");
                    string End = Start.Substring(0, IndexEnd).Replace("challenge :", "").Replace("'", "").Replace(" ", "");
                    capcthavalue = End;
                    ImageURL = "https://www.google.com/recaptcha/api/image?c=" + End;
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Cannot Find challenge Captcha on Page. Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
                    GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.Path_CaptchaRequired);
                    nvc.Clear();
                    return nvc;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("1 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Image Url --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Image Url >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }


            WebClient webclient = new WebClient();
            try
            {
                int intIPPort = 80;
                if (!string.IsNullOrEmpty(IPPort) && GlobusRegex.ValidateNumber(IPPort))
                {
                    intIPPort = int.Parse(IPPort);
                }
                ChangeIP_WebClient(IPAddress, intIPPort, IPUsername, IPpassword, ref webclient);
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- ChangeIP_WebClient() --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- ChangeIP_WebClient() >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }

            try
            {
                int StartIndex = textUrl.IndexOf("phx-signup-form");
                if (StartIndex > 0)
                {
                    string Start = textUrl.Substring(StartIndex);
                    int EndIndex = Start.IndexOf("name=\"authenticity_token");
                    string End = Start.Substring(0, EndIndex).Replace("phx-signup-form", "").Replace("method=\"POST\"", "").Replace("action=\"https://twitter.com/account/create\"", "");
                    authenticitytoken = End.Replace("class=\"\">", "").Replace("<input type=\"hidden\"", "").Replace("class=\"\">", "").Replace("value=\"", "").Replace("\n", "").Replace("\"", "").Replace(" ", "");
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Cannot find Authenticity Token On Page ]");
                    nvc.Clear();
                    return nvc;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("2 :" + ex.StackTrace);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Authenticity --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting Authenticity >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
            }


            string postData_SignUp = "user%5Bname%5D=" + username + "&user%5Bemail%5D=" + Email + "&user%5Buser_password%5D=" + Password + "&context=front&authenticity_token=" + authenticitytoken + "";
            string res_PostSignUp = globusHelper.postFormData(new Uri("https://twitter.com/signup"), postData_SignUp, "", "", "", "", "");

            int tempCount_usernameCheckLoop = 0;
        usernameCheckLoop:
            int tempCount_passwordCheckLoop = 0;

            if (username.Contains(" "))
            {
                username = username.Replace(" ", "_");
            }

            string headr = globusHelper.gResponse.Headers.ToString();
            string Usernamecheck = globusHelper.getHtmlfromUrlIP(new Uri("https://twitter.com/users/username_available?suggest=1&username=" + username + "&full_name=" + name + "&email=&suggest_on_username=true&context=signup"), IPAddress, IPPort, IPUsername, IPpassword, "https://twitter.com/signup", "");

            //if (EmailCheck.Contains("\"taken\":true")
            //    || res_PostSignUp.Contains("You already have an account with this username and password"))
            //{
            //    Log("Email : " + Email + " has already been taken. An email can only be used on one Twitter account at a time");
            //    Created = false;
            //    GlobusFileHelper.AppendStringToTextfileNewLine(emailData.Replace("\0", ""), Globals.path_EmailAlreadyTaken);
            //}
            //else 
            if (Usernamecheck.Contains("Username has already been taken"))
            {
                //Created = false;
                Log("[ " + DateTime.Now + " ] => [ Username : " + username + " has already been taken ]");
                if (username.Count() > 12)
                {
                    username = username.Remove(8); //Removes the extra characters
                }

                if (UsernameType == "String")
                {
                    Log("[ " + DateTime.Now + " ] => [ Adding String To Username ]");
                    username = username + RandomStringGenerator.RandomString(5);
                }
                else if (UsernameType == "Numbers")
                {
                    Log("[ " + DateTime.Now + " ] => [ Adding Numbers To Username ]");
                    username = username + RandomStringGenerator.RandomNumber(5);
                }
                else
                {
                    Log("[ " + DateTime.Now + " ] => [ Adding Strings & Numbers To Username ]");
                    username = username + RandomStringGenerator.RandomStringAndNumber(5);
                    Log("[ " + DateTime.Now + " ] => [ New user name :- " + username + " ]");
                }

                if (username.Count() > 15)
                {
                    username = username.Remove(13); //Removes the extra characters
                }
                tempCount_usernameCheckLoop++;
                if (tempCount_usernameCheckLoop < 5)
                {
                    goto usernameCheckLoop;
                }
            }
            else if (EmailCheck.Contains("You cannot have a blank email address"))
            {
                Log("[ " + DateTime.Now + " ] => [ Email Address is Blank ]");
                Created = false;
            }


            if (Created)
            {
                nvc.Add("Email", Email);
                nvc.Add("username", username);
                nvc.Add("name", name);
                nvc.Add("Password", Password);
                nvc.Add("authenticitytoken", authenticitytoken);
                nvc.Add("IPAddress", IPAddress);
                nvc.Add("IPpassword", IPpassword);
                nvc.Add("IPPort", IPPort);
                nvc.Add("IPUsername", IPUsername);
                nvc.Add("IPpassword", IPpassword);
                nvc.Add("capcthavalue", capcthavalue);
                nvc.Add("ImageURL", ImageURL);
                //nvc.Add("globusHelper", globusHelper);
            }

            return nvc;
        }


        public static int NoOfSuccessfullyCreatedAccount = 0;
        public static int NoOfNonCreatedAccounts = 0;
        public static int NoOfAlreadyCreatedAccounts = 0;
        public static int NoOfNonCreatedAccountsIP = 0;
        //public void SubmitCaptcha(System.Collections.Specialized.NameValueCollection nvc, GlobusHttpHelper globusHelper)
        //{

        //    string Email = nvc["Email"];
        //    string name = nvc["name"];
        //    string Password = nvc["Password"];
        //    string username = nvc["username"];
        //    string capcthavalue = nvc["capcthavalue"];
        //    string captchaText = nvc["captchaText"];
        //    string ImageURL = nvc["ImageURL"];


        //    string IPAddress = nvc["IPAddress"];
        //    string IPPort = nvc["IPPort"];
        //    string IPUsername = nvc["IPUsername"];
        //    string IPpassword = nvc["IPpassword"];

        //    if (IPpassword.Contains(','))
        //    {
        //        IPpassword = IPpassword.Split(',')[0];
        //    }

        //    string emailData = nvc["emailData"];
        //    string authenticitytoken = nvc["authenticitytoken"];
        //    string AccountCreatePageSource = string.Empty;


        //    try
        //    {
        //        if (!string.IsNullOrEmpty(ImageURL))
        //        {
        //            string EscapeDataString_name = Uri.EscapeDataString(name);

        //            //Replace Space (which unicode value is %20) in Pluse ....

        //            EscapeDataString_name = EscapeDataString_name.Replace("%20", "+");


        //            //string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + name + "&user%5Bemail%5D=" + Email.Replace(" ", "").Replace("@", "%40") + "&user%5Buser_password%5D=" + HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
        //            //string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Email.Replace(" ", "").Replace("@", "%40") + "&user%5Buser_password%5D=" + System.Web.HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + System.Web.HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";

        //            string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Uri.EscapeDataString(Email.Replace(" ", "")) + "&user%5Buser_password%5D=" + System.Web.HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + System.Web.HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";


        //            AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");
        //        }
        //        else
        //        {
        //            string EscapeDataString_name = Uri.EscapeDataString(name);

        //            //Replace Space (which unicode value is %20) in Pluse ....
        //            EscapeDataString_name = EscapeDataString_name.Replace("%20", "+");


        //            //string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + name + "&user%5Bemail%5D=" + Email.Replace(" ", "").Replace("@", "%40") + "&user%5Buser_password%5D=" + Password + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&user%5Buse_cookie_personalization%5D=1&asked_cookie_personalization_setting=1&context=front&ad_ref=&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";
        //            string postdata = "authenticity_token=" + authenticitytoken + "&user%5Bname%5D=" + EscapeDataString_name + "&user%5Bemail%5D=" + Email.Replace(" ", "").Replace("@", "%40") + "&user%5Buser_password%5D=" + System.Web.HttpUtility.UrlEncode(Password) + "&user%5Bscreen_name%5D=" + username + "&user%5Bremember_me_on_signup%5D=1&user%5Bremember_me_on_signup%5D=&context=front&ad_ref=&recaptcha_challenge_field=" + capcthavalue + "&recaptcha_response_field=" + System.Web.HttpUtility.UrlEncode(captchaText) + "&user%5Bdiscoverable_by_email%5D=1&user%5Bsend_email_newsletter%5D=1";


        //            AccountCreatePageSource = globusHelper.postFormData(new Uri("https://twitter.com/account/create"), postdata, "https://twitter.com/signup", "", "", "", "");
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Log(ex.Message);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting AccountCreatePageSource --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
        //        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting AccountCreatePageSource >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
        //    }
        //    if (AccountCreatePageSource.Contains("id=\"signout-form\"") && AccountCreatePageSource.Contains("/logout"))
        //    {
        //        Log("[ " + DateTime.Now + " ] => [ Account created With Email :" + Email + " ]");
        //        //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":"  + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
        //        //Count no of succesfully create accounts .
        //        NoOfSuccessfullyCreatedAccount++;

        //        //_NoOfFreeAccount++;

        //        try
        //        {
        //            if (!Globals.IsUseFakeEmailAccounts)
        //            {
        //                Log("[ " + DateTime.Now + " ] => [ Going for Email Verification : " + Email + " ]");
        //                Thread.Sleep(5000);

        //                ClsEmailActivator EmailActivate = new ClsEmailActivator();
        //                bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
        //                if (verified)
        //                {
        //                    //GlobusFileHelper.AppendStringToTextfileNewLine(Email.Replace("\0", "") + ":" + Password, Globals.path_SuccessfulCreatedAccounts);
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email.Replace("\0", "") + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_EmailVerifiedAccounts);
        //                    Log("[ " + DateTime.Now + " ] => [ Account Verified : " + Email + " ]");
        //                }
        //                else
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email.Replace("\0", "") + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_NonEmailVerifiedAccounts);
        //                    Log("[ " + DateTime.Now + " ] => [ Account Couldn't be Email Verified : " + Email + " ]");
        //                }
        //            }

        //            #region Profile/Follow
        //            try
        //            {
        //                //get PostAuthentication Token .....

        //                string checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

        //                if (string.IsNullOrEmpty(checkPageSource))
        //                {
        //                    checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");
        //                }

        //                string pstAuthToken = PostAuthenticityToken(checkPageSource, "postAuthenticityToken");

        //                #region Profilig Of new created account
        //                try
        //                {
        //                    new Thread(() =>
        //                        {
        //                            startProfilingAfterAccountCreation(new object[] { Email, Password, IPAddress, IPPort, IPUsername, IPpassword, pstAuthToken, globusHelper });
        //                        }).Start();
        //                }
        //                catch (Exception)
        //                {
        //                }
        //                #endregion


        //                #region for Follow
        //                try
        //                {
        //                    new Thread(() =>
        //                       {
        //                           Follow(ref globusHelper, Email, pstAuthToken);
        //                       }).Start();
        //                }
        //                catch (Exception)
        //                {

        //                }

        //                #endregion


        //            }
        //            catch (Exception)
        //            {
        //            }
        //            #endregion

        //            clsFBAccount insertUpdateDataBase = new clsFBAccount();
        //            insertUpdateDataBase.InsertUpdateFBAccount(Email.Replace(" ", ""), Password, username, IPAddress, IPPort, IPUsername, IPpassword, "", "");




        //        }
        //        catch (Exception ex)
        //        {
        //            //Log(ex.Message);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
        //        }
        //    }
        //    else if (AccountCreatePageSource.Contains("/welcome/recommendations") && AccountCreatePageSource.Contains(username))
        //    {
        //        Log("[ " + DateTime.Now + " ] => [ Account created With Email :" + Email + " ]");
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password +  ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
        //        //GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_SuccessfulCreatedAccounts);
        //        //Count no of succesfully create accounts .
        //        NoOfSuccessfullyCreatedAccount++;

        //        //After Account creation
        //        //if (Created)
        //        //{
        //        try
        //        {
        //            if (!Globals.IsUseFakeEmailAccounts)
        //            {
        //                Log("[ " + DateTime.Now + " ] => [ Going for Email Verification : " + Email + " ]");
        //                Thread.Sleep(5000);

        //                ClsEmailActivator EmailActivate = new ClsEmailActivator();
        //                bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
        //                if (verified)
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email.Replace("\0", "") + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_EmailVerifiedAccounts);
        //                    Log("[ " + DateTime.Now + " ] => [ Account Verified : " + Email + " ]");
        //                }
        //                else
        //                {
        //                    GlobusFileHelper.AppendStringToTextfileNewLine(Email.Replace("\0", "") + ":" + Password + ":" + username + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_NonEmailVerifiedAccounts);
        //                    Log("[ " + DateTime.Now + " ] => [ Account Couldn't be Email Verified : " + Email + " ]");
        //                }
        //            }

        //            #region Profile/Follow
        //            try
        //            {
        //                //get PostAuthentication Token .....

        //                string checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");

        //                if (string.IsNullOrEmpty(checkPageSource))
        //                {
        //                    checkPageSource = globusHelper.getHtmlfromUrl(new Uri("https://twitter.com/"), "", "");
        //                }

        //                string pstAuthToken = PostAuthenticityToken(checkPageSource, "postAuthenticityToken");

        //                #region Profilig Of new created account
        //                try
        //                {
        //                    new Thread(() =>
        //                    {
        //                        startProfilingAfterAccountCreation(new object[] { Email, Password, IPAddress, IPPort, IPUsername, IPpassword, pstAuthToken, globusHelper });
        //                    }).Start();

        //                }
        //                catch (Exception)
        //                {
        //                }
        //                #endregion


        //                #region for Follow
        //                try
        //                {
        //                    new Thread(() =>
        //                    {
        //                        Follow(ref globusHelper, Email, pstAuthToken);
        //                    }).Start();
        //                }
        //                catch (Exception)
        //                {

        //                }

        //                #endregion


        //            }
        //            catch (Exception)
        //            {
        //            }
        //            #endregion

        //            clsFBAccount insertUpdateDataBase = new clsFBAccount();
        //            insertUpdateDataBase.InsertUpdateFBAccount(Email.Replace(" ", ""), Password, username, IPAddress, IPPort, IPUsername, IPpassword, "", "");
        //        }
        //        catch (Exception ex)
        //        {
        //            //Log(ex.Message);
        //            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation --> " + ex.Message, Globals.Path_AccountCreatorErrorLog);
        //            GlobusFileHelper.AppendStringToTextfileNewLine("Error --> TwitterSignup -  SignupMultiThreaded() -- Getting After Account creation >>>> " + ex.Message + " || DateTime :- " + DateTime.Now, Globals.Path_TwtErrorLogs);
        //        }
        //    }
        //    else if (AccountCreatePageSource.Contains("Password is too obvious") || AccountCreatePageSource.Contains("\"active error\">Password"))
        //    {
        //        //Count No Of Failure acccounts 
        //        NoOfNonCreatedAccounts++;
        //        //Password = Password + RandomStringGenerator.RandomString(3);
        //        Log("[ " + DateTime.Now + " ] => [ Please Create Accounts With Secure Password ]");
        //        Log("[ " + DateTime.Now + " ] => [ Password is too obvious ]");
        //        Log("[ " + DateTime.Now + " ] => [ Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
        //        //return;
        //    }
        //    else if (AccountCreatePageSource.Contains("/sessions/destroy") && AccountCreatePageSource.Contains("/signup"))
        //    {
        //        //Count No Of Failure acccounts 
        //        NoOfNonCreatedAccounts++;

        //        //Write Account Info in Test file 
        //        //if Account creation session is exprired .
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
        //        Log("[ " + DateTime.Now + " ] => [ You can't do that right now. ]");
        //        Log("[ " + DateTime.Now + " ] => [ Sorry, please try again later ]");
        //        Log("[ " + DateTime.Now + " ] => [ and Change IP to create more accounts. ]");
        //        return;
        //    }
        //    else
        //    {
        //        //Count No Of Failure acccounts 
        //        NoOfNonCreatedAccounts++;
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword, Globals.path_FailedCreatedAccounts);
        //        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + Password, Globals.path_FailedCreatedAccountsOnlyEmailPass);
        //        Log("[ " + DateTime.Now + " ] => [ Couldn't create Account ]");
        //        Log("[ " + DateTime.Now + " ] => [ Email : Password --> " + Email + ":" + Password + ":" + IPAddress + ":" + IPPort + ":" + IPUsername + ":" + IPpassword + " ]");
        //    }

        //}


        #region Follow Implementation
        private void Follow(ref GlobusHttpHelper globusHttpHelper, string email, string postAuthenticityToken)
        {

            if (_QFollowUserName.Count < 1 && _lstFollowUserName.Count > 0)
            {
                foreach (string item in _lstFollowUserName)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            _QFollowUserName.Enqueue(item);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            if (_QFollowUserName.Count > 0)
            {
                try
                {



                    string FollowedUsername = _QFollowUserName.Dequeue();
                    string username = string.Empty;
                    string userid = string.Empty;
                    bool isFollowed = false;

                    if (!string.IsNullOrEmpty(FollowedUsername))
                    {
                        string date = DateTime.Today.ToString();
                        DataTable dt_SelectFollowDataAccordingDate = obj_ClsDB_TwitterSignup.SelectFollowDataAccordingDate(email, date);

                        int countRows_SelectFollowDataAccordingDate = dt_SelectFollowDataAccordingDate.Rows.Count;

                        bool isAlreadyFollowed = true;
                        string status = string.Empty;
                        DataTable dt = obj_ClsDB_TwitterSignup.SelectFollowData(email);

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr.ItemArray[2].ToString().Contains(FollowedUsername) || dr.ItemArray[3].ToString().Contains(FollowedUsername))
                            {
                                Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + email + " ]");
                                isFollowed = true;
                            }
                        }

                        if (!isFollowed)
                        {
                            string Status = string.Empty;

                            if (NumberHelper.ValidateNumber(FollowedUsername))
                            {
                                username = TwitterSignup_TwitterDataScrapper.GetUserIdToUserName_New(FollowedUsername, out Status, ref globusHttpHelper);
                                userid = FollowedUsername;
                            }
                            else
                            {
                                string outStatus = string.Empty;
                                //userid = TwitterSignup_TwitterDataScrapper.GetUsernameToUserID_New(FollowedUsername, out outStatus);
                                userid = TwitterSignup_TwitterDataScrapper.GetUsernameToUserID_New(FollowedUsername, out Status, ref globusHttpHelper);
                                username = FollowedUsername;
                            }
                            //if (_DontFollowUsersThatUnfollowedBefore)
                            //{
                            //    isAlreadyFollowed = AlreadyFollowing.Exists(s => (s == FollowedUsername));
                            //}
                            if (!isAlreadyFollowed)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + email + " ]");
                                // continue;
                            }

                            // Follow 
                            //Obj_Follow.FollowUsingProfileID(ref globusHttpHelper, "", postAuthenticityToken, FollowedUsername, out status);
                            Obj_Follow.FollowUsingProfileID_New(ref globusHttpHelper, "", postAuthenticityToken, FollowedUsername, out status);

                            if (status == "followed")
                            {
                                Log("[ " + DateTime.Now + " ] => [ Followed " + FollowedUsername + " From " + email + " ]");
                                obj_ClsDB_TwitterSignup.Follow_InserIntotb_Randomiser(email, "Followed", FollowedUsername, DateTime.Now.ToString(), "0", "0");

                                obj_ClsDB_TwitterSignup.InsertUpdateFollowTable(email, userid, username);
                            }
                            else
                            {
                                _QFollowUserName.Enqueue(FollowedUsername);
                                Log("[ " + DateTime.Now + " ] => [ Not Followed " + FollowedUsername + " From " + email + " ]");
                            }

                            ////Delay
                            //int delay = new Random().Next(MinTimeDelay, MaxTimeDelay);

                            //Log((delay / (60 * 1000)).ToString() + " Minutes Delay With User Name : " + UserName);

                            //Thread.Sleep(delay);
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Already Followed " + FollowedUsername + " From " + email + " ]");
                        }
                    }
                }
                catch
                {
                }
            }
        }
        #endregion



        #region Get User Authentication Token

        public static string PostAuthenticityToken(string data, string paramName)
        {
            string value = string.Empty;
            int startIndx = data.IndexOf(paramName);
            if (startIndx > 0)
            {
                int indexstart = startIndx + paramName.Length + 3;
                int endIndx = data.IndexOf("\"", startIndx);

                value = data.Substring(startIndx, endIndx - startIndx).Replace(",", "");

                if (value.Contains(paramName))
                {
                    try
                    {
                        string[] getOuthentication = System.Text.RegularExpressions.Regex.Split(data, "\"postAuthenticityToken\":\"");
                        string[] authenticity = Regex.Split(getOuthentication[1], ",");

                        if (authenticity[0].IndexOf("\"") > 0)
                        {
                            int indexStart1 = authenticity[0].IndexOf("\"");
                            string start = authenticity[0].Substring(0, indexStart1);
                            value = start.Replace("\"", "").Replace(":", "");
                        }
                        //else
                        //{
                        //    //value = getOuthentication[0].Substring(getOuthentication[1].IndexOf(":") + 2, getOuthentication[1].IndexOf(",\"currentUser") - getOuthentication[1].IndexOf(":") - 3);
                        //    value = authenticity[0].Replace("\"", "").Replace(":", "");
                        //}
                    }
                    catch { };
                }

                return value;
            }
            else
            {
                string[] array = Regex.Split(data, "<input type=\"hidden\"");
                foreach (string item in array)
                {
                    if (item.Contains("authenticity_token"))
                    {
                        int startindex = item.IndexOf("value=\"");
                        if (startindex > 0)
                        {
                            string start = item.Substring(startindex).Replace("value=\"", "");
                            int endIndex = start.IndexOf("\"");
                            string end = start.Substring(0, endIndex);
                            value = end;
                            break;
                        }
                    }
                }

            }
            return value;
        }

        #endregion



        #region A/c Profiling In Accounts Creator Module

        public int count_profileDescription = 0;
        public int count_profileURL = 0;
        public int count_profileLocation = 0;
        public int count_profilePic = 0;

        public void startProfilingAfterAccountCreation(object oo)
        {
            ProfileManager.ProfileManager profileUpdater = new ProfileManager.ProfileManager();

            string postAuthenticityToken = string.Empty;
            string Email = string.Empty;
            string password = string.Empty;
            string p_Add = string.Empty;
            string p_port = string.Empty;
            string P_username = string.Empty;
            string p_pass = string.Empty;
            string profileUsername = string.Empty;
            string profileURL = string.Empty;
            string profileDescription = string.Empty;
            string profileLocation = string.Empty;
            string profilePic = string.Empty;


            Array ACProfileArr = (Array)oo;

            Email = (string)ACProfileArr.GetValue(0);
            password = (string)ACProfileArr.GetValue(1);
            p_Add = (string)ACProfileArr.GetValue(2);
            p_port = (string)ACProfileArr.GetValue(3);
            P_username = (string)ACProfileArr.GetValue(4);
            p_pass = (string)ACProfileArr.GetValue(5);
            postAuthenticityToken = (string)ACProfileArr.GetValue(6);
            GlobusHttpHelper globusHttpHelper = (GlobusHttpHelper)ACProfileArr.GetValue(7);


            //********Get location from Uploaded list...

            if (AC_IsUseLoaction)
            {
                if (count_profileLocation < _lstAcLocation.Count)
                {
                    profileLocation = _lstAcLocation[count_profileLocation];
                    count_profileLocation++;
                    //AddToListAccountsLogs(Email + " >>>> Location : " + profileLocation);
                }
                else
                {
                    count_profileLocation = 0;
                    profileLocation = _lstAcLocation[count_profileLocation];
                    //AddToListAccountsLogs(Email + " >>>> Location : " + profileLocation);
                }
            }


            // *******Get profile Usl From uploaded list ....!!

            if (AC_IsUseProfileURL)
            {
                if (count_profileURL < _lstAcProfileURL.Count)
                {
                    profileURL = _lstAcProfileURL[count_profileURL];
                    count_profileURL++;
                    //AddToListAccountsLogs(Email + " >>>> Url : " + profileURL);
                }
                else
                {
                    count_profileURL = 0;
                    profileURL = _lstAcProfileURL[count_profileURL];
                    //AddToListAccountsLogs(Email + " >>>> Url : " + profileURL);
                }
            }

            //***********Get discription from Uploaded List..!!

            if (AC_IsUseDescription)
            {
                if (count_profileDescription < _lstAcUseDescription.Count)
                {
                    profileDescription = _lstAcUseDescription[count_profileDescription];
                    count_profileDescription++;
                    //AddToListAccountsLogs(Email + " >>>> Description : " + profileDescription);
                }
                else
                {
                    count_profileDescription = 0;
                    profileDescription = _lstAcUseDescription[count_profileDescription];
                    //AddToListAccountsLogs(Email + " >>>> Description : " + profileDescription);
                }
            }



            //***********Get Profile image from Uploaded List..!!

            if (AC_IsUseProfileImage)
            {
                if (count_profilePic < _lstAcProfileImage.Count)
                {
                    profilePic = _lstAcProfileImage[count_profilePic];
                    count_profilePic++;
                    //AddToListAccountsLogs(Email + " >>>> Description : " + profileDescription);
                }
                else
                {
                    count_profilePic = 0;
                    profilePic = _lstAcProfileImage[count_profilePic];
                    //AddToListAccountsLogs(Email + " >>>> Description : " + profileDescription);
                }
            }




            //******Get change Details of Prodile ********

            try
            {
                Thread.Sleep(500);

                if (!string.IsNullOrEmpty(profileUsername) || !string.IsNullOrEmpty(profileLocation) || !string.IsNullOrEmpty(profileURL) || !string.IsNullOrEmpty(profileDescription) || !string.IsNullOrEmpty(profilePic))
                {
                    if (profileUpdater.UpdateProfile(profileUsername, profileLocation, profileURL, profileDescription, profilePic, postAuthenticityToken, ref globusHttpHelper))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Profile Updated : " + Email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + password + ":" + p_Add + ":" + p_port + ":" + P_username + ":" + p_pass, Globals.path_SuccessfullyProfiledAccounts);
                    }
                    else
                    {
                        Log("[ " + DateTime.Now + " ] => [ Unable to Update Profile : " + Email + " ]");
                        GlobusFileHelper.AppendStringToTextfileNewLine(Email + ":" + password + ":" + p_Add + ":" + p_port + ":" + P_username + ":" + p_pass, Globals.path_FailedToProfileAccounts);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion


        public static void AbortThreads()
        {
            Dictionary<string, Thread> tempdictionary_Threads = new Dictionary<string, Thread>();
            foreach (KeyValuePair<string, Thread> item in dictionary_Threads)
            {
                try
                {
                    tempdictionary_Threads.Add(item.Key, item.Value);
                }
                catch { }
            }
            try
            {
                foreach (KeyValuePair<string, Thread> item in tempdictionary_Threads)//(KeyValuePair<string, Thread> item in dictionary_Threads)
                {
                    try
                    {
                        string key = item.Key;
                        string threadName = Regex.Split(key, "_")[0];
                        string module = "AccountCreator";// module.Replace("_", "");
                        //Thread thread = item.Value;
                        if (threadName.Contains(module))
                        {
                            //Log("Aborting : " + key);
                            //thread.Abort();
                            Thread thread = item.Value;
                            int abortCounter = 0;

                            if (thread != null)
                            {
                                //if (thread.ThreadState == ThreadState.Running || thread.ThreadState == ThreadState.WaitSleepJoin || thread.ThreadState == ThreadState.Background)
                                //{
                                //    thread.Abort();
                                //}
                                //else
                                //{
                                thread.Abort();
                                //}
                                //Log("Aborted : " + key);
                                dictionary_Threads.Remove(key);

                            }

                        }

                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine("Error in Abort in Profile Manager Foreach Loop " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        }

    }
}
