using AutoMapper;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Mappings
{
    public class AuditLogMapping : Profile
    {
        public AuditLogMapping()
        {
            CreateMap<AuditLog, AuditLogDto>();
            CreateMap<AuditLog, AuditLogDetailDto>();
        }
    }
}

