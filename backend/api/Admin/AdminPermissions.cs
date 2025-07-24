namespace Beloyal.API.Admin;

public static class AdminPermissions
{
  public static List<string> GetAllPermissions() =>
    typeof(AdminPermissions).GetFields().Select(f => (string)f.GetValue(null)!).ToList();

  public static bool IsValid(List<string> permissions) =>
    GetAllPermissions().All(v => permissions.Any(vv => vv == v));
}
