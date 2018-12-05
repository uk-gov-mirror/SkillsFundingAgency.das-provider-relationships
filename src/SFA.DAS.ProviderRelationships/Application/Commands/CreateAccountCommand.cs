using System;
using MediatR;

namespace SFA.DAS.ProviderRelationships.Application.Commands
{
    public class CreateAccountCommand : IRequest
    {
        public long AccountId { get; }
        public string HashedId { get; }
        public string PublicHashedId { get; }
        public string Name { get; }
        public DateTime Created { get; }

        public CreateAccountCommand(long accountId, string hashedId, string publicHashedId, string name, DateTime created)
        {
            AccountId = accountId;
            HashedId = hashedId;
            PublicHashedId = publicHashedId;
            Name = name;
            Created = created;
        }
    }
}