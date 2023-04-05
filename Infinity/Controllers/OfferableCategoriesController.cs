using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Infinity.Classes;
using Infinity.Utilities;

namespace Infinity.Controllers
{
    public class OfferableCategoriesController : ApiController
    {
        private InfinityEntities entities = new InfinityEntities();
        [HttpPost]
        [ActionName("GetCategories")]
        public HttpResponseMessage GetOfferableCategories(Envelope<OfferableCategories> envelope)
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
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, entities.OfferableType.Where(c =>
                    c.ParentRef == envelope.Entity.Id && c.Status == Constants.RecordStatus.ACTIVE)
                    .OrderBy(c => c.Priority).Select(c => new SimpleIconicListItem { Id = c.Id, Title = c.Title, imageUrl = c.PhotoUrl }).ToList(), "application/json");

                //return Request.CreateResponse(HttpStatusCode.OK, entities.usp_GetOfferableCategories(null,
                //Constants.PARTS_CATEGORY_TITLE_IN_DB).Where(r => r.ParentRef == envelope.Entity.Id).Select
                //    (p => new { p.Id, p.Title }).OrderBy(p => p.Title).ToList(), "application/json");
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

        [HttpPost]
        [ActionName("GetCategoriesRoot")]
        public HttpResponseMessage GetOfferableCategories(EmptyEnvelope envelope)
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
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, entities.OfferableType.Where(c => c.ParentRef == null && c.Status == Constants.RecordStatus.ACTIVE)
                    .OrderBy(c => c.Priority).Select(c => new SimpleIconicListItem { Id = c.Id, Title = c.Title, imageUrl = c.PhotoUrl }).ToList(), "application/json");

                //return Request.CreateResponse(HttpStatusCode.OK, entities.usp_GetOfferableCategories(null,
                //Constants.PARTS_CATEGORY_TITLE_IN_DB).Where(r => r.ParentRef == envelope.Entity.Id).Select
                //    (p => new { p.Id, p.Title }).OrderBy(p => p.Title).ToList(), "application/json");
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

        public string getOfferableCategoriesAsXml(string type)
        {
            throw new NotImplementedException();
            //InfinityEntities entities = new InfinityEntities();
            //var result = entities.usp_GetOfferableCategoriesAsXml(null,type).GetEnumerator();
            //result.MoveNext();
            //return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;

        }
        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
        }
    }
}
