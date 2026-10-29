using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class PolicyNomineeService : IPolicyNomineeService
    {
        private readonly IPolicyNomineeRepository _repo;

        public PolicyNomineeService(IPolicyNomineeRepository repo)
        {
            _repo = repo;
        }

        public Domain.PolicyNomineeLister GetAll(Domain.PolicyNomineeLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.PolicyNominee Upsert(Domain.PolicyNominee mPolicyNominee)
        {
            return _repo.Upsert(mPolicyNominee);
        }

        public Domain.PolicyNominee Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }
    }
}
