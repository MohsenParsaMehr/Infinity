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
using Infinity.Classes;
using Infinity.Models;
using Infinity.Utilities;

namespace Infinity.Controllers
{
    public class GroceryController : ApiController
    {
        private GroceryContext db = new GroceryContext();

        // GET api/Grocery
        private InfinityEntities entities = new InfinityEntities();
        [HttpPost]
        [ActionName("GetGroceries")]
        public HttpResponseMessage GetGroceries(Envelope<GetGroceryParams> envelope)
        {            
            HttpResponseMessage errorResponse;
            string securityCheckResult;
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((securityCheckResult = Security.analyzeRequest(envelope,  ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                return Request.CreateResponse(HttpStatusCode.OK,db.getGroceries(envelope).ToList(), "application/json");
                //entities.usp_GetGroceries(envelope.Entity.TypeId,envelope.Entity.CustomerRoleId                     

                //return Request.CreateResponse(HttpStatusCode.OK, entities.usp_GetOfferableCategories(null,
                //Constants.PARTS_CATEGORY_TITLE_IN_DB).Where(r => r.ParentRef == envelope.Entity.Id).Select
                //    (p => new { p.Id, p.Title }).OrderBy(p => p.Title).ToList(), "application/json");
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

        // GET api/Grocery/5
        public Grocery GetGrocery(int id)
        {
            Grocery grocery = db.Groceries.Find(id);
            if (grocery == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return grocery;
        }

        // PUT api/Grocery/5
        public HttpResponseMessage PutGrocery(int id, Grocery grocery)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != grocery.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(grocery).State = EntityState.Modified;

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

        // POST api/Grocery
        public HttpResponseMessage PostGrocery(Grocery grocery)
        {
            if (ModelState.IsValid)
            {
                db.Groceries.Add(grocery);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, grocery);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = grocery.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Grocery/5
        public HttpResponseMessage DeleteGrocery(int id)
        {
            Grocery grocery = db.Groceries.Find(id);
            if (grocery == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Groceries.Remove(grocery);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, grocery);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}