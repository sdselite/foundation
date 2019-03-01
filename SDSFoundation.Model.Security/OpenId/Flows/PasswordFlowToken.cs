using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Security.OpenId.Flows
{
    public class PasswordFlowToken
    {
        public PasswordFlowToken(string tenantToken, string accessToken, int expiration)
        {
            AccessToken = accessToken;
            TenantToken = tenantToken;
            Expires = DateTime.Now.AddSeconds(expiration);
        }


        public string TenantToken { get; set; }

        public string AccessToken { get; set; }


        public bool IsExpiredOrInvalid
        {
            get
            {
                //Offset for latency
                if (ValidatedOn < Expires.AddSeconds(-15) && ValidatedOn > DateTime.MinValue && string.IsNullOrWhiteSpace(TenantToken) == false && string.IsNullOrWhiteSpace(AccessToken) == false)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public DateTime ValidatedOn { get; set; }

        public DateTime Expires { get; set; }

    }
}
