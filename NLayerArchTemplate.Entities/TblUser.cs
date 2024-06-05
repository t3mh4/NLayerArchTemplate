using NLayerArchTemplate.Entities.Abstracts;
using NLayerArchTemplate.Entities.Interfaces;

namespace NLayerArchTemplate.Entities;

public record TblUser : AAuditEntity, IBaseEntity<int>
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public bool IsActive { get; set; }
}