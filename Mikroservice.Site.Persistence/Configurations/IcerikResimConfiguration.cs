using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class IcerikResimConfiguration : IEntityTypeConfiguration<IcerikResim>
    {
        public void Configure(EntityTypeBuilder<IcerikResim> builder)
        {
            builder.ToTable("IcerikResim");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Baslik)
                .HasMaxLength(300);

            builder.Property(x => x.ResimUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(x => x.YuklemeTarihi)
                .HasDefaultValueSql("NOW()")
                .HasColumnType("timestamp without time zone");

            builder.Property(x => x.Sira)
                .HasDefaultValue(0);

            // Icerik (TPH koku) -> Resimler iliskisi
            builder.HasOne(x => x.Icerik)
                .WithMany(x => x.Resimler)
                .HasForeignKey(x => x.IcerikId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.IcerikId, x.Sira });

            // Soft delete: sorgularda yalnizca aktif resimler gorunsun
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
