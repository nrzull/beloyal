using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
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
  public async Task<Results<Ok<GetOperationsResponse.Ok>, BadRequest>> GetOperations(
    [FromServices] GetOperationsEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetOperationsResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
