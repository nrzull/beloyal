using Beloyal.API.Web.Shops.CreateShop;
using Beloyal.API.Web.Shops.CreateShopAccess;
using Beloyal.API.Web.Shops.RemoveShopAccess;
using Beloyal.API.Web.Shops.UpdateShop;

namespace Beloyal.API.Web.Shops;

public static class ShopsExtensions
{
  public static void UseShopsEndpoints(this WebApplicationBuilder builder)
  {
    builder.Services.AddScoped<CreateShopEndpoint>();
    builder.Services.AddScoped<UpdateShopEndpoint>();
    builder.Services.AddScoped<CreateShopAccessEndpoint>();
    builder.Services.AddScoped<RemoveShopAccessEndpoint>();
  }
}
