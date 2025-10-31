using Beloyal.API.Auth;

namespace Beloyal.API.Web.Middlewares;

public class HubAuthMiddleware(AuthContext authContext) : IMiddleware
{
  public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
  {
    if (ctx.Request.Path.StartsWithSegments("/api/ws"))
    {
      ctx.Request.Cookies.TryGetValue(AuthContext.GetAccessTokenName(), out var token);
      if (token is { Length: > 1 } t1)
      {
        var decodedAccessToken = await authContext.DecodeAccessToken(t1);
        ctx.Items["accessToken"] = decodedAccessToken;
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
