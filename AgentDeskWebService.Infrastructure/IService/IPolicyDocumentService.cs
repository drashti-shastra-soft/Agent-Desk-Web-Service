using AgentDeskWebService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IPolicyDocumentService
    {
        Domain.PolicyDocument Upsert(Domain.PolicyDocument mPolicyDocument);
        Domain.PolicyDocument Get(int id);
        void Delete(int id, int loginUserId);
        List<PolicyDocument> Fetch(int policyId);
    }
}
