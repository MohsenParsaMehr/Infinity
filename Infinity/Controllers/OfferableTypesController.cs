using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WhiteDream.Classes;
using WhiteDream.Models;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class OfferableTypesController : ApiController
    {
        private OfferableTypesContext db = new OfferableTypesContext();
        private YJEntities entities = new YJEntities();

        // GET api/OfferableTypes
        [HttpPost]
        [ActionName("GetPartTypes")]
        public HttpResponseMessage GetOfferableTypes(Envelope<OfferableTypes> offerableTypeEnvelope)
        {
            HttpResponseMessage errorResponse;
            if (offerableTypeEnvelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((errorResponse = Security.analyzeRequest(offerableTypeEnvelope, Request, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkForbiddenPhrase: false, checkContactInfoEmbedding: false, checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                db.Configuration.ProxyCreationEnabled = false;

                return errorResponse= Request.CreateResponse(HttpStatusCode.OK, entities.OfferableTypes.Where(ot=>ot.CategoryRef==offerableTypeEnvelope.Entity.Id &&
                    ot.Status==Constants.RecordStatus.ACTIVE).Select
                    (y => new { y.Id, y.Title }).OrderBy(y=>y.Title).ToList(), "application/json");
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
        public HttpResponseMessage GetOfferableTypesAsXml(int categoryId)
        {
            return new HttpResponseMessage() { 
                Content = new StringContent(db.getOfferableTypesAsXml(categoryId), Encoding.UTF8, "application/xml")
            };
        }

        // GET api/OfferableTypes/5
        public OfferableTypes GetOfferableTypes(int id)
        {
            OfferableTypes offerabletypes = db.OfferableTypes.Find(id);
            if (offerabletypes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return offerabletypes;
        }

        // PUT api/OfferableTypes/5
        public HttpResponseMessage PutOfferableTypes(int id, OfferableTypes offerabletypes)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != offerabletypes.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(offerabletypes).State = EntityState.Modified;

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

        // POST api/OfferableTypes
        public HttpResponseMessage PostOfferableTypes(OfferableTypes offerabletypes)
        {
            if (ModelState.IsValid)
            {
                db.OfferableTypes.Add(offerabletypes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, offerabletypes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = offerabletypes.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/OfferableTypes/5
        public HttpResponseMessage DeleteOfferableTypes(int id)
        {
            OfferableTypes offerabletypes = db.OfferableTypes.Find(id);
            if (offerabletypes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.OfferableTypes.Remove(offerabletypes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, offerabletypes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}