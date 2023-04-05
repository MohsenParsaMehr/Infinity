using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WhiteDream.Models;

namespace WhiteDream.Controllers
{
    public class OfferablePicturesController : ApiController
    {
        private OfferablePicturesContext db = new OfferablePicturesContext();

        // GET api/OfferablePictures
        public IEnumerable<OfferablePictures> GetOfferablePictures()
        {
            return db.OfferablePictures.AsEnumerable();
        }

        // GET api/OfferablePictures/5
        public OfferablePictures GetOfferablePictures(int id)
        {
            OfferablePictures offerablepictures = db.OfferablePictures.Find(id);
            if (offerablepictures == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return offerablepictures;
        }

        // PUT api/OfferablePictures/5
        public HttpResponseMessage PutOfferablePictures(int id, OfferablePictures offerablepictures)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != offerablepictures.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(offerablepictures).State = EntityState.Modified;

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

        // POST api/OfferablePictures
        public HttpResponseMessage PostOfferablePictures(int offerableId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string imagePath = HttpContext.Current.Server.MapPath("~") + "Images" + Path.DirectorySeparatorChar + "Offerables" + Path.DirectorySeparatorChar + "Parts" + Path.DirectorySeparatorChar + offerableId  +Path.DirectorySeparatorChar;
                    Directory.CreateDirectory(imagePath);

                    for (byte imageIndex = 0; imageIndex < HttpContext.Current.Request.Files.Count; imageIndex++)
                    {
                        HttpPostedFile image = HttpContext.Current.Request.Files[imageIndex];
                        string fileName = imagePath + imageIndex + image.FileName.Substring(image.FileName.LastIndexOf("."));
                        if (/*!File.Exists(fileName) && image.ContentLength > 0 && */image.ContentLength <= Constants.MAX_IMAGE_SIZE/*&& image.ContentType.StartsWith("image/")*/)
                        {
                            image.SaveAs(fileName);
                            db.insertPictureInfo(offerableId, fileName);                           
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

        // DELETE api/OfferablePictures/5
        public HttpResponseMessage DeleteOfferablePictures(int id)
        {
            OfferablePictures offerablepictures = db.OfferablePictures.Find(id);
            if (offerablepictures == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.OfferablePictures.Remove(offerablepictures);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, offerablepictures);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}