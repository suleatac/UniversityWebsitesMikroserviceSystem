using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SSSKategoriConfiguration : IEntityTypeConfiguration<SikcaSorulanSoruKategori>
    {
        public void Configure(EntityTypeBuilder<SikcaSorulanSoruKategori> builder)
        {
           builder.HasKey(x => x.Id);

            // =========================
            // PROPERTIES
            // =========================
            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Sira)
                .IsRequired();

            // =========================
            // RELATIONSHIP
            // =========================

            builder.HasMany(x => x.SikcaSorulanSorus)
                .WithOne(x => x.Kategori)
                .HasForeignKey(x => x.KategoriId)
                .OnDelete(DeleteBehavior.Restrict);

            // =========================
            // INDEX
            // =========================

            builder.HasIndex(x => x.Ad)
                .IsUnique(false);

            builder.HasIndex(x => x.Sira);

            // =========================
            // FILTER
            // =========================
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
