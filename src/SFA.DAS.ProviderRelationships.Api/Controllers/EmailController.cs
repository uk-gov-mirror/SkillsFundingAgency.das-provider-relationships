using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using MediatR;
using SFA.DAS.ProviderRegistrations.Application.Commands.SendReportTrainingProviderNotification;
using SFA.DAS.ProviderRelationships.Api.RouteValues.Email;

namespace SFA.DAS.ProviderRelationships.Api.Controllers
{
    [RoutePrefix("email")]
    public class EmailController : ApiController
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route]
        public async Task<IHttpActionResult> Get([FromUri] SendReportTrainingProviderEmailRouteValues routeValues, CancellationToken cancellationToken)
        {
            await _mediator.Send(new SendReportTrainingProviderNotificationCommand(
                routeValues.EmployerEmailAddress, routeValues.EmailReported, routeValues.TrainingProvider,
                routeValues.TrainingProviderSenderName, routeValues.EmailSent), cancellationToken);

            return Ok();
        }
    }
}