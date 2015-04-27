using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CampaignManager;
using System.Threading;
using BaseLib;
using System.IO;

namespace MixedCampaignManager.classes
{
    class Cls_FollowStart
    {
        public BaseLib.Events CampaignFollowLogEvents = new BaseLib.Events();

        public static bool _IsFollowProcessStart = true;

        public int counterThreadsCampaignFollow = 0;

        public readonly object lockerThreadsCampaignFollow = new object();

        public static Events CampaignStopLogevents = new Events();
        public static Events CampaignStartLogevents = new Events();
        public static Events CampaignFinishedLogevents = new Events();
        public  static string campName = string.Empty;
        
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

        public void StartProcess(String CampaignName, String featurName, DataRow CampRow)
        {
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
                catch { }

                //DataRow[] drModelDetails = CompaignsDataSet.Tables[0].Select("CampaignName = '" + CampaignName + "'");

                //if (drModelDetails.Count() == 0)
                //{

                //}

                //Get 1st row from arrey 
                DataRow DrCampaignDetails = CampRow;

                //Set all Details 

                string AcFilePath = DrCampaignDetails.ItemArray[2].ToString();
                string FollowFilePath = DrCampaignDetails.ItemArray[3].ToString();
                bool divideEql = (Convert.ToInt32(DrCampaignDetails.ItemArray[5]) == 1) ? true : false;
                bool dividebyUser = (Convert.ToInt32(DrCampaignDetails.ItemArray[6]) == 1) ? true : false;
                int NoOfUsersDivid = Convert.ToInt32(DrCampaignDetails.ItemArray[7]);
                bool IsfastFollow = (Convert.ToInt32(DrCampaignDetails.ItemArray[8]) == 1) ? true : false;
                int NoOfFollowPerAc = Convert.ToInt32(DrCampaignDetails.ItemArray[9]);
                bool IsSchedulDaily = (Convert.ToInt32(DrCampaignDetails.ItemArray[12]) == 1) ? true : false;
                DateTime SchedulerStartTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[13].ToString());
                DateTime SchedulerEndTime = Convert.ToDateTime(DrCampaignDetails.ItemArray[14].ToString());
                int DelayStar = Convert.ToInt32(DrCampaignDetails.ItemArray[15]);
                int DelayEnd = Convert.ToInt32(DrCampaignDetails.ItemArray[16]);
                int NoOfThreads = Convert.ToInt32(DrCampaignDetails.ItemArray[17]);

                List<string> _lstUserAccounts = new List<string>();
                List<string> _lstFollowersName = new List<string>();
                List<List<string>> list_lstTargetUsers = new List<List<string>>();



                classes.Cls_AccountsManager Obj_AccManger = new Cls_AccountsManager();


                //Check Files is existing...
                if (!File.Exists(FollowFilePath))
                {
                    Log("[ " + DateTime.Now + " ] => [ Followers Users File Doesn't Exixst. Please change account File. ]");
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
                else if (!File.Exists(AcFilePath))
                {


                    Log("[ " + DateTime.Now + " ] => [ Account File Doesn't Exixst. Please change account File. ]");
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


                // Get User ID and pass from File ...

                _lstUserAccounts = Globussoft.GlobusFileHelper.ReadLargeFile(AcFilePath);

                Log("[ " + DateTime.Now + " ] => [ " + _lstUserAccounts.Count + " Accounts is uploaded. ]");

                //Get Followers Name 
                _lstFollowersName = Globussoft.GlobusFileHelper.ReadLargeFile(FollowFilePath);
                _lstFollowersName = _lstFollowersName.Distinct().ToList();
                Log("[ " + DateTime.Now + " ] => [ " + _lstFollowersName.Count + " Followers uploaded. ]");

                if (_lstFollowersName.Count == 0)
                {
                    Log("[ " + DateTime.Now + " ] => [ Please Upload correct Followers File. ]");
                    if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                    {
                        try
                        {
                            cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                        }
                        catch { };
                    }
                    if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                    {
                        MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(CampaignName);
                    }
                    return;
                }

                CampaignTweetAccountContainer objCampaignFollowAccountContainer = Obj_AccManger.AccountManager(_lstUserAccounts);

                if (dividebyUser || divideEql)
                {
                    int splitNo = 0;
                    if (divideEql)
                    {
                        splitNo = _lstFollowersName.Count / _lstUserAccounts.Count;
                    }
                    else if (dividebyUser)
                    {
                        if (NoOfUsersDivid != 0)
                        {
                            int res = Convert.ToInt32(NoOfUsersDivid);
                            splitNo = res;
                        }
                    }
                    if (splitNo == 0)
                    {
                        splitNo = RandomNumberGenerator.GenerateRandom(0, _lstFollowersName.Count - 1);
                    }
                    list_lstTargetUsers = Split(_lstFollowersName, splitNo);
                }


                //create logger Event object for accessing log MSG's ..
                CustomUserControls.followusecontrols.CampaignFollowerLogEvents.addToLogger += new EventHandler(logEvents_addToLogger);

                //set Max thread 
                ThreadPool.SetMaxThreads(NoOfThreads, NoOfThreads);

                int LstCounter = 0;
                foreach (var item in objCampaignFollowAccountContainer.dictionary_CampaignAccounts)
                {
                    try
                    {
                        //check list for breaking loop 
                        //if list of follow members list is completed.
                        if (LstCounter == list_lstTargetUsers.Count && (dividebyUser || divideEql))
                        {
                            Log("[ " + DateTime.Now + " ] => [ Account is grater than List of users. ]");
                            break;
                        }

                        List<string> list_lstTargetUsers_item = new List<string>();

                        if (dividebyUser || divideEql)
                        {
                            list_lstTargetUsers_item = list_lstTargetUsers[LstCounter];
                        }
                        else
                        {
                            list_lstTargetUsers_item = _lstFollowersName;
                        }

                        //get event from accountmanager class
                        // and create Event for printing log messages 
                        ((CampaignAccountManager)item.Value).logEvents.addToLogger += new EventHandler(logEvents_addToLogger);


                        //Manage no of threads
                        if (counterThreadsCampaignFollow >= NoOfThreads)
                        {
                            lock (lockerThreadsCampaignFollow)
                            {
                                Monitor.Wait(lockerThreadsCampaignFollow);
                            }
                        }

                        Thread threadGetStartProcessForFollow = new Thread(GetStartProcessForFollow);
                        threadGetStartProcessForFollow.Name = CampaignName + "_" + item.Key;
                        threadGetStartProcessForFollow.IsBackground = true;
                        threadGetStartProcessForFollow.Start(new object[] { item, list_lstTargetUsers_item, NoOfFollowPerAc, DelayStar, DelayEnd, CampaignName, IsSchedulDaily, SchedulerEndTime, divideEql, dividebyUser, FollowFilePath });



                        Thread.Sleep(1000);

                        LstCounter++;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
               
            }
        }



        int count_AccountforCampaignFollower = 0;
        public void  GetStartProcessForFollow(object param)
        {
            string CampaignName = string.Empty;
            try
            {
                Interlocked.Increment(ref counterThreadsCampaignFollow);

                Array paramsArray = new object[5];
                paramsArray = (Array)param;

                KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager> keyValuePair = (KeyValuePair<string, MixedCampaignManager.classes.CampaignAccountManager>)paramsArray.GetValue(0);

                List<string> TargetedUser = (List<string>)paramsArray.GetValue(1);
                
                int NoOfFollowPerAc = (int)paramsArray.GetValue(2);

                int DelayStart = (int)paramsArray.GetValue(3);

                int DelayEnd = (int)paramsArray.GetValue(4);

                CampaignName = (string)paramsArray.GetValue(5);

                bool IsSchedulDaily = (bool)paramsArray.GetValue(6);

                DateTime SchedulerEndTime = (DateTime)paramsArray.GetValue(7);

                bool divideEql= (bool) paramsArray.GetValue(8);

                bool dividebyUser= (bool) paramsArray.GetValue(9);

                string tempDeleteFollowFilePath = (string)paramsArray.GetValue(10);
                //Initialize Values in Local Variable 

                string accKey = (string)keyValuePair.Key;
                MixedCampaignManager.classes.CampaignAccountManager campACCManager = (MixedCampaignManager.classes.CampaignAccountManager)keyValuePair.Value;



                //Add List Of Working thread
                //we are using this list when we stop/abort running camp processes..
                try
                {
                    MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Add(CampaignName + "_" + campACCManager.Username, Thread.CurrentThread);
                }
                catch (Exception)
                {
                }

                count_AccountforCampaignFollower = CampaignAccountsList.dictionary_CampaignAccounts.Count();
                //get account logging 
                if (!(CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).Keys.Contains(accKey))
                {
                    campACCManager.Login();

                    try
                    {
                        CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts.Add(accKey, campACCManager);
                    }
                    catch { };
                }
                else
                {
                    try
                    {
                        campACCManager = null;
                        bool values = (CampaignManager.CampaignAccountsList.dictionary_CampaignAccounts).TryGetValue(accKey, out campACCManager);
                    }
                    catch (Exception)
                    {
                    }
                }

                //check account is logged in 
                if (campACCManager.IsLoggedIn)
                {
                    //check account is suspended
                    if (campACCManager.IsNotSuspended)
                    {
                        Follower.Follower Objfollower = new Follower.Follower();
                        int follow_count = 0;
                        List<string> _TargetUser = new List<string>();
                        _TargetUser.AddRange(TargetedUser);
                        foreach (var TargetedUser_item in TargetedUser)
                        {
                            long  FollowerId = 0;
                            string Followername = string.Empty;
                            //Get status from follow process
                            string status = string.Empty;

                            String FollowerStatus = string.Empty;


                            //check follower count par account 
                            if ((follow_count == NoOfFollowPerAc) && !divideEql && !dividebyUser)
                            {
                                Log("[ " + DateTime.Now + " ] => [ Finish Follow Limit " + NoOfFollowPerAc + "Today ]");
                                break;
                            }


                            //Check Scheduled Task end time 
                            //If task is scheduled 
                            if (IsSchedulDaily)
                            {
                                if (SchedulerEndTime.Hour == DateTime.Now.Hour && DateTime.Now.Minute >= SchedulerEndTime.Minute)
                                {
                                    _IsFollowProcessStart = true;
                                    
                                    
                                    new Thread(() =>
                                    {
                                        //frm_mixcampaignmanager frmcamp = new frm_mixcampaignmanager();
                                        //frmcamp.StoprunningCampaign(CampaignName);
                                        //frmcamp.StartFollowCampaign(CampaignName, "");


                                        RaiseCampaignSearchLogEvents(campName);
                                        RaiseCampaignStartLogEvents(campName);
                                    }).Start();

                                    
                                    break;
                                }
                            }


                            //Get Follower ID From Name 
                            try
                            {
                                string followerDetails = classes.CampTwitterDataScrapper.GetUserIDFromUsername_New(TargetedUser_item, out FollowerStatus, ref campACCManager.globusHttpHelper);


                                if (String.IsNullOrEmpty(followerDetails))
                                {

                                    Log("[ " + DateTime.Now + " ] => [ User :-  " + TargetedUser_item + " Suspended. Which is not followed By :- " + accKey + " ]");
                                    RemoveFollwerFromTxtFile(tempDeleteFollowFilePath, TargetedUser_item);
                                    continue;
                                }

                                if (BaseLib.NumberHelper.ValidateNumber(followerDetails))
                                {
                                    //FollowerId = Int32.Parse(followerDetails);
                                    FollowerId = ((long.Parse(followerDetails)));
                                    Followername = TargetedUser_item;                                
                                }
                                else
                                {
                                    //FollowerId = Convert.ToInt32(TargetedUser_item);
                                    FollowerId = (long.Parse(TargetedUser_item));
                                    Followername = followerDetails;
                                }

                                //call Follow Method by object follower class ..
                                Objfollower.FollowUsingProfileID_New(ref campACCManager.globusHttpHelper, "", campACCManager.postAuthenticityToken, TargetedUser_item, out status);

                                if (status == "followed")
                                {    
                               
                                   RemoveFollwerFromTxtFile(tempDeleteFollowFilePath, TargetedUser_item);

                                   follow_count++;
                                    try
                                    {
                                        //Insert details in Report table 
                                        RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, Followername, FollowerId, "", "", "", "");
                                    }
                                    catch { };

                                    //Logg current status of Follow process
                                    Log("[ " + DateTime.Now + " ] => [ " + TargetedUser_item + " Followed by :- " + accKey + " ]");

                                    //Delay after every Follow..
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStart, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " seconds ]");
                                    Thread.Sleep(delay * 1000);


                                }
                                    //Code added by Lijo John for protected accounts
                                else if (status == "pending")
                                {

                                    RemoveFollwerFromTxtFile(tempDeleteFollowFilePath, TargetedUser_item);

                                    follow_count++;
                                    try
                                    {
                                        //Insert details in Report table 
                                        RepositoryClasses.ReportTableRepository.InsertReport(CampaignName, accKey, Followername, FollowerId, "", "", "", "");
                                    }
                                    catch { };

                                    //Logg current status of Follow process
                                    Log("[ " + DateTime.Now + " ] => [ Follow request sent by :- " + accKey + " awaiting for approval since " + TargetedUser_item + " is protected]");

                                    //Delay after every Follow..
                                    int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStart, DelayEnd);
                                    Log("[ " + DateTime.Now + " ] => [ Delay :- " + delay + " seconds ]");
                                    Thread.Sleep(delay * 1000);


                                }

                                else if (status == "Already Followed")
                                {
                                    RemoveFollwerFromTxtFile(tempDeleteFollowFilePath, TargetedUser_item);

                                    follow_count++;

                                    Log("[ " + DateTime.Now + " ] => [ " + TargetedUser_item + " Already Followed by :- " + accKey + " ]");

                                }
                                else
                                {
                                    Log("[ " + DateTime.Now + " ] => [ " + TargetedUser_item + "  could not Followed by :- " + accKey + " ]");
                                }
                                
                                //Delay after every Follow..
                                //int delay = BaseLib.RandomNumberGenerator.GenerateRandom(DelayStar * 1000, DelayEnd * 1000);
                                //Log("[ " + DateTime.Now + " ] => [ Delay :- " + TimeSpan.FromMilliseconds(delay).Seconds + " seconds ]");
                                //Thread.Sleep(delay);
                            }
                            catch (Exception ex)
                            {
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error -->GetStartProcessForFollow()---> Get Follower ID From Name --> " + ex.Message, Globals.Path_FollowerErroLog);
                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error -->GetStartProcessForFollow()---> Get Follower ID From Name --> " + ex.Message, Globals.Path_FollowerErroLog);
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                //count_AccountforCampaignFollower--;
                
                Interlocked.Decrement(ref counterThreadsCampaignFollow);
                lock (lockerThreadsCampaignFollow)
                {
                    Monitor.Pulse(lockerThreadsCampaignFollow);
                }
                if (counterThreadsCampaignFollow == 0)
                {
                    _IsFollowProcessStart = true;
                    if (cls_variables.lstCampaignStartShedular.Contains(CampaignName))
                    {
                        try
                        {
                            cls_variables.lstCampaignStartShedular.Remove(CampaignName);
                        }
                        catch { };
                    }
                    if (MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.ContainsKey(CampaignName))
                    {
                        MixedCampaignManager.classes.cls_variables.Lst_WokingThreads.Remove(CampaignName);
                    }
                    //frm_mixcampaignmanager objfrm = new frm_mixcampaignmanager();
                    //objfrm.controlStartButtonImage(CampaignName);
                    RaiseCampaignFineshedEvent(CampaignName);
                    Log("[ " + DateTime.Now + " ] => [ Process is completed for Campaign " + CampaignName + " ]");
                    Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(CampaignName +": "+DateTime.Now, Globals.path_LogCampaignCompleted);
                }
            }
        }

        public void RemoveFollwerFromTxtFile(string tempDeleteFollowFilePath, string TargetedUser_item)
        {
            lock (lockerThreadsCampaignFollow)
            {
                try
                {
                    List<string> quotelist = File.ReadAllLines(tempDeleteFollowFilePath).ToList();
                    string firstItem = TargetedUser_item;
                    bool notpaddeltag = false;

                    #region commented

                    //using (var reader = File.OpenText(tempDeleteFollowFilePath))
                    //{
                    //    while ((text = reader.ReadLine()) == firstItem)
                    //    {
                    //        notpaddeltag = true;
                    //        break;

                    //    }
                    //    lineCount++;

                    //} 
                    #endregion

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

        

        void logEvents_addToLogger(object sender, EventArgs e)
        {
            if (e is EventsArgs)
            {
                EventsArgs eArgs = e as EventsArgs;
                Log(eArgs.log);
            }
        }

        private void Log(string message)
        {
            EventsArgs eventArgs = new EventsArgs(message);
            CampaignFollowLogEvents.LogText(eventArgs);
        }


        public static List<List<string>> Split(List<string> source, int splitNumber)
        {
            if (splitNumber <= 0)
            {
                splitNumber = 1;
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / splitNumber)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

    }
}
