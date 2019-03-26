//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SDSFoundation.Model.Security.Installer;
//using SDSFoundation.ExtensionMethods.NetStandard.Serialization;
//using System.Collections.Generic;
//using SDSFoundation.Security.OpenIdDict.Flows.Password;
//using SDSFoundation.Security.OpenIdDict.Base.Windows;

//namespace SDSFoundation.Security.OpenIdDict.Tests
//{
//    [TestClass]
//    public class CommandLineOptionTests : SecureProgram<CommandLineOptionTests>
//    {

//        [TestMethod]
//        public void ConvertOptionsToToken()
//        {

//                var passphrase = "mysecret";
//        var options = new CommandLineOptions()
//        {
//            AuthorizationServer = "http://localhost:54540",
//            ClientId = "Global",
//            ClientSecret = "f066481c-2cea-47c7-a049-e7ab14ac2038",
//            SiteId = "b4eacb0e-aff5-4888-b4fb-a6ff6df6775a",
//            DeviceId = "4525d5aa-43cb-49d4-b678-298b6d280af2",
//            UserName = "administrator@global.com",
//            Password = "Secret1!",
//            TenantId = "f066481c-2cea-47c7-a049-e7ab14ac2038"
//        };


//        options.ConfigurationToken = options.ToConfigurationToken(passphrase);



//            var deserialized = ConfigurationToken.GetCommandLineOptionsFromConfigurationToken(options.ConfigurationToken, passphrase);
      

//        }

//        //[TestMethod]
//        //public void TestConfigurationFileAndArgs()
//        //{
//        //    //var args = new List<string>().ToArray();
//        //    //var args = new List<string>() { "--t", "123", "--r", "true", "--z", "true", "--x", "true" }.ToArray();

//        //    var args = new List<string>() { "--r", "true" }.ToArray();

//        //    var commandLineOptions = Initialize(args: args, appSettingsFileName: "appsettings.json", maximumLicenseAge: 3);
//        //    Login(tokenExpirationSeconds: 3600, ignoreInvalidCertificate: true);

//        //    //var runTask = Run<ExampleService>();
//        //    runTask.Wait();

//        //    var didRun = runTask.Result;
//        //}
   
//    }
//}
