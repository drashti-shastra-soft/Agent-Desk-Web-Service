using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class PolicyTypeRepository : IPolicyTypeRepository
    {
        public List<Domain.PolicyType> GetAll()
        {
            var mPolicyType = new List<Domain.PolicyType>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyType = (from pt in context.PolicyTypes
                                        where pt.IsActive
                                        select new
                                        {
                                            Id = pt.Id,
                                            Name = pt.Name,
                                        }).AsEnumerable();

                    mPolicyType = JsonSerializer.Deserialize<List<Domain.PolicyType>>(JsonSerializer.Serialize(efPolicyType.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyType;
        }
    }
}
