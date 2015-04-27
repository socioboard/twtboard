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
    public class ProxyChecker
    {
        string proxyAddress { get; set; }
        string proxyPort { get; set; }
        string proxyUsername { get; set; }
        string proxyPassword { get; set; }
        public int IsPublic { get; set; }

        Globussoft.GlobusHttpHelper httpGbs = new Globussoft.GlobusHttpHelper();
        public ProxyChecker(string proxyAddress, string proxyPort, string proxyUsername, string proxyPassword, int IsPublic)
        {
            this.proxyAddress = proxyAddress;
            this.proxyPort = proxyPort;
            this.proxyUsername = proxyUsername;
            this.proxyPassword = proxyPassword;
            this.IsPublic = IsPublic;
        }

        public bool CheckProxy()
        {
            try
            {
                int Working = 0;
                string LoggedInIp = string.Empty;

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                GlobusHttpHelper HttpHelper1 = new GlobusHttpHelper();
                string pageSource = HttpHelper1.getHtmlfromUrlProxyChecker(new Uri("https://twitter.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);

                if (string.IsNullOrEmpty(pageSource))//(string.IsNullOrEmpty(pageSource) && string.IsNullOrEmpty(PgSrcHome))
                {
                    Thread.Sleep(500);
                    pageSource = HttpHelper1.getHtmlfromUrlProxyChecker(new Uri("https://twitter.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);
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
                                string query = "Select * from tb_Proxies WHERE ProxyAddress = '" + proxyAddress + "' AND ProxyPort = '" + proxyPort + "' ";
                                System.Data.DataSet ds = DataBaseHandler.SelectQuery(query, "tb_Proxies");
                                if (ds.Tables["tb_Proxies"].Rows.Count > 0)
                                {
                                    //UPDATE tb_Proxies SET Working = 1 WHERE ProxyAddress = '" + proxyAddress + "' , ProxyPort = '" + proxyPort + "' ";
                                    string UpdateQuery = "UPDATE tb_Proxies SET Working = 1 WHERE ProxyAddress = '" + proxyAddress + "' AND ProxyPort = '" + proxyPort + "' ";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_Proxies");
                                }
                                else
                                {
                                    string UpdateQuery = "INSERT INTO tb_Proxies VALUES ('" + proxyAddress + "', '" + proxyPort + "', '" + "" + "', '" + "" + "' , 1 , 0 , '" + "" + "' ) ";
                                    DataBaseHandler.InsertQuery(UpdateQuery, "tb_Proxies");
                                }

                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(proxyAddress + ":" + proxyPort, Globals.Path_ExsistingProxies);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckProxy() 1 --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckProxy() 1 --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckProxy() 2 --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckProxy() 2--> " + ex.Message, Globals.Path_TwtErrorLogs);
                return false;
            }
        }

        public bool CheckPvtProxy()
        {
            try
            {
                int Working = 0;
                string LoggedInIp = string.Empty;

                ChilkatHttpHelpr HttpHelper = new ChilkatHttpHelpr();
                GlobusHttpHelper HttpHelper1 = new GlobusHttpHelper();
                //string pageSource = HttpHelper.getHtmlfromUrlProxyChecker("https://twitter.com/", proxyAddress, proxyPort, proxyUsername, proxyPassword);
                string pageSource = HttpHelper1.getHtmlfromUrlProxyChecker(new Uri("https://twitter.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);
                if (string.IsNullOrEmpty(pageSource))//(string.IsNullOrEmpty(pageSource) && string.IsNullOrEmpty(PgSrcHome))
                {
                    Thread.Sleep(500);
                    pageSource = HttpHelper1.getHtmlfromUrlProxyChecker(new Uri("https://twitter.com/"), proxyAddress, int.Parse(proxyPort), proxyUsername, proxyPassword);
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
                                string query = "Select * from tb_Proxies WHERE ProxyAddress = '" + proxyAddress + "' AND ProxyPort = '" + proxyPort + "' AND  ProxyUsername = '" + proxyUsername + "' AND ProxyPassword = '"+  proxyPassword +"' ";
                                System.Data.DataSet ds = DataBaseHandler.SelectQuery(query, "tb_Proxies");
                                if (ds.Tables["tb_Proxies"].Rows.Count > 0)
                                {
                                    string UpdateQuery = "UPDATE tb_Proxies SET Working = 1 WHERE ProxyAddress = '" + proxyAddress + "' AND ProxyPort = '" + proxyPort + "'  AND  ProxyUsername = '" + proxyUsername + "' AND ProxyPassword = '" + proxyPassword + "' ";
                                    DataBaseHandler.UpdateQuery(UpdateQuery, "tb_Proxies");
                                }
                                else
                                {
                                    string UpdateQuery = "INSERT INTO tb_Proxies VALUES ('" + proxyAddress + "', '" + proxyPort + "', '" + proxyUsername + "','" + proxyPassword  + "', 1 , 1 , '" + "" + "' ) ";
                                    DataBaseHandler.InsertQuery(UpdateQuery, "tb_Proxies");
                                }

                                Globussoft.GlobusFileHelper.AppendStringToTextfileNewLine(proxyAddress + ":" + proxyPort + ":" + proxyUsername + ":" +proxyPassword, Globals.Path_ExsistingPvtProxies);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckPvtProxy() 1 --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                        GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckPvtProxy() 1 --> " + ex.Message, Globals.Path_TwtErrorLogs);
                    }
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(DateTime.Now + " --> Error --> CheckPvtProxy() 2 --> " + ex.Message, Globals.Path_ProxySettingErroLog);
                GlobusFileHelper.AppendStringToTextfileNewLine("Error --> CheckPvtProxy() 2--> " + ex.Message, Globals.Path_TwtErrorLogs);
                return false;
            }
        }

    }
}
