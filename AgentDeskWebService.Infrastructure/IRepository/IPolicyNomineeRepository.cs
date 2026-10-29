using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyNomineeRepository
    {
        Domain.PolicyNomineeLister GetAll(Domain.PolicyNomineeLister mLister);
        Domain.PolicyNominee Upsert(Domain.PolicyNominee mPolicyNominee);
        Domain.PolicyNominee Get(int id);
        void Delete(int id, int loginUserId);
    }
}
