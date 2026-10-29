using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IAgencyService
    {
        Domain.AgencyLister GetAll(Domain.AgencyLister mLister);

        Domain.Agency Upsert(Domain.Agency mAgency);

        Domain.Agency Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Agency> GetActiveAgency();
    }
}
