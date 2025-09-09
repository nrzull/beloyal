using Beloyal.API.Test.Companies;
using Beloyal.API.Web.CompanyAccessInvitations.CreateInvitation;

namespace Beloyal.API.Test.Web.CompanyAccessInvitations;

public class CreateInvitationEndpointTest : TestLifetime
{
  [Fact]
  public Task Create() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var endpoint = scope.Get<CreateInvitationEndpoint>();

        var result = await endpoint.Handle(access, new("+12345678901", access.RoleId));
        Assert.IsType<CreateInvitationResponse.Ok>(result);

        if (result is CreateInvitationResponse.Ok ok)
        {
          Assert.Equal("+12345678901", ok.Data.Phone);
          Assert.Equal(company.Id, ok.Data.Company.Id);
          Assert.Equal(access.RoleId, ok.Data.Role.Id);
        }
      }
    );
}
