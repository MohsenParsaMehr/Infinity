using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Infinity.Models;
using Infinity.Classes;
using Infinity.Utilities;

namespace Infinity.Controllers
{
    public class PaymentController : ApiController
    {
        private PaymentContext db = new PaymentContext();
        private InfinityEntities entities = new InfinityEntities();
        [HttpPost]
        [ActionName("GetPaymentMethods")]
        public HttpResponseMessage GetPaymentMethods(EmptyEnvelope envelope)
        {
            HttpResponseMessage errorResponse;
            string securityCheckResult;
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((securityCheckResult = Security.analyzeRequest(envelope, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }
                
                return Request.CreateResponse(HttpStatusCode.OK,db.getPaymentMethods(envelope).ToList() , "application/json");
            }
            catch (EntityException ee)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
      

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}