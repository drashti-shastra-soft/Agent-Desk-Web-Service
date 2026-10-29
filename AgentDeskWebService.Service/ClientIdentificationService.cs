using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class ClientIdentificationService : IClientIdentificationService
    {
        private readonly IClientIdentificationRepository _repo;

        public ClientIdentificationService(IClientIdentificationRepository repo)
        {
            _repo = repo;
        }

        public Domain.ClientIdentification Upsert(Domain.ClientIdentification mClientIdentification)
        {
            return _repo.Upsert(mClientIdentification);
        }

        public Domain.ClientIdentification Get(int id)
        {
            return _repo.Get(id);
        }
        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
