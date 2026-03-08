using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
    internal class DepartmentConfiguration : IEntityTypeConfiguration<DepartmentEntity>
    {
        public void Configure(EntityTypeBuilder<DepartmentEntity> builder)
        {
            // 1. Primary Key
            builder.HasKey(d => d.Id);

            // 2. Departman Adı (Zorunlu, Max 100 Karakter)
            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Departman adının benzersiz olması raporlama karışıklığını önler
            builder.HasIndex(d => d.Name)
                   .IsUnique();

            // 3. Açıklama (Max 500 Karakter, Zorunlu Değil)
            builder.Property(d => d.Description)
                   .HasMaxLength(500);

            // 4. İlişki Yapılandırması: Department -> AppUser
            // Bir departmanın birçok kullanıcısı olabilir.
            // İlişki zaten AppUserConfiguration'da tanımlanmış olabilir ama 
            // burada "Principal" tarafı (Department) üzerinden de teyit ediyoruz.
            builder.HasMany(d => d.Users)
                   .WithOne(u => u.Department)
                   .HasForeignKey(u => u.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);
            // Departmanda kullanıcı varsa departmanın silinmesini engelleriz.

            // 5. Soft Delete Filtresi
            builder.HasQueryFilter(d => !d.IsDeleted);
        }
    }
}
