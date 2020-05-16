using System;
using System.Collections.Generic;
using System.Text;
using LSA.Entity;
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
        }
    }
}
