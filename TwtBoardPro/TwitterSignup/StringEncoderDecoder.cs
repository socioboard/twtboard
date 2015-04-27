using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Events
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
    }
}
