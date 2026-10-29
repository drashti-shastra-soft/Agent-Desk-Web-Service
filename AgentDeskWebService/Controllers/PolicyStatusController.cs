using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IService;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System;

namespace AgentDeskWebService.Controllers
{
    [Authorize, RoutePrefix("api/PolicyStatus")]
    public class PolicyStatusController : ApiController
    {
        private readonly IPolicyStatusService _service;

        public PolicyStatusController(IPolicyStatusService service)
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

        [HttpGet, Route("GetActiveBy/PolicyTypes/{policyTypes}")]

        public HttpResponseMessage GetActiveBy(string policyTypes)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.GetActiveBy(policyTypes));
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
