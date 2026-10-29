using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AgentDeskWebService.Repository
{
    public class ClientDocumentRepository : IClientDocumentRepository
    {
        public Domain.ClientDocument Upsert(Domain.ClientDocument mClientDocument)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientDocument = context.ClientDocuments.FirstOrDefault(x => x.Id == mClientDocument.Id);

                    if (efClientDocument == null || efClientDocument.Id <= default(int))
                    {
                        efClientDocument = new Persistence.ClientDocument
                        {
                            CreatedBy = mClientDocument.CreatedBy,
                            CreatedDate = DateTime.Now,
                        };
                        context.ClientDocuments.Add(efClientDocument);
                    }
                    else
                    {
                        efClientDocument.LastModifiedBy = mClientDocument.LastModifiedBy;
                        efClientDocument.LastModifiedDate = DateTime.Now;
                    }


                    efClientDocument.ClientId = mClientDocument.ClientId;
                    efClientDocument.FilePath = mClientDocument.FilePath;
                    efClientDocument.MIME = mClientDocument.MIME;
                    efClientDocument.Base64String = mClientDocument.Base64String;

                    context.SaveChanges();
                    mClientDocument.Id = efClientDocument.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientDocument;
        }

        public Domain.ClientDocument Get(int id)
        {
            var mClientDocument = new Domain.ClientDocument();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientDocument = (from cd in context.ClientDocuments
                                            where cd.DeletedDate == null && cd.DeletedBy == null && cd.ClientId == id
                                            select new
                                            {
                                                Id = cd.Id,
                                                ClientId = cd.ClientId,
                                                FilePath = cd.FilePath,
                                                MIME = cd.MIME,
                                                Base64String = cd.Base64String,
                                            }).FirstOrDefault();

                    if (efClientDocument != null)
                    {
                        mClientDocument = JsonSerializer.Deserialize<Domain.ClientDocument>(JsonSerializer.Serialize(efClientDocument));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mClientDocument;
        }

        public List<Domain.ClientDocument> Fetch(int clientId)
        {
            var mClientDocuments = new List<Domain.ClientDocument>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientDocument = (from cd in context.ClientDocuments
                                            where cd.DeletedBy == null && cd.DeletedDate == null
                                            where cd.ClientId == clientId
                                            select new
                                            {
                                                Id = cd.Id,
                                                ClientId = cd.ClientId,
                                                FilePath = cd.FilePath,
                                                MIME = cd.MIME,
                                                Base64String = cd.Base64String
                                            }).AsEnumerable();


                    mClientDocuments = JsonSerializer.Deserialize<List<Domain.ClientDocument>>(JsonSerializer.Serialize(efClientDocument.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mClientDocuments;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientDocuments = context.ClientDocuments.Where(x => x.Id == id).FirstOrDefault();
                    if (efClientDocuments != null && efClientDocuments.Id > default(int))
                    {
                        efClientDocuments.DeletedBy = loginUserId;
                        efClientDocuments.DeletedDate = DateTime.Now;
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
