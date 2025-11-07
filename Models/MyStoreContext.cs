using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _24dh111520_LTW.Models
{
    public partial class MyStoreContext : DbContext
    {
        public MyStoreContext() { }

        public MyStoreContext(DbContextOptions<MyStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // ✅ Kết nối PostgreSQL
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MyStore;Username=postgres;Password=12345");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bảng User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);
                entity.ToTable("user"); // ✅ PostgreSQL tự động lowercase table name
                entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(255);
                entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(50);
                entity.Property(e => e.UserRole).HasColumnName("userrole").HasMaxLength(1);
            });

            // Bảng Customer
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.ToTable("customer");
                entity.Property(e => e.CustomerId).HasColumnName("customerid");
                entity.Property(e => e.CustomerName).HasColumnName("customername").HasMaxLength(255);
                entity.Property(e => e.CustomerPhone).HasColumnName("customerphone").HasMaxLength(15);
                entity.Property(e => e.CustomerEmail).HasColumnName("customeremail").HasMaxLength(255);
                entity.Property(e => e.CustomerAddress).HasColumnName("customeraddress").HasMaxLength(255);
                entity.Property(e => e.Username).HasColumnName("username");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("fk_user_customer");
            });

            // Bảng Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.ToTable("category");
                entity.Property(e => e.CategoryId).HasColumnName("categoryid");
                entity.Property(e => e.CategoryName).HasColumnName("categoryname").HasMaxLength(255);
            });

            // Bảng Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.ToTable("product");
                entity.Property(e => e.ProductId).HasColumnName("productid");
                entity.Property(e => e.CategoryId).HasColumnName("categoryid");
                entity.Property(e => e.ProductName).HasColumnName("productname").HasMaxLength(255);
                entity.Property(e => e.ProductDecription).HasColumnName("productdecription").HasMaxLength(1000);
                entity.Property(e => e.ProductPrice).HasColumnName("productprice");
                entity.Property(e => e.ProductImage).HasColumnName("productimage").HasMaxLength(255);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_pro_category");
            });

            // Bảng Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.ToTable("order");
                entity.Property(e => e.OrderId).HasColumnName("orderid");
                entity.Property(e => e.CustomerId).HasColumnName("customerid");
                entity.Property(e => e.OrderDate).HasColumnName("orderdate").HasColumnType("date");
                entity.Property(e => e.TotalAmount).HasColumnName("totalamount");
                entity.Property(e => e.PaymentStatus).HasColumnName("paymentstatus").HasMaxLength(50);
                entity.Property(e => e.AddressDelivery).HasColumnName("addressdelivery").HasMaxLength(255);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("fk_order_customer");
            });

            // Bảng OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("orderdetail");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ProductId).HasColumnName("productid");
                entity.Property(e => e.OrderId).HasColumnName("orderid");
                entity.Property(e => e.Quantity).HasColumnName("quantity");
                entity.Property(e => e.UnitPrice).HasColumnName("unitprice");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_orderdetail_product");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_orderdetail_order");
            });
        }
    }
}
