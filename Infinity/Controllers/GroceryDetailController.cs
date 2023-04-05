using Infinity.Classes;
using Infinity.Models;
using Infinity.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Infinity.Controllers
{
    
    public class GroceryDetailController : ApiController
    {
        
        private GroceryContext db = new GroceryContext();

        // GET api/g
        private InfinityEntities entities = new InfinityEntities();            
        
        [HttpPost]
        [ActionName("GetGroceryDetail")]
        public HttpResponseMessage GetGroceryDetail(Envelope<GetGroceryDetailParams> envelope)
        {
            HttpResponseMessage errorResponse;
            string securityCheckResult;
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((securityCheckResult = Security.analyzeRequest(envelope, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                //var pictures = (from OfferablePicture in entities.OfferablePicture
                //                where
                //                  OfferablePicture.Picture.Status == "Active" &&
                //                  OfferablePicture.OfferableRef == envelope.Entity.Id

                //                select (new
                //                {
                //                    OfferableRef = OfferablePicture.OfferableRef,
                //                    Url = OfferablePicture.Picture.Url,
                //                    Alternate = OfferablePicture.Picture.Alternate,
                //                    Description = OfferablePicture.Picture.Description
                //                })).ToList();

                var result = from g in entities.Grocery
                             join Off in entities.Off on new { Id = g.Offerable.Id } equals new { Id = Off.OfferableRef } into Off_join
                             from Off in Off_join.DefaultIfEmpty()
                             where
                               (g.Offerable.Status == "Active" &&
                               g.Offerable.Id == envelope.Entity.Id) &&
                               (Off.ValidFrom == null ||
                               Off.ValidFrom <= DateTime.Now) &&
                               (Off.ValidTo == null ||
                               Off.ValidTo >= DateTime.Now) &&
                               (Off.ValidFrom == null ||
                               Off.ValidFrom <= DateTime.Now) &&
                               (Off.ValidTo == null ||
                               Off.ValidTo >= DateTime.Now)
                             orderby
                               (decimal?)Off.Price,
                               g.Offerable.Name,
                               g.ExpireDate,
                               g.Offerable.IsSpecial descending,
                               g.ProductionDate
                             select new
                             {
                                 g.Offerable.Name,
                                 IsSpecial = (bool?)g.Offerable.IsSpecial,
                                 OffPrice = (decimal?)Off.Price,
                                 SPRef = (int?)g.Offerable.SPRef,
                                 TypeRef = (int?)g.Offerable.TypeRef,
                                 g.Offerable.Notes,
                                 BrandRef = (int?)g.Offerable.BrandRef,
                                 g.ScoresToBuy,
                                 g.ScoreCount,
                                 g.FreshnessGuaranteeDays,
                                 Id = (int?)g.Offerable.Id ,

                                 Pictures =
                                 g.Offerable.OfferablePicture.Select(p => new
                                 {
                                     OfferableRef = p.OfferableRef,
                                     Url = p.Picture.Url,
                                     ThumbnailUrl = p.Picture.ThumbnailUrl,
                                     Alternate = p.Picture.Alternate,
                                     Description = p.Picture.Description
                                 }),
                                 OfferableVariations =
                                 g.Offerable.OfferableVariation.Select(v =>new
                                 {
                                     Title=v.Title,
                                     Quantity = v.Quantity,
                                     Weight = v.Weight,
                                     Price = v.Price
                                 }),
                                 OrderItem=
                                 g.Offerable.OrderItem.Select(o=> new
                                 {
                                     Quantity = o.Quantity
                                 })
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result.ToList(), "application/json");
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

        // GET api/g/5
        public Grocery GetGrocery(int id)
        {
            Grocery grocery = db.Groceries.Find(id);
            if (grocery == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return grocery;
        }

        // PUT api/g/5
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

        // POST api/g
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

        // DELETE api/g/5
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
