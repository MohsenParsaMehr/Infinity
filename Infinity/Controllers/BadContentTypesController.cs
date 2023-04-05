using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WhiteDream.Classes;
using WhiteDream.Models;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class BadContentTypesController : ApiController
    {
        private BadContentTypesContext db = new BadContentTypesContext();
        private YJEntities entities = new YJEntities();

        // GET api/BadContentTypes
        [HttpPost]
        [ActionName("GetBadContentTypes")]
        public HttpResponseMessage GetBadContentTypes(EmptyEnvelope envelope)
        {
            HttpResponseMessage errorResponse;
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((errorResponse = Security.analyzeRequest(envelope, Request, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, entities.BadContentTypes.Select
               (b => new { b.Id, b.Title }).ToList(), "application/json");
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
            //entities.Configuration.LazyLoadingEnabled = false;
                       
           
        }

        // GET api/BadContentTypes/5
        public BadContentTypes GetBadContentTypes(int id)
        {
            BadContentTypes badcontenttypes = db.BadContentTypes.Find(id);
            if (badcontenttypes == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return badcontenttypes;
        }

        // PUT api/BadContentTypes/5
        public HttpResponseMessage PutBadContentTypes(int id, BadContentTypes badcontenttypes)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != badcontenttypes.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(badcontenttypes).State = EntityState.Modified;

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

        // POST api/BadContentTypes
        public HttpResponseMessage PostBadContentTypes(BadContentTypes badcontenttypes)
        {
            if (ModelState.IsValid)
            {
                db.BadContentTypes.Add(badcontenttypes);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, badcontenttypes);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = badcontenttypes.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/BadContentTypes/5
        public HttpResponseMessage DeleteBadContentTypes(int id)
        {
            BadContentTypes badcontenttypes = db.BadContentTypes.Find(id);
            if (badcontenttypes == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.BadContentTypes.Remove(badcontenttypes);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, badcontenttypes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}