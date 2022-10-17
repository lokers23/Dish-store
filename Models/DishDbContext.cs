using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DishStore.Models
{
    public partial class DishDbContext : DbContext
    {
        public DishDbContext()
        {
        }

        public DishDbContext(DbContextOptions<DishDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Dish> Dishes { get; set; } = null!;
        public virtual DbSet<DishOrder> DishOrders { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-GCRTC60\\SQLEXPRESS;Initial Catalog=DishDb;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Material).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Dishes_Categories");

                entity.HasOne(d => d.Manufacturer)
                    .WithMany(p => p.Dishes)
                    .HasForeignKey(d => d.ManufacturerId)
                    .HasConstraintName("FK_Dishes_Manufacturers");
            });

            modelBuilder.Entity<DishOrder>(entity =>
            {
                entity.HasKey(e => new { e.DishId, e.OrderId });

                entity.HasOne(d => d.Dish)
                    .WithMany(p => p.DishOrders)
                    .HasForeignKey(d => d.DishId)
                    .HasConstraintName("FK_DishOrders_Dishes");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DishOrders)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_DishOrders_Orders");
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.Property(e => e.Country).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.DateOrder).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PaymentMethod).HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Login).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
