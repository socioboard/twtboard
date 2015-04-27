using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenPOP.POP3;
using System.Text.RegularExpressions;
using Globussoft;

namespace BaseLib
{
    public class FBLoginChecker
    {
        Globussoft.GlobusHttpHelper HttpHelper = new Globussoft.GlobusHttpHelper();

        public Events pumpMessageEvent = new Events();

        string Username = string.Empty;
        string Password = string.Empty;

        string proxyAddress = string.Empty;
        string proxyPort = string.Empty;
        string proxyUsername = string.Empty;
        string proxyPassword = string.Empty;

        string incorrectEmailFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\IncorrectEmail.txt";
        string failedAccountsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FailedAccounts.txt";

        #region Earlier CheckLoginMethod
        //public bool CheckLogin(string response, string username, string password)
        //{
        //    this.Username = username;
        //    this.Password = password;

        //    if (!string.IsNullOrEmpty(response))
        //    {
        //        if (response.ToLower().Contains("unusual login activity"))
        //        {
        //            Console.WriteLine("Unusual Login Activity: " + username);
        //            PumpMessage("Unusual Login Activity: " + username);
        //            return false;
        //        }
        //        if (response.ToLower().Contains("incorrect username"))
        //        {
        //            Console.WriteLine("Incorrect username: " + username);
        //            PumpMessage("Incorrect username: " + username);
        //            return false;
        //        }
        //        if (response.ToLower().Contains("Choose a verification method".ToLower()))
        //        {
        //            Console.WriteLine("Choose a verification method: " + username);
        //            PumpMessage("Choose a verification method: " + username);
        //            return false;
        //        }
        //        if (response.ToLower().Contains("not logged in".ToLower()))
        //        {
        //            Console.WriteLine("not logged in: " + username);
        //            PumpMessage("not logged in: " + username);
        //            return false;
        //        }
        //        if (response.Contains("Please log in to continue".ToLower()))
        //        {
        //            Console.WriteLine("Please log in to continue: " + username);
        //            PumpMessage("Please log in to continue: " + username);
        //            return false;
        //        }
        //        if (response.Contains("re-enter your password"))
        //        {
        //            Console.WriteLine("Wrong password for: " + username);
        //            PumpMessage("Wrong password for: " + username);
        //            return false;
        //        }
        //        if (response.Contains("Incorrect Email"))
        //        {
        //            Console.WriteLine("Incorrect email: " + username);
        //            PumpMessage("Incorrect email: " + username);
        //            return false;
        //        }
        //        if (response.Contains("have been blocked"))
        //        {
        //            Console.WriteLine("you have been blocked: " + username);
        //            PumpMessage("you have been blocked: " + username);
        //            return false;
        //        }
        //        if (response.Contains("account has been disabled"))
        //        {
        //            Console.WriteLine("your account has been disabled: " + username);
        //            PumpMessage("your account has been disabled: " + username);
        //            return false;
        //        }
        //        if (response.Contains("Please complete a security check"))
        //        {
        //            Console.WriteLine("Please complete a security check: " + username);
        //            PumpMessage("Use a phone to verify your account : " + username);
        //            InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountCreated));
        //            return false;
        //        }
        //        if (response.Contains("Please complete a security check"))
        //        {
        //            Console.WriteLine("You must log in to see this page: " + username);
        //            PumpMessage("required phone verification : " + username);
        //            return false;
        //        }
        //        if (response.Contains("Your account is temporarily locked"))
        //        {
        //            Console.WriteLine("Your account is temporarily locked: " + username);
        //            PumpMessage("your account has been disabled: " + username);
        //            return false;
        //        }
        //        if (response.Contains("You must log in to see this page"))
        //        {
        //            Console.WriteLine("You must log in to see this page: " + username);
        //            PumpMessage("You must log in to see this page: " + username);
        //            return false;
        //        }
        //        if (response.Contains("Suspicious activity has been detected on your Facebook account"))
        //        {
        //            Console.WriteLine("Suspicious activity: " + username);
        //            PumpMessage("Suspicious activity: " + username);
        //            return false;
        //        }

        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //} 
        #endregion


        /// <summary>
        /// Checks if account is valid
        /// </summary>
        public bool CheckLogin(string response, string username, string password, string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword)
        {
            this.Username = username;
            this.Password = password;

            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;

            if (!string.IsNullOrEmpty(response))
            {
                if (response.ToLower().Contains("unusual login activity"))
                {
                    Console.WriteLine("Unusual Login Activity: " + username);
                    PumpMessage("Unusual Login Activity: " + username);
                    return false;
                }
                if (response.ToLower().Contains("incorrect username"))
                {
                    Console.WriteLine("Incorrect username: " + username);
                    PumpMessage("Incorrect username: " + username);
                    return false;
                }
                if (response.ToLower().Contains("Choose a verification method".ToLower()))
                {
                    Console.WriteLine("Choose a verification method: " + username);
                    PumpMessage("Choose a verification method: " + username);
                    return false;
                }
                if (response.ToLower().Contains("not logged in".ToLower()))
                {
                    Console.WriteLine("not logged in: " + username);
                    PumpMessage("not logged in: " + username);
                    return false;
                }
                if (response.Contains("Please log in to continue".ToLower()))
                {
                    Console.WriteLine("Please log in to continue: " + username);
                    PumpMessage("Please log in to continue: " + username);
                    return false;
                }
                if (response.Contains("re-enter your password"))
                {
                    Console.WriteLine("Wrong password for: " + username);
                    PumpMessage("Wrong password for: " + username);
                    return false;
                }
                if (response.Contains("Incorrect Email"))
                {
                    Console.WriteLine("Incorrect email: " + username);

                    try
                    {
                        ///Write Incorrect Emails in text file
                        GlobusFileHelper.AppendStringToTextfileNewLine(username + ":" + password, incorrectEmailFilePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.IncorrectEmail));
                    PumpMessage("Incorrect email: " + username);
                    return false;
                }
                if (response.Contains("have been blocked"))
                {
                    Console.WriteLine("you have been blocked: " + username);
                    WriteToTextFile();
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountDisabled));
                    PumpMessage("you have been blocked: " + username);
                    return false;
                }
                if (response.Contains("account has been disabled"))
                {
                    Console.WriteLine("your account has been disabled: " + username);
                    WriteToTextFile();
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountDisabled));
                    PumpMessage("your account has been disabled: " + username);
                    return false;
                }
                if (response.Contains("Please complete a security check"))
                {
                    Console.WriteLine("Please complete a security check: " + username);
                    PumpMessage("Use a phone to verify your account : " + username);
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountCreated));
                    return false;
                }
                if (response.Contains("Please complete a security check"))
                {
                    Console.WriteLine("You must log in to see this page: " + username);
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountCreated));
                    PumpMessage("required phone verification : " + username);
                    return false;
                }
                if (response.Contains("<input value=\"Sign Up\" onclick=\"RegistrationBootloader.bootloadAndValidate();"))
                {
                    Console.WriteLine("Not logged in with: " + username);
                    //InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.AccountCreated));
                    PumpMessage("Not logged in with: " + username);
                    return false;
                }
                if (response.Contains("Account Not Confirmed"))
                {
                    Console.WriteLine("Account Not Confirmed " + Username);
                    PumpMessage("Account Not Confirmed " + Username);
                    return false;
                }
                if (response.Contains("Your account is temporarily locked"))
                {
                    Console.WriteLine("Your account is temporarily locked: " + username);
                    WriteToTextFile();
                    PumpMessage("your account has been disabled: " + username);
                    return false;
                }
                if (response.Contains("You must log in to see this page"))
                {
                    Console.WriteLine("You must log in to see this page: " + username);
                    WriteToTextFile();
                    PumpMessage("You must log in to see this page: " + username);
                    return false;
                }
                if (response.Contains("Suspicious activity has been detected on your Facebook account"))
                {
                    Console.WriteLine("Suspicious activity: " + username);
                    WriteToTextFile();
                    PumpMessage("Suspicious activity: " + username);
                    return false;
                }

            }
            else
            {
                return false;
            }
            return true;
        }

        private void PumpMessage(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            pumpMessageEvent.LogText(eventArgs);
        }


        /// <summary>
        /// Checks for Email Verification
        /// Also does Skip
        /// </summary>
        public bool CheckVerification(string response, ref Globussoft.GlobusHttpHelper HttpHelper)
        {
            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not
            string pageSourceCheck = string.Empty;
            pageSourceCheck = response;
            string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;

            #region Commented
            //if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
            //{
            //    pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
            //}
            //if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
            //{
            //    pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
            //}
            //if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            //{
            //    pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
            //}
            //if (pageSrc3.Contains("complete the sign-up process"))
            //{
            //    return false;
            //}
            //if (pageSourceCheck.Contains("complete the sign-up process"))
            //{
            //    return false;
            //}
            
            #endregion

            ///Skip Code
            if ((pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step")) || pageSourceCheck.Contains("window.location.replace(\"http:\\/\\/www.facebook.com\\/gettingstarted.php") || pageSourceCheck.Contains("window.location.replace(\"http:\\/\\/www.facebook.com\\/confirmemail.php"))
            {
                pageSrc1 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers"));
                //}
                Thread.Sleep(300);
                //if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
                //{
                pageSrc2 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic"));
                Thread.Sleep(300);
                //}
                //if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
                //{
                pageSrc3 = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/gettingstarted.php?step=summary"));
                Thread.Sleep(300);
                //}

                ///Check if asks for email
                //If asks then Set Status as PhoneVerfiedOnly
                if (pageSrc3.Contains("complete the sign-up process"))
                {
                    PumpMessage("Account : " + Username + " is NOT Email verified");
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                    return false;
                }
                else if (pageSourceCheck.Contains("please login to your email account below") || pageSourceCheck.Contains("Go to your email"))
                {
                    PumpMessage("Account : " + Username + " is NOT Email verified");
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                    return false;
                }
            }

            ///Check if asks for email
            //If asks then Set Status as PhoneVerfiedOnly
            if (pageSourceCheck.Contains("complete the sign-up process"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }
            else if (pageSourceCheck.Contains("window.location.replace(\"http:\\/\\/www.facebook.com\\/confirmemail.php"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }
            else if (pageSourceCheck.Contains("please login to your email account below") || pageSourceCheck.Contains("Go to your email"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }

            #endregion
            //** FB Account Check email varified or not ***********************************************************************************//

            PumpMessage("Account : " + Username + " is Email & Phone verified");
            InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhonePlusEmailVerified));


            return true;
        }

        private readonly POPClient popClient = new POPClient();


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
            // *** Change by ritesh 10-9-11 ********************************************
            return lstUrl;
        }

        public bool CheckVerificationChilkat(string response, ref Chilkat.Http Http)
        {
            //** FB Account Check email varified or not ***********************************************************************************//
            #region  FB Account Check email varified or not
            string pageSourceCheck = string.Empty;
            pageSourceCheck = response;
            string pageSrc1 = string.Empty;
            string pageSrc2 = string.Empty;
            string pageSrc3 = string.Empty;
            string pageSrc4 = string.Empty;
            string substr1 = string.Empty;


            //if (pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step"))
            //{
            //    pageSrc1 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers");
            //}
            //if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
            //{
            //    pageSrc2 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic");
            //}
            //if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
            //{
            //    pageSrc3 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=summary");
            //}
            //if (pageSrc3.Contains("complete the sign-up process"))
            //{
            //    return false;

            //}
            //if (pageSourceCheck.Contains("complete the sign-up process"))
            //{
            //    return false;
            //}
            //#endregion
            ////** FB Account Check email varified or not ***********************************************************************************//
            //return true;

            if ((pageSourceCheck.Contains("Are your friends already on Facebook?") && pageSourceCheck.Contains("Skip this step")) || pageSourceCheck.Contains("window.location.replace(\"http:\\/\\/www.facebook.com\\/gettingstarted.php"))
            {
                pageSrc1 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=classmates_coworkers");
                //}
                Thread.Sleep(300);
                //if (pageSrc1.Contains("Fill out your Profile Info") && pageSrc1.Contains("Skip"))
                //{
                pageSrc2 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=upload_profile_pic");
                Thread.Sleep(300);
                //}
                //if (pageSrc2.Contains("Set your profile picture") && pageSrc2.Contains("Skip"))
                //{
                pageSrc3 = Http.QuickGetStr("http://www.facebook.com/gettingstarted.php?step=summary");
                Thread.Sleep(300);
                //}

                ///Check if asks for email
                //If asks then Set Status as PhoneVerfiedOnly
                if (pageSrc3.Contains("complete the sign-up process"))
                {
                    PumpMessage("Account : " + Username + " is NOT Email verified");
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                    return false;
                }
                else if (pageSourceCheck.Contains("please login to your email account below") || pageSourceCheck.Contains("Go to your email"))
                {
                    PumpMessage("Account : " + Username + " is NOT Email verified");
                    InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                    return false;
                }
            }

            ///Means NOT Email verified
            //Check if asks for email
            //If asks then Set Status as PhoneVerfiedOnly
            if (pageSourceCheck.Contains("complete the sign-up process"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }
            else if (pageSourceCheck.Contains("window.location.replace(\"http:\\/\\/www.facebook.com\\/confirmemail.php"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }
            else if (pageSourceCheck.Contains("please login to your email account below") || pageSourceCheck.Contains("Go to your email"))
            {
                PumpMessage("Account : " + Username + " is NOT Email verified");
                InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhoneVerfiedOnly));
                return false;
            }

            #endregion
            //** FB Account Check email varified or not ***********************************************************************************//

            PumpMessage("Account : " + Username + " is Email & Phone verified");
            InsertUpdateDatabase(AccountStatus.Status(ProfileStatus.PhonePlusEmailVerified));
            return true;

        }

        /// <summary>
        /// Write Failed Accounts to FailedAccounts.txt on Desktop
        /// </summary>
        private void WriteToTextFile()
        {
            try
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(Username + ":" + Password, failedAccountsFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertUpdateDatabase(string status)
        {
            try
            {
                ///Insert
                DataBaseHandler.InsertQuery("Insert into tb_FBAccount values('" + Username + "','" + Password + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + "" + "','" + AccountStatus.Status(ProfileStatus.AccountCreated) + "')", "tb_FBAccount");
            }
            catch (Exception)  //If already there, Update
            {
                ///Update records in Database
                string updateQuery = "Update tb_FBAccount SET ProfileStatus='" + status + "' WHERE UserName='" + Username + "'";
                DataBaseHandler.UpdateQuery(updateQuery, "tb_FBAccount");
            }
        }

        
    }
}
