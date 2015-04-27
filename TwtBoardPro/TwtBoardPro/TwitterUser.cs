using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

namespace twtboardpro
{
    public class TwitterUser
    {
        public string user_ID { get; set; }

        public string user_ScreenName { get; set; }

        public bool user_IsActive { get; set; }

        public string user_FollowingsFollowerRatio { get; set; }

        public bool user_HasManyLinks { get; set; }

        public List<string> user_ListTweets { get; set; }

        public TwitterUser()
        {
            user_ID = string.Empty;
            user_ScreenName = string.Empty;
            user_IsActive = true;
            user_FollowingsFollowerRatio = string.Empty;
            user_HasManyLinks = false;
            user_ListTweets = new List<string>();

            //try
            //{
            //    TweetAccountContainer.dictionary_TwitterUsers.Add(user_ID, this);
            //}
            //catch { }
        }

        public static string userIdForUpdate = string.Empty;
        public static string passwordForUpdate = string.Empty;
        public void UpdatePassword(string userid, string pass)
        {
            userIdForUpdate = userid.ToString();
            passwordForUpdate = pass.ToString();
        }


    }
}
