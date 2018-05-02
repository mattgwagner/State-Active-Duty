using Microsoft.EntityFrameworkCore;

namespace StateActiveDuty.Web.Models
{
    public class Database : DbContext
    {
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=Data.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<PurchaseOrder>()
                .OwnsOne(order => order.Vendor)
                .OwnsOne(order => order.PhysicalAddress);
        }

        public static void Init(Database db)
        {
            db.Database.Migrate();
        }
    }
}