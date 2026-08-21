using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SeoMetadataConfiguration : IEntityTypeConfiguration<SeoMetadata>
    {
        public void Configure(EntityTypeBuilder<SeoMetadata> builder)
        {
            // Primary key
            builder.HasKey(s => s.Id);

            // Scalar properties - uzunluk ve gerekli/opsiyonel ayarları
            builder.Property(s => s.MetaTitle)
                   .HasMaxLength(200);

            builder.Property(s => s.MetaDescription)
                   .HasMaxLength(1000);

            builder.Property(s => s.MetaKeywords)
                   .HasMaxLength(1000);

            builder.Property(s => s.CanonicalUrl)
                   .HasMaxLength(500);

            builder.Property(s => s.Robots)
                   .HasMaxLength(50);

            builder.Property(s => s.OgTitle)
                   .HasMaxLength(200);

            builder.Property(s => s.OgDescription)
                   .HasMaxLength(1000);

            builder.Property(s => s.OgImage)
                   .HasMaxLength(1000);

            // Booleans için default değer atama (isteğe göre değiştirilebilir)
            builder.Property(s => s.IsIndexable)
                   .HasDefaultValue(true);

            builder.Property(s => s.IsFollowable)
                   .HasDefaultValue(true);
        }
    }
}