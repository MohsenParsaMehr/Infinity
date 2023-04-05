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
using Infinity.Classes;
using Infinity.Utilities;

namespace Infinity.Controllers
{
    public class UsersController : ApiController
    {
        private InfinityEntities db = new InfinityEntities();

        // GET api/Users
        public IEnumerable<User> GetUsers()
        {
            return db.User.AsEnumerable();
        }

        // GET api/Users/5
        public User GetUsers(int id)
        {
            User users = db.User.Find(id);
            if (users == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return users;
        }

        // PUT api/Users/5
        public HttpResponseMessage PutUsers(int id, User users)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != users.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(users).State = EntityState.Modified;

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

        // POST api/Users
        [AllowAnonymous]
        [HttpPost]
        [ActionName("Signup")]
        public HttpResponseMessage Signup(Envelope<User> userEnvelope)
        {
            if (ModelState.IsValid)
            {
                db.Configuration.ProxyCreationEnabled = false;
                HttpResponseMessage errorResponse;
                string securityCheckResult = "";
                if (string.IsNullOrEmpty(userEnvelope.Entity.Username) || string.IsNullOrEmpty(userEnvelope.Entity.Password))
                {
                    errorResponse = Request.CreateResponse(HttpStatusCode.NotFound);
                    return errorResponse;
                }
                try
                {
                    if ((securityCheckResult = Security.analyzeRequest(userEnvelope, false,
                         false , true, true, true, true, true, true,
                        userEnvelope.Entity.Username )) != "")
                    {
                        HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                        securityResponse.ReasonPhrase = securityCheckResult;
                        return securityResponse;
                    }
                    //todo: check if there is not already same record and if so, update it instead of insert
                    userEnvelope.Entity.InsertDate = DateTime.Now;
                    
                    if (string.IsNullOrEmpty(userEnvelope.Entity.Status))
                        userEnvelope.Entity.Status = Constants.RecordStatus.ACTIVE;

                    db.User.Add(userEnvelope.Entity);
                    db.SaveChanges();

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(userEnvelope.Entity.Id.ToString(), Encoding.UTF8, "text/plain")
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
        private InfinityEntities entities = new InfinityEntities();
        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage Login(Envelope<User> envelope)
        {
            if (ModelState.IsValid)
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
                    usp_userLogin_Result user = (usp_userLogin_Result)entities.usp_userLogin(envelope.Entity.Username, envelope.Entity.Password, null).SingleOrDefault();
                    if (user != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, user, "application/json");

                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound);                                  
                    }                    
                    
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
                //    HttpResponseMessage errorResponse;
                //    if (string.IsNullOrEmpty(userEnvelope.Entity.Username) || string.IsNullOrEmpty(userEnvelope.Entity.Password))
                //    {
                //        errorResponse = Request.CreateResponse(HttpStatusCode.NotFound);
                //        return errorResponse;
                //    }
                //    try
                //    {
                //        if ((errorResponse = Security.analyzeRequest(userEnvelope, Request, false,
                //             false, false, false, true, true, true, true)) != null)
                //        {
                //            return errorResponse;
                //        }
                //        var loggedInUser = (db.User.Where(u => u.Username == userEnvelope.Entity.Username &&
                //            u.Password == userEnvelope.Entity.Password));//.SingleOrDefault()

                //        if (loggedInUser.ToList().Count != 0)
                //        {
                //            if (loggedInUser.First().Status == Constants.RecordStatus.ACTIVE)
                //            {
                //                loggedInUser.First().SPRef = null; //todo: temp
                //                //return Request.CreateResponse<User>(HttpStatusCode.OK, loggedInUser);
                //                return Request.CreateResponse<User>(HttpStatusCode.OK, loggedInUser.First()      , "application/json");
                //            }
                //            else
                //            {
                //                errorResponse = Request.CreateResponse(HttpStatusCode.Forbidden);
                //                errorResponse.ReasonPhrase = Constants.ReasonPhrases.USER_NOT_ACTIVE;
                //                return errorResponse;
                //            }
                //        }
                //        else
                //        {
                //            errorResponse = Request.CreateResponse(HttpStatusCode.NotFound);
                //            return errorResponse;
                //        }

                //    }
                //    catch (EntityException ee)
                //    {
                //        errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);
                //        return errorResponse;
                //    }
                //    catch (Exception e)
                //    {
                //        errorResponse = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                //        return errorResponse;
                //        //throw e;
                //    }
                }

                else                
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
          

        }
        [HttpPost]
        [ActionName("GetUserAddresses")]
        public HttpResponseMessage GetUserAddresses(Envelope<Entity> envelope)
        {
            if (ModelState.IsValid)
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

                    var result = from User in db.User
                                 where
                                   User.Id == envelope.Entity.Id
                                 select new
                                 {
                                     User.Person.Mobile,
                                     Addresses =
                                     from Address in db.Address
                                     join Province in db.Province on Address.Id equals Province.Id into Province_join
                                     from Province in Province_join.DefaultIfEmpty()
                                     join City in db.City on Address.Id equals City.Id into City_join
                                     from City in City_join.DefaultIfEmpty()
                                     where
                                       Address.UserRef == User.Id
                                     select new
                                     {
                                         Address.Id,
                                         Address.Title,
                                         Address.Phone,
                                         Address.PostalAddress,
                                         Address.PostalCode,
                                         City = City.Title,
                                         Province = Province.Title
                                     }
                                 };                                                                   
                   
                        return Request.CreateResponse(HttpStatusCode.OK, result.SingleOrDefault(), "application/json");                   
                    

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
            
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError,"Model invalid");
        }

        [HttpPost]
        [ActionName("SendAddress")]
        public HttpResponseMessage SendAddress(Envelope<Address> envelope)
        {
            if (ModelState.IsValid)
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

                    entities.Address.Add(envelope.Entity);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, envelope.Entity, "application/json");


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

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Model invalid");
        }

        [HttpPost]
        [ActionName("GetProvinces")]
        public HttpResponseMessage GetProvinces(EmptyEnvelope envelope)
        {
            if (ModelState.IsValid)
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

                    var result = from Province in db.Province
                                 select new
                                 {
                                     Province.Id,
                                     Province.Title,
                                     Province.PrefixCode
                                 };

                    return Request.CreateResponse(HttpStatusCode.OK, result.ToList(), "application/json");


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

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Model invalid");
        }
        [HttpPost]
        [ActionName("GetCities")]
        public HttpResponseMessage GetCities(Envelope<Entity> envelope)
        {
            if (ModelState.IsValid)
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

                    var result = from City in db.City
                                 where City.ProvinceRef == envelope.Entity.Id
                                 select new
                                 {
                                     City.Id,
                                     City.Title,
                                     City.PrefixCode
                                 };

                    return Request.CreateResponse(HttpStatusCode.OK, result.ToList(), "application/json");

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

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Model invalid");
        }
        // DELETE api/Users/5
        public HttpResponseMessage DeleteUsers(int id)
        {
            User users = db.User.Find(id);
            if (users == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.User.Remove(users);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}