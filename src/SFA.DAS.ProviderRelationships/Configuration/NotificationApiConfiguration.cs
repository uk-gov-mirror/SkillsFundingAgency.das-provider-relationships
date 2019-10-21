using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class NotificationApiConfiguration : IAzureActiveDirectoryClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
        public string Tenant { get; set; }
    }
}