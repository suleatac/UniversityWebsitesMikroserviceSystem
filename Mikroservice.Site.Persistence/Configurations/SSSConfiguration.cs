using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

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
            // INDEX
            // =========================
            builder.HasIndex(x => new { x.SiteId, x.DilId });
            builder.HasIndex(x => new { x.ParentId, x.Sira });
            builder.HasIndex(x => x.SeoUrl);

            // =========================
            // SELF RELATION (TREE)
            // =========================
            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            // 🔥 çok önemli: cascade olursa tüm menü silinir! Bu yüzden soft delete kullanıyoruz. Silme işlemi yaparken IsDeleted = true yapacağız. Böylece alt menüler silinmeyecek.


            // =========================
            // FILTER
            // =========================
            builder.HasQueryFilter(b => !b.IsDeleted);
        }
    }
}
