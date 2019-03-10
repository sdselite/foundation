using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDSFoundation.Model.Security.Installer;
using SDSFoundation.ExtensionMethods.NetStandard.Serialization;

namespace SDSFoundation.Security.OpenIdDict.Tests
{
    [TestClass]
    public class CommandLineOptionTests
    {
        private string passphrase = "mysecret";
        private CommandLineOptions options = new CommandLineOptions()
        {
            AuthorizationServer = "http://localhost:54540",
            ClientId = "Global",
            ClientSecret = "f066481c-2cea-47c7-a049-e7ab14ac2038",
            SiteId = "b4eacb0e-aff5-4888-b4fb-a6ff6df6775a",
            DeviceId = "4525d5aa-43cb-49d4-b678-298b6d280af2",
            UserName = "administrator@global.com",
            Password = "Secret1!"
        };

        [TestMethod]
        public void ConvertOptionsToToken()
        {
            options.ConfigurationToken = options.ToConfigurationToken(passphrase);
            var deserialized = ConfigurationToken.GetCommandLineOptionsFromConfigurationToken(options.ConfigurationToken, passphrase);


        }
   
    }
}
