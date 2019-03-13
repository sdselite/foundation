using Microsoft.Extensions.Configuration;
using SDSFoundation.Model.Security.Encryption;
using SDSFoundation.Model.Security.Installer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace SDSFoundation.Model.Security.Configuration.ConfigurationProviders.Windows
{
    public class SecureConfigurationProvider : ConfigurationProvider
    {
        private EncryptionHelper encryptionHelper = new EncryptionHelper();
        private readonly CommandLineOptions commandLineOptions;
        public SecureConfigurationProvider(CommandLineOptions options, int maximumLicenseAge = 7) : base()
        {
            this.commandLineOptions = options;
            UpdateOptionsWithLicenseFile(false, maximumLicenseAge);
            Data = GetSecrets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reRun"></param>
        /// <param name="maximumLicenseAge">Days that a license can be processed</param>
        private void UpdateOptionsWithLicenseFile(bool reRun = false, int maximumLicenseAge = 7)
        {
            //Get key
            var filePath = Directory.GetCurrentDirectory();
            var licenseFileNameAndPath = Directory.GetFiles(filePath)?.ToList().Where(x => x.ToLower().Trim().EndsWith(".license")).FirstOrDefault();

            if (licenseFileNameAndPath != null)
            {
                var fileName = licenseFileNameAndPath.Split('\\').Last().Split('.').First();
                var licenseText = File.ReadAllText(licenseFileNameAndPath);


                var networkInterfaces = NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .OrderBy(nic => nic.OperationalStatus)
                .Select(nic => new { NetworkAdapter = nic, PhysicalAddress = nic.GetPhysicalAddress().ToString() })
                .ToList();



                bool wasParsed = false;

                foreach (var networkInterface in networkInterfaces)
                {
                    try
                    {
                        var passphrase = fileName + networkInterface.PhysicalAddress;
                        var parsedOptions = ConfigurationToken.GetCommandLineOptionsFromConfigurationToken(licenseText, passphrase);
                        MergeParsedOptions(parsedOptions, passphrase, licenseFileNameAndPath);
                        wasParsed = true;
                        break;
                    }
                    catch (Exception ex)
                    {
                        //Do Nothing
                    }
                }

                if (!wasParsed && !reRun)
                {
                    var startDate = DateTime.Now.AddDays(maximumLicenseAge * -1).Date;
                    var endDate = DateTime.Now.AddDays(1).Date;
                    var currentProcessDate = startDate;

                    while(currentProcessDate <= endDate)
                    {
                        //First attempt to get options using original encryption.  If it can be parsed, re-encrypt it so that it can only be used on this device
                        try
                        {

                            var parsedFile = ConfigurationToken.GetCommandLineOptionsFromConfigurationToken(licenseText, currentProcessDate.ToShortDateString() + fileName);

                            var firstNetworkInterface = networkInterfaces?.FirstOrDefault();
                            if (firstNetworkInterface != null)
                            {
                                var newEncryptedText = parsedFile.ToConfigurationToken(fileName + firstNetworkInterface.PhysicalAddress);
                                File.WriteAllText(licenseFileNameAndPath, newEncryptedText);

                                //Do this again.  Should work the next run
                                UpdateOptionsWithLicenseFile(true);

                                break;
                            }

                        }
                        catch (Exception ex)
                        {
                            //Do nothing.  We expect an exception if it has been re-encyrpted
                        }

                        currentProcessDate = currentProcessDate.AddDays(1);
                    }


                }




            }

        }


        /// <summary>
        /// Updates the changes and forces an update to the file (saves it) but only if there are changes 
        /// </summary>
        /// <param name="fileOptions"></param>
        /// <param name="passphrase"></param>
        /// <param name="licenseFileNameAndPath"></param>
        private void MergeParsedOptions(CommandLineOptions fileOptions, string passphrase, string licenseFileNameAndPath)
        {
            //the options passed through the command line override the file
            bool hasChanges = false;
            if (string.IsNullOrWhiteSpace(commandLineOptions.AuthorizationServer) == false)
            {
                fileOptions.AuthorizationServer = commandLineOptions.AuthorizationServer;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.TenantId) == false)
            {
                fileOptions.TenantId = commandLineOptions.TenantId;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.ClientId) == false && commandLineOptions.ClientId != fileOptions.ClientId)
            {
                fileOptions.ClientId = commandLineOptions.ClientId;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.ClientSecret) == false && commandLineOptions.ClientSecret != fileOptions.ClientSecret)
            {
                fileOptions.ClientSecret = commandLineOptions.ClientSecret;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.DeviceId) == false && commandLineOptions.DeviceId != fileOptions.DeviceId)
            {
                fileOptions.DeviceId = commandLineOptions.DeviceId;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.Password) == false && commandLineOptions.Password != fileOptions.Password)
            {
                fileOptions.Password = commandLineOptions.Password;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.SiteId) == false && commandLineOptions.SiteId != fileOptions.SiteId)
            {
                fileOptions.SiteId = commandLineOptions.SiteId;
                hasChanges = true;
            }

            if (string.IsNullOrWhiteSpace(commandLineOptions.UserName) == false && commandLineOptions.UserName != fileOptions.UserName)
            {
                fileOptions.UserName = commandLineOptions.UserName;
                hasChanges = true;
            }

            //Set the commandLineOptions to equal the fileOptions settings - this merges the two
            commandLineOptions.AuthorizationServer = fileOptions.AuthorizationServer;
            commandLineOptions.ClientId = fileOptions.ClientId;
            commandLineOptions.ClientSecret = fileOptions.ClientSecret;
            commandLineOptions.DeviceId = fileOptions.DeviceId;
            commandLineOptions.Password = fileOptions.Password;
            commandLineOptions.SiteId = fileOptions.SiteId;
            commandLineOptions.UserName = fileOptions.UserName;
            commandLineOptions.TenantId = fileOptions.TenantId;

            if (hasChanges)
            {
                //Save the changes
                var newEncryptedText = fileOptions.ToConfigurationToken(passphrase);
                File.WriteAllText(licenseFileNameAndPath, newEncryptedText);
            }

        }

        private IDictionary<string, string> GetSecrets()
        {
            var secretsDictionary = new Dictionary<string, string>
           {
                {"AuthorizationServer", commandLineOptions.AuthorizationServer},
                {"ClientId", commandLineOptions.ClientId},
                {"ClientSecret", commandLineOptions.ClientSecret},
                {"ConfigurationToken", commandLineOptions.ConfigurationToken},
                {"DeviceId", commandLineOptions.DeviceId},
                {"Password", commandLineOptions.Password},
                {"SiteId", commandLineOptions.SiteId},
                {"UserName", commandLineOptions.UserName},
                {"TenantId", commandLineOptions.TenantId},
           };
            return secretsDictionary;
        }

    }
}
