using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SDSFoundation.Model.Schedule.NetStandard.Core.UnitTests
{
    public static class ApplicationSettings
    {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
       .AddJsonFile("config.json", false, true)
       .Build();

        public static string ConnectionString
        {
            get
            {
                var connectionString = configuration["Data:DefaultConnection:ConnectionString"];

                return connectionString;
            }
        }


        public static string RootJobsFolder
        {
            get
            {
                return configuration["ApplicationSettings:RootJobsFolder"];
            }
        }

        public static string RunAsCluster
        {
            get
            {
                return configuration["ApplicationSettings:RunAsCluster"];
            }
        }

        public static string QuartzMode
        {
            get
            {
                return configuration["ApplicationSettings:QuartzMode"];
            }
        }

        public static string RejectRemoteRequests
        {
            get
            {
                return configuration["ApplicationSettings:RejectRemoteRequests"];
            }
        }

        public static string ThreadCount
        {
            get
            {
                return configuration["ApplicationSettings:ThreadCount"];
            }
        }

        public static string MisfireThreshold
        {
            get
            {
                return configuration["ApplicationSettings:MisfireThreshold"];
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return configuration["ApplicationSettings:ProxyAddress"];
            }
        }
        public static string ServerPort
        {
            get
            {
                return configuration["ApplicationSettings:ServerPort"];
            }
        }


        public static List<string> AuthorizedTenants
        {
            get
            {
                List<string> result = null;


                var resultStr = configuration["ApplicationSettings:AuthorizedTenants"];

                if (!string.IsNullOrWhiteSpace(resultStr))
                {
                    result = resultStr.Split(',').Select(x => x.Trim()).ToList();
                }

                return result;
            }
        }

        public static string InstanceName
        {
            get
            {
                //InstanceName
                var result = configuration["ApplicationSettings:InstanceName"];

                //If the instance name is set to empty string, use the instance id as the name
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = InstanceId;
                }

                return result;
            }
        }

        public static string InstanceId
        {
            get
            {
                var result = configuration["ApplicationSettings:InstanceId"];

                //Insure that the Instance Id is unique.  If configured to all zeros, it will be unique
                if (result == Guid.Empty.ToString())
                {
                    result = Guid.NewGuid().ToString();
                }

                return result;
            }
        }



    }
}
