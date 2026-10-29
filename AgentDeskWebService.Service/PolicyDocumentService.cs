using AgentDeskWebService.Domain;
using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class PolicyDocumentService : IPolicyDocumentService
    {
        private readonly IPolicyDocumentRepository _repo;

        public PolicyDocumentService(IPolicyDocumentRepository repo)
        {
            _repo = repo;
        }

        public PolicyDocument Upsert(PolicyDocument mPolicyDocument)
        {
            return _repo.Upsert(mPolicyDocument);
        }

        public PolicyDocument Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
             _repo.Delete(id, loginUserId);
        }

        public List<PolicyDocument> Fetch(int policyId)
        {
            return _repo.Fetch(policyId);
        }

    }
}
