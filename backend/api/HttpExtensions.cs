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

  public static void UseCustomCors(this WebApplication app)
  {
    app.UseCors("generic");
  }

  // public static void SetAccessTokenCookie(this HttpContext httpContext, EncodedToken token)
  // {
  //   httpContext.Response.Cookies.Append(
  //     AuthContext.GetAccessTokenName(),
  //     token.Token,
  //     new CookieOptions
  //     {
  //       Path = "/",
  //       SameSite = SameSiteMode.None,
  //       Expires = token.ExpiredAt,
  //       HttpOnly = true,
  //       Secure = true,
  //       Domain = httpContext.Request.Host.Host,
  //     }
  //   );
  // }
}
