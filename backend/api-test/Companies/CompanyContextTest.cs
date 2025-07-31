using Beloyal.API.Companies;
using Beloyal.API.Test.Users;
using Beloyal.API.Types;

namespace Beloyal.API.Test.Companies;

public class CompanyContextTest : TestLifetime
{
  [Fact]
  public Task CreateCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var user = await UserFixture.User(scope);
        var companyContext = scope.Get<CompanyContext>();
        var tin = new Tin("0123456789");

        await Assert.ThrowsAnyAsync<Exception>(() => companyContext.CreateCompany(user, tin));

        await companyContext.CreateWhiteCompany(tin, DateTimeOffset.UtcNow.AddMonths(1));

        (Company company, var _, CompanyAccess access) = await companyContext.CreateCompany(
          user,
          tin
        );

        Assert.Equal(company.Tin, tin.Value);
        Assert.Equal(user.Id, access.UserId);
      }
    );

  [Fact]
  public Task OnlyUniqueCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        await Assert.ThrowsAnyAsync<Exception>(() => CompanyFixture.Company(scope));
      }
    );

  [Fact]
  public Task UpdateWhiteCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var user = await UserFixture.User(scope);
        var companyContext = scope.Get<CompanyContext>();
        var tin = new Tin("0123456789");

        var whiteCompany = await companyContext.CreateWhiteCompany(
          tin,
          DateTimeOffset.UtcNow.AddMonths(1)
        );

        await companyContext.UpdateWhiteCompany(whiteCompany, DateTimeOffset.UtcNow.AddMonths(-2));

        await Assert.ThrowsAnyAsync<Exception>(() => companyContext.CreateCompany(user, tin));

        await companyContext.UpdateWhiteCompany(whiteCompany, DateTimeOffset.UtcNow.AddYears(1));

        await companyContext.CreateCompany(user, tin);
      }
    );

  [Fact]
  public Task RemoveWhiteCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var user = await UserFixture.User(scope);
        var companyContext = scope.Get<CompanyContext>();
        var tin = new Tin("0123456789");

        var whiteCompany = await companyContext.CreateWhiteCompany(
          tin,
          DateTimeOffset.UtcNow.AddMonths(1)
        );

        await companyContext.RemoveWhiteCompany(tin);

        await Assert.ThrowsAnyAsync<Exception>(() => companyContext.CreateCompany(user, tin));
      }
    );

  [Fact]
  public Task CreateCompanyAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var user = await UserFixture.User(scope, "qwewe@gma.com");
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        var access2 = await companyContext.CreateCompanyAccess(company, user, access.Role);

        Assert.Equal(access.RoleId, access2.RoleId);
        Assert.Equal(access.CompanyId, access.CompanyId);
      }
    );

  [Fact]
  public Task UpdateCompanyAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        var newRole = await companyContext.CreateCompanyRole(company, "qwe", []);

        await companyContext.UpdateCompanyAccess(access, newRole);

        Assert.Equal(access.RoleId, newRole.Id);
      }
    );

  [Fact]
  public Task RemoveCompanyAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, user) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        await companyContext.RemoveCompanyAccess(access);

        Assert.DoesNotContain(
          await companyContext.GetCompanyAccesses(user),
          (v) => v.Id == access.Id
        );
      }
    );

  [Fact]
  public Task CreateCompanyRole() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        var newRole = await companyContext.CreateCompanyRole(company, "qwe", []);

        Assert.Equal("qwe", newRole.Name);
      }
    );

  [Fact]
  public Task UpdateCompanyRole() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        var newRole = await companyContext.CreateCompanyRole(company, "qwe", []);

        await companyContext.UpdateCompanyRole(newRole, "qweqwe", newRole.Permissions);

        Assert.Equal("qweqwe", newRole.Name);
      }
    );

  [Fact]
  public Task CreateCompanyAccessInvitation() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, owner) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        await Assert.ThrowsAnyAsync<Exception>(() =>
          companyContext.CreateCompanyAccessInvitation(company, access.Role, new(owner.Email))
        );

        var user = await UserFixture.User(scope, "qwewqe@gma.com");

        var invitation = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new(user.Email)
        );

        Assert.Equal(invitation.Email, user.Email);
        Assert.Equal(invitation.CompanyId, company.Id);

        var invitation2 = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new(user.Email)
        );

        Assert.Equal(invitation.Id, invitation2.Id);
      }
    );

  [Fact]
  public Task UpdateCompanyAccessInvitation() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();
        var user = await UserFixture.User(scope, "qwewqe@gma.com");

        var invitation = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new(user.Email)
        );

        await companyContext.UpdateCompanyAccessInvitation(
          invitation,
          CompanyAccessInvitationStatus.Accepted
        );

        Assert.Equal(CompanyAccessInvitationStatus.Accepted, invitation.Status);
      }
    );

  [Fact]
  public Task GetCompanyAccessInvitation() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();
        var user = await UserFixture.User(scope, "qwewqe@gma.com");

        await companyContext.CreateCompanyAccessInvitation(company, access.Role, new(user.Email));

        var invitation = await companyContext.GetCompanyAccessInvitation(company, new(user.Email));

        Assert.NotNull(invitation);

        Assert.Equal(company.Id, invitation.CompanyId);
        Assert.Equal(user.Email, invitation.Email);
      }
    );

  [Fact]
  public Task GetCompanyAccessesByUser() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, owner) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        Assert.Contains(await companyContext.GetCompanyAccesses(owner), v => v.Id == access.Id);
      }
    );

  [Fact]
  public Task GetCompanyAccessesByCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();

        Assert.Contains(await companyContext.GetCompanyAccesses(company), v => v.Id == access.Id);
      }
    );

  [Fact]
  public Task GetCompanyAccessInvitationsByCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();
        var user = await UserFixture.User(scope, "qwewqe@gma.com");

        var invitation = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new(user.Email)
        );

        Assert.Contains(
          await companyContext.GetCompanyAccessInvitations(company),
          v => v.Id == invitation.Id
        );
      }
    );

  [Fact]
  public Task GetCompanyAccessInvitationsByEmail() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, access, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();
        var user = await UserFixture.User(scope, "qwewqe@gma.com");

        var invitation = await companyContext.CreateCompanyAccessInvitation(
          company,
          access.Role,
          new(user.Email)
        );

        Assert.Contains(
          await companyContext.GetCompanyAccessInvitations(new Email(user.Email)),
          v => v.Id == invitation.Id
        );
      }
    );

  [Fact]
  public Task GetCompanyRoles() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var companyContext = scope.Get<CompanyContext>();
        var roles = await companyContext.GetCompanyRoles(company);

        Assert.True(roles.Count() > 0);
        Assert.All(roles, (v) => Assert.Equal(v.CompanyId, company.Id));
      }
    );
}
