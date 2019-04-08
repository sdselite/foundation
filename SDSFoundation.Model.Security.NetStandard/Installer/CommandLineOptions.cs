using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Security.Installer
{
    /// <summary>
    /// https://github.com/commandlineparser/commandline
    /// </summary>
    public class CommandLineOptions
    {
        //[Option(HelpText = "Set the tenant token.", Required = false)]
        [Option('t', "TenantId", HelpText = "Set the tenant token.")]
        public string TenantId { get; set; }

        [Option('a', "AuthorizationServer", HelpText = "Set the URL to the OpenId authorization server.")]
        public string AuthorizationServer { get; set; }

        [Option('c', "ClientId", HelpText = "Set ClientId (Typically your Tenant name).")]
        public string ClientId { get; set; }

        [Option('s', "ClientSecret", HelpText = "Set the client secret for the OpenId Connect authorization server.")]
        public string ClientSecret { get; set; }

        [Option('l', "SiteId", HelpText = "Set the client (location) site token.")]
        public string SiteId { get; set; }

        [Option('d', "DeviceId", HelpText = "Set the client device token.")]
        public string DeviceId { get; set; }

        [Option('u', "UserName", HelpText = "Set the service account username (Authorization server service account configured in the application, not a Windows service account).")]
        public string UserName { get; set; }

        [Option('p', "Password", HelpText = "Set the service account password (Authorization server service account configured in the application, not a Windows service account).")]
        public string Password { get; set; }

        [Option('w', "ConfigurationToken", HelpText = "Manually provide a configuration token (Not suggested).")]
        public string ConfigurationToken { get; set; }

        [Option('r', "RunAsConsole", HelpText = "Run in Console mode.")]
        public bool RunAsConsole { get; set; }

        [Option('x', "Configuration", HelpText = "Application Specific Configuration.  Convention: SettingName1:SettingValue1,SettingName2:SettingValue2...")]
        public string Configuration { get; set; }

        //[Option('z', "Install", HelpText = "Installs as a service using the name provided.  If no service name is provided a default name is used.")]
        //public bool Install { get; set; }

        //[Option('x', "Uninstall", HelpText = "Uninstalls the service using the name provided.  If no name is provided it uninstalls the application in the current path.")]
        //public bool Uninstall { get; set; }
    }
}
