using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.ProviderRelationships.Api.Logging;
using SFA.DAS.ProviderRelationships.Domain.Data;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.Api.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            For<ILoggingContext>().Use(c => GetLoggingContext(c));
            For<IProviderRelationshipsDbContextFactory>().Use<DbContextWithNServiceBusTransactionFactory>();
        }

        private ILoggingContext GetLoggingContext(IContext context)
        {
            LoggingContext loggingContext = null;

            try
            {
                loggingContext = new LoggingContext(context.GetInstance<HttpContextBase>());
            }
            catch (HttpException)
            {
            }

            return loggingContext;
        }

        //todo not going to do this anymore, going to use the nservicebus publish instead
        //private void ConfigureNotificationsApi(ProviderRelationshipsConfiguration config)
        //{
        //    HttpClient httpClient;

        //    if (string.IsNullOrWhiteSpace(config.NotificationApiConfiguration.ClientId))
        //    {
        //        httpClient = new HttpClientBuilder()
        //            .WithBearerAuthorisationHeader(new JwtBearerTokenGenerator(config.NotificationApiConfiguration))
        //            .Build();
        //    }
        //    else
        //    {
        //        httpClient = new HttpClientBuilder()
        //            .WithBearerAuthorisationHeader(new AzureActiveDirectoryBearerTokenGenerator(config.NotificationApiConfiguration))
        //            .Build();
        //    }

        //    For<INotificationsApi>().Use<NotificationsApi>().Ctor<HttpClient>().Is(httpClient);

        //    For<INotificationsApiClientConfiguration>().Use(config.NotificationApiConfiguration);
        //}
    }
}