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
    public class PartConditionsController : ApiController
    {
        private PartConditionsContext db = new PartConditionsContext();

        // GET api/PartConditions
        //public IEnumerable<PartConditions> GetPartConditions()
        //{
        //    return db.PartConditions.AsEnumerable();
        //}
        public HttpResponseMessage GetPartConditionsAsXml()
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(db.getPartConditionsAsXml(), Encoding.UTF8, "application/xml")
            };
        }

        // GET api/PartConditions/5
        public PartConditions GetPartConditions(int id)
        {
            PartConditions partconditions = db.PartConditions.Find(id);
            if (partconditions == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return partconditions;
        }

        // PUT api/PartConditions/5
        public HttpResponseMessage PutPartConditions(int id, PartConditions partconditions)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != partconditions.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(partconditions).State = EntityState.Modified;

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

        // POST api/PartConditions
        public HttpResponseMessage PostPartConditions(PartConditions partconditions)
        {
            if (ModelState.IsValid)
            {
                db.PartConditions.Add(partconditions);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, partconditions);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = partconditions.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PartConditions/5
        public HttpResponseMessage DeletePartConditions(int id)
        {
            PartConditions partconditions = db.PartConditions.Find(id);
            if (partconditions == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.PartConditions.Remove(partconditions);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, partconditions);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}