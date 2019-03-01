using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Security.OpenId.Flows
{
    public class PasswordFlowCredentials
    {

        public PasswordFlowCredentials(string clientId, string clientSecret, string email, string password, string siteId, string deviceId)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Email = email;
            Password = password;
            SiteId = siteId;
            DeviceId = deviceId;
        }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SiteId { get; set; }
        public string DeviceId { get; set; }
    }
}
