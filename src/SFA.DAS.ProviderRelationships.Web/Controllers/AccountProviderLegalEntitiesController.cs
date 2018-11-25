﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands;
using SFA.DAS.ProviderRelationships.Application.Queries;
using SFA.DAS.ProviderRelationships.Extensions;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerRole.Any)]
    [RoutePrefix("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities/{accountLegalEntityId}")]
    public class AccountProviderLegalEntitiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public AccountProviderLegalEntitiesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpNotFoundForNullModel]
        [Route]
        public async Task<ActionResult> Get(GetAccountProviderLegalEntityRouteValues routeValues)
        {
            var query = new GetAccountProviderLegalEntityQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<GetAccountProviderLegalEntityViewModel>(result);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public ActionResult Get(GetAccountProviderLegalEntityViewModel model)
        {
            TempData.Add(model.Operations);
            
            return RedirectToAction("Update", new UpdateAccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProviderId.Value, AccountLegalEntityId = model.AccountLegalEntityId.Value });
        }

        [HttpNotFoundForNullModel]
        [Route("update")]
        public async Task<ActionResult> Update(UpdateAccountProviderLegalEntityRouteValues routeValues)
        {
            var operations = TempData.Get<List<OperationViewModel>>();

            if (operations == null)
            {
                return RedirectToAction("Get", new GetAccountProviderLegalEntityRouteValues { AccountProviderId = routeValues.AccountProviderId.Value, AccountLegalEntityId = routeValues.AccountLegalEntityId.Value });
            }
            
            var query = new GetAccountProviderLegalEntityQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<UpdateAccountProviderLegalEntityViewModel>(result);

            if (model != null)
            {
                model.Operations = operations;
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update")]
        public async Task<ActionResult> Update(UpdateAccountProviderLegalEntityViewModel model)
        {
            var operations = model.Operations.Where(o => o.IsEnabled).Select(o => o.Value).ToHashSet();
            var command = new UpdatePermissionsCommand(model.AccountId.Value, model.AccountProviderId.Value, model.AccountLegalEntityId.Value, model.UserRef.Value, operations);
            var accountProviderLegalEntityId = await _mediator.Send(command);
            
            //return RedirectToAction("Updated", new { AccountProviderLegalEntityId = accountProviderLegalEntityId });
            return RedirectToAction("Get", new GetAccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProviderId.Value, AccountLegalEntityId = model.AccountLegalEntityId });
        }
    }
}