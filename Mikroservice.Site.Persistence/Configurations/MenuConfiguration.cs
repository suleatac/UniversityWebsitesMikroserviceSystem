using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class MenuConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.HasKey(x => x.Id);

            // =========================
            // PROPERTIES
            // =========================
            builder.Property(x => x.Ad)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Link)
                .HasMaxLength(500);

            builder.Property(x => x.IconUrl)
                .HasMaxLength(300);

            builder.Property(x => x.Sira)
                .IsRequired();

            builder.Property(x => x.OlusturulmaTarihi)
                .IsRequired().HasColumnType("timestamp without time zone");
            builder.HasQueryFilter(b => !b.IsDeleted);
            // =========================
            // SITE
            // =========================
            builder.HasOne(x => x.Site)
                .WithMany(s => s.Menus)
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
            // HEDEF
            // =========================
            builder.HasOne(x => x.Hedef)
                .WithMany()
                .HasForeignKey(x => x.HedefId)
                .OnDelete(DeleteBehavior.SetNull);
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
            builder.HasIndex(x => new { x.SiteId, x.DilId });

            builder.HasIndex(x => new { x.ParentId, x.Sira });
        }
    }
}
