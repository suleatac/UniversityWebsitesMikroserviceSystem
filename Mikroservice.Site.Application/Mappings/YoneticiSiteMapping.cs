using AutoMapper;
using Mikroservice.Site.Application.DTOs.YoneticiSiteDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class YoneticiSiteMapping : Profile
    {
        public YoneticiSiteMapping()
        {
            CreateMap<YoneticiSite, YoneticiSiteDto>();

            CreateMap<YoneticiSite, YoneticiSiteDetailDto>();
                
        }
    }
}