using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _24dh111520_LTW.Models;

public partial class MyStoreContext : DbContext
{
    public MyStoreContext()
    {
    }

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
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=MyStore;User Id=sa;Password=Quan12345@;TrustServerCertificate=True;"
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Bảng User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username);
            entity.ToTable("User");
            entity.Property(e => e.Username).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserRole).HasMaxLength(1);
        });

        // Bảng Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);
            entity.ToTable("Customer");
            entity.Property(e => e.CustomerName).HasMaxLength(255);
            entity.Property(e => e.CustomerPhone).HasMaxLength(15);
            entity.Property(e => e.CustomerEmail).HasMaxLength(255);
            entity.Property(e => e.CustomerAddress).HasMaxLength(255);

            entity.HasOne(d => d.UsernameNavigation)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.Username)
                .HasConstraintName("FK_User_Customer");
        });

        // Bảng Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);
            entity.ToTable("Category");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
        });

        // Bảng Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.ToTable("Product");
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductDecription).HasMaxLength(1000);
            entity.Property(e => e.ProductImage).HasMaxLength(255);

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Pro_Category");
        });

        // Bảng Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.ToTable("Order");
            entity.Property(e => e.AddressDelivery).HasMaxLength(255);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.OrderDate).HasColumnType("date");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_Customer");
        });

        // Bảng OrderDetail
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("OrderDetail");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetail_Product");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
