using Beloyal.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithCustomerAuthAttribute() : TypeFilterAttribute(typeof(WithCustomerAuthFilter));

public class WithCustomerAuthFilter(AuthContext authContext) : IAsyncResourceFilter
{
  public async Task OnResourceExecutionAsync(
    ResourceExecutingContext ctx,
    ResourceExecutionDelegate next
  )
  {
    ctx.HttpContext.Request.Headers.TryGetValue("X-Customer-Token", out var rawCustomerTokens);

    var token = rawCustomerTokens.FirstOrDefault()?.Replace("Bearer ", "");
    if (token is not { Length: > 1 })
    {
      ctx.Result = new UnauthorizedObjectResult(ctx.ModelState);
      return;
    }

    try
    {
      var decodedToken = authContext.DecodeCustomerAccessToken(token!);
      ctx.HttpContext.Items["customerAccessToken"] = decodedToken;
    }
    catch (Exception e)
    {
      Console.WriteLine($"ERR: {e.Message}. invalid customer token {token}");
      ctx.Result = new UnauthorizedObjectResult(ctx.ModelState);
      return;
    }

    await next();
  }
}
