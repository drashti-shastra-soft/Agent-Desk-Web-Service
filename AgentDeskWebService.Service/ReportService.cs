using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Service
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public Domain.PolicyLister GetAllLifeInsuranceList(Domain.PolicyLister mLister)
        {
            return _repo.GetAllLifeInsuranceList(mLister);
        }

        public Domain.PolicyLister GetAllHealthInsuranceList(Domain.PolicyLister mLister)
        {
            return _repo.GetAllHealthInsuranceList(mLister);
        }

        public Domain.PolicyLister GetAllPropertyInsuranceList(Domain.PolicyLister mLister)
        {
            return _repo.GetAllPropertyInsuranceList(mLister);
        }
        
        public Domain.PolicyLister GetAllVehicleInsuranceList(Domain.PolicyLister mLister)
        {
            return _repo.GetAllVehicleInsuranceList(mLister);
        }

        public List<Domain.Report> PremiumCollectionReport()
        {
            return _repo.PremiumCollectionReport();
        }
    }
}
