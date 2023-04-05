using Infinity.Classes;
using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;

namespace Infinity.Models
{
    public class OrderContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Infinity.Models.OrderContext>());

        public OrderContext() : base("name=OrderContext")
        {
        }
        private InfinityEntities entities = new InfinityEntities();
        public IQueryable<Object> getOrderHistory(Envelope<Entity> requestEnvelope)
        {
            entities.Configuration.ProxyCreationEnabled = false;
            var orderHistory =
                    from order in entities.Order
                    where
                      order.UserRef == requestEnvelope.Entity.Id
                    select new
                    {
                        order.UpdateDate,
                        order.Status,
                        order.Type,
                        order.DeliveryMethod.Title,
                        order.DeliveryMethod.Cost,
                        OrderItems =
                            from OrderItem in entities.OrderItem
                            from Grocery in entities.Grocery
                            where
                            OrderItem.OrderRef == order.Id
                            select new
                            {
                                OrderItem.Quantity,
                                OrderItem.Type,
                                OrderItem.Status,
                                Grocery.Offerable.Name,
                                Grocery.Offerable.Notes,
                                Grocery.Weight,
                                Grocery.ImageUrl,
                                Grocery.Offerable.Id,
                                OfferableVariation =
                                from OfferableVariation in entities.OfferableVariation
                                where
                                OfferableVariation.OfferableRef == OrderItem.OfferableRef
                                select new
                                {
                                    OfferableVariation.Title,
                                    OfferableVariation.Notes,
                                    OfferableVariation.Weight,
                                    Price =                                         
                                        new
                                        {
                                            PriceValue= OfferableVariation.Price.PriceValue
                                        }
                                                                                   
                                }
                            }
                    };
            return orderHistory;
        }
        public DbSet<Order> Orders { get; set; }
    }
}