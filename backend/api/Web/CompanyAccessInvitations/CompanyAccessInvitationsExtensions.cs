using Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;

namespace Beloyal.API.Web.CompanyAccessInvitations;

public static class CompanyAccessInvitationsExtensions
{
  public static void UseCompanyAccessInvitationsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateInvitationEndpoint>();
  }
}
