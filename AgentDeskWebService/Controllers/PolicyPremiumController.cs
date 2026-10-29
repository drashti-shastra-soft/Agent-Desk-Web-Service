using AgentDeskWebService.Infrastructure.CustomException;
using AgentDeskWebService.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AgentDeskWebService.Controllers
{
    [Authorize, RoutePrefix("api/PolicyPremium")]
    public class PolicyPremiumController : ApiController
    {
        private readonly IPolicyPremiumService _service;

        public PolicyPremiumController(IPolicyPremiumService service)
        {
            _service = service;
        }

        [HttpPost]
        public HttpResponseMessage Upsert(Domain.PolicyPremium mPolicyPremium)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, _service.Upsert(mPolicyPremium));
            }
            catch (APIRequestFailedException ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Unexpected Error: " + ex.Message);
            }
            return response;
        }


        //[HttpPost]
        //public HttpResponseMessage Upsert(List<Domain.PolicyPremium> mPolicyPremium)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    try
        //    {
        //        _service.Upsert(mPolicyPremium);
        //        response = Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (APIRequestFailedException ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Unexpected Error: " + ex.Message);
        //    }
        //    return response;
        //}

    }
}
