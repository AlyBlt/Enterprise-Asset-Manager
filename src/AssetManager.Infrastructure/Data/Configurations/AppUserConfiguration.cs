using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AssetManager.Infrastructure.Data.Configurations;

// IEntityTypeConfiguration yerine BaseEntityConfiguration'dan miras alıyoruz
internal class AppUserConfiguration : BaseEntityConfiguration<AppUserEntity>
{
    public override void Configure(EntityTypeBuilder<AppUserEntity> builder)
    {
        // 1. BaseEntity kurallarını (Id, CreatedAt, IsDeleted Index/Filter) uygular
        base.Configure(builder);

        // 2. Kullanıcı Adı (Zorunlu, Max 50 Karakter ve Benzersiz)
        builder.Property(u => u.Username)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(u => u.Username)
               .IsUnique()
               .HasFilter("[IsDeleted] = 0");

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

        // 6. Rol (Enum -> String dönüşümü)
        builder.Property(u => u.Role)
               .HasConversion<string>()
               .HasMaxLength(20);

        // 7. İlişki Yapılandırması: AppUser -> Department
        builder.HasOne(u => u.Department)
               .WithMany(d => d.Users)
               .HasForeignKey(u => u.DepartmentId)
               .OnDelete(DeleteBehavior.Restrict);

        // NOT: builder.HasKey ve builder.HasQueryFilter silindi çünkü base içinde hallediliyor.
    }
}