using Infinity.Classes;
using Infinity.Utilities;
using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
namespace Infinity.Models
{
    public class PaymentContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Infinity.Models.PaymentContext>());

        public PaymentContext() : base("name=PaymentContext")
        {
        }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        private InfinityEntities entities = new InfinityEntities();
        public IEnumerable<Object> getPaymentMethods(EmptyEnvelope envelope)
        {
            
            
            try
            {                
                entities.Configuration.ProxyCreationEnabled = false;

                var result = from PaymentMethod in entities.PaymentMethod                             
                             where
                               PaymentMethod.Status == Constants.RecordStatus.ACTIVE
                             select new
                             {
                                 PaymentMethod.Id,
                                 PaymentMethod.Title,
                                 PaymentMethod.Notes,
                                 PaymentMethod.ImageUrl,
                                 PaymentMethod.Discount,
                                 EPaymentBank = 
                                    from EPaymentBank in entities.EPaymentBank
                                    where
                                        EPaymentBank.PaymentMethodRef == PaymentMethod.Id
                                    select new
                                    {
                                        EPaymentBank.Id,
                                        EPaymentBank.Title,
                                        EPaymentBank.LogoUrl,
                                        EPaymentBank.RedirectUrl,
                                        EPaymentBank.Notes
                                    }                                
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
    }
}
