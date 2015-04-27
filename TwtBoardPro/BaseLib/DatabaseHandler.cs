using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using Finisar.SQLite;
using System.Data.SQLite;
using System.Windows.Forms;

namespace BaseLib
{
    public class DataBaseHandler
    {
       
        public static string CONstr = "Data Source=" + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\twtboardpro\\DB_twtboardpro.db" + ";Version=3;";

        public static DataSet SelectQuery(string query, string tablename)
        {
            
                DataSet DS = new DataSet();
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    AD.Fill(DS, tablename);

                }
                return DS;
            //}
            //catch
            //{
            //    return new DataSet();
            //}
        }

        public static void InsertQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void DeleteQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void UpdateQuery(string query, string tablename)
        {
            try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public static void InsertQuery1(string query, string tablename)
        {
            //try
            {
                using (SQLiteConnection CON = new SQLiteConnection(CONstr))
                {
                    SQLiteCommand CMD = new SQLiteCommand(query, CON);
                    SQLiteDataAdapter AD = new SQLiteDataAdapter(CMD);
                    DataSet DS = new DataSet();
                    AD.Fill(DS, tablename);
                }
            }
            //catch (Exception ex)
            //{
               // Console.WriteLine(ex.StackTrace);
            //}
        }

        public static bool CreateUnfollowerTable()
        {
            bool _IsTbAvailable= false;
            try
            {
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
                        if (!result.Any(s => (("Unfollow").Contains(s))))
                        {
                            string CreateUnfollowerTBQuery = "CREATE TABLE 'Unfollow' ('id' INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL , 'UserName' TEXT NOT NULL , 'Unfollower_id' TEXT NOT NULL , 'Unfollower_UserName' TEXT NOT NULL , 'DateUnfollowed' TEXT NOT NULL , 'Status' VARCHAR)";
                             SQLiteCommand CMD = new SQLiteCommand(CreateUnfollowerTBQuery, CON);
                            CMD.ExecuteNonQuery();
                            _IsTbAvailable= true;
                        }
                        else
                        {
                            _IsTbAvailable= true;
                        }
                    }
                    else
                    {
                        _IsTbAvailable= false;
                    }
                }
            }
            catch (Exception)
            {
                _IsTbAvailable= false;
            }
            return _IsTbAvailable;
        }

    }
}
