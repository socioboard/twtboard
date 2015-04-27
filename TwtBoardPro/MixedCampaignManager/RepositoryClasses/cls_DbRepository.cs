using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using BaseLib;


namespace MixedCampaignManager.RepositoryClasses
{
    class cls_DbRepository
    {
        public static DataSet selectQuery(String Query, String DbTableName)
        {
            
                DataSet DS = new DataSet();
                string constr = BaseLib.DataBaseHandler.CONstr;
                try
                {
                    using (SQLiteConnection CON = new SQLiteConnection(constr))
                    {
                        SQLiteCommand CMD = new SQLiteCommand(Query, CON);
                        SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                        AD.Fill(DS, DbTableName);

                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);

                }
            return DS;
        }

        public static void InsertQuery(string query, string tablename)
        {
            using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                DataSet DS = new DataSet();
                AD.Fill(DS, tablename);
            }
        }

        public static DataSet UpdateQuery(String Query, String DbTableName)
        {
            DataSet DS = new DataSet();
            using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(Query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                AD.Fill(DS, DbTableName);
            }
            return DS;
        }

        public static DataSet getAllStatusFromCapaign()
        {
            String Query = "SELECT * from Campaign_report";
            DataSet DS = new DataSet();
            using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(Query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                AD.Fill(DS, "CampaignReport");
            }
            return DS;
        }

        public static bool DeleteSetting(string CampaignName, string FeaturName)
        {
            bool _IsExecuted = false;
            try
            {
                string tablename = string.Empty;
                List<string> result = new List<string>();
                using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
                {
                    CON.Open();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", CON);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString());
                    }

                    if (result.Count != 0)
                    {
                        if (result.Any(s => (s.Equals("Campaign_" + (FeaturName.ToLower())))))
                        {
                            tablename = result.Where(x => x.Contains("Campaign_" + (FeaturName.ToLower()))).ToArray()[0].ToString();

                            string DeleteQuery = "DELETE FROM " + tablename + " WHERE CampaignName='" + CampaignName + "'";
                            cmd = new SQLiteCommand(DeleteQuery, CON);
                            cmd.ExecuteNonQuery();

                            _IsExecuted = true;
                        }
                        else
                        {
                            _IsExecuted = false;
                        }
                    }
                    else
                    {
                        _IsExecuted = false;
                    }
                }
            }
            catch (Exception)
            {
                _IsExecuted = false;
            }
            return _IsExecuted;
        }

    }

    class ReportTableRepository
    {
        public static bool InsertReport(String CampName, String UserName, String FollowerName, long FollowerId, string TweetId, String TweetMessage, String RetweetedMessage, String ReplyMessage)
        {
            if (String.IsNullOrEmpty(CampName))
            {
                return false;
            }
            else if (String.IsNullOrEmpty(UserName))
            {
                return false;
            }

            DateTime InsertDateTime = DateTime.Now;

            String Query = "INSERT INTO  Campaign_Report (CampaignName, UserName, FollowerName, FollowerId, TweetId, TweetMessage,  RetweetMessage, ReplyMessage, DateTime)"
            + " VALUES ('" + CampName + "','" + UserName + "','" + FollowerName + "','" + FollowerId + "','" + TweetId + "','" + TweetMessage + "','" + RetweetedMessage + "','" + ReplyMessage + "','" + InsertDateTime.ToString() + "')";

            using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
            {
                SQLiteCommand CMD = new SQLiteCommand(Query, CON);
                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                DataSet DS = new DataSet();
                AD.Fill(DS, "Campaign_Report");

                return true;
            }
        }

        public static bool UpdateReport(String CampName, String UserName, String FollowerName, int FollowerId, int TweetId, String TweetMessage, Boolean IsRetweeted, String ReplyMessage)
        {
            //get row from table 
            string selectrowQuery = string.Empty;

            if (IsRetweeted != false && TweetId != 0)
            {

                selectrowQuery = "Select * from Campaign_Report WHERE CampaignName= '" + CampName + "' AND UserName= '" + UserName + "'";

                DataSet DS = cls_DbRepository.selectQuery(selectrowQuery, "abc");

                if (DS.Tables[0].Rows.Count != 0)
                {
                }
            }
            return true;
        }

    }

    class CreateTableRepository
    {

        //Cheking table 

        public bool CheckAndCreateTable()
        {
            try
            {
                DataSet DS = new DataSet();
                using (SQLiteConnection CON = new SQLiteConnection(BaseLib.DataBaseHandler.CONstr))
                {
                    CON.Open();
                    List<string> result = new List<string>();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table'", CON);
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(reader["name"].ToString());
                    }

                    if (result.Count != 0)
                    {
                        if (!result.Any(s => (("Campaign_Report").Contains(s))))
                        {
                            string CreateReportQuery = "CREATE TABLE 'Campaign_Report' ('indx' INTEGER PRIMARY KEY  NOT NULL ,'CampaignName' VARCHAR NOT NULL ,'UserName' VARCHAR NOT NULL ,'FollowerName' VARCHAR NOT NULL ,'FollowerId' INTEGER,'TweetMessage' TEXT,'TweetId' TEXT DEFAULT (null) ,'RetweetMessage' TEXT,'ReplyMessage' TEXT,'DateTime' TEXT)";
                            SQLiteCommand CMD = new SQLiteCommand(CreateReportQuery, CON);
                            CMD.ExecuteNonQuery();
                        }
                        if (!result.Any(s => (("Campaign_follow").Contains(s))))
                        {
                            string CreatefollowQuery = "CREATE TABLE 'Campaign_follow' ('indx' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , 'CampaignName' VARCHAR, 'AcFilePath' VARCHAR, 'FollowingFilePath' VARCHAR, 'DividEql' INTEGER, 'DivideByUser' INTEGER, 'NoOfUser' INTEGER, 'FastFollow' INTEGER, 'NoOfFollowPerAc' INTEGER, 'ScheduledDaily' INTEGER, 'StartTime' TEXT, 'EndTime' TEXT, 'DelayFrom' INTEGER, 'DelayTo' INTEGER, 'Threads' INTEGER, 'Module' VARCHAR)";
                            SQLiteCommand CMD = new SQLiteCommand(CreatefollowQuery, CON);
                            CMD.ExecuteNonQuery();
                        }
                        if (!result.Any(s => (("Campaign_reply").Contains(s))))
                        {
                            string CreatereplyQuery = "CREATE TABLE 'Campaign_reply' ('indx' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , 'CampaignName' VARCHAR, 'AcFilePath' VARCHAR, 'ReplyFilePath' VARCHAR, 'Keyword' VARCHAR, 'IsUsername' INTEGER, 'UniqueMessage' INTEGER, 'ReplyParDay' INTEGER, 'NoofReplyParDay' INTEGER, 'NoofReplyParAc' INTEGER, 'ScheduledDaily' INTEGER, 'StartTime' TEXT, 'EndTime' TEXT, 'DelayFrom' INTEGER, 'DelayTo' INTEGER, 'Threads' INTEGER, 'Module' VARCHAR)";
                            SQLiteCommand CMD = new SQLiteCommand(CreatereplyQuery, CON);
                            CMD.ExecuteNonQuery();
                        }
                        if (!result.Any(s => (("Campaign_retweet").Contains(s))))
                        {
                            string CreateretweetQuery = "CREATE TABLE 'Campaign_retweet' ('indx' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , 'CampaignName' VARCHAR, 'AcFilePath' VARCHAR, 'Keyword' VARCHAR, 'IsUsername' INTEGER, 'UniqueMessage' INTEGER, 'RetweetParDay' INTEGER, 'NoofRetweetParDay' INTEGER, 'NoofRetweetParAc' INTEGER, 'ScheduledDaily' INTEGER, 'StartTime' TEXT, 'EndTime' TEXT, 'DelayFrom' INTEGER, 'DelayTo' INTEGER, 'Threads' INTEGER, 'Module' VARCHAR)";
                            SQLiteCommand CMD = new SQLiteCommand(CreateretweetQuery, CON);
                            CMD.ExecuteNonQuery();
                        }
                        if (!result.Any(s => (("Campaign_tweet").Contains(s))))
                        {
                            //string CreatetweetQuery = "CREATE TABLE 'Campaign_tweet' ('indx' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE ,'CampaignName' VARCHAR, 'AcFilePath' VARCHAR, 'TweetMsgFilePath' VARCHAR, 'DuplicateMsg' INTEGER, 'AllTweetParAc' INTEGER, 'HashTag' INTEGER, 'TweetParDay' INTEGER,'NoOfTweetParDay' INTEGER, 'NoOfTweetPerAc' INTEGER, 'ScheduledDaily' INTEGER, 'StartTime' TEXT, 'EndTime' TEXT, 'DelayFrom' INTEGER, 'DelayTo' INTEGER, 'Threads' INTEGER, 'Module' VARCHAR)";
                            string CreatetweetQuery = "CREATE TABLE 'Campaign_tweet' ('indx' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE ,'CampaignName' VARCHAR, 'AcFilePath' VARCHAR, 'TweetMsgFilePath' VARCHAR, 'TweetImageFolderPath' VARCHAR, 'TweetUploadUserFilePath' VARCHAR , 'IsUploadUserFilePath' INTEGER ,'DuplicateMsg' INTEGER, 'AllTweetParAc' INTEGER, 'HashTag' INTEGER, 'TweetParDay' INTEGER,'NoOfTweetParDay' INTEGER, 'NoOfTweetPerAc' INTEGER, 'TweetWithImage' INTEGER,  'ScheduledDaily' INTEGER, 'StartTime' TEXT, 'EndTime' TEXT, 'DelayFrom' INTEGER, 'DelayTo' INTEGER, 'Threads' INTEGER, 'Module' VARCHAR)";

                            SQLiteCommand CMD = new SQLiteCommand(CreatetweetQuery, CON);
                            CMD.ExecuteNonQuery();
                        }
                        else
                        {
                            try
                            {
                                DataSet DS1 = new DataSet();
                                string SelectColumns = "SELECT sql FROM sqlite_master  WHERE tbl_name = 'Campaign_tweet' AND type = 'table'";
                                SQLiteCommand CMD = new SQLiteCommand(SelectColumns, CON);
                                SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                                AD.Fill(DS1, "Campaign_tweet");

                                string[] TableColumnsArr = DS1.Tables[0].Rows[0]["sql"].ToString().Split(',');

                                if (TableColumnsArr.Count() < 19)
                                {
                                    result.Clear();
                                    CMD = new SQLiteCommand();
                                    CMD = new SQLiteCommand("DROP TABLE IF EXISTS Campaign_tweet;", CON);
                                    CMD.ExecuteReader();
                                    CheckAndCreateTable();
                                }
                            }
                            catch
                            {

                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
