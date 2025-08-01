using Beloyal.API.Companies;
using Beloyal.API.Test.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Users;
using Beloyal.API.Web.Self.AcceptCompanyAccessInvitation;

namespace Beloyal.API.Test.Web.Self;

public class AcceptCompanyAccessInvitationEndpointTest : TestLifetime
{
  [Fact]
  public Task Accept() =>
    WithScope(
      async (scope, _) =>
      {
        var companyContext = scope.Get<CompanyContext>();
        var (company, access, _) = await CompanyFixture.Company(scope);
        var endpoint = scope.Get<AcceptCompanyAccessInvitationEndpoint>();

        var user = await UserFixture.User(scope, "qweqwe@gma.com");
        var user2 = await UserFixture.User(scope, "qweqweqwe@gma.com");

        var invitation = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new("qweqwe@gma.com")
        );

        var invitation2 = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new("qweqweqwe@gma.com")
        );

        await Assert.ThrowsAnyAsync<Exception>(() => endpoint.Handle(user, new(invitation2.Id)));

        var result = await endpoint.Handle(user, new(invitation.Id));
        Assert.IsType<AcceptCompanyAccessInvitationResponse.Ok>(result);

        if (result is AcceptCompanyAccessInvitationResponse.Ok ok)
        {
          Assert.Contains(
            await companyContext.GetCompanyAccesses(UserId.From(user)),
            v => ok.Data.Id == v.Id
          );
        }
      }
    );
}
