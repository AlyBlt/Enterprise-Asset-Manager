using AssetManager.Core.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
