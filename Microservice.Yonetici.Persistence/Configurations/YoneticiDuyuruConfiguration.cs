using Microservice.Yonetici.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservice.Yonetici.Persistence.Configurations
{
    public class YoneticiDuyuruConfiguration : IEntityTypeConfiguration<YoneticiDuyuru>
    {
        public void Configure(EntityTypeBuilder<YoneticiDuyuru> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Baslik).HasMaxLength(200).IsRequired();
            builder.Property(o => o.Icerik).IsRequired();
            builder.Property(o => o.EklenmeTarihi).HasColumnType("timestamp without time zone");
            builder.Property(o => o.Aktif).IsRequired();
        }
    }
}
