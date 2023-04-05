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
using WhiteDream.Classes;
using WhiteDream.Models;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class ProvincesController : ApiController
    {
        private ProvincesContext db = new ProvincesContext();
        private YJEntities entities = new YJEntities();
        // GET api/Provinces
        [HttpPost]
        [ActionName("GetProvinces")]
        public HttpResponseMessage GetProvinces(EmptyEnvelope envelope)
        {
            HttpResponseMessage errorResponse;
            if (envelope == null)
            {
                errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Insuficient info.");
                return errorResponse;
            }
            try
            {
                if ((errorResponse = Security.analyzeRequest(envelope, Request, ignoreRemainingCheckOnFail: true,
                    checkCredentials:false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, entities.Provinces.Select
                    (p => new { p.Id, p.Title }).OrderBy(p => p.Title).ToList(), "application/json");           
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

        // GET api/Provinces/5
        public Provinces GetProvinces(int id)
        {
            Provinces provinces = db.Provinces.Find(id);
            if (provinces == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return provinces;
        }

        // PUT api/Provinces/5
        public HttpResponseMessage PutProvinces(int id, Provinces provinces)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != provinces.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(provinces).State = EntityState.Modified;

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

        // POST api/Provinces
        public HttpResponseMessage PostProvinces(Provinces provinces)
        {
            if (ModelState.IsValid)
            {
                db.Provinces.Add(provinces);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, provinces);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = provinces.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Provinces/5
        public HttpResponseMessage DeleteProvinces(int id)
        {
            Provinces provinces = db.Provinces.Find(id);
            if (provinces == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Provinces.Remove(provinces);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, provinces);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}