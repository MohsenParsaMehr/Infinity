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
    public class DeliveryMethodController : ApiController
    {
        private DeliveryMethodContext db = new DeliveryMethodContext();

        [HttpPost]
        [ActionName("GetDeliveryMethods")]
        public HttpResponseMessage GetDeliveryMethods(EmptyEnvelope envelope)
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
                    checkCredentials: false,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }

                return Request.CreateResponse(HttpStatusCode.OK, db.getDeliveryMethods(envelope).ToList(), "application/json");
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
        // GET api/DeliveryMethod
        public IEnumerable<DeliveryMethod> GetDeliveryMethods()
        {
            return db.DeliveryMethods.AsEnumerable();
        }

        // GET api/DeliveryMethod/5
        public DeliveryMethod GetDeliveryMethod(int id)
        {
            DeliveryMethod deliverymethod = db.DeliveryMethods.Find(id);
            if (deliverymethod == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return deliverymethod;
        }

        // PUT api/DeliveryMethod/5
        public HttpResponseMessage PutDeliveryMethod(int id, DeliveryMethod deliverymethod)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != deliverymethod.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(deliverymethod).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/DeliveryMethod
        public HttpResponseMessage PostDeliveryMethod(DeliveryMethod deliverymethod)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryMethods.Add(deliverymethod);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, deliverymethod);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = deliverymethod.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/DeliveryMethod/5
        public HttpResponseMessage DeleteDeliveryMethod(int id)
        {
            DeliveryMethod deliverymethod = db.DeliveryMethods.Find(id);
            if (deliverymethod == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.DeliveryMethods.Remove(deliverymethod);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, deliverymethod);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}