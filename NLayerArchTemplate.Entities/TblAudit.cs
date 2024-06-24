using NLayerArchTemplate.Entities.Interfaces;

namespace NLayerArchTemplate.Entities;

public record TblAudit : IBaseEntity<int>
{
    public int Id { get; set; }
    public string TableName { get; set; }
    public string Type { get; set; }
    public string Data { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
}
