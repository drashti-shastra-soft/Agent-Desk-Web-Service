using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IService;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System;

namespace AgentDeskWebService.Controllers
{
    [Authorize, RoutePrefix("api/PolicyType")]
    public class PolicyTypeController : ApiController
    {
        private readonly IPolicyTypeService _service;

        public PolicyTypeController(IPolicyTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetAll());
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
    }
}
