using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageBlockMediaConfiguration : IEntityTypeConfiguration<PageBlockMedia>
    {
        public void Configure(EntityTypeBuilder<PageBlockMedia> builder)
        {
            builder.HasKey(x => x.Id);

            // =========================
            // PROPERTIES
            // =========================
            builder.Property(x => x.ResimUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.VideoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Sira)
                .HasDefaultValue(0);

            // =========================
            // BLOCK
            // =========================
            builder.HasOne(x => x.PageBlock)
                .WithMany(b => b.Medias)
                .HasForeignKey(x => x.PageBlockId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // INDEX (PERFORMANS)
            // =========================
            builder.HasIndex(x => new { x.PageBlockId, x.Sira });
        }
    }
}
