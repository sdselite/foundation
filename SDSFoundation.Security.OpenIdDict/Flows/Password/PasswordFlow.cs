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
using SDSFoundation.ExtensionMethods.NetStandard.Serialization;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SDSFoundation.Security.OpenIdDict.Flows.Password
{
    public class PasswordFlow
    {
        private string openIdConnectPath;
        private PasswordFlowCredentials credentials;
        private bool ignoreInvalidCertificate;

        private int tokenExpirationSeconds;
        private string accessToken = string.Empty;
        public PasswordFlow(HttpClient client, PasswordFlowCredentials credentials, string openIdConnectPath, int tokenExpirationSeconds, bool ignoreInvalidCertificate = false)
        {
            this.openIdConnectPath = openIdConnectPath;
            this.ignoreInvalidCertificate = ignoreInvalidCertificate;
            this.credentials = credentials;
            this.tokenExpirationSeconds = tokenExpirationSeconds;
           
        }

        /// <summary>
        /// If an HTTP Client is not provided, one is created
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="openIdConnectPath"></param>
        /// <param name="tokenExpirationSeconds"></param>
        public PasswordFlow(PasswordFlowCredentials credentials, string openIdConnectPath, int tokenExpirationSeconds, bool ignoreInvalidCertificate = false)
        {
            this.openIdConnectPath = openIdConnectPath;
            this.ignoreInvalidCertificate = ignoreInvalidCertificate;
            this.credentials = credentials;
            this.tokenExpirationSeconds = tokenExpirationSeconds;
        }

        private HttpResponseMessage SendHttpRequest(HttpRequestMessage request)
        {

            if (ignoreInvalidCertificate)
            {
                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };


                    using (var client = new HttpClient(httpClientHandler))
                    {
                        client.BaseAddress = new Uri(openIdConnectPath);
                        //AddSiteAndDeviceTokenHeaders(client);

                        var sendAsyncTask = client.SendAsync(request,  HttpCompletionOption.ResponseContentRead);
                        sendAsyncTask.Wait();


                        var responseTextAwaiter = sendAsyncTask.Result.Content.ReadAsStringAsync();
                        responseTextAwaiter.Wait();

                        var responseText = responseTextAwaiter.Result;

                        return sendAsyncTask.Result;
                        // Make request here.
                    }
                }


            }
            else
            {
                var client = new HttpClient() { BaseAddress = new Uri(openIdConnectPath) };
                //if (request.Method == HttpMethod.Get)
                //{
                //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                //    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                //}
                //AddSiteAndDeviceTokenHeaders(client);

                var sendAsyncTask = client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
                sendAsyncTask.Wait();

                return sendAsyncTask.Result;
            }


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
                    //AddSiteAndDeviceTokenHeaders();


                    return await Task.FromResult<string>(accessToken);
                }
                else
                {
                    //RemoveSiteAndDeviceTokenHeaders();
                    return await Task.FromResult<string>(string.Empty);
                }

            }
            else //We already have an unexpired token
            {
                //AddSiteAndDeviceTokenHeaders();
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
         
            var request = new HttpRequestMessage(HttpMethod.Get, openIdConnectPath.TrimEnd('/') + "/api/userinfo");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response =  SendHttpRequest(request);

            response.EnsureSuccessStatusCode();


            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;

        }

        private void AddSiteAndDeviceTokenHeaders(HttpClient client)
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
                if (client.DefaultRequestHeaders.Contains("TenantToken") == false)
                {
                    client.DefaultRequestHeaders.Add("TenantToken", credentials.TenantId);
                }


            }

        }


        //private void RemoveSiteAndDeviceTokenHeaders()
        //{
        //    if (client.DefaultRequestHeaders.Contains("TenantToken"))
        //    {
        //        client.DefaultRequestHeaders.Remove("TenantToken");
        //    }
        //    if (client.DefaultRequestHeaders.Contains("SiteToken"))
        //    {
        //        client.DefaultRequestHeaders.Remove("SiteToken");
        //    }
        //    if (client.DefaultRequestHeaders.Contains("DeviceToken"))
        //    {
        //        client.DefaultRequestHeaders.Remove("DeviceToken");
        //    }
        //    if (client.DefaultRequestHeaders.Contains("ClientIPAddress"))
        //    {
        //        client.DefaultRequestHeaders.Remove("ClientIPAddress");
        //    }

        //    if (client.DefaultRequestHeaders.Contains("username"))
        //    {
        //        client.DefaultRequestHeaders.Remove("username");
        //    }

        //    if (client.DefaultRequestHeaders.Contains("password"))
        //    {
        //        client.DefaultRequestHeaders.Remove("password");
        //    }


        //}


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


            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = credentials.ClientId,
                ["tenantId"] = credentials.TenantId,
                ["client_secret"] = credentials.ClientSecret,
                ["username"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials.Email)),
                ["password"] = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials.Password)),
                ["siteId"] = credentials.SiteId,
                ["deviceId"] = credentials.DeviceId,
                ["clientIPAddress"] = credentials.IPAddress
            });

             var response =  SendHttpRequest(request);
            response.EnsureSuccessStatusCode();
            var responseText = await response.Content.ReadAsStringAsync();


            var payload = JObject.Parse(responseText);
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
            request.Headers.Add("tenantId", credentials.TenantId);
            request.Headers.Add("siteId", credentials.SiteId);
            request.Headers.Add("deviceId", credentials.DeviceId);
            request.Headers.Add("clientIPAddress", credentials.IPAddress);

            var response = SendHttpRequest(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
