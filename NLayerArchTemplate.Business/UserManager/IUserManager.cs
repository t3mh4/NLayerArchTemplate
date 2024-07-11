using NLayerArchTemplate.Dtos.Login;
using NLayerArchTemplate.Dtos.User;

namespace NLayerArchTemplate.Business.UserManager;

public interface IUserManager
{
    Task<UserAuthorizationDto> CheckAuthorization(LoginDto dto, CancellationToken ct);
    Task<List<UserListItemDto>> GetUserList(CancellationToken ct);
    Task<UserCoreDto> GetByUserId(int userId, CancellationToken ct);
    Task<int> Save(UserSaveDto user, List<string> modifiedProperties, CancellationToken ct);
    Task<UserAuthorizationDto> GetByUserName(string username, CancellationToken ct = default);
    Task Delete(int userId, CancellationToken ct);
}
