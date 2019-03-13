using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDSFoundation.Model.Security.Installer;
using SDSFoundation.ExtensionMethods.NetStandard.Serialization;
using System.Collections.Generic;
using SDSFoundation.Security.OpenIdDict.Flows.Password;
using SDSFoundation.Security.OpenIdDict.Base.Windows;

namespace SDSFoundation.Security.OpenIdDict.Tests
{
    [TestClass]
    public class CommandLineOptionTests : SecureProgram
    {

        [TestMethod]
        public void ConvertOptionsToToken()
        {

                var passphrase = "mysecret";
        var options = new CommandLineOptions()
        {
            AuthorizationServer = "http://localhost:54540",
            ClientId = "Global",
            ClientSecret = "f066481c-2cea-47c7-a049-e7ab14ac2038",
            SiteId = "b4eacb0e-aff5-4888-b4fb-a6ff6df6775a",
            DeviceId = "4525d5aa-43cb-49d4-b678-298b6d280af2",
            UserName = "administrator@global.com",
            Password = "Secret1!",
            TenantId = "f066481c-2cea-47c7-a049-e7ab14ac2038"
        };


        options.ConfigurationToken = options.ToConfigurationToken(passphrase);



            var deserialized = ConfigurationToken.GetCommandLineOptionsFromConfigurationToken(options.ConfigurationToken, passphrase);
      

        }

        [TestMethod]
        public void TestConfigurationFileAndArgs()
        {
            var args = new List<string>().ToArray();
             InitializeConfiguration(args, "appsettings.json");

            var credentials = new PasswordFlowCredentials(
 tenantId: TenantId,
clientId: ClientId,
clientSecret: ClientSecret,
email: UserName,
password: Password,
siteId: SiteId,
deviceId: DeviceId
);

            //Note - when an http client already exists, that httpclient may be passed as a parameter in an overloaded constructor.  Otherwise, an HttpClient will be created for you.
            PasswordFlow passwordFlow = new PasswordFlow(credentials, AuthorizationServer, 3600);
            var hasValidCredentialsTask = passwordFlow.ValidateCredentials();
            hasValidCredentialsTask.Wait();
        }
   
    }
}
