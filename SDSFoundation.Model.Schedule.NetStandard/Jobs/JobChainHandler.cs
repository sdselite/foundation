using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SDSFoundation.Model.Schedule.NetStandard.Jobs
{
    // Quartz.NET 3.x
    public class JobChainHandler : IJobListener
    {
        public string Name => "JobChainHandler";

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken ct)
        {
            return Task.FromResult<object>(null);
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken ct)
        {
            return Task.FromResult<object>(null);
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken ct)
        {
            if (jobException != null)
            {
                return;
            }

            if (context == null)
            {
                throw new ArgumentNullException("Completed job does not have valid Job Execution Context");
            }

            var finishedJob = context.JobDetail;

            await context.Scheduler.DeleteJob(finishedJob.Key);

            var childJobs = finishedJob.JobDataMap.Get(Constants.NextJobKey) as List<JobKey>;

            if (childJobs == null)
            {
                return;
            }

            foreach (var jobKey in childJobs)
            {
                var newJob = await context.Scheduler.GetJobDetail(jobKey);

                if (newJob == null)
                {
                    Debug.WriteLine($"Could not find Job with ID: {jobKey}");
                    continue;
                }

                var oldJobMap = context.JobDetail.JobDataMap.Get(Constants.PayloadKey) as Dictionary<string, object>;

                newJob.JobDataMap.Put(Constants.PayloadKey, oldJobMap);

                await context.Scheduler.AddJob(newJob, true, false);
                await context.Scheduler.TriggerJob(jobKey);
            }
        }
    }
}
