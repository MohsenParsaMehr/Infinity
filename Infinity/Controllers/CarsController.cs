using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using WhiteDream.Classes;
using WhiteDream.Models;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class CarsController : ApiController
    {
        private CarBrandsContext db = new CarBrandsContext();
        private YJEntities entities = new YJEntities();

        [HttpPost]
        public HttpResponseMessage GetCarBrands(EmptyEnvelope envelope)
        {
            HttpResponseMessage errorResponse;
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Incomplete information");
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

                return Request.CreateResponse(HttpStatusCode.OK, entities.CarBrands.Select
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
        public HttpResponseMessage GetCarBrandsAsXml()
        {
            return new HttpResponseMessage() { Content = new StringContent(db.getCarBrandsAsXml(), Encoding.UTF8, "application/xml") };
            
        }

        // GET api/Cars/5
        public CarBrands GetCarBrands(int id)
        {
            CarBrands carbrands = db.CarBrands.Find(id);
            if (carbrands == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return carbrands;
        }

        // PUT api/Cars/5
        public HttpResponseMessage PutCarBrands(int id, CarBrands carbrands)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != carbrands.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(carbrands).State = EntityState.Modified;

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

        // POST api/Cars
        public HttpResponseMessage PostCarBrands(CarBrands carbrands)
        {
            if (ModelState.IsValid)
            {
                db.CarBrands.Add(carbrands);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, carbrands);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = carbrands.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Cars/5
        public HttpResponseMessage DeleteCarBrands(int id)
        {
            CarBrands carbrands = db.CarBrands.Find(id);
            if (carbrands == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CarBrands.Remove(carbrands);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, carbrands);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}