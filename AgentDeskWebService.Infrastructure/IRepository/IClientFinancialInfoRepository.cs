using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IClientFinancialInfoRepository
    {
        Domain.ClientFinancialInfo Upsert(Domain.ClientFinancialInfo mClientFinancialInfo);
        Domain.ClientFinancialInfo Get(int id);
        void Delete(int id, int loginUserId);
    }
}
