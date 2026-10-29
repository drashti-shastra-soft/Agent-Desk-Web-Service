using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public List<Domain.Policy> GetAllLifeInsurance()
        {
            return _repo.GetAllLifeInsurance();
        }

        public List<Domain.Policy> GetAllHealthInsurance()
        {
            return _repo.GetAllHealthInsurance();
        }

        public List<Domain.Policy> GetAllPropertyInsurance()
        {
            return _repo.GetAllPropertyInsurance();
        }
        
        public List<Domain.Policy> GetAllVehicleInsurance()
        {
            return _repo.GetAllVehicleInsurance();
        }

        public int GetTotalLifeInsurance()
        {
            return _repo.GetLifeInsuranceCount();
        }

        public int GetHealthInsuranceCount()
        {
            return _repo.GetHealthInsuranceCount();
        }

        public int GetPropertyInsuranceCount()
        {
            return _repo.GetPropertyInsuranceCount();
        }

        public int GetVehicleInsuranceCount()
        {
            return _repo.GetVehicleInsuranceCount();
        }
    }
}
