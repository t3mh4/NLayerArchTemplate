using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerArchTemplate.Entities;

namespace NLayerArchTemplate.DataAccess.Configurations;

public class AuditEntityTypeConfiguration : IEntityTypeConfiguration<TblAudit>
{
    public void Configure(EntityTypeBuilder<TblAudit> builder)
    {
        builder.ToTable("Audit");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).IsRequired();
        builder.Property(c => c.TableName)
               .IsRequired()
               .HasMaxLength(50);
        builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(10);
        builder.Property(c => c.Data)
                .IsRequired()
                .HasColumnType("text");
        builder.Property(c => c.CreatedBy)
                .IsRequired()
                .HasMaxLength(150);
        builder.Property(c => c.CreatedDate)
                .HasColumnType("datetime");
    }
}