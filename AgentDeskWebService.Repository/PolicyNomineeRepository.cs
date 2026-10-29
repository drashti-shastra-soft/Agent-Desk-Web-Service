using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class PolicyNomineeRepository : IPolicyNomineeRepository
    {

        public Domain.PolicyNomineeLister GetAll(Domain.PolicyNomineeLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from pn in context.PolicyNominees
                                join p in context.Policies on pn.PolicyId equals p.Id
                                where pn.DeletedBy == null && pn.DeletedDate == null
                                select new
                                {
                                    Id = pn.Id,
                                    PolicyId = pn.PolicyId,
                                    Name = pn.Name,
                                    Age = pn.Age,
                                    Relation = pn.Relation,
                                    NomineePercentage = pn.NomineePercentage,
                                    IsActive = pn.IsActive,
                                    Policy = p.PolicyNumber,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;
                        

                        if (criteria.PolicyId > 0)
                            query = query.Where(x => x.PolicyId == criteria.PolicyId);

                        if (criteria.Age > 0)
                            query = query.Where(x => x.Age == criteria.Age);

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }

                    // Switch to in-memory for complex string filters
                    var efPolicyNominee = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicyNominee.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicyNominee = efPolicyNominee.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.PolicyNominee>>(JsonSerializer.Serialize(efPolicyNominee.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.PolicyNominee Upsert(Domain.PolicyNominee mPolicyNominee)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyNominee = context.PolicyNominees.FirstOrDefault(x => x.Id == mPolicyNominee.Id);

                    if (efPolicyNominee == null || efPolicyNominee.Id <= default(int))
                    {
                        efPolicyNominee = new Persistence.PolicyNominee();
                        efPolicyNominee.CreatedBy = mPolicyNominee.CreatedBy;
                        efPolicyNominee.CreatedDate = DateTime.Now;
                        context.PolicyNominees.Add(efPolicyNominee);
                    }
                    else
                    {
                        efPolicyNominee.LastModifiedBy = mPolicyNominee.LastModifiedBy;
                        efPolicyNominee.LastModifiedDate = DateTime.Now;
                    }

                    efPolicyNominee.PolicyId = mPolicyNominee.PolicyId;
                    efPolicyNominee.Name = mPolicyNominee.Name;
                    efPolicyNominee.Age = mPolicyNominee.Age;
                    efPolicyNominee.Relation = mPolicyNominee.Relation;
                    efPolicyNominee.NomineePercentage = mPolicyNominee.NomineePercentage;
                    efPolicyNominee.IsActive = mPolicyNominee.IsActive;

                    context.SaveChanges();
                    mPolicyNominee.Id = efPolicyNominee.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyNominee;
        }

        public Domain.PolicyNominee Get(int id)
        {
            var mPolicyNominee = new Domain.PolicyNominee();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyNominee = (from pn in context.PolicyNominees
                                    where pn.DeletedDate == null && pn.DeletedBy == null && pn.Id == id
                                    select new
                                    {
                                        Id = pn.Id,
                                        PolicyId = pn.PolicyId,
                                        Name = pn.Name,
                                        Age = pn.Age,
                                        Relation = pn.Relation,
                                        NomineePercentage = pn.NomineePercentage,
                                        IsActive = pn.IsActive,
                                    }).FirstOrDefault();

                    if (efPolicyNominee != null)
                    {
                        mPolicyNominee = JsonSerializer.Deserialize<Domain.PolicyNominee>(JsonSerializer.Serialize(efPolicyNominee));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyNominee;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyNominee = context.PolicyNominees.Where(x => x.Id == id).FirstOrDefault();
                    if (efPolicyNominee != null && efPolicyNominee.Id > default(int))
                    {
                        efPolicyNominee.DeletedBy = loginUserId;
                        efPolicyNominee.DeletedDate = DateTime.Now;
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
