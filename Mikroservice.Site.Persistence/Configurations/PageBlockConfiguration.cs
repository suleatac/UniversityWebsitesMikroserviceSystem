using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageBlockConfiguration : IEntityTypeConfiguration<PageBlock>
    {
        public void Configure(EntityTypeBuilder<PageBlock> builder)
        {
            builder.HasKey(x => x.Id);

            // =========================
            // PROPERTIES
            // =========================
            builder.Property(x => x.ContentType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Content)
                .HasColumnType("text");

            builder.Property(x => x.VideoUrl)
                .HasMaxLength(500);

            builder.Property(x => x.VideoType)
                .HasMaxLength(20);

            builder.Property(x => x.BackgroundImageUrl)
                .HasMaxLength(500);

            builder.Property(x => x.BackgroundColor)
                .HasMaxLength(50);

            builder.Property(x => x.Animation)
                .HasMaxLength(50);

            builder.Property(x => x.ColumnSize)
                .HasDefaultValue(12);

            builder.Property(x => x.RowNumber)
                .HasDefaultValue(0);

            // Soft delete filter
            builder.HasQueryFilter(b => !b.IsDeleted);

            // =========================
            // SECTION
            // =========================
            builder.HasOne(x => x.PageSection)
                .WithMany(s => s.Blocks)
                .HasForeignKey(x => x.PageSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // SELF REFERENCE (nested satirlar)
            // =========================
            builder.HasOne(x => x.Parent)
                .WithMany(b => b.Children)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // MEDIAS (carousel)
            // =========================
            builder.HasMany(x => x.Medias)
                .WithOne(m => m.PageBlock)
                .HasForeignKey(m => m.PageBlockId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // INDEX (PERFORMANS)
            // =========================
            builder.HasIndex(x => new { x.PageSectionId, x.ParentId, x.RowNumber });
        }
    }
}
