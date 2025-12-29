using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Obtiene un repositorio para la entidad especificada
        /// </summary>
        IRepository<TEntity, TId> Repository<TEntity, TId>()
            where TEntity : BaseEntity<TId>;


        /// <summary>
        /// Guarda todos los cambios pendientes
        /// </summary>
        Task<int> CompleteAsync();

        /// <summary>
        /// Inicia una transacción explícita
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Confirma la transacción actual
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// Revierte la transacción actual
        /// </summary>
        Task RollbackTransactionAsync();
    }
}
