using Beloyal.API.Admin;
using Beloyal.API.Test.Users;
using Beloyal.API.Users;

namespace Beloyal.API.Test.Admin;

public class AdminContextTest : TestLifetime
{
  [Fact]
  public Task CreateAdminAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var user = await UserFixture.User(scope, "qweqwe@gma.com");
        Assert.Null(user.AdminAccess);

        var access = await adminContext.CreateAdminAccess(user, role);

        Assert.Equal(access.UserId, user.Id);
        Assert.Equal(access.RoleId, role.Id);
        Assert.NotNull(user.AdminAccess);
      }
    );

  [Fact]
  public Task UpdateAdminAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var role2 = await adminContext.CreateAdminRole("qwe", []);

        var user = await UserFixture.User(scope, "qweqwe@gma.com");

        var access = await adminContext.CreateAdminAccess(user, role);
        await adminContext.UpdateAdminAccess(access, role2);

        Assert.Equal(access.RoleId, role2.Id);
      }
    );

  [Fact]
  public Task RemoveAdminAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        var user = await UserFixture.User(scope, "qweqwe@gma.com");

        var access = await adminContext.CreateAdminAccess(user, role);
        Assert.NotNull(user.AdminAccess);

        await adminContext.RemoveAdminAccess(access);
        Assert.Null(user.AdminAccess);
      }
    );

  [Fact]
  public Task CreateAdminRole() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        Assert.Equal("qwe", role.Name);
      }
    );

  [Fact]
  public Task UpdateAdminRole() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var role = await adminContext.CreateAdminRole("qwe", []);
        await adminContext.UpdateAdminRole(role, "qweqwe", role.Permissions);
        Assert.Equal("qweqwe", role.Name);
      }
    );

  [Fact]
  public Task GetAccess() =>
    WithScope(
      async (scope, _) =>
      {
        var adminContext = scope.Get<AdminContext>();
        var user = await UserFixture.User(scope, "qweqwe@gma.com");
        var role = await adminContext.CreateAdminRole("qwe", []);
        await adminContext.CreateAdminAccess(user, role);

        var access = await adminContext.GetAccess(UserId.From(user));
        Assert.Equal(access!.UserId, user.Id);
        Assert.Equal(access.RoleId, role.Id);
      }
    );
}
