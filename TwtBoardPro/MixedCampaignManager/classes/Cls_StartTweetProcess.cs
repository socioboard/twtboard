using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using BaseLib;
using System.Threading;
using CampaignManager;
using System.IO;
using System.Collections.Specialized;
using Globussoft;
using System.Drawing;
using System.Net;
using System.Text.RegularExpressions;

namespace MixedCampaignManager.classes
{
    public class Cls_StartTweetProcess
    {

        #region global declaration
        public BaseLib.Events CampaignTweetLogEvents = new BaseLib.Events();
        public  bool _IsTweetProcessStart = true;
        Queue<string> QtweetMsg = new Queue<string>();
        Queue<string> QtweetImagePath = new Queue<string>();
        Queue<string> que_TweetMessages_Campaign_Hashtags = new Queue<string>();
        public int counterThreadsCamapignTweet = 0;
       // int SingleTweetMsgCount = 0;
        int countTweetMessageAccount = 0;
        public readonly object lockerThreadsCampaignTweet = new object();
        public static readonly object locker_que_hashtags = new object();
        int tempCountForTweetImageRename = 1;

        
        #region start and stop campaign
        public static Events CampaignStopLogevents = new Events();
        public static Events CampaignStartLogevents = new Events();
        public static Events CampaignFinishedLogevents = new Events();
        public static string campName = string.Empty;

        public void RaiseCampaignSearchLogEvents(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignStopLogevents.LogText(eArgs);
        }

        public void RaiseCampaignStartLogEvents(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignStartLogevents.LogText(eArgs);
        }

        public void RaiseCampaignFineshedEvent(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignFinishedLogevents.LogText(eArgs);
        }
        #endregion

        #endregion

        #region startTweeting
        public void startTweeting(String CampaignName, String featurName, DataRow CampRow)
        {
            //Get 1st row from arrey
            
            try
            {

                string val = string.Empty;
                try
                {
                    try
                    {
                        val = CampRow["indx"].ToString();
                    }
                    catch { }
                    if (string.IsNullOrEmpty(val))
                    {

                        DataRow[] drModelDetails = null;

                        DataSet CompaignsDataSet = cls_variables.LoadCampaignTemp();
                        drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");
                        try
                        {
                            //if (drModelDetails.Count() > 3)
                            {
                                CampRow = drModelDetails[0];
                            }
                        }
                        catch { };
                    }

                }
                catch { };
                DataRow DrCampaignDetails = CampRow;
                
                //Get all Details from Row 
                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string TweetFilePath = DrCampaignDetails.ItemArray[3].ToString();
                string TweetImageFolderPath = DrCampaignDetails.ItemArray[4].ToString();
                bool IsDuplicatMsg = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                bool IsAllTweetParAc = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                bool IsHashTag = (Convert.ToInt32(DrCampaignDetails.ItemArray[7]) == 1) ? true : false;
                bool IsTweetParDey = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                int NoOfTweetPerday = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                int NoOfTweetPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
                bool IsTweetWithImage = (Convert.ToInt32(DrCampaignDetails.ItemArray[11]) == 1) ? true : false;
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
                int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                int NoOfThreads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);
                string UserListFilePath = DrCampaignDetails.ItemArray[20].ToString();
                bool IsUserList = (Convert.ToInt32(DrCampaignDetails.ItemArray[21]) == 1) ? true : false;



                bool IsTweetMentionUserScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[23]) == 1) ? true : false;
                string TweetMentionUserNameScrapedList = DrCampaignDetails.ItemArray[24].ToString();
                bool IsTweetFollowerScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[25]) == 1) ? true : false;
                bool IsTweetFollowingScrapedList = (Convert.ToInt32(DrCampaignDetails.ItemArray[26]) == 1) ? true : false;
                int NoOfTweetMentionUserScrapedList = Convert.ToInt32(DrCampaignDetails.ItemArray[27]);
                int NoOfTweetScrapedUserScrapedLIst = Convert.ToInt32(DrCampaignDetails.ItemArray[28]);


                List<string> _lstTweetMsg = new List<string>();
                List<string> _lstTweetImages = new List<string>();
                List<string> _lstUserAccounts = new List<string>();
                List<string> _lstUserListForMsg = new List<string>();
                List<string> lst_structTweetFollowersIDs = new List<string>();
                List<string> lst_structTweetFollowingsIds = new List<string>();
                GlobusHttpHelper globusHttpHelper = new GlobusHttpHelper();
                

                classes.Cls_AccountsManager Obj_AccManger = new Cls_AccountsManager();

                //Check Files is existing...
                //if (File.Exists(TweetFilePath))
                //if (!File.Exists(TweetFilePath))
                //{
                //    Log("[ " + DateTime.Now + " ] => [ Tweet Message File Doesn't Exixst. Please change account File. ]");
                //    return;
                //}
                lock (this)
                {
                    if (IsHashTag)
                    {
                        if (Globals.HashTags.Count == 0)
                        {
                            Log("[ " + DateTime.Now + " ] => [ No Hash Tags In List. To get Hash Tags Please check Use Hash tags Checkbox and Click getHashTag button ]");
                            MessageBox.Show("No Hash Tags In List. To get Hash Tags Please check Use Hash tags Checkbox and Click getHashTag button");
                            return;
                        }
                    }

                    if (!File.Exists(AcFilePath))
                    {
                        Log("[ " + DateTime.Now + " ] => [ Account File Doesn't Exixst. Please change account File. ]");
                        return;
                    }

                    ///Get Followers and Follwing from scraping
                    ///

                    #region Follwers and following
                    if (IsTweetMentionUserScrapedList && !string.IsNullOrEmpty(TweetMentionUserNameScrapedList))
                    {
                        CampTwitterDataScrapper CamptweetScrapper = new CampTwitterDataScrapper();
                        string ReturnStatus = string.Empty;
                        CamptweetScrapper.CounterDataNo = NoOfTweetScrapedUserScrapedLIst;
                        Globals.IsMobileVersion = true;
                        if (IsTweetFollowerScrapedList)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Scraping Followers ]");
                            lst_structTweetFollowersIDs = CamptweetScrapper.GetFollowers_New_ForMobileVersion(TweetMentionUserNameScrapedList.Trim(), out ReturnStatus);
                            Log("[ " + DateTime.Now + " ] => [ Followers Scrapped  : " + lst_structTweetFollowersIDs.Count + "]");

                            if (lst_structTweetFollowersIDs.Count > 0)
                            {
                                foreach (string item in lst_structTweetFollowersIDs)
                                {
                                    string userid = item.Split(':')[1];
                                    _lstUserListForMsg.Add(userid);
                                }

                                if (lst_structTweetFollowersIDs.Count > 0)
                                {
                                    //listUserIDs.AddRange(lst_structTweetFollowersIDs);
                                    Log("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + TweetMentionUserNameScrapedList + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowersList);
                                }

                            }
                        }
                        if (IsTweetFollowingScrapedList)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Scraping Following ]");
                            lst_structTweetFollowersIDs = CamptweetScrapper.GetFollowings_NewForMobileVersion(TweetMentionUserNameScrapedList.Trim(), out ReturnStatus);
                            Log("[ " + DateTime.Now + " ] => [ Following Scrapped : " + lst_structTweetFollowersIDs.Count + "]");
                            if (lst_structTweetFollowersIDs.Count > 0)
                            {
                                foreach (string item in lst_structTweetFollowersIDs)
                                {
                                    string userid = item.Split(':')[1];
                                    _lstUserListForMsg.Add(userid);
                                }

                                if (lst_structTweetFollowersIDs.Count > 0)
                                {
                                    //listUserIDs.AddRange(lst_structTweetFollowersIDs);
                                    Log("[ " + DateTime.Now + " ] => [ Added " + lst_structTweetFollowersIDs.Count + " Followers from User: " + TweetMentionUserNameScrapedList + " ]");
                                    Log("[ " + DateTime.Now + " ] => [ Data Exported to " + Globals.Path_ScrapedFollowersList);
                                }

                            }
                        }
                        Globals.IsMobileVersion = false;
                        if (_lstUserListForMsg.Count == 0)
                        {
                            Log("[ " + DateTime.Now + " ] => [ Failed to Scrap Following or Followers ]");
                            if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                            {
                                try
                                {
                                    cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                                }
                                catch { };
                                if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                                {
                                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(CampaignName);
                                }
                                RaiseCampaignFineshedEvent(CampaignName);
                            }
                            return;
                        }

                    }
                    #endregion

                    /// Get User ID and pass from File ...
                    _lstUserAccounts = Globussoft.GlobusFileHelper.ReadLargeFile(AcFilePath);

                    _lstUserAccounts = _lstUserAccounts.Distinct().ToList();
                    countTweetMessageAccount = _lstUserAccounts.Count;

                    if (_lstUserAccounts.Count == 0)
                    {
                        MessageBox.Show("Account File is Empty.");
                        if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                        {
                            try
                            {
                                cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                            }
                            catch { };
                            
                        }
                        return;
                    }

                    ///Get Followers Name 
                    if (File.Exists(TweetFilePath))
                    {
                        _lstTweetMsg = Globussoft.GlobusFileHelper.ReadLargeFileForSpinnedMessage(TweetFilePath);

                        //_lstTweetMsg = _lstTweetMsg.Distinct().ToList();

                        if (_lstTweetMsg.Count == 0)
                        {
                            MessageBox.Show("Tweet Message File is Empty.");
                            if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                            {
                                try
                                {
                                    cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                                }
                                catch { };
                            }
                            return;
                        }
                    }

                    if (IsUserList)
                    {
                        if (File.Exists(UserListFilePath))
                        {
                            _lstUserListForMsg = Globussoft.GlobusFileHelper.ReadLargeFileForSpinnedMessage(UserListFilePath);

                        }
                    }

                    if (!IsDuplicatMsg)
                    {
                        _lstTweetMsg = _lstTweetMsg.Distinct().ToList();
                    }
                    ///Add remaining messages
                    if (IsDuplicatMsg == true)
                    {
                        #region
                        int countDifference = _lstUserAccounts.Count - _lstTweetMsg.Count;

                        if (countDifference != 0)
                        {
                            List<string> TemList = new List<string>();
                            for (int i = 0; i < countDifference; i++)
                            {
                                try
                                {
                                    TemList.Add(_lstTweetMsg[i]);
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Contains("Index was out of range"))
                                    {
                                        i = 0;
                                    }
                                }
                                if (TemList.Count == countDifference)
                                {
                                    Log("[ " + DateTime.Now + " ] => [ " + countDifference + " Duplicate Messages Uploaded. ]");
                                    _lstTweetMsg.AddRange(TemList);
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    else if ((!IsDuplicatMsg == true) && _lstUserAccounts.Count > _lstTweetMsg.Count)
                    {
                        if (_lstTweetMsg.Count > 0)
                        {
                            //Log("[ " + DateTime.Now + " ] => [ Message is less than accounts ]");
                            //DialogResult dialogResult = MessageBox.Show("Message is less than accounts. Do you want to repeat remaining Messages.", "Warning", MessageBoxButtons.YesNo);
                            //if (dialogResult == DialogResult.Yes)
                            {
                                #region
                                int countDifference = _lstUserAccounts.Count - _lstTweetMsg.Count;

                                if (countDifference != 0)
                                {
                                    List<string> TemList = new List<string>();
                                    for (int i = 0; i < countDifference; i++)
                                    {
                                        try
                                        {
                                            TemList.Add(_lstTweetMsg[i]);
                                        }
                                        catch (Exception ex)
                                        {
                                            if (ex.Message.Contains("Index was out of range"))
                                            {
                                                // i = 0;
                                            }
                                        }
                                        if (TemList.Count == countDifference)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ " + countDifference + " Duplicate Messages Uploaded. ]");
                                            _lstTweetMsg.AddRange(TemList);
                                            break;
                                        }
                                    }
                                }
                                #endregion
                            }
                            //else
                            //{
                            //}
                        }
                    }

                    //If userlist  is uploaded
                    if (IsUserList && _lstUserListForMsg.Count > 0)
                    {
                        List<string> TempMsgList = new List<string>();
                        TempMsgList.Clear();
                        bool _RepeatusernameForAllMsg = false;
                        try
                        {
                            if (_lstUserListForMsg.Count < _lstTweetMsg.Count)
                            {
                                if (false)  //for client eric
                                {
                                    DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        _RepeatusernameForAllMsg = true;
                                    }
                                    else
                                    {
                                    } 
                                }
                            }
                        }
                        catch { };


                        //
                        int UsernameCounter = 0;
                        int MsgListCounter = 0;
                        int CatchCounter = 0;
                        while (true)
                        {
                            try
                            {
                                string NewMSg = "@" + _lstUserListForMsg[UsernameCounter] + " " + _lstTweetMsg[MsgListCounter];
                                TempMsgList.Add(NewMSg);
                                UsernameCounter++;
                                MsgListCounter++;
                            }
                            catch
                            {
                                if (_RepeatusernameForAllMsg)
                                {
                                    UsernameCounter = 0;
                                }
                                if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                {
                                    break;
                                }
                                CatchCounter++;
                            };

                            if (MsgListCounter >= _lstTweetMsg.Count)
                            {
                                break;
                            }
                        }

                        // Check if Mention adding is Succesfull.
                        if (TempMsgList.Count > 0)
                        {
                            _lstTweetMsg.Clear();
                            _lstTweetMsg.AddRange(TempMsgList);
                        }
                        else
                        {
                            if (false) //for eric client
                            {
                                MessageBox.Show("mention Message creation is Failure.");
                            }
                            else
                            {
                                Log("[ " + DateTime.Now + " ] => [ mention Message creation is Failure. ]");
                            }
                        }
                    }

                    else
                    {
                        if (IsTweetMentionUserScrapedList)
                        {
                            List<string> TempMsgList = new List<string>();
                            TempMsgList.Clear();
                            bool _RepeatusernameForAllMsg = false;
                            try
                            {
                                if (_lstUserListForMsg.Count < _lstTweetMsg.Count)
                                {
                                    if (false) //for client eric
                                    {
                                        DialogResult dialogResult = MessageBox.Show("Do you want to repeat mention user name for remaining Messages.", "", MessageBoxButtons.YesNo);
                                        if (dialogResult == DialogResult.Yes)
                                        {
                                            _RepeatusernameForAllMsg = true;
                                        }
                                        else
                                        {
                                        } 
                                    }
                                }
                            }
                            catch { };


                            //
                            int UsernameCounter = 0;
                            int MsgListCounter = 0;
                            int CatchCounter = 0;
                            while (true)
                            {
                                try
                                {
                                    //string NewMSg = "@" + _lstUserListForMsg[UsernameCounter] + " " + _lstTweetMsg[MsgListCounter];
                                    //TempMsgList.Add(NewMSg);
                                    //UsernameCounter++;
                                    //MsgListCounter++;

                                    string NewMSg = string.Empty;
                                    int i = 1;
                                    //try
                                    {
                                        while (i <= NoOfTweetMentionUserScrapedList)
                                        {
                                            NewMSg += "@" + _lstUserListForMsg[UsernameCounter] + " ";
                                            UsernameCounter++;
                                            i++;
                                        }
                                        NewMSg = NewMSg + _lstTweetMsg[MsgListCounter];
                                        TempMsgList.Add(NewMSg);
                                        MsgListCounter++;
                                    }
                                    //catch
                                    //{
                                    //}
                                }
                                catch
                                {
                                    if (_RepeatusernameForAllMsg)
                                    {
                                        UsernameCounter = 0;
                                    }
                                    if (!_RepeatusernameForAllMsg && CatchCounter >= 1)
                                    {
                                        break;
                                    }
                                    CatchCounter++;
                                };

                                if (MsgListCounter >= _lstTweetMsg.Count)
                                {
                                    break;
                                }
                            }

                            // Check if Mention adding is Succesfull.
                            if (TempMsgList.Count > 0)
                            {
                                _lstTweetMsg.Clear();
                                _lstTweetMsg.AddRange(TempMsgList);
                            }
                            else
                            {
                                if (false)  //for client eric
                                {
                                    MessageBox.Show("mention Message creation is Failure.");
                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ mention Message creation is Failure. ]");
                                }
                            }
                        }
                    }

                    //Check When User Wants to post tweet message with image 
                    if (IsTweetWithImage)
                    {
                        #region << Load Images>>
                        if (!Directory.Exists(TweetImageFolderPath))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Image Folder Does't Exist. ]");
                            return;
                        }
                        ///Load Image ...
                        _lstTweetImages = GetImagePathFromFolder(TweetImageFolderPath);

                        if (_lstTweetImages.Count != 0)
                        {
                            if (_lstTweetImages.Count < _lstTweetMsg.Count)
                            {
                                ///If Images is less than messages 
                                ///its asking for repeat images.
                                //Log("[ " + DateTime.Now + " ] => [ Message is less than accounts ]");
                                Log("[ " + DateTime.Now + " ] => [ images is less than message ]");
                                if (false) //for client eric
                                {
                                    DialogResult dialogResult = MessageBox.Show("Images is less than Messages. Do you want to repeat remaining images.", "Warning", MessageBoxButtons.YesNo);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        int remaining = _lstTweetMsg.Count - _lstTweetImages.Count;

                                        for (int i = 0; i < remaining; i++)
                                        {
                                            string Imagepath = string.Empty;

                                            try
                                            {
                                                Imagepath = _lstTweetImages[i];
                                            }
                                            catch (Exception ex)
                                            {
                                                i = 0;
                                                Imagepath = _lstTweetImages[i];
                                            }
                                            _lstTweetImages.Add(Imagepath);
                                        }
                                    }
                                    else
                                    {
                                    } 
                                }
                            }
                            else
                            {
                                ///if  images is greater than messages. 
                            }
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Please Folder is Empty. Please add images in :-  " + TweetImageFolderPath + " ]");
                            return;
                        }

                        ///print number of Images ...
                        Log("[ " + DateTime.Now + " ] => [ " + _lstTweetImages.Count() + " Images uploaded. ]");

                        #endregion
                    }
                    
                }

                //Queue<string> QtweetMsg = new Queue<string>();
                //Upload account in account Container..
                CampaignTweetAccountContainer objCampaignTweetAccountContainer = Obj_AccManger.AccountManager(_lstUserAccounts);

                Log("[ " + DateTime.Now + " ] => [ " + objCampaignTweetAccountContainer.dictionary_CampaignAccounts.Count + " Accounts Uploaded. ]");

                Log("[ " + DateTime.Now + " ] => [ " + _lstTweetMsg.Count + " Tweet Messages Uploaded. ]");

                //IF user want to add hash tage from start of all MSG's ..
                if (IsHashTag)
                {
                    //List<string> temp_lstTweetMsg = new List<string>();
                    //_lstTweetMsg.ForEach(TMsg => temp_lstTweetMsg.Add("# " + TMsg));

                    //if (temp_lstTweetMsg.Count != 0)
                    //{
                    //    _lstTweetMsg.Clear();
                    //    _lstTweetMsg.AddRange(temp_lstTweetMsg);
                    //}
                    IsHashTag = true;
                    foreach (string Data in Globals.HashTags)
                    {
                       que_TweetMessages_Campaign_Hashtags.Enqueue(Data);
                    }
                    
                }
                else
                {
                   IsHashTag = false;
                }

                
                if (!IsAllTweetParAc && QtweetMsg.Count == 0)
                {
                    _lstTweetMsg.ForEach(abc => { QtweetMsg.Enqueue(abc); });
                }

                if (IsTweetWithImage && _lstTweetImages.Count != 0)
                {
                    _lstTweetImages.ForEach(img => { QtweetImagePath.Enqueue(img); });
                }


                ThreadPool.SetMaxThreads(NoOfThreads, NoOfThreads);

                foreach (var Account in objCampaignTweetAccountContainer.dictionary_CampaignAccounts)

                    try
                    {
                        {

                            //ThreadPool.QueueUserWorkItem(new WaitCallback(StartCampaignTweeting), new object[] { Account, _lstTweetMsg, IsAllTweetParAc, DelayStar, DelayEnd, NoOfTweetPerAc, CampaignName, IsSchedulDaily, SchedulerEndTime });

                            if (counterThreadsCamapignTweet >= NoOfThreads)
                            {
                                lock (lockerThreadsCampaignTweet)
                                {
                                    Monitor.Wait(lockerThreadsCampaignTweet);
                                }
                            }

                            ((CampaignAccountManager)Account.Value).logEvents.addToLogger += new EventHandler(logEvents_addToLogger);



                            if (IsTweetWithImage)
                            {
                                ///Tweet message With Image 
                                Thread threadGetStartProcessForTweet = new Thread(StartTweetingWithImages);
                                threadGetStartProcessForTweet.Name = CampaignName + "_" + Account.Key;
                                threadGetStartProcessForTweet.IsBackground = true;
                                threadGetStartProcessForTweet.Start(new object[] { Account, _lstTweetMsg, _lstTweetImages, IsTweetWithImage, IsAllTweetParAc, IsHashTag, DelayStar, DelayEnd, NoOfTweetPerAc, CampaignName, IsSchedulDaily, SchedulerEndTime, TweetFilePath, IsUserList, UserListFilePath });
                            }
                            else
                            {
                                /// Tweet Only Messages 
                                Thread threadGetStartProcessForTweet = new Thread(StartCampaignTweeting);
                                threadGetStartProcessForTweet.Name = CampaignName + "_" + Account.Key;
                                threadGetStartProcessForTweet.IsBackground = true;
                                threadGetStartProcessForTweet.Start(new object[] { Account, _lstTweetMsg, IsAllTweetParAc, DelayStar, DelayEnd, NoOfTweetPerAc, CampaignName, IsSchedulDaily, SchedulerEndTime, IsHashTag, TweetFilePath, IsUserList, UserListFilePath });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    finally
                    {
                        countTweetMessageAccount--;
                        if (countTweetMessageAccount == 0)
                        {
                            
                           // Log("[ " + DateTime.Now + " ] => [ Process complete ]");
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            
        } 
        #endregion

        #region StartCampaignTweeting Commented
        //private void StartCampaignTweeting(object parameters)
        //{
        //    try
        //    {
        //        Interlocked.Increment(ref counterThreadsCamapignTweet);

        //        Tweeter.Tweeter Objtweet;

        //        //get all parems from object 

        //        Array paramsArray = new object[2];

        //        paramsArray = (Array)parameters;

        //        //get value from param
        //        KeyValuePair<string, CampaignAccountManager> keyValue = (KeyValuePair<string, CampaignAccountManager>)paramsArray.GetValue(0);

        //        //get Message list prom param 
        //        List<string> _lstTweetMsg = (List<string>)paramsArray.GetValue(1);

        //        //get bool value for tweet per account 
        //        bool IsAllTweetParAc = (bool)paramsArray.GetValue(2);

        //        //get delay start and end timming from param 
        //        int DelayStar = (int)paramsArray.GetValue(3);

        //        int DelayEnd = (int)paramsArray.GetValue(4);

        //        //get Number of tweets par account .
        //        int NoOfTweetPerAc = (int)paramsArray.GetValue(5);


        //        string CampaignName = (string)paramsArray.GetValue(6);


        //        //Get scheduler time and status if task is scheduled 
        //        bool IsSchedulDaily = (bool)paramsArray.GetValue(7);
        //        DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(8);

        //        //set get account details from kay value pair 
        //        //and create event to print log messages from account related.. 
        //        string accKey = keyValue.Key;

        //        CampaignAccountManager tweetAccountManager = keyValue.Value;




        //        //Add List Of Working thread
        //        //we are using this list when we stop/abort running camp processes..
        //        try
        //        {
        //            MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + tweetAccountManager.Username, Thread.CurrentThread);
        //        }
        //        catch (Exception)
        //        {
        //        }


        //        //attempt login from account 
        //        if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
        //        {
        //            //get account logging 
        //            tweetAccountManager.Login();
        //            try
        //            {
        //                CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, tweetAccountManager);
        //            }
        //            catch { };
        //        }
        //        else
        //        {
        //            try
        //            {
        //                tweetAccountManager = null;
        //                bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out tweetAccountManager);
        //                //if (!tweetAccountManager.IsLoggedIn && !tweetAccountManager.IsNotSuspended)
        //                //{

        //                //}
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        }

        //        //check account is logged in 
        //        if (tweetAccountManager.IsLoggedIn)
        //        {
        //            //check account is suspended or not 
        //            if (tweetAccountManager.IsNotSuspended)
        //            {
        //                //check condition if user wants to all tweet per account 
        //                if (IsAllTweetParAc)
        //                {
        //                    #region All Tweet Par Account
        //                    Objtweet = new Tweeter.Tweeter();

        //                    int MSGcounter = 0;

        //                    for (int i = 0; i < _lstTweetMsg.Count; i++)
        //                    {
        //                        try
        //                        {
        //                            //Check Scheduled Task end time 
        //                            //If task is scheduled 
        //                            if (IsSchedulDaily)
        //                            {
        //                                if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
        //                                {
        //                                    _IsTweetProcessStart = false;
        //                                    new Thread(() =>
        //                                        {
        //                                            frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
        //                                            frmcamp.StoprunningCampaign(CampaignName);
        //                                        }).Start();
        //                                    break;
        //                                }
        //                            }


        //                            //Process is execuite
        //                            string tweetMsg = _lstTweetMsg[i];

        //                            string status = string.Empty;

        //                            Objtweet.Tweet(ref tweetAccountManager.globusHttpHelper, "", tweetAccountManager.postAuthenticityToken, tweetMsg, out status);

        //                            if (status == "posted")
        //                            {
        //                                //If susceesfully posted 

        //                                Log("Message :- " + tweetMsg + " is posted from " + keyValue.Key);

        //                                try
        //                                {
        //                                    //Insert details in Report table 
        //                                    RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
        //                                }
        //                                catch { };

        //                            }
        //                            else
        //                            {
        //                                //not successFully Posted 

        //                                Log("Message is not posted from " + accKey);
        //                            }

        //                            //Delay after every Tweet..
        //                            int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar * 1000, DelayEnd * 1000);
        //                            Log("Delay :- " + delay + " Milliseconds");
        //                            Thread.Sleep(delay);



        //                        }
        //                        catch (Exception)
        //                        {
        //                        }
        //                        finally
        //                        {

        //                        }
        //                    }
        //                    #endregion
        //                }
        //                else
        //                {
        //                    #region Single Tweet par account

        //                    try
        //                    {
        //                        Objtweet = new Tweeter.Tweeter();

        //                        if (QtweetMsg.Count == 0)
        //                        {
        //                            Log("Message is finished from message list.");
        //                            return;
        //                        }


        //                        //Check Scheduler end time 
        //                        //If task is scheduled 
        //                        if (IsSchedulDaily)
        //                        {
        //                            if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
        //                            {
        //                                _IsTweetProcessStart = false;
        //                                new Thread(() =>
        //                                {
        //                                    frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
        //                                    frmcamp.StoprunningCampaign(CampaignName);
        //                                }).Start();

        //                                return;
        //                            }
        //                        }


        //                        //if (SingleTweetMsgCount >= _lstTweetMsg.Count)
        //                        //{
        //                        //    AddToCampaignLoggerListBox("Message is finished from message list.");
        //                        //    return;
        //                        //}


        //                        string tweetMsg = QtweetMsg.Dequeue();

        //                        string status = string.Empty;

        //                        Objtweet.Tweet(ref tweetAccountManager.globusHttpHelper, "", tweetAccountManager.postAuthenticityToken, tweetMsg, out status);

        //                        if (status == "posted")
        //                        {
        //                            try
        //                            {
        //                                //Insert details in Report table 
        //                                RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
        //                            }
        //                            catch { };

        //                            //If susceesfully posted 

        //                            Log("Message :- " + tweetMsg + " is posted from " + keyValue.Key);

        //                            //control threads according to number of message par account 

        //                            if (NoOfTweetPerAc != 0)
        //                            {
        //                                if (NoOfTweetPerAc == SingleTweetMsgCount)
        //                                {
        //                                    Log("Number of tweet per account Limit is Finshed From :- " + accKey);
        //                                    return;
        //                                }
        //                                SingleTweetMsgCount++;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            //not successFully Posted 
        //                            Log("Message :- " + tweetMsg + " is not posted from " + keyValue.Key);
        //                            QtweetMsg.Enqueue(tweetMsg);
        //                            SingleTweetMsgCount--;
        //                        }

        //                        //Delay after every Tweet..
        //                        int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar * 1000, DelayEnd * 1000);
        //                        Log("Delay :- " + delay + " Milliseconds");
        //                        Thread.Sleep(delay);
        //                    }
        //                    catch (Exception)
        //                    {
        //                    }
        //                    finally
        //                    {

        //                    }

        //                    #endregion
        //                }
        //            }
        //            else
        //            {
        //                ////If account is suspended ...
        //                ////its returns without performing any operation 
        //                //AddToCampaignLoggerListBox("User is suspended  " + keyValue.Key);
        //                //return;
        //            }
        //        }
        //        else
        //        {
        //            ////If not logged in account 
        //            //AddToCampaignLoggerListBox("Login failed from " + keyValue.Key);
        //            //return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        Interlocked.Decrement(ref counterThreadsCamapignTweet);

        //        lock (lockerThreadsCampaignTweet)
        //        {
        //            Monitor.Pulse(lockerThreadsCampaignTweet);
        //        }
        //    }
        //} 
        #endregion

        #region StartCampaignTweeting
        List<string> TemplstTweetMsg = new List<string>();
        private void StartCampaignTweeting(object parameters)
        {
            int SingleTweetMsgCount = 0;

            string CampaignName = string.Empty;
            //CampaignAccountManager tweetAccountManager = null;
            try
            {
                Interlocked.Increment(ref counterThreadsCamapignTweet);
                Tweeter.Tweeter Objtweet;

                //get all parems from object 
                Array paramsArray = new object[2];
                paramsArray = (Array)parameters;

                //get value from param
                KeyValuePair<string, CampaignAccountManager> keyValue = (KeyValuePair<string, CampaignAccountManager>)paramsArray.GetValue(0);

                //get Message list prom param 
                List<string> _lstTweetMsg = (List<string>)paramsArray.GetValue(1);

                //get bool value for tweet per account 
                bool IsAllTweetParAc = (bool)paramsArray.GetValue(2);

                //get delay start and end timming from param 
                int DelayStar = (int)paramsArray.GetValue(3);
                int DelayEnd = (int)paramsArray.GetValue(4);

                //get Number of tweets par account .
                int NoOfTweetPerAc = (int)paramsArray.GetValue(5);

                CampaignName = (string)paramsArray.GetValue(6);


                //Get scheduler time and status if task is scheduled 
                bool IsSchedulDaily = (bool)paramsArray.GetValue(7);
                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(8);
                bool IsHashTagsCampaign = (bool)paramsArray.GetValue(9);

                string TweetFilePath = (string)paramsArray.GetValue(10);
                bool IsUserList = (bool)paramsArray.GetValue(11);
                string UserListFilePath = (string)paramsArray.GetValue(12);
                //set get account details from kay value pair 
                //and create event to print log messages from account related.. 
                string accKey = keyValue.Key;
                string Hashtags = string.Empty;
                CampaignAccountManager tweetAccountManager = keyValue.Value;


                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + tweetAccountManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }


                //attempt login from account 
                if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
                {
                    //get account logging 
                    tweetAccountManager.Login();
                    try
                    {
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, tweetAccountManager);
                    }
                    catch { };
                }
                else
                {
                    try
                    {
                        tweetAccountManager = null;
                        bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out tweetAccountManager);
                        //if (!tweetAccountManager.IsLoggedIn && !tweetAccountManager.IsNotSuspended)
                        //{

                        //}
                    }
                    catch (Exception)
                    {
                    }
                }

                //check account is logged in 
                if (tweetAccountManager.IsLoggedIn)
                {
                    //check account is suspended or not 

                    //Comment by Prabhat 06.11.13
                    if (tweetAccountManager.IsNotSuspended)
                    {
                        //check condition if user wants to all tweet per account 
                        if (IsAllTweetParAc)
                        {
                            #region All Tweet Par Account
                            Objtweet = new Tweeter.Tweeter();

                            int MSGcounter = 0;

                          

                         //   TemplstTweetMsg = _lstTweetMsg;
                            for (int i = 0; i < _lstTweetMsg.Count; i++)
                            {
                                try
                                {
                                    //Check Scheduled Task end time 
                                    //If task is scheduled 
                                    if (IsSchedulDaily)
                                    {
                                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                        {
                                            _IsTweetProcessStart = true;
                                            new Thread(() =>
                                            {
                                                //frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                                //frmcamp.StoprunningCampaign(CampaignName);
                                                //frmcamp.startTweet(CampaignName, "");


                                                RaiseCampaignSearchLogEvents(campName);
                                                RaiseCampaignStartLogEvents(campName);

                                            }).Start();
                                            break;
                                        }
                                    }


                                    //Process is execuite
                                   string tweetMsg = _lstTweetMsg[i];

                                    //string tweetMsg = string.Empty;

                                    //try
                                    //{
                                    //    tweetMsg = TemplstTweetMsg[i];
                                    //    TemplstTweetMsg.Remove(tweetMsg);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //  tweetMsg = _lstTweetMsg[i];
                                    //}

                                    if (IsHashTagsCampaign)
                                    {
                                        lock (locker_que_hashtags)
                                        {

                                            if (que_TweetMessages_Campaign_Hashtags.Count > 0)
                                            {
                                                try
                                                {
                                                    Hashtags = que_TweetMessages_Campaign_Hashtags.Dequeue();
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                                }
                                            }
                                            else
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                                //break;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Hashtags))
                                    {
                                        tweetMsg = Hashtags + " " + tweetMsg;
                                    }

                                    string status = string.Empty;

                                    Objtweet.Tweet(ref tweetAccountManager.globusHttpHelper, tweetAccountManager.postAuthenticityToken, tweetMsg, out status);

                                    if (status == "posted")
                                    {
                                        //If susceesfully posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " is posted from " + keyValue.Key + " ]");

                                        try
                                        {
                                            //Insert details in Report table 
                                            RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
                                        }

                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() -- Tweet -- > " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() --  Tweet   --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }
                                        try
                                        {
                                            RemoveFollwerFromTxtFile(TweetFilePath, tweetMsg);
                                            if (IsUserList)
                                            {
                                                RemoveMentionUser(UserListFilePath,tweetMsg,TweetFilePath);
                                            }
                                        }
                                        catch { }


                                    }
                                    else
                                    {
                                        //not successFully Posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message is not posted from " + accKey + " ]");
                                    }

                                    //Delay after every Tweet..
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " Seconds ]");
                                    Thread.Sleep(delay * 1000);

                                }
                                catch (Exception ex)
                                {
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() -- Tweet -- IsAllTweetParAc --> " + ex.Message, Globals.Path_TweetingErroLog);
                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() --  Tweet -- IsAllTweetParAc  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                }
                                finally
                                {

                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Single Tweet par account

                            try
                            {
                                Objtweet = new Tweeter.Tweeter();

                                while (true)
                                {
                                    if (QtweetMsg.Count == 0)
                                    {
                                        Log("[ " + DateTime.Now + " ] => [ Message is finished from message list. ]");
                                        Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                        Log("-----------------------------------------------------------------------------------------------------------------------");
                                        break;
                                    }

                                    //Check Scheduler end time 
                                    //If task is scheduled 
                                    if (IsSchedulDaily)
                                    {
                                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                        {
                                            _IsTweetProcessStart = true;
                                            new Thread(() =>
                                            {
                                                //frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                                //frmcamp.StoprunningCampaign(CampaignName);
                                                //frmcamp.startTweet(CampaignName, "");

                                                RaiseCampaignSearchLogEvents(campName);
                                                RaiseCampaignStartLogEvents(campName);
                                            }).Start();

                                            return;
                                        }
                                    }

                                    //if (SingleTweetMsgCount >= _lstTweetMsg.Count)
                                    //{
                                    //    AddToCampaignLoggerListBox("Message is finished from message list.");
                                    //    return;
                                    //}


                                    string tweetMsg = QtweetMsg.Dequeue();

                                    if (IsHashTagsCampaign)
                                    {
                                        lock (locker_que_hashtags)
                                        {

                                            if (que_TweetMessages_Campaign_Hashtags.Count > 0)
                                            {
                                                try
                                                {
                                                    Hashtags = que_TweetMessages_Campaign_Hashtags.Dequeue();
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                                }
                                            }
                                            else
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                                //break;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Hashtags))
                                    {
                                        tweetMsg = Hashtags + " " + tweetMsg;
                                    }
                                    string status = string.Empty;

                                    Objtweet.Tweet(ref tweetAccountManager.globusHttpHelper, tweetAccountManager.postAuthenticityToken, tweetMsg, out status);

                                    if (status == "posted")
                                    {
                                        try
                                        {
                                            //Insert details in Report table 
                                            RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() -- Tweet  --> " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() --  Tweet   --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }

                                        //If susceesfully posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " is posted from " + keyValue.Key + " ]");

                                        try
                                        {
                                            RemoveFollwerFromTxtFile(TweetFilePath, tweetMsg);
                                        }
                                        catch { }

                                        if (IsUserList)
                                        {
                                            RemoveMentionUser(UserListFilePath, tweetMsg, TweetFilePath);
                                        }

                                        //control threads according to number of message par account 

                                        if (NoOfTweetPerAc != 0)
                                        {
                                            if (NoOfTweetPerAc == SingleTweetMsgCount)
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ Finished number of tweet as per account Limit From :- " + accKey + " ]");
                                                Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                                Log("-----------------------------------------------------------------------------------------------------------------------");
                                                return;
                                            }

                                            SingleTweetMsgCount++;
                                        }
                                    }
                                    else
                                    {
                                        //not successFully Posted 
                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " is not posted from " + keyValue.Key + " ]");
                                        QtweetMsg.Enqueue(tweetMsg);

                                        ///decrease counter when posting is failed 
                                        if (SingleTweetMsgCount > 0)
                                            SingleTweetMsgCount--;
                                    }

                                    //Delay after every Tweet..
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " Seconds ]");
                                    Thread.Sleep(delay*1000);
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() -- Tweet -- IsAllTweetParAcNotTrue --> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweeting() --  Tweet -- IsAllTweetParAcNotTrue  --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }
                            finally
                            {

                            }

                            #endregion
                        }
                    }
                    else
                    {
                        //If account is suspended ...
                        //its returns without performing any operation 
                        //AddToCampaignLoggerListBox("User is suspended  " + keyValue.Key);
                        //return;
                    }
                }
                else
                {
                    ////If not logged in account 
                    //AddToCampaignLoggerListBox("Login failed from " + keyValue.Key);
                    //return;
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweetingEndBlock -- Tweet   --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartCampaignTweetingEndBlock --  Tweet   --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            finally
            {
               
                Interlocked.Decrement(ref counterThreadsCamapignTweet);

                lock (lockerThreadsCampaignTweet)
                {
                    Monitor.Pulse(lockerThreadsCampaignTweet);
                }
               
                //counterThreadsCamapignTweet--;
                if (counterThreadsCamapignTweet == 0)
                {
                    if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                    {
                        MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(CampaignName);
                    }
                    if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                    {
                        try
                        {
                            cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                            
                        }
                        catch { };
                    }
                    _IsTweetProcessStart = true;

                    

                    RaiseCampaignFineshedEvent(CampaignName);
                    Log("[ " + DateTime.Now + " ] => [ Process completed for campaign " + CampaignName + " ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(CampaignName + ": " + DateTime.Now, Globals.path_LogCampaignCompleted);
                }
            }
        } 
        #endregion

        #region StartTweetingWithImages
        private void StartTweetingWithImages(object parameters)
        {
            int SingleTweetMsgCount = 0;
            string CampaignName =string.Empty;
            //CampaignAccountManager tweetAccountManager = null;
            try
            {
                Interlocked.Increment(ref counterThreadsCamapignTweet);

                Tweeter.Tweeter Objtweet;

                //get all parems from object 

                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                //get value from param
                KeyValuePair<string, CampaignAccountManager> keyValue = (KeyValuePair<string, CampaignAccountManager>)paramsArray.GetValue(0);

                //get Message list prom param 
                List<string> _lstTweetMsg = (List<string>)paramsArray.GetValue(1);

                //Get Image Path List ..
                List<string> _lstTweetImages = (List<string>)paramsArray.GetValue(2);

                //Get Image Path List ..
                bool IsTweetWithImage = (bool)paramsArray.GetValue(3);


                //get bool value for tweet per account 
                bool IsAllTweetParAc = (bool)paramsArray.GetValue(4);

                bool IsHashTagsCampaign=(bool)paramsArray.GetValue(5);

                //get delay start and end timming from param 
                int DelayStar = (int)paramsArray.GetValue(6);

                int DelayEnd = (int)paramsArray.GetValue(7);

                //get Number of tweets par account .
                int NoOfTweetPerAc = (int)paramsArray.GetValue(8);

                CampaignName = (string)paramsArray.GetValue(9);


                //Get scheduler time and status if task is scheduled 
                bool IsSchedulDaily = (bool)paramsArray.GetValue(10);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(11);

                string TweetFilePath = (string)paramsArray.GetValue(12);

                bool IsUserList = (bool)paramsArray.GetValue(13);
                string UserListFilePath = (string)paramsArray.GetValue(14);
                //set get account details from kay value pair 
                //and create event to print log messages from account related.. 
                string accKey = keyValue.Key;
                string Hashtags = string.Empty;

                CampaignAccountManager tweetAccountManager = keyValue.Value;
                

                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + tweetAccountManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }


                //attempt login from account 
                if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
                {
                    //get account logging 
                    tweetAccountManager.Login();
                    try
                    {
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, tweetAccountManager);
                    }
                    catch { };
                }
                else
                {
                    try
                    {
                        tweetAccountManager = null;
                        bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out tweetAccountManager);
                    }
                    catch (Exception)
                    {
                    }
                }

                //check account is logged in 
                if (tweetAccountManager.IsLoggedIn)
                {
                    //check account is suspended or not 
                    if (tweetAccountManager.IsNotSuspended)
                    {
                        //check condition if user wants to all tweet per account 
                        if (IsAllTweetParAc)
                        {
                            #region All Tweet Par Account
                            Objtweet = new Tweeter.Tweeter();

                            int MSGcounter = 0;
                            int CounterImage = 0;
                            //for (int i = 0; i < _lstTweetMsg.Count; i++)
                           // while (_lstTweetMsg.Count > 0 && _lstTweetImages.Count > 0)
                            while (_lstTweetMsg.Count > 0 || _lstTweetImages.Count > 0)
                            {
                                try
                                {
                                    //Check Scheduled Task end time 
                                    //If task is scheduled 
                                    if (IsSchedulDaily)
                                    {
                                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                        {
                                            //_IsTweetProcessStart = false;
                                            _IsTweetProcessStart = true;
                                            new Thread(() =>
                                            {
                                                RaiseCampaignSearchLogEvents(campName);
                                                RaiseCampaignStartLogEvents(campName);
                                            }).Start();
                                            break;
                                        }
                                    }


                                    //Process is execuite
                                    string tweetMsg = string.Empty;
                                    if (_lstTweetMsg.Count >0)
                                    {
                                        tweetMsg = _lstTweetMsg[MSGcounter];
                                    }
                                    if (IsHashTagsCampaign)
                                    {
                                        lock (locker_que_hashtags)
                                        {

                                            if (que_TweetMessages_Campaign_Hashtags.Count > 0)
                                            {
                                                try
                                                {
                                                    Hashtags = que_TweetMessages_Campaign_Hashtags.Dequeue();
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingWithImages() -- IsHashTagsCampaign -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingWithImages() -- IsHashTagsCampaign -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                                }
                                            }
                                            else
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                                //break;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Hashtags))
                                    {
                                        tweetMsg = Hashtags + " " + tweetMsg;
                                    }
                                    string ImagePath = _lstTweetImages[CounterImage];

                                    string status = string.Empty;

                                    try
                                    {
                                        TweetMessageWithImage(tweetAccountManager.postAuthenticityToken, tweetMsg, ImagePath, ref tweetAccountManager.globusHttpHelper, out status);
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- TweetWithImage -- TweetMessageWithImage --> " + ex.Message, Globals.Path_TweetingErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  TweetWithImage -- TweetMessageWithImage  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }

                                    if (status == "posted")
                                    {
                                        //If susceesfully posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " and Image :- " + ImagePath + " is posted from " + keyValue.Key + " ]");

                                        try
                                        {
                                            //Insert details in Report table 
                                            RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
                                        }
                                        catch { };

                                        try
                                        {
                                            RemoveFollwerFromTxtFile(TweetFilePath, tweetMsg);
                                        }
                                        catch { }
                                        if (IsUserList)
                                        {
                                            RemoveMentionUser(UserListFilePath, tweetMsg, TweetFilePath);
                                        }
                                        //if (File.Exists(ImagePath))
                                        //{
                                        //        try
                                        //        {
                                        //            string ImageFileData = Globals.path_DesktopUsedTweetedImage;

                                        //            WebClient objWebClient = new WebClient();

                                        //            string ImageFileData1 = Globals.path_DesktopUsedTweetedImage + "\\" + tempCountForTweetImageRename + ".jpg";
                                        //            objWebClient.DownloadFile(ImagePath, ImageFileData1);
                                        //            tempCountForTweetImageRename++;
                                        //            countTweetMessageAccount++;
                                        //        }
                                        //        catch { }
                                           
                                        //}
                                    }
                                    else
                                    {
                                        //not successFully Posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message is not posted from " + accKey + " ]");
                                    }

                                    //Delay after every Tweet..
                                    
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " seconds ]");
                                    Thread.Sleep(delay * 1000);
                                }
                                catch (Exception)
                                {
                                }
                                finally
                                {
                                    MSGcounter++;
                                    CounterImage++;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region Single Tweet par account

                            try
                            {
                                Objtweet = new Tweeter.Tweeter();

                                while (true)
                                {
                                    if ((_lstTweetImages.Count == 0))
                                    {
                                        if (QtweetMsg.Count == 0)
                                        {
                                            Log("[ " + DateTime.Now + " ] => [ Message is finished from message list. ]");
                                            Log("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED ]");
                                            Log("-----------------------------------------------------------------------------------------------------------------------");
                                            return;
                                        }
                                    }


                                    //Check Scheduler end time 
                                    //If task is scheduled 
                                    if (IsSchedulDaily)
                                    {
                                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                        {
                                           // _IsTweetProcessStart = false;
                                            _IsTweetProcessStart = true;
                                            new Thread(() =>
                                            {
                                                RaiseCampaignSearchLogEvents(campName);
                                                RaiseCampaignStartLogEvents(campName);
                                            }).Start();

                                            return;
                                        }
                                    }


                                    //if (SingleTweetMsgCount >= _lstTweetMsg.Count)
                                    //{
                                    //    AddToCampaignLoggerListBox("Message is finished from message list.");
                                    //    return;
                                    //}
                                    string tweetMsg = string.Empty;
                                    if (_lstTweetMsg.Count > 0)
                                    {
                                         tweetMsg = QtweetMsg.Dequeue();
                                    }
                                    if (IsHashTagsCampaign)
                                    {
                                        lock (locker_que_hashtags)
                                        {

                                            if (que_TweetMessages_Campaign_Hashtags.Count > 0)
                                            {
                                                try
                                                {
                                                    Hashtags = que_TweetMessages_Campaign_Hashtags.Dequeue();
                                                }
                                                catch (Exception ex)
                                                {
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> StartTweetingMultithreaded() -- TweetWithImage -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                                    GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetAccountManager --  TweetWithImage -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                                }
                                            }
                                            else
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ All Loaded Hash Tags Are Used ]");
                                                //break;
                                            }
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(Hashtags))
                                    {
                                        tweetMsg = Hashtags + " " + tweetMsg;
                                    }

                                    string ImagePath = QtweetImagePath.Dequeue();

                                    string status = string.Empty;

                                    try
                                    {
                                        TweetMessageWithImage(tweetAccountManager.postAuthenticityToken, tweetMsg, ImagePath, ref tweetAccountManager.globusHttpHelper, out status);
                                    }
                                    catch (Exception ex)
                                    {
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImage() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImage() --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                    }

                                    if (status == "posted")
                                    {
                                        try
                                        {
                                            //Insert details in Report table 
                                            RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, "", tweetMsg, "", "");
                                        }
                                        catch (Exception ex)
                                        {
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->  Single Tweet par account -- TweetWithImage -- InsertingDataindatabase --> " + ex.Message, Globals.Path_TweetingErroLog);
                                            GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->  Single Tweet par account --  TweetWithImage -- InsertingDataindatabase  --> " + ex.Message, Globals.Path_TweetAccountManager);
                                        }

                                        //If susceesfully posted 

                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " and Image :- " + ImagePath + " is posted from " + keyValue.Key + " ]");

                                        try
                                        {
                                            RemoveFollwerFromTxtFile(TweetFilePath, tweetMsg);
                                        }
                                        catch { }

                                        if (IsUserList)
                                        {
                                            RemoveMentionUser(UserListFilePath, tweetMsg, TweetFilePath);
                                        }
                                        if (File.Exists(ImagePath))
                                        {
                                            
                                            #region Save image at desktop
                                            try
                                            {
                                                string ImageFileData = Globals.path_DesktopUsedTweetedImage;
                                               
                                                System.IO.FileInfo info = new System.IO.FileInfo(ImagePath);
                                                string imageName = info.Name;

                                                WebClient objWebClient = new WebClient();

                                                string ImageFileData1 = Globals.path_DesktopUsedTweetedImage + "\\" + imageName + ".jpg";
                                                objWebClient.DownloadFile(ImagePath, ImageFileData1);
                                                countTweetMessageAccount++;
                                                tempCountForTweetImageRename++;
                                            }
                                            catch { }
                                            try
                                            {
                                                File.Delete(ImagePath);
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.Write("Error : " + ex.StackTrace);
                                            }
                                            #endregion

                                        }
                                        //control threads according to number of message par account 

                                        if (NoOfTweetPerAc != 0)
                                        {
                                            if (NoOfTweetPerAc == SingleTweetMsgCount)
                                            {
                                                Log("[ " + DateTime.Now + " ] => [ Number of tweet per account Limit is Finshed From :- " + accKey + " ]");
                                                return;
                                            }
                                            SingleTweetMsgCount++;
                                        }
                                    }
                                    else
                                    {
                                        //not successFully Posted 
                                        Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " and image:= " + ImagePath + " is not posted from " + keyValue.Key + " ]");
                                        //Log("[ " + DateTime.Now + " ] => [ Message :- " + tweetMsg + " is not posted from " + keyValue.Key + " ]");
                                        QtweetMsg.Enqueue(tweetMsg);
                                        QtweetImagePath.Enqueue(ImagePath);

                                        if (SingleTweetMsgCount > 0)
                                            SingleTweetMsgCount--;
                                    }

                                    //Delay after every Tweet..
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " seconds ]");
                                    Thread.Sleep(delay*1000);
                                }
                            }
                            catch (Exception ex)
                            {
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->  Single Tweet par account -- TweetWithImage -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->  Single Tweet par account --  TweetWithImage -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
                            }
                            finally
                            {

                            }

                            #endregion
                        }
                    }
                    else
                    {
                        ////If account is suspended ...
                        ////its returns without performing any operation 
                        //AddToCampaignLoggerListBox("User is suspended  " + keyValue.Key);
                        //return;
                    }
                }
                else
                {
                    ////If not logged in account 
                    //AddToCampaignLoggerListBox("Login failed from " + keyValue.Key);
                    //return;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                
                Interlocked.Decrement(ref counterThreadsCamapignTweet);

                lock (lockerThreadsCampaignTweet)
                {
                    Monitor.Pulse(lockerThreadsCampaignTweet);
                }
                //Log("[ " + DateTime.Now + " ] => [ process is completed for User " + tweetAccountManager + " ]");
                        countTweetMessageAccount--;
                        
                        if (counterThreadsCamapignTweet == 0)
                        {
                            if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                            {
                                MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(CampaignName);
                            }
                            if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                            {
                                try
                                {
                                    cls_variables.lstCampaignStartShedular.Remove(CampaignName);

                                }
                                catch { };
                            }
                            _IsTweetProcessStart = true;
                            RaiseCampaignFineshedEvent(CampaignName);
                           
                            Log("[ " + DateTime.Now + " ] => [ Process completed for campaign "+ CampaignName+" ]");
                            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(CampaignName + ": " + DateTime.Now, Globals.path_LogCampaignCompleted);
                        }
                    
            }
        } 
        #endregion
        public void RemoveMentionUser(string tempDeleteUserFilePath, string TargetedUser_item,string tempTweetFilePath)
        {
            lock (lockerThreadsCampaignTweet)
            {
                try
                {
                    List<string> quotelist = File.ReadAllLines(tempDeleteUserFilePath).ToList();

                    List<string> quotelistTweet = File.ReadAllLines(tempTweetFilePath).ToList();
                    string firstItem = TargetedUser_item.Split(' ')[0];
                    firstItem = firstItem.Replace("@", "").Trim();
                    string tweetItem = TargetedUser_item.Replace("@" + firstItem, "").Trim();
                    bool notpaddeltag = false;
                    bool IsTweetTag = false;

                    if (quotelist.Contains(firstItem))
                    {
                        quotelist.Remove(firstItem);
                        notpaddeltag = true;
                    }
                    if (quotelistTweet.Contains(tweetItem))
                    {
                        quotelistTweet.Remove(tweetItem);
                        IsTweetTag = true;
                    }

                    if (notpaddeltag)
                    {
                        File.WriteAllLines(tempDeleteUserFilePath, quotelist.ToArray());
                    }
                    if (IsTweetTag)
                    {
                        File.WriteAllLines(tempTweetFilePath, quotelistTweet.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile  --> " + ex.Message, Globals.Path_FollowerErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + "Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile --> " + ex.Message, Globals.Path_FollowerErroLog);
                }
            }
        }

        public void RemoveFollwerFromTxtFile(string tempDeleteFollowFilePath, string TargetedUser_item)
        {
            lock (lockerThreadsCampaignTweet)
            {
                try
                {
                    List<string> quotelist = File.ReadAllLines(tempDeleteFollowFilePath).ToList();
                    string firstItem = TargetedUser_item;
                    bool notpaddeltag = false;

                    if (quotelist.Contains(TargetedUser_item))
                    {
                        quotelist.Remove(TargetedUser_item);
                        notpaddeltag = true;
                    }

                    if (notpaddeltag)
                    {
                        File.WriteAllLines(tempDeleteFollowFilePath, quotelist.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile  --> " + ex.Message, Globals.Path_FollowerErroLog);
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + "Error -->RemoveFollwerFromTxtFile()---> remove follwer From txtfile --> " + ex.Message, Globals.Path_FollowerErroLog);
                }
            }

        }

        #region GetImagePathFromFolder
        private List<string> GetImagePathFromFolder(string folderPath)
        {
            List<string> _lstTemimageFilePath = new List<string>();

            string[] files = System.IO.Directory.GetFiles(folderPath);

            if (files.Length > 0)
            {
                foreach (string item in files)
                {
                    if (item.Contains(".ini"))
                    {
                        continue;
                    }
                    _lstTemimageFilePath.Add(item);
                }
            }
            else
            {
                _lstTemimageFilePath.Clear();
            }

            return _lstTemimageFilePath;
        } 
        #endregion

        #region TweetMessageWithImage
        public static long UnixTimestampFromDateTime(DateTime date)
        {
            long unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp;
        }
       
        public void TweetMessageWithImage(string postAuthenticityToken, string tweetMessage, string ImageFilePath, ref Globussoft.GlobusHttpHelper globusHttpHelper, out string status)
        {
            string status1 = string.Empty;
            bool IsLocalFile = true;
            string MediaId = string.Empty;
            ///Read Image Data in Byte 
            ///convert byte in base 64 string 
            try
            {
                string _base64String = Convert.ToBase64String(File.ReadAllBytes(ImageFilePath));

                try
                {
                    //_base64String = Uri.EscapeDataString(_base64String);
                    _base64String = StringEncoderDecoder.EncodeBase64String(_base64String);
                }
                catch {

                    
                }
                ///Make Post data as given value.
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("post_authenticity_token", postAuthenticityToken);
                nvc.Add("iframe_callback", "window.top.swift_tweetbox_1379509362916");
                nvc.Add("in_reply_to_status_id", "");
                nvc.Add("impression_id", "");
                nvc.Add("earned", "");
                nvc.Add("status", tweetMessage);
                nvc.Add("media_data[]", _base64String);
                nvc.Add("media_empty", "");
                nvc.Add("place_id", "");

                ///call method for posting 
                ///
                string txid = (UnixTimestampFromDateTime(System.DateTime.Now) * 1000).ToString();

                string postData = "authenticity_token=" + postAuthenticityToken + "&iframe_callback=&media=" + _base64String + "&upload_id=" + txid + "&origin=https%3A%2F%2Ftwitter.com";
                string response_ = globusHttpHelper.postFormData(new Uri("https://upload.twitter.com/i/media/upload.iframe?origin=https%3A%2F%2Ftwitter.com"), postData, "https://twitter.com/", "", "", "", "https://twitter.com/");
                
                MediaId = globusHttpHelper.getBetween(response_, "snowflake_media_id\":", ",").Replace("snowflake_media_id\":", "").Trim();
                tweetMessage = Uri.EscapeDataString(tweetMessage);
                string finalpostdata = "authenticity_token=" + postAuthenticityToken + "&media_ids=" + MediaId + "&place_id=&status="+tweetMessage+"&tagged_users=";

                response_ = globusHttpHelper.postFormData(new Uri("https://twitter.com/i/tweet/create"), finalpostdata, "https://twitter.com/", "", "XMLHttpRequest", "", "https://twitter.com/");

                //globusHttpHelper.HttpUploadImageFileWithMessage("https://upload.twitter.com/i/tweet/create_with_media.iframe", ImageFilePath, "media_data[]", "application/octet-stream", nvc, true, ref status1);

                //if (status1 == "okay")
                //{
                //    status1 = "posted";
                //}

                if (response_.Contains("tweet_id"))
                {
                    status1 = "posted";
                }
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImagePostData() -- Tweet -- que_TweetMessages_Hashtags --> " + ex.Message, Globals.Path_TweetingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> TweetMessageWithImagePostData() --  Tweet -- que_TweetMessages_Hashtags  --> " + ex.Message, Globals.Path_TweetAccountManager);
            }
            status = status1;
        } 
        #endregion

        #region logEvents_addToLogger
        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                Log(eArgs.log);
            }
        } 
        #endregion

        #region Log
        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            CampaignTweetLogEvents.LogText(eventArgs);
        } 
        #endregion

    }

}
