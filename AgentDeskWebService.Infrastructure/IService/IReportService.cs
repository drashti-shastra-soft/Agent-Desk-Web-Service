using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IReportService
    {
        Domain.PolicyLister GetAllLifeInsuranceList(Domain.PolicyLister mLister);

        Domain.PolicyLister GetAllHealthInsuranceList(Domain.PolicyLister mLister);

        Domain.PolicyLister GetAllPropertyInsuranceList(Domain.PolicyLister mLister);

        Domain.PolicyLister GetAllVehicleInsuranceList(Domain.PolicyLister mLister);

        List<Domain.Report> PremiumCollectionReport();

    }
}
