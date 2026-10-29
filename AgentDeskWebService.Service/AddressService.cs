using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;
        private readonly IClientRepository _clientRepo;

        public AddressService(IAddressRepository repo, IClientRepository clientRepo)
        {
            _repo = repo;
            _clientRepo = clientRepo;
        }

        public Domain.Address Upsert(Domain.Address mAddress)
        {
            mAddress = _repo.Upsert(mAddress);

            if (mAddress.ClientId > 0)
            {
                _clientRepo.UpdateAddress(mAddress.ClientId.Value, mAddress.Id, mAddress.LastModifiedBy);
            }

            return mAddress;
        }

        public Domain.Address Get(int id)
        {
            return _repo.Get(id);
        }

        public Domain.Address GetByClientId(int clientId)
        {
            return _repo.GetByClientId(clientId);
        }
    }
}
