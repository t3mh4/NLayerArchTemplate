﻿using Microsoft.EntityFrameworkCore;
using NLayerArchTemplate.Core.ConstantMessages;
using NLayerArchTemplate.DataAccess.Repositories;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;
using NLayerArchTemplate.DataAccess.Services.UserService;
using NLayerArchTemplate.Core.Enums;
using NLayerArchTemplate.Core.Extensions;

namespace NLayerArchTemplate.DataAccess.Services.UserService;

public class UserService : Repository<TblUser>, IUserService
{

    public UserService(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserCoreDto> GetByUserId(int userId, CancellationToken ct)
    {
        var query = _dbContext.Users.Where(w => w.Id == userId)
             .Select(s => new UserCoreDto
             {
                 Id = s.Id,
                 Username = s.Username,
                 Name = s.Name,
                 Surname = s.Surname,
                 Email = s.Email,
                 IsActive = s.IsActive,
             });
        return await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
    }

    public async Task<UserAuthorizationDto> GetByUsername(string username, CancellationToken ct)
    {
        var query = _dbContext.Users.Where(w => w.Username == username)
              .Select(s => new UserAuthorizationDto
              {
                  Id = s.Id,
                  Username = s.Username,
                  Password = s.Password,
                  Name = s.Name,
                  Surname = s.Surname,
                  Email = s.Email,
                  IsActive = s.IsActive,
                  IsDeleted = s.IsDeleted
              });
        return await query.FirstOrDefaultAsync(ct).ConfigureAwait(false);
    }

    public async Task<List<UserListItemDto>> GetUserList(CancellationToken ct)
    {
        var adminId = UserEnum.Admin.ToInt32();
        var query = _dbContext.Users.Where(w => w.Id != adminId)
            .Select(s => new UserListItemDto
            {
                Id = s.Id,
                Username = s.Username,
                Name = s.Name,
                Surname = s.Surname,
                Email = s.Email,
                IsActive = s.IsActive
            });
        return await query.ToListAsync(ct).ConfigureAwait(false);
    }
}
