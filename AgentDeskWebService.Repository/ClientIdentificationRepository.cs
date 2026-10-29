using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class ClientIdentificationRepository : IClientIdentificationRepository
    {
        public Domain.ClientIdentification Upsert(Domain.ClientIdentification mClientIdentification)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientIdentification = context.ClientIdentifications.FirstOrDefault(x => x.Id == mClientIdentification.Id);

                    if (efClientIdentification == null || efClientIdentification.Id <= default(int))
                    {
                        efClientIdentification = new Persistence.ClientIdentification();
                        efClientIdentification.CreatedBy = mClientIdentification.Id;
                        efClientIdentification.CreatedDate = DateTime.Now;
                        context.ClientIdentifications.Add(efClientIdentification);
                    }
                    else
                    {
                        efClientIdentification.LastModifiedBy = mClientIdentification.LastModifiedBy;
                        efClientIdentification.LastModifiedDate = DateTime.Now;
                    }


                    efClientIdentification.ClientId = mClientIdentification.ClientId;
                    efClientIdentification.PANNumber = mClientIdentification.PANNumber;
                    efClientIdentification.AadhaarNumber = mClientIdentification.AadhaarNumber;
                    efClientIdentification.NomineeName = mClientIdentification.NomineeName;
                    efClientIdentification.NomineeRelationship = mClientIdentification.NomineeRelationship;
                    efClientIdentification.NomineeContact = mClientIdentification.NomineeContact;

                    context.SaveChanges();
                    mClientIdentification.Id = efClientIdentification.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientIdentification;
        }

        public Domain.ClientIdentification Get(int id)
        {
            var mClientIdentification = new Domain.ClientIdentification();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientIdentification = (from ci in context.ClientIdentifications
                                                  where ci.DeletedDate == null && ci.DeletedBy == null && ci.ClientId == id
                                                  select new
                                                  {
                                                      Id = ci.Id,
                                                      ClientId = ci.ClientId,
                                                      PANNumber = ci.PANNumber,
                                                      AadhaarNumber = ci.AadhaarNumber,
                                                      NomineeName = ci.NomineeName,
                                                      NomineeRelationship = ci.NomineeRelationship,
                                                      NomineeContact = ci.NomineeContact,
                                                  }).FirstOrDefault();

                    if (efClientIdentification != null)
                    {
                        mClientIdentification = JsonSerializer.Deserialize<Domain.ClientIdentification>(JsonSerializer.Serialize(efClientIdentification));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientIdentification;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientIdentification = context.ClientIdentifications.Where(x => x.Id == id).FirstOrDefault();
                    if (efClientIdentification != null && efClientIdentification.Id > default(int))
                    {
                        efClientIdentification.DeletedBy = loginUserId;
                        efClientIdentification.DeletedDate = DateTime.Now;
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
