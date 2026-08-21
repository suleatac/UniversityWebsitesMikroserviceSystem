using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageRouteConfiguration : IEntityTypeConfiguration<PageRoute>
    {
        public void Configure(EntityTypeBuilder<PageRoute> builder)
        {
            // Primary key
            builder.HasKey(pr => pr.Id);

            // Unique index: aynı site içinde slug benzersiz olsun
            builder.HasIndex(pr => new { pr.SiteId, pr.Slug })
                   .IsUnique();

            // Properties
            builder.Property(pr => pr.Slug)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(pr => pr.PageTypeId)
                   .IsRequired();

            builder.Property(pr => pr.SiteId)
                   .IsRequired();

            builder.Property(pr => pr.ContentId)
                   .IsRequired(false);

            builder.Property(pr => pr.IsActive)
                   .HasDefaultValue(true);

            builder.Property(pr => pr.CreatedAt)
                   .IsRequired();

            // Relationships

            // PageRoute -> Site (many-to-one). Site entity doesn't declare PageRoutes collection,
            // bu yüzden WithMany() parametresiz kullanıyoruz.
            builder.HasOne(pr => pr.Site)
                   .WithMany()
                   .HasForeignKey(pr => pr.SiteId)
                   .OnDelete(DeleteBehavior.Cascade);

            // PageRoute -> PageType (many-to-one)
            builder.HasOne(pr => pr.PageType)
                   .WithMany(pt => pt.PageRoutes)
                   .HasForeignKey(pr => pr.PageTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // One-to-one with SeoMetadata is configured in SeoMetadataConfiguration

            builder.HasOne(s => s.SeoMetadata)
                   .WithOne(pr => pr.PageRoute)
                   .HasForeignKey<PageRoute>(s => s.SeoMetadataId).IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);



        }
    }
}