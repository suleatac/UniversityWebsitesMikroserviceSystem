using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class IcerikConfiguration : IEntityTypeConfiguration<Icerik>
    {
        public void Configure(EntityTypeBuilder<Icerik> builder)
        {

            builder.HasKey(x => x.Id);

            // Discriminator
            builder.HasDiscriminator<IcerikTip>("Tip")
                .HasValue<Haber>(IcerikTip.Haber)
                .HasValue<Duyuru>(IcerikTip.Duyuru)
                .HasValue<Bilgi>(IcerikTip.Bilgi)
                .HasValue<Etkinlik>(IcerikTip.Etkinlik)
                .HasValue<Video>(IcerikTip.Video)
                .HasValue<Banner>(IcerikTip.Banner);

            // =========================
            // COMMON PROPERTIES
            // =========================
            builder.Property(x => x.Baslik)
                .IsRequired()
                .HasMaxLength(300);

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



            // =========================
            // INDEX
            // =========================
            builder.HasIndex(x => new { x.SiteId, x.DilId });

            builder.HasIndex(x => new { x.SiteId, x.SeoUrl })
                .IsUnique();
            // =========================
            // FILTER
            // =========================

            builder.HasQueryFilter(b => !b.IsDeleted);

        }
    }
}
