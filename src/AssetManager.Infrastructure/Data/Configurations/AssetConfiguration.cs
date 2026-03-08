using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
    internal class AssetConfiguration : IEntityTypeConfiguration<AssetEntity>
    {
        public void Configure(EntityTypeBuilder<AssetEntity> builder)
        {
            // 1. Primary Key
            builder.HasKey(a => a.Id);

            // 2. Varlık Adı (Zorunlu, Max 200 Karakter)
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // 3. Kategori (Max 50 Karakter)
            builder.Property(a => a.Category)
                   .IsRequired()
                   .HasMaxLength(50);

            // 4. Seri Numarası (Zorunlu ve Benzersiz olması takibi kolaylaştırır)
            builder.Property(a => a.SerialNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(a => a.SerialNumber)
                   .IsUnique(); // Aynı seri numarasına sahip iki farklı kayıt olamaz

            // 5. Maddi Değer (Decimal hassasiyeti: 18 basamak, virgülden sonra 2 basamak)
            builder.Property(a => a.Value)
                   .HasPrecision(18, 2);

            // 6. İlişki Yapılandırması: Asset -> AppUser
            // Bir varlık bir kullanıcıya zimmetlenebilir. 
            // Kullanıcı silindiğinde varlık silinmesin (SetNull), zimmet boşa çıksın.
            builder.HasOne(a => a.AssignedUser)
                   .WithMany() // Bir kullanıcının birden fazla zimmetli varlığı olabilir
                   .HasForeignKey(a => a.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            // 7. Soft Delete Filtresi
            builder.HasQueryFilter(a => !a.IsDeleted);

            builder.Property(a => a.Status)
                   .HasConversion<string>(); // Veritabanında string olarak ("Active") saklar
        }
    }
}
