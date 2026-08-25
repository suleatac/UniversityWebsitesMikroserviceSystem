using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageTypeConfiguration : IEntityTypeConfiguration<PageType>
    {
        public void Configure(EntityTypeBuilder<PageType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.PageTypeId)
                .HasConversion<int>()
                .IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ViewName).HasMaxLength(200);

            builder.HasOne(x => x.Dil)
                .WithMany()
                .HasForeignKey(x => x.DilId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Site)
                .WithMany(x => x.PageTypes)
                .HasForeignKey(x => x.SiteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Template)
                .WithMany(x => x.PageTypes)
                .HasForeignKey(x => x.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.SiteId, x.DilId, x.Slug }).IsUnique();
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}