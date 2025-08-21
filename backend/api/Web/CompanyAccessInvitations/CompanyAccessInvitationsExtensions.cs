using Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;
using Beloyal.API.Web.CompanyAccessInvitations.GetInvitations;

namespace Beloyal.API.Web.CompanyAccessInvitations;

public static class CompanyAccessInvitationsExtensions
{
  public static void UseCompanyAccessInvitationsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateInvitationEndpoint>();
    builder.Services.AddScoped<GetInvitationsEndpoint>();
  }
}
