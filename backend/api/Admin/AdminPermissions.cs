namespace Beloyal.API.Admin;

public static class AdminPermissions
{
  public static string white_companies_read = nameof(white_companies_read);
  public static string white_companies_write = nameof(white_companies_write);

  public static List<string> GetAllPermissions() =>
    typeof(AdminPermissions).GetFields().Select(f => (string)f.GetValue(null)!).ToList();

  public static bool IsValid(List<string> permissions)
  {
    var allPermissions = GetAllPermissions();
    return permissions.All(v => allPermissions.Any(vv => vv == v));
  }
}
