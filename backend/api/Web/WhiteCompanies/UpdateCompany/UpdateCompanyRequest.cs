namespace Beloyal.API.Web.WhiteCompanies.UpdateCompany;

public record UpdateCompanyRequest(Guid Id, DateTimeOffset ExpiredAt);
