using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;

namespace AgentDeskWebService.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;

        public ClientService(IClientRepository repo)
        {
            _repo = repo;
        }

        public Domain.ClientLister GetAll(Domain.ClientLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Client Upsert(Domain.Client mClient)
        {
            return _repo.Upsert(mClient);
        }

        public Domain.Client Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public void UpdateAddress(int id, int loginUserId, int addressId)
        {
            _repo.UpdateAddress(id, loginUserId, addressId);
        }

    }
}
