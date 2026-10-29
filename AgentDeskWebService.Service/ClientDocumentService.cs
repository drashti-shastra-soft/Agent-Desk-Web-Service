using AgentDeskWebService.Infrastructure.IRepository;
using AgentDeskWebService.Infrastructure.IService;
using AgentDeskWebService.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace AgentDeskWebService.Service
{
    public class ClientDocumentService : IClientDocumentService
    {
        private readonly IClientDocumentRepository _repo;

        public ClientDocumentService(IClientDocumentRepository repo)
        {
            _repo = repo;
        }

        public Domain.ClientDocument Upsert(Domain.ClientDocument mClientDocument)
        {
            if (mClientDocument == null)
                throw new Infrastructure.CustomException.APIRequestFailedException("Object must have a value!");

            if (mClientDocument.Base64String.IsNullOrEmpty())
                throw new Infrastructure.CustomException.APIRequestFailedException("Document is required!");

            string relativeFolder = "~/Document/";
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(mClientDocument.FilePath);
            mClientDocument.FilePath = uniqueFileName;
            StaticMethods.SaveBase64Image(mClientDocument.Base64String, relativeFolder, uniqueFileName);

            return _repo.Upsert(mClientDocument);
        }

        public Domain.ClientDocument Get(int id)
        {
            return _repo.Get(id);
        }

        public List<Domain.ClientDocument> Fetch(int clientId)
        {
            var mDocuments = _repo.Fetch(clientId);

            if (mDocuments != null && mDocuments.Count > 0)
            {
                foreach (var item in mDocuments)
                {
                    item.Base64String = StaticMethods.GetImageBase64(HostingEnvironment.MapPath($"~/Document/{item.FilePath}"));
                }
            }

            return mDocuments;
        }

        public void Delete(int id, int loginUserId)
        {
            _repo.Delete(id, loginUserId);
        }

    }

}
