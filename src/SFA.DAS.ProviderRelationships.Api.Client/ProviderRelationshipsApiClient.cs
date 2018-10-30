﻿using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ProviderRelationships.Api.Client.Application;
using SFA.DAS.ProviderRelationships.Types;

namespace SFA.DAS.ProviderRelationships.Api.Client
{
    public class ProviderRelationshipsApiClient : IProviderRelationshipsApiClient
    {
        private readonly IProviderRelationshipService _service;

        public ProviderRelationshipsApiClient(IProviderRelationshipService service)
        {
            _service = service;
        }

        public Task<bool> HasRelationshipWithPermission(ProviderRelationshipRequest request, CancellationToken cancellationToken = default) =>
            _service.HasRelationshipWithPermission(request.Ukprn, request.Permission, cancellationToken);

        public Task<ProviderRelationshipResponse> ListRelationshipsWithPermission(ProviderRelationshipRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}