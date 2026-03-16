using AutoMapper;
using MediatR;
using Microservice.Ogrenci.Application.Contracts.IRepositories;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public class GetOgrencisQueryHandler(IMapper mapper,IOgrenciRepository ogrenciRepository) : IRequestHandler<GetOgrencisQuery, List<Domain.Entities.Ogrenci>>
    {
        public async Task<List<Domain.Entities.Ogrenci>> Handle(GetOgrencisQuery request, CancellationToken cancellationToken)
        {
            var personels = await ogrenciRepository.GetOgrencis();
            return personels;
        }
    }
}
