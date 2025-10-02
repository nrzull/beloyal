using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Shops;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Operations.CancelOperation;
using Beloyal.API.Web.Operations.GetOperations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using GetOperationsForAgent = Beloyal.API.Web.AgentOperations.GetOperations.GetOperationsEndpoint;

namespace Beloyal.API.Web.AgentOperations;

[ApiController]
[Route("/api/agent/operations")]
[WithAuth]
[WithCompanyAccess]
[WithShopAccess]
public class AgentOperationsController : ControllerBase
{
  [HttpGet]
  public async Task<Results<Ok<GetOperationsResponse.Ok>, BadRequest>> GetOperations(
    [FromServices] GetOperationsForAgent endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromServices] ShopAccess? shopAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), shopAccess.Ensure()) switch
    {
      GetOperationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}/cancel")]
  public async Task<Results<Ok<CancelOperationResponse.Ok>, BadRequest>> CancelOperation(
    [FromServices] CancelOperationEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromRoute(Name = "id")] Guid bonusId
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), BonusId.From(bonusId)) switch
    {
      CancelOperationResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
