using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IClientFinancialInfoService
    {
        Domain.ClientFinancialInfo Upsert(Domain.ClientFinancialInfo mClientFinancial);
        Domain.ClientFinancialInfo Get(int id);
        void Delete(int id, int loginUserId);
    }
}
