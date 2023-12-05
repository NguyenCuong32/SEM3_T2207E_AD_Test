using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class FashionContext : DbContext
    {
        public FashionContext(DbContextOptions<FashionContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<DetailOrderModel> DetailOrders { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<SeasonModel> seasonsModels { get; set; } 
        public DbSet<AgesModel> AgesModels { get; set; } 
        public DbSet<GendersModel> GendersModels { get; set; } 
    }
}
