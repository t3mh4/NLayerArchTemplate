using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLayerArchTemplate.Entities.Interfaces;

public interface IBaseEntity<TKeyType>
{
    [Key]
    TKeyType Id { get; set; }
}
