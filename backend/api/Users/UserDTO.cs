namespace Beloyal.API.Users;

public record UserDTO(Guid Id, string Email)
{
  public static UserDTO From(User v) => new(Id: v.Id, Email: v.Email);
};
