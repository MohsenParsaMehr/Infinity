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
    public class SPActivityFieldsController : ApiController
    {
        private SPActivityFieldsContext db = new SPActivityFieldsContext();
        private YJEntities entities = new YJEntities();

        // GET api/SPActivityFields
        public IEnumerable<SPActivityFields> GetSPActivityFields()
        {
            return db.SPActivityFields.AsEnumerable();
        }

        // GET api/SPActivityFields/5
        public SPActivityFields GetSPActivityFields(int id)
        {
            SPActivityFields spactivityfields = db.SPActivityFields.Find(id);
            if (spactivityfields == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return spactivityfields;
        }

        // PUT api/SPActivityFields/5
        public HttpResponseMessage PutSPActivityFields(int id, SPActivityFields spactivityfields)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != spactivityfields.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(spactivityfields).State = EntityState.Modified;

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

        // POST api/SPActivityFields
        public HttpResponseMessage PostSPActivityFields(List<SPActivityFields> spactivityfields)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;
                try
                {                    
                    //if (spactivityfields.CarBrandRef == 0)
                    //    spactivityfields.CarBrandRef = null;
                    //if (spactivityfields.OfferableCategoryRef == 0)
                    //    spactivityfields.OfferableCategoryRef = null;
                    //if (spactivityfields.OfferableTypeRef == 0)
                    //    spactivityfields.OfferableTypeRef = null;                   
                    foreach (var item in spactivityfields)
                    {
                        entities.SPActivityFields.Add(item);
                    }
                    
                    entities.SaveChanges();
                    
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("true", Encoding.UTF8, "text/plain")
                    };
                }
                catch (EntityException ee)
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
                    return response;
                }
                catch (Exception e)
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                    return response;
                    //throw e;
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SPActivityFields/5
        public HttpResponseMessage DeleteSPActivityFields(int id)
        {
            SPActivityFields spactivityfields = db.SPActivityFields.Find(id);
            if (spactivityfields == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SPActivityFields.Remove(spactivityfields);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, spactivityfields);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}