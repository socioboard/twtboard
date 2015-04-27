using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;

namespace MixedCampaignManager.classes
{
    public class cls_variables
    {
        public static Dictionary<string, MixedCampaignManager.classes.CampaignAccountManager> AllLoginCampAccounts = new Dictionary<string, CampaignAccountManager>();

        public static Dictionary<string, Thread> Lst_WokingThreads = new Dictionary<string, Thread>();

        public static string _StartFrom { get; set; }

        public static string _EndTo { get; set; }

        public static int _DelayFrom = 20;

        public static int _DelayTo = 25;

        public static List<string> lstCampaignStartShedular = new List<string>();

        #region <<Settings For Follow Campaign >>
        //Save file path of selected file from user....
        public static string _followersfilepath { get; set; }

        // Settings Of checkboxes and Redio buttons ....

        //  public static bool _IsDivideData = false;
        public static int _DivideEqually = 0;

        
        public static int _DivideByGivenNo = 0;


        public static int _IsFastFollow = 0;
        public static int NoOfFollowPerAc = 0;

        public static int _IsScheduledDaily = 0;

        //No of User.....

        public static int _NoofUsers = 0;
        public static int _NoofFollowParAC = 10;

        #endregion


        #region << Settings for Tweet campaign >>

        //Save file path of selected file from user....
        public static string _TweetMSGfilepath { get; set; }

        public static string _TweetImageFolderPath { get; set; }

        public static int _IsDuplicatMSG = 0;

        public static int _IsAllTweetParAC = 0;

        public static int _IsHashTage = 0;

        public static int _IsTweetParDay = 0;

        public static int _MaxNoOfTweetsParDay = 10;

        public static int _NoOfTweetsParAC = 10;

        public static int _Istweetwithimage = 0;

        public static string _TweetUploadUserList { get; set; }
        public static int _IsTweetUploadUserList = 0;
        public static int _IsTweetMentionUserScrapedList = 0;

        public static string _TweetMentionUserName { get; set; }

        public static int _NoOfTweetMentionUserName =1;

        public static int _NoOfTweetScrapedUser = 50;

        public static int _IsTweetFollowerList = 0;
        public static int _IsTweetFollowingList = 0;
        #endregion


        #region <<Settings for Retweet campaign >>

        public static string _retweetKeyword { get; set; }

        public static int _IsUsername = 0;

        public static int _IsRetweetParDay = 0;

        public static int _NoofRetweetParDay = 10;

        public static int _NoofRetweetParAc = 10;

        #endregion


        #region <<Settings for Reply campaign >>

        public static string _replyMsgFilePath { get; set; }

        public static string _replyKeyword { get; set; }

        public static int _IsReplyUsername = 0;

        public static int _IsreplyParDay = 0;

        public static int _IsUniqueMessage = 0;

        public static int _NoofreplyParDay = 0;

        public static int _NoofreplyParAc = 10;

        public static int _replyUseDuplicateMsg = 0;

        #endregion


        public static  DataSet LoadCampaignTemp()
        {
            DataSet CompaignsDataSet = null;
            try
            {
                string query = "Select indx, CampaignName, AcFilePath, FollowingFilePath, '0' as Image,DividEql, DivideByUser, NoOfUser, FastFollow, NoOfFollowPerAc, '0' as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'0' as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath,'0' as IsDuplicate, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_follow "
             + " UNION ALL "
             + "Select indx, CampaignName, AcFilePath, TweetMsgFilePath,TweetImageFolderPath as Image,DuplicateMsg, AllTweetParAc as DivideByUser, HashTag as NoOfUser, TweetParDay, NoOfTweetParDay, NoOfTweetPerAc as TweetPac,TweetWithImage as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,'' as IsUniqueMessage, TweetUploadUserFilePath, IsUploadUserFilePath, '0' as IsDuplicate,IsTweetMentionUserScrapedList,TweetMentionUserNameScrapedList,IsTweetFollowersScrapedList,IsTweetFollowingScrapedList,NoOfTweetMentionUserScrapedList,NoOfScrapedUserScrapedList from Campaign_tweet "
             + " UNION ALL "
             + " SELECT  indx, CampaignName, AcFilePath, Keyword, '0' as Image,IsUsername,'0' as DivideByUser,'0' as NoOfUser, RetweetParDay, NoofRetweetParDay, NoofRetweetParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, '0' as IsDuplicate ,'0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_retweet "
             + " UNION ALL "
             + " SELECT  indx, CampaignName, AcFilePath, ReplyFilePath, Keyword as Image,  IsUsername,'0' as DivideByUser,'0' as NoOfUser, ReplyParDay, NoofReplyParDay, NoofReplyParAc as TweetPac, '0' as TweetWImg, ScheduledDaily, StartTime, EndTime, DelayFrom, DelayTo, Threads, Module,UniqueMessage as IsUniqueMessage,'0' as TweetUploadUserFilePath,'0' as IsUploadUserFilePath, IsDuplicateMessage, '0' as IsMentionUserScrapedList,'0' as TweetMentionUserNameScrapedList,'0' as IsTweetFollowerScrapedList,'0' as IsTweetFollowingScrapedList,'0' as NoOfTweetMentionUserScrapedList,'0' as NoOfTweetScrapedUserScrapedLIst from Campaign_reply ";

                CompaignsDataSet = RepositoryClasses.cls_DbRepository.selectQuery(query, "Union");
                
            }
            catch { }
            return CompaignsDataSet;
        }

    }

    public class AccountCreatorEntities
    {
        #region << Account creator properties >>

        public static string _AccountdeatilsFolderPath { get; set; }

        public static string ImageFolderPath { get; set; }

        public static string ProfileDetailsFolderPath { get; set; }

        public static bool _UseString = false;
        public static bool _OnlyNumbers = false;
        public static bool _UseBoth = false;
        public static bool _Location = false;
        public static bool _Description = false;
        public static bool _ProfileUrl = false;
        public static bool _ProfileImage = false;

        #endregion


    }
}
