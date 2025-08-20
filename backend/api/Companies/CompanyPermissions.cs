namespace Beloyal.API.Companies;

public static class CompanyPermissions
{
  public static string company_roles_read = nameof(company_roles_read);
  public static string company_roles_write = nameof(company_roles_write);
  public static string company_accesses_read = nameof(company_accesses_read);
  public static string company_access_invitations_read = nameof(company_access_invitations_read);
  public static string company_access_invitations_write = nameof(company_access_invitations_write);

  public static List<string> GetAllPermissions() =>
    typeof(CompanyPermissions).GetFields().Select(f => (string)f.GetValue(null)!).ToList();

  public static bool IsValid(List<string> permissions)
  {
    var allPermissions = GetAllPermissions();
    return permissions.All(v => allPermissions.Any(vv => vv == v));
  }
}
