namespace Beloyal.API.Companies;

public static class CompanyPermissions
{
  public static List<string> GetAllPermissions() =>
    typeof(CompanyPermissions).GetFields().Select(f => (string)f.GetValue(null)!).ToList();

  public static bool IsValid(List<string> permissions) =>
    GetAllPermissions().All(v => permissions.Any(vv => vv == v));
}
