using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface IClientRepository
    {
        Domain.ClientLister GetAll(Domain.ClientLister mLister);

        Domain.Client Upsert(Domain.Client mClient);

        Domain.Client Get(int id);

        void Delete(int id, int loginUserId);

        void UpdateAddress(int id, int addressId, int loginUserId);

        void UpdateAddress(int id, int addressId, int? loginUserId);
        Domain.Client GetByDetails(string firstName, string middleName, string lastName, string phoneNumber, DateTime? dob, int companyId);

    }
}
