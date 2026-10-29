using System.Collections.Generic;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IPolicyStatusService
    {
        List<Domain.PolicyStatus> GetAll();

        List<Domain.PolicyStatus> GetActiveBy(string policyTypes);
    }
}
