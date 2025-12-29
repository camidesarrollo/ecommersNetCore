using AutoMapper;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Ecommers.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementa el patrón Unit of Work para coordinar transacciones y repositorios
    /// </summary>
    public class UnitOfWork(EcommersContext context, IMapper mapper) : IUnitOfWork, IDisposable
    {
        private readonly EcommersContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly Hashtable _repositories = [];
        private bool _disposed = false;

        /// <summary>
        /// Obtiene un repositorio genérico para una entidad con ID genérico.
        /// Usa caché para evitar crear múltiples instancias del mismo repositorio.
        /// </summary>
        /// <typeparam name="TEntity">Tipo de entidad que hereda de BaseEntity</typeparam>
        /// <typeparam name="TId">Tipo del identificador (int, long, Guid, etc.)</typeparam>
        /// <returns>Instancia del repositorio para la entidad especificada</returns>
        public IRepository<TEntity, TId> Repository<TEntity, TId>()
            where TEntity : BaseEntity<TId>
        {
            // Crear clave única para el tipo de entidad y el tipo de ID
            var typeName = $"{typeof(TEntity).FullName}-{typeof(TId).FullName}";

            // Si no existe en caché, crear nueva instancia
            if (!_repositories.ContainsKey(typeName))
            {
                var repoInstance = new Repository<TEntity, TId>(_context, _mapper);
                _repositories.Add(typeName, repoInstance);
            }

            return (IRepository<TEntity, TId>)_repositories[typeName]!;
        }

        /// <summary>
        /// Guarda todos los cambios pendientes en todas las entidades rastreadas.
        /// </summary>
        /// <returns>Número de registros afectados</returns>
        public async Task<int> CompleteAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log y manejo de errores de concurrencia
                throw new InvalidOperationException("Error de concurrencia al guardar cambios", ex);
            }
            catch (DbUpdateException ex)
            {
                // Log y manejo de errores de base de datos
                throw new InvalidOperationException("Error al guardar cambios en la base de datos", ex);
            }
        }

        /// <summary>
        /// Inicia una transacción de base de datos explícita.
        /// Útil cuando necesitas control total sobre commit/rollback.
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            if (_context.Database.CurrentTransaction == null)
            {
                await _context.Database.BeginTransactionAsync();
            }
        }

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.CommitAsync();
            }
        }

        /// <summary>
        /// Revierte la transacción actual.
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.CurrentTransaction.RollbackAsync();
            }
        }

        /// <summary>
        /// Libera todos los recursos utilizados por el contexto.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implementación del patrón Dispose.
        /// </summary>
        /// <param name="disposing">True si se está llamando desde Dispose(), false si desde el finalizador</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Limpiar recursos administrados
                    _context.Dispose();
                    _repositories.Clear();
                }

                _disposed = true;
            }
        }
    }
}
