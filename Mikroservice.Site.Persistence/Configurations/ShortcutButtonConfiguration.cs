using Microsoft.EntityFrameworkCore;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class ShortcutButtonConfiguration:IEntityTypeConfiguration<ShortcutButton>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShortcutButton> builder) 
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

            builder.Property(x => x.ImageUrl)
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
                .WithMany(s => s.ShortcutButtons)
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
            // INDEX (PERFORMANS)
            // =========================
            builder.HasIndex(x => new { x.SiteId, x.DilId });
        }
        
    }
}
