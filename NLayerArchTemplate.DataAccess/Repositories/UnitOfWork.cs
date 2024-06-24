using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NLayerArchTemplate.Core.Abstracts;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using NtierArchTemplate.DataAccess.Services.UserService;

namespace NLayerArchTemplate.DataAccess.Repositories;

public class UnitOfWork : ADisposable, IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _contextAccessor;

    public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
    {
        _context = context;
        _contextAccessor = contextAccessor;
    }
    public IUserService UserService
    {
        get
        {
            return _contextAccessor.HttpContext.RequestServices.GetRequiredService<IUserService>();
        }
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var repositiryType = typeof(Repository<>);
        return (IRepository<T>)Activator.CreateInstance(repositiryType.MakeGenericType(typeof(T)), _context);
    }

    public async Task<int> SaveAsync(CancellationToken ct)
    {
        var result = await _context.SaveChangesAsync(ct).ConfigureAwait(false);
        return result;
        //using var transaction = _context.Database.BeginTransaction();
        //try
        //{
        //    var result = await _context.SaveChangesAsync(ct);
        //    transaction.Commit();
        //    return result;
        //}
        //catch (Exception)
        //{
        //    transaction.Rollback();
        //    throw;
        //}
        //var transactionOptions = new TransactionOptions
        //{
        //    IsolationLevel = IsolationLevel.ReadCommitted,
        //    Timeout = TransactionManager.MaximumTimeout
        //};

        //using var transaction = new TransactionScope(TransactionScopeOption.Required,
        //                                             transactionOptions,
        //                                             TransactionScopeAsyncFlowOption.Enabled);
        //// handle request handler
        //var result =await _context.SaveChangesAsync(ct).ConfigureAwait(false);

        //// complete database transaction
        //transaction.Complete();
        //return result;
    }

    protected override void DisposeManagedResources()
    {
        _context?.Dispose();
    }
}
