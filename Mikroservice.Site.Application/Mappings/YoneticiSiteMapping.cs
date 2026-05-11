using AutoMapper;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class YoneticiSiteMapping : Profile
    {
        public YoneticiSiteMapping()
        {
            CreateMap<YoneticiSite, YoneticiSiteDto>()
                .ForMember(dest => dest.YoneticiTipiAdi, opt => opt.MapFrom(src => src.YoneticiTipi != null ? src.YoneticiTipi.TipAdi : string.Empty));
            CreateMap<YoneticiSite, YoneticiSiteDetailDto>()
                .ForMember(dest => dest.YoneticiTipiAdi, opt => opt.MapFrom(src => src.YoneticiTipi != null ? src.YoneticiTipi.TipAdi : string.Empty));
        }
    }
}