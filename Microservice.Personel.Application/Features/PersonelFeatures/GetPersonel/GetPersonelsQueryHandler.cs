using AutoMapper;
using MediatR;
using Microservice.Personel.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public class GetPersonelsQueryHandler(IMapper mapper,IPersonelRepository personelRepository) : IRequestHandler<GetPersonelsQuery, List<Microservice.Personel.Domain.Entities.Personel>>
    {
        public async Task<List<Domain.Entities.Personel>> Handle(GetPersonelsQuery request, CancellationToken cancellationToken)
        {
            var personels = await personelRepository.GetPersonels();
            return personels;
        }
    }
}
