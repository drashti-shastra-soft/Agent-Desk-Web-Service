using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class ClientHealthService : IClientHealthService
    {
        private readonly IClientHealthRepository _repo;

        public ClientHealthService(IClientHealthRepository repo)
        {
            _repo = repo;
        }

        public Domain.ClientHealth Upsert(Domain.ClientHealth mClientHealth)
        {
            return _repo.Upsert(mClientHealth);
        }

        public Domain.ClientHealth Get(int id)
        {
            return _repo.Get(id);
        }
        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
