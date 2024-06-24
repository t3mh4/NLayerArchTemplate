using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLayerArchTemplate.Core.Helper;
using NLayerArchTemplate.Entities;

namespace NLayerArchTemplate.DataAccess.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<TblUser>
{
    public void Configure(EntityTypeBuilder<TblUser> builder)
    {
        builder.ToTable("User");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(p => p.Username);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(250);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(75);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(75);
        builder.Property(x => x.Email).HasMaxLength(250);
        builder.Property(x => x.IsActive);
        //Global Query Filter
        builder.HasQueryFilter(x => !x.IsDeleted);
        //Seed Data
        builder.HasData(new TblUser
        {
            Id = 1,
            Username = "admin",
            Password = PasswordHelper.HashPassword("1234"),
            Name = "Admin",
            Surname = "",
            //CreatedBy = "Admin",
            //CreatedDate = DateTime.Now,
            IsActive = true,
            Email = ""
        });
    }
}