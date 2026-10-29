using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class ClientFinancialInfoService : IClientFinancialInfoService
    {
        private readonly IClientFinancialInfoRepository _repo;

        public ClientFinancialInfoService(IClientFinancialInfoRepository repo)
        {
            _repo = repo;
        }

        public Domain.ClientFinancialInfo Upsert(Domain.ClientFinancialInfo mClientFinancial)
        {
            return _repo.Upsert(mClientFinancial);
        }

        public Domain.ClientFinancialInfo Get(int id)
        {
            return _repo.Get(id);
        }
        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
