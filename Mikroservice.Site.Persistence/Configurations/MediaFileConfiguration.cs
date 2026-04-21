using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
    {
        public void Configure(EntityTypeBuilder<MediaFile> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Path).IsRequired().HasMaxLength(300);
            builder.Property(m => m.Url).IsRequired().HasMaxLength(300);
            builder.Property(m => m.CreatedAt).IsRequired().HasColumnType("timestamp without time zone");
            builder.HasOne(m => m.Site)
                .WithMany(s => s.MediaFiles)
                .HasForeignKey(m => m.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(m => m.Dil)
                .WithMany()
                .HasForeignKey(m => m.DilId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
