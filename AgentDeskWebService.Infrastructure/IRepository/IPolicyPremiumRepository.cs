using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyPremiumRepository
    {
        Domain.PolicyPremium Upsert(Domain.PolicyPremium mPolicyPremium);

        Domain.PolicyPremium UpsertAndUpdateFup(Domain.PolicyPremium mPolicyPremium);
        //void Upsert(Domain.PolicyPremium mPolicyPremium);
    }
}
