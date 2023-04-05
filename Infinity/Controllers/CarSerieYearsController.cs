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
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class CarSerieYearsController : ApiController
    {
        private YJEntities db = new YJEntities();

        // GET api/CarSerieYears
        [HttpPost]
        public HttpResponseMessage GetCarSerieYears(Envelope<CarSeries> carSeriesEnvelope)
        {
            HttpResponseMessage errorResponse;
            if (carSeriesEnvelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((errorResponse = Security.analyzeRequest(carSeriesEnvelope, Request, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkForbiddenPhrase: false, checkContactInfoEmbedding: false, checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                db.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK, db.CarSerieYears.Where(y => y.CarSerieRef == carSeriesEnvelope.Entity.Id).Select
                    (y => new { y.Id, y.Title }).OrderBy(y => y.Title).ToList(), "application/json");
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

        // GET api/CarSerieYears/5
        public CarSerieYears GetCarSerieYears(int id)
        {
            CarSerieYears carserieyears = db.CarSerieYears.Find(id);
            if (carserieyears == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return carserieyears;
        }

        // PUT api/CarSerieYears/5
        public HttpResponseMessage PutCarSerieYears(int id, CarSerieYears carserieyears)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != carserieyears.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(carserieyears).State = EntityState.Modified;

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

        // POST api/CarSerieYears
        public HttpResponseMessage PostCarSerieYears(CarSerieYears carserieyears)
        {
            if (ModelState.IsValid)
            {
                db.CarSerieYears.Add(carserieyears);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, carserieyears);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = carserieyears.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/CarSerieYears/5
        public HttpResponseMessage DeleteCarSerieYears(int id)
        {
            CarSerieYears carserieyears = db.CarSerieYears.Find(id);
            if (carserieyears == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CarSerieYears.Remove(carserieyears);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, carserieyears);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}