using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class PageTypeConfiguration : IEntityTypeConfiguration<PageType>
    {
        public void Configure(EntityTypeBuilder<PageType> builder)
        {
            // Primary key
            builder.HasKey(pt => pt.Id);

            // Properties
            builder.Property(pt => pt.Code)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(pt => pt.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            // Unique constraint on Code
            builder.HasIndex(pt => pt.Code)
                   .IsUnique();

            // Relationships
            builder.HasMany(pt => pt.PageRoutes)
                   .WithOne(pr => pr.PageType)
                   .HasForeignKey(pr => pr.PageTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}