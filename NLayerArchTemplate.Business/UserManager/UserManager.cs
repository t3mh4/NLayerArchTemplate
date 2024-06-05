using AutoMapper;
using Microsoft.Extensions.Logging;
using NLayerArchTemplate.Business.Abstracts;
using NLayerArchTemplate.Core.Exceptions;
using NLayerArchTemplate.Core.Helper;
using NLayerArchTemplate.DataAccess.Repositories.Interfaces;
using NLayerArchTemplate.Dtos.Login;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;
using NtierArchTemplate.Business.UserManager;
using NtierArchTemplate.DataAccess.Services.UserService;
using System.Diagnostics;

namespace NLayerArchTemplate.Business.UserManager;

public class UserManager : ABaseManager, IUserManager
{
    private readonly ILogger<UserManager> _logger;
    private readonly IUserService _userService;

    public UserManager(IUnitOfWork uow,
                       IMapper mapper,
                       ILogger<UserManager> logger) : base(uow, mapper)
    {
        _logger = logger;
        _userService = uow.UserService;
        Debug.WriteLine("UserManager is created.");
    }

    public async Task<UserAuthorizationDto> CheckAuthorization(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByUsername(dto.Username, cancellationToken);
        if (user == null)
        {
            _logger.LogError(GetException(string.Format("{0} kullanıcı adı bulunamadı.", user.Username)), _error);
            return null;
        }
        if (!user.IsActive)
        {
            _logger.LogError(GetException(string.Format("{0} kullanıcı adı aktif değil.", user.Username)), _error);
            return null;
        }
        if (user.IsDeleted)
        {
            _logger.LogError(GetException(string.Format("{0} kullanıcı adı silinmiş.", user.Username)), _error);
            return null;
        }
        if (!PasswordHelper.VerifyPassword(user.Password, dto.Password))
        {
            _logger.LogError(GetException(string.Format("{0} için hatalı kullanıcı adı yada şifre.", user.Username)), _error);
            return null;
        }
        user.Password = string.Empty;
        return user;
    }

    public async Task<List<UserListItemDto>> GetUserList(CancellationToken ct)
    {
        return await _userService.GetUserList(ct);
    }

    public async Task<UserCoreDto> GetByUserId(int userId, CancellationToken ct)
    {
        if (userId == 0) return new UserCoreDto() { IsActive = true };
        return await _userService.GetByUserId(userId, ct);
    }

    public async Task<int> Save(UserSaveDto user, List<string> modifiedProperties, CancellationToken ct)
    {
        var tblUser = _mapper.Map<TblUser>(user);
        if (tblUser.Id == 0)
        {
            tblUser.Password = PasswordHelper.HashPassword(user.Password);
            await _userService.AddAsync(tblUser, ct);
        }
        else
        {
            if (modifiedProperties.Exists(e => e == nameof(tblUser.Password)))
                tblUser.Password = PasswordHelper.HashPassword(user.Password);
            await _userService.UpdateAsync(tblUser, modifiedProperties, ct);
        }
        await _uow.SaveAsync();
        return tblUser.Id;
    }

    public async Task<UserAuthorizationDto> GetByUserName(string username, CancellationToken ct = default)
    {
        return await _userService.GetByUsername(username, ct);
    }

    public async Task Delete(int userId, CancellationToken ct)
    {
        var user = await _userService.GetAsync(g => g.Id == userId, ct);
        if (user == null) throw new DataNotFoundException();
        user.IsDeleted = true;
        await _userService.UpdateAsync(user, new List<string> { nameof(user.IsDeleted) }, ct);
        await _uow.SaveAsync();
    }

    private string _error { get { return "UserManager.CheckAuthorization"; } }
}
