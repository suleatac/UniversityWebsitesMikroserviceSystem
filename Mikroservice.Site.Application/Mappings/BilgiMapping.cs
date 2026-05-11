using AutoMapper;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class BilgiMapping : Profile
    {
        public BilgiMapping()
        {
            CreateMap<Bilgi, BilgiDto>();
            CreateMap<Bilgi, BilgiDetailDto>();
        }
    }
}