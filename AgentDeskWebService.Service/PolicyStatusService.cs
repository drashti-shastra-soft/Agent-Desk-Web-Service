using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class PolicyStatusService : IPolicyStatusService
    {
        private readonly IPolicyStatusRepository _repo;

        public PolicyStatusService(IPolicyStatusRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.PolicyStatus> GetAll()
        {
            return _repo.GetAll();
        }

        public List<Domain.PolicyStatus> GetActiveBy(string policyTypes)
        {
            return _repo.GetActiveBy(policyTypes);
        }
    }
}
