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
    public class CarSeriesController : ApiController
    {
        private CarSeriesContext db = new CarSeriesContext();
        private YJEntities entities = new YJEntities();
        // GET api/CarSeries
        [HttpPost]
        public HttpResponseMessage GetCarSeries(Envelope<CarBrands> envelope)
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
                    checkForbiddenPhrase: false, checkContactInfoEmbedding: false, checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, entities.CarSeries.Where(p => p.CarBrandRef == envelope.Entity.Id).Select
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

        // GET api/CarSeries/5
        public CarSeries GetCarSeries(int id)
        {
            CarSeries carseries = db.CarSeries.Find(id);
            if (carseries == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return carseries;
        }

        // PUT api/CarSeries/5
        public HttpResponseMessage PutCarSeries(int id, CarSeries carseries)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != carseries.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(carseries).State = EntityState.Modified;

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

        // POST api/CarSeries
        public HttpResponseMessage PostCarSeries(CarSeries carseries)
        {
            if (ModelState.IsValid)
            {
                db.CarSeries.Add(carseries);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, carseries);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = carseries.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/CarSeries/5
        public HttpResponseMessage DeleteCarSeries(int id)
        {
            CarSeries carseries = db.CarSeries.Find(id);
            if (carseries == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CarSeries.Remove(carseries);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, carseries);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}