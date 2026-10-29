using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IClientIdentificationService
    {
        Domain.ClientIdentification Upsert(Domain.ClientIdentification mClientIdentification);
        Domain.ClientIdentification Get(int id);
        void Delete(int id, int loginUserId);

    }
}
