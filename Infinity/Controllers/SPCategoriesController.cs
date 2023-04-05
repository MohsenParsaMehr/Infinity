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
    public class SPCategoriesController : ApiController
    {
        private SPCategoriesContext db = new SPCategoriesContext();
        private YJEntities entities = new YJEntities();

        // GET api/SPCategories
        public HttpResponseMessage GetSPCategories(string type)
        {
            var result = entities.usp_GetSPCategoriesAsXml(null,type).GetEnumerator();
            result.MoveNext();
            string res = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
            return new HttpResponseMessage() { Content = new StringContent(res, Encoding.UTF8, "application/xml") };
        }

        // GET api/SPCategories/5
        public SPCategories GetSPCategories(int id)
        {
            SPCategories spcategories = db.SPCategories.Find(id);
            if (spcategories == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return spcategories;
        }

        // PUT api/SPCategories/5
        public HttpResponseMessage PutSPCategories(int id, SPCategories spcategories)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != spcategories.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(spcategories).State = EntityState.Modified;

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

        // POST api/SPCategories
        public HttpResponseMessage PostSPCategories(SPCategories spcategories)
        {
            if (ModelState.IsValid)
            {
                db.SPCategories.Add(spcategories);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, spcategories);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = spcategories.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/SPCategories/5
        public HttpResponseMessage DeleteSPCategories(int id)
        {
            SPCategories spcategories = db.SPCategories.Find(id);
            if (spcategories == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.SPCategories.Remove(spcategories);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, spcategories);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}