using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
   

            // =========================
            // PROPERTIES
            // =========================

            builder.Property(x => x.Sira)
                .IsRequired();

            // Location enum -> int olarak saklanir (MenuLocation)
            builder.Property(x => x.Location)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.IsVisible)
                .IsRequired()
                .HasDefaultValue(true);

            // =========================
            // SELF RELATION (TREE)
            // =========================
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            // 🔥 çok önemli: cascade olursa tüm menü silinir! Bu yüzden soft delete kullanıyoruz. Silme işlemi yaparken IsDeleted = true yapacağız. Böylece alt menüler silinmeyecek.

            // =========================
            // INDEX (PERFORMANS)
            // =========================
         
            builder.HasIndex(x => new { x.ParentId, x.Sira });

            // Footer/header menuleri site + dil + bolge bazinda sorgulanir
            builder.HasIndex(x => new { x.SiteId, x.DilId, x.Location });

        }
    }
}
