using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class ClientHealthRepository : IClientHealthRepository
    {
        public Domain.ClientHealth Upsert(Domain.ClientHealth mClientHealth)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientHealth = context.ClientHealths.FirstOrDefault(x => x.Id == mClientHealth.Id);

                    if (efClientHealth == null || efClientHealth.Id <= default(int))
                    {
                        efClientHealth = new Persistence.ClientHealth();
                        efClientHealth.CreatedBy = mClientHealth.Id;
                        efClientHealth.CreatedDate = DateTime.Now;
                        context.ClientHealths.Add(efClientHealth);
                    }
                    else
                    {
                        efClientHealth.LastModifiedBy = mClientHealth.LastModifiedBy;
                        efClientHealth.LastModifiedDate = DateTime.Now;
                    }

                    efClientHealth.ClientId = mClientHealth.ClientId;
                    efClientHealth.MedicalHistory = mClientHealth.MedicalHistory;
                    efClientHealth.CurrentMedications = mClientHealth.CurrentMedications;
                    efClientHealth.SmokingAlcoholHabits = mClientHealth.SmokingAlcoholHabits;
                    efClientHealth.FamilyMedicalHistory = mClientHealth.FamilyMedicalHistory;

                    context.SaveChanges();
                    mClientHealth.Id = efClientHealth.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientHealth;
        }

        public Domain.ClientHealth Get(int id)
        {
            var mClientHealth = new Domain.ClientHealth();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientIdentification = (from ch in context.ClientHealths
                                                  where ch.DeletedDate == null && ch.DeletedBy == null && ch.ClientId == id
                                                  select new
                                                  {
                                                      Id = ch.Id,
                                                      ClientId = ch.ClientId,
                                                      MedicalHistory = ch.MedicalHistory,
                                                      CurrentMedications = ch.CurrentMedications,
                                                      SmokingAlcoholHabits = ch.SmokingAlcoholHabits,
                                                      FamilyMedicalHistory = ch.FamilyMedicalHistory,
                                                  }).FirstOrDefault();

                    if (efClientIdentification != null)
                    {
                        mClientHealth = JsonSerializer.Deserialize<Domain.ClientHealth>(JsonSerializer.Serialize(efClientIdentification));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientHealth;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientHealth = context.ClientHealths.Where(x => x.Id == id).FirstOrDefault();
                    if (efClientHealth != null && efClientHealth.Id > default(int))
                    {
                        efClientHealth.DeletedBy = loginUserId;
                        efClientHealth.DeletedDate = DateTime.Now;
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
