using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.Api.Client.Configuration;

namespace SFA.DAS.ProviderRelationships.Api.Client.ReadStore.Data
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ReadStoreConfiguration _configuration;

        public DocumentClientFactory(ReadStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDocumentClient CreateDocumentClient()
        {
            //todo: more appropriate retry policy for read-only
            var connectionPolicy = new ConnectionPolicy
            {
                RetryOptions =
                {
                    MaxRetryAttemptsOnThrottledRequests = 3,
                    MaxRetryWaitTimeInSeconds = 2
                }
            };

            return new DocumentClient(new Uri(_configuration.Uri), _configuration.AuthKey, connectionPolicy);
        }
    }
}