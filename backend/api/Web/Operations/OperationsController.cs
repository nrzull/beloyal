using Beloyal.API.Bonuses;
using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Operations.CancelOperation;
using Beloyal.API.Web.Operations.GetOperations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Operations;

[ApiController]
[Route("/api/company/operations")]
[WithAuth]
[WithCompanyAccess]
public class OperationsController : ControllerBase
{
  [HttpGet]
  [WithCompanyPermissions(nameof(CompanyPermissions.operations_read))]
  public async Task<Results<Ok<GetOperationsResponse.Ok>, BadRequest>> GetOperations(
    [FromServices] GetOperationsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetOperationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPatch("{id}/cancel")]
  [WithCompanyPermissions(nameof(CompanyPermissions.operations_write))]
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
