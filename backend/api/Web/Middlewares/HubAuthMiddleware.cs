using Beloyal.API.Auth;

namespace Beloyal.API.Web.Middlewares;

public class HubAuthMiddleware(AuthContext authContext) : IMiddleware
{
  public async Task InvokeAsync(HttpContext ctx, RequestDelegate next)
  {
    if (ctx.Request.Path.StartsWithSegments("/api/ws"))
    {
      ctx.Request.Cookies.TryGetValue(AuthContext.GetAccessTokenName(), out var token);

      if (token is not { Length: > 1 })
      {
        ctx.Response.StatusCode = 401;
        return;
      }

      var decodedAccessToken = authContext.DecodeAccessToken(token!);
      ctx.Items["accessToken"] = decodedAccessToken;
    }

    await next(ctx);
  }
}
