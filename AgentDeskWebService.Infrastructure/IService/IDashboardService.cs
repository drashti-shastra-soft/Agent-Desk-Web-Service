using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IDashboardService
    {
        List<Domain.Policy> GetAllLifeInsurance();

        List<Domain.Policy> GetAllHealthInsurance();

        List<Domain.Policy> GetAllPropertyInsurance();

        List<Domain.Policy> GetAllVehicleInsurance();

        int GetTotalLifeInsurance();

        int GetHealthInsuranceCount();

        int GetPropertyInsuranceCount();

        int GetVehicleInsuranceCount();
    }
}
