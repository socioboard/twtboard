/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */

using System;
using System.Collections;


namespace DeathByCaptcha
{
    /**
     * <summary>CAPTCHA details.</summary>
     */
    public class Captcha
    {
        protected int _id;

        /**
         * <value>CAPTCHA ID.</value>
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
                _text = null;
                _correct = false;
            }
        }

        /**
         * <value>true if the CAPTCHA is uploaded.</value>
         */
        public bool Uploaded
        {
            get
            {
                return 0 < _id;
            }
        }


        protected string _text;

        /**
         * <value>CAPTCHA text, null if not solved.</value>
         */
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = (null != value && !value.Equals(String.Empty))
                    ? value
                    : null;
            }
        }

        /**
         * <value>true if the CAPTCHA was solved.</value>
         */
        public bool Solved
        {
            get
            {
                return Uploaded && null != _text;
            }
        }


        protected bool _correct;

        /**
         * <value>Correctness state -- true if the CAPTCHA was solved correctly.</value>
         */
        public bool Correct
        {
            get
            {
                return Solved && _correct;
            }
        }


        /**
         * <summary>Instantiate an empty non-uploaded CAPTCHA.</summary>
         */
        public Captcha()
        {
            this.Id = 0;
        }

        /**
         * <summary>Instantiate a CAPTCHA and populate its properties from API response.</summary>
         * <param name="src">API response.</param>
         */
        public Captcha(Hashtable src) : this()
        {
            if (null != src && src.ContainsKey("captcha") && null != src["captcha"]) {
                try {
                    this.Id = Convert.ToInt32(src["captcha"]);
                } catch (System.Exception) {
                    //
                }
                if (this.Uploaded) {
                    try {
                        this.Text = (string)src["text"];
                    } catch (System.Exception) {
                        //
                    }
                    if (this.Solved) {
                        try {
                            this._correct = (bool)src["is_correct"];
                        } catch (System.Exception) {
                            //
                        }
                    }
                }
            }
        }


        /**
         * <summary>Represent a CAPTCHA as its numeric ID.</summary>
         */
        public int ToInt()
        {
            return this._id;
        }

        /**
         * <summary>Represent a CAPTCHA as its text.</summary>
         */
        public override string ToString()
        {
            return this._text;
        }

        /**
         * <summary>Represent a CAPTCHA as its correctness state.</summary>
         */
        public bool ToBoolean()
        {
            return this.Correct;
        }
    }
}
