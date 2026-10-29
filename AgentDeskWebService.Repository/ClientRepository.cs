using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace AgentDeskWebService.Repository
{
    public class ClientRepository : IClientRepository
    {
        public Domain.ClientLister GetAll(Domain.ClientLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from c in context.Clients
                                join g in context.Groups on c.GroupId equals g.Id
                                where c.DeletedBy == null && c.DeletedDate == null
                                orderby c.Id descending
                                select new
                                {
                                    Id = c.Id,
                                    CompanyId = c.CompanyId,
                                    GroupId = c.GroupId,
                                    FirstName = c.FirstName,
                                    MiddleName = c.MiddleName,
                                    LastName = c.LastName,
                                    DOB = c.DOB,
                                    Gender = c.Gender,
                                    EmailAddress = c.EmailAddress,
                                    PhoneNumber = c.PhoneNumber,
                                    Group = g.Name,
                                    IsActive = c.IsActive
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.CompanyId > 0)
                            query = query.Where(x => x.CompanyId == criteria.CompanyId);

                        if (criteria.FirstName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.FirstName) && x.FirstName.ToUpper().Contains(criteria.FirstName.ToUpper()));

                        if (criteria.LastName.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.LastName) && x.LastName.ToUpper().Contains(criteria.LastName.ToUpper()));

                        if (criteria.EmailAddress.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.EmailAddress) && x.EmailAddress.ToUpper().Contains(criteria.EmailAddress.ToUpper()));

                        if (criteria.Group.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Group) && x.Group.ToUpper().Contains(criteria.Group.ToUpper()));

                        if (criteria.PhoneNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.ToUpper().Contains(criteria.PhoneNumber.ToUpper()));

                        if (criteria.Status.IsNotNullOrEmpty())
                        {
                            if (criteria.Status == "Active")
                                query = query.Where(x => x.IsActive);
                            else if (criteria.Status == "Disabled")
                                query = query.Where(x => !x.IsActive);
                        }
                    }
                    // Switch to in-memory for complex string filters
                    var efClient = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efClient.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efClient = efClient.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Client>>(JsonSerializer.Serialize(efClient.ToList()));

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.Client Upsert(Domain.Client mClient)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClient = context.Clients.FirstOrDefault(x => x.Id == mClient.Id);

                    if (efClient == null || efClient.Id <= default(int))
                    {
                        efClient = new Persistence.Client();
                        efClient.CreatedBy = mClient.CreatedBy;
                        efClient.CreatedDate = DateTime.Now;
                        context.Clients.Add(efClient);
                    }
                    else
                    {
                        efClient.LastModifiedBy = mClient.LastModifiedBy;
                        efClient.LastModifiedDate = DateTime.Now;
                    }

                    efClient.GroupId = mClient.GroupId;
                    efClient.CompanyId = mClient.CompanyId;
                    efClient.FirstName = mClient.FirstName;
                    efClient.MiddleName = mClient.MiddleName;
                    efClient.LastName = mClient.LastName;
                    efClient.AddressId = mClient.AddressId;
                    efClient.DOB = mClient.DOB;
                    efClient.Gender = mClient.Gender;
                    efClient.Occupation = mClient.Occupation;
                    efClient.MaritalStatus = mClient.MaritalStatus;
                    efClient.EmailAddress = mClient.EmailAddress;
                    efClient.PhoneNumber = mClient.PhoneNumber;
                    efClient.IsActive = mClient.IsActive;

                    context.SaveChanges();
                    mClient.Id = efClient.Id;

                }
            }
            catch (Exception)
            {
                throw;
            }
            return mClient;
        }

        public Domain.Client Get(int id)
        {
            var mClient = new Domain.Client();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClient = (from c in context.Clients
                                    where c.DeletedDate == null && c.DeletedBy == null && c.Id == id
                                    select new
                                    {
                                        Id = c.Id,
                                        AddressId = c.AddressId,
                                        GroupId = c.GroupId,
                                        CompanyId = c.CompanyId,
                                        FirstName = c.FirstName,
                                        MiddleName = c.MiddleName,
                                        LastName = c.LastName,
                                        EmailAddress = c.EmailAddress,
                                        PhoneNumber = c.PhoneNumber,
                                        DOB = c.DOB,
                                        Gender = c.Gender,
                                        MaritalStatus = c.MaritalStatus,
                                        Occupation = c.Occupation,
                                        IsActive = c.IsActive,
                                    }).FirstOrDefault();

                    if (efClient != null)
                    {
                        mClient = JsonSerializer.Deserialize<Domain.Client>(JsonSerializer.Serialize(efClient));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mClient;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClient = context.Clients.Where(x => x.Id == id).FirstOrDefault();
                    if (efClient != null && efClient.Id > default(int))
                    {
                        efClient.DeletedBy = loginUserId;
                        efClient.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAddress(int id, int addressId, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClient = context.Clients.FirstOrDefault(x => x.Id == id);
                    if (efClient != null)
                    {
                        efClient.AddressId = addressId;

                        //var audit = context.AuditTrails.Find(efClient.AuditId);
                        if (efClient != null)
                        {
                            efClient.LastModifiedBy = loginUserId;
                            efClient.LastModifiedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateAddress(int id, int addressId, int? loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClient = context.Clients.FirstOrDefault(x => x.Id == id);
                    if (efClient != null)
                    {
                        efClient.AddressId = addressId;
                        //var audit = context.AuditTrails.Find(efClient.AuditId);
                        if (efClient != null)
                        {
                            efClient.LastModifiedBy = loginUserId;
                            efClient.LastModifiedDate = DateTime.Now;
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Domain.Client GetByDetails(string firstName, string middleName, string lastName, string phoneNumber,DateTime? dob, int companyId)
        {
            var mClient = new Domain.Client();
            using (var context = new Persistence.AgentDeskDBEntities())
            {
                firstName = (firstName ?? "").Trim().ToLower();
                middleName = (middleName ?? "").Trim().ToLower();
                lastName = (lastName ?? "").Trim().ToLower();
                phoneNumber = (phoneNumber ?? "").Trim().ToLower();

                var query = context.Clients.Where(c => c.CompanyId == companyId && c.DeletedDate == null && c.DeletedBy == null
                                                    && c.FirstName == firstName
                                                    && (c.MiddleName ?? "") == middleName
                                                    && (c.LastName ?? "") == lastName);
                                                    

                if (phoneNumber != "" && dob != null)
                    query = query.Where(c => c.PhoneNumber == phoneNumber || c.DOB == dob);
                else if (phoneNumber != "")
                    query = query.Where(c => c.PhoneNumber == phoneNumber);
                else if (dob != null)
                    query = query.Where(c => c.DOB == dob);


                var efClient = query.OrderBy(c => c.Id)
                    .Select(c => new
                    {
                        Id = c.Id,
                        AddressId = c.AddressId,
                        GroupId = c.GroupId,
                        FirstName = c.FirstName,
                        MiddleName = c.MiddleName,
                        LastName = c.LastName,
                        EmailAddress = c.EmailAddress,
                        PhoneNumber = c.PhoneNumber,
                        DOB = c.DOB,
                        Gender = c.Gender,
                        MaritalStatus = c.MaritalStatus,
                        Occupation = c.Occupation,
                        IsActive = c.IsActive
                    }).FirstOrDefault();

                if (efClient != null)
                    mClient = JsonSerializer.Deserialize<Domain.Client>(JsonSerializer.Serialize(efClient));
            }
            return mClient;
        }
    }
}
 