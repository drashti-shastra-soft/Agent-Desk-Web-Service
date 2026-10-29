using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyDocumentRepository
    {
        Domain.PolicyDocument Upsert(Domain.PolicyDocument mPolicyDocument);

        Domain.PolicyDocument Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.PolicyDocument> Fetch(int policyId);
    }
}
