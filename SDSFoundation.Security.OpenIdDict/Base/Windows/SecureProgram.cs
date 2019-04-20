using CommandLine;
using Microsoft.Extensions.Configuration;
using SDSFoundation.Model.Security.Installer;
using SDSFoundation.Model.Security.Configuration.ConfigurationProviders.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SDSFoundation.Security.OpenIdDict.Flows.Password;
using System.Security.Claims;
using log4net;
using System.Xml;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace SDSFoundation.Security.OpenIdDict.Base.Windows
{
    /// <summary>
    /// Base class intended to secure configuration of .net core console / windows applications that integrate with OpenId
    /// </summary>
    public class SecureProgram<TProgram>
    {
        public static IConfigurationRoot Configuration { get; set; }

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

        /// <summary>
        /// Logs message and writes Information to the console.  To write to console, the application must be in console mode.  To log a message, logging must be configured.  Both are optional.
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(string message)
        {

            Console.WriteLine(string.Format("{0}    Info: {1}", DateTime.Now.ToShortTimeString(), message));

            if (IsLoggingEnabled)
            {
                Log.Info(message);
            }

        }

        /// <summary>
        /// Logs message and writes Warning to the console.  To write to console, the application must be in console mode.  To log a message, logging must be configured.  Both are optional.
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {

            Console.WriteLine(string.Format("{0}    Warning: {1}", DateTime.Now.ToShortTimeString(), message));

            if (IsLoggingEnabled)
            {
                Log.Warn(message);
            }

        }

        /// <summary>
        /// Logs message and writes Exception to the console.  To write to console, the application must be in console mode.  To log a message, logging must be configured.  Both are optional.
        /// </summary>
        /// <param name="message"></param>
        public static void LogException(string message, Exception exception)
        {

            Console.WriteLine(string.Format("{0}    Description: {1}{2}                 Exception: {3}", DateTime.Now.ToShortTimeString(), message, Environment.NewLine, GetErrorMessageFromException(exception)));

            if (IsLoggingEnabled)
            {
                Log.Error(message, exception);
            }

        }

        protected static CommandLineOptions Initialize(string[] args, string appSettingsFileName = "appsettings.json", int maximumLicenseAge = 7)
        {
            //Configure Log file settings
            InitializeLogging();

            LogInfo("Starting...");
            var commandLineOptionsConfigured = InitializeConfiguration(args, appSettingsFileName, maximumLicenseAge);
            if(commandLineOptionsConfigured != null)
            {
                RunAsConsole = commandLineOptionsConfigured.RunAsConsole;
            }
            

            if (args != null && args.Count() > 0)
            {
                LogInfo("Arguments provided:");
                foreach (var arg in args)
                {
                    if (!string.IsNullOrWhiteSpace(arg))
                        LogInfo(Environment.NewLine + " " + arg);
                }

            }

            LogInfo("Authenticating...");


#if (DEBUG)

            if (IgnoreInvalidSSLCertInDebug)
            {
                LogWarning("Ignoring invalid SSL certificates while in debug mode!  Deploy in Release Mode!");
                //Allowing insecure connections because of integration with Azure development deployments, which do not have a valid SSL cert
                //Only use this in development.
                IgnoreInvalidSSLCert = true;
            }
#endif

            return commandLineOptionsConfigured;
        }

        protected static void Login(int tokenExpirationSeconds = 3600, bool ignoreInvalidCertificate = false)
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
            PasswordFlow passwordFlow = new PasswordFlow(credentials, AuthorizationServer, tokenExpirationSeconds, ignoreInvalidCertificate);
            var hasValidCredentialsTask = passwordFlow.ValidateCredentials();
            hasValidCredentialsTask.Wait();

            Claims = PasswordFlowTokenCache.Instance.GetTokenClaims(ClientId, hasValidCredentialsTask.Result);
        }



        /// <summary>
        /// Returns a json string
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="actionParameters"></param>
        /// <param name="tokenExpirationSeconds"></param>
        /// <param name="ignoreInvalidCertificate"></param>
        /// <returns></returns>
        public static string ExecuteSecureAction(ClaimsPrincipal principal, string actionName, Dictionary<string, string> actionParameters = null, int tokenExpirationSeconds = 3600, bool ignoreInvalidCertificate = false)
        {
            if (!principal.Identity.IsAuthenticated) throw new Exception("User is not authenticated!  Unable to execute action!");

            var credentials = new PasswordFlowCredentials(
                  tenantId: TenantId,
                  clientId: ClientId,
                  clientSecret: ClientSecret,
                  email: UserName,
                  password: Password,
                  siteId: SiteId,
                  deviceId: DeviceId);

            var passwordFlow = new PasswordFlow(credentials, AuthorizationServer, tokenExpirationSeconds, ignoreInvalidCertificate);
           
            var result = passwordFlow.ExecuteSecureAction(credentials, actionName, actionParameters);
            result.Wait();
            return result.Result;
        }

        /// <summary>
        /// Gets a combination of local and encrypted settings.  Local settings are used over encrypted setting.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="settingName"></param>
        /// <returns></returns>
        protected static string GetSetting(string path, string settingName)
        {
            try
            {
                var result = string.Empty;


                if (Configuration != null)
                {
                    try
                    {
                        result = Configuration.AsEnumerable().Where(x => x.Key == string.Format("{0}:{1}", path, settingName)).Select(x => x.Value).FirstOrDefault();
                    }
                    catch (Exception)
                    {

                    }
                }

                //if(string.IsNullOrWhiteSpace(result) && EncryptedConfiguration != null)
                //{
                //    if (EncryptedConfiguration.ContainsKey(settingName))
                //    {
                //        result = EncryptedConfiguration[settingName];
                //    }
                //}


                return result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
         
        }

        public static List<Claim> Claims { get; private set; } = new List<Claim>();
        public static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TProgram));

        protected static bool IsLoggingEnabled
        {
            get
            {
                return !string.IsNullOrWhiteSpace(LogFileName);
            }
        }

        protected static string LogFileName { get; set; } = "log4net.config";

        protected static bool IgnoreInvalidSSLCert { get; set; } = false;

        protected static bool IgnoreInvalidSSLCertInDebug { get; set; } = true;

        protected static bool RunAsConsole { get; set; } = false;

        protected static bool Uninstall { get; set; } = false;

        protected static bool Install { get; set; } = false;

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

        protected static string TenantId
        {
            get
            {
                return GetSetting("TenantId");
            }
        }

        protected static string ClientId
        {
            get
            {
                return GetSetting("ClientId");
            }
        }

        //public static Dictionary<string, string> EncryptedConfiguration
        //{
        //    get
        //    {
        //        var result = new Dictionary<string, string>();
        //        var configString = GetSetting("Configuration");

        //        if (!string.IsNullOrWhiteSpace(configString))
        //        {
        //            var settingsSplit = configString.Split(',')?.ToList();
        //            if(settingsSplit != null && settingsSplit.Count > 0)
        //            {
        //                foreach (var setting in settingsSplit)
        //                {
        //                    var settingSplit = setting.Split(',')?.ToList();
        //                    if(settingSplit != null && settingSplit.Count == 2)
        //                    {
        //                        result.Add(settingSplit[0], settingSplit[1]);
        //                    }
        //                }
        //            }
                    
        //        }
        //        //customSettingsStr = customSettingsStr + $(val).attr("settingName") + ':' + $(val).val() + ",";

        //        return result;
        //    }
        //}

        //Configuration


        public static Dictionary<string, string> SettingsDictionary
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                var keyValuePairs = Configuration.AsEnumerable().Select(x => new { Key = x.Key, Value = x.Value }).ToList();

                if(keyValuePairs != null && keyValuePairs.Count > 0)
                {
                    foreach (var keyValuePair in keyValuePairs)
                    {
                        if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
                        {
                            result.Add(keyValuePair.Key, keyValuePair.Value);
                        }
                    }
                }

                return result;
            }
        }

        private static string GetSetting(string settingName)
        {
            try
            {
                if (Configuration != null)
                {
                    var result = Configuration.AsEnumerable().Where(x => x.Key.ToLower() == string.Format("{0}", settingName.ToLower())).Select(x => x.Value).FirstOrDefault();

                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {

                return string.Empty;
            }

        }

        private static CommandLineOptions InitializeConfiguration(string[] args, string appSettingsFileName = "appsettings.json", int maximumLicenseAge = 7)
        {

            CommandLineOptions options = null;


            if(args != null && args.Count() > 0)
            {
                var commandLineOptions = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(x => {
                    options = x;
                }).WithNotParsed(errors => {

                    if (errors != null && errors.Count() > 0)
                    {
                        foreach (var ex in errors)
                        {
                            LogWarning("Error Parsing CommandLineOption: " + ex.Tag.ToString());
                        }
                    }

                });
            }


            IConfigurationBuilder builder = null;
            // Set up configuration sources.

            if(options != null)
            {
                builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddCustomConfiguration(options, maximumLicenseAge)
                .AddJsonFile(appSettingsFileName, optional: true);
            }
            else
            {
                builder = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(AppContext.BaseDirectory))
               .AddCustomConfiguration(maximumLicenseAge)
               .AddJsonFile(appSettingsFileName, optional: true);
            }


            Configuration = builder.Build();

            var mySetting = TenantId;

            var userName = Configuration.AsEnumerable().Where(x => x.Key == "UserName").Select(x => x.Value).FirstOrDefault();

            return options;
        }

        private static string GetErrorMessageFromException(Exception ex)
        {
            string result = string.Empty;
            if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
            {
                result = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {

                    result = string.Format("{0}{1}                 Inner Exception: {2}{3}", result.Trim(), Environment.NewLine, GetErrorMessageFromException(ex.InnerException), Environment.NewLine);
                }
            }

            return result;
        }
        private static void InitializeLogging()
        {
            if (IsLoggingEnabled)
            {
                XmlDocument log4netConfig = new XmlDocument();
                log4netConfig.Load(File.OpenRead(LogFileName));
                var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                           typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

                Log.Info("Logging Initialized.");
            }
        }
    }
}
