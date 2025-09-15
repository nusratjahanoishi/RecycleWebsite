using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextUses.Models;

namespace NextUses.Data
{
    public class NextUsesDB : IdentityDbContext<Users>
    {
        public NextUsesDB(DbContextOptions<NextUsesDB> options) : base(options)
        {
        }

        // Identity User table
        public DbSet<Users> users { get; set; }

        // Custom tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        public DbSet<RiderApplication> RiderApplications { get; set; }
        public DbSet<GeneralSetting> GeneralSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Product -> Category
            builder.Entity<Product>()
                   .HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // ProductGallery -> Product
            builder.Entity<ProductGallery>()
                   .HasOne(pg => pg.Product)
                   .WithMany(p => p.GalImages)
                   .HasForeignKey(pg => pg.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Product -> User
            builder.Entity<Product>()
                   .HasOne(p => p.Users)
                   .WithMany(u => u.Products)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Billing -> Orders
            builder.Entity<Order>()
                   .HasOne(o => o.Billing)
                   .WithMany(b => b.Orders)
                   .HasForeignKey(o => o.BillingId)
                   .OnDelete(DeleteBehavior.Cascade);
            // Product -> Orders
            builder.Entity<Order>()
                   .HasOne(o => o.Product)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(o => o.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Billing -> User (Many-to-One)
            builder.Entity<Billing>()
                   .HasOne(b => b.Users)
                   .WithMany(u => u.Billings)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RiderApplication>()
            .HasOne(ra => ra.Order)
            .WithMany(o => o.RiderApplications)
            .HasForeignKey(ra => ra.OrderId)
            .OnDelete(DeleteBehavior.Restrict);



        }

    }
}
