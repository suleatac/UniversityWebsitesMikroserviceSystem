using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mikroservice.Ogrenci.Application.Contracts;
using Mikroservice.Ogrenci.Persistence;
using System.Data;

namespace Mikroservice.Ogrenci.Infrastructure.Persistence.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;
        private readonly AppDbContext _dbContext;

        public IDbTransaction? Transaction => _transaction?.GetDbTransaction();

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> ExecuteInTransactionAsync<T>(
    Func<CancellationToken, Task<T>> operation,
    IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
    CancellationToken cancellationToken = default)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await BeginTransactionAsync(isolationLevel, cancellationToken);
                try
                {
                    var result = await operation(cancellationToken);
                    await CommitAsync(cancellationToken);
                    return result;
                }
                catch
                {
                    await RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
        public async Task<IDbTransaction> BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Zaten aktif bir transaction var.");
            }

            _transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return _transaction.GetDbTransaction();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Commit için transaction başlatılmamış.");
            }

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                return; // Nothing to rollback
            }

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            _dbContext.Dispose();
        }

    
    }
}