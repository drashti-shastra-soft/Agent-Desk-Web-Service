using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IPolicyTypeRepository
    {
        List<Domain.PolicyType> GetAll();
    }
}
