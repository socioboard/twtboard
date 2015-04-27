using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BaseLib
{
    public class NumberHelper
    {
        public static bool ValidateNumber(string strInputNo)
        {
            Regex IdCheck = new Regex("^[0-9]*$");

            if (!string.IsNullOrEmpty(strInputNo) && IdCheck.IsMatch(strInputNo))
            {
                return true;
            }

            return false;
        }
    }
}
