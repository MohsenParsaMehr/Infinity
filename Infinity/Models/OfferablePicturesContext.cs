using System.Data.Entity;

namespace WhiteDream.Models
{
    public class OfferablePicturesContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<WhiteDream.Models.OfferablePicturesContext>());
        YJEntities entities = new YJEntities();
        public OfferablePicturesContext() : base("name=OfferablePicturesContext")
        {
        }
        public void insertPictureInfo(int offerableId, string imageUrl)
        {
            Pictures picture = new Pictures() { Url = imageUrl, Status = "Unconfirmed" };
            entities.Pictures.Add(picture);
            OfferablePictures offerablePicture = new OfferablePictures() { OfferableRef = offerableId,Pictures=picture };
            entities.OfferablePictures.Add(offerablePicture);
            entities.SaveChanges();
       
        }
        public DbSet<OfferablePictures> OfferablePictures { get; set; }
    }
}
