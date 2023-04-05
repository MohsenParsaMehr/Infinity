using System.Data.Entity;

namespace WhiteDream.Models
{
    public class OfferableTypePropertiesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WhiteDream.Models.OfferableTypePropertiesContext>());
        private YJEntities entities = new YJEntities();
        public OfferableTypePropertiesContext() : base("name=OfferableTypePropertiesContext")
        {
        }
        public string getOfferableTypePropertiesAsXml(int offerableTypeId)
        {
            var result = entities.usp_getOfferableTypeProperties(offerableTypeId).GetEnumerator();
            result.MoveNext();
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
        }

        public DbSet<OfferableTypeProperties> OfferableTypeProperties { get; set; }
    }
}
