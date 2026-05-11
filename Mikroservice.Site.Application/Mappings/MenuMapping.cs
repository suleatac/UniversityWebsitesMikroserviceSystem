using AutoMapper;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class MenuMapping : Profile
    {
        public MenuMapping()
        {
            // READ
            CreateMap<Menu, MenuDetailDto>();
        }
    }
}