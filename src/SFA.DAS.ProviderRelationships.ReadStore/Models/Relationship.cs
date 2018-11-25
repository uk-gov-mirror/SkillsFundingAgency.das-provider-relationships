﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.ReadStore.Models
{
    internal class Relationship : Document
    {
        [JsonProperty("accountId")]
        public long AccountId { get; private set; }

        [JsonProperty("accountLegalEntityId")]
        public long AccountLegalEntityId { get; private set; }

        [JsonProperty("accountProviderId")]
        public long AccountProviderId { get; private set; }

        [JsonProperty("accountProviderLegalEntityId")]
        public long AccountProviderLegalEntityId { get; private set; }

        [JsonProperty("ukprn")]
        public long Ukprn { get; private set; }

        [JsonProperty("operations")]
        public IEnumerable<Operation> Operations { get; private set; } = new HashSet<Operation>();

        [JsonProperty("outboxData")]
        public IEnumerable<OutboxMessage> OutboxData => _outboxData;

        [JsonProperty("created")]
        public DateTime Created { get; private set; }

        [JsonProperty("updated")]
        public DateTime? Updated { get; private set; }

        [JsonProperty("deleted")]
        public DateTime? Deleted { get; private set; }

        [JsonIgnore]
        private readonly List<OutboxMessage> _outboxData = new List<OutboxMessage>();

        public Relationship(long accountId, long accountLegalEntityId, long accountProviderId, long accountProviderLegalEntityId, long ukprn, HashSet<Operation> grantedOperations, DateTime created, string messageId)
            : base(1, "relationship")
        {
            Id = Guid.NewGuid();
            AccountId = accountId;
            AccountLegalEntityId = accountLegalEntityId;
            AccountProviderId = accountProviderId;
            AccountProviderLegalEntityId = accountProviderLegalEntityId;
            Ukprn = ukprn;
            Operations = grantedOperations;
            Created = created;

            AddOutboxMessage(messageId, created);
        }

        [JsonConstructor]
        private Relationship()
        {
        }

        public void Delete(DateTime deleted, string messageId)
        {
            ProcessMessage(messageId, deleted, () =>
            {
                if (IsDeletedDateChronological(deleted))
                {
                    EnsureRelationshipHasNotAlreadyBeenDeleted();
                
                    Deleted = deleted;
                }
            });
        }

        public void UpdatePermissions(HashSet<Operation> grantedOperations, DateTime updated, string messageId)
        {
            ProcessMessage(messageId, updated, () =>
            {
                if (IsUpdatedDateChronological(updated))
                {
                    Operations = grantedOperations;
                    Updated = updated;
                }
            });
        }

        private void AddOutboxMessage(string messageId, DateTime created)
        {
            if (messageId is null)
            {
                throw new ArgumentNullException(nameof(messageId));
            }
            
            _outboxData.Add(new OutboxMessage(messageId, created));
        }

        private void EnsureRelationshipHasNotAlreadyBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires relationship has not already been deleted");
            }
        }

        private bool IsDeletedDateChronological(DateTime deleted)
        {
            return Deleted == null || deleted > Deleted.Value;
        }

        private bool IsMessageAlreadyProcessed(string messageId)
        {
            return OutboxData.Any(m => m.MessageId == messageId);
        }

        private bool IsUpdatedDateChronological(DateTime updated)
        {
            return updated > Created && (Updated == null || updated > Updated.Value);
        }

        private void ProcessMessage(string messageId, DateTime created, Action action)
        {
            if (IsMessageAlreadyProcessed(messageId))
            {
                return;
            }

            AddOutboxMessage(messageId, created);
            
            action();
        }
    }
}