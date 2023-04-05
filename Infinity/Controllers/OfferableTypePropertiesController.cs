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
using WhiteDream.Models;

namespace WhiteDream.Controllers
{
    public class OfferableTypePropertiesController : ApiController
    {
        private OfferableTypePropertiesContext db = new OfferableTypePropertiesContext();

        // GET api/OfferableTypeProperties
        public IEnumerable<OfferableTypeProperties> GetOfferableTypeProperties()
        {
            return db.OfferableTypeProperties.AsEnumerable();
        }
        public HttpResponseMessage GetOfferableTypePropertiesAsXml(int offerableTypeId)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(db.getOfferableTypePropertiesAsXml(offerableTypeId), Encoding.UTF8, "application/xml")
            };
        }

        // GET api/OfferableTypeProperties/5
        public OfferableTypeProperties GetOfferableTypeProperties(int id)
        {
            OfferableTypeProperties offerabletypeproperties = db.OfferableTypeProperties.Find(id);
            if (offerabletypeproperties == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return offerabletypeproperties;
        }

        // PUT api/OfferableTypeProperties/5
        public HttpResponseMessage PutOfferableTypeProperties(int id, OfferableTypeProperties offerabletypeproperties)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != offerabletypeproperties.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(offerabletypeproperties).State = EntityState.Modified;

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

        // POST api/OfferableTypeProperties
        public HttpResponseMessage PostOfferableTypeProperties(OfferableTypeProperties offerabletypeproperties)
        {
            if (ModelState.IsValid)
            {
                db.OfferableTypeProperties.Add(offerabletypeproperties);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, offerabletypeproperties);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = offerabletypeproperties.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/OfferableTypeProperties/5
        public HttpResponseMessage DeleteOfferableTypeProperties(int id)
        {
            OfferableTypeProperties offerabletypeproperties = db.OfferableTypeProperties.Find(id);
            if (offerabletypeproperties == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.OfferableTypeProperties.Remove(offerabletypeproperties);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, offerabletypeproperties);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}