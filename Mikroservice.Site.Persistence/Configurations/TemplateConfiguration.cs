using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(b => b.TemplateAdi).IsRequired().HasMaxLength(200);
            builder.Property(b => b.TemplateTuru).IsRequired().HasMaxLength(100);
            builder.Property(b => b.FolderName).IsRequired().HasMaxLength(300);
            builder.Property(b => b.LayoutPath).IsRequired().HasMaxLength(300);
            // 1 Template → N Site
            builder.HasMany(x => x.Sites)
                .WithOne(x => x.Template)
                .HasForeignKey(x => x.TemplateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
