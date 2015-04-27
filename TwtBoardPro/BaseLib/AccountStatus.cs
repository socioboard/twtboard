using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class AccountStatus
    {

        public static string Status(ProfileStatus profileStatus)
        {
            switch (profileStatus)
            {
                
                case ProfileStatus.AccountCreated:
                    return "0";
                case ProfileStatus.Profiled:
                    return "1";
                case ProfileStatus.EmailVerified:
                    return "3";
                case ProfileStatus.IncorrectEmail:
                    return "5";
                case ProfileStatus.AccountDisabled:
                    return "404";
                default:
                    return null;
            }
        }
    }

    /// <summary>
    /// AccountCreated => New Account Created
    /// PhoneVerfiedOnly => Accounts that are either PVA or are not asked for PVA
    /// EmailVerifiedOnly => Accounts that are Email Verified but are asked for PVA
    /// </summary>
    public enum ProfileStatus { AccountCreated, Profiled,  AccountDisabled, IncorrectEmail, EmailVerified }
}

