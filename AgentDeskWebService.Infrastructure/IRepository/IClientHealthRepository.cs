using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IClientHealthRepository
    {
        Domain.ClientHealth Upsert(Domain.ClientHealth mClientHealth);
        Domain.ClientHealth Get(int id);
        void Delete(int id, int loginUserId);

    }
}
