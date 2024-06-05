using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NLayerArchTemplate.Core;
using NLayerArchTemplate.DataAccess.Configurations;
using NLayerArchTemplate.Entities;
using NLayerArchTemplate.Entities.Abstracts;
using System.Diagnostics;
using System.Security.Claims;

namespace NLayerArchTemplate.DataAccess;

public class ApplicationDbContext : DbContext
{
    private readonly IServiceProvider _serviceprovider;
    private IHttpContextAccessor _httpContextAccessor => _serviceprovider.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;

    //Migration için bu constructor kullanılıyor
    public ApplicationDbContext() : base()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                IServiceProvider serviceProvider) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        _serviceprovider = serviceProvider;
        Debug.WriteLine("ApplicationDbContext is created.");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configurasyon class'larını aşağıda ki gibi tek tek de ekleyebiliriz.
        //modelBuilder.ApplyConfiguration(new UserConfiguration());
        //Configurasyon class'larını aşağıda ki gibi tek seferde ekleyebiliriz.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectonString());
    }

    private string GetConnectonString()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json")
            .Build();
        return configuration.GetConnectionString("DefaultConnection");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var username = _httpContextAccessor.HttpContext.User.Claims.First(f => f.Type == ClaimTypes.NameIdentifier)?.Value;
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted);
        foreach (var entry in entries)
        {
            if (entry.Entity is AAuditEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedBy = username;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.ModifiedDate = DateTime.Now;
                    entity.ModifiedBy = username;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entity.DeletedDate = DateTime.Now;
                    entity.DeletedBy = username;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public virtual DbSet<TblUser> Users { get; set; }
}
