using Infinity.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace Infinity.Classes
{
    public class Helpers
    {
        public static HttpResponseMessage handleRequest(Envelope<Entity> requestEnvelope,
            Func<Envelope<Entity>,IEnumerable<Object>> dataContextMethod, HttpRequestMessage Request, bool isList = false)
        {
            HttpResponseMessage errorResponse;
            string securityCheckResult = "";
            if (requestEnvelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if (!string.IsNullOrEmpty(securityCheckResult = Security.analyzeRequest(requestEnvelope, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)))
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }                
                
                return Request.CreateResponse(HttpStatusCode.OK,(isList? dataContextMethod(requestEnvelope).ToList():dataContextMethod(requestEnvelope)), "application/json");
            }
            catch (EntityException ee)
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
                return errorResponse;
            }
            catch (Exception e)
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return errorResponse;
                //throw e;
            }
        }
    }
}