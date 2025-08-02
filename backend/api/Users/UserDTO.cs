namespace Beloyal.API.Users;

public record UserDTO(Guid Id, string? Email, string? Phone)
{
  public static UserDTO From(User v) => new(Id: v.Id, Email: v.Email, Phone: v.Phone);
};
