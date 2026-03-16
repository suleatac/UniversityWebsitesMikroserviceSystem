using AutoMapper;

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
