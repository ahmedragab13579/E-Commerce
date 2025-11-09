using Application.Services.InterFaces.Humans;
using E_Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace E_Infrastructure.Data
{
    public class E_ApplicationDbContext: DbContext
    {

        public E_ApplicationDbContext(DbContextOptions<E_ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<ProductRecommendation> ProductRecommendations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<ProductRecommendation>()
                .HasOne(pr => pr.Product) 
                .WithMany(p => p.PrimaryRecommendations) 
                .HasForeignKey(pr => pr.ProductId) 
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<ProductRecommendation>()
                .HasOne(pr => pr.RecommendedProduct) 
                .WithMany(p => p.RecommendedAs) 
                .HasForeignKey(pr => pr.RecommendedProductId) 
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
