using System.Linq.Expressions;

namespace NLayerArchTemplate.DataAccess.Repositories.Interfaces;

public interface IRepository<E> where E : class
{
    Task<IList<E>> GetAllAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken);
    Task<IList<E>> GetAllAsync(CancellationToken cancellationToken);
    Task<E> GetAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken);
    Task AddAsync(E entity, CancellationToken cancellationToken);
    Task AddRangeAsync(IReadOnlyList<E> entity, CancellationToken cancellationToken);
    Task UpdateAsync(E entity, CancellationToken cancellationToken);
    Task UpdateAsync(E entity, IReadOnlyList<string> modifiedProperties, CancellationToken cancellationToken);
    Task DeleteAsync(E entity, CancellationToken cancellationToken);
    Task DeleteRangeAsync(IReadOnlyList<E> entities, CancellationToken cancellationToken);
}
