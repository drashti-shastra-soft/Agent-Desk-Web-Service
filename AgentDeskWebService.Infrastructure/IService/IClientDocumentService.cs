using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IClientDocumentService
    {
        Domain.ClientDocument Upsert(Domain.ClientDocument mClientDocument);

        Domain.ClientDocument Get(int id);

        List<Domain.ClientDocument> Fetch(int clientId);

        void Delete(int id, int loginUserId);
    }
}
