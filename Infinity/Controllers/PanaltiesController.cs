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
using WhiteDream.Classes;
using WhiteDream.Models;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class PanaltiesController : ApiController
    {
        private PenaltiesContext db = new PenaltiesContext();

        // GET api/Panalties
        public IEnumerable<Penalties> GetPenalties()
        {
            return db.Penalties.AsEnumerable();
        }

        // GET api/Panalties/5
        public Penalties GetPenalties(int id)
        {
            Penalties penalties = db.Penalties.Find(id);
            if (penalties == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return penalties;
        }

        // PUT api/Panalties/5
        public HttpResponseMessage PutPenalties(int id, Penalties penalties)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != penalties.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(penalties).State = EntityState.Modified;

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

        // POST api/Panalties
        public HttpResponseMessage PostPenalties(Envelope<Penalties> penaltyEnvelope)
        {
            if (ModelState.IsValid)
            {

                HttpResponseMessage response;
                try
                {
                    //if ((response = Security.analyzeRequest(penaltyEnvelope, Request,
                    //    partEnvelope.Entity.ChasisNo, partEnvelope.Entity.Code,
                    //    partEnvelope.Entity.DedicatedCode, partEnvelope.Entity.GuarranteeCompany,
                    //    partEnvelope.Entity.GurranteeTerms, partEnvelope.Entity.ProcurementCode,
                    //    partEnvelope.Entity.TechnicalNo, partEnvelope.Entity.Offerables.Name,
                    //    partEnvelope.Entity.Offerables.Notes)) != null)
                    //{
                    //    return response;
                    //}
                    ////todo: check if there is not already same record and if so, update it instead of insert
                    

                    //entities.Parts.Add(partEnvelope.Entity);
                    //entities.SaveChanges();

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(penaltyEnvelope.Entity.Id.ToString(), Encoding.UTF8, "text/plain")
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

        // DELETE api/Panalties/5
        public HttpResponseMessage DeletePenalties(int id)
        {
            Penalties penalties = db.Penalties.Find(id);
            if (penalties == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Penalties.Remove(penalties);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, penalties);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}