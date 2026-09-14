using AutoMapper;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class PageTypeMapping : Profile
    {
        public PageTypeMapping()
        {
            // READ
            CreateMap<PageType, PageTypeDto>();
        }
    }
}
