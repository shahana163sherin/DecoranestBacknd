using DecoranestBacknd.DecoraNest.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace DecoranestBacknd.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
       public  DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        //Configure relationship and contraints here --> OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //User->cart--> One-to-many
           modelBuilder.Entity<Cart>()
                .HasOne(u=>u.User)
                .WithMany(c=>c.Carts)
                .HasForeignKey(u=>u.UserID);

            //User-->Wishlist-->one-to-many

            modelBuilder.Entity<Wishlist>()
                .HasOne(u => u.User)
                .WithMany(w => w.Wishlists)
                .HasForeignKey(u => u.UserID);

            //order->orderItems-->one-to-many

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(o=>o.Order)
                .HasForeignKey(o => o.OrderID);

            //cart-->product

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Product)
                .WithMany(c=>c.Carts)
                .HasForeignKey(c => c.ProductId);

            //Wishlist-->Product one-to-many

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Product)
                .WithMany(w=>w.Wishlists)
                .HasForeignKey(w => w.ProductId);

            modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

            modelBuilder.Entity<Cart>()
            .Property(c => c.TotalPrice)
            .HasPrecision(18, 2);


            modelBuilder.Entity<Wishlist>()
            .Property(c => c.TotalPrice)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
          .Property(c => c.TotalAmount)
         .HasPrecision(18, 2);


            modelBuilder.Entity<OrderItem>()
            .Property(c => c.Price)
            .HasPrecision(18, 2);




            base.OnModelCreating(modelBuilder);

        }
    }
}
