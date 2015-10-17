using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;

namespace Follower
{
    public class Follower
    {
        //Globussoft.GlobusHttpHelper globusHttpHelper = new Globussoft.GlobusHttpHelper();
        //FaceDominator.Facebooker faceBooker = new FaceDominator.Facebooker();

        #region global declaration
        public Events logEvents = new Events();
        string userID = string.Empty;
        
        #endregion

        #region FollowUsingProfileID
        public void FollowUsingProfileID(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string user_id_toFollow, out string status)
        {
            try
            {
                string FollowingData = string.Empty;
                if (NumberHelper.ValidateNumber(user_id_toFollow))
                {
                    FollowingData = "user_id=" + user_id_toFollow + "&post_authenticity_token=" + postAuthenticityToken + "";
                }
                else
                {
                    FollowingData = "screen_name=" + user_id_toFollow + "&post_authenticity_token=" + postAuthenticityToken + "";
                }
                string res_PostFollow = globusHttpHelper.postFormData(new Uri("https://api.twitter.com/1/friendships/create.json"), FollowingData, "https://api.twitter.com/receiver.html", string.Empty, "XMLHttpRequest", "true", "https://api.twitter.com");

                status = "followed";
            }
            catch (Exception ex)
            {
                status = "not followed";
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID() --> " + ex.Message, Globals.Path_FollowerErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FollowUsingProfileID() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        } 
        #endregion

        /// <summary>
        /// Change follow code  after API change 
        /// </summary>
        /// <param name="thisHttpHelpr"></param>
        /// <param name="ForeignhttpHelpr"></param>

        #region FollowUsingProfileID_New
        public void FollowUsingProfileID_New(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string user_id_toFollow, out string status)
        {
            try
            {
                string data_user_id = string.Empty;
                string PostData = string.Empty;
                
                if (NumberHelper.ValidateNumber(user_id_toFollow))
                {
                    string tempScreenName = string.Empty;
                    data_user_id = user_id_toFollow;
                    string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/intent/user?user_id=" + user_id_toFollow), "", "");
                    if (string.IsNullOrEmpty(pageSource))
                    {
                        pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/intent/user?user_id=" + user_id_toFollow), "", "");
                    }
                    tempScreenName = globusHttpHelper.getBetween(pageSource, "<span class=\"name\">", "</span>");
                    if (!string.IsNullOrEmpty(tempScreenName))
                    {
                        pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + tempScreenName), "", "");
                    }

                    if (!string.IsNullOrEmpty(pageSource))
                    {
                        try
                        {
                            int startIndx = pageSource.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                            int endIndx = pageSource.IndexOf("\"", startIndx);
                            userID = pageSource.Substring(startIndx, endIndx - startIndx);
                        }
                        catch { }

                        if (string.IsNullOrEmpty(userID))
                        {
                            userID = string.Empty;
                            //string[] useridarr = System.Text.RegularExpressions.Regex.Split(pageSource, "data-user-id="); //account-group js-mini-current-user
                            string[] useridarr = System.Text.RegularExpressions.Regex.Split(pageSource, "account-group js-mini-current-user");
                            //foreach (string useridarr_item in useridarr)
                            //{
                            //    if (useridarr_item.Contains("data-screen-name="))
                            //    {
                            //        userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("data-screen-name=") - 3);
                            //        break;
                            //    }
                            //}

                            try
                            {
                                userID = Utils.getBetween(useridarr[1],"data-user-id=\"","\"");
                            }
                            catch { };
                        }
                    }
                }
                else
                {
                    try
                    {
                        string data_id = string.Empty;
                        string pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id_toFollow), "", "");
                        if (string.IsNullOrEmpty(pageSource))
                        {
                            pageSource = globusHttpHelper.getHtmlfromUrl(new Uri("https://twitter.com/" + user_id_toFollow), "", "");
                        }

                        try
                        {
                            int startIndx = pageSource.IndexOf("data-user-id=\"") + "data-user-id=\"".Length;
                            int endIndx = pageSource.IndexOf("\"", startIndx);
                            userID = pageSource.Substring(startIndx, endIndx - startIndx);
                            if (string.IsNullOrEmpty(userID))
                            {
                                try
                                {
                                    //string[] GetUserId = System.Text.RegularExpressions.Regex.Split(pageSource, "user-actions btn-group not-following not-muting");
                                    string[] useridarr = System.Text.RegularExpressions.Regex.Split(pageSource, "account-group js-mini-current-user");

                                    try 
                                    {
                                        userID = Utils.getBetween(useridarr[1], "data-user-id=\"", "\"");
                                    }
                                    catch { };

                                }
                                catch { };
                            }
                        

                        }
                        catch { }

                        if (string.IsNullOrEmpty(userID))
                        {
                            userID = string.Empty;
                            string[] useridarr = System.Text.RegularExpressions.Regex.Split(pageSource, "data-user-id=");
                            foreach (string useridarr_item in useridarr)
                            {
                                if (useridarr_item.Contains("data-screen-name="))
                                {
                                    userID = useridarr_item.Substring(0 + 1, useridarr_item.IndexOf("data-screen-name=") - 3);
                                    break;
                                }
                            }
                        }


                        if (globusHttpHelper.gResponse.ResponseUri.ToString().Contains("suspended"))
                        {
                            status = "not followed";
                            Log(user_id_toFollow + " :-  Account is suspended ");
                            return;
                        }

                        string[] data_id1 = System.Text.RegularExpressions.Regex.Split(pageSource, "data-user-id=");
                        if (pageSource.Contains("js-stream-item stream-item stream-item"))
                        {
                            if (pageSource.Contains("profile-card-inner"))
                            {
                                int startindex = pageSource.IndexOf("profile-card-inner");
                                string start = pageSource.Substring(startindex).Replace("profile-card-inner", "");
                                int endindex = start.IndexOf("\">");
                                string end = start.Substring(start.IndexOf("data-user-id="), endindex - start.IndexOf("data-user-id=")).Replace("data-user-id=", "").Replace("\"", "");
                                data_user_id = end.Trim();
                            }
                            else
                            {
                                try
                                {
                                    //int startindex = pageSource.IndexOf("ProfileTweet-authorDetails\">");
                                    //string start = pageSource.Substring(startindex).Replace("ProfileTweet-authorDetails\">", "");
                                    //int endindex = start.IndexOf("\">");
                                    //string end = start.Substring(start.IndexOf("data-user-id="), endindex - start.IndexOf("data-user-id=")).Replace("data-user-id=", "").Replace("\"", "");
                                    //data_user_id = end.Trim();

                                    string[] getDataUserID = System.Text.RegularExpressions.Regex.Split(pageSource, "ProfileNav");
                                    data_user_id = Utils.getBetween(getDataUserID[1],"data-user-id=\"","\"");
                                }
                                catch { };

                            }
                        }
                        else 
                        {

                            try
                            {
                                int startindex = pageSource.IndexOf("profile_id");
                                string start = pageSource.Substring(startindex).Replace("profile_id", "");
                                int endindex = start.IndexOf(",");
                                string end = start.Substring(0, endindex).Replace("&quot;", "").Replace("\"", "").Replace(":","").Trim();
                                data_user_id = end.Trim();
                            }
                            catch { }

                            if (string.IsNullOrEmpty(data_user_id))
                            {
                                try
                                {
                                    int startindex = pageSource.IndexOf("ProfileTweet-authorDetails\">");
                                    string start = pageSource.Substring(startindex).Replace("ProfileTweet-authorDetails\">", "");
                                    int endindex = start.IndexOf("\">");
                                    string end = start.Substring(start.IndexOf("data-user-id="), endindex - start.IndexOf("data-user-id=")).Replace("data-user-id=", "").Replace("\"", "");
                                    data_user_id = end.Trim();
                                }
                                catch { }
                            }

                            if (string.IsNullOrEmpty(data_user_id))
                            {
                                try
                                {
                                    int startindex = pageSource.IndexOf("stats js-mini-profile-stats \" data-user-id=\"");
                                    if (startindex == -1)
                                    {
                                        startindex = pageSource.IndexOf("user-actions btn-group not-following not-muting \" data-user-id=\"");
                                    }
                                    if (startindex == -1)
                                    {
                                        startindex = pageSource.IndexOf("user-actions btn-group not-following not-muting protected\" data-user-id=\"");
                                    }
                                    string start = pageSource.Substring(startindex).Replace("stats js-mini-profile-stats \" data-user-id=\"", "").Replace("user-actions btn-group not-following not-muting \" data-user-id=\"", "").Replace("user-actions btn-group not-following not-muting protected\" data-user-id=\"", "").Trim();
                                    //int endindex = start.IndexOf("\">");
                                    int endindex = start.IndexOf("\"");
                                    string end = start.Substring(0, endindex);
                                    data_user_id = end.Replace("\"", "");
                                }
                                catch { }
                            }
                        }

                       
                    }
                    catch (Exception err)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID1()-->  " + err.Message, Globals.Path_FollowerErroLog);
                    }
                }
                DataSet dt = DataBaseHandler.SelectQuery("SELECT * FROM tb_user_follower_details Where followerId = '" + data_user_id + "' and userId = '" + userID + "' ", "tb_user_follower_details");
                int count_NO_RoWs = dt.Tables[0].Rows.Count;
                if (count_NO_RoWs == 0)
                {
                    if (!string.IsNullOrEmpty(data_user_id))
                    {
                        PostData = "authenticity_token=" + postAuthenticityToken + "&user_id=" + data_user_id;
                        string res_PostFollow = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/user/follow"), PostData, "https://twitter.com/" + user_id_toFollow, string.Empty, "XMLHttpRequest", "true", "");

                        try
                        {
                            if (!string.IsNullOrEmpty(user_id_toFollow) && !string.IsNullOrEmpty(data_user_id))
                            {
                                // string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + user_id_toFollow + "' ,'" + data_user_id + "') ";
                                string query = "INSERT INTO  tb_user_follower_details (followerName,followerId,userId) VALUES ('" + user_id_toFollow + "' ,'" + data_user_id + "','" + userID + "') ";
                                DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                            }
                        }
                        catch (Exception ex)
                        {
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID_New() -->insertingdataintodatabase--> " + ex.Message, Globals.Path_FollowerErroLog);
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FollowUsingProfileID_New() -->insertingdataintodatabase--> " + ex.Message, Globals.Path_TwtErrorLogs);
                        }
                        status = "followed";
                        if (res_PostFollow.Contains("pending"))
                        {
                            status = "pending";
                        }
                        else if(res_PostFollow.Contains("following"))
                        {
                        status = "followed";
                        }
                    }
                    else
                    {
                        status = "not followed";
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID1()(User ID is null) ", Globals.Path_FollowerErroLog);
                    }
                }
                else
                {
                    status = "Already Followed";
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID1()--> DataBase Insert error ", Globals.Path_FollowerErroLog);
                    return;
                }
            }
            catch (Exception ex)
            {
                status = "not followed";
                //Log("Method>>  FollowUsingProfileID  --- class>>  Follower.cs : Follow Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> FollowUsingProfileID() --> " + ex.Message, Globals.Path_FollowerErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> FollowUsingProfileID() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        } 
        #endregion


        #region DirectMessage
        public void SendDirectMessage(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string user_id_toFollow, string msgBodyCompose,string EmailId, out string status)
        {
            try
            {
                string data_user_id = string.Empty;
                string PostData = string.Empty;
                string Last_message_Id = string.Empty;
                if (user_id_toFollow.Contains(":"))
                {
                    data_user_id = user_id_toFollow.Split(':')[0];
                }
                else 
                {
                    data_user_id = user_id_toFollow;
                }
               // msgBodyCompose = msgBodyCompose.Replace(" ", "+").Trim();
                msgBodyCompose = msgBodyCompose.Trim();
                if (!string.IsNullOrEmpty(user_id_toFollow))
                {
                    PostData = "authenticity_token=" + postAuthenticityToken + "&lastMsgId=" + Last_message_Id + "&screen_name=" + data_user_id + "&scribeContext%5Bcomponent%5D=dm_existing_conversation_dialog&text=" + Uri.EscapeDataString(msgBodyCompose) + "&tweetboxId=";

                    //PostData = "authenticity_token=" + postAuthenticityToken + "&user_id=" + data_user_id;
                    string res_PostFollow = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/direct_messages/new"), PostData, "https://twitter.com/" + user_id_toFollow, string.Empty, "XMLHttpRequest", "true", "");

                    try
                    {
                        string direct_message_id=string.Empty;
                        try
                        {
                            direct_message_id = Utils.getBetween(res_PostFollow, "data-message-id=\\\"", "\\\""); // data-message-id=\"611051888205697024\"
                        }
                        catch{};
                        string username=string.Empty;
                        try
                        {
                            username=Utils.getBetween(res_PostFollow,"href=\\\"\\/","\\\"");
                        }
                        catch{};
                        
                        if (!string.IsNullOrEmpty(user_id_toFollow) && !string.IsNullOrEmpty(data_user_id))
                        {
                            // string query = "INSERT INTO tb_UsernameDetails (Username , Userid) VALUES ('" + user_id_toFollow + "' ,'" + data_user_id + "') ";
                            //string query = "INSERT INTO  tb_user_follower_details (followerName,followerId,userId) VALUES ('" + user_id_toFollow + "' ,'" + data_user_id + "','" + userID + "') ";
                            //DataBaseHandler.InsertQuery(query, "tb_UsernameDetails");
                        }
                        if (res_PostFollow.Contains("DirectMessage--sent"))
                        {
                            try
                            {
                                string Insertquery = "insert into tb_DirectMessageDetails(Username,FollowerId,Message) values('" + EmailId + "','" + data_user_id + "','" + msgBodyCompose + "')";
                                DataBaseHandler.InsertQuery(Insertquery, "tb_DirectMessageDetails");
                            }
                            catch { };
                        }
                    }
                    catch (Exception ex)
                    {
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> SendDirectMessage() -->insertingdataintodatabase--> " + ex.Message, Globals.Path_FollowerErroLog);
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> SendDirectMessage() -->insertingdataintodatabase--> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }

                    status = "Message send";

                }
                else
                {
                    status = "Message not send";
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> SendDirectMessage()(User ID is null) ", Globals.Path_FollowerErroLog);
                }
                
                
            }
            catch (Exception ex)
            {
                status = "Message not send";
                //Log("Method>>  FollowUsingProfileID  --- class>>  Follower.cs : Follow Exception " + ex.Message);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_FollowerErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> SendDirectMessage() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
        } 
        #endregion

        #region SetRefHttpHelpr
        private void SetRefHttpHelpr(ref Globussoft.GlobusHttpHelper thisHttpHelpr, ref Globussoft.GlobusHttpHelper ForeignhttpHelpr)
        {
            thisHttpHelpr = ForeignhttpHelpr;
        } 
        #endregion

        #region Log
        private void Log(string message)
        {
            EventsArgs eArgs = new EventsArgs(message);
            logEvents.LogText(eArgs);
        } 
        #endregion
    }
}
