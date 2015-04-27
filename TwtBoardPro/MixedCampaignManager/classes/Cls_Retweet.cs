using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseLib;
using CampaignManager;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace MixedCampaignManager.classes
{
    class Cls_Retweet
    {
        #region global declaration
        public BaseLib.Events CampaignReTweetLogEvents = new BaseLib.Events();
        public bool _IsReTweetProcessStart = true;
        public int counterThreadsCampaignRetweet = 0;
        public readonly object lockerThreadsCamapignRetweet = new object();
        public static Events CampaignFinishedLogevents = new Events();
        #endregion

        #region StartRetweetProcess

        public void StartRetweetProcess(DataSet CompaignsDataSet, String CampaignName)
        {
            try
            {
                DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                if (drModelDetails.Count() == 0)
                {

                }

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = drModelDetails[0];


                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string _retweetKeyword = DrCampaignDetails.ItemArray[3].ToString();
                bool _IsUsername = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                bool _IsUniqueMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[19]) == 1) ? true : false;
                bool _IsRetweetParDay = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                int _NoofRetweetParDay = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                int _NoofRetweetParAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
                int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                int NoOfThreads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);

                List<string> _lstUserAccounts = new List<string>();
                List<List<CampTwitterDataScrapper.StructTweetIDs>> list_lstTargetTweetMessages = new List<List<CampTwitterDataScrapper.StructTweetIDs>>();
                classes.Cls_AccountsManager Obj_AccManger = new Cls_AccountsManager();

                //Checking Account File is Exist or not 
                if (!File.Exists(AcFilePath))
                {
                    ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ Account File Doesn't Exist. Please Update Account File of Campaign " + CampaignName + " ]");
                    return;
                }
                // Get User ID and pass from File ...
                _lstUserAccounts = Globussoft.GlobusFileHelper.ReadLargeFile(AcFilePath);

                _lstUserAccounts = _lstUserAccounts.Distinct().ToList();

                if (_lstUserAccounts.Count == 0)
                {
                    MessageBox.Show("Account File is Empty.");
                    return;
                }

                if (string.IsNullOrEmpty(_retweetKeyword))
                {
                    MessageBox.Show("Re-Tweet key word is not mention.");
                    return;
                }

                CampaignTweetAccountContainer objCampaignReTweetAccountContainer = Obj_AccManger.AccountManager(_lstUserAccounts);

                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ " + objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count + " Accounts Uploaded. ]");


                //get All tweets from entered user name 
                //Scrap Tweets using Username
                List<CampTwitterDataScrapper.StructTweetIDs> lst_Struct_TweetData = new List<CampTwitterDataScrapper.StructTweetIDs>();
                CampTwitterDataScrapper CamptweetScrapper = new CampTwitterDataScrapper();
                int noOfRecordsForUniqueMessage = (_NoofRetweetParAc * (objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count));
                //List<string> _lstTweetMessageForUnique = new List<string>();
                int splitNo = 0;

                //Get details according to enter keyword is user name or keyword
                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ Getting Tweets from " + _retweetKeyword + " KeyWord. ]");
                CamptweetScrapper.noOfRecords = (_NoofRetweetParAc * (objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count));
                if (_IsUsername)
                {
                    lst_Struct_TweetData = CamptweetScrapper.TweetExtractor_ByUserName_New(_retweetKeyword);
                }
                else if (_IsUniqueMessage)
                {
                    lst_Struct_TweetData = CamptweetScrapper.GetTweetData_New_ForCampaign(_retweetKeyword, noOfRecordsForUniqueMessage);

                    splitNo = lst_Struct_TweetData.Count / _lstUserAccounts.Count;
                    if (splitNo == 0)
                    {
                        splitNo = RandomNumberGenerator.GenerateRandom(0, lst_Struct_TweetData.Count - 1);
                    }
                    list_lstTargetTweetMessages = Split(lst_Struct_TweetData, splitNo);

                }
                else
                {
                    lst_Struct_TweetData = CamptweetScrapper.GetTweetData_New_ForCampaign(_retweetKeyword, _NoofRetweetParAc);

                }

                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ " + lst_Struct_TweetData.Count + " Tweet Founded. ]");

                if (lst_Struct_TweetData.Count == 0)
                {
                    ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ " + lst_Struct_TweetData.Count + " Tweet Founded. ]");
                    MessageBox.Show("No records Found from " + _retweetKeyword + " Keywored.");
                    return;
                }

                MixedCampaignManager.CustomUserControls.retweetusercontrols Objretweetusercontrol = new CustomUserControls.retweetusercontrols();
                Objretweetusercontrol.retweetusercontrolslogEvents.addToLogger += new EventHandler(logEvents_addToLogger);


                ThreadPool.SetMaxThreads(NoOfThreads, NoOfThreads);
                int LstCounter = 0;
                foreach (var Account in objCampaignReTweetAccountContainer.dictionary_CampaignAccounts)
                {
                    ((CampaignAccountManager)Account.Value).logEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                    if (_IsUniqueMessage)
                    {
                        lst_Struct_TweetData = list_lstTargetTweetMessages[LstCounter];
                    }

                    if (counterThreadsCampaignRetweet >= NoOfThreads)
                    {
                        lock (lockerThreadsCamapignRetweet)
                        {
                            Monitor.Wait(lockerThreadsCamapignRetweet);
                        }
                    }


                    Thread threadGetStartProcessForRetweet = new Thread(startRetweeting);
                    threadGetStartProcessForRetweet.Name = CampaignName + "_" + Account.Key;
                    threadGetStartProcessForRetweet.IsBackground = true;
                    threadGetStartProcessForRetweet.Start(new object[] { Account, lst_Struct_TweetData, _IsRetweetParDay, _NoofRetweetParDay, _NoofRetweetParAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime });

                    Thread.Sleep(1000);
                    LstCounter++;
                }
            }
            catch (Exception Err)
            {
                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ There is some error,Retweet Process cannot be started ]");
            }
        }
        #endregion

        #region startRetweeting
        public void startRetweeting(object parameters)
        {
            Tweeter.Tweeter tweeter = new Tweeter.Tweeter();
            tweeter.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);
            string CampaignName = string.Empty;
            try
            {
                Interlocked.Increment(ref counterThreadsCampaignRetweet);

                Queue<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs> Queue_Struct_TweetData = new Queue<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs>();

                int Tweetcounter = 0;

                //get all parems from object 

                Array paramsArray = new object[2];

                paramsArray = (Array)parameters;

                //get value from param
                KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager> keyValue = (KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager>)paramsArray.GetValue(0);

                string accKey = keyValue.Key;

                MixedCampaignManager.classes.CampaignAccountManager RetweetAccountManager = keyValue.Value;

                List<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs> lst_Struct_TweetData1 = (List<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs>)paramsArray.GetValue(1);

                bool IsRetweetParDay = (bool)paramsArray.GetValue(2);

                int NoofRetweetParDay = (int)paramsArray.GetValue(3);

                int NoofRetweetParAc = (int)paramsArray.GetValue(4);

                int minDelay = (int)paramsArray.GetValue(5);

                int maxDelay = (int)paramsArray.GetValue(6);

                 CampaignName = (string)paramsArray.GetValue(7);


                //Get scheduler time and status if task is scheduled 
                bool IsSchedulDaily = (bool)paramsArray.GetValue(8);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(9);

                // Add list Value in Queue list ...
                lst_Struct_TweetData1.ForEach(acc => { Queue_Struct_TweetData.Enqueue(acc); });


                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + RetweetAccountManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }

                //get account logging 
                if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
                {
                    RetweetAccountManager.Login();

                    try
                    {
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, RetweetAccountManager);
                    }
                    catch { };
                }
                else
                {
                    try
                    {
                        RetweetAccountManager = null;
                        bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out RetweetAccountManager);
                    }
                    catch (Exception)
                    {
                    }
                }

                if (RetweetAccountManager.IsLoggedIn)
                {
                    if (RetweetAccountManager.IsNotSuspended)
                    {
                        while (Queue_Struct_TweetData.Count > 0)
                        {
                            string tweetStatus = string.Empty;
                            int delay = 0;

                            if (Queue_Struct_TweetData.Count == 0)
                            {
                                break;
                            }

                            if (Tweetcounter == NoofRetweetParAc)
                            {
                                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ Per account re-tweet limit has exceeded from " + keyValue.Key + " ]");
                                break;
                            }


                            //Check Scheduled Task end time 
                            //If task is scheduled 
                            if (IsSchedulDaily)
                            {
                                if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                {
                                    _IsReTweetProcessStart = true;
                                    new Thread(() =>
                                    {
                                        frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                        frmcamp.StoprunningCampaign(CampaignName);
                                    }).Start();
                                    break;
                                }
                            }

                            try
                            {
                                delay = RandomNumberGenerator.GenerateRandom((minDelay), (maxDelay));
                            }
                            catch (Exception ex)
                            {
                                delay = 10;
                            }

                            MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs TweetDetails = Queue_Struct_TweetData.Dequeue();

                            tweeter.ReTweet(ref RetweetAccountManager.globusHttpHelper, "", RetweetAccountManager.postAuthenticityToken, TweetDetails.ID_Tweet, "", out tweetStatus);

                            if (tweetStatus == "posted")
                            {
                                Tweetcounter++;
                                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ >> Retweeted  : >> " + TweetDetails.wholeTweetMessage + " by " + keyValue.Key + " ]");
                                try
                                {
                                    RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, TweetDetails.ID_Tweet, "", TweetDetails.wholeTweetMessage, "");
                                }
                                catch (Exception)
                                {
                                }
                            }
                            else
                            {
                                ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ >> Couldn't Retweet  : >> " + TweetDetails.wholeTweetMessage + " by " + keyValue.Key + " ]");
                            }

                            //ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ Delay :- " + TimeSpan.FromMilliseconds(delay).Seconds + " seconds ] ");
                            ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " seconds ]");
                            Thread.Sleep(delay * 1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> startRetweeting()  --> " + ex.Message, Globals.Path_TweetingErroLog);
                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> startRetweeting() --> " + ex.Message, Globals.Path_TwtErrorLogs);
            }
            finally
            {
                tweeter.logEvents.addToLogger -= new EventHandler(logEvents_addToLogger);
                Interlocked.Decrement(ref counterThreadsCampaignRetweet);

                lock (lockerThreadsCamapignRetweet)
                {
                    Monitor.Pulse(lockerThreadsCamapignRetweet);
                }
                if (counterThreadsCampaignRetweet == 0)
                {
                    RaiseCampaignFineshedEvent(CampaignName);
                    ReTweetUserControlLog("[ " + DateTime.Now + " ] => [ PROCESS COMPLETED for Campaign "+CampaignName +" ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(CampaignName + ": " + DateTime.Now, Globals.path_LogCampaignCompleted);
                }
            }
        }
        #endregion

        #region Logger setting
        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                ReTweetUserControlLog(eArgs.log);
            }
        }
        #endregion

        #region ReTweetUserControlLog
        private void ReTweetUserControlLog(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            CampaignReTweetLogEvents.LogText(eventArgs);
        }
        public void RaiseCampaignFineshedEvent(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignFinishedLogevents.LogText(eArgs);
        }
        #endregion

        #region Split method
        public static List<List<CampTwitterDataScrapper.StructTweetIDs>> Split(List<CampTwitterDataScrapper.StructTweetIDs> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList<CampTwitterDataScrapper.StructTweetIDs>())
                .ToList();
        } 
        #endregion
    }
}
