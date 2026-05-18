using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(256); // Genelde kullanıcı adları için 256 idealdir

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100); // Create, Update, Delete vb.

            builder.Property(x => x.EntityName)
                .HasMaxLength(256); // Tablo adı veya Entity adı

            builder.Property(x => x.EntityId)
                .HasMaxLength(100); // GUID veya ID'nin string hali

            builder.Property(x => x.Description)
                .HasMaxLength(2000); // Detaylı açıklama için geniş tutuldu

            builder.Property(x => x.IpAddress)
                .IsRequired()
                .HasMaxLength(50); // IPv6 desteği için 50 karakter yeterlidir

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()").HasColumnType("timestamp without time zone"); // PostgreSQL için varsayılan tarih

            // Indexler (Log tabloları hızlı büyür, sorgu performansını artırmak için kritik)
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Username);
            builder.HasIndex(x => x.CreatedAt);
            builder.HasIndex(x => x.EntityName);
            builder.HasIndex(x => x.EntityId);

            // Site bazlı filtreleme yapılacaksa indeks eklenmeli
            builder.HasIndex(x => x.SiteId);

            // Tablo Adı (Opsiyonel)
            builder.ToTable("AuditLogs");
        }
    }
}
