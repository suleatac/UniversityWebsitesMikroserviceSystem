using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PopupConfiguration : IEntityTypeConfiguration<Popup>
    {
        public void Configure(EntityTypeBuilder<Popup> builder)
        {
            builder.HasKey(x => x.Id);

            // One-to-one relationship with Site
            builder.HasOne(p => p.Site)
                   .WithOne(s => s.Popup)
                   .HasForeignKey<Popup>(p => p.SiteId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.Property(x => x.KisaAciklama)
                .HasMaxLength(500);



            builder.Property(x => x.Link)
                .HasMaxLength(500);

            builder.Property(x => x.ResimUrl)
                .HasMaxLength(500);

            builder.Property(x => x.SeoUrl)
                .HasMaxLength(300);

            builder.Property(x => x.GosterimSayisi)
                .HasDefaultValue(0);

            builder.Property(x => x.EklemeTarihi).HasDefaultValueSql("NOW()").HasColumnType("timestamp without time zone");
            builder.Property(x => x.YayimTarihi).IsRequired().HasColumnType("timestamp without time zone");
            builder.Property(x => x.BaslamaTarihi).HasColumnType("timestamp without time zone");
            builder.Property(x => x.BitisTarihi).HasColumnType("timestamp without time zone");

            builder.Property(x => x.TamEkranMi)
                .HasDefaultValue(false);

            builder.Property(x => x.GosterimSuresiSaniye)
                .HasDefaultValue(5);

            builder.Property(x => x.CookieIleTekrarGosterme)
                .HasDefaultValue(true);

            // Soft delete filter
            builder.HasQueryFilter(b => !b.IsDeleted);

            // Unique index on SiteId (one-to-one)
            builder.HasIndex(x => x.SiteId).IsUnique();
        }
    }
}
