using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class ClientFinancialInfoRepository : IClientFinancialInfoRepository
    {
        public Domain.ClientFinancialInfo Upsert(Domain.ClientFinancialInfo mClientFinancialInfo)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientFinancialInfo = context.ClientFinancialInfoes.FirstOrDefault(x => x.Id == mClientFinancialInfo.Id);

                    if (efClientFinancialInfo == null || efClientFinancialInfo.Id <= default(int))
                    {
                        efClientFinancialInfo = new Persistence.ClientFinancialInfo();
                        efClientFinancialInfo.CreatedBy = mClientFinancialInfo.Id;
                        efClientFinancialInfo.CreatedDate = DateTime.Now;
                        context.ClientFinancialInfoes.Add(efClientFinancialInfo);
                    }
                    else
                    {
                        efClientFinancialInfo.LastModifiedBy = mClientFinancialInfo.LastModifiedBy;
                        efClientFinancialInfo.LastModifiedDate = DateTime.Now;
                    }

                    efClientFinancialInfo.ClientId = mClientFinancialInfo.ClientId;
                    efClientFinancialInfo.AnnualIncome = mClientFinancialInfo.AnnualIncome;
                    efClientFinancialInfo.ExistingLoansEMI = mClientFinancialInfo.ExistingLoansEMI;
                    efClientFinancialInfo.TaxSavingInvestments = mClientFinancialInfo.TaxSavingInvestments;
                    efClientFinancialInfo.BankName = mClientFinancialInfo.BankName;
                    efClientFinancialInfo.AccountHolder = mClientFinancialInfo.AccountHolder;
                    efClientFinancialInfo.AccountNumber = mClientFinancialInfo.AccountNumber;
                    efClientFinancialInfo.IFSCCode = mClientFinancialInfo.IFSCCode;
                    efClientFinancialInfo.Branch = mClientFinancialInfo.Branch;
                    efClientFinancialInfo.AccountType = mClientFinancialInfo.AccountType;

                    context.SaveChanges();
                    mClientFinancialInfo.Id = efClientFinancialInfo.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientFinancialInfo;
        }

        public Domain.ClientFinancialInfo Get(int id)
        {
            var mClientFinancialInfo = new Domain.ClientFinancialInfo();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientFinancial = (from cf in context.ClientFinancialInfoes
                                             where cf.DeletedDate == null && cf.DeletedBy == null && cf.ClientId == id
                                             select new
                                             {
                                                 Id = cf.Id,
                                                 ClientId = cf.ClientId,
                                                 AnnualIncome = cf.AnnualIncome,
                                                 ExistingLoansEMI = cf.ExistingLoansEMI,
                                                 TaxSavingInvestments = cf.TaxSavingInvestments,
                                                 BankName = cf.BankName,
                                                 AccountHolder = cf.AccountHolder,
                                                 AccountNumber = cf.AccountNumber,
                                                 IFSCCode = cf.IFSCCode,
                                                 Branch = cf.Branch,
                                                 AccountType = cf.AccountType,
                                             }).FirstOrDefault();

                    if (efClientFinancial != null)
                    {
                        mClientFinancialInfo = JsonSerializer.Deserialize<Domain.ClientFinancialInfo>(JsonSerializer.Serialize(efClientFinancial));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientFinancialInfo;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientFinancialInfo = context.ClientFinancialInfoes.Where(x => x.Id == id).FirstOrDefault();
                    if (efClientFinancialInfo != null && efClientFinancialInfo.Id > default(int))
                    {
                        efClientFinancialInfo.DeletedBy = loginUserId;
                        efClientFinancialInfo.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
