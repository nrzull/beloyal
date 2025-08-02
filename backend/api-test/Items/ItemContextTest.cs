using Beloyal.API.Items;
using Beloyal.API.Test.Companies;

namespace Beloyal.API.Test.Items;

public class ItemContextTest : TestLifetime
{
  [Fact]
  public Task CreateGroup() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var itemContext = scope.Get<ItemContext>();

        var group = await itemContext.CreateGroup(company, "root");
        var group2 = await itemContext.CreateGroup(company, "sub", group);

        Assert.Equal(group.Id, group2.ParentId);
      }
    );

  [Fact]
  public Task RemoveGroup() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var itemContext = scope.Get<ItemContext>();

        var group = await itemContext.CreateGroup(company, "root");
        var group2 = await itemContext.CreateGroup(company, "sub", group);
        var item = await itemContext.CreateItem(company, "item", group);
        var item2 = await itemContext.CreateItem(company, "item", group2);

        await Assert.ThrowsAnyAsync<Exception>(() => itemContext.RemoveGroup(group));

        await Assert.ThrowsAnyAsync<Exception>(() => itemContext.RemoveGroup(group2));
        await itemContext.RemoveItem(item2);
        await itemContext.RemoveGroup(group2);

        await Assert.ThrowsAnyAsync<Exception>(() => itemContext.RemoveGroup(group));
        await itemContext.RemoveItem(item);

        await itemContext.RemoveGroup(group);
      }
    );

  [Fact]
  public Task CreateItem() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var itemContext = scope.Get<ItemContext>();

        var group = await itemContext.CreateGroup(company, "root");
        var item = await itemContext.CreateItem(company, "item", group);

        Assert.Equal(group.Id, item.ParentId);
      }
    );

  [Fact]
  public Task RemoveItem() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var itemContext = scope.Get<ItemContext>();
        var companyId = CompanyId.From(company);

        var item = await itemContext.CreateItem(company, "item");

        var (groups, items) = await itemContext.GetDescendants(companyId);
        Assert.Single(items);

        await itemContext.RemoveItem(item);

        (groups, items) = await itemContext.GetDescendants(companyId);
        Assert.Empty(items);
      }
    );

  [Fact]
  public Task GetDescendantsByCompany() =>
    WithScope(
      async (scope, _) =>
      {
        var (company, _, _) = await CompanyFixture.Company(scope);
        var itemContext = scope.Get<ItemContext>();
        var companyId = CompanyId.From(company);

        var group = await itemContext.CreateGroup(company, "root");
        var item = await itemContext.CreateItem(company, "item");

        var (groups, items) = await itemContext.GetDescendants(companyId);
        Assert.Single(groups);
        Assert.Single(items);

        var group2 = await itemContext.CreateGroup(company, "root", group);

        (groups, items) = await itemContext.GetDescendants(companyId);
        Assert.Equal(2, groups.Count());

        await itemContext.RemoveItem(item);

        (groups, items) = await itemContext.GetDescendants(companyId);
        Assert.Empty(items);
      }
    );
}
