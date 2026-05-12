using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class YoneticiSiteConfiguration : IEntityTypeConfiguration<YoneticiSite>
    {
        public void Configure(EntityTypeBuilder<YoneticiSite> builder)
        {
            // 🔥 Primary Key
            builder.HasKey(x => x.Id);

            // 🔐 Personel Id
            builder.Property(x => x.PersonelId)
                .IsRequired();

            // 🔗 Site ilişkisi
            builder.HasOne(x => x.Site)
                .WithMany(x => x.YoneticiSites)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔗 YoneticiTipi (role per site)
            builder.HasOne(x => x.YoneticiTipi)
                .WithMany()
                .HasForeignKey(x => x.YoneticiTipiId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧠 Soft delete index (opsiyonel ama önerilir)
            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
            builder.HasQueryFilter(b => !b.IsDeleted);
            builder.HasIndex(x => new { x.SiteId, x.PersonelId });
        }
    }
}
