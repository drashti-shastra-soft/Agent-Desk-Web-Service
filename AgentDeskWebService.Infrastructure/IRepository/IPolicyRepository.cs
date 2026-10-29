using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyRepository
    {
        Domain.PolicyLister GetAll(Domain.PolicyLister mLister);

        Domain.Policy Upsert(Domain.Policy mPolicy);

        Domain.Policy Get(int id);

        void Delete(int id, int loginUserId);

        Domain.Policy GetByPolicyNumber(string policyNumber);

        Domain.Policy GetPdf(int id);

        bool CheckPolicyNumberExist(Domain.Policy mPolicy);

        Domain.Policy ImportPolicy(Domain.Policy mPolicy);

    }
}
