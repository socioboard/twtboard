using System;
using System.Collections;
using System.Text;


/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */
namespace SimpleJson
{
    public class FinalToken
    {}


    public class Reader
    {
        protected string _src = null;
        protected int _length = -1;
        protected int _offset = 0;

        public string Source
        {
            get
            {
                return _src;
            }
        }

        public bool Eol
        {
            get
            {
                return _length <= _offset;
            }
        }


        public override string ToString()
        {
            return this._src;
        }


        protected static bool IsNumeric(char c, bool isFirst)
        {
            return ('-' == c || Char.IsDigit(c)) ||
                   (!isFirst && ('.' == c || 'e' == c || 'E' == c));
        }

        protected static bool IsNumeric(char c)
        {
            return Reader.IsNumeric(c, false);
        }

        protected static bool IsWhitespace(char c)
        {
            return ' ' == c || '\t' == c || '\r' == c || '\n' == c;
        }

        protected static bool IsSeparator(char c)
        {
            return ',' == c || ':' == c;
        }


        protected object ReadNumber()
        {
            StringBuilder sb = new StringBuilder();
            do {
                sb.Append(this._src[this._offset++]);
            } while (!this.Eol && Reader.IsNumeric(this._src[this._offset]));
            string num = sb.ToString();
            return (num.Contains(".") || num.Contains("e") || num.Contains("E"))
                ? Double.Parse(num.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator),
                               System.Globalization.NumberStyles.Float)
                : Int32.Parse(num,
                              System.Globalization.NumberStyles.Integer);
        }

        protected string ReadString()
        {
            char c;
            StringBuilder sb = new StringBuilder();
            while (!this.Eol) {
                this._offset++;
                c = this._src[this._offset];
                if ('"' == c) {
                    this._offset++;
                    break;
                } else if ('\\' == c) {
                    this._offset++;
                    c = this._src[this._offset];
                    switch (c) {
                    case 'b':
                        c = '\b';
                        break;
                    case 'f':
                        c = '\f';
                        break;
                    case 'n':
                        c = '\n';
                        break;
                    case 'r':
                        c = '\r';
                        break;
                    case 't':
                        c = '\t';
                        break;
                    case 'u':
                        c = Convert.ToChar(Int32.Parse(this._src.Substring(this._offset + 1, 4),
                                                       System.Globalization.NumberStyles.AllowHexSpecifier));
                        this._offset += 4;
                        break;
                    }
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected IList ReadList()
        {
            ArrayList list = new ArrayList();
            while (!this.Eol) {
                object item = this.ReadToken();
                if (item is FinalToken) {
                    break;
                } else {
                    list.Add(item);
                }
            }
            return list;
        }

        protected IDictionary ReadDictionary()
        {
            Hashtable dict = new Hashtable();
            while (!this.Eol) {
                object key = this.ReadToken();
                if (key is FinalToken) {
                    break;
                } else {
                    dict.Add((string)key, this.ReadToken());
                }
            }
            return dict;
        }

        protected object ReadToken()
        {
            char c;
            while (!this.Eol) {
                c = this._src[this._offset];
                if (Reader.IsWhitespace(c) || Reader.IsSeparator(c)) {
                    this._offset++;
                    continue;
                } else if ('n' == c) {
                    this._offset += 4;
                    return null;
                } else if ('t' == c) {
                    this._offset += 4;
                    return true;
                } else if ('f' == c) {
                    this._offset += 5;
                    return false;
                } else if (Reader.IsNumeric(c, true)) {
                    return this.ReadNumber();
                } else if ('"' == c) {
                    return this.ReadString();
                } else if ('[' == c) {
                    this._offset++;
                    return this.ReadList();
                } else if ('{' == c) {
                    this._offset++;
                    return this.ReadDictionary();
                } else if (']' == c || '}' == c) {
                    this._offset++;
                    return new FinalToken();
                } else {
                    throw new FormatException();
                }
            }
            return null;
        }


        public Reader(string src)
        {
            this._src = src;
            this._length = (null != src)
                ? src.Length
                : -1;
        }


        public ICollection Read()
        {
            this._offset = 0;
            return (ICollection)this.ReadToken();
        }

        public static ICollection Read(string src)
        {
            return (new Reader(src)).Read();
        }
    }
}
