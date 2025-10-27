using Beloyal.API.Web.Customers.CreateBooster;
using Beloyal.API.Web.Customers.GetCustomer;
using Beloyal.API.Web.Customers.GetCustomerBoosters;
using Beloyal.API.Web.Customers.GetCustomerOperations;
using Beloyal.API.Web.Customers.GetCustomers;

namespace Beloyal.API.Web.Customers;

public static class CustomersExtensions
{
  public static void UseCustomersEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<GetCustomersEndpoint>();
    builder.Services.AddScoped<GetCustomerEndpoint>();
    builder.Services.AddScoped<GetCustomerOperationsEndpoint>();
    builder.Services.AddScoped<GetCustomerBoostersEndpoint>();
    builder.Services.AddScoped<CreateBoosterEndpoint>();
  }
}
