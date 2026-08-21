using AutoMapper;
using Mikroservice.Site.Application.DTOs.SiteDtos;

namespace Mikroservice.Site.Application.Mappings
{
    public class PageRouteMapping : Profile
    {
        public PageRouteMapping()
        {
            CreateMap<Domain.Entities.Site, PageRouteDto>();

        }
    }
}
