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
using WhiteDream.Models;
using WhiteDream.Classes;
using WhiteDream.Utilities;
using System.Linq.Expressions;

namespace WhiteDream.Controllers
{
    public class BlackListsController : ApiController
    {
        private BlackListsContext db = new BlackListsContext();

        // GET api/BlackLists
        public IEnumerable<BlackLists> GetBlackLists()
        {
            return db.BlackLists.AsEnumerable();
        }
        private bool blackListFilter(BlackLists blackList){
            return true;
        }
        // GET api/BlackLists/5
        public List<BlackLists> GetBlackLists(Envelope<BlackLists> blackListEnvelope)
        {
            //todo: HEY!, consider both userref and ip address maybe specified together
            if (blackListEnvelope.Counter <= Constants.MAX_SERVICE_REQUEST_COUNT)
            {
                //string token = Encryption.decryptSymmetric(blackListEnvelope.Token);
                //string[] tokenParts = token.Split('\n');
                //if()
                Envelope<Users> userEnvelope = new Envelope<Users>();
                userEnvelope.Counter = 0;
                userEnvelope.IpAddress = blackListEnvelope.IpAddress;
                userEnvelope.Entity = new Users() { Username = blackListEnvelope.Username, Password = blackListEnvelope.Password };
                HttpResponseMessage loginResponse = new UsersController().Login(userEnvelope);
                if (loginResponse.StatusCode == HttpStatusCode.OK)
                {

                    Func<BlackLists, bool> predicate = new Func<BlackLists, bool>(blackListFilter);
                    new YJEntities().BlackLists.Where(predicate);
                }
            }
                throw new NotImplementedException();
                /////
            //BlackLists blacklists = db.BlackLists.Find(id);
            //if (blacklists == null)
            //{
            //    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            //}

            //return blacklists;
        }

        // PUT api/BlackLists/5
        public HttpResponseMessage PutBlackLists(int id, BlackLists blacklists)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != blacklists.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(blacklists).State = EntityState.Modified;

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

        // POST api/BlackLists
        public HttpResponseMessage PostBlackLists(BlackLists blacklists)
        {
            if (ModelState.IsValid)
            {
                db.BlackLists.Add(blacklists);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, blacklists);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = blacklists.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/BlackLists/5
        public HttpResponseMessage DeleteBlackLists(int id)
        {
            BlackLists blacklists = db.BlackLists.Find(id);
            if (blacklists == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.BlackLists.Remove(blacklists);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, blacklists);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}