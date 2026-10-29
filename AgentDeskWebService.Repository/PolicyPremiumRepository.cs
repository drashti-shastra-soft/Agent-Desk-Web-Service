using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Linq;

namespace AgentDeskWebService.Repository
{
    public class PolicyPremiumRepository : IPolicyPremiumRepository
    {
        //public Domain.PolicyPremium Upsert(Domain.PolicyPremium mPolicyPremium)
        //{
        //    try
        //    {
        //        using (var context = new Persistence.AgentDeskDBEntities())
        //        {
        //            var efPolicyPremium = context.PolicyPremiums.FirstOrDefault(x => x.Id == mPolicyPremium.Id);
        //            if (efPolicyPremium == null || efPolicyPremium.Id <= default(int))
        //            {
        //                efPolicyPremium = new Persistence.PolicyPremium
        //                {
        //                    CreatedBy = mPolicyPremium.CreatedBy,
        //                    CreatedDate = DateTime.Now,
        //                };
        //                context.PolicyPremiums.Add(efPolicyPremium);
        //            }

        //            efPolicyPremium.PolicyNumber = mPolicyPremium.PolicyNumber;
        //            efPolicyPremium.DueDate = mPolicyPremium.DueDate;
        //            efPolicyPremium.AdjDate = mPolicyPremium.AdjDate;
        //            efPolicyPremium.Premium = mPolicyPremium.Premium;
        //            efPolicyPremium.IsAdjusted = mPolicyPremium.IsAdjusted;

        //            context.SaveChanges();
        //            mPolicyPremium.Id = efPolicyPremium.Id;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}

        public Domain.PolicyPremium Upsert(Domain.PolicyPremium mPolicyPremium)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyPremium = context.PolicyPremiums.FirstOrDefault(x => x.Id == mPolicyPremium.Id);

                    if (efPolicyPremium == null || efPolicyPremium.Id <= default(int))
                    {
                        efPolicyPremium = new Persistence.PolicyPremium();
                        efPolicyPremium.CreatedBy = mPolicyPremium.CreatedBy;
                        efPolicyPremium.CreatedDate = DateTime.Now;
                        context.PolicyPremiums.Add(efPolicyPremium);
                    }

                    efPolicyPremium.PolicyNumber = mPolicyPremium.PolicyNumber;
                    efPolicyPremium.DueDate = mPolicyPremium.DueDate;
                    efPolicyPremium.AdjDate = mPolicyPremium.AdjDate;
                    efPolicyPremium.Premium = mPolicyPremium.Premium;
                    efPolicyPremium.IsAdjusted = mPolicyPremium.IsAdjusted;

                    context.SaveChanges();
                    mPolicyPremium.Id = efPolicyPremium.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyPremium;
        }


        public Domain.PolicyPremium UpsertAndUpdateFup(Domain.PolicyPremium mPolicyPremium)
        {
            using (var context = new Persistence.AgentDeskDBEntities())
            using (var tx = context.Database.BeginTransaction())
            {
                var adjDate = mPolicyPremium.AdjDate.Date;

                // verify the policy number (unique, so no company filter)
                var efPolicy = context.Policies.FirstOrDefault(p => p.PolicyNumber == mPolicyPremium.PolicyNumber
                                                                 && p.DeletedDate == null && p.DeletedBy == null);
                if (efPolicy == null)
                    throw new APIRequestFailedException("Policy number not found!");

                //  premium row (re-uploading the same policy + AdjDate updates it, no duplicate)
                var efPremium = context.PolicyPremiums.FirstOrDefault(x => x.Id == mPolicyPremium.Id)
                             ?? context.PolicyPremiums.FirstOrDefault(x => x.PolicyNumber == mPolicyPremium.PolicyNumber
                                                                        && x.AdjDate == adjDate);
                if (efPremium == null)
                {
                    efPremium = new Persistence.PolicyPremium();
                    efPremium.CreatedBy = mPolicyPremium.CreatedBy;
                    efPremium.CreatedDate = DateTime.Now;
                    context.PolicyPremiums.Add(efPremium);
                }

                efPremium.PolicyNumber = mPolicyPremium.PolicyNumber;
                efPremium.DueDate = mPolicyPremium.DueDate;
                efPremium.AdjDate = adjDate;
                efPremium.Premium = mPolicyPremium.Premium;
                efPremium.IsAdjusted = true;                

                // 3. next FUP date = AdjDate
                efPolicy.FUPDate = adjDate;
                efPolicy.LastModifiedBy = mPolicyPremium.CreatedBy;
                efPolicy.LastModifiedDate = DateTime.Now;

                context.SaveChanges();
                tx.Commit();

                mPolicyPremium.Id = efPremium.Id;
                mPolicyPremium.IsAdjusted = true;
            }
            return mPolicyPremium;
        }
    }
}
