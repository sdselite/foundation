using Microsoft.Extensions.Configuration;
using SDSFoundation.Model.Security.Installer;
using System;
using System.Collections.Generic;
using System.Text;

namespace SDSFoundation.Model.Security.Configuration.ConfigurationProviders.Windows
{
    class SecureConfigurationSource : IConfigurationSource
    {
        private readonly CommandLineOptions options;
        private readonly int maximumLicenseAge;
        public SecureConfigurationSource(CommandLineOptions options, int maximumLicenseAge = 7)
        {
            this.options = options;
            this.maximumLicenseAge = maximumLicenseAge;
        }

        public SecureConfigurationSource(int maximumLicenseAge = 7)
        {
            this.options = new CommandLineOptions();
            this.maximumLicenseAge = maximumLicenseAge;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecureConfigurationProvider(options, maximumLicenseAge);
        }
    }
}
