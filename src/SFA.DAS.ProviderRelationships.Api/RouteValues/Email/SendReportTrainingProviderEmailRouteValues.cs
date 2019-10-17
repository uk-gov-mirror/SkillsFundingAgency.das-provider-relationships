using System;

namespace SFA.DAS.ProviderRelationships.Api.RouteValues.Email
{
    public class SendReportTrainingProviderEmailRouteValues
    {
        public string EmployerEmailAddress { get; set; }
        public DateTime EmailReported { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingProviderSenderName { get; set; }
        public DateTime EmailSent { get; set; }
    }
}