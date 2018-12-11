using SFA.DAS.CosmosDb;

namespace SFA.DAS.ProviderRelationships.Configuration
{
    //todo: main config will contain one of these
    //todo: types now has a reference to SFA.DAS.CosmosDb!
    //need 2 of these
    public class ProviderRelationshipsReadStoreConfiguration : ICosmosDbConfiguration
    {
        public string Uri { get; set; }
        public string AuthKey { get; set; }
    }
}