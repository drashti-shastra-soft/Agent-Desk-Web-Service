using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace AgentDeskWebService.Repository
{
    public class GroupRepository : IGroupRepository
    {
        public Domain.GroupLister GetAll(Domain.GroupLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from g in context.Groups
                                where g.DeletedBy == null && g.DeletedDate == null
                                orderby g.Id descending
                                select new
                                {
                                    Id = g.Id,
                                    CompanyId = g.CompanyId,
                                    Code = g.Code,
                                    Name = g.Name,
                                    HeadName = g.HeadName,
                                    MobileNumber = g.MobileNumber,
                                    IsActive = g.IsActive,
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

                        if (criteria.HeadName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.HeadName) && x.HeadName.ToUpper().Contains(criteria.HeadName.ToUpper()));

                        if (criteria.MobileNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.MobileNumber) && x.MobileNumber.ToUpper().Contains(criteria.MobileNumber.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }
                    // Switch to in-memory for complex string filters
                    var efGroups = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efGroups.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efGroups = efGroups.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Group>>(JsonSerializer.Serialize(efGroups.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Group Upsert(Domain.Group mGroup)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efGroup = context.Groups.FirstOrDefault(x => x.Id == mGroup.Id);

                    if (efGroup == null || efGroup.Id <= default(int))
                    {
                        efGroup = new Persistence.Group();
                        efGroup.CreatedBy = mGroup.CreatedBy;
                        efGroup.CreatedDate = DateTime.Now;
                        context.Groups.Add(efGroup);
                    }
                    else
                    {
                        efGroup.LastModifiedBy = mGroup.LastModifiedBy;
                        efGroup.LastModifiedDate = DateTime.Now;
                    }

                    efGroup.CompanyId = mGroup.CompanyId;
                    efGroup.Code = mGroup.Code;
                    efGroup.Name = mGroup.Name;
                    efGroup.HeadName = mGroup.HeadName;
                    efGroup.MobileNumber = mGroup.MobileNumber;
                    efGroup.IsActive = mGroup.IsActive;

                    context.SaveChanges();
                    mGroup.Id = efGroup.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mGroup;
        }

        public Domain.Group Get(int id)
        {
            var mGroup = new Domain.Group();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efGroup = (from g in context.Groups
                                   where g.DeletedDate == null && g.DeletedBy == null && g.Id == id
                                   select new
                                   {
                                       Id = g.Id,
                                       CompanyId = g.CompanyId,
                                       Code = g.Code,
                                       Name = g.Name,
                                       HeadName = g.HeadName,
                                       MobileNumber = g.MobileNumber,
                                       IsActive = g.IsActive,
                                   }).FirstOrDefault();

                    if (efGroup != null)
                    {
                        mGroup = JsonSerializer.Deserialize<Domain.Group>(JsonSerializer.Serialize(efGroup));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mGroup;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efGroup = context.Groups.Where(x => x.Id == id).FirstOrDefault();
                    if (efGroup != null && efGroup.Id > default(int))
                    {
                        efGroup.DeletedBy = loginUserId;
                        efGroup.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.Group> GetActiveGroup()
        {
            var mGroup = new List<Domain.Group>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efGroup = (from g in context.Groups
                                   where g.DeletedDate == null && g.DeletedBy == null && g.IsActive
                                   select new
                                   {
                                       Id = g.Id,
                                       Name = g.Code + " - " + g.Name
                                   }).AsEnumerable();

                    mGroup = JsonSerializer.Deserialize<List<Domain.Group>>(JsonSerializer.Serialize(efGroup.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mGroup;
        }

        public Domain.Group GetByName(string name ,int companyId)
        {
            name = (name ?? "").Trim();
            using (var context = new Persistence.AgentDeskDBEntities())
            {
                var ef = context.Groups.Where(g => g.Name == name && g.CompanyId == companyId).OrderBy(g => g.Id).FirstOrDefault();
                if (ef == null) return null;

                return new Domain.Group
                {
                    Id = ef.Id,
                    Code = ef.Code,
                    Name = ef.Name,
                    HeadName = ef.HeadName,
                    MobileNumber = ef.MobileNumber,
                    IsActive = ef.IsActive
                };
            }
        }

        public bool CodeExists(int companyId, string code)
        {
            using (var context = new Persistence.AgentDeskDBEntities())
            {
                return context.Groups.Any(g => g.CompanyId == companyId && g.Code == code);
            }
        }
    }
}
