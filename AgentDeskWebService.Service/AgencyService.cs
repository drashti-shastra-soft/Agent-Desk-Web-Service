using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System.Collections.Generic;

namespace AgentDeskWebService.Service
{
    public class AgencyService : IAgencyService
    {
        private readonly IAgencyRepository _repo;

        public AgencyService(IAgencyRepository repo)
        {
            _repo = repo;
        }

        public Domain.AgencyLister GetAll(Domain.AgencyLister mLister)
        {
            return _repo.GetAll(mLister);
        }

        public Domain.Agency Upsert(Domain.Agency mAgency)
        {
            return _repo.Upsert(mAgency);
        }

        public Domain.Agency Get(int id)
        {
            return _repo.Get(id);
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

        public List<Domain.Agency> GetActiveAgency()
        {
            return _repo.GetActiveAgency();
        }
    }
}
