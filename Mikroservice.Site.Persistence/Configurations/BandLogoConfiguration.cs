using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class BandLogoConfiguration : IEntityTypeConfiguration<BandLogo>
    {
        public void Configure(EntityTypeBuilder<BandLogo> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.ImgUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Link)
                .HasMaxLength(500);

            builder.HasOne(x => x.Site)
                .WithMany(s => s.BandLogos)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Dil)
                .WithMany()
                .HasForeignKey(x => x.DilId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
