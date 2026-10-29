using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class AgencyRepository : IAgencyRepository
    {
        public Domain.AgencyLister GetAll(Domain.AgencyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from a in context.Agencies
                                where a.DeletedBy == null && a.DeletedDate == null
                                orderby a.Id descending
                                select new
                                {
                                    Id = a.Id,
                                    CompanyId = a.CompanyId,
                                    AgencyType = a.AgencyType,
                                    Code = a.Code,
                                    Name = a.Name,
                                    MobileNumber = a.MobileNumber,
                                    EmailAddress = a.EmailAddress,
                                    CompanyName = a.CompanyName,
                                    IsActive = a.IsActive,
                                };

                    // Apply server-side filters where possible
                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.CompanyId > 0)
                            query = query.Where(x => x.CompanyId == criteria.CompanyId);

                        if (criteria.Code.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Code) && x.Code.ToUpper().Contains(criteria.Code.ToUpper()));

                        if (criteria.Name.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Name) && x.Name.ToUpper().Contains(criteria.Name.ToUpper()));

                        if (criteria.MobileNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.ToUpper().Contains(criteria.MobileNumber.ToUpper()));

                        if (criteria.EmailAddress.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.EmailAddress) && x.EmailAddress.ToUpper().Contains(criteria.EmailAddress.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }
                    // Switch to in-memory for complex string filters
                    var efAgencies = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efAgencies.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efAgencies = efAgencies.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Agency>>(JsonSerializer.Serialize(efAgencies.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Agency Upsert(Domain.Agency mAgency)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAgency = context.Agencies.FirstOrDefault(x => x.Id == mAgency.Id);

                    if (efAgency == null || efAgency.Id <= default(int))
                    {
                        efAgency = new Persistence.Agency();
                        efAgency.CreatedBy = mAgency.CreatedBy;
                        efAgency.CreatedDate = DateTime.Now;
                        context.Agencies.Add(efAgency);
                    }
                    else
                    {
                        efAgency.LastModifiedBy = mAgency.LastModifiedBy;
                        efAgency.LastModifiedDate = DateTime.Now;
                    }

                    efAgency.CompanyId = mAgency.CompanyId;
                    efAgency.AgencyType = mAgency.AgencyType;
                    efAgency.Code = mAgency.Code;
                    efAgency.Name = mAgency.Name;
                    efAgency.MobileNumber = mAgency.MobileNumber;
                    efAgency.EmailAddress = mAgency.EmailAddress;
                    efAgency.CompanyName = mAgency.CompanyName;
                    efAgency.IsActive = mAgency.IsActive;

                    context.SaveChanges();
                    mAgency.Id = efAgency.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAgency;
        }

        public Domain.Agency Get(int id)
        {
            var mAgency = new Domain.Agency();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAgency = (from a in context.Agencies
                                   where a.DeletedDate == null && a.DeletedBy == null && a.Id == id
                                   select new
                                   {
                                       Id = a.Id,
                                       CompanyId = a.CompanyId,
                                       AgencyType = a.AgencyType,
                                       Code = a.Code,
                                       Name = a.Name,
                                       MobileNumber = a.MobileNumber,
                                       EmailAddress = a.EmailAddress,
                                       CompanyName = a.CompanyName,
                                       IsActive = a.IsActive,
                                   }).FirstOrDefault();

                    if (efAgency != null)
                    {
                        mAgency = JsonSerializer.Deserialize<Domain.Agency>(JsonSerializer.Serialize(efAgency));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAgency;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAgency = context.Agencies.Where(x => x.Id == id).FirstOrDefault();
                    if (efAgency != null && efAgency.Id > default(int))
                    {
                        efAgency.DeletedBy = loginUserId;
                        efAgency.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.Agency> GetActiveAgency()
        {
            var mAgency = new List<Domain.Agency>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAgency = (from g in context.Agencies
                                    where g.DeletedDate == null && g.DeletedBy == null && g.IsActive
                                   select new
                                   {
                                       Id = g.Id,
                                       Name = g.Code + " - " + g.Name
                                   }).AsEnumerable();

                    mAgency = JsonSerializer.Deserialize<List<Domain.Agency>>(JsonSerializer.Serialize(efAgency.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAgency;
        }
    }
}
