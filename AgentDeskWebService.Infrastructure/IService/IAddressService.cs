using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IAddressService
    {
        Domain.Address Upsert(Domain.Address mAddress);

        Domain.Address Get(int id);

        Domain.Address GetByClientId(int clientId);
    }
}
