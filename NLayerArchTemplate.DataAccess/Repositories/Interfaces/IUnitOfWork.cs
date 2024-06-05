using NtierArchTemplate.DataAccess.Services.UserService;

namespace NLayerArchTemplate.DataAccess.Repositories.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
    public IUserService UserService { get; }

    IRepository<T> Repository<T>() where T : class;
}
