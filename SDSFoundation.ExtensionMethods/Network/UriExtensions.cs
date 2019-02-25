using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.ExtensionMethods.Network
{
    public static class UriExtensions
    {

        //var userName = primaryUriWithCredentials.UserInfo.Split(':').First();
        //var credential = primaryUriWithCredentials.UserInfo.Split(':').Last();
        public static string GetUserName(this Uri uri)
        {
            return ParseUriUserInfo(uri, false);
        }

        public static string GetPassword(this Uri uri)
        {
            return ParseUriUserInfo(uri, true);
        }

        private static string ParseUriUserInfo(Uri uri, bool returnPassword)
        {
            var result = string.Empty;

            if (uri.UserInfo != null && string.IsNullOrEmpty(uri.UserInfo) == false)
            {
                var splitVal = uri.UserInfo.Split(':');

                if (!returnPassword)
                {
                    if (splitVal != null && splitVal.Count() > 0)
                    {
                        result = splitVal.First();
                    }
                }
                else
                {
                    if (splitVal != null && splitVal.Count() > 1)
                    {
                        result = splitVal.Last();
                    }
                }

            }

            return result;
        }
    }
}
