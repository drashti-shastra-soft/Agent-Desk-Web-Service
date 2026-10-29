using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IAddressRepository
    {
        Domain.Address Upsert(Domain.Address mAddress);
        Domain.Address Get(int id);
        Domain.Address GetByClientId(int clientId);
        Domain.Address GetByAddress(Domain.Address mAddress);
    }
}
