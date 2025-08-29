using Beloyal.API.Companies;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Messengers.CreateTgBot;
using Beloyal.API.Web.Messengers.GetTgBot;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Messengers;

[ApiController]
[Route("/api/company/messengers")]
[WithAuth]
[WithCompanyAccess]
public class MessengersController : ControllerBase
{
  [HttpGet("telegram-bot")]
  public async Task<IActionResult> GetTgBot(
    [FromServices] GetTgBotEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess
  ) =>
    await endpoint.Handle(companyAccess.Ensure()) switch
    {
      GetTgBotResponse.NotFound => NotFound(),
      GetTgBotResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };

  [HttpPost("telegram-bot")]
  public async Task<IActionResult> CreateTgBot(
    [FromServices] CreateTgBotEndpoint endpoint,
    [FromServices] CompanyAccess? companyAccess,
    [FromBody] CreateTgBotRequest req
  ) =>
    await endpoint.Handle(companyAccess.Ensure(), req) switch
    {
      CreateTgBotResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
