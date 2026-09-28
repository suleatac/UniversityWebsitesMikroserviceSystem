using AutoMapper;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class PageSectionMapping : Profile
    {
        public PageSectionMapping()
        {
            // READ
            CreateMap<PageSection, PageSectionDto>();
            CreateMap<PageBlock, PageBlockDto>()
                .ForMember(d => d.Children, opt => opt.MapFrom(s => s.Children));
            CreateMap<PageBlockMedia, PageBlockMediaDto>();

            // WRITE (media input -> entity)
            CreateMap<PageBlockMediaInputDto, PageBlockMedia>();
        }
    }
}
