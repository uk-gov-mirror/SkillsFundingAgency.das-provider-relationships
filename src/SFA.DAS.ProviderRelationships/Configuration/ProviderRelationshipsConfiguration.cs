﻿using SFA.DAS.ProviderRelationships.Authentication;
using SFA.DAS.ProviderRelationships.Extensions;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    public class ProviderRelationshipsConfiguration
    {
        public string AllowedHashstringCharacters { get; set; }
        public string Hashstring { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string EmployerPortalBaseUrl { get; set; }
        public string NServiceBusLicense { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public IdentityServerConfiguration Identity { get; set; }

        public ProviderRelationshipsConfiguration InitialTransform()
        {
            NServiceBusLicense = NServiceBusLicense.HtmlDecode();
            return this;
        }
    }
}