using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class ReportRepository : IReportRepository
    {
        public Domain.PolicyLister GetAllLifeInsuranceList(Domain.PolicyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from p in context.Policies
                                join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                join c in context.Clients on p.ClientId equals c.Id
                                where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 1 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                select new
                                {
                                    PolicyNumber = p.PolicyNumber,
                                    Client = c.FirstName + " " + c.LastName,
                                    FUPDate = p.FUPDate,
                                    PremiumAmount = p.PremiumAmount,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.PolicyNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PolicyNumber) && x.PolicyNumber.ToUpper().Contains(criteria.PolicyNumber.ToUpper()));

                        if (criteria.FUPDate != default(DateTime))
                        {
                            var startFrom = criteria.FUPDate?.Date;
                            var startTo = startFrom?.AddDays(1).AddTicks(-1);

                            query = query.Where(x => x.FUPDate >= startFrom && x.FUPDate <= startTo);
                        }

                        if (criteria.Client.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Client) && x.Client.ToUpper().Contains(criteria.Client.ToUpper()));

                        if (criteria.PremiumAmount.HasValue)
                            query = query.Where(x => x.PremiumAmount.HasValue && x.PremiumAmount.Value == criteria.PremiumAmount.Value);

                    }

                    var efPolicies = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicies.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicies = efPolicies.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicies.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.PolicyLister GetAllHealthInsuranceList(Domain.PolicyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from p in context.Policies
                                join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                join c in context.Clients on p.ClientId equals c.Id
                                where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 2 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                select new
                                {
                                    PolicyNumber = p.PolicyNumber,
                                    Client = c.FirstName + " " + c.LastName,
                                    FUPDate = p.FUPDate,
                                    PremiumAmount = p.PremiumAmount,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.PolicyNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PolicyNumber) && x.PolicyNumber.ToUpper().Contains(criteria.PolicyNumber.ToUpper()));

                        if (criteria.FUPDate != default(DateTime))
                        {
                            var startFrom = criteria.FUPDate?.Date;
                            var startTo = startFrom?.AddDays(1).AddTicks(-1);

                            query = query.Where(x => x.FUPDate >= startFrom && x.FUPDate <= startTo);
                        }

                        if (criteria.Client.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Client) && x.Client.ToUpper().Contains(criteria.Client.ToUpper()));

                        if (criteria.PremiumAmount.HasValue)
                            query = query.Where(x => x.PremiumAmount.HasValue && x.PremiumAmount.Value == criteria.PremiumAmount.Value);
                    }

                    var efPolicies = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicies.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicies = efPolicies.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicies.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.PolicyLister GetAllPropertyInsuranceList(Domain.PolicyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from p in context.Policies
                                join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                join c in context.Clients on p.ClientId equals c.Id
                                where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 3 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                select new
                                {
                                    PolicyNumber = p.PolicyNumber,
                                    Client = c.FirstName + " " + c.LastName,
                                    FUPDate = p.FUPDate,
                                    PremiumAmount = p.PremiumAmount,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.PolicyNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PolicyNumber) && x.PolicyNumber.ToUpper().Contains(criteria.PolicyNumber.ToUpper()));

                        if (criteria.FUPDate != default(DateTime))
                        {
                            var startFrom = criteria.FUPDate?.Date;
                            var startTo = startFrom?.AddDays(1).AddTicks(-1);

                            query = query.Where(x => x.FUPDate >= startFrom && x.FUPDate <= startTo);
                        }

                        if (criteria.Client.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Client) && x.Client.ToUpper().Contains(criteria.Client.ToUpper()));

                        if (criteria.PremiumAmount.HasValue)
                            query = query.Where(x => x.PremiumAmount.HasValue && x.PremiumAmount.Value == criteria.PremiumAmount.Value);
                    }

                    var efPolicies = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicies.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicies = efPolicies.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicies.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public Domain.PolicyLister GetAllVehicleInsuranceList(Domain.PolicyLister mLister)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var query = from p in context.Policies
                                join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                join c in context.Clients on p.ClientId equals c.Id
                                where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 4 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                select new
                                {
                                    PolicyNumber = p.PolicyNumber,
                                    Client = c.FirstName + " " + c.LastName,
                                    FUPDate = p.FUPDate,
                                    PremiumAmount = p.PremiumAmount,
                                };

                    if (mLister.SearchCriteria != null)
                    {
                        var criteria = mLister.SearchCriteria;

                        if (criteria.PolicyNumber.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.PolicyNumber) && x.PolicyNumber.ToUpper().Contains(criteria.PolicyNumber.ToUpper()));

                        if (criteria.FUPDate != default(DateTime))
                        {
                            var startFrom = criteria.FUPDate?.Date;
                            var startTo = startFrom?.AddDays(1).AddTicks(-1);

                            query = query.Where(x => x.FUPDate >= startFrom && x.FUPDate <= startTo);
                        }

                        if (criteria.Client.IsNotNullOrEmpty())
                            query = query.Where(x => !string.IsNullOrEmpty(x.Client) && x.Client.ToUpper().Contains(criteria.Client.ToUpper()));

                        if (criteria.PremiumAmount.HasValue)
                            query = query.Where(x => x.PremiumAmount.HasValue && x.PremiumAmount.Value == criteria.PremiumAmount.Value);
                    }

                    var efPolicies = query.AsEnumerable();

                    var pagination = mLister.Pagination ?? new Domain.Pagination();

                    pagination.TotalRecord = Math.Max(efPolicies.Count(), 1);

                    if (pagination.Take <= 0)
                    {
                        pagination.Take = pagination.TotalRecord;
                        pagination.Skip = 0;
                    }

                    pagination.CurrentPage = pagination.Take > 0 ? (pagination.Skip / pagination.Take) + 1 : 1;

                    pagination.TotalPage = pagination.Take > 0 ? (int)Math.Ceiling((double)pagination.TotalRecord / pagination.Take) : 1;

                    efPolicies = efPolicies.Skip(pagination.Skip).Take(pagination.Take);

                    mLister.Pagination = pagination;

                    mLister.List = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicies.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mLister;
        }

        public List<Domain.Report> PremiumCollectionReport()
        {
            var reports = new List<Domain.Report>();

            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var today = DateTime.Today;

                    var efReports = (from p in context.Policies
                                     join c in context.Clients on p.ClientId equals c.Id
                                     join a in context.Addresses on c.AddressId equals a.Id
                                     where p.DeletedBy == null && p.DeletedDate == null && p.NeedToPremiumCollect && p.FUPDate < today
                                     group new { p, c } by a.Area into g
                                     from x in g
                                     orderby g.Key, x.c.FirstName, x.c.LastName, x.p.FUPDate
                                     select new
                                     {
                                         Area = g.Key,
                                         Client = x.c.FirstName + " " + x.c.LastName,
                                         PolicyNumber = x.p.PolicyNumber,
                                         PolicyType = x.p.PolicyType.Name,
                                         FUPDate = x.p.FUPDate
                                     }).ToList();

                    reports = JsonSerializer.Deserialize<List<Domain.Report>>(JsonSerializer.Serialize(efReports));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reports;
        }
    }
}
