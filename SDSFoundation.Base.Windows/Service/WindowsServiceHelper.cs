using System;
using System.Collections.Generic;
using System.Text;
using Topshelf;

namespace SDSFoundation.Base.Windows.NetStandard.Service
{
    public class WindowsServiceHelper
    {


        public event EventHandler<WindowsServiceExceptionEventArgs> ServiceException;

        public void RunService<TService>(TService service, string serviceName = "", string displayName = "", string description = "", bool startAutomatically = true, int restartAttempts = 3, int resetPeriodDays = 1, int serviceStartTimeoutSeconds = 90)
            where TService : WindowsServiceBase<TService>
        {

            if (string.IsNullOrWhiteSpace(serviceName))
            {
                serviceName = typeof(TService).Name;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                description = serviceName;
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = serviceName;
            }

            var serviceRunner = HostFactory.Run(x =>
            {
                MapCommandLineParameters(x);

                x.OnException((serviceException) => HandleServiceException(serviceException));

                if (startAutomatically)
                {
                    x.StartAutomatically();
                }
                else
                {
                    x.StartManually();
                }

                x.EnableServiceRecovery(rc =>
                {

                    if (restartAttempts > 0)
                    {
                        for (int i = 0; i < restartAttempts; i++)
                        {
                            rc.RestartService(i);
                        }
                    }


                    //number of days until the error count resets
                    rc.SetResetPeriod(resetPeriodDays);

                });


                x.Service<TService>(s =>
                {

                    s.ConstructUsing(hostSettings =>
                    {
                        service.TopShelfSettings = hostSettings;
                        return service;
                    });

                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());

                    x.SetStartTimeout(new TimeSpan(0, 0, serviceStartTimeoutSeconds));
                });



                x.SetDescription(description);
                x.SetDisplayName(displayName);
                x.SetServiceName(serviceName);

                x.RunAsLocalSystem();

            });

            var exitCode = (int)Convert.ChangeType(serviceRunner, serviceRunner.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

        protected virtual void OnServiceException(WindowsServiceExceptionEventArgs e)
        {
            ServiceException?.Invoke(this, e);
        }

        /// <summary>
        /// Sets up the default parameter mappings
        /// </summary>
        /// <param name="configurator"></param>
        private void MapCommandLineParameters(Topshelf.HostConfigurators.HostConfigurator configurator)
        {
            //Topshelf will throw an exception if we do not explicitly add these custom command line arguments
            configurator.AddCommandLineDefinition("t", z => { });
            configurator.AddCommandLineDefinition("TenantId", z => { });
            configurator.AddCommandLineDefinition("a", z => { });
            configurator.AddCommandLineDefinition("AuthorizationServer", z => { });
            configurator.AddCommandLineDefinition("c", z => { });
            configurator.AddCommandLineDefinition("ClientId", z => { });
            configurator.AddCommandLineDefinition("s", z => { });
            configurator.AddCommandLineDefinition("ClientSecret", z => { });
            configurator.AddCommandLineDefinition("l", z => { });
            configurator.AddCommandLineDefinition("SiteId", z => { });
            configurator.AddCommandLineDefinition("d", z => { });
            configurator.AddCommandLineDefinition("DeviceId", z => { });
            configurator.AddCommandLineDefinition("u", z => { });
            configurator.AddCommandLineDefinition("p", z => { });
            configurator.AddCommandLineDefinition("w", z => { });
            configurator.AddCommandLineDefinition("ConfigurationToken", z => { });
        }

        private void HandleServiceException(Exception exception)
        {
            OnServiceException(new WindowsServiceExceptionEventArgs() { ServiceException = exception });
        }

    }
}
