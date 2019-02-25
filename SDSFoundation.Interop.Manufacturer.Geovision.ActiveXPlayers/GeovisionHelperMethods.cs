using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Interop.ActiveX.Manufacturer.Geovision.ActiveXPlayers
{
    public class GeovisionHelperMethods
    {
        public DateTime ParseIPDateTime(string ipDateTime)
        {
            DateTimeOffset dt;
            try
            {
                var year = ipDateTime.Substring(0, 4);
                var month = ipDateTime.Substring(4, 2);
                var day = ipDateTime.Substring(6, 2);
                var hour = ipDateTime.Substring(8, 2);
                var minute = ipDateTime.Substring(10, 2);
                var second = ipDateTime.Substring(12, 2);
                var millisecond = "000";
                try
                {
                    millisecond = ipDateTime.Substring(14, 3);
                }
                catch (Exception)
                {
                    millisecond = "000";
                }

                var dateString = String.Format("{0}-{1}-{2}T{3}:{4}:{5}.{6}-00:00", year, month, day, hour, minute, second, millisecond);
                dt = DateTimeOffset.Parse(dateString, null);

            }
            catch (Exception ex)
            {
                return new DateTime();
            }


            return dt.DateTime;
        }

        public List<DateTime> ParseDelimitedIPDateTime(string delimitedIpDateTime)
        {
            var result = new List<DateTime>();
            if (string.IsNullOrWhiteSpace(delimitedIpDateTime))
            {
                return result;
            }

            var splitList = delimitedIpDateTime.Split('#');
            foreach (var ipDateTime in splitList)
            {
                var parsedDate = ParseIPDateTime(ipDateTime);
                result.Add(parsedDate);
            }
            return result;
        }

        public string GetIPDateTime(DateTime dateTime)
        {
            var year = dateTime.Year.ToString();

            var month = dateTime.Month.ToString();
            if (month.Length == 1) month = "0" + month;

            var day = dateTime.Day.ToString();
            if (day.Length == 1) day = "0" + day;

            var hour = dateTime.Hour.ToString();
            if (hour.Length == 1) hour = "0" + hour;

            var minute = dateTime.Minute.ToString();
            if (minute.Length == 1) minute = "0" + minute;

            var second = dateTime.Second.ToString();
            if (second.Length == 1) second = "0" + second;

            var millisecond = dateTime.Millisecond.ToString();
            if (millisecond.Length == 2) millisecond = "0" + millisecond;
            if (millisecond.Length == 1) millisecond = "00" + millisecond;

            var result = string.Concat(year, month, day, hour, minute, second, millisecond);
            return result;
        }
    }
}
