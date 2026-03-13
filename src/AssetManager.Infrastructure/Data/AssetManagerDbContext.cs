using AssetManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssetManager.Infrastructure.Data
{
    internal class AssetManagerDbContext : DbContext
    {
        public AssetManagerDbContext(DbContextOptions<AssetManagerDbContext> options) : base(options) { }

        public DbSet<AssetEntity> Assets { get; set; }
        public DbSet<AppUserEntity> Users { get; set; }
        public DbSet<DepartmentEntity> Departments { get; set; }
        public DbSet<AuditLogEntity> AuditLogs { get; set; }

        
        // Tüm tablolardaki tarihleri merkezi yönetmek için
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    // CreatedAt'in güncellenmesini kesinlikle engelliyoruz
                    entry.Property(x => x.CreatedAt).IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
