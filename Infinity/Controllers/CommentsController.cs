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
using WhiteDream.Models;

namespace WhiteDream.Controllers
{
    public class CommentsController : ApiController
    {
        private CommentsContext db = new CommentsContext();
        private YJEntities entities = new YJEntities();

        // GET api/Comments
        public IEnumerable<Comments> GetComments()
        {
            return db.Comments.AsEnumerable();
        }

        // GET api/Comments/5
        public Comments GetComments(int id)
        {
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return comments;
        }

        // PUT api/Comments/5
        public HttpResponseMessage PutComments(int id, Comments comments)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != comments.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(comments).State = EntityState.Modified;

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

        // POST api/Comments
        public HttpResponseMessage PostComments(List<Comments > commentsList)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response;
                try
                {
                    foreach(var comments in commentsList){
                    if (comments.SPRef == 0)
                        comments.SPRef = null;
                    if (comments.OfferableRef == 0)
                        comments.OfferableRef = null;
                    if (comments.QuestionRef == 0)
                        comments.QuestionRef = null;
                    if (comments.BadContentTypeRef == 0)
                        comments.BadContentTypeRef = null;

                    if (comments.SPRef == null && comments.OfferableRef == null && comments.QuestionRef == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "user favorite for what entity?");
                    }
                    comments.Status = Constants.RecordStatus.ACTIVE;
                    comments.InsertDate = DateTime.Now;
                    comments.UpdateDate = DateTime.Now;

                    entities.Comments.Add(comments);
                    entities.SaveChanges();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(commentsList[0].Id.ToString(), Encoding.UTF8, "text/plain")
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

        // DELETE api/Comments/5
        public HttpResponseMessage DeleteComments(int id)
        {
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Comments.Remove(comments);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, comments);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}