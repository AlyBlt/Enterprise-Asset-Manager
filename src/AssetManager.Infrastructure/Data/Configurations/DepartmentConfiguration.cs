using AssetManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManager.Infrastructure.Data.Configurations;

internal class DepartmentConfiguration : BaseEntityConfiguration<DepartmentEntity>
{
    public override void Configure(EntityTypeBuilder<DepartmentEntity> builder)
    {
        // 1. BaseEntity'den gelen Id, CreatedAt, IsDeleted Index/Filter kurallarını yükle
        base.Configure(builder);

        // 2. Departman Adı (Zorunlu, Max 100 Karakter ve Benzersiz)
        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(d => d.Name)
               .IsUnique()
               .HasFilter("[IsDeleted] = 0");

        // 3. Açıklama (Max 500 Karakter)
        builder.Property(d => d.Description)
               .HasMaxLength(500);

        // 4. İlişki Yapılandırması: Department -> AppUser
        // Kullanıcılar bu departmana bağlıyken departmanın silinmesini (Restrict) engelliyoruz
        builder.HasMany(d => d.Users)
               .WithOne(u => u.Department)
               .HasForeignKey(u => u.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

    }
}