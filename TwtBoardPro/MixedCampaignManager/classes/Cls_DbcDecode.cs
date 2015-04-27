using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Globussoft;
using BaseLib;

namespace MixedCampaignManager.classes
{
    public class Cls_DbcDecode
    {
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
                    //Log("Please check your DBC Account Details");
                    return null;
                }
                if (client.Balance == 0.0)
                {
                    //Log("You have 0 Balance in your DBC Account");
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
                        //Log("CAPTCHA was not solved");
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

    }

    public class ClsEmailActivator
    {
        public ClsEmailActivator()
        {

        }

        public bool ActivateAccount(string Email, string Password, ref GlobusHttpHelper globusHelper)
        {
            EmailActivator.ClsEmailActivator EmailActivate = new EmailActivator.ClsEmailActivator();
            bool verified = EmailActivate.EmailVerification(Email.Replace(" ", ""), Password, ref globusHelper);
            if (verified)
            {
                return verified;
            }
            else
            {
                return verified;
            }
        }

    }
}
