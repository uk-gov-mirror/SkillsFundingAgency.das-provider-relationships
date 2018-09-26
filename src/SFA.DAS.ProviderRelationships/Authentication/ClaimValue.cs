﻿using SFA.DAS.ProviderRelationships.Authentication.Interfaces;
using SFA.DAS.ProviderRelationships.Configuration.Interfaces;

namespace SFA.DAS.ProviderRelationships.Authentication
{
    public sealed class ClaimValue : IClaimValue
    {
        private readonly IClaimIdentifierConfiguration _config;

        public ClaimValue(IClaimIdentifierConfiguration config)
        {
            _config = config;
        }

        public string DisplayName => Generate(_config.DisplayName);
        public string Email => Generate(_config.Email);
        public string Id => Generate(_config.Id);

        //public string FamilyName() => Generate(_config.FamilyName);
        //public string GivenName() => Generate(_config.GivenName);

        private string Generate(string claimType)
        {
            return $"{_config.ClaimsBaseUrl}{claimType}";
        }
    }
}
