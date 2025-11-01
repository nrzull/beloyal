using Beloyal.API.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Beloyal.API.Web.Filters;

public class WithAuthAttribute() : TypeFilterAttribute(typeof(WithAuthFilter));

public class WithAuthFilter(AuthContext authContext, IHostEnvironment env) : IAsyncResourceFilter
{
  public async Task OnResourceExecutionAsync(
    ResourceExecutingContext ctx,
    ResourceExecutionDelegate next
  )
  {
    ctx.HttpContext.Request.Cookies.TryGetValue(AuthContext.GetAccessTokenName(), out var token);

    ctx.HttpContext.Request.Cookies.TryGetValue(
      AuthContext.GetRefreshTokenName(),
      out var refreshToken
    );

    if (token is not { Length: > 1 } || refreshToken is not { Length: > 1 })
    {
      ctx.Result = new UnauthorizedObjectResult(ctx.ModelState);
      return;
    }

    try
    {
      var decodedAccessToken = await authContext.VerifyTokens(token, refreshToken);

      if (decodedAccessToken is VerifyTokensResult.Ok ok)
      {
        ctx.HttpContext.Items["accessToken"] = ok.AccessToken;
      }
      else if (decodedAccessToken is VerifyTokensResult.Refresh refresh)
      {
        ctx.HttpContext.SetAuthCookies(
          refresh.EncodedAccessToken,
          refresh.EncodedRefreshToken,
          env.IsProduction()
        );
        ctx.HttpContext.Items["accessToken"] = refresh.AccessToken;
      }
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
