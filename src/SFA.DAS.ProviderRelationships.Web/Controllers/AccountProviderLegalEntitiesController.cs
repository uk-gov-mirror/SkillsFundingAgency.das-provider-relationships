﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MediatR;
using MoreLinq.Extensions;
using SFA.DAS.Authorization.EmployerFeatures;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderRelationships.Application.Commands.UpdatePermissions;
using SFA.DAS.ProviderRelationships.Application.Queries.GetAccountProvider;
using SFA.DAS.ProviderRelationships.Application.Queries.GetUpdatedAccountProviderLegalEntity;
using SFA.DAS.ProviderRelationships.Web.Extensions;
using SFA.DAS.ProviderRelationships.Web.RouteValues.AccountProviderLegalEntities;
using SFA.DAS.ProviderRelationships.Web.RouteValues.Operations;
using SFA.DAS.ProviderRelationships.Web.Urls;
using SFA.DAS.ProviderRelationships.Web.ViewModels.AccountProviderLegalEntities;
using SFA.DAS.Validation.Mvc;

namespace SFA.DAS.ProviderRelationships.Web.Controllers
{
    [DasAuthorize(EmployerFeature.ProviderRelationships, EmployerUserRole.Owner)]
    [RoutePrefix("accounts/{accountHashedId}/providers/{accountProviderId}/legalentities/{accountLegalEntityId}")]
    public class AccountProviderLegalEntitiesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IEmployerUrls _employerUrls;

        public AccountProviderLegalEntitiesController(IMediator mediator, IMapper mapper, IEmployerUrls employerUrls)
        {
            _mediator = mediator;
            _mapper = mapper;
            _employerUrls = employerUrls;
        }
       
        [HttpGet]
        [HttpNotFoundForNullModel]
        [Route]
        public ActionResult Get(GetAccountProviderLegalEntityRouteValues routeValues)
        {
            return RedirectToAction("Set", "Operations", new OperationRouteValue {
                AccountProviderId = routeValues.AccountProviderId.Value,
                AccountLegalEntityId = routeValues.AccountLegalEntityId.Value
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route]
        public ActionResult Get(GetAccountProviderLegalEntityViewModel model)
        {
            TempData.Set(model.Operations);

            return RedirectToAction("Update", new UpdateAccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProviderId.Value, AccountLegalEntityId = model.AccountLegalEntityId.Value });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update")]
        public async Task<ActionResult> Update(UpdateAccountProviderLegalEntityViewModel model)
        {
            var operations = model.Operations.Where(o => o.IsEnabled.HasValue && o.IsEnabled.Value).Select(o => o.Value).ToHashSet();
            var command = new UpdatePermissionsCommand(model.AccountId.Value, model.AccountProviderId.Value, model.AccountLegalEntityId.Value, model.UserRef.Value, operations);

            await _mediator.Send(command);

            if (Session["Invitation"] as bool? == true)
            {
                var provider = await _mediator.Send(new GetAccountProviderQuery(model.AccountId.Value, model.AccountProviderId.Value));
                return Redirect($"{_employerUrls.Account()}/addedprovider/{HttpUtility.UrlEncode(provider.AccountProvider.ProviderName)}");
            }

            return RedirectToAction("Updated", new UpdatedAccountProviderLegalEntityRouteValues { AccountProviderId = model.AccountProviderId.Value, AccountLegalEntityId = model.AccountLegalEntityId.Value });
        }

        [HttpNotFoundForNullModel]
        [Route("updated")]
        public async Task<ActionResult> Updated(UpdatedAccountProviderLegalEntityRouteValues routeValues)
        {
            if (Session["Invitation"] as bool? == true)
            {
                var provider = await _mediator.Send(new GetAccountProviderQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value));
                return Redirect($"{_employerUrls.Account()}/addedprovider/{HttpUtility.UrlEncode(provider.AccountProvider.ProviderName)}");
            }

            var query = new GetUpdatedAccountProviderLegalEntityQuery(routeValues.AccountId.Value, routeValues.AccountProviderId.Value, routeValues.AccountLegalEntityId.Value);
            var result = await _mediator.Send(query);
            var model = _mapper.Map<UpdatedAccountProviderLegalEntityViewModel>(result);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("updated")]
        public ActionResult Updated(UpdatedAccountProviderLegalEntityViewModel model)
        {
            switch (model.Choice)
            {
                case "YourTrainingProviders":
                    return RedirectToAction("Index", "AccountProviders");
                case "AddTrainingProvider":
                    return RedirectToAction("Find", "AccountProviders");
                case "GoToHomepage":
                    return Redirect(_employerUrls.Account());               
                default:
                    throw new ArgumentOutOfRangeException(nameof(model.Choice), model.Choice);
            }
        }
    }
}