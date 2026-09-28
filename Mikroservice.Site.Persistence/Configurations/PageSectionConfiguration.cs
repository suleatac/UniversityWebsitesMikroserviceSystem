using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageSectionConfiguration : IEntityTypeConfiguration<PageSection>
    {
        public void Configure(EntityTypeBuilder<PageSection> builder)
        {
            builder.HasKey(x => x.Id);

            // =========================
            // PROPERTIES
            // =========================
            builder.Property(x => x.Baslik)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.BackgroundColor)
                .HasMaxLength(50);

            builder.Property(x => x.BackgroundImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Sira)
                .IsRequired();

            builder.Property(x => x.Yayinda)
                .HasDefaultValue(true);

            builder.Property(x => x.OlusturulmaTarihi)
                .IsRequired()
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone");

            // Soft delete filter
            builder.HasQueryFilter(s => !s.IsDeleted);

            // =========================
            // SITE
            // =========================
            builder.HasOne(x => x.Site)
                .WithMany()
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // DIL
            // =========================
            builder.HasOne(x => x.Dil)
                .WithMany()
                .HasForeignKey(x => x.DilId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // BLOCKS
            // =========================
            builder.HasMany(x => x.Blocks)
                .WithOne(b => b.PageSection)
                .HasForeignKey(b => b.PageSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // INDEX (PERFORMANS)
            // =========================
            builder.HasIndex(x => new { x.SiteId, x.DilId, x.Sira });
        }
    }
}
