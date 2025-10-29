using Beloyal.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithAuthAttribute() : TypeFilterAttribute(typeof(WithAuthFilter));

public class WithAuthFilter(AuthContext authContext) : IAsyncResourceFilter
{
  public async Task OnResourceExecutionAsync(
    ResourceExecutingContext ctx,
    ResourceExecutionDelegate next
  )
  {
    ctx.HttpContext.Request.Cookies.TryGetValue(AuthContext.GetAccessTokenName(), out var token);

    if (token is not { Length: > 1 })
    {
      ctx.Result = new UnauthorizedObjectResult(ctx.ModelState);
      return;
    }

    try
    {
      var decodedAccessToken = authContext.DecodeAccessToken(token!);
      ctx.HttpContext.Items["accessToken"] = decodedAccessToken;
    }
    catch (Exception e)
    {
      Console.WriteLine($"ERR: {e.Message}. invalid token {token}");
      ctx.Result = new UnauthorizedObjectResult(ctx.ModelState);
      return;
    }

    await next();
  }
}
