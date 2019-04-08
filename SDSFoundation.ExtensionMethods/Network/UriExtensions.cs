using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SDSFoundation.ExtensionMethods.Network
{
    public static class UriExtensions
    {

        /// <summary>
        /// Provide a URL or IP Address that is comma separated
        /// http://api.ipstack.com/[URLorIP]?access_key=YOUR_ACCESS_KEY
        /// https://www.db-ip.com/api/free.php
        /// </summary>
        /// <param name="uri">Uri containing the path to the server you are resolving</param>
        /// <param name="apiAddress">the API address with [URL] as a placeholder for the domain or IP address</param>
        /// <returns></returns>
        public static string GetGeolocationData(this Uri uri, string apiAddress)
        {
            var stringToReplace = "[URL]";
            var result = string.Empty;
            var ipAddressParsed = uri.GetIP();

            apiAddress = apiAddress.Replace(stringToReplace, ipAddressParsed);
    
 
            var request = new HttpRequestMessage(HttpMethod.Get, apiAddress);

            var client = new HttpClient();
            var sendAsyncTask = client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            sendAsyncTask.Wait();

            var response = sendAsyncTask.Result;

            response.EnsureSuccessStatusCode();


            var responseContentTask = response.Content.ReadAsStringAsync();
            responseContentTask.Wait();
            return responseContentTask.Result;
        }


        public static string GetIPFromString(string url)
        {
            try
            {
                url = url.Replace("http://", ""); //remove http://
                url = url.Replace("https://", ""); //remove https://
                url = url.Substring(0, url.IndexOf("/")); //remove everything after the first /
            }
            catch (Exception)
            {
                //Do nothing
            }


            return url;
        }

        public static string GetIP(this Uri uri)
        {
            var url = GetIPFromString(uri.AbsoluteUri);

            try
            {
                IPHostEntry hosts = Dns.GetHostEntry(url);
                if (hosts.AddressList.Length > 0) return hosts.AddressList[0].ToString();
            }
            catch
            {
                //Do nothing
            }
            return uri.Host;
        }

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
