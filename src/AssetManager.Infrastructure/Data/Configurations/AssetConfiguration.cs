using AssetManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
  
    internal class AssetConfiguration : BaseEntityConfiguration<AssetEntity>
    {
        public override void Configure(EntityTypeBuilder<AssetEntity> builder)
        {
            // 1. BaseEntity içindeki Id, CreatedAt, IsDeleted kurallarını uygular
            base.Configure(builder);

            // 2. Varlık Adı (Zorunlu, Max 200 Karakter)
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(a => a.Name)
                   .IsUnique()
                   .HasFilter("[IsDeleted] = 0");

            // 3. Kategori (Max 50 Karakter)
            builder.Property(a => a.Category)
                   .IsRequired()
                   .HasMaxLength(50);

            // 4. Seri Numarası (Zorunlu ve Benzersiz)
            builder.Property(a => a.SerialNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(a => a.SerialNumber)
                   .IsUnique();
                  
            // 5. Maddi Değer
            builder.Property(a => a.Value)
                   .HasPrecision(18, 2);

            // 6. İlişki Yapılandırması: Asset -> AppUser
            builder.HasOne(a => a.AssignedUser)
                   .WithMany()
                   .HasForeignKey(a => a.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            // 7. Status Enum Dönüşümü
            builder.Property(a => a.Status)
                   .HasConversion<string>();

        }
    }
}
