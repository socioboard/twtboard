using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BaseLib;

namespace TwitterSignup
{
    class ClsDB_TwitterSignup
    {
        public DataTable SelectFollowDataAccordingDate(string useremail, string date)
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


    }
}
