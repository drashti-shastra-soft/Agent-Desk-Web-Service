using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public Domain.Address Upsert(Domain.Address mAddress)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAddress = context.Addresses.FirstOrDefault(x => x.Id == mAddress.Id || x.ClientId == mAddress.ClientId);
                    if (efAddress == null || efAddress.Id <= default(int))
                    {
                        efAddress = new Persistence.Address();
                        efAddress.CreatedBy = mAddress.CreatedBy;
                        efAddress.CreatedDate = DateTime.Now;
                        context.Addresses.Add(efAddress);
                    }
                    else
                    {
                        efAddress.LastModifiedBy = mAddress.LastModifiedBy;
                        efAddress.LastModifiedDate = DateTime.Now;
                    }

                    efAddress.CompanyId = mAddress.CompanyId;
                    efAddress.Line1 = mAddress.Line1;
                    efAddress.Line2 = mAddress.Line2;
                    efAddress.Area = mAddress.Area;
                    efAddress.City = mAddress.City;
                    efAddress.State = mAddress.State;
                    efAddress.ZipCode = mAddress.ZipCode;
                    efAddress.ClientId = mAddress.ClientId;

                    context.SaveChanges();
                    mAddress.Id = efAddress.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public Domain.Address Get(int id)
        {
            var mAddress = new Domain.Address();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAddress = (from c in context.Addresses
                                     where c.Id == id
                                     select new
                                     {
                                         Id = c.Id,
                                         CompanyId = c.CompanyId,
                                         Line1 = c.Line1,
                                         Line2 = c.Line2,
                                         Area = c.Area,
                                         City = c.City,
                                         State = c.State,
                                         ZipCode = c.ZipCode,
                                         ClientId = c.ClientId
                                     }).FirstOrDefault();

                    if (efAddress != null)
                    {
                        mAddress = JsonSerializer.Deserialize<Domain.Address>(JsonSerializer.Serialize(efAddress));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }

        public Domain.Address GetByClientId(int clientId)
        {
            var mAddress = new Domain.Address();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efAddress = context.Addresses.Where(x => x.ClientId == clientId && x.ClientId > 0)
                                           .Select(a => new
                                           {
                                               Id = a.Id,
                                               CompanyId = a.CompanyId,
                                               Line1 = a.Line1,
                                               Line2 = a.Line2,
                                               Area = a.Area,
                                               City = a.City,
                                               State = a.State,
                                               ZipCode = a.ZipCode
                                           }).FirstOrDefault();

                    if (efAddress != null)
                    {
                        mAddress = JsonSerializer.Deserialize<Domain.Address>(JsonSerializer.Serialize(efAddress));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mAddress;
        }
        public Domain.Address GetByAddress(Domain.Address mAddress)
        {
            using (var context = new Persistence.AgentDeskDBEntities())
            {
                var line1 = (mAddress.Line1 ?? "").Trim();
                var line2 = (mAddress.Line2 ?? "").Trim();
                var area = (mAddress.Area ?? "").Trim();
                var city = (mAddress.City ?? "").Trim();
                var zip = (mAddress.ZipCode ?? "").Trim();

                var ef = context.Addresses
                    .Where(x => x.Line1 == line1
                             && (x.Line2 ?? "") == line2
                             && (x.Area ?? "") == area
                             && (x.City ?? "") == city
                             && (x.ZipCode ?? "") == zip)
                    .OrderBy(x => x.Id)
                    .FirstOrDefault();

                if (ef == null) return null;

                return new Domain.Address
                {
                    Id = ef.Id,
                    CompanyId = ef.CompanyId,
                    Line1 = ef.Line1,
                    Line2 = ef.Line2,
                    Area = ef.Area,
                    City = ef.City,
                    State = ef.State,
                    ZipCode = ef.ZipCode,
                };
            }
        }
    }
}
