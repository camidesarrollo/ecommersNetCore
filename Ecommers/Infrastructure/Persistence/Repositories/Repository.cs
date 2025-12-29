using AutoMapper;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Ecommers.Infrastructure.Persistence.Repositories
{
    public class Repository<TEntity, TId>(EcommersContext context, IMapper mapper) : IRepository<TEntity, TId>
            where TEntity : BaseEntity<TId>
    {
        protected readonly EcommersContext _context = context;
        protected readonly IMapper _mapper = mapper;

        public IQueryable<TEntity> GetQuery()
        {
            var infraType = GetInfrastructureType();
            var propName = GetDbSetPropertyName(infraType);

            // Obtener propiedad del DbSet (ej: "Configuraciones")
            var dbSetProperty = _context.GetType().GetProperty(propName)
                ?? throw new InvalidOperationException($"No DbSet found for {infraType.Name}");

            var dbSet = dbSetProperty.GetValue(_context)
                ?? throw new InvalidOperationException($"DbSet {propName} is null");

            // dbSet es DbSet<InfraType>
            var queryableInfra = dbSet as IQueryable
                ?? throw new InvalidOperationException($"DbSet {propName} is not IQueryable");

            // ⬇️ MUY IMPORTANTE
            // Se transforma IQueryable<Infraestructura> → IQueryable< Dominio >
            // usando AutoMapper.ProjectTo()
            var projectedQueryable = _mapper
                .ProjectTo<TEntity>(queryableInfra);

            return projectedQueryable;
        }


        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            var infraType = GetInfrastructureType();
            var propName = GetDbSetPropertyName(infraType);

            var dbSetProperty = _context.GetType().GetProperty(propName)
                ?? throw new InvalidOperationException($"No DbSet found for {infraType.Name}");

            var dbSet = dbSetProperty.GetValue(_context)
                ?? throw new InvalidOperationException($"DbSet {propName} is null");

            var parameter = Expression.Parameter(infraType, "e");
            var property = Expression.Property(parameter, "Id");
            var constant = Expression.Constant(id);
            var equals = Expression.Equal(property, constant);
            var lambda = Expression.Lambda(equals, parameter);

            var firstOrDefaultMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == "FirstOrDefaultAsync"
                    && m.GetParameters().Length == 3
                    && m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                .MakeGenericMethod(infraType);

            var task = (Task)firstOrDefaultMethod.Invoke(null, [dbSet, lambda, default(CancellationToken)])!;
            await task.ConfigureAwait(false);

            var infraEntity = task.GetType().GetProperty("Result")?.GetValue(task);

            if (infraEntity == null)
                return null;

            return _mapper.Map<TEntity>(infraEntity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var infraType = GetInfrastructureType();
            var propName = GetDbSetPropertyName(infraType);

            var dbSetProperty = _context.GetType().GetProperty(propName)
                ?? throw new InvalidOperationException($"No DbSet found for {infraType.Name}");

            var dbSet = dbSetProperty.GetValue(_context)
                ?? throw new InvalidOperationException($"DbSet {propName} is null");

            var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == "ToListAsync" && m.GetParameters().Length == 2)
                .MakeGenericMethod(infraType);

            var task = (Task)toListAsyncMethod.Invoke(null, [dbSet, default(CancellationToken)])!;
            await task.ConfigureAwait(false);

            var infraEntities = task.GetType().GetProperty("Result")?.GetValue(task);

            return _mapper.Map<IEnumerable<TEntity>>(infraEntities);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var infraType = GetInfrastructureType();
            var propName = GetDbSetPropertyName(infraType);

            var dbSetProperty = _context.GetType().GetProperty(propName)
                ?? throw new InvalidOperationException($"No DbSet found for {infraType.Name}");

            var dbSet = dbSetProperty.GetValue(_context)
                ?? throw new InvalidOperationException($"DbSet {propName} is null");

            // ✅ Convertir la expresión de TEntity a infraType
            var infraPredicate = ConvertExpression(predicate, infraType);

            // Aplicar Where con la expresión convertida
            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(infraType);

            var filteredQuery = whereMethod.Invoke(null, [dbSet, infraPredicate]);

            // Ejecutar ToListAsync
            var toListAsyncMethod = typeof(EntityFrameworkQueryableExtensions)
                .GetMethods()
                .First(m => m.Name == "ToListAsync" && m.GetParameters().Length == 2)
                .MakeGenericMethod(infraType);

            var task = (Task)toListAsyncMethod.Invoke(null, [filteredQuery, default(CancellationToken)])!;
            await task.ConfigureAwait(false);

            var infraEntities = task.GetType().GetProperty("Result")?.GetValue(task);

            return _mapper.Map<IEnumerable<TEntity>>(infraEntities);
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                var infraType = GetInfrastructureType();
                var infraEntity = _mapper.Map(entity, typeof(TEntity), infraType)
                    ?? throw new InvalidOperationException(
                        $"AutoMapper devolvió null al mapear {typeof(TEntity).Name} → {infraType.Name}");

                var propName = GetDbSetPropertyName(infraType);

                var dbSet = _context.GetType()
                    .GetProperty(propName)?
                    .GetValue(_context)
                    ?? throw new InvalidOperationException(
                        $"No existe DbSet llamado '{propName}' en el DbContext.");

                var addMethod = dbSet.GetType().GetMethod("Add")
                    ?? throw new InvalidOperationException(
                        $"No se encontró el método Add() en el DbSet de {infraType.Name}");

                addMethod.Invoke(dbSet, [infraEntity]);

                // ✅ Agregar await para guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var message =
                    $"Error al ejecutar AddAsync para tipo Dominio = '{typeof(TEntity).Name}' " +
                    $"mapeado a Infraestructura = '{GetInfrastructureType().Name}'. " +
                    $"Detalle del error: {ex.Message}";

                throw new Exception(message, ex);
            }
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await AddAsync(entity);
            }
        }

        public void Update(TEntity entity)
        {
            entity.SetUpdated();

            var infraType = GetInfrastructureType();
            var infraEntity = _mapper.Map(entity, typeof(TEntity), infraType);

            var propName = GetDbSetPropertyName(infraType);

            var dbSet = _context.GetType()
                .GetProperty(propName)?
                .GetValue(_context);

            var updateMethod = dbSet?.GetType().GetMethod("Update");
            updateMethod?.Invoke(dbSet, [infraEntity]);
        }

        public void Remove(TEntity entity)
        {
            var infraType = GetInfrastructureType();

            // Buscar la entidad ya trackeada
            var tracked = _context.ChangeTracker
                .Entries()
                .FirstOrDefault(e =>
                    e.Entity.GetType() == infraType &&
                    Equals(
                        infraType.GetProperty("Id")?.GetValue(e.Entity),
                        entity.Id
                    ))
                ?.Entity;

            if (tracked != null)
            {
                // ✅ Ya está trackeada → eliminar directamente
                _context.Remove(tracked);
                return;
            }

            // ❗ No está trackeada → usar stub
            var infraEntity = Activator.CreateInstance(infraType)
                ?? throw new InvalidOperationException("No se pudo crear la entidad infra.");

            infraType.GetProperty("Id")!.SetValue(infraEntity, entity.Id);

            _context.Attach(infraEntity);
            _context.Entry(infraEntity).State = EntityState.Deleted;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        private static string GetDbSetPropertyName(Type infraType)
        {
            var propName = infraType.Name;
            if (propName.EndsWith('D'))
                propName = propName[..^1];
            return propName;
        }

        private static Type GetInfrastructureType()
        {
            var typeName = typeof(TEntity).Name;

            if (typeName.EndsWith('D'))
                typeName = typeName[..^1];

            var infraNamespace = " Ecommers.Infrastructure.Persistence.Entities";
            var fullTypeName = $"{infraNamespace}.{typeName}";

            var infraType = Type.GetType(fullTypeName);
            return infraType ?? throw new InvalidOperationException($"Infrastructure type not found: {fullTypeName}");
        }

        // ✅ Método para convertir expresiones de TEntity a infraType
        private static LambdaExpression ConvertExpression(Expression<Func<TEntity, bool>> predicate, Type infraType)
        {
            var parameter = Expression.Parameter(infraType, "e");
            var visitor = new PropertyReplacerVisitor(predicate.Parameters[0], parameter, infraType); // ✅ CORRECTO
            var body = visitor.Visit(predicate.Body);

            return Expression.Lambda(body, parameter);
        }

        // Removed the unused private field '_sourceType' from the PropertyReplacerVisitor class as it is not being read anywhere in the code.

        private class PropertyReplacerVisitor(
           ParameterExpression oldParameter,
           ParameterExpression newParameter,
           Type targetType) : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParameter = oldParameter;
            private readonly ParameterExpression _newParameter = newParameter;
            private readonly Type _targetType = targetType;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParameter ? _newParameter : base.VisitParameter(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                // Si es una propiedad del tipo fuente, buscarla en el tipo destino
                if (node.Expression is ParameterExpression param && param == _oldParameter)
                {
                    var sourcePropName = node.Member.Name;

                    // Buscar la propiedad en el tipo de infraestructura
                    var targetProp = _targetType.GetProperty(sourcePropName,
                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) ?? throw new InvalidOperationException(
                            $"Property '{sourcePropName}' not found in type '{_targetType.Name}'");

                    // Crear nueva expresión de propiedad con el tipo correcto
                    return Expression.Property(_newParameter, targetProp);
                }

                return base.VisitMember(node);
            }
        }
    }
}
