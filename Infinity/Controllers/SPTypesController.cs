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
    public class SPTypesController : ApiController
    {
        private SPTypesContext db = new SPTypesContext();

        // GET api/SPTypes
        public IEnumerable<SPTypes> GetSPTypes()
        {
            return db.SPTypes.AsEnumerable();
        }
        public HttpResponseMessage GetSPTypesAsXml(int categoryId)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(db.getSPTypesAsXml(categoryId), Encoding.UTF8, "application/xml")
            };
        }
        // GET api/SPTypes/5
        public SPTypes GetSPTypes(int id)
        {
            SPTypes sptypes = db.SPTypes.Find(id);
            if (sptypes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return sptypes;
        }

        // PUT api/SPTypes/5
        public HttpResponseMessage PutSPTypes(int id, SPTypes sptypes)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != sptypes.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(sptypes).State = EntityState.Modified;

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

        // POST api/SPTypes
        public HttpResponseMessage PostSPTypes(SPTypes sptypes)
        {
            if (ModelState.IsValid)
            {
                db.SPTypes.Add(sptypes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, sptypes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = sptypes.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SPTypes/5
        public HttpResponseMessage DeleteSPTypes(int id)
        {
            SPTypes sptypes = db.SPTypes.Find(id);
            if (sptypes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SPTypes.Remove(sptypes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, sptypes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}