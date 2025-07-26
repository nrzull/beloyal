namespace Beloyal.API.Web.Self.GetSelf;

public abstract record GetSelfResponse
{
  public record Ok(Guid UserId) : GetSelfResponse;
}
