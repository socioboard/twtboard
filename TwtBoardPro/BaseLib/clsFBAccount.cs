using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Data;
using System.Text.RegularExpressions;


namespace BaseLib
{
    public class clsFBAccount
    {
        string strUsernaem=string.Empty;
        string strPassword=string.Empty;
        string strScreen_name = string.Empty;
        string strProxiaddress =string.Empty;
        string strProxyport=string.Empty;
        string strProxyName=string.Empty;
        string strProxypassword=string.Empty;
        string strProfileStatus = string.Empty;
        
        public List<string> SelectAccouts()
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_FBAccount";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_FBAccount");

                DataTable dt = ds.Tables["tb_FBAccount"];

                foreach (DataRow row in dt.Rows)
                {

                    string str = null;
                    foreach (var item in row.ItemArray)
                    {
                        str = str + item.ToString() + ":";
                    }
                    lstAccount.Add(str);
                }

                return lstAccount;
            }
            catch (Exception)
            {

                return new List<string>();
            }
        }

        public DataTable SelectAccoutsForGridView()
        {
            try
            {
                string followerCount = "0";
                string followingCount = "0";
                int totalFollowerCount = 0;
                int totalFollowingCount = 0;
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM tb_FBAccount";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, "tb_FBAccount");

                DataTable dt = ds.Tables["tb_FBAccount"];
                foreach (DataRow DR in ds.Tables[0].Rows)
                {
                    try
                    {
                        followerCount = DR["FollowerCount"].ToString();
                        followingCount = DR["FollowingCount"].ToString();
                        if (!string.IsNullOrEmpty(followerCount))
                        {
                            if (!string.IsNullOrEmpty(followingCount))
                            {
                                string[] followerCount_Split = Regex.Split(followerCount, " ");
                                string[] follwingCount_Split = Regex.Split(followingCount, " ");

                                followerCount = followerCount_Split[0];
                                followingCount = follwingCount_Split[0];
                                if (totalFollowerCount == 0)
                                {
                                    totalFollowerCount = Convert.ToInt32(followerCount);
                                }
                                else
                                {
                                    totalFollowerCount = totalFollowerCount + Convert.ToInt32(followerCount);
                                }

                                if (totalFollowingCount == 0)
                                {
                                    totalFollowingCount = Convert.ToInt32(followingCount);
                                }
                                else
                                {
                                    totalFollowingCount = totalFollowingCount + Convert.ToInt32(followingCount);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                        
                    }
                }
                dt.Rows.Add("", "", "", Convert.ToString(totalFollowerCount), Convert.ToString(totalFollowingCount), "", "", "", "", "", "", "", "");
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public DataTable SelectAccoutsForGridView(string table)
        {
            try
            {
                List<string> lstAccount = new List<string>();
                string strQuery = "SELECT * FROM "+table+"";

                DataSet ds = DataBaseHandler.SelectQuery(strQuery, table);

                DataTable dt = ds.Tables[table];

                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public void InsertUpdateFBAccount(string Username,string password, string username , string proxiaddress ,string proxyport,string proxyName,string proxypassword,string friendcount,string profilename)
        {
            try
            {
                this.strUsernaem=Username;
                this.strPassword=password;
                this.strProxiaddress=proxiaddress;
                this.strProxyport=proxyport;
                this.strProxyName=proxyName;
                this.strProxypassword=proxypassword;
                this.strProfileStatus = "";
                this.strScreen_name = username;
                string strQuery = "INSERT INTO tb_FBAccount VALUES ('" + Username + "','" + password + "', '"+ username +"' ,'" + proxiaddress + "','" + proxyport + "','" + proxyName + "','"+proxypassword+ "','"+friendcount+ "','"+profilename+"','"+strProfileStatus+"') ";

                DataBaseHandler.InsertQuery(strQuery, "tb_FBAccount");
            }
            catch (Exception)
            {
                try
                {
                    UpdateTDAccount(strUsernaem, strPassword, strProxiaddress, strProxyport, strProxyName, strProxypassword);
                }
                catch { }
            }
        }

        public void UpdateTDAccount(string Usernaem, string password, string proxiaddress, string proxyport, string proxyName, string proxypassword)
        {
            try
            {
                string strTable = "tb_FBAccount";
                string strQuery = "UPDATE tb_FBAccount SET Password='" + password + "', ProxyAddress='" + proxiaddress + "', ProxyPort='" + proxyport + "', ProxyUserName='" + proxyName + "', ProxyPassword='" + proxypassword + "' WHERE UserName='" + Usernaem+"'";

                DataBaseHandler.UpdateQuery(strQuery, strTable);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
