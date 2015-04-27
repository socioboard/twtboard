using System;
using System.Collections;
using System.Text;


/**
 * @author Sergey Kolchin <ksa242@gmail.com>
 */
namespace SimpleJson
{
    public class Writer
    {
        public static string WriteBoolean(bool b)
        {
            return b ? "true" : "false";
        }

        public static string WriteInteger(int i)
        {
            return i.ToString();
        }

        public static string WriteDouble(double i)
        {
            return i.ToString().Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
        }

        public static string WriteString(string str)
        {
            return "\"" + str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }

        public static string WriteList(IList list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            bool isFirst = true;
            foreach (object item in list) {
                if (isFirst) {
                    isFirst = false;
                } else {
                    sb.Append(",");
                }
                sb.Append(Writer.Write(item));
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string WriteDictionary(IDictionary dict)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            bool isFirst = true;
            foreach (DictionaryEntry e in dict) {
                if (isFirst) {
                    isFirst = false;
                } else {
                    sb.Append(",");
                }
                sb.Append(Writer.Write((string)e.Key));
                sb.Append(":");
                sb.Append(Writer.Write(e.Value));
            }
            sb.Append("}");
            return sb.ToString();
        }

        public static string Write(object o)
        {
            if (null == o) {
                return "null";
            } else if (o is bool) {
                return Writer.WriteBoolean((bool)o);
            } else if (o is int) {
                return Writer.WriteInteger((int)o);
            } else if (o is double || o is float) {
                return Writer.WriteDouble((double)o);
            } else if (o is IList) {
                return Writer.WriteList((IList)o);
            } else if (o is IDictionary) {
                return Writer.WriteDictionary((IDictionary)o);
            } else {
                return Writer.WriteString((string)o);
            }
        }
    }
}
