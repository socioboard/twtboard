using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace Follower
{
    public class Unfollower
    {
        #region global declaration
        public Events logEvents = new Events();
        public static bool  IsCampaignManagerOpen = true;
        public static bool IsSettingOpen_NewUi = true;
        public static bool IsServerModule_NewUi = true;
        public static bool IsFollowYourFollowers_NewUi = true;
        public static bool IsSendDirectMessage_NewUi = true;
        public static bool IsArabFollowOpen = true;
        public static bool IsAbout_NewUi = true;
        public static bool IsSheduler_NewUi = true;
        #endregion

        #region UnFollowUsingProfileID
        public void UnFollowUsingProfileID(ref Globussoft.GlobusHttpHelper globusHttpHelper, string pgSrc, string postAuthenticityToken, string user_id_toUnFollow, out string status)
        {            
            string currentStatus = string.Empty;
            try
            {
                
                string unfollowpostdata = "authenticity_token=" + postAuthenticityToken + "&user_id=" + user_id_toUnFollow.Trim();// + user_id_toUnFollow;
                //string res_PostFollow = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/user/unfollow"), unfollowpostdata, "https://api.twitter.com/receiver.html", string.Empty, "XMLHttpRequest", "true", "https://api.twitter.com");
                string res_PostFollow = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/user/unfollow"), unfollowpostdata, "https://twitter.com/following", string.Empty, "XMLHttpRequest", "true", "");
                
                //if (res_PostFollow.EndsWith("new_state\":\"not-following\"}"))
                if (res_PostFollow.StartsWith("{\"new_state\":\"not-following")||res_PostFollow.EndsWith("new_state\":\"not-following\"}"))
                {
                    currentStatus = "Unfollowed";
                }
                else if (res_PostFollow.StartsWith("{\"new_state\":\"not-following"))
                {
                    currentStatus = "Already Unfollowed";
                }
                else
                {
                    currentStatus = "not Unfollowed";
                }
                status = currentStatus;
            }
            catch (Exception ex)
            {
                status = "not Unfollowed";
                //Log("[ " + DateTime.Now + " ] => [ Method>>  UnFollowUsingProfileID  --- class>>  Unfollower.cs : UnFollow Exception " + ex.Message + " ]");
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> Unfollower -- UnFollowUsingProfileID() --> " + ex.Message, Globals.Path_UnfollowerErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> Unfollower -- UnFollowUsingProfileID() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
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
