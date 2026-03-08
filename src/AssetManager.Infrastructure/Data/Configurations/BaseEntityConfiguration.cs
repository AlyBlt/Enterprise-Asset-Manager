using AssetManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssetManager.Infrastructure.Data.Configurations
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Tüm tablolarda Id birincil anahtar olsun
            builder.HasKey(x => x.Id);

            // CreatedAt alanı zorunlu olsun ve varsayılan olarak UTC şimdiki zamanı alsın
            builder.Property(x => x.CreatedAt)
                   .IsRequired();

            // IsDeleted alanı varsayılan olarak false olsun
            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasIndex(x => x.IsDeleted);

            // Soft-delete filtresi (Global Query Filter)
            // Bu sayede her sorguda .Where(x => !x.IsDeleted) yazmana gerek kalmaz!
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
