using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using BaseLib;
using System.Threading;
using System.IO;

namespace MixedCampaignManager.classes
{
    class Cls_ReplyStart
    {
        public BaseLib.Events CampaignreplyLogEvents = new BaseLib.Events();
        public bool _IsReplyProcessStart = true;

        public int counterThreadsCampaignReply = 0;

        public readonly object lockerThreadsCampaignReply = new object();
        public static Events CampaignFinishedLogevents = new Events();

        public void StartProcess(DataSet CompaignsDataSet, string CampaignName)
        {

            DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

            if (drModelDetails.Count() == 0)
            {

            }

            //Get 1st row from arrey 
            DataRow DrCampaignDetails = drModelDetails[0];

            //get details from row
            string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
            string _ReplyMsgfilePath = DrCampaignDetails.ItemArray[3].ToString();
            string _ReplyKeyword = DrCampaignDetails.ItemArray[4].ToString();
            bool _IsUsername = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
            bool _IsUniqueMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[19]) == 1) ? true : false;
            bool _IsReplyParDay = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
            int _NoofReplyParDay = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
            int _NoofReplyParAc = Convert.ToInt32(DrCampaignDetails.ItemArray[10]);
            bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
            //bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[11]) == 1) ? true : false;
            DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
            DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
            //int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[14]);
            //int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
            //int NoOfThreads = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);

            int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
            int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
            int NoOfThreads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);
            bool IsDuplicateMessage = (Convert.ToInt32(DrCampaignDetails.ItemArray[22]) == 1) ? true : false;

            List<string> _lstUserAccounts = new List<string>();
            List<List<CampTwitterDataScrapper.StructTweetIDs>> list_lstTargetMessages = new List<List<CampTwitterDataScrapper.StructTweetIDs>>();

            classes.Cls_AccountsManager Obj_AccManger = new Cls_AccountsManager();

            //checking files 
            if (!File.Exists(AcFilePath))
            {
                Log("[ " + DateTime.Now + " ] => [ Account file doesn't exist. Please Update new file in campaign " + CampaignName + " ]");
                return;
            }
            else if (!File.Exists(_ReplyMsgfilePath))
            {
                Log("[ " + DateTime.Now + " ] => [ reply Message file doesn't exist. Please Update new file in campaign " + CampaignName + " ]");
                return;
            }
            else if (!File.Exists(_ReplyKeyword))
            {
                Log("[ " + DateTime.Now + " ] => [ reply Keyword file doesn't exist. Please Update new file in campaign " + CampaignName + " ]");
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

            if (string.IsNullOrEmpty(_ReplyKeyword))
            {
                MessageBox.Show("Reply keyword File is not mention.");
                return;
            }

            CampaignManager.CampaignTweetAccountContainer objCampaignReTweetAccountContainer = Obj_AccManger.AccountManager(_lstUserAccounts);

            Log("[ " + DateTime.Now + " ] => [ " + objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count + " Accounts Uploaded. ]");


            //Get the Reply Messages from File path 

            List<string> _lst_ReplyMessages = new List<string>();
            List<string> _lst_ReplyKeywordFile = new List<string>();

            _lst_ReplyMessages = Globussoft.GlobusFileHelper.ReadLargeFile(_ReplyMsgfilePath);

            //_lst_ReplyMessages = _lst_ReplyMessages.Distinct().ToList();
            if (!IsDuplicateMessage)
            {
                _lst_ReplyMessages = _lst_ReplyMessages.Distinct().ToList();
            }


            if (IsDuplicateMessage == true)
            {
                #region
                int countDifference = _lstUserAccounts.Count - _lst_ReplyMessages.Count;

                if (countDifference != 0)
                {
                    List<string> TemList = new List<string>();
                    for (int i = 0; i < countDifference; i++)
                    {
                        try
                        {
                            TemList.Add(_lst_ReplyMessages[i]);
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
                            _lst_ReplyMessages.AddRange(TemList);
                            break;
                        }
                    }
                }
                #endregion
            }
            else if ((!IsDuplicateMessage == true) && _lstUserAccounts.Count > _lst_ReplyMessages.Count)
            {
                if (_lst_ReplyMessages.Count > 0)
                {
                    //Log("[ " + DateTime.Now + " ] => [ Message is less than accounts ]");
                    //DialogResult dialogResult = MessageBox.Show("Message is less than accounts. Do you want to repeat remaining Messages.", "Warning", MessageBoxButtons.YesNo);
                    //if (dialogResult == DialogResult.Yes)
                    {
                        #region
                        int countDifference = _lstUserAccounts.Count - _lst_ReplyMessages.Count;

                        if (countDifference != 0)
                        {
                            List<string> TemList = new List<string>();
                            for (int i = 0; i < countDifference; i++)
                            {
                                try
                                {
                                    TemList.Add(_lst_ReplyMessages[i]);
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
                                    _lst_ReplyMessages.AddRange(TemList);
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


            _lst_ReplyKeywordFile = Globussoft.GlobusFileHelper.ReadLargeFile(_ReplyKeyword);
            _lst_ReplyKeywordFile = _lst_ReplyKeywordFile.Distinct().ToList();

            if (_lst_ReplyKeywordFile.Count == 0)
            {
                MessageBox.Show("Reply Keyword File is Empty.");
                return;
            }

            if (_lst_ReplyMessages.Count == 0)
            {
                MessageBox.Show("Tweet Message File is Empty.");
                return;
            }


            //Add remaining messages
            if (_lstUserAccounts.Count > _lst_ReplyMessages.Count)
            {
                Log("[ " + DateTime.Now + " ] => [ Message is less than accounts ]");
                DialogResult dialogResult = MessageBox.Show("Message is less than accounts. Do you want to repeat remaining Messages.", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    #region
                    int countDifference = _lstUserAccounts.Count - _lst_ReplyMessages.Count;

                    if (countDifference != 0)
                    {
                        List<string> TemList = new List<string>();
                        for (int i = 0; i < countDifference; i++)
                        {
                            try
                            {
                                TemList.Add(_lst_ReplyMessages[i]);
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
                                _lst_ReplyMessages.AddRange(TemList);
                                break;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Puja
                    //int count = _lst_ReplyMessages.Count;
                    //Queue<string> queUserAccount = new Queue<string>();
                    //Queue<string> queReplyMessages = new Queue<string>();
                    //for (int i = 0; i < count; i++)
                    //{
                    //    try
                    //    {
                    //        queUserAccount.Enqueue(_lstUserAccounts[i].ToString());
                    //    }
                    //    catch { }
                    //}
                    //foreach (string item in _lst_ReplyMessages)
                    //{
                    //    try
                    //    {
                    //        queReplyMessages.Enqueue(item);
                    //    }
                    //    catch
                    //    { }
                    //}

                    //_lstUserAccounts.Clear();

                    ////  queUserAccount.Dequeue(_lstUserAccounts(_item));



                    //foreach (string _item in queUserAccount)
                    //{
                    //    try
                    //    {
                    //        _lstUserAccounts.Add(queUserAccount.Dequeue());
                    //    }
                    //    catch
                    //    { }
                    //} 
                    #endregion

                   
                }
            }

            Log("[ " + DateTime.Now + " ] => [ " + _lst_ReplyMessages.Count + " Reply Messages uploaded. ]");

            //get All tweets from entered user name 
            //Scrap Tweets using Username

            List<CampTwitterDataScrapper.StructTweetIDs> lst_Struct_ReplyData = new List<CampTwitterDataScrapper.StructTweetIDs>();

            List<CampTwitterDataScrapper.StructTweetIDs> lst_Struct_ReplyDataTemp = new List<CampTwitterDataScrapper.StructTweetIDs>();

            CampTwitterDataScrapper CamptweetScrapper = new CampTwitterDataScrapper();

           int noOfRecordsForUniqueMessage = (_NoofReplyParAc * (objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count));
           //List<string> _lstTweetMessageForUnique = new List<string>();
           int splitNo = 0;
            //Get details according to enter keyword is user name or keyword

            CamptweetScrapper.noOfRecords = (_NoofReplyParAc * (objCampaignReTweetAccountContainer.dictionary_CampaignAccounts.Count));

            Log("[ " + DateTime.Now + " ] => [ Getting  tweets. ]");
            foreach (string _ReplyKeywordTemp in _lst_ReplyKeywordFile)
            {
                if (_IsUsername)
                {
                    lst_Struct_ReplyDataTemp = CamptweetScrapper.TweetExtractor_ByUserName_New(_ReplyKeywordTemp);

                }
                
                else
                {
                    lst_Struct_ReplyDataTemp = CamptweetScrapper.GetTweetData_New_ForCampaign(_ReplyKeywordTemp, _NoofReplyParAc);

                    
                }

                lst_Struct_ReplyData.AddRange(lst_Struct_ReplyDataTemp);
            }

            if (_IsUniqueMessage)
            {
               // lst_Struct_ReplyDataTemp = CamptweetScrapper.GetTweetData_New_ForCampaign(_ReplyKeywordTemp, noOfRecordsForUniqueMessage);

                splitNo = lst_Struct_ReplyData.Count / _lstUserAccounts.Count;
                if (splitNo == 0)
                {
                    splitNo = RandomNumberGenerator.GenerateRandom(0, lst_Struct_ReplyData.Count - 1);
                }
                list_lstTargetMessages = Split(lst_Struct_ReplyData, splitNo);

            }


            Log("[ " + DateTime.Now + " ] => [ Tweet found process completed. ]");
            if (lst_Struct_ReplyData.Count == 0)
            {
                Log("[ " + DateTime.Now + " ] => [ " + lst_Struct_ReplyData.Count + " Tweet Founded. ]");
                MessageBox.Show("No records Found from " + _ReplyKeyword + " Keywords.");
                return;
            }
            else
                Log("[ " + DateTime.Now + " ] => [ " + lst_Struct_ReplyData.Count + " Tweet Founded. ]");


            int LstCounter = 0;
            foreach (var Account in objCampaignReTweetAccountContainer.dictionary_CampaignAccounts)
            {
                ((CampaignAccountManager)Account.Value).logEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                //ThreadPool.QueueUserWorkItem(new WaitCallback(GetStartReply), new object[] { Account, lst_Struct_ReplyData, _lst_ReplyMessages, _IsReplyParDay, _NoofReplyParDay, _NoofReplyParAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime });
                //GetStartReply(new object[] { Account, lst_Struct_ReplyData, _lst_ReplyMessages, _IsReplyParDay, _NoofReplyParDay, _NoofReplyParAc, DelayStar, DelayEnd, CampaignName });
                if (LstCounter == list_lstTargetMessages.Count && (_IsUniqueMessage))
                {
                    Log("[ " + DateTime.Now + " ] => [ Account is greater than List of tweets. ]");
                    break;
                }

                if (_IsUniqueMessage)
                {
                    lst_Struct_ReplyData = list_lstTargetMessages[LstCounter];
                }
                

                if (counterThreadsCampaignReply >= NoOfThreads)
                {
                    lock (lockerThreadsCampaignReply)
                    {
                        Monitor.Wait(lockerThreadsCampaignReply);
                    }
                }

                #region New Licensing Feature Added By Sonu
                try
                {
                    if (Globals.IsBasicVersion || Globals.IsProVersion || Globals.IsFreeVersion)
                    {
                        string queryCheckDataBaseEmpty = "select * from tb_FBAccount";
                        DataSet DS1 = DataBaseHandler.SelectQuery(queryCheckDataBaseEmpty, "tb_FBAccount");
                        if (!(DS1.Tables[0].Rows.Count == 0))
                        {
                            DataTable DT = DS1.Tables[0];
                            bool check = DT.Select().Any(x => x.ItemArray[0].ToString() == Account.Key);
                            if (!check)
                            {
                                System.Windows.Forms.MessageBox.Show("Please Upload this Account in Account Manager");
                                return;
                            }
                            else
                            {
                                Thread threadGetStartProcessForReply = new Thread(GetStartReply);
                                threadGetStartProcessForReply.Name = CampaignName + "_" + Account.Key;
                                threadGetStartProcessForReply.IsBackground = true;
                                threadGetStartProcessForReply.Start(new object[] { Account, lst_Struct_ReplyData, _lst_ReplyMessages, _IsReplyParDay, _NoofReplyParDay, _NoofReplyParAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime });
                            }
                        }
                        else
                        {
                            System.Windows.Forms.MessageBox.Show("Please Upload this Account in Account Manager");
                            return;
                        }
                    }
                    else
                    {
                        Thread threadGetStartProcessForReply = new Thread(GetStartReply);
                        threadGetStartProcessForReply.Name = CampaignName + "_" + Account.Key;
                        threadGetStartProcessForReply.IsBackground = true;
                        threadGetStartProcessForReply.Start(new object[] { Account, lst_Struct_ReplyData, _lst_ReplyMessages, _IsReplyParDay, _NoofReplyParDay, _NoofReplyParAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime });
                    }
                }
                catch { };
                #endregion

                #region old Code
                //Thread threadGetStartProcessForReply = new Thread(GetStartReply);
                //threadGetStartProcessForReply.Name = CampaignName + "_" + Account.Key;
                //threadGetStartProcessForReply.IsBackground = true;
                //threadGetStartProcessForReply.Start(new object[] { Account, lst_Struct_ReplyData, _lst_ReplyMessages, _IsReplyParDay, _NoofReplyParDay, _NoofReplyParAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime });
                #endregion

                Thread.Sleep(1000);
                LstCounter++;
            }
        }

        int count_AccountForReply_CompleteMessage = 0;
        public void GetStartReply(Object parameters)
        {
            string CampaignName = string.Empty;
            try
            {
                Interlocked.Increment(ref counterThreadsCampaignReply);

                Tweeter.Tweeter tweeter = new Tweeter.Tweeter();

                tweeter.logEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                Array paramsArray = new object[10];

                paramsArray = (Array)parameters;

                //get value from param
                KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager> keyValue = (KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager>)paramsArray.GetValue(0);

                string accKey = keyValue.Key;

                MixedCampaignManager.classes.CampaignAccountManager ReplyAccountManager = keyValue.Value;

                List<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs> lst_ReplyData = (List<MixedCampaignManager.classes.CampTwitterDataScrapper.StructTweetIDs>)paramsArray.GetValue(1);

                List<string> lst_ReplyMsgs = (List<string>)paramsArray.GetValue(2);

                bool _IsReplyParDay = (bool)paramsArray.GetValue(3);

                int _NoofReplyParDay = (int)paramsArray.GetValue(4);

                int _NoofReplyParAc = (int)paramsArray.GetValue(5);

                int DelayStar = (int)paramsArray.GetValue(6);

                int DelayEnd = (int)paramsArray.GetValue(7);

                CampaignName = (string)paramsArray.GetValue(8);

                //Get scheduler time and status if task is scheduled 
                bool IsSchedulDaily = (bool)paramsArray.GetValue(9);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(10);


                int MsgCounter = 0;

                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + ReplyAccountManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }

                //get account logging 
                if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
                {
                    ReplyAccountManager.Login();
                    try
                    {
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, ReplyAccountManager);
                    }
                    catch { };
                }
                else
                {
                    try
                    {
                        ReplyAccountManager = null;
                        bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out ReplyAccountManager);
                    }
                    catch (Exception)
                    {
                    }
                }

                count_AccountForReply_CompleteMessage = CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Count();


                if (!ReplyAccountManager.IsLoggedIn || !ReplyAccountManager.IsNotSuspended)
                {
                    return;
                }

                foreach (var lst_ReplyData_item in lst_ReplyData)
                {

                    //Check Scheduled Task end time 
                    //If task is scheduled 
                    if (IsSchedulDaily)
                    {
                        if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                        {
                            _IsReplyProcessStart = true;
                            new Thread(() =>
                            {
                                frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                frmcamp.StoprunningCampaign(CampaignName);
                            }).Start();
                            break;
                        }
                    }

                   // foreach (string Msg in lst_ReplyMsgs)
                    {
                        string tweetId = lst_ReplyData_item.ID_Tweet;

                        string tweetUsername = lst_ReplyData_item.username__Tweet_User;

                        if (MsgCounter == lst_ReplyMsgs.Count)
                        {
                            MsgCounter = 0;
                        }

                        string Msg = lst_ReplyMsgs[MsgCounter];

                        string _wholeTweetMessage = lst_ReplyData_item.wholeTweetMessage;
                        string tweetStatus;

                        tweeter.Reply(ref ReplyAccountManager.globusHttpHelper, "", ReplyAccountManager.postAuthenticityToken, tweetId, tweetUsername, Msg, out tweetStatus);

                        if (tweetStatus == "posted")
                        {
                            Log("[ " + DateTime.Now + " ] => [ Message :- " + Msg + " Tweet :- " + _wholeTweetMessage + " by " + keyValue.Key + " ]");

                            try
                            {
                                RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, "", 0, tweetId, "", "", Msg);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            Log("[ " + DateTime.Now + " ] => [ Status :- " + tweetStatus + " by " + keyValue.Key + " ]");
                        }

                        int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar * 1000, DelayEnd * 1000);
                        Log("[ " + DateTime.Now + " ] => [ Delay :- " + TimeSpan.FromMilliseconds(delay).Seconds + " Seconds. ]");
                        Thread.Sleep(delay);
                        MsgCounter++; 
                    }
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                count_AccountForReply_CompleteMessage--;
                Interlocked.Decrement(ref counterThreadsCampaignReply);

                lock (lockerThreadsCampaignReply)
                {
                    Monitor.Pulse(lockerThreadsCampaignReply);
                    if (counterThreadsCampaignReply == 0)
                    {
                        RaiseCampaignFineshedEvent(CampaignName);
                        Log("[ " + DateTime.Now + " ] => [ Process completed. ]");
                        Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(CampaignName + ": " + DateTime.Now, Globals.path_LogCampaignCompleted);
                    }
                }
                
                
            }
        }

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                Log(eArgs.log);
            }
        }

        public void RaiseCampaignFineshedEvent(string log)
        {
            EventsArgs eArgs = new EventsArgs(log);
            CampaignFinishedLogevents.LogText(eArgs);
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            CampaignreplyLogEvents.LogText(eventArgs);
        }

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
    }
}
