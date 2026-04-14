using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class YoneticiSiteConfiguration : IEntityTypeConfiguration<YoneticiSite>
    {
        public void Configure(EntityTypeBuilder<YoneticiSite> builder)
        {
            // Composite Key
            builder.HasKey(x => new { x.YoneticiId, x.SiteId });

            // Yonetici ilişkisi
            builder.HasOne(x => x.Yonetici)
                .WithMany(x => x.YoneticiSites)
                .HasForeignKey(x => x.YoneticiId)
                .OnDelete(DeleteBehavior.Cascade);

            // Site ilişkisi
            builder.HasOne(x => x.Site)
                .WithMany(x => x.YoneticiSites)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // YoneticiTipi (role per site)
            builder.HasOne(x => x.YoneticiTipi)
                .WithMany()
                .HasForeignKey(x => x.YoneticiTipiId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
