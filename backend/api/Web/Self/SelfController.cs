using Beloyal.API.Users;
using Beloyal.API.Web.Filters;
using Beloyal.API.Web.Self.GetSelf;
using Microsoft.AspNetCore.Mvc;

namespace Beloyal.API.Web.Self;

[ApiController]
[WithAuth]
[WithUser]
[Route("/api/self")]
public class SelfController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetSelf(
    [FromServices] GetSelfEndpoint endpoint,
    [FromServices] User? user
  ) =>
    await endpoint.Handle(user.Ensure()) switch
    {
      GetSelfResponse.Ok ok => Ok(ok),
      _ => BadRequest(),
    };
}
