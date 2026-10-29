using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        public List<Domain.Policy> GetAllLifeInsurance()
        {
            var mPolicy = new List<Domain.Policy>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = (from p in context.Policies
                                    join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                    join c in context.Clients on p.ClientId equals c.Id
                                    where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 1 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                    orderby p.FUPDate ascending
                                    select new
                                    {
                                        Client = c.FirstName + " " + c.LastName,
                                        FUPDate = p.FUPDate,
                                        PremiumAmount = p.PremiumAmount,
                                        PayMode = p.PayMode,
                                    }).AsEnumerable();


                    mPolicy = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicy.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public List<Domain.Policy> GetAllHealthInsurance()
        {
            var mPolicy = new List<Domain.Policy>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = (from p in context.Policies
                                    join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                    join c in context.Clients on p.ClientId equals c.Id
                                    where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 2 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                    orderby p.FUPDate ascending
                                    select new
                                    {
                                        Client = c.FirstName + " " + c.LastName,
                                        FUPDate = p.FUPDate,
                                        PremiumAmount = p.PremiumAmount,
                                        PayMode = p.PayMode,
                                    }).AsEnumerable();


                    mPolicy = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicy.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public List<Domain.Policy> GetAllPropertyInsurance()
        {
            var mPolicy = new List<Domain.Policy>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = (from p in context.Policies
                                    join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                    join c in context.Clients on p.ClientId equals c.Id
                                    where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 3 && p.FUPDate.HasValue && p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                    orderby p.FUPDate ascending
                                    select new
                                    {
                                        Client = c.FirstName + " " + c.LastName,
                                        FUPDate = p.FUPDate,
                                        PremiumAmount = p.PremiumAmount,
                                        PayMode = p.PayMode,
                                    }).AsEnumerable();


                    mPolicy = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicy.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public List<Domain.Policy> GetAllVehicleInsurance()
        {
            var mPolicy = new List<Domain.Policy>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicy = (from p in context.Policies
                                    join pt in context.PolicyTypes on p.PolicyTypeId equals pt.Id
                                    join c in context.Clients on p.ClientId equals c.Id
                                    where p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 4 && p.FUPDate.HasValue &&                   p.FUPDate.Value.Month == DateTime.Now.Month && p.FUPDate.Value.Year == DateTime.Now.Year
                                    orderby p.FUPDate ascending
                                    select new
                                    {
                                        Client = c.FirstName + " " + c.LastName,
                                        FUPDate = p.FUPDate,
                                        PremiumAmount = p.PremiumAmount,
                                        PayMode = p.PayMode,
                                    }).AsEnumerable();


                    mPolicy = JsonSerializer.Deserialize<List<Domain.Policy>>(JsonSerializer.Serialize(efPolicy.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicy;
        }

        public int GetLifeInsuranceCount()
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    return context.Policies.Where(p => p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 1).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetHealthInsuranceCount()
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    return context.Policies.Where(p => p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 2).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetPropertyInsuranceCount()
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    return context.Policies.Where(p => p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 3).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetVehicleInsuranceCount()
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    return context.Policies.Where(p => p.DeletedBy == null && p.DeletedDate == null && p.PolicyTypeId == 4).Count();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}                         
