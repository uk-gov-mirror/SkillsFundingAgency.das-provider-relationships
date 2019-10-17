using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendReportTrainingProviderNotification
{
    public class SendReportTrainingProviderNotificationCommand : IRequest
    {
        public string EmployerEmailAddress { get; set; }
        public DateTime EmailReported { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingProviderSenderName { get; set; }
        public DateTime EmailSent { get; set; }

        public SendReportTrainingProviderNotificationCommand
        (string employerEmailAddress, DateTime emailReported, string trainingProvider, string trainingProviderSenderName, DateTime emailSent)
        {
            EmployerEmailAddress = employerEmailAddress;
            EmailReported = emailReported;
            TrainingProvider = trainingProvider;
            TrainingProviderSenderName = trainingProviderSenderName;
            EmailSent = emailSent;
        }
    }
}
