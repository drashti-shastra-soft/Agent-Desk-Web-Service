using AgentDeskWebService.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgentDeskWebService.Repository
{
    public class PolicyDocumentRepository : IPolicyDocumentRepository
    {
        public Domain.PolicyDocument Upsert(Domain.PolicyDocument mPolicyDocument)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efClientDocument = context.PolicyDocuments.FirstOrDefault(x => x.Id == mPolicyDocument.Id);

                    if (efClientDocument == null || efClientDocument.Id <= default(int))
                    {
                        efClientDocument = new Persistence.PolicyDocument();
                        efClientDocument.CreatedBy = mPolicyDocument.Id;
                        efClientDocument.CreatedDate = DateTime.Now;
                        context.PolicyDocuments.Add(efClientDocument);
                    }
                    else
                    {
                        efClientDocument.LastModifiedBy = mPolicyDocument.LastModifiedBy;
                        efClientDocument.LastModifiedDate = DateTime.Now;
                    }


                    efClientDocument.PolicyId = mPolicyDocument.PolicyId;
                    efClientDocument.FilePath = mPolicyDocument.FilePath;
                    efClientDocument.MIME = mPolicyDocument.MIME;
                    efClientDocument.Base64String = mPolicyDocument.Base64String;

                    context.SaveChanges();
                    mPolicyDocument.Id = efClientDocument.Id;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mPolicyDocument;
        }

        public Domain.PolicyDocument Get(int id)
        {
            var mPolicyDocument = new Domain.PolicyDocument();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyDocument = (from pd in context.PolicyDocuments
                                            where pd.DeletedDate == null && pd.DeletedBy == null && pd.Id == id
                                            select new
                                            {
                                                Id = pd.Id,
                                                PolicyId = pd.PolicyId,
                                                FilePath = pd.FilePath,
                                                MIME = pd.MIME,
                                                Base64String = pd.Base64String,
                                            }).FirstOrDefault();

                    if (efPolicyDocument != null)
                    {
                        mPolicyDocument = JsonSerializer.Deserialize<Domain.PolicyDocument>(JsonSerializer.Serialize(efPolicyDocument));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return mPolicyDocument;
        }

        public void Delete(int id, int loginUserId)
        {
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyDocuments = context.PolicyDocuments.Where(x => x.Id == id).FirstOrDefault();
                    if (efPolicyDocuments != null && efPolicyDocuments.Id > default(int))
                    {
                        efPolicyDocuments.DeletedBy = loginUserId;
                        efPolicyDocuments.DeletedDate = DateTime.Now;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Domain.PolicyDocument> Fetch(int policyId)
        {
            var mPolicyDocuments = new List<Domain.PolicyDocument>();
            try
            {
                using (var context = new Persistence.AgentDeskDBEntities())
                {
                    var efPolicyDocument = (from pd in context.PolicyDocuments
                                            where pd.DeletedBy == null && pd.DeletedDate == null
                                            where pd.PolicyId == policyId
                                            select new
                                            {
                                                Id = pd.Id,
                                                PolicyId = pd.PolicyId,
                                                FilePath = pd.FilePath,
                                                MIME = pd.MIME,
                                                Base64String = pd.Base64String
                                            }).AsEnumerable();


                    mPolicyDocuments = JsonSerializer.Deserialize<List<Domain.PolicyDocument>>(JsonSerializer.Serialize(efPolicyDocument.ToList()));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return mPolicyDocuments;
        }
    }
}
