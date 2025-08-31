using Beloyal.API.Web.Mailings.GetMailings;

namespace Beloyal.API.Web.Mailings;

public static class MailingsExtensions
{
  public static void UseMailingsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetMailingsEndpoint>();
  }
}
