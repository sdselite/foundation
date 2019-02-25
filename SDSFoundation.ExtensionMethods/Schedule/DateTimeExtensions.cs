using System;

namespace SDSFoundation.ExtensionMethods.Schedule
{
    public static class DateTimeExtensions
    {

        public static DateTime FromTimeZone(this DateTime time, string timeZoneName)
        {
            if (string.IsNullOrWhiteSpace(timeZoneName) == false && timeZoneName.ToLower() != "unknown")
            {
                TimeZoneInfo tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneName);

                var adjustedTimezoneTime = TimeZoneInfo.ConvertTime(time, tzInfo);

                return adjustedTimezoneTime;
            }

            return time;
        }

        public static double GetLocalTZAdjustment(this DateTime inputTime, string timezoneName)
        {
            var adjustedTimezoneTime = inputTime.FromTimeZone(timezoneName);
            var tzDiff = Math.Round(adjustedTimezoneTime.Subtract(inputTime).TotalHours, 2);

            return tzDiff;
        }

        /// <summary>
        /// WARNING - This is for DISPLAY purposes only 
        /// </summary>
        /// <param name="inputTime"></param>
        /// <param name="timezoneName"></param>
        /// <returns></returns>
        public static DateTime GetDisplayTimeByTimezone(this DateTime inputTime, string timezoneName)
        {
            var tzAdjust = GetLocalTZAdjustment(inputTime, timezoneName);
            var localtimeOffset = GetLocalTZAdjustment(inputTime.ToLocalTime(), timezoneName) * -1;

            var result = inputTime.AddHours(tzAdjust).AddHours(localtimeOffset);

            return result;
        }


        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        }


    }
}
