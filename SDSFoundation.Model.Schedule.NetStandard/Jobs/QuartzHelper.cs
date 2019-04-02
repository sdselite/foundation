using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Model.Schedule.NetStandard.Jobs
{
    public class QuartzHelper
    {
        /// <summary>
        /// Used to chain jobs in a way that is persisted.  Requires Quartz.net and using a database for the jobs.
        /// 1) Chain jobs and persist the links even if the scheduler goes down.
        /// 2) Be able to pass a payload between parent and children jobs.
        /// https://blog.harveydelaney.com/implementing-job-chaining-in-quartz-net/
        /// </summary>
        /// <typeparam name="TJob"></typeparam>
        /// <param name="payloadMap"></param>
        /// <param name="childrenJobs"></param>
        /// <returns></returns>
        public IJobDetail CreateJob<TJob>(IJobExecutionContext context, Dictionary<string, object> payloadMap, params IJobDetail[] childrenJobs)
    where TJob : IJob
        {
            var newJob = JobBuilder.Create<TJob>()
                .WithIdentity(Guid.NewGuid().ToString("N"))
                .StoreDurably(true)
                .RequestRecovery(false)
                .Build();

            newJob.JobDataMap.Put(Constants.PayloadKey, payloadMap);

            if (childrenJobs != null && childrenJobs.Length > 0)
            {
                var jkList = childrenJobs.Select(job => job.Key).ToList();

                newJob.JobDataMap.Put(Constants.NextJobKey, jkList);
            }

            //TaskScheduler.Schedule(newJob); RL: this doesn't work in .net standard.  

            //RL: This is not how it was done in the example code
            var addJobTask = context.Scheduler.AddJob(jobDetail: newJob, replace: true, storeNonDurableWhileAwaitingScheduling: false);

            return newJob;
        }

        private readonly string tenantName;
            public QuartzHelper(string tenantName)
            {
                tablePrefix = string.Format("{0}_", tenantName);
                this.tenantName = tenantName;
            }

            public QuartzHelper()
            {
                throw new System.Security.SecurityException("The default constructor cannot be used.");
            }

            private readonly string threadPooltype = "Quartz.Simpl.SimpleThreadPool, Quartz";
            private readonly string jobStoretype = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            private readonly string dataSource = "default";
            private readonly string tablePrefix = "QRTZ_";
            private readonly string driverDelegateType = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
            private readonly string provider = "SqlServer";
            private readonly string recentHistoryType = "Quartz.Plugins.RecentHistory.ExecutionHistoryPlugin, Quartz.Plugins.RecentHistory";
            private readonly string storeType = "Quartz.Plugins.RecentHistory.Impl.InProcExecutionHistoryStore, Quartz.Plugins.RecentHistory";
            private readonly string useProperties = "true";
            //the 'binary' serialization type only works against the full .NET Framework
            private readonly string serializerType = "json";


            public NameValueCollection GetCommonSettings(string instanceId, string connectionString, bool runAsCluster, int threadCount = 1, int misfireThreshold = 60000)
            {
                var clientOrServerProperties = new NameValueCollection
                {
                    ["quartz.scheduler.instanceName"] = tenantName,
                    ["quartz.scheduler.instanceId"] = instanceId,
                    ["quartz.threadPool.type"] = threadPooltype,
                    ["quartz.threadPool.threadCount"] = threadCount.ToString(),
                    ["quartz.jobStore.misfireThreshold"] = misfireThreshold.ToString(),
                    ["quartz.jobStore.type"] = jobStoretype,
                    ["quartz.jobStore.useProperties"] = useProperties,
                    ["quartz.jobStore.dataSource"] = dataSource,
                    ["quartz.jobStore.tablePrefix"] = tablePrefix,
                    ["quartz.jobStore.driverDelegateType"] = driverDelegateType,
                    ["quartz.dataSource.default.connectionString"] = connectionString,
                    ["quartz.dataSource.default.provider"] = provider,
                    ["quartz.plugin.recentHistory.type"] = recentHistoryType,
                    ["quartz.plugin.recentHistory.storeType"] = storeType,
                    ["quartz.serializer.type"] = serializerType,
                    ["quartz.jobStore.clustered"] = runAsCluster.ToString(),
                };

                return clientOrServerProperties;
            }

            public NameValueCollection GetClientSettings(NameValueCollection commonSettings, string proxyAddress)
            {

                if (!string.IsNullOrWhiteSpace(proxyAddress))
                {
                    var clientConfigurationProperties = new NameValueCollection
                    {
                        ["quartz.scheduler.proxy"] = "true",
                        ["quartz.scheduler.proxy.address"] = proxyAddress
                    };

                    commonSettings.Add(clientConfigurationProperties);
                }

                return commonSettings;
            }

            public NameValueCollection GetServerSettings(NameValueCollection commonSettings, string serverPort, bool rejectRemoteRequests)
            {

                var serverConfigurationProperties = new NameValueCollection
                {
                    ["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz",
                    ["quartz.scheduler.exporter.port"] = serverPort,
                    ["quartz.scheduler.exporter.bindName"] = "QuartzScheduler",
                    ["quartz.scheduler.exporter.channelType"] = "tcp",
                    ["quartz.scheduler.exporter.channelName"] = "httpQuartz",
                    ["quartz.scheduler.exporter.rejectRemoteRequests"] = rejectRemoteRequests.ToString()
                };

                commonSettings.Add(serverConfigurationProperties);
                return commonSettings;
            }

            public NameValueCollection GetStandAloneSettings(NameValueCollection commonSettings)
            {
                //Nothing to change as of yet
                return commonSettings;
            }


    }
}
