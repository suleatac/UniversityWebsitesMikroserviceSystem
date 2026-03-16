using AutoMapper;
using MediatR;
using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microservice.Shared;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public class GetOgrencisQueryHandler(IMapper mapper,IOgrenciRepository ogrenciRepository) : IRequestHandler<GetOgrencisQuery, ServiceResult<List<Domain.Entities.Ogrenci>> >
    {
        public async Task<ServiceResult<List<Domain.Entities.Ogrenci>>> Handle(GetOgrencisQuery request, CancellationToken cancellationToken)
        {
            var ogrencis = await ogrenciRepository.GetOgrencis();
            return ServiceResult<List<Domain.Entities.Ogrenci>>.SuccessAsOK(ogrencis);
          
        }
    }
}
