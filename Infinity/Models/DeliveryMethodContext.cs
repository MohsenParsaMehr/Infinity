using Infinity.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Infinity.Models
{
    public class DeliveryMethodContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Infinity.Models.DeliveryMethodContext>());

        public DeliveryMethodContext() : base("name=DeliveryMethodContext")
        {
        }
        private InfinityEntities entities = new InfinityEntities();
        public IEnumerable<Object> getDeliveryMethods(EmptyEnvelope envelope)
        {
            try
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var result = from DeliveryMethod in entities.DeliveryMethod
                             where
                               DeliveryMethod.Status == Constants.RecordStatus.ACTIVE
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
                             };

                return result;
            }
            catch (EntityException ee)
            {
                throw ee;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
    }
}