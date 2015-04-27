using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;

namespace Randomiser
{
    public class clsDB_Randomiser
    {
        public void QTF_InserIntotb_Randomiser(string LoginUserName, string MessageType, string Message, string DateTime, string MessageStatus, string TableStatus)
        {
            try
            {
                string strQuery = "INSERT INTO tb_Randomiser(LoginUserName , MessageType , Message , DateTime , MessageStatus , TableStatus) VALUES ('" + LoginUserName + "' , '" + MessageType + "' , '" + Message + "' , '" + DateTime + "' , '" + MessageStatus + "' , '" + TableStatus+"')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        public void Follow_InserIntotb_Randomiser(string LoginUserName, string Follow, string FollowUserName, string DateTime, string MessageStatus, string TableStatus)
        {
            try
            {
                string strQuery = "INSERT INTO tb_Randomiser(LoginUserName , Follow , FollowUserName , DateTime , MessageStatus , TableStatus) VALUES ('" + LoginUserName + "' , '" + Follow + "' , '" + FollowUserName + "' , '" + DateTime + "' , '" + MessageStatus + "' , '" + TableStatus + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        

        public DataTable SelectFollowData(string useremail)
        {
            try
            {

                string strQuery = "SELECT * FROM tb_Follow where username='" + useremail + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Follow");

                DataTable dt = ds.Tables["tb_Follow"];
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public DataTable SelectFollowDataAccordingDate(string useremail,string date)
        {
            try
            {

                string strQuery = "SELECT * FROM tb_Follow where username='" + useremail + "' and DateFollowed = '" + date + "'";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_Follow");

                DataTable dt = ds.Tables["tb_Follow"];
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }


        public void InsertUpdateFollowTable(string useremail, string following_id, string following_username)
        {
            try
            {
                string strDateTime = DateTime.Today.ToString();
                string strQuery = "INSERT INTO tb_Follow (username, following_id, following_username, DateFollowed) VALUES ('" + useremail + "' , '" + following_id + "' , '" + following_username + "' , '" + strDateTime + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Follow");
            }
            catch (Exception)
            {
                //string strQuery = "UPDATE tb_Follow SET username='" + useremail + "', following_id='" + following_id + "', following_username='" + following_username + "' WHERE username='" + useremail + "'";
                //DataBaseHandler.UpdateQuery(strQuery, "tb_Follow");
            }
        }

        public void Mention_InsertIntotb_Randomiser(string LoginUserName,string ReplyUserName, string MessageType, string Message, string DateTime, string MessageStatus, string TableStatus)
        {
            try
            {
                string strQuery = "INSERT INTO tb_Randomiser(LoginUserName ,ReplyUserName, MessageType , Message , DateTime , MessageStatus , TableStatus) VALUES ('" + LoginUserName + "' , '" + ReplyUserName + "' , '" + MessageType + "' , '" + Message + "' , '" + DateTime + "' , '" + MessageStatus + "' , '" + TableStatus + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        public void Mention_InsertIntotb_RandomiserMention(string LoginUserName, string ScreenName, string TweetUserId, string TweetUserName, string TweetMessage)
        {
            try
            {
                string strQuery = "INSERT INTO tb_RandomiserMention(LoginUserName ,ScreenName, TweetUserId , TweetUserName , TweetMessage , MentionStatus , TableStatus) VALUES ('" + LoginUserName + "' , '" + ScreenName + "' , '" + TweetUserId + "' , '" + TweetUserName + "' , '" + TweetMessage + "' , '0' , '0')";
                DataBaseHandler.InsertQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
        }

        public void ReTweet_InserIntotb_Randomiser(string LoginUserName, string MessageType, string TweetId, string DateTime, string MessageStatus, string TableStatus)
        {
            try
            {
                string strQuery = "INSERT INTO tb_Randomiser(LoginUserName , MessageType , TweetId , DateTime , MessageStatus , TableStatus) VALUES ('" + LoginUserName + "' , '" + MessageType + "' , '" + TweetId + "' , '" + DateTime + "' , '" + MessageStatus + "' , '" + TableStatus + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        public DataSet Selecttb_Randomiser(string userName)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT ReplyUserName FROM tbRandomiser WHERE (UserName = '" + userName + "')";
                ds = DataBaseHandler.SelectQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet Selecttb_RandomiserMention(string userName, string tweetUserName, string TweetMessage)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT  * FROM tb_RandomiserMention WHERE (LoginUserName = '" + userName + "' and TweetUserName = '" + tweetUserName + "' and TweetMessage = '" + TweetMessage + "' and TableStatus='0')";
                ds = DataBaseHandler.SelectQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
            return ds;
        }
       

        public DataSet SelectAllfromtb_RandomiserMention()
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT  * FROM tb_RandomiserMention WHERE (TableStatus='0')";
                ds = DataBaseHandler.SelectQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
            return ds;
        }

        public void Updatetb_Randomiser()
        {
            try
            {
                string strQuery = "update tb_Randomiser set TableStatus='1'";
                DataBaseHandler.UpdateQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        public void DeleteAllfromtb_Randomiser()
        {
            try
            {
                string strQuery = "Delete from tb_Randomiser";
                DataBaseHandler.DeleteQuery(strQuery, "tb_Randomiser");
            }
            catch
            {
            }
        }

        public void DeleteAllfromtb_RandomiserMention()
        {
            try
            {
                string strQuery = "Delete from tb_RandomiserMention";
                DataBaseHandler.DeleteQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
        }

        public DataSet Selecttb_MentionReplyCompaign(string userName)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT TweetUserName,ReplyUserName FROM tb_MentionReplyCampaign WHERE (UserName = '" + userName + "') AND (Status = '0') AND (RandomiserMentionStatus = '0')";
                ds = DataBaseHandler.SelectQuery(strQuery, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet Selecttb_RandomiserMention(string userName)
        {
            DataSet ds = new DataSet();
            try
            {
                string strQuery = "SELECT TweetUserName FROM tb_RandomiserMention WHERE (LoginUserName = '" + userName + "') AND (TableStatus = '0') AND (MentionStatus = '0')";
                ds = DataBaseHandler.SelectQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
            return ds;
        }

        public void Updatetb_MentionReplyCampaign(string userName, string replyUserName)
        {
            try
            {
                string strQuery = "UPDATE tb_MentionReplyCampaign SET RandomiserMentionStatus = '1' WHERE        (UserName = '" + userName + "') AND (ReplyUserName = '" + replyUserName + "' OR TweetUserName='" + replyUserName + "')";
                DataBaseHandler.UpdateQuery(strQuery, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
        }

        public void Updatetb_RandomiserMention(string userName, string tweetUserName)
        {
            try
            {
                string strQuery = "UPDATE tb_RandomiserMention SET MentionStatus = '1' WHERE        (LoginUserName = '" + userName + "') AND (TweetUserName='" + tweetUserName + "')";
                DataBaseHandler.UpdateQuery(strQuery, "tb_RandomiserMention");
            }
            catch
            {
            }
        }

    }
}
