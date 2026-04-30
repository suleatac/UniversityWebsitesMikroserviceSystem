using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SiteConfiguration :IEntityTypeConfiguration<Domain.Entities.Site>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Site> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.SiteAdi).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteAdiEng).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteUrl).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteAlanAdi).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteEPostaSifre).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteEPostaHost).IsRequired().HasMaxLength(200);
            builder.Property(s => s.SiteEPostaPort).IsRequired();
            builder.Property(s => s.SiteEPosta).IsRequired().HasMaxLength(200);
            builder.Property(s => s.TemplateId).IsRequired();

            builder.HasQueryFilter(b => !b.IsDeleted);
            

            builder.HasOne(s => s.Template)
                   .WithMany(t => t.Sites)
                   .HasForeignKey(s => s.TemplateId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Birim)
                   .WithMany(b => b.Sites)
                   .HasForeignKey(s => s.BirimId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.SiteOzellikleri)
                   .WithOne(so => so.Site)
                   .HasForeignKey<SiteOzellikleri>(so => so.SiteId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}