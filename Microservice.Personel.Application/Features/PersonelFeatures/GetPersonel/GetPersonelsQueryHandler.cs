using AutoMapper;
using MediatR;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public class GetPersonelsQueryHandler(IMapper mapper,IPersonelRepository personelRepository) : IRequestHandler<GetPersonelsQuery, ServiceResult<List<Microservice.Personel.Domain.Entities.Personel>>>
    {
        public async Task<ServiceResult<List<Domain.Entities.Personel>>> Handle(GetPersonelsQuery request, CancellationToken cancellationToken)
        {
            var personels = await personelRepository.GetPersonels();
            return ServiceResult<List<Domain.Entities.Personel>>.SuccessAsOK(personels);
        }
    }
}
