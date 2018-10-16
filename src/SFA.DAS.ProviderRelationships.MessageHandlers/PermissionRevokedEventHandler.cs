using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.ProviderRelationships.Data;
using SFA.DAS.ProviderRelationships.Messages;

namespace SFA.DAS.ProviderRelationships.MessageHandlers
{
    public class PermissionRevokedEventHandler : IHandleMessages<PermissionRevokedEvent>
    {
        public PermissionRevokedEventHandler(Lazy<ProviderRelationshipsDbContext> db)
        {
        }

        public Task Handle(PermissionRevokedEvent message, IMessageHandlerContext context)
        {
            /*_db.Delete(new Permission
            {
                AccountLegalEntityId = message.AccountLegalEntityId,
                Type = message.Type
            });*/

            return Task.CompletedTask;
        }
    }
}
