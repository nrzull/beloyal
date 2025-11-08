using Beloyal.API.Auth;
using Polly;
using Polly.Extensions.Http;

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

    var timeoutPolicy = HttpPolicyExtensions
      .HandleTransientHttpError()
      .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(4, retryAttempt))
      );

    var policy = Policy.WrapAsync(timeoutPolicy);

    builder.Services.AddHttpClient("TgHttpClient").AddPolicyHandler(policy);
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
      refreshToken.ExpiredAt,
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
