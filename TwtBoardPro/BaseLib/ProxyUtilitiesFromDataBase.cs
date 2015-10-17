using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BaseLib
{
    public class IPUtilitiesFromDataBase
    {

        public List<string> GetPrivateProxies()
        {
            List<string> lst_Proxies = new List<string>();
            try
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                DataSet ds = setting.SelectPrivateIPData();
                if (ds.Tables != null && ds.Tables["tb_IP"].Rows.Count > 0)
                {
                    foreach (DataRow dRow in ds.Tables["tb_IP"].Rows)
                    {
                        string IP = dRow.ItemArray[0].ToString() + ":" + dRow.ItemArray[1].ToString() + ":" + dRow.ItemArray[2].ToString() + ":" + dRow.ItemArray[3].ToString();
                        lst_Proxies.Add(IP);
                    }
                }
            }
            catch { }

            return lst_Proxies;
        }

        public List<string> GetPublicProxies()
        {
            List<string> lst_Proxies = new List<string>();
            try
            {
                clsDBQueryManager setting = new clsDBQueryManager();
                DataSet ds = setting.SelectPublicIPData();
                if (ds.Tables != null && ds.Tables["tb_IP"].Rows.Count > 0)
                {
                    foreach (DataRow dRow in ds.Tables["tb_IP"].Rows)
                    {
                        string IP = dRow.ItemArray[0].ToString() + ":" + dRow.ItemArray[1].ToString() + ":" + dRow.ItemArray[2].ToString() + ":" + dRow.ItemArray[3].ToString();
                        lst_Proxies.Add(IP);
                    }
                }
            }
            catch { }

            return lst_Proxies;
        }

        int accountsPerIP = 10;  //Change this to change Number of Accounts to be set per IP
        static int i = 0;

        /// <summary>
        /// Assigns "accountsPerIP" number of proxies to accounts in Database, only picks up only those accounts where IPAddress is Null or Empty
        /// </summary>
        public void AssignProxiesToAccounts(List<string> lstProxies, int noOfAccountsPerIP)
        {
            try
            {
                DataSet ds = new DataSet();

                if (noOfAccountsPerIP > 0)
                {
                    accountsPerIP = noOfAccountsPerIP; 
                }
                else
                {
                    MessageBox.Show("You entered invalid Accounts per IP... Default value \"10\" Set");
                }

                using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                {

                    foreach (string item in lstProxies)
                    {
                        ds.Clear();

                        string account = item;

                        string IPAddress = string.Empty;
                        string IPPort = string.Empty;
                        string IPUsername = string.Empty;
                        string IPpassword = string.Empty;

                        int DataCount = account.Split(':').Length;

                        if (DataCount == 2)
                        {
                            IPAddress = account.Split(':')[0];
                            IPPort = account.Split(':')[1];
                        }
                        else if (DataCount > 2)
                        {
                            IPAddress = account.Split(':')[0];
                            IPPort = account.Split(':')[1];
                            IPUsername = account.Split(':')[2];
                            IPpassword = account.Split(':')[3];
                        }

                        //using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + IPAddress + "'", con))
                        using (SQLiteDataAdapter ad = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + IPAddress + "' and IPPort = '" + IPPort + "'", con))
                        {
                            ad.Fill(ds);
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                ds.Clear();
                                using (SQLiteDataAdapter ad1 = new SQLiteDataAdapter("SELECT * FROM tb_FBAccount WHERE IPAddress = '" + "" + "' OR IPAddress = '" + null + "'", con))
                                {
                                    ad1.Fill(ds);

                                    int count = accountsPerIP;  //Set count = accountsPerIP so that it sets max this number of accounts to each IP
                                    if (ds.Tables[0].Rows.Count < count)
                                    {
                                        count = ds.Tables[0].Rows.Count;
                                    }
                                    for (int i = 0; i < count; i++)
                                    {
                                        string UpdateQuery = "Update tb_FBAccount Set IPAddress='" + IPAddress + "', IPPort='" + IPPort + "', IPUsername='" + IPUsername + "', IPpassword='" + IPpassword + "' WHERE UserName='" + ds.Tables[0].Rows[i]["UserName"].ToString() + "'";
                                        DataBaseHandler.UpdateQuery(UpdateQuery, "tb_FBAccount");
                                    }
                                }

                            }
                        }

                    }

                }
        }
        catch(Exception ex)
        {
            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> IPUtillitesFromDB -- AssignProxiesToAccounts() --> " + ex.Message, Globals.Path_IPSettingErroLog);
            Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine("Error --> IPUtillitesFromDB -- AssignProxiesToAccounts() --> " + ex.Message, Globals.Path_TwtErrorLogs);
        }

        }

    }
}
