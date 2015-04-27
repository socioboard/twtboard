using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;

namespace ReplyInterface
{
    public class clsDB_ReplyInterface
    {
        public void InserIntotb_ReplyCampaign(string StatusId, string PostAuthenticityToken, string UserId, string UserName, string ScreenName, string TweetUserId, string TweetUserName, string ReplyUserId, string ReplyUserName, string TweetText, string TweetDateTime, string Reply, string ReplyDateTime, string Status)
        {
            try
            {
                string strQuery = "INSERT INTO tb_ReplyCampaign(StatusId , PostAuthenticityToken , UserId , UserName , ScreenName , TweetUserId , TweetUserName , ReplyUserId, ReplyUserName, TweetText,TweetDateTime, Reply,ReplyDateTime, Status) VALUES ('" + StatusId + "' , '" + PostAuthenticityToken + "' , '" + UserId + "' , '" + UserName + "' , '" + ScreenName + "' , '" + TweetUserId + "' , '" + TweetUserName + "' , '" + ReplyUserId + "' , '" + ReplyUserName + "' , '" + TweetText + "' , '" + TweetDateTime + "', '" + Reply + "' , '" + ReplyDateTime + "' , '" + Status + "')";
                DataBaseHandler.InsertQuery(strQuery, "tb_ReplyCampaign");

                UpdateRecord_GetNewTweet(StatusId, UserName);
            }
            catch
            {
            }
        }

        private void UpdateRecord_GetNewTweet(string statusId, string UserName)
        {
            try
            {
                string query = "Update tb_ReplyCampaign set Status='0' where StatusId='" + statusId + "' and UserName='" + UserName + "'";
                DataBaseHandler.UpdateQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
        }

        public DataSet SelectFromtb_ReplyCampaign()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select * from tb_ReplyCampaign where Status='0'";
                ds = DataBaseHandler.SelectQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet SelectFromtb_ReplyCampaign(string userName)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select * from tb_ReplyCampaign where Status='0' and UserName='" + userName + "'";
                ds = DataBaseHandler.SelectQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet SelectDistinctAllFromtb_ReplyCampaign()
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select StatusId,PostAuthenticityToken,UserId,UserName,ScreenName,TweetUserId,TweetUserName,ReplyUserId,ReplyUserName,TweetText,Reply,count(StatusId) as TotalStatusId from tb_ReplyCampaign where Status='0' group by StatusId";
                ds = DataBaseHandler.SelectQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet SelectTweetAndReplyFromtb_ReplyCampaign(string statusId, string UserName)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select distinct TweetText, Reply, TweetDateTime, ReplyDateTime from tb_ReplyCampaign where StatusId='" + StringEncoderDecoder.Encode(statusId) + "' and UserName='" + StringEncoderDecoder.Encode(UserName) + "' and Status='0'";
                ds = DataBaseHandler.SelectQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public DataSet SelectStatusIdUserNameTweetAndReplyFromtb_ReplyCampaign(string statusId, string UserName, string tweet, string reply)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = "Select StatusId, UserName ,TweetText, Reply from tb_ReplyCampaign where StatusId='" + StringEncoderDecoder.Encode(statusId) + "' and UserName='" + StringEncoderDecoder.Encode(UserName) + "' and TweetText='" + StringEncoderDecoder.Encode(tweet) + "' and Reply='" + StringEncoderDecoder.Encode(reply) + "'";
                ds = DataBaseHandler.SelectQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
            return ds;
        }

        public void DeleteFromtb_ReplyCampaign()
        {
            try
            {
                string query = "Update tb_ReplyCampaign set Status='1' where Status='0'";
                DataBaseHandler.UpdateQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
        }

        public void DeleteRecordsAfterReplyFromtb_ReplyCampaign(string statusId, string UserName)
        {
            try
            {
                string query = "Update tb_ReplyCampaign set Status='1' where Status='0' and StatusId='" + statusId + "' and UserName='" + UserName + "'";
                DataBaseHandler.UpdateQuery(query, "tb_ReplyCampaign");
            }
            catch
            {
            }
        }

    }
}
