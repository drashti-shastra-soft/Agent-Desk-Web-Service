using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IGroupRepository
    {
        Domain.GroupLister GetAll(Domain.GroupLister mLister);

        Domain.Group Upsert(Domain.Group mGroup);

        Domain.Group Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Group> GetActiveGroup();
        Domain.Group GetByName(string name, int companyId);

        bool CodeExists(int companyId, string code);
    }
}
