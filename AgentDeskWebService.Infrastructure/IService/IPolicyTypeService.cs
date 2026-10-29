using System.Collections.Generic;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IPolicyTypeService
    {
        List<Domain.PolicyType> GetAll();
    }
}
