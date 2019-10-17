using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Types;
using SFA.DAS.ProviderRelationships.Application.Commands.SendReportTrainingProviderNotification;
using SFA.DAS.Testing;

namespace SFA.DAS.ProviderRelationships.UnitTests.Application.Commands
{
    [TestFixture]
    [Parallelizable]
    public class SendReportTrainingProviderNotificationCommandHandlerTests : FluentTest<SendReportTrainingProviderNotificationCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingSendUpdatedPermissionsNotificationCommand_ThenShouldCallClientToNotify()
        {
            return RunAsync(f => f.Handle(),
                f => f.Client.Verify(client => client.SendEmail(It.Is<Email>(email =>
                    email.SystemId == "x"
                    && email.TemplateId == "ReportTrainingProviderNotification_dev"
                    && email.Subject == "x"
                    && email.RecipientsAddress == "monitoring@esfa.zendesk.com"
                    && email.ReplyToAddress == "noreply@sfa.gov.uk"
                    && email.Tokens["employer_email_address"] == f.EmployerEmailAddress
                    && email.Tokens["email_reported"] == f.FormattedEmailReported
                    && email.Tokens["training_provider"] == f.TrainingProvider
                    && email.Tokens["training_provider_name"] == f.TrainingProviderSenderName
                    && email.Tokens["email_sent"] == f.FormattedEmailSent
                ))));
        }
    }

    public class SendReportTrainingProviderNotificationCommandHandlerTestsFixture
    {
        public SendReportTrainingProviderNotificationCommand Command { get; set; }
        public IRequestHandler<SendReportTrainingProviderNotificationCommand, Unit> Handler { get; set; }
        public Mock<INotificationsApi> Client { get; set; }
        public string EmployerEmailAddress { get; set; }
        public DateTime EmailReported { get; set; }
        public string TrainingProvider { get; set; }
        public string TrainingProviderSenderName { get; set; }
        public DateTime EmailSent { get; set; }
        public string FormattedEmailReported { get; set; }
        public string FormattedEmailSent { get; set; }

        public SendReportTrainingProviderNotificationCommandHandlerTestsFixture()
        {
            EmployerEmailAddress = "imanemployer@test.com";
            EmailReported = new DateTime(2018, 02, 02);
            FormattedEmailReported = EmailReported.ToString();
            TrainingProvider = "The Learning Tree";
            TrainingProviderSenderName = "Terry Holt";
            EmailSent = new DateTime(2018, 01, 01);
            FormattedEmailSent = EmailSent.ToString();


            Command = new SendReportTrainingProviderNotificationCommand(EmployerEmailAddress, EmailReported, TrainingProvider, TrainingProviderSenderName, EmailSent);
            Client = new Mock<INotificationsApi>();


            Handler = new SendReportTrainingProviderNotificationCommandHandler(Client.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
        }


    }
}