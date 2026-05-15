using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    public class YonetimDuyuruOkunduConfiguration : IEntityTypeConfiguration<YonetimDuyuruOkundu>
    {
        public void Configure(EntityTypeBuilder<YonetimDuyuruOkundu> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.KeycloakUserId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(o => o.OkunmaTarihi)
                .HasColumnType("timestamp without time zone");

            builder.HasOne(o => o.YonetimDuyuru)
                .WithMany(d => d.OkunduBilgileri)
                .HasForeignKey(o => o.YonetimDuyuruId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(o => new { o.YonetimDuyuruId, o.KeycloakUserId }).IsUnique();
        }
    }
}
