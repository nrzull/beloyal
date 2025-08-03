using Beloyal.API.Companies;

namespace Beloyal.API.Messengers;

public interface IMessengerContext<TBot, TClient>
{
  Task<TBot> CreateBot(Company company, string token);

  Task<TClient> EnsureClient(CompanyId companyId, TBot bot);

  void DisconnectClient(CompanyId companyId);

  Task PrepareClients();
}
