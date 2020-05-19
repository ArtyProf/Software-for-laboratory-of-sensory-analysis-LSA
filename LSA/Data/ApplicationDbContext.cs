using System;
using System.Collections.Generic;
using System.Text;
using LSA.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LSA.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Taster> Tasters{ get; set; }
        public DbSet<Tasting> Tastings { get; set; }
        public DbSet<TasterToTasting> TasterToTastings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductToTasting> ProductToTastings { get; set; }
        public DbSet<TastingHistory> TastingHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TasterToTasting>()
                .HasKey(bc => new { bc.TasterId, bc.TastingId });
            modelBuilder.Entity<TasterToTasting>()
                .HasOne(bc => bc.Taster)
                .WithMany(b => b.TasterToTastings)
                .HasForeignKey(bc => bc.TasterId);
            modelBuilder.Entity<TasterToTasting>()
                .HasOne(bc => bc.Tasting)
                .WithMany(c => c.TasterToTastings)
                .HasForeignKey(bc => bc.TastingId);

            modelBuilder.Entity<ProductToTasting>()
                .HasKey(bc => new { bc.ProductId, bc.TastingId });
            modelBuilder.Entity<ProductToTasting>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.ProductToTastings)
                .HasForeignKey(bc => bc.ProductId);
            modelBuilder.Entity<ProductToTasting>()
                .HasOne(bc => bc.Tasting)
                .WithMany(c => c.ProductToTastings)
                .HasForeignKey(bc => bc.TastingId);

            modelBuilder.Entity<Tasting>()
                .HasMany(c => c.TastingHistory)
                .WithOne(e => e.Tasting)
                .IsRequired();

            modelBuilder.Entity<Taster>()
                .HasMany(c => c.TastingHistory)
                .WithOne(e => e.Taster)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasMany(c => c.TastingHistory)
                .WithOne(e => e.Product)
                .IsRequired();
        }
    }
}
