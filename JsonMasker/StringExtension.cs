using System;
using System.Collections.Generic;
using System.Text;

namespace JsonMasker
{
    public static class StringExtension
    {
        public static string GetLast(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }

        public static string GetFirst(this string source, int length)
        {
            if (length >= source.Length)
                return source;
            return source.Substring(0, length);
        }
    }
}
