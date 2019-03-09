using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Security.OpenIdDict.Flows.Password
{
    public sealed class PasswordFlowTokenCache
    {
        private static readonly Lazy<PasswordFlowTokenCache> singletonObject = new Lazy<PasswordFlowTokenCache>(() => new PasswordFlowTokenCache());
        private static List<PasswordFlowToken> tokens = new List<PasswordFlowToken>();

        private PasswordFlowTokenCache() { }

        private static List<PasswordFlowToken> Tokens { get { return tokens; } }

        public List<Claim> GetTokenClaims(string clientId, string accessToken, bool updateValidatupdateValidatedOnDate = false)
        {
            var tokenFound = GetValidAccessToken(clientId, accessToken, updateValidatupdateValidatedOnDate);

            if (tokenFound == null)
            {
                return null;
            }
            else
            {
                return tokenFound.TokenClaims;
            }

        }

        public PasswordFlowToken GetValidAccessToken(string tenantToken, string accessToken, bool updateValidatupdateValidatedOnDate = false)
        {
            PasswordFlowToken tokenFound = null;
            tokenFound = Tokens.Where(x => x.TenantToken == tenantToken && x.AccessToken == accessToken).FirstOrDefault();


            if (tokenFound != null)
            {
                if (updateValidatupdateValidatedOnDate)
                {
                    tokenFound.ValidatedOn = DateTime.Now;
                }

                //Remove the invalid or expired token if the token is not valid.
                if (tokenFound.IsExpiredOrInvalid)
                {

                    Tokens.Remove(tokenFound);
                    tokenFound = null;
                }
            }


            return tokenFound;
        }

        public void AddAccessToken(string tenantToken, string accessToken, int expiresInSeconds, List<Claim> tokenClaims)
        {
            var matchingToken = Tokens.Where(x => x.TenantToken == tenantToken && x.AccessToken == accessToken).FirstOrDefault();
            if (matchingToken == null)
            {
                //add a new token because it doesn't exist
                PasswordFlowToken newToken = new PasswordFlowToken(tenantToken, accessToken, expiresInSeconds);
                newToken.TokenClaims = tokenClaims;
                tokens.Add(newToken);
            }
        }







        public static PasswordFlowTokenCache Instance => singletonObject.Value;
    }
}
