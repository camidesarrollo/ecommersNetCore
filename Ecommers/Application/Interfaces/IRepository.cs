using System.Linq.Expressions;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IRepository<TEntity, TId> where TEntity : BaseEntity<TId>
    {
        IQueryable<TEntity> GetQuery();
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
