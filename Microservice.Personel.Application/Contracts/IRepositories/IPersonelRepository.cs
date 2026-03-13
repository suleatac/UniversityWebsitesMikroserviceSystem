using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Personel.Application.Contracts.IRepositories
{
    public interface IPersonelRepository
    {
        Task<List<Domain.Entities.Personel>> GetPersonels();
    }
}
