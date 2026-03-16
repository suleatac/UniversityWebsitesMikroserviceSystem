namespace Microservice.Ogrenci.Application.Contracts.IRepositories
{
    public interface IUnitOfWork
    {
        Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    }
}
