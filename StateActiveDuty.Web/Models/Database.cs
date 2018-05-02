using Microsoft.EntityFrameworkCore;

namespace StateActiveDuty.Web.Models
{
    public class Database : DbContext
    {
        public virtual DbSet<Unit> Units { get; set; }

        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=Data.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Unit>()
                .OwnsOne(unit => unit.POC);

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