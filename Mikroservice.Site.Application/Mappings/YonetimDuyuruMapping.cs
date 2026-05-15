using AutoMapper;
using Microservice.Site.Domain.Entities;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Mikroservice.Site.Application.Mappings
{
    public class YonetimDuyuruMapping : Profile
    {
        public YonetimDuyuruMapping()
        {
            // READ
            CreateMap<YonetimDuyuru, YonetimDuyuruDto>();
            CreateMap<YonetimDuyuru, YonetimDuyuruDetailDto>();
        }
    }
}
