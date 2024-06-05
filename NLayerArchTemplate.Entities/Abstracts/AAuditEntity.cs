using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NLayerArchTemplate.Entities.Abstracts;

public record AAuditEntity
{
    [Column(TypeName = "datetime2(3)")]
    [Required]
    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(150)]
    public string CreatedBy { get; set; }

    [Column(TypeName = "datetime2(3)")]
    public DateTime? ModifiedDate { get; set; }

    [MaxLength(150)]
    public string ModifiedBy { get; set; }

    [Column(TypeName = "datetime2(3)")]
    public DateTime? DeletedDate { get; set; }

    [MaxLength(150)]
    public string DeletedBy { get; set; }

    public bool IsDeleted { get; set; }
}