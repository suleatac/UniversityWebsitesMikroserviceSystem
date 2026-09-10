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

        }
    }
}
