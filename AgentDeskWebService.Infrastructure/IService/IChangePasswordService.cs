using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IChangePasswordService
    {
        void Upsert(Domain.ChangePassword mChangePassword);
    }
}