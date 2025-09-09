namespace Beloyal.API.Users;

public record UserDTO(Guid Id, string? Phone)
{
  public static UserDTO From(User v) => new(Id: v.Id, Phone: v.Phone);
};
