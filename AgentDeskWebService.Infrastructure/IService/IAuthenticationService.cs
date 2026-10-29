using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IAuthenticationService
    {
        Domain.User Authenticate(Domain.Credential mCredential);

    }
}
