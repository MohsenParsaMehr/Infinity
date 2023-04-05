using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using WhiteDream.Models;

namespace WhiteDream.Controllers
{
    public class ServiceProvidersController : ApiController
    {
        private ServiceProvidersContext db = new ServiceProvidersContext();
        private YJEntities entities = new YJEntities();

        // GET api/ServiceProviders
        public IEnumerable<ServiceProviders> GetServiceProviders()
        {
            return db.ServiceProviders.AsEnumerable();
        }

        // GET api/ServiceProviders/5
        public ServiceProviders GetServiceProviders(int id)
        {
            ServiceProviders serviceproviders = db.ServiceProviders.Find(id);
            if (serviceproviders == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return serviceproviders;
        }

        // PUT api/ServiceProviders/5
        public HttpResponseMessage PutServiceProviders(int id, ServiceProviders serviceproviders)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != serviceproviders.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(serviceproviders).State = EntityState.Modified;

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
        [HttpPost]
        [ActionName("SendSP")]

        // POST api/ServiceProviders
        public HttpResponseMessage PostServiceProviders(ServiceProviders serviceprovider)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;
                try
                {
                    if (serviceprovider.TypeRef == null || serviceprovider.TypeRef <= 0)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "SPType not provided");
                    }
                    serviceprovider.Status = Constants.RecordStatus.UNCONFIRMED;

                    entities.ServiceProviders.Add(serviceprovider);
                    entities.SaveChanges();
                    
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(serviceprovider.Id.ToString(), Encoding.UTF8, "text/plain")
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
        [HttpPost]
        [ActionName("Logo")]
        public HttpResponseMessage PostLogo(int SPId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string imagePath = HttpContext.Current.Server.MapPath("~") + "Images" + Path.DirectorySeparatorChar + Path.DirectorySeparatorChar + SPId + Path.DirectorySeparatorChar;
                    Directory.CreateDirectory(imagePath);

                    for (byte imageIndex = 0; imageIndex < HttpContext.Current.Request.Files.Count; imageIndex++)
                    {
                        HttpPostedFile image = HttpContext.Current.Request.Files[imageIndex];
                        string fileName = imagePath + imageIndex + image.FileName.Substring(image.FileName.LastIndexOf("."));
                        if (image.ContentLength > 0 && image.ContentLength <= Constants.MAX_IMAGE_SIZE/*&& image.ContentType.StartsWith("image/")*/)
                        {
                            image.SaveAs(fileName);
                            db.updateLogo(SPId, fileName);
                            //((IObjectContextAdapter)db).ObjectContext.ObjectStateManager.ChangeObjectState(offerablePicture, EntityState.Added);
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "اندازه عکس بیش از حد مجاز است");
                        }
                    }

                    //db.SaveChanges();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (IOException ioex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ioex);
            }
            catch (EntityException ee)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        // DELETE api/ServiceProviders/5
        public HttpResponseMessage DeleteServiceProviders(int id)
        {
            ServiceProviders serviceproviders = db.ServiceProviders.Find(id);
            if (serviceproviders == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ServiceProviders.Remove(serviceproviders);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, serviceproviders);
        }
       
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}