using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderMappings : Profile
    {
        public AccountProviderMappings()
        {
            CreateMap<AccountProvider, AccountProviderBasicDto>();
            
            CreateMap<AccountProvider, Application.Queries.GetAccountProvider.Dtos.AccountProviderDto>()
                .ForMember(d => d.AccountLegalEntities, o => o.MapFrom(s => s.Account.AccountLegalEntities.OrderBy(ale => ale.Name)));
            
            CreateMap<AccountProvider, Application.Queries.GetAccountProviders.Dtos.AccountProviderDto>()
                .ForMember(d => d.AccountProviderLegalEntitiesWithPermissionsCount, o => o.MapFrom(s => s.AccountProviderLegalEntities.Count(aple => aple.Permissions.Any())));
        }
    }
}