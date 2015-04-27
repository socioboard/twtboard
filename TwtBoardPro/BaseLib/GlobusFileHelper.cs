using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

namespace Globussoft
{
    public static class GlobusFileHelper
    {
        
        public static String ReadStringFromTextfile(string filepath)
        {
            StreamReader reader = new StreamReader(filepath);
            string fileText=reader.ReadToEnd();
            reader.Close();
            return fileText;
        }

        private static int _bufferSize = 16384;

        public static List<string> ReadLargeFile(string filename)
        {
            List<string> listFileContent = new List<string>();

            StringBuilder stringBuilder = new StringBuilder();
            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    char[] fileContents = new char[_bufferSize];
                    int charsRead = streamReader.Read(fileContents, 0, _bufferSize);

                    // Can't do much with 0 bytes
                    //if (charsRead == 0)
                    //    throw new Exception("File is 0 bytes");

                    while (charsRead > 0)
                    {
                        stringBuilder.Append(fileContents);
                        charsRead = streamReader.Read(fileContents, 0, _bufferSize);
                    }

                    string[] contentArray = stringBuilder.ToString().Split(new char[] { '\r', '\n' });
                    foreach (string line in contentArray)
                    {
                        //listFileContent.Add(line.Replace("#", "").Replace("\0", "").Replace("�", " "));//listFileContent.Add(line.Replace("#", ""));
                        listFileContent.Add(line.Replace("\0", "").Replace("�", " "));

                    }
                    listFileContent.RemoveAll(s => string.IsNullOrEmpty(s));
                }
            }
            return listFileContent;
        }

        public static List<string> ReadLargeFileForSpinnedMessage(string filename)
        {
            List<string> listFileContent = new List<string>();

            StringBuilder stringBuilder = new StringBuilder();
            using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    char[] fileContents = new char[_bufferSize];
                    int charsRead = streamReader.Read(fileContents, 0, _bufferSize);

                    // Can't do much with 0 bytes
                    //if (charsRead == 0)
                    //    throw new Exception("File is 0 bytes");

                    while (charsRead > 0)
                    {
                        stringBuilder.Append(fileContents);
                        charsRead = streamReader.Read(fileContents, 0, _bufferSize);
                    }

                    string[] contentArray = stringBuilder.ToString().Split(new char[] { '\r', '\n' });
                    foreach (string line in contentArray)
                    {
                        listFileContent.Add(line.Replace("\0", "").Replace("�", " "));//listFileContent.Add(line.Replace("#", ""));
                    }
                    listFileContent.RemoveAll(s => string.IsNullOrEmpty(s));
                }
            }
            return listFileContent;
        }

        public static List<string> ReadFiletoStringList(string filepath)
        {
            List<string> list = new List<string>();
            StreamReader reader = new StreamReader(filepath);
            string text = "";
            while ((text = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(text.Trim()))
                {
                    list.Add(text.Replace("�", " ").Replace("\0", "")); 
                }
            }
            reader.Close();
            return list;

        }


        public static List<string> ReadTweetFiletoStringList(string filepath)
        {
            List<string> list = new List<string>();
            StreamReader reader = new StreamReader(filepath);
            string text = "";
            while ((text = reader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(text.Trim()))
                {
                    list.Add(text.Replace("�", " ").Replace("\0", ""));
                }
            }
            reader.Close();
            return list;

        }



        public static void WriteStringToTextfile(string content,string filepath)
        {
           StreamWriter writer = new StreamWriter(filepath);
           writer.Write(content);
           writer.Close();
           
        }

        public static void WriteStringToTextfileNewLine(String content, string filepath)
        {

            StreamWriter writer = new StreamWriter(filepath);

            StringReader reader = new StringReader(content);

            while(reader.ReadLine()!=null)
            { 
                writer.WriteLine(content);
            }           

            writer.Close();
        }

        public static void AppendStringToTextfileNewLine(String content, string filepath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepath, true))
                {
                    using (StringReader reader = new StringReader(content))
                    {
                        string temptext = "";

                        while ((temptext = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(temptext);
                        }
                    }
                }
            }
            catch(Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static void AppendStringToTextfileNewLineWithCarat(String content, string filepath)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filepath, true);

                StringReader reader = new StringReader(content);

                string temptext = "";

                while ((temptext = reader.ReadLine()) != null)
                {
                    writer.WriteLine(temptext);
                }
                for (int i = 0; i < 80; i++)
                {
                    writer.Write("-");
                }
                writer.WriteLine();

                writer.Close();
            }
            catch (Exception ex)
            { 
            
            }
        }
        
        public static void WriteListtoTextfile(List<string> list,string filepath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filepath))
                {
                    foreach (string listitem in list)
                    {
                        writer.WriteLine(listitem);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        
        public static List<string> readcsvfile(string filpath)
        {
            List<string> tempdata = new List<string>();
            StreamReader sr = new StreamReader(filpath, Encoding.GetEncoding(1250));
            string strline = "";
            int x = 0;
            while (!sr.EndOfStream)
            {
                x++;
                strline = sr.ReadLine();
                tempdata.Add(strline);
            }
            sr.Close();
            return tempdata;
        }

        public static void ReplaceStringFromCsv(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new StreamReader(filePath);
            string content = reader.ReadToEnd();
            reader.Close();

            content = Regex.Replace(content, searchText + "\r\n", replaceText);

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(content);
            writer.Close();
        }

        /// <summary>
        /// Get Spinned Sentences
        /// </summary>
        /// <param name="RawComment">Format: (I/we/us) (am/are) planning to go (market/outing)...</param>
        /// <returns></returns>
        public static List<string> GetSpinnedComments(string RawComment)
        {
            #region Using Hashtable
            ///// <summary>
            ///// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            ///// </summary>
            //Hashtable commentsHashTable = new Hashtable();

            /////This is final possible cominations of comments
            //List<string> listModComments = new List<string>();

            /////Put braces data in list of string array
            //List<string[]> listDataInsideBracesArray = new List<string[]>();

            /////This Regex will fetch data within braces and put it in list of string array
            //var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
            //foreach (Match Data in regex.Matches(RawComment))
            //{
            //    string data = Data.Value.Replace("(", "").Replace(")", "");
            //    string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
            //    commentsHashTable.Add(Data, DataInsideBracesArray);
            //    listDataInsideBracesArray.Add(DataInsideBracesArray);
            //}

            //string ModifiedComment = RawComment;

            //IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

            //List<string> listModifiedComment = new List<string>();

            //listModifiedComment.Add(ModifiedComment);

            ////en.Reset();

            //string ModifiedComment1 = ModifiedComment;

            //#region Assigning Values and adding in List
            //foreach (string[] item in listDataInsideBracesArray)
            //{
            //    en.MoveNext();
            //    foreach (string modItem in listModifiedComment)
            //    {
            //        foreach (string innerItem in item)
            //        {
            //            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
            //            listModComments.Add(ModComment);
            //        }
            //    }
            //    listModifiedComment.AddRange(listModComments);
            //    //string ModComment = ModifiedComment1.Replace(en.Key, item
            //}
            //#endregion

            //List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            ////listComments.AddRange(listRequiredComments);
            //return listRequiredComments;
            #endregion

            #region Using Dictionary
            /// <summary>
            /// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            /// </summary>
            //Hashtable commentsHashTable = new Hashtable();
            Dictionary<Match, string[]> commentsHashTable = new Dictionary<Match, string[]>();

            ///This is final possible cominations of comments
            List<string> listModComments = new List<string>();

            ///Put braces data in list of string array
            List<string[]> listDataInsideBracesArray = new List<string[]>();

            ///This Regex will fetch data within braces and put it in list of string array
            var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);

            //var regex = new Regex(@"{[^{}]*}", RegexOptions.Compiled);

            foreach (Match Data in regex.Matches(RawComment))
            {
                try
                {
                    string data = Data.Value.Replace("(", "").Replace(")", "");
                    string[] DataInsideBracesArray = data.Split('|');
                    commentsHashTable.Add(Data, DataInsideBracesArray);
                    listDataInsideBracesArray.Add(DataInsideBracesArray);
                }
                catch { };
            }

            string ModifiedComment = RawComment;

            IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

            List<string> listModifiedComment = new List<string>();

            listModifiedComment.Add(ModifiedComment);

            //en.Reset();

            string ModifiedComment1 = ModifiedComment;

            #region Assigning Values and adding in List
            foreach (string[] item in listDataInsideBracesArray)
            {
                en.MoveNext();
                foreach (string modItem in listModifiedComment)
                {
                    foreach (string innerItem in item)
                    {
                        try
                        {
                            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
                            listModComments.Add(ModComment);
                        }
                        catch { };
                    }
                }

                listModifiedComment.AddRange(listModComments);
                //string ModComment = ModifiedComment1.Replace(en.Key, item
            }
            #endregion

            List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            //listComments.AddRange(listRequiredComments);
            return listRequiredComments;
            #endregion
        }

        /// <summary>
        /// Get Spinned Sentences
        /// </summary>
        /// <param name="RawComment">Format: (I/we/us) (am/are) planning to go (market/outing)...</param>
        /// <param name="RawComment">Format: "|" or "/"...</param>
        /// <returns></returns>
        public static List<string> GetSpinnedComments(string RawComment, char separator)
        {
            #region Using Hashtable
            ///// <summary>
            ///// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            ///// </summary>
            //Hashtable commentsHashTable = new Hashtable();

            /////This is final possible cominations of comments
            //List<string> listModComments = new List<string>();

            /////Put braces data in list of string array
            //List<string[]> listDataInsideBracesArray = new List<string[]>();

            /////This Regex will fetch data within braces and put it in list of string array
            //var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
            //foreach (Match Data in regex.Matches(RawComment))
            //{
            //    string data = Data.Value.Replace("(", "").Replace(")", "");
            //    string[] DataInsideBracesArray = data.Split(separator);//data.Split('/');
            //    commentsHashTable.Add(Data, DataInsideBracesArray);
            //    listDataInsideBracesArray.Add(DataInsideBracesArray);
            //}

            //string ModifiedComment = RawComment;

            //IDictionaryEnumerator en = commentsHashTable.GetEnumerator();

            //List<string> listModifiedComment = new List<string>();

            //listModifiedComment.Add(ModifiedComment);

            ////en.Reset();

            //string ModifiedComment1 = ModifiedComment;

            //#region Assigning Values and adding in List
            //foreach (string[] item in listDataInsideBracesArray)
            //{
            //    en.MoveNext();
            //    foreach (string modItem in listModifiedComment)
            //    {
            //        foreach (string innerItem in item)
            //        {
            //            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
            //            listModComments.Add(ModComment);
            //        }
            //    }
            //    listModifiedComment.AddRange(listModComments);
            //    //string ModComment = ModifiedComment1.Replace(en.Key, item
            //}
            //#endregion

            //List<string> listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));

            ////listComments.AddRange(listRequiredComments);
            //return listRequiredComments;
            #endregion

            #region Using Dictionary
            /// <summary>
            /// Hashtable that stores (DataInsideBraces) as Key and DataInsideBracesArray as Value
            /// </summary>
            //Hashtable commentsHashTable = new Hashtable();
            Dictionary<Match, string[]> commentsHashTable = new Dictionary<Match, string[]>();

            ///This is final possible cominations of comments
            List<string> listModComments = new List<string>();

            ///Put braces data in list of string array
            List<string[]> listDataInsideBracesArray = new List<string[]>();

            /// list of modified comments ...
            List<string> listModifiedComment = new List<string>();

            List<string> templistModifiedComment = new List<string>();




            try
            {
                ///This Regex will fetch data within braces and put it in list of string array
                var regex = new Regex(@"\(([^)]*)\)", RegexOptions.Compiled);
                foreach (Match Data in regex.Matches(RawComment))
                {
                    string data = Data.Value.Replace("(", "").Replace(")", "");
                    string[] DataInsideBracesArray = data.Split(separator);
                    commentsHashTable.Add(Data, DataInsideBracesArray);
                    listDataInsideBracesArray.Add(DataInsideBracesArray);
                }

                string ModifiedComment = RawComment;

                IDictionaryEnumerator en = commentsHashTable.GetEnumerator();


                listModifiedComment.Add(ModifiedComment);

                //en.Reset();
                string ModifiedComment1 = ModifiedComment;

                #region Assigning Values and adding in List
                foreach (string[] item in listDataInsideBracesArray)
                {
                    listModComments.Clear();
                    en.MoveNext();
                    foreach (string modItem in listModifiedComment)
                    {
                        foreach (string innerItem in item)
                        {
                            string ModComment = modItem.Replace(en.Key.ToString(), innerItem);
                            listModComments.Add(ModComment);

                            //if (listModComments.Count >= 100000)
                            //{
                            //    break;
                            //}
                        }
                    }
                    listModifiedComment.AddRange(listModComments);

                    //if (listModifiedComment.Count >= 1000000)
                    //{
                    //    break;
                    //}
                }
                #endregion

            }
            catch (Exception)
            {
            }
            List<string> listRequiredComments = new List<string>();
            try
            {
                listRequiredComments = listModifiedComment.FindAll(s => !s.Contains("("));
            }
            catch { };

            return listRequiredComments;
            #endregion
        }

        public static string LoadTextFileUsingOFD()
        {
            string path = string.Empty;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt";
                ofd.InitialDirectory = BaseLib.Globals.path_DesktopFolder;//Application.StartupPath;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                }
            }
            return path;
        }

    }
}
