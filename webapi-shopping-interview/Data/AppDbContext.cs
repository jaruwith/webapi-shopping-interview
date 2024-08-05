using Microsoft.EntityFrameworkCore;
using webapi_shopping_interview.Model;

namespace webapi_shopping_interview.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var images = new Images();

            // Seed initial data
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "coke",
                    Description = "coke",
                    Price = 10.00m,
                    Stock = 69,
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
                    Stock = 85,
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
                    Stock = 82,
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
                    Stock = 76,
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
                    Stock = 81,
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
                    Stock = 83,
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
                    Stock = 0,
                    Image = images.imageCoketested,
                    CreatedAt = DateTime.Parse("2024-07-31T01:35:04.4450000"),
                    UpdatedAt = DateTime.MinValue
                }
            );
        }

    }
}