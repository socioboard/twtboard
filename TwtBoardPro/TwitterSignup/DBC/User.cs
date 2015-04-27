/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

using System;
using System.Collections;
using System.Globalization;


namespace DeathByCaptcha
{
    /**
     * <summary>Death by Captcha account details.</summary>
     */
    public class User
    {
        protected double _balance = 0.0;


        protected int _id = 0;

        /**
         * <value>DBC account ID.</value>
         */
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = Math.Max(0, value);
                _balance = 0.0;
                _banned = false;
            }
        }

        /**
         * <value>Flag whether the account is of logged in user.</value>
         */
        public bool LoggedIn
        {
            get
            {
                return 0 < _id;
            }
        }

        /**
         * <value>Account balance (in US cents).</value>
         */
        public double Balance
        {
            get
            {
                return _balance;
            }
        }


        protected bool _banned = false;

        /**
         * <value>Flag whether the account is banned.</value>
         */
        public bool Banned
        {
            get
            {
                return LoggedIn && _banned;
            }
        }


        /**
         * <summary>Instantiate a DBC account details object.</summary>
         */
        public User()
        {}

        /**
         * <summary>Instantiate a DBC account details object populated from API response.</summary>
         * <param name="src">API response.</param>
         */
        public User(Hashtable src) : this()
        {
            if (null != src && src.ContainsKey("user")) {
                try {
                    this.Id = Convert.ToInt32(src["user"]);
                } catch (System.Exception) {
                    //
                }
                if (this.LoggedIn) {
                    try {
                        this._balance = Convert.ToDouble(src["balance"]);
                    } catch (System.Exception) {
                        //
                    }
                    try {
                        this._banned = Convert.ToBoolean(src["is_banned"]);
                    } catch (System.Exception) {
                        //
                    }
                }
            }
        }


        /**
         * <summary>Represent a DBC account as numeric ID.</summary>
         */
        public int ToInt()
        {
            return this._id;
        }

        /**
         * <summary>Represent a DBC account as a flag indicating if the account is logged in and not banned.</summary>
         */
        public bool ToBoolean()
        {
            return this.LoggedIn && !this.Banned;
        }
    }
}
