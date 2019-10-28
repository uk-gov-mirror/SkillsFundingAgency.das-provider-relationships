using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.ProviderRegistrations.Configuration;
using SFA.DAS.ProviderRegistrations.Web.Authorization;

namespace SFA.DAS.ProviderRegistrations.Web.Extensions
{
    public static class IWebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(
                    ProviderRegistrationsConfigurationKeys.ProviderRegistrations));
        }
    }

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDasAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ProviderMatch", policy => policy.Requirements.Add(new ProviderRequirement()));
            });

            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IAuthorizationHandler, ProviderHandler>();

            return services;
        }
    }
}