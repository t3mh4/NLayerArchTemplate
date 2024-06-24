using System.ComponentModel.DataAnnotations;

namespace NLayerArchTemplate.Entities.Interfaces;

public interface IBaseEntity<TKeyType>
{
    [Key]
    TKeyType Id { get; set; }
}
