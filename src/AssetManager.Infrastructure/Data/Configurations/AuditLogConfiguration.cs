using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditLogEntity> builder)
        {
            // 1. Primary Key
            builder.HasKey(l => l.Id);

            // 2. Kullanıcı Adı (İşlemi kimin yaptığını hızlıca görmek için)
            builder.Property(l => l.UserName)
                   .HasMaxLength(100);

            // 3. Action (Create, Update, Delete gibi kısa kelimeler)
            builder.Property(l => l.Action)
                   .IsRequired()
                   .HasMaxLength(20);

            // 4. EntityName (Asset, User vb. hangi tabloya dokunulduğu)
            builder.Property(l => l.EntityName)
                   .IsRequired()
                   .HasMaxLength(50);

            // 5. EntityId (İşlem gören satırın ID'si - String olması farklı tipler için esneklik sağlar)
            builder.Property(l => l.EntityId)
                   .IsRequired()
                   .HasMaxLength(50);

            // 6. Details (Değişikliklerin detayları - Uzun olabilir)
            builder.Property(l => l.Details)
                   .HasMaxLength(2000); // Çok detaylı loglar için geniş bir alan

            // 7. IP Adresi
            builder.Property(l => l.IpAddress)
                   .HasMaxLength(50);

            // 8. Zaman Damgası (Sıralama için indeks ekliyoruz)
            builder.Property(l => l.TimestampUtc)
                   .IsRequired();

            // Performans: Loglar genellikle zamana göre sorgulanır.
            builder.HasIndex(l => l.TimestampUtc);

            // Performans: Belirli bir varlığın geçmişini görmek için EntityName ve EntityId indeksi.
            builder.HasIndex(l => new { l.EntityName, l.EntityId });
        }
    }
}
