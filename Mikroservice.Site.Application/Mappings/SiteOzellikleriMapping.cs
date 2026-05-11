using AutoMapper;
using Mikroservice.Site.Application.DTOs.SiteOzellikleriDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class SiteOzellikleriMapping : Profile
    {
        public SiteOzellikleriMapping()
        {
            CreateMap<SiteOzellikleri, SiteOzellikleriDto>();
        }
    }
}