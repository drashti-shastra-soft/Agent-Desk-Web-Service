using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IPolicyPremiumService
    {
        Domain.PolicyPremium Upsert(Domain.PolicyPremium mPolicyPremium);

        //void Upsert(List<Domain.PolicyPremium> mPolicyPremium);
    }
}
