using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Globussoft;

namespace BaseLib
{
    public class FBAccountChecker
    {

        string Content = string.Empty;
        string ChkData = string.Empty;

        public bool AccountCheck(string response, string Username, string password, ref string statusMessage)
        {
            Content = Username + ":" + password;
            if (!string.IsNullOrEmpty(response))
            {
                if (response.Contains("Account Disabled"))
                {
                    statusMessage = "Account Disabled with " + Username;
                    ChkData = "DisableFbAccount";
                }
                else if (response.Contains("Incorrect Email"))
                {
                    ChkData = "IncorrectFbAccount";
                    statusMessage = "Incorrect Email with " + Username;
                }
                else if (response.Contains("Account Not Confirmed"))
                {
                    ChkData = "AccountNotConfirmed";
                    statusMessage = "Account Not Confirmed with " + Username;
                }
                else if (response.Contains("security check"))
                {
                    ChkData = "PhoneVerifyFbAccount";
                    statusMessage = "Required Phone verification with " + Username;
                }
                else if (response.Contains("Use a phone to verify your account"))
                {
                    ChkData = "PhoneVerifyFbAccount";
                    statusMessage = "Required Phone verification with " + Username;
                }
                else if (response.Contains("temporarily locked"))
                {
                    ChkData = "TemporarilyFbAccount";
                    statusMessage = "Your account is temporarily locked with " + Username;
                }
                else if (response.Contains("post_form_id") && response.Contains("Home") && response.Contains("Find Friends"))
                {
                    ChkData = "CorrectFbAccount";
                    statusMessage = "Correct FbAccount with " + Username;
                }
                else
                {
                    ChkData = "NotCorrectFbAccount";
                    statusMessage = "There is not Correct FbAccount with " + Username;
                }

                if (!string.IsNullOrEmpty(ChkData) && !string.IsNullOrEmpty(Content))
                {
                    CreateFileLikeDeskTop(Content, ChkData);
                    return true;
                }
                return false;              
            }
            else
            {
                
                return false;
            }
        }

        
        public void CreateFileLikeDeskTop(string Content, string CheckData)
        {
            //string strPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FaceDominatorLikeAccount";
            string strPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FaceDominatorFbAccount";

            try
            {
                #region Commented
                //if (Directory.Exists(strPath))
                //{
                //    if (CheckLike=="Like")
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\LikedFbAccount.txt"); 
                //    }
                //    if (CheckLike == "UnLike")
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\UnLikedFbAccount.txt");
                //    }
                //}
                //else
                //{
                //    Directory.CreateDirectory(strPath);
                //    if (CheckLike == "Like")
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\LikedFbAccount.txt");
                //    }
                //    if (CheckLike == "UnLike")
                //    {
                //        GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\UnLikedFbAccount.txt");
                //    }
                //} 
                #endregion
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                if (CheckData == "FriendProfileUrl")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\FriendProfileUrl.txt");

                }
                else if (CheckData == "DisableFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\DisableFbAccount.txt");

                }
                else if (CheckData == "IncorrectFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\IncorrectFbAccount.txt");

                }
                else if (CheckData == "PhoneVerifyFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\PhoneVerifyFbAccount.txt");

                }
                else if (CheckData == "CorrectFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\CorrectFbAccount.txt");
                }
                else if (CheckData == "TemporarilyFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\TemporarilyFbAccount.txt");
                }
                else if (CheckData == "AccountNotConfirmed")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\AccountNotConfirmed.txt");
                }
                else if (CheckData == "NotCorrectFbAccount")
                {
                    GlobusFileHelper.AppendStringToTextfileNewLine(Content, strPath + "\\NotCorrectFbAccount.txt");
                }
                //GlobusFileHelper.WriteListtoTextfile(listDisabledFbAccount, strPath + "\\DisableFbAccount.txt");
                //    GlobusFileHelper.WriteListtoTextfile(listIncorrectFbAccount, strPath + "\\IncorrectFbAccount.txt");
                //    GlobusFileHelper.WriteListtoTextfile(listPhoneVerifyFbAccount, strPath + "\\PhoneVerifyFbAccount.txt");
                //    GlobusFileHelper.WriteListtoTextfile(listCorrectFbAccount, strPath + "\\CorrectFbAccount.txt");
                //    GlobusFileHelper.WriteListtoTextfile(listTemporarilyFbAccount, strPath + "\\TemporarilyFbAccount.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private void CreateFolderFileDeskTop()
        //{
        //    string strPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\FaceDominatorFbAccount";

        //    try
        //    {
        //        if (Directory.Exists(strPath))
        //        {
        //            GlobusFileHelper.WriteListtoTextfile(listDisabledFbAccount, strPath + "\\DisableFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listIncorrectFbAccount, strPath + "\\IncorrectFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listPhoneVerifyFbAccount, strPath + "\\PhoneVerifyFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listCorrectFbAccount, strPath + "\\CorrectFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listTemporarilyFbAccount, strPath + "\\TemporarilyFbAccount.txt");

        //        }
        //        else
        //        {
        //            Directory.CreateDirectory(strPath);
        //            GlobusFileHelper.WriteListtoTextfile(listDisabledFbAccount, strPath + "\\DisableFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listIncorrectFbAccount, strPath + "\\IncorrectFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listPhoneVerifyFbAccount, strPath + "\\PhoneVerifyFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listCorrectFbAccount, strPath + "\\CorrectFbAccount.txt");
        //            GlobusFileHelper.WriteListtoTextfile(listTemporarilyFbAccount, strPath + "\\TemporarilyFbAccount.txt");

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.Message);
        //    }
        //}
    }
}
