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
    public class PriceTypesController : ApiController
    {
        private PriceTypesContext db = new PriceTypesContext();

        // GET api/PriceTypes
        //public IEnumerable<PriceTypes> GetPriceTypes()
        //{
        //    return db.PriceTypes.AsEnumerable();
        //}
        public HttpResponseMessage GetPriceTypesAsXml()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(db.getPriceTypesAsXml(), Encoding.UTF8, "application/xml")
            };
        }

        // GET api/PriceTypes/5
        public PriceTypes GetPriceTypes(int id)
        {
            PriceTypes pricetypes = db.PriceTypes.Find(id);
            if (pricetypes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return pricetypes;
        }

        // PUT api/PriceTypes/5
        public HttpResponseMessage PutPriceTypes(int id, PriceTypes pricetypes)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != pricetypes.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(pricetypes).State = EntityState.Modified;

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

        // POST api/PriceTypes
        public HttpResponseMessage PostPriceTypes(PriceTypes pricetypes)
        {

            if (ModelState.IsValid)
            {
                db.PriceTypes.Add(pricetypes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pricetypes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = pricetypes.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PriceTypes/5
        public HttpResponseMessage DeletePriceTypes(int id)
        {
            PriceTypes pricetypes = db.PriceTypes.Find(id);
            if (pricetypes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.PriceTypes.Remove(pricetypes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, pricetypes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}