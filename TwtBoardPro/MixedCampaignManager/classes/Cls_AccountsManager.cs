using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampaignManager;
using BaseLib;

namespace MixedCampaignManager.classes
{
    public class Cls_AccountsManager
    {

        public CampaignTweetAccountContainer AccountManager(List<string> templist)
        {
            CampaignTweetAccountContainer objCampaignTweetAccountContainer = new CampaignTweetAccountContainer();
            //List<CampaignTweetAccountContainer> lst_CampaignTweetAccount = new List<CampaignTweetAccountContainer>();
            foreach (string item in templist)
            {

                string account = item;
                string[] AccArr = account.Split(':');
                if (AccArr.Count() > 1)
                {
                    string accountUser = account.Split(':')[0];
                    string accountPass = account.Split(':')[1];
                    string screanName = string.Empty;
                    string IPAddress = string.Empty;
                    string IPPort = string.Empty;
                    string IPUsername = string.Empty;
                    string IPpassword = string.Empty;
                    string status = string.Empty;

                    int DataCount = account.Split(':').Length;
                    if (DataCount == 2)
                    {
                        //Globals.accountMode = AccountMode.NoIP;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];

                    }
                    else if (DataCount == 3)
                    {
                        //Globals.accountMode = AccountMode.PublicIP;
                        screanName = account.Split(':')[2];
                    }
                    else if (DataCount == 4)
                    {
                        //Globals.accountMode = AccountMode.PublicIP;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        IPAddress = account.Split(':')[2];
                        IPPort = account.Split(':')[3];
                    }
                    else if (DataCount == 5)
                    {
                        //Globals.accountMode = AccountMode.PublicIP;
                        screanName = account.Split(':')[2];
                        IPAddress = account.Split(':')[3];
                        IPPort = account.Split(':')[4];
                    }
                    else if (DataCount > 5 && DataCount < 7)
                    {
                        //Globals.accountMode = AccountMode.PrivateIP;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        IPAddress = account.Split(':')[2];
                        IPPort = account.Split(':')[3];
                        IPUsername = account.Split(':')[4];
                        IPpassword = account.Split(':')[5];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, IPAddress, IPPort, IPUsername, IPpassword, "", "0");
                    }
                    else if (DataCount == 7)
                    {
                        //Globals.accountMode = AccountMode.PrivateIP;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        IPAddress = account.Split(':')[3];
                        IPPort = account.Split(':')[4];
                        IPUsername = account.Split(':')[5];
                        IPpassword = account.Split(':')[6];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, IPAddress, IPPort, IPUsername, IPpassword, "", "0");
                    }
                    else if (DataCount == 9)
                    {
                        //Globals.accountMode = AccountMode.PrivateIP;
                        accountUser = account.Split(':')[0];
                        accountPass = account.Split(':')[1];
                        IPAddress = account.Split(':')[2];
                        IPPort = account.Split(':')[3];
                        IPUsername = account.Split(':')[4];
                        IPpassword = account.Split(':')[5];
                        //dt.Rows.Add(accountUser, accountPass, string.Empty , string.Empty, IPAddress, IPPort, IPUsername, IPpassword, "", "0");
                    }
                    //if (Globals.IsFreeVersion)
                    //{
                    //    if (dt.Rows.Count >= 5)
                    //    {
                    //        FrmFreeTrial frmFreeTrial = new FrmFreeTrial();
                    //        frmFreeTrial.TopMost = true;
                    //        frmFreeTrial.BringToFront();
                    //        frmFreeTrial.ShowDialog();
                    //        break;
                    //    }
                    //}

                    //dt.Rows.Add(accountUser, accountPass, screanName, string.Empty, IPAddress, IPPort, IPUsername, IPpassword, "", "0");

                    try
                    {
                        CampaignAccountManager twitter = new CampaignAccountManager();
                        twitter.Username = accountUser;
                        twitter.Password = accountPass;
                        twitter.IPAddress = IPAddress;
                        twitter.IPPort = IPPort;
                        twitter.IPUsername = IPUsername;
                        twitter.IPpassword = IPpassword;
                        twitter.Screen_name = screanName;
                       // twitter.profileStatus = 0;
                        twitter.AccountStatus = "";
                        objCampaignTweetAccountContainer.dictionary_CampaignAccounts.Add(twitter.Username, twitter);

                        //lst_CampaignTweetAccount.Add(objCampaignTweetAccountContainer);
                    }
                    catch { }

                    //string profileStatus = "0";
                }
                else
                {
                   // AddToListBox("Account has some problem : " + item);
                }
            }
            return objCampaignTweetAccountContainer;
        }
    }
}
