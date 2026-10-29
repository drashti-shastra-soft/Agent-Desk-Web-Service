using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace AgentDeskWebService.Service
{
    public class PolicyTypeService : IPolicyTypeService
    {
        private readonly IPolicyTypeRepository _repo;

        public PolicyTypeService(IPolicyTypeRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.PolicyType> GetAll()
        {
            return _repo.GetAll();
        }
    }
}
