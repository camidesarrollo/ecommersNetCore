using Ecommers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecommers.Infrastructure.Queries
{
    public class CommonQueries<TEntity> where TEntity : class
    {
        public static TEntity? GetByOrden(int orden)
        {
            using var db = new EcommersContext();
            var dbSet = db.Set<TEntity>();

            // Busca una propiedad llamada "SortOrder"
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, "SortOrder");
            var constant = Expression.Constant(orden);
            var equality = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

            return dbSet.FirstOrDefault(lambda);
        }

        public static int FindLastOrden()
        {
            using var db = new EcommersContext();
            var dbSet = db.Set<TEntity>();

            // Busca el máximo valor de SortOrder
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, "SortOrder");
            var conversion = Expression.Convert(property, typeof(int?));
            var lambda = Expression.Lambda<Func<TEntity, int?>>(conversion, parameter);

            return (dbSet.Max(lambda) ?? 0) + 1;
        }

        public static TEntity? GetById(int id)
        {
            using var db = new EcommersContext();
            return db.Set<TEntity>().Find(id);
        }

        public static List<TEntity> GetAll()
        {
            using var db = new EcommersContext();
            return db.Set<TEntity>().ToList();
        }

        public static TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = new EcommersContext();
            return db.Set<TEntity>().FirstOrDefault(predicate);
        }

        public static List<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            using var db = new EcommersContext();
            return db.Set<TEntity>().Where(predicate).ToList();
        }
    }
}