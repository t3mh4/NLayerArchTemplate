using Microsoft.EntityFrameworkCore;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using System.Linq.Expressions;

namespace NLayerArchTemplate.DataAccess.Repositories;

public class Repository<E> : IRepository<E> where E : class
{
    private readonly DbSet<E> _dbSet;
    protected readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext context)
    {
        _dbContext = context;
        _dbSet = _dbContext.Set<E>();
    }

    public async Task AddAsync(E entity, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(IReadOnlyList<E> entities, CancellationToken cancellationToken)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public async Task UpdateAsync(E entity, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }, cancellationToken);
    }

    public async Task UpdateAsync(E entity, IReadOnlyList<string> modifiedProperties, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _dbSet.Attach(entity);
            var entry = _dbContext.Entry(entity);
            foreach (var property in modifiedProperties)
            {
                if (string.IsNullOrEmpty(property)) continue;
                entry.Property(property).IsModified = true;
            }
        }, cancellationToken);
    }

    public async Task DeleteAsync(E entity, CancellationToken cancellationToken)
    {
        //_dbSet.Attach(entity);
        //_dbContext.Entry(entity).State = EntityState.Deleted;
        await Task.Run(() =>
        {
            _dbContext.Remove(entity);
        }, cancellationToken);
    }

    public async Task DeleteRangeAsync(IReadOnlyList<E> entities, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            _dbContext.RemoveRange(entities);
            //foreach (var entity in entities)
            //{
            //    _dbSet.Attach(entity);
            //    _dbContext.Entry(entity).State = EntityState.Deleted;
            //}
        }, cancellationToken);
    }

    public async Task<E> GetAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken) => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<IList<E>> GetAllAsync(CancellationToken cancellationToken) => await _dbSet.ToListAsync(cancellationToken);

    public async Task<IList<E>> GetAllAsync(Expression<Func<E, bool>> predicate, CancellationToken cancellationToken) => await _dbSet.Where(predicate).ToListAsync(cancellationToken);
}
