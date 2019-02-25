using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers
{
    public static class GeovisionExtensionMethods
    {
        public static bool IsMidnight(this DateTime dateTime, bool ignoreMilliSeconds = false)
        {
            if (dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0 && (ignoreMilliSeconds || (!ignoreMilliSeconds && dateTime.Millisecond == 0)))
            {
                return true;
            }

            return false;
        }
        public static string ToIPDateTime(this DateTime dateTime)
        {
            GeovisionHelperMethods helperMethods = new GeovisionHelperMethods();
            var result = helperMethods.GetIPDateTime(dateTime);

            return result;
        }

        /// <summary>
        /// The time provided here is already a DateTime format, but a string datatype and must be converted to a valid DateTime and then to IP Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToIPDateTimeFromDateTimeString(this string dateTime)
        {
            GeovisionHelperMethods helperMethods = new GeovisionHelperMethods();
            var canParseDateTime = DateTime.TryParse(dateTime, out DateTime parsedTime);

            if (canParseDateTime)
            {
                return parsedTime.ToIPDateTime();
            }
            else
            {
                throw new InvalidCastException("The DateTime provided is not a valid DateTime and cannot be parsed as an IP DateTime");
            }


        }


        public static DateTime FromIPDateTime(this string dateTimeStr)
        {
            DateTime result = new DateTime();

            if (string.IsNullOrWhiteSpace(dateTimeStr) == false)
            {
                if (dateTimeStr == "00000000000000000")
                {
                    result = DateTime.MinValue.Date;
                }
                else
                {
                    GeovisionHelperMethods helperMethods = new GeovisionHelperMethods();
                    result = helperMethods.ParseIPDateTime(dateTimeStr);
                }
            }

            return result;
        }

        public static String TryGetStringValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            TValue result;

            if (dictionary.TryGetValue(key, out result))
            {
                return result.ToString();
            }
            else
            {
                return string.Empty;
            }


        }
    }
}
