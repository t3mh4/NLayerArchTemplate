using NLayerArchTemplate.Core.Abstracts;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using NLayerArchTemplate.DataAccess.Services.UserService;
using NLayerArchTemplate.Entities.Interfaces;
using NtierArchTemplate.DataAccess.Services.UserService;
using System.Collections;
using System.Diagnostics;

namespace NLayerArchTemplate.DataAccess.Repositories;

public class UnitOfWork : ADisposable, IUnitOfWork
{
    private ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Debug.WriteLine("UnitOfWork is created.");
    }
    public IUserService UserService => new UserService(_context);

    public IRepository<T> Repository<T>() where T : class
    {
        var repositiryType = typeof(Repository<>);
        return (IRepository<T>)Activator.CreateInstance(repositiryType.MakeGenericType(typeof(T)), _context);
    }

    public async Task<int> SaveAsync()
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var result = await _context.SaveChangesAsync();
            transaction.Commit();
            return result;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }

    protected override void DisposeManagedResources()
    {
        _context?.Dispose();
    }
}
