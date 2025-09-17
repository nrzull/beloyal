using Beloyal.API.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Types;
using Beloyal.API.Users;
using Beloyal.API.Web.Self.CreateCompany;

namespace Beloyal.API.Test.Web.Self;

public class CreateCompanyEndpointTest : TestLifetime
{
  [Fact]
  public Task CreateCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var endpoint = scope.Get<CreateCompanyEndpoint>();
        var companyContext = scope.Get<CompanyContext>();

        var user = await UserFixture.User(scope, "+12345678901");
        var tin = new Tin("1234567891");
        var percent = new Percent(10);

        var req = new CreateCompanyRequest(tin.Value, percent.Value, percent.Value, new(1));

        await Assert.ThrowsAnyAsync<Exception>(() => endpoint.Handle(user, req));

        await companyContext.CreateWhiteCompany(tin, DateTimeOffset.UtcNow.AddDays(1));

        Assert.IsType<CreateCompanyResponse.Ok>(await endpoint.Handle(user, req));

        Assert.Single(await companyContext.GetCompanyAccesses(UserId.From(user)));
      }
    );
}
