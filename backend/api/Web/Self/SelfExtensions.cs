using Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;
using Beloyal.API.Web.Self.CreateCompany;
using Beloyal.API.Web.Self.GetCompanyAccessInvitations;
using Beloyal.API.Web.Self.GetSelf;

namespace Beloyal.API.Web.Self;

public static class SelfExtensions
{
  public static void UseSelfEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetSelfEndpoint>();
    builder.Services.AddScoped<CreateCompanyEndpoint>();
    builder.Services.AddScoped<AcceptCompanyAccessInvitationEndpoint>();
    builder.Services.AddScoped<GetCompanyAccessInvitationsEndpoint>();
  }
}
