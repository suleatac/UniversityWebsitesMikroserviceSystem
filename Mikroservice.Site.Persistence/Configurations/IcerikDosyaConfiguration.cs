using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class IcerikDosyaConfiguration : IEntityTypeConfiguration<IcerikDosya>
    {
        public void Configure(EntityTypeBuilder<IcerikDosya> builder)
        {
            builder.ToTable("IcerikDosya");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Baslik)
                .HasMaxLength(300);

            builder.Property(x => x.DosyaUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.DosyaAdi)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.DosyaTuru)
                .HasMaxLength(20);

            builder.Property(x => x.YuklemeTarihi)
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone");

            builder.Property(x => x.Sira)
                .HasDefaultValue(0);

            // Icerik (TPH koku) -> Dosyalar iliskisi
            builder.HasOne(x => x.Icerik)
                .WithMany(x => x.Dosyalar)
                .HasForeignKey(x => x.IcerikId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir icerigin dosyalari site+dil bagimsiz, IcerikId uzerinden sorgulanir
            builder.HasIndex(x => new { x.IcerikId, x.Sira });

            // Soft delete: sorgularda yalnizca aktif dosyalar gorunsun
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
