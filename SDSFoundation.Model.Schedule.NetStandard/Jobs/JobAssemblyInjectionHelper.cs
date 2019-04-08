using Microsoft.Extensions.DependencyModel;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace SDSFoundation.Model.Schedule.NetStandard.Jobs
{


    public static class JobAssemblyInjectionHelper
    {
        private static List<LocalJob> allLocalJobs = new List<LocalJob>();

        public static IScheduler CreateAndInitializeSchedulerWithCommonSettings(string clientId, string instanceId, string connectionString, bool runAsCluster, string rootJobsFolder, string excludedJobFilterExpression = "", string includedJobFilterExpression = "", int threadCount = 1, int misfireThreshold = 60000)
        {
            if (!string.IsNullOrWhiteSpace(rootJobsFolder) && (allLocalJobs == null || allLocalJobs.Count == 0))
            {
                allLocalJobs = JobAssemblyInjectionHelper.GetLocalJobs(rootJobsFolder);
            }


            var quartzHelper = new QuartzHelper(clientId);
            var commonSettings = quartzHelper.GetCommonSettings(instanceId, connectionString, runAsCluster, threadCount, misfireThreshold);

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(commonSettings);
            var clusteredScheduleFactoryTask = schedulerFactory.GetScheduler();
            clusteredScheduleFactoryTask.Wait();
            var scheduler = clusteredScheduleFactoryTask.Result;

            if (!string.IsNullOrWhiteSpace(rootJobsFolder))
            {
                var filteredJobs = allLocalJobs.Where(x => 
                            (!string.IsNullOrWhiteSpace(includedJobFilterExpression) && x.FileName.ToLower().Contains(includedJobFilterExpression))
                            ||
                            (!string.IsNullOrWhiteSpace(excludedJobFilterExpression) && !x.FileName.ToLower().Contains(excludedJobFilterExpression))
                            ).ToList();
                JobAssemblyInjectionHelper.AddMissingJobsToScheduler(scheduler, filteredJobs);
            }

            return scheduler;
        }

        public static void RunJobs(IScheduler scheduler, List<LocalJob> localJobsList)
        {


            if (localJobsList != null && localJobsList.Count > 0)
            {
                foreach (var localJob in localJobsList)
                {
                    //Load the job from the file system
                    var currentAssembly = JobAssemblyInjectionHelper.LoadFile(localJob.FileName);
                    var currentType = currentAssembly.GetType(localJob.JobClassName);
 
                    //var currentJob = ObjectUtils.InstantiateType<IJob>(currentType);


                    var injectedJob = JobBuilder.Create(currentType)
                      .WithIdentity(localJob.JobName, localJob.JobGroup)
                      .OfType(currentType)
                      .Build();


                }
            }

        }

        public static void AddMissingJobsToScheduler(IScheduler scheduler, List<LocalJob> localJobsList)
        {

            var keysTask = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
            keysTask.Wait();


            var jobKeys = keysTask.Result;


            if (localJobsList != null && localJobsList.Count > 0)
            {
                //Check for the existance of the job / group combination
                foreach (var localJob in localJobsList)
                {
                    bool keyExists = true;

                    if (jobKeys != null && jobKeys.Count > 0)
                    {
                        var existingKeyInDatabase = jobKeys.Where(x => x.Name == localJob.JobName && x.Group == localJob.JobGroup).FirstOrDefault();
                        if (existingKeyInDatabase == null)
                        {
                            keyExists = false;
                        }
                    }
                    else
                    {
                        keyExists = false;
                    }

                    if (!keyExists)
                    {
                        IJobDetail jobDetail = null;
                        var isClusteredJob = localJob.FileName.ToLower().Contains("clusteredjobs");

                        var jobAssembly = JobAssemblyInjectionHelper.LoadFile(localJob.FileName);
                        var jobAssemblyType = jobAssembly.GetType(localJob.JobClassName);

                        if (isClusteredJob) //All clustered jobs should store state durably by default
                        {
                            jobDetail = JobBuilder.Create().WithIdentity(localJob.JobName, localJob.JobGroup).OfType(jobAssemblyType).StoreDurably().Build();

                        }
                        else //All other jobs should not store state durably by default
                        {
                            jobDetail = JobBuilder.Create().WithIdentity(localJob.JobName, localJob.JobGroup).OfType(jobAssemblyType).StoreDurably().Build();
                        }


                        //var trigger =  TriggerBuilder.Create()
                        // .WithIdentity(localJob.JobName + " Default Trigger", localJob.JobGroup)
                        // .WithSimpleSchedule()
                        // .Build();

                        //var scheduleJobTask = scheduler.ScheduleJob(jobDetail, trigger);
                        //scheduleJobTask.Wait();

                        //var jobStatus = scheduleJobTask.Status;

                        var addJobTask = scheduler.AddJob(jobDetail, false);
                        addJobTask.Wait();
                    }
                }
            }
        }

        /// <summary>
        /// Interogates the file system starting at the root job folder [ApplicationSettings.RootJobsFolder]
        /// </summary>
        /// <param name="directoryPath">Optional.  Used to traverse paths</param>
        /// <returns>Dictionary<JobName, JobGroup> of jobs that are configured for execution on the current machine</returns>
        public static List<LocalJob> GetLocalJobs(string rootJobsFolder, string directoryPath = "")
        {
            var result = new List<LocalJob>();
            if ((!string.IsNullOrWhiteSpace(directoryPath) && Directory.Exists(directoryPath)) || (!string.IsNullOrWhiteSpace(rootJobsFolder) && Directory.Exists(rootJobsFolder)))
            {

                string[] jobDirectories = null;
                if (!string.IsNullOrWhiteSpace(directoryPath))
                {
                    jobDirectories = Directory.GetDirectories(directoryPath);
                }
                else
                {
                    jobDirectories = Directory.GetDirectories(rootJobsFolder);
                }

                if (jobDirectories != null && jobDirectories.Count() > 0)
                {
                    foreach (var currentDirectory in jobDirectories)
                    {
                        //Get all assemblies 
                        var localAssemblyFileNames = Directory.GetFiles(currentDirectory, "*.dll");

                        if (localAssemblyFileNames != null && localAssemblyFileNames.Count() > 0)
                        {
                            foreach (var currentAssemblyPathAndFileName in localAssemblyFileNames)
                            {
                                var currentAssemblyFileName = currentAssemblyPathAndFileName.Split('\\').Last();
                                //Get all classes in the current assembly that implement IJob

                                try
                                {
                                    var currentAssembly = LoadFile(currentAssemblyPathAndFileName);
                                 
                                    var assemblyTypes = currentAssembly.GetTypes();

                                    if (assemblyTypes != null && assemblyTypes.Count() > 0)
                                    {
                                        foreach (var assemblyType in assemblyTypes)
                                        {
                                            if (assemblyType.GetInterface(typeof(Quartz.IJob).Name) != null)
                                            {
                                                var jobName = assemblyType.FullName.Split('.').Last();
                                                var localJob = new LocalJob() { FileName = currentAssemblyPathAndFileName, JobName = jobName, JobClassName = assemblyType.FullName };
                                                var currentJobGroupName = string.Format("{0}", currentAssembly.GetName().Name).Replace('\\', '_');
                                     
                                                if (currentAssemblyPathAndFileName.ToLower().Contains("clusteredjobs"))
                                                {
                                                    //Replace the "clusteredjobs" start with empty string
  
                                                    var groupNameExtension = currentDirectory.Replace(rootJobsFolder, "").Replace('\\', '_').TrimStart('.').Trim();
                                                    currentJobGroupName = string.Format("{0}_{1}", currentJobGroupName, groupNameExtension);
                                                }
                                              
                                                localJob.JobGroup = currentJobGroupName.Replace('.', '_');

                                                result.Add(localJob);
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }

                        //Get all child jobs
                        var childJobs = GetLocalJobs(rootJobsFolder, currentDirectory);
                        if (childJobs != null && childJobs.Count > 0)
                        {
                            foreach (var childJob in childJobs)
                            {
                                result.Add(childJob);
                            }

                        }
 
                    }
                }
            }

            allLocalJobs = result;

            //Wire up assembly resolver so that the application can locate injected assembly types
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            return result;
        }
 

        public static System.Reflection.Assembly LoadFile(string path)
        {
            System.Reflection.Assembly assembly = null;

#if NET_CORE
            // Requires nuget - System.Runtime.Loader
            assembly = System.Runtime.Loader.AssemblyLoadContext.Default
                   .LoadFromAssemblyPath(path);
#else
            assembly = System.Reflection.Assembly.LoadFile(path);
#endif

            // System.Type myType = ass.GetType("Custom.Thing.SampleClass");
            // object myInstance = Activator.CreateInstance(myType);
            return assembly;
        }


        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Ignore missing resources
            if (args.Name.Contains(".resources"))
                return null;

            // check for assemblies already loaded
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;


            string filename = args.Name.Split(',')[0] + ".dll".ToLower();



            foreach (var localJob in allLocalJobs)
            {
                if (localJob.FileName.EndsWith(filename))
                {
                    try
                    {
                        return System.Reflection.Assembly.LoadFrom(localJob.FileName);
                    }
                    catch (Exception ex)
                    {
                        //Do nothing.  Try the next type if one exists
                    }
                }

            }

            return null;
        }


    }
}
