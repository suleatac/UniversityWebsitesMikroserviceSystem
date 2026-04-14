using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class BirimConfiguration : IEntityTypeConfiguration<Birim>
    {
        public void Configure(EntityTypeBuilder<Birim> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasOne(b => b.Parent)
                   .WithMany(b => b.Children)
                   .HasForeignKey(b => b.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.Ad).IsRequired().HasMaxLength(200);
            builder.HasIndex(b => b.ParentId);
            builder.HasIndex(b => b.Sira);
            builder.HasIndex(b => b.Ad);
            builder.HasQueryFilter(b => !b.IsDeleted);
            // 1 Birim → N Site
            builder.HasMany(x => x.Sites)
                .WithOne(x => x.Birim)
                .HasForeignKey(x => x.BirimId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
