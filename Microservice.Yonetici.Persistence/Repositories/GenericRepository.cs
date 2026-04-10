using Microservice.Yonetici.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Microservice.Yonetici.Persistence.Repositories
{
    public class GenericRepository<T>(AppDbContext appDbContext) : IGenericRepository<T> where T : class
    {
        private DbSet<T> _dbSet => appDbContext.Set<T>();
        public async ValueTask AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public ValueTask<T?> GetByIdAsync(int id)
        {
            return _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}
