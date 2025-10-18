namespace Beloyal.API.Web.Shops.UpdateShop;

public record UpdateShopRequest(Guid Id, string Name, string? TimeZone);
