﻿using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SFA.DAS.NServiceBus.ClientOutbox;

namespace SFA.DAS.ProviderRelationships.Jobs.TriggeredJobs
{
    public class ProcessClientOutboxMessagesJob
    {
        private readonly IProcessClientOutboxMessagesJob _processClientOutboxMessagesJob;

        public ProcessClientOutboxMessagesJob(IProcessClientOutboxMessagesJob processClientOutboxMessagesJob)
        {
            _processClientOutboxMessagesJob = processClientOutboxMessagesJob;
        }

        public Task Run([TimerTrigger("0 */10 * * * *", RunOnStartup = true)] TimerInfo timer, TraceWriter logger)
        {
            return _processClientOutboxMessagesJob.RunAsync();
        }
    }
}