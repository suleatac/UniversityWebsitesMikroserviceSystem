using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class GaleriResimConfiguration : IEntityTypeConfiguration<GaleriResim>
    {
        public void Configure(EntityTypeBuilder<GaleriResim> builder)
        {
            builder.Property(x => x.Kategori).HasMaxLength(150);

            // Liste sayfasindaki kategori filtreleri ve iliskili resimler
            // bu indeks uzerinden sorgulanir.
            builder.HasIndex(x => new { x.SiteId, x.DilId, x.Kategori });
        }
    }
}
