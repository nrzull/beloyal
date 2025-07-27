using Beloyal.API.Admin;
using Beloyal.API.Companies;
using Beloyal.API.Users;

namespace Beloyal.API.Web.Self.GetSelf;

public abstract record GetSelfResponse
{
  public record Ok(UserDTO User, AdminAccessDTO? AdminAccess, CompanyAccessDTO? CompanyAccess)
    : GetSelfResponse;
}
