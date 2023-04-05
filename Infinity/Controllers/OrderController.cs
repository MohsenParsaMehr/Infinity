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
using Infinity.Models;
using Infinity.Utilities;
using Infinity.Classes;
using System.Data.SqlTypes;

namespace Infinity.Controllers
{
    public class OrderController : ApiController
    {
        private OrderContext db = new OrderContext();
        private InfinityEntities entities = new InfinityEntities();
        [HttpPost]
        [ActionName("GetShoppingCart")]
        public HttpResponseMessage GetShoppingCart(Envelope<Entity> envelope)
        {
            HttpResponseMessage errorResponse;
            string securityCheckResult = "";
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if (!string.IsNullOrEmpty(securityCheckResult = Security.analyzeRequest(envelope,  ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)))
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

              
                var result = from orderItem in entities.OrderItem
                             //from grocery in entities.Grocery
                             where
                               orderItem.Order.UserRef == envelope.Entity.Id &&
                               orderItem.Order.Status == "Active" &&
                               (orderItem.Order.Type == null ||
                               orderItem.Order.Type == "Normal") &&
                               (orderItem.Type == null || orderItem.Type == "Normal") &&
                               orderItem.Status == "Active" 
                                
                               
                             orderby
                               orderItem.InsertDate
                             select new
                             {
                                 orderItem.Quantity,

                                 orderItem.Offerable.Grocery.Offerable.Id,
                                 orderItem.Offerable.Grocery.Offerable.Name,
                                 orderItem.Offerable.Grocery.ImageUrl,
                                 OfferableVariation =
                                 orderItem.Offerable.OfferableVariation.Select(v => new
                                 {
                                     Title = v.Title,
                                     Quantity = v.Quantity,
                                     Weight = v.Weight,
                                     Price = v.Price
                                 }),
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

        [HttpPost]
        [ActionName("GetOrderReview")]
        public HttpResponseMessage GetOrderReview(Envelope<OrderReviewInfo> envelope)
        {
            HttpResponseMessage errorResponse;
            string securityCheckResult = "";
            if (envelope == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Incomplete information");
            }
            try
            {
                if (!string.IsNullOrEmpty(securityCheckResult = Security.analyzeRequest(envelope, ignoreRemainingCheckOnFail: true,
                    checkCredentials: false /*((envelope.Username == null || envelope.Password == null) ? false : true)*/,
                    checkBlackListHistory: false, checkBrouthForceAttack: false,
                    checkServiceDenialAttack: true, checkTampering: false)))
                {
                    HttpResponseMessage securityResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                    securityResponse.ReasonPhrase = securityCheckResult;
                    return securityResponse;
                }
                entities.Configuration.ProxyCreationEnabled = false;

                var address =       
                    (from Address in entities.Address
                    join Province in entities.Province on Address.Id equals Province.Id into Province_join
                    from Province in Province_join.DefaultIfEmpty()
                    join City in entities.City on Address.Id equals City.Id into City_join
                    from City in City_join.DefaultIfEmpty()
                    where
                    Address.Id == envelope.Entity.SelectedAddressId
                    select new
                    {
                        Address.Id,
                        Address.Title,
                        Address.Phone,
                        Address.PostalAddress,
                        Address.PostalCode,
                        City = City.Title,
                        Province = Province.Title
                    }).SingleOrDefault();
                var deliveryMethod = (from DeliveryMethod in entities.DeliveryMethod
                    where
                    DeliveryMethod.Status == Constants.RecordStatus.ACTIVE &&
                    DeliveryMethod.Id == envelope.Entity.SelectedDeliveryMethodId
                    select new
                    {
                        DeliveryMethod.Id,
                        DeliveryMethod.Title,
                        DeliveryMethod.IsLocal,
                        DeliveryMethod.Cost,
                        DeliveryMethod.Notes,
                        DeliveryMethod.DeliveryTimeFrom,
                        DeliveryMethod.DeliveryTimeTo,
                        DeliveryMethod.ImageUrl,
                        DeliveryMethod.FreeIfOrderAbove
                    }).SingleOrDefault();
               
                var paymentMethod = (from PaymentMethod in entities.PaymentMethod
                    where
                    PaymentMethod.Status == Constants.RecordStatus.ACTIVE &&
                    PaymentMethod.Id == envelope.Entity.SelectedPaymentMethodId
                              
                    select new
                    {
                        PaymentMethod.Id,
                        PaymentMethod.Title,
                        PaymentMethod.Notes,
                        PaymentMethod.ImageUrl,
                        PaymentMethod.Discount,
                        //EPaymentBank =
                        //   from EPaymentBank in entities.EPaymentBank
                        //   where
                        //       EPaymentBank.PaymentMethodRef == PaymentMethod.Id
                        //   select new
                        //   {
                        //       EPaymentBank.Id,
                        //       EPaymentBank.Title,
                        //       EPaymentBank.LogoUrl,
                        //       EPaymentBank.RedirectUrl,
                        //       EPaymentBank.Notes
                        //   }
                    }).SingleOrDefault();

                return Request.CreateResponse(HttpStatusCode.OK,new {address,deliveryMethod,paymentMethod }, "application/json");
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

        [HttpPost]
        [ActionName("GetOrderHistory")]
        public HttpResponseMessage GetOrderHistory(Envelope<Entity> envelope)
        {
            return Helpers.handleRequest(envelope, db.getOrderHistory, Request,true);          
        }

        [HttpPost]
        [ActionName("SendOrder")]
        public HttpResponseMessage SendAddress(Envelope<Order> envelope)
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
                    if (envelope.Entity.OrderItem.Count > 0)
                    {
                        entities.Configuration.ProxyCreationEnabled = false;
                        DateTime today = DateTime.Now;
                        envelope.Entity.InsertDate = today;
                        envelope.Entity.UpdateDate = today;
                        foreach (var item in envelope.Entity.OrderItem)
                        {
                            item.InsertDate = today;
                            item.UpdateDate = today;
                        }
                        entities.Order.Add(envelope.Entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, new { envelope.Entity.Id }, "application/json");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Empty Order"));
                    }

                }
                catch (EntityException ee)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ee);                    
                }                
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);                    
                    //throw e;
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Model invalid");
        }


        // GET api/Order
        public IEnumerable<Order> GetOrders()
        {
            return db.Orders.AsEnumerable();
        }

        // GET api/Order/5
        public Order GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return order;
        }

        // PUT api/Order/5
        public HttpResponseMessage PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != order.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(order).State = EntityState.Modified;

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

        // POST api/Order
        public HttpResponseMessage PostOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, order);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Order/5
        public HttpResponseMessage DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Orders.Remove(order);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, order);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}