using CommandLine;
using Microsoft.Extensions.Configuration;
using SDSFoundation.Model.Security.Installer;
using SDSFoundation.Model.Security.Configuration.ConfigurationProviders.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SDSFoundation.Model.Security.Configuration.Base.Windows
{
    /// <summary>
    /// Base class intended to secure configuration of .net core console / windows applications that integrate with OpenId
    /// </summary>
    public class SecureProgram
    {
        private static IConfigurationRoot Configuration;

        protected static string Uninstall
        {
            get
            {
                return GetSetting("Uninstall");
            }
        }

        protected static string Install
        {
            get
            {
                return GetSetting("Install");
            }
        }

        protected static string Password
        {
            get
            {
                return GetSetting("Password");
            }
        }

        protected static string UserName
        {
            get
            {
                return GetSetting("UserName");
            }
        }

        protected static string DeviceId
        {
            get
            {
                return GetSetting("DeviceId");
            }
        }

        protected static string SiteId
        {
            get
            {
                return GetSetting("SiteId");
            }
        }

        protected static string ClientSecret
        {
            get
            {
                return GetSetting("ClientSecret");
            }
        }

        protected static string AuthorizationServer
        {
            get
            {
                return GetSetting("AuthorizationServer");
            }
        }

        protected static string ClientId
        {
            get
            {
                return GetSetting("ClientId");
            }
        }


        protected static void InitializeConfiguration(string[] args, string appSettingsFileName = "appsettings.json")
        {

            CommandLineOptions options = null;
            var commandLineOptions = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(o => {
                    options = o;
                });


            var pw = Password;

            Console.OutputEncoding = Encoding.UTF8;


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddCustomConfiguration(options)
                .AddJsonFile(appSettingsFileName, optional: true);

            Configuration = builder.Build();

            var applicationToken = GetApplicationSetting("ApplicationToken");

            var userName = Configuration.AsEnumerable().Where(x => x.Key == "UserName").Select(x => x.Value).FirstOrDefault();
        }

        public static string GetApplicationSetting(string settingName)
        {
            if (Configuration != null)
            {
                return GetSetting("ApplicationSettings", settingName);
            }
            else
            {
                return string.Empty;
            }

        }

        protected static string GetSetting(string path, string settingName)
        {
            if (Configuration != null)
            {
                var result = Configuration.AsEnumerable().Where(x => x.Key == string.Format("{0}:{1}", path, settingName)).Select(x => x.Value).FirstOrDefault();

                return result;
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetSetting(string settingName)
        {
            if (Configuration != null)
            {
                var result = Configuration.AsEnumerable().Where(x => x.Key == string.Format("{0}", settingName)).Select(x => x.Value).FirstOrDefault();

                return result;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
