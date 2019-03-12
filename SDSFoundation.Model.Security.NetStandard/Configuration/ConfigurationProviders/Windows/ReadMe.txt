This provider securely persists configuration settings to file and assists in securing device, site, application, and single-signon parameters.  
Command line options can be parsed from args, or provided manually.  

In order to generate the token, there is a utility in the Security Services portal in Admin ->  Devices -> Device -> Application Security


CommandLineOptions (options): 

    CommandLineOptions options = null;

            var commandLineOptions = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(o=> {
                    options = o;
                });


Example usage in App Main:


            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddCustomConfiguration(options)
                .AddJsonFile("appsettings.json", optional: true);

            Configuration = builder.Build();
