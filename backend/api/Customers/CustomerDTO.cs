using Beloyal.API.Users;

namespace Beloyal.API.Customers;

public record CustomerDTO(Guid Id, DateTimeOffset CreatedAt, UserDTO User)
{
  public static CustomerDTO From(Customer v) =>
    new(Id: v.Id, User: UserDTO.From(v.User), CreatedAt: v.CreatedAt);
};
