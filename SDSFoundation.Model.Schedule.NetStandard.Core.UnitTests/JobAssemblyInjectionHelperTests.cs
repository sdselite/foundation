using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using SDSFoundation.Base.AspNetCore;
using SDSFoundation.Model.Schedule.NetStandard.Jobs;
using SDSFoundation.Security.OpenIdDict.Base.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SDSFoundation.Model.Schedule.NetStandard.Core.UnitTests
{
    [TestClass]
    public class JobAssemblyInjectionHelperTests : SecureProgram<JobAssemblyInjectionHelperTests>
    {


        [TestMethod]
        public void GetLocalJobsTest1()
        {
            Initialize(new List<string>().ToArray(), appSettingsFileName: "appsettings.json", maximumLicenseAge: 12);

            var test = Configuration;
            //LogInfo("Logging In.");
            //Login(tokenExpirationSeconds: 3600, ignoreInvalidCertificate: IgnoreInvalidSSLCert);
            //LogInfo("Log In Successful.");

            var allLocalJobs = JobAssemblyInjectionHelper.GetLocalJobs(ApplicationSettings.RootJobsFolder);

            UpdateAndRunAllJobs(allLocalJobs);

            Assert.IsTrue(allLocalJobs != null && allLocalJobs.Count > 0);
        }

        public void UpdateAndRunAllJobs(List<LocalJob> allLocalJobs)
        {
            var quartzHelper = new QuartzHelper(ClientId);

            //var clusteredCommonSettings = quartzHelper.GetCommonSettings(ApplicationSettings.InstanceName, "AUTO", ApplicationSettings.ConnectionString, true, int.Parse(ApplicationSettings.ThreadCount), int.Parse(ApplicationSettings.MisfireThreshold));
            //ISchedulerFactory clusteredScheduleFactory = new StdSchedulerFactory(clusteredCommonSettings);
            //var clusteredScheduleFactoryTask = clusteredScheduleFactory.GetScheduler();
            //clusteredScheduleFactoryTask.Wait();
            //var clusteredScheduler = clusteredScheduleFactoryTask.Result;
         

            //var allClusteredJobs = allLocalJobs.Where(x => x.FileName.ToLower().Contains("clusteredjobs")).ToList();
            //JobAssemblyInjectionHelper.AddMissingJobsToScheduler(clusteredScheduler, allClusteredJobs);
            //JobAssemblyInjectionHelper.RunJobs(clusteredScheduler, allClusteredJobs);

            var nonClusteredCommonSettings = quartzHelper.GetCommonSettings(ApplicationSettings.InstanceId, ApplicationSettings.ConnectionString, false, int.Parse(ApplicationSettings.ThreadCount), int.Parse(ApplicationSettings.MisfireThreshold));
            ISchedulerFactory nonClusteredScheduleFactory = new StdSchedulerFactory(nonClusteredCommonSettings);
            var nonClusteredScheduleFactoryTask = nonClusteredScheduleFactory.GetScheduler();
            nonClusteredScheduleFactoryTask.Wait();
            var nonClusteredScheduler = nonClusteredScheduleFactoryTask.Result;
            

            var allNonClusteredJobs = allLocalJobs.Where(x => !x.FileName.ToLower().Contains("clusteredjobs")).ToList();
            JobAssemblyInjectionHelper.AddMissingJobsToScheduler(nonClusteredScheduler, allNonClusteredJobs);
            JobAssemblyInjectionHelper.RunJobs(nonClusteredScheduler, allNonClusteredJobs);


            nonClusteredScheduler.Start();
            //clusteredScheduler.Start();
        }




    }
}
