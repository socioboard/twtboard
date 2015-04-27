using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseLib
{
    public class RandomStringGenerator
    {
        private const string _chars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";

        private const string _Numbers = "0123456789";

        private const string _Both = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";

        public static string RandomString(int size)
        {
            Random _rng = new Random();

            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public static string RandomNumber(int size)
        {
            Random _rng = new Random();

            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _Numbers[_rng.Next(_Numbers.Length)];
            }
            return new string(buffer);
        }


        public static string RandomStringAndNumber(int size)
        {
            Random _rng = new Random();

            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _Both[_rng.Next(_rng.Next(0, size), _Both.Length)];
            }
            return new string(buffer);
        }

    }
}
