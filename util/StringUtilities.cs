using System;
using System.Linq;
using System.Text;

namespace PWCrackService.util
{
    class StringUtilities
    {
        public static String Capitalize(String str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Trim().Length == 0)
            {
                return str;
            }
            var firstLetterUppercase = str.Substring(0, 1).ToUpper();
            var theRest = str.Substring(1);
            return firstLetterUppercase + theRest;
        }

        public static String Reverse(String str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Trim().Length == 0)
            {
                return str;
            }
            var reverseString = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                reverseString.Append(str.ElementAt(str.Length - 1 - i));
            }
            return reverseString.ToString();
        }
    }
}