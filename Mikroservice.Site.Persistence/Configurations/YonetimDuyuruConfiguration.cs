using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Site.Persistence.Configurations
{
    public class YonetimDuyuruConfiguration : IEntityTypeConfiguration<YonetimDuyuru>
    {
        public void Configure(EntityTypeBuilder<YonetimDuyuru> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Baslik).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Icerik).IsRequired();
            builder.Property(o => o.EklenmeTarihi).HasColumnType("timestamp without time zone");
            builder.Property(o => o.Aktif).IsRequired();
        }
    }
}
