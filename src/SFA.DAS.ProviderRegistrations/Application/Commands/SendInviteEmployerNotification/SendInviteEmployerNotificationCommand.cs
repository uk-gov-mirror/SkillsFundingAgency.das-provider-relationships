using System;
using MediatR;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendInviteEmployerNotification
{
    public class SendInviteEmployerNotificationCommand : IRequest
    {
        public string EmployerName { get; set; }
        public string EmployerOrganisation { get; set; }
        public string ProviderName { get; set; }
        public string ProviderOrganisation { get; set; }
        public string EmployerEmailAddress { get; set; }
        public string BeginJourneyLink { get; set; }
        public string UnsubscribeLink { get; set; }
        public string ReportProviderLink { get; set; }


        public SendInviteEmployerNotificationCommand
            (string employerName, string providerName, string providerOrganisation, string employerEmailAddress, string beginJourneyLink, string unsubscribeLink, string reportProviderLink, string employerOrganisation)
        {
            EmployerName = employerName;
            ProviderName = providerName;
            ProviderOrganisation = providerOrganisation;
            EmployerEmailAddress = employerEmailAddress;
            BeginJourneyLink = beginJourneyLink;
            UnsubscribeLink = unsubscribeLink;
            ReportProviderLink = reportProviderLink;
            EmployerOrganisation = employerOrganisation;
        }
    }
}