using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Globussoft;
using System.Text.RegularExpressions;

namespace BaseLib
{
    public class SpinnedListGenerator
    {
        public static List<string> GetSpinnedList(List<string> inputList)
        {
            List<string> tempList = new List<string>();
            foreach (string item in inputList)
            {
                tempList.Add(item);
            }
            inputList.Clear();
            foreach (string item in tempList)
            {
                List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item);
                inputList.AddRange(tempSpunList);
            }
            return inputList;
        }

        public static List<string> GetSpinnedList(List<string> inputList, char separator)
        {
            List<string> tempList = new List<string>();
           
            foreach (string item in inputList)
            {
                tempList.Add(item);
            }
            inputList.Clear();
            foreach (string item in tempList)
            {
                    
                List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item, separator);
                inputList.AddRange(tempSpunList);
                   
            }
            
            return inputList;
        }

        public static List<string> GetSpinnedListOneString(string inputstringMessage, char separator)
        {
            List<string> tempList = new List<string>();
            List<string> inputList = new List<string>();

            tempList.Add(inputstringMessage);
          
            inputList.Clear();
            foreach (string item in tempList)
            {

                List<string> tempSpunList = GlobusFileHelper.GetSpinnedComments(item, separator);
                inputList.AddRange(tempSpunList);

            }

            return inputList;
        }

        public static string spinLargeText(Random rnd, string str)
        {
            // Loop over string until all patterns exhausted.
            //string pattern = "{[^{}]*}";
            string pattern = @"\(([^)]*)\)";
            Match m = Regex.Match(str, pattern);
            while (m.Success)
            {
                // Get random choice and replace pattern match.
                string seg = str.Substring(m.Index + 1, m.Length - 2);
                string[] choices = seg.Split('|');
                str = str.Substring(0, m.Index) + choices[rnd.Next(choices.Length)] + str.Substring(m.Index + m.Length);
                m = Regex.Match(str, pattern);
            }

            // Return the modified string.
            return str;
        }

    }
}
