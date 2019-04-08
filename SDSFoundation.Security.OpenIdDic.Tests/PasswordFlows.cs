using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDSFoundation.Security.OpenIdDict.Flows.Password;
using SDSFoundation.ExtensionMethods.Network;
using System;
using Newtonsoft.Json.Linq;

namespace SDSFoundation.Security.OpenIdDic.Tests
{
    [TestClass]
    public class PasswordFlows
    {
        public const string AuthorizationServer = "http://localhost:54540";
        public const string ClientId = "Global";
        public const string ClientSecret = "f066481c-2cea-47c7-a049-e7ab14ac2038";

        public const string UserName = "administrator@global.com";
        public const string Password = "Secret1!";

        public const string SiteId = "b4eacb0e-aff5-4888-b4fb-a6ff6df6775a";
        public const string DeviceId = "4525d5aa-43cb-49d4-b678-298b6d280af2";
        public const string TenantId = "f066481c-2cea-47c7-a049-e7ab14ac2038";

        [TestMethod]
        public void PasswordFlowTesting_HappyPath()
        {

            var credentials = new PasswordFlowCredentials(
           tenantId: TenantId,
          clientId: ClientId,
          clientSecret: ClientSecret,
          email: UserName,
          password: Password,
          siteId: SiteId,
          deviceId: DeviceId);

            //Note - when an http client already exists, that httpclient may be passed as a parameter in an overloaded constructor.  Otherwise, an HttpClient will be created for you.
            PasswordFlow passwordFlow = new PasswordFlow(credentials, AuthorizationServer, 3600, true);
            var hasValidCredentialsTask = passwordFlow.ValidateCredentials();
            hasValidCredentialsTask.Wait();

            var accessToken1 = hasValidCredentialsTask.Result;
            //Get available claims
            var tokenClaims = PasswordFlowTokenCache.Instance.GetTokenClaims(ClientId, accessToken1);

            Assert.IsTrue(tokenClaims?.Count > 0, "No Claims Found");


        }

        [TestMethod]
        public void IpAddressToGPSCoordinates()
        {


            UriBuilder uriBuilder = new UriBuilder();
            var ipAddress = UriExtensions.GetIPFromString("35.137.210.156");

            uriBuilder.Host = ipAddress;
            uriBuilder.Scheme = "http";

            var uri = uriBuilder.Uri;

            var apiAddress = "http://api.ipstack.com/[URL]?access_key=fbe51630a05b6d0a5c8a4dccc0955aea";
            var geoData = uri.GetGeolocationData(apiAddress);

            dynamic result = JObject.Parse(geoData);

            var lat = result.latitude.Value;
            var lng = result.longitude.Value;


        }
    }
}
