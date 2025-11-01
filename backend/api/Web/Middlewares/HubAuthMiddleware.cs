using Beloyal.API.Auth;

namespace Beloyal.API.Web.Middlewares;

public class HubAuthMiddleware(AuthContext authContext, IHostEnvironment env) : IMiddleware
{
  public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
  {
    if (ctx.Request.Path.StartsWithSegments("/api/ws"))
    {
      ctx.Request.Cookies.TryGetValue(AuthContext.GetAccessTokenName(), out var token);

      ctx.Request.Cookies.TryGetValue(AuthContext.GetRefreshTokenName(), out var refreshToken);

      if (token is { Length: > 1 } && refreshToken is { Length: > 1 })
      {
        var result = await authContext.VerifyTokens(token, refreshToken);
        if (result is VerifyTokensResult.Ok ok)
        {
          ctx.Items["accessToken"] = ok.AccessToken;
        }
        else if (result is VerifyTokensResult.Refresh refresh)
        {
          ctx.SetAuthCookies(
            refresh.EncodedAccessToken,
            refresh.EncodedRefreshToken,
            env.IsProduction()
          );
          ctx.Items["accessToken"] = refresh.AccessToken;
        }
      }

      ctx.Request.Headers.TryGetValue("Authorization", out var rawCustomerTokens);

      var customerToken = rawCustomerTokens.FirstOrDefault()?.Replace("Bearer ", "");
      if (customerToken is { Length: > 1 } t2)
      {
        var decodedToken = await authContext.DecodeCustomerAccessToken(t2);
        ctx.Items["customerAccessToken"] = decodedToken;
      }

      if (ctx.Items["accessToken"] is null && ctx.Items["customerAccessToken"] is null)
      {
        ctx.Response.StatusCode = 401;
        return;
      }
    }

    await next(ctx);
  }
}
