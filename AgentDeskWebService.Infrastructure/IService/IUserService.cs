using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IService
{
    public interface IUserService
    {
        Domain.UserLister GetAll(Domain.UserLister mLister);
        Domain.User Upsert(Domain.User mUser);
        Domain.User Get(int id);
        void Delete(int id, int loginUserId);
        bool CheckEmailAddressExist(Domain.User mUser);
    }
}
