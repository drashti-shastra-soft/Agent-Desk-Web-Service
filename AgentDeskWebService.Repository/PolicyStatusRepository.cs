using AgentDeskWebService.Infrastructure.IRepository;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System;

namespace AgentDeskWebService.Repository
{
    public class PolicyStatusRepository : IPolicyStatusRepository
    {
        public List<Domain.PolicyStatus> GetAll()
        {
            var mPolicyStatus = new List<Domain.PolicyStatus>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyStatus = (from ps in context.PolicyStatus
                                          join pt in context.PolicyTypes on ps.PolicyTypeId equals pt.Id
                                          where ps.IsActive
                                          select new
                                          {
                                              Id = ps.Id,
                                              Name = ps.Name + " (" + pt.Name + ")",
                                          }).AsEnumerable();

                    mPolicyStatus = JsonSerializer.Deserialize<List<Domain.PolicyStatus>>(JsonSerializer.Serialize(efPolicyStatus.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyStatus;
        }


        public List<Domain.PolicyStatus> GetActiveBy(string policyType)
        {
            var mPolicyStatus = new List<Domain.PolicyStatus>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    int policyTypeId = Convert.ToInt32(policyType);

                    var efPolicyType = (from ps in context.PolicyStatus
                                        join pt in context.PolicyTypes on ps.PolicyTypeId equals pt.Id
                                        where ps.IsActive && ps.PolicyTypeId == policyTypeId
                                        select new
                                        {
                                            Id = ps.Id,
                                            PolicyTypeId = ps.PolicyTypeId,
                                            Name = ps.Name,
                                            Description = ps.Description,
                                            IsActive = ps.IsActive
                                        }).ToList();

                    mPolicyStatus = JsonSerializer.Deserialize<List<Domain.PolicyStatus>>(JsonSerializer.Serialize(efPolicyType));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyStatus;
        }
    }
}
