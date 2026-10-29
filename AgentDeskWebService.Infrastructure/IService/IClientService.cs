using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IClientService
    {
        Domain.ClientLister GetAll(Domain.ClientLister mLister);

        Domain.Client Upsert(Domain.Client mClient);

        Domain.Client Get(int id);

        void Delete(int id, int loginUserId);

        void UpdateAddress(int id, int loginUserId, int addressId);
    }
}
