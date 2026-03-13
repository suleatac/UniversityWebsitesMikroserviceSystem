using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Personel.Api.RecurringJob
{
    public class PersonelMapping:Profile
    {
        public PersonelMapping()
        {
            CreateMap<Microservice.Personel.Domain.Entities.Personel, Microservice.Personel.Domain.Entities.Personel>().ReverseMap();
        }
    }
}
