using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyStatusRepository
    {
        List<Domain.PolicyStatus> GetAll();

        List<Domain.PolicyStatus> GetActiveBy(string policyTypes);


    }
}
