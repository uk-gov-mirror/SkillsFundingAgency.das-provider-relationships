﻿using System.ComponentModel.DataAnnotations;
using MediatR;
using SFA.DAS.ProviderRelationships.Validation;

namespace SFA.DAS.ProviderRelationships.Application
{
    public class SearchProvidersQuery : IRequest<SearchProvidersQueryResponse>
    {
        [Required(ErrorMessage = ErrorMessages.InvalidUkprn)]
        [RegularExpression(@"[\d+]{8}", ErrorMessage = ErrorMessages.InvalidUkprn)]
        public string Ukprn { get; set; }
    }
}