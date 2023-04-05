using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WhiteDream.Classes;
using WhiteDream.Utilities;

namespace WhiteDream.Controllers
{
    public class PartsController : ApiController
    {
        private YJEntities entities = new YJEntities();

        [HttpPost]
        [ActionName("GetAsXml")]
        public HttpResponseMessage GetAsXml(Envelope<Parts> partCriteriaEnvelope)
        {
            HttpResponseMessage errorResponse;
            if (partCriteriaEnvelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if ((errorResponse = Security.analyzeRequest(partCriteriaEnvelope, Request, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkForbiddenPhrase: false, checkContactInfoEmbedding: false, checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)) != null)
                {
                    return errorResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                var result = entities.usp_getPartsAsXml(partCriteriaEnvelope.Entity.Offerables.Name, partCriteriaEnvelope.Entity.TechnicalNo,
                    partCriteriaEnvelope.Entity.ChasisNo,null, null, null,null ,//todo: change null vaules to real values
                    partCriteriaEnvelope.Entity.Offerables.ProvinceRef, partCriteriaEnvelope.Entity.Offerables.CityRef, partCriteriaEnvelope.Entity.VehicleTypeRef, null).GetEnumerator();
                result.MoveNext();
                string res = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
                return new HttpResponseMessage() { Content = new StringContent(res, Encoding.UTF8, "application/xml") };
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
        //todo: delete following Get method
        // GET api/parts
        public HttpResponseMessage Get(string criteria,string technicalNo,string chasisNo, int? carBrandId,int? carSerieId, int? carSerieYearId,int? categoryId,int? provinceId,int? cityId)
        {            
            var result = entities.usp_getPartsAsXml(criteria,technicalNo,chasisNo,carBrandId, carSerieId, carSerieYearId, categoryId, provinceId, cityId,null,null).GetEnumerator();
            result.MoveNext();
            string res = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
            return new HttpResponseMessage() { Content = new StringContent(res, Encoding.UTF8, "application/xml") };
        }

        // GET api/parts/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/parts
        public HttpResponseMessage Post(Envelope<Parts> partEnvelope)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage errorResponse;
            try
            {               
                if ((errorResponse= Security.analyzeRequest(partEnvelope, Request, false,((partEnvelope.Username==null||partEnvelope.Password==null)?false:true),true,true,true,true,true,true,
                    partEnvelope.Entity.ChasisNo, partEnvelope.Entity.Code,
                    partEnvelope.Entity.DedicatedCode,partEnvelope.Entity.GuarranteeCompany,
                    partEnvelope.Entity.GurranteeTerms,partEnvelope.Entity.ProcurementCode,
                    partEnvelope.Entity.TechnicalNo,partEnvelope.Entity.Offerables.Name,
                    partEnvelope.Entity.Offerables.Notes)) != null)
                {
                    return errorResponse;
                }
                //todo: check if there is not already same record and if so, update it instead of insert
                    partEnvelope.Entity.Offerables.InsertDate = DateTime.Now;
                    foreach (OfferableVariation variation in partEnvelope.Entity.Offerables.OfferableVariation)
                    {
                        variation.Prices.InsertDate = DateTime.Now;
                    }
                    
                    //if (partEnvelope.Entity.Offerables.OperatorUserRef == 0)
                    //    partEnvelope.Entity.Offerables.OperatorUserRef = null;
                    //if (partEnvelope.Entity.QualityRef == 0)
                    //    partEnvelope.Entity.QualityRef = null;
                    //if (partEnvelope.Entity.CountryRef == 0)
                    //    partEnvelope.Entity.CountryRef = null;

                    entities.Parts.Add(partEnvelope.Entity);
                    entities.SaveChanges();
                                                                               
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(partEnvelope.Entity.Offerables.Id.ToString(), Encoding.UTF8, "text/plain")
                    };                
                                   
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
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }            
        }

        // PUT api/parts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/parts/5
        public void Delete(int id)
        {
        }
    }
}
