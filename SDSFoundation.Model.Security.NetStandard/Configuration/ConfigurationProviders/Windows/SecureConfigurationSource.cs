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
        public SecureConfigurationSource(CommandLineOptions options)
        {
            this.options = options;
        }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new SecureConfigurationProvider(options);
        }
    }
}
