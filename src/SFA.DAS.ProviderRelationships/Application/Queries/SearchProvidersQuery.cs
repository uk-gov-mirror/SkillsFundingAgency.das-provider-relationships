﻿using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Queries
{
    public class SearchProvidersQuery : IRequest<SearchProvidersQueryResponse>
    {
        public long AccountId { get; }
        public long Ukprn { get; }

        public SearchProvidersQuery(long accountId, long ukprn)
        {
            AccountId = accountId;
            Ukprn = ukprn;
        }
    }
}