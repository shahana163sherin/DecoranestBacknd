using DecoranestBacknd.DecoraNest.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace DecoranestBacknd.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } 
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cart>()
                 .HasOne(u => u.User)
                 .WithMany(c => c.Carts)
                 .HasForeignKey(u => u.UserID);



            modelBuilder.Entity<Wishlist>()
                .HasOne(u => u.User)
                .WithMany(w => w.Wishlists)
                .HasForeignKey(u => u.UserID);



            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(o => o.Order)
                .HasForeignKey(o => o.OrderID);



            modelBuilder.Entity<CartItem>()
              .HasOne(c => c.Product)
              .WithMany(c => c.CartItems)
              .HasForeignKey(c => c.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(c => c.CartId);




            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(w => w.Wishlists)
                .HasForeignKey(w => w.ProductId);

            modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<RefreshToken>()
     .HasOne(rt => rt.User)
     .WithMany(u => u.RefreshTokens)
     .HasForeignKey(rt => rt.User_Id)
     .HasPrincipalKey(u => u.User_Id)   // <-- important fix
     .OnDelete(DeleteBehavior.Cascade);


            
            modelBuilder.Entity<User>().HasData(new User
            {
                User_Id = 1,
                Name = "Admin1",
                Email = "admin@deco.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
                IsBlocked = false
            });







            modelBuilder.Entity<CartItem>()
            .Property(c => c.UnitPrice)
            .HasPrecision(18, 2);


            modelBuilder.Entity<Wishlist>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
          .Property(c => c.TotalAmount)
         .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);


            modelBuilder.Entity<OrderItem>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
    .HasOne(p => p.Category)
    .WithMany(c => c.Products)
    .HasForeignKey(p => p.CategoryId);


            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(o => o.Order)
                .HasForeignKey<Payment>(p => p.OrderId);


            base.OnModelCreating(modelBuilder);

        }
    }
}