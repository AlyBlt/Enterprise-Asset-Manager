using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUserEntity>
    {
        public void Configure(EntityTypeBuilder<AppUserEntity> builder)
        {
            // 1. Primary Key Ayarı (BaseEntity'den geliyor)
            builder.HasKey(u => u.Id);

            // 2. Kullanıcı Adı (Zorunlu, Max 50 Karakter ve Benzersiz)
            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(u => u.Username)
                   .IsUnique(); // Aynı kullanıcı adından iki tane olamaz

            // 3. Tam Ad (Zorunlu, Max 100 Karakter)
            builder.Property(u => u.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            // 4. E-posta (Zorunlu, Max 150 Karakter)
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(150);

            // 5. Şifre Hash (Zorunlu)
            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            // 6. Rol (Enum Değerini Veritabanında String Olarak Tutmak Okunabilirliği Artırır)
            builder.Property(u => u.Role)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            // 7. İlişki Yapılandırması: AppUser -> Department
            // Bir kullanıcının bir departmanı olur (One), bir departmanın çok kullanıcısı olur (Many)
            builder.HasOne(u => u.Department)
                   .WithMany(d => d.Users)
                   .HasForeignKey(u => u.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict); // Departman silinirse kullanıcılar hata versin (Silinmesin)

            // 8. Soft Delete Filtresi (Bu entity için özel sorgu filtresi)
            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
