using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;

namespace MentionsReplyInterface
{
    public class clsDB_MentionsReplyInterface
    {
        #region InserIntotb_MentionReplyCampaign
        public void InserIntotb_MentionReplyCampaign(string StatusId, string PostAuthenticityToken, string UserId, string UserName, string ScreenName, string TweetUserId, string TweetUserName, string ReplyUserId, string ReplyUserName, string TweetText, string TweetDateTime, string Reply, string ReplyDateTime, string Status)
        {
            try
            {
                string strQuery = "INSERT INTO tb_MentionReplyCampaign(StatusId , PostAuthenticityToken , UserId , UserName , ScreenName , TweetUserId , TweetUserName , ReplyUserId, ReplyUserName, TweetText,TweetDateTime, Reply,ReplyDateTime, Status) VALUES ('" + StatusId + "' , '" + PostAuthenticityToken + "' , '" + UserId + "' , '" + UserName + "' , '" + ScreenName + "' , '" + TweetUserId + "' , '" + TweetUserName + "' , '" + ReplyUserId + "' , '" + ReplyUserName + "' , '" + TweetText + "' , '" + TweetDateTime + "', '" + Reply + "' , '" + ReplyDateTime + "' , '" + Status + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_MentionReplyCampaign");
                UpdateRecord_GetNewTweet(StatusId, UserName);
            }
            catch
            {
            }
        } 
        #endregion

        #region UpdateRecord_GetNewTweet
        private void UpdateRecord_GetNewTweet(string statusId, string UserName)
        {
            try
            {
                string query = "Update tb_MentionReplyCampaign set Status='0' where StatusId='" + statusId + "' and UserName='" + UserName + "'";
                DataBaseHandler.UpdateQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
        } 
        #endregion

        #region SelectFromtb_MentionReplyCampaign
        public DataSet SelectFromtb_MentionReplyCampaign()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select * from tb_MentionReplyCampaign where Status='0'";
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectFromtb_MentionReplyCampaign
        public DataSet SelectFromtb_MentionReplyCampaign(string userName)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select * from tb_MentionReplyCampaign where Status='0' and UserName='" + userName + "'";
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectDistinctAllFromtb_MentionReplyCampaign
        public DataSet SelectDistinctAllFromtb_MentionReplyCampaign()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT StatusId, PostAuthenticityToken, UserId, UserName, ScreenName, TweetUserId, TweetUserName, ReplyUserId, ReplyUserName, TweetText, Reply, COUNT(StatusId) AS TotalStatusId FROM tb_MentionReplyCampaign WHere (Status = '0' AND (ReplyUserName <> '')) GROUP BY StatusId";// AND (ReplyUserName <> ''
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectDistinctStatusIdFromtb_MentionReplyCampaign
        public DataSet SelectDistinctStatusIdFromtb_MentionReplyCampaign()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT distinct StatusId FROM tb_MentionReplyCampaign WHere (Status = '0')";// AND (ReplyUserName <> ''
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectDistinctStatusIdFromtb_MentionReplyCampaign
        public DataSet SelectDistinctStatusIdFromtb_MentionReplyCampaign(string statusId)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "SELECT StatusId, PostAuthenticityToken, UserId, UserName, ScreenName, TweetUserId, TweetUserName, ReplyUserId, ReplyUserName, TweetText, Reply, COUNT(StatusId) AS TotalStatusId FROM tb_MentionReplyCampaign WHere (Status = '0' and StatusId='" + statusId + "')";// AND (ReplyUserName <> ''
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectTweetAndReplyFromtb_MentionReplyCampaign
        public DataSet SelectTweetAndReplyFromtb_MentionReplyCampaign(string statusId, string UserName)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select distinct TweetText, Reply, TweetDateTime, ReplyDateTime from tb_MentionReplyCampaign where StatusId='" + StringEncoderDecoder.Encode(statusId) + "' and UserName='" + StringEncoderDecoder.Encode(UserName) + "' and Status='0'";
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region SelectStatusIdUserNameTweetAndReplyFromtb_MentionReplyCampaign
        public DataSet SelectStatusIdUserNameTweetAndReplyFromtb_MentionReplyCampaign(string statusId, string UserName, string tweet, string reply)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select StatusId, UserName ,TweetText, Reply from tb_MentionReplyCampaign where StatusId='" + StringEncoderDecoder.Encode(statusId) + "' and UserName='" + StringEncoderDecoder.Encode(UserName) + "' and TweetText='" + StringEncoderDecoder.Encode(tweet) + "' and Reply='" + StringEncoderDecoder.Encode(reply) + "'";
                ds = DataBaseHandler.SelectQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
            return ds;
        } 
        #endregion

        #region DeleteFromtb_MentionReplyCampaign
        public void DeleteFromtb_MentionReplyCampaign()
        {
            try
            {
                string query = "Update tb_MentionReplyCampaign set Status='1' where Status='0'";
                DataBaseHandler.UpdateQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
        } 
        #endregion

        #region DeleteCompletlyFromtb_MentionReplyCampaign
        public void DeleteCompletlyFromtb_MentionReplyCampaign()
        {
            try
            {
                string query = "delete from tb_MentionReplyCampaign";
                DataBaseHandler.UpdateQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
        } 
        #endregion

        #region DeleteRecordsAfterReplyFromtb_MentionReplyCampaign
        public void DeleteRecordsAfterReplyFromtb_MentionReplyCampaign(string statusId, string UserName)
        {
            try
            {
                string query = "Update tb_MentionReplyCampaign set Status='1' where Status='0' and StatusId='" + statusId + "' and UserName='" + UserName + "'";
                DataBaseHandler.UpdateQuery(query, "tb_MentionReplyCampaign");
            }
            catch
            {
            }
        } 
        #endregion
    }
}
