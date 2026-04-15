using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class HaberConfiguration : IEntityTypeConfiguration<Haber>
    {
        public void Configure(EntityTypeBuilder<Haber> builder)
        {

          
        }
    }
}