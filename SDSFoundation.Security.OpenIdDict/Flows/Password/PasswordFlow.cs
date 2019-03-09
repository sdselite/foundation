using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Security.OpenIdDict.Flows.Password
{
    public class PasswordFlow
    {
        private string openIdConnectPath;
        private PasswordFlowCredentials credentials;
        private HttpClient client;
        private int tokenExpirationSeconds;
        private string accessToken = string.Empty;
        public PasswordFlow(HttpClient client, PasswordFlowCredentials credentials, string openIdConnectPath, int tokenExpirationSeconds)
        {
            this.openIdConnectPath = openIdConnectPath;
            this.credentials = credentials;
            this.client = client;
            this.tokenExpirationSeconds = tokenExpirationSeconds;
        }

        /// <summary>
        /// If an HTTP Client is not provided, one is created
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="openIdConnectPath"></param>
        /// <param name="tokenExpirationSeconds"></param>
        public PasswordFlow(PasswordFlowCredentials credentials, string openIdConnectPath, int tokenExpirationSeconds)
        {
            this.openIdConnectPath = openIdConnectPath;
            this.credentials = credentials;



            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(openIdConnectPath)
                //BaseAddress = new Uri(Configuration.SiteServicesProxyBaseAddress)
            };

            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));


            this.client = httpClient;
            this.tokenExpirationSeconds = tokenExpirationSeconds;
        }


        /// <summary>
        /// Returns an access token
        /// </summary>
        /// <returns></returns>
        public async Task<string> ValidateCredentials()
        {
            bool hasCurrentToken = false;
            //Determine if the token already exists.  If so and not expired, return true
            if (string.IsNullOrWhiteSpace(accessToken) == false)
            {
                var existingToken = PasswordFlowTokenCache.Instance.GetValidAccessToken(credentials.ClientId, accessToken, false);

                if (existingToken != null && existingToken.IsExpiredOrInvalid == false)
                {
                    hasCurrentToken = true;
                    accessToken = existingToken.AccessToken;
                }

            }

            if (hasCurrentToken == false)
            {

                var getTokenTask = GetTokenAsync(credentials);
                getTokenTask.Wait();

                accessToken = (string)getTokenTask.Result["access_token"];


                var claimsJson = await GetUserClaimsJson();
                var parsedClaims = ParseClaims(claimsJson);

                PasswordFlowTokenCache.Instance.AddAccessToken(credentials.ClientId, accessToken, tokenExpirationSeconds, parsedClaims);

                //Validate the token
                var getResourceTask = GetResourceAsync(credentials, accessToken);
                getResourceTask.Wait();
                var getResourceResult = getResourceTask.Result;


                var validAccessToken = PasswordFlowTokenCache.Instance.GetValidAccessToken(credentials.ClientId, accessToken, true);
                if (validAccessToken != null && validAccessToken.IsExpiredOrInvalid == false)
                {
                    AddSiteAndDeviceTokenHeaders();



                    return await Task.FromResult<string>(accessToken);
                }
                else
                {
                    RemoveSiteAndDeviceTokenHeaders();
                    return await Task.FromResult<string>(string.Empty);
                }

            }
            else //We already have an unexpired token
            {
                AddSiteAndDeviceTokenHeaders();
                return await Task.FromResult<string>(accessToken);
            }

        }

        private List<Claim> ParseClaims(string jsonFormattedClaims)
        {
            // var claims = ParseClaims(responseContent);
            var applicationClaims = JObject.Parse(jsonFormattedClaims)["Application"].ToList();
            List<Claim> claims = new List<Claim>();

            if (applicationClaims?.Count > 0)
            {
                foreach (var claim in applicationClaims)
                {
                    var jsonString = claim.ToString();
                    var deserializedClaim = JsonConvert.DeserializeObject<Claim>(jsonString, new PasswordFlowClaimConverter());
                    claims.Add(deserializedClaim);
                }
            }

            return claims;

        }
        private async Task<string> GetUserClaimsJson()
        {

            var request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress.AbsoluteUri.TrimEnd('/') + "/api/userinfo");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            response.EnsureSuccessStatusCode();


            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;

        }

        private void AddSiteAndDeviceTokenHeaders()
        {

            if (string.IsNullOrWhiteSpace(accessToken) == false)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                if (client.DefaultRequestHeaders.Contains("SiteToken") == false)
                {
                    client.DefaultRequestHeaders.Add("SiteToken", credentials.SiteId);
                }
                if (client.DefaultRequestHeaders.Contains("DeviceToken") == false)
                {
                    client.DefaultRequestHeaders.Add("DeviceToken", credentials.DeviceId);
                }
                if (client.DefaultRequestHeaders.Contains("ClientIPAddress") == false)
                {
                    client.DefaultRequestHeaders.Add("ClientIPAddress", credentials.IPAddress);
                }



            }

        }


        private void RemoveSiteAndDeviceTokenHeaders()
        {

            if (client.DefaultRequestHeaders.Contains("SiteToken"))
            {
                client.DefaultRequestHeaders.Remove("SiteToken");
            }
            if (client.DefaultRequestHeaders.Contains("DeviceToken"))
            {
                client.DefaultRequestHeaders.Remove("DeviceToken");
            }
            if (client.DefaultRequestHeaders.Contains("ClientIPAddress"))
            {
                client.DefaultRequestHeaders.Remove("ClientIPAddress");
            }

            if (client.DefaultRequestHeaders.Contains("username"))
            {
                client.DefaultRequestHeaders.Remove("username");
            }

            if (client.DefaultRequestHeaders.Contains("password"))
            {
                client.DefaultRequestHeaders.Remove("password");
            }


        }


        public async Task<bool> ValidateAccessToken(string accessToken)
        {
            var validAccessToken = PasswordFlowTokenCache.Instance.GetValidAccessToken(credentials.ClientId, accessToken);
            if (validAccessToken != null && validAccessToken.IsExpiredOrInvalid == false)
            {
                return await Task.FromResult<bool>(true);
            }
            else
            {
                return await Task.FromResult<bool>(false);
            }
        }



        private async Task<JObject> GetTokenAsync(PasswordFlowCredentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, openIdConnectPath + "/connect/token");

            if (string.IsNullOrWhiteSpace(credentials.IPAddress))
            {
                //Force the IP address to be provided
                credentials.SyncIPAddress();
            }


            var encodedPw = Convert.ToBase64String(
            System.Text.Encoding.ASCII.GetBytes(
                 credentials.Password));

            //var decodedPw = Encoding.ASCII.GetString(Convert.FromBase64String(encodedPw));
            //var decodedPw = Convert.FromBase64String(
            //System.Text.Encoding.ASCII.G(
            //     encodedPw));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {


                ["grant_type"] = "password",
                ["client_id"] = credentials.ClientId,
                ["client_secret"] = credentials.ClientSecret,
                ["username"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials.Email)),
                ["password"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials.Password)),
                ["siteId"] = credentials.SiteId,
                ["deviceId"] = credentials.DeviceId,
                ["clientIPAddress"] = credentials.IPAddress
            });


            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();

            var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (payload["error"] != null)
            {
                throw new InvalidOperationException("An error occurred while retrieving an access token.");
            }

            return payload; // (string)payload["access_token"];
        }


        private async Task<string> GetResourceAsync(PasswordFlowCredentials credentials, string token)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, openIdConnectPath + "/api/message");
            //var request = new HttpRequestMessage(HttpMethod.Post, openIdConnectPath + "/api/GetMessage");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("client_id", credentials.ClientId);
            request.Headers.Add("siteId", credentials.SiteId);
            request.Headers.Add("deviceId", credentials.DeviceId);
            request.Headers.Add("clientIPAddress", credentials.IPAddress);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
