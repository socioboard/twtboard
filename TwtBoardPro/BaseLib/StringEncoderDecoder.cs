using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class StringEncoderDecoder
    {
        public static string Encode(string data)
        {
            string encoded = data.Replace("'", "^");
            return encoded;
        }
        public static string Decode(string data)
        {
            string decoded = data.Replace("^", "'");
            return decoded;
        }

        /// <Encode Base 64 long Data>
        /// Long Data string bracking in past 
        /// then thanged all data in encoded formate 
        /// finally added all encoded data and make a single string. 
        /// </summary>
        /// <param name="Base64string"></param>
        /// <returns></returns>
        public static string EncodeBase64String(string Base64string)
        {
            String value = Base64string;
            int limit = 2000;

            StringBuilder sb = new StringBuilder();
            int loops = value.Length / limit;

            for (int i = 0; i <= loops; i++)
            {
                if (i < loops)
                {
                    sb.Append(Uri.EscapeDataString(value.Substring(limit * i, limit)));
                }
                else
                {
                    sb.Append(Uri.EscapeDataString(value.Substring(limit * i)));
                }
            }
            return sb.ToString();
        }

    }
}
