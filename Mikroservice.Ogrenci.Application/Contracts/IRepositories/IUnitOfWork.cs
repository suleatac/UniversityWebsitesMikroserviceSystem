using System.Data;

namespace Microservice.Ogrenci.Application.Contracts.IRepositories
{
    public interface IUnitOfWork
    {
        IDbTransaction? Transaction { get; }
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
             CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
