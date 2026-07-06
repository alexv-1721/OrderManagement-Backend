
using Microsoft.EntityFrameworkCore;
using OrderManagement.API.Model;
namespace OrderManagement.API.DataContext
{
    public class AppDBContext :DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .Property(e => e.Cart)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<CartModel[]>(v, (System.Text.Json.JsonSerializerOptions?)null));

            modelBuilder.Entity<UserModel>()
                .Property(e => e.PlacedOrders)
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<int[]>(v, (System.Text.Json.JsonSerializerOptions?)null));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
