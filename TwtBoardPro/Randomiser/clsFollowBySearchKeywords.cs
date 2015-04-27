using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randomiser
{
    public class clsFollowBySearchKeywords
    {
        static clsFollowBySearchKeywords obj_clsUseFollowSetting;

        #region Global Instance Variable
        public bool _Followbysinglekeywordperaccount = false;
        
        public int _NoFollowByPerAccount = 5;

        public List<string> lstKeywords = new List<string>();

        #endregion

        clsFollowBySearchKeywords()
        {
        }
        public static clsFollowBySearchKeywords GetObject()
        {
            if (obj_clsUseFollowSetting == null)
            {
                
                try
                {
                    obj_clsUseFollowSetting = new clsFollowBySearchKeywords();

                }
                catch
                {
                }
            }

            return obj_clsUseFollowSetting;
        }
    }
}
