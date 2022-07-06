using Microsoft.EntityFrameworkCore;

namespace API_Alluring.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        #region Db set
        public DbSet<RefreshToken> RefreshToken { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }


        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductVariant> ProductVariants { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Discount> Discount { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasKey(o => o.OrderId);
                entity.Property(o => o.OrderDate).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(o => o.Phone).HasMaxLength(11).HasColumnType("VARCHAR(11)");
                entity.Property(o => o.MethodPay).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");
                entity.HasKey(od => new { od.OrderId, od.ProductId, od.ProductVariantId });

                entity.HasOne(od => od.Order)
                    .WithMany(od => od.OrderDetails)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(od => od.OrderId)
                    .HasConstraintName("FK_OrderDetail_Order");
                entity.HasOne(od => od.Product)
                    .WithMany(od => od.OrderDetails)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasForeignKey(od => od.ProductId)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.Property(u => u.UserName).HasMaxLength(50).HasColumnType("VARCHAR(50)");
                entity.Property(u => u.Email).HasMaxLength(100).HasColumnType("VARCHAR(100)");
                entity.Property(u => u.Role).HasMaxLength(20).HasColumnType("VARCHAR(20)");
                entity.Property(u => u.IsEmailVerified).HasDefaultValueSql("0");
                entity.Property(u => u.Password).HasColumnType("VARCHAR(MAX)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
                entity.Property(c => c.Address).HasMaxLength(150);
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(c => c.Phone).HasMaxLength(11).HasColumnType("VARCHAR(11)");
                entity.Property(c => c.Birth).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.Property(p => p.ProductName).HasMaxLength(50);
                entity.Property(p => p.isShow).HasDefaultValueSql("1");
                entity.Property(p => p.Gender).HasDefaultValueSql("1");
                entity.HasOne(p => p.Category)
                    .WithMany(p => p.Product)
                    .HasForeignKey(p => p.CategoryId)
                    .HasConstraintName("FK_Product_Category");
                entity.HasOne(p => p.Discount)
                    .WithMany(p => p.Product)
                    .HasForeignKey(p => p.DiscountId)
                    .HasConstraintName("FK_Product_Discount");
            });

            modelBuilder.Entity<ProductVariant>(entity =>
            {
                entity.ToTable("ProductVariant");
                entity.Property(p => p.colorName).HasMaxLength(50);
                entity.HasOne(p => p.Product)
                    .WithMany(p => p.ProductVariant)
                    .HasForeignKey(p => p.ProductId)
                    .HasConstraintName("FK_ProductVariant_Product");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");
                entity.Property(c => c.DiscountName).HasMaxLength(50);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.Property(c => c.CategoryName).HasMaxLength(50);
            });
        }

    }
}
