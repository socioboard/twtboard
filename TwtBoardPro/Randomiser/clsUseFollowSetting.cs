using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Randomiser
{
    public class clsUseFollowSetting
    {
        static clsUseFollowSetting obj_clsUseFollowSetting;

        #region Global Instance Variable
        public bool _DontFollowUsersThatUnfollowedBefore = false;
        public bool _DontFollowUsersWithNoPicture = false;
        public bool _DontFollowUsersWithManyLinks = false;
        public bool _DontFollowUsersWithFollowingsFollowersRatio = false;
        public bool _NoOFfollow = false;
        public bool _UseGroups = false;
        public int _NoOfLinks = 40;
        public int _FollowingsFollowersRatio = 80;
        public int _MaximumFollow = 10;
        public string _UseGroup = string.Empty;
  
        #endregion

        clsUseFollowSetting()
        {
        }
        public static clsUseFollowSetting GetObject()
        {
            if (obj_clsUseFollowSetting == null)
            {
                
                try
                {
                    obj_clsUseFollowSetting = new clsUseFollowSetting();

                }
                catch
                {
                }
            }
            
            return obj_clsUseFollowSetting;
        }
    }
}
