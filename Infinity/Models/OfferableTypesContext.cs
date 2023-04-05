using System.Data.Entity;

namespace WhiteDream.Models
{
    public class OfferableTypesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WhiteDream.Models.OfferableTypesContext>());
        private YJEntities entities = new YJEntities();
        public OfferableTypesContext() : base("name=OfferableTypesContext")
        {
        }
        public string getOfferableTypesAsXml(int categoryId)
        {            
            var result = entities.usp_GetOfferableTypesByCategoryAsXml(categoryId).GetEnumerator();
            result.MoveNext();
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
        }

       
        public DbSet<OfferableTypes> OfferableTypes { get; set; }
    }
}
