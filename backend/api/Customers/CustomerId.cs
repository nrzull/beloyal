using Vogen;

namespace Beloyal.API.Customers;

[ValueObject<Guid>]
public partial struct CustomerId
{
  public static CustomerId From(Customer v) => From(v.Id);
}
