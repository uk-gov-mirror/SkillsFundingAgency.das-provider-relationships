using SFA.DAS.ProviderRelationships.ReadStore.Application.Commands;
using SFA.DAS.ProviderRelationships.ReadStore.Application.Queries;
using SFA.DAS.ProviderRelationships.ReadStore.Mediator;
using StructureMap;

namespace SFA.DAS.ProviderRelationships.ReadStore.DependencyResolution
{
    internal class ReadStoreMediatorRegistry : Registry
    {
        public ReadStoreMediatorRegistry()
        {
            For<ReadStoreServiceFactory>().Use<ReadStoreServiceFactory>(c => c.GetInstance);
            For<IReadStoreMediator>().Use<ReadStoreMediator>();
            For<IReadStoreRequestHandler<BatchDeleteRelationshipsCommand, Unit>>().Use<BatchDeleteRelationshipsCommandHandler>();
            For<IReadStoreRequestHandler<BatchUpdateRelationshipAccountLegalEntityNamesCommand, Unit>>().Use<BatchUpdateRelationshipAccountLegalEntityNamesCommandHandler>();
            For<IReadStoreRequestHandler<BatchUpdateRelationshipAccountNamesCommand, Unit>>().Use<BatchUpdateRelationshipAccountNamesCommandHandler>();
            For<IReadStoreRequestHandler<GetRelationshipWithPermissionQuery, GetRelationshipWithPermissionQueryResult>>().Use<GetRelationshipWithPermissionQueryHandler>();
            For<IReadStoreRequestHandler<HasRelationshipWithPermissionQuery, bool>>().Use<HasRelationshipWithPermissionQueryHandler>();
            For<IReadStoreRequestHandler<HasPermissionQuery, bool>>().Use<HasPermissionQueryHandler>();
        }
    }
}