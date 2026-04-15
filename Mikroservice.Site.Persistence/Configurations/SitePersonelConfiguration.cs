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

            // =========================
            // SITE (ZORUNLU)
            // =========================
            builder.HasOne(x => x.Site)
                .WithMany(s => s.SitePersonels)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Restrict);
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

            // =========================
            // FILTER
            // =========================
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
