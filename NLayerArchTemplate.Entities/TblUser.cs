using NLayerArchTemplate.Entities.Attributes;
using NLayerArchTemplate.Entities.Interfaces;

namespace NLayerArchTemplate.Entities;

[CreateAudit]
[UpdateAudit]
[DeleteAudit]
public record TblUser : IBaseEntity<int>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}