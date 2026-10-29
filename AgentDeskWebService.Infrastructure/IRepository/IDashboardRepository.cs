using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IDashboardRepository
    {
        List<Domain.Policy> GetAllLifeInsurance();

        List<Domain.Policy> GetAllHealthInsurance();

        List<Domain.Policy> GetAllPropertyInsurance();

        List<Domain.Policy> GetAllVehicleInsurance();

        int GetLifeInsuranceCount();
        int GetHealthInsuranceCount();
        int GetPropertyInsuranceCount();
        int GetVehicleInsuranceCount();
    }
}
