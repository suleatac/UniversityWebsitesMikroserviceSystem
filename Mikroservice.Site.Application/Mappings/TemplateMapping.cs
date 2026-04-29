using AutoMapper;
using Mikroservice.Site.Application.DTOs.TemplateDtos;

namespace Mikroservice.Site.Application.Mappings
{
    public class TemplateMapping : Profile
    {
        public TemplateMapping()
        {
            // READ
            CreateMap<Domain.Entities.Template, TemplateListDto>();
            CreateMap<Domain.Entities.Template, TemplateDetailDto>();
        }
    }
}
