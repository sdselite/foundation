﻿using CommandLine;
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
        [Option('a', "auth", Required = false, HelpText = "Set the URL to the OpenId authorization server")]
        public string AuthorizationServer { get; set; }

        [Option('c', "clientid", Required = false, HelpText = "Set ClientId (Typically your Tenant name).")]
        public string ClientId { get; set; }

        [Option('s', "secret", Required = false, HelpText = "Set the client secret for the OpenId authorization server")]
        public string ClientSecret { get; set; }

        [Option('l', "site", Required = false, HelpText = "Set the client (location) site token")]
        public string SiteId { get; set; }

        [Option('d', "device", Required = false, HelpText = "Set the client device token")]
        public string DeviceId { get; set; }

        [Option("user", Required = false, HelpText = "Set the service account username (Authorization server service account configured in the application, not a Windows service account)")]
        public string UserName { get; set; }

        [Option("password", Required = false, HelpText = "Set the service account password (Authorization server service account configured in the application, not a Windows service account)")]
        public string Password { get; set; }

        [Option('t', "configtoken", Required = false, HelpText = "Initializes the application using a configuration token which is used to securely configure the initial settings")]
        public string ConfigurationToken { get; set; }

        [Option('i', "install", Required = false, HelpText = "Installs as a service using the name provided.  If no service name is provided a default name is used.")]
        public bool Install { get; set; }

        [Option('u', "uninstall", Required = false, HelpText = "Uninstalls the service using the name provided.  If no name is provided it uninstalls the application in the current path.")]
        public bool Uninstall { get; set; }
    }
}
