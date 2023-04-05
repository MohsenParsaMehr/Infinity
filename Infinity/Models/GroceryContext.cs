using Infinity.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;

namespace Infinity.Models
{
    public class GroceryContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Infinity.Models.GroceryContext>());
        private InfinityEntities entities = new InfinityEntities();
        public GroceryContext() : base("name=GroceryContext")
        {
        }
        public IEnumerable<Object> getGroceries(Envelope<GetGroceryParams> requestEnvelope)
        {
            var result =
                from Grocery in entities.Grocery 
                from Off in entities.Off
                from OfferableVariation in entities.OfferableVariation
                where
                  Grocery.Offerable.Status == Constants.RecordStatus.ACTIVE &&
                  OfferableVariation.Price.ValidTo == null &&
                  (Off.CustomerRoleRef == null ||
                  Off.CustomerRoleRef == requestEnvelope.Entity.CustomerRoleId) &&
                  (Off.ValidFrom == null ||
                  Off.ValidFrom <= SqlFunctions.GetDate()) &&
                  (Off.ValidTo == null ||
                  Off.ValidTo >= SqlFunctions.GetDate()) &&
                  Grocery.Offerable.TypeRef == requestEnvelope.Entity.TypeId ||
                  Grocery.Offerable.Status == Constants.RecordStatus.ACTIVE &&
                  OfferableVariation.Price.ValidTo >= SqlFunctions.GetDate() &&
                  (Off.CustomerRoleRef == null ||
                  Off.CustomerRoleRef == requestEnvelope.Entity.CustomerRoleId) &&
                  (Off.ValidFrom == null ||
                  Off.ValidFrom <= SqlFunctions.GetDate()) &&
                  (Off.ValidTo == null ||
                  Off.ValidTo >= SqlFunctions.GetDate()) &&
                  Grocery.Offerable.TypeRef == requestEnvelope.Entity.TypeId &&
                  OfferableVariation.Price.ValidFrom <= SqlFunctions.GetDate()
                //orderby
                //  Off.Price,
                //  Off.Offerable.Name,
                //  Grocery.ExpireDate,
                //  Off.Offerable.IsSpecial descending,
                //  OfferableVariation.Quantity descending
                select new {
                  Id = (int?)Grocery.Offerable.Id,
                  Grocery.Offerable.Name,
                  IsSpecial = (bool?)Grocery.Offerable.IsSpecial,
                  Grocery.ImageUrl,
                  OfferableVariation.Title,
                  OfferableVariation.Weight,
                  PriceValue = (decimal?)OfferableVariation.Price.PriceValue,
                  OffPrice =Off.Price ,
                  OfferableVariation.Quantity,
                  Grocery.ExpireDate,
                  Grocery.Offerable.UpdateDate,
                  BrandTitle = Grocery.Offerable.Brand.Title,
                  //Status = Off.Offerable.Status,
                  //PriceValidTo = OfferableVariation.Price.ValidTo,
                  //Off.CustomerRoleRef,
                  //OffValidFrom = Off.ValidFrom,
                  //Off.Offerable.TypeRef,
                  //OffValidTo = Off.ValidTo,
                  //PriceValidFrom = OfferableVariation.Price.ValidFrom
                };
            
            if (requestEnvelope.sortCriterias == null || requestEnvelope.sortCriterias.BySaving)
            {
                result = result.OrderByDescending(r => r.OffPrice).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.IsSpecial).ThenByDescending(r => r.Quantity);
            }            
            else if (requestEnvelope.sortCriterias.ByPriceLowToHigh)
            {
                result = result.OrderBy(r => r.PriceValue).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.IsSpecial).ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByMostSale)
            {
                
            }
            else if (requestEnvelope.sortCriterias.BySpecialOfer)
            {
                result = result.OrderByDescending(r => r.IsSpecial).ThenByDescending(r => r.OffPrice)
                    .ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByMostVisit)
            {
                result = result.OrderByDescending(r => r.IsSpecial).ThenByDescending(r => r.OffPrice)
                    .ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByNewProducts)
            {
                result = result.OrderBy(r=>r.UpdateDate).ThenByDescending(r => r.IsSpecial)
                    .ThenByDescending(r => r.OffPrice).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByPriceHighToLow)
            {
                result = result.OrderByDescending(r => r.PriceValue).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.IsSpecial).ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByBrandAtoZ)
            {
                result = result.OrderBy(r=>r.BrandTitle).ThenByDescending(r => r.PriceValue).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.IsSpecial).ThenByDescending(r => r.Quantity);
            }
            else if (requestEnvelope.sortCriterias.ByBrandZtoA)
            {
                result = result.OrderByDescending(r => r.BrandTitle).ThenByDescending(r => r.PriceValue).ThenBy(r => r.Name).ThenBy(r => r.ExpireDate)
                    .ThenByDescending(r => r.IsSpecial).ThenByDescending(r => r.Quantity);
            }
            if (requestEnvelope.searchCriterias != null)
            {

                if(requestEnvelope.searchCriterias.PriceFrom != null)
                {
                    var filteredResult = result.Where(r => r.PriceValue >= requestEnvelope.searchCriterias.PriceFrom);
                    result = filteredResult;
                        //from r in result
                        //     where
                        //        r.PriceValue >= requestEnvelope.searchCriterias.PriceFrom
                        //     select r;
                }
                if(requestEnvelope.searchCriterias.PriceTo != null)
                {
                    var filteredResult = result.Where(r => r.PriceValue <= requestEnvelope.searchCriterias.PriceTo);
                    result = filteredResult;
                    //result = from r in result
                    //         where
                    //            r.PriceValue <= requestEnvelope.searchCriterias.PriceTo
                    //         select r;
                }
            } else
            {

            }
            return result;
        }
        public DbSet<Grocery> Groceries { get; set; }

        public DbSet<Offerable> Offerables { get; set; }
    }
}
