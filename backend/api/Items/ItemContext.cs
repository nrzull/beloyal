using Beloyal.API.Companies;
using Microsoft.EntityFrameworkCore;

namespace Beloyal.API.Items;

public class ItemContext(ApplicationDbContext dbContext)
{
  public async Task<ItemGroup> CreateGroup(Company company, string name, ItemGroup? parent = null)
  {
    var category = new ItemGroup
    {
      Company = company,
      Name = name,
      Parent = parent,
    };

    dbContext.Add(category);
    await dbContext.SaveChangesAsync();

    return category;
  }

  public async Task<(List<ItemGroup>, List<Item>)> GetChildren(CompanyId companyId)
  {
    var groups = await dbContext
      .ItemGroups.Where(v => v.ParentId == null && v.CompanyId == companyId.Value)
      .ToListAsync();

    var items = await dbContext
      .Items.Where(v => v.ParentId == null && v.CompanyId == companyId.Value)
      .ToListAsync();

    return (groups, items);
  }

  public async Task<(List<ItemGroup>, List<Item>)> GetChildren(ItemGroupId groupId)
  {
    var groups = await dbContext.ItemGroups.Where(v => v.ParentId == groupId.Value).ToListAsync();

    var items = await dbContext.Items.Where(v => v.ParentId == groupId.Value).ToListAsync();

    return (groups, items);
  }

  public async Task<(List<ItemGroup>, List<Item>)> GetDescendants(CompanyId companyId)
  {
    var (groups, items) = await GetChildren(companyId);

    foreach (var group in groups)
    {
      var (subGroups, subItems) = await GetDescendants(ItemGroupId.From(group));

      groups = groups.Concat(subGroups).ToList();
      items = items.Concat(subItems).ToList();
    }

    return (groups, items);
  }

  public async Task<(List<ItemGroup>, List<Item>)> GetDescendants(ItemGroupId groupId)
  {
    List<ItemGroup> groups = [];
    List<Item> items = [];

    async Task Executor(ItemGroupId v)
    {
      var (subGroups, subItems) = await GetChildren(v);

      groups = groups.Concat(subGroups).ToList();
      items = items.Concat(subItems).ToList();

      foreach (var group in subGroups)
      {
        await Executor(ItemGroupId.From(group));
      }
    }

    await Executor(groupId);

    return (groups, items);
  }

  public async Task RemoveGroup(ItemGroup group)
  {
    var (groups, items) = await GetDescendants(ItemGroupId.From(group));
    if (groups.Count() > 0 || items.Count() > 0)
    {
      throw new Exception("cannot remove while children there");
    }

    dbContext.Remove(group);
    await dbContext.SaveChangesAsync();
  }

  public async Task<Item> CreateItem(Company company, string name, ItemGroup? parent = null)
  {
    var product = new Item
    {
      Company = company,
      Name = name,
      Parent = parent,
    };

    dbContext.Add(product);
    await dbContext.SaveChangesAsync();

    return product;
  }

  public async Task RemoveItem(Item item)
  {
    dbContext.Remove(item);
    await dbContext.SaveChangesAsync();
  }
}
