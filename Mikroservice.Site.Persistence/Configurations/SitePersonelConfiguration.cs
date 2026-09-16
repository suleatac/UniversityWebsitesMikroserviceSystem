using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SitePersonelConfiguration : IEntityTypeConfiguration<SitePersonel>
    {
        public void Configure(EntityTypeBuilder<SitePersonel> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonelId).IsRequired();
            builder.Property(x => x.UnvanId).IsRequired();
            builder.Property(x => x.PersonelTipId).IsRequired();
            builder.Property(x => x.SiteId).IsRequired();
            // =========================
            // SITE (ZORUNLU)
            // =========================
            builder.HasOne(x => x.Site)
                .WithMany(s => s.SitePersonels)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================
            // PageType (ZORUNLU)
            // =========================
            builder.HasOne(x => x.PageType)
              .WithMany(x => x.SitePersonels)
              .HasForeignKey(x => x.PageTypeId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.PageTypeId)
              .IsRequired();

            // =========================
            // SeoUrl (ZORUNLU)
            // =========================
            builder.Property(x => x.SeoUrl)
              .IsRequired()
              .HasMaxLength(300);

            // =========================
            // Telefon (ZORUNLU)
            // =========================
            builder.HasMany(x => x.PersonelTelefons)
                .WithOne(x => x.SitePersonel)
                .HasForeignKey(x => x.SitePersonelId)
                .OnDelete(DeleteBehavior.Restrict);
            // =========================
            // UNVAN (ZORUNLU)
            // =========================
            builder.HasOne(x => x.Unvan)
                .WithMany(u => u.SitePersonels)
                .HasForeignKey(x => x.UnvanId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // STRING ALANLAR
            // =========================
            builder.Property(x => x.ResimUrl).HasMaxLength(500);
            builder.Property(x => x.BlogAdress).HasMaxLength(250);
            builder.Property(x => x.TwitterAdress).HasMaxLength(250);
            builder.Property(x => x.FacebookAdress).HasMaxLength(250);
            builder.Property(x => x.InstagramAdress).HasMaxLength(250);

            // =========================
            // ENUM
            // =========================
            builder.HasOne(x => x.PersonelTip)
                .WithMany(pt => pt.SitePersonels)
                .HasForeignKey(x => x.PersonelTipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.SiteId, x.PersonelId }).IsUnique();



            // Partial unique index: sadece aktif (silinmemis) kayitlar icin benzersizlik kontrolu.
            // Soft delete edilen kayitlar Index'e dahil degil; boylece silinen kaydin SeoUrl'si yeniden kullanilabilir.
            builder.HasIndex(x => new { x.SiteId, x.SeoUrl })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = FALSE");




            // =========================
            // FILTER
            // =========================
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
