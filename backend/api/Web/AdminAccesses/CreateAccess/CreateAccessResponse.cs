using Beloyal.API.Admin;

namespace Beloyal.API.Web.AdminAccesses.CreateAccess;

public abstract record CreateAccessResponse
{
  public record Ok(AdminAccessDTO Data) : CreateAccessResponse;
}
