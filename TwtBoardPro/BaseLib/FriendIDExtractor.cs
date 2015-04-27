using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseLib
{
    public class FriendIDExtractor
    {
        public List<string> ExtractFriendIDs(ref Globussoft.GlobusHttpHelper HttpHelper, ref string userID)
        {
            string pgSrc_HomePage = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/"));
            string ProFileURL = string.Empty;

            string UserId = string.Empty;

            #region Get User or Account ID
            if (pgSrc_HomePage.Contains("http://www.facebook.com/profile.php?id="))
            {
                ///Modified Sumit [10-12-2011]
                #region

                int startIndx = pgSrc_HomePage.IndexOf("http://www.facebook.com/profile.php?id=");
                int endIndx = pgSrc_HomePage.IndexOf("\"", startIndx + 1);
                ProFileURL = pgSrc_HomePage.Substring(startIndx, endIndx - startIndx);
                if (ProFileURL.Contains("&"))
                {
                    string[] Arr = ProFileURL.Split('&');
                    ProFileURL = Arr[0];
                }

                #endregion
            }
            if (ProFileURL.Contains("http://www.facebook.com/profile.php?id="))
            {
                UserId = ProFileURL.Replace("http://www.facebook.com/profile.php?id=", "");
                if (UserId.Contains("&"))
                {
                    UserId = UserId.Remove(UserId.IndexOf("&"));
                }
                userID = UserId;
            }
            #endregion

            List<string> lstFriend = new List<string>();
            string pgSrc_FriendsPage = HttpHelper.getHtmlfromUrl(new Uri("http://www.facebook.com/profile.php?id=" + UserId + "&sk=friends&v=friends"));
            if (pgSrc_FriendsPage.Contains("http://www.facebook.com/profile.php?id="))
            {
                string[] arr = Regex.Split(pgSrc_FriendsPage, "href");
                foreach (string strhref in arr)
                {
                    if (!strhref.Contains("<!DOCTYPE"))
                    {
                        if (strhref.Contains("profile.php?id"))
                        {
                            int startIndx = strhref.IndexOf("profile.php?id") + "profile.php?id".Length + 1;
                            int endIndx = strhref.IndexOf("\"", startIndx);

                            string profileID = strhref.Substring(startIndx, endIndx - startIndx);

                            if (profileID.Contains("&"))
                            {
                                profileID = profileID.Remove(profileID.IndexOf("&"));
                            }
                            if (profileID.Contains("\\"))
                            {
                                profileID = profileID.Replace("\\", "");
                            }
                            lstFriend.Add(profileID);
                        }
                    }
                }
            }
            List<string> itemId = lstFriend.Distinct().ToList();
            return itemId;
        }

        public List<string> ExtractFriendIDs(ref BaseLib.ChilkatHttpHelpr HttpHelper, ref Chilkat.Http http, ref string userID)
        {
            try
            {
                string pgSrc_HomePage = HttpHelper.GetHtml("http://www.facebook.com/", ref http);
                string ProFileURL = string.Empty;

                string UserId = string.Empty;

                #region Get User or Account ID
                if (pgSrc_HomePage.Contains("http://www.facebook.com/profile.php?id="))
                {
                    ///Modified Sumit [10-12-2011]
                    #region

                    int startIndx = pgSrc_HomePage.IndexOf("http://www.facebook.com/profile.php?id=");
                    int endIndx = pgSrc_HomePage.IndexOf("\"", startIndx + 1);
                    ProFileURL = pgSrc_HomePage.Substring(startIndx, endIndx - startIndx);
                    if (ProFileURL.Contains("&"))
                    {
                        string[] Arr = ProFileURL.Split('&');
                        ProFileURL = Arr[0];
                    }

                    #endregion
                }
                if (ProFileURL.Contains("http://www.facebook.com/profile.php?id="))
                {
                    UserId = ProFileURL.Replace("http://www.facebook.com/profile.php?id=", "");
                    if (UserId.Contains("&"))
                    {
                        UserId = UserId.Remove(UserId.IndexOf("&"));
                    }
                    userID = UserId;
                } 
                #endregion

                List<string> lstFriend = new List<string>();
                string pgSrc_FriendsPage = HttpHelper.GetHtml("http://www.facebook.com/profile.php?id=" + UserId + "&sk=friends&v=friends", ref http);
                if (pgSrc_FriendsPage.Contains("http://www.facebook.com/profile.php?id="))
                {
                    string[] arr = Regex.Split(pgSrc_FriendsPage, "href");
                    foreach (string strhref in arr)
                    {
                        if (!strhref.Contains("<!DOCTYPE"))
                        {
                            if (strhref.Contains("profile.php?id"))
                            {
                                int startIndx = strhref.IndexOf("profile.php?id") + "profile.php?id".Length + 1;
                                int endIndx = strhref.IndexOf("\"", startIndx);

                                string profileID = strhref.Substring(startIndx, endIndx - startIndx);

                                if (profileID.Contains("&"))
                                {
                                    profileID = profileID.Remove(profileID.IndexOf("&"));
                                }
                                if (profileID.Contains("\\"))
                                {
                                    profileID = profileID.Replace("\\", "");
                                }
                                lstFriend.Add(profileID);
                            }
                        }
                    }
                }
                List<string> itemId = lstFriend.Distinct().ToList();
                return itemId;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
