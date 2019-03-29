using Microsoft.Extensions.DependencyModel;
using Quartz;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace SDSFoundation.Model.Schedule.NetStandard.Jobs
{


    public static class JobAssemblyInjectionHelper
    {
        public static void RunJobs(IScheduler scheduler, List<LocalJob> localJobsList)
        {


            if (localJobsList != null && localJobsList.Count > 0)
            {
                var keysTask = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());
                keysTask.Wait();

                var keys = keysTask.Result;

                foreach (var localJob in localJobsList)
                {
                    var jobKey = keys.Where(x => x.Group == localJob.JobGroup && x.Name == localJob.JobName).FirstOrDefault();
                    var jobTriggersTask = scheduler.GetTriggersOfJob(jobKey);
                    jobTriggersTask.Wait();
                    var jobTriggers = jobTriggersTask.Result;

                    //Load the job from the file system
                    var currentAssembly = JobAssemblyInjectionHelper.LoadFile(localJob.FileName);
                    var currentType = currentAssembly.GetType(localJob.JobName);

                    var injectedJob = JobBuilder.Create(currentType)
                      .WithIdentity(localJob.JobName, localJob.JobGroup)
                      .Build();

                    if (jobTriggers != null && jobTriggers.Count > 0)
                    {
                        foreach (var jobTrigger in jobTriggers)
                        {
                            var scheduleTimeTask = scheduler.ScheduleJob(injectedJob, jobTrigger);
                            scheduleTimeTask.Wait();

                            var taskScheduledAt = scheduleTimeTask.Result;
                        }

                    }

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
                        var jobAssemblyType = jobAssembly.GetType(localJob.JobName);

                        if (isClusteredJob) //All clustered jobs should store state durably by default
                        {
                            jobDetail = JobBuilder.Create().WithIdentity(localJob.JobName, localJob.JobGroup).OfType(jobAssemblyType).StoreDurably().Build();

                        }
                        else //All other jobs should not store state durably by default
                        {
                            jobDetail = JobBuilder.Create().WithIdentity(localJob.JobName, localJob.JobGroup).OfType(jobAssemblyType).StoreDurably().Build();
                        }

                        ////Add the job to the scheduler
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
        public static List<LocalJob> GetLocalJobs(string rootJobsFolder, string instanceName, string instanceId, string directoryPath = "")
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
                                                var localJob = new LocalJob() { FileName = currentAssemblyPathAndFileName, JobName = assemblyType.FullName };

                                                var currentJobGroupName = string.Empty;

                                                if (!string.IsNullOrWhiteSpace(rootJobsFolder) && !string.IsNullOrWhiteSpace(assemblyType.FullName))
                                                {
                                                    currentJobGroupName = currentAssemblyPathAndFileName.Replace(rootJobsFolder, "").Replace(currentAssemblyFileName, "").TrimStart('\\').Replace('\\', '_');
                                                }

                                                if (!currentJobGroupName.ToLower().StartsWith("clusteredjobs"))
                                                {
                                                    currentJobGroupName = string.Format("{0}_{1}", instanceName, instanceId);
                                                }
                                                else
                                                {
                                                    //Replace the "clusteredjobs" start with empty string
                                                    var clusteredJobsText = currentJobGroupName.Split('_').First();

                                                    currentJobGroupName = currentJobGroupName.Replace(clusteredJobsText, "").TrimStart('_').TrimEnd('_');
                                                }

                                                localJob.JobGroup = currentJobGroupName;

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
                        var childJobs = GetLocalJobs(rootJobsFolder, instanceName, instanceId, currentDirectory);
                        if (childJobs != null && childJobs.Count > 0)
                        {
                            foreach (var childJob in childJobs)
                            {
                                result.Add(childJob);
                            }

                        }

                        ////Get sub directories and parse for jobs
                        //var subDirectories = Directory.GetDirectories(currentDirectory);
                        //if(subDirectories != null && subDirectories.Count() > 0)
                        //{
                        //    foreach (var subdirectory in subDirectories)
                        //    {
                        //        var subDirectoryJobs = GetLocalJobs(rootJobsFolder, subdirectory);
                        //        if(subDirectoryJobs != null && subDirectoryJobs.Count > 0)
                        //        {
                        //            foreach (var subDirectoryJob in subDirectoryJobs)
                        //            {
                        //                result.Add(subDirectoryJob.Key, subDirectoryJob.Value);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }


            return result;
        }


        //public static Dictionary<string, string> GetJobGroupsDictionary(string rootJobsFolder, string instanceName, string instanceId, Dictionary<string, string> localJobsDictionary)
        //{
        //    Dictionary<string, string> result = new Dictionary<string, string>();

        //    if(localJobsDictionary != null && localJobsDictionary.Count > 0)
        //    {
        //        foreach (var localJobEntry in localJobsDictionary)
        //        {
        //            var currentJobGroupName = string.Empty;

        //            if (!string.IsNullOrWhiteSpace(rootJobsFolder) && !string.IsNullOrWhiteSpace(localJobEntry.Key))
        //            {
        //                currentJobGroupName = localJobEntry.Key.Replace(rootJobsFolder, "").Split(',').First().TrimStart('\\').Replace('\\', '_');
        //            }

        //            if (!currentJobGroupName.ToLower().StartsWith("clusteredjobs"))
        //            {
        //                currentJobGroupName = string.Format("{0}_{1}", instanceName, instanceId);
        //            }
        //            else
        //            {
        //                //Replace the "clusteredjobs" start with empty string
        //                var clusteredJobsText = currentJobGroupName.Split('_').First();

        //                currentJobGroupName = currentJobGroupName.Replace(clusteredJobsText, "").TrimStart('_');
        //            }

        //            result.Add(localJobEntry.Key, currentJobGroupName);
        //        }
        //    }



            
        //    return result;
        //}

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

 
    }
}
