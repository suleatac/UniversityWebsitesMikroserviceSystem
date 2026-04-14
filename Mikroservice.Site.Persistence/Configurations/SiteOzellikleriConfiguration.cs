using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SiteOzellikleriConfiguration : IEntityTypeConfiguration<SiteOzellikleri>
    {
        public void Configure(EntityTypeBuilder<SiteOzellikleri> builder)
        {
            builder.HasKey(so => so.Id);
            builder.Property(so => so.SiteTelNo).HasMaxLength(20);
            builder.Property(so => so.SiteFaxNo).HasMaxLength(20);
            builder.HasOne(so => so.Site)
                   .WithOne(s => s.SiteOzellikleri)
                   .HasForeignKey<SiteOzellikleri>(so => so.SiteId)
                   .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
