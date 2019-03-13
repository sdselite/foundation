using Microsoft.Extensions.Configuration;
using SDSFoundation.Model.Security.Installer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Security.Configuration.ConfigurationProviders.Windows
{
    public static class SecureConfigurationExtensions
    {
        public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builder, CommandLineOptions options, int maximumLicenseAge = 7)
        {
            return builder.Add(new SecureConfigurationSource(options, maximumLicenseAge));
        }
    }
}
