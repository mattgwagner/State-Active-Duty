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
    }
}