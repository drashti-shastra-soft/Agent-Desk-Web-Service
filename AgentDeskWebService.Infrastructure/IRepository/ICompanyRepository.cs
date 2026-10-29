using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebService.Infrastructure.IRepository
{
    public interface ICompanyRepository
    {
        Domain.CompanyLister GetAll(Domain.CompanyLister mLister);

        Domain.Company Upsert(Domain.Company mCompany);

        Domain.Company Get(int id);

        void Delete(int id, int loginUserId);

        List<Domain.Company> GetActiveCompanies();
    }
}
