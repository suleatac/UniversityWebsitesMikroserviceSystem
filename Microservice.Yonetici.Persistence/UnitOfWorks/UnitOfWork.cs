using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Microservice.Yonetici.Persistence.UnitOfWorks
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

        public async Task<IDbTransaction> BeginTransactionAsync(
           IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
           CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                return _transaction.GetDbTransaction(); // 🔥 tekrar açma
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


        public IExecutionStrategy GetExecutionStrategy()
        {
            return _dbContext.Database.CreateExecutionStrategy();
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
            _dbContext.Dispose();
        }


    }
}
