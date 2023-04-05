using System.Data.Entity;

namespace WhiteDream.Models
{
    public class ServiceProvidersContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WhiteDream.Models.ServiceProvidersContext>());
        YJEntities entities = new YJEntities();
        public ServiceProvidersContext() : base("name=ServiceProvidersContext")
        {
        }
        public void updateLogo(int SPId, string imageUrl)
        {
            ServiceProviders sp = entities.ServiceProviders.Find(SPId);
            sp.Logo = imageUrl;
            sp.Status = sp.Status+"|"+Constants.RecordStatus.IMAGE_UNCONFIRMED;

            entities.SaveChanges();

        }
        public DbSet<ServiceProviders> ServiceProviders { get; set; }
    }
}
