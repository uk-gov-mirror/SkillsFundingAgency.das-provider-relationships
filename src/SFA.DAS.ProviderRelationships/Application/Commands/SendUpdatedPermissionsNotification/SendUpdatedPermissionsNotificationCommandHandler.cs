﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PAS.Account.Api.Client;
using SFA.DAS.PAS.Account.Api.Types;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.ProviderRelationships.Application.Commands.SendUpdatedPermissionsNotification
{
    public class SendUpdatedPermissionsNotificationCommandHandler : AsyncRequestHandler<SendUpdatedPermissionsNotificationCommand>
    {
        private readonly IPasAccountApiClient _client;
        private readonly Lazy<ProviderRelationshipsDbContext> _db;
        private const string TemplateId = "UpdatedPermissionsEventNotification";

        public SendUpdatedPermissionsNotificationCommandHandler(IPasAccountApiClient client, Lazy<ProviderRelationshipsDbContext> db)
        {
            _client = client;
            _db = db;
        }

        protected override async Task Handle(SendUpdatedPermissionsNotificationCommand request, CancellationToken cancellationToken)
        {
            var organisation = await _db.Value.AccountLegalEntities.SingleAsync(a => a.Id == request.AccountLegalEntityId, cancellationToken);

            var provider = await _db.Value.Providers.SingleAsync(p => p.Ukprn == request.Ukprn, cancellationToken);

            var operations = GetPermissionsEmailText(request.PreviousOperations, request.GrantedOperations, organisation.Name);

            if (!string.IsNullOrEmpty(operations))
            {
                await _client.SendEmailToAllProviderRecipients(request.Ukprn, new ProviderEmailRequest {
                    TemplateId = TemplateId,
                    Tokens = new Dictionary<string, string>
                    {
                        { "training_provider_name", provider.Name },
                        { "organisation_name", organisation.Name },
                        { "permissions_set", operations }
                    }
                });
            }
        }

        private string GetPermissionsEmailText(HashSet<Operation> previousOperations, HashSet<Operation> grantedOperations, string organisationName)
        {
            string permissionsUpdatedText = string.Empty;

            if (!previousOperations.Any())
            {
                permissionsUpdatedText = $"{organisationName} has changed your apprenticeship service permissions. " +
                    "\r\n \r\n" +
                    $"You can now {GetOperationText(grantedOperations)} on their behalf."; 
            }

            var removedOperations = previousOperations.Except(grantedOperations);
            var newOperations = grantedOperations.Except(previousOperations);
            if (removedOperations.Any())
            {
                if (!newOperations.Any())
                {
                    var remainingOperationsText = grantedOperations.Count > 0
                        ? $"You can still {GetOperationText(grantedOperations)} on their behalf."
                        : $"You cannot do anything in the apprenticeship service on their behalf at the moment.";

                    permissionsUpdatedText = $"{organisationName} has removed your permission to {GetOperationText(removedOperations)}." +
                        "\r\n \r\n" +
                        $"{remainingOperationsText}";
                }
                else
                {
                    permissionsUpdatedText = $"{organisationName} has:" +
                        "\r\n \r\n" +
                        $"\u2022 given you permission to {GetOperationText(newOperations)}" +
                        $"{Environment.NewLine}" +
                        $"\u2022 removed you permission to {GetOperationText(removedOperations)}";
                }
            }

            return permissionsUpdatedText;
        }

        private string GetOperationText(IEnumerable<Operation> operations)
        {
            return  string.Join(" and ", operations?
              .Where(ope => !string.IsNullOrEmpty(ope.ToString()))
              .Select(ope => ope.GetDisplayName().ToLower()));
        }
    }
}