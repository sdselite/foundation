using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
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
    }
}
