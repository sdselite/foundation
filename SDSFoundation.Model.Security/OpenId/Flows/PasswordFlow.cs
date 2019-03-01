using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Model.Security.OpenId.Flows
{
    public class PasswordFlow
    {
        private string openIdConnectPath; // = "http://localhost:54540";
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


        public async Task<bool> ValidateCredentials()
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
                PasswordFlowTokenCache.Instance.AddAccessToken(credentials.ClientId, accessToken, tokenExpirationSeconds);

                //Validate the token
                var getResourceTask = GetResourceAsync(credentials, accessToken);
                getResourceTask.Wait();
                var getResourceResult = getResourceTask.Result;


                var validAccessToken = PasswordFlowTokenCache.Instance.GetValidAccessToken(credentials.ClientId, accessToken, true);
                if (validAccessToken != null && validAccessToken.IsExpiredOrInvalid == false)
                {
                    AddSiteAndDeviceTokenHeaders();
                    return await Task.FromResult<bool>(true);
                }
                else
                {
                    RemoveSiteAndDeviceTokenHeaders();
                    return await Task.FromResult<bool>(false);
                }

            }
            else //We already have an unexpired token
            {
                AddSiteAndDeviceTokenHeaders();
                return await Task.FromResult<bool>(true);
            }

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



        public async Task<JObject> GetTokenAsync(PasswordFlowCredentials credentials)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, openIdConnectPath + "/connect/token");

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = credentials.ClientId,
                ["client_secret"] = credentials.ClientSecret,
                ["username"] = credentials.Email,
                ["password"] = credentials.Password,
                ["siteId"] = credentials.SiteId,
                ["deviceId"] = credentials.DeviceId
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


        //public async Task<JObject> RefreshTokenAsync(PasswordFlowCredentials credentials, string refreshToken)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post, openIdConnectPath + "/connect/token");

        //    request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        //    {
        //        ["client_id"] = credentials.ClientId,
        //        ["client_secret"] = credentials.ClientSecret,
        //        ["grant_type"] = "refresh_token",
        //        ["scope"] = "offline_access",
        //        ["refresh_token"] = refreshToken
        //    });


        //    var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
        //    response.EnsureSuccessStatusCode();

        //    var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
        //    if (payload["error"] != null)
        //    {
        //        throw new InvalidOperationException("An error occurred while refreshing the token.");
        //    }

        //    return payload;
        //}


        public async Task<string> GetResourceAsync(PasswordFlowCredentials credentials, string token)
        {

            var request = new HttpRequestMessage(HttpMethod.Get, openIdConnectPath + "/api/message");
            //var request = new HttpRequestMessage(HttpMethod.Post, openIdConnectPath + "/api/GetMessage");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("client_id", credentials.ClientId);
            request.Headers.Add("siteId", credentials.SiteId);
            request.Headers.Add("deviceId", credentials.DeviceId);

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
