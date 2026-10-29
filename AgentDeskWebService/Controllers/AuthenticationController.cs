using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IService;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System;

namespace AgentDeskWebService.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpPost]
        public HttpResponseMessage Post(Domain.Credential mCredential)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _service.Authenticate(mCredential));
            }
            catch (APIRequestFailedException ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
