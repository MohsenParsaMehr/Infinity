using System.Data.Entity;

namespace WhiteDream.Models
{
    public class SPTypesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WhiteDream.Models.SPTypesContext>());
        YJEntities entities = new YJEntities();
        public SPTypesContext() : base("name=SPTypesContext")
        {
        }
        public string getSPTypesAsXml(int categoryId)
        {
            var result = entities.usp_GetOfferableTypesByCategoryAsXml(categoryId).GetEnumerator();
            result.MoveNext();
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + result.Current;
        }
        public DbSet<SPTypes> SPTypes { get; set; }
    }
}
