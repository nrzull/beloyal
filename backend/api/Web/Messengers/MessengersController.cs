using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Messengers.CreateTgBot;
using Beloyal.API.Web.Messengers.GetTgBot;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Messengers;

[ApiController]
[Route("/api/company/messengers")]
[WithAuth]
[WithCompanyAccess]
public class MessengersController : ControllerBase
{
  [HttpGet("telegram-bot")]
  public async Task<Results<Ok<GetTgBotResponse.Ok>, NotFound, BadRequest>> GetTgBot(
    [FromServices] GetTgBotEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetTgBotResponse.NotFound => TypedResults.NotFound(),
      GetTgBotResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };

  [HttpPost("telegram-bot")]
  public async Task<Results<Ok<CreateTgBotResponse.Ok>, BadRequest>> CreateTgBot(
    [FromServices] CreateTgBotEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateTgBotRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateTgBotResponse.Ok ok => TypedResults.Ok(ok),
      _ => TypedResults.BadRequest(),
    };
}
