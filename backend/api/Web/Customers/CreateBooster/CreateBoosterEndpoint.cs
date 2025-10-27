using Beloyal.API.Boosters;
using Beloyal.API.Companies;
using Beloyal.API.Customers;

namespace Beloyal.API.Web.Customers.CreateBooster;

public class CreateBoosterEndpoint(BoosterContext boosterContext, CustomerContext customerContext)
{
  public async Task<CreateBoosterResponse> Handle(
    CompanyAccess companyAccess,
    CustomerId customerId,
    CreateBoosterRequest req
  )
  {
    var customer = await customerContext.GetCustomer(CompanyId.From(companyAccess), customerId);
    if (customer is null)
    {
      throw new Exception("invalid customer");
    }

    var booster = await boosterContext.AddBooster(
      customer,
      new Types.Percent(req.CashbackPercent),
      req.ExpiredAt
    );

    return new CreateBoosterResponse.Ok(BoosterDTO.From(booster));
  }
}
