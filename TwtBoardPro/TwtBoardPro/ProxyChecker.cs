using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;
using System.Threading;
using System.Data.SQLite;
using Globussoft;

namespace twtboardpro
{
    public class IPChecker
    {
        string IPAddress { get; set; }
        string IPPort { get; set; }
        string IPUsername { get; set; }
        string IPpassword { get; set; }
        public int IsPublic { get; set; }

        Globussoft.GlobusHttpHelper httpGbs = new Globussoft.GlobusHttpHelper();
        public IPChecker(string IPAddress, string IPPort, string IPUsername, string IPpassword, int IsPublic)
        {
            this.IPAddress = IPAddress;
            this.IPPort = IPPort;
            this.IPUsername = IPUsername;
            this.IPpassword = IPpassword;
            this.IsPublic = IsPublic;
        }

        public bool CheckIP()
        {
            try
            {
                int Working = 0;
                string LoggedInIp = string.Empty;

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                GlobusHttpHelper HttpHelper1 = new GlobusHttpHelper();
                string pageSource = HttpHelper1.getHtmlfromUrlIPChecker(new Uri("https://twitter.com/"), IPAddress, int.Parse(IPPort), IPUsername, IPpassword);

                if (string.IsNullOrEmpty(pageSource))//(string.IsNullOrEmpty(pageSource) && string.IsNullOrEmpty(PgSrcHome))
                {
                    Thread.Sleep(500);
                    pageSource = HttpHelper1.getHtmlfromUrlIPChecker(new Uri("https://twitter.com/"), IPAddress, int.Parse(IPPort), IPUsername, IPpassword);
                    if (string.IsNullOrEmpty(pageSource))
                    {
                        return false;
                    }
                }
                ///Logic to check...
                if (pageSource.Contains("Sign in") || pageSource.Contains("Sign up") && pageSource.Contains("Twitter"))
                {
                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                Working = 1;
                                string query = "Select * from tb_IP WHERE IPAddress = '" + IPAddress + "' AND IPPort = '" + IPPort + "' ";
                                System.Data.DataSet ds = DataBaseHandler.SelectQuery(query, "tb_IP");
                                if (ds.Tables["tb_IP"].Rows.Count > 0)
                                {
                                    //UPDATE tb_IP SET Working = 1 WHERE IPAddress = '" + IPAddress + "' , IPPort = '" + IPPort + "' ";
                                    string UpdateQuery = "UPDATE tb_IP SET Working = 1 WHERE IPAddress = '" + IPAddress + "' AND IPPort = '" + IPPort + "' ";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_IP");
                                }
                                else
                                {
                                    string UpdateQuery = "INSERT INTO tb_IP VALUES ('" + IPAddress + "', '" + IPPort + "', '" + "" + "', '" + "" + "' , 1 , 0 , '" + "" + "' ) ";
                                    DataBaseHandler.InsertQuery(UpdateQuery, "tb_IP");
                                }

                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(IPAddress + ":" + IPPort, Globals.Path_ExsistingProxies);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckIP() 1 --> " + ex.Message, Globals.Path_IPSettingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckIP() 1 --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckIP() 2 --> " + ex.Message, Globals.Path_IPSettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckIP() 2--> " + ex.Message, Globals.Path_TwtErrorLogs);
                return false;
            }
        }

        public bool CheckPvtIP()
        {
            try
            {
                int Working = 0;
                string LoggedInIp = string.Empty;

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                GlobusHttpHelper HttpHelper1 = new GlobusHttpHelper();
                //string pageSource = HttpHelper.getHtmlfromUrlIPChecker("https://twitter.com/", IPAddress, IPPort, IPUsername, IPpassword);
                string pageSource = HttpHelper1.getHtmlfromUrlIPChecker(new Uri("https://twitter.com/"), IPAddress, int.Parse(IPPort), IPUsername, IPpassword);
                if (string.IsNullOrEmpty(pageSource))//(string.IsNullOrEmpty(pageSource) && string.IsNullOrEmpty(PgSrcHome))
                {
                    Thread.Sleep(500);
                    pageSource = HttpHelper1.getHtmlfromUrlIPChecker(new Uri("https://twitter.com/"), IPAddress, int.Parse(IPPort), IPUsername, IPpassword);
                    if (string.IsNullOrEmpty(pageSource))
                    {
                        return false;
                    }
                }
                ///Logic to check...
                //if (pageSource.Contains("class=\"signin\"") && pageSource.Contains("class=\"signup\"") && pageSource.Contains("Twitter"))
                if (pageSource.Contains("Sign in") || pageSource.Contains("Sign up") && pageSource.Contains("Twitter"))
                {
                    try
                    {
                        using (SQLiteConnection con = new SQLiteConnection(DataBaseHandler.CONstr))
                        {
                            using (SQLiteDataAdapter ad = new SQLiteDataAdapter())
                            {
                                Working = 1;
                                string query = "Select * from tb_IP WHERE IPAddress = '" + IPAddress + "' AND IPPort = '" + IPPort + "' AND  IPUsername = '" + IPUsername + "' AND IPpassword = '"+  IPpassword +"' ";
                                System.Data.DataSet ds = DataBaseHandler.SelectQuery(query, "tb_IP");
                                if (ds.Tables["tb_IP"].Rows.Count > 0)
                                {
                                    string UpdateQuery = "UPDATE tb_IP SET Working = 1 WHERE IPAddress = '" + IPAddress + "' AND IPPort = '" + IPPort + "'  AND  IPUsername = '" + IPUsername + "' AND IPpassword = '" + IPpassword + "' ";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_IP");
                                }
                                else
                                {
                                    string UpdateQuery = "INSERT INTO tb_IP VALUES ('" + IPAddress + "', '" + IPPort + "', '" + IPUsername + "','" + IPpassword  + "', 1 , 1 , '" + "" + "' ) ";
                                    DataBaseHandler.InsertQuery(UpdateQuery, "tb_IP");
                                }

                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(IPAddress + ":" + IPPort + ":" + IPUsername + ":" +IPpassword, Globals.Path_ExsistingPvtProxies);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckPvtIP() 1 --> " + ex.Message, Globals.Path_IPSettingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckPvtIP() 1 --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckPvtIP() 2 --> " + ex.Message, Globals.Path_IPSettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckPvtIP() 2--> " + ex.Message, Globals.Path_TwtErrorLogs);
                return false;
            }
        }

    }
}
