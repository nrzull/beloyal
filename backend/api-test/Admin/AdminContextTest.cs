namespace Beloyal.API.Test.Admin;

public class AdminContextTest : TestLifetime
{
  [Fact]
  public Task CreateAdminAccess() => WithScope(async (scope, _) => { });

  [Fact]
  public Task UpdateAdminAccess() => WithScope(async (scope, _) => { });

  [Fact]
  public Task RemoveAdminAccess() => WithScope(async (scope, _) => { });

  [Fact]
  public Task CreateAdminRole() => WithScope(async (scope, _) => { });

  [Fact]
  public Task UpdateAdminRole() => WithScope(async (scope, _) => { });
}
