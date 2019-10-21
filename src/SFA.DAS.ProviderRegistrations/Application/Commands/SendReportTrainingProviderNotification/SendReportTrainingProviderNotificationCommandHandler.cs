using System.Collections.Generic;
using MediatR;
using SFA.DAS.Notifications.Api.Client;
using SFA.DAS.Notifications.Api.Types;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.NServiceBus;

namespace SFA.DAS.ProviderRegistrations.Application.Commands.SendReportTrainingProviderNotification
{
    public class SendReportTrainingProviderNotificationCommandHandler : RequestHandler<SendReportTrainingProviderNotificationCommand>
    {
        private readonly INotificationsApi _notificationsApi;
        private readonly IEventPublisher _eventPublisher;

        private const string ReplyToAddress = "noreply@sfa.gov.uk";
        private const string DefaultSubject = "x";
        private const string DefaultSystemId = "x";
        private const string ReportTrainingProviderAddress = "monitoring@esfa.zendesk.com"; //todo config?
        private const string TemplateId = "ReportTrainingProviderNotification_dev"; //todo when this story is complete _dev should be removed and the associated non dev template created on gov notify

        private const string EmployerEmailAddressTokenKey = "employer_email_address";
        private const string EmailReportedTokenKey = "email_reported";
        private const string TrainingProviderTokenKey = "training_provider";
        private const string TrainingProviderSenderNameTokenKey = "training_provider_name";
        private const string EmailSentTokenKey = "email_sent";

        public SendReportTrainingProviderNotificationCommandHandler(INotificationsApi notificationsApi, IEventPublisher eventPublisher)
        {
            _notificationsApi = notificationsApi;
            _eventPublisher = eventPublisher;
        }

        protected override void Handle(SendReportTrainingProviderNotificationCommand request)
        {
            _eventPublisher.Publish(new SendEmailCommand(TemplateId, ReportTrainingProviderAddress, ReplyToAddress, new Dictionary<string, string> {
                { EmployerEmailAddressTokenKey, request.EmployerEmailAddress },
                { EmailReportedTokenKey, request.EmailReported.ToString() }, //todo format datetimes correctly following confirmation on story
                { TrainingProviderTokenKey, request.TrainingProvider },
                { TrainingProviderSenderNameTokenKey, request.TrainingProviderSenderName },
                { EmailSentTokenKey, request.EmailSent.ToString() }}));

            //_notificationsApi.SendEmail(new Email(
            //    systemId: DefaultSystemId,
            //    templateId: TemplateId,
            //    subject: DefaultSubject,
            //    recipientsAddress: ReportTrainingProviderAddress,
            //    replyToAddress: ReplyToAddress,
            //    tokens: new Dictionary<string, string> {
            //        { EmployerEmailAddressTokenKey, request.EmployerEmailAddress },
            //        { EmailReportedTokenKey, request.EmailReported.ToString() }, //todo format datetimes correctly following confirmation on story
            //        { TrainingProviderTokenKey, request.TrainingProvider },
            //        { TrainingProviderSenderNameTokenKey, request.TrainingProviderSenderName },
            //        { EmailSentTokenKey, request.EmailSent.ToString() } //todo format datetimes correctly following confirmation on story
            //    }
            //));
        }
    }
}
