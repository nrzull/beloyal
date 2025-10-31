using Beloyal.API.Auth;

namespace Beloyal.API;

public static class HttpExtensions
{
  public static void SetupHttp(this WebApplicationBuilder builder)
  {
    builder.WebHost.ConfigureKestrel(opts =>
    {
      var port = int.Parse(Environment.GetEnvironmentVariable("API_WEB_PORT")!);
      opts.ListenLocalhost(port);
    });
  }

  public static void SetupCors(this WebApplicationBuilder builder)
  {
    builder.Services.AddCors(options =>
    {
      options.AddPolicy(
        "generic",
        policy =>
        {
          policy
            .SetIsOriginAllowed(origin => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Access-Control-Allow-Origin", "Content-Type");
        }
      );
    });
  }

  public static void SetupOpenApi(this WebApplicationBuilder builder)
  {
    builder.Services.AddOpenApi();
  }

  public static void UseCustomCors(this WebApplication app)
  {
    app.UseCors("generic");
  }

  public static string? GetAccessTokenCookie(this HttpContext ctx) =>
    ctx.Request.Cookies.FirstOrDefault(v => v.Key == AuthContext.GetAccessTokenName()).Value;

  public static string? GetRefreshTokenCookie(this HttpContext ctx) =>
    ctx.Request.Cookies.FirstOrDefault(v => v.Key == AuthContext.GetRefreshTokenName()).Value;

  public static void SetAuthCookies(
    this HttpContext httpContext,
    EncodedAccessToken accessToken,
    EncodedRefreshToken refreshToken,
    bool secure
  )
  {
    SetCookie(
      httpContext,
      AuthContext.GetAccessTokenName(),
      accessToken.Token,
      accessToken.ExpiredAt,
      secure
    );

    SetCookie(
      httpContext,
      AuthContext.GetRefreshTokenName(),
      refreshToken.Token,
      refreshToken.ExpiredAt,
      secure
    );
  }

  static void SetCookie(
    HttpContext httpContext,
    string cookieName,
    string token,
    DateTimeOffset expiredAt,
    bool secure
  )
  {
    httpContext.Response.Cookies.Append(
      cookieName,
      token,
      new CookieOptions
      {
        Path = "/",
        SameSite = secure ? SameSiteMode.None : SameSiteMode.Unspecified,
        Expires = expiredAt,
        HttpOnly = true,
        Secure = secure,
        Domain = httpContext.Request.Host.Host,
      }
    );
  }
}
