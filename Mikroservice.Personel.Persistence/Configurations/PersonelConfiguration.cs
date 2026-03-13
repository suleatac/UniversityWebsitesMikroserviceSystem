using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Personel.Persistence.Configurations
{
    public class PersonelConfiguration : IEntityTypeConfiguration<Microservice.Personel.Domain.Entities.Personel>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Microservice.Personel.Domain.Entities.Personel> builder)
        {
            builder.HasKey(o => o.id);
            builder.Property(o => o.personid).IsRequired();
            builder.Property(o => o.tcnumarasi).IsRequired();
            builder.Property(o => o.adi).IsRequired();
            builder.Property(o => o.soyadi).IsRequired();
            builder.Property(o => o.uyruk).IsRequired();
            builder.Property(o => o.cinsiyeti).IsRequired();
            builder.Property(o => o.babaadi).IsRequired();
            builder.Property(o => o.anaadi).IsRequired();
            builder.Property(o => o.dogumyeri).IsRequired();
            builder.Property(o => o.dogumtarihi).IsRequired();
            builder.Property(o => o.kadrotipi).IsRequired();
            builder.Property(o => o.kadrounvan).IsRequired();
            builder.Property(o => o.personencryptedid).IsRequired();
        }
    }
}
