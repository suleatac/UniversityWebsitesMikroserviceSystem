using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class SertifikaParmakIziConfiguration : IEntityTypeConfiguration<SertifikaParmakIzi>
    {
        public void Configure(EntityTypeBuilder<SertifikaParmakIzi> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(b => b.SertifikaParmakIziNumarasi).IsRequired();
            builder.Property(o => o.SertifikaYili).HasColumnType("timestamp without time zone");
            // 1 Sertifika → N Site (opsiyonel kullanım)
            builder.HasMany(x => x.Sites)
                .WithOne(x => x.SertifikaParmakIzi)
                .HasForeignKey(x => x.SertifikaParmakIziId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
