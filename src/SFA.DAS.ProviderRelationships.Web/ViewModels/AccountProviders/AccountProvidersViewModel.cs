using System.Collections.Generic;
using SFA.DAS.ProviderRelationships.Dtos;

namespace SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviders
{
    public class AccountProvidersViewModel
    {
        public List<AccountProviderSummaryDto> AccountProviders { get; set; }
        public int AccountLegalEntitiesCount { get; set; }
    }
}