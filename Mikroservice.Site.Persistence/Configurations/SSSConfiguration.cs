using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SSSConfiguration : IEntityTypeConfiguration<SikcaSorulanSoru>
    {
        public void Configure(EntityTypeBuilder<SikcaSorulanSoru> builder)
        {

            builder.HasKey(x => x.Id);

            // =========================
            // STRING
            // =========================
            builder.Property(x => x.Soru)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Cevap)
                .IsRequired();

            builder.Property(x => x.SeoUrl)
                .HasMaxLength(300);

            // =========================
            // SITE
            // =========================
            builder.HasOne(x => x.Site)
                .WithMany(s => s.SikcaSorulanSorus)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            // =========================
            // DIL
            // =========================
            builder.HasOne(x => x.Dil)
                .WithMany()
                .HasForeignKey(x => x.DilId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // KATEGORI
            // =========================
            builder.HasOne(x => x.Kategori)
                .WithMany(k => k.SikcaSorulanSorus)
                .HasForeignKey(x => x.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // INDEX
            // =========================
            builder.HasIndex(x => new { x.SiteId, x.DilId });

            builder.HasIndex(x => new { x.KategoriId, x.Sira });

            builder.HasIndex(x => x.SeoUrl);

            // =========================
            // FILTER
            // =========================
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
