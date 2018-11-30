using System.Linq;
using AutoMapper;
using SFA.DAS.ProviderRelationships.Dtos;
using SFA.DAS.ProviderRelationships.Models;
using SFA.DAS.ProviderRelationships.Types.Dtos;

namespace SFA.DAS.ProviderRelationships.Mappings
{
    public class AccountProviderLegalEntityMappings : Profile
    {
        public AccountProviderLegalEntityMappings()
        {
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntityBasicDto>()
                .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.AccountProvider.Provider.Name));
                
            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntitySummaryDto>()
                .ForMember(d => d.Operations, o => o.MapFrom(s => s.Permissions.Select(p => p.Operation)));

            CreateMap<AccountProviderLegalEntity, AccountProviderLegalEntityDto>()
                .ForMember(d => d.AccountId, o => o.MapFrom(s => s.AccountProvider.Account.Id))
                .ForMember(d => d.AccountPublicHashedId, o => o.MapFrom(s => s.AccountProvider.Account.PublicHashedId))
                .ForMember(d => d.AccountName, o => o.MapFrom(s => s.AccountProvider.Account.Name));
        }
    }
}