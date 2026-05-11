using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class UnvanConfiguration:IEntityTypeConfiguration<Unvan>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Unvan> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.KisaAd)
                .HasMaxLength(50);

            builder.Property(x => x.Sira)
                .IsRequired();
            builder.HasQueryFilter(b => !b.IsDeleted);

            // 1 Template → N Site
            builder.HasOne(x => x.PersonelTip)
                .WithMany(x => x.Unvans)
                .HasForeignKey(x => x.TipId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Parent)
                 .WithMany(b => b.Children)
                 .HasForeignKey(b => b.ParentId)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(b => b.ParentId);
            builder.HasIndex(b => b.Sira);
            builder.HasIndex(b => b.Ad);
            builder.HasIndex(b => b.KisaAd);

        }
    }
}
