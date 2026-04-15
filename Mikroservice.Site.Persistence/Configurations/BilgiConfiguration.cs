using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Persistence.Configurations
{
    internal class BilgiConfiguration : IEntityTypeConfiguration<Bilgi>
    {
        public void Configure(EntityTypeBuilder<Bilgi> builder)
        {


        }
    }
    
}
