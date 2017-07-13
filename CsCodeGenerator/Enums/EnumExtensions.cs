using System;
using System.Collections.Generic;

namespace CsCodeGenerator
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Convert string to Lowercase first, then replaces underscore '_' with space ' ', and  optionally adds append string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="append"></param>
        /// <returns></returns>
        public static string ToTextLower<T>(this T value, string append = "")
        {
            var result = value.ToString().ToLower().Replace("_", " ") + append;
            return result;
        }
    }
}
