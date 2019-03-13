using System;
using System.Net;

namespace SDSFoundation.Security.OpenIdDict.Flows.Password
{
    public class PasswordFlowCredentials
    {

        public PasswordFlowCredentials(string tenantId, string clientId, string clientSecret, string email, string password, string siteId, string deviceId, string ipAddress)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Email = email;
            Password = password;
            SiteId = siteId;
            DeviceId = deviceId;
            IPAddress = ipAddress;
            TenantId = tenantId;
        }

        public PasswordFlowCredentials(string tenantId, string clientId, string clientSecret, string email, string password, string siteId, string deviceId)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Email = email;
            Password = password;
            SiteId = siteId;
            DeviceId = deviceId;
            TenantId = tenantId;
        }


        /// <summary>
        /// Populates the IPAddress property with the machine IP address.   
        /// </summary>
        /// <returns></returns>
        public string SyncIPAddress()
        {
            var ipAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipAddress = Convert.ToString(IP);
                }
            }

            this.IPAddress = ipAddress;
            return ipAddress;
        }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SiteId { get; set; }
        public string TenantId { get; set; }

        public string DeviceId { get; set; }

        public string IPAddress { get; set; }
    }
}
