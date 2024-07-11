using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;

namespace NLayerArchTemplate.DataAccess.Services.UserService;

public interface IUserService : IRepository<TblUser>
{
    Task<UserCoreDto> GetByUserId(int userId, CancellationToken ct);
    Task<List<UserListItemDto>> GetUserList(CancellationToken ct);
    Task<UserAuthorizationDto> GetByUsername(string username, CancellationToken ct);
}
