using SDSFoundation.Model.Security.OpenId.Flows;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Extensions.Compression.Client;
using System.Net.Http.Extensions.Compression.Core.Compressors;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace SDSFoundation.Base.Security.WebApi.Proxy.Client
{
    /// <summary>
    /// Client configuration.
    /// </summary>
    public static partial class Configuration
    {
        /// <summary>
        /// Web Api Base Address.
        /// </summary>
        public static string SiteServicesProxyBaseAddress = string.Empty;
    }

    public abstract partial class ClientBase
    {
        public HttpClient HttpClient { get; protected set; }
        public PasswordFlowCredentials credentials;
        private string openIdConnectPath;
        private int tokenExpirationSeconds;

        protected ClientBase(PasswordFlowCredentials credentials, string siteServicesProxyBaseAddress, string openIdConnectPath, int tokenExpirationSeconds)
        {

        //            Transfer-Encoding: chunked
        //Accept-Encoding: gzip, deflate
        // DeviceGateway.Services.SiteServicesHub.WebApiProxy.Configuration.SiteServicesProxyBaseAddress = siteServicesProxyBaseAddress;

        //((System.Net.Http.HttpClientHandler)(((System.Net.Http.HttpMessageInvoker)(this.HttpClient))._handler)).AutomaticDecompression = System.Net.DecompressionMethods.None
        //HttpClientHandler handler = new HttpClientHandler()
        //{
        //    AutomaticDecompression = System.Net.DecompressionMethods.None
        //};

        //HttpClient = new HttpClient(handler)
        //{
        //    BaseAddress = new Uri(Configuration.SiteServicesProxyBaseAddress)

        //};

        HttpClient = new HttpClient(new ClientCompressionHandler(new GZipCompressor(), new DeflateCompressor()))
            {
                BaseAddress = new Uri(Configuration.SiteServicesProxyBaseAddress)
            };

            this.credentials = credentials;
            this.openIdConnectPath = openIdConnectPath;
            this.tokenExpirationSeconds = tokenExpirationSeconds;

            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));

        }



        public void UpdateClient(HttpClient client)
        {
            this.HttpClient = client;
        }


        public void ValidateCredentials()
        {

            PasswordFlow passwordFlow = new PasswordFlow(HttpClient, credentials, openIdConnectPath, tokenExpirationSeconds);
            var hasValidCredentialsTask = passwordFlow.ValidateCredentials();
            hasValidCredentialsTask.Wait();

            var hasValidCredentials = hasValidCredentialsTask.Result;

            if (hasValidCredentials == false)
            {
                throw new System.Security.SecurityException("Invalid credentials provided.");
            }
        }

    }
}
