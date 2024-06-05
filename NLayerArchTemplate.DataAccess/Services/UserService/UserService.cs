using Microsoft.EntityFrameworkCore;
using NLayerArchTemplate.DataAccess.Repositories;
using NLayerArchTemplate.Dtos.User;
using NLayerArchTemplate.Entities;
using NtierArchTemplate.DataAccess.Services.UserService;

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
                 IsActive = s.IsActive,
             });
        return await query.FirstOrDefaultAsync(ct) ?? throw new Exception("Kullanıcı Bulunamadı..!!");
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
                  IsActive = s.IsActive,
                  IsDeleted = s.IsDeleted
              });
        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task<List<UserListItemDto>> GetUserList(CancellationToken ct)
    {
        var query = _dbContext.Users
              .Select(s => new UserListItemDto
              {
                  Id = s.Id,
                  Username = s.Username,
                  Name = s.Name,
                  Surname = s.Surname,
                  IsActive = s.IsActive,
                  CreatedBy = s.CreatedBy,
                  CreatedDate = s.CreatedDate,
                  ModifiedBy = s.ModifiedBy,
                  ModifiedDate = s.ModifiedDate,
              });
        return await query.ToListAsync(ct);
    }
}
