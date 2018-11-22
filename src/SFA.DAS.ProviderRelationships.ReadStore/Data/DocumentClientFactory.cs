﻿using System;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using SFA.DAS.ProviderRelationships.ReadStore.Configuration;

namespace SFA.DAS.ProviderRelationships.ReadStore.Data
{
    internal class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly ProviderRelationshipsReadStoreConfiguration _configuration;

        public DocumentClientFactory(ProviderRelationshipsReadStoreConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDocumentClient CreateDocumentClient()
        {
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