using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Microservice.Site.Application.Contracts.IRepositories
{
    public interface IUnitOfWork
    {
        IDbTransaction? Transaction { get; }
        Task<IDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        IExecutionStrategy GetExecutionStrategy();
    }
}
