using System;
using System.Collections.Generic;
using System.Linq;

namespace Profile.UI.Extentions
{
    public static class StringExtentions
    {
        public static string TrimAndUppercaseFirst(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            else
            {
                var result = value.Trim();
                result = result.First().ToString().ToUpper() + result.Substring(1);

                return result;
            }
        }
    }
}