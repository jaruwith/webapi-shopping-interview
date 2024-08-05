using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var images = new Images();

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(s => s.Product)
                .HasForeignKey<Stock>(s => s.ProductId);

            // Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "coke",
                    Description = "coke",
                    Price = 10.00m,
                    Image = images.imageCoke,
                    CreatedAt = DateTime.Parse("2024-07-31T01:29:40.0490000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.2253321")
                },
                new Product
                {
                    ProductId = 2,
                    Name = "pepsi",
                    Description = "pepsi",
                    Price = 20.00m,
                    Image = images.imagePepsi,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.2155640")
                },
                new Product
                {
                    ProductId = 3,
                    Name = "pepsi(1000ml)",
                    Description = "pepsi(1000ml)",
                    Price = 30.00m,
                    Image = images.imagePepsi1000ml,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.2059936")
                },
                new Product
                {
                    ProductId = 4,
                    Name = "coke(1000ml)",
                    Description = "coke(1000ml)",
                    Price = 30.00m,
                    Image = images.imageCoke1000ml,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.1954431")
                },
                new Product
                {
                    ProductId = 5,
                    Name = "green tea",
                    Description = "green tea",
                    Price = 40.00m,
                    Image = images.imageGreentea,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.1864804")
                },
                new Product
                {
                    ProductId = 6,
                    Name = "thai tea",
                    Description = "thai tea",
                    Price = 50.00m,
                    Image = images.imageThaitea,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.Parse("2024-08-01T21:47:30.1757295")
                },
                new Product
                {
                    ProductId = 7,
                    Name = "coke(taste)",
                    Description = "coke(taste)",
                    Price = 0.00m,
                    Image = images.imageCoketested,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.MinValue
                }
            );

            modelBuilder.Entity<Stock>().HasData(
                new Stock { StockId = 1, ProductId = 1, Quantity = 69 },
                new Stock { StockId = 2, ProductId = 2, Quantity = 85 },
                new Stock { StockId = 3, ProductId = 3, Quantity = 82 },
                new Stock { StockId = 4, ProductId = 4, Quantity = 76 },
                new Stock { StockId = 5, ProductId = 5, Quantity = 81 },
                new Stock { StockId = 6, ProductId = 6, Quantity = 83 },
                new Stock { StockId = 7, ProductId = 7, Quantity = 0 }
            );
        }
    }
}
