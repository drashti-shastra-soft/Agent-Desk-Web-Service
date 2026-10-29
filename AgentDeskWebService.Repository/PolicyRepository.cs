using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class PolicyRepository : IPolicyRepository
    {
        public Domain.PolicyLister GetAll(Domain.PolicyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from p in context.Policies
                                join c in context.Clients on p.ClientId equals c.Id
                                join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                join ps in context.PolicyStatus on p.PolicyStatusId equals ps.Id
                                where p.DeletedBy == null && p.DeletedDate == null
                                orderby p.Id descending
                                select new
                                {
                                    Id = p.Id,
                                    PolicyTypeId = p.PolicyTypeId,
                                    CompanyId = p.CompanyId,
                                    PolicyType = pt.Name,
                                    PolicyStatusId = p.PolicyStatusId,
                                    PolicyStatus = ps.Name,
                                    PolicyNumber = p.PolicyNumber,
                                    ClientId = p.ClientId,
                                    SumAssured = p.SumAssured,
                                    IDV = p.IDV,
                                    FUPDate = p.FUPDate,
                                    CommercementDate = p.CommercementDate,
                                    MaturityDate = p.MaturityDate,
                                    StartDate = p.StartDate,
                                    EndDate = p.EndDate,
                                    PremiumAmount = p.PremiumAmount,
                                    PayMode = p.PayMode,
                                    IsActive = p.IsActive,
                                    NeedToPremiumCollect = p.NeedToPremiumCollect,
                                    Client = c.FirstName + " " + (c.MiddleName == null || c.MiddleName == "" ? "" : c.MiddleName + " ") + c.LastName,
                                    DOB = c.DOB,
                                    Term = p.Term,
                                    PPT = p.PPT,
                                    Plan  = p.Plan
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.CompanyId > 0)
                            query = query.Where(x => x.CompanyId == criteria.CompanyId);

                        if (criteria.ClientId > 0)
                            query = query.Where(x => x.ClientId == criteria.ClientId);

                        if (criteria.PolicyNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PolicyNumber) && x.PolicyNumber.ToUpper().Contains(criteria.PolicyNumber.ToUpper()));

                        if (criteria.PolicyTypeId > 0)
                            query = query.Where(x => x.PolicyTypeId == criteria.PolicyTypeId);

                        if (criteria.PolicyStatusId > 0)
                            query = query.Where(x => x.PolicyStatusId == criteria.PolicyStatusId);


                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }

                    // Switch to in-memory for complex string filters
                    var efPolicy = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicy.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicy = efPolicy.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicy.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Policy Upsert(Domain.Policy mPolicy)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = context.Policies.FirstOrDefault(x => x.Id == mPolicy.Id);

                    if (efPolicy == null || efPolicy.Id <= default(int))
                    {
                        efPolicy = new Persistence.Policy();
                        efPolicy.CreatedBy = mPolicy.CreatedBy;
                        efPolicy.CreatedDate = DateTime.Now;
                        context.Policies.Add(efPolicy);
                    }
                    else
                    {
                        efPolicy.LastModifiedBy = mPolicy.LastModifiedBy;
                        efPolicy.LastModifiedDate = DateTime.Now;
                    }

                    efPolicy.ClientId = mPolicy.ClientId;
                    efPolicy.CompanyId = mPolicy.CompanyId;
                    efPolicy.PolicyTypeId = mPolicy.PolicyTypeId;
                    efPolicy.PolicyNumber = mPolicy.PolicyNumber;
                    efPolicy.SumAssured = mPolicy.SumAssured;
                    efPolicy.IDV = mPolicy.IDV;
                    efPolicy.FUPDate = mPolicy.FUPDate;
                    efPolicy.CommercementDate = mPolicy.CommercementDate;
                    efPolicy.MaturityDate = mPolicy.MaturityDate;
                    efPolicy.StartDate = mPolicy.StartDate;
                    efPolicy.EndDate = mPolicy.EndDate;
                    efPolicy.PremiumAmount = mPolicy.PremiumAmount;
                    efPolicy.PolicyStatusId = mPolicy.PolicyStatusId;
                    efPolicy.PayMode = mPolicy.PayMode;
                    efPolicy.FilePath = mPolicy.FilePath;
                    efPolicy.MIME = mPolicy.MIME;
                    efPolicy.IsActive = mPolicy.IsActive;
                    efPolicy.Term = mPolicy.Term;
                    efPolicy.PPT = mPolicy.PPT;
                    efPolicy.Plan = mPolicy.Plan;
                    efPolicy.NeedToPremiumCollect = mPolicy.NeedToPremiumCollect;

                    context.SaveChanges();
                    mPolicy.Id = efPolicy.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public Domain.Policy Get(int id)
        {
            var mPolicy = new Domain.Policy();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = (from p in context.Policies
                                    where p.DeletedDate == null && p.DeletedBy == null && p.Id == id
                                    select new
                                    {
                                        Id = p.Id,
                                        ClientId = p.ClientId,
                                        PolicyTypeId = p.PolicyTypeId,
                                        CompanyId = p.CompanyId,
                                        PolicyNumber = p.PolicyNumber,
                                        SumAssured = p.SumAssured,
                                        IDV = p.IDV,
                                        FUPDate = p.FUPDate,
                                        CommercementDate = p.CommercementDate,
                                        MaturityDate = p.MaturityDate,
                                        StartDate = p.StartDate,
                                        EndDate = p.EndDate,
                                        PremiumAmount = p.PremiumAmount,
                                        PolicyStatusId = p.PolicyStatusId,
                                        PayMode = p.PayMode,
                                        FilePath = p.FilePath,
                                        MIME = p.MIME,
                                        IsActive = p.IsActive,
                                        NeedToPremiumCollect = p.NeedToPremiumCollect,
                                        Term = p.Term,
                                        PPT = p.PPT,
                                        Plan = p.Plan
                                    }).FirstOrDefault();

                    if (efPolicy != null)
                    {
                        mPolicy = JsonSerializer.Deserialize<Domain.Policy>(JsonSerializer.Serialize(efPolicy));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = context.Policies.Where(x => x.Id == id).FirstOrDefault();
                    if (efPolicy != null && efPolicy.Id > default(int))
                    {
                        efPolicy.DeletedBy = loginUserId;
                        efPolicy.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Domain.Policy GetByPolicyNumber(string policyNumber)
        {
            var mPolicy = new Domain.Policy();

            using (var context = new Persistence.AgentDeskDBEntities())
            {
                var efPolicy = (from p in context.Policies
                                join c in context.Clients on p.ClientId equals c.Id
                                where p.PolicyNumber == policyNumber
                                select new
                                {
                                    p.Id,
                                    p.ClientId,
                                    p.PolicyNumber,
                                    p.PremiumAmount,
                                    ClientName = c.FirstName + " " + c.LastName
                                }).FirstOrDefault();

                if (efPolicy != null)
                {
                    mPolicy.Id = efPolicy.Id;
                    mPolicy.ClientId = efPolicy.ClientId;
                    mPolicy.PolicyNumber = efPolicy.PolicyNumber;
                    mPolicy.PremiumAmount = efPolicy.PremiumAmount;
                    mPolicy.ClientName = efPolicy.ClientName;
                }
            }

            return mPolicy;
        }

        public Domain.Policy GetPdf(int id)
        {
            var mPolicy = new Domain.Policy();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = context.Policies.FirstOrDefault(x => x.Id == id);

                    if (efPolicy != null)
                    {
                        var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Policy", efPolicy.FilePath);

                        if (System.IO.File.Exists(filePath))
                        {
                            byte[] bytes = System.IO.File.ReadAllBytes(filePath);

                            mPolicy.Id = efPolicy.Id;
                            mPolicy.MIME = efPolicy.MIME;
                            mPolicy.Base64String = Convert.ToBase64String(bytes);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mPolicy;
        }

        public bool CheckPolicyNumberExist(Domain.Policy mPolicy)
        {
            bool isExist = false;
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    isExist = context.Policies.Where(x => x.Id != mPolicy.Id && x.PolicyNumber.Trim().ToUpper() == mPolicy.PolicyNumber.Trim().ToUpper()).Count() > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isExist;
        }

        public Domain.Policy ImportPolicy(Domain.Policy mPolicy)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = context.Policies.FirstOrDefault(x => x.PolicyNumber == mPolicy.PolicyNumber && x.DeletedDate == null && x.DeletedBy == null);

                    if (efPolicy == null)
                    {
                        efPolicy = new Persistence.Policy();
                        efPolicy.CreatedBy = mPolicy.CreatedBy;
                        efPolicy.CreatedDate = DateTime.Now;
                        efPolicy.PolicyTypeId = mPolicy.PolicyTypeId;
                        efPolicy.PolicyStatusId = mPolicy.PolicyStatusId;
                        context.Policies.Add(efPolicy);
                    }
                    else
                    {
                        efPolicy.LastModifiedBy = mPolicy.LastModifiedBy;
                        efPolicy.LastModifiedDate = DateTime.Now;
                    }

                    efPolicy.ClientId = mPolicy.ClientId;
                    efPolicy.CompanyId = mPolicy.CompanyId;
                    efPolicy.PolicyNumber = mPolicy.PolicyNumber;
                    efPolicy.SumAssured = mPolicy.SumAssured;
                    efPolicy.PremiumAmount = mPolicy.PremiumAmount;
                    efPolicy.PayMode = mPolicy.PayMode;
                    efPolicy.CommercementDate = mPolicy.CommercementDate;
                    efPolicy.FUPDate = mPolicy.FUPDate;
                    efPolicy.Term = mPolicy.Term;
                    efPolicy.PPT = mPolicy.PPT;
                    efPolicy.Plan = mPolicy.Plan;
                    efPolicy.MaturityDate = mPolicy.MaturityDate;
                    efPolicy.IsActive = mPolicy.IsActive = true;

                    context.SaveChanges();
                    mPolicy.Id = efPolicy.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

       
    }
}
