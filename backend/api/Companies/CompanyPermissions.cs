namespace Beloyal.API.Companies;

public static class CompanyPermissions
{
  public static string company_roles_read = nameof(company_roles_read);
  public static string company_roles_write = nameof(company_roles_write);

  public static string company_accesses_read = nameof(company_accesses_read);
  public static string company_accesses_write = nameof(company_accesses_write);

  public static string company_access_invitations_read = nameof(company_access_invitations_read);
  public static string company_access_invitations_write = nameof(company_access_invitations_write);

  public static string company_current_write = nameof(company_current_write);

  public static string company_options_read = nameof(company_options_read);
  public static string company_options_write = nameof(company_options_write);

  public static string promotions_read = nameof(promotions_read);
  public static string promotions_write = nameof(promotions_write);

  public static string shops_read = nameof(shops_read);
  public static string shops_write = nameof(shops_write);

  public static string shop_accesses_read = nameof(shop_accesses_read);
  public static string shop_accesses_write = nameof(shop_accesses_write);

  public static string shop_promotions_read = nameof(shop_promotions_read);
  public static string shop_promotions_write = nameof(shop_promotions_write);

  public static string mailings_read = nameof(mailings_read);
  public static string mailings_write = nameof(mailings_write);

  public static string messengers_read = nameof(messengers_read);
  public static string messengers_write = nameof(messengers_write);

  public static string chats_read = nameof(chats_read);
  public static string chats_write = nameof(chats_write);

  public static string levels_read = nameof(levels_read);
  public static string levels_write = nameof(levels_write);

  public static string boosters_read = nameof(boosters_read);
  public static string boosters_write = nameof(boosters_write);

  public static string operations_read = nameof(operations_read);
  public static string operations_write = nameof(operations_write);

  public static string customers_read = nameof(customers_read);

  public static List<string> GetAllPermissions() =>
    typeof(CompanyPermissions).GetFields().Select(f => (string)f.GetValue(null)!).ToList();

  public static bool IsValid(List<string> permissions)
  {
    var allPermissions = GetAllPermissions();
    return permissions.All(v => allPermissions.Any(vv => vv == v));
  }
}
