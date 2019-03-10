using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using SDSFoundation.ExtensionMethods.NetStandard.Serialization;

namespace SDSFoundation.Model.Security.Installer
{
    public static class ConfigurationToken
    {
        

        /// <summary>
        /// Populates the command line options
        /// </summary>
        /// <param name="configurationToken"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        public static CommandLineOptions GetCommandLineOptionsFromConfigurationToken(string configurationToken, string passphrase)
        {
            Encryption.EncryptionHelper encryptionHelper = new Encryption.EncryptionHelper();
            var result = encryptionHelper.Decrypt<CommandLineOptions>(configurationToken, passphrase);

            return result;
        }


        public static string ToConfigurationToken(this CommandLineOptions commandLineOptions, string passphrase)
        {
            Encryption.EncryptionHelper encryptionHelper = new Encryption.EncryptionHelper();
            var result = encryptionHelper.Encrypt<CommandLineOptions>(commandLineOptions, passphrase);

            return result;
        }


    }
}
